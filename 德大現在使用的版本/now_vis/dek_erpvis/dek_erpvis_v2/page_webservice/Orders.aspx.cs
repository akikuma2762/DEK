using dek_erpvis_v2.cls;
using dek_erpvis_v2.webservice;
using ERPVIS;
using Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace dek_erpvis_v2.page_webservice
{
    public partial class Orders : System.Web.UI.Page
    {
        public string unit = "";
        public string color = "";//顏色修改相關
        public string path = "";
        public string title = "";
        public string subtitle = "";
        public string dt_st = "";
        public string dt_ed = "";
        public string chart_unit = "數量";
        public string chartType = "";
        public string right_title = "";
        public string xString = "";
        public string yString = "";
        public string chartData = "";
        public string status = "";
        public string Xtext = "";
        public string Ytext = "";
        string statusText = "";
        int CUST_TOTAL = 0;
        string[] str = null;
        public string th = "";
        public string tr = "";
        public string 排行內總計 = "";
        public string rate = "";
        public string Total = "";
        public int yTotal = 0;
        string acc = "";
        string URL_NAME = "";
        string view_YN = "N";
        int count = 0;
        DataTable Line = new DataTable();
        DataTable custom = new DataTable();
        DataTable dt = new DataTable();
        myclass myclass = new myclass();
        德大機械 德大機械 = new 德大機械();
        Service service = new Service();

        //Event-------------------------------
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                path = 德大機械.get_title_web_path("SLS");
                path = path.Replace("</ol>", "<li><u><a href='../dp_SD/Order.aspx'>訂單數量與金額統計</a></u></li></ol>");
                URL_NAME = "Orders";
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                color = HtmlUtil.change_color(acc);
                if (myclass.user_view_check(URL_NAME, acc) == true)
                {
                    if (!IsPostBack)
                    {
                        string[] daterange = 德大機械.德大專用月份(acc).Split(',');
                        dt_st = daterange[0];
                        dt_ed = daterange[1];
                        textbox_dt1.Text = daterange[0];
                        textbox_dt2.Text = daterange[1];
                        get_yStringContent();
                        MainProcess();
                    }
                }
                else Response.Write("<script>alert('您無法瀏覽此頁面 請向該單位主管申請權限!');location.href='../index.aspx';</script>");
            }
            else Response.Redirect(myclass.logout_url);

        }
        protected void Button_submit_Click(object sender, EventArgs e)
        {
            string[] daterange = 德大機械.德大專用月份(acc).Split(',');
            dt_st = daterange[0];
            dt_ed = daterange[1];
            get_search_time(DataTableUtils.toString(((Control)sender).ID.Split('_')[1]));
            MainProcess();
        }
        //Function------------------------------
        private void MainProcess()
        {
            GetCondition();
            GetChartData();
            Set_HTML_Table();
        }
        private void GetCondition()
        {
            xString = dropdownlist_X.SelectedItem.Text;
            yString = dropdownlist_y.SelectedItem.Text;
            Xtext = dropdownlist_X.SelectedValue;
            Ytext = dropdownlist_y.SelectedValue;
            status = DropDownList_orderStatus.SelectedValue;
            statusText = DropDownList_orderStatus.SelectedItem.Text;
            title = dt_st + "~" + dt_ed + " " + statusText + "統計";
            count = DataTableUtils.toInt(txt_showCount.Text);
            dt_st = textbox_dt1.Text;
            dt_ed = textbox_dt2.Text;
            chartType = dropdownlist_chartType.SelectedValue;
        }
        private void GetChartData()
        {
            string Json = "";
            DataTable dt = new DataTable();
            //產線用
            if (Xtext == "Line")
            {
                Json = service.GetOrdersInformation(dt_st, dt_ed, DropDownList_orderStatus.SelectedValue, dekModel.Image, Xtext, Ytext, 0);
                right_title = "訂單總數量";
            }
            //客戶用
            else if (Xtext == "Custom")
            {
                //要先算出目前的總數量||總金額
                Json = service.GetOrdersInformation(dt_st, dt_ed, DropDownList_orderStatus.SelectedValue, dekModel.Image, Xtext, Ytext, 0);
                dt = ERPVIS.i_TEC.iTechDB.JsontoDataTable(Json);
                if (dt.Rows.Count > 0 && dt != null)
                {
                    int num = 0;
                    foreach (DataRow row in dt.Rows)
                        num += DataTableUtils.toInt(DataTableUtils.toString(row[yString]));
                    Total = TransThousand(num);
                }
                else
                    Total = "0";

                Json = service.GetOrdersInformation(dt_st, dt_ed, DropDownList_orderStatus.SelectedValue, dekModel.Image, Xtext, Ytext, count);
                right_title = "前" + count + "名客戶下單總數量";
            }
            dt = ERPVIS.i_TEC.iTechDB.JsontoDataTable(Json);

            if (Xtext == "Line")
            {
                dt = myclass.Add_LINE_GROUP(dt).ToTable();
                xString = "產線群組";
            }
            else if (Xtext == "Custom")
                xString = "客戶簡稱";

            if (dt.Rows.Count > 0)
            {
                yTotal = 0;
                chartData = HtmlUtil.Set_Chart(dt, xString, yString, "", out yTotal, out _);
                if (yString == "金額")
                {
                    unit = "NTD ";
                    chart_unit = "金額";
                    排行內總計 = "NTD " + TransThousand(yTotal);
                }
                else                      //Y軸= 數量
                {
                    排行內總計 = TransThousand(yTotal) + " 台";
                }
                //判斷X軸"客戶"或"產線"，區分顯示標題
                if (xString == "客戶簡稱") //X軸= 客戶
                {
                    subtitle = "前" + count + "名客戶總" + yString + "  " + unit + TransThousand(yTotal);
                    right_title = "前" + count + "名客戶下單總" + yString;
                    divBlock.Attributes["style"] = "display:block";
                }
                else                                      //X軸= 產線
                {
                    subtitle = "總" + yString + "  " + unit + TransThousand(yTotal);
                    right_title = "訂單總" + yString;
                    divBlock.Attributes["style"] = "display:none";
                }
                //平均占比 = 前幾名數量 / 總數量 * 100 (For右上方小圖)            
                rate = DataTableUtils.toString(DataTableUtils.toDouble(yTotal) / DataTableUtils.toDouble(Total) * 100).Split('.')[0];
            }
            else
            {
                subtitle = "沒有資料";
                排行內總計 = "沒有資料";
                rate = "0";
            }


        }
        private void Set_HTML_Table()
        {
            string Json = service.GetOrdersInformation(dt_st, dt_ed, status, dekModel.Table, Xtext, Ytext, 0);
            dt = ERPVIS.i_TEC.iTechDB.JsontoDataTable(Json);
            if (dt.Rows.Count > 0 && dt != null)
            {
                //dt.Rows.Add("測試", "80", "100", "10001");
                dt = myclass.Add_LINE_GROUP(dt).ToTable();

                dt.Columns.RemoveAt(0);
                dt.Columns["產線群組"].SetOrdinal(1);
                //測試資料
                //dt.Rows.Add("123", "123", "54886961", "89886066");
                //取得所有產線
                Line = dt.DefaultView.ToTable(true, new string[] { "產線群組" });
                //取得所有客戶
                custom = dt.DefaultView.ToTable(true, new string[] { "客戶簡稱" });

                string titlename = "";
                th = "<th>客戶簡稱</th>\n";
                th += HtmlUtil.Set_Table_Title(Line, out titlename, "產線群組");
                th += "<th>小計</th>\n";
                titlename = "客戶簡稱," + titlename + "小計,";
                str = titlename.Split(',');
                tr = HtmlUtil.Set_Table_Content(custom, titlename, order_callback);
            }
            else
                HtmlUtil.NoData(out th, out tr);
        }
        private string order_callback(DataRow row, string field_name)
        {
            string value = "";
            if (field_name == "小計")
            {
                string url = HtmlUtil.AttibuteValue("cust", DataTableUtils.toString(row["客戶簡稱"]), "").Trim() + "," +
                            HtmlUtil.AttibuteValue("date_str", dt_st, "") + "," +
                            HtmlUtil.AttibuteValue("date_end", dt_ed, "") + "," +
                            HtmlUtil.AttibuteValue("condi", DropDownList_orderStatus.SelectedValue, "");
                string href = string.Format("Orders_Details.aspx?key={0} ' target='_blank'",
                    WebUtils.UrlStringEncode(url));

                value = TransThousand(DataTableUtils.toString(CUST_TOTAL));
                value = " align='right'>" + HtmlUtil.ToTag("u", HtmlUtil.ToHref(value, href));
            }
            else if (field_name != "客戶簡稱" && field_name != "小計")
            {
                //進入第一個產線後，將上一個總和歸0
                if (field_name == str[1])
                    CUST_TOTAL = 0;
                string sqlcmd = "客戶簡稱 ='" + DataTableUtils.toString(row["客戶簡稱"]) + "' and 產線群組 = '" + field_name + "'";
                DataRow[] rows = dt.Select(sqlcmd);
                int LINE_TOTAL = 0;
                if (rows.Length == 0)
                { }
                else
                {
                    for (int i = 0; i < rows.Length; i++)
                    {
                        for (int j = 0; j < Line.Rows.Count; j++)
                        {
                            if (field_name == DataTableUtils.toString(rows[i]["產線群組"]) && field_name == DataTableUtils.toString(Line.Rows[j]["產線群組"]))
                                LINE_TOTAL += DataTableUtils.toInt(DataTableUtils.toString(rows[i][dropdownlist_y.SelectedItem.Text]));
                        }
                    }
                }
                value = ">" + TransThousand(DataTableUtils.toString(LINE_TOTAL));
                CUST_TOTAL += LINE_TOTAL;
            }

            if (value == "")
                return value;
            else
                return "<td" + value + "</td>\n";
        }
        //金額，千分位轉換
        private string TransThousand(object yValue)
        {
            int yValue_trans = DataTableUtils.toInt(DataTableUtils.toString(yValue));
            return DataTableUtils.toString(yValue_trans.ToString("N0"));
        }
        private void get_search_time(string btnID)
        {
            switch (btnID)
            {
                case "month":
                    string[] daterange = 德大機械.德大專用月份(acc).Split(',');
                    dt_st = daterange[0];
                    dt_ed = daterange[1];
                    break;
                case "submit":
                    string st_m = textbox_dt1.Text;
                    string ed_m = textbox_dt2.Text;
                    if (st_m == "" && ed_m == "") break;

                    DateTime st = DateTime.ParseExact(st_m, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime ed = DateTime.ParseExact(ed_m, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                    dt_st = st.ToString("yyyyMMdd");
                    dt_ed = ed.ToString("yyyyMMdd");
                    break;
            }
        }
        private void get_yStringContent()
        {
            // 2019.07.02，動態DropDownList，控管金額權限顯示(ru)
            view_YN = 德大機械.function_yn(acc, "訂單金額");
            dropdownlist_y.Items.Add(new ListItem("數量", "qty"));
            if (view_YN == "Y")
                dropdownlist_y.Items.Add(new ListItem("金額", "amt"));
        }
    }
}