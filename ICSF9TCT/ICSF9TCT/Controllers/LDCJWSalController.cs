using ICSF9TCT.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;

using System.Web.Mvc;


using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace ICSF9TCT.Controllers
{
    public class LDCJWSalController : Controller
    {
        #region Variable Definition
        StringBuilder msg = new StringBuilder();
        string strValue = "";

        string strQry = string.Empty;
        string strErrorMessage = string.Empty;

        public static DateTime dDocDate;
        public static decimal dRate;
        public static decimal dGross;
        // Header 
        int Warehouse = 0;
        public static int iDocDate;
        public static int iInvTag;
        public static int iBookNo;
        public static int iFgCode;
        static string GateEntryNo;
        static int GateEntryDate;
        static string PONo;
        static int PODate;
        //static string LRNo;
        //static int LRDate;
        static int iCode;
        static int TransactionId;
        static int liid;


        static decimal fgqty;
        public static int fgid;
        static decimal GRNJW_FGQty;
        static int vWarehoseID;
        static decimal DC_RM_Bal_Qty;
        static decimal Consumed_RM_Qty;
        static decimal DC_RM_Qty;
        static decimal DC_ConsumedQty;
        public static int VendAnnx_vType = 0;
        public static string VendAnnx_Abbr = "";
        static int DC_vType;
        static string DC_Docno;
        static string DC_Abbr;
        static int DC_TransactionId;
        static string FgName = "";
        static string FgCode = "";
        public static int rmid;
        static decimal rmqty;
        public static int scrapcnt = 0;
        public static int scrapid = 0;
        static decimal scrapqty = 0;

        int systemDate = 0;
        //static Boolean RMStockQty = true;
        static Boolean RMLinkQty = true;
        static string RMName = "";
        static string StrRecoDCNo = "";
        static string StrRecoDCDate = "";
        static decimal reqrmqty;
        static decimal balrmqty;
        //static decimal stockrmqty;
        static decimal linkrmqty;
        public static int reqrmid;

        static int iHeaderId;

        static int deleteSGSuccess = 0;
        static int deleteVASuccess = 0;

        public static int linkId = 0;
        static string tags;
        BL_DB DataAcesslayer = new BL_DB();
        static int BranchID;
        static int DeptId;
        static int TaxCodeId;
        static int WCId;
        static int GateEntryNoID;
        static int Transporter;
        static int Vehicle;
        static int process;
        static int AnnexureStatus;
        //static int WId;
        static int PSupplyId;

        string vBAbbr = "";
        string vBName = "";
        public static int vBType = 0;

        string vBomAbbr = "";
        string vBomName = "";
        int vBomType = 0;


        string vGRNJWAbbr = "";
        string vGRNJWName = "";
        public static int vGRNJWType = 0;




        string vPAnxJWAbbr = "";
        string vPAnxJWName = "";
        int vPAnxJWType = 0;

        string vPCustScrpAbbr = "";
        string vPCustScrpName = "";
        int vPCustScrpType = 0;

        //Before Save CheckFGQty Variables
        static int iRowID = 0;
        static int iBodyId = 0;
        static int chkRMexists = 0;
        static int chkRMMaxId = 0;
        //static decimal chkRMBalQty = 0;
        static decimal chkReqQtySum = 0;
        static decimal bomfgqty = 0;
        BL_DB objDB = new BL_DB();
        #endregion

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

        [HttpPost]
        public ActionResult UpdateDCJWSal(int CompanyId, string SessionId, string User, int LoginId, int vtype, string docNo, List<PEBody> BodyData)
        {

            clsGeneric.Log_write(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), SessionId, CompanyId, docNo, vtype, User);

            strValue = $@"SELECT CHD.AnnexureStatus FROM  dbo.tCore_HeaderData" + vtype + "_0 AS CHD INNER JOIN " +
                "dbo.tCore_Header_0 AS CH ON CHD.iHeaderId = CH.iHeaderId WHERE (CH.iVoucherType =" + vtype + ") AND (CH.sVoucherNo = N'" + docNo + "')";
            AnnexureStatus = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, strValue));
            //0,Pending,1,Annex not Generate,2,Updated
            if (AnnexureStatus == 0 || AnnexureStatus == 1) // 0 inside 1 outside
            {
                try
                {

                    clsGeneric.writeLog("Method Name:" + "UpdateDCJWSal");
                    clsGeneric.writeLog("Company:" + (CompanyId));
                    clsGeneric.writeLog("SessionId:" + (SessionId));
                    clsGeneric.writeLog("Vno:" + (docNo));

                    clsGeneric.createTableRMReqQtySAL(CompanyId, User, docNo);
                    clsGeneric.createTablelinkinfoSAL(CompanyId, User, docNo);
                    clsGeneric.createTablelinkrmusedSAL(CompanyId, User, docNo);

                    vBType = vtype;
                    string sVNo = docNo;
                    strValue = $@"select SAbbr from[dbo].[cCore_Vouchers_0]  where iVoucherType = '" + vtype + "'";
                    vBAbbr = Convert.ToString(clsGeneric.ShowRecord(CompanyId, strValue));
                    strValue = $@"select Sname from[dbo].[cCore_Vouchers_0]  where iVoucherType = '" + vtype + "'";
                    vBName = Convert.ToString(clsGeneric.ShowRecord(CompanyId, strValue));


                    vGRNJWAbbr = "GRNJWSal";
                    strValue = $@"select iVoucherType from  [dbo].[cCore_Vouchers_0] where sAbbr = '" + vGRNJWAbbr + "'";
                    vGRNJWType = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, strValue));
                    strValue = $@"select Sname from  [dbo].[cCore_Vouchers_0] where sAbbr = '" + vGRNJWAbbr + "'";
                    vGRNJWName = Convert.ToString(clsGeneric.ShowRecord(CompanyId, strValue));


                    strValue = $@"SELECT distinct max(dbo.tCore_Links_0.iLinkId) " +
                                            " FROM dbo.tCore_Header_0 INNER JOIN dbo.tCore_Data_0 ON dbo.tCore_Header_0.iHeaderId = dbo.tCore_Data_0.iHeaderId INNER JOIN " +
                                            " dbo.tCore_Indta_0 ON dbo.tCore_Data_0.iBodyId = dbo.tCore_Indta_0.iBodyId INNER JOIN dbo.tCore_Links_0 ON dbo.tCore_Data_0.iTransactionId = dbo.tCore_Links_0.iTransactionId " +
                                            " WHERE(dbo.tCore_Header_0.iVoucherType =" + vGRNJWType + ")";
                    linkId = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, strValue));

                    vPAnxJWAbbr = "AnxJWSale";
                    strValue = $@"select iVoucherType from  [dbo].[cCore_Vouchers_0] where sAbbr = '" + vPAnxJWAbbr + "'";
                    vPAnxJWType = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, strValue));
                    strValue = $@"select Sname from  [dbo].[cCore_Vouchers_0] where sAbbr = '" + vPAnxJWAbbr + "'";
                    vPAnxJWName = Convert.ToString(clsGeneric.ShowRecord(CompanyId, strValue));


                    vPCustScrpAbbr = "CustScrap";
                    strValue = $@"select iVoucherType from  [dbo].[cCore_Vouchers_0] where sAbbr = '" + vPCustScrpAbbr + "'";
                    vPCustScrpType = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, strValue));
                    strValue = $@"select Sname from  [dbo].[cCore_Vouchers_0] where sAbbr = '" + vPCustScrpAbbr + "'";
                    vPCustScrpName = Convert.ToString(clsGeneric.ShowRecord(CompanyId, strValue));


                    DateTime tody = DateTime.Now.Date;

                    clsGeneric.writeLog("System Date:" + (tody));
                    systemDate = clsGeneric.GetDateToInt(tody);

                    strValue = $@"SELECT   CONCAT('iTag', CVM.iMasterType) AS TagFld FROM    dbo.cCore_MasterDef AS CMD INNER JOIN " +
                                " dbo.cCore_VoucherMasters_0 AS CVM ON CMD.iMasterTypeId = CVM.iMasterType " +
                                " WHERE (CVM.iVoucherType = " + vtype + ") and (CVM.iMasterType NOT IN (3, 4))";

                    DataSet dr = DataAcesslayer.GetData(strValue, CompanyId, ref strErrorMessage);

                    clsGeneric.writeLog("Getting from Tags:" + (dr));
                    if (dr != null)
                    {
                        tags = "";
                        for (int ir = 0; ir < dr.Tables[0].Rows.Count; ir++)
                        {
                            if (tags == "")
                            {
                                tags = "ISNULL(tgs." + Convert.ToString(dr.Tables[0].Rows[ir][0]) + ",0) " + Convert.ToString(dr.Tables[0].Rows[ir][0]);
                            }
                            else
                            {
                                tags = tags + ", " + " ISNULL(tgs." + Convert.ToString(dr.Tables[0].Rows[ir][0]) + ",0) " + Convert.ToString(dr.Tables[0].Rows[ir][0]);

                            }

                        }

                    }
                    if (tags == "") // tags not used
                    {
                        strValue = "SELECT d.iBookNo,d.iInvTag,d.iFaTag, h.iDate,  TD.RecoItem, i.fQuantity, i.mRate, i.mGross, hd.iHeaderId AS HeaderID, " +
                                " d.iTransactionId, " +
                                " bt.iBatchId, bt.sBatchNo, bt.fRate,bt.iMfDate,d.iCode," +
                                " hd.GateEntryNo,hd.GateEntryDate, hd.PONo, hd.PODate, hd.DeliveryTerms,hd.PmtTerms,hd.Price, hd.PackandForwd,  i.iProduct " +
                                " from  tCore_Data_0 d join tCore_Header_0 h ON h.iHeaderId = d.iHeaderId " +
                                " join tCore_Indta_0 i on i.ibodyid = d.ibodyid " +
                                " left join  tCore_HeaderData" + vtype + "_0 hd   on hd.iHeaderId = h.iHeaderId " +
                                " join mCore_Account ac on d.ibookNo = ac.iMasterId INNER JOIN " +
                                " dbo.tCore_Data" + vtype + "_0 AS TD ON TD.iBodyId = d.iBodyId " +
                                " INNER JOIN  dbo.tCore_Batch_0 bt ON d.iBodyId = bt.iBodyId " +
                                " AND iVoucherType =" + vtype + " and sVoucherNo = '" + docNo + "' ";
                    }
                    else
                    {
                        strValue = " SELECT d.iBookNo,d.iInvTag, d.iFaTag,h.iDate, TD.RecoItem, i.fQuantity, i.mRate, i.mGross, hd.iHeaderId AS HeaderID, " +
                                " d.iTransactionId, " +
                                " bt.iBatchId, bt.sBatchNo, bt.fRate,bt.iMfDate,d.iCode," +
                                " hd.GateEntryNo,hd.GateEntryDate, hd.PONo, hd.PODate, hd.DeliveryTerms,hd.PmtTerms,hd.Price, hd.PackandForwd," +
                                 tags + " ,i.iProduct"+
                                " from  tCore_Data_0 d join tCore_Header_0 h ON h.iHeaderId = d.iHeaderId " +
                                " join tCore_Indta_0 i on i.ibodyid = d.ibodyid " +
                                " left join  tCore_HeaderData" + vtype + "_0 hd   on hd.iHeaderId = h.iHeaderId " +
                                " join mCore_Account ac on d.ibookNo = ac.iMasterId INNER JOIN " +
                                " dbo.tCore_Data" + vtype + "_0 AS TD ON TD.iBodyId = d.iBodyId " +
                                " INNER JOIN  dbo.tCore_Data_Tags_0 as tgs ON d.iBodyId = tgs.iBodyId " +
                                " INNER JOIN  dbo.tCore_Batch_0 bt ON d.iBodyId = bt.iBodyId " +
                                " AND iVoucherType =" + vtype + " and sVoucherNo = '" + docNo + "' ";
                    }

                    strValue = "SELECT d.iBookNo,d.iInvTag,d.iFaTag, h.iDate,  TD.RecoItem, i.fQuantity, i.mRate, i.mGross, hd.iHeaderId AS HeaderID, " +
                                " d.iTransactionId, " +
                                " bt.iBatchId, bt.sBatchNo, bt.fRate,bt.iMfDate,d.iCode," +
                                " 'hd.GateEntryNo','hd.GateEntryDate', hd.PONo, hd.PODate, hd.DeliveryTerms,hd.PmtTerms,hd.Price, hd.PackandForwd,i.iProduct " +
                                " from  tCore_Data_0 d join tCore_Header_0 h ON h.iHeaderId = d.iHeaderId " +
                                " join tCore_Indta_0 i on i.ibodyid = d.ibodyid " +
                                " left join  tCore_HeaderData" + vtype + "_0 hd   on hd.iHeaderId = h.iHeaderId " +
                                " join mCore_Account ac on d.ibookNo = ac.iMasterId INNER JOIN " +
                                " dbo.tCore_Data" + vtype + "_0 AS TD ON TD.iBodyId = d.iBodyId " +
                                " INNER JOIN  dbo.tCore_Batch_0 bt ON d.iBodyId = bt.iBodyId " +
                                " AND iVoucherType =" + vtype + " and sVoucherNo = '" + docNo + "' ";


                    clsGeneric.writeLog("Getting from Voucher:" + (strValue));
                    DataSet ds = DataAcesslayer.GetData(strValue, CompanyId, ref strErrorMessage);

                    clsGeneric.writeLog("Getting from Voucher:" + (ds));
                    if (ds != null)
                    {


                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            /*
                            iBookNo = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                            iInvTag = Convert.ToInt32(ds.Tables[0].Rows[0][1]);
                            Warehouse = Convert.ToInt32(ds.Tables[0].Rows[0][1]);
                            BranchID = Convert.ToInt32(ds.Tables[0].Rows[0][2]);
                            iDocDate = Convert.ToInt32(ds.Tables[0].Rows[0][3]);
                            iHeaderId = Convert.ToInt32(ds.Tables[0].Rows[0][8]);
                            //sNarration = Convert.ToString(ds.Tables[0].Rows[0][3]);
                            GateEntryNo = Convert.ToString(ds.Tables[0].Rows[0][15]); // 9
                            GateEntryDate = Convert.ToInt32(ds.Tables[0].Rows[0][16]);
                            PONo = Convert.ToString(ds.Tables[0].Rows[0][17]);
                            PODate = Convert.ToInt32(ds.Tables[0].Rows[0][18]);
                            //LRNo = Convert.ToString(ds.Tables[0].Rows[0][13]);
                            //LRDate = Convert.ToInt32(ds.Tables[0].Rows[0][14]);
                            //VehicleNo = Convert.ToString(ds.Tables[0].Rows[0][15]);
                            //Transporter = Convert.ToString(ds.Tables[0].Rows[0][16]);
                            TransactionId = Convert.ToInt32(ds.Tables[0].Rows[0][9]);
                            iCode = Convert.ToInt32(ds.Tables[0].Rows[0][14]);
                            */


                            iBookNo = Convert.ToInt32(ds.Tables[0].Rows[0]["iBookNo"]);
                            iInvTag = Convert.ToInt32(ds.Tables[0].Rows[0]["iInvTag"]);
                            Warehouse = Convert.ToInt32(ds.Tables[0].Rows[0]["iInvTag"]);
                            BranchID = Convert.ToInt32(ds.Tables[0].Rows[0]["iFaTag"]);
                            iDocDate = Convert.ToInt32(ds.Tables[0].Rows[0]["iDate"]);
                            iHeaderId = Convert.ToInt32(ds.Tables[0].Rows[0]["HeaderID"]);
                            //sNarration = Convert.ToString(ds.Tables[0].Rows[0][3]);
                            PONo = Convert.ToString(ds.Tables[0].Rows[0]["PONo"]);
                            PODate = Convert.ToInt32(ds.Tables[0].Rows[0]["PODate"]);
                            TransactionId = Convert.ToInt32(ds.Tables[0].Rows[0]["iTransactionId"]);
                            iCode = Convert.ToInt32(ds.Tables[0].Rows[0]["iCode"]);

                            /*
                            if (tags != "")
                            {
                                DeptId = Convert.ToInt32(ds.Tables[0].Rows[0][23]);
                                WCId = Convert.ToInt32(ds.Tables[0].Rows[0][24]);
                                GateEntryNoID = Convert.ToInt32(ds.Tables[0].Rows[0][25]);
                                Transporter = Convert.ToInt32(ds.Tables[0].Rows[0][26]);
                                Vehicle = Convert.ToInt32(ds.Tables[0].Rows[0][27]);
                                process = Convert.ToInt32(ds.Tables[0].Rows[0][28]);
                                TaxCodeId = Convert.ToInt32(ds.Tables[0].Rows[0][29]);
                                PSupplyId = 0;
                            }*/
                        }

                        vBomAbbr = "SalJWBOM";
                        strValue = $@"select iVoucherType from [dbo].[cCore_Vouchers_0]  where sAbbr='" + vBomAbbr + "'";
                        vBomType = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, strValue));
                        strValue = $@"select Sname from [dbo].[cCore_Vouchers_0]  where sAbbr='" + vBomAbbr + "'";
                        vBomName = Convert.ToString(clsGeneric.ShowRecord(CompanyId, strValue));

                        string faTagValue = GetFaTagValue(SessionId, vtype, docNo);

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            fgid = Convert.ToInt32(ds.Tables[0].Rows[i][4]);
                            bool b1 = string.IsNullOrEmpty(ds.Tables[0].Rows[i][5].ToString());
                            if (!b1)
                            GRNJW_FGQty = Convert.ToDecimal(ds.Tables[0].Rows[i][5]);
                            //QUery for BOM add filter for fatag   [AND (d.iFaTag = Value Calulate from FatagName)    ]
                            strValue = "SELECT hd.FGQty, i.iProduct, i.fQuantity,cd.ScrapProduct, cd.ScrapQty FROM dbo.tCore_Data_0 AS d INNER JOIN " +
                                    " dbo.tCore_Header_0 AS h ON h.iHeaderId = d.iHeaderId INNER JOIN dbo.tCore_Indta_0 AS i ON i.iBodyId = d.iBodyId INNER JOIN " +
                                    " dbo.tCore_HeaderData" + vBomType + "_0 AS hd ON hd.iHeaderId = h.iHeaderId INNER JOIN " +
                                    " dbo.tCore_Data" + vBomType + "_0 AS cd ON d.iBodyId = cd.iBodyId " +
                                     " WHERE(hd.FGCode = " + fgid + " ) AND (d.iBookNo =" + iBookNo + " ) AND (d.iFaTag = " + faTagValue + ")";

                            DataSet ds1 = DataAcesslayer.GetData(strValue, CompanyId, ref strErrorMessage);

                            clsGeneric.writeLog("Getting from Voucher:" + (ds1));
                            if (ds1.Tables[0].Rows.Count != 0)
                            {
                                for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                                {
                                    rmid = Convert.ToInt32(ds1.Tables[0].Rows[j][1]);
                                    scrapid = Convert.ToInt32(ds1.Tables[0].Rows[j][3]);
                                    bool b2 = string.IsNullOrEmpty(ds1.Tables[0].Rows[j][2].ToString());
                                    if (!b2)
                                        rmqty = Math.Round(Convert.ToDecimal(ds1.Tables[0].Rows[j][2]) * GRNJW_FGQty, 2);
                                    scrapqty = Math.Round(Convert.ToDecimal(ds1.Tables[0].Rows[j][4]) * GRNJW_FGQty, 2);
                                    bool b3 = string.IsNullOrEmpty(ds1.Tables[0].Rows[j][0].ToString());
                                    if (!b3)
                                        fgqty = Convert.ToDecimal(ds1.Tables[0].Rows[j][0]);

                                    //insert fatag value
                                    strQry = $@"Insert into RMReqQtySAL (FgId,FgQty,GRNJW_FGQty,rmid,rmqty,batchid,batchname,bMfDate,bfrate,vno,loggeduser,ScrapId,ScrapQty,TaxCode) values('{fgid}' ,{fgqty},{GRNJW_FGQty}, {rmid},{rmqty},{ds.Tables[0].Rows[i][10]},'{ds.Tables[0].Rows[i][11]}',{ds.Tables[0].Rows[i][13]},{ds.Tables[0].Rows[i][12]},'{docNo}','{User}',{scrapid},{scrapqty},{TaxCodeId})";
                                    int insert = DataAcesslayer.GetExecute(strQry, Convert.ToInt32(CompanyId), ref strErrorMessage);

                                    clsGeneric.writeLog("Populated temp table: " + insert + " for Fg: " + fgid + "fgQty :" + fgqty + " Rm id " + rmid + " rm Qty" + rmqty + "docNo" + docNo + "User" + User);

                                }
                            }
                            else
                            {
                                strQry = "";
                                strQry = " Product :-" + clsGeneric.ShowRecord(CompanyId, "SElect Sname from mCore_Product where iMasterId = " + fgid);
                                strQry = strQry + ", Party :-" + clsGeneric.ShowRecord(CompanyId, "SElect Sname from mCore_Account where iMasterId =" + iBookNo);

                                return Json(new { status = false, data = new { message = "Record is not found " + strQry } });
                            }

                        }
                        //strValue = $@"SELECT TOP (1) dbo.muCore_Account.VendorWH FROM dbo.vmCore_Account AS a INNER JOIN dbo.muCore_Account ON a.iMasterId = dbo.muCore_Account.iMasterId WHERE a.iMasterId = " + iBookNo;
                        //strValue = $@"SELECT  WSH.iMasterId AS JWWarehouseID" +
                        //" FROM  dbo.muCore_Account_Settings AS MUCAS WITH (READUNCOMMITTED)INNER JOIN " +
                        //" dbo.mCore_Account AS mca WITH(READUNCOMMITTED) ON MUCAS.iPrimaryAccount = mca.iMasterId INNER JOIN " +
                        //" dbo.mCore_Warehouse AS WSH WITH(READUNCOMMITTED) ON MUCAS.JWWarehouse = WSH.iMasterId " +
                        //" WHERE(MUCAS.iMasterId = " + iBookNo + ")";
                        //vWarehoseID = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, strValue)); //no need
                        // Collect the Sum of RMQty with RMID 
                        // Extracting Stock Quantity 
                        // Extracting Link Qunatity 

                        //RMStockQty = true;
                        RMLinkQty = true;
                        linkrmqty = 0;
                        //stockrmqty = 0;
                        reqrmqty = 0;
                        reqrmid = 0;
                        strValue = $@"SELECT SUM(rmqty) AS Qty, RMID FROM dbo.RMReqQtySAL GROUP BY Id,FgId,rmid, vno, [loggeduser] HAVING (vno = '" + docNo + "') AND ([loggeduser] = '" + User + "') Order by id";

                        clsGeneric.RMwriteLog("RM Name" + " " + " Available Link Qty " + " " + " Required Qty ");
                        DataSet ds3 = DataAcesslayer.GetData(strValue, CompanyId, ref strErrorMessage);
                        if (ds3 != null)
                        {
                            for (int k = 0; k < ds3.Tables[0].Rows.Count; k++)
                            {
                                bool b3 = string.IsNullOrEmpty(ds3.Tables[0].Rows[k][0].ToString());
                                if (!b3)
                                reqrmqty = Convert.ToDecimal(ds3.Tables[0].Rows[k][0]);
                                reqrmid = Convert.ToInt32(ds3.Tables[0].Rows[k][1]);
                                RMName = clsGeneric.ShowRecord(CompanyId, "select sname from mCore_Product where iMasterId=" + reqrmid);
                                // Extracting Stock Quantity 
                                //stockrmqty = Convert.ToDecimal(getStockQty(reqrmid, vWarehoseID, iDocDate, CompanyId));
                                //clsGeneric.writeLog("Stock RM Qty Compare");
                                //clsGeneric.writeLog("RMID " + reqrmid);
                                //clsGeneric.writeLog("Stock RM Qty" + stockrmqty);
                                //clsGeneric.writeLog("RMQty :" + reqrmqty);
                                //if (stockrmqty >= reqrmqty)
                                //{
                                //}
                                //else
                                //{
                                //    RMStockQty = false;
                                //}
                                // Extracting Link Quantity 

                                //RMStockQty = true;
                                linkrmqty = Convert.ToDecimal(getLinkQty(reqrmid, iBookNo, iDocDate, CompanyId));

                                clsGeneric.writeLog("Link RM Qty Compare");
                                clsGeneric.writeLog("RMID " + reqrmid);
                                clsGeneric.writeLog("Link RM Qty" + linkrmqty);
                                clsGeneric.writeLog("RMQty :" + reqrmqty);

                                if (linkrmqty >= reqrmqty)
                                {

                                }
                                else
                                {

                                    RMLinkQty = false;
                                }

                                clsGeneric.RMwriteLog(RMName + " " + linkrmqty + " " + reqrmqty);
                            }
                        }



                        if (RMLinkQty == false)
                        {
                            clsGeneric.createTableRMReqQtySAL(CompanyId, User, docNo);
                            clsGeneric.createTablelinkinfoSAL(CompanyId, User, docNo);
                            clsGeneric.createTablelinkrmusedSAL(CompanyId, User, docNo);
                            clsGeneric.Update_AnnexureStatusSAL(CompanyId, iHeaderId, 1);

                            return Json(new { status = false, data = new { message = "Posting Failed, RM :- " + RMName + ", Link Quantity not available" } });
                        }
                        //if (RMStockQty == false)
                        //{
                        //    clsGeneric.createTableRMReqQty(CompanyId, User, docNo);
                        //    clsGeneric.createTablelinkinfo(CompanyId, User, docNo);
                        //    clsGeneric.createTablelinkrmused(CompanyId, User, docNo);
                        //    clsGeneric.Update_AnnexureStatus(CompanyId, iHeaderId, 1);
                        //    return Json(new { status = false, data = new { message = "Posting Failed, RM :- "+ RMName + "Stock Quantity not available" } });
                        //}

                        //Posting here.
                        if (RMLinkQty)
                        {

                            #region Header And Footer Data For Annexure Vendor JW - Header


                            Hashtable headerAVJW = new Hashtable();
                            headerAVJW.Add("DocNo", null);
                            headerAVJW.Add("Date", iDocDate);
                            headerAVJW.Add("CustomerAC__Id", iBookNo);
                            headerAVJW.Add("SalesAC__Id", iCode);
                            //headerAVJW.Add("Branch__Id", BranchID);
                            //headerAVJW.Add("Dept__Id", DeptId);
                            //headerAVJW.Add("Works Center__Id", WCId); //4

                            headerAVJW.Add("Warehouse__Id", Warehouse); //4 

                            //headerAVJW.Add("Gate Entry No__Id", GateEntryNoID);
                            //headerAVJW.Add("Transporter__Id", Transporter);
                            //headerAVJW.Add("Vehicle__Id", Vehicle);

                            //headerAVJW.Add("GateEntryNo", GateEntryNo);
                            //headerAVJW.Add("GateEntryDate", GateEntryDate);
                            AddTagsToHeader(ref headerAVJW, SessionId, vtype, docNo);


                            headerAVJW.Add("PONo", PONo);
                            headerAVJW.Add("PODate", PODate);

                            //headerAVJW.Add("BillNo", "");
                            //headerAVJW.Add("BillDate", DCDate);
                            //headerAVJW.Add("LRNo", LRNo);
                            //headerAVJW.Add("LRDate", LRDate);
                            headerAVJW.Add("RemarksStore", "");
                            headerAVJW.Add("RemarksQC", "");

                            headerAVJW.Add("DCJWNo", docNo);
                            headerAVJW.Add("DCJWDate", iDocDate);

                            PSupplyId = 0;
                            headerAVJW.Add("PlaceofSupply__Id", PSupplyId);
                            Hashtable headerflagAVJW = new Hashtable
                            {
                                {"Approved" , true},
                                {"NoBatchCheck", true }, // true
                                {"NoLinkCheck", false},
                                {"CheckNegativeStock", false},
                                {"Suspended", false},
                                {"UnsaveInv", true},
                                {"UpdateFA", false },
                                {"UpdateInv", true }
                            };
                            //headerAVJW.Add("Flags", headerflagAVJW);
                            HashData objHashRequest = new HashData();

                            List<System.Collections.Hashtable> lstBody = new List<System.Collections.Hashtable>();

                            #endregion
                            clsGeneric.createTablelinkinfoSAL(CompanyId, User, docNo);
                            clsGeneric.createTablelinkrmusedSAL(CompanyId, User, docNo);

                            updateTables(docNo, User, iBookNo, iDocDate, CompanyId, SessionId, vtype);

                            strValue = $@"Select t1.iTransactionID,t1.iVoucherType,t1.sVoucherNo, t2.UsedQty, t1.iProduct,t1.iDate,t1.mrate, " +
                                " t1.BatchId, t1.BatchNo, t1.BMfDate, t1.Brate,t1.Process, t2.fgid, ABS(t2.fgqty), t2.fgrate,WorksCenter,Dept,TaxCode, t1.DCNo, t1.DCDate  from  linkinfoSAL as t1 inner join linkrmusedSAL as t2 on " +
                                " t1.liid = t2.liid where (t1.vno = '" + docNo + "') AND (t1.loggeduser = '" + User + "')" +
                                " and t2.loggeduser = '" + User + "' and t2.vno = '" + docNo + "'";
                            DataSet ds7 = DataAcesslayer.GetData(strValue, CompanyId, ref strErrorMessage);
                            if (ds7 != null)
                            {
                                StrRecoDCNo = "";
                                StrRecoDCDate = "";
                                for (int kkk2 = 0; kkk2 < ds7.Tables[0].Rows.Count; kkk2++)
                                {
                                    strQry = $@"select sName from mCore_Product where iMasterId=" + Convert.ToInt32(ds7.Tables[0].Rows[kkk2][12]);
                                    FgName = clsGeneric.ShowRecord(CompanyId, strQry);
                                    strQry = $@"select sCode from mCore_Product where iMasterId=" + Convert.ToInt32(ds7.Tables[0].Rows[kkk2][12]);
                                    FgCode = clsGeneric.ShowRecord(CompanyId, strQry);
                                    fgqty = Convert.ToDecimal(ds7.Tables[0].Rows[kkk2][13]);
                                    DC_TransactionId = Convert.ToInt32(ds7.Tables[0].Rows[kkk2][0]);
                                    DC_vType = Convert.ToInt32(ds7.Tables[0].Rows[kkk2][1]);
                                    strQry = $@"select sAbbr from cCore_Vouchers_0 where iVouchertype=" + Convert.ToInt32(ds7.Tables[0].Rows[kkk2][1]);
                                    DC_Abbr= clsGeneric.ShowRecord(CompanyId, strQry);
                                    DC_Docno = Convert.ToString(ds7.Tables[0].Rows[kkk2][2]);
                                    bool b4 = string.IsNullOrEmpty(ds7.Tables[0].Rows[kkk2][3].ToString());
                                    if (!b4)
                                        Consumed_RM_Qty = Convert.ToDecimal(ds7.Tables[0].Rows[kkk2][3]);
                                    Hashtable bodyRefereceAVJW = new Hashtable();
                                    bodyRefereceAVJW.Add("BaseTransactionId", DC_TransactionId);
                                    bodyRefereceAVJW.Add("VoucherType", DC_vType);
                                    //bodyRefereceAVJW.Add("VoucherNo", vBAbbr + ":" + DC_Docno);
                                    bodyRefereceAVJW.Add("VoucherNo", DC_Abbr + ":" + DC_Docno);
                                    bodyRefereceAVJW.Add("UsedValue", Consumed_RM_Qty);
                                    bodyRefereceAVJW.Add("LinkId", linkId);
                                    bodyRefereceAVJW.Add("RefId", DC_TransactionId);


                                    Hashtable bodyBatchAVJW = new Hashtable
                                    {
                                        //{"BatchId" , Convert.ToInt32(ds7.Tables[0].Rows[kkk2][7])},
                                        {"BatchNo", Convert.ToString(ds7.Tables[0].Rows[kkk2][8])}
                                        //{"BatchRate", Convert.ToDecimal(ds7.Tables[0].Rows[kkk2][10])},
                                        //{"MfgDate", Convert.ToInt32(ds7.Tables[0].Rows[kkk2][9])}
                                    };


                                    Hashtable bodyAVJW = new Hashtable();
                                    //bodyAVJW.Add("Process__Id", Convert.ToInt32(ds7.Tables[0].Rows[kkk2][11]));
                                    //bodyAVJW.Add("Tax Code__Id", Convert.ToInt32(ds7.Tables[0].Rows[kkk2][17]));

                                    bodyAVJW.Add("Item__Id", Convert.ToInt32(ds7.Tables[0].Rows[kkk2][4]));
                                    bodyAVJW.Add("Quantity", Convert.ToDecimal(Consumed_RM_Qty));


                                    List<System.Collections.Hashtable> lstRefernce = new List<System.Collections.Hashtable>();
                                    List<System.Collections.Hashtable> lstBatch = new List<System.Collections.Hashtable>();
                                    lstBatch.Add(bodyBatchAVJW);
                                    lstRefernce.Add(bodyRefereceAVJW);

                                    bodyAVJW.Add("L-GRN Job Work Sales", lstRefernce);
                                    bodyAVJW.Add("Rate", Convert.ToDecimal(ds7.Tables[0].Rows[kkk2][6]));
                                    bodyAVJW.Add("Batch", bodyBatchAVJW);
                                    //bodyAVJW.Add("DCBatch", Convert.ToString(ds7.Tables[0].Rows[kkk2][8]));
                                    //bodyAVJW.Add("DCMFGDate", Convert.ToInt32(ds7.Tables[0].Rows[kkk2][9]));
                                    bodyAVJW.Add("DJSItemCode", FgCode);
                                    bodyAVJW.Add("DJSItemName", FgName);
                                    bodyAVJW.Add("DJSItemQty", fgqty);
                                    bodyAVJW.Add("sRemarks", "");
                                    bodyAVJW.Add("GRNJWSNo", DC_Abbr + ":" + DC_Docno);
                                    bodyAVJW.Add("GRNJWSDate", Convert.ToInt32(ds7.Tables[0].Rows[kkk2][5]));

                                    // Added By Rakesh suggested by majid
                                    bodyAVJW.Add("CustDCNo", Convert.ToString(ds7.Tables[0].Rows[kkk2]["DCNo"]));
                                    bodyAVJW.Add("CustDCDate", Convert.ToInt32(ds7.Tables[0].Rows[kkk2]["DCDate"]));



                                   

                                    AddTagsToBody(ref bodyAVJW, SessionId, vtype, docNo);


                                    lstBody.Add(bodyAVJW);
                                }


                                strValue = $@"Select t1.DCNo as DCNo,  CONVERT(varchar, dbo.IntToDate(t1.DCDate), 105) as DCDate   from  linkinfoSAL as t1 inner join linkrmusedSAL as t2 on " +
                                " t1.liid = t2.liid where (t1.vno = '" + docNo + "') AND (t1.loggeduser = '" + User + "')" +
                                " and t2.loggeduser = '" + User + "' and t2.vno = '" + docNo + "' GROUP BY t1.DCNo, t1.DCDate ";
                                DataSet ds8 = DataAcesslayer.GetData(strValue, CompanyId, ref strErrorMessage);

                                if(ds8 != null)
                                { 
                                    for (int i = 0; i < ds8.Tables[0].Rows.Count; i++)
                                    {
                                        StrRecoDCNo = StrRecoDCNo + Convert.ToString(ds8.Tables[0].Rows[i]["DCNo"]) + "\n";
                                        StrRecoDCDate = StrRecoDCDate + Convert.ToString(ds8.Tables[0].Rows[i]["DCDate"]) + "\n";
                                    }
                                }


                                string updateNo = "Update dbo.tCore_Data6150_0 Set RecoDCNo = '"+ StrRecoDCNo + "', RecoDCDate = '" + StrRecoDCDate + "'  where iBodyId = (select Top(1) iBodyId from tCore_Data_0 where iHeaderId = " + iHeaderId + ")";
                                DataAcesslayer.GetExecute(updateNo, CompanyId, ref strErrorMessage);
                                //string updateDate = "Update dbo.tCore_Data6150_0 Set RecoDCDate = '"+ StrRecoDCDate + "'  where iBodyId = (select Top(1) iBodyId from tCore_Data_0 where iHeaderId = " + iHeaderId + ")";
                                //DataAcesslayer.GetExecute(updateDate, CompanyId, ref strErrorMessage);
                                //Update dbo.tCore_Data6150_0 Set RecoDCNo = ''  where iBodyId = (select Top(1) iBodyId from tCore_Data_0 where iHeaderId = 501)
                                //Update dbo.tCore_Data6150_0 Set RecoDCDate = ''  where iBodyId = (select Top(1) iBodyId from tCore_Data_0 where iHeaderId = 501)

                                System.Collections.Hashtable objHash = new System.Collections.Hashtable();
                                objHash.Add("Body", lstBody);
                                objHash.Add("Header", headerAVJW);

                                List<System.Collections.Hashtable> lstHash = new List<System.Collections.Hashtable>();
                                lstHash.Add(objHash);
                                objHashRequest.data = lstHash;
                                string sContentAVJW = JsonConvert.SerializeObject(objHashRequest);
                                clsGeneric.writeLog("Upload URLAVJW :" + "http://localhost/Focus8API/Transactions/Vouchers/" + vPAnxJWName);
                                clsGeneric.writeLog("URL Param :" + sContentAVJW);
                                using (var clientAVJW = new WebClient())
                                {

                                    clientAVJW.Encoding = Encoding.UTF8;
                                    clientAVJW.Headers.Add("fSessionId", SessionId);
                                    clientAVJW.Headers.Add("Content-Type", "application/json");
                                    var responseAVJW = clientAVJW.UploadString("http://localhost/Focus8API/Transactions/Vouchers/" + vPAnxJWName, sContentAVJW);

                                    clsGeneric.writeLog("Response form AVJW :" + responseAVJW);
                                    if (responseAVJW != null)
                                    {
                                        var responseDataAVJW = JsonConvert.DeserializeObject<APIResponse.PostResponse>(responseAVJW);
                                        if (responseDataAVJW.result == -1)
                                        {
                                            clsGeneric.createTableRMReqQtySAL(CompanyId, User, docNo);
                                            clsGeneric.createTablelinkinfoSAL(CompanyId, User, docNo);
                                            clsGeneric.createTablelinkrmusedSAL(CompanyId, User, docNo);
                                            clsGeneric.Update_AnnexureStatusSAL(CompanyId, vtype, iHeaderId, 1);


                                            return Json(new { status = false, data = new { message = responseDataAVJW.message } });
                                        }
                                        else
                                        {
                                            UpdateStatus(vtype, docNo, 1, CompanyId);

                                            strValue = $@"Select Count(1)  FROM dbo.RMReqQtySAL where ScrapId != 0" +
                                            " and (vno = '" + docNo + "') AND ([loggeduser] = '" + User + "')";
                                            scrapcnt = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, strValue));

                                            if (scrapcnt != 0)
                                            {

                                                HashData objHashRequest_VenScrap = new HashData();
                                                Hashtable headerVenScrap = new Hashtable();
                                                headerVenScrap.Add("DocNo", null);
                                                headerVenScrap.Add("Date", iDocDate);
                                                headerVenScrap.Add("VendorAC__Id", iBookNo);

                                                //headerVenScrap.Add("Branch__Id", BranchID);
                                                //headerVenScrap.Add("Dept__Id", DeptId);
                                                //headerVenScrap.Add("Works Center__Id", WCId); //4

                                                //headerVenScrap.Add("GRNJWNo", docNo);
                                                //headerVenScrap.Add("GRNJWDate", iDocDate);


                                                headerVenScrap.Add("DCJWNo", docNo);
                                                headerVenScrap.Add("DCJWDate", iDocDate);

                                                Hashtable headerflagVenScrap = new Hashtable
                                                {
                                                    {"Approved" , true},
                                                    {"Suspended", false},
                                                    {"UnsaveInv", false},
                                                    {"UpdateFA", false },
                                                    {"UpdateInv", false}
                                                };
                                                headerVenScrap.Add("Flags", headerflagVenScrap);

                                                AddTagsToHeader(ref headerVenScrap, SessionId, vtype, docNo);

                                                List<System.Collections.Hashtable> lstBody_VenScrap = new List<System.Collections.Hashtable>();

                                                strValue = $@"Select ScrapId, ABS(ScrapQty) FROM dbo.RMReqQtySAL where (ScrapId != 0) and (vno = '" + docNo + "') AND ([loggeduser] = '" + User + "') Order by id";
                                                clsGeneric.RMwriteLog("Scrap Id" + " " + " Scrap Qty ");
                                                DataSet ds_vs = DataAcesslayer.GetData(strValue, CompanyId, ref strErrorMessage);
                                                if (ds_vs != null)
                                                {
                                                    scrapid = 0;
                                                    for (int k_vs = 0; k_vs < ds_vs.Tables[0].Rows.Count; k_vs++)
                                                    {
                                                        scrapid = Convert.ToInt32(ds_vs.Tables[0].Rows[k_vs][0]);
                                                        Hashtable bodyVenScrap = new Hashtable();
                                                        bodyVenScrap.Add("Item__Id", Convert.ToInt32(ds_vs.Tables[0].Rows[k_vs][0]));
                                                        bodyVenScrap.Add("Quantity", Convert.ToDecimal(ds_vs.Tables[0].Rows[k_vs][1]));
                                                        bodyVenScrap.Add("Rate", 0);
                                                        clsGeneric.RMwriteLog(scrapid + ", " + Convert.ToInt32(ds_vs.Tables[0].Rows[k_vs][1]));

                                                        AddTagsToBody(ref bodyVenScrap, SessionId, vtype, docNo);

                                                        lstBody_VenScrap.Add(bodyVenScrap);



                                                    }
                                                }

                                                System.Collections.Hashtable objHash_VenScrap = new System.Collections.Hashtable();
                                                objHash_VenScrap.Add("Body", lstBody_VenScrap);
                                                objHash_VenScrap.Add("Header", headerVenScrap);

                                                List<System.Collections.Hashtable> lstHash_VenScrap = new List<System.Collections.Hashtable>();
                                                lstHash_VenScrap.Add(objHash_VenScrap);
                                                objHashRequest_VenScrap.data = lstHash_VenScrap;
                                                string sContent_VenScrap = JsonConvert.SerializeObject(objHashRequest_VenScrap);
                                                clsGeneric.writeLog("Upload VenScrap :" + "http://localhost/Focus8API/Transactions/Vouchers/" + vPCustScrpName);
                                                clsGeneric.writeLog("URL Param :" + sContent_VenScrap);
                                                using (var clientVenScrap = new WebClient())
                                                {
                                                    clientVenScrap.Encoding = Encoding.UTF8;
                                                    clientVenScrap.Headers.Add("fSessionId", SessionId);
                                                    clientVenScrap.Headers.Add("Content-Type", "application/json");

                                                    var responseVenScrap = clientVenScrap.UploadString("http://localhost/Focus8API/Transactions/Vouchers/" + vPCustScrpName, sContent_VenScrap);

                                                    clsGeneric.writeLog("Response form VenScrap :" + (responseVenScrap));
                                                    if (responseVenScrap != null)
                                                    {
                                                        var responseDataVenScrap = JsonConvert.DeserializeObject<APIResponse.PostResponse>(responseVenScrap);
                                                        if (responseDataVenScrap.result == -1)
                                                        {
                                                            clsGeneric.createTableRMReqQtySAL(CompanyId, User, docNo);
                                                            clsGeneric.createTablelinkinfoSAL(CompanyId, User, docNo);
                                                            clsGeneric.createTablelinkrmusedSAL(CompanyId, User, docNo);
                                                            clsGeneric.Update_AnnexureStatusSAL(CompanyId, vtype, iHeaderId, 1);
                                                            return Json(new { status = false, data = new { message = responseDataVenScrap.message } });
                                                        }
                                                        else
                                                        {

                                                            clsGeneric.createTableRMReqQtySAL(CompanyId, User, docNo);
                                                            clsGeneric.createTablelinkinfoSAL(CompanyId, User, docNo);
                                                            clsGeneric.createTablelinkrmusedSAL(CompanyId, User, docNo);
                                                            //updateRMStkValue(docNo, vtype, CompanyId);
                                                            clsGeneric.Update_AnnexureStatusSAL(CompanyId, vtype, iHeaderId, 2);
                                                            return Json(new { status = true, data = new { message = "Posting Successful" } });
                                                        }

                                                    }

                                                }

                                            } // scrapcnt checking
                                            else
                                            {
                                                clsGeneric.createTableRMReqQtySAL(CompanyId, User, docNo);
                                                clsGeneric.createTablelinkinfoSAL(CompanyId, User, docNo);
                                                clsGeneric.createTablelinkrmusedSAL(CompanyId, User, docNo);
                                                //updateRMStkValue(docNo, vtype, CompanyId);
                                                clsGeneric.Update_AnnexureStatusSAL(CompanyId, vtype, iHeaderId, 2);
                                                return Json(new { status = true, data = new { message = "Posting Successful" } });
                                            }
                                        }

                                    }

                                }

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    clsGeneric.createTableRMReqQtySAL(CompanyId, User, docNo);
                    clsGeneric.createTablelinkinfoSAL(CompanyId, User, docNo);
                    clsGeneric.createTablelinkrmusedSAL(CompanyId, User, docNo);
                    clsGeneric.Update_AnnexureStatusSAL(CompanyId, vtype, iHeaderId, 1);
                    clsGeneric.writeLog("Exception occured:" + (ex.Message));
                    return Json(new { status = false, data = new { message = "Posting Failed " } });
                    throw;
                } // try
            }
            else
            {
                return Json(new { status = false, data = new { message = "Annaxre already created" } });

            }
            clsGeneric.createTableRMReqQtySAL(CompanyId, User, docNo);
            clsGeneric.createTablelinkinfoSAL(CompanyId, User, docNo);
            clsGeneric.createTablelinkrmusedSAL(CompanyId, User, docNo);
            clsGeneric.Update_AnnexureStatusSAL(CompanyId, vtype, iHeaderId, 1);

            return Json(new { status = true, data = new { succes = msg.ToString() } });


        }

        public static string GetFaTagValue(string SessionID, int vtype, string docNo)
        {
            Hashtable extHeader = new Hashtable();

            char[] spearator = { '-' };
            String[] strlist;
            string faTag;


            using (var webClient = new WebClient())
            {
                webClient.Encoding = Encoding.UTF8;
                webClient.Headers.Add("fSessionId", SessionID);
                webClient.Headers.Add("Content-Type", "application/json");
                clsGeneric.writeLog("Download URL: " + "http://localhost/Focus8API/Screen/Transactions/" + vtype + "/" + docNo);
                var resDoc = webClient.DownloadString("http://localhost/Focus8API/Screen/Transactions/" + vtype + "/" + docNo);
                clsGeneric.writeLog("Response of Focus Fields Data " + resDoc);
                if (resDoc != null)
                {
                    var resDataDoc = JsonConvert.DeserializeObject<APIResponse.PostResponse>(resDoc);
                    if (resDataDoc.result == -1)
                    {
                        //return Json(new { status = false, data = new { message = resDataDoc.message } });
                    }
                    else
                    {
                        if (resDataDoc.data.Count != 0)
                        {
                            extHeader = JsonConvert.DeserializeObject<Hashtable>(JsonConvert.SerializeObject(resDataDoc.data[0]["Header"]));
                        }
                    }
                }
            }

            faTag = (extHeader["FATagName"] == null) ? "" : extHeader["FATagName"].ToString();
            string faTagValue = "0";

            strlist = faTag.Split(spearator);
            if (strlist[0].Equals("H"))
            {
                //strlist[1].ToString() = profitcenter
                faTagValue = extHeader[strlist[1]].ToString();
            }


            return faTagValue;
        }

        public void AddTagsToHeader(ref Hashtable Header, string SessionID, int vtype, string docNo)
        {
            Hashtable extHeader = new Hashtable();

            char[] spearator = { '-' };
            String[] strlist;
            string tag1;
            string tag2;
            string faTag;


            using (var webClient = new WebClient())
            {
                webClient.Encoding = Encoding.UTF8;
                webClient.Headers.Add("fSessionId", SessionID);
                webClient.Headers.Add("Content-Type", "application/json");
                clsGeneric.writeLog("Download URL: " + "http://localhost/Focus8API/Screen/Transactions/" + vtype + "/" + docNo);
                var resDoc = webClient.DownloadString("http://localhost/Focus8API/Screen/Transactions/" + vtype + "/" + docNo);
                clsGeneric.writeLog("Response of Focus Fields Data " + resDoc);
                if (resDoc != null)
                {
                    var resDataDoc = JsonConvert.DeserializeObject<APIResponse.PostResponse>(resDoc);
                    if (resDataDoc.result == -1)
                    {
                        //return Json(new { status = false, data = new { message = resDataDoc.message } });
                    }
                    else
                    {
                        if (resDataDoc.data.Count != 0)
                        {
                            extHeader = JsonConvert.DeserializeObject<Hashtable>(JsonConvert.SerializeObject(resDataDoc.data[0]["Header"]));
                        }
                    }
                }
            }

            tag1 = (extHeader["Tag1Name"] == null) ? "" : extHeader["Tag1Name"].ToString(); //extHeader["Tag1Name"].ToString();
            tag2 = (extHeader["Tag2Name"] == null) ? "" : extHeader["Tag2Name"].ToString();  //extHeader["Tag2Name"].ToString();
            faTag = (extHeader["FATagName"] == null) ? "" : extHeader["FATagName"].ToString();  //extHeader["FATagName"].ToString();            

            strlist = tag1.Split(spearator);
            if (strlist[0].Equals("H"))
            {
                //strlist[1].ToString() = profitcenter
                Header.Add(strlist[1].ToString(), extHeader[strlist[1]].ToString());
            }


            strlist = tag2.Split(spearator);
            if (strlist[0].Equals("H"))
            {
                //strlist[1].ToString() = profitcenter
                Header.Add(strlist[1].ToString(), extHeader[strlist[1]].ToString());
            }


            //FATagName replace of Department__Id
            strlist = faTag.Split(spearator);
            if (strlist[0].Equals("H"))
            {
                //strlist[1].ToString() = profitcenter
                Header.Add(strlist[1].ToString(), extHeader[strlist[1]].ToString());
            }
        }


        public void AddTagsToBody(ref Hashtable Body, string SessionID, int vtype, string docNo)
        {
            Hashtable extHeader = new Hashtable();
            List<Hashtable> extBody = new List<Hashtable>();
            List<Hashtable> extFooter = new List<Hashtable>();

            char[] spearator = { '-' };
            String[] strlist;
            string tag1;
            string tag2;
            string faTag;


            using (var webClient = new WebClient())
            {
                webClient.Encoding = Encoding.UTF8;
                webClient.Headers.Add("fSessionId", SessionID);
                webClient.Headers.Add("Content-Type", "application/json");
                clsGeneric.writeLog("Download URL: " + "http://localhost/Focus8API/Screen/Transactions/" + vtype + "/" + docNo);
                var resDoc = webClient.DownloadString("http://localhost/Focus8API/Screen/Transactions/" + vtype + "/" + docNo);
                clsGeneric.writeLog("Response of Focus Fields Data " + resDoc);
                if (resDoc != null)
                {
                    var resDataDoc = JsonConvert.DeserializeObject<APIResponse.PostResponse>(resDoc);
                    if (resDataDoc.result == -1)
                    {
                        //return Json(new { status = false, data = new { message = resDataDoc.message } });
                    }
                    else
                    {
                        if (resDataDoc.data.Count != 0)
                        {
                            extHeader = JsonConvert.DeserializeObject<Hashtable>(JsonConvert.SerializeObject(resDataDoc.data[0]["Header"]));
                            if (resDataDoc.data[0]["Footer"].ToString() != "[]")
                            {
                                extFooter = JsonConvert.DeserializeObject<List<Hashtable>>(JsonConvert.SerializeObject(resDataDoc.data[0]["Footer"]));
                            }
                            extBody = JsonConvert.DeserializeObject<List<Hashtable>>(JsonConvert.SerializeObject(resDataDoc.data[0]["Body"]));

                            bool iApproved = Convert.ToBoolean(JsonConvert.DeserializeObject<Hashtable>(JsonConvert.SerializeObject(extHeader["Flags"]))["Approved"]);
                            if (!iApproved)
                            {
                                //return Json(new { status = true, data = new { message = "Source Document is not Authorized" } });
                            }
                        }
                    }
                }
            }

            tag1 = (extHeader["Tag1Name"] == null) ? "" : extHeader["Tag1Name"].ToString(); //extHeader["Tag1Name"].ToString();
            tag2 = (extHeader["Tag2Name"] == null) ? "" : extHeader["Tag2Name"].ToString();  //extHeader["Tag2Name"].ToString();
            faTag = (extHeader["FATagName"] == null) ? "" : extHeader["FATagName"].ToString();  //extHeader["FATagName"].ToString();




            strlist = tag1.Split(spearator);
            if (strlist[0].Equals("H"))
            {
                //strlist[1].ToString() = profitcenter
                //headerData.Add(strlist[1].ToString(), extHeader[strlist[1]].ToString());
            }
            else
            {
                if (!tag1.Equals(""))
                {
                    Body.Add(strlist[1].ToString(), extBody[0][strlist[1]].ToString());
                }

            }

            strlist = tag2.Split(spearator);
            if (strlist[0].Equals("H"))
            {
                //strlist[1].ToString() = profitcenter
                //headerData.Add(strlist[1].ToString(), extHeader[strlist[1]].ToString());
            }
            else
            {
                if (!tag2.Equals(""))
                {
                    Body.Add(strlist[1].ToString(), extBody[0][strlist[1]].ToString());
                }
            }

            //FATagName replace of Department__Id
            strlist = faTag.Split(spearator);
            if (strlist[0].Equals("H"))
            {
                //strlist[1].ToString() = profitcenter
                //headerData.Add(strlist[1].ToString(), extHeader[strlist[1]].ToString());
            }
            else
            {
                if (!faTag.Equals(""))
                {
                    Body.Add(strlist[1].ToString(), extBody[0][strlist[1]].ToString());
                }
            }
        }

        static string getStockQty(int RMId, int Warehouse, int DocDate, int CompanyId)
        {
            BL_DB DataAcesslayer = new BL_DB();
            string strErrorMessage = string.Empty;
            string strValue = "";
            strValue = $@"select sum(fQiss+fQrec) as qty from vCore_ibals_0 where iProduct=" + reqrmid + " and iInvtag=" + vWarehoseID + " and idate<=" + iDocDate;
            DataSet ds4 = DataAcesslayer.GetData(strValue, CompanyId, ref strErrorMessage);

            clsGeneric.writeLog("StockQty:" + strValue);
            if (ds4 != null)
            {
                bool b1 = string.IsNullOrEmpty(ds4.Tables[0].Rows[0][0].ToString());
                if (!b1)
                    return Convert.ToString(ds4.Tables[0].Rows[0][0]);
            }
            return "0";
        }
        static string getLinkQty(int tRMId, int tBookno, int tDocDate, int tCompanyId)
        {

            string strErrorMessage = string.Empty;
            string strValue = "";
            BL_DB DataAcesslayer = new BL_DB();

            strValue = $@"Select sum(balance) from (SELECT SUM(vCore_Links" + linkId + "_0.Balance)[Balance] FROM tCore_Header_0" +
                " JOIN cCore_Vouchers_0 with (ReadUncommitted) ON cCore_Vouchers_0.iVoucherType = tCore_Header_0.iVoucherType " +
                " JOIN mSec_Users EUser with (ReadUncommitted) ON EUser.iUserId = tCore_Header_0.iUserId " +
                " JOIN mSec_Users MUser with (ReadUncommitted) ON MUser.iUserId = tCore_Header_0.iModifiedBy" +
                " JOIN tCore_Data_0 ON tCore_Data_0.iHeaderId = tCore_Header_0.iHeaderId " +
                " JOIN tCore_Indta_0 ON tCore_Indta_0.iBodyId = tCore_Data_0.iBodyId " +
                " JOIN dbo.tCore_HeaderData" + vGRNJWType + "_0 ON dbo.tCore_Header_0.iHeaderId = dbo.tCore_HeaderData" + vGRNJWType + "_0.iHeaderId " +
                " join vCore_Links" + linkId + "_0 on vCore_Links" + linkId + "_0.iRefId = tCore_Data_0.iTransactionId " +
                " WHERE tCore_Header_0.iVoucherType =" + vGRNJWType + " AND bVersion = 0 AND bClosed = 0 GROUP BY tCore_Header_0.iHeaderId,tCore_Header_0.iDate, tCore_Header_0.sVoucherNo, " +
                " tCore_Header_0.iCreatedDate,dbo.tCore_Data_0.iTransactionId,dbo.tCore_Data_0.iInvTag, dbo.tCore_Data_0.iBookNo,dbo.tCore_Indta_0.iProduct,dbo.vCore_Links" + linkId + "_0.iRefId," +
                " tCore_Header_0.iModifiedDate, tCore_Header_0.iCreatedTime, tCore_Header_0.iModifiedTime, tCore_Header_0.bSuspended, tCore_Header_0.bCancelled,iAuth HAVING " +
                " SUM(vCore_Links" + linkId + "_0.Balance) <> 0 and dbo.tCore_Indta_0.iProduct=" + tRMId + " and tCore_Header_0.iDate<=" + tDocDate +
                " And dbo.tCore_Data_0.iBookNo=" + tBookno + ") as ab ";

            clsGeneric.writeLog("LinkQty:" + strValue);
            DataSet ds = DataAcesslayer.GetData(strValue, tCompanyId, ref strErrorMessage);
            linkrmqty = 0;
            if (ds != null)
            {

                linkrmqty = Convert.ToDecimal(ds.Tables[0].Rows[0][0]);

                return linkrmqty.ToString();

            }
            return linkrmqty.ToString();
        }
        static void updateTables(string docNo, string User, int iBookNo, int Docdate, int CompanyId, String SessionId, int Vtype)
        {
            BL_DB DataAcesslayer = new BL_DB();
            string strErrorMessage = string.Empty;
            string strValue = "";
            reqrmid = 0;

            string faTagValue = GetFaTagValue(SessionId, Vtype, docNo);


            strValue = $@"SELECT  Distinct RMId FROM dbo.RMReqQtySAL where (vno = '" + docNo + "') AND ([loggeduser] = '" + User + "')";
            DataSet ds4 = DataAcesslayer.GetData(strValue, CompanyId, ref strErrorMessage);
            if (ds4 != null)
            {
                reqrmid = 0;
                for (int kk = 0; kk < ds4.Tables[0].Rows.Count; kk++)
                {

                    reqrmid = Convert.ToInt32(ds4.Tables[0].Rows[kk][0]);

                    strValue = $@"INSERT INTO linkinfoSAL (iHeaderId,iDate,sVoucherNo,Balance,iTransactionId,iProduct,mrate,iRefId, iInvTag, iBookNo, " +
                                " VendorWarehouse, iVoucherType, ConsumedQty,BatchId,BatchNo,BMfDate,Brate,Process,WorksCenter, Dept,TaxCode,DCNo, DCDate,vno, loggeduser) SELECT DISTINCT tCore_Header_0.iHeaderId,tCore_Header_0.iDate, tCore_Header_0.sVoucherNo " +
                                ",SUM(vCore_Links" + linkId + "_0.Balance)[Balance],dbo.tCore_Data_0.iTransactionId,dbo.tCore_Indta_0.iProduct, dbo.tCore_Indta_0.mRate,dbo.vCore_Links" +
                                 linkId + "_0.iRefId,dbo.tCore_Data_0.iInvTag, dbo.tCore_Data_0.iBookNo,0, dbo.tCore_Header_0.iVoucherType,0,Bt.iBatchId, Bt.sBatchNo, Bt.iMfDate, Bt.fRate,DT.iTag307,null,DT.iTag3002, DT.Itag13, tCore_HeaderData" + vGRNJWType + "_0.DCNo, tCore_HeaderData" + vGRNJWType + "_0.DCDate, '" +
                                 docNo + "','" + User + "' FROM tCore_Header_0 JOIN cCore_Vouchers_0 with (ReadUncommitted) ON cCore_Vouchers_0.iVoucherType = tCore_Header_0.iVoucherType " +
                                 " JOIN mSec_Users EUser with (ReadUncommitted) ON EUser.iUserId = tCore_Header_0.iUserId JOIN mSec_Users MUser with (ReadUncommitted) ON MUser.iUserId = " +
                                 " tCore_Header_0.iModifiedBy JOIN tCore_Data_0 ON tCore_Data_0.iHeaderId = tCore_Header_0.iHeaderId " +
                                 " JOIN tCore_Indta_0 ON tCore_Indta_0.iBodyId = tCore_Data_0.iBodyId JOIN dbo.tCore_HeaderData" + vGRNJWType + "_0 ON dbo.tCore_Header_0.iHeaderId = dbo.tCore_HeaderData" + vGRNJWType + "_0.iHeaderId " +
                                " join vCore_Links" + linkId + "_0 on vCore_Links" + linkId + "_0.iRefId = tCore_Data_0.iTransactionId " +
                                " INNER JOIN dbo.tCore_Batch_0 AS Bt ON dbo.tCore_Data_0.iBodyId = Bt.iBodyId " +
                                "INNER JOIN dbo.tCore_Data_Tags_0 AS DT ON dbo.tCore_Data_0.iBodyId = DT.iBodyId " +
                                " WHERE tCore_Header_0.iVoucherType =" + vGRNJWType + " AND (tCore_Data_0.iFaTag = " + faTagValue + ") AND bVersion = 0 AND bClosed = 0  GROUP BY tCore_Header_0.iHeaderId,tCore_Header_0.iDate, tCore_Header_0.sVoucherNo," +
                                " dbo.tCore_Indta_0.mRate,tCore_Header_0.iCreatedDate,dbo.tCore_Data_0.iTransactionId,dbo.tCore_Data_0.iInvTag, dbo.tCore_Data_0.iBookNo " +
                                ",dbo.tCore_Indta_0.iProduct,dbo.vCore_Links" + linkId + "_0.iRefId, dbo.tCore_Header_0.iVoucherType" +
                                ",tCore_Header_0.iModifiedDate, tCore_Header_0.iCreatedTime, tCore_Header_0.iModifiedTime, tCore_Header_0.bSuspended, tCore_Header_0.bCancelled,iAuth,Bt.iBatchId, Bt.sBatchNo, Bt.iMfDate, Bt.fRate,DT.iTag307,DT.iTag3002, DT.Itag13, tCore_HeaderData" + vGRNJWType + "_0.DCNo, tCore_HeaderData" + vGRNJWType + "_0.DCDate " +
                                " HAVING SUM(vCore_Links" + linkId + "_0.Balance) <> 0 and tCore_Header_0.iDate<=" + Docdate +
                                " and dbo.tCore_Indta_0.iProduct=" + reqrmid + " And dbo.tCore_Data_0.iBookNo=" + iBookNo +
                                " ORDER BY tCore_Header_0.iDate DESC, dbo.tCore_Data_0.iTransactionId asc";

                    DataAcesslayer.GetExecute(strValue, CompanyId, ref strErrorMessage);



                }

                // Loop linkinfo table and update Consumed Qty 
                balrmqty = 0;
                reqrmqty = 0;
                Consumed_RM_Qty = 0;
                DC_RM_Bal_Qty = 0;
                reqrmid = 0;
                strValue = $@"SELECT RMQty, RMId,batchid,batchname,bMfDate,bfrate,FgId,GRNJW_FGQty  FROM dbo.RMReqQtySAL where (vno = '" + docNo + "') AND ([loggeduser] = '" + User + "')";
                DataSet ds5 = DataAcesslayer.GetData(strValue, CompanyId, ref strErrorMessage);

                if (ds5 != null)
                {
                    for (int kkk = 0; kkk < ds5.Tables[0].Rows.Count; kkk++)
                    {
                        bool b4 = string.IsNullOrEmpty(ds5.Tables[0].Rows[kkk][6].ToString());
                        if (!b4)
                            fgid = Convert.ToInt32(ds5.Tables[0].Rows[kkk][6]);
                        bool b5 = string.IsNullOrEmpty(ds5.Tables[0].Rows[kkk][7].ToString());
                        if (!b5)
                            fgqty = Convert.ToDecimal(ds5.Tables[0].Rows[kkk][7]);

                        reqrmqty = 0;
                        bool b3 = string.IsNullOrEmpty(ds5.Tables[0].Rows[kkk][0].ToString());
                        if (!b3)
                            reqrmqty = Convert.ToDecimal(ds5.Tables[0].Rows[kkk][0]);
                        reqrmid = Convert.ToInt32(ds5.Tables[0].Rows[kkk][1]);
                        strValue = $@"Select liid,iHeaderId,iDate,sVoucherNo,(Balance-ConsumedQty) as Qty,iTransactionId,iProduct,iRefId, iInvTag, iBookNo, VendorWarehouse, iVoucherType, ConsumedQty from linkinfoSAL  where (Balance-ConsumedQty) <> 0 and iProduct=" + reqrmid + " and (vno = '" + docNo + "') AND ([loggeduser] = '" + User + "') ORDER BY iDate";
                        DataSet ds6 = DataAcesslayer.GetData(strValue, CompanyId, ref strErrorMessage);
                        for (int kkk1 = 0; kkk1 < ds6.Tables[0].Rows.Count; kkk1++)
                        {
                            if (reqrmqty == 0)
                            {
                                break;
                            }

                            //if (reqrmqty != 0)
                            //{
                            liid = Convert.ToInt32(ds6.Tables[0].Rows[kkk1][0]);

                            bool b41 = string.IsNullOrEmpty(ds6.Tables[0].Rows[kkk1][4].ToString());
                            if (!b41)
                                DC_RM_Qty = Convert.ToDecimal(ds6.Tables[0].Rows[kkk1][4]);
                            bool b51 = string.IsNullOrEmpty(ds6.Tables[0].Rows[kkk1][12].ToString());
                            if (!b51)
                                DC_ConsumedQty = Convert.ToDecimal(ds6.Tables[0].Rows[kkk1][12]);
                            if (DC_RM_Qty <= reqrmqty)
                            {
                                balrmqty = reqrmqty - DC_RM_Qty;
                                Consumed_RM_Qty = DC_RM_Qty;
                                DC_RM_Bal_Qty = 0;
                            }
                            else
                            {
                                DC_RM_Bal_Qty = DC_RM_Qty - reqrmqty;
                                Consumed_RM_Qty = reqrmqty;
                                balrmqty = 0;
                                reqrmqty = 0;
                            }
                            strValue = $@"INSERT INTO linkrmusedSAL (liid,UsedQty,RunningLDocQty,fgid,fgqty,vno, loggeduser) values (" + liid + "," + Consumed_RM_Qty + "," + DC_RM_Qty + "," + fgid + "," + fgqty + ",'" + docNo + "','" + User + "')";
                            DataAcesslayer.GetExecute(strValue, CompanyId, ref strErrorMessage);
                            strValue = $@"update linkinfoSAL set ConsumedQty = ConsumedQty + " + Consumed_RM_Qty + " where liid=" + liid;
                            DataAcesslayer.GetExecute(strValue, CompanyId, ref strErrorMessage);
                            reqrmqty = balrmqty;
                            //}


                        }
                    }
                }


            }
        }

        static void updateRMStkValue(string docNo, int vType, int CompanyId)
        {
            BL_DB DataAcesslayer = new BL_DB();
            string strErrorMessage = string.Empty;
            string strqry;
            string strValue;
            int itemId = 0;
            string itemName = string.Empty;
            decimal StkValue;

            //strValue = $@"select iVoucherType from  [dbo].[cCore_Vouchers_0] where sAbbr = '" + VendAnnx_Abbr + "'";
            //VendAnnx_vType  = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, strValue));
            strqry = $@"SELECT  CI.iProduct, CD.iBodyId  FROM  dbo.tCore_Header_0 AS CH INNER JOIN  dbo.tCore_Data_0 AS CD ON CH.iHeaderId = CD.iHeaderId INNER JOIN  dbo.tCore_Indta_0 AS CI ON CD.iBodyId = CI.iBodyId " +
                " WHERE (CH.iVoucherType =" + vType + ") AND (CH.sVoucherNo = N'" + docNo + "')";
            DataSet ds4 = DataAcesslayer.GetData(strqry, CompanyId, ref strErrorMessage);
            if (ds4 != null)
            {
                reqrmid = 0;
                for (int kk = 0; kk < ds4.Tables[0].Rows.Count; kk++)
                {

                    itemId = Convert.ToInt32(ds4.Tables[0].Rows[kk][0]);
                    itemName = clsGeneric.ShowRecord(CompanyId, "select sname from mCore_Product where iMasterId=" + itemId);
                    strValue = $@"SELECT SUM(Ci.mGross) AS Gross FROM dbo.tCore_Header_0 AS CH " +
                        " INNER JOIN dbo.tCore_HeaderData6151_0 AS CHD ON CH.iHeaderId = CHD.iHeaderId " +
                        " INNER JOIN dbo.tCore_Data_0 ON CH.iHeaderId = dbo.tCore_Data_0.iHeaderId " +
                        " INNER JOIN dbo.tCore_Indta_0 AS Ci ON dbo.tCore_Data_0.iBodyId = Ci.iBodyId " +
                        " INNER JOIN dbo.tCore_Data" + VendAnnx_vType + "_0 AS CD ON dbo.tCore_Data_0.iBodyId = CD.iBodyId " +
                        " WHERE (CH.iVoucherType=" + VendAnnx_vType + ") AND (CHD.GRNJWNo = N'" + docNo + "')" +
                    "GROUP BY CD.GRJItemName HAVING(CD.GRJItemName = N'" + itemName + "')";
                    StkValue = Convert.ToDecimal(clsGeneric.ShowRecord(CompanyId, strValue));
                    strValue = $@"update tCore_IndtaBodyScreenData_0 set mInput15=" + StkValue + ", mVal15=" + StkValue + " where iBodyId=" + Convert.ToInt32(ds4.Tables[0].Rows[kk][1]);
                    DataAcesslayer.GetExecute(strValue, CompanyId, ref strErrorMessage);
                }
            }
        }

        public ActionResult DeleteDCJWSal(int CompanyId, string SessionId, string User, int LoginId, int vtype, string docNo, List<PEBody> BodyData)
        {
            String pDocNo = "";
            String sDocNo = "";
            vPAnxJWType = 0;
            clsGeneric.writeLog(" ------------------ DeleteDCJWSal starts here -------------------");
            clsGeneric.writeLog("fSessionId :" + SessionId);

            vPAnxJWAbbr = "AnxJWSale";
            strValue = $@"select iVoucherType from  [dbo].[cCore_Vouchers_0] where sAbbr = '" + vPAnxJWAbbr + "'";
            vPAnxJWType = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, strValue));
            strValue = $@"select Sname from  [dbo].[cCore_Vouchers_0] where sAbbr = '" + vPAnxJWAbbr + "'";
            vPAnxJWName = Convert.ToString(clsGeneric.ShowRecord(CompanyId, strValue));

            vPCustScrpAbbr = "CustScrap";
            strValue = $@"select iVoucherType from  [dbo].[cCore_Vouchers_0] where sAbbr = '" + vPCustScrpAbbr + "'";
            vPCustScrpType = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, strValue));
            strValue = $@"select Sname from  [dbo].[cCore_Vouchers_0] where sAbbr = '" + vPCustScrpAbbr + "'";
            vPCustScrpName = Convert.ToString(clsGeneric.ShowRecord(CompanyId, strValue));

            strValue = $@"SELECT CH.sVoucherNo FROM dbo.tCore_Header_0 AS CH INNER JOIN " +
            "dbo.tCore_HeaderData" + vPAnxJWType + "_0 AS CHD ON CH.iHeaderId = CHD.iHeaderId WHERE(CHD.DCJWNo= N'" + docNo + "')";
            pDocNo = Convert.ToString(clsGeneric.ShowRecord(CompanyId, strValue));

            strValue = $@"SELECT CH.sVoucherNo FROM dbo.tCore_Header_0 AS CH INNER JOIN " +
            "dbo.tCore_HeaderData" + vPCustScrpType + "_0 AS CHD ON CH.iHeaderId = CHD.iHeaderId WHERE(CHD.DCJWNo= N'" + docNo + "')";
            sDocNo = Convert.ToString(clsGeneric.ShowRecord(CompanyId, strValue));

            BL_DB DataAcesslayer = new BL_DB();

            using (var clientDel_CustScrap = new WebClient())
            {

                clientDel_CustScrap.Encoding = Encoding.UTF8;
                clientDel_CustScrap.Headers.Add("fSessionId", SessionId);
                clientDel_CustScrap.Headers.Add("Content-Type", "application/json");
                string url1 = "http://localhost/Focus8API/Transactions/" + vPCustScrpName + "/" + sDocNo;
                clsGeneric.writeLog("url :" + url1);
                var responseDel_CustScrap = clientDel_CustScrap.UploadString(url1, "DELETE", "");
                clsGeneric.writeLog("response of Del_VenScrap :" + responseDel_CustScrap);
                if (responseDel_CustScrap != null)
                {
                    var response_DEL_DataCustScrap = JsonConvert.DeserializeObject<APIResponse.PostResponse>(responseDel_CustScrap);
                    if (response_DEL_DataCustScrap.result == -1)
                    {
                        deleteSGSuccess = 0;

                    }
                    else
                    {

                        deleteSGSuccess++;
                    }
                }
            }

            using (var clientDel_AVJW = new WebClient())
            {

                clientDel_AVJW.Encoding = Encoding.UTF8;

                clientDel_AVJW.Headers.Add("Content-Type", "application/json");
                var responseDel_AVJW = clientDel_AVJW.UploadString("http://localhost/Focus8API/Transactions/" + vPAnxJWName + "/" + pDocNo, "DELETE", "");

                clsGeneric.writeLog("URL form AVJW :" + "http://localhost/Focus8API/Transactions/" + vPAnxJWName + "/" + pDocNo);
                clsGeneric.writeLog("Response form AVJW :" + responseDel_AVJW);
                if (responseDel_AVJW != null)
                {
                    var response_DEL_DataAVJW = JsonConvert.DeserializeObject<APIResponse.PostResponse>(responseDel_AVJW);
                    if (response_DEL_DataAVJW.result == -1)
                    {
                        deleteVASuccess = 0;
                        // return Json(new { status = false, data = new { message = "Vendor Annexure voucher not Found for this GRN No" } });
                    }
                    else
                    {
                        deleteVASuccess++;

                        //return Json(new { status = true, data = new { message = "Deleted Successful" } });
                    }
                }
            }
            if (deleteSGSuccess == 0)
            {
                return Json(new { status = false, data = new { message = "Scrap Generation voucher not Found for this GRN No" } });
            }
            if (deleteVASuccess == 0)
            {
                return Json(new { status = false, data = new { message = "Vendor Annexure voucher not Found for this GRN No" } });
            }

            return Json(new { status = true, data = new { message = "Deleted Successful" } });
        }

        public ActionResult CheckFGQty(int CompanyId, string SessionId, string User, int LoginId, int irowNo, int vtype, string docNo, int docDate, int Branch, int iBookNo, int Dept, int WorksCenter, int Warehouse, List<BodyData> BodyData)
        {
            msg.Clear();
            clsGeneric.writeLog(" ------------------ DCJWSal (CheckFGQty) starts here -------------------");
            clsGeneric.writeLog("fSessionid : " + SessionId);
            //string error = "";
            int Cnt = 0;
            BL_DB objDB = new BL_DB();
            clsGeneric.createTableTempRMFG_DCJWSal(CompanyId, User, docNo);
            string path = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();

            Cnt = BodyData.Count;
            if (Cnt > 0)
            {
                vGRNJWAbbr = "GRNJWSal";
                strValue = $@"select iVoucherType from  [dbo].[cCore_Vouchers_0] where sAbbr = '" + vGRNJWAbbr + "'";
                vGRNJWType = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, strValue));

                vBomAbbr = "SalJWBOM";
                strValue = $@"select iVoucherType from [dbo].[cCore_Vouchers_0]  where sAbbr='" + vBomAbbr + "'";
                vBomType = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, strValue));

                strValue = $@"SELECT distinct max(dbo.tCore_Links_0.iLinkId) " +
                            " FROM dbo.tCore_Header_0 INNER JOIN dbo.tCore_Data_0 ON dbo.tCore_Header_0.iHeaderId = dbo.tCore_Data_0.iHeaderId INNER JOIN " +
                            " dbo.tCore_Indta_0 ON dbo.tCore_Data_0.iBodyId = dbo.tCore_Indta_0.iBodyId INNER JOIN dbo.tCore_Links_0 ON dbo.tCore_Data_0.iTransactionId = dbo.tCore_Links_0.iTransactionId " +
                            " WHERE(dbo.tCore_Header_0.iVoucherType =" + vGRNJWType + ")";

                clsGeneric.writeLog("Query for link  : " + strValue);
                linkId = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, strValue));

                for (int k_CFQ = 0; k_CFQ < Cnt; k_CFQ++)
                {
                    fgqty = 0;
                    fgid = 0;
                    fgid = Convert.ToInt32(BodyData[k_CFQ].Item);
                    fgqty = Convert.ToDecimal(BodyData[k_CFQ].Quantity);
                    if (fgqty != 0)
                    {

                        strValue = "SELECT i.iBodyId, ROW_NUMBER() OVER(ORDER BY FGCode ASC) AS RowID, hd.FGQty, i.iProduct, ABS(i.fQuantity) as fQuantity,cd.ScrapProduct, ABS(cd.ScrapQty) as ScrapQty FROM dbo.tCore_Data_0 AS d INNER JOIN " +
                                        " dbo.tCore_Header_0 AS h ON h.iHeaderId = d.iHeaderId INNER JOIN dbo.tCore_Indta_0 AS i ON i.iBodyId = d.iBodyId INNER JOIN " +
                                        " dbo.tCore_HeaderData" + vBomType + "_0 AS hd ON hd.iHeaderId = h.iHeaderId INNER JOIN " +
                                        " dbo.tCore_Data" + vBomType + "_0 AS cd ON d.iBodyId = cd.iBodyId " +
                                         " WHERE(hd.FGCode = " + fgid + " ) AND(d.iBookNo =" + iBookNo + " )";
                        //clsGeneric.writeLog("Query for FGQty, RMQty  : " + strValue);

                        DataSet ds1 = DataAcesslayer.GetData(strValue, CompanyId, ref strErrorMessage);
                        if (ds1 != null && ds1.Tables[0].Rows.Count != 0)
                        {
                            for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                            {
                                iBodyId = Convert.ToInt32(ds1.Tables[0].Rows[j][0]);
                                iRowID = Convert.ToInt32(ds1.Tables[0].Rows[j][1]);
                                rmid = Convert.ToInt32(ds1.Tables[0].Rows[j][3]);
                                scrapid = Convert.ToInt32(ds1.Tables[0].Rows[j][5]);
                                bool b2 = string.IsNullOrEmpty(ds1.Tables[0].Rows[j][4].ToString());
                                if (!b2)
                                    rmqty = Convert.ToDecimal(ds1.Tables[0].Rows[j][4]);
                                scrapqty = Convert.ToDecimal(ds1.Tables[0].Rows[j][6]);
                                bool b3 = string.IsNullOrEmpty(ds1.Tables[0].Rows[j][2].ToString());
                                if (!b3)
                                    bomfgqty = Convert.ToDecimal(ds1.Tables[0].Rows[j][2]);

                                linkrmqty = Convert.ToDecimal(getLinkQty(rmid, iBookNo, docDate, CompanyId));
                                chkRMexists = 0;
                                strValue = $@"select top 1 count(1)  from TempRMFG_DCJWSal where rmId=" + rmid + " AND loggeduser='" + User + "'";
                                chkRMexists = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, strValue));
                                strValue = $@"select Max(id)  from TempRMFG_DCJWSal where rmId=" + rmid + " AND loggeduser='" + User + "'";
                                chkRMMaxId = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, strValue));


                                if (chkRMexists == 1)
                                {
                                    // strValue = $@"select TOP 1 (balQty) from TempRMFG_GRNJWPur where RMid=" + rmid + " and id<=" + chkRMMaxId + " order by id desc";
                                    // chkRMBalQty = Convert.ToDecimal(clsGeneric.ShowRecord(CompanyId, strValue));
                                    strValue = $@"select Sum(reqQty) from TempRMFG_DCJWSal where RMid=" + rmid + " and id<=" + chkRMMaxId + " AND loggeduser='" + User + "'";
                                    chkReqQtySum = Convert.ToDecimal(clsGeneric.ShowRecord(CompanyId, strValue));
                                    strValue = $@"Insert into TempRMFG_DCJWSal (iRowId,iBodyId,FgId,FgQty,rmid,BomQty,LinkQty,reqQty,RMMaxqty,FGRowNo,GrdRowNo,vno,loggeduser) values('{iRowID}','{iBodyId}','{fgid}' ,{fgqty},{rmid},{rmqty},{linkrmqty},{rmqty * fgqty},{(linkrmqty - chkReqQtySum) / rmqty},{j + 1},{irowNo},'{docNo}','{User}')";
                                    clsGeneric.writeLog("Insert Query  : " + strValue);
                                    int insert = DataAcesslayer.GetExecute(strValue, Convert.ToInt32(CompanyId), ref strErrorMessage);
                                }
                                else
                                {
                                    strValue = $@"Insert into TempRMFG_DCJWSal (iRowId,iBodyId,FgId,FgQty,rmid,BomQty,LinkQty,reqQty,RMMaxqty,FGRowNo,GrdRowNo,vno,loggeduser) values('{iRowID}','{iBodyId}','{fgid}' ,{fgqty},{rmid},{rmqty},{linkrmqty},{rmqty * fgqty},{linkrmqty / rmqty},{j + 1},{irowNo},'{docNo}','{User}')";
                                    clsGeneric.writeLog("Insert Query  : " + strValue);
                                    int insert = DataAcesslayer.GetExecute(strValue, Convert.ToInt32(CompanyId), ref strErrorMessage);
                                }


                            }
                        }
                    }
                }
            }
            return new EmptyResult();
        }

        public ActionResult GetMaxFGLst(int CompanyId, string User, int item, string UnitLocation, string Vendor, string SessionId, int LoginId, int vtype, string DocNo, string iDocDate, int DCQty)
        {
            string strNotepad = "";
            string error = "";
            int iCnt = 0;
            string UserWithSession = "";
            clsGeneric.Log_write(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), SessionId, CompanyId, DocNo, vtype, User);
            UserWithSession = User + "-" + SessionId;
            strQry = $@"delete from ICSDCJWSalLst where loggeduser='" + UserWithSession + "'";
            objDB.GetExecute(strQry, CompanyId, ref error);
            clsGeneric.createICSDCJWSalLst(CompanyId, User, DocNo);
            strQry = $@"exec spICS_DCJWSalMaxFG " + item + "," + DCQty + "," + Vendor + "," + UnitLocation + ",'" + UserWithSession + "'";
            clsGeneric.writeLog("fSessionid : " + strQry);
            DataSet ds = objDB.GetData(strQry, CompanyId, ref strErrorMessage);

            //strQry = $@"select p.sName+SPACE(75-Len(p.sName)) as RMName, dRMBOMQty,dFGBOMQty,dGRNJWSalQty,dAnxJWSaleQty,dRetJWSaleQty,((dGRNJWSalQty - (dAnxJWSaleQty + dRetJWSaleQty))) as BalQty, CONVERT(DECIMAL(18,6),(((dGRNJWSalQty - (dAnxJWSaleQty + dRetJWSaleQty))) / dRMBOMQty)) as MaxFGQTY " +
            //" from ICSDCJWSalLst as d inner join mCore_Product as p On d.iRMId = p.iMasterId where loggeduser='" + UserWithSession + "' " +
            //" order by abs(((dGRNJWSalQty - (dAnxJWSaleQty + dRetJWSaleQty))) / dRMBOMQty) asc ";
            //((dGRNJWSalQty - (dAnxJWSaleQty + dRetJWSaleQty))) as BalQty,

            strQry = $@"select Left(p.sName,50) + SPACE(55 - Len(Left(p.sName,50))) as RMName," +
            " CAST(dRMBOMQty AS VARCHAR(15)) + SPACE(15 - Len(dRMBOMQty)) as dRMBOMQty,dFGBOMQty," +
            " CAST(dGRNJWSalQty AS VARCHAR(15)) + SPACE(15 - Len(dGRNJWSalQty)) as dGRNJWSalQty," +
            " CAST(dAnxJWSaleQty AS VARCHAR(15)) + SPACE(15 - Len(dAnxJWSaleQty)) as dAnxJWSaleQty," +
            " CAST(dRetJWSaleQty AS VARCHAR(15)) + SPACE(15 - Len(dRMBOMQty)) as dRetJWSaleQty," +
            " CAST(((dGRNJWSalQty - (dAnxJWSaleQty + dRetJWSaleQty))) AS VARCHAR(15))  + SPACE(15 - Len(dGRNJWSalQty - (dAnxJWSaleQty + dRetJWSaleQty))) as BalQty," +
            " CONVERT(DECIMAL(18, 2), (((dGRNJWSalQty - (dAnxJWSaleQty + dRetJWSaleQty))) / dRMBOMQty)) as MaxFGQTY" +
            " from ICSDCJWSalLst as d inner join mCore_Product as p On d.iRMId = p.iMasterId where loggeduser='" + UserWithSession + "' " +
            " order by abs(((dGRNJWSalQty - (dAnxJWSaleQty + dRetJWSaleQty))) / dRMBOMQty) asc";
            clsGeneric.writeLog("Query :" + strQry);
            DataSet ds1 = DataAcesslayer.GetData(strQry, CompanyId, ref strErrorMessage);
            clsGeneric.writeLog("Getting from Voucher:" + (ds1));
            iCnt = 0;
            if (ds1 != null)
            {
                DataAcesslayer.GetExecute(strQry, CompanyId, ref strErrorMessage);
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    strNotepad = "RMName__________________________________________________RMBOMQty________FGBOMQty_GRNJWSalQty_____AnxJWSaleQty____RetJWSaleQty____BalQty__________MaxFGQTY \n";
                    for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                    {
                        iCnt = iCnt + 1;
                        strNotepad = strNotepad + Convert.ToString(ds1.Tables[0].Rows[j][0]) + " " + Convert.ToString(ds1.Tables[0].Rows[j][1]) + " " + Convert.ToString(ds1.Tables[0].Rows[j][2]) + "  " + Convert.ToString(ds1.Tables[0].Rows[j][3]) + "  " + Convert.ToString(ds1.Tables[0].Rows[j][4]) + " " + Convert.ToString(ds1.Tables[0].Rows[j][5]) + " " + Convert.ToString(ds1.Tables[0].Rows[j][6]) + " " + Convert.ToString(ds1.Tables[0].Rows[j][7]) + "\n";
                    }
                }
            }
            //    return Json(new { strErrorMessage = "", data = new { message = "Notpad Export Successfully" } });
            //return new EmptyResult();
            string jsonFinal = strNotepad;

            return Json(new { status = true, data = new { json_data = jsonFinal } });
        }
    }
}