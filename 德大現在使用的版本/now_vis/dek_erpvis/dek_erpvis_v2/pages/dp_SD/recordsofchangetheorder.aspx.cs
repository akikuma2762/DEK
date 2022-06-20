using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using dek_erpvis_v2.cls;
using dekERP_dll.dekErp;
using Support;

namespace dek_erpvis_v2.pages.dp_SD
{
    public partial class recordsofchangetheorder_New : System.Web.UI.Page
    {
        //-------------------------------------------------參數 OR 引用------------------------------------------------------------
        string acc = "";
        string dt_str = "";
        string dt_end = "";
        public string color = "";
        public string path = "";
        public string title = "";
        public string subtitle = "";
        public string col_data_Points = "";
        public string date_str = "";
        public string date_end = "";
        public StringBuilder th = new StringBuilder();
        public StringBuilder tr = new StringBuilder();
        DataTable dt_monthtotal = new DataTable();
        ERP_Sales SLS = new ERP_Sales();
        List<string> avoid_again = new List<string>();
        DataTable Custom = new DataTable();
        public List<string> count_list = new List<string>();

        //-----------------------------------------------------EVENT-----------------------------------------------------------------------------
        //載入事件
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                //避免有問題
                count_list.Add("");
                count_list.Add("");
                count_list.Add("");
                count_list.Add("");


                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                path = 德大機械.get_title_web_path("SLS");
                color = HtmlUtil.change_color(acc);
                if (myclass.user_view_check("recordsofchangetheorder", acc))
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
            Set_Chart();
            Set_Table();
            Set_ListTotal();
        }

        //取得本月變更次數
        private void Get_MonthTotal()
        {
            dt_monthtotal = SLS.Recordsofchangetheorder_Details(dt_str, dt_end, dropdownlist_Factory.SelectedItem.Value);
        }

        //設定統計圖
        private void Set_Chart()
        {
            if (HtmlUtil.Check_DataTable(dt_monthtotal))
            {
                double count = 0;
                //產生變更次數
                DataTable dt_Change = HtmlUtil.PrintChart_DataTable(dt_monthtotal, "客戶名稱", "數量", "", true);

                //需先排序後
                DataView dv = new DataView(dt_Change);
                dv.Sort = $"數量 desc";
                dt_Change = dv.ToTable();

                if (!CheckBox_All.Checked)
                {
                    //留下前N名的資料
                    DataRow[] rows = dt_Change.Select();
                    for (int i = DataTableUtils.toInt(txt_showCount.Text); i < rows.Length; i++)
                        rows[i].Delete();
                    dt_Change.AcceptChanges();

                    foreach (DataRow row in dt_Change.Rows)
                        count += DataTableUtils.toDouble(DataTableUtils.toString(row["數量"]));
                }
                col_data_Points = HtmlUtil.Set_Chart(dt_Change, "客戶名稱", "數量", "", out _);
                title = CheckBox_All.Checked ? $"所有客戶訂單變更：{dt_monthtotal.Rows.Count}次" : $"前{txt_showCount.Text}名客戶訂單變更次數:{count:0}次";
                subtitle = $"{HtmlUtil.changetimeformat(dt_str)}~{HtmlUtil.changetimeformat(dt_end)}";
            }
        }

        //設定統計表
        private void Set_Table()
        {
            if (HtmlUtil.Check_DataTable(dt_monthtotal))
            {
                Custom = dt_monthtotal.DefaultView.ToTable(true, new string[] { "客戶名稱", "訂單號碼" });
                Custom.Columns.Add("變更次數");
                Custom.Columns["訂單號碼"].ColumnName = "訂單次數";

                string column = "";
                th.Append(HtmlUtil.Set_Table_Title(Custom, out column));
                tr.Append(HtmlUtil.Set_Table_Content(Custom, column, recordsofchangetheorder_callback));
            }
            else
                HtmlUtil.NoData(out th, out tr);
        }

        //例外處理
        private string recordsofchangetheorder_callback(DataRow row, string field_name)
        {
            if (avoid_again.IndexOf(DataTableUtils.toString(row["客戶名稱"]).Trim()) == -1 && DataTableUtils.toString(row["客戶名稱"]).Trim() != "")
            {
                string value = "";
                if (field_name == "訂單次數")
                {
                    string sqlcmd = $"客戶名稱='{row["客戶名稱"]}' ";
                    DataRow[] rows = Custom.Select(sqlcmd);
                    value = rows.Length.ToString();
                }
                else if (field_name == "變更次數")
                {
                    string sqlcmd = $"客戶名稱='{row["客戶名稱"]}'";
                    DataRow[] rows = dt_monthtotal.Select(sqlcmd);
                    string url = $"recordsofchangetheorder_details.aspx?key={WebUtils.UrlStringEncode($"cust={DataTableUtils.toString(row["客戶名稱"])},date_str={dt_str},date_end={dt_end},type={dropdownlist_Factory.SelectedItem.Value}")}";
                    value = $"<u><a href=\"{url}\"> {rows.Length} </a></u> ";
                    avoid_again.Add(DataTableUtils.toString(row["客戶名稱"]).Trim());
                }
                return value == "" ? "" : $"<td>{value}</td>";
            }
            else
                return "1";
        }

        //取得以下數值
        //1.本月訂單變更量
        //2.變更交期至本月
        //3.變更交期至他月
        //4.本月提前或落後
        private void Set_ListTotal()
        {
            //新增本月變更總數量
            count_list[0] = HtmlUtil.Check_DataTable(dt_monthtotal) ? dt_monthtotal.Rows.Count.ToString() : "0";

            //新增他月改至本月數量
            DataTable dt = SLS.Change_Date_To_Now(date_str.Replace("-", ""), date_end.Replace("-", ""), dropdownlist_Factory.SelectedItem.Value);
            count_list[1] = HtmlUtil.Check_DataTable(dt) ? $"<u><a href=\"recordsofchangetheorder_details.aspx?key={WebUtils.UrlStringEncode($"cust=,date_str={dt_str},date_end={dt_end},type={dropdownlist_Factory.SelectedItem.Value},genre=1")}\"> {dt.Rows.Count} </a></u> ": "0";


            //新增本月改至他月數量
            dt = SLS.Change_Date_To_Other(date_str.Replace("-", ""), date_end.Replace("-", ""), dropdownlist_Factory.SelectedItem.Value);
            count_list[2] = HtmlUtil.Check_DataTable(dt) ? $"<u><a href=\"recordsofchangetheorder_details.aspx?key={WebUtils.UrlStringEncode($"cust=,date_str={dt_str},date_end={dt_end},type={dropdownlist_Factory.SelectedItem.Value},genre=2")}\"> {dt.Rows.Count} </a></u> " : "0";

            //新增本月提前或是落後數量
            dt = SLS.Change_Now_To_Now(date_str.Replace("-", ""), date_end.Replace("-", ""), dropdownlist_Factory.SelectedItem.Value);
            count_list[3] = HtmlUtil.Check_DataTable(dt) ? $"<u><a href=\"recordsofchangetheorder_details.aspx?key={WebUtils.UrlStringEncode($"cust=,date_str={dt_str},date_end={dt_end},type={dropdownlist_Factory.SelectedItem.Value},genre=3")}\"> {dt.Rows.Count} </a></u> " : "0";
        }
    }
}