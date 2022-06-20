using dek_erpvis_v2.cls;
using dek_erpvis_v2.webservice;
using dekERP_dll.dekErp;
using Support;
using Support.DB;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace dek_erpvis_v2.pages.dp_SD
{
    public partial class UntradedCustomer_New : System.Web.UI.Page
    {
        //-------------------------------------------------參數 OR 引用------------------------------------------------------------
        public string color = "";
        public string dt_str = "";
        public string dt_end = "";
        public string path = "";
        public StringBuilder th = new StringBuilder();
        public StringBuilder tr = new StringBuilder();
        string acc = "";
        ERP_Sales SLS = new ERP_Sales();
        DataTable dt_monthtotal = new DataTable();

        //----------------------------------------------------Event----------------------------------------------------------------------
        //載入事件
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                color = HtmlUtil.change_color(acc);
                path = 德大機械.get_title_web_path("SLS");
                if (myclass.user_view_check("UntradedCustomer", acc))
                {
                    if (!IsPostBack)
                    {
                        if (WebUtils.GetAppSettings("Show_dek") == "1" || HtmlUtil.Search_acc_Column(acc, "Can_dek") == "Y")
                            PlaceHolder_hide.Visible = true;
                        else
                            PlaceHolder_hide.Visible = false;
                        MainProcess();
                    }
                }
                else
                    Response.Write("<script>alert('您無法瀏覽此頁面 請向該單位主管申請權限!');location.href='../index.aspx';</script>");
            }
            else
                Response.Redirect(myclass.logout_url);
        }

        //查詢事件
        protected void button_select_Click(object sender, EventArgs e)
        {
            dt_str = txt_str.Text.Replace("-", "");
            dt_end = txt_end.Text.Replace("-", "");
            MainProcess();
        }

        //-----------------------------------------------------Function---------------------------------------------------------------------      
        //需要執行的程式
        private void MainProcess()
        {
            Get_MonthTotal();
            Set_Table();
        }

        //取得未交易數
        private void Get_MonthTotal()
        {
            dt_monthtotal = SLS.UntradedCustomer(dt_str, dt_end, DropDownList_selectedcondi.SelectedValue, DataTableUtils.toInt(TextBox_dayval.Text), dropdownlist_Factory.SelectedItem.Value); 
        }

        //設定表格
        private void Set_Table()
        {
            DataTable dt = dt_monthtotal;
            if (HtmlUtil.Check_DataTable(dt))
            {
                string titlename = "";
                th.Append(HtmlUtil.Set_Table_Title(dt, out titlename));
                tr.Append(HtmlUtil.Set_Table_Content(dt, titlename, UntradedCustomer_callback));
            }
            else
                HtmlUtil.NoData(out th, out tr);
        }

        //例外處理
        private string UntradedCustomer_callback(DataRow row, string field_name)
        {
            return field_name == "最後交易日" ? $"<td>{HtmlUtil.changetimeformat(DataTableUtils.toString(row[field_name]))}</td>" : "";
        }
    }
}