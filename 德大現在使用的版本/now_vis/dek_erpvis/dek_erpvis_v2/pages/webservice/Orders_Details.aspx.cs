using dek_erpvis_v2.cls;
using dek_erpvis_v2.webservice;
using Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace dek_erpvis_v2.pages.webservice
{
    public partial class Orders_Details : System.Web.UI.Page
    {
        public string color = "";
        public string cust_name = "";
        public string url_text = "";
        public string Get_cust = "";
        public string Get_Dstart = "";
        public string Get_Dend = "";
        public string Get_SOStatus = "";
        public string title_text = "";
        public string tr = "";
        public string th = "";
        string acc = "";
        Service service = new Service();
        myclass myclass = new myclass();
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                color = HtmlUtil.change_color(acc);
                if (acc != "")
                {
                    if (!IsPostBack)
                        MainProcess();
                }
                else
                    Response.Redirect("Orders.aspx", false);
            }
            else
                Response.Redirect("Orders.aspx", false);
        }
        private void MainProcess()
        {
            GetCondi();
            Set_Html_Table();
        }
        private void GetCondi()
        {
            Response.Buffer = false;
            if (Request.QueryString["key"] != null)
            {
                string[] parameter = HtmlUtil.Return_str(Request.QueryString["key"]);
                //轉成客戶的代號
                GlobalVar.UseDB_setConnString(myclass.GetConnByDataWin);
                string sqlcmd = "Select * from tbm01 where ba03='" + parameter[1] + "'";
                DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
                Get_cust = DataTableUtils.toString(dt.Rows[dt.Rows.Count - 1]["ba01"]);
                cust_name = parameter[1];
                Get_Dstart = parameter[3];
                Get_Dend = parameter[5];
                Get_SOStatus = parameter[7];
            }
            else
                Response.Redirect("Orders.aspx", false);
        }

        private void Set_Html_Table()
        {
            string Json = service.GetOrdersDetailInformation(Get_Dstart, Get_Dend, Get_cust, Get_SOStatus);
            DataTable dt = ERPVIS.i_TEC.iTechDB.JsontoDataTable(Json);
            if (HtmlUtil.Check_DataTable(dt))
            {
                dt = myclass.Add_LINE_GROUP(dt).ToTable();

                dt.Columns.RemoveAt(0);
                dt.Columns["產線群組"].SetOrdinal(3);
                string titlename = "";
                th = HtmlUtil.Set_Table_Title(dt, out titlename);

                tr = HtmlUtil.Set_Table_Content(dt, titlename, orderdetail_callback);

            }
            else
                HtmlUtil.NoData(out th, out tr);

        }
        private string orderdetail_callback(DataRow row, string field_name)
        {
            string value = "";
            if (field_name == "訂單狀態")
            {
                if (DataTableUtils.toString(row[field_name]) == "1")
                    value = "已結案";
                else if (DataTableUtils.toString(row[field_name]) == "2")
                    value = "未結案";
            }
            if (value == "")
                return "";
            else
                return "<td>" + value + "</td>";
        }
    }
}
