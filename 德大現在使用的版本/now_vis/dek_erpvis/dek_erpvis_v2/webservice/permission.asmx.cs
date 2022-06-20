using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml;
using Support;
using dek_erpvis_v2.cls;
using System.Data;
using System.Net;
using System.Text;
using System.IO;

namespace dek_erpvis_v2.webservice
{
    /// <summary>
    ///permission 的摘要描述
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
    [System.Web.Script.Services.ScriptService]
    public class permission : System.Web.Services.WebService
    {

        [WebMethod]
        public XmlNode authorize_permission(string key)
        {
            myclass myclass = new myclass();
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement xmlElem = xmlDoc.CreateElement("ROOT");
            DataTableUtils.Conn_String = myclass.GetConnByDekVisErp;

            string val = myclass.Base64Decode(key);
            string table_name = "WEB_USER";
            string id = "WU" + myclass.get_ran_id();
            string user_acc = val.Split(',')[0];
            string url = val.Split(',')[1];

            DataTable dt = DataTableUtils.DataTable_GetRowHeader(table_name);
            DataRow row = dt.NewRow();
            row["WU_ID"] = id;
            row["WB_URL"] = url;
            row["USER_ACC"] = user_acc;
            row["VIEW_NY"] = "N";
            bool OK = DataTableUtils.Insert_DataRow(table_name, row);

            if (OK == true)
            {
                xmlElem.SetAttribute("system_msg", "伺服器回應 : 申請成功!請等待管理員審核");
                string link = "http://vis.deta.com.tw:57957/pages/SYS_CONTROL/rights_application.aspx";
                string message = $"\r\n" +
                                 $"[申請人]：{HtmlUtil.Search_acc_Column(user_acc, "USER_NAME")}" +
                                 $"\r\n" +
                                 $"[申請頁面]：{change_pagename(url)}" +
                                 $"\r\n" +
                                 $"[網站連結]：{link}";

                lineNotify("", message);
            }
            else
                xmlElem.SetAttribute("system_msg", "伺服器回應 : 申請失敗!請重新操作");
            xmlDoc.AppendChild(xmlElem);
            dt.Dispose();
            return xmlDoc.DocumentElement;
        }
        [WebMethod]
        public XmlNode set_notice_for_adm(string key)
        {
            myclass myclass = new myclass();
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement xmlElem = xmlDoc.CreateElement("ROOT");
            DataTableUtils.Conn_String = myclass.GetConnByDekVisErp;
            string val = myclass.Base64Decode(key);
            string sqlcmd = $"select * from USERS where user_acc = '{val}' and adm='Y'";
            DataRow raw = DataTableUtils.DataTable_GetDataRow(sqlcmd);
            string adm_NY = "";
            if (raw != null)
                adm_NY = raw["adm"].ToString();
            if (adm_NY == "Y")
            {
                DataTableUtils.Conn_String = myclass.GetConnByDekVisErp;
                string 取得申請瀏覽權限的數量_cmd = "SELECT count(VIEW_NY) as VIEW_NY_COUNT FROM WEB_USER where VIEW_NY='N'";//
                DataRow row = DataTableUtils.DataTable_GetDataRow(取得申請瀏覽權限的數量_cmd);
                string count = DataTableUtils.toString(row["VIEW_NY_COUNT"]);
                xmlElem.SetAttribute("system_msg", count);
            }
            xmlDoc.AppendChild(xmlElem);
            return xmlDoc.DocumentElement;
        }
        [WebMethod]
        public XmlNode delete_ACC(string key)
        {
            myclass myclass = new myclass();
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement xmlElem = xmlDoc.CreateElement("ROOT");
            DataTableUtils.Conn_String = myclass.GetConnByDekVisErp;
            string val = myclass.Base64Decode(key);
            string YN = val.Split(',')[0];
            string admACC = val.Split(',')[1];
            string target = val.Split(',')[2];

            string adm_NY = DataTableUtils.DataTable_GetDataRow("USERS", $"user_acc = '{admACC}' and adm='{YN}'")["adm"].ToString();
            if (adm_NY == "Y")
            {
                string SQLCMD = $"DELETE FROM USERS WHERE USER_ACC = '{target}' DELETE FROM WEB_USER WHERE USER_ACC = '{target}' DELETE FROM SYSTEM_USERSLOGIN_log WHERE USER_ACC = '{target}' DELETE FROM SYSTEM_PMR WHERE USER_ACC = '{target}'";//
                int count = DataTableUtils.ExecSQL(SQLCMD);
                //xmlElem.SetAttribute("system_msg", DataTableUtils.toString(count));

                if (count > 0)
                {

                    xmlElem.SetAttribute("system_msg", $"伺服器回應 : 刪除成功! {DataTableUtils.toString(count)} ");
                }
                else
                {
                    xmlElem.SetAttribute("system_msg", $"伺服器回應 : 刪除失敗!請重新操作 {DataTableUtils.toString(count)}");
                }

            }
            xmlDoc.AppendChild(xmlElem);
            return xmlDoc.DocumentElement;
        }
        public static void lineNotify(string token, string msg)
        {
            token = WebUtils.GetAppSettings("remind_admin");//用於提醒管理者有人申請權限
            try
            {
                var request = (HttpWebRequest)WebRequest.Create("https://notify-api.line.me/api/notify");
                var postData = string.Format("message={0}", msg);
                var data = Encoding.UTF8.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                request.Headers.Add("Authorization", "Bearer " + token);

                using (var stream = request.GetRequestStream()) stream.Write(data, 0, data.Length);
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        private string change_pagename(string page)
        {
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisErp);
            string sqlcmd = $"select * from WEB_PAGES where WEB_URL = '{page}'";


            try
            {
                DataRow row = DataTableUtils.DataTable_GetDataRow(sqlcmd);
                return DataTableUtils.toString(row["WEB_PAGENAME"]);
            }
            catch
            {
                return "";
            }
        }
    }
}
