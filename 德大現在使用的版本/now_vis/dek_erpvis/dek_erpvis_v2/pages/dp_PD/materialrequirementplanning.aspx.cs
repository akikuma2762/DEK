using dek_erpvis_v2.cls;
using Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using dek_erpvis_v2.webservice;
using System.Web.UI.WebControls;
using dekERP_dll;
using dekERP_dll.dekErp;
using System.Text;

namespace dek_erpvis_v2.pages.dp_PD
{
    public partial class materialrequirementplanning_New : System.Web.UI.Page
    {
        //-------------------------------------------------參數 OR 引用------------------------------------------------------------
        public string color = "";
        public StringBuilder th = new StringBuilder();
        public StringBuilder tr = new StringBuilder();
        public string title_text = "";
        public string type = "";
        public string type_code = "";
        public string title = "";
        public string search_condi = "";
        public string safty_text = "";
        public string min_text = "";
        public string chart_card_text = "";
        string dt_str = "";
        string dt_end = "";
        public string date_str = "";
        public string date_end = "";
        public string path = "";
        string acc = "";
        double total_var;
        double month_var;
        DataTable dt_monthtotal = new DataTable();
        ERP_Materials PCD = new ERP_Materials();
        List<string> avoid_again = new List<string>();
        List<string> use_list = new List<string>();

        //----------------------------------------------------Event----------------------------------------------------------------------
        //載入事件
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                path = 德大機械.get_title_web_path("PCD");
                color = HtmlUtil.change_color(acc);
                if (myclass.user_view_check("materialrequirementplanning", acc))
                {
                    if (!IsPostBack)
                    {
                        string[] date = 德大機械.德大專用月份(acc).Split(',');
                        dt_str = date[0];
                        dt_end = date[1];
                        date_str = date[0];
                        date_end = date[1];
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
                    Response.Write("<script>alert('您無法瀏覽此頁面 請向該單位主管申請權限!');location.href='../index.aspx';</script>");
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
            Set_Value();
            Set_Table();
        }

        //取得本月領料數量
        private void Get_MonthTotal()
        {
            string item_name = "";
            switch (DropDownList_select_type.SelectedItem.Value)
            {
                case "0":
                    item_name = DropDownList_materialstype.SelectedItem.Value;
                    break;
                case "1":
                    item_name = TextBox_keyword.Text;
                    break;
                case "2":
                    item_name = TextBox_keyword.Text;
                    break;
            }
            //dt_monthtotal = PCD.materialrequirementplanning_Detail(dt_str, dt_end, RadioButtonList_select_type, item_name, dropdownlist_Factory.SelectedItem.Value);
            dt_monthtotal = PCD.materialrequirementplanning_Detail_DropDownList(dt_str, dt_end, DropDownList_select_type, item_name, dropdownlist_Factory.SelectedItem.Value);
            if (HtmlUtil.Check_DataTable(dt_monthtotal))
            {
                dt_monthtotal.Columns["領料單明細品號"].ColumnName = "品號";
                dt_monthtotal.Columns["領料單明細品名規格"].ColumnName = "品名規格";
            }

        }

        //設定各參數數值
        private void Set_Value()
        {
            min_text = demo_vertical2.Value;
            safty_text = demo_vertical3.Value;
        }

        //設定表格
        private void Set_Table()
        {
            if (HtmlUtil.Check_DataTable(dt_monthtotal))
            {
                DataTable Item = dt_monthtotal.DefaultView.ToTable(true, new string[] { "品號" });
                if (HtmlUtil.Check_DataTable(Item))
                {
                    Item.Columns.Add("品名規格");
                    DataTable use = dt_monthtotal.DefaultView.ToTable(true, new string[] { "用途說明" });
                    foreach (DataRow row in use.Rows)
                    {
                        Item.Columns.Add(DataTableUtils.toString(row["用途說明"]));
                        use_list.Add(DataTableUtils.toString(row["用途說明"]));
                    }
                    Item.Columns.Add("總計");
                    Item.Columns.Add("月用量");
                    Item.Columns.Add($"安全存量(X{safty_text})");
                    Item.Columns.Add($"最小採購量(X{min_text})");
                    Item.Columns.Add("使用客戶與數量");
                }
                string column = "";
                th.Append(HtmlUtil.Set_Table_Title(Item, out column));
                tr.Append(HtmlUtil.Set_Table_Content(Item, column, materialrequirementplanning_callback));
            }
            else
                HtmlUtil.NoData(out th, out tr);
        }

        //例外處理
        private string materialrequirementplanning_callback(DataRow row, string field_name)
        {
            string value = "";
            string sqlcmd = "";
            if (avoid_again.IndexOf(DataTableUtils.toString(row["品號"])) == -1)
            {
                if (field_name == "品號")
                {
                    string url = $"materialrequirementplanning_details.aspx?key={WebUtils.UrlStringEncode($"item_code={row[field_name]},date_str={dt_str},date_end={dt_end},type={dropdownlist_Factory.SelectedItem.Value}")}";
                    value = $" <u><a href=\"{url}\">{row[field_name]}</a></u>";
                }
                else if (field_name == "品名規格")
                {
                    sqlcmd = $"品號='{row["品號"]}'";
                    DataRow[] rows = dt_monthtotal.Select(sqlcmd);

                    if (rows != null && rows.Length > 0)
                        value = DataTableUtils.toString(rows[0][field_name]);
                }
                else if (use_list.IndexOf(field_name) != -1)
                {
                    total_var = 0;
                    sqlcmd = $"品號='{row["品號"]}' and 用途說明='{field_name}'";
                    DataRow[] rows = dt_monthtotal.Select(sqlcmd);
                    if (rows != null && rows.Length > 0)
                    {
                        for (int i = 0; i < rows.Length; i++)
                            total_var += DataTableUtils.toDouble(DataTableUtils.toString(rows[i]["領料數量"]));
                    }
                    month_var += total_var;
                    value = total_var.ToString("0");
                }
                else if (field_name == "總計")
                    value = month_var.ToString("0");
                else if (field_name == "月用量")
                    value = (month_var / DaysBetween(dt_str, dt_end)).ToString("0.00");
                else if (field_name == $"安全存量(X{safty_text})")
                    value = (month_var / DaysBetween(dt_str, dt_end) * DataTableUtils.toDouble(safty_text)).ToString("0.00");
                else if (field_name == $"最小採購量(X{min_text})")
                    value = (month_var / DaysBetween(dt_str, dt_end) * DataTableUtils.toDouble(min_text)).ToString("0.00");
                else if (field_name == "使用客戶與數量")
                {
                    var custom = dt_monthtotal.AsEnumerable().Where(w => w.Field<string>("品號") == DataTableUtils.toString(row["品號"])).GroupBy(g => g.Field<string>("使用客戶").Trim()).ToDictionary(t => t.Key);
                    foreach (string key in custom.Keys)
                        value += $"{key}:{custom[key].Count()}, ";
                    month_var = 0;
                }
                return value == "" ? "" : $"<td>{value}</td>";
            }
            else
                return "1";
        }

        //計算有幾個月
        private int DaysBetween(string date_str, string date_end)
        {
            DateTime dt1 = DateTime.ParseExact(date_str, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
            DateTime dt2 = DateTime.ParseExact(date_end, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
            int ans = (dt2.Year - dt1.Year) * 12 + (dt2.Month - dt1.Month);
            if (ans <= 0) ans = 1;
            return ans;
        }
    }
}