using ICSF9TCT.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ICSF9TCT.Controllers
{
    public class StoreIssueController : Controller
    {
        #region Variable Definition
        string strQry = string.Empty;
        string strErrorMessage = string.Empty;
        BL_DB DataAcesslayer = new BL_DB();

        public string vBAbbr = "";
        public string vBName = "";
        public string vPAbbr = "PurReq";
        public string vPName = "";
        public int vPvType = 0;
        string Sys_IPAddress = "";
        //----- Base Header Variable 
        int iHDocdate = 0;
        int iHHeaderId = 0;
        string sHDocNo = "";
        int iHDept_Id = 0;
        int iHDept_Users_Id = 0;
        int iHWarehouse_Id = 0;
        string sHNarration = "";
        string sHBaseRefNo = "";
        int iHBaseRefDate = 0;
        decimal dHNet = 0;
        int iRecordCnt = 0;

        //----- Base Details variable 
        int iBCost_Center_Id = 0;
        int iBProductId = 0;
        int iBUnitId = 0;
        int iBBodyId = 0;
        decimal dBIndentQty = 0;
        decimal dBRate = 0;
        decimal dBStockAvailable = 0;
        decimal dBGross = 0;
        string sBRemarks = "";
        string sBPartyItemCode = "";
        string sBPartyItemDesc = "";
        string sBRequiredFor = "";
        decimal dBBaseQty = 0;
        


        Hashtable extHeader = new Hashtable();
        List<Hashtable> extBody = new List<Hashtable>();
        List<Hashtable> extFooter = new List<Hashtable>();

        #endregion
        [HttpPost]
        public ActionResult UpdateStoreIssue(int CompanyId, string SessionId, string User, int LoginId, int vtype, string docNo, List<PEBody> BodyData)
        {
            var lPurReqBody = new List<PurReqBodyData>();
            int Cnt_lPurReqBody = 0;
            int PostingStatus = 0;
            Sys_IPAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();

            clsGeneric.Log_write(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), SessionId, CompanyId, docNo, vtype, User);
            clsGeneric.writeLog("IP Address :" + Sys_IPAddress);
            BL_DB DataAcesslayer = new BL_DB();
            
            try
            {
                strQry = $@"SELECT  sName  FROM  dbo.cCore_Vouchers_0 WHERE (iVoucherType =" + vtype + " )";
                vBName = Convert.ToString(clsGeneric.ShowRecord(CompanyId, strQry));
                strQry = $@"SELECT  sAbbr  FROM  dbo.cCore_Vouchers_0 WHERE (iVoucherType =" + vtype + " )";
                vBAbbr = Convert.ToString(clsGeneric.ShowRecord(CompanyId, strQry));

                strQry = $@"SELECT  iVoucherType   FROM  dbo.cCore_Vouchers_0 WHERE (sAbbr='" + vPAbbr + "')";
                vPvType = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, strQry));
                strQry = $@"SELECT  sName  FROM  dbo.cCore_Vouchers_0 WHERE (iVoucherType =" + vPvType + " )";
                vPName = Convert.ToString(clsGeneric.ShowRecord(CompanyId, strQry));

                strQry = $@"SELECT tchd.iHeaderId FROM dbo.tCore_HeaderData"+ vPvType + "_0 AS tchd INNER JOIN " +
                " dbo.tCore_Header_0 AS tch ON tchd.iHeaderId = tch.iHeaderId WHERE  (tch.iVoucherType = " + vPvType + ") AND (tchd.BaseRefNo = N'" + docNo + "')";
                PostingStatus = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, strQry));
                if (PostingStatus == 0)
                {



                    strQry = $@"SELECT DISTINCT tch.iHeaderId, tch.sVoucherNo, tch.iDate, tci.iProduct, tcdtag.iTag5 AS Department, tcdtag.iTag3006 AS Dept, tcdn.sRemarks, tchd.PostDocDate, tchd.PostDocNo, tcd.iFaTag AS Warehouse, tchd.sNarration, tci.iUnit,tci.mRate," +
                        " (SELECT ABS(SUM(tci1.fQuantity)) AS Qty FROM dbo.tCore_Data_0 AS tcd1 INNER JOIN  dbo.tCore_Indta_0 AS tci1 ON tcd1.iBodyId = tci1.iBodyId WHERE (tcd1.iHeaderId = tcd.iHeaderId) AND (tci1.iProduct = tci.iProduct)) as Qty,(SELECT ABS(SUM(tci1.mGross)) AS Qty FROM dbo.tCore_Data_0 AS tcd1 INNER JOIN  dbo.tCore_Indta_0 AS tci1 ON tcd1.iBodyId = tci1.iBodyId WHERE (tcd1.iHeaderId = tcd.iHeaderId) AND (tci1.iProduct = tci.iProduct)) as Gross, tcibsd.mInput0 AS MinStock, tcibsd.mInput1 AS StockQty, tcibsd.mInput2 AS PendReqQty " +
                        " FROM dbo.tCore_Header_0 AS tch INNER JOIN dbo.tCore_HeaderData" + vtype + "_0 AS tchd ON tch.iHeaderId = tchd.iHeaderId INNER JOIN " +
                        " dbo.tCore_Data_0 AS tcd ON tch.iHeaderId = tcd.iHeaderId INNER JOIN dbo.tCore_Data" + vtype + "_0 AS tcdn ON tcd.iBodyId = tcdn.iBodyId INNER JOIN " +
                        " dbo.tCore_Indta_0 AS tci ON tcd.iBodyId = tci.iBodyId INNER JOIN dbo.tCore_Data_Tags_0 AS tcdtag ON tcd.iBodyId = tcdtag.iBodyId" +
                        " Inner Join dbo.tCore_IndtaBodyScreenData_0 AS tcibsd ON tcd.iBodyId = tcibsd.iBodyId " +
                        " WHERE (tch.iVoucherType = " + vtype + ") AND (tch.sVoucherNo = N'" + docNo + "')";

                    DataSet ds1 = DataAcesslayer.GetData(strQry, CompanyId, ref strErrorMessage);
                    iRecordCnt = ds1.Tables[0].Rows.Count;
                    iHHeaderId = 0; iHDocdate = 0; iHDept_Id = 0; iHDept_Users_Id = 0; iHWarehouse_Id = 0; dHNet = 0;
                    sHDocNo = ""; sHNarration = ""; sHBaseRefNo = ""; sHBaseRefNo = "";

                    if (ds1 != null && ds1.Tables.Count > 0 && iRecordCnt > 0)
                    {
                        iHHeaderId = 0; iHDocdate = 0; iHDept_Id = 0; iHDept_Users_Id = 0; iHWarehouse_Id = 0; dHNet = 0;
                        sHDocNo = ""; sHNarration = ""; sHBaseRefNo = ""; sHBaseRefNo = "";
                        iHHeaderId = Convert.ToInt32(ds1.Tables[0].Rows[0][0]);
                        sHDocNo = Convert.ToString(ds1.Tables[0].Rows[0][1]);
                        iHDocdate = Convert.ToInt32(ds1.Tables[0].Rows[0][2]);
                        iHDept_Id = Convert.ToInt32(ds1.Tables[0].Rows[0][4]);
                        iHDept_Users_Id = Convert.ToInt32(ds1.Tables[0].Rows[0][5]);
                        iHWarehouse_Id = Convert.ToInt32(ds1.Tables[0].Rows[0][9]);
                        sHNarration = Convert.ToString(ds1.Tables[0].Rows[0][10]);
                    }
                    
                   
                    lPurReqBody.Clear();
                    for (int i = 0; i <= iRecordCnt - 1; i++)
                    {
                        iBProductId = Convert.ToInt32(ds1.Tables[0].Rows[i][3]); // product 
                        iBUnitId = Convert.ToInt32(ds1.Tables[0].Rows[i][11]); // unit
                        dBIndentQty = Convert.ToDecimal(ds1.Tables[0].Rows[i][17]); // Qty
                        dBRate = Convert.ToDecimal(ds1.Tables[0].Rows[i][12]); // Rate
                        dBGross = Convert.ToDecimal(ds1.Tables[0].Rows[i][14]); // Gross
                        dBBaseQty = Convert.ToDecimal(ds1.Tables[0].Rows[i][13]); // Qty
                        //dBStockAvailable = Convert.ToDecimal(ds1.Tables[0].Rows[i][13]); // Qty
                        if (dBIndentQty != 0)
                        {
                            lPurReqBody.Add(new PurReqBodyData() { Body__Id = iBBodyId, Item__Id = iBProductId, Unit__Id = iBUnitId, Stock_Available = dBStockAvailable, Quantity = dBIndentQty, Rate = dBRate, Gross = dBGross, sRemarks = sBRemarks, PartyItemCode = sBPartyItemCode, PartyItemDesc = sBPartyItemDesc, RequiredFor = sBRequiredFor, BaseQuantity = dBBaseQty });
                        }
                        iBBodyId = 0; iBProductId = 0; iBUnitId = 0; iBBodyId = 0; dBIndentQty = 0; dBRate = 0; dBStockAvailable = 0; dBGross = 0; dBBaseQty = 0;
                        sBRemarks = ""; sBPartyItemCode = ""; sBPartyItemDesc = ""; sBRequiredFor = "";
                    }
                    Cnt_lPurReqBody = 0;
                    // POSTING CODE HERE 
                    if (lPurReqBody.Count > 0)
                    {
                        Hashtable headerPurReq = new Hashtable();
                        HashData objHashRequest_PurReq = new HashData();
                        List<System.Collections.Hashtable> lstBody_PurReq = new List<System.Collections.Hashtable>();

                        headerPurReq.Add("DocNo", sHDocNo);
                        headerPurReq.Add("Date", iHDocdate);
                        headerPurReq.Add("Department__Id", iHDept_Id);
                        headerPurReq.Add("Dept Users__Id", iHDept_Users_Id);
                        headerPurReq.Add("Warehouse__Id", iHWarehouse_Id);
                        headerPurReq.Add("sNarration", sHNarration);
                        headerPurReq.Add("BaseRefNo", sHDocNo);
                        headerPurReq.Add("BaseRefDate", iHDocdate);
                        headerPurReq.Add("Net", dHNet);
                        Cnt_lPurReqBody = lPurReqBody.Count;
                        lstBody_PurReq.Clear();
                        foreach (var row in lPurReqBody)
                        {
                            Hashtable bodyPurReq = new Hashtable();
                            bodyPurReq.Add("Item__Id", row.Item__Id);
                            bodyPurReq.Add("Unit__Id", row.Unit__Id);
                            bodyPurReq.Add("Quantity", row.Quantity);
                            bodyPurReq.Add("Rate", row.Rate);
                            bodyPurReq.Add("Gross", row.Gross);
                            bodyPurReq.Add("sRemarks", row.sRemarks);
                            bodyPurReq.Add("PartyItemCode", row.PartyItemCode);
                            bodyPurReq.Add("PartyItemDesc", row.PartyItemDesc);
                            bodyPurReq.Add("RequiredFor", row.RequiredFor);
                            bodyPurReq.Add("BaseQuantity", row.BaseQuantity);
                            Hashtable bodyPurReq_Stock_Available = new Hashtable
                            {
                                {"Input",  row.Stock_Available},
                                {"FieldName", "Stock Available"},
                                {"Value", row.Stock_Available},
                                { "ColMap", 0}

                            };
                            bodyPurReq.Add("Stock Available", bodyPurReq_Stock_Available);
                            lstBody_PurReq.Add(bodyPurReq);

                        }



                        System.Collections.Hashtable objHash = new System.Collections.Hashtable();
                        objHash.Add("Body", lstBody_PurReq);
                        objHash.Add("Header", headerPurReq);
                        List<System.Collections.Hashtable> lstHash = new List<System.Collections.Hashtable>();
                        lstHash.Add(objHash);
                        objHashRequest_PurReq.data = lstHash;
                        string sContentPurReq = JsonConvert.SerializeObject(objHashRequest_PurReq);
                        using (var webClient_PurReq = new WebClient())
                        {
                            webClient_PurReq.Encoding = Encoding.UTF8;
                            webClient_PurReq.Headers.Add("fSessionId", SessionId);
                            webClient_PurReq.Headers.Add("Content-Type", "application/json");
                            clsGeneric.writeLog("Upload URL Of RptPro :" + ("http://localhost/Focus8API/Transactions/Vouchers/" + vPvType));
                            var res_PurReq = webClient_PurReq.UploadString("http://localhost/Focus8API/Transactions/Vouchers/" + vPvType, sContentPurReq);
                            clsGeneric.writeLog("Response of Focus Fields Data " + res_PurReq);
                            if (res_PurReq != null)
                            {
                                var responseDataPurReq = JsonConvert.DeserializeObject<APIResponse.PostResponse>(res_PurReq);
                                if (responseDataPurReq.result == -1)
                                {

                                    
                                    return Json(new { status = false, data = new { message = responseDataPurReq.message } });
                                }
                                else
                                {
                                    var iMasterId = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(responseDataPurReq.data[0]["VoucherNo"]));
                                    //UpdateBatchs(ivTypePosting, iHDate, iMasterId, CompanyId);
                                    UpdateStatus(vtype, docNo, iHDocdate, 1, CompanyId);
                                    return Json(new { status = true, data = new { message = "Posting Successful" } });
                                }
                            }
                        }

                    }

                    //using (var webClient_StoreIssue = new WebClient())
                    //{
                    //    webClient_StoreIssue.Encoding = Encoding.UTF8;
                    //    webClient_StoreIssue.Headers.Add("fSessionId", SessionId);
                    //    webClient_StoreIssue.Headers.Add("Content-Type", "application/json");
                    //    clsGeneric.writeLog("Download URL: " + "http://localhost/Focus8API/Screen/Transactions/" + vtype + "/" + docNo);
                    //    var res_StoreIssue = webClient_StoreIssue.DownloadString("http://localhost/Focus8API/Screen/Transactions/" + vtype + "/" + docNo);
                    //    clsGeneric.writeLog("Response of Focus Fields Data " + res_StoreIssue);
                    //    if (res_StoreIssue != null)
                    //    {
                    //        var resData_StoreIssue = JsonConvert.DeserializeObject<APIResponse.PostResponse>(res_StoreIssue);
                    //        if (resData_StoreIssue.result == -1)
                    //        {
                    //            //return Json(new { status = false, data = new { message = resData_StoreIssue.message } });
                    //        }
                    //        else
                    //        {
                    //            if (resData_StoreIssue.data.Count != 0)
                    //            {
                    //                extHeader = JsonConvert.DeserializeObject<Hashtable>(JsonConvert.SerializeObject(resData_StoreIssue.data[0]["Header"]));
                    //                if (resData_StoreIssue.data[0]["Footer"].ToString() != "[]")
                    //                {
                    //                    extFooter = JsonConvert.DeserializeObject<List<Hashtable>>(JsonConvert.SerializeObject(resData_StoreIssue.data[0]["Footer"]));
                    //                }
                    //                extBody = JsonConvert.DeserializeObject<List<Hashtable>>(JsonConvert.SerializeObject(resData_StoreIssue.data[0]["Body"]));
                    //                iHHeaderId = 0; iHDocdate = 0; iHDept_Id = 0; iHDept_Users_Id = 0; iHWarehouse_Id = 0; dHNet = 0;
                    //                sHDocNo = ""; sHNarration = ""; sHBaseRefNo = ""; sHBaseRefNo = "";
                    //                iHHeaderId = Convert.ToInt32(extHeader["HeaderId"]);
                    //                iHDocdate = Convert.ToInt32(extHeader["Date"]);
                    //                sHDocNo = extHeader["DocNo"].ToString();
                    //                iHDept_Id = Convert.ToInt32(extHeader["Department__Id"]);
                    //                iHDept_Users_Id = Convert.ToInt32(extHeader["Dept__Id"]);
                    //                iHWarehouse_Id = Convert.ToInt32(extHeader["Warehouse__Id"]);
                    //                if (extHeader["sNarration"] != null)
                    //                {
                    //                    sHNarration = extHeader["sNarration"].ToString();
                    //                }



                    //                //sHBaseRefNo = extHeader["xxx"];
                    //                //iHBaseRefDate = Convert.ToInt32(extHeader["xxx"]);
                    //                dHNet = Convert.ToDecimal(extHeader["Net"]);

                    //                lPurReqBody.Clear();
                    //                int tempFor1 = extBody.Count - 1;
                    //                for (int i = 0; i <= tempFor1; i++)
                    //                {
                    //                    iBCost_Center_Id = Convert.ToInt32(extBody[i]["Cost Center__Id"]);
                    //                    iBBodyId = Convert.ToInt32(extBody[i]["TransactionId"]);
                    //                    iBProductId = Convert.ToInt32(extBody[i]["Item__Id"]);
                    //                    iBUnitId = Convert.ToInt32(extBody[i]["Unit__Id"]);
                    //                    dBIndentQty = Convert.ToInt32(extBody[i]["Quantity"]);
                    //                    dBRate = Convert.ToInt32(extBody[i]["TransactionId"]);
                    //                    dBStockAvailable = Convert.ToInt32(extBody[i]["TransactionId"]);
                    //                    dBGross = Convert.ToInt32(extBody[i]["TransactionId"]);
                    //                    dBBaseQty = Convert.ToInt32(extBody[i]["BaseQuantity"]);

                    //                    if (dBIndentQty != 0)
                    //                    {
                    //                        lPurReqBody.Add(new PurReqBodyData() { Body__Id = iBBodyId, Item__Id = iBProductId, Unit__Id = iBUnitId, Stock_Available = dBStockAvailable, Quantity = dBIndentQty, Rate = dBRate, Gross = dBGross, sRemarks = sBRemarks, PartyItemCode = sBPartyItemCode, PartyItemDesc = sBPartyItemDesc, RequiredFor = sBRequiredFor, BaseQuantity = dBBaseQty });
                    //                    }
                    //                    iBBodyId = 0; iBProductId = 0; iBUnitId = 0; iBBodyId = 0; dBIndentQty = 0; dBRate = 0; dBStockAvailable = 0; dBGross = 0; dBBaseQty = 0;
                    //                    sBRemarks = ""; sBPartyItemCode = ""; sBPartyItemDesc = ""; sBRequiredFor = "";


                    //                }

                    //            }
                    //        }
                    //    }
                    //}
                }
                else
                {
                    return Json(new { status = false, data = new { message = "Document Already Posted" } });
                }
            }
            catch (Exception ex)
            {

                clsGeneric.writeLog("Exception occured:" + (ex.Message));
                return Json(new { status = false, data = new { message = "Posting Failed " } });
                throw;
            }

            return Json(new { status = true, data = new { message = "Posting Successful " } });
        }

        static void UpdateStatus(int Type, string vno, int vdate, int PostingStatus, int CompanyId)
        {
            BL_DB DataAcesslayer = new BL_DB();
            string strErrorMessage = string.Empty;
            string strValue = "";
            strValue = $@"Update dbo.tCore_HeaderData" + Type + "_0  set dbo.tCore_HeaderData" + Type + "_0.PostingStatus=" + PostingStatus +
                ",PostDocNo='"+ vno + "',PostDocDate=" + vdate  + " from  dbo.tCore_HeaderData" + Type + "_0 AS CHD INNER JOIN  dbo.tCore_Header_0 AS CH ON CHD.iHeaderId = CH.iHeaderId " +
                "WHERE      (CH.iVoucherType =" + Type + ") AND (CH.sVoucherNo = N'" + vno + "')";
            clsGeneric.writeLog("Query :" + strValue);
            DataAcesslayer.GetExecute(strValue, CompanyId, ref strErrorMessage);
            if (strErrorMessage != "")
            {
                clsGeneric.writeLog("strErrorMessage :" + strErrorMessage);
            }
        }


    }
}