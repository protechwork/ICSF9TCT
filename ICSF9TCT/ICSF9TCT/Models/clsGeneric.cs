
using Focus.Common.DataStructs;
using System;
using System.Data;
using System.Linq;
using System.Net;

namespace ICSF9TCT.Models
{
    public class clsGeneric
    {
        public static string DecimalCustomFormat(double Digits)
        {
            return string.Format("{0:0.00}", Digits).Replace(".00", "");

        }
        public static void writeLog(string sContent)
        {
            //if (Global.logEnabled)
                FConvert.LogFile(DateTime.Now.ToString("MMyy") + "ICSF9TCT{0}.log", DateTime.Now.ToString() + " : " + sContent);

        }
        public static void writeLog(string FileName, string sContent)
        {
            //if (Global.logEnabled)
            FConvert.LogFile(DateTime.Now.ToString() + "_" + FileName, sContent);

        }

        public static void RMwriteLog(string sContent)
        {
            FConvert.LogFile(DateTime.Now.ToString("MMyy") + "RMstock.log", DateTime.Now.ToString() + " : " + sContent);

        }
        public static void writeLogSavingFailed(string sContent)
        {
            FConvert.LogFile(DateTime.Now.ToString("MMyy") + "AdiswaraPostingFailed.log", DateTime.Now.ToString() + " : " + sContent);
        }
        public static void writeLogJson(string sContent)
        {
            FConvert.LogFile(DateTime.Now.ToString("MMyy") + "AdiswaraJson.log", DateTime.Now.ToString() + " : " + sContent);
        }
        public static void writeLogJson(string FileName, string sContent)
        {
            FConvert.LogFile(FileName,  sContent);
        }

