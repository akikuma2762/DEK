using dek_erpvis_v2.cls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace dek_erpvis_v2.pages
{
    public partial class ErrorPage : System.Web.UI.Page
    {
        public string strErrMsg = "";
        public string strErrNameSpace = "";
        public string strErrDetail = "";
        public string strErrURL = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // 取得錯誤訊息
                strErrMsg = "錯誤訊息：" + Utilities.LastError.InnerException.Message;

                // 取得錯誤的NameSpace
                strErrNameSpace = "錯誤範圍：" + Utilities.LastError.InnerException.Source;

                // 取得錯誤的詳細資料

                List<string> detail = new List<string>(Utilities.LastError.InnerException.StackTrace.Replace("於", "*").Split('*'));
                for (int i = 1; i < detail.Count - 4; i++)
                    strErrDetail += $"於{detail[i]}<br />";
                // 取得錯誤的頁面URL
                List<string> url = new List<string>(Request["aspxerrorpath"].ToString().Split('/'));
                strErrURL = url[url.Count - 1];
            }
            catch
            {
                Response.Redirect("../login.aspx");
            }
        }
    }
}