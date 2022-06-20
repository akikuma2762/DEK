using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using dek_erpvis_v2.cls;
using Support;
using dek_erpvis_v2.webservice;
using dekERP_dll;
using dekERP_dll.dekErp;
using System.Text;

namespace dek_erpvis_v2.pages.dp_PD
{
    public partial class supplierscore_New : System.Web.UI.Page
    {
        //-------------------------------------------------參數 OR 引用------------------------------------------------------------
        public string color = "";
        public StringBuilder th = new StringBuilder();
        public StringBuilder tr = new StringBuilder();
        public string dt_str = "";
        public string dt_end = "";
        public string date_str = "";
        public string date_end = "";
        public string path = "";
        string acc = "";
        ERP_Materials PCD = new ERP_Materials();
        DataTable dt_monthtotal = new DataTable();
        double buy_count = 0;
        double give_count = 0;
        List<string> avoid_again = new List<string>();
        //-----------------------------------------------------EVENT-----------------------------------------------------------------------------
        //載入事件
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                path = 德大機械.get_title_web_path("PCD");
                color = HtmlUtil.change_color(acc);
                if (myclass.user_view_check("supplierscore", acc))
                {
                    if (!IsPostBack)
                    {
                        string[] date = 德大機械.德大專用月份(acc).Split(',');
                        dt_str = date[0];
                        dt_end = date[1];
                        date_str = HtmlUtil.changetimeformat(date[0], "-");
                        date_end = HtmlUtil.changetimeformat(date[1], "-");
                        txt_str.Text = HtmlUtil.changetimeformat(date[0], "-");
                        txt_end.Text = HtmlUtil.changetimeformat(date[1], "-");
                        if (WebUtils.GetAppSettings("Show_dek") == "1" || HtmlUtil.Search_acc_Column(acc, "Can_dek") == "Y")
                            PlaceHolder_hide.Visible = true;
                        else
                            PlaceHolder_hide.Visible = false;
                        MainProcess();
                    }
                }
                else
                    Response.Write("<script>alert('您無此權限!');location.href='../index.aspx';</script>");

            }
            else
                Response.Redirect(myclass.logout_url);
        }
        //查詢事件
        protected void button_select_Click(object sender, EventArgs e)
        {
            string[] date = 德大機械.德大專用月份(acc).Split(',');
            date_str = HtmlUtil.changetimeformat(date[0], "-");
            date_end = HtmlUtil.changetimeformat(date[1], "-");
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

        //取得本月達交率
        private void Get_MonthTotal()
        {
            dt_monthtotal = PCD.Supplierscore_Detail(dt_str, dt_end, dropdownlist_Factory.SelectedItem.Value);
        }

        //設定表格
        private void Set_Table()
        {
            if (HtmlUtil.Check_DataTable(dt_monthtotal))
            {
                DataTable Custom = dt_monthtotal.DefaultView.ToTable(true, new string[] { "供應商" });
                Custom.Columns.Add("採購次數");
                Custom.Columns.Add("期限內已交總數量");
                Custom.Columns.Add("採購總數量");
                Custom.Columns.Add("達交率");

                string columns = "";
                th.Append(HtmlUtil.Set_Table_Title(Custom, out columns));
                tr.Append(HtmlUtil.Set_Table_Content(Custom, columns, supplierscore));
            }
            else
                HtmlUtil.NoData(out th, out tr);
        }

        //例外處理
        private string supplierscore(DataRow row, string field_name)
        {
            string value = "";
            string sqlcmd = "";
            if (avoid_again.IndexOf(DataTableUtils.toString(row["供應商"]).Trim()) == -1)
            {
                if (field_name == "採購次數")
                {
                    sqlcmd = $"供應商='{DataTableUtils.toString(row["供應商"])}'";
                    DataRow[] rows = dt_monthtotal.Select(sqlcmd);
                    value = rows.Length.ToString();
                }
                else if (field_name == "期限內已交總數量")
                {
                    sqlcmd = $"供應商='{DataTableUtils.toString(row["供應商"])}'";
                    DataRow[] rows = dt_monthtotal.Select(sqlcmd);

                    if (rows != null && rows.Length > 0)
                    {
                        for (int i = 0; i < rows.Length; i++)
                            give_count += DataTableUtils.toDouble(DataTableUtils.toString(rows[i]["期限內已交數量"]));
                    }
                    value = give_count.ToString("0");
                }
                else if (field_name == "採購總數量")
                {
                    sqlcmd = $"供應商='{DataTableUtils.toString(row["供應商"])}'";
                    DataRow[] rows = dt_monthtotal.Select(sqlcmd);

                    if (rows != null && rows.Length > 0)
                    {
                        for (int i = 0; i < rows.Length; i++)
                            buy_count += DataTableUtils.toDouble(DataTableUtils.toString(rows[i]["採購數量"]));
                    }
                    value = buy_count.ToString("0");
                }
                else if (field_name == "達交率")
                {
                    double precent = 0;
                    precent = give_count * 100 / buy_count;
                    string url = $"supplierscore_details.aspx?key={WebUtils.UrlStringEncode($"sup_sname={row["供應商"]},date_str={dt_str},date_end={dt_end},type={dropdownlist_Factory.SelectedItem.Value}")}";
                    value = $" <u><a href=\"{url}\">{precent:0.00}</a></u>";
                    give_count = 0;
                    buy_count = 0;
                }
                return value == "" ? "" : $"<td  align=\"right\"> {value} </td>";
            }
            else
                return "1";
        }
    }
}