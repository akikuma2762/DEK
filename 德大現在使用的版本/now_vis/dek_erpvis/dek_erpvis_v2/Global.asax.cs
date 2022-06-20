using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using dek_erpvis_v2.cls;

namespace dek_erpvis_v2
{
    public class Global : System.Web.HttpApplication
    {

        private static string MSUserAgentsRegex = @"[^\w](Word|Excel|PowerPoint|ms-office)([^\w]|\z)";
        protected void Application_OnPostAuthenticateRequest(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(Request.UserAgent, MSUserAgentsRegex))
            {
                Response.Write("<html><head><meta http-equiv='refresh' content='0'/></head><body></body></html>");
                Response.End();
            }
        }
        void Application_Start(object sender, EventArgs e)   //初始化站点的在线人数
        {
           

        }

        void Application_End(object sender, EventArgs e)
        {
            

        }

        void Application_Error(object sender, EventArgs e)
        {
           
        }

        void Session_Start(object sender, EventArgs e)      //站点在线人数加一
        {
           
        }
        void Session_End(object sender, EventArgs e)          //站点在线人数减一
        {
            

        }

        //Http请求开始和结束时的处理事件
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
           
        }
        protected void Application_EndRequest(object sender, EventArgs e)
        {

        }

        //Http请求验证的处理事件
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

    }
}