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

    public class InwQCClearController : Controller
    {
        #region Variable Definition


        string strQry = string.Empty;
        string strErrorMessage = string.Empty;


        BL_DB DataAcesslayer = new BL_DB();

        public int PHQCResult = 0;
        public int PHTransactionId = 0;
        decimal pHRejQty = 0;
        string pHQCRemarks = "";
        int pHCheckedBy = 0;
        int pHCheckedDate = 0;
        decimal pHGRNQuantity = 0;
        decimal pHAcceptedQty = 0;
        int pHAWarehouse = 0;
        int pHRWarehouse = 0;
        decimal pHProdnQuantity = 0;
        decimal pHReworkQty = 0;
        int pHReworkWarehouse = 0;
        string pHProdnDoc = "";
        int pHProdnDate = 0;
        string pHGRNNo = "";
        int pHGRNDate = 0;
        int pHiCode = 0;
        // public static int passed=0;
        // public static int failed;
        public static int QCPConsume_Count = 0;
        public static int QCPConsume_Passed = 0;
        public static int QCPConsume_Failed = 0;
        public static int QCPProduce_Count = 0;
        public static int QCPProduce_Passed = 0;
        public static int QCPProduce_Failed = 0;

        public static int QCInwConsume_Count = 0;
        public static int QCInwConsume_Passed = 0;
        public static int QCInwConsume_Failed = 0;

        public static int QCInwProduce_Count = 0;
        public static int QCInwProduce_Passed = 0;
        public static int QCInwProduce_Failed = 0;

        public static int QCGRNTransfer_Count = 0;
        public static int QCGRNTransfer_Passed = 0;
        public static int QCGRNTransfer_Failed = 0;

        int pHiBookNo = 0;
        int pHiInvTag = 0;
        int pHiFaTag = 0;
        int pHItem = 0;
        int pHPartyName = 0;
        int Cnt = 0;
        int pHPartyAC__Id = 0;
        int pHStockAC__Id = 0;
        int phiDate = 0;
        int pHiBatchid = 0;
        string pHsBatchNo = "";
        int pHiMfDate = 0;
        decimal pHfRate;
        static int PostingStatus;

        //static Boolean logenabled = false;

        public int vPvtype = 1281;
        string vAbbr = "";
        string sqlstring = "";
        StringBuilder msg = new StringBuilder();
        List<string> strDetailIDList = new List<string>();
        #endregion

   



        //Function Add By Shaikh @ 14-01-2023
        [HttpPost]
        public ActionResult UpdateInQC(int CompanyId, string SessionId, string User, int LoginId, int vtype, string docNo, List<PEBody> BodyData)
        {
            clsGeneric.Log_write(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), SessionId, CompanyId, docNo, vtype, User);
            QCInwConsume_Count = 0;
            QCInwConsume_Passed = 0;
            QCInwConsume_Failed = 0;
            strQry = $@"SELECT CHD.PostingStatus FROM tCore_HeaderData" + vtype + "_0 AS CHD INNER JOIN tCore_Header_0 AS CH ON CHD.iHeaderId = CH.iHeaderId " +
                     " WHERE (CH.iVoucherType = " + vtype + ") AND (CH.sVoucherNo = N'" + docNo + "')";

            PostingStatus = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, strQry));
            //0,Pending,1,Updated
            clsGeneric.writeLog("PostingStatus 0,Pending,1,Updated  : " + PostingStatus);
            if (PostingStatus == 0)
            {

                try
                {

                    strQry = $@"SELECT DISTINCT CHD.RejectedQty, CHD.QCResult, isnull(CHD.TransId,0), CHD.QCRemarks, CHD.CheckedBy, CHD.CheckedDate, " +
                    " CHD.GRNQuantity, CHD.AcceptedQty, CHD.AWarehouse, CHD.RWarehouse, CHD.GRNNo, CHD.GRNDate, CD.iCode, CD.iBookNo, CD.iInvTag, CD.iFaTag, CHD.QCItem, CHD.PartyName,CHD.AcctPost1, CHD.AcctPost2,isNull(CHD.QCDocType,0) as QCDocType,CH.iDate " +
                    " FROM tCore_HeaderData" + vtype + "_0 AS CHD INNER JOIN tCore_Header_0 AS CH ON CHD.iHeaderId = CH.iHeaderId INNER JOIN " +
                    " tCore_Data_0 AS CD ON CH.iHeaderId = CD.iHeaderId WHERE (CH.iVoucherType =" + vtype + ") AND (CH.sVoucherNo = N'" + docNo + "')";
                    DataSet ds6 = DataAcesslayer.GetData(strQry, CompanyId, ref strErrorMessage);
                    QCInwConsume_Count = ds6.Tables[0].Rows.Count;
                    QCInwProduce_Count = ds6.Tables[0].Rows.Count;
                    if (ds6 != null && ds6.Tables.Count > 0 && ds6.Tables[0].Rows.Count > 0)
                    {

                        pHRejQty = Convert.ToDecimal(ds6.Tables[0].Rows[0][0]);
                        PHQCResult = Convert.ToInt32(ds6.Tables[0].Rows[0][1]);
                        PHTransactionId = Convert.ToInt32(ds6.Tables[0].Rows[0][2]);
                        pHQCRemarks = Convert.ToString(ds6.Tables[0].Rows[0][3]);
                        pHCheckedBy = Convert.ToInt32(ds6.Tables[0].Rows[0][4]);
                        pHCheckedDate = Convert.ToInt32(ds6.Tables[0].Rows[0][5]);
                        pHGRNQuantity = Convert.ToDecimal(ds6.Tables[0].Rows[0][6]);
                        pHAcceptedQty = Convert.ToDecimal(ds6.Tables[0].Rows[0][7]);
                        pHAWarehouse = Convert.ToInt32(ds6.Tables[0].Rows[0][8]);
                        pHRWarehouse = Convert.ToInt32(ds6.Tables[0].Rows[0][9]);
                        pHGRNNo = Convert.ToString(ds6.Tables[0].Rows[0][10]);
                        pHGRNDate = Convert.ToInt32(ds6.Tables[0].Rows[0][11]);
                        pHiCode = Convert.ToInt32(ds6.Tables[0].Rows[0][12]);
                        pHiBookNo = Convert.ToInt32(ds6.Tables[0].Rows[0][13]);
                        pHiInvTag = Convert.ToInt32(ds6.Tables[0].Rows[0][14]);
                        pHiFaTag = Convert.ToInt32(ds6.Tables[0].Rows[0][15]);
                        pHItem = Convert.ToInt32(ds6.Tables[0].Rows[0][16]);
                        pHPartyName = Convert.ToInt32(ds6.Tables[0].Rows[0][17]);
                        pHStockAC__Id = Convert.ToInt32(ds6.Tables[0].Rows[0][18]);
                        pHPartyAC__Id = Convert.ToInt32(ds6.Tables[0].Rows[0][19]);
                        phiDate = Convert.ToInt32(ds6.Tables[0].Rows[0][21]);
                        // update by rizwan 26-12-2021
                        // If QC Doctype= GRNJW then vPvType=1284 GRNJWPur 
                        if (Convert.ToInt32(ds6.Tables[0].Rows[0][20]) == 1)
                        {
                            vPvtype = 1284;
                            strQry = $@"Update tCore_IndtaBodyScreenData_0 set mInput3=" + pHRejQty + ", mVal3=" + pHRejQty + " FROM tCore_IndtaBodyScreenData_0 AS a " +
                           " WITH (ReadUnCommitted) INNER JOIN dbo.tCore_Data_0 AS b ON a.iBodyId = b.iBodyId INNER JOIN " +
                           " dbo.tCore_Header_0 AS c ON b.iHeaderId = c.iHeaderId " +
                                 " WHERE (a.iBodyId = " + PHTransactionId + ") ";
                            DataAcesslayer.GetExecute(strQry, CompanyId, ref strErrorMessage);
                        }
                        if (Convert.ToInt32(ds6.Tables[0].Rows[0][20]) == 0)
                        {
                            strQry = $@"Update tCore_IndtaBodyScreenData_0 set mInput4=" + pHRejQty + ", mVal4=" + pHRejQty + " FROM tCore_IndtaBodyScreenData_0 AS a " +
                           " WITH (ReadUnCommitted) INNER JOIN dbo.tCore_Data_0 AS b ON a.iBodyId = b.iBodyId INNER JOIN " +
                           " dbo.tCore_Header_0 AS c ON b.iHeaderId = c.iHeaderId " +
                                 " WHERE (a.iBodyId = " + PHTransactionId + ") ";
                            DataAcesslayer.GetExecute(strQry, CompanyId, ref strErrorMessage);
                        }
                        // If QC Doctype= GRN Imp then vPvType=1288 GRNImp 
                        if (Convert.ToInt32(ds6.Tables[0].Rows[0][20]) == 2)
                        {
                            vPvtype = 1288;
                            strQry = $@"Update tCore_IndtaBodyScreenData_0 set mInput4=" + pHRejQty + ", mVal4=" + pHRejQty + " FROM tCore_IndtaBodyScreenData_0 AS a " +
                           " WITH (ReadUnCommitted) INNER JOIN dbo.tCore_Data_0 AS b ON a.iBodyId = b.iBodyId INNER JOIN " +
                           " dbo.tCore_Header_0 AS c ON b.iHeaderId = c.iHeaderId " +
                                 " WHERE (a.iBodyId = " + PHTransactionId + ") ";
                            DataAcesslayer.GetExecute(strQry, CompanyId, ref strErrorMessage);
                        }

                        strQry = $@"Update tCore_Data" + vPvtype + "_0 set QCStatus = 2,QCRemarks='" + pHQCRemarks + "' FROM dbo.tCore_Data_0 AS CD " +
                            " INNER JOIN tCore_Header_0 AS CH ON CD.iHeaderId = CH.iHeaderId " +
                            " INNER JOIN tCore_Data" + vPvtype + "_0 AS CDN ON CD.iBodyId = CDN.iBodyId " +
                            "WHERE  (CD.iTransactionId = " + PHTransactionId + ") AND (CDN.iBodyId =" + PHTransactionId + ")";
                        DataAcesslayer.GetExecute(strQry, CompanyId, ref strErrorMessage);

                        //---------------------- QCGRN 
                        HashData objHashRequest_QCGRN = new HashData();
                        Hashtable headerQCGRN = new Hashtable();
                        headerQCGRN.Add("DocNo", null);
                        headerQCGRN.Add("Date", pHCheckedDate);
                        headerQCGRN.Add("PartyAC__Id", pHPartyAC__Id); //8 
                        //headerQCGRN.Add("Warehouse__Id", pHiInvTag);
                        headerQCGRN.Add("StockAC__Id", pHStockAC__Id); //583
                        headerQCGRN.Add("Branch__Id", pHiFaTag);
                        headerQCGRN.Add("GRNNo", pHGRNNo);
                        headerQCGRN.Add("GRNDate", pHGRNDate);
                        headerQCGRN.Add("GRNQuantity", pHGRNQuantity);
                        headerQCGRN.Add("PartyName__Id", pHPartyName);
                        headerQCGRN.Add("CheckedBy__Id", pHCheckedBy);
                        headerQCGRN.Add("CheckedDate", pHCheckedDate);
                        headerQCGRN.Add("QCRemarks", pHQCRemarks);
                        headerQCGRN.Add("RejectedQty", pHRejQty);
                        //Add New Field Sugestted by Aamer Sir 03-03-2022
                        headerQCGRN.Add("RefDoc", docNo);

                        strQry = $@"SELECT sBatchNo, SUM(IND.fQuantityInBase)[fQuantity]," +
                            " CASE WHEN SUM(IND.fQuantityInBase) > 0 THEN SUM(CASE WHEN IND.fQuantityInBase >= 0 THEN IND.mStockValue ELSE - IND.mStockValue END) / SUM(IND.fQuantityInBase) ELSE 0 END[mRate], " +
                            "SUM(CASE WHEN IND.fQuantityInBase >= 0 THEN IND.mStockValue ELSE -IND.mStockValue END)[StockValue], " +
                            " MAX(bat.iExpiryDate)[iExpiryDate], MAX(bat.iMfDate)[iMfDate], SUM(IND.fAlternateQty)[fAlternateQty] " +
                            "FROM tCore_Header_0 " +
                            " JOIN tCore_Data_0 as tcd ON tCore_Header_0.iHeaderId = tcd.iHeaderId " +
                            " JOIN tCore_Indta_0 as IND ON tcd.iBodyId = IND.iBodyId JOIN tCore_Batch_0 as bat ON tcd.iBodyId = bat.iBodyId " +
                            " JOIN cCore_Vouchers_0 as CV WITH (READUNCOMMITTED) ON CV.iVoucherType = tCore_Header_0.iVoucherType " +
                            " JOIN vrCore_Product ON vrCore_Product.iMasterId = IND.iProduct AND vrCore_Product.iTreeId = 0 " +
                            " and tcd.iBodyId=" + PHTransactionId + " WHERE LEN(bat.sBatchNo) > 0 AND tCore_Header_0.bUpdateStocks = 1 AND tCore_Header_0.bSuspended = 0 AND bSuspendUpdateStocks = 0 AND tCore_Header_0.iDate BETWEEN 0 AND " + phiDate + "  GROUP BY IND.iProduct,bat.sBatchNo HAVING SUM(IND.fQuantityInBase) <> 0 ORDER BY len(sBatchNo),sBatchNo ";
                        DataSet ds0 = DataAcesslayer.GetData(strQry, CompanyId, ref strErrorMessage);
                        if (ds0 != null && ds0.Tables.Count > 0 && ds0.Tables[0].Rows.Count > 0)
                        {
                            //pHiBatchid = Convert.ToInt32(ds0.Tables[0].Rows[0][0]);
                            pHsBatchNo = Convert.ToString(ds0.Tables[0].Rows[0][0]);
                            pHiMfDate = Convert.ToInt32(ds0.Tables[0].Rows[0][5]);
                            pHfRate = Convert.ToDecimal(ds0.Tables[0].Rows[0][2]);
                        }

                        List<System.Collections.Hashtable> lstBody_QCGRN = new List<System.Collections.Hashtable>();

                        Hashtable bodyQCGRN = new Hashtable();
                        List<System.Collections.Hashtable> lstBatch = new List<System.Collections.Hashtable>();
                        bodyQCGRN.Add("Warehouse__Id", pHiInvTag);
                        bodyQCGRN.Add("Item__Id", pHItem);
                        bodyQCGRN.Add("Quantity", pHGRNQuantity);
                        Hashtable bodyBatchQCGRN = new Hashtable
                        {
                            //{"BatchId" , pHiBatchid},
                            {"BatchNo", pHsBatchNo},
                            {"BatchRate",pHfRate},
                            {"MfgDate__Id", pHiMfDate }
                            //{"InvTagId", pHiInvTag }
                            //{"InvTagId", pHAWarehouse }
                        };
                        Hashtable bodyStkQCGRN = new Hashtable
                        {

                            //{"Input", pHsBatchNo},
                            // {"Input", pHAcceptedQty * pHfRate},
                            {"FieldName", "Stk Value"},
                            {"FieldId", 2680},
                            {"ColMap", 0}

                        };
                        bodyQCGRN.Add("Stk Value", bodyStkQCGRN);
                        bodyQCGRN.Add("Batch", bodyBatchQCGRN);
                        bodyQCGRN.Add("Rate", pHfRate);
                        lstBody_QCGRN.Add(bodyQCGRN);



                        System.Collections.Hashtable objHash_QCGRN = new System.Collections.Hashtable();
                        objHash_QCGRN.Add("Body", lstBody_QCGRN);
                        objHash_QCGRN.Add("Header", headerQCGRN);

                        List<System.Collections.Hashtable> lstHash_QCGRN = new List<System.Collections.Hashtable>();
                        lstHash_QCGRN.Add(objHash_QCGRN);
                        objHashRequest_QCGRN.data = lstHash_QCGRN;
                        string sContent_QCGRN = JsonConvert.SerializeObject(objHashRequest_QCGRN);
                        clsGeneric.writeLog("Upload Inward QC Consumption :" + "http://localhost/Focus8API/Transactions/Vouchers/Inward QC Consumption");
                        clsGeneric.writeLog("URL Param :" + sContent_QCGRN);
                        using (var clientQCGRN = new WebClient())
                        {
                            clientQCGRN.Encoding = Encoding.UTF8;
                            clientQCGRN.Headers.Add("fSessionId", SessionId);
                            clientQCGRN.Headers.Add("Content-Type", "application/json");

                            var responseQCGRN = clientQCGRN.UploadString("http://localhost/Focus8API/Transactions/Vouchers/Inward QC Consumption", sContent_QCGRN);
                            clsGeneric.writeLog("response Inward QC Consumption :" + responseQCGRN);

                            //clsGeneric.writeLog("Response form QCGRN :" + (responseQCGRN));
                            if (responseQCGRN != null)
                            {
                                var responseDataQCGRN = JsonConvert.DeserializeObject<APIResponse.PostResponse>(responseQCGRN);
                                if (responseDataQCGRN.result == -1)
                                {
                                    // Commented Code required to Remove After Successful testing 05-05-2022
                                    //clsGeneric.createTableRMReqQtySAL(CompanyId, User, docNo);
                                    //clsGeneric.createTablelinkinfoSAL(CompanyId, User, docNo);
                                    //clsGeneric.createTablelinkrmusedSAL(CompanyId, User, docNo);
                                    //clsGeneric.Update_AnnexureStatusSAL(CompanyId, vtype, iHeaderId, 1);
                                    //Update_PostingStatus(CompanyId, vtype, docNo, 0);
                                    //return Json(new { status = false, data = new { message = responseDataQCGRN.message } });
                                    QCInwConsume_Failed++;
                                }
                                else
                                {
                                    var iMasterId = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(responseDataQCGRN.data[0]["VoucherNo"]));
                                    Update_stk(CompanyId, iMasterId, 5377);
                                    // Commented Code required to Remove After Successful testing 05-05-2022
                                    //clsGeneric.createTableRMReqQtySAL(CompanyId, User, docNo);
                                    //clsGeneric.createTablelinkinfoSAL(CompanyId, User, docNo);
                                    //clsGeneric.createTablelinkrmusedSAL(CompanyId, User, docNo);
                                    //updateRMStkValue(docNo, vtype, CompanyId);
                                    //clsGeneric.Update_AnnexureStatusSAL(CompanyId, vtype, iHeaderId, 2);
                                    //Update_PostingStatus(CompanyId, vtype, docNo, 1);
                                    //return Json(new { status = true, data = new { message = "Posting Successful" } });
                                    QCInwConsume_Passed++;
                                }

                            }

                        }

                        // --------------------End of QCGRN



                        //return Json(new { status = false, data = new { message = "Posting Successful " } });

                    }



                    // Code goes to for produce 
                    if (QCInwConsume_Passed != 0)
                    {

                        //---------------------- InwdPro 
                        HashData objHashRequest_QCInwdPro = new HashData();
                        Hashtable headerQCInwdPro = new Hashtable();
                        headerQCInwdPro.Add("DocNo", null);
                        headerQCInwdPro.Add("Date", pHCheckedDate);
                        headerQCInwdPro.Add("PartyAC__Id", pHPartyAC__Id); //8 
                                                                           //headerQCInwdPro.Add("InvTag__Id", pHiInvTag);
                        headerQCInwdPro.Add("StockAC__Id", pHStockAC__Id); //583
                        headerQCInwdPro.Add("Branch__Id", pHiFaTag);
                        headerQCInwdPro.Add("GRNNo", pHGRNNo);
                        headerQCInwdPro.Add("GRNDate", pHGRNDate);
                        headerQCInwdPro.Add("GRNQuantity", pHGRNQuantity);
                        headerQCInwdPro.Add("PartyName__Id", pHPartyName);
                        headerQCInwdPro.Add("CheckedBy__Id", pHCheckedBy);
                        headerQCInwdPro.Add("CheckedDate", pHCheckedDate);
                        headerQCInwdPro.Add("QCRemarks", pHQCRemarks);
                        headerQCInwdPro.Add("RejectedQty", pHRejQty);
                        //Add New Field Sugestted by Aamer Sir 03-03-2022
                        headerQCInwdPro.Add("RefDoc", docNo);
                        // Commented Code required to Remove After Successful testing 05-05-2022
                        //strQry = $@"Select iBatchid,sBatchNo,iMfDate,fRate from tCore_Batch_0 where (iBodyId=" + PHTransactionId + ")";
                        //DataSet ds0 = DataAcesslayer.GetData(strQry, CompanyId, ref strErrorMessage);
                        //if (ds0 != null && ds0.Tables.Count > 0 && ds0.Tables[0].Rows.Count > 0)
                        //{
                        //    pHiBatchid = Convert.ToInt32(ds0.Tables[0].Rows[0][0]);
                        //    pHsBatchNo = Convert.ToString(ds0.Tables[0].Rows[0][1]);
                        //    pHiMfDate = Convert.ToInt32(ds0.Tables[0].Rows[0][2]);
                        //    pHfRate = Convert.ToInt32(ds0.Tables[0].Rows[0][3]);

                        //}

                        List<System.Collections.Hashtable> lstBody_QCInwdPro = new List<System.Collections.Hashtable>();
                        Cnt = 1;
                        if (pHRejQty > 0)
                        {
                            Cnt++;
                        }

                        for (int k_vs = 1; k_vs <= Cnt; k_vs++)
                        {
                            if (k_vs == 1)
                            {
                                Hashtable bodyQCInwdPro = new Hashtable();
                                List<System.Collections.Hashtable> lstBatch = new List<System.Collections.Hashtable>();
                                bodyQCInwdPro.Add("Warehouse__Id", pHAWarehouse);
                                bodyQCInwdPro.Add("Item__Id", pHItem);
                                bodyQCInwdPro.Add("Quantity", pHAcceptedQty);
                                Hashtable bodyBatchQCInwdPro = new Hashtable
                                {
                                    //{"BatchId" , pHiBatchid},
                                    {"BatchNo", pHsBatchNo},
                                    {"BatchRate",pHfRate},
                                    {"MfgDate__Id", pHiMfDate }
                                    //{"MfgDate", pHiMfDate },
                                    //{"InvTagId", pHiInvTag }
                                    //{"InvTagId", pHAWarehouse }
                                };
                                Hashtable bodyStkQCInwdPro = new Hashtable
                                {

                                    //{"Input", pHsBatchNo},
                                   // {"Input", pHAcceptedQty * pHfRate},
                                    {"FieldName", "Stk Value"},
                                    {"FieldId", 2271},
                                    {"ColMap", 0}

                                };
                                //bodyQCInwdPro.Add("Stk Value", bodyStkQCInwdPro);
                                bodyQCInwdPro.Add("Batch", bodyBatchQCInwdPro);
                                bodyQCInwdPro.Add("Rate", pHfRate);
                                lstBody_QCInwdPro.Add(bodyQCInwdPro);
                            }
                            if (k_vs == 2)
                            {
                                Hashtable bodyQCInwdPro = new Hashtable();
                                bodyQCInwdPro.Add("Warehouse__Id", pHRWarehouse);
                                bodyQCInwdPro.Add("Item__Id", pHItem);
                                bodyQCInwdPro.Add("Quantity", pHRejQty);
                                Hashtable bodyBatchQCInwdPro = new Hashtable
                            {
                                    //{"BatchId" , pHiBatchid},
                                    {"BatchNo", pHsBatchNo},
                                    {"BatchRate",pHfRate},
                                    {"MfgDate__Id", pHiMfDate }
                                    //{"MfgDate", pHiMfDate },
                                    //{"InvTagId", pHiInvTag }
                            };
                                bodyQCInwdPro.Add("Batch", bodyBatchQCInwdPro);
                                bodyQCInwdPro.Add("Rate", pHfRate);
                                lstBody_QCInwdPro.Add(bodyQCInwdPro);
                            }
                        }
                        System.Collections.Hashtable objHash_QCInwdPro = new System.Collections.Hashtable();
                        objHash_QCInwdPro.Add("Body", lstBody_QCInwdPro);
                        objHash_QCInwdPro.Add("Header", headerQCInwdPro);

                        List<System.Collections.Hashtable> lstHash_QCInwdPro = new List<System.Collections.Hashtable>();
                        lstHash_QCInwdPro.Add(objHash_QCInwdPro);
                        objHashRequest_QCInwdPro.data = lstHash_QCInwdPro;
                        string sContent_QCInwdPro = JsonConvert.SerializeObject(objHashRequest_QCInwdPro);
                        clsGeneric.writeLog("Upload QCInwdPro :" + "http://localhost/Focus8API/Transactions/Vouchers/Inward QC Production-Auto");
                        clsGeneric.writeLog("URL Param :" + sContent_QCInwdPro);
                        using (var clientQCInwdPro = new WebClient())
                        {
                            clientQCInwdPro.Encoding = Encoding.UTF8;
                            clientQCInwdPro.Headers.Add("fSessionId", SessionId);
                            clientQCInwdPro.Headers.Add("Content-Type", "application/json");

                            var responseQCInwdPro = clientQCInwdPro.UploadString("http://localhost/Focus8API/Transactions/Vouchers/Inward QC Production-Auto", sContent_QCInwdPro);
                            clsGeneric.writeLog("response QCInwdPro :" + responseQCInwdPro);

                            //clsGeneric.writeLog("Response form QCInwdPro :" + (responseQCInwdPro));
                            if (responseQCInwdPro != null)
                            {
                                var responseDataQCInwdPro = JsonConvert.DeserializeObject<APIResponse.PostResponse>(responseQCInwdPro);
                                if (responseDataQCInwdPro.result == -1)
                                {
                                    // Commented Code required to Remove After Successful testing 05-05-2022
                                    //clsGeneric.createTableRMReqQtySAL(CompanyId, User, docNo);
                                    //clsGeneric.createTablelinkinfoSAL(CompanyId, User, docNo);
                                    //clsGeneric.createTablelinkrmusedSAL(CompanyId, User, docNo);
                                    //clsGeneric.Update_AnnexureStatusSAL(CompanyId, vtype, iHeaderId, 1);
                                    Update_PostingStatus(CompanyId, vtype, docNo, 0);
                                    //return Json(new { status = false, data = new { message = responseDataQCInwdPro.message } });
                                    QCInwProduce_Failed++;
                                }
                                else
                                {
                                    var iMasterId = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(responseDataQCInwdPro.data[0]["VoucherNo"]));
                                    Update_stk(CompanyId, iMasterId, 2049);
                                    // Commented Code required to Remove After Successful testing 05-05-2022
                                    //clsGeneric.createTableRMReqQtySAL(CompanyId, User, docNo);
                                    //clsGeneric.createTablelinkinfoSAL(CompanyId, User, docNo);
                                    //clsGeneric.createTablelinkrmusedSAL(CompanyId, User, docNo);
                                    //updateRMStkValue(docNo, vtype, CompanyId);
                                    //clsGeneric.Update_AnnexureStatusSAL(CompanyId, vtype, iHeaderId, 2);
                                    Update_PostingStatus(CompanyId, vtype, docNo, 1);
                                    //return Json(new { status = true, data = new { message = "Posting Successful" } });
                                    QCInwProduce_Passed++;
                                }

                            }

                        }
                    }
                    // --------------------End of QCInwdPro

                    clsGeneric.writeLog("QCInwConsume_Count" + QCInwConsume_Count);
                    clsGeneric.writeLog("QCInwConsume_Passed" + QCInwConsume_Passed);
                    clsGeneric.writeLog("QCInwConsume_Failed" + QCInwConsume_Failed);

                    clsGeneric.writeLog("QCInwProduce_Count" + QCInwProduce_Count);
                    clsGeneric.writeLog("QCInwProduce_Passed" + QCInwProduce_Passed);
                    clsGeneric.writeLog("QCInwProduce_Failed" + QCInwProduce_Failed);



                    //return Json(new { status = false, data = new { message = "Posting Failed " } });
                    if (QCInwProduce_Passed != 0)
                    {
                        return Json(new { status = false, data = new { message = "Posting Successful " } });
                    }
                    else
                    {
                        return Json(new { status = false, data = new { message = "Posting Failed " } });
                    }



                }
                catch (Exception ex)
                {
                    //clsGeneric.createTableRMReqQty(CompanyId, User, docNo);
                    //clsGeneric.writeLog("delete Query :" + strQry);
                    clsGeneric.writeLog("Exception occured:" + (ex.Message));
                    return Json(new { status = false, data = new { message = "Posting Failed " } });
                    throw;
                }
            }
            else
            {
                return Json(new { status = false, data = new { message = "Document already Posted" } });
            }
        }
        // End of New Function Add By Shaikh 24-01-2022

        

        [HttpPost]
        public ActionResult UpdateGRNRM(int CompanyId, string SessionId, string User, int LoginId, int vtype, string docNo, List<PEBody> BodyData)
        {
            clsGeneric.Log_write(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), SessionId, CompanyId, docNo, vtype, User);
            strQry = $@"SELECT CHD.PostingStatus FROM tCore_HeaderData2054_0 AS CHD INNER JOIN tCore_Header_0 AS CH ON CHD.iHeaderId = CH.iHeaderId " +
                     " WHERE (CH.iVoucherType = " + vtype + ") AND (CH.sVoucherNo = N'" + docNo + "')";
            QCGRNTransfer_Count = 0;
            QCGRNTransfer_Passed = 0;
            QCGRNTransfer_Failed = 0;
            PostingStatus = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, strQry));
            //0,Pending,1,Updated
            clsGeneric.writeLog("PostingStatus 0,Pending,1,Updated  : " + PostingStatus);
            pHItem = 0;
            if (PostingStatus == 0)
            {

                try
                {

                    strQry = $@"SELECT DISTINCT CHD.RejectedQty, CHD.QCResult, CHD.TransId, CHD.QCRemarks, CHD.CheckedBy, CHD.CheckedDate, " +
                    " CHD.GRNQuantity, CHD.AcceptedQty, CHD.AWarehouse, CHD.RWarehouse, CHD.GRNNo, CHD.GRNDate, CD.iCode, CD.iBookNo, CD.iInvTag, CD.iFaTag, CHD.Item, CHD.PartyName,CHD.AcctPost1, CHD.AcctPost2,isNull(CHD.QCDocType,0) as QCDocType " +
                    " FROM tCore_HeaderData2054_0 AS CHD INNER JOIN tCore_Header_0 AS CH ON CHD.iHeaderId = CH.iHeaderId INNER JOIN " +
                    " tCore_Data_0 AS CD ON CH.iHeaderId = CD.iHeaderId WHERE (CH.iVoucherType =" + vtype + ") AND (CH.sVoucherNo = N'" + docNo + "')";
                    clsGeneric.writeLog("Query" + strQry);
                    DataSet ds6 = DataAcesslayer.GetData(strQry, CompanyId, ref strErrorMessage);
                    QCGRNTransfer_Count = ds6.Tables[0].Rows.Count;
                    pHItem = 0;
                    if (ds6 != null && ds6.Tables.Count > 0 && ds6.Tables[0].Rows.Count > 0)
                    {

                        pHRejQty = Convert.ToDecimal(ds6.Tables[0].Rows[0][0]);
                        PHQCResult = Convert.ToInt32(ds6.Tables[0].Rows[0][1]);
                        PHTransactionId = Convert.ToInt32(ds6.Tables[0].Rows[0][2]);
                        pHQCRemarks = Convert.ToString(ds6.Tables[0].Rows[0][3]);
                        pHCheckedBy = Convert.ToInt32(ds6.Tables[0].Rows[0][4]);
                        pHCheckedDate = Convert.ToInt32(ds6.Tables[0].Rows[0][5]);
                        pHGRNQuantity = Convert.ToDecimal(ds6.Tables[0].Rows[0][6]);
                        pHAcceptedQty = Convert.ToDecimal(ds6.Tables[0].Rows[0][7]);
                        pHAWarehouse = Convert.ToInt32(ds6.Tables[0].Rows[0][8]);
                        pHRWarehouse = Convert.ToInt32(ds6.Tables[0].Rows[0][9]);
                        pHGRNNo = Convert.ToString(ds6.Tables[0].Rows[0][10]);
                        pHGRNDate = Convert.ToInt32(ds6.Tables[0].Rows[0][11]);
                        pHiCode = Convert.ToInt32(ds6.Tables[0].Rows[0][12]);
                        pHiBookNo = Convert.ToInt32(ds6.Tables[0].Rows[0][13]);
                        pHiInvTag = Convert.ToInt32(ds6.Tables[0].Rows[0][14]);
                        pHiFaTag = Convert.ToInt32(ds6.Tables[0].Rows[0][15]);
                        pHItem = Convert.ToInt32(ds6.Tables[0].Rows[0][16]);
                        pHPartyName = Convert.ToInt32(ds6.Tables[0].Rows[0][17]);
                        pHStockAC__Id = Convert.ToInt32(ds6.Tables[0].Rows[0][18]);
                        pHPartyAC__Id = Convert.ToInt32(ds6.Tables[0].Rows[0][19]);
                        // update by rizwan 26-12-2021
                        // If QC Doctype= GRNJW then vPvType=1284 GRNJWPur 
                        if (Convert.ToInt32(ds6.Tables[0].Rows[0][20]) == 1)
                        {
                            vPvtype = 1284;
                            strQry = $@"Update tCore_IndtaBodyScreenData_0 set mInput3=" + pHRejQty + ", mVal3=" + pHRejQty + " FROM tCore_IndtaBodyScreenData_0 AS a " +
                           " WITH (ReadUnCommitted) INNER JOIN dbo.tCore_Data_0 AS b ON a.iBodyId = b.iBodyId INNER JOIN " +
                           " dbo.tCore_Header_0 AS c ON b.iHeaderId = c.iHeaderId " +
                                 " WHERE (a.iBodyId = " + PHTransactionId + ") ";
                            DataAcesslayer.GetExecute(strQry, CompanyId, ref strErrorMessage);
                        }
                        else
                        {
                            strQry = $@"Update tCore_IndtaBodyScreenData_0 set mInput4=" + pHRejQty + ", mVal4=" + pHRejQty + " FROM tCore_IndtaBodyScreenData_0 AS a " +
                           " WITH (ReadUnCommitted) INNER JOIN dbo.tCore_Data_0 AS b ON a.iBodyId = b.iBodyId INNER JOIN " +
                           " dbo.tCore_Header_0 AS c ON b.iHeaderId = c.iHeaderId " +
                                 " WHERE (a.iBodyId = " + PHTransactionId + ") ";
                            DataAcesslayer.GetExecute(strQry, CompanyId, ref strErrorMessage);
                        }

                        strQry = $@"Update tCore_Data" + vPvtype + "_0 set QCStatus = 2,QCRemarks='" + pHQCRemarks + "' FROM dbo.tCore_Data_0 AS CD " +
                            " INNER JOIN tCore_Header_0 AS CH ON CD.iHeaderId = CH.iHeaderId " +
                            " INNER JOIN tCore_Data" + vPvtype + "_0 AS CDN ON CD.iBodyId = CDN.iBodyId " +
                            "WHERE  (CD.iTransactionId = " + PHTransactionId + ") AND (CDN.iBodyId =" + PHTransactionId + ")";
                        DataAcesslayer.GetExecute(strQry, CompanyId, ref strErrorMessage);

                        //---------------------- QCGRN 
                        HashData objHashRequest_QCGRN = new HashData();
                        Hashtable headerQCGRN = new Hashtable();
                        headerQCGRN.Add("DocNo", null);
                        headerQCGRN.Add("Date", pHCheckedDate);
                        headerQCGRN.Add("PartyAC__Id", pHPartyAC__Id); //8 
                        headerQCGRN.Add("InvTag__Id", pHiInvTag);
                        headerQCGRN.Add("StockAC__Id", pHStockAC__Id); //583
                        headerQCGRN.Add("Branch__Id", pHiFaTag);
                        headerQCGRN.Add("GRNNo", pHGRNNo);
                        headerQCGRN.Add("GRNDate", pHGRNDate);
                        headerQCGRN.Add("GRNQuantity", pHGRNQuantity);
                        headerQCGRN.Add("PartyName__Id", pHPartyName);
                        headerQCGRN.Add("CheckedBy__Id", pHCheckedBy);
                        headerQCGRN.Add("CheckedDate", pHCheckedDate);
                        headerQCGRN.Add("QCRemarks", pHQCRemarks);
                        headerQCGRN.Add("RejectedQty", pHRejQty);

                        strQry = $@"Select iBatchid,sBatchNo,iMfDate,fRate from tCore_Batch_0 where (iBodyId=" + PHTransactionId + ")";
                        DataSet ds0 = DataAcesslayer.GetData(strQry, CompanyId, ref strErrorMessage);
                        if (ds0 != null && ds0.Tables.Count > 0 && ds0.Tables[0].Rows.Count > 0)
                        {
                            pHiBatchid = Convert.ToInt32(ds0.Tables[0].Rows[0][0]);
                            pHsBatchNo = Convert.ToString(ds0.Tables[0].Rows[0][1]);
                            pHiMfDate = Convert.ToInt32(ds0.Tables[0].Rows[0][2]);
                            pHfRate = Convert.ToInt32(ds0.Tables[0].Rows[0][3]);

                        }

                        List<System.Collections.Hashtable> lstBody_QCGRN = new List<System.Collections.Hashtable>();
                        Cnt = 1;
                        if (pHRejQty > 0)
                        {
                            Cnt++;
                        }

                        for (int k_vs = 1; k_vs <= Cnt; k_vs++)
                        {
                            if (k_vs == 1)
                            {
                                Hashtable bodyQCGRN = new Hashtable();
                                List<System.Collections.Hashtable> lstBatch = new List<System.Collections.Hashtable>();
                                bodyQCGRN.Add("InvTag2__Id", pHAWarehouse);
                                bodyQCGRN.Add("Item__Id", pHItem);
                                bodyQCGRN.Add("Quantity", pHAcceptedQty);
                                Hashtable bodyBatchQCGRN = new Hashtable
                                {
                                    //{"BatchId" , pHiBatchid},
                                    {"BatchNo", pHsBatchNo},
                                    //{"MfgDate", pHiMfDate },
                                    //{"InvTagId", pHiInvTag }
                                    //{"InvTagId", pHAWarehouse }
                                };
                                Hashtable bodyStkQCGRN = new Hashtable
                                {

                                    //{"Input", pHsBatchNo},
                                   // {"Input", pHAcceptedQty * pHfRate},
                                    {"FieldName", "Stk Value"},
                                    {"FieldId", 2271},
                                    {"ColMap", 0}

                                };
                                bodyQCGRN.Add("Stk Value", bodyStkQCGRN);
                                bodyQCGRN.Add("Batch", bodyBatchQCGRN);
                                //bodyQCGRN.Add("Rate", 0);
                                lstBody_QCGRN.Add(bodyQCGRN);
                            }
                            if (k_vs == 2)
                            {
                                Hashtable bodyQCGRN = new Hashtable();
                                bodyQCGRN.Add("InvTag2__Id", pHRWarehouse);
                                bodyQCGRN.Add("Item__Id", pHItem);
                                bodyQCGRN.Add("Quantity", pHRejQty);
                                Hashtable bodyBatchQCGRN = new Hashtable
                            {
                                    //{"BatchId" , pHiBatchid},
                                    {"BatchNo", pHsBatchNo},
                                    //{"MfgDate", pHiMfDate },
                                    //{"InvTagId", pHiInvTag }
                            };
                                bodyQCGRN.Add("Batch", bodyBatchQCGRN);
                                //bodyQCGRN.Add("Rate", 0);
                                lstBody_QCGRN.Add(bodyQCGRN);
                            }
                        }
                        System.Collections.Hashtable objHash_QCGRN = new System.Collections.Hashtable();
                        objHash_QCGRN.Add("Body", lstBody_QCGRN);
                        objHash_QCGRN.Add("Header", headerQCGRN);

                        List<System.Collections.Hashtable> lstHash_QCGRN = new List<System.Collections.Hashtable>();
                        lstHash_QCGRN.Add(objHash_QCGRN);
                        objHashRequest_QCGRN.data = lstHash_QCGRN;
                        string sContent_QCGRN = JsonConvert.SerializeObject(objHashRequest_QCGRN);
                        clsGeneric.writeLog("Upload QCGRN :" + "http://localhost/Focus8API/Transactions/Vouchers/QC GRN Transfer");
                        clsGeneric.writeLog("URL Param :" + sContent_QCGRN);
                        using (var clientQCGRN = new WebClient())
                        {
                            clientQCGRN.Encoding = Encoding.UTF8;
                            clientQCGRN.Headers.Add("fSessionId", SessionId);
                            clientQCGRN.Headers.Add("Content-Type", "application/json");

                            var responseQCGRN = clientQCGRN.UploadString("http://localhost/Focus8API/Transactions/Vouchers/QC GRN Transfer", sContent_QCGRN);
                            clsGeneric.writeLog("response QCGRN :" + responseQCGRN);

                            //clsGeneric.writeLog("Response form QCGRN :" + (responseQCGRN));
                            if (responseQCGRN != null)
                            {
                                var responseDataQCGRN = JsonConvert.DeserializeObject<APIResponse.PostResponse>(responseQCGRN);
                                if (responseDataQCGRN.result == -1)
                                {
                                    //clsGeneric.createTableRMReqQtySAL(CompanyId, User, docNo);
                                    //clsGeneric.createTablelinkinfoSAL(CompanyId, User, docNo);
                                    //clsGeneric.createTablelinkrmusedSAL(CompanyId, User, docNo);
                                    //clsGeneric.Update_AnnexureStatusSAL(CompanyId, vtype, iHeaderId, 1);
                                    Update_PostingStatus(CompanyId, vtype, docNo, 0);
                                    //return Json(new { status = false, data = new { message = responseDataQCGRN.message } });
                                    QCGRNTransfer_Failed++;
                                }
                                else
                                {
                                    var iMasterId = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(responseDataQCGRN.data[0]["VoucherNo"]));
                                    Update_stk(CompanyId, iMasterId, 5377);
                                    //clsGeneric.createTableRMReqQtySAL(CompanyId, User, docNo);
                                    //clsGeneric.createTablelinkinfoSAL(CompanyId, User, docNo);
                                    //clsGeneric.createTablelinkrmusedSAL(CompanyId, User, docNo);
                                    //updateRMStkValue(docNo, vtype, CompanyId);
                                    //clsGeneric.Update_AnnexureStatusSAL(CompanyId, vtype, iHeaderId, 2);
                                    Update_PostingStatus(CompanyId, vtype, docNo, 1);
                                    //return Json(new { status = true, data = new { message = "Posting Successful" } });
                                    QCGRNTransfer_Passed++;
                                }

                            }

                        }

                        // --------------------End of QCGRN



                        //return Json(new { status = false, data = new { message = "Posting Successful " } });

                    }
                    //return Json(new { status = false, data = new { message = "Posting Failed " } });
                    if (QCGRNTransfer_Passed != 0)
                    {
                        return Json(new { status = false, data = new { message = "Posting Successful " } });
                    }
                    else
                    {
                        return Json(new { status = false, data = new { message = "Posting Failed " } });
                    }
                }
                catch (Exception ex)
                {
                    //clsGeneric.createTableRMReqQty(CompanyId, User, docNo);
                    //clsGeneric.writeLog("delete Query :" + strQry);
                    clsGeneric.writeLog("Exception occured:" + (ex.Message));
                    return Json(new { status = false, data = new { message = "Posting Failed " } });
                    throw;
                }
            }
            else
            {
                return Json(new { status = false, data = new { message = "Document already Posted" } });
            }
        }

        public static void Update_PostingStatus(int QcompanyId, int Qvtype, string QdocNo, int QStatus = 0)
        {
            BL_DB objDB = new BL_DB();
            string strQry = "";
            string error = "";
            DataTable dt = new DataTable();
            strQry = $@"update tCore_HeaderData" + Qvtype + "_0 set PostingStatus = " + QStatus +
                " FROM dbo.tCore_HeaderData" + Qvtype + "_0 AS CHD INNER JOIN dbo.tCore_Header_0 AS CH ON CHD.iHeaderId = CH.iHeaderId " +
                " WHERE (CH.iVoucherType = " + Qvtype + ") AND (CH.sVoucherNo = N'" + QdocNo + "')";
            objDB.GetExecute(strQry, QcompanyId, ref error);

            clsGeneric.writeLog("Update Posting Status - tCore_HeaderData" + Qvtype + "_0 Query : " + strQry);

        }
        public static void Update_stk(int QcompanyId, string QdocNo, int transVtype)
        {
            BL_DB objDB = new BL_DB();
            string strQry = "";
            string error = "";
            DataTable dt = new DataTable();
            strQry = $@"update tCore_IndtaBodyScreenData_0 set mInput0 = mGross, mVal0 =mGross FROM tCore_Header_0 AS CH " +
                " INNER JOIN tCore_Data_0 AS CD ON CH.iHeaderId = CD.iHeaderId INNER JOIN tCore_HeaderData" + transVtype + "_0 AS CHD ON  " +
                " CH.iHeaderId = CHD.iHeaderId INNER JOIN tCore_IndtaBodyScreenData_0 AS CIS ON CD.iBodyId = CIS.iBodyId INNER JOIN " +
                " tCore_Indta_0 AS CI ON CD.iBodyId = CI.iBodyId WHERE (CH.sVoucherNo = N'" + QdocNo + "')";
            objDB.GetExecute(strQry, QcompanyId, ref error);

            clsGeneric.writeLog("Update Posting Status - tCore_HeaderData" + QdocNo + "_0 Query : " + strQry);

        }
        public ActionResult BS_PopulteGRNNO(int CompanyId, string SessionId, string User, int LoginId, int bvtype, string bdocNo, int idocdate, int Dept, int Vendor, int Warehouse, int QCItem)
        {
            BL_DB DataAcesslayer = new BL_DB();
            msg.AppendLine(CompanyId.ToString());
            msg.AppendLine(SessionId.ToString());
            clsGeneric.createICSPendingGRNRMQC(CompanyId, User, bdocNo);
            sqlstring = "insert into ICSPendingGRNRMQC SELECT tcd.iBodyId,tci.fQuantity,ccv.sAbbr + ':' + tch.sVoucherNo as sabbrVNO ,tch.sVoucherNo, tch.iDate, tcbn.sBatchNo, tcbn.iBatchId, tcbn.iMfDate,'" + bdocNo + "' ,'" + User +
                "' FROM dbo.cCore_Vouchers_0 AS ccv INNER JOIN dbo.tCore_Header_0 AS tch ON ccv.iVoucherType = tch.iVoucherType INNER JOIN  " +
                " dbo.tCore_Data_0 AS tcd ON tch.iHeaderId = tcd.iHeaderId INNER JOIN dbo.tCore_Indta_0 AS tci ON tcd.iBodyId = tci.iBodyId INNER JOIN " +
                " dbo.tCore_Batch_0 AS tcbn ON tcd.iBodyId = tcbn.iBodyId WHERE (tch.bCancelled = 0) AND (tch.bSuspended = 0) AND (tcd.iAuthStatus = 1) " +
                " AND (tch.iVoucherType = 1281) AND (tcd.iBookNo = " + Vendor + ") AND (tcd.iInvTag = " + Warehouse + ") AND (tcd.iFaTag = " + Dept + ") AND (tci.iProduct = " + QCItem + ")";

            DataAcesslayer.GetExecute(sqlstring, CompanyId, ref strErrorMessage);

            return Json(new { status = true, data = new { succes = msg.ToString() } });
        }

        public ActionResult BS_PopulteOtherFlds(int CompanyId, string SessionId, string User, int LoginId, int bvtype, string bdocNo, int idocdate, int Dept, int Vendor, int Warehouse, int QCItem, int iBodyId)
        {
            BL_DB DataAcesslayer = new BL_DB();
            sqlstring = "SELECT tci.fQuantity,tch.sVoucherNo,  format(dbo.IntToDate(tch.iDate),'dd/MM/yyyy') iDate, tcbn.sBatchNo, tcbn.iBatchId, format(dbo.IntToDate(tcbn.iMfDate),'dd/MM/yyyy')  iMfDate " +
                " FROM dbo.cCore_Vouchers_0 AS ccv INNER JOIN dbo.tCore_Header_0 AS tch ON ccv.iVoucherType = tch.iVoucherType INNER JOIN  " +
                " dbo.tCore_Data_0 AS tcd ON tch.iHeaderId = tcd.iHeaderId INNER JOIN dbo.tCore_Indta_0 AS tci ON tcd.iBodyId = tci.iBodyId INNER JOIN " +
                " dbo.tCore_Batch_0 AS tcbn ON tcd.iBodyId = tcbn.iBodyId WHERE (tch.bCancelled = 0) AND (tch.bSuspended = 0) AND (tcd.iAuthStatus = 1) " +
                " AND (tch.iVoucherType = 1281) AND (tcd.iBookNo = " + Vendor + ") AND (tcd.iInvTag = " + Warehouse + ") AND (tcd.iFaTag = " + Dept + ") AND (tci.iProduct = " + QCItem + ") and (tcd.iBodyId=" + iBodyId + ")";

            DataSet ds2 = DataAcesslayer.GetData(sqlstring, CompanyId, ref strErrorMessage);
            //string JSONString = string.Empty;
            //JSONString = JsonConvert.SerializeObject(ds2);
            //strdetailID[1]] = ds2.Tables[0].Rows[1]["fQuantity"].ToString();
            string[] strDetailID = new string[ds2.Tables[0].Columns.Count];
            int arryIndex = 0;
            foreach (DataColumn col in ds2.Tables[0].Columns)
            {

                foreach (DataRow row in ds2.Tables[0].Rows)
                {
                    strDetailID[arryIndex] = row[col.ColumnName].ToString();
                    //strDetailID[arryIndex] = row["Ad_detailsID"].ToString());
                }
                arryIndex++;

            }




            return Json(new { status = true, data = new { succes = strDetailID } });
            //return Json(new { status = true, data = new { succes = msg.ToString() } });
        }


    }
}