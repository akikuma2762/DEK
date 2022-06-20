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
    public partial class UntradedCustomer : System.Web.UI.Page
    {
        public string color = "";
        public string date_str = "";
        public string date_end = "";
        public string th = "";
        public string tr = "";
        public string data_name = "";
        public string title_text = "";
        public string path = "";
        string acc = "";
        string URL_NAME = "";
        public string timerange = "";
        myclass myclass = new myclass();
        Service service = new Service();
        //----------------------EVENT--------------------------------------
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                path = 德大機械.get_title_web_path("SLS");
                path = path.Replace("</ol>", "<li><u><a href='../webservice/UntradedCustomer.aspx'>未交易客戶</a></u></li></ol>");
                URL_NAME = "UntradedCustomer";
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                color = HtmlUtil.change_color(acc);
                // if (myclass.user_view_check(URL_NAME, acc) == true)
                if (HtmlUtil.Search_acc_Column(acc) == "A" || HtmlUtil.Search_acc_Column(acc) == "B")
                {
                    if (!IsPostBack)
                        Set_HtmlTable();
                }
                else
                    //   Response.Write("<script>alert('您無法瀏覽此頁面 請向該單位主管申請權限!');location.href='../index.aspx';</script>");
                    Response.Write("<script>alert('目前尚未開放權限!');location.href='../index.aspx';</script>");
            }
            else
                Response.Redirect(myclass.logout_url);
        }
        protected void button_select_Click(object sender, EventArgs e)
        {
            Set_HtmlTable();
        }
        //-------------------------FUNCTION-------------------------------
        private void Set_HtmlTable()
        {
            date_str = DataTableUtils.toString(txt_time_str.Value);
            date_end = DataTableUtils.toString(txt_time_end.Value);
            string Json = service.GetUntradedCustomerInformation(date_str, date_end, DropDownList_selectedcondi.SelectedValue, DataTableUtils.toInt(TextBox_dayval.Text));
            DataTable dt = ERPVIS.i_TEC.iTechDB.JsontoDataTable(Json);
            timerange = TextBox_dayval.Text;
            string titlename = "";
            th = HtmlUtil.Set_Table_Title(dt, out titlename);
            tr = HtmlUtil.Set_Table_Content(dt, titlename);
        }
    }
}