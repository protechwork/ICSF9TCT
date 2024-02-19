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
//using System.Web.Http.Cors;
using System.Web.Mvc;

namespace ICSF9TCT.Controllers
{

    public class ProdnPlanController : Controller
    {
        #region Variable Definition
        string strQry = string.Empty;
        string strErrorMessage = string.Empty;


        BL_DB DataAcesslayer = new BL_DB();
        static int PostingStatus=0;
        static int PPCRMStatus=0;




        public string vBAbbr = "";
        public string vBName = "";
        public string vPAbbr = "";
        public string vPName = "";
        public int vPvType = 7950;

        public string vPAbbr1 = "";
        public string vPName1 = "";
        public int vPvType1 = 7960;
        StringBuilder msg = new StringBuilder();

        //----- Base Header Variable 
        int bHiDocdate = 0;
        string bHNarration = "";
        string bHPlanMonth = "";
        int bHPPCMonth = 0;
        int bHPPCWeekEndDate = 0;
        int bHPPCWeekStartDate = 0;
        int bHPPCWeekDays = 0;
        int bHPPCDueDate = 0;
        //----- Base Details variable 
        int @iProductId = 0;
        decimal @pqty = 0;
        int id = 0;
        int fgid = 0;
        int pfgid = 0;
        decimal fgqty = 0;

        string PName = "";

        int iWIPWH = 0;
        string Sys_IPAddress = "";
        #endregion
        [HttpPost]
        public ActionResult UpdateProdnPlan(int CompanyId, string SessionId, string User, int LoginId, int vtype, string docNo, List<PEBody> BodyData)
        {
            Sys_IPAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();

            clsGeneric.Log_write(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), SessionId, CompanyId, docNo, vtype, User);
            clsGeneric.writeLog("IP Address :" + Sys_IPAddress);
            clsGeneric.createTableCollectSFG_ProdnPlan(CompanyId, User, docNo);
            PostingStatus = 0;
            BL_DB DataAcesslayer = new BL_DB();
            strQry = $@"SELECT CHD.PostingStatus FROM tCore_HeaderData" + vtype +  "_0 AS CHD INNER JOIN tCore_Header_0 AS CH ON CHD.iHeaderId = CH.iHeaderId " +
                     " WHERE (CH.iVoucherType = " + vtype + ") AND (CH.sVoucherNo = N'" + docNo + "')";

