using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ICSF9TCT.Models;
using System.Data;
using Newtonsoft.Json;

namespace ICSF9TCT.Controllers
{
    public class InwQCClearController : Controller
    {
        #region Variable Definition
        StringBuilder msg = new StringBuilder();
        string strErrorMessage = string.Empty;
        string vAbbr = "";
        string sqlstring = "";
        
        List<string> strDetailIDList = new List<string>();
        #endregion

        public ActionResult BS_PopulteGRNNO(int CompanyId, string SessionId, string User, int LoginId, int bvtype, string bdocNo,int idocdate,int Dept,int Vendor,int Warehouse,int QCItem)
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
        
        public ActionResult BS_PopulteOtherFlds(int CompanyId, string SessionId, string User, int LoginId, int bvtype, string bdocNo, int idocdate, int Dept, int Vendor, int Warehouse, int QCItem,int iBodyId)
        {
            BL_DB DataAcesslayer = new BL_DB();
            sqlstring = "SELECT tci.fQuantity,tch.sVoucherNo,  format(dbo.IntToDate(tch.iDate),'dd/MM/yyyy') iDate, tcbn.sBatchNo, tcbn.iBatchId, format(dbo.IntToDate(tcbn.iMfDate),'dd/MM/yyyy')  iMfDate " +
                " FROM dbo.cCore_Vouchers_0 AS ccv INNER JOIN dbo.tCore_Header_0 AS tch ON ccv.iVoucherType = tch.iVoucherType INNER JOIN  " +
                " dbo.tCore_Data_0 AS tcd ON tch.iHeaderId = tcd.iHeaderId INNER JOIN dbo.tCore_Indta_0 AS tci ON tcd.iBodyId = tci.iBodyId INNER JOIN " +
                " dbo.tCore_Batch_0 AS tcbn ON tcd.iBodyId = tcbn.iBodyId WHERE (tch.bCancelled = 0) AND (tch.bSuspended = 0) AND (tcd.iAuthStatus = 1) " +
                " AND (tch.iVoucherType = 1281) AND (tcd.iBookNo = " + Vendor + ") AND (tcd.iInvTag = " + Warehouse + ") AND (tcd.iFaTag = " + Dept + ") AND (tci.iProduct = " + QCItem + ") and (tcd.iBodyId=" +  iBodyId  + ")";

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