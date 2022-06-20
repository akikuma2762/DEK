using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dek_erpvis_v2.cls
{


    public class BasePage : System.Web.UI.Page
    {
        public BasePage()
        {
            this.PreRender += new EventHandler(Page_PreRender);
        }
        /// <summary>
        /// BasePage 的摘要描述
        /// </summary>
        private void SetLoadTime()
        {
            Session["loadTime"] = Server.UrlEncode(DateTime.Now.ToString());
        }
        void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetLoadTime();
            }
       //     ClientScript.RegisterHiddenField("loadTime", Session["loadTime"].ToString());
        }
        /// <summary>
        /// 取得值，指出網頁是否經由重新整理動作回傳 (PostBack)
        /// </summary>
        protected bool IsRefresh
        {
            get
            {
                if (HttpContext.Current.Request["loadTime"] as string == Session["loadTime"] as string)
                {
                    SetLoadTime();
                    return false;
                }
                return true;
            }
        }
    }
}