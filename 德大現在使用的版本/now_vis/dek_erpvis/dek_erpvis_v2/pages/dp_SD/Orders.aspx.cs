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
    public partial class Orders_New : System.Web.UI.Page
    {
        //-------------------------------------------------參數 OR 引用------------------------------------------------------------

        public string color = "";
        public string path = "";
        public string yText = "";
        public string add_total = "";
        public string Total_All = "";
        public string Overdue_Total = "";
        public string right_title = "";
        public string 排行內總計 = "";
        public string rate = "";
        public string title = "";
        public string subtitle = "";
        public string xText = "";
        public string chart_unit = "";
        public string chart_Overdue = "";
        public string chartData = "";
        public StringBuilder th = new StringBuilder();
        public StringBuilder tr = new StringBuilder();
        public StringBuilder th_month = new StringBuilder();
        public StringBuilder tr_month = new StringBuilder();
        public string date_str = "";
        public string date_end = "";
        string acc = "";
        string dt_str = "";
        string dt_end = "";
        double total = 0;
        ERP_Sales SLS = new ERP_Sales();
        DataTable dt_monthtotal = new DataTable();
        DataTable dt_months = new DataTable();
        DataTable dt_Overdue = new DataTable();
        myclass myclass = new myclass();
        List<string> Line_Name = new List<string>();
        List<string> avoid_again = new List<string>();

        //----------------------------------------------------Event----------------------------------------------------------------------
        //載入事件
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);

                path = 德大機械.get_title_web_path("SLS");
                color = HtmlUtil.change_color(acc);

                if (myclass.user_view_check("Orders", acc))
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
        protected void Button_submit_Click(object sender, EventArgs e)
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
            get_yStringContent();
            Get_MonthTotal();
            Set_Value();
            Set_Chart();
            Set_Table();
            Set_Month();
        }

        //設定下拉選單的選項
        private void get_yStringContent()
        {
            if (dropdownlist_y.Items.Count == 0)
            {
                dropdownlist_y.Items.Add(new ListItem("數量", "QUANTITY"));
                if (德大機械.function_yn(acc, "訂單金額") == "Y")
                    dropdownlist_y.Items.Add(new ListItem("金額", "AMOUNT"));
            }
        }

        //取得本月訂單數量跟逾期數量
        private void Get_MonthTotal()
        {
            dekERP_dll.OrderStatus status = new dekERP_dll.OrderStatus();
            switch (DropDownList_orderStatus.SelectedItem.Value)
            {
                case "0":
                    status = dekERP_dll.OrderStatus.All;
                    break;
                case "1":
                    status = dekERP_dll.OrderStatus.Finished;
                    break;
                case "2":
                    status = dekERP_dll.OrderStatus.Unfinished;
                    break;
                case "3":
                    status = dekERP_dll.OrderStatus.Scheduled;
                    break;
                case "4":
                    status = dekERP_dll.OrderStatus.Unscheduled;
                    break;
                case "5":
                    status = dekERP_dll.OrderStatus.Stock;
                    break;
                case "6":
                    status = dekERP_dll.OrderStatus.Unstock;
                    break;
            }

            dt_monthtotal = SLS.Orders_Detail(dt_str, dt_end, status, dropdownlist_Factory.SelectedItem.Value);
            dt_Overdue = SLS.Orders_Over_Detail(dt_str, dropdownlist_Factory.SelectedItem.Value);
            dt_months = SLS.Orders_Detail(dt_str, dt_end, dekERP_dll.OrderStatus.All, dropdownlist_Factory.SelectedItem.Value);
            if (HtmlUtil.Check_DataTable(dt_monthtotal))
            {
                dt_monthtotal = myclass.Add_LINE_GROUP(dt_monthtotal).ToTable();
                dt_monthtotal.Columns["產線群組"].ColumnName = "產線";
                dt_monthtotal.Columns["客戶簡稱"].ColumnName = "客戶";
            }

            if (HtmlUtil.Check_DataTable(dt_Overdue))
            {
                dt_Overdue = myclass.Add_LINE_GROUP(dt_Overdue).ToTable();
                dt_Overdue.Columns["產線群組"].ColumnName = "產線";
                dt_Overdue.Columns["客戶簡稱"].ColumnName = "客戶";
            }

            if (HtmlUtil.Check_DataTable(dt_months))
            {
                dt_months = myclass.Add_LINE_GROUP(dt_months).ToTable();
                dt_months.Columns["產線群組"].ColumnName = "產線";
                dt_months.Columns["客戶簡稱"].ColumnName = "客戶";
            }
        }

        //設定各參數數值
        private void Set_Value()
        {
            title = $"{DropDownList_orderStatus.SelectedItem.Text}統計";
            subtitle = $"{HtmlUtil.changetimeformat(dt_str)}~{HtmlUtil.changetimeformat(dt_end)}";
            xText = dropdownlist_X.SelectedItem.Text;
            yText = dropdownlist_y.SelectedItem.Text;
            chart_unit = dropdownlist_y.SelectedItem.Text;
        }

        //取得直方圖
        private void Set_Chart()
        {
            if (HtmlUtil.Check_DataTable(dt_monthtotal))
            {
                //本月訂單
                DataTable dt_normal = HtmlUtil.PrintChart_DataTable(dt_monthtotal, dropdownlist_X.SelectedItem.Text, dropdownlist_y.SelectedItem.Text);
                //逾期訂單
                DataTable dt_Over = HtmlUtil.PrintChart_DataTable(dt_Overdue, dropdownlist_X.SelectedItem.Text, dropdownlist_y.SelectedItem.Text);

                List<string> List_Overdue = new List<string>();
                int yTotal = 0;
                int Overdue = 0;
                double month_count = 0;
                if (dropdownlist_X.SelectedItem.Text == "產線")
                {
                    DataTable ds = dt_normal.Copy();
                    ds.Merge(dt_Over);
                    foreach (DataRow row in ds.Rows)
                    {
                        if (List_Overdue.IndexOf(DataTableUtils.toString(row[dropdownlist_X.SelectedItem.Text])) == -1)
                            List_Overdue.Add(DataTableUtils.toString(row[dropdownlist_X.SelectedItem.Text]));
                    }
                    chartData = HtmlUtil.Set_Chart(dt_normal, dropdownlist_X.SelectedItem.Text, dropdownlist_y.SelectedItem.Text, "", out yTotal, out _, List_Overdue, false);
                }
                else
                {
                    //需先排序後
                    DataView dv = new DataView(dt_normal);
                    dv.Sort = $"{dropdownlist_y.SelectedItem.Text} desc";
                    dt_normal = dv.ToTable();

                    //取得所有的訂單金額||數量
                    foreach (DataRow row in dt_normal.Rows)
                        month_count += DataTableUtils.toDouble(DataTableUtils.toString(row[dropdownlist_y.SelectedItem.Text]));

                    //取得前N名
                    if (!CheckBox_All.Checked)
                    {
                        //留下前N名的資料
                        DataRow[] rows = dt_normal.Select();
                        for (int i = DataTableUtils.toInt(txt_showCount.Text); i < rows.Length; i++)
                            rows[i].Delete();
                        dt_normal.AcceptChanges();
                        if (dt_normal.Rows.Count < DataTableUtils.toInt(txt_showCount.Text))
                        {
                            DataTable ds = dt_normal.Copy();
                            ds.Merge(dt_Over);
                            foreach (DataRow row in ds.Rows)
                            {
                                if (List_Overdue.IndexOf(DataTableUtils.toString(row[dropdownlist_X.SelectedItem.Text])) == -1)
                                    List_Overdue.Add(DataTableUtils.toString(row[dropdownlist_X.SelectedItem.Text]));
                            }
                            chartData = HtmlUtil.Set_Chart(dt_normal, dropdownlist_X.SelectedItem.Text, dropdownlist_y.SelectedItem.Text, "", out yTotal, out _, List_Overdue, false);
                        }
                        else
                            chartData = HtmlUtil.Set_Chart(dt_normal, dropdownlist_X.SelectedItem.Text, dropdownlist_y.SelectedItem.Text, "", out yTotal, out List_Overdue, null, false);
                    }
                    else
                        chartData = HtmlUtil.Set_Chart(dt_normal, dropdownlist_X.SelectedItem.Text, dropdownlist_y.SelectedItem.Text, "", out yTotal, out List_Overdue, null, false);
                }
                chart_Overdue = HtmlUtil.Set_Chart(dt_Over, dropdownlist_X.SelectedItem.Text, dropdownlist_y.SelectedItem.Text, "", out Overdue, out _, List_Overdue, false);


                //右方顯示資訊設定
                Overdue_Total = HtmlUtil.Trans_Thousand(Overdue.ToString());
                add_total = dropdownlist_X.SelectedItem.Text == "產線" ? HtmlUtil.Trans_Thousand((yTotal + Overdue).ToString()) : HtmlUtil.Trans_Thousand((Overdue + DataTableUtils.toInt(month_count.ToString("0"))).ToString());
                Total_All = dropdownlist_X.SelectedItem.Text == "產線" ? HtmlUtil.Trans_Thousand(yTotal.ToString()) : HtmlUtil.Trans_Thousand(month_count.ToString("0"));

                if (dropdownlist_X.SelectedItem.Text == "客戶")
                {
                    排行內總計 = dropdownlist_y.SelectedItem.Text == "數量" ? HtmlUtil.Trans_Thousand(yTotal) : $"NTD {HtmlUtil.Trans_Thousand(yTotal)}";
                    if (CheckBox_All.Checked == true)
                        right_title = $"所有客戶訂單總{yText}";
                    else
                        right_title = $"前{txt_showCount.Text}名客戶訂單總{yText}";
                    rate = (DataTableUtils.toDouble(yTotal + ".00") * 100 / month_count).ToString("0");
                    divBlock.Attributes["style"] = "display:block";
                }
                else                                      //X軸= 產線
                {
                    right_title = $"訂單總{yText}";
                    rate = "100";
                    divBlock.Attributes["style"] = "display:none";

                }

                if (dropdownlist_y.SelectedItem.Text == "金額")
                {
                    month_title.Attributes["style"] = " display: inline-block; width: 18%; text-align: right;  ";
                    month_content.Attributes["style"] = "display: inline-block; width: 55%; text-align: right;";
                    over_title.Attributes["style"] = " display: inline-block; width: 18%; text-align: right;  ";
                    over_content.Attributes["style"] = "display: inline-block; width: 55%; text-align: right;";
                    total_title.Attributes["style"] = " display: inline-block; width: 18%; text-align: right;  ";
                    total_content.Attributes["style"] = "display: inline-block; width: 55%; text-align: right;";
                }
                else
                {
                    month_title.Attributes["style"] = "display:none";
                    month_content.Attributes["style"] = "display: inline-block; width: 55%; text-align: right;";
                    over_title.Attributes["style"] = "display:none";
                    over_content.Attributes["style"] = "display: inline-block; width: 55%; text-align: right;";
                    total_title.Attributes["style"] = "display:none";
                    total_content.Attributes["style"] = "display: inline-block; width: 55%; text-align: right;";
                }

            }
            else
            {
                subtitle = "沒有資料";
                排行內總計 = "沒有資料";
                rate = "0";
            }
        }

        //設定表格
        private void Set_Table()
        {
            if (HtmlUtil.Check_DataTable(dt_monthtotal))
            {
                DataTable dt_Copy = dt_monthtotal.Copy();

                if (HtmlUtil.Check_DataTable(dt_Overdue))
                    dt_Copy.Merge(dt_Overdue);

                DataTable Line = dt_Copy.DefaultView.ToTable(true, new string[] { "產線" });
                DataTable Custom = dt_Copy.DefaultView.ToTable(true, new string[] { "客戶" });

                //新增產線
                foreach (DataRow row in Line.Rows)
                {
                    Custom.Columns.Add(DataTableUtils.toString(row["產線"]));
                    Line_Name.Add(DataTableUtils.toString(row["產線"]));
                }
                //新增小計
                Custom.Columns.Add("小計");
                //新增逾期
                Custom.Columns.Add($"逾期{dropdownlist_y.SelectedItem.Text}");

                string columns = "";
                th.Append(HtmlUtil.Set_Table_Title(Custom, out columns));
                tr.Append(HtmlUtil.Set_Table_Content(Custom, columns, order_callback));
            }
            else
                HtmlUtil.NoData(out th, out tr);
        }

        //例外處理
        private string order_callback(DataRow row, string field_name)
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
                            count += DataTableUtils.toDouble(DataTableUtils.toString(rows[i][dropdownlist_y.SelectedItem.Text]));

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
                        string url = $"Orders_Details.aspx?key={WebUtils.UrlStringEncode($"cust={DataTableUtils.toString(row["客戶"])},date_str={dt_str},date_end={dt_end},codi={dropdownlist_y.SelectedItem.Value},type={dropdownlist_Factory.SelectedItem.Value},mode={"order"},dt_Row={DataTableUtils.toString(row["客戶"])}")}";
                        value = HtmlUtil.Trans_Thousand(total.ToString("0"));
                        value = $" align=\"right\"><u><a href=\"{url}\"> {value} </a></u> ";
                    }
                    else
                        value = $" align=\"right\">0";
                }

                //逾期數量的處理
                else if (field_name == $"逾期{dropdownlist_y.SelectedItem.Text}")
                {
                    if (HtmlUtil.Check_DataTable(dt_Overdue))
                    {
                        DataRow[] rows = dt_Overdue.Select($"客戶='{row["客戶"]}'");
                        if (rows != null && rows.Length > 0)
                        {
                            total = 0;
                            for (int i = 0; i < rows.Length; i++)
                                total += DataTableUtils.toDouble(DataTableUtils.toString(rows[i][dropdownlist_y.SelectedItem.Text]));

                            if (total == 0)
                                value = "align=\"right\"> 0";
                            else
                            {
                                string url = $"Orders_Details.aspx?key={WebUtils.UrlStringEncode($"cust={DataTableUtils.toString(row["客戶"])},date_str={dt_str},type={dropdownlist_Factory.SelectedItem.Value},mode={"order_Overdue"},dt_Row={DataTableUtils.toString(row["客戶"])}")}";
                                value = HtmlUtil.Trans_Thousand(total.ToString("0"));
                                value = $" align=\"right\"><u><a href=\"{url}\"> {value} </a></u> ";
                            }
                        }
                        else
                            value = " align=\"right\"> 0";
                    }
                    else
                        value = " align=\"right\"> 0";
                    avoid_again.Add(DataTableUtils.toString(row["客戶"]).Trim());
                }
                return value == "" ? "" : $"<td {value} </td>";
            }
            else
                return "1";
        }

        //計算各月份的數量用
        private void Set_Month()
        {
            if (HtmlUtil.Check_DataTable(dt_months))
            {
                DataTable dt_Copy = dt_months.Clone();

                //清空陣列
                Line_Name.Clear();
                avoid_again.Clear();

                dt_Copy.Columns["計算月份"].ReadOnly = false;
                dt_Copy.Columns["計算月份"].DataType = typeof(string);
                dt_Copy.Columns["計算月份"].MaxLength = 15;
                //先匯入過
                foreach (DataRow row in dt_months.Rows)
                    dt_Copy.ImportRow(row);

                //改格式
                foreach (DataRow row in dt_Copy.Rows)
                    row["計算月份"] = DataTableUtils.toString(row["計算月份"]).Insert(4, "/");

                DataTable month = dt_Copy.DefaultView.ToTable(true, new string[] { "計算月份" });
                DataTable target = dt_Copy.DefaultView.ToTable(true, new string[] { dropdownlist_X.SelectedItem.Text });

                //新增總計
                target.Rows.Add("總計");

                //新增產線
                foreach (DataRow row in month.Rows)
                {
                    target.Columns.Add(DataTableUtils.toString(row["計算月份"]));
                    Line_Name.Add(DataTableUtils.toString(row["計算月份"]));
                }
                //新增小計
                target.Columns.Add("小計");
                //新增逾期 20220615
                target.Columns.Add($"逾期{dropdownlist_y.SelectedItem.Text}");
                string columns = "";
                th_month.Append(HtmlUtil.Set_Table_Title(target, out columns));
                tr_month.Append(HtmlUtil.Set_Table_Content(target, columns, order_month_callback));
            }
            else
                HtmlUtil.NoData(out th_month, out tr_month);
        }

        //例外處理
        private string order_month_callback(DataRow row, string field_name)
        {
            string url = "";
            string value = "";
            string SelectedItem = dropdownlist_X.SelectedItem.Text;
            if (avoid_again.IndexOf(DataTableUtils.toString(row[SelectedItem]).Trim()) == -1)
            {
                if (Line_Name.IndexOf(field_name) != -1)
                {
                    string sqlcmd = DataTableUtils.toString(row[SelectedItem]) != "總計" ? $"{SelectedItem}='{row[SelectedItem]}' and 計算月份={field_name.Replace("/", "")}" : $"計算月份={field_name.Replace("/", "")}";
                    DataRow[] rows = dt_months.Select(sqlcmd);
                    value = rows.Length > 0 ? rows.Length.ToString() : "";
                }
                else if (field_name == "小計")
                {
                    string sqlcmd = DataTableUtils.toString(row[SelectedItem]) != "總計" ? $"{SelectedItem}='{row[SelectedItem]}'" : "";
                    DataRow[] rows = dt_months.Select(sqlcmd);
                    value = rows.Length.ToString();
                    //新增小計超連結20220615
                    int total = int.Parse(value);
                    if (total != 0)
                    {
                        url = mk_url(row, SelectedItem, "month");
                        value = HtmlUtil.Trans_Thousand(total.ToString("0"));
                        value = $"<u><a href=\"{url}\"> {value} </a></u> ";
                    }
                    if (row[0].ToString().Trim() == "總計")
                    {
                        url = mk_url(row, SelectedItem,"month_total");
                        value = HtmlUtil.Trans_Thousand(total.ToString("0"));
                        value = $"<u><a href=\"{url}\"> {value} </a></u> ";
                    }
                }
                //新增逾期數量處裡20220615
                else if (field_name == $"逾期{dropdownlist_y.SelectedItem.Text}")
                {
                    avoid_again.Add(DataTableUtils.toString(row[SelectedItem]).Trim());

                    if (HtmlUtil.Check_DataTable(dt_Overdue))
                    {
                        DataRow[] rows = new DataRow[] { };
                        if (SelectedItem == "產線")
                        {
                            rows = dt_Overdue.Select($"產線='{row["產線"]}'");
                        }
                        else if (SelectedItem == "客戶")
                        {
                            rows = dt_Overdue.Select($"客戶='{row["客戶"]}'");
                        }
                        if (rows != null && rows.Length > 0)
                        {
                            total = 0;
                            for (int i = 0; i < rows.Length; i++)
                                total += DataTableUtils.toDouble(DataTableUtils.toString(rows[i][dropdownlist_y.SelectedItem.Text]));
                            if (total == 0)
                                value = "0";
                            else
                            {
                                url = mk_url(row, SelectedItem, "month_Overdue");
                                value = HtmlUtil.Trans_Thousand(total.ToString("0"));
                                value = $"<u><a href=\"{url}\"> {value} </a></u> ";
                            }
                        }
                        else
                        {
                            value = "0";
                        }

                        //計算逾期總計20220617
                        if (row[0].ToString().Trim() == "總計")
                        {
                            DataRow[] dt_rows = dt_Overdue.Select($"產線<>'T1'");
                            total = 0;
                            for (int i = 0; i < dt_rows.Length; i++)
                                total += DataTableUtils.toDouble(DataTableUtils.toString(dt_rows[i][dropdownlist_y.SelectedItem.Text]));
                            if (total == 0)
                                value = "0";
                            else
                            {
                                url = mk_url(row, SelectedItem, "month_Overdue_Total");
                                value = HtmlUtil.Trans_Thousand(total.ToString("0"));
                                value = $"<u><a href=\"{url}\"> {value} </a></u> ";
                            }
                        }
                    }
                    else
                    {
                        value = "0";
                    }
                }
                return value == "" ? "" : $"<td align=\"right\"> {value} </td>";
            }
            else
                return "1";
        }

        //傳遞orderDetails參數0617
        private string mk_url(DataRow row,string SelectedItem,string mode) {
            string url = "";
           string new_dt_end = mode.Contains("Overdue") ? "": dt_end;
            if (SelectedItem == "產線")
            {
                url = $"Orders_Details.aspx?key={WebUtils.UrlStringEncode($"selectItem_X={DataTableUtils.toString(SelectedItem)},date_str={dt_str},date_end={new_dt_end},codi={dropdownlist_y.SelectedItem.Value},type={dropdownlist_Factory.SelectedItem.Value},mode={$"order_{mode}"},dt_Row={DataTableUtils.toString(row["產線"])}")}";
            }
            else if (SelectedItem == "客戶")
            {
                url = $"Orders_Details.aspx?key={WebUtils.UrlStringEncode($"selectItem_X={DataTableUtils.toString(SelectedItem)},date_str={dt_str},date_end={new_dt_end},codi={dropdownlist_y.SelectedItem.Value},type={dropdownlist_Factory.SelectedItem.Value},mode={$"order_{mode}"},dt_Row={DataTableUtils.toString(row["客戶"])}")}";
            }
            return url;
        }
    }
}