using dek_erpvis_v2.cls;
using Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace dek_erpvis_v2.pages.SYS_CONTROL
{
    public partial class Business_Report : System.Web.UI.Page
    {
        public string date_str = "";
        public string date_end = "";
        public string th = "";
        public string tr = "";
        public string data_name = "";
        public string title_text = "";
        public string path = "";
        string acc = "";
        public string name = "";
        string URL_NAME = "";
        public string part = "";

        DataTable public_dt = null;
        myclass myclass = new myclass();
        clsDB_Server clsDB_sw = new clsDB_Server("");
        德大機械 德大機械 = new 德大機械();
        //一開始載入的內容
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                URL_NAME = "Business_Report";
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                part = DataTableUtils.toString(userInfo["user_DPM"]);
                if (myclass.user_view_check(URL_NAME, acc) == true)
                {
                    if (!IsPostBack)
                    { 
                        date_str = DateTime.Now.AddDays(-30).ToString("yyyyMMdd");
                        date_end = DateTime.Now.ToString("yyyyMMdd");
                        GotoCenn();
                        InitDropDown();
                        InitDropDownType();
                    }
                }
                else
                {
                    Response.Write("<script>alert('您無法瀏覽此頁面 請向該單位主管申請權限!');location.href='../index.aspx';</script>");
                }
            }


            else
            {
                Response.Redirect(myclass.logout_url);
            }
        }
        //確認是否打開伺服器
        private void GotoCenn()
        {
            clsDB_sw.dbOpen(myclass.GetConnByDekVisErp);
            if (clsDB_sw.IsConnected == true)
            {
                load_page_data();
            }
            else
            {
                Response.Write("<script language='javascript'>alert('伺服器回應 : 無法載入資料，" + clsDB_sw.ErrorMessage + " 請聯絡德科人員或檢查您的網路連線。');</script>");
                無資料處理();
            }
        }
        //打開之後載入資料
        private void load_page_data()
        {
            Response.BufferOutput = false;
            set_table_title();
            set_table_content();
        }
        //找到表單的欄位名稱(直接用前端把它隱藏掉了，不然th沒有數值的話，他無法作DOM的查詢動作)
        private void set_table_title()
        {
            string searchword = "";
            string searchpart = "";
            string searchtype = "";
            public_dt = new DataTable();
            clsDB_sw.dbOpen(myclass.GetConnByDekVisErp);
            //做搜尋的條件
            if(textbox_item.Text != "" || DropDownList_Part.Text != "---請選擇---" || DropDownList_selectedcondi.Text != "---請選擇---")
            {
                if(textbox_item.Text != "")//類似的標題
                {
                     searchword = "title like'%" + textbox_item.Text + "%' AND ";
                }
                if(DropDownList_Part.Text != "")//選中的部門
                {
                     searchpart = "part = '" + DropDownList_Part.Text + "' AND ";
                }
                if(DropDownList_selectedcondi.Text != "")//選中的種類
                {
                     searchtype = "type = '" + DropDownList_selectedcondi.Text + "' AND ";
                }
            }
            string sqlcmd = 德大機械.業務報告.報告時間查詢(searchword,searchpart,searchtype, date_str, date_end);//回傳語法
            public_dt = clsDB_sw.DataTable_GetTable(sqlcmd);

            //title
            for (int i = 1; i < public_dt.Columns.Count; i++)
            {
                string col_name = public_dt.Columns[i].ColumnName;
                th += "<th>" + col_name + "</th>\n";
            }
        }
        //找到表單的欄位資料
        public string set_table_content()
        {
            tr = "";
            if (clsDB_sw.IsConnected == true )
            {
                if (public_dt != null)
                {
                    if (public_dt.Rows.Count > 0)
                    {

                        foreach (DataRow row in public_dt.Rows)
                        {
                            //找到ID
                            string item_code_new = DataTableUtils.toString(row["id"]);
                            //把標題設超連結
                            string item_code_ahref = "<u><a href=Business_Report_detail.aspx?id=" + item_code_new + ">" + DataTableUtils.toString(row["標題"]) + "</u></a>";
                            tr += "<tr>\n";
                            tr += "<td align=center width=120><font size=3>" + DataTableUtils.toString(row["所屬部門"]) + "<br>" + DataTableUtils.toString(row["類型"]) + "</font></td>\n";
                            //要設定欄位高度，不然他會無法置中
                            tr += "<td style=line-height:46px><font size=4>" + item_code_ahref + "</font></td>\n";
                            tr += "<td align=center width=200>" + "發佈人<br><font size=3>" + DataTableUtils.toString(row["撰寫人"]) + "</font></td>\n";
                            tr += "<td align=right width=120><font size=3>" + "最後更新日期<br>" + DataTableUtils.toString(row["上傳時間"]) + "</font></td>\n";
                            tr += "<td style=display:none width=0>" + DataTableUtils.toString(row["所屬部門"]) + "</font></td>\n";
                            tr += "</tr>\n";

                        }
                        public_dt.Dispose();
                    }
                    else
                    {
                        無資料處理();
                    }
                    public_dt.Dispose();
                }
                else
                {
                    Response.Redirect(myclass.logout_url);
                }
            }
            return "";
        }
        //沒有資料時，就載入以下資訊
        private void 無資料處理()
        {
            th = "<th class='center'>沒有資料載入</th>";
            tr = "<tr> <td class='center'> no data </ td ></ tr >";
            title_text = "'沒有資料'";
        }
        //查詢精靈那邊，按鈕的觸發事件
        private void get_search_time(string btnID)
        {
            switch (btnID)
            {
                case "month":
                    var s = 德大機械.德大專用月份(acc);
                    date_str = s.Split(',')[0];
                    date_end = s.Split(',')[1];
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
        //進行元件的字串分割，留下"_"之後的名稱
        protected void button_select_Click(object sender, EventArgs e)
        {
            get_search_time(DataTableUtils.toString(((Control)sender).ID.Split('_')[1]));
            GotoCenn();
            if (DataTableUtils.toString(((Control)sender).ID.Split('_')[1]) != "select")
            {
                txt_time_str.Value = "";
                txt_time_end.Value = "";
            }
        }
        //跳到新增頁面
        protected void button_jump_Click(object sender, EventArgs e)
        {
            Response.Redirect("Add_Report.aspx");
        }
        //計算日期差距
        private int DaysBetween(DateTime d1, DateTime d2)
        {
            TimeSpan span = d2.Subtract(d1);
            return (int)span.TotalDays;
        }
        //顯示在查詢精靈的下拉式選單的內容(部門)
        private void InitDropDown()
        {
            clsDB_sw.dbOpen(myclass.GetConnByDekVisErp);
            if (clsDB_sw.IsConnected == true)
            {
                DataTable dt = clsDB_sw.GetDataTable("SELECT distinct DPM_NAME,DPM_NAME2 FROM DEPARTMENT where DPM_NAME2 != '系統'");
                if (dt.Rows.Count > 0)
                {
                    ListItem list = new ListItem("---請選擇---","");
                    DropDownList_Part.Items.Add(list);

                    foreach (DataRow row in dt.Rows)
                    {
                        list = new ListItem(DataTableUtils.toString(row["DPM_NAME2"]), DataTableUtils.toString(row["DPM_NAME"]));
                        DropDownList_Part.Items.Add(list);
                    }
                }
            }
        }
        //顯示在查詢精靈的下拉式選單的內容(類別)
        private void InitDropDownType()
        {
            clsDB_sw.dbOpen(myclass.GetConnByDekVisErp);
            if (clsDB_sw.IsConnected == true)
            {
                DataTable dt = clsDB_sw.GetDataTable("SELECT Report_Type FROM Type_Report");
                if (dt.Rows.Count > 0)
                {
                    ListItem list = new ListItem("---請選擇---", "");
                    DropDownList_selectedcondi.Items.Add(list);

                    foreach (DataRow row in dt.Rows)
                    {
                        list = new ListItem(DataTableUtils.toString(row["Report_Type"]), DataTableUtils.toString(row["Report_Type"]));
                        DropDownList_selectedcondi.Items.Add(list);
                    }
                }
            }
        }
    }
}