        #region Function is used to get Integer Date to DateTime
        public static Date GetIntToDate(int iDate)
        {
            try
            {

                return (new Date(iDate, CalendarType.Gregorean));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        //-----------------------------------------------
      
        //-----------------------------------------------
        #region ShowRecord 
        public static string oShowRecord(int companyId, string strquery)
        {
            BL_DB objDB = new BL_DB();
            string error = "";
            DataSet dt0 = objDB.GetData(strquery, companyId, ref error);
            if (dt0 != null  && dt0.Tables.Count > 0 && dt0.Tables[0].Rows.Count > 0)
            {
                bool b1 = string.IsNullOrEmpty(dt0.Tables[0].Rows[0][0].ToString());
                if (!b1)
                    return dt0.Tables[0].Rows[0][0].ToString();
            }
            return "0";
        }
        public static string ShowRecord(int companyId, string strquery)
        {
            BL_DB objDB = new BL_DB();
            string error = "";
            DataSet dt0 = objDB.GetData(strquery, companyId, ref error);
            if (dt0 != null && dt0.Tables.Count > 0 && dt0.Tables[0].Rows.Count > 0)
            {
                bool b1 = string.IsNullOrEmpty(dt0.Tables[0].Rows[0][0].ToString());
                if (!b1)
                    return dt0.Tables[0].Rows[0][0].ToString();
            }
            return "0";
        }

        public static void Log_write (string fControllerName, string fMethodName, string fSessionId, int fCompanyId, string fdocNo, int fvtype, string fUser)
        {
            clsGeneric.writeLog(" ------------------ " + fControllerName +  " starts here {" + fUser + "}-------------------");
            clsGeneric.writeLog("fSessionid : " + fSessionId);
            clsGeneric.writeLog("Company Id : " + fCompanyId);
            clsGeneric.writeLog("Doc No : " + fdocNo);
            clsGeneric.writeLog("v type : " + fvtype);
            clsGeneric.writeLog("Controller Name :" + fControllerName  );
            clsGeneric.writeLog("Method Name:" + fMethodName);
        }

        #endregion
        #region Update AnnexureStatus 

        public static void Update_AnnexureStatus(int companyId, int vtype, int HeaderId, int Status = 0)
        {


            BL_DB objDB = new BL_DB();
            string strQry = "";
            string error = "";
            DataTable dt = new DataTable();
            strQry = $@"update tCore_HeaderData" + vtype + "_0 set AnnexureStatus = " + Status + " where iHeaderId=" + HeaderId;
            objDB.GetExecute(strQry, companyId, ref error);

            clsGeneric.writeLog("Update AnnexureStatus - tCore_HeaderData" + vtype + "_0 Query : " + strQry);

        }
        public static void Update_AnnexureStatusSAL(int companyId, int vtype, int HeaderId, int Status = 0)
        {


            BL_DB objDB = new BL_DB();
            string strQry = "";
            string error = "";
            DataTable dt = new DataTable();
            strQry = $@"update tCore_HeaderData" + vtype + "_0 set AnnexureStatus = " + Status + " where iHeaderId=" + HeaderId;
            objDB.GetExecute(strQry, companyId, ref error);

            clsGeneric.writeLog("Update AnnexureStatus - tCore_HeaderData" + vtype + "_0 Query : " + strQry);

        }

        public static void ReSaveVoucher(string sSessionId, int Vouchertype, string VoucherNo)
        {
            BL_DB objDB = new BL_DB();
            
            string strVal = string.Empty;
            
            strVal = "http://localhost/focus8api/Screen/Transactions/resavevoucher/" + Vouchertype + "/" + VoucherNo;
            clsGeneric.writeLog("URL OF ReSave :-" + strVal);
            using (var client = new WebClient())
            {

                //client.Encoding = Encoding.UTF8;
                client.Headers.Add("fSessionId", sSessionId);
                client.Headers.Add("Content-Type", "application/json");
                var response = client.DownloadString(strVal);
                clsGeneric.writeLog("Response of Resave :- " + response);

            }
        }

        #endregion

       

        
        
       

        #region Function is used to get Date to Integer
        public static int GetDateToInt(DateTime dt)
        {
            int val;
            val = Convert.ToInt16(dt.Year) * 65536 + Convert.ToInt16(dt.Month) * 256 + Convert.ToInt16(dt.Day);
            return val;
        }
        #endregion

        #region Get Desired default Value of Master
        public static string getValue(string sTableName, int Companyid, string strFldName, string fldValue, string returnValue)
        {
            BL_DB objDb = new BL_DB();
            string strErrorMessage = default(string);
            //int fieldid = default(int);
            string sqlsticks = string.Empty;

            sqlsticks = "select sName from cCore_Vouchers_0 where ivoucherType = " + fldValue + "";
            DataSet dsFld = new DataSet();
            dsFld = objDb.GetData(sqlsticks, Companyid, ref strErrorMessage);

            clsGeneric.writeLog("Query : " + sqlsticks);

            if (dsFld.Tables[0].Rows.Count <= 0)
                return "";
            return dsFld.Tables[0].Rows[0][0].ToString();

        }
        #endregion

        #region Temp Table 
        public static void createTableCollectSFG_ProdnPlan(int companyId, string User, string Vno)
        {


            BL_DB objDB = new BL_DB();
            string strQry = "";
            string error = "";
            DataTable dt = new DataTable();
            strQry = "select * from sysObjects where name='TableCollectSFG_ProdnPlan'";
            dt = objDB.GetData(strQry, companyId, ref error).Tables[0];
            if (dt.Rows.Count <= 0)
            {

                strQry = $@"create table TableCollectSFG_ProdnPlan (id bigint primary key identity(1,1), iVariantId bigint default 0, iProductId bigint default 0, planQty Decimal(20,3) default 0,BOMQty Decimal(20,3) default 0,SFGReqQty Decimal(20,3) default 0,ParentProductId bigint default 0,iLevel bigint default 0, sUnit varchar(100), iRowIndex bigint default 0, iInvTagValue bigint default 0,BranchId bigint default 0,WCId bigint default 0,FGID bigint default 0,loggeduser varchar(50) null)";
                objDB.GetExecute(strQry, companyId, ref error);

                clsGeneric.writeLog("create Query createTableCollectSFG_ProdnPlan: " + strQry);


            }
            else
            {
                strQry = $@"delete from TableCollectSFG_ProdnPlan where loggeduser='" + User + "'";
                objDB.GetExecute(strQry, companyId, ref error);

                clsGeneric.writeLog("delete Query createTableCollectSFG_ProdnPlan: " + strQry);


            }
        }

        public static void createTable_IssProQC(int companyId, string User, string Vno)
        {
            BL_DB objDB = new BL_DB();
            string strQry = "";
            string error = "";
            DataTable dt = new DataTable();
            strQry = "select * from sysObjects where name='ICSPendingIssProQC'";
            dt = objDB.GetData(strQry, companyId, ref error).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                strQry = $@"create table ICSPendingIssProQC ([id] [int] IDENTITY(1,1) Primary key NOT NULL, vNo varchar(100) null,docDate bigint null,vType bigint null,itemId bigint null, qty decimal(10,2) null,stkRate decimal(10,2) null,warehouseId bigint null, PONo varchar(100) null,POID bigint null, batchNo varchar(100) null,mfgdt varchar(100),batch_id  bigint null, Dept_Id bigint null, status int default 0,iBody_id bigint null)";
                objDB.GetExecute(strQry, companyId, ref error);
                clsGeneric.writeLog("create Query ICSPendingIssProQC: " + strQry);
            }
            
        }
        public static void createICSgtEntRfPndngLst(int companyId, string User, string Vno)
        {


            BL_DB objDB = new BL_DB();
            string strQry = "";
            string error = "";
            DataTable dt = new DataTable();
            strQry = "select * from sysObjects where name='ICSgtEntRfPndngLst'";
            dt = objDB.GetData(strQry, companyId, ref error).Tables[0];
            if (dt.Rows.Count <= 0)
            {

                strQry = $@"CREATE TABLE ICSgtEntRfPndngLst([id] [int] IDENTITY(1,1) Primary key NOT NULL,[VoucherNo] [varchar](100) NULL,[iDate] int NULL,[sName] [varchar](1000) NULL, [AbbrNo] [varchar](150) NULL, [Abbr] [varchar](100) NULL, vno varchar(100), loggeduser varchar(50))";
                objDB.GetExecute(strQry, companyId, ref error);
                clsGeneric.writeLog("Query : " + strQry);
                clsGeneric.writeLog("s1");

            }
            else
            {
                strQry = $@"delete from ICSgtEntRfPndngLst where loggeduser='" + User + "'";
                objDB.GetExecute(strQry, companyId, ref error);
                clsGeneric.writeLog("delete Query : " + strQry);
                clsGeneric.writeLog("s2");

            }
        }


        public static void createICSPendingGRNRMQC(int companyId, string User, string Vno)
        {


            BL_DB objDB = new BL_DB();
            string strQry = "";
            string error = "";
            DataTable dt = new DataTable();
            strQry = "select * from sysObjects where name='ICSPendingGRNRMQC'";
            dt = objDB.GetData(strQry, companyId, ref error).Tables[0];
            if (dt.Rows.Count <= 0)
            {

                strQry = $@"create table ICSPendingGRNRMQC ([ibodyId] [bigint],fQuantity decimal(10,2),[sAbbrVNo] [varchar](200) NULL,[sVoucherNo] [varchar](100) NULL,[iDate] int NULL,[sBatchNo] [varchar](100) NULL, [iBatchId] bigint, [iMFDate] [bigint], vno varchar(100), loggeduser varchar(50))";
                objDB.GetExecute(strQry, companyId, ref error);
                clsGeneric.writeLog("Query : " + strQry);
                clsGeneric.writeLog("s1");

            }
            else
            {
                //strQry = $@"delete from ICSPendingGRNRMQC where vno='" + Vno  + "'and loggeduser='" + User + "'";
                strQry = $@"delete from ICSPendingGRNRMQC where loggeduser='" + User + "'";
                //strQry = $@"delete from ICSPendingGRNRM";
                objDB.GetExecute(strQry, companyId, ref error);
                clsGeneric.writeLog("delete Query : " + strQry);
                clsGeneric.writeLog("s2");

            }
        }




        // start PPCPlanRMReq
        public static void createTableCollectSFG_PPCPlanRMReq(int companyId, string User, string Vno)
        {


            BL_DB objDB = new BL_DB();
            string strQry = "";
            string error = "";
            DataTable dt = new DataTable();
            strQry = "select * from sysObjects where name='TableCollectSFG_PPCPlanRMReq'";
            dt = objDB.GetData(strQry, companyId, ref error).Tables[0];
            if (dt.Rows.Count <= 0)
            {

                strQry = $@"create table TableCollectSFG_PPCPlanRMReq(id bigint primary key identity(1,1), iVariantId bigint default 0, iProductId bigint default 0, planQty Decimal(20,3) default 0,BOMQty Decimal(20,3) default 0,SFGReqQty Decimal(20,3) default 0,ParentProductId bigint default 0,iLevel bigint default 0, sUnit varchar(100), iRowIndex bigint default 0, iInvTagValue bigint default 0,BranchId bigint default 0,WCId bigint default 0,FGID bigint default 0,vno varchar(50) null,loggeduser varchar(50) null )";
                objDB.GetExecute(strQry, companyId, ref error);

                clsGeneric.writeLog("create Query TableCollectSFG_PPCPlanRMReq: " + strQry);


            }
            else
            {
                strQry = $@"delete from TableCollectSFG_PPCPlanRMReq where loggeduser='" + User + "' and vno='" + Vno + "'";
                objDB.GetExecute(strQry, companyId, ref error);

                clsGeneric.writeLog("delete Query TableCollectSFG_PPCPlanRMReq: " + strQry);


            }
        }


        public static void createTableCollectSFG_PPCPlan(int companyId, string User, string Vno)
        {


            BL_DB objDB = new BL_DB();
            string strQry = "";
            string error = "";
            DataTable dt = new DataTable();
            strQry = "select * from sysObjects where name='TableCollectSFG_PPCPlan'";
            dt = objDB.GetData(strQry, companyId, ref error).Tables[0];
            if (dt.Rows.Count <= 0)
            {

                strQry = $@"create table TableCollectSFG_PPCPlan (id bigint primary key identity(1,1), iVariantId bigint default 0, iProductId bigint default 0, planQty Decimal(20,3) default 0,BOMQty Decimal(20,3) default 0,SFGReqQty Decimal(20,3) default 0,ParentProductId bigint default 0,iLevel bigint default 0, sUnit varchar(100), iRowIndex bigint default 0, iInvTagValue bigint default 0,BranchId bigint default 0,WCId bigint default 0,FGID bigint default 0,loggeduser varchar(50) null)";
                objDB.GetExecute(strQry, companyId, ref error);

                clsGeneric.writeLog("create Query createTableCollectSFG_PPCPlan: " + strQry);


            }
            else
            {
                strQry = $@"delete from TableCollectSFG_PPCPlan where loggeduser='" + User + "'";
                objDB.GetExecute(strQry, companyId, ref error);

                clsGeneric.writeLog("delete Query createTableCollectSFG_PPCPlan: " + strQry);


            }
        }


        public static void createtblgetMaxQty_GRNJWSal(int companyId, string User, string Vno)
        {


            BL_DB objDB = new BL_DB();
            string strQry = "";
            string error = "";
            DataTable dt = new DataTable();
            strQry = "select * from sysObjects where name='tblgetMaxQty_GRNJWSal'";
            dt = objDB.GetData(strQry, companyId, ref error).Tables[0];
            if (dt.Rows.Count <= 0)
            {

                strQry = $@"create table tblgetMaxQty_GRNJWSal (id bigint primary key identity(1,1), iFgId bigint default 0, iRMId bigint default 0,dPlanQty Decimal(20,3) default 0,dRMBOMQty Decimal(20,3) default 0,dFGBOMQty Decimal(20,3) default 0,dGRNJWSalQty Decimal(20,3) default 0,dAnxJWSaleQty Decimal(20,3) default 0,dRetJWSaleQty Decimal(20,3) default 0,iParentProductId bigint default 0,iLevel bigint default 0, iVendor bigint default 0,iBranchId bigint default 0,IPAddress varchar(100) null,sVNo varchar(100) null,loggeduser varchar(50) null)";
                objDB.GetExecute(strQry, companyId, ref error);

                clsGeneric.writeLog("create Query tblgetMaxQty_GRNJWSal: " + strQry);


            }
            else
            {
                strQry = $@"delete from tblgetMaxQty_GRNJWSal where loggeduser='" + User + "'";
                objDB.GetExecute(strQry, companyId, ref error);

                clsGeneric.writeLog("delete Query tblgetMaxQty_GRNJWSal: " + strQry);


            }
        }

        public static void createTableRMRequisition_PPCPlanRMReq(int companyId, string User, string Vno)
        {


            BL_DB objDB = new BL_DB();
            string strQry = "";
            string error = "";
            DataTable dt = new DataTable();
            strQry = "select * from sysObjects where name='RMRequisition_PPCPlanRMReq'";
            dt = objDB.GetData(strQry, companyId, ref error).Tables[0];
            if (dt.Rows.Count <= 0)
            {

                strQry = $@" create table RMRequisition_PPCPlanRMReq(id bigint identity(1,1) primary key,refid bigint,PlanMonth varchar(50), Branch bigint default 0, WorksCenter bigint default 0,Warehouse bigint default 0,Product bigint default 0,ParentID bigint default 0,MParentID bigint default 0,Fqty DECIMAL(14,6) Default 0,RMQty DECIMAL(14,6) Default 0, PlanQty DECIMAL(14,6) Default 0, SalQty DECIMAL(14,6) Default 0, RMStock DECIMAL(14,6) Default 0, PReq DECIMAL(14,6) Default 0,VStock DECIMAL(14,6) Default 0, BRMReq DECIMAL(14,6) Default 0, Q2P DECIMAL(14,6) Default 0,iBodyId bigint default 0,vno varchar(50) null, loggeduser varchar(50) null) ";
                objDB.GetExecute(strQry, companyId, ref error);
                clsGeneric.writeLog("create Query RMRequisition_PPCPlanRMReq: " + strQry);


            }
            else
            {
                strQry = $@"delete from RMRequisition_PPCPlanRMReq where loggeduser='" + User + "' and vno='" + Vno + "'";
                objDB.GetExecute(strQry, companyId, ref error);

                clsGeneric.writeLog("delete Query RMRequisition_PPCPlanRMReq: " + strQry);


            }
        }

        #endregion

    }
}