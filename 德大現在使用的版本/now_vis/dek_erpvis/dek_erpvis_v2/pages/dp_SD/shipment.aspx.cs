using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Support;
using dek_erpvis_v2.cls;
using dek_erpvis_v2.webservice;
using System.Data.OleDb;
using dekERP_dll;
using dekERP_dll.dekErp;
using System.Configuration;
using System.Text;

namespace dek_erpvis_v2.pages.dp_SD
{
    public partial class shipment_New : System.Web.UI.Page
    {
        //-------------------------------------------------參數 OR 引用------------------------------------------------------------    
        public string color = "";
        public string path = "";
        public string title = "";
        public string subtitle = "";
        public string xText = "";
        public string col_data_Points = "";
        public string date_str = "";
        public string date_end = "";
        public StringBuilder th = new StringBuilder();
        public StringBuilder tr = new StringBuilder();
        string acc = "";
        string dt_str = "";
        string dt_end = "";
        double total = 0;
        ERP_Sales SLS = new ERP_Sales();
        DataTable dt_monthtotal = new DataTable();
        myclass myclass = new myclass();
        List<string> Line_Name = new List<string>();
        List<string> avoid_again = new List<string>();
        //-----------------------------------------------------EVENT-----------------------------------------------------------------------------
        //載入事件
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);

                path = 德大機械.get_title_web_path("SLS");
                color = HtmlUtil.change_color(acc);
                if (myclass.user_view_check("shipment", acc))
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
        }

        //取得本月出貨數
        private void Get_MonthTotal()
        {
            dt_monthtotal = SLS.Shipment_Detail(dt_str, dt_end, dropdownlist_Factory.SelectedItem.Value);

            if (HtmlUtil.Check_DataTable(dt_monthtotal))
            {
                dt_monthtotal = myclass.Add_LINE_GROUP(dt_monthtotal).ToTable();
                dt_monthtotal.Columns["產線群組"].ColumnName = "產線";
                dt_monthtotal.Columns["客戶簡稱"].ColumnName = "客戶";
                dt_monthtotal.Columns["小計"].ColumnName = "數量";
            }
        }

        //取得直方圖
        private void Set_Chart()
        {
            if (HtmlUtil.Check_DataTable(dt_monthtotal))
            {
                int count = 0;
                DataTable dt_invo = HtmlUtil.PrintChart_DataTable(dt_monthtotal, dropdownlist_X.SelectedItem.Text, "數量");

                if (dropdownlist_X.SelectedItem.Text == "客戶")
                {
                    DataView dv = new DataView(dt_invo);
                    dv.Sort = $"數量 desc";
                    dt_invo = dv.ToTable();

                    //取得前N名
                    if (!CheckBox_All.Checked)
                    {
                        //留下前N名的資料
                        DataRow[] rows = dt_invo.Select();
                        for (int i = DataTableUtils.toInt(txt_showCount.Text); i < rows.Length; i++)
                            rows[i].Delete();
                        dt_invo.AcceptChanges();
                    }
                }
                col_data_Points = HtmlUtil.Set_Chart(dt_invo, dropdownlist_X.SelectedItem.Text, "數量", "", out count);

                title = $"總出貨數量 : {count} 台";
                subtitle = $"{HtmlUtil.changetimeformat(dt_str)}~{HtmlUtil.changetimeformat(dt_end)}";
                xText = $"{dropdownlist_X.SelectedItem.Text}";
            }
            else
                subtitle = "沒有資料";
        }

        //設定表格
        private void Set_Table()
        {
            if (HtmlUtil.Check_DataTable(dt_monthtotal))
            {
                DataTable Line = dt_monthtotal.DefaultView.ToTable(true, new string[] { "產線" });
                DataTable Custom = dt_monthtotal.DefaultView.ToTable(true, new string[] { "客戶" });

                //新增產線
                foreach (DataRow row in Line.Rows)
                {
                    Custom.Columns.Add(DataTableUtils.toString(row["產線"]));
                    Line_Name.Add(DataTableUtils.toString(row["產線"]));
                }
                //新增小計
                Custom.Columns.Add("小計");
                string columns = "";
                th.Append(HtmlUtil.Set_Table_Title(Custom, out columns));
                tr.Append(HtmlUtil.Set_Table_Content(Custom, columns, shipment_callback));

            }
            else
                HtmlUtil.NoData(out th, out tr);
        }

        //例外處理
        private string shipment_callback(DataRow row, string field_name)
        {
            string value = "";
            if (avoid_again.IndexOf(DataTableUtils.toString(row["客戶"]).Trim()) == -1)
            {
                //產線的處理
                if (Line_Name.IndexOf(field_name) != -1)
                {
                    if (Line_Name.IndexOf(field_name) == 0)
                        total = 0;

                    string sqlcmd = $"客戶='{DataTableUtils.toString(row["客戶"])}' and 產線='{field_name}'";
                    DataRow[] rows = dt_monthtotal.Select(sqlcmd);
                    if (rows != null && rows.Length > 0)
                    {
                        double count = 0;
                        for (int i = 0; i < rows.Length; i++)
                            count += DataTableUtils.toDouble(DataTableUtils.toString(rows[i]["數量"]));

                        value = $" align=\"right\">{count:0}</a></u> ";
                        total += count;
                    }
                    else
                        value = $" align=\"right\">0</a></u> ";
                }

                //小計的處理
                else if (field_name == "小計")
                {
                    if (total != 0)
                    {
                        string url = $"shipment_details.aspx?key={WebUtils.UrlStringEncode($"cust={DataTableUtils.toString(row["客戶"])},date_str={dt_str},date_end={dt_end},type={dropdownlist_Factory.SelectedItem.Value}")}";
                        value = HtmlUtil.Trans_Thousand(total.ToString("0"));
                        value = $" align=\"right\"><u><a href=\"{url}\"> {value} </a></u> ";
                    }
                    else
                        value = $" align=\"right\">0";
                    avoid_again.Add(DataTableUtils.toString(row["客戶"]).Trim());
                }
                return value == "" ? "" : $"<td {value} </td>";
            }
            else
                return "1";
        }
    }
}