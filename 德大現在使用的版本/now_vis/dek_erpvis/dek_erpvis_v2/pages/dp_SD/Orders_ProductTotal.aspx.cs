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
    public partial class Orders_ProductTotal : System.Web.UI.Page
    {

        //-------------------------------------------------參數 OR 引用------------------------------------------------------------
        public string color = "";
        public string dt_str = "";
        public string dt_end = "";
        public string path = "";
        public string title = "";
        public string order = "";
        public StringBuilder th = new StringBuilder();
        public StringBuilder tr = new StringBuilder();
        public StringBuilder Energy = new StringBuilder();
        string acc = "";
        myclass myclass = new myclass();
        ERP_Sales SLS = new ERP_Sales();
        DataTable dt_monthtotal = new DataTable();
        List<string> Line_Name = new List<string>();
        List<string> avoid_again = new List<string>();
        List<int> month_count = new List<int>();
        bool ok = true;
        clsDB_Server clsdb = new clsDB_Server(myclass.GetConnByDekVisErp);


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
                if (myclass.user_view_check("Orders_ProductTotal", acc))
                {
                    if (!IsPostBack)
                    {
                        string[] date = 德大機械.德大專用月份(acc).Split(',');

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
            if (dropdownlist_model.SelectedItem.Value == "day")
            {
                Energy = Set_DayEnergy(dropdownlist_Factory.SelectedItem.Value);
                title = "每日生產統計";
                order = "0";
            }
            else
            {
                Energy = Set_MonthEnergy(dropdownlist_Factory.SelectedItem.Value);
                title = "每月生產統計";
                order = "1";
            }
                
        }

        //取得未交易數
        private void Get_MonthTotal()
        {
            dt_monthtotal = SLS.Orders_StartDay(txt_str.Text.Replace("-", ""), txt_end.Text.Replace("-", ""), dropdownlist_Factory.SelectedItem.Value);
            if (HtmlUtil.Check_DataTable(dt_monthtotal))
            {
                dt_monthtotal = myclass.Add_LINE_GROUP(dt_monthtotal).ToTable();
                dt_monthtotal.Columns["產線群組"].ColumnName = "產線";
            }
        }

        //設定表格
        private void Set_Table()
        {
            DataTable dt = dt_monthtotal;
            //日產能
            if (HtmlUtil.Check_DataTable(dt) && dropdownlist_model.SelectedItem.Value == "day")
            {

                DataTable day = dt.DefaultView.ToTable(true, new string[] { "預計開工日" });
                DataTable Line = dt.DefaultView.ToTable(true, new string[] { "產線" });

                //新增產線
                foreach (DataRow row in Line.Rows)
                {
                    day.Columns.Add(DataTableUtils.toString(row["產線"]));
                    Line_Name.Add(DataTableUtils.toString(row["產線"]));
                }
                //新增小計
                day.Columns.Add("小計");

                string titlename = "";
                th.Append(HtmlUtil.Set_Table_Title(day, out titlename));
                tr.Append(HtmlUtil.Set_Table_Content(day, titlename, Orders_DayTotal_callback));
            }
            //月產能
            else if (HtmlUtil.Check_DataTable(dt) && dropdownlist_model.SelectedItem.Value == "month")
            {
                DataTable custom = dt.DefaultView.ToTable(true, new string[] { "客戶簡稱" });
                DataTable month = dt.DefaultView.ToTable(true, new string[] { "計算月份" });

                custom.Rows.Add("總計");

                //新增產線
                foreach (DataRow row in month.Rows)
                {
                    custom.Columns.Add(DataTableUtils.toString(row["計算月份"]));
                    Line_Name.Add(DataTableUtils.toString(row["計算月份"]));
                }
                //新增小計
                custom.Columns.Add("小計");

                string titlename = "";
                th.Append(HtmlUtil.Set_Table_Title(custom, out titlename));
                tr.Append(HtmlUtil.Set_Table_Content(custom, titlename, Orders_CustomTotal_callback));
            }
            else
                HtmlUtil.NoData(out th, out tr);
        }

        //例外處理(每日)
        private string Orders_DayTotal_callback(DataRow row, string field_name)
        {
            string value = "";
            if (field_name == "預計開工日")
                value = HtmlUtil.changetimeformat(row[field_name].ToString());
            else if (Line_Name.IndexOf(field_name) != -1)
            {
                string url = $"Orders_ProductTotal_Details.aspx?key={WebUtils.UrlStringEncode($"date_str={txt_str.Text.Replace("-", "")},date_end={txt_end.Text.Replace("-", "")},workday={row["預計開工日"]},Line={field_name},type={dropdownlist_Factory.SelectedItem.Value}")}";

                DataRow[] rows = dt_monthtotal.Select($"預計開工日='{row["預計開工日"]}' and 產線='{field_name}'");
                if (rows.Length > 0)
                    value = $"<u><a href=\"{url}\"> {rows.Length} </a></u>";
            }
            else if (field_name == "小計")
            {
                string url = $"Orders_ProductTotal_Details.aspx?key={WebUtils.UrlStringEncode($"date_str={txt_str.Text.Replace("-", "")},date_end={txt_end.Text.Replace("-", "")},workday={row["預計開工日"]},Line=所有,type={dropdownlist_Factory.SelectedItem.Value}")}";

                DataRow[] rows = dt_monthtotal.Select($"預計開工日='{row["預計開工日"]}' ");
                value = $"<u><a href=\"{url}\"> {rows.Length} </a></u>";
            }

            return $"<td >{value}</td>";
        }

        //例外處理(每月)
        private string Orders_CustomTotal_callback(DataRow row, string field_name)
        {
            string value = "";
            if (avoid_again.IndexOf(DataTableUtils.toString(row["客戶簡稱"]).Trim()) == -1)
            {
                if (Line_Name.IndexOf(field_name) != -1)
                {
                    if (row["客戶簡稱"].ToString() != "總計")
                    {
                        DataRow[] rows = dt_monthtotal.Select($"客戶簡稱='{row["客戶簡稱"]}' and 計算月份='{field_name}'");
                        if (rows.Length > 0)
                        {
                            string url = $"Orders_ProductTotal_Details.aspx?key={WebUtils.UrlStringEncode($"date_str={txt_str.Text.Replace("-", "")},date_end={txt_end.Text.Replace("-", "")},custom={row["客戶簡稱"]},Date={field_name},add=0,type={dropdownlist_Factory.SelectedItem.Value}")}";
                            value = $"<u><a href=\"{url}\"> {rows.Length} </a></u>";
                        }

                        if (ok)
                            month_count.Add(rows.Length);
                        else
                            month_count[Line_Name.IndexOf(field_name)] = month_count[Line_Name.IndexOf(field_name)] + rows.Length;
                    }
                    else
                    {
                        string url = $"Orders_ProductTotal_Details.aspx?key={WebUtils.UrlStringEncode($"date_str={txt_str.Text.Replace("-", "")},date_end={txt_end.Text.Replace("-", "")},custom={""},Date={field_name},add=0,type={dropdownlist_Factory.SelectedItem.Value}")}";
                        value = $"<u><a href=\"{url}\"> { month_count[Line_Name.IndexOf(field_name)]} </a></u>";
                    }
                }
                else if (field_name == "小計")
                {
                    if (row["客戶簡稱"].ToString() != "總計")
                    {
                        DataRow[] rows = dt_monthtotal.Select($"客戶簡稱='{row["客戶簡稱"]}' ");
                        string url = $"Orders_ProductTotal_Details.aspx?key={WebUtils.UrlStringEncode($"date_str={txt_str.Text.Replace("-", "")},date_end={txt_end.Text.Replace("-", "")},custom={row["客戶簡稱"]},Date={""},add=0,type={dropdownlist_Factory.SelectedItem.Value}")}";
                        value = $"<u><a href=\"{url}\"> {rows.Length} </a></u>";


                        avoid_again.Add(DataTableUtils.toString(row["客戶簡稱"]).Trim());
                        ok = false;
                    }
                    else
                    {
                        int total = 0;
                        for (int i = 0; i < month_count.Count; i++)
                            total += month_count[i];

                        string url = $"Orders_ProductTotal_Details.aspx?key={WebUtils.UrlStringEncode($"date_str={txt_str.Text.Replace("-", "")},date_end={txt_end.Text.Replace("-", "")},custom={""},Date={""},add=0,type={dropdownlist_Factory.SelectedItem.Value}")}";
                        value = $"<u><a href=\"{url}\"> {total} </a></u>";
                    }
                }
                return value == "" ? "" : $"<td >{value}</td>";
            }
            else
                return "1";
        }

        //設定每日最大產能
        public StringBuilder Set_DayEnergy(string value)
        {
            string sqlcmd = "";
            StringBuilder result = new StringBuilder();

            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisErp);
            switch (value)
            {
                case "sowon":
                    sqlcmd = "SELECT 產線名稱,sum(目標件數) 量能 FROM (SELECT ASSEMBLY_GROUP.GROUP_NAME 產線名稱, ASSEMBLY_LINE_GROUP.LINE_ID FROM ASSEMBLY_GROUP, ASSEMBLY_LINE_GROUP WHERE ASSEMBLY_LINE_GROUP.GROUP_ID = ASSEMBLY_GROUP.GROUP_ID and ISNUMERIC( ASSEMBLY_LINE_GROUP.LINE_ID)=1 and ASSEMBLY_LINE_GROUP.LINE_ID <> '11' ) a LEFT JOIN (SELECT 工作站編號,目標件數 FROM dekvisassm.dbo.工作站型態資料表 WHERE 工作站是否使用中 = '1') b ON cast(a.LINE_ID as int) = cast(b.工作站編號 as int) where 工作站編號 IS NOT NULL group by 產線名稱";
                    break;
                case "dek":
                    sqlcmd = "SELECT 產線名稱,sum(目標件數) 量能 FROM (SELECT ASSEMBLY_GROUP.GROUP_NAME 產線名稱, ASSEMBLY_LINE_GROUP.LINE_ID FROM ASSEMBLY_GROUP, ASSEMBLY_LINE_GROUP WHERE ASSEMBLY_LINE_GROUP.GROUP_ID = ASSEMBLY_GROUP.GROUP_ID and ISNUMERIC( ASSEMBLY_LINE_GROUP.LINE_ID)=1 and ASSEMBLY_LINE_GROUP.LINE_ID = '11' ) a LEFT JOIN (SELECT 工作站編號,目標件數 FROM dekvisassm.dbo.工作站型態資料表 WHERE 工作站是否使用中 = '1') b ON cast(a.LINE_ID as int) = cast(b.工作站編號 as int) where 工作站編號 IS NOT NULL group by 產線名稱";
                    break;
                case "iTec":
                    sqlcmd = "SELECT 產線名稱,sum(目標件數) 量能 FROM (SELECT ASSEMBLY_GROUP.GROUP_NAME 產線名稱, ASSEMBLY_LINE_GROUP.LINE_ID FROM ASSEMBLY_GROUP, ASSEMBLY_LINE_GROUP WHERE ASSEMBLY_LINE_GROUP.GROUP_ID = ASSEMBLY_GROUP.GROUP_ID and ISNUMERIC( ASSEMBLY_LINE_GROUP.LINE_ID)=1 ) a LEFT JOIN (SELECT OriLine 工作站編號,目標件數 FROM detaVisHor.dbo.工作站型態資料表,detaVisHor.dbo.工作站對應資料表 WHERE 工作站是否使用中 = '1' and 工作站對應資料表.TrsLine = 工作站型態資料表.工作站編號 ) b ON cast(a.LINE_ID as int) = cast(b.工作站編號 as int) where 工作站編號 IS NOT NULL group by 產線名稱";
                    break;
            }
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);


            if (HtmlUtil.Check_DataTable(dt))
            {
                result.Append($"<div class=\"col-md-3 col-sm-12 col-xs-12 _select _setborder\"><div class=\"x_panel\"><table id=\"TB\" class=\"table table-ts table-bordered nowrap\" cellspacing=\"0\" width=\"100%\"><thead ><tr id=\"tr_row\"><th style=\"font-size: 20px; text-align:center\">產線</th><th style=\"font-size: 20px; text-align:center\">日產能</th></tr></thead><tbody>");

                foreach (DataRow row in dt.Rows)
                    result.Append($"<tr> <td align=\"center\" style='font-size: 20px; color: black'> <b> {row["產線名稱"]} </b> </td> <td align=\"center\" style='font-size: 20px; color: black'> <b> {row["量能"]} </b> </td> </tr>");

                result.Append($"</tbody> </table> </div> </div>");
            }
            else
                result.Append("");
            return result;
        }

        //設定每月產能
        public StringBuilder Set_MonthEnergy(string value)
        {
            StringBuilder result = new StringBuilder();
            //取得起始日
            DateTime start = HtmlUtil.StrToDate(txt_str.Text.Replace("-", ""));
            //取得結束日
            DateTime end = HtmlUtil.StrToDate(txt_str.Text.Replace("-", "")).AddMonths(1);
            //取得該月工作天
            double time = Get_DifferenceDay(start, end);
            string max = "";
            string sqlcmd = "";
            if (value == "sowon")
                sqlcmd = "SELECT sum(目標件數) 量能 FROM (SELECT ASSEMBLY_GROUP.GROUP_NAME 產線名稱, ASSEMBLY_LINE_GROUP.LINE_ID FROM ASSEMBLY_GROUP, ASSEMBLY_LINE_GROUP WHERE ASSEMBLY_LINE_GROUP.GROUP_ID = ASSEMBLY_GROUP.GROUP_ID and ISNUMERIC( ASSEMBLY_LINE_GROUP.LINE_ID)=1 and ASSEMBLY_LINE_GROUP.LINE_ID <> '11' ) a LEFT JOIN (SELECT 工作站編號,目標件數 FROM dekvisassm.dbo.工作站型態資料表 WHERE 工作站是否使用中 = '1') b ON cast(a.LINE_ID as int) = cast(b.工作站編號 as int) where 工作站編號 IS NOT NULL ";
            else if (value == "dek")
                sqlcmd = "SELECT sum(目標件數) 量能 FROM (SELECT ASSEMBLY_GROUP.GROUP_NAME 產線名稱, ASSEMBLY_LINE_GROUP.LINE_ID FROM ASSEMBLY_GROUP, ASSEMBLY_LINE_GROUP WHERE ASSEMBLY_LINE_GROUP.GROUP_ID = ASSEMBLY_GROUP.GROUP_ID and ISNUMERIC( ASSEMBLY_LINE_GROUP.LINE_ID)=1 and ASSEMBLY_LINE_GROUP.LINE_ID = '11' ) a LEFT JOIN (SELECT 工作站編號,目標件數 FROM dekvisassm.dbo.工作站型態資料表 WHERE 工作站是否使用中 = '1') b ON cast(a.LINE_ID as int) = cast(b.工作站編號 as int) where 工作站編號 IS NOT NULL ";
            else if (value == "iTec")
                sqlcmd = "SELECT sum(目標件數) 量能 FROM (SELECT ASSEMBLY_GROUP.GROUP_NAME 產線名稱, ASSEMBLY_LINE_GROUP.LINE_ID FROM ASSEMBLY_GROUP, ASSEMBLY_LINE_GROUP WHERE ASSEMBLY_LINE_GROUP.GROUP_ID = ASSEMBLY_GROUP.GROUP_ID and ISNUMERIC( ASSEMBLY_LINE_GROUP.LINE_ID)=1 ) a LEFT JOIN (SELECT OriLine 工作站編號,目標件數 FROM detaVisHor.dbo.工作站型態資料表,detaVisHor.dbo.工作站對應資料表 WHERE 工作站是否使用中 = '1' and 工作站對應資料表.TrsLine = 工作站型態資料表.工作站編號 ) b ON cast(a.LINE_ID as int) = cast(b.工作站編號 as int) where 工作站編號 IS NOT NULL ";


            DataTable dt = clsdb.GetDataTable(sqlcmd);
            max = HtmlUtil.Check_DataTable(dt) ? (DataTableUtils.toDouble(dt.Rows[0]["量能"].ToString()) * time).ToString() : "0";

            result.Append($"<div class=\"col-md-3 col-sm-12 col-xs-12 _select _setborder\"><div class=\"x_panel\"><table id=\"TB\" class=\"table table-ts table-bordered nowrap\" cellspacing=\"0\" width=\"100%\"><thead ><tr id=\"tr_row\"><th style=\"font-size: 20px; text-align:center\">廠區</th><th style=\"font-size: 20px; text-align:center\">月產能({end:MM}月)</th></tr></thead><tbody>");
            result.Append($"<tr> <td align=\"center\" style='font-size: 20px; color: black'> <b> {dropdownlist_Factory.SelectedItem.Text} </b> </td> <td align=\"center\" style='font-size: 20px; color: black'> <b> {max} </b> </td> </tr>");
            result.Append($"</tbody> </table> </div> </div>");
            return result;

        }

        //取得兩日期相差天數
        private double Get_DifferenceDay(DateTime start, DateTime end)
        {
            //取得相距天數
            TimeSpan span = end - start;
            double total_day = span.TotalDays;

            //取得假日時段
            string sqlcmd = $"select distinct PK_Holiday from WorkTime_Holiday where {start:yyyyMMdd}<=PK_Holiday and PK_Holiday<={end.AddDays(-1):yyyyMMdd}";
            DataTable dt = clsdb.GetDataTable(sqlcmd);

            if (HtmlUtil.Check_DataTable(dt))
                return total_day - DataTableUtils.toDouble(dt.Rows.Count);
            else
                return total_day;
        }
    }
}