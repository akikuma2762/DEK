using dek_erpvis_v2.cls;
using Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace dek_erpvis_v2.pages.dp_SD
{
    public partial class waitingfortheproduction : System.Web.UI.Page
    {
        public string 實際生產_data = "";
        public string 預定生產_data = "";
        public string titel_text = "'沒有資料'";//9月份未生產剩餘:700
        public string 當前進度 = "沒有資料";
        public int total_days = 0;
        public int to_today = 0;
        public string 本月剩餘 = "0";
        public string 生產進度 = "0";
        public string 日均產量 = "0";
        public int 日均產量_實際生產 = 0;
        public int 日均產量_預定生產 = 0;
        public int 實際生產_data_y_max = 0;
        public int 預定生產_data_y_max = 0;
        public string th = "";
        public string tr = "";
        public string titel = "";
        public string path = "";
        string 未生產剩餘 = "";
        string date_str = "";
        string date_end = "";
        string title = "";
        string condi = "";
        string line_condi = "";
        string URL_NAME = "";
        string acc = "";
        string add_text = "";
        string line_condi_where = "";
        string condi_where = "";
        myclass myclass = new myclass();
        德大機械 德大機械 = new 德大機械();
        clsDB_Server clsDB_sw = new clsDB_Server("");
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                path = 德大機械.get_title_web_path("SLS");
                URL_NAME = "waitingfortheproduction";
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                if (/*myclass.user_view_check(URL_NAME, acc) == true*/acc !=null)
                {
                    if (!IsPostBack)
                    {
                        string dt1 = DateTime.Now.ToString("ss.fff");
                        date_str = 德大機械.德大專用月份().Split(',')[0];
                        date_end = 德大機械.德大專用月份().Split(',')[1];
                        iniCbx();
                        GotoCenn();
                        //啟動效能報告--------------------------------------------------------------------------------------------
                        double end_time = DataTableUtils.toDouble(DateTime.Now.ToString("ss.fff"))-DataTableUtils.toDouble(dt1);
                        Response.Write("<script language='javascript'>alert('伺服器回應 : finish -> " + end_time + "');</script>");
                        //runtime = 1.6/s ------> 0.0/s
                    }
                }
                else
                {
                    Response.Write("<script>alert('您無法瀏覽此頁面 請向該單位主管申請權限!');location.href='../index.aspx';</script>");
                    //Response.Redirect(myclass.logout_url);
                }
            }
            else
            {
                Response.Redirect(myclass.logout_url);
            }
        }
        private void GotoCenn()
        {
            clsDB_sw.dbOpen(myclass.GetConnByDetaSowon);
            if (clsDB_sw.IsConnected == true)
            {

                get_condition();
                Response.BufferOutput = false;
                set_col_dataseries_實際生產內容();
                set_col_dataseries_預定生產();
                set_table_title();

                設定圖形參數();
                當前進度_set();
                本月剩餘_set();
                生產進度_set();
            }
            else
            {
                Response.Write("<script language='javascript'>alert('伺服器回應 : 無法載入資料，" + clsDB_sw.ErrorMessage + " 請聯絡德科人員或檢查您的網路連線。');</script>");
                無資料處理();
            }
        }
        private void iniCbx()
        {
            clsDB_sw.dbOpen(myclass.GetConnByDekVisErp);
            string sql_cmd = 德大機械.業務部_未生產分析.取得德大產線群組();
            DataTable dt = clsDB_sw.DataTable_GetTable(sql_cmd);
            ListItem listItem = new ListItem("全部", "");
            DropDownList_LINE.Items.Add(listItem);
            foreach (DataRow row in dt.Rows)
            {
                string LINE = "";
                sql_cmd = 德大機械.業務部_未生產分析.取得德大產線群組_單一(DataTableUtils.toString(row["GROUP_NAME"]));
                DataTable dr = clsDB_sw.DataTable_GetTable(sql_cmd);
                for (int i = 0; i < dr.Rows.Count; i++)
                {
                    LINE += DataTableUtils.toString(dr.Rows[i]["LINE_ID"]) + ",";
                }
                LINE = LINE.Substring(0, LINE.Length - 1);
                listItem = new ListItem(DataTableUtils.toString(row["GROUP_NAME"]), LINE);
                DropDownList_LINE.Items.Add(listItem);
            }
            dt.Dispose();
            dt.Clear();
        }
        protected void 無資料處理()
        {
            th = "<th class='center'>沒有資料載入</th>";
            tr = "<tr class='even gradeX'> <td class='center'> no data </ td ></ tr >";

        }
        protected void set_col_dataseries_實際生產內容()
        {
            clsDB_sw.dbOpen(myclass.GetConnByDekdekVisAssm);
            string sqlcmd = 德大機械.業務部_未生產分析.實際生產內容(date_str, date_end, line_condi);
            DataTable dt_實際生產表 = clsDB_sw.DataTable_GetTable(sqlcmd);
            string label_text = "";
            string y_val = "";
            for (int i = 0; i < dt_實際生產表.Rows.Count; i++)
            {
                string 下架日 = dt_實際生產表.Rows[i]["下架日"].ToString();
                add_text += 下架日 + ",";
                label_text = 下架日.Substring(6, 2);
                y_val = dt_實際生產表.Rows[i]["實際生產數量"].ToString();

                實際生產_data_y_max += DataTableUtils.toInt(y_val);
                實際生產_data += "{label:'" + label_text + "號',y:" + y_val + " },";
            }
            if (DataTableUtils.toDouble(實際生產_data_y_max) > 0 || DataTableUtils.toDouble(dt_實際生產表.Rows.Count) > 0)
            {
                日均產量_實際生產 = (int)Math.Ceiling((DataTableUtils.toDouble(實際生產_data_y_max) / DataTableUtils.toDouble(dt_實際生產表.Rows.Count)));
            }
            else
            {
                日均產量_實際生產 = 0;
            }


            dt_實際生產表.Dispose();

            clsDB_sw.dbClose();
            clsDB_sw.dbOpen(myclass.GetConnByDetaSowon);
        }
        protected void set_col_dataseries_預定生產()
        {
            string sqlcmd = 德大機械.業務部_未生產分析.預定生產內容(condi);///////////////////////////
            DataTable dt_預定生產表 = clsDB_sw.GetDataTable(sqlcmd);
            string label_text = "";
            string y_val = "";
            for (int i = 0; i < dt_預定生產表.Rows.Count; i++)
            {
                label_text = dt_預定生產表.Rows[i]["日期"].ToString().Substring(6, 2);
                y_val = dt_預定生產表.Rows[i]["預定生產數量"].ToString();

                預定生產_data += "{label:'" + label_text + "號',y:" + y_val + " },";
                預定生產_data_y_max += DataTableUtils.toInt(y_val);
            }
            日均產量_預定生產 = (int)Math.Ceiling((DataTableUtils.toDouble(預定生產_data_y_max) / DataTableUtils.toDouble(dt_預定生產表.Rows.Count)));

            dt_預定生產表.Dispose();
        }
        protected void set_table_title()
        {
            //titel
            string sqlcmd = 德大機械.業務部_未生產分析.未生產列表欄位(condi);
            DataTable dt = myclass.Add_LINE_GROUP(clsDB_sw.DataTable_GetTable(sqlcmd)).ToTable();
            clsDB_sw.dbOpen(myclass.GetConnByDetaSowon);
            string col_name = "";

            th = "<th>客戶/產線</th>\n                                            ";
            foreach (DataRow row in dt.Rows)
            {
                if (row["產線群組"] != null)
                {
                    if (col_name != DataTableUtils.toString(row["產線群組"]))
                    {
                        col_name = DataTableUtils.toString(row["產線群組"]);
                        title += col_name + ",";
                        th += "<th>" + col_name + "</th>\n                                            ";
                    }
                }
            }

            th += "<th>小計</th>\n  ";
            dt.Dispose();
        }
        protected string set_table_content()
        {
            string dt1 = DateTime.Now.ToString("ss.fff");
            clsDB_sw.dbOpen(myclass.GetConnByDetaSowon);
            //content            
            if (clsDB_sw.IsConnected == true && myclass.user_view_check(URL_NAME, acc) == true)//此判斷為權限檢查用
            {
                string strCmd = 德大機械.業務部_未生產分析.未生產列表內容_客戶欄(condi);
                string cust_sname = "";
                DataTable dr = clsDB_sw.GetDataTable(strCmd);
                for (int i = 0; i < dr.Rows.Count; i++)
                {
                    cust_sname = DataTableUtils.toString(dr.Rows[i]["客戶簡稱"]);
                    tr += "<tr>\n";
                    tr += "<td><u><a href='waitingfortheproduction_details.aspx?cust_name=" + cust_sname + "' target='_blank'>" + cust_sname + "</u></td>\n";

                    strCmd = 德大機械.業務部_未生產分析.未生產列表內容_客戶未生產詳細(cust_sname, condi);
                    DataTable dw = myclass.Add_LINE_GROUP(clsDB_sw.DataTable_GetTable(strCmd)).ToTable();

                    int CUST_TOTAL = 0;
                    for (int j = 0; j < title.Split(',').Length - 1; j++)
                    {
                        int LINE_TOTAL = 0;
                        string 產線群組 = DataTableUtils.toString(title.Split(',')[j]);

                        for (int k = 0; k < dw.Rows.Count; k++)
                        {
                            if (產線群組 == DataTableUtils.toString(dw.Rows[k]["產線群組"]))
                            {
                                LINE_TOTAL += DataTableUtils.toInt(DataTableUtils.toString(dw.Rows[k]["小計"]));
                            }
                        }
                        CUST_TOTAL += LINE_TOTAL;
                        tr += "<td>" + LINE_TOTAL + "</td>\n";
                    }
                    tr += "<td><u><a href='waitingfortheproduction_details.aspx?cust_name=" + cust_sname + ",date_str=" + date_str + ",date_end=" + date_end + "' target='_blank'>" + CUST_TOTAL + " </u></td>\n";
                    tr += "</tr>\n";
                    Response.Write(tr);
                    Response.Flush();
                    Response.Clear();
                    tr = "";
                    dw.Dispose();
                }
                dr.Dispose();
            }
            //啟動效能報告--------------------------------------------------------------------------------------------
            double end_time = DataTableUtils.toDouble(DateTime.Now.ToString("ss.fff")) - DataTableUtils.toDouble(dt1);
            Response.Write("<script language='javascript'>alert('伺服器回應 : finish -> " + end_time + "');</script>");
            //runtime = 1.6/s ------> 0.0/s
            return "";
        }
        protected void 設定圖形參數()
        {
            未生產剩餘 = DataTableUtils.toString(預定生產_data_y_max - 實際生產_data_y_max);
            if ((預定生產_data_y_max - 實際生產_data_y_max) < 0) 未生產剩餘 = "0";
            titel_text = "'" + date_str + "~" + date_end + " 未生產剩餘 : " + 未生產剩餘 + " '";
        }
        protected void 當前進度_set()
        {
            DataTableUtils.Conn_String = myclass.GetConnByDetaSowon;
            GlobalVar.Open();
            string sqlcmd = 德大機械.業務部_未生產分析.預計生產量_至今(date_str, DateTime.Today.ToString("yyyyMMdd"), condi_where);
            DataRow row = DataTableUtils.DataTable_GetDataRow(sqlcmd);
            int 預計生產量_至今 = DataTableUtils.toInt(DataTableUtils.toString(row["至今累積量"]) );

            if (實際生產_data_y_max < 預計生產量_至今)
            {
                當前進度 = "落後";
            }
            else if (實際生產_data_y_max == 預計生產量_至今)
            {
                當前進度 = "標準";
            }
            else if (實際生產_data_y_max > 預計生產量_至今)
            {
                當前進度 = "超前";
            }

            if (未生產剩餘 == "0")
            {
                當前進度 = "達標";
            }
        }
        protected void 本月剩餘_set()
        {
            int cunt = 1;
            int ans = 0;
            int precen = 0;
            string[] text_days = add_text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            total_days = 預定生產_data.Split('}').Length - 1;

            foreach (string temp_day in text_days)
            {
                DateTime dt2 = DateTime.ParseExact(temp_day, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                if (DateTime.Compare(DateTime.Today, dt2) > 0)
                {
                    cunt++;
                }
            }

            to_today = cunt;
            ans = total_days - to_today;
            precen = (int)((DataTableUtils.toDouble(ans) / DataTableUtils.toDouble(total_days)) * 100);
            本月剩餘 = precen + "%";
        }
        protected void 生產進度_set()
        {
            int precen = (int)(DataTableUtils.toDouble(實際生產_data_y_max) / DataTableUtils.toDouble(預定生產_data_y_max) * 100);
            生產進度 = precen + "%";
        }
        protected void get_condition()
        {
            condi = " where A22_FAB.STR_DATE >=" + date_str + " and A22_FAB.STR_DATE <= " + date_end + " ";
            if (DataTableUtils.toString(DropDownList_LINE.SelectedValue) != "")
            {
                string line_id = DataTableUtils.toString(DropDownList_LINE.SelectedValue);
                string or = "or";
                line_condi_where = "";
                condi_where = "";

                for (int i = 0; i < line_id.Split(',').Length; i++)
                {
                    string line_num = DataTableUtils.toString(line_id.Split(',')[i]);

                    if (i != line_id.Split(',').Length - 1)
                    {
                        condi_where += "A22_FAB.FAB_USER = '" + line_num + "' " + or + " ";
                        line_condi_where += "工作站狀態資料表.工作站編號 = '" + line_num + "' " + or + " ";
                    }
                    else
                    {
                        condi_where += "A22_FAB.FAB_USER = '" + line_num + "'";
                        line_condi_where += "工作站狀態資料表.工作站編號 = ' " + line_num + "'";
                    }
                }
                condi += "and(" + condi_where + ")";
                line_condi += "and(" + line_condi_where + ")";
            }
        }
        private void get_search_time(string btnID)
        {
            switch (btnID)
            {
                case "day":
                    date_str = get_datetime(0);
                    date_end = get_datetime(0);
                    break;
                case "week":
                    date_str = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + 1).ToString("yyyyMMdd");
                    date_end = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + 5).ToString("yyyyMMdd");
                    break;
                case "month":
                    date_str = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyyMMdd");
                    date_end = new DateTime(DateTime.Now.AddMonths(1).Year, DateTime.Now.AddMonths(1).Month, 1).AddDays(-1).ToString("yyyyMMdd");
                    break;
                case "firsthalf":
                    date_str = DateTime.Now.ToString("yyyy0101");
                    date_end = DateTime.Now.ToString("yyyy0630");
                    break;
                case "lasthalf":
                    date_str = DateTime.Now.ToString("yyyy0701");
                    date_end = DateTime.Now.ToString("yyyy1231");
                    break;
                case "year":
                    date_str = DateTime.Now.ToString("yyyy0101");
                    date_end = DateTime.Now.ToString("yyyy1231");
                    break;
                case "select":
                    string st_m = DataTableUtils.toString(txt_time_str.Value);
                    string ed_m = DataTableUtils.toString(txt_time_end.Value);
                    DateTime dt_st = DateTime.ParseExact(st_m, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime dt_ed = DateTime.ParseExact(ed_m, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                    if (DaysBetween(dt_st, dt_ed) > 730)
                    {
                        Response.Write("<script language='javascript'>alert('伺服器回應 : 日期起始不得大於 730 天 !');</script>");
                    }
                    else
                    {
                        date_str = dt_st.ToString("yyyyMMdd");
                        date_end = dt_ed.ToString("yyyyMMdd");
                    }
                    break;
            }
        }
        private string get_datetime(int num)
        {
            return DateTime.Now.AddDays(-num).ToString("yyyyMMdd");
        }
        private int DaysBetween(DateTime d1, DateTime d2)
        {
            TimeSpan span = d2.Subtract(d1);
            return (int)span.TotalDays;
        }
        protected void button_select_Click(object sender, EventArgs e)
        {
            get_search_time(DataTableUtils.toString(((Control)sender).ID.Split('_')[1]));
            GotoCenn();
            txt_time_str.Value = "";
            txt_time_end.Value = "";
        }
    }
}