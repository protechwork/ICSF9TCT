using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ICSF9TCT.Models;
using System.Text;
using System.Collections;
using Newtonsoft.Json;
using System.Net;

namespace ICSF9TCT.Controllers
{
    public class ASSampleController : Controller
    {
        #region Variable Definition
        StringBuilder msg = new StringBuilder();
        string strErrorMessage = string.Empty;
        string vAbbr = "";
        string svName = "";
        int ipvType = 1290;
        string pvAbbr = "";
        string pvName = "";
        #endregion

        public ActionResult ASLIssPro(int CompanyId, string SessionId, string User, int LoginId, int bvtype, string bdocNo, List<PEBody> BodyData)
        {



            clsGeneric.Log_write(RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString(), SessionId, CompanyId, bdocNo, bvtype, User);
            
            string strValue = "";
            strValue = $@"SELECT  sAbbr  FROM  dbo.cCore_Vouchers_0 WHERE (iVoucherType =" + bvtype + " )";
            BL_DB DataAcesslayer = new BL_DB();
            vAbbr = Convert.ToString(clsGeneric.ShowRecord(CompanyId, strValue));
            strValue = $@"SELECT  sName  FROM  dbo.cCore_Vouchers_0 WHERE (iVoucherType =" + bvtype + " )";
            svName = Convert.ToString(clsGeneric.ShowRecord(CompanyId, strValue));

            strValue = $@"SELECT  sAbbr  FROM  dbo.cCore_Vouchers_0 WHERE (iVoucherType =" + ipvType + " )";
            pvAbbr = Convert.ToString(clsGeneric.ShowRecord(CompanyId, strValue));
            strValue = $@"SELECT  sName  FROM  dbo.cCore_Vouchers_0 WHERE (iVoucherType =" + ipvType + " )";
            pvName = Convert.ToString(clsGeneric.ShowRecord(CompanyId, strValue));


            using (var client1 = new WebClient())
            {
                client1.Encoding = Encoding.UTF8;
                client1.Headers.Add("fSessionId", SessionId);
                client1.Headers.Add("Content-Type", "application/json");
                var responseAVJW1 = client1.DownloadString("http://localhost/Focus8API/Screen/Transactions/" + svName + "/" + bdocNo);

                

                if (responseAVJW1 != null)
                {
                    var responseDataBDoc = JsonConvert.DeserializeObject<APIResponse.PostResponse>(responseAVJW1);
                    var extHeader = JsonConvert.DeserializeObject<Hashtable>(JsonConvert.SerializeObject(responseDataBDoc.data[0]["Header"]));
                    var Docdate = Convert.ToInt32(extHeader["Date"]);
                    
                }
                return Json(new { status = true, data = new { succes = msg.ToString() } });
            }


            Hashtable headerAVJW = new Hashtable();
            headerAVJW.Add("DocNo", null);
            headerAVJW.Add("Date", 122202);
            HashData objHashRequest = new HashData();
            List<System.Collections.Hashtable> lstBody = new List<System.Collections.Hashtable>();
            Hashtable body = new Hashtable();

            body.Add("Item__Id", 1);
            body.Add("Quantity", 10);

            lstBody.Add(body);
            System.Collections.Hashtable objHash = new System.Collections.Hashtable();
            objHash.Add("Body", lstBody);
            objHash.Add("Header", headerAVJW);
            List<System.Collections.Hashtable> lstHash = new List<System.Collections.Hashtable>();
            lstHash.Add(objHash);
            objHashRequest.data = lstHash;
            string sContentAVJW = JsonConvert.SerializeObject(objHashRequest);
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers.Add("fSessionId", SessionId);
                client.Headers.Add("Content-Type", "application/json");
                var responseAVJW = client.UploadString("http://localhost/Focus8API/Transactions/Vouchers/" + pvName, sContentAVJW);
                if (responseAVJW != null)
                {
                    var responseDataAVJW = JsonConvert.DeserializeObject<APIResponse.PostResponse>(responseAVJW);
                    if (responseDataAVJW.result == -1)
                    {
                        return Json(new { status = false, data = new { message = responseDataAVJW.message } });
                    }
                    else
                    {
                        return Json(new { status = true, data = new { message = responseDataAVJW.message } });
                    }
                }
                return Json(new { status = true, data = new { succes = msg.ToString() } });
            }

        }
    }
}