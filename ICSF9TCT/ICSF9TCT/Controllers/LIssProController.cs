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
using System.Web.Routing;

namespace ICSF9TCT.Controllers
{
    public class LIssProController : Controller
    {
        #region Variable Definition

        string strQry = string.Empty;
        string strValue = string.Empty;
        string strErrorMessage = string.Empty;
        BL_DB DataAcesslayer = new BL_DB();


        public static int PostingStatus;


        public static int giitemid = 0;
        public static int giUnderQc = 0;

        public static string vBAbbr = "";
        public static string vBName = "";

        public static int bWarehouse__Id = 0;
        public static int bBranch__Id = 0;

        public static int bProd_Id = 0;
        public static int AWarehouse__Id = 0;
        public static int RWarehouse__Id = 0;
        public static int RWWarehouse__Id = 0;
        public static int Is_UnderQc = 0;
        string vAbbr = "";
        string vSname = "";
        public static int bWIPAC__Id = 0;
        public static int bPurchaseAC__Id = 0;
        public static int ibAcctPost1 = 0;
        public static int ibAcctPost2 = 0;
        public static string sRootTagNo = "";
        public static double dRejectedQty = 0;
        public static int iRejectedWH = 0;

        public static int bDate = 0;
        public static decimal bProdQty = 0;
        public static int bProdOrderId = 0;
        public static int iFgId = 0;
        public static decimal bnet = 0;
        public static int biVaraintId = 0;
        public static int bWorksCenter__Id = 0;
        public static int bDept__Id = 0;
        public static decimal bbQty = 0;
        public static int bbItemId = 0;
        public static int bbBatchId = 0;
        public static string bbBatchNo = "";
        public static int bbUnitId = 0;
        public static int bbTransactionId = 0;
        public static decimal bbRate = 0;





        public static decimal ScrapRate = 0;
        public static decimal ScrapValue = 0;
        public static decimal sumofScrapValue = 0;
        public static decimal bbomqty = 0;
        public static decimal bProdrate = 0;
        public static int bMainOPItem = 0;
        public static int vPRptProVtype = 0;
        public static string vPRptProAbbr = "RptPro";
        public static string vPRptProName = "";

        public static int vPPrdOrdVtype = 0;
        public static string vPPrdOrdAbbr = "PrdOrd";
        public static string vPPrdOrdName = "";

        public static int vPostVtype = 0;
        public static string vPostAbbr = "ProdnInwQC";
        public static string vPostName = "";

        int vBType2 = 0;
        string vBAbbr2 = "";
        string vBName2 = "";
        string vBDocNo2 = "";

        int pHDocdate = 0;
        string pHDocNo = "";
        int pHBranch = 0;
        int pHWorksCentre = 0;
        int pHDept = 0;
        int pHVendorAccount = 0;
        int pBWarehouse = 0;
        public int PBItem = 0;
        public int PBTransactionId = 0;
        decimal pBQuantity = 0;

        static string RMName = "";
        
        StringBuilder msg = new StringBuilder();



        #endregion

        [HttpPost]
        public ActionResult LUpdateIssPro(int CompanyId, string SessionId, string User, int LoginId, int bvtype, string bdocNo, int bdocdate, List<PEBody> BodyData)
        {
            try
            {

                clsGeneric.Log_write(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), SessionId, CompanyId, bdocNo, bvtype, User);
                PostingStatus = 0;
                strQry = $@"SELECT CHD.PostingStatus FROM  dbo.tCore_HeaderData" + bvtype + "_0 AS CHD INNER JOIN " +
                    "dbo.tCore_Header_0 AS CH ON CHD.iHeaderId = CH.iHeaderId WHERE (CH.iVoucherType =" + bvtype + ") AND (CH.sVoucherNo = N'" + bdocNo + "')";
                PostingStatus = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, strQry));
                // 0,Pending, 1,Updated
                clsGeneric.writeLog("PostingStatus 0,Pending, 1,Updated  : " + PostingStatus);

