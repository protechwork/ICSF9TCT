using ICSF9TCT.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    public class GeneralizedController : Controller
    {
        #region Variable Definition
        int HeaderId = 0;
        int HeaderBatchId = 0;
        int Docdate = 0;
        StringBuilder msg = new StringBuilder();
        string strErrorMessage = string.Empty;
        int vStatus = 0;
        string vAbbr = "";
        string vAbbrPosting = "";
        string vSnamePosting = "";
        Hashtable extHeader = new Hashtable();
        List<Hashtable> extBody = new List<Hashtable>();
        List<Hashtable> extFooter = new List<Hashtable>();
        public string sessionID = "";
        #endregion


        [HttpPost]
        public ActionResult ChangeMngToTrans(int CompanyId, string SessionId, string User, int LoginId, int vtype, string docNo, int docdate, List<PEBody> BodyData)
        {
            BL_DB focus_db = new BL_DB();
            string strQuery;
            string _source;
            string _destiny;
            string _parent_id = "";
            DataSet ds;
            string error_msg = "";

            Hashtable ebayHashTable = new Hashtable();


            Hashtable ebay_invoice = new Hashtable();

            using (var webClient = new WebClient())
            {
                webClient.Encoding = Encoding.UTF8;
                webClient.Headers.Add("fSessionId", SessionId);
                webClient.Headers.Add("Content-Type", "application/json");
                clsGeneric.writeLog("Download URL: " + "http://localhost/Focus8API/Screen/Transactions/" + vtype + "/" + docNo);
                var resDoc = webClient.DownloadString("http://localhost/Focus8API/Screen/Transactions/" + vtype + "/" + docNo);
                clsGeneric.writeLog("Response of Focus Fields Data " + resDoc);
                if (resDoc != null)
                {
                    var resDataDoc = JsonConvert.DeserializeObject<APIResponse.PostResponse>(resDoc);
                    if (resDataDoc.result == -1)
                    {
                        return Json(new { status = false, data = new { message = resDataDoc.message } });
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


            string dateToReplaceWith = extHeader["Date"].ToString();


            string dynamicMasterBodyQuery = "Select tbat.*  from cCore_Vouchers_0 ccv  INNER JOIN " +
            " dbo.tCore_Header_0 AS tch ON ccv.iVoucherType = tch.iVoucherType INNER JOIN " +
            " dbo.tCore_Data_0 AS tcd ON tch.iHeaderId = tcd.iHeaderId INNER JOIN " +
            //" dbo.tCore_Indta_0 AS tci ON tcd.iBodyId = tci.iBodyId INNER JOIN " +
            " dbo.tCore_Batch_0 AS tbat ON tcd.iBodyId = tbat.iBodyId " +
            //" dbo.tCore_Data_Tags_0 AS tcdt ON tcd.iBodyId = tcdt.iBodyId INNER JOIN " +
            //" dbo.vCore_BodyScreenData_0 AS vcbsd ON tcd.iBodyId = vcbsd.iBodyId " +
            " where (tch.sVoucherNo = '" + docNo + "') AND (ccv.iVoucherType = " + vtype + ") " +
            " ORDER BY tch.iDate DESC";


            DataSet dynamicMasterDS = focus_db.GetData(dynamicMasterBodyQuery, CompanyId, ref strErrorMessage);
            if (dynamicMasterDS != null)
            {
                for (int iDynamicMaster = 0; iDynamicMaster < dynamicMasterDS.Tables[0].Rows.Count; iDynamicMaster++)
                {
                    string iBodyId = Convert.ToString(dynamicMasterDS.Tables[0].Rows[iDynamicMaster]["iBodyId"]);

                    string udpateQuery = $@"UPDATE tCore_Batch_0 SET iMfDate="+ dateToReplaceWith + " WHERE iBodyId=" + iBodyId;
                    clsGeneric.writeLog("Query :" + udpateQuery);
                    focus_db.GetExecute(udpateQuery, CompanyId, ref strErrorMessage);
                    if (strErrorMessage != "")
                    {
                        clsGeneric.writeLog("strErrorMessage :" + strErrorMessage);
                    }
                }
            }

            
            string returnMsg = "Updated Mfg Date Successfully";

           

            return Json(new { status = true, data = new { message = returnMsg } });
        }

    }
}