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
    public class DCJWSalController : Controller
    {
        #region Variable Definition

        public string strQry = string.Empty;
        public string strValue = string.Empty;
        public string strErrorMessage = string.Empty;
        BL_DB DataAcesslayer = new BL_DB();
        public static int PostingStatus;
        StringBuilder msg = new StringBuilder();
        public static string vBAbbr = "";
        public static string vBName = "";

        public static int vPVtype = 0;
        public static string vPAbbr = "AnxJWSale";
        public static string vPName = "";
        public static int bDate = 0;
        int UnitLocation = 0;
        int Vendor = 0;
        int FGId = 0;
        double DCJWSal_Qty = 0;
        string pono = "";
        int podate = 0;
        string narration = "";

        #endregion
        [HttpPost]
        public ActionResult UpdateAnxJWSale(int CompanyId, string SessionId, string User, int LoginId, int vtype, string docNo, int docdate, List<PEBody> BodyData)
        {
            try
            {

                clsGeneric.Log_write(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), SessionId, CompanyId, docNo, vtype, User);

                strQry = $@"SELECT CHD.PostingStatus FROM  dbo.tCore_HeaderData" + vtype + "_0 AS CHD INNER JOIN " +
                    "dbo.tCore_Header_0 AS CH ON CHD.iHeaderId = CH.iHeaderId WHERE (CH.iVoucherType =" + vtype + ") AND (CH.sVoucherNo = N'" + docNo + "')";
                PostingStatus = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, strQry));
                // 0,Pending, 1,Updated
                clsGeneric.writeLog("PostingStatus 0,Pending, 1,Updated  : " + PostingStatus);
                if (PostingStatus == 0)
                {
                    strQry = $@"SELECT  sAbbr  FROM  dbo.cCore_Vouchers_0 WHERE (iVoucherType =" + vtype + " )";
                    vBAbbr = Convert.ToString(clsGeneric.ShowRecord(CompanyId, strQry));
                    strQry = $@"SELECT  sName  FROM  dbo.cCore_Vouchers_0 WHERE (iVoucherType =" + vtype + " )";
                    vBName = Convert.ToString(clsGeneric.ShowRecord(CompanyId, strQry));

                    strQry = $@"SELECT  iVoucherType  FROM  dbo.cCore_Vouchers_0 WHERE (sAbbr ='" + vPAbbr + "')";
                    vPVtype = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, strQry));
                    strQry = $@"SELECT  sName  FROM  dbo.cCore_Vouchers_0 WHERE (iVoucherType ='" + vPVtype + "')";
                    vPName = Convert.ToString(clsGeneric.ShowRecord(CompanyId, strQry));


                    //strQry = $@" SELECT DISTINCT CH.iDate, CD.iFaTag UnitLocation, CD.iBookNo Vendor, tci.iProduct FGID, abs(tci.fQuantity) PlanQty,tci.mRate, tci.mGross, CHD.PONo, CHD.PODate, CHD.sNarration, CHD.Price, CHD.PackandForwd, CHD.FreightTerm, CHD.DeliveryTerms, CHD.Insurance  " +
                    //" FROM dbo.tCore_Header_0 AS CH INNER JOIN dbo.tCore_Data_0 AS CD ON CH.iHeaderId = CD.iHeaderId INNER JOIN " +
                    //" dbo.tCore_Indta_0 AS tci ON CD.iBodyId = tci.iBodyId INNER JOIN dbo.tCore_HeaderData" + vtype + "_0 AS CHD ON CD.iHeaderId = CHD.iHeaderId " +
                    //" WHERE     (CH.iVoucherType =" + vtype + ") AND (CH.sVoucherNo = N'" + docNo + "')";
                    //Add New Field RecoItem in DC Job Work Sales Body Reconcile as per RecoItem

                    strQry = $@" SELECT DISTINCT CH.iDate, CD.iFaTag UnitLocation, CD.iBookNo Vendor, tBody.RecoItem FGID, abs(tci.fQuantity) PlanQty,tci.mRate, tci.mGross, CHD.PONo, CHD.PODate, CHD.sNarration, CHD.Price, CHD.PackandForwd, CHD.FreightTerm, CHD.DeliveryTerms, CHD.Insurance  " +
                    " FROM dbo.tCore_Header_0 AS CH INNER JOIN dbo.tCore_Data_0 AS CD ON CH.iHeaderId = CD.iHeaderId INNER JOIN " +
                    " dbo.tCore_Indta_0 AS tci ON CD.iBodyId = tci.iBodyId INNER JOIN dbo.tCore_HeaderData" + vtype + "_0 AS CHD ON CD.iHeaderId = CHD.iHeaderId INNER JOIN " +
                    " dbo.tCore_Data" + vtype + "_0 AS tBody ON CD.iBodyId = tBody.iBodyId " +
                    " WHERE     (CH.iVoucherType =" + vtype + ") AND (CH.sVoucherNo = N'" + docNo + "')";

                    clsGeneric.writeLog("strQry:" + strQry);
                    DataSet ds = DataAcesslayer.GetData(strQry, CompanyId, ref strErrorMessage);
                    clsGeneric.writeLog("Getting from Voucher:" + (ds));
                    if (ds != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            // As per suggestion from Majid and Aamir Sir only one fg product @23-02-2023 
                            //for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                            //{
                            bDate = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                            UnitLocation = Convert.ToInt32(ds.Tables[0].Rows[0][1]);
                            Vendor = Convert.ToInt32(ds.Tables[0].Rows[0][2]);
                            FGId = Convert.ToInt32(ds.Tables[0].Rows[0][3]);
                            DCJWSal_Qty = Convert.ToDouble(ds.Tables[0].Rows[0][4]);
                            pono = Convert.ToString(ds.Tables[0].Rows[0][7]);
                            podate = Convert.ToInt32(ds.Tables[0].Rows[0][8]);
                            narration = Convert.ToString(ds.Tables[0].Rows[0][9]);
                            //}
                        }

                    }

                    if (FGId != 0)
                    {
                        if (OpenSalJWBOM(vtype, docNo, CompanyId, bDate, UnitLocation, Vendor, FGId, DCJWSal_Qty, vPVtype, pono, podate, narration, SessionId, User, LoginId))
                        {
                            return Json(new { status = true, data = new { message = "Posted Successful" } });
                        }
                        else
                        {
                            return Json(new { status = false, data = new { message = "Posted not Successful" } });
                        }
                    }


                }



            }
            catch (Exception ex)
            {
                msg.Clear();
                msg.Append(ex.Message);
                clsGeneric.writeLog("Exception occured:" + (ex.Message));
                return Json(new { status = false, data = new { message = "Posting Failed " } });
                throw;

            } //End Catch
            return Json(new { status = true, data = new { message = "Posted Successful" } });
        }
        public bool OpenSalJWBOM(int cType, string cvno, int cCompanyId, int cDate, int cUnitLocation, int cVendor, int cFGId, double cDCJWSal_Qty, int cBOMVtype, string cpono, int cpodate, string cnarration, string cSessionId, string cUser, int cLoginId)
        {
            string vBOMAbbr = "SalJWBOM";
            int vBOMtype = 0;
            strQry = $@"SELECT  iVoucherType  FROM  dbo.cCore_Vouchers_0 WHERE (sAbbr ='" + vBOMAbbr + "')";
            vBOMtype = Convert.ToInt32(clsGeneric.ShowRecord(cCompanyId, strQry));
            int iRMID = 0;
            double dBOMRMQty = 0;
            double dBOMFGQty = 0;

            strQry = $@" SELECT TCI.iProduct, ABS(TCI.fQuantity) AS RMQuantity, TCHD.FGQty FROM dbo.tCore_Header_0 AS CH " +
                " INNER JOIN dbo.tCore_Data_0 AS CD ON CH.iHeaderId = CD.iHeaderId INNER JOIN dbo.tCore_Indta_0 AS TCI " +
                " ON CD.iBodyId = TCI.iBodyId INNER JOIN dbo.tCore_HeaderData" + vBOMtype + "_0 AS TCHD ON CD.iHeaderId = TCHD.iHeaderId " +
                "WHERE (CH.iVoucherType =" + vBOMtype + ") AND (TCHD.FGCode =" + cFGId + ") AND (CD.iFaTag = " + cUnitLocation + ") AND (CD.iBookNo =" + cVendor + ")";
            clsGeneric.writeLog("Query :" + strQry);
            DataSet ds1 = DataAcesslayer.GetData(strQry, cCompanyId, ref strErrorMessage);
            clsGeneric.writeLog("Getting from Voucher:" + (ds1));
            if (ds1 != null)
            {

                if (ds1.Tables[0].Rows.Count > 0)
                {
                    HashData objHashRequest_AnxJWSale = new HashData();
                    Hashtable headerAnxJWSale = new Hashtable();
                    headerAnxJWSale.Add("DocNo", cvno);
                    headerAnxJWSale.Add("Date", cDate);
                    headerAnxJWSale.Add("CustomerAC__Id", cVendor);
                    headerAnxJWSale.Add("Unit Location__Id", cUnitLocation);
                    headerAnxJWSale.Add("sNarration", cnarration);
                    headerAnxJWSale.Add("PONo", cpono);
                    headerAnxJWSale.Add("PODate", cpodate);
                    headerAnxJWSale.Add("GateEntryNo", 0);
                    headerAnxJWSale.Add("GateEntryDate", 0);
                    headerAnxJWSale.Add("DCJWNo", cvno);
                    headerAnxJWSale.Add("DCJWDate", cDate);

                    List<System.Collections.Hashtable> lstBody_AnxJWSale = new List<System.Collections.Hashtable>();
                    lstBody_AnxJWSale.Clear();
                    for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                    {
                        Hashtable bodyAnxJWSale = new Hashtable();
                        iRMID = Convert.ToInt32(ds1.Tables[0].Rows[j][0]);
                        dBOMRMQty = Convert.ToDouble(ds1.Tables[0].Rows[j][1]);
                        dBOMFGQty = Convert.ToDouble(ds1.Tables[0].Rows[j][2]);
                        bodyAnxJWSale.Add("Item__Id", iRMID);
                        bodyAnxJWSale.Add("Quantity", cDCJWSal_Qty * dBOMRMQty);
                        bodyAnxJWSale.Add("BOMQty", dBOMRMQty);
                        bodyAnxJWSale.Add("FGQty", DCJWSal_Qty);
                        bodyAnxJWSale.Add("FGCode", cFGId);

                        lstBody_AnxJWSale.Add(bodyAnxJWSale);
                    }

                    System.Collections.Hashtable objHash_AnxJWSale = new System.Collections.Hashtable();
                    objHash_AnxJWSale.Add("Body", lstBody_AnxJWSale);
                    objHash_AnxJWSale.Add("Header", headerAnxJWSale);
                    List<System.Collections.Hashtable> lstHash_AnxJWSale = new List<System.Collections.Hashtable>();
                    lstHash_AnxJWSale.Add(objHash_AnxJWSale);
                    objHashRequest_AnxJWSale.data = lstHash_AnxJWSale;
                    string sContent_AnxJWSale = JsonConvert.SerializeObject(objHashRequest_AnxJWSale);
                    clsGeneric.writeLog("Upload RptPro :" + "http://localhost/Focus8API/Transactions/Vouchers/" + vPName);
                    clsGeneric.writeLog("URL Param :" + sContent_AnxJWSale);
                    using (var clientDel_AnxJWSale = new WebClient())
                    {

                        clientDel_AnxJWSale.Encoding = Encoding.UTF8;
                        clientDel_AnxJWSale.Headers.Add("fSessionId", cSessionId);
                        clientDel_AnxJWSale.Headers.Add("Content-Type", "application/json");
                        string url = "http://localhost/Focus8API/Transactions/" + vPName + "/" + cvno;
                        clsGeneric.writeLog("url  Delete RptPro : " + url);
                        var responseDel_RptPro = clientDel_AnxJWSale.UploadString(url, "DELETE", "");
                        clsGeneric.writeLog("Response form Delete RptPro :" + (responseDel_RptPro));
                    }
                    clsGeneric.writeLog("Upload URL Of AnxJWSale :" + ("http://localhost/Focus8API/Transactions/Vouchers/" + vPName));
                    using (var clientAnxJWSale = new WebClient())
                    {
                        clientAnxJWSale.Encoding = Encoding.UTF8;
                        clientAnxJWSale.Headers.Add("fSessionId", cSessionId);
                        clientAnxJWSale.Headers.Add("Content-Type", "application/json");

                        var responseAnxJWSale = clientAnxJWSale.UploadString("http://localhost/Focus8API/Transactions/Vouchers/" + vPName, sContent_AnxJWSale);
                        clsGeneric.writeLog("Response form RptPro :" + (responseAnxJWSale));
                        if (responseAnxJWSale != null)
                        {
                            var responseDataAnxJWSale = JsonConvert.DeserializeObject<APIResponse.PostResponse>(responseAnxJWSale);
                            if (responseDataAnxJWSale.result == -1)
                            {
                                //UpdateStatus(mivtype, msdocNo, 0, miCompanyId);
                                //return Json(new { status = false, data = new { message = responseDataRptPro.message } });
                                return false;
                            }
                            else
                            {
                                UpdateStatus(cType, cvno, 1, cCompanyId);
                                return true;
                            }
                        }
                    }

                }
            }
            return true;
        }

        static void UpdateBatchs(int miType, int midocDate, string msvno, decimal mdbrate, int miCompanyId)
        {
            BL_DB DataAcesslayer = new BL_DB();
            string strErrorMessage = string.Empty;
            string strValue = "";
            strValue = $@"Update dbo.tCore_Batch_0  set dbo.tCore_Batch_0.iMfDate =" + midocDate +
                ", dbo.tCore_Batch_0.frate=" + mdbrate + " FROM   dbo.tCore_Header_0 AS CH INNER JOIN    dbo.tCore_Data_0 AS CD ON CH.iHeaderId = CD.iHeaderId INNER JOIN " +
                " dbo.tCore_Batch_0 AS B ON CD.iBodyId = B.iBodyId  WHERE LTRIM(RTRIM(ISNULL(B.iMfDate, ''))) <> '' AND  (CH.iVoucherType =" + miType + ") AND (CH.sVoucherNo = N'" + msvno + "')";
            clsGeneric.writeLog("Query :" + strValue);
            DataAcesslayer.GetExecute(strValue, miCompanyId, ref strErrorMessage);
            if (strErrorMessage != "")
            {
                clsGeneric.writeLog("strErrorMessage :" + strErrorMessage);
            }

        }
        static void UpdateStatus(int Type, string vno, int PostingStatus, int CompanyId)
        {
            BL_DB DataAcesslayer = new BL_DB();
            string strErrorMessage = string.Empty;
            string strValue = "";
            strValue = $@"Update dbo.tCore_HeaderData" + Type + "_0  set dbo.tCore_HeaderData" + Type + "_0.PostingStatus=" + PostingStatus +
                " from  dbo.tCore_HeaderData" + Type + "_0 AS CHD INNER JOIN  dbo.tCore_Header_0 AS CH ON CHD.iHeaderId = CH.iHeaderId " +
                "WHERE      (CH.iVoucherType =" + Type + ") AND (CH.sVoucherNo = N'" + vno + "')";
            clsGeneric.writeLog("Query :" + strValue);
            DataAcesslayer.GetExecute(strValue, CompanyId, ref strErrorMessage);
            if (strErrorMessage != "")
            {
                clsGeneric.writeLog("strErrorMessage :" + strErrorMessage);
            }

        }
        public ActionResult CheckAnxJWSale(int CompanyId, string SessionId, string User, int LoginId, int vtype, string docNo, int docDate, int Branch, int Vendor, int Item,double Item_qty)
        {
            double dBOMFGQty = 0;
            double dBOMRMQty = 0;
            int iRMProdId = 0;
            int iCnt = 0;
            double dQtyGRNJWSal = 0;
            double dQtyAnxJWSale = 0;
            double dQtyRetJWSale = 0;
            double dMaxQty = 0; 
            msg.Clear();
            clsGeneric.createtblgetMaxQty_GRNJWSal(CompanyId, User, docNo);
            clsGeneric.Log_write(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), SessionId, CompanyId, docNo, vtype, User);
            clsGeneric.writeLog("fSessionid : " + SessionId);

            BL_DB objDB = new BL_DB();

            strQry = $@"SELECT TCI.iProduct, ABS(TCI.fQuantity) AS RMQuantity, TCHD.FGQty FROM dbo.tCore_Header_0 AS CH " +
                    " INNER JOIN dbo.tCore_Data_0 AS CD ON CH.iHeaderId = CD.iHeaderId INNER JOIN dbo.tCore_Indta_0 AS TCI " +
                    " ON CD.iBodyId = TCI.iBodyId INNER JOIN dbo.tCore_HeaderData7425_0 AS TCHD ON CD.iHeaderId = TCHD.iHeaderId " +
                    " WHERE (CH.iVoucherType = 7425) AND (TCHD.FGCode = "+ Item +") AND (CD.iFaTag = "+ Branch +") AND (CD.iBookNo = "+ Vendor +")";
            clsGeneric.writeLog("Query :" + strQry);
            DataSet ds1 = DataAcesslayer.GetData(strQry, CompanyId, ref strErrorMessage);
            clsGeneric.writeLog("Getting from Voucher:" + (ds1));
            iCnt = 0;
            strQry = $@" delete from tblgetMaxQty_GRNJWSal where svno='" + docNo + "' and loggeduser='" + User + "' and ifgid=" + Item;
            if (ds1 != null)
            {
                
                DataAcesslayer.GetExecute(strQry, CompanyId, ref strErrorMessage);
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                    {
                        iCnt = iCnt + 1; 
                        iRMProdId = Convert.ToInt32(ds1.Tables[0].Rows[j][0]);
                        dBOMRMQty = Convert.ToDouble(ds1.Tables[0].Rows[j][1]);
                        dBOMFGQty = Convert.ToDouble(ds1.Tables[0].Rows[j][2]);
                        strQry = $@"select [dbo].[fCore_GetQty](" + iRMProdId + ","+ Vendor  + "," + Branch + ",'GRNJWSal') ";
                        dQtyGRNJWSal = Convert.ToDouble(clsGeneric.ShowRecord(CompanyId, strQry));
                        // Scalar function fCore_GetQty to get the Qty of RM Abbervation Wise
                        strQry = $@"select [dbo].[fCore_GetQty](" + iRMProdId + "," + Vendor + "," + Branch + ",'AnxJWSale') ";
                        dQtyAnxJWSale  = Convert.ToDouble(clsGeneric.ShowRecord(CompanyId, strQry));

                        strQry = $@"select [dbo].[fCore_GetQty](" + iRMProdId + "," + Vendor + "," + Branch + ",'RetJWSale') ";
                        dQtyRetJWSale = Convert.ToDouble(clsGeneric.ShowRecord(CompanyId, strQry));

                        strQry = $@" insert into tblgetMaxQty_GRNJWSal(iFgId, iRMId,dPlanQty, dRMBOMQty, dFGBOMQty, dGRNJWSalQty, dAnxJWSaleQty, dRetJWSaleQty, iParentProductId, iLevel, iVendor, iBranchId, IPAddress, sVNo, loggeduser) values ( " +
                                    Item + "," + iRMProdId + ","+ Item_qty + "," + dBOMRMQty + "," + dBOMFGQty + "," + dQtyGRNJWSal + "," + dQtyAnxJWSale + "," + dQtyRetJWSale + "," + Item + "," + iCnt + "," + Vendor + "," + Branch + ",'101:10:10:10','" + docNo + "','" + User + "')";
                        DataAcesslayer.GetExecute(strQry, CompanyId, ref strErrorMessage);
                        
                    }
                }

                dMaxQty = Convert.ToDouble(clsGeneric.ShowRecord(CompanyId, "select top 1 abs(((dGRNJWSalQty - (dAnxJWSaleQty + dRetJWSaleQty)))/dRMBOMQty)  from tblgetMaxQty_GRNJWSal order by abs(((dGRNJWSalQty - (dAnxJWSaleQty + dRetJWSaleQty)))/dRMBOMQty) asc"));
                //dMaxQty = 100.10; 
                return Json(new { status = true, data = new { message = dMaxQty}});
            }


            //return View(ds1);
            dMaxQty = Convert.ToDouble(clsGeneric.ShowRecord(CompanyId, "select top 1 abs(((dGRNJWSalQty - (dAnxJWSaleQty + dRetJWSaleQty)))/dRMBOMQty)  from tblgetMaxQty_GRNJWSal abs(((dGRNJWSalQty - (dAnxJWSaleQty + dRetJWSaleQty)))/dRMBOMQty) asc"));
            //dMaxQty = 100.10;
            return Json(new { status = true, data = new { message = dMaxQty } });
            //return new EmptyResult();
        }


    }
}