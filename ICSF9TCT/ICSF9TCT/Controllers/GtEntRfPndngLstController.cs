using ICSF9TCT.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ICSF9TCT.Controllers
{
    public class GtEntRfPndngLstController : Controller
    {
        #region Variable Definition
        BL_DB objDB = new BL_DB();
        
        string strQry = "";
        string strErrorMessage = string.Empty;
        #endregion
        public ActionResult GtEntRfPLst(int CompanyId, string User, string Branch, string Vendor, string SessionId, int LoginId, int vtype, string docNo, string idocDate, string refLst)
        {
            string error="";
            clsGeneric.Log_write(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), SessionId, CompanyId, docNo, vtype, User);
            strQry = $@"delete from ICSgtEntRfPndngLst";
            objDB.GetExecute(strQry, CompanyId, ref error);
            clsGeneric.createICSgtEntRfPndngLst(CompanyId, User, docNo);
            strQry = $@"exec ICS_gtEntRfPndngLst '" + docNo + "','" + User + "'," + idocDate + ",'" + Vendor + "','" + Branch + "','" + refLst + "'";
            clsGeneric.writeLog("fSessionid : " + strQry);
            DataSet ds = objDB.GetData(strQry, CompanyId, ref strErrorMessage);
            //return Json(new { status = false, data = new { message = "das" } });
            return new EmptyResult();
        }


    }
}