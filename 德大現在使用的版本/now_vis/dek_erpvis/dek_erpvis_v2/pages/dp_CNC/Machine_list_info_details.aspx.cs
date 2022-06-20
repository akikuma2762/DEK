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
    public partial class Machine_list_info_details : System.Web.UI.Page
    {
        public string color = "";
        public string date_str = "";//new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyyMMdd");
        public string date_end = ""; //new DateTime(DateTime.Now.AddMonths(1).Year, DateTime.Now.AddMonths(1).Month, 1).AddDays(-1).ToString("yyyyMMdd");
        public string th = "";
        public string tr = "";
        public string data_name = "";
        public string title_text = "";
        public string path = "";
        string acc = "";
        public string total_worktime = "";
        public string status_th = "";
        public string status_tr = "";
        public string timerange = "";
        public string machine = "";
        public string area = "";
        public string str_First_Day = "";
        public string str_Last_Day = "";
        public string str_Dev_Name = "";
        public string js_code = "";
        public string condition;
        public string pie_data_points = "";
        public int dt_count = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                color = HtmlUtil.change_color(acc);
                if (TextBox_date.Text == "")
                    TextBox_date.Text = DateTime.Now.ToString("yyyy-MM-dd");
                Set_CheckBoxList();
                Gocenn();
            }
            else
                Response.Redirect(myclass.logout_url);

        }
        //存入資料庫
        protected void Button_submit_Click(object sender, EventArgs e)
        {
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            string sqlcmd = $"SELECT * FROM status_history_info where _id = '{TextBox_ID.Text}'";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);

            if (HtmlUtil.Check_DataTable(dt))
            {
                DataRow row = dt.NewRow();
                row["_id"] = DataTableUtils.toString(dt.Rows[0]["_id"]);
                row["Type"] = DropDownList_Status.SelectedItem.Text;
                //取代字串，避免JS異常
                row["Detail"] = TextBox_content.Text.Replace("'", "^").Replace('"', '#').Replace(" ", "$").Replace("\r\n", "@");
                bool ok = DataTableUtils.Update_DataRow("status_history_info", $"_id = '{TextBox_ID.Text}'", row);
            }
            Gocenn();
        }
        //從資料庫找值後回填至元件內
        protected void Button_Search_Click(object sender, EventArgs e)
        {
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            string sqlcmd = $"select * from status_history_info where _id = '{TextBox_ID.Text}'";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);

            if (HtmlUtil.Check_DataTable(dt))
            {
                DropDownList_Status.SelectedValue = DataTableUtils.toString(dt.Rows[0]["Type"]);
                TextBox_content.Text = DataTableUtils.toString(dt.Rows[0]["Detail"]);
            }
        }
        private void Gocenn()
        {
            Response.Buffer = false;
            machine = "";

            if (Request.QueryString["key"] != null)
            {
                Dictionary<string, string> keyValues = HtmlUtil.Return_dictionary(Request.QueryString["key"]);
                machine = HtmlUtil.Search_Dictionary(keyValues, "machine");
                Set_Dropdownlist();
                Set_bar();
                Set_Html_Table();
                Set_Percent();
                Set_Work();
                machine = CNCUtils.MachName_translation(machine);
            }
            else
                Response.Redirect("Machine_list_info.aspx", false);
        }
        //產生上下午的BAR
        private void Set_bar()
        {
            area = "";
            js_code = "";
            str_Dev_Name = machine;
            str_First_Day = TextBox_date.Text.Replace('-', '/');
            str_Last_Day = DateTime.Parse(str_First_Day).AddDays(1).ToString("yyyy/MM/dd");
            //0-12
            //            js_code += "mTimer_status = setTimeout(function () { draw_Axial('" + machine + "',''); }, 1000);\n";



            area += $" <div id='chart_{machine}' class='col-md-12 col-sm-12 col-xs-12' > " +
                    "       <div class='dashboard_graph x_panel'  style=\" border: 2px solid #1f1f1f \" > " +
                    "           <div class='col-md-12 col-sm-12 col-xs-12' style='padding:0px 0px;'> " +
                    "               <div class='left col-md-12 col-sm-12 col-xs-12' style='font-size:19px'> " +
                    "                   <div class='col-xs-12 col-sm-12'> " +
                    $"                       <div id = 'ma_div_{machine}' class=\"scrollbar\" > " +
                    $"                          <canvas id = 'ma_canvas_{machine}' width = '1200' height = '40'> " +
                    "                           </canvas> " +
                    "                        </div> " +
                    "                   </div> " +
                    "               </div> " +
                    "           </div> " +
                    "       </div> " +
                    " </div> ";

            //12-24
            //js_code += "mTimer_status2 = setTimeout(function () { draw_Axial('" + machine + "','1'); }, 2000);\n";


            area += $" <div id='chart_{machine}1' class='col-md-12 col-sm-12 col-xs-12'   > " +
                    "       <div class='dashboard_graph x_panel'  style=\" border: 2px solid #1f1f1f \" > " +
                    "           <div class='col-md-12 col-sm-12 col-xs-12' style='padding:0px 0px;'> " +
                    "               <div class='left col-md-12 col-sm-12 col-xs-12' style='font-size:19px'> " +
                    "                   <div class='col-xs-12 col-sm-12'> " +
                    $"                       <div id = 'ma_div_{machine}1' class=\"scrollbar\" > " +
                    $"                          <canvas id = 'ma_canvas_{machine}1' width = '1200' height = '40'> " +
                    "                           </canvas> " +
                    "                        </div> " +
                    "                   </div> " +
                    "               </div> " +
                    "           </div> " +
                    "       </div> " +
                    " </div> ";

        }
        //產生DATATABLE
        private void Set_Html_Table()
        {
            condition = get_condition(CheckBoxList_status);
            int time = 0;
            if (TextBox_time.Text != "")
                time = Int16.Parse(TextBox_time.Text) * 60;
            else
                TextBox_time.Text = "0";

            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            string sqlcmd = $"SELECT     " +
                            $"status_change.status_chinese as 狀態,    " +
                            //   $"mach_show_name AS 異警資訊,    " +
                            $"update_time as 開始時間,   " +
                            $"enddate_time as 結束時間,    " +
                            $"  CAST(timespan AS double) as 持續時間,    " +
                            $"Type as 類型,   " +
                            $" Detail as 內容, " +
                            $" status_history_info._id as 新增 " +
                            $"FROM    status_history_info        " +
                            $"LEFT JOIN    machine_info ON machine_info.mach_name = status_history_info.mach_name    " +
                            $"Left join status_change on status_history_info.status = status_change.status_english     " +
                            $"where  CAST(timespan AS signed)  > {time}	" +
                            $"and    status_history_info.mach_name = '{machine}'    " +
                             $"and    substring(update_time,1,8) = {TextBox_date.Text.Replace("-", "")}    " +

                            // $"and    CAST(update_time AS double) >= '{TextBox_date.Text.Replace("-", "")}000000'	" +
                            //$"and    CAST(update_time AS double) <= '{TextBox_date.Text.Replace("-", "")}235959'" +
                            $" {condition} ";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
            ////抓到前一天的最後一筆
            sqlcmd = $"SELECT     " +
                           $"status_change.status_chinese as 狀態,    " +
                           //  $"mach_show_name AS 異警資訊,    " +
                           $"update_time as 開始時間,   " +
                           $"enddate_time as 結束時間,    " +
                           $"  CAST(timespan AS double) as 持續時間,    " +
                           $"Type as 類型,   " +
                           $" Detail as 內容, " +
                           $" status_history_info._id as 新增 " +
                           $"FROM    status_history_info        " +
                           $"LEFT JOIN    machine_info ON machine_info.mach_name = status_history_info.mach_name    " +
                           $"Left join status_change on status_history_info.status = status_change.status_english     " +
                           $"where  CAST(timespan AS signed)  > {time}	" +
                           $"and    status_history_info.mach_name = '{machine}'    " +
                           //    $"and    substring(update_time,1,8) < {TextBox_date.Text}    " +
                           $"and    CAST(update_time AS double) <= '{TextBox_date.Text.Replace("-", "")}000000'	" +
                           $"and    CAST(enddate_time AS double) >= '{TextBox_date.Text.Replace("-", "")}000000'	" +
                           $" {condition} " +
                           $"order by status_history_info._id desc limit 1";
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            DataTable dr = DataTableUtils.GetDataTable(sqlcmd);
            try
            {
                dr.Merge(dt, true, MissingSchemaAction.Ignore);
            }
            catch
            {

            }
            //如果是今天，抓當下的這一筆
            if (TextBox_date.Text.Replace("-", "").Contains(DateTime.Now.ToString("yyyyMMdd")))
            {
                sqlcmd = $"SELECT " +
                         $"status_chinese AS 狀態," +
                         //  $"mach_show_name As 異警資訊," +
                         $"update_time as 開始時間 " +
                         $"FROM    status_currently_info        " +
                         $"LEFT JOIN    status_change ON status_change.status_english = status_currently_info.status    " +
                         $"left join machine_info    on status_currently_info.mach_name = machine_info.mach_name    " +
                         $" where status_currently_info.mach_name = '{machine}' " + condition;
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                DataTable dt_now = DataTableUtils.GetDataTable(sqlcmd);
                try
                {
                    dr.Merge(dt_now, true, MissingSchemaAction.Ignore);
                }
                catch
                {

                }
            }
            condition = WebUtils.UrlStringEncode(condition + $" and CAST(timespan AS signed)  > {time} ");
            string title = "";
            if (HtmlUtil.Check_DataTable(dr))
            {
                dt_count = 1;
                th = HtmlUtil.Set_Table_Title(dr, out title);
                tr = HtmlUtil.Set_Table_Content(dr, title, Machine_list_info_details_callback);
            }
            else
            {
                HtmlUtil.NoData(out th, out tr);
                dt_count = 0;
            }


        }
        //欄位的處理
        private string Machine_list_info_details_callback(DataRow row, string field_name)
        {
            string value = "";
            if (field_name == "開始時間" || field_name == "結束時間")
            {
                try
                {
                    value = Convert.ToDateTime(DateTime.ParseExact(DataTableUtils.toString(row[field_name]), "yyyyMMddHHmmss.f", System.Globalization.CultureInfo.CurrentCulture)).ToString("yyyy/MM/dd HH:mm:ss");
                }
                catch
                {
                    value = "  ";
                }
            }
            else if (field_name == "持續時間")
            {
                if (DataTableUtils.toString(row[field_name]) != "")
                    value = calculation_time(DataTableUtils.toString(row[field_name]).Split('.')[0]);
                else
                    value = "  ";
            }
            else if (field_name == "新增")
            {
                if (DataTableUtils.toString(row[field_name]) != "" && DataTableUtils.toString(row["內容"]) == "")
                    value = $"<b><u><a href=\"javascript: void()\" id={DataTableUtils.toString(row[field_name])} onclick=set_id('{DataTableUtils.toString(row[field_name])}','{DataTableUtils.toString(row["類型"])}','{DataTableUtils.toString(row["內容"])}','{Convert.ToDateTime(DateTime.ParseExact(DataTableUtils.toString(row["開始時間"]), "yyyyMMddHHmmss.f", System.Globalization.CultureInfo.CurrentCulture)).ToString("yyyy/MM/dd HH:mm:ss").Replace(" ", "*")}') data-toggle = \"modal\" data-target = \"#exampleModal\">新增</a></u></b>";
                else if (DataTableUtils.toString(row[field_name]) != "" && DataTableUtils.toString(row["內容"]) != "")
                    value = $"<b><u><a href=\"javascript: void()\" id={DataTableUtils.toString(row[field_name])} onclick=set_id('{DataTableUtils.toString(row[field_name])}','{DataTableUtils.toString(row["類型"])}','{DataTableUtils.toString(row["內容"])}','{Convert.ToDateTime(DateTime.ParseExact(DataTableUtils.toString(row["開始時間"]), "yyyyMMddHHmmss.f", System.Globalization.CultureInfo.CurrentCulture)).ToString("yyyy/MM/dd HH:mm:ss").Replace(" ", "*")}') data-toggle = \"modal\" data-target = \"#exampleModal\">編輯</a></u></b>";
                else
                    value = "    ";
            }
            else if (field_name == "內容")
            {
                List<string> list = new List<string>((DataTableUtils.toString(row[field_name]).Replace("$", " ").Replace('#', '"').Replace("^", "'").Replace("@", "\r\n")).Split('\n'));
                for (int i = 0; i < list.Count; i++)
                    value += list[i] + " <br> ";
            }
            else if (field_name == "異警資訊")
            {
                if (DataTableUtils.toString(row["狀態"]) == "警報")
                {
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                    string sqlcmd = $"SELECT     alarm_type," +
                                    $"alarm_num," +
                                    $"alarm_mesg " +
                                    $"FROM     status_history_info " +
                                    $"left join alarm_history_info " +
                                    $"on alarm_history_info.mach_name  =  status_history_info.mach_name  " +
                                    $"and alarm_history_info.update_time = status_history_info.update_time " +
                                    $"and alarm_history_info.timespan = status_history_info.timespan " +
                                    $"LEFT JOIN    machine_info ON machine_info.mach_name = status_history_info.mach_name   " +
                                    $"where (status_history_info.status = 'EMERGENCY' OR status_history_info.status = 'ALARM') " +
                                    $"and  mach_show_name = '{DataTableUtils.toString(row["異警資訊"])}' " +
                                    $"and alarm_history_info.update_time = '{DataTableUtils.toString(row["開始時間"])}' " +
                                    $"and alarm_history_info.timespan = '{DataTableUtils.toString(row["持續時間"])}' " +
                                    $"order by status_history_info._id desc limit 1 ";
                    DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
                    if (HtmlUtil.Check_DataTable(dt))
                        value = DataTableUtils.toString(dt.Rows[0]["alarm_type"]) + "-" + DataTableUtils.toString(dt.Rows[0]["alarm_num"]) + "-" + DataTableUtils.toString(dt.Rows[0]["alarm_mesg"]);
                    else
                        value = "   ";
                }
                else
                    value = "   ";
            }

            if (value == "")
                return value;
            else
                return "<td>" + value + "</td>\n";
        }
        //設定選取的個數
        private string get_condition(CheckBoxList checkBoxList)
        {
            string condition = "";

            for (int i = 0; i < checkBoxList.Items.Count; i++)
            {
                if (checkBoxList.Items[i].Selected)
                {
                    if (condition == "")
                        condition += $" and ( status_chinese ='{checkBoxList.Items[i].Text}' ";
                    else
                        condition += $" OR  status_chinese ='{checkBoxList.Items[i].Text}' ";
                }
            }
            if (condition == "")
                return "";
            else
                return condition + ")";
        }
        //搜尋
        protected void button_select_Click(object sender, EventArgs e)
        {
            Gocenn();
        }
        //在Dropdownlist設置ITEM
        private void Set_Dropdownlist()
        {
            if (DropDownList_Status.Items.Count == 0)
            {
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                string sqlcmd = "SELECT * FROM status_type";
                DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
                ListItem listItem = new ListItem();
                listItem = new ListItem("", "");
                DropDownList_Status.Items.Add(listItem);
                foreach (DataRow row in dt.Rows)
                {
                    listItem = new ListItem(DataTableUtils.toString(row["type"]), DataTableUtils.toString(row["type"]));
                    DropDownList_Status.Items.Add(listItem);
                }
            }
        }
        //產生運轉 警報 待機  離線的%->圓餅圖
        private void Set_Percent()
        {
            pie_data_points = "";
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            string sqlcmd = $"select * from status_realtime_info where mach_name='{machine}' and work_date='{TextBox_date.Text.Replace("-", "")}'";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);

            if (HtmlUtil.Check_DataTable(dt))
            {

                pie_data_points += "{y:" + DataTableUtils.toInt(DataTableUtils.toString(dt.Rows[0]["operate_rate_now"])) + ", name:'運轉' , label:'運轉',color:'#00b400'}," +
                                   "{y:" + DataTableUtils.toInt(DataTableUtils.toString(dt.Rows[0]["disc_rate_now"])) + ", name:'離線' , label:'離線',color:'#a9a9a9'}," +
                                   "{y:" + DataTableUtils.toInt(DataTableUtils.toString(dt.Rows[0]["alarm_rate_now"])) + ", name:'警報' , label:'警報',color:'#ff0000'}," +
                                   "{y:" + DataTableUtils.toInt(DataTableUtils.toString(dt.Rows[0]["emergency_rate_now"])) + ", name:'警告' , label:'警告',color:'#ff00ff'}," +
                                   "{y:" + DataTableUtils.toInt(DataTableUtils.toString(dt.Rows[0]["suspend_rate_now"])) + ", name:'暫停' , label:'暫停',color:'#ffff00'}," +
                                   "{y:" + DataTableUtils.toInt(DataTableUtils.toString(dt.Rows[0]["idle_rate_now"])) + ", name:'待機' , label:'待機',color:'#ff9b32'}," +
                                   "{y:" + DataTableUtils.toInt(DataTableUtils.toString(dt.Rows[0]["manual_rate_now"])) + ", name:'手動' , label:'手動',color:'#02cdfc'}," +
                                   "{y:" + DataTableUtils.toInt(DataTableUtils.toString(dt.Rows[0]["warmup_rate_now"])) + ", name:'暖機' , label:'暖機',color:'#b22222'},";
            }
        }
        //顯示總工時 總離線 總待機 總警告 總運轉
        private void Set_Work()
        {
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            string sqlcmd = $"select * from status_realtime_info where mach_name = '{machine}' and work_date = '{TextBox_date.Text.Replace("-", "")}'";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);

            if (HtmlUtil.Check_DataTable(dt))
            {
                total_worktime =
                                    " <div class=\"col-md-12 col-sm-12 col-xs-12\" > " +
                                        " <div class=\"dashboard_graph x_panel\"> " +
                                            " <div class=\"x_content\"> " +
                                             " <table id=\"TB\" class=\"table table-ts table-bordered nowrap\" cellspacing=\"0\" width=\"100%\"> " +
                                                " <thead style='display:none'> " +
                                                    " <tr id=\"tr_row\"> " +
                                                        "<th>狀態</th><th >時間</th>" +
                                                    " </tr> " +
                                                " </thead> " +
                                                " <tbody> " +
                                                    " <tr> " +
                                                        " <td align=\"center\"  style='font-Size:25px;color:darkred'> " +
                                                            "<b> 綜合工時 </b>" +
                                                        " </td> " +
                                                        " <td align=\"center\"  style='font-Size:25px;color:darkred'> " +
                                                        $"<b> { calculation_time(DataTableUtils.toString(dt.Rows[0]["work_time"]))} </b> " +
                                                        " </td> " +
                                                    " </tr> " +

                                                    " <tr> " +
                                                    " <td align=\"center\"  style='font-Size:20px;'> " +
                                                    "<b>  運轉時間 </b> " +
                                                    " </td> " +
                                                    " <td align=\"center\"  style='font-Size:20px;'> " +
                                                    $"<b>  { calculation_time(DataTableUtils.toString(dt.Rows[0]["operate_time"]))} </b> " +
                                                    " </td> " +
                                                    " </tr> " +
                                                    " <tr> " +

                                                    " <tr> " +
                                                    " <td align=\"center\"  style='font-Size:20px;'> " +
                                                    "<b>  離線時間 </b> " +
                                                    " </td> " +
                                                    " <td align=\"center\"  style='font-Size:20px;'> " +
                                                    $"<b>  { calculation_time(DataTableUtils.toString(dt.Rows[0]["disc_time"]))} </b> " +
                                                    " </td> " +
                                                    " </tr> " +
                                                                                     " <tr> " +
                                                    " <td align=\"center\"  style='font-Size:20px;'> " +
                                                    "<b>  警報時間 </b> " +
                                                    " </td> " +
                                                    " <td align=\"center\"  style='font-Size:20px;'> " +
                                                    $"<b>  { calculation_time(DataTableUtils.toString(dt.Rows[0]["emergency_time"]))} </b> " +
                                                    " </td> " +
                                                    " </tr> " +

                                                    " <tr> " +
                                                    " <td align=\"center\"  style='font-Size:20px;'> " +
                                                    "<b>  警告時間 </b> " +
                                                    " </td> " +
                                                    " <td align=\"center\"  style='font-Size:20px;'> " +
                                                    $"<b>  { calculation_time(DataTableUtils.toString(dt.Rows[0]["alarm_time"]))} </b> " +
                                                    " </td> " +
                                                    " </tr> " +

                                                    " <tr> " +
                                                    " <td align=\"center\"  style='font-Size:20px;'> " +
                                                    "<b>  暫停時間 </b> " +
                                                    " </td> " +
                                                    " <td align=\"center\"  style='font-Size:20px;'> " +
                                                    $"<b>  { calculation_time(DataTableUtils.toString(dt.Rows[0]["suspend_time"]))} </b> " +
                                                    " </td> " +
                                                    " </tr> " +


                                                    " <tr> " +
                                                    " <td align=\"center\"  style='font-Size:20px;'> " +
                                                    "<b>  待機時間 </b> " +
                                                    " </td> " +
                                                    " <td align=\"center\"  style='font-Size:20px;'> " +
                                                    $"<b>  { calculation_time(DataTableUtils.toString(dt.Rows[0]["idle_time"]))} </b> " +
                                                    " </td> " +
                                                    " </tr> " +


                                                    " <td align=\"center\"  style='font-Size:20px;'> " +
                                                    "<b>  手動時間 </b> " +
                                                    " </td> " +
                                                    " <td align=\"center\"  style='font-Size:20px;'> " +
                                                    $"<b>  { calculation_time(DataTableUtils.toString(dt.Rows[0]["manual_time"]))} </b> " +
                                                    " </td> " +
                                                    " </tr> " +

                                                    " <tr> " +
                                                    " <td align=\"center\"  style='font-Size:20px;'> " +
                                                    "<b>  暖機時間 </b> " +
                                                    " </td> " +
                                                    " <td align=\"center\"  style='font-Size:20px;'> " +
                                                    $"<b>  { calculation_time(DataTableUtils.toString(dt.Rows[0]["warmup_time"]))} </b> " +
                                                    " </td> " +
                                                    " </tr> " +



                                                " </tbody> " +
                                             " </table> " +
                                            " </div> " +
                                        " </div> " +
                                    " </div> ";
            }

        }
        private void Set_CheckBoxList()
        {
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            string sqlcmd = "select * from status_change";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);

            if (HtmlUtil.Check_DataTable(dt) && CheckBoxList_status.Items.Count == 0)
            {
                ListItem listItem = new ListItem();
                foreach (DataRow row in dt.Rows)
                {
                    listItem = new ListItem(DataTableUtils.toString(row["status_chinese"]), DataTableUtils.toString(row["status_chinese"]));
                    CheckBoxList_status.Items.Add(listItem);
                }
            }
        }
        //返回計算的時間
        private string calculation_time(string second)
        {
            if (second != "")
            {
                var timespan = TimeSpan.FromSeconds(DataTableUtils.toDouble(second));
                string day = "";
                if (timespan.ToString("%d") != "0")
                    day = timespan.ToString("%d") + " 天  ";
                return day + timespan.ToString(@"hh\:mm\:ss");
            }
            else

                return "00:00:00";
        }
    }
}