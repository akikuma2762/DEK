using dek_erpvis_v2.cls;
using Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace dek_erpvis_v2.pages.dp_CNC
{
    public partial class Production_History_details_new : System.Web.UI.Page
    {
        public string color = "";
        public string machine = "";
        public string url_text = "";
        public string product = "";
        public string start = "";
        public string end = "";
        public string title_text = "";
        public string tr = "";
        public string th = "";
        string acc = "";
        public int dt_count = 0;
        //  List<string> Session_List = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                color = HtmlUtil.change_color(acc);
                if (acc != "")
                {
                    if (!IsPostBack)
                        MainProcess();
                    //  HtmlUtil.Insert_StatusList(acc, "Production_History");
                }
                else
                    Response.Redirect("Production_History.aspx", false);
            }
            else
                Response.Redirect("Production_History.aspx", false);
        }
        private void MainProcess()
        {
            GetCondi();
            Set_Html_Table();
        }
        private void GetCondi()
        {
            Response.Buffer = false;
            if (Request.QueryString["key"] != null)
            {
                Dictionary<string, string> keyValues = HtmlUtil.Return_dictionary(Request.QueryString["key"]);
                machine = HtmlUtil.Search_Dictionary(keyValues, "machine");
                product = HtmlUtil.Search_Dictionary(keyValues, "product");
                start = HtmlUtil.Search_Dictionary(keyValues, "date_str");
                end = HtmlUtil.Search_Dictionary(keyValues, "date_end");

            }
            else
                Response.Redirect("Production_History.aspx", false);
        }
        private void Set_Html_Table()
        {
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            string sqlcmd = $"SELECT  machine_info.mach_show_name AS 機台名稱, IFNULL(CONCAT(product_name, '-', craft_name), SUBSTRING_INDEX(program_history_info.main_prog, '/', - 1)) AS 工藝名稱, program_history_info.update_time AS 開始時間, program_history_info.enddate_time AS 結束時間, program_history_info.timespan AS 加工時間 FROM program_history_info,craft_info,machine_info WHERE update_time >= '{start}000000' AND enddate_time <= '{end}235959' AND machine_info.mach_show_name = '{machine}' AND program_history_info.main_progflg = 'true' AND IFNULL(CONCAT(product_name, '-', craft_name), SUBSTRING_INDEX(program_history_info.main_prog, '/', - 1)) = '{product}' and machine_info.mach_name = program_history_info.mach_name and program_history_info.mach_name = craft_info.mach_name AND SUBSTRING_INDEX(program_history_info.main_prog, '/', - 1) = craft_info.program";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
            string title = "";
            if (HtmlUtil.Check_DataTable(dt))
            {
                th = HtmlUtil.Set_Table_Title(dt, out title);
                tr = HtmlUtil.Set_Table_Content(dt, title, Production_HistoryDetail_callback);
                dt_count = 2;
            }
            else
            {
                dt_count = 0;
                HtmlUtil.NoData(out th, out tr);
            }

        }
        private string Production_HistoryDetail_callback(DataRow row, string field_name)
        {

            string value = "";
            if (field_name == "開始時間" || field_name == "結束時間")
                value = Convert.ToDateTime(DateTime.ParseExact(DataTableUtils.toString(row[field_name]), "yyyyMMddHHmmss.f", System.Globalization.CultureInfo.CurrentCulture)).ToString("yyyy/MM/dd HH:mm:ss");
            else if (field_name == "加工時間")
            {
                int seconds = DataTableUtils.toInt(DataTableUtils.toString(row[field_name]).Split('.')[0]);
                var timespan = TimeSpan.FromSeconds(seconds);
                string day = "";
                if (timespan.ToString("%d") != "0")
                    day = timespan.ToString("%d") + " 天  ";
                value = day + timespan.ToString(@"hh\:mm\:ss");
            }
            else if (field_name == "產品名稱")
                value = CNCUtils.change_productname(DataTableUtils.toString(row[field_name]));

            if (value == "")
                return value;
            else
                return $"<td>{value}</td>\n";
        }
    }
}