                strQry = $@" SELECT TOP (1) mBOM.iProductId FROM dbo.tCore_Header_0 AS CH INNER JOIN " +
                        " dbo.tCore_Data_0 AS CD ON CH.iHeaderId = CD.iHeaderId INNER JOIN " +
                        " dbo.tCore_Header_2_0 AS CHN ON CH.iHeaderId = CHN.iHeaderId INNER JOIN " +
                        " dbo.tMrp_ProdOrder_0 AS MP ON CHN.iProdOrderId = MP.iProdOrderId INNER JOIN " +
                        " dbo.mMRP_BOMBody AS mBOM ON MP.iVaraintId = mBOM.iVariantId INNER JOIN " +
                        " dbo.vmCore_Product AS vmP ON mBOM.iProductId = vmP.iMasterId INNER JOIN " +
                        " dbo.ICS_BOMVariant AS ICSB ON mBOM.iVariantId = ICSB.iVariantId " +
                        " WHERE      (CH.iVoucherType = " + bvtype + ") AND (CH.sVoucherNo = N'" + bdocNo + "') AND (mBOM.bMainOutPut = 1)" +
                        " ORDER BY ICSB.iVersion DESC ";
                giitemid = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, strQry));
                clsGeneric.writeLog("Item  : " + giitemid);

                strQry = $@" SELECT TOP (1) vmW.IsUnderQC FROM dbo.tCore_Header_0 AS CH INNER JOIN " +
                        " dbo.tCore_Data_0 AS CD ON CH.iHeaderId = CD.iHeaderId INNER JOIN " +
                        " dbo.tCore_Header_2_0 AS CHN ON CH.iHeaderId = CHN.iHeaderId INNER JOIN " +
                        " dbo.tMrp_ProdOrder_0 AS MP ON CHN.iProdOrderId = MP.iProdOrderId INNER JOIN " +
                        " dbo.mMRP_BOMBody AS mBOM ON MP.iVaraintId = mBOM.iVariantId INNER JOIN " +
                        " dbo.vmCore_Warehouse AS vmW ON mBOM.iInvTagValue = vmW.iMasterId INNER JOIN " +
                        " dbo.vmCore_Product AS vmP ON mBOM.iProductId = vmP.iMasterId INNER JOIN " +
                        " dbo.vmCore_Department AS vmD ON CD.iFaTag = vmD.iMasterId INNER JOIN " +
                        " dbo.ICS_BOMVariant AS ICSB ON mBOM.iVariantId = ICSB.iVariantId " +
                        " WHERE      (CH.iVoucherType =  " + bvtype + ") AND (CH.sVoucherNo = N'" + bdocNo + "') AND (mBOM.bMainOutPut = 1) " +
                        " ORDER BY ICSB.iVersion DESC ";
                giUnderQc = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, strQry));
                clsGeneric.writeLog("Under Qc  : " + giUnderQc);

                if (PostingStatus == 0)
                {

                    //if (giUnderQc != 0)
                    //{

                    if (blnPostRptPro(CompanyId, SessionId, User, LoginId, bvtype, bdocNo, bdocdate, BodyData))
                    {

                        //if (blnPostProdnInwQC(CompanyId, SessionId, User, LoginId, bvtype, bdocNo, BodyData))
                        //{
                        UpdateStatus(bvtype, bdocNo, 1, CompanyId);
                        return Json(new { status = true, data = new { message = "Posted Successful" } });
                        //}
                        //else
                        //{
                        //    UpdateStatus(bvtype, bdocNo, 0, CompanyId);
                        //    return Json(new { status = false, data = new { message = "Posting Failed" } });
                        //}

                    }
                    else
                    {
                        UpdateStatus(bvtype, bdocNo, 0, CompanyId);
                        return Json(new { status = false, data = new { message = "Posting Failed" } });
                    }
                    //}
                    //else
                    //{

                    //    if (blnPostRptPro(CompanyId, SessionId, User, LoginId, bvtype, bdocNo, BodyData))
                    //    {
                    //        UpdateStatus(bvtype, bdocNo, 1, CompanyId);
                    //        return Json(new { status = true, data = new { message = "Posted Successful" } });
                    //    }
                    //    else
                    //    {
                    //        UpdateStatus(bvtype, bdocNo, 0, CompanyId);
                    //        return Json(new { status = false, data = new { message = "Posting Failed" } });
                    //    }
                    //}
                }
                else
                {
                    return Json(new { status = false, data = new { message = "Posting already performed" } });
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


        }

        public bool blnPostRptPro(int miCompanyId, string msSessionId, string msUser, int msLoginId, int mivtype, string msdocNo, int msdocDate, List<PEBody> mlBodyData)
        {
            //clsGeneric.Log_write(RouteData.Values["controller"].ToString(), "blnPostRptPro", msSessionId, miCompanyId, msdocNo, mivtype, msUser);
            clsGeneric.writeLog("------------------ Function Name :-  " + " blnPostRptPro " + "------------------");
            try
            {
                

                strQry = $@"SELECT  sAbbr  FROM  dbo.cCore_Vouchers_0 WHERE (iVoucherType =" + mivtype + " )";
                vBAbbr = Convert.ToString(clsGeneric.ShowRecord(miCompanyId, strQry));
                strQry = $@"SELECT  sName  FROM  dbo.cCore_Vouchers_0 WHERE (iVoucherType =" + mivtype + " )";
                vBName = Convert.ToString(clsGeneric.ShowRecord(miCompanyId, strQry));

                strQry = $@"SELECT  iVoucherType  FROM  dbo.cCore_Vouchers_0 WHERE (sAbbr ='" + vPRptProAbbr + "')";
                vPRptProVtype = Convert.ToInt32(clsGeneric.ShowRecord(miCompanyId, strQry));
                strQry = $@"SELECT  sName  FROM  dbo.cCore_Vouchers_0 WHERE (iVoucherType ='" + vPRptProVtype + "')";
                vPRptProName = Convert.ToString(clsGeneric.ShowRecord(miCompanyId, strQry));

                strQry = $@"SELECT  iVoucherType  FROM  dbo.cCore_Vouchers_0 WHERE (sAbbr ='" + vPPrdOrdAbbr + "')";
                vPPrdOrdVtype = Convert.ToInt32(clsGeneric.ShowRecord(miCompanyId, strQry));
                strQry = $@"SELECT  sName  FROM  dbo.cCore_Vouchers_0 WHERE (iVoucherType ='" + vPPrdOrdVtype + "')";
                vPPrdOrdName = Convert.ToString(clsGeneric.ShowRecord(miCompanyId, strQry));
                strErrorMessage = "";
                // not require code condition in query
                /*strQry = $@" SELECT DISTINCT CH.iDate, CD.iCode AS PurchaseAccount, CD.iBookNo AS workinPrgAc, CD.iFaTag AS Branch,  " +
                    " CD.iInvTag AS Warehouse, CHN.fProdSize, CHN.iProdOrderId, CH.fNet, CHD.AcctPost1, CHD.AcctPost2,CHD.FinishedGoods,Chd.RootTagNo,Chd.RejectedQty, CHD.RejectedWH" +
                    " FROM dbo.tCore_Header_0 AS CH INNER JOIN dbo.tCore_Data_0 AS CD ON CH.iHeaderId = CD.iHeaderId INNER JOIN " +
                    " dbo.tCore_Header_2_0 AS CHN ON CH.iHeaderId = CHN.iHeaderId " +
                    " INNER JOIN  dbo.tCore_HeaderData" + mivtype + "_0 AS CHD ON CH.iHeaderId = CHD.iHeaderId WHERE(CH.iVoucherType = " + mivtype + ") AND(CH.sVoucherNo = N'" + msdocNo + "') and CD.iCode <> 0";*/

                strQry = $@" SELECT DISTINCT CH.iDate, CD.iCode AS PurchaseAccount, CD.iBookNo AS workinPrgAc, CD.iFaTag AS Branch,  " +
                    " CD.iInvTag AS Warehouse, CHN.fProdSize, CHN.iProdOrderId, CH.fNet, CHD.AcctPost1, CHD.AcctPost2,CHD.FinishedGoods,Chd.RootTagNo,Chd.RejectedQty, CHD.RejectedWH" +
                    " FROM dbo.tCore_Header_0 AS CH INNER JOIN dbo.tCore_Data_0 AS CD ON CH.iHeaderId = CD.iHeaderId INNER JOIN " +
                    " dbo.tCore_Header_2_0 AS CHN ON CH.iHeaderId = CHN.iHeaderId " +
                    " INNER JOIN  dbo.tCore_HeaderData" + mivtype + "_0 AS CHD ON CH.iHeaderId = CHD.iHeaderId WHERE(CH.iVoucherType = " + mivtype + ") AND(CH.sVoucherNo = N'" + msdocNo + "') ";



                clsGeneric.writeLog("strQry:" + strQry);
                DataSet ds = DataAcesslayer.GetData(strQry, miCompanyId, ref strErrorMessage);
                clsGeneric.writeLog("Getting from Voucher:" + (ds));
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        bDate = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                        bWIPAC__Id = Convert.ToInt32(ds.Tables[0].Rows[0][1]);
                        bPurchaseAC__Id = Convert.ToInt32(ds.Tables[0].Rows[0][2]);
                        //bPurchaseAC__Id = Convert.ToInt32(ds.Tables[0].Rows[0][1]);
                        //bWIPAC__Id = Convert.ToInt32(ds.Tables[0].Rows[0][2]);
                        bBranch__Id = Convert.ToInt32(ds.Tables[0].Rows[0][3]);
                        bWarehouse__Id = Convert.ToInt32(ds.Tables[0].Rows[0][4]);
                        bProdQty = Convert.ToDecimal(ds.Tables[0].Rows[0][5]);
                        bProdOrderId = Convert.ToInt32(ds.Tables[0].Rows[0][6]);
                        iFgId = Convert.ToInt32(ds.Tables[0].Rows[0][10]);

                        bnet = Math.Abs(Convert.ToDecimal(ds.Tables[0].Rows[0][7]));
                        //bDept__Id = Convert.ToInt32(ds.Tables[0].Rows[0][8]);
                        //bWorksCenter__Id = Convert.ToInt32(ds.Tables[0].Rows[0][9]);
                        ibAcctPost1 = Convert.ToInt32(ds.Tables[0].Rows[0][8]);
                        ibAcctPost2 = Convert.ToInt32(ds.Tables[0].Rows[0][9]);
                        sRootTagNo= ds.Tables[0].Rows[0][11].ToString();
                        dRejectedQty = Convert.ToDouble(ds.Tables[0].Rows[0][12]);
                        iRejectedWH = Convert.ToInt32(ds.Tables[0].Rows[0][13]);

                    } // end if (ds.Tables[0].Rows.Count > 0)

                    HashData objHashRequest_RptPro = new HashData();
                    Hashtable headerRptPro = new Hashtable();
                    headerRptPro.Add("DocNo", msdocNo);
                    headerRptPro.Add("Date", bDate);

                    strQry = $@"Select iItem from tMrp_ProdOrderBody_0 where iProdOrderId=" + bProdOrderId;
                    iFgId = Convert.ToInt32(clsGeneric.ShowRecord(miCompanyId, strQry));

                    strQry = $@"select iWIPAccount from muCore_Product_OtherDetails where iMasterId=" + iFgId;
                    bWIPAC__Id = Convert.ToInt32(clsGeneric.ShowRecord(miCompanyId, strQry));

                    //headerRptPro.Add("PurchaseAC__Id", bPurchaseAC__Id);
                    //headerRptPro.Add("PurchaseAC__Id", ibAcctPost1);

                    //Product - Other details - WIP Account 
                    headerRptPro.Add("WIPAC__Id", bWIPAC__Id);
                    //headerRptPro.Add("WIPAC__Id", ibAcctPost2);
                    headerRptPro.Add("BatchNo", bProdOrderId);
                    // Change By Azhar @ 22-02-2023 @ 18-38
                    //headerRptPro.Add("Department__Id", bBranch__Id);
                    headerRptPro.Add("Unit Location__Id", bBranch__Id);
                    
                    //headerRptPro.Add("Works Center__Id", bWorksCenter__Id);
                    headerRptPro.Add("Warehouse__Id", bWarehouse__Id);
                    //headerRptPro.Add("ProdOrderId", bProdOrderId);
                    headerRptPro.Add("ProdQty", bProdQty);
                    headerRptPro.Add("RootTagNo", sRootTagNo);

                    List<System.Collections.Hashtable> lstBody_RptPro = new List<System.Collections.Hashtable>();
                    lstBody_RptPro.Clear();
                    strQry = $@" SELECT iDate, iDueDate, sRemarks, iOrderStatus," +
                       " ISNULL(iVaraintId, 0)iVaraintId, iProdOrderId, sProdOrderNo, iWareHouseId, ISNULL(iIssueType, 0)iIssueType, ISNULL(sSONO, '')sSONO, " +
                       " ISNULL(iCustomer, 0)iCustomer, ISNULL(sSpecialInstruction, '')sSpecialInstruction," +
                       " isnull(iTagFilterId,0) iTagFilterId,isnull(iTagFilterValue,0) iTagFilterValue,isnull(sBatchNo,'') sBatchNo " +
                       " FROM tMrp_ProdOrder_0 with(readuncommitted) WHERE iProdOrderId =" + bProdOrderId;

                    DataSet ds1 = DataAcesslayer.GetData(strQry, miCompanyId, ref strErrorMessage);
                    clsGeneric.writeLog("Getting from Voucher:" + (ds1));
                    if (ds1 != null)
                    {
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            biVaraintId = Convert.ToInt32(ds1.Tables[0].Rows[0][4]);
                            //strQry = $@" SELECT  BB.iInvTagValue FROM dbo.mMRP_BomHeader AS BH INNER JOIN dbo.mMRP_BOMBody AS BB ON BH.iBomId = BB.iVariantId " +
                            //" INNER JOIN dbo.ICS_BOMVariant AS vb ON BB.iVariantId = vb.iVariantId WHERE (BH.iBomId = " + biVaraintId + ") AND (BB.bInput = 0) and (BB.bMainOutPut=1)";
                            //bWarehouse__Id= Convert.ToInt32(clsGeneric.ShowRecord(miCompanyId, strQry));
                            //headerRptPro.Add("Warehouse__Id", bWarehouse__Id);
                        }
                        strQry = $@" SELECT BH.iBomId, BB.bMainOutPut, BB.bInput, BB.iBomBodyId, BB.iProductId, BB.fQty, BB.iUnit, " +
                      " BB.iInvTagValue AS Warehouse, BB.iRowIndex, BB.fRate, vb.iTagValue AS Dept " +
                      " FROM dbo.mMRP_BomHeader AS BH INNER JOIN dbo.mMRP_BOMBody AS BB ON BH.iBomId = BB.iVariantId " +
                      " INNER JOIN dbo.ICS_BOMVariant AS vb ON BB.iVariantId = vb.iVariantId " +
                      " WHERE (BH.iBomId = " + biVaraintId + ") AND (BB.bInput = 0)";
                        sumofScrapValue = 0;
                        DataSet ds2 = DataAcesslayer.GetData(strQry, miCompanyId, ref strErrorMessage);
                        clsGeneric.writeLog("Getting from Voucher:" + (ds2));
                       
                        if (ds2 != null)
                        {
                            if (ds2.Tables[0].Rows.Count > 0)
                            {

                                for (int j = 0; j < ds2.Tables[0].Rows.Count; j++)
                                {
                                    
                                    ScrapRate = 0;
                                    ScrapValue = 0;
                                    // Scrap Rate Getting 
                                    bbomqty = Math.Abs(Convert.ToDecimal(ds2.Tables[0].Rows[j][5]));
                                    if (Convert.ToInt32(ds2.Tables[0].Rows[j][1]) == 0 && Convert.ToInt32(ds2.Tables[0].Rows[j][2]) == 0)
                                    {
                                        strQry = $@"Select ScrapRate from muCore_Product_Settings where iMasterId=" + Convert.ToInt32(ds2.Tables[0].Rows[j][4]);
                                        ScrapRate = Math.Abs(Convert.ToDecimal(clsGeneric.ShowRecord(miCompanyId, strQry)));
                                        ScrapValue = ScrapRate * (bProdQty * bbomqty);
                                        sumofScrapValue = sumofScrapValue + ScrapValue;
                                    } // end if (Convert.ToInt32(ds2.Tables[0].Rows[j][1]) == 0 && Convert.ToInt32(ds2.Tables[0].Rows[j][2]) == 0)
                                    else
                                    {
                                        bMainOPItem = Convert.ToInt32(ds2.Tables[0].Rows[j][4]);
                                    }






                                } // end for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                                bProdrate = (bnet - sumofScrapValue) / bProdQty;
                                
                                for (int k1 = 0; k1 < ds2.Tables[0].Rows.Count; k1++)
                                {
                                    bbomqty = Math.Abs(Convert.ToDecimal(ds2.Tables[0].Rows[k1][5]));
                                    if (Convert.ToInt32(ds2.Tables[0].Rows[k1][1]) == 0 && Convert.ToInt32(ds2.Tables[0].Rows[k1][2]) == 0)
                                    {
                                        strQry = $@"Select ScrapRate from muCore_Product_Settings where iMasterId=" + Convert.ToInt32(ds2.Tables[0].Rows[k1][4]);
                                        ScrapRate = Math.Abs(Convert.ToDecimal(clsGeneric.ShowRecord(miCompanyId, strQry)));
                                        bProdrate = ScrapRate;

                                    }
                                    Hashtable bodyRptPro = new Hashtable();
                                    //Product - Other details - Stock account (Body)
                                    strQry = $@"select iStocksAccount from muCore_Product_OtherDetails where iMasterId=" + Convert.ToInt32(ds2.Tables[0].Rows[k1][4]);
                                    bPurchaseAC__Id = Convert.ToInt32(clsGeneric.ShowRecord(miCompanyId, strQry));

                                    bodyRptPro.Add("Account__Id", bPurchaseAC__Id);
                                    bodyRptPro.Add("Warehouse__Id", Convert.ToInt32(ds2.Tables[0].Rows[k1][7]));
                                    bodyRptPro.Add("Item__Id", Convert.ToInt32(ds2.Tables[0].Rows[k1][4]));
                                    bodyRptPro.Add("Unit__Id", Convert.ToInt32(ds2.Tables[0].Rows[k1][6]));
                                    bodyRptPro.Add("RejectedWH__Id", iRejectedWH);
                                    bodyRptPro.Add("BaseDocNo", msdocNo);
                                    bodyRptPro.Add("BaseDocDate", msdocDate);

                                    // Change suggested by Amair Sir @16-04-2022 Format two decimal 
                                    bodyRptPro.Add("Quantity", clsGeneric.DecimalCustomFormat(Convert.ToDouble(bProdQty * bbomqty)));

                                    bodyRptPro.Add("Rate", bProdrate);

                                    Hashtable bodyBatchRptPro = new Hashtable
                                        {
                                            {"BatchNo",  msdocNo + "/" + Convert.ToInt32( k1 + 1 ) },
                                            {"MfgDate", Convert.ToInt32(bDate)},
                                            {"BatchRate",  bProdrate},
                                            // Change suggested by Amair Sir @16-04-2022 Format two decimal 
                                            {"Qty", clsGeneric.DecimalCustomFormat(Convert.ToDouble(bProdQty * bbomqty))}


                                        };
                                    
                                        
                                   
                                    Hashtable bodyRejectedRptPro = new Hashtable
                                    {
                                        {"Input",  dRejectedQty},
                                        {"FieldId", 2},
                                        {"ColMap",  0},
                                        {"Value", dRejectedQty}

                                    };
                                    
                                    

                                    List<System.Collections.Hashtable> lstBatch = new List<System.Collections.Hashtable>();
                                    lstBatch.Add(bodyBatchRptPro);
                                    List<System.Collections.Hashtable> lstRejected = new List<System.Collections.Hashtable>();
                                    lstRejected.Add(bodyRejectedRptPro);
                                    bodyRptPro.Add("Batch", bodyBatchRptPro);
                                    bodyRptPro.Add("Rejected Qty", bodyRejectedRptPro);
                                    dRejectedQty = 0;
                                    lstBody_RptPro.Add(bodyRptPro);
                                    
                                }


                            } //end if (ds2.Tables[0].Rows.Count > 0)
                        } // end if (ds2 != null)
                    } // end if (ds1 != null)

                    System.Collections.Hashtable objHash_RptPro = new System.Collections.Hashtable();
                    objHash_RptPro.Add("Body", lstBody_RptPro);
                    objHash_RptPro.Add("Header", headerRptPro);

                    List<System.Collections.Hashtable> lstHash_RptPro = new List<System.Collections.Hashtable>();
                    lstHash_RptPro.Add(objHash_RptPro);
                    objHashRequest_RptPro.data = lstHash_RptPro;
                    string sContent_RptPro = JsonConvert.SerializeObject(objHashRequest_RptPro);
                    clsGeneric.writeLog("Upload RptPro :" + "http://localhost/Focus8API/Transactions/Vouchers/" + vPRptProName);
                    clsGeneric.writeLog("URL Param :" + sContent_RptPro);
                    using (var clientDel_RptPro = new WebClient())
                    {

                        clientDel_RptPro.Encoding = Encoding.UTF8;
                        clientDel_RptPro.Headers.Add("fSessionId", msSessionId);
                        clientDel_RptPro.Headers.Add("Content-Type", "application/json");
                        string url = "http://localhost/Focus8API/Transactions/" + vPRptProName + "/" + msdocNo;
                        clsGeneric.writeLog("url  Delete RptPro : " + url);
                        var responseDel_RptPro = clientDel_RptPro.UploadString(url, "DELETE", "");
                        clsGeneric.writeLog("Response form Delete RptPro :" + (responseDel_RptPro));
                    }
                    clsGeneric.writeLog("Upload URL Of RptPro :" + ("http://localhost/Focus8API/Transactions/Vouchers/" + vPRptProName));
                    using (var clientRptPro = new WebClient())
                    {
                        clientRptPro.Encoding = Encoding.UTF8;
                        clientRptPro.Headers.Add("fSessionId", msSessionId);
                        clientRptPro.Headers.Add("Content-Type", "application/json");

                        var responseRptPro = clientRptPro.UploadString("http://localhost/Focus8API/Transactions/Vouchers/" + vPRptProName, sContent_RptPro);
                        clsGeneric.writeLog("Response form RptPro :" + (responseRptPro));
                        if (responseRptPro != null)
                        {
                            var responseDataRptPro = JsonConvert.DeserializeObject<APIResponse.PostResponse>(responseRptPro);
                            if (responseDataRptPro.result == -1)
                            {
                                //UpdateStatus(mivtype, msdocNo, 0, miCompanyId);
                                //return Json(new { status = false, data = new { message = responseDataRptPro.message } });
                                return false;
                            }
                            else
                            {
                                var a = Math.Abs((bnet - sumofScrapValue) / bProdQty);

                                var iMasterId = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(responseDataRptPro.data[0]["VoucherNo"]));
                                UpdateBatchs(vPRptProVtype, bDate, iMasterId, a, miCompanyId);



                                var iHeaderId = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(responseDataRptPro.data[0]["HeaderId"]));
                                strQry = $@" insert into dbo.tCore_Header_2_0 (fProdSize,iProdOrderId,iLCNo,iPaymentTerms,iRctIss,iProcessId,iHeaderId) " +
                                         "values (" + bProdQty + "," + bProdOrderId + ",0,0,0," + bProdOrderId + "," + iHeaderId + ")";
                                DataAcesslayer.GetExecute(strQry, miCompanyId, ref strErrorMessage);
                                //UpdateStatus(mivtype, msdocNo, 1, miCompanyId);

                                strQry = $@" update dbo.tCore_IndtaBodyScreenData_0   set mInput1 =" + a + ",mVal1=" + a + ", mInput2=" + (a * bProdQty) + ", mVal2=" + (a * bProdQty) +
                                    " FROM dbo.tCore_Header_0 AS TCH INNER JOIN dbo.tCore_Data_0 AS TCD ON TCH.iHeaderId = TCD.iHeaderId INNER JOIN " + // ,mInput3=TCI.mGross ,mVal3=TCI.mGross
                                    " dbo.tCore_Indta_0 AS TCI ON TCD.iBodyId = TCI.iBodyId INNER JOIN dbo.tCore_IndtaBodyScreenData_0 AS TCIBSD ON TCI.iBodyId = TCIBSD.iBodyId " +
                                    " WHERE (TCH.iVoucherType =" + vPRptProVtype + ") AND (TCH.sVoucherNo = N'" + iMasterId + "') AND (TCI.iProduct = " + bMainOPItem + ") ";
                                //DataAcesslayer.GetExecute(strQry, miCompanyId, ref strErrorMessage);

                                strQry = $@" update dbo.tCore_IndtaBodyScreenData_0 set mInput1=p.ScrapRate,mVal1=p.ScrapRate, mInput2=(p.ScrapRate * TCI.fQuantity),mVal2=(p.ScrapRate * TCI.fQuantity) " + // ,mInput3=TCI.mGross, mVal3=TCI.mGross
                                    " FROM dbo.tCore_Header_0 AS TCH INNER JOIN  dbo.tCore_Data_0 AS TCD ON TCH.iHeaderId = TCD.iHeaderId INNER JOIN " +
                                    "dbo.tCore_Indta_0 AS TCI ON TCD.iBodyId = TCI.iBodyId INNER JOIN dbo.tCore_IndtaBodyScreenData_0 AS TCIBSD ON TCI.iBodyId = TCIBSD.iBodyId INNER JOIN " +
                                    "dbo.muCore_Product_Settings AS p ON TCI.iProduct = p.iMasterId WHERE (TCH.iVoucherType = " + vPRptProVtype + ") AND (TCH.sVoucherNo = N'" + iMasterId + "') AND (TCI.iProduct <> " + bMainOPItem + ")";
                                //DataAcesslayer.GetExecute(strQry, miCompanyId, ref strErrorMessage);

                                //strQry = $@" update dbo.tCore_Indta_0 set mRate = " + a + " FROM dbo.tCore_Header_0 AS TCH INNER JOIN " +
                                //   " dbo.tCore_Data_0 AS TCD ON TCH.iHeaderId = TCD.iHeaderId INNER JOIN dbo.tCore_Indta_0 AS TCI ON TCD.iBodyId = TCI.iBodyId " +
                                //  " WHERE(TCH.iVoucherType = " + vPRptProVtype + ") AND(TCH.sVoucherNo = N'" + iMasterId + "') AND (TCI.iProduct = " + bMainOPItem + ")";
                                strQry = $@" update dbo.tCore_IndtaBodyScreenData_0 set mInput1=" + a + ",mVal1=" + a + ", mInput2=" + (a * bProdQty) + ",mVal2=" + (a * bProdQty) +  //,mInput3=TCI.mGross, mVal3=TCI.mGross
                                    " FROM dbo.tCore_Header_0 AS TCH INNER JOIN  dbo.tCore_Data_0 AS TCD ON TCH.iHeaderId = TCD.iHeaderId INNER JOIN " +
                                    "dbo.tCore_Indta_0 AS TCI ON TCD.iBodyId = TCI.iBodyId INNER JOIN dbo.tCore_IndtaBodyScreenData_0 AS TCIBSD ON TCI.iBodyId = TCIBSD.iBodyId INNER JOIN " +
                                    "dbo.muCore_Product_Settings AS p ON TCI.iProduct = p.iMasterId WHERE (TCH.iVoucherType = " + vPRptProVtype + ") AND (TCH.sVoucherNo = N'" + iMasterId + "') AND (TCI.iProduct = " + bMainOPItem + ")";
                                // DataAcesslayer.GetExecute(strQry, miCompanyId, ref strErrorMessage);
                                //return Json(new { status = true, data = new { message = "Posting Successful" } });


                                
                                if (blnPendingIssProQC(miCompanyId, msSessionId, msUser, msLoginId, mivtype, msdocNo, vPRptProVtype, Convert.ToString(iMasterId), Convert.ToInt32(iHeaderId),  mlBodyData))
                                {
                                    msg.Clear();
                                    msg.Append("Posting Successful");
                                    return true;
                                }

                                //msg.Clear();
                                //msg.Append("Posting Successful");
                                //return true;
                            }
                        } // end  if (responseRptPro != null)

                    } // end using (var clientRptPro = new WebClient())

                } // end if (ds != null)


            }  //End Try 
            catch (Exception ex)
            {
                msg.Clear();
                msg.Append(ex.Message);
                clsGeneric.writeLog("Exception occured:" + (ex.Message));
                //return Json(new { status = false, data = new { message = "Posting Failed " } });
                throw;

            } //End Catch
            return false;
        }    // end UpdateIssPro


        public bool blnPostProdnInwQC(int miCompanyId, string msSessionId, string msUser, int miLoginId, int mivtype, string msdocNo, List<PEBody> mlBodyData)
        {

            clsGeneric.writeLog("------------------ Function Name :-  " + " blnPostProdnInwQC " + "------------------");


            try
            {
                bProd_Id = 0;
                bProdOrderId = 0;
                //strQry = $@"SELECT Top (1) CHD.QCStatus FROM  dbo.tCore_HeaderData" + vtype + "_0 AS CHD INNER JOIN " +
                //"dbo.tCore_Header_0 AS CH ON CHD.iHeaderId = CH.iHeaderId WHERE (CH.iVoucherType =" + vtype + ") AND (CH.sVoucherNo = N'" + docNo + "')";
                //PostingStatus = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, strQry));
                // 0,Pending,1,Under QC,2,QC Clear
                //clsGeneric.writeLog("PostingStatus 0,Pending,1,Under QC,2,QC Clear  : " + PostingStatus);
                //if (PostingStatus == 0)
                //{
                strQry = $@"SELECT  sAbbr  FROM  dbo.cCore_Vouchers_0 WHERE (iVoucherType =" + mivtype + " )";
                vBAbbr = Convert.ToString(clsGeneric.ShowRecord(miCompanyId, strQry));
                strQry = $@"SELECT  sName  FROM  dbo.cCore_Vouchers_0 WHERE (iVoucherType =" + mivtype + " )";
                vBName = Convert.ToString(clsGeneric.ShowRecord(miCompanyId, strQry));


                //strQry = $@"SELECT      TOP (1) CHN.iProdOrderId, MP.iVaraintId, mBOM.iProductId, mBOM.iInvTagValue, vmW.IsUnderQC, vmP.QCProdnWH, CD.iFaTag AS Branch, vmD.RejectedWH, vmD.ReworkWH, CHN.fProdSize  " +
                //" FROM         dbo.tCore_Header_0 AS CH INNER JOIN  " +
                //" dbo.tCore_Data_0 AS CD ON CH.iHeaderId = CD.iHeaderId INNER JOIN  " +
                //" dbo.tCore_Header_2_0 AS CHN ON CH.iHeaderId = CHN.iHeaderId INNER JOIN  " +
                //" dbo.tMrp_ProdOrder_0 AS MP ON CHN.iProdOrderId = MP.iProdOrderId INNER JOIN  " +
                //" dbo.mMRP_BOMBody AS mBOM ON MP.iVaraintId = mBOM.iVariantId INNER JOIN  " +
                //" dbo.vmCore_Warehouse AS vmW ON mBOM.iInvTagValue = vmW.iMasterId INNER JOIN  " +
                //" dbo.vmCore_Product AS vmP ON mBOM.iProductId = vmP.iMasterId INNER JOIN  " +
                //" dbo.vmCore_Department AS vmD ON CD.iFaTag = vmD.iMasterId  " +
                //" WHERE      (CH.iVoucherType = " + vtype + ") AND (CH.sVoucherNo = N'" + docNo + "') AND (mBOM.bMainOutPut = 1)";
                strQry = $@"SELECT TOP (1) CHN.iProdOrderId, MP.iVaraintId, mBOM.iProductId, mBOM.iInvTagValue, vmW.IsUnderQC, vmP.QCProdnWH, CD.iFaTag AS Branch, vmD.RejectedWH, vmD.ReworkWH, CHN.fProdSize, ICSB.iVersion  " +
                " FROM  dbo.tCore_Header_0 AS CH INNER JOIN  " +
                " dbo.tCore_Data_0 AS CD ON CH.iHeaderId = CD.iHeaderId INNER JOIN  " +
                " dbo.tCore_Header_2_0 AS CHN ON CH.iHeaderId = CHN.iHeaderId INNER JOIN  " +
                " dbo.tMrp_ProdOrder_0 AS MP ON CHN.iProdOrderId = MP.iProdOrderId INNER JOIN  " +
                " dbo.mMRP_BOMBody AS mBOM ON MP.iVaraintId = mBOM.iVariantId INNER JOIN  " +
                " dbo.vmCore_Warehouse AS vmW ON mBOM.iInvTagValue = vmW.iMasterId INNER JOIN  " +
                " dbo.vmCore_Product AS vmP ON mBOM.iProductId = vmP.iMasterId INNER JOIN  " +
                " dbo.vmCore_Department AS vmD ON CD.iFaTag = vmD.iMasterId INNER JOIN  " +
                " dbo.ICS_BOMVariant AS ICSB ON mBOM.iVariantId = ICSB.iVariantId  " +
                " WHERE(CH.iVoucherType = " + mivtype + ") AND(CH.sVoucherNo = N'" + msdocNo + "') AND(mBOM.bMainOutPut = 1)  " +
                " ORDER BY ICSB.iVersion DESC  ";

                DataSet ds = DataAcesslayer.GetData(strQry, miCompanyId, ref strErrorMessage);

                clsGeneric.writeLog("Record Count:" + (ds.Tables[0].Rows.Count));
                clsGeneric.writeLog("Getting from Voucher:" + (ds));
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        bProdOrderId = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                        bProd_Id = Convert.ToInt32(ds.Tables[0].Rows[0][2]);
                        clsGeneric.writeLog("Get Item for Production QC clearance :-" + bProd_Id);
                        pBWarehouse = Convert.ToInt32(ds.Tables[0].Rows[0][3]);
                        Is_UnderQc = Convert.ToInt32(ds.Tables[0].Rows[0][4]);
                        AWarehouse__Id = Convert.ToInt32(ds.Tables[0].Rows[0][5]);
                        bBranch__Id = Convert.ToInt32(ds.Tables[0].Rows[0][6]);
                        RWarehouse__Id = Convert.ToInt32(ds.Tables[0].Rows[0][7]);
                        RWWarehouse__Id = Convert.ToInt32(ds.Tables[0].Rows[0][8]);
                        pBQuantity = Convert.ToDecimal(ds.Tables[0].Rows[0][9]);
                    } // end if (ds.Tables[0].Rows.Count > 0)
                      //if (AWarehouse__Id != 0 && Is_UnderQc == 1)
                      //{
                    if (Is_UnderQc == 1)
                    {
                        if (AWarehouse__Id != 0)
                        {
                            vBAbbr2 = "QCItemPara";
                            strValue = $@"SELECT  sName  FROM  dbo.cCore_Vouchers_0 WHERE (sAbbr ='" + vBAbbr2 + "' )";
                            vBName2 = Convert.ToString(clsGeneric.ShowRecord(miCompanyId, strValue));
                            strValue = $@"SELECT  iVoucherType  FROM  dbo.cCore_Vouchers_0 WHERE (sAbbr ='" + vBAbbr2 + "' )";
                            vBType2 = Convert.ToInt32(clsGeneric.ShowRecord(miCompanyId, strValue));


                            vPostAbbr = "ProdnInwQC";
                            strValue = $@"SELECT  sName  FROM  dbo.cCore_Vouchers_0 WHERE (sAbbr ='" + vPostAbbr + "' )";
                            vPostName = Convert.ToString(clsGeneric.ShowRecord(miCompanyId, strValue));
                            //Working Start
                            using (var clientIssPro = new WebClient())
                            {

                                clientIssPro.Encoding = Encoding.UTF8;
                                clientIssPro.Headers.Add("fSessionId", msSessionId);
                                clientIssPro.Headers.Add("Content-Type", "application/json");
                                var responseIssPro = clientIssPro.DownloadString("http://localhost/Focus8API/Screen/Transactions/" + vBName + "/" + msdocNo);
                                clsGeneric.writeLog("Download URL : " + "http://localhost/Focus8API/Screen/Transactions/" + vBName + "/" + msdocNo);
                                clsGeneric.writeLog("response " + vBName + " : " + responseIssPro);
                                //BranchId = 0;
                                if (responseIssPro != null)
                                {
                                    var responseDataIssPro = JsonConvert.DeserializeObject<APIResponse.PostResponse>(responseIssPro);
                                    if (responseDataIssPro.result == -1)
                                    {
                                        //return Json(new { status = false, data = new { message = responseDataIssPro.message } });
                                        msg.Clear();
                                        msg.Append(responseDataIssPro.message);
                                        return false;
                                    }
                                    else
                                    {
                                        if (responseDataIssPro.data.Count != 0)
                                        {

                                            var b1extHeader = JsonConvert.DeserializeObject<Hashtable>(JsonConvert.SerializeObject(responseDataIssPro.data[0]["Header"]));
                                            //BlankRate = 0;
                                            if (responseDataIssPro.data[0]["Footer"].ToString() != "[]")
                                            {
                                                var b1extFooter = JsonConvert.DeserializeObject<List<Hashtable>>(JsonConvert.SerializeObject(responseDataIssPro.data[0]["Footer"]));
                                                //BlankRate = Convert.ToDecimal(extFooter[7]["Input"]);
                                            }
                                            var b1extBody = JsonConvert.DeserializeObject<List<Hashtable>>(JsonConvert.SerializeObject(responseDataIssPro.data[0]["Body"]));

                                            pHDept = Convert.ToInt32(b1extHeader["Dept__Id"]);
                                            pHDocdate = Convert.ToInt32(b1extHeader["Date"]);
                                            pHDocNo = Convert.ToString(b1extHeader["DocNo"]);
                                            pHBranch = Convert.ToInt32(b1extHeader["Branch__Id"]);
                                            pHVendorAccount = Convert.ToInt32(b1extHeader["VendorAC__Id"]);
                                            pHWorksCentre = Convert.ToInt32(b1extHeader["Works Center__Id"]);
                                            //Warehouse from BOM Output
                                            //pBWarehouse = Convert.ToInt32(b1extHeader["Warehouse__Id"]);



                                            int tempFor1 = b1extBody.Count - 1;
                                            for (int i = 0; i <= 0; i++)
                                            {
                                                //PBItem = bProd_Id;
                                                PBTransactionId = Convert.ToInt32(b1extBody[i]["TransactionId"]);

                                                //vStatus = Convert.ToInt32(b1extBody[i]["QCStatus"]);
                                                //if (vStatus == 2)
                                                //{
                                                //    cntvStatus++;
                                                //}

                                                //strValue = $@"SELECT QCRequire FROM muCore_Product_Units AS ExtraFld WHERE(iMasterId =" + PBItem + " )";
                                                //vQCRequire = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, strValue));

                                                if (PostingStatus == 0)
                                                {

                                                    //pBWarehouse = Convert.ToInt32(b1extBody[i]["Warehouse__Id"]);
                                                    Hashtable headerCBROD = new Hashtable();
                                                    headerCBROD.Add("DocNo", "");
                                                    //headerCBROD.Add("DocNo", msdocNo);
                                                    headerCBROD.Add("Date", pHDocdate);
                                                    headerCBROD.Add("Branch__Id", pHBranch);
                                                    headerCBROD.Add("Warehouse__Id", pBWarehouse);
                                                    headerCBROD.Add("Dept__Id", pHDept);
                                                    headerCBROD.Add("Works Center__Id", pHWorksCentre); //4
                                                    headerCBROD.Add("PartyName__Id", pHVendorAccount); //4
                                                    headerCBROD.Add("Item__Id", bProd_Id);
                                                    clsGeneric.writeLog("Set Item for Production QC clearance :- " + bProd_Id);
                                                    headerCBROD.Add("AWarehouse__Id", AWarehouse__Id);
                                                    headerCBROD.Add("RWarehouse", RWarehouse__Id);
                                                    headerCBROD.Add("ReworkWarehouse", RWWarehouse__Id);



                                                    //headerCBROD.Add("Quantity", Convert.ToInt32(b1extBody[i]["Quantity"]));
                                                    //headerCBROD.Add("Rejected Qty", Convert.ToInt32(b1extBody[i][44]));
                                                    headerCBROD.Add("TransId", PBTransactionId);
                                                    headerCBROD.Add("ProdnDoc", pHDocNo);
                                                    headerCBROD.Add("ProdnDate", pHDocdate);
                                                    headerCBROD.Add("ProdnQty", pBQuantity);
                                                    headerCBROD.Add("PostingStatus", 0);
                                                    headerCBROD.Add("Rejected Reason__Id", 1);

                                                    Hashtable headerflagCBROD = new Hashtable
                                                    {

                                                        {"AlreadyLoaded", false},
                                                        {"Amended", false},
                                                        {"Approved", false},
                                                        {"AuthByHigherUps", false},
                                                        {"Cancelled", false},
                                                        {"CantEdit", false},
                                                        {"CantPrint", false},
                                                        {"CheckCreditLimit", true},
                                                        {"CheckNegativeBudget", true},
                                                        {"CheckNegativeCash", true},
                                                        {"CheckNegativeStock", true},
                                                        {"CheckOverdueBills", true},
                                                        {"CheckReorderLevel", true},
                                                        {"Editing", true},
                                                        {"FromPDC", false},
                                                        {"FromTrigger", false},
                                                        {"FromUI", false},
                                                        {"FromWeb", false},
                                                        {"Internal", false},
                                                        {"NoBatchCheck", false},
                                                        {"NoLinkCheck", false},
                                                        {"PostCashEntry", false},
                                                        {"PostingFromUI", false},
                                                        {"RequestCrLimit", false},
                                                        {"Suspended", false},
                                                        {"TDSCertPrepared", false},
                                                        {"TDSPaid", false},
                                                        {"UnsaveInv", false},
                                                        {"UpdateFA", false},
                                                        {"UpdateInv", false},
                                                        {"Version", false},
                                                        {"WMSAllocated", false}


                                                    };
                                                    //headerCBROD.Add("Flags", headerflagCBROD);
                                                    List<System.Collections.Hashtable> lstBody = new List<System.Collections.Hashtable>();
                                                    strValue = $@"SELECT CH.sVoucherNo FROM tCore_Header_0 AS CH INNER JOIN tCore_HeaderData" + vBType2 + "_0 AS CHD ON CH.iHeaderId = CHD.iHeaderId " +
                                                        " WHERE (CH.iVoucherType = " + vBType2 + ") AND (CHD.Item =" + bProd_Id + " )";
                                                    clsGeneric.writeLog("Query", strValue);
                                                    vBDocNo2 = "";
                                                    vBDocNo2 = Convert.ToString(clsGeneric.ShowRecord(miCompanyId, strValue));
                                                    if (vBDocNo2 != "" && vBDocNo2 != "0")
                                                    {
                                                        using (var clientQCItemPara = new WebClient())
                                                        {

                                                            clientQCItemPara.Encoding = Encoding.UTF8;
                                                            clientQCItemPara.Headers.Add("fSessionId", msSessionId);
                                                            clientQCItemPara.Headers.Add("Content-Type", "application/json");

                                                            var responseQCItemPara = clientQCItemPara.DownloadString("http://localhost/Focus8API/Screen/Transactions/" + vBType2 + "/" + vBDocNo2);
                                                            clsGeneric.writeLog("Download URL : " + "http://localhost/Focus8API/Screen/Transactions/" + vBType2 + "/" + vBDocNo2);
                                                            clsGeneric.writeLog("response QCItemPara : " + responseQCItemPara);
                                                            if (responseQCItemPara != null)
                                                            {

                                                                var responseDataQCItemPara = JsonConvert.DeserializeObject<APIResponse.PostResponse>(responseQCItemPara);
                                                                if (responseDataQCItemPara.result == -1)
                                                                {
                                                                    //return Json(new { status = false, data = new { message = responseDataQCItemPara.message } });
                                                                    msg.Clear();
                                                                    msg.Append(responseDataQCItemPara.message);
                                                                    return false;
                                                                }
                                                                else
                                                                {
                                                                    if (responseDataQCItemPara.data.Count != 0)
                                                                    {
                                                                        var p2extHeader = JsonConvert.DeserializeObject<Hashtable>(JsonConvert.SerializeObject(responseDataQCItemPara.data[0]["Header"]));

                                                                        if (responseDataQCItemPara.data[0]["Footer"].ToString() != "[]")
                                                                        {
                                                                            var p2extFooter = JsonConvert.DeserializeObject<List<Hashtable>>(JsonConvert.SerializeObject(responseDataQCItemPara.data[0]["Footer"]));

                                                                        }
                                                                        var p2extBody = JsonConvert.DeserializeObject<List<Hashtable>>(JsonConvert.SerializeObject(responseDataQCItemPara.data[0]["Body"]));
                                                                        int tempFor2 = p2extBody.Count - 1;
                                                                        for (int j = 0; j <= tempFor2; j++)
                                                                        {
                                                                            Hashtable bodyCBROD = new Hashtable();
                                                                            bodyCBROD.Add("Parameter QC__Id", Convert.ToInt32(p2extBody[j]["Parameter QC__Id"]));
                                                                            bodyCBROD.Add("Specification", Convert.ToString(p2extBody[j]["Specification"]));
                                                                            //Add By Rizwan 03-03-2022 suggested Aamer Sir
                                                                            bodyCBROD.Add("InstrumentUsed", Convert.ToString(p2extBody[j]["InstrumentUsed"]));
                                                                            //Add By Rizwan 21-02-2022 suggested Aamer Sir
                                                                            //Get Min & Max Spec
                                                                            var aa2 = JsonConvert.DeserializeObject<Hashtable>(JsonConvert.SerializeObject(p2extBody[j]["Min Spec"]))["Value"];
                                                                            Hashtable bodyMinSpec = new Hashtable
                                                                                {
                                                                                    {"Input", Convert.ToDecimal(aa2)},
                                                                                    {"FieldName", "Min Spec"},
                                                                                    {"ColMap", 0},
                                                                                    {"Value",Convert.ToDecimal(aa2)}

                                                                                };
                                                                            aa2 = JsonConvert.DeserializeObject<Hashtable>(JsonConvert.SerializeObject(p2extBody[j]["Max Spec"]))["Value"];
                                                                            Hashtable bodyMaxSpec = new Hashtable
                                                                                {
                                                                                    {"Input", Convert.ToDecimal(aa2)},
                                                                                    {"FieldName", "Max Spec"},
                                                                                    {"ColMap", 1},
                                                                                    {"Value",Convert.ToDecimal(aa2)}

                                                                                };
                                                                            Hashtable bodyflagCBROD = new Hashtable
                                                                            {
                                                                                {"BRS", false},
                                                                                {"ConfirmBins", true},
                                                                                {"ForexFlux", false},
                                                                                {"FreeQty", false},
                                                                                {"InternalIIDST", false},
                                                                                {"PDC", false},
                                                                                {"PDCDisc", false},
                                                                                {"SuspendBase", false},
                                                                                {"SuspendFA", false},
                                                                                {"SuspendLink", false},
                                                                                {"SuspendRef", false},
                                                                                {"TransferToPnL", false},
                                                                                {"Void", false}
                                                                            };
                                                                            //bodyCBROD.Add("BodyFlags", bodyflagCBROD);
                                                                            bodyCBROD.Add("Min Spec", bodyMinSpec);
                                                                            bodyCBROD.Add("Max Spec", bodyMaxSpec);
                                                                            lstBody.Add(bodyCBROD);
                                                                        }
                                                                    }
                                                                }
                                                            }

                                                        }
                                                    }
                                                    else
                                                    {
                                                        msg.Clear();
                                                        msg.Append("QC Item Wise Parameter:- ");
                                                        strValue = $@"Select Sname from mcore_product  WHERE(iMasterId =" + bProd_Id + " )";
                                                        RMName = Convert.ToString(clsGeneric.ShowRecord(miCompanyId, strValue));
                                                        msg.AppendLine(RMName);
                                                        clsGeneric.writeLog("MSG " + msg);
                                                    }
                                                    if (lstBody.Count != 0)
                                                    {
                                                        System.Collections.Hashtable objHash = new System.Collections.Hashtable();
                                                        objHash.Add("Body", lstBody);
                                                        objHash.Add("Header", headerCBROD);

                                                        List<System.Collections.Hashtable> lstHash = new List<System.Collections.Hashtable>();
                                                        lstHash.Add(objHash);
                                                        HashData objHashRequest = new HashData();
                                                        objHashRequest.data = lstHash;
                                                        string sContentInQC = JsonConvert.SerializeObject(objHashRequest);
                                                        clsGeneric.writeLog("Inwards QC Clearance Contents : " + sContentInQC);
                                                        clsGeneric.writeLog("Upload url : " + "http://localhost/Focus8API/Transactions/Vouchers/" + vPostName);
                                                        using (var clientInQC = new WebClient())
                                                        {
                                                            clientInQC.Encoding = Encoding.UTF8;
                                                            clientInQC.Headers.Add("fSessionId", msSessionId);
                                                            clientInQC.Headers.Add("Content-Type", "application/json");
                                                            var responseInQC = clientInQC.UploadString("http://localhost/Focus8API/Transactions/Vouchers/" + vPostName, sContentInQC);
                                                            clsGeneric.writeLog("response InQC  : " + responseInQC);
                                                            if (responseInQC != null)
                                                            {
                                                                var responseDataInQC = JsonConvert.DeserializeObject<APIResponse.PostResponse>(responseInQC);

                                                                if (responseDataInQC.result == 1)
                                                                {
                                                                    // UpdateStatus(vtype, docNo, 1, CompanyId);
                                                                    //UpdateQCStatus(vtype, docNo, 1, CompanyId);
                                                                    //return Json(new { status = true, data = new { message = "Posting Successful" } });
                                                                    msg.Clear();
                                                                    msg.Append("Posting Successful");
                                                                    return true;

                                                                }

                                                            }
                                                        }
                                                    }
                                                }

                                            }

                                        }

                                    }

                                }

                            }
                        }
                        //else
                        //{
                        //    return Json(new { status = false, data = new { message = "Accepted warehouse Or Under QC Blank" } });
                        //}
                        else
                        {
                            //return Json(new { status = false, data = new { message = "Accepted warehouse Blank In Item" } });
                            msg.Clear();
                            msg.Append("Accepted warehouse Blank In Item");
                            return false;
                        }
                    }
                    else
                    {
                        //return Json(new { status = false, data = new { message = "QC Not Required" } });
                        msg.Clear();
                        msg.Append("QC Not Required");
                        return false;
                    }

                } // end if (ds != null)
                else
                {
                    clsGeneric.writeLog("Error:" + "Main Out not defined in respective Bill of Material");
                    //return Json(new { status = false, data = new { message = "Main Out not defined in respective Bill of Material" } });
                    msg.Clear();
                    msg.Append("Main Out not defined in respective Bill of Material");
                    return false;
                }
                //    } // End If (PostingStatus != 0)
                //else
                //{
                //    return Json(new { status = false, data = new { message = "Document Already Posted" } });
                //}
            }  //End Try 
            catch (Exception ex)
            {
                clsGeneric.writeLog("Exception occured:" + (ex.Message));
                //return Json(new { status = false, data = new { message = ex.Message + "Posting Failed " } });
                msg.Clear();
                msg.Append(ex.Message);
                return false;
                throw;
            } //End Catch
            //return Json(new { status = false, data = new { message = "Posting Failed" } });
            msg.Clear();
            msg.Append("Posting Failed");
            return false;

        }    // end UpdateIssPro_InQc

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

        [HttpPost]
        public ActionResult ProdOrdAutoClose(int CompanyId, string User, string SessionId, int LoginId, int vtype, string idocDate)
        {

            clsGeneric.writeLog(" ------------------ ProdOrdAutoClose - IssPro starts here -------------------");
            clsGeneric.writeLog("fSessionid : " + SessionId);
            strQry = $@"Update tMrp_ProdOrder_0 set iOrderStatus=6 where iDueDate<=" + idocDate;
            clsGeneric.writeLog("update Query  : " + strQry);
            DataAcesslayer.GetExecute(strQry, CompanyId, ref strErrorMessage);
            clsGeneric.writeLog("Return Message  : " + strErrorMessage);
            return new EmptyResult();

        }

        [HttpPost]
        public ActionResult NotUsedPendingIssProQC(int CompanyId, string User, string SessionId, int LoginId, int vtype, string idocDate, string bdocNo, List<PEBody> BodyData)
        {
            #region Local Variable Definition
            int iItemid = 0;
            decimal dQty = 0;
            //decimal dstkRate = 0;
            int iWId = 0;
            //string sPONO = "";
            int iPOid = 0;
            string sbatchNo = "";

            string sMfgDt = "";
            int iBatchid = 0;
            int iDeptId = 0;
            //int iStatus = 0;
            int iBodyId = 0;
            decimal dItemQty = 0;
            decimal dItemRt = 0;
            #endregion

            clsGeneric.Log_write(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), SessionId, CompanyId, bdocNo, vtype, User);
            var IssProBody = new List<IssProBodyData>();
            IssProBody.Clear();
            
            BL_DB DataAcesslayer = new BL_DB();

            strQry = $@"SELECT  sAbbr  FROM  dbo.cCore_Vouchers_0 WHERE (iVoucherType =" + vtype + " )";
            vAbbr = Convert.ToString(clsGeneric.ShowRecord(CompanyId, strQry));
            strQry = $@"SELECT  sName  FROM  dbo.cCore_Vouchers_0 WHERE (iVoucherType =" + vtype + " )";
            vSname = Convert.ToString(clsGeneric.ShowRecord(CompanyId, strQry));
            IssProBody.Clear();
            using (var clientIssPro = new WebClient())
            {
                clientIssPro.Encoding = Encoding.UTF8;
                clientIssPro.Headers.Add("fSessionId", SessionId);
                clientIssPro.Headers.Add("Content-Type", "application/json");
                clsGeneric.writeLog("Download IssPro URL: " + "http://localhost/Focus8API/Screen/Transactions/" + vtype + "/" + bdocNo);
                var rIssPro = clientIssPro.DownloadString("http://localhost/Focus8API/Screen/Transactions/" + vtype + "/" + bdocNo);
                clsGeneric.writeLog("response IssPro: " + rIssPro);
                if (rIssPro != null)
                {
                    var rDIssPro = JsonConvert.DeserializeObject<APIResponse.PostResponse>(rIssPro);
                    if (rDIssPro.result == -1)
                    {
                        return Json(new { status = false, data = new { message = rDIssPro.message } });
                    }
                    else
                    {
                        if (rDIssPro.data.Count != 0)
                        {
                            var extHeader = JsonConvert.DeserializeObject<Hashtable>(JsonConvert.SerializeObject(rDIssPro.data[0]["Header"]));
                            if (rDIssPro.data[0]["Footer"].ToString() != "[]")
                            {
                                var extFooter = JsonConvert.DeserializeObject<List<Hashtable>>(JsonConvert.SerializeObject(rDIssPro.data[0]["Footer"]));
                            }
                            var extBody = JsonConvert.DeserializeObject<List<Hashtable>>(JsonConvert.SerializeObject(rDIssPro.data[0]["Body"]));

                            iDeptId = Convert.ToInt32(extHeader["Department__Id"]);
                            iWId = Convert.ToInt32(extHeader["Warehouse__Id"]);
                            iPOid = Convert.ToInt32(extHeader["BatchNo"]);
                            dQty = Convert.ToDecimal(extHeader["ProdQty"].ToString());
                            iFgId = Convert.ToInt32(extHeader["FinishedGoods__Id"]);

                            int tempFor1 = extBody.Count - 1;
                            for (int i = 0; i <= tempFor1; i++)
                            {
                                iItemid = 0;
                                dItemQty = 0;
                                dItemRt = 0;
                                iBodyId = 0;
                                iBatchid = 0;
                                sbatchNo = "";
                                sMfgDt = "";

                                iItemid = Convert.ToInt32(extBody[i]["Item__Id"]);
                                dItemQty = Convert.ToDecimal(extBody[i]["Quantity"]);
                                dItemRt = Convert.ToDecimal(extBody[i]["Rate"]);
                                iBodyId = Convert.ToInt32(extBody[i]["TransactionId"]);
                                iBatchid = Convert.ToInt32(JsonConvert.DeserializeObject<Hashtable>(JsonConvert.SerializeObject(extBody[i]["Batch"]))["BatchId"]);
                                sbatchNo = Convert.ToString(JsonConvert.DeserializeObject<Hashtable>(JsonConvert.SerializeObject(extBody[i]["Batch"]))["BatchNo"]);
                                sMfgDt = Convert.ToString(JsonConvert.DeserializeObject<Hashtable>(JsonConvert.SerializeObject(extBody[i]["Batch"]))["MfgDate"]);


                                strQry = $@"insert into ICSPendingIssProQC(vNo,docDate,vType,itemId,qty";
                                vBAbbr = Convert.ToString(clsGeneric.ShowRecord(CompanyId, strQry));
                            }


                        }
                    }
                }
            } // end using 
            clsGeneric.writeLog("update Query  : " + strQry);
            return new EmptyResult();
        }
        public ActionResult PendingIssProQC(int CompanyId, string User, string SessionId, int LoginId, int vtype, string idocDate, string bdocNo, List<PEBody> BodyData)
        {
            clsGeneric.Log_write(RouteData.Values["controller"].ToString(), "PendingIssProQC", SessionId, CompanyId, bdocNo, vtype, User);
            clsGeneric.writeLog("------------------ Function Name :-  " + " PendingIssProQC " + "------------------");
            try
            {

                strQry = $@"SELECT  sAbbr  FROM  dbo.cCore_Vouchers_0 WHERE (iVoucherType =" + vtype + " )";
                vBAbbr = Convert.ToString(clsGeneric.ShowRecord(CompanyId, strQry));
                strQry = $@"SELECT  sName  FROM  dbo.cCore_Vouchers_0 WHERE (iVoucherType =" + vtype + " )";
                vBName = Convert.ToString(clsGeneric.ShowRecord(CompanyId, strQry));

                strQry = $@"SELECT  iVoucherType  FROM  dbo.cCore_Vouchers_0 WHERE (sAbbr ='" + vPRptProAbbr + "')";
                vPRptProVtype = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, strQry));
                strQry = $@"SELECT  sName  FROM  dbo.cCore_Vouchers_0 WHERE (iVoucherType ='" + vPRptProVtype + "')";
                vPRptProName = Convert.ToString(clsGeneric.ShowRecord(CompanyId, strQry));

                strQry = $@"SELECT  iVoucherType  FROM  dbo.cCore_Vouchers_0 WHERE (sAbbr ='" + vPPrdOrdAbbr + "')";
                vPPrdOrdVtype = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, strQry));
                strQry = $@"SELECT  sName  FROM  dbo.cCore_Vouchers_0 WHERE (iVoucherType ='" + vPPrdOrdVtype + "')";
                vPPrdOrdName = Convert.ToString(clsGeneric.ShowRecord(CompanyId, strQry));

                strQry = $@" SELECT DISTINCT CH.iDate, CD.iCode AS PurchaseAccount, CD.iBookNo AS workinPrgAc, CD.iFaTag AS Branch,  " +
                    " CD.iInvTag AS Warehouse, CHN.fProdSize, CHN.iProdOrderId, CH.fNet, CHD.AcctPost1, CHD.AcctPost2,CHD.FinishedGoods" +
                    " FROM dbo.tCore_Header_0 AS CH INNER JOIN dbo.tCore_Data_0 AS CD ON CH.iHeaderId = CD.iHeaderId INNER JOIN " +
                    " dbo.tCore_Header_2_0 AS CHN ON CH.iHeaderId = CHN.iHeaderId " +
                    " INNER JOIN  dbo.tCore_HeaderData" + vtype + "_0 AS CHD ON CH.iHeaderId = CHD.iHeaderId WHERE(CH.iVoucherType = " + vtype + ") AND(CH.sVoucherNo = N'" + bdocNo + "') and CD.iCode <> 0";
                clsGeneric.writeLog("strQry:" + strQry);
                DataSet ds = DataAcesslayer.GetData(strQry, CompanyId, ref strErrorMessage);
                clsGeneric.writeLog("Getting from Voucher:" + (ds));
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        // idocDate = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                        bWIPAC__Id = Convert.ToInt32(ds.Tables[0].Rows[0][1]);
                        bPurchaseAC__Id = Convert.ToInt32(ds.Tables[0].Rows[0][2]);
                        //bPurchaseAC__Id = Convert.ToInt32(ds.Tables[0].Rows[0][1]);
                        //bWIPAC__Id = Convert.ToInt32(ds.Tables[0].Rows[0][2]);
                        bBranch__Id = Convert.ToInt32(ds.Tables[0].Rows[0][3]);
                        bWarehouse__Id = Convert.ToInt32(ds.Tables[0].Rows[0][4]);
                        bProdQty = Convert.ToDecimal(ds.Tables[0].Rows[0][5]);
                        bProdOrderId = Convert.ToInt32(ds.Tables[0].Rows[0][6]);
                        
                        iFgId = Convert.ToInt32(ds.Tables[0].Rows[0][10]);

                        bnet = Math.Abs(Convert.ToDecimal(ds.Tables[0].Rows[0][7]));
                        //bDept__Id = Convert.ToInt32(ds.Tables[0].Rows[0][8]);
                        //bWorksCenter__Id = Convert.ToInt32(ds.Tables[0].Rows[0][9]);
                        ibAcctPost1 = Convert.ToInt32(ds.Tables[0].Rows[0][8]);
                        ibAcctPost2 = Convert.ToInt32(ds.Tables[0].Rows[0][9]);
                    } // end if (ds.Tables[0].Rows.Count > 0)

                    //HashData objHashRequest_RptPro = new HashData();
                    //Hashtable headerRptPro = new Hashtable();
                    //headerRptPro.Add("DocNo", bdocNo);
                    //headerRptPro.Add("Date", idocDate);
                    

                    strQry = $@"select iWIPAccount from muCore_Product_OtherDetails where iMasterId=" + iFgId;
                    bWIPAC__Id = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, strQry));


                    ////headerRptPro.Add("PurchaseAC__Id", bPurchaseAC__Id);
                    ////headerRptPro.Add("PurchaseAC__Id", ibAcctPost1);

                    ////Product - Other details - WIP Account 
                    //headerRptPro.Add("WIPAC__Id", bWIPAC__Id);
                    ////headerRptPro.Add("WIPAC__Id", ibAcctPost2);
                    //headerRptPro.Add("BatchNo", bProdOrderId);
                    //headerRptPro.Add("Department__Id", bBranch__Id);
                    ////headerRptPro.Add("Works Center__Id", bWorksCenter__Id);
                    //headerRptPro.Add("Warehouse__Id", bWarehouse__Id);
                    ////headerRptPro.Add("ProdOrderId", bProdOrderId);
                    //headerRptPro.Add("ProdQty", bProdQty);

                    List<System.Collections.Hashtable> lstBody_RptPro = new List<System.Collections.Hashtable>();
                    lstBody_RptPro.Clear();
                    strQry = $@" SELECT iDate, iDueDate, sRemarks, iOrderStatus," +
                       " ISNULL(iVaraintId, 0)iVaraintId, iProdOrderId, sProdOrderNo, iWareHouseId, ISNULL(iIssueType, 0)iIssueType, ISNULL(sSONO, '')sSONO, " +
                       " ISNULL(iCustomer, 0)iCustomer, ISNULL(sSpecialInstruction, '')sSpecialInstruction," +
                       " isnull(iTagFilterId,0) iTagFilterId,isnull(iTagFilterValue,0) iTagFilterValue,isnull(sBatchNo,'') sBatchNo " +
                       " FROM tMrp_ProdOrder_0 with(readuncommitted) WHERE iProdOrderId =" + bProdOrderId;

                    DataSet ds1 = DataAcesslayer.GetData(strQry, CompanyId, ref strErrorMessage);
                    clsGeneric.writeLog("Getting from Voucher:" + (ds1));
                    if (ds1 != null)
                    {
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            biVaraintId = Convert.ToInt32(ds1.Tables[0].Rows[0][4]);

                        }
                        strQry = $@" SELECT BH.iBomId, BB.bMainOutPut, BB.bInput, BB.iBomBodyId, BB.iProductId, BB.fQty, BB.iUnit, " +
                      " BB.iInvTagValue AS Warehouse, BB.iRowIndex, BB.fRate, vb.iTagValue AS Dept " +
                      " FROM dbo.mMRP_BomHeader AS BH INNER JOIN dbo.mMRP_BOMBody AS BB ON BH.iBomId = BB.iVariantId " +
                      " INNER JOIN dbo.ICS_BOMVariant AS vb ON BB.iVariantId = vb.iVariantId " +
                      " WHERE (BH.iBomId = " + biVaraintId + ") AND (BB.bInput = 0)";
                        sumofScrapValue = 0;
                        DataSet ds2 = DataAcesslayer.GetData(strQry, CompanyId, ref strErrorMessage);
                        clsGeneric.writeLog("Getting from Voucher:" + (ds2));

                        if (ds2 != null)
                        {
                            if (ds2.Tables[0].Rows.Count > 0)
                            {

                                for (int j = 0; j < ds2.Tables[0].Rows.Count; j++)
                                {

                                    ScrapRate = 0;
                                    ScrapValue = 0;
                                    // Scrap Rate Getting 
                                    bbomqty = Math.Abs(Convert.ToDecimal(ds2.Tables[0].Rows[j][5]));
                                    if (Convert.ToInt32(ds2.Tables[0].Rows[j][1]) == 0 && Convert.ToInt32(ds2.Tables[0].Rows[j][2]) == 0)
                                    {
                                        strQry = $@"Select ScrapRate from muCore_Product_Settings where iMasterId=" + Convert.ToInt32(ds2.Tables[0].Rows[j][4]);
                                        ScrapRate = Math.Abs(Convert.ToDecimal(clsGeneric.ShowRecord(CompanyId, strQry)));
                                        ScrapValue = ScrapRate * (bProdQty * bbomqty);
                                        sumofScrapValue = sumofScrapValue + ScrapValue;

                                    } // end if (Convert.ToInt32(ds2.Tables[0].Rows[j][1]) == 0 && Convert.ToInt32(ds2.Tables[0].Rows[j][2]) == 0)
                                    else
                                    {
                                        bMainOPItem = Convert.ToInt32(ds2.Tables[0].Rows[j][4]);
                                    }






                                } // end for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                                bProdrate = (bnet - sumofScrapValue) / bProdQty;
                                //for (int k1 = 0; k1 < ds2.Tables[0].Rows.Count; k1++)
                                //{
                                //    bbomqty = Math.Abs(Convert.ToDecimal(ds2.Tables[0].Rows[k1][5]));
                                //    if (Convert.ToInt32(ds2.Tables[0].Rows[k1][1]) == 0 && Convert.ToInt32(ds2.Tables[0].Rows[k1][2]) == 0)
                                //    {
                                //        strQry = $@"Select ScrapRate from muCore_Product_Settings where iMasterId=" + Convert.ToInt32(ds2.Tables[0].Rows[k1][4]);
                                //        ScrapRate = Math.Abs(Convert.ToDecimal(clsGeneric.ShowRecord(CompanyId, strQry)));
                                //        bProdrate = ScrapRate;

                                //    }
                                //    Hashtable bodyRptPro = new Hashtable();
                                //    //Product - Other details - Stock account (Body)
                                //    strQry = $@"select iStocksAccount from muCore_Product_OtherDetails where iMasterId=" + Convert.ToInt32(ds2.Tables[0].Rows[k1][4]);
                                //    bPurchaseAC__Id = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, strQry));

                                //    bodyRptPro.Add("Account__Id", bPurchaseAC__Id);
                                //    bodyRptPro.Add("Warehouse__Id", Convert.ToInt32(ds2.Tables[0].Rows[k1][7]));
                                //    bodyRptPro.Add("Item__Id", Convert.ToInt32(ds2.Tables[0].Rows[k1][4]));
                                //    bodyRptPro.Add("Unit__Id", Convert.ToInt32(ds2.Tables[0].Rows[k1][6]));

                                //    bodyRptPro.Add("BaseDocNo", bdocNo);
                                //    bodyRptPro.Add("BaseDocDate", idocDate);

                                //    // Change suggested by Amair Sir @16-04-2022 Format two decimal 
                                //    bodyRptPro.Add("Quantity", clsGeneric.DecimalCustomFormat(Convert.ToDouble(bProdQty * bbomqty)));

                                //    bodyRptPro.Add("Rate", bProdrate);

                                //    Hashtable bodyBatchRptPro = new Hashtable
                                //        {
                                //            {"BatchNo",  bdocNo + "/" + Convert.ToInt32( k1 + 1 ) },
                                //            {"MfgDate", Convert.ToInt32(idocDate)},
                                //            {"BatchRate",  bProdrate},
                                //            // Change suggested by Amair Sir @16-04-2022 Format two decimal 
                                //            {"Qty", clsGeneric.DecimalCustomFormat(Convert.ToDouble(bProdQty * bbomqty))}


                                //        };

                                //    Hashtable bodyRejectedRptPro = new Hashtable
                                //        {
                                //            {"Input",  0},
                                //            {"FieldId", 2},
                                //            {"ColMap",  0},
                                //            {"Value", 0}

                                //        };

                                //    List<System.Collections.Hashtable> lstBatch = new List<System.Collections.Hashtable>();
                                //    lstBatch.Add(bodyBatchRptPro);
                                //    List<System.Collections.Hashtable> lstRejected = new List<System.Collections.Hashtable>();
                                //    lstRejected.Add(bodyRejectedRptPro);
                                //    bodyRptPro.Add("Batch", bodyBatchRptPro);
                                //    bodyRptPro.Add("Rejected", bodyRejectedRptPro);
                                //    lstBody_RptPro.Add(bodyRptPro);
                                //}


                            } //end if (ds2.Tables[0].Rows.Count > 0)
                        } // end if (ds2 != null)
                    } // end if (ds1 != null)





                } // end if (ds != null)


            }  //End Try 
            catch (Exception ex)
            {
                msg.Clear();
                msg.Append(ex.Message);
                clsGeneric.writeLog("Exception occured:" + (ex.Message));
                //return Json(new { status = false, data = new { message = "Posting Failed " } });
                throw;

            } //End Catch
            return new EmptyResult();
        }    // end UpdateIssPro

        public bool blnPendingIssProQC(int miCompanyId, string msSessionId, string msUser, int miLoginId, int mivtype, string msdocNo, int iRctprodvtype, string sRctproddocNo, int iRctprodHeaderId, List<PEBody> mlBodyData)
        {

            #region Local Variable Definition
            string vRctprodAbbr = "";
            string vRctprodName = "";
            int idocdate = 0;
            int iItemid = 0;
            decimal dQty = 0;
            //decimal dstkRate = 0;
            int iWId = 0;
            string sPONO = "";
            int iPOid = 0;
            string sbatchNo = "";

            string sMfgDt = "";
            int iBatchid = 0;
            int iDeptId = 0;
            //int iStatus = 0;
            int iBodyId = 0;
            int iStatus = 0;
            int iQCRequire = 0;
            decimal dItemQty = 0;
            decimal dItemRt = 0;
            string strsql = "";
            #endregion

            clsGeneric.writeLog("------------------ Function Name :-  " + " blnPendingIssProQC( " + "------------------");
            try
            {
                clsGeneric.createTable_IssProQC(miCompanyId, msUser, msdocNo);
                strQry = $@"SELECT  sAbbr  FROM  dbo.cCore_Vouchers_0 WHERE (iVoucherType =" + mivtype + " )";
                vBAbbr = Convert.ToString(clsGeneric.ShowRecord(miCompanyId, strQry));
                strQry = $@"SELECT  sName  FROM  dbo.cCore_Vouchers_0 WHERE (iVoucherType =" + mivtype + " )";
                vBName = Convert.ToString(clsGeneric.ShowRecord(miCompanyId, strQry));

                strQry = $@"SELECT  sAbbr  FROM  dbo.cCore_Vouchers_0 WHERE (iVoucherType =" + iRctprodvtype + " )";
                vRctprodAbbr = Convert.ToString(clsGeneric.ShowRecord(miCompanyId, strQry));
                strQry = $@"SELECT  sName  FROM  dbo.cCore_Vouchers_0 WHERE (iVoucherType =" + iRctprodvtype + " )";
                vRctprodName = Convert.ToString(clsGeneric.ShowRecord(miCompanyId, strQry));

                BL_DB DataAcesslayer = new BL_DB();
                
                
                using (var clientIssPro = new WebClient())
                {
                    clientIssPro.Encoding = Encoding.UTF8;
                    clientIssPro.Headers.Add("fSessionId", msSessionId);
                    clientIssPro.Headers.Add("Content-Type", "application/json");
                    clsGeneric.writeLog("Download Rct form Prod URL: " + "http://localhost/Focus8API/Screen/Transactions/" + iRctprodvtype + "/" + sRctproddocNo);
                    var rIssPro = clientIssPro.DownloadString("http://localhost/Focus8API/Screen/Transactions/" + iRctprodvtype + "/" + sRctproddocNo);
                    clsGeneric.writeLog("response Rct form Prod: " + rIssPro);
                    if (rIssPro != null)
                    {
                        var rDIssPro = JsonConvert.DeserializeObject<APIResponse.PostResponse>(rIssPro);
                        if (rDIssPro.result == -1)
                        {
                            return false;
                        }
                        else
                        {
                            if (rDIssPro.data.Count != 0)
                            {
                                var extHeader = JsonConvert.DeserializeObject<Hashtable>(JsonConvert.SerializeObject(rDIssPro.data[0]["Header"]));
                                if (rDIssPro.data[0]["Footer"].ToString() != "[]")
                                {
                                    var extFooter = JsonConvert.DeserializeObject<List<Hashtable>>(JsonConvert.SerializeObject(rDIssPro.data[0]["Footer"]));
                                }
                                var extBody = JsonConvert.DeserializeObject<List<Hashtable>>(JsonConvert.SerializeObject(rDIssPro.data[0]["Body"]));
                                //As per suggestion from Majid modified by Shaikh Azhar @ 24-02-2023 
                                //iDeptId = Convert.ToInt32(extHeader["Department__Id"]);
                                iDeptId = Convert.ToInt32(extHeader["Unit Location__Id"]);
                                idocdate = Convert.ToInt32(extHeader["Date"]);
                                iPOid = Convert.ToInt32(extHeader["BatchNo"]);
                                sPONO = clsGeneric.ShowRecord(miCompanyId, "Select sProdOrderNo from tMrp_ProdOrder_0 where iProdOrderId=" + iPOid);
                                dQty = Convert.ToDecimal(extHeader["ProdQty"].ToString());
                                iFgId = Convert.ToInt32(extHeader["FinishedGoods__Id"]);

                                int tempFor1 = extBody.Count - 1;
                                for (int i = 0; i <= tempFor1; i++)
                                {
                                    iItemid = 0;
                                    dItemQty = 0;
                                    dItemRt = 0;
                                    iBodyId = 0;
                                    iBatchid = 0;
                                    sbatchNo = "";
                                    sMfgDt = "";

                                    iItemid = Convert.ToInt32(extBody[i]["Item__Id"]);
                                    dItemQty = Convert.ToDecimal(extBody[i]["Quantity"]);
                                    dItemRt = Convert.ToDecimal(extBody[i]["Rate"]);
                                    iBodyId = Convert.ToInt32(extBody[i]["TransactionId"]);
                                    iWId = Convert.ToInt32(extBody[i]["Warehouse__Id"]);
                                    iBatchid = Convert.ToInt32(JsonConvert.DeserializeObject<Hashtable>(JsonConvert.SerializeObject(extBody[i]["Batch"]))["BatchId"]);
                                    sbatchNo = Convert.ToString(JsonConvert.DeserializeObject<Hashtable>(JsonConvert.SerializeObject(extBody[i]["Batch"]))["BatchNo"]);
                                    sMfgDt = Convert.ToString(JsonConvert.DeserializeObject<Hashtable>(JsonConvert.SerializeObject(extBody[i]["Batch"]))["MfgDate"]);
                                    strsql = $@" Select QCRequire from muCore_Product_Units where iMasterId ="+ iItemid ;
                                    iQCRequire = Convert.ToInt32(clsGeneric.ShowRecord(miCompanyId, strsql));
                                    // iQCRequire Change suggested By Mukram Sahab Dated 26-02-2023 Done Shaikh Azhar 

                                    if (iQCRequire == 1)
                                    { 
                                        iStatus = 1;
                                        strQry = $@"insert into ICSPendingIssProQC " + 
                                              "(vNo,docDate,vType,itemId,qty,stkRate,warehouseId,PONo,POID,batchNo,mfgdt,batch_id,Dept_Id,[status],iBody_id) Values  ( '"  +
                                               msdocNo + "'," + idocdate + "," + mivtype + "," + iItemid + "," + dItemQty + "," + dItemRt + "," + iWId + ",'"+ sPONO  + "'," + iPOid + ",'" + sbatchNo + "','" + sMfgDt + "'," + iBatchid + "," + iDeptId + ","+ iStatus  + "," + iBodyId + ")";
                                        clsGeneric.writeLog("strQry:" + strQry);

                                        DataAcesslayer.GetExecute(strQry, miCompanyId, ref strErrorMessage);
                                    }
                                    //return true;


                                }
                                return true;

                            }
                        }
                    }
                } // end using 

                return true;

            } // End Try 
            catch (Exception ex)
            {
                clsGeneric.writeLog("Exception occured:" + (ex.Message));
                //return Json(new { status = false, data = new { message = ex.Message + "Posting Failed " } });
                msg.Clear();
                msg.Append(ex.Message);
                return false;
                throw;
            } //End Catch
        }

    }
}


