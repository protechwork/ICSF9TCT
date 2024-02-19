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
    
    public class RelProdOrdController : Controller
    {
        #region Variable Definition
        string strQry = string.Empty;
        string strErrorMessage = string.Empty;
        int iReleaseProdnOrder = 0;
        //string PPCDocNo = "";
        int FailedProdOrdCnt = 0;
        
        
        string sPPCPlanNo = "";
        string sPlanMonth = "";
        int sWeekEndDate = 0;
        int sWeekStartDate = 0;
        int sWeekDays = 0;
        int iDueDate = 0;
        Int32 iBookNo = 0;
        Int32 BomId = 0;
        Int32 DeptId = 0;
        Int32 iFaTag = 0;
        public static Int32 iBodyId = 0;
        Int32 iInvTag = 0;
        
        
        Int32 iCode = 0;
        Int32 iFGCode = 0;
        Int32 iDate = 0;
        Int32 iProduct = 0;
        Int32 iUnitId = 0;
        decimal fQuantity = 0;
        
        Int32 WCId = 0;
        //string vRMReqAbbr = "RM Req";
        //Int32 vRMReqType = 0;
        //string vRMReqName = "";
        int iRecordCnt = 0;
        string FGCode = "";
        
        
        
        
        
        
        
        
        string PostDocNo = ""; 
        BL_DB DataAcesslayer = new BL_DB();
        #endregion
        public ActionResult UpdateRelProdOrd(int CompanyId, string SessionId, string User, int LoginId,  int vtype, string docNo, List<PEBody> BodyData)
        {
            clsGeneric.Log_write(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), SessionId, CompanyId, docNo, vtype, User);

            //clsGeneric.createTableCollectSFG_PPCPlan(CompanyId, User, docNo);
            BL_DB DataAcesslayer = new BL_DB();
            strQry = $@"SELECT tchd.ReleaseProdnOrder FROM dbo.tCore_Header_0 AS tch INNER JOIN dbo.tCore_HeaderData" + vtype + "_0 AS tchd ON tch.iHeaderId = tchd.iHeaderId " +
                     " WHERE (tch.sVoucherNo = N'" + docNo + "') AND (tch.iVoucherType = " + vtype + ")";
            iReleaseProdnOrder = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, strQry));

            //strQry = $@"SELECT tchd.ReleaseRMReq FROM dbo.tCore_Header_0 AS tch INNER JOIN dbo.tCore_HeaderData" + vtype + "_0 AS tchd ON tch.iHeaderId = tchd.iHeaderId " +
            //         " WHERE (tch.sVoucherNo = N'" + docNo + "') AND (tch.iVoucherType = " + vtype + ")";
            //iReleaseRMReq = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, strQry));



            //strQry = $@"select iVoucherType from  [dbo].[cCore_Vouchers_0] where sAbbr = '" + vRMReqAbbr + "'";
            //vRMReqType = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, strQry));
            //strQry = $@"select Sname from  [dbo].[cCore_Vouchers_0] where sAbbr = '" + vRMReqAbbr + "'";
            //vRMReqName = Convert.ToString(clsGeneric.ShowRecord(CompanyId, strQry));
            //PPCDocNo = docNo;

            if (iReleaseProdnOrder != 0)
            {
                try
                {
                    //if (iReleaseProdnOrder != 0)
                    //{
                        iBodyId = 0;
                    strQry = $@" SELECT  tcdn.ProdnPlanNo, tcdn.ProdnPlanDate, tcd.iBookNo, tcd.iFaTag, tcd.iInvTag AS warehouse, tcd.iCode, tcdn.FGCode, tcdn.RMReqStatus, tci.iProduct, tci.fQuantity, tci.iUnit, tch.iDate,tcd.iBodyId, tcd.iDueDate, tcdn.ProdnOrderStatus " +
                        " FROM dbo.tCore_Header_0 AS tch INNER JOIN dbo.tCore_HeaderData" + vtype + "_0 AS tchd ON tch.iHeaderId = tchd.iHeaderId INNER JOIN dbo.tCore_Data_0 AS tcd ON tch.iHeaderId = tcd.iHeaderId INNER JOIN " +
                         " dbo.tCore_Data" + vtype + "_0 AS tcdn ON tcd.iBodyId = tcdn.iBodyId INNER JOIN dbo.tCore_Indta_0 AS tci ON tcd.iBodyId = tci.iBodyId " +
                         //"INNER JOIN dbo.tCore_Data_Tags_0 AS tcdt ON tcd.iBodyId = tcdt.iBodyId" +
                         " INNER JOIN  dbo.muCore_Product_Settings AS CPS ON tci.iProduct = CPS.iMasterId " +
                         " WHERE  (tcdn.ProdnOrderStatus = 0) and  (tch.sVoucherNo = N'" + docNo + "') AND (tch.iVoucherType = " + vtype + ")";
                             //AND (isnull(CPS.IsJWItem,0) = 0)";
                        DataSet ds1 = DataAcesslayer.GetData(strQry, CompanyId, ref strErrorMessage);
                        iRecordCnt = ds1.Tables[0].Rows.Count;
                        if (ds1 != null && ds1.Tables.Count > 0 && iRecordCnt > 0)
                        {
                            sPPCPlanNo = Convert.ToString(ds1.Tables[0].Rows[0][0]);
                            sPlanMonth = Convert.ToString(ds1.Tables[0].Rows[0][1]);
                            iBookNo = Convert.ToInt32(ds1.Tables[0].Rows[0][2]);
                            iFaTag = Convert.ToInt32(ds1.Tables[0].Rows[0][3]); //Branch
                            // as per suggested by Majid, Mukram and Kha Sahab dated @20-04-2023 
                            DeptId = Convert.ToInt32(ds1.Tables[0].Rows[0][3]); //Branch
                            iCode = Convert.ToInt32(ds1.Tables[0].Rows[0][5]);
                            iFGCode = Convert.ToInt32(ds1.Tables[0].Rows[0][6]);
                            iDate = Convert.ToInt32(ds1.Tables[0].Rows[0][11]);
                        //sWeekEndDate = Convert.ToInt32(ds1.Tables[0].Rows[0][15]);
                        //sWeekStartDate = Convert.ToInt32(ds1.Tables[0].Rows[0][16]);
                        //sWeekDays = Convert.ToInt32(ds1.Tables[0].Rows[0][17]);
                        iDueDate = Convert.ToInt32(ds1.Tables[0].Rows[0][13]);

                    }
                        iBodyId = 0;
                        int t = 1;
                        int tt = 0;
                        for (int i = 0; i <= iRecordCnt - 1; i++)
                        {
                            tt++;
                            iInvTag = Convert.ToInt32(ds1.Tables[0].Rows[i][4]); // warehouse 
                            iProduct = Convert.ToInt32(ds1.Tables[0].Rows[i][8]); // Product 
                            fQuantity = Convert.ToDecimal(ds1.Tables[0].Rows[i][9]); // Qty 
                            //WCId = Convert.ToInt32(ds1.Tables[0].Rows[i][13]); // WorkCentre  

                            strQry = $@"Select SCode from mCore_Product where (iMasterId=" + iProduct + ")";
                            FGCode = Convert.ToString(clsGeneric.ShowRecord(CompanyId, strQry));

                            strQry = $@"select Top 1 iVariantId from ICS_BOMVariant where (iProductId=" + iProduct + ") Order by Iversion desc";
                            BomId = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, strQry));
                           // as per suggested by Majid, Mukram and Kha Sahab dated @20-04-2023 
                           // strQry = $@"SELECT ISNULL(iTagValue, 0) AS iTagValue FROM dbo.mMRP_BomHeader AS MBH WHERE (iBomId =" + BomId + ")";
                           // DeptId = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, strQry));

                            strQry = $@"select iDefaultBaseUnit from muCore_Product_Units where (iMasterId=" + iProduct + ")";
                            iUnitId = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, strQry));
                            iBodyId = Convert.ToInt32(ds1.Tables[0].Rows[i][12]);

                            HashData objHashRequest = new HashData();
                            Hashtable headerProd_Ord = new Hashtable();
                        //PostDocNo = FGCode + "/" + clsGeneric.GetIntToDate(sWeekStartDate).Year.ToString().Substring(2, 2).ToString() + clsGeneric.GetIntToDate(sWeekStartDate).Month.ToString("d2") + "/" + docNo + "/" + clsGeneric.GetIntToDate(sWeekStartDate).ToString("ddMM") + "-" + clsGeneric.GetIntToDate(sWeekEndDate).ToString("ddMM") + "/" + tt;
                        //PostDocNo = FGCode + "/" + docNo + "/" + clsGeneric.GetIntToDate(iDueDate).ToString("ddMMyyyy") + "/" + tt;
                        // above one is old style, below new Suggested By Majid Sir and instructed By Kha Shahb  
                        PostDocNo = FGCode + "-" + docNo + "-" + tt;
                        headerProd_Ord.Add("productionorderno", PostDocNo );
                            
                        //clsGeneric.GetIntToDate(sWeekStartDate).ToString("MM/dd") + "-" + clsGeneric.GetIntToDate(sWeekEndDate).ToString("MM/dd")
                        headerProd_Ord.Add("date", clsGeneric.GetIntToDate(iDate).ToString());
                            //DateTime lastDate = new DateTime(clsGeneric.GetIntToDate(iDate).Year, clsGeneric.GetIntToDate(iDate).Month, 1).AddMonths(1).AddDays(-1);
                            DateTime lastDate = GetLastDayOfNextMonth(iDate);
                        headerProd_Ord.Add("duedate", clsGeneric.GetIntToDate(iDueDate).ToString());
                        //headerProd_Ord.Add("duedate", iDueDate);
                        headerProd_Ord.Add("product__id", iProduct);
                            //headerProd_Ord.Add("customer__id", 19);
                            headerProd_Ord.Add("quantity", fQuantity);
                            headerProd_Ord.Add("TagFilterValue", DeptId);
                        

                            headerProd_Ord.Add("unit__id", iUnitId);
                            headerProd_Ord.Add("warehouse__id", iInvTag);
                            headerProd_Ord.Add("OrderStatus", 2);
                            headerProd_Ord.Add("Issue Type", "Single Level");
                            //headerProd_Ord.Add("purchasetypeid", 2);
                            headerProd_Ord.Add("remarks", "Remark");
                            List<Hashtable> lstHash = new List<Hashtable>();
                            lstHash.Add(headerProd_Ord);
                            objHashRequest.data = lstHash;
                            string sContent = JsonConvert.SerializeObject(objHashRequest);
                            clsGeneric.writeLog(" Contents :" + sContent);
                            clsGeneric.writeLog(" URL :" + "http://localhost/Focus8API/MRP/MakeToOrder");
                            using (var client = new WebClient())
                            {
                                client.Headers.Add("fSessionId", SessionId);
                                client.Headers.Add("Content-Type", "application/json");
                                string sUrl = "http://localhost/Focus8API/MRP/MakeToOrder";
                                string strResponse = client.UploadString(sUrl, sContent);
                                clsGeneric.writeLog(" strResponse :" + strResponse);
                                var objHashResponse = JsonConvert.DeserializeObject<APIResponse.PostResponse>(strResponse);
                                if (objHashResponse.result == -1)
                                {

                                    FailedProdOrdCnt++;
                                }
                                else
                                {
                                    FailedProdOrdCnt = 0;
                                    strErrorMessage = "";
                                    strErrorMessage = "";
                                    strQry = $@"update tMrp_ProdOrder_0  set iTagFilterId=3,sSONO='" + docNo + "/" + t.ToString() + "', iTagFilterValue=" + DeptId + ",iOrderStatus=2 where sProdOrderNo='" + PostDocNo + "'";
                                    DataAcesslayer.GetExecute(strQry, CompanyId, ref strErrorMessage);
                                    
                                    strQry = $@"update tCore_Data" + vtype + "_0  set ProdOrderStatus=1 where iBodyId=" + iBodyId;
                                    iBodyId = 0;
                                    DataAcesslayer.GetExecute(strQry, CompanyId, ref strErrorMessage);

                                }
                                t++;
                            } // end using (var client = new WebClient())


                        } // end of for (int i = 0; i <= ds1.Tables[0].Rows.Count - 1; i++)




                    //}
                    // --------------------------------------- RM Requisition -------------------
                    //if (iReleaseRMReq != 0)
                    //{
                    //    clsGeneric.createTableRMRequisition(CompanyId, User, docNo);


                    //    iBodyId = 0;
                    //    strQry = $@" SELECT tchd.PPCPlanNo, tchd.PlanMonth, tcd.iBookNo, tcd.iFaTag, tcd.iInvTag AS warehouse, tcd.iCode, tcdn.FGCode, tcdn.ProdOrderStatus, tcdn.RMReqStatus, tci.iProduct, tci.fQuantity, tci.iUnit, tch.iDate, tcdt.iTag3012 AS WC,tcdt.iBodyId " +
                    //" FROM dbo.tCore_Header_0 AS tch INNER JOIN dbo.tCore_HeaderData" + vtype + "_0 AS tchd ON tch.iHeaderId = tchd.iHeaderId INNER JOIN dbo.tCore_Data_0 AS tcd ON tch.iHeaderId = tcd.iHeaderId INNER JOIN " +
                    // " dbo.tCore_Data" + vtype + "_0 AS tcdn ON tcd.iBodyId = tcdn.iBodyId INNER JOIN dbo.tCore_Indta_0 AS tci ON tcd.iBodyId = tci.iBodyId INNER JOIN dbo.tCore_Data_Tags_0 AS tcdt ON tcd.iBodyId = tcdt.iBodyId " +
                    // " WHERE  (RMReqStatus=0)  and (tch.sVoucherNo = N'" + docNo + "') AND (tch.iVoucherType = " + vtype + ")";
                    //    DataSet ds2 = DataAcesslayer.GetData(strQry, CompanyId, ref strErrorMessage);
                    //    if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                    //    {
                    //        sPPCPlanNo = Convert.ToString(ds2.Tables[0].Rows[0][0]);
                    //        sPlanMonth = Convert.ToString(ds2.Tables[0].Rows[0][1]);
                    //        iBookNo = Convert.ToInt32(ds2.Tables[0].Rows[0][2]);
                    //        iFaTag = Convert.ToInt32(ds2.Tables[0].Rows[0][3]); //Branch

                    //        iCode = Convert.ToInt32(ds2.Tables[0].Rows[0][5]);
                    //        iFGCode = Convert.ToInt32(ds2.Tables[0].Rows[0][6]);
                    //        iDate = Convert.ToInt32(ds2.Tables[0].Rows[0][12]);

                    //    }
                    //    iBodyId = 0;

                    //    int tt = 1;
                    //    for (int i = 0; i <= ds2.Tables[0].Rows.Count - 1; i++)
                    //    {

                    //        iInvTag = Convert.ToInt32(ds2.Tables[0].Rows[i][4]); // warehouse 

                    //        iProduct = Convert.ToInt32(ds2.Tables[0].Rows[i][9]); // Product 
                    //        fQuantity = Convert.ToDecimal(ds2.Tables[0].Rows[i][10]); // Qty 
                    //        WCId = Convert.ToInt32(ds2.Tables[0].Rows[i][13]); // WorkCentre  

                    //        strQry = $@"Select SCode from mCore_Product where (iMasterId=" + iProduct + ")";
                    //        FGCode = Convert.ToString(clsGeneric.ShowRecord(CompanyId, strQry));

                    //        strQry = $@"select Top 1 iVariantId from ICS_BOMVariant where (iProductId=" + iProduct + ") Order by Iversion desc";
                    //        BomId = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, strQry));
                    //        iBodyId = Convert.ToInt32(ds2.Tables[0].Rows[i][14]);
                    //        //strQry = $@"SELECT ISNULL(iTagValue, 0) AS iTagValue FROM dbo.mMRP_BomHeader AS MBH WHERE (iBomId =" + BomId + ")";
                    //        //DeptId = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, strQry));

                    //        strQry = $@"select iDefaultBaseUnit from muCore_Product_Units where (iMasterId=" + iProduct + ")";
                    //        iUnitId = Convert.ToInt32(clsGeneric.ShowRecord(CompanyId, strQry));




                    //        strQry = $@"insert into RMRequisition (PlanMonth,Product,RMqty,fQty,planQty,SalQty,BRMReq,RMStock,branch,worksCenter,ParentId,warehouse,Preq,iBodyId, vno, loggeduser)" +
                    //            " SELECT '" + sPlanMonth + "',iProductId, fQty,'" + fQuantity + "','" + fQuantity + "',fQty * " + fQuantity + ", (fQty * " + fQuantity + ")* fQty ,(select sum(fQiss+fQrec) from tCore_ibals_0 where (iProduct=D.iProductId) AND (iInvTag =d.iInvTagValue)), " + iFaTag + "," + WCId + "," + iProduct + ", d.iInvTagValue ,0," + iBodyId + ",'" + docNo + "','" + User + "' FROM  dbo.mMRP_BOMBody AS D WHERE(iVariantId IN(SELECT iVariantId" +
                    //            " FROM dbo.ICS_BOMVariant AS VB WHERE(iProductId = " + iProduct + "))) AND(bMainOutPut = 0) AND(bInput = 1) AND(iProductId NOT IN(SELECT iProductId FROM  dbo.ICS_BOMVariant AS Vc))";
                    //        DataAcesslayer.GetExecute(strQry, CompanyId, ref strErrorMessage);
                    //        if (strErrorMessage != "")
                    //        {
                    //            clsGeneric.writeLog("Error :-" + strErrorMessage);
                    //        }




                    //    } // end for for (int i = 0; i <= ds2.Tables[0].Rows.Count - 1; i++) 



                    //    strQry = $@"Select id,PlanMonth,Branch, WorksCenter,Warehouse,Product,ParentID,MParentID,Fqty,RMQty,PlanQty, SalQty, RMStock, PReq,VStock, BRMReq, Q2P,iBodyId from RMRequisition where vno='" + docNo + "' and loggeduser='" + User + "' order by id";
                    //    DataSet ds3 = DataAcesslayer.GetData(strQry, CompanyId, ref strErrorMessage);
                    //    if (strErrorMessage != "")
                    //    {
                    //        clsGeneric.writeLog("Error :-" + strErrorMessage);
                    //    }

                    //    if (ds3 != null && ds3.Tables.Count > 0 && ds3.Tables[0].Rows.Count > 0)
                    //    {

                    //        for (int i = 0; i <= ds3.Tables[0].Rows.Count - 1; i++)
                    //        {
                    //            strQry = "Select Sum(BRMReq) from RMRequisition where  id!=" + Convert.ToInt32(ds3.Tables[0].Rows[i][0]) + " and id<" + Convert.ToInt32(ds3.Tables[0].Rows[i][0]) + " and product=" + Convert.ToInt32(ds3.Tables[0].Rows[i][5]) + " and warehouse=" + Convert.ToInt32(ds3.Tables[0].Rows[i][4]) + " and vno='" + docNo + "' and loggeduser='" + User + "'";
                    //            iPreq = Convert.ToDecimal(clsGeneric.ShowRecord(CompanyId, strQry ));
                    //            strQry = $@"update RMRequisition set Preq=" + iPreq +  " where vno='" + docNo + "' and loggeduser='" + User + "' and id=" + Convert.ToInt32(ds3.Tables[0].Rows[i][0]);
                    //            DataAcesslayer.GetExecute(strQry, CompanyId, ref strErrorMessage);

                    //        }

                    //    }
                    //    strQry = $@"update RMRequisition set VStock= case when (RMStock-PReq) > 0 then  RMStock-PReq else 0 end where vno='" + docNo + "' and loggeduser='" + User + "'";
                    //    DataAcesslayer.GetExecute(strQry, CompanyId, ref strErrorMessage);
                    //    //Q2P=case when (case when(RMStock - PReq) > 0 then RMStock-PReq else 0 end > 0 ) then BRMreq-case when (case when(RMStock - PReq) > 0 then RMStock-PReq else 0 end-PReq) > 0 then  RMStock-PReq else 0 end else 0 end,
                    //    //strQry = $@"update RMRequisition set Q2P= case when VStock > 0 then  BRMReq-VStock else 0 end where vno='" + docNo + "' and loggeduser='" + User + "'";
                    //    strQry = $@"update RMRequisition set Q2P= case when VStock < BRMReq then  BRMReq-VStock else 0 end where vno='" + docNo + "' and loggeduser='" + User + "'";
                    //    DataAcesslayer.GetExecute(strQry, CompanyId, ref strErrorMessage);

                    //    //might be sending required here
                    //    strQry = $@"SELECT Branch FROM  dbo.RMRequisition AS RMR1 WHERE (Q2P > 0) AND vno='" + docNo + "' and loggeduser='" + User + "' GROUP BY Branch ";
                    //    DataSet ds_RMR1 = DataAcesslayer.GetData(strQry, CompanyId, ref strErrorMessage);
                    //    if (ds_RMR1 != null && ds_RMR1.Tables.Count > 0 && ds_RMR1.Tables[0].Rows.Count > 0)
                    //    {

                    //        for (int i = 0; i <= ds_RMR1.Tables[0].Rows.Count - 1; i++)
                    //        {
                    //            BranchFilter = Convert.ToInt32(ds_RMR1.Tables[0].Rows[i][0]);
                    //            strQry = $@"SELECT WorksCenter FROM dbo.RMRequisition as RMR2 Where (Q2P > 0) AND vno='" + docNo + "' and loggeduser='" + User + "' AND (Branch =" + BranchFilter + ") GROUP BY WorksCenter";
                    //            DataSet ds_RMR2 = DataAcesslayer.GetData(strQry, CompanyId, ref strErrorMessage);
                    //            if (ds_RMR2 != null && ds_RMR2.Tables.Count > 0 && ds_RMR2.Tables[0].Rows.Count > 0)
                    //            {


                    //                for (int j = 0; j <= ds_RMR2.Tables[0].Rows.Count - 1; j++)
                    //                {

                    //                    WCFilter = Convert.ToInt32(ds_RMR2.Tables[0].Rows[j][0]);

                    //                    HashData objHashRequest_RMReq = new HashData();
                    //                    List<System.Collections.Hashtable> lstBodyRMReq = new List<System.Collections.Hashtable>();
                    //                    HashData objHashRequest = new HashData();
                    //                    Hashtable headerRMReq = new Hashtable();
                    //                    headerRMReq.Add("DocNo", null);
                    //                    headerRMReq.Add("Date", getNextMonthStartDate(iDate));
                    //                    headerRMReq.Add("Branch__Id", BranchFilter);
                    //                    headerRMReq.Add("Dept__Id", 3);
                    //                    headerRMReq.Add("Works Center__Id", WCFilter);
                    //                    headerRMReq.Add("Warehouse__Id", iInvTag);

                    //                    strQry = $@"SELECT id, PlanMonth, Branch, WorksCenter, Warehouse, Product, ParentID, MParentID, Fqty, RMQty, PlanQty, SalQty, isnull(RMStock,0) as RMStock, PReq, VStock, BRMReq, Q2P,iBodyId FROM dbo.RMRequisition Where (Q2P > 0) AND vno='" + docNo + "' and loggeduser='" + User + "' AND (Branch =" + BranchFilter + ") AND (WorksCenter = "+ WCFilter + ")";
                    //                    DataSet ds_RMR3 = DataAcesslayer.GetData(strQry, CompanyId, ref strErrorMessage);
                    //                    if (ds_RMR3 != null && ds_RMR3.Tables.Count > 0 && ds_RMR3.Tables[0].Rows.Count > 0)
                    //                    {

                    //                        for (int k = 0; k <= ds_RMR3.Tables[0].Rows.Count - 1; k++)
                    //                        {
                    //                            Hashtable bodyRMReq = new Hashtable();
                    //                            bodyRMReq.Add("Item__Id", Convert.ToInt32(ds_RMR3.Tables[0].Rows[k][5]));
                    //                            iBodyId = Convert.ToInt32(ds_RMR3.Tables[0].Rows[k][17]);
                    //                            StockAvailable = Convert.ToDecimal(ds_RMR3.Tables[0].Rows[k][12]);
                    //                            Hashtable bodyStockAvailableRMReq = new Hashtable
                    //                                {
                    //                                    {"Input" , StockAvailable},
                    //                                    {"FieldName", "Stock Available"},
                    //                                    {"ColMap", 0},
                    //                                    {"Value", StockAvailable}
                    //                                };
                    //                            SalQty = Convert.ToDecimal(ds_RMR3.Tables[0].Rows[k][11]);
                    //                            Hashtable bodySalQtyRMReq = new Hashtable
                    //                                {
                    //                                  {"Input" , SalQty},
                    //                                  {"FieldName", "SalQty"},
                    //                                  {"ColMap", 1},
                    //                                  {"Value", SalQty}
                    //                                };
                    //                            RMStock = Convert.ToDecimal(ds_RMR3.Tables[0].Rows[k][12]);
                    //                            Hashtable bodyRMStockRMReq = new Hashtable
                    //                                {
                    //                                  {"Input" , RMStock},
                    //                                  {"FieldName", "RM Stock"},
                    //                                  {"ColMap", 2},
                    //                                  {"Value", RMStock}
                    //                                };
                    //                            PrevRMReq = Convert.ToDecimal(ds_RMR3.Tables[0].Rows[k][13]);
                    //                            Hashtable bodyPrevRMReqRMReq = new Hashtable
                    //                                {
                    //                                  {"Input" , PrevRMReq},
                    //                                  {"FieldName", "Prev RM Req"},
                    //                                  {"ColMap", 3},
                    //                                  {"Value", PrevRMReq}
                    //                                };
                    //                            VStock = Convert.ToDecimal(ds_RMR3.Tables[0].Rows[k][14]);
                    //                            Hashtable bodyVStockRMReq = new Hashtable
                    //                                {
                    //                                  {"Input" , VStock},
                    //                                  {"FieldName", "Prev RM Req"},
                    //                                  {"ColMap", 4},
                    //                                  {"Value", VStock}
                    //                                };
                    //                            BRMReq = Convert.ToDecimal(ds_RMR3.Tables[0].Rows[k][15]);
                    //                            Hashtable bodyBRMReqRMReq = new Hashtable
                    //                                {
                    //                                  {"Input" , BRMReq},
                    //                                  {"FieldName", "BRMReq"},
                    //                                  {"ColMap", 5},
                    //                                  {"Value", BRMReq}
                    //                                };
                    //                            Q2P = Convert.ToDecimal(ds_RMR3.Tables[0].Rows[k][16]);
                    //                            Hashtable bodyQ2pRMReq = new Hashtable
                    //                                {
                    //                                  {"Input" , Q2P},
                    //                                  {"FieldName", "Q2P"},
                    //                                  {"ColMap", 6},
                    //                                  {"Value", Q2P}
                    //                                };
                    //                            bodyRMReq.Add("Quantity", Q2P);
                    //                            //List<System.Collections.Hashtable> lstStockAvailable = new List<System.Collections.Hashtable>();
                    //                            //lstStockAvailable.Add(bodyStockAvailableRMReq);
                    //                            //List<System.Collections.Hashtable> lstSalQty = new List<System.Collections.Hashtable>();
                    //                            //lstSalQty.Add(bodySalQtyRMReq);
                    //                            //List<System.Collections.Hashtable> lstRMStock = new List<System.Collections.Hashtable>();
                    //                            //lstRMStock.Add(bodyRMStockRMReq);
                    //                            //List<System.Collections.Hashtable> lstPrevRMReq = new List<System.Collections.Hashtable>();
                    //                            //lstPrevRMReq.Add(bodyPrevRMReqRMReq);
                    //                            //List<System.Collections.Hashtable> lstVStock = new List<System.Collections.Hashtable>();
                    //                            //lstVStock.Add(bodyVStockRMReq);
                    //                            //List<System.Collections.Hashtable> lstBRMReq = new List<System.Collections.Hashtable>();
                    //                            //lstBRMReq.Add(bodyBRMReqRMReq);
                    //                            //List<System.Collections.Hashtable> lstQ2pRMReq = new List<System.Collections.Hashtable>();
                    //                            //lstQ2pRMReq.Add(bodyQ2pRMReq);
                    //                            bodyRMReq.Add("Stock Available", bodyStockAvailableRMReq);
                    //                            bodyRMReq.Add("SalQty", bodySalQtyRMReq);
                    //                            bodyRMReq.Add("RM Stock", bodyRMStockRMReq);
                    //                            bodyRMReq.Add("Prev RM Req", bodyPrevRMReqRMReq);
                    //                            bodyRMReq.Add("V Stock", bodyVStockRMReq);
                    //                            bodyRMReq.Add("BRMReq", bodyBRMReqRMReq);
                    //                            bodyRMReq.Add("Q2P", bodyQ2pRMReq);
                    //                            lstBodyRMReq.Add(bodyRMReq);

                    //                        }//for (int k = 0; k <= ds_RMR3.Tables[0].Rows.Count - 1; k++)

                    //                        System.Collections.Hashtable objHash = new System.Collections.Hashtable();
                    //                        objHash.Add("Body", lstBodyRMReq);
                    //                        objHash.Add("Header", headerRMReq);

                    //                        List<System.Collections.Hashtable> lstHash = new List<System.Collections.Hashtable>();
                    //                        lstHash.Add(objHash);
                    //                        objHashRequest.data = lstHash;
                    //                        string sContentRMReq = JsonConvert.SerializeObject(objHashRequest);
                    //                        clsGeneric.writeLog("URL :" + "http://localhost/Focus8API/Transactions/Vouchers/" + vRMReqName);
                    //                        clsGeneric.writeLog("URL Param :" + sContentRMReq);
                    //                        clsGeneric.writeLog("Param Bodyid:" + iBodyId);
                    //                        using (var clientRMReq = new WebClient())
                    //                        {

                    //                            clientRMReq.Encoding = Encoding.UTF8;
                    //                            clientRMReq.Headers.Add("fSessionId", SessionId);
                    //                            clientRMReq.Headers.Add("Content-Type", "application/json");

                    //                            var responseRMReq = clientRMReq.UploadString("http://localhost/Focus8API/Transactions/Vouchers/" + vRMReqName, sContentRMReq);
                    //                            clsGeneric.writeLog("Response form RMReq :" + (responseRMReq));
                    //                            if (responseRMReq != null)
                    //                            {
                    //                                var responseDataRMReq = JsonConvert.DeserializeObject<APIResponse.PostResponse>(responseRMReq);
                    //                                if (responseDataRMReq.result == -1)
                    //                                {
                    //                                    //return Json(new { status = false, data = new { message = responseDataRMReq.message } });
                    //                                    FailedRMReqCnt = FailedRMReqCnt + 1; 
                    //                                }
                    //                                else
                    //                                {
                    //                                    FailedRMReqCnt = 0;
                    //                                    //return Json(new { status = true, data = new { message = "Posting Successful" } });
                    //                                    strQry = $@"update tCore_Data" + vtype + "_0  set RMReqStatus=1 where iBodyId=" + iBodyId;
                    //                                    DataAcesslayer.GetExecute(strQry, CompanyId, ref strErrorMessage);

                    //                                }
                    //                            } // end of (responseRMReq != null)
                    //                        } // For using (var clientRMReq = new WebClient()) 

                    //                    }//if (ds_RMR3 != null && ds_RMR3.Tables.Count > 0 && ds_RMR3.Tables[0].Rows.Count > 0)
                    //                }//for (int j = 0; j <= ds_RMR2.Tables[0].Rows.Count - 1; j++)
                    //            }//if (ds_RMR2 != null && ds_RMR2.Tables.Count > 0 && ds_RMR2.Tables[0].Rows.Count > 0)
                    //        }//for (int i = 0; i <= ds_RMR1.Tables[0].Rows.Count - 1; i++)
                    //    }//if (ds_RMR1 != null && ds_RMR1.Tables.Count > 0 && ds_RMR1.Tables[0].Rows.Count > 0)
                    //    else
                    //    {
                    //        FailedRMReqCnt++;
                    //        clsGeneric.writeLog("Last Query" + strQry);
                    //    }




                    //} // end of (iReleaseRMReq != 0)


                }
                catch (Exception ex)
                {
                    //clsGeneric.createTableCollectSFG_PPCPlan(CompanyId, User, docNo);
                    clsGeneric.writeLog("Exception occured:" + (ex.Message));
                    return Json(new { status = false, data = new { message = "Posting Failed " } });
                    throw;
                }
            }
            else
            {
                return Json(new { status = false, data = new { message = "No Action Define, Release Production order Or RM Requisition" } });
            }
            if (FailedProdOrdCnt == 0)
            {

                

                
                if ((iReleaseProdnOrder == 1) && (FailedProdOrdCnt != 0))
                {
                    return Json(new { status = false, data = new { message = "Posting Failed " } });
                }
                else
                {
                    if (iRecordCnt ==0 )
                    {
                        return Json(new { status = true, data = new { message = "Release Production not applied for Job Work Item's" } });
                    }
                    else
                    {
                        return Json(new { status = true, data = new { message = "Posted Successful" } });
                    }
                    
                    
                }

            }
            else
            {
                return Json(new { status = false, data = new { message = "Posting Failed " } });
            }
            
        }
        static void UpdateStatusofRMnotPosted(int Type, string vno, int CompanyId)
        {
            BL_DB DataAcesslayer = new BL_DB();
            string strErrorMessage = string.Empty;
            string strValue = "";

            
            strValue = $@"Update tCore_Data" + Type  + "_0 set RMReqStatus = 1 FROM dbo.tCore_Data_0 AS TCD INNER JOIN " +
                " dbo.tCore_Header_0 AS TCH ON TCD.iHeaderId = TCH.iHeaderId INNER JOIN dbo.tCore_Data"+ Type + "_0 AS TCDN ON " +
                " TCD.iBodyId = TCDN.iBodyId WHERE (TCH.sVoucherNo = N'" + vno  + "') AND (TCH.iVoucherType = " + Type  + ") AND (TCDN.RMReqStatus = 0)";
            clsGeneric.writeLog("Query :" + strValue);
            DataAcesslayer.GetExecute(strValue, CompanyId, ref strErrorMessage);
            if (strErrorMessage != "")
            {
                clsGeneric.writeLog("strErrorMessage :" + strErrorMessage);
            }

        }
        static Int32 getNextMonthStartDate(Int32 vDate)
        {
            DateTime dtDate;  
            dtDate = clsGeneric.GetIntToDate(vDate);
            DateTime nextMonth;
            if (dtDate.Day > 1)
                nextMonth = dtDate.AddDays(-(dtDate.Day - 1)).AddMonths(1);
            else
                nextMonth = dtDate.AddMonths(1);
            //var date = nextMonth.ToString("dd/MM/yyyy");
            
            return clsGeneric.GetDateToInt(nextMonth);

        }

        public DateTime  GetLastDayOfNextMonth(Int32 sDate)
        {
            DateTime dtDate;
            dtDate = clsGeneric.GetIntToDate(sDate);

            DateTime today = dtDate; 
            today = today.AddMonths(2);
            DateTime lastDay = today.AddDays(-(today.Day));
            return lastDay;
        }


    }
}