            PostingStatus = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, strQry));

            //0,Pending,1,Updated
            if (PostingStatus == 0)
            {

                try
                {
                    strQry = $@"SELECT  sAbbr  FROM  dbo.cCore_Vouchers_0 WHERE (iVoucherType =" + vtype + " )";
                    vBAbbr = Convert.ToString(clsGeneric.ShowRecord(CompanyId, strQry));
                    strQry = $@"SELECT  sName  FROM  dbo.cCore_Vouchers_0 WHERE (iVoucherType =" + vtype + " )";
                    vBName = Convert.ToString(clsGeneric.ShowRecord(CompanyId, strQry));

                    strQry = $@"SELECT  sAbbr  FROM  dbo.cCore_Vouchers_0 WHERE (iVoucherType =" + vPvType + " )";
                    vPAbbr = Convert.ToString(clsGeneric.ShowRecord(CompanyId, strQry));
                    strQry = $@"SELECT  sName  FROM  dbo.cCore_Vouchers_0 WHERE (iVoucherType =" + vPvType + " )";
                    vPName = Convert.ToString(clsGeneric.ShowRecord(CompanyId, strQry));


                    strQry = $@"SELECT  sAbbr  FROM  dbo.cCore_Vouchers_0 WHERE (iVoucherType =" + vPvType1 + " )";
                    vPAbbr1 = Convert.ToString(clsGeneric.ShowRecord(CompanyId, strQry));
                    strQry = $@"SELECT  sName  FROM  dbo.cCore_Vouchers_0 WHERE (iVoucherType =" + vPvType1 + " )";
                    vPName1 = Convert.ToString(clsGeneric.ShowRecord(CompanyId, strQry));


                    strQry = $@" SELECT CH.iDate, CHD.sNarration, CHD.PlanMonth, CHD.PPCMonth, CI.iProduct,CI.fQuantity,CI.iUnit, " +
                        " CHD.WeekEndDate, CHD.WeekStartDate, CHD.WeekDays, CD.iDueDate " +
                        " FROM dbo.tCore_Header_0 AS CH INNER JOIN  dbo.tCore_Data_0 AS CD ON CH.iHeaderId = CD.iHeaderId INNER JOIN " +
                        " dbo.tCore_Indta_0 AS CI ON CD.iBodyId = CI.iBodyId INNER JOIN dbo.tCore_HeaderData" + vtype + "_0 AS CHD ON " +
                        " CH.iHeaderId = CHD.iHeaderId WHERE (CH.iVoucherType =" + vtype + ") AND (CH.sVoucherNo = N'" + docNo + "') ";
                    DataSet ds6 = DataAcesslayer.GetData(strQry, CompanyId, ref strErrorMessage);
                    if (ds6 != null && ds6.Tables.Count > 0 && ds6.Tables[0].Rows.Count > 0)
                    {
                        bHiDocdate = Convert.ToInt32(ds6.Tables[0].Rows[0][0]);
                        bHNarration = Convert.ToString(ds6.Tables[0].Rows[0][1]);
                        bHPlanMonth = Convert.ToString(ds6.Tables[0].Rows[0][2]);
                        bHPPCMonth = Convert.ToInt32(ds6.Tables[0].Rows[0][3]);
                        bHPPCWeekEndDate = Convert.ToInt32(ds6.Tables[0].Rows[0][7]);
                        bHPPCWeekStartDate = Convert.ToInt32(ds6.Tables[0].Rows[0][8]);
                        bHPPCWeekDays = Convert.ToInt32(ds6.Tables[0].Rows[0][9]);
                        bHPPCDueDate = Convert.ToInt32(ds6.Tables[0].Rows[0][10]);

                    }

                    for (int i = 0; i <= ds6.Tables[0].Rows.Count - 1; i++)
                    {
                        @iProductId = Convert.ToInt32(ds6.Tables[0].Rows[i][4]);
                        @pqty = Convert.ToDecimal(ds6.Tables[0].Rows[i][5]);
                        strQry = $@"exec spICS_CalBomSFG " + @pqty + "," + @iProductId + ",'" + bHPlanMonth + "',1, " + @iProductId + ",'" + User + "'";
                        DataSet ds = DataAcesslayer.GetData(strQry, CompanyId, ref strErrorMessage);

                    }
                    HashData objHashRequest_PPCPlan = new HashData();
                    Hashtable headerPPCPlan = new Hashtable();
                    headerPPCPlan.Add("DocNo", docNo);
                    headerPPCPlan.Add("Date", bHiDocdate);
                    headerPPCPlan.Add("PPCPlanDate", bHiDocdate);
                    //headerPPCPlan.Add("Branch__Id", 2);
                    headerPPCPlan.Add("sNarration", bHNarration);
                    headerPPCPlan.Add("PPCPlanNo", docNo);
                    headerPPCPlan.Add("PPCMonth", bHPPCMonth);
                    headerPPCPlan.Add("ProdnPlanNo", docNo);
                    headerPPCPlan.Add("ProdnPlanDate", bHiDocdate);


                    headerPPCPlan.Add("ReleaseProdnOrder", 0);
                    headerPPCPlan.Add("ReleaseRMReq", 0);
                    headerPPCPlan.Add("WeekStartDate", bHPPCWeekStartDate);
                    headerPPCPlan.Add("WeekEndDate", bHPPCWeekEndDate);
                    headerPPCPlan.Add("WeekDays", bHPPCWeekDays);
                    headerPPCPlan.Add("DueDate", bHPPCDueDate);

                    List<System.Collections.Hashtable> lstBody_PPCPlan = new List<System.Collections.Hashtable>();
                    lstBody_PPCPlan.Clear();

                    strQry = $@" SELECT TOP (100) PERCENT id, iVariantId, iProductId, planQty,BOMQty,SFGReqQty, iInvTagValue,BranchId,WCId,FGId,ParentProductId FROM dbo.TableCollectSFG_PPCPlan AS tm " +
                        " WHERE (loggeduser = '" + User + "') ORDER BY FGId,iLevel,id DESC";
                    //ORDER BY FGId, id DESC, iLevel";
                    DataSet ds2 = DataAcesslayer.GetData(strQry, CompanyId, ref strErrorMessage);
                    if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                    {
                        for (int j = 0; j <= ds2.Tables[0].Rows.Count - 1; j++)
                        {
                            Hashtable bodyPPCPlan = new Hashtable();
                            bodyPPCPlan.Add("Item__Id", Convert.ToInt32(ds2.Tables[0].Rows[j][2]));

                            bodyPPCPlan.Add("Quantity", Convert.ToDecimal(Convert.ToDecimal(ds2.Tables[0].Rows[j][5])));

                            //bodyPPCPlan.Add("Warehouse__Id", Convert.ToInt32(ds2.Tables[0].Rows[j][6]));
                            //BranchId = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, "SELECT Top 1 Branch FROM dbo.muCore_Product_Settings WHERE (iMasterId =" + Convert.ToInt32(ds2.Tables[0].Rows[j][2]) + ")"));
                            //iWIPWH = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, "SELECT Top 1 WIPWH FROM muCore_Department WHERE (iMasterId =" + BranchId + ")"));
                            //bodyPPCPlan.Add("Warehouse__Id", iWIPWH);
                            strQry = $@"SELECT  sName  FROM  dbo.vcCore_Product where ( iMasterId=" + Convert.ToInt32(ds2.Tables[0].Rows[j][2]) + " )";
                            PName = Convert.ToString(clsGeneric.ShowRecord(CompanyId, strQry));
                            if (Convert.ToInt32(ds2.Tables[0].Rows[j][8]) == 0)
                            {
                                return Json(new { status = true, data = new { message = "Product :- " + PName + " , " + "Issue :- Work Centre Missing" } });
                            }
                            bodyPPCPlan.Add("Works Center__Id", Convert.ToInt32(ds2.Tables[0].Rows[j][8]));
                            if (Convert.ToInt32(ds2.Tables[0].Rows[j][7]) == 0)
                            {
                                //clsGeneric.createTableCollectSFG_PPCPlan(CompanyId, User, docNo);
                                return Json(new { status = true, data = new { message = "Product :- " + PName + " , " + "Issue :-  Branch Missing" } });
                            }

                            bodyPPCPlan.Add("Branch__Id", Convert.ToInt32(ds2.Tables[0].Rows[j][7]));
                            iWIPWH = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, "SELECT Top 1 WIPWH FROM muCore_Department WHERE (iMasterId =" + Convert.ToInt32(ds2.Tables[0].Rows[j][7]) + ")"));
                            bodyPPCPlan.Add("Warehouse__Id", iWIPWH);



                            bodyPPCPlan.Add("FGCode", Convert.ToInt32(ds2.Tables[0].Rows[j][9]));



                            Hashtable bodyBOMQtyPPCPlan = new Hashtable
                            {
                                {"Input", Convert.ToDecimal(ds2.Tables[0].Rows[j][4])},
                                {"FieldName","BOM Qty"},
                                {"ColMap", 8},
                                {"Value", Convert.ToDecimal(ds2.Tables[0].Rows[j][4])}
                            };
                            Hashtable bodySFGReqQtyPPCPlan = new Hashtable
                            {
                                {"Input", Convert.ToDecimal(ds2.Tables[0].Rows[j][5])},
                                {"FieldName","SFG Req Qty"},
                                {"ColMap", 9},
                                {"Value", Convert.ToDecimal(ds2.Tables[0].Rows[j][5])}
                            };
                            Hashtable bodyPlanQtyPPCPlan = new Hashtable
                            {
                                {"Input", Convert.ToDecimal(ds2.Tables[0].Rows[j][3])},
                                {"FieldName","Plan Qty"},
                                {"ColMap", 7},
                                {"Value", Convert.ToDecimal(ds2.Tables[0].Rows[j][3])}
                            };
                            bodyPPCPlan.Add("Plan Qty", bodyPlanQtyPPCPlan);
                            bodyPPCPlan.Add("SFG Req Qty", bodySFGReqQtyPPCPlan);
                            bodyPPCPlan.Add("BOM Qty", bodyBOMQtyPPCPlan);
                            // 29-03-2022 added by Azhar Requested By Majid Sir 
                            bodyPPCPlan.Add("ParentFGCode__Id", Convert.ToInt32(ds2.Tables[0].Rows[j][10]));
                            lstBody_PPCPlan.Add(bodyPPCPlan);
                        }
                    }

                    System.Collections.Hashtable objHash = new System.Collections.Hashtable();
                    objHash.Add("Body", lstBody_PPCPlan);
                    objHash.Add("Header", headerPPCPlan);
                    List<System.Collections.Hashtable> lstHash = new List<System.Collections.Hashtable>();
                    lstHash.Add(objHash);
                    objHashRequest_PPCPlan.data = lstHash;
                    string sContentPPCPlan = JsonConvert.SerializeObject(objHashRequest_PPCPlan);
                    clsGeneric.writeLog("Content of PPCPlan :" + sContentPPCPlan);
                    clsGeneric.writeLog("URL of PPCPlan :" + "http://localhost/Focus8API/Transactions/Vouchers/" + vPName);
                    using (var clientPPCPlan = new WebClient())
                    {
                        clientPPCPlan.Encoding = Encoding.UTF8;
                        clientPPCPlan.Headers.Add("fSessionId", SessionId);
                        clientPPCPlan.Headers.Add("Content-Type", "application/json");

                        var responsePPCPlan = clientPPCPlan.UploadString("http://localhost/Focus8API/Transactions/Vouchers/" + vPName, sContentPPCPlan);

                        clsGeneric.writeLog("Response form PPCPlan :" + (responsePPCPlan));
                        if (responsePPCPlan != null)
                        {
                            var responseDataPPCPlan = JsonConvert.DeserializeObject<APIResponse.PostResponse>(responsePPCPlan);
                            if (responseDataPPCPlan.result == -1)
                            {
                                clsGeneric.createTableCollectSFG_ProdnPlan(CompanyId, User, docNo);
                                UpdateStatus(vtype, docNo, 0, CompanyId);
                                return Json(new { status = false, data = new { message = responseDataPPCPlan.message } });
                            }
                            else
                            {
                                clsGeneric.createTableCollectSFG_ProdnPlan(CompanyId, User, docNo);
                                //clsGeneric.createTableCollectSFG_PPCPlanRMReq(CompanyId, User, docNo);
                                //clsGeneric.createTableRMRequisition_PPCPlanRMReq(CompanyId, User, docNo);
                                //if (sUpdatePPCRMReq(CompanyId, SessionId, User, LoginId, vtype, docNo, BodyData))
                                //{
                                    UpdateStatus(vtype, docNo, 1, CompanyId);
                                    //clsGeneric.createTableCollectSFG_PPCPlanRMReq(CompanyId, User, docNo);
                                    //clsGeneric.createTableRMRequisition_PPCPlanRMReq(CompanyId, User, docNo);
                                    return Json(new { status = true, data = new { message = "Posting Successful" } });

                                //}
                                //else
                                //{
                                //    UpdateStatus(vtype, docNo, 0, CompanyId);
                                //    clsGeneric.createTableCollectSFG_PPCPlanRMReq(CompanyId, User, docNo);
                                //    clsGeneric.createTableRMRequisition_PPCPlanRMReq(CompanyId, User, docNo);
                                //    return Json(new { status = false, data = new { message = "Posting failed" } });
                                //}
                            }

                        }

                    }

                    return Json(new { status = true, data = new { message = "Posting Successful " } });

                }

                catch (Exception ex)
                {
                    clsGeneric.createTableCollectSFG_ProdnPlan(CompanyId, User, docNo);
                    clsGeneric.writeLog("Exception occured:" + (ex.Message));
                    return Json(new { status = false, data = new { message = "Posting Failed " } });
                    throw;
                }
            }
            else
            {
                return Json(new { status = true, data = new { message = "Document already Posted" } });
            }

        }

        [HttpPost]
        public ActionResult LUpdateProdnPlan(int CompanyId, string SessionId, string User, int LoginId, int vtype, string docNo, List<PEBody> BodyData)
        {
            Sys_IPAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();

            clsGeneric.Log_write(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), SessionId, CompanyId, docNo, vtype, User);
            clsGeneric.writeLog("IP Address :" + Sys_IPAddress);
            clsGeneric.createTableCollectSFG_ProdnPlan(CompanyId, User, docNo);
            PostingStatus = 0;
            BL_DB DataAcesslayer = new BL_DB();
            strQry = $@"SELECT CHD.PostingStatus FROM tCore_HeaderData" + vtype + "_0 AS CHD INNER JOIN tCore_Header_0 AS CH ON CHD.iHeaderId = CH.iHeaderId " +
                     " WHERE (CH.iVoucherType = " + vtype + ") AND (CH.sVoucherNo = N'" + docNo + "')";

            PostingStatus = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, strQry));

            //0,Pending,1,Updated
            if (PostingStatus == 0)
            {

                try
                {
                    strQry = $@"SELECT  sAbbr  FROM  dbo.cCore_Vouchers_0 WHERE (iVoucherType =" + vtype + " )";
                    vBAbbr = Convert.ToString(clsGeneric.ShowRecord(CompanyId, strQry));
                    strQry = $@"SELECT  sName  FROM  dbo.cCore_Vouchers_0 WHERE (iVoucherType =" + vtype + " )";
                    vBName = Convert.ToString(clsGeneric.ShowRecord(CompanyId, strQry));

                    strQry = $@"SELECT  sAbbr  FROM  dbo.cCore_Vouchers_0 WHERE (iVoucherType =" + vPvType + " )";
                    vPAbbr = Convert.ToString(clsGeneric.ShowRecord(CompanyId, strQry));
                    strQry = $@"SELECT  sName  FROM  dbo.cCore_Vouchers_0 WHERE (iVoucherType =" + vPvType + " )";
                    vPName = Convert.ToString(clsGeneric.ShowRecord(CompanyId, strQry));


                    strQry = $@"SELECT  sAbbr  FROM  dbo.cCore_Vouchers_0 WHERE (iVoucherType =" + vPvType1 + " )";
                    vPAbbr1 = Convert.ToString(clsGeneric.ShowRecord(CompanyId, strQry));
                    strQry = $@"SELECT  sName  FROM  dbo.cCore_Vouchers_0 WHERE (iVoucherType =" + vPvType1 + " )";
                    vPName1 = Convert.ToString(clsGeneric.ShowRecord(CompanyId, strQry));


                    strQry = $@" SELECT CH.iDate, CHD.sNarration, CHD.PlanMonth, CHD.PPCMonth, CI.iProduct,CI.fQuantity,CI.iUnit, " +
                        " CHD.WeekEndDate, CHD.WeekStartDate, CHD.WeekDays, CD.iDueDate " +
                        " FROM dbo.tCore_Header_0 AS CH INNER JOIN  dbo.tCore_Data_0 AS CD ON CH.iHeaderId = CD.iHeaderId INNER JOIN " +
                        " dbo.tCore_Indta_0 AS CI ON CD.iBodyId = CI.iBodyId INNER JOIN dbo.tCore_HeaderData" + vtype + "_0 AS CHD ON " +
                        " CH.iHeaderId = CHD.iHeaderId WHERE (CH.iVoucherType =" + vtype + ") AND (CH.sVoucherNo = N'" + docNo + "') ";
                    DataSet ds6 = DataAcesslayer.GetData(strQry, CompanyId, ref strErrorMessage);
                    if (ds6 != null && ds6.Tables.Count > 0 && ds6.Tables[0].Rows.Count > 0)
                    {
                        bHiDocdate = Convert.ToInt32(ds6.Tables[0].Rows[0][0]);
                        bHNarration = Convert.ToString(ds6.Tables[0].Rows[0][1]);
                        bHPlanMonth = Convert.ToString(ds6.Tables[0].Rows[0][2]);
                        bHPPCMonth = Convert.ToInt32(ds6.Tables[0].Rows[0][3]);
                        bHPPCWeekEndDate = Convert.ToInt32(ds6.Tables[0].Rows[0][7]);
                        bHPPCWeekStartDate = Convert.ToInt32(ds6.Tables[0].Rows[0][8]);
                        bHPPCWeekDays = Convert.ToInt32(ds6.Tables[0].Rows[0][9]);
                        bHPPCDueDate = Convert.ToInt32(ds6.Tables[0].Rows[0][10]);

                    }

                    for (int i = 0; i <= ds6.Tables[0].Rows.Count - 1; i++)
                    {
                        @iProductId = Convert.ToInt32(ds6.Tables[0].Rows[i][4]);
                        @pqty = Convert.ToDecimal(ds6.Tables[0].Rows[i][5]);
                        strQry = $@"exec spICS_CalBomSFG " + @pqty + "," + @iProductId + ",'" + bHPlanMonth + "',1, " + @iProductId + ",'" + User + "'";
                        DataSet ds = DataAcesslayer.GetData(strQry, CompanyId, ref strErrorMessage);

                    }
                    HashData objHashRequest_ProdnPlan = new HashData();
                    Hashtable headerPPCPlan = new Hashtable();
                    headerPPCPlan.Add("DocNo", docNo);
                    headerPPCPlan.Add("Date", bHiDocdate);
                    headerPPCPlan.Add("PPCPlanDate", bHiDocdate);
                    //headerPPCPlan.Add("Branch__Id", 2);
                    headerPPCPlan.Add("sNarration", bHNarration);
                    headerPPCPlan.Add("PPCPlanNo", docNo);
                    headerPPCPlan.Add("PPCMonth", bHPPCMonth);
                    headerPPCPlan.Add("ProdnPlanNo", docNo);
                    headerPPCPlan.Add("ProdnPlanDate", bHiDocdate);
                    headerPPCPlan.Add("ReleaseProdnOrder", 0);
                    headerPPCPlan.Add("ReleaseRMReq", 0);
                    headerPPCPlan.Add("WeekStartDate", bHPPCWeekStartDate);
                    headerPPCPlan.Add("WeekEndDate", bHPPCWeekEndDate);
                    headerPPCPlan.Add("WeekDays", bHPPCWeekDays);
                    headerPPCPlan.Add("DueDate", bHPPCDueDate);

                    List<System.Collections.Hashtable> lstBody_ProdnPlan = new List<System.Collections.Hashtable>();
                    lstBody_ProdnPlan.Clear();

                    strQry = $@" SELECT TOP (100) PERCENT id, iVariantId, iProductId, planQty,BOMQty,SFGReqQty, iInvTagValue,BranchId,WCId,FGId,ParentProductId FROM dbo.TableCollectSFG_ProdnPlan AS tm " +
                        " WHERE (loggeduser = '" + User + "') ORDER BY FGId,iLevel,id DESC";
                    //ORDER BY FGId, id DESC, iLevel";
                    DataSet ds2 = DataAcesslayer.GetData(strQry, CompanyId, ref strErrorMessage);
                    if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                    {
                        for (int j = 0; j <= ds2.Tables[0].Rows.Count - 1; j++)
                        {
                            Hashtable bodyPPCPlan = new Hashtable();
                            bodyPPCPlan.Add("Item__Id", Convert.ToInt32(ds2.Tables[0].Rows[j][2]));

                            bodyPPCPlan.Add("Quantity", Convert.ToDecimal(Convert.ToDecimal(ds2.Tables[0].Rows[j][5])));

                            //bodyPPCPlan.Add("Warehouse__Id", Convert.ToInt32(ds2.Tables[0].Rows[j][6]));
                            //BranchId = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, "SELECT Top 1 Branch FROM dbo.muCore_Product_Settings WHERE (iMasterId =" + Convert.ToInt32(ds2.Tables[0].Rows[j][2]) + ")"));
                            //iWIPWH = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, "SELECT Top 1 WIPWH FROM muCore_Department WHERE (iMasterId =" + BranchId + ")"));
                            //bodyPPCPlan.Add("Warehouse__Id", iWIPWH);
                            strQry = $@"SELECT  sName  FROM  dbo.vcCore_Product where ( iMasterId=" + Convert.ToInt32(ds2.Tables[0].Rows[j][2]) + " )";
                            PName = Convert.ToString(clsGeneric.ShowRecord(CompanyId, strQry));
                            if (Convert.ToInt32(ds2.Tables[0].Rows[j][8]) == 0)
                            {
                                return Json(new { status = true, data = new { message = "Product :- " + PName + " , " + "Issue :- Work Centre Missing" } });
                            }
                            bodyPPCPlan.Add("Works Center__Id", Convert.ToInt32(ds2.Tables[0].Rows[j][8]));
                            if (Convert.ToInt32(ds2.Tables[0].Rows[j][7]) == 0)
                            {
                                //clsGeneric.createTableCollectSFG_ProdnPlan(CompanyId, User, docNo);
                                return Json(new { status = true, data = new { message = "Product :- " + PName + " , " + "Issue :-  Branch Missing" } });
                            }

                            bodyPPCPlan.Add("Branch__Id", Convert.ToInt32(ds2.Tables[0].Rows[j][7]));
                            iWIPWH = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, "SELECT Top 1 WIPWH FROM muCore_Department WHERE (iMasterId =" + Convert.ToInt32(ds2.Tables[0].Rows[j][7]) + ")"));
                            bodyPPCPlan.Add("Warehouse__Id", iWIPWH);

                            
                            bodyPPCPlan.Add("FGCode", Convert.ToInt32(ds2.Tables[0].Rows[j][9]));



                            Hashtable bodyBOMQtyPPCPlan = new Hashtable
                            {
                                {"Input", Convert.ToDecimal(ds2.Tables[0].Rows[j][4])},
                                {"FieldName","BOM Qty"},
                                {"ColMap", 8},
                                {"Value", Convert.ToDecimal(ds2.Tables[0].Rows[j][4])}
                            };
                            Hashtable bodySFGReqQtyPPCPlan = new Hashtable
                            {
                                {"Input", Convert.ToDecimal(ds2.Tables[0].Rows[j][5])},
                                {"FieldName","SFG Req Qty"},
                                {"ColMap", 9},
                                {"Value", Convert.ToDecimal(ds2.Tables[0].Rows[j][5])}
                            };
                            Hashtable bodyPlanQtyPPCPlan = new Hashtable
                            {
                                {"Input", Convert.ToDecimal(ds2.Tables[0].Rows[j][3])},
                                {"FieldName","Plan Qty"},
                                {"ColMap", 7},
                                {"Value", Convert.ToDecimal(ds2.Tables[0].Rows[j][3])}
                            };
                            bodyPPCPlan.Add("Plan Qty", bodyPlanQtyPPCPlan);
                            bodyPPCPlan.Add("SFG Req Qty", bodySFGReqQtyPPCPlan);
                            bodyPPCPlan.Add("BOM Qty", bodyBOMQtyPPCPlan);
                            // 29-03-2022 added by Azhar Requested By Majid Sir 
                            bodyPPCPlan.Add("ParentFGCode__Id", Convert.ToInt32(ds2.Tables[0].Rows[j][10]));
                            lstBody_ProdnPlan.Add(bodyPPCPlan);
                        }
                    }

                    System.Collections.Hashtable objHash = new System.Collections.Hashtable();
                    objHash.Add("Body", lstBody_ProdnPlan);
                    objHash.Add("Header", headerPPCPlan);
                    List<System.Collections.Hashtable> lstHash = new List<System.Collections.Hashtable>();
                    lstHash.Add(objHash);
                    objHashRequest_ProdnPlan.data = lstHash;
                    string sContentPPCPlan = JsonConvert.SerializeObject(objHashRequest_ProdnPlan);
                    clsGeneric.writeLog("Content of PPCPlan :" + sContentPPCPlan);
                    clsGeneric.writeLog("URL of PPCPlan :" + "http://localhost/Focus8API/Transactions/Vouchers/" + vPName);
                    using (var clientPPCPlan = new WebClient())
                    {
                        clientPPCPlan.Encoding = Encoding.UTF8;
                        clientPPCPlan.Headers.Add("fSessionId", SessionId);
                        clientPPCPlan.Headers.Add("Content-Type", "application/json");

                        var responsePPCPlan = clientPPCPlan.UploadString("http://localhost/Focus8API/Transactions/Vouchers/" + vPName, sContentPPCPlan);

                        clsGeneric.writeLog("Response form PPCPlan :" + (responsePPCPlan));
                        if (responsePPCPlan != null)
                        {
                            var responseDataPPCPlan = JsonConvert.DeserializeObject<APIResponse.PostResponse>(responsePPCPlan);
                            if (responseDataPPCPlan.result == -1)
                            {
                                clsGeneric.createTableCollectSFG_ProdnPlan(CompanyId, User, docNo);
                                UpdateStatus(vtype, docNo, 0, CompanyId);
                                return Json(new { status = false, data = new { message = responseDataPPCPlan.message } });
                            }
                            else
                            {
                                clsGeneric.createTableCollectSFG_ProdnPlan(CompanyId, User, docNo);
                                //clsGeneric.createTableCollectSFG_ProdnPlanRMReq(CompanyId, User, docNo);
                                //clsGeneric.createTableRMRequisition_ProdnPlanRMReq(CompanyId, User, docNo);



                                //if (sUpdatePPCRMReq(CompanyId, SessionId, User, LoginId, vtype, docNo, BodyData))
                                //{
                                //    UpdateStatus(vtype, docNo, 1, CompanyId);
                                //    clsGeneric.createTableCollectSFG_ProdnPlanRMReq(CompanyId, User, docNo);
                                //    clsGeneric.createTableRMRequisition_ProdnPlanRMReq(CompanyId, User, docNo);
                                //    return Json(new { status = true, data = new { message = "Posting Successful" } });

                                //}
                                //else
                                //{
                                //    UpdateStatus(vtype, docNo, 0, CompanyId);
                                //    clsGeneric.createTableCollectSFG_ProdnPlanRMReq(CompanyId, User, docNo);
                                //    clsGeneric.createTableRMRequisition_ProdnPlanRMReq(CompanyId, User, docNo);
                                //    return Json(new { status = false, data = new { message = "Posting failed" } });
                                //}
                            }

                        }

                    }

                    return Json(new { status = true, data = new { message = "Posting Successful " } });

                }

                catch (Exception ex)
                {
                    clsGeneric.createTableCollectSFG_ProdnPlan(CompanyId, User, docNo);
                    clsGeneric.writeLog("Exception occured:" + (ex.Message));
                    return Json(new { status = false, data = new { message = "Posting Failed " } });
                    throw;
                }
            }
            else
            {
                return Json(new { status = true, data = new { message = "Document already Posted" } });
            }

        }

        [HttpPost]
        //public ActionResult UpdatePPCRMReq(int CompanyId, string SessionId, string User, int LoginId, int vtype, string docNo, List<PEBody> BodyData)
        //{
        //    Sys_IPAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();
        //    clsGeneric.Log_write(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), SessionId, CompanyId, docNo, vtype, User);
        //    clsGeneric.writeLog("IP Address :" + Sys_IPAddress);
        //    clsGeneric.createTableCollectSFG_PPCPlanRMReq(CompanyId, User, docNo);
        //    clsGeneric.createTableRMRequisition_PPCPlanRMReq(CompanyId, User, docNo);
        //    strQry = $@"SELECT  sAbbr  FROM  dbo.cCore_Vouchers_0 WHERE (iVoucherType =" + vPvType1 + " )";
        //    vPAbbr1 = Convert.ToString(clsGeneric.ShowRecord(CompanyId, strQry));
        //    strQry = $@"SELECT  sName  FROM  dbo.cCore_Vouchers_0 WHERE (iVoucherType =" + vPvType1 + " )";
        //    vPName1 = Convert.ToString(clsGeneric.ShowRecord(CompanyId, strQry));
        //    try
        //    {
        //        BL_DB DataAcesslayer = new BL_DB();
        //        string strErrorMessage = string.Empty;
        //        string strQry = string.Empty;

        //        strQry = $@"SELECT CHD.PPCRMStatus FROM tCore_HeaderData" + vtype + "_0 AS CHD INNER JOIN tCore_Header_0 AS CH ON CHD.iHeaderId = CH.iHeaderId " +
        //             " WHERE (CH.iVoucherType = " + vtype + ") AND (CH.sVoucherNo = N'" + docNo + "')";
        //        PPCRMStatus = 0;
        //        PPCRMStatus = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, strQry));

        //        if (PPCRMStatus == 0)
        //        {

        //            strQry = $@" SELECT CH.iDate, CHD.sNarration, CHD.PlanMonth, CHD.PPCMonth, CI.iProduct,CI.fQuantity,CI.iUnit, " +
        //                " CHD.WeekEndDate, CHD.WeekStartDate, CHD.WeekDays, CD.iDueDate " +
        //                " FROM dbo.tCore_Header_0 AS CH INNER JOIN  dbo.tCore_Data_0 AS CD ON CH.iHeaderId = CD.iHeaderId INNER JOIN " +
        //                " dbo.tCore_Indta_0 AS CI ON CD.iBodyId = CI.iBodyId INNER JOIN dbo.tCore_HeaderData" + vtype + "_0 AS CHD ON " +
        //                " CH.iHeaderId = CHD.iHeaderId WHERE (CH.iVoucherType =" + vtype + ") AND (CH.sVoucherNo = N'" + docNo + "') ";
        //            DataSet ds6 = DataAcesslayer.GetData(strQry, CompanyId, ref strErrorMessage);

        //            clsGeneric.writeLog("Rows Count :" + ds6.Tables[0].Rows.Count);
        //            if (strErrorMessage != "")
        //            {
        //                clsGeneric.writeLog("strErrorMessage :" + strErrorMessage);
        //            }
        //            if (ds6 != null && ds6.Tables.Count > 0 && ds6.Tables[0].Rows.Count > 0)
        //            {
        //                bHiDocdate = Convert.ToInt32(ds6.Tables[0].Rows[0][0]);
        //                bHNarration = Convert.ToString(ds6.Tables[0].Rows[0][1]);
        //                bHPlanMonth = Convert.ToString(ds6.Tables[0].Rows[0][2]);
        //                bHPPCMonth = Convert.ToInt32(ds6.Tables[0].Rows[0][3]);
        //                bHPPCWeekEndDate = Convert.ToInt32(ds6.Tables[0].Rows[0][7]);
        //                bHPPCWeekStartDate = Convert.ToInt32(ds6.Tables[0].Rows[0][8]);
        //                bHPPCWeekDays = Convert.ToInt32(ds6.Tables[0].Rows[0][9]);
        //                bHPPCDueDate = Convert.ToInt32(ds6.Tables[0].Rows[0][10]);

        //            }
        //            @iProductId = 0;
        //            @pqty = 0;
        //            for (int i = 0; i <= ds6.Tables[0].Rows.Count - 1; i++)
        //            {
        //                @iProductId = Convert.ToInt32(ds6.Tables[0].Rows[i][4]);
        //                @pqty = Convert.ToDecimal(ds6.Tables[0].Rows[i][5]);
        //                strQry = $@"exec spICS_CalBomSFGRMReq " + @pqty + "," + @iProductId + ",'" + bHPlanMonth + "',1, " + @iProductId + ",'" + docNo + "','" + User + "'";
        //                DataSet ds = DataAcesslayer.GetData(strQry, CompanyId, ref strErrorMessage);

        //            }
        //            strQry = $@" select * from TableCollectSFG_PPCPlanRMReq where vno='" + docNo + "' and loggeduser='" + User + "' Order By id ";
        //            DataSet ds1 = DataAcesslayer.GetData(strQry, CompanyId, ref strErrorMessage);

        //            clsGeneric.writeLog("Rows Count :" + ds1.Tables[0].Rows.Count);
        //            if (strErrorMessage != "")
        //            {
        //                clsGeneric.writeLog("strErrorMessage :" + strErrorMessage);
        //            }
        //            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
        //            {
        //                for (int j = 0; j <= ds6.Tables[0].Rows.Count; j++)
        //                {
        //                    id = Convert.ToInt32(ds1.Tables[0].Rows[j][0]);
        //                    fgid = Convert.ToInt32(ds1.Tables[0].Rows[j][2]);
        //                    fgqty = Convert.ToDecimal(ds1.Tables[0].Rows[j][5]);
        //                    strQry = $@"select FGID from TableCollectSFG_PPCPlanRMReq where id=" + id;
        //                    pfgid = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, strQry));

        //                    strQry = $@" insert into RMRequisition_PPCPlanRMReq (refid,Product,RMqty,fQty,planQty,SalQty,BRMReq,branch,worksCenter,ParentId,MParentID,warehouse,Preq,iBodyId, vno, loggeduser) " +
        //                                " Select " + id + ",BB.iProductId, bb.fQty," + fgqty + "," + fgqty + ",bb.fQty * " + fgqty + ",bb.fQty * " + fgqty + " * fqty, 1,2," + fgid + "," + pfgid + ",bb.iInvTagValue,0,bb.iBomBodyId,'" + docNo + "','" + User + "'" +
        //                                " FROM dbo.mMRP_BomVariantHeader AS BVH INNER JOIN " +
        //                                " dbo.mMRP_BomHeader AS BH ON BH.iBomId = BVH.iBomId INNER JOIN " +
        //                                " dbo.mMRP_BOMBody AS BB ON BB.iVariantId = BVH.iVariantId " +
        //                                " WHERE(bb.iVariantId IN(SELECT top 1 iVariantId  FROM dbo.ICS_BOMVariant AS VB WHERE(iProductId = " + fgid + ")Order by Iversion desc)) " +
        //                                " and(iProductId NOT IN(SELECT iProductId FROM  dbo.ICS_BOMVariant AS Vc)) AND " +
        //                                " (bMainOutPut = 0) AND(bInput = 1) Order by Iversion desc ";
        //                    clsGeneric.writeLog("Query :" + strQry);
        //                    DataAcesslayer.GetExecute(strQry, CompanyId, ref strErrorMessage);
        //                    if (strErrorMessage != "")
        //                    {
        //                        clsGeneric.writeLog("strErrorMessage :" + strErrorMessage);
        //                    }

        //                }

        //                Hashtable headerPPCRMReq = new Hashtable();
        //                headerPPCRMReq.Add("DocNo", docNo);
        //                headerPPCRMReq.Add("Date", bHiDocdate);
        //                headerPPCRMReq.Add("sNarration", bHNarration);
        //                headerPPCRMReq.Add("PlanMonth", bHPlanMonth);
        //                headerPPCRMReq.Add("PPCMonth", bHPPCMonth);
        //                headerPPCRMReq.Add("WeekStartDate", bHPPCWeekStartDate);
        //                headerPPCRMReq.Add("WeekEndDate", bHPPCWeekEndDate);
        //                headerPPCRMReq.Add("WeekDays", bHPPCWeekDays);
        //                List<System.Collections.Hashtable> lstBody = new List<System.Collections.Hashtable>();

        //                for (int k = 0; k <= ds1.Tables[0].Rows.Count - 1; k++)
        //                {
        //                    id = Convert.ToInt32(ds1.Tables[0].Rows[k][0]);
        //                    strQry = $@" select * from RMRequisition_PPCPlanRMReq where refid=" + id + " and vno='" + docNo + "' and loggeduser='" + User + "' Order By id ";

        //                    DataSet ds2 = DataAcesslayer.GetData(strQry, CompanyId, ref strErrorMessage);

        //                    clsGeneric.writeLog("Rows Count :" + ds2.Tables[0].Rows.Count);
        //                    if (strErrorMessage != "")
        //                    {
        //                        clsGeneric.writeLog("strErrorMessage :" + strErrorMessage);
        //                    }
        //                    if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
        //                    {
        //                        for (int j1 = 0; j1 <= ds2.Tables[0].Rows.Count - 1; j1++)
        //                        {
        //                            Hashtable bodyPPCRMReq = new Hashtable();
        //                            bodyPPCRMReq.Add("Item__Id", ds2.Tables[0].Rows[j1][6]);
        //                            //Change by Rizwan 26-06-2022 Replace FGQty to RMQty
        //                            bodyPPCRMReq.Add("Quantity", ds2.Tables[0].Rows[j1][10]);
        //                            bodyPPCRMReq.Add("ParentFGCode__Id", ds1.Tables[0].Rows[k][2]);
        //                            bodyPPCRMReq.Add("Branch__Id", Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, "select branch from muCore_Product_Settings where iMasterId=" +  ds1.Tables[0].Rows[k][2] )));

        //                            //bodyPPCRMReq.Add("FGCode__Id", ds1.Tables[0].Rows[k][11]);
        //                            Hashtable bodyPPCPlanQty = new Hashtable
        //                                {
        //                                    {"Input", ds2.Tables[0].Rows[j1][9]},
        //                                    {"FieldName",  "PPC Plan Qty" },
        //                                    {"ColMap", 0},
        //                                    {"Value",  ds2.Tables[0].Rows[j1][9]}
        //                                };

        //                            bodyPPCRMReq.Add("PPC Plan Qty", bodyPPCPlanQty);
        //                            bodyPPCRMReq.Add("FGCode__Id", ds1.Tables[0].Rows[k][13]);
        //                            lstBody.Add(bodyPPCRMReq);
        //                        }
        //                    }


        //                }
        //                System.Collections.Hashtable objHash = new System.Collections.Hashtable();
        //                objHash.Add("Body", lstBody);
        //                objHash.Add("Header", headerPPCRMReq);
        //                List<System.Collections.Hashtable> lstHash = new List<System.Collections.Hashtable>();
        //                lstHash.Add(objHash);
        //                HashData objHashRequest = new HashData();
        //                objHashRequest.data = lstHash;
        //                string sContentPPCRMReq = JsonConvert.SerializeObject(objHashRequest);
        //                clsGeneric.writeLog("Content " + vPName1 + " : " + sContentPPCRMReq);
        //                clsGeneric.writeLog("Upload URL: " + "http://localhost/Focus8API/Transactions/Vouchers/" + vPName1);

        //                using (var clientPPCRMReq = new WebClient())
        //                {
        //                    clientPPCRMReq.Encoding = Encoding.UTF8;
        //                    clientPPCRMReq.Headers.Add("fSessionId", SessionId);
        //                    clientPPCRMReq.Headers.Add("Content-Type", "application/json");
        //                    var responsePPCRMReq = clientPPCRMReq.UploadString("http://localhost/Focus8API/Transactions/Vouchers/" + vPName1, sContentPPCRMReq);
        //                    clsGeneric.writeLog("response PPCRMReq: " + responsePPCRMReq);
        //                    if (responsePPCRMReq != null)
        //                    {
        //                        var responseDataPPCRMReq = JsonConvert.DeserializeObject<APIResponse.PostResponse>(responsePPCRMReq);
        //                        if (responseDataPPCRMReq.result == -1)
        //                        {
        //                            UpdateStatusPPCRMReq(vtype, docNo, 0, CompanyId);
        //                            clsGeneric.createTableCollectSFG_PPCPlanRMReq(CompanyId, User, docNo);
        //                            clsGeneric.createTableRMRequisition_PPCPlanRMReq(CompanyId, User, docNo);
        //                            return Json(new { status = false, data = new { message = "Posting Failed " } });
        //                        }
        //                        else
        //                        {
        //                            UpdateStatusPPCRMReq(vtype, docNo, 1, CompanyId);
        //                            clsGeneric.createTableCollectSFG_PPCPlanRMReq(CompanyId, User, docNo);
        //                            clsGeneric.createTableRMRequisition_PPCPlanRMReq(CompanyId, User, docNo);
        //                            return Json(new { status = true, data = new { message = "Posting Successful " } });

        //                        }
        //                    }

        //                }

        //                return Json(new { status = false, data = new { message = "Posting Failed " } });

        //            }
        //            else
        //            {
        //                //return false;
        //                return Json(new { status = false, data = new { message = "Posting Failed " } });
        //            }
        //        }
        //        else
        //        {
        //            return Json(new { status = true, data = new { message = "Document already Posted" } });
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        clsGeneric.writeLog("Exception occured:" + (ex.Message));
        //        clsGeneric.createTableCollectSFG_PPCPlanRMReq(CompanyId, User, docNo);
        //        clsGeneric.createTableRMRequisition_PPCPlanRMReq(CompanyId, User, docNo);
        //        return Json(new { status = false, data = new { message = ex.Message } });
        //        throw;
        //    }

        //}

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
        static void UpdateStatusPPCRMReq(int Type, string vno, int PPCRMStatus, int CompanyId)
        {
            BL_DB DataAcesslayer = new BL_DB();
            string strErrorMessage = string.Empty;
            string strValue = "";
            strValue = $@"Update dbo.tCore_HeaderData" + Type + "_0  set dbo.tCore_HeaderData" + Type + "_0.PPCRMStatus=" + PPCRMStatus +
                " from  dbo.tCore_HeaderData" + Type + "_0 AS CHD INNER JOIN  dbo.tCore_Header_0 AS CH ON CHD.iHeaderId = CH.iHeaderId " +
                "WHERE      (CH.iVoucherType =" + Type + ") AND (CH.sVoucherNo = N'" + vno + "')";
            clsGeneric.writeLog("Query :" + strValue);
            DataAcesslayer.GetExecute(strValue, CompanyId, ref strErrorMessage);
            if (strErrorMessage != "")
            {
                clsGeneric.writeLog("strErrorMessage :" + strErrorMessage);
            }

        }
        private bool sUpdatePPCRMReq(int sCompanyId, string sSessionId, string sUser, int sLoginId, int svtype, string sdocNo, List<PEBody> sBodyData)
        {
            clsGeneric.writeLog("Method Name :" + "sUpdatePPCRMReq");
            //strQry = $@"SELECT  sAbbr  FROM  dbo.cCore_Vouchers_0 WHERE (iVoucherType =" + vPvType1 + " )";
            //vPAbbr1 = Convert.ToString(clsGeneric.ShowRecord(CompanyId, strQry));
            //strQry = $@"SELECT  sName  FROM  dbo.cCore_Vouchers_0 WHERE (iVoucherType =" + vPvType1 + " )";
            //vPName1 = Convert.ToString(clsGeneric.ShowRecord(CompanyId, strQry));
            try
            {
                BL_DB DataAcesslayer = new BL_DB();
                string strErrorMessage = string.Empty;
                string strQry = string.Empty;
                bool blnupdate = false;
                strQry = $@" SELECT CH.iDate, CHD.sNarration, CHD.PlanMonth, CHD.PPCMonth, CI.iProduct,CI.fQuantity,CI.iUnit, " +
                        " CHD.WeekEndDate, CHD.WeekStartDate, CHD.WeekDays, CD.iDueDate " +
                        " FROM dbo.tCore_Header_0 AS CH INNER JOIN  dbo.tCore_Data_0 AS CD ON CH.iHeaderId = CD.iHeaderId INNER JOIN " +
                        " dbo.tCore_Indta_0 AS CI ON CD.iBodyId = CI.iBodyId INNER JOIN dbo.tCore_HeaderData" + svtype + "_0 AS CHD ON " +
                        " CH.iHeaderId = CHD.iHeaderId WHERE (CH.iVoucherType =" + svtype + ") AND (CH.sVoucherNo = N'" + sdocNo + "') ";
                DataSet ds6 = DataAcesslayer.GetData(strQry, sCompanyId, ref strErrorMessage);

                clsGeneric.writeLog("Rows Count :" + ds6.Tables[0].Rows.Count);
                if (strErrorMessage != "")
                {
                    clsGeneric.writeLog("strErrorMessage :" + strErrorMessage);
                }
                if (ds6 != null && ds6.Tables.Count > 0 && ds6.Tables[0].Rows.Count > 0)
                {
                    bHiDocdate = Convert.ToInt32(ds6.Tables[0].Rows[0][0]);
                    bHNarration = Convert.ToString(ds6.Tables[0].Rows[0][1]);
                    bHPlanMonth = Convert.ToString(ds6.Tables[0].Rows[0][2]);
                    bHPPCMonth = Convert.ToInt32(ds6.Tables[0].Rows[0][3]);
                    bHPPCWeekEndDate = Convert.ToInt32(ds6.Tables[0].Rows[0][7]);
                    bHPPCWeekStartDate = Convert.ToInt32(ds6.Tables[0].Rows[0][8]);
                    bHPPCWeekDays = Convert.ToInt32(ds6.Tables[0].Rows[0][9]);
                    bHPPCDueDate = Convert.ToInt32(ds6.Tables[0].Rows[0][10]);

                }
                @iProductId = 0;
                @pqty = 0;
                for (int i = 0; i <= ds6.Tables[0].Rows.Count - 1; i++)
                {
                    @iProductId = Convert.ToInt32(ds6.Tables[0].Rows[i][4]);
                    @pqty = Convert.ToDecimal(ds6.Tables[0].Rows[i][5]);
                    strQry = $@"exec spICS_CalBomSFGRMReq " + @pqty + "," + @iProductId + ",'" + bHPlanMonth + "',1, " + @iProductId + ",'" + sdocNo + "','" + sUser + "'";
                    DataSet ds = DataAcesslayer.GetData(strQry, sCompanyId, ref strErrorMessage);

                }
                strQry = $@" select * from TableCollectSFG_PPCPlanRMReq where vno='" + sdocNo + "' and loggeduser='" + sUser + "' Order By id ";
                DataSet ds1 = DataAcesslayer.GetData(strQry, sCompanyId, ref strErrorMessage);

                clsGeneric.writeLog("Rows Count :" + ds1.Tables[0].Rows.Count);
                if (strErrorMessage != "")
                {
                    clsGeneric.writeLog("strErrorMessage :" + strErrorMessage);
                }
                if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                {
                    id = 0;
                    for (int j = 0; j <= ds1.Tables[0].Rows.Count - 1; j++)
                    {
                        id = Convert.ToInt32(ds1.Tables[0].Rows[j][0]);
                        fgid = Convert.ToInt32(ds1.Tables[0].Rows[j][2]);

                        fgqty = Convert.ToDecimal(ds1.Tables[0].Rows[j][5]);

                        strQry = $@"select FGID from TableCollectSFG_PPCPlanRMReq where id=" + id;
                        pfgid = Convert.ToInt32(clsGeneric.ShowRecord(sCompanyId, strQry));
                        strQry = $@" insert into RMRequisition_PPCPlanRMReq (refid,Product,RMqty,fQty,planQty,SalQty,BRMReq,branch,worksCenter,ParentId,MParentID,warehouse,Preq,iBodyId, vno, loggeduser) " +
                                    " Select " + id + ",BB.iProductId, bb.fQty," + fgqty + "," + fgqty + ",bb.fQty * " + fgqty + ",bb.fQty * " + fgqty + " * fqty, 1,2," + fgid + "," + pfgid + ",bb.iInvTagValue,0,bb.iBomBodyId,'" + sdocNo + "','" + sUser + "'" +
                                    " FROM dbo.mMRP_BomVariantHeader AS BVH INNER JOIN " +
                                    " dbo.mMRP_BomHeader AS BH ON BH.iBomId = BVH.iBomId INNER JOIN " +
                                    " dbo.mMRP_BOMBody AS BB ON BB.iVariantId = BVH.iVariantId " +
                                    " WHERE(bb.iVariantId IN(SELECT top 1 iVariantId  FROM dbo.ICS_BOMVariant AS VB WHERE(iProductId = " + fgid + ")Order by Iversion desc)) " +
                                    " and(iProductId NOT IN(SELECT iProductId FROM  dbo.ICS_BOMVariant AS Vc)) AND " +
                                    " (bMainOutPut = 0) AND(bInput = 1) Order by Iversion desc ";
                        clsGeneric.writeLog("Query :" + strQry);
                        DataAcesslayer.GetExecute(strQry, sCompanyId, ref strErrorMessage);
                        if (strErrorMessage != "")
                        {
                            clsGeneric.writeLog("strErrorMessage :" + strErrorMessage);
                        }

                    }

                    Hashtable headerPPCRMReq = new Hashtable();
                    headerPPCRMReq.Add("DocNo", sdocNo);
                    headerPPCRMReq.Add("PPCPlanNo", sdocNo);
                    headerPPCRMReq.Add("PPCPlanDate", bHiDocdate);
                    headerPPCRMReq.Add("Date", bHiDocdate);
                    headerPPCRMReq.Add("sNarration", bHNarration);
                    headerPPCRMReq.Add("PlanMonth", bHPlanMonth);
                    headerPPCRMReq.Add("PPCMonth", bHPPCMonth);
                    headerPPCRMReq.Add("WeekStartDate", bHPPCWeekStartDate);
                    headerPPCRMReq.Add("WeekEndDate", bHPPCWeekEndDate);
                    headerPPCRMReq.Add("WeekDays", bHPPCWeekDays);
                    List<System.Collections.Hashtable> lstBody = new List<System.Collections.Hashtable>();
                    id = 0;
                    for (int k = 0; k <= ds1.Tables[0].Rows.Count - 1; k++)
                    {
                        id = Convert.ToInt32(ds1.Tables[0].Rows[k][0]);
                        strQry = $@" select * from RMRequisition_PPCPlanRMReq where refid=" + id + " and vno='" + sdocNo + "' and loggeduser='" + sUser + "' Order By id ";

                        DataSet ds2 = DataAcesslayer.GetData(strQry, sCompanyId, ref strErrorMessage);

                        clsGeneric.writeLog("Rows Count :" + ds2.Tables[0].Rows.Count);
                        if (strErrorMessage != "")
                        {
                            clsGeneric.writeLog("strErrorMessage :" + strErrorMessage);
                        }
                        if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                        {
                            for (int j1 = 0; j1 <= ds2.Tables[0].Rows.Count - 1; j1++)
                            {
                                Hashtable bodyPPCRMReq = new Hashtable();
                                bodyPPCRMReq.Add("Item__Id", ds2.Tables[0].Rows[j1][6]);
                                bodyPPCRMReq.Add("Branch__Id", Convert.ToInt32(clsGeneric.ShowRecord(sCompanyId, "Select branch from muCore_Product_Settings where iMasterId=" + Convert.ToInt32(ds1.Tables[0].Rows[k][2]))));
                                //bodyPPCRMReq.Add("Quantity", ds2.Tables[0].Rows[j1][9]);
                                //Change By Rizwan raised by Majid Sir 12-07-2022
                                bodyPPCRMReq.Add("Quantity", ds2.Tables[0].Rows[j1][12]);
                                bodyPPCRMReq.Add("ParentFGCode__Id", Convert.ToInt32(ds1.Tables[0].Rows[k][2]));
                                bodyPPCRMReq.Add("FGCode__Id", ds1.Tables[0].Rows[k][13]);
                                Hashtable bodyPPCPlanQty = new Hashtable
                                        {
                                            {"Input", ds2.Tables[0].Rows[j1][9]},
                                            {"FieldName",  "PPC Plan Qty" },
                                            {"ColMap", 0},
                                            {"Value",  ds2.Tables[0].Rows[j1][9]}
                                        };

                                bodyPPCRMReq.Add("PPC Plan Qty", bodyPPCPlanQty);
                                lstBody.Add(bodyPPCRMReq);
                            }
                        }


                    }
                    System.Collections.Hashtable objHash = new System.Collections.Hashtable();
                    objHash.Add("Body", lstBody);
                    objHash.Add("Header", headerPPCRMReq);
                    List<System.Collections.Hashtable> lstHash = new List<System.Collections.Hashtable>();
                    lstHash.Add(objHash);
                    HashData objHashRequest = new HashData();
                    objHashRequest.data = lstHash;
                    string sContentPPCRMReq = JsonConvert.SerializeObject(objHashRequest);
                    clsGeneric.writeLog("Content " + vPName1 + " : " + sContentPPCRMReq);
                    clsGeneric.writeLog("Upload URL: " + "http://localhost/Focus8API/Transactions/Vouchers/" + vPName1);

                    using (var clientPPCRMReq = new WebClient())
                    {
                        clientPPCRMReq.Encoding = Encoding.UTF8;
                        clientPPCRMReq.Headers.Add("fSessionId", sSessionId);
                        clientPPCRMReq.Headers.Add("Content-Type", "application/json");
                        var responsePPCRMReq = clientPPCRMReq.UploadString("http://localhost/Focus8API/Transactions/Vouchers/" + vPName1, sContentPPCRMReq);
                        clsGeneric.writeLog("response PPCRMReq: " + responsePPCRMReq);
                        if (responsePPCRMReq != null)
                        {
                            var responseDataPPCRMReq = JsonConvert.DeserializeObject<APIResponse.PostResponse>(responsePPCRMReq);
                            if (responseDataPPCRMReq.result == -1)
                            {
                                blnupdate = false;
                            }
                            else
                            {
                                blnupdate = true;
                            }
                        }

                    }

                    if (blnupdate)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                    //return true;

                }
                else
                {
                    return false;
                }


            }
            catch (Exception ex)
            {
                clsGeneric.writeLog("Exception occured:" + (ex.Message));
                return false;
                throw;
            }

        }
        

    }
}