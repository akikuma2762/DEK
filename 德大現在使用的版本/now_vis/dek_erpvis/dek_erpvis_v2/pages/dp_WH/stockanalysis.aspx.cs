using dek_erpvis_v2.cls;
using Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using dekERP_dll.dekErp;

namespace dek_erpvis_v2.pages.dp_WH
{
    public partial class stockanalysis_New : System.Web.UI.Page
    {
        //-------------------------------------------------參數 OR 引用------------------------------------------------------------
        int day = 0;
        string acc = "";
        public string color = "";
        public string path = "";
        public string _val總庫存 = "";
        public string _val一般庫存 = "";
        public string _val逾期庫存 = "";
        public string subtitle = "";
        public string pie_data_points = "";
        public string col_data_points_nor = "";
        public string col_data_points_sply = "";
        public StringBuilder th = new StringBuilder();
        public StringBuilder tr = new StringBuilder();
        public StringBuilder th_money = new StringBuilder();
        public StringBuilder tr_money = new StringBuilder();
        DataTable dt_monthtotal = new DataTable();
        ERP_House WHE = new ERP_House();
        myclass myclass = new myclass();
        List<string> avoid_again = new List<string>();
        List<string> Line_Name = new List<string>();
        int total = 0;
        string datetime = "";
        //----------------------------------------------------Event----------------------------------------------------------------------
        //載入事件
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                path = 德大機械.get_title_web_path("WHE");
                color = HtmlUtil.change_color(acc);
                if (myclass.user_view_check("stockanalysis", acc))
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
                    Response.Write("<script>alert('您無此權限!');location.href='../index.aspx';</script>");
            }
            else
                Response.Redirect(myclass.logout_url);
        }
        //查詢事件
        protected void button_select_Click(object sender, EventArgs e)
        {
            MainProcess();
        }

        //-----------------------------------------------------Function---------------------------------------------------------------------
        //需要執行的程式
        private void MainProcess()
        {
            Get_MonthTotal();
            Set_Chart();
            Set_Table();
            Set_MoneyTable();
        }

        //取得所有的庫存
        private void Get_MonthTotal()
        {
            dt_monthtotal = WHE.stockanalysis_Details(dropdownlist_Factory.SelectedItem.Value);
            if (HtmlUtil.Check_DataTable(dt_monthtotal))
            {
                dt_monthtotal = myclass.Add_LINE_GROUP(dt_monthtotal).ToTable();
                //移除產線代號
                dt_monthtotal.Columns.Remove("產線代號");
                dt_monthtotal.Columns["產線群組"].SetOrdinal(1);
            }
        }

        //設定直方圖與圓餅圖數據
        private void Set_Chart()
        {
            if (HtmlUtil.Check_DataTable(dt_monthtotal))
            {
                //--------------------------------------------------------------圓餅圖----------------------------------------------------------------------------------------
               
                if (CheckBox_All.Checked)
                {
                    subtitle = "所有庫存";
                    day = 0;
                }
                else
                {
                    subtitle = $"庫存期限大於{txt_showCount.Text}天";
                    day = DataTableUtils.toInt(txt_showCount.Text);
                }
                   

                _val總庫存 = dt_monthtotal.Rows.Count.ToString();

                string sqlcmd = $"庫存天數>='{day}'";
                DataRow[] rows = dt_monthtotal.Select(sqlcmd);
                _val逾期庫存 = rows.Length.ToString();

                _val一般庫存 = (dt_monthtotal.Rows.Count - rows.Length).ToString();

                pie_data_points = "";
                pie_data_points += "{" +
                                  $"y:{dt_monthtotal.Rows.Count - rows.Length},name:'一般庫存', label:'一般庫存'  " +
                                   "},";
                pie_data_points += "{" +
                                    $"y:{rows.Length},name:'逾期庫存', label:'逾期庫存',exploded: true" +
                                    "},";
                //-------------------------------------------------------雙色直方圖----------------------------------------------------------------------------------------------
                //取得目前所有產線
                DataTable Line = dt_monthtotal.DefaultView.ToTable(true, new string[] { "產線群組" });
                List<string> Line_Number = new List<string>();
                foreach (DataRow row in Line.Rows)
                    Line_Number.Add(DataTableUtils.toString(row["產線群組"]));

                //產生一般庫存的表格
                DataTable dt_Normal = HtmlUtil.PrintChart_DataTable(dt_monthtotal, "產線群組", "數量", $" and 庫存天數<'{day}'", true);
                //產生逾期庫存的表格
                DataTable dt_Overdue = HtmlUtil.PrintChart_DataTable(dt_monthtotal, "產線群組", "數量", $" and 庫存天數>='{day}'", true);

                col_data_points_sply = HtmlUtil.Set_Chart(dt_Normal, "產線群組", "數量", "", out _, out _, Line_Number, false);
                col_data_points_nor = HtmlUtil.Set_Chart(dt_Overdue, "產線群組", "數量", "", out _, out _, Line_Number, false);
            }
        }

        //設定逾期表格
        private void Set_Table()
        {
            if (HtmlUtil.Check_DataTable(dt_monthtotal))
            {
                DataTable dt_Copy = dt_monthtotal.Clone();

                string sqlcmd = CheckBox_All.Checked ? "" : $"庫存天數>='{day}'";
                datetime = CheckBox_All.Checked ? "0" : day.ToString();
                DataRow[] rows = dt_monthtotal.Select(sqlcmd);
                for (int i = 0; i < rows.Length; i++)
                    dt_Copy.ImportRow(rows[i]);
                dt_Copy.AcceptChanges();

                DataTable Line = dt_Copy.DefaultView.ToTable(true, new string[] { "產線群組" });
                DataTable Custom = dt_Copy.DefaultView.ToTable(true, new string[] { "客戶簡稱" });

                //新增產線
                foreach (DataRow row in Line.Rows)
                {
                    Custom.Columns.Add(DataTableUtils.toString(row["產線群組"]));
                    Line_Name.Add(DataTableUtils.toString(row["產線群組"]));
                }
                //新增小計
                Custom.Columns.Add("小計");

                string column = "";
                if (HtmlUtil.Check_DataTable(Custom))
                {
                    th.Append(HtmlUtil.Set_Table_Title(Custom, out column));
                    tr.Append(HtmlUtil.Set_Table_Content(Custom, column, stockanalysis_callback));
                }
                else
                    HtmlUtil.NoData(out th, out tr);
            }
            else
                HtmlUtil.NoData(out th, out tr);
        }

        //例外處理
        private string stockanalysis_callback(DataRow row, string field_name)
        {
            if (avoid_again.IndexOf(DataTableUtils.toString(row["客戶簡稱"]).Trim()) == -1)
            {
                string value = "";
                if (Line_Name.IndexOf(field_name) != -1)
                {
                    string sqlcmd = $"客戶簡稱='{row["客戶簡稱"]}' and 產線群組='{field_name}' and 庫存天數>='{day}'";
                    DataRow[] rows = dt_monthtotal.Select(sqlcmd);
                    value = rows.Length.ToString();
                    total += rows.Length;
                }
                else if (field_name == "小計")
                {
                    string url = $"stockanalysis_details.aspx?key={WebUtils.UrlStringEncode($"cust={DataTableUtils.toString(row["客戶簡稱"])},datetime={datetime},type={dropdownlist_Factory.SelectedItem.Value}")}";
                    value = HtmlUtil.Trans_Thousand(total.ToString("0"));
                    value = $" <u><a href=\"{url}\"> {value} </a></u> ";
                    avoid_again.Add(DataTableUtils.toString(row["客戶簡稱"]).Trim());
                    total = 0;
                }

                return value == "" ? "" : $"<td>{value}</td>";
            }
            else
                return "1";
        }

        //設定庫存表格
        private void Set_MoneyTable()
        {
            if (HtmlUtil.Check_DataTable(dt_monthtotal))
            {
                //先複製
                DataTable dt_clone = dt_monthtotal.Clone();
                foreach (DataRow row in dt_monthtotal.Rows)
                    dt_clone.ImportRow(row);
                dt_clone.AcceptChanges();

                //移除原因
                dt_clone.Columns.Remove("庫存原因");

                List<string> list = new List<string>(WebUtils.GetAppSettings("cansee_stockmoney").Split(','));

                if(list.IndexOf(acc) == -1)
                    dt_clone.Columns.Remove("庫存金額");

                string column = "";
                th_money.Append(HtmlUtil.Set_Table_Title(dt_clone, out column));
                tr_money.Append(HtmlUtil.Set_Table_Content(dt_clone, column, WareHouseMoney_callback));

            }
            else
                HtmlUtil.NoData(out th_money, out tr_money);
        }

        //例外處理
        private string WareHouseMoney_callback(DataRow row, string field_name)
        {
            string value = "";
            if (field_name == "入庫日")
                value = HtmlUtil.changetimeformat(DataTableUtils.toString(row[field_name]));
            else if (field_name == "庫存金額")
                value = HtmlUtil.Trans_Thousand(DataTableUtils.toString(row[field_name]).Split('.')[0]);
            return value == "" ? "" : $"<td>{value}</td>";
        }

    }
}