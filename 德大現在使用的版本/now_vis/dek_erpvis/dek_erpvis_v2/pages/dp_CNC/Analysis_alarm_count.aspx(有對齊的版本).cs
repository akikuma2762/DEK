using System;
using Support;
using dek_erpvis_v2.cls;
using System.Collections.Generic;
using System.Web.UI;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;

namespace dek_erpvis_v2.pages.dp_CNC
{
    public partial class Analysis_alarm_count : System.Web.UI.Page
    {
        public string color = "";
        public string timerange = "";
        public DateTime FirstDay = new DateTime();
        public DateTime LastDay = new DateTime();
        public List<string> dev_list = null;
        public string Type = "week";
        string text = "";
        public string TimeUnit = "分鐘";
        public string dev_name = "";
        public string[] Top_Count = { "0", "0", "0" };
        public string[] Top_Time = { "0", "0", "0" };
        string[] followcount = null;
        public string error_AlarmInfo_th = "";
        public string error_count_tr = "";
        public string error_time_tr = "";

        public string AlarmInfo_Str = "";   //圓餅圖資料來源 
        public string Chart_AlarmInfo = ""; //組圓餅圖資料  
        public string Chart_Count = "";     //Count組圓餅圖資料(存放使用)
        public string Chart_Time = "";      //Time組圓餅圖資料(存放使用)
        string URL_NAME = "", acc = "";
        public bool b_Page_Load = true;

        public List<string> ls_data = new List<string>();
        public bool is_ok = false;
        public string s_data = null;
        public DataTable dt_data = null;

        myclass myclass = new myclass();
        CNCUtils cNC_Class = new CNCUtils();

        //porcess
        protected void Page_Load(object sender, EventArgs e)
        {
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            FirstDay = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);  //單位：周
            LastDay = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + 7).AddSeconds(-1);
            if (LastDay > DateTime.Now) LastDay = DateTime.Now;

            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                //效能測試用
                DateTime start = DateTime.Now;
                string endtime = "1990/01/01 上午 00:00:00";
                DateTime end = DateTime.Parse(endtime);
                string url = System.IO.Path.GetFileName(Request.PhysicalPath).Split('.')[0];
                HtmlUtil.Time_Look(acc, url, start, end, Request.ServerVariables["HTTP_USER_AGENT"]);

                URL_NAME = "Analysis_alarm_count";
                color = HtmlUtil.change_color(acc);
                if (myclass.user_view_check(URL_NAME, acc) == true)
                {
                    if (!IsPostBack)
                    {
                        //DateTime S01 = DateTime.Now;
                        set_page_content();

                        //DateTime E01 = DateTime.Now;
                        //TimeSpan Y = E01 - S01;
                    }
                }
                else
                {
                    Response.Write("<script>alert('您無法瀏覽此頁面 請向該單位主管申請權限!');location.href='../index.aspx';</script>");
                }

                //效能測試
                end = DateTime.Now;
                HtmlUtil.Time_Look(acc, url, start, end, Request.ServerVariables["HTTP_USER_AGENT"]);
            }
            else
            {
                Response.Redirect(myclass.logout_url);
            }

        }
        private void set_page_content()
        {
            Read_Data();
            Get_MachType_List();
        }

        //fuction        
        public void Read_Data(string machgroup = "", string Button_Name = "")
        {
            if (Button_Name != "")
                Type = Button_Name;
            DateTime S01 = DateTime.Now;
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            List<string> ST_AlarmInfo = new List<string>();//取所有可能異警資訊
            ls_data.Clear();
            if (b_Page_Load || DropDownList_MachType.Text == "--Select--")
            {
                DataRow SQL_row = DataTableUtils.DataTable_GetRowHeader("mach_type_group").NewRow();
                SQL_row["read_status"] = "False";
                is_ok = DataTableUtils.Update_DataRow("mach_type_group", "read_status = 'True'", SQL_row);
                foreach (DataRow row in DataTableUtils.GetDataTable("select mach_name from machine_info").Rows)
                    ls_data.Add(row.ItemArray[0].ToString());
            }
            else
            {
                if (machgroup != "不存在")
                    ls_data = DataTableUtils.GetDataTable("select mach_name from mach_group where group_name = '" + machgroup + "'").Rows[0].ItemArray[0].ToString().Split(',').ToList();
                else
                    ls_data = null;
            }

            if (ls_data != null && ls_data.Count != 0)
            {
                string date_s = DataTableUtils.toString(FirstDay).Replace('-', '/');
                string date_e = DataTableUtils.toString(LastDay).Replace('-', '/');
                timerange = date_s + " ~ " + date_e;

                string sqlcmd = "select * from alarm_history_info where update_time > '" + FirstDay.ToString("yyyyMMddHHmmss") + "' union select * from alarm_currently_info  where  update_time > '" + FirstDay.ToString("yyyyMMddHHmmss") + "'";
                dt_data = DataTableUtils.GetDataTable(sqlcmd);
                DateTime updatetime;
                DateTime endtime;
                string uptime = "", edtime = ""; ;

                if (dt_data != null && dt_data.Rows.Count != 0)
                {
                    foreach (DataRow row in dt_data.Rows)
                    {
                        if (ls_data.IndexOf(DataTableUtils.toString(row["mach_name"])) > 0)
                        {
                            updatetime = DateTime.ParseExact(DataTableUtils.toString(row["update_time"]), "yyyyMMddHHmmss.f", System.Globalization.CultureInfo.CurrentCulture);
                            uptime = updatetime.ToString("yyyy-MM-dd tt hh:mm:ss", System.Globalization.CultureInfo.CurrentCulture);

                            //updatetime = DateTime.Parse(DataTableUtils.toString(row["update_time"]).Split('.')[0]);

                            if (DataTableUtils.toString(row["enddate_time"]) == "null")
                                endtime = DateTime.Now;
                            else
                                endtime = DateTime.ParseExact(DataTableUtils.toString(row["enddate_time"]), "yyyyMMddHHmmss.f", System.Globalization.CultureInfo.CurrentCulture);

                            edtime = endtime.ToString("yyyy-MM-dd tt hh:mm:ss", System.Globalization.CultureInfo.CurrentCulture);

                            ST_AlarmInfo.Add(DataTableUtils.toString(row["mach_name"]) + "," +
                                DataTableUtils.toString(row["alarm_type"]) + "," +
                                DataTableUtils.toString(row["alarm_num"]) + "," +
                                DataTableUtils.toString(row["alarm_mesg"]) + "," +
                                updatetime + "," +
                               edtime + "," +
                                DataTableUtils.toString(row["timespan"]));
                        }

                    }
                }
                string[] AlarmInfo = ST_AlarmInfo.ToArray();//List<string>轉string[]            
                string[] AlarmMesg = new string[AlarmInfo.Count()];
                string[] AlarmMesg_Time = new string[AlarmInfo.Count()];
                for (int iIndex = 0; iIndex < AlarmInfo.Length; iIndex++)   //取異警內容
                {
                    AlarmMesg[iIndex] = AlarmInfo[iIndex].Split(',')[3];
                    AlarmMesg_Time[iIndex] = AlarmInfo[iIndex].Split(',')[3] + "," + AlarmInfo[iIndex].Split(',')[6];
                }
                string[] Alarm_Data;



                Alarm_Data = Get_Alarm_Count(AlarmMesg);             //異警次數(top3)        
                Chart_Count = getChartData_Count(combine_str("count", Alarm_Data));
                Alarm_Data = Get_Alarm_Time(AlarmMesg, AlarmMesg_Time);   //異警時間(top3)
                Chart_Time = getChartData_Count(combine_str("time", Alarm_Data, TimeUnit));

                Alarm_Table();                   //慢
            }



        }
        //比較慢
        //private void test()
        //{
        //    string sqlcmd = "select a.mach_name, " +
        //                    "a.alarm_num, " +
        //                    "a.alarm_type, " +
        //                    "a.alarm_mesg, " +
        //                    "a.update_time, " +
        //                    "a.enddate_time, " +
        //                    "a.timespan, " +
        //                    "ai.work_staff " +
        //                    "from " +
        //                    "(select * from alarm_history_info where update_time > '" + FirstDay.ToString("yyyyMMddHHmmss") + "' " +
        //                    "union " +
        //                    "select * from alarm_currently_info  where  update_time > '" + FirstDay.ToString("yyyyMMddHHmmss") + "') as a " +
        //                    "inner join aps_info as ai  on  a.mach_name = ai.mach_name";
        //    dt_data = DataTableUtils.GetDataTable(sqlcmd);
        //    DateTime updatetime, endtime;
        //    if (dt_data != null && dt_data.Rows.Count != 0)
        //    {
        //        foreach (DataRow row in dt_data.Rows)
        //        {
        //            updatetime = DateTime.ParseExact(DataTableUtils.toString(row["update_time"]), "yyyyMMddHHmmss.f", System.Globalization.CultureInfo.CurrentCulture);
        //            if (DataTableUtils.toString(row["enddate_time"]) == "null") endtime = DateTime.Now;
        //            else endtime = DateTime.ParseExact(DataTableUtils.toString(row["enddate_time"]), "yyyyMMddHHmmss.f", System.Globalization.CultureInfo.CurrentCulture);

        //            //   string workstaff = DataTableUtils.toString(DataTableUtils.DataTable_GetDataRow("select work_staff from aps_info where mach_name = '" + DataTableUtils.toString(row["mach_name"]) + "' and '" + DataTableUtils.toString(updatetime) + "' >= start_time and end_time <= '" + DataTableUtils.toString(endtime) + "'")[0]);

        //            TimeSpan alarm_continue_time = TimeSpan.FromSeconds(DataTableUtils.toDouble(DataTableUtils.toString(row["timespan"])));
        //            error_count_tr += "<tr>";
        //            error_count_tr += "<td style=\"width:15%\">" + CNCUtils.MachName_translation(DataTableUtils.toString(row["mach_name"])) + "</td>";
        //            error_count_tr += "<td style=\"width:35%\">" + DataTableUtils.toString(row["alarm_type"]) + "-" + DataTableUtils.toString(row["alarm_num"]) + "-" + DataTableUtils.toString(row["alarm_mesg"]) + "</td>";
        //            error_count_tr += "<td style=\"width:20%\">" + Convert.ToDateTime(updatetime).ToString("yyyy/MM/dd HH:mm:ss") + "</td>";
        //            error_count_tr += "<td style=\"width:15%\">" + alarm_continue_time.Hours + ":" + alarm_continue_time.Minutes + ":" + alarm_continue_time.Seconds + "</td>\n";
        //            error_count_tr += "<td style=\"width:15%\">" + DataTableUtils.toString(row["work_staff"]) + "</td>";
        //            error_count_tr += "</tr>";
        //        }
        //    }
        //    error_AlarmInfo_th = "<tr style='background-color:dimgray;color: white;'></tr>\n";
        //    error_AlarmInfo_th += "<th>設備名稱</th>\n<th>異常訊息</th>\n<th>起始時間</th>\n<th>持續時間</th>\n<th>作業人員</th>";
        //}

        private string combine_str(string type, string[] value, string timetype = "")
        {   //目前捨棄秒數
            for (int i = 0; i < value.Length; i++)
            {
                double a = 0;
                if (i == 0)
                    text = "";
                string[] com = value[i].Split(',');
                if (type == "time")
                {
                    if (com[1] != "")
                    {
                        a = double.Parse(com[1]);
                        if (timetype == "分鐘")
                            a /= 60.0;
                        else if (timetype == "(小時)")
                            a /= 3600.0;
                    }
                    else
                    {
                        com[0] = "未輸入原因";

                    }
                    com[1] = Math.Round(a, 2).ToString();
                }
                if (type == "count")
                {
                    if (com[0] == "")
                        com[0] = "未輸入原因";
                }
                text += com[0] + "^ " + com[1] + ",";//20200131edit
            }
            return "Type^" + type + "," + text;//20200131edit
        }

        public string[] Get_Alarm_Count(string[] AlarmInfo)
        {
            AlarmInfo_Str = "";
            var result = from s in AlarmInfo group s by s;          //統計不重複字串及該字串出現次數
            string[] AlarmMesgCount = new string[result.Count()];   //內容+次數
            int[] i_AlarmCount = new int[result.Count()];           //次數排序     
            int iIndex = 0;
            foreach (var s in result)
            {
                AlarmMesgCount[iIndex] = s.Key + "," + s.Count().ToString();
                i_AlarmCount[iIndex] = s.Count();
                iIndex++;
            }
            return Get_Alarm_Data(i_AlarmCount, AlarmMesgCount, "count");
        }
        public string[] Get_Alarm_Time(string[] AlarmMesg, string[] AlarmMesg_Time)
        {
            AlarmInfo_Str = "";
            var result = from s in AlarmMesg group s by s;      //統計不重複字串及該字串出現次數
            string[] AlarmMesgCount = new string[result.Count()];
            int iIndex = 0;
            foreach (var s in result)
            {
                AlarmMesgCount[iIndex] = s.Key;
                iIndex++;
            }
            int[] i_AlarmTime = new int[result.Count()];
            string[] AlarmTimeData = new string[AlarmMesgCount.Count()];//串接異常訊息&總發生時間

            for (int iIndex_1 = 0; iIndex_1 < AlarmMesgCount.Count(); iIndex_1++)
            {
                double d_AlarmTime = 0;//紀錄第n個異常訊息總發生時間
                for (int iIndex_2 = 0; iIndex_2 < AlarmMesg_Time.Count(); iIndex_2++)
                {
                    if (AlarmMesgCount[iIndex_1] == AlarmMesg_Time[iIndex_2].Split(',')[0])
                        d_AlarmTime += DataTableUtils.toDouble(AlarmMesg_Time[iIndex_2].Split(',')[1]);
                }
                AlarmTimeData[iIndex_1] = AlarmMesgCount[iIndex_1] + "," + (int)d_AlarmTime;
                i_AlarmTime[iIndex_1] = (int)d_AlarmTime;
            }
            return Get_Alarm_Time_Data(followcount, AlarmTimeData, "time");
        }
        public string[] Get_Alarm_Data(int[] i_Alarm_Data, string[] s_AlarmMesg, string Cal_Type)
        {
            Array.Sort(i_Alarm_Data);
            Array.Reverse(i_Alarm_Data);

            //內容順序排序,取前3(用次數多寡比對)
            int Top_count = 0, Top_alarm_timecount = 0;
            followcount = new string[i_Alarm_Data.Count()];
            string[] Top_result = new string[i_Alarm_Data.Count()];//次數排序
            for (int iIndex_1 = 0; iIndex_1 < i_Alarm_Data.Count(); iIndex_1++)
            {
                for (int iIndex_2 = 0; iIndex_2 < s_AlarmMesg.Count(); iIndex_2++)
                {
                    if (i_Alarm_Data[iIndex_1].ToString() == s_AlarmMesg[iIndex_2].Split(',')[1])
                    {
                        Top_result[Top_count] = s_AlarmMesg[iIndex_2];//排序
                        s_AlarmMesg[iIndex_2] = ",";//避免出現相同次數，造成重複
                        Top_alarm_timecount += i_Alarm_Data[iIndex_1];//累計總次數
                        Top_count++;
                        break;
                    }
                }
                if (Top_count >= i_Alarm_Data.Count()) break;
            }
            for (int i = 0; i < Top_result.Length; i++)
            {
                string a = Top_result[i].Split(',')[0];
                followcount[i] = a;
            }
            return Top_result;
        }
        //讓時間的標籤跟隨次數
        public string[] Get_Alarm_Time_Data(string[] i_Alarm_Data, string[] s_AlarmMesg, string Cal_Type)
        {
            int Top_count = 0;
            followcount = new string[i_Alarm_Data.Count()];
            string[] Top_result = new string[i_Alarm_Data.Count()];
            for (int iIndex_1 = 0; iIndex_1 < i_Alarm_Data.Count(); iIndex_1++)
            {
                for (int iIndex_2 = 0; iIndex_2 < s_AlarmMesg.Count(); iIndex_2++)
                {
                    if (i_Alarm_Data[iIndex_1].ToString() == s_AlarmMesg[iIndex_2].Split(',')[0])
                    {
                        Top_result[Top_count] = s_AlarmMesg[iIndex_2];
                        s_AlarmMesg[iIndex_2] = ",";
                        Top_count++;
                        break;
                    }
                }
                if (Top_count >= i_Alarm_Data.Count()) break;
            }
            return Top_result;
        }

        private void Alarm_Table()//表格資料
        {
            string sqlcmd = "select * from alarm_history_info where update_time > '" + FirstDay.ToString("yyyyMMddHHmmss") + "' union select * from alarm_currently_info  where  update_time > '" + FirstDay.ToString("yyyyMMddHHmmss") + "'";

            dt_data = DataTableUtils.GetDataTable(sqlcmd);
            DateTime updatetime, endtime;
            if (dt_data != null && dt_data.Rows.Count != 0)
            {
                foreach (DataRow row in dt_data.Rows)
                {
                    if (ls_data.IndexOf(DataTableUtils.toString(row["mach_name"])) > 0)
                    {
                        updatetime = DateTime.ParseExact(DataTableUtils.toString(row["update_time"]), "yyyyMMddHHmmss.f", System.Globalization.CultureInfo.CurrentCulture);
                        if (DataTableUtils.toString(row["enddate_time"]) == "null") endtime = DateTime.Now;
                        else endtime = DateTime.ParseExact(DataTableUtils.toString(row["enddate_time"]), "yyyyMMddHHmmss.f", System.Globalization.CultureInfo.CurrentCulture);
                        string sql = "select work_staff from aps_info where mach_name = '" + DataTableUtils.toString(row["mach_name"]) + "' and (" + DateTime.Parse(DataTableUtils.toString(updatetime)).ToString("yyyyMMddHHmmss") + "<= start_time OR end_time >= " + DateTime.Parse(DataTableUtils.toString(endtime)).ToString("yyyyMMddHHmmss") + ")";
                        DataRow rew = DataTableUtils.DataTable_GetDataRow(sql);
                        string workstaff = DataTableUtils.toString(rew[0]);

                        TimeSpan alarm_continue_time = TimeSpan.FromSeconds(DataTableUtils.toDouble(DataTableUtils.toString(row["timespan"])));
                        error_count_tr += "<tr>";
                        error_count_tr += "<td style=\"width:15%\">" + CNCUtils.MachName_translation(DataTableUtils.toString(row["mach_name"])) + "</td>";
                        error_count_tr += "<td style=\"width:35%\">" + DataTableUtils.toString(row["alarm_type"]) + "-" + DataTableUtils.toString(row["alarm_num"]) + "-" + DataTableUtils.toString(row["alarm_mesg"]) + "</td>";
                        error_count_tr += "<td style=\"width:20%\">" + Convert.ToDateTime(updatetime).ToString("yyyy/MM/dd HH:mm:ss") + "</td>";
                        error_count_tr += "<td style=\"width:15%\">" + alarm_continue_time.Hours + ":" + alarm_continue_time.Minutes + ":" + alarm_continue_time.Seconds + "</td>\n";
                        error_count_tr += "<td style=\"width:15%\">" + workstaff + "</td>";
                        error_count_tr += "</tr>";
                    }
                }
            }
            //error_AlarmInfo_th = "<tr style='background-color:dimgray;color: white;'></tr>\n";
            error_AlarmInfo_th = "<th>設備名稱</th>\n<th>異常訊息</th>\n<th>起始時間</th>\n<th>持續時間</th>\n<th>作業人員</th>";
        }


        public string getChartData_Count(string dev_status)
        {
            if (dev_status.Substring(dev_status.Length - 1, 1) == ",")//判斷最後是否是","
                dev_status = dev_status.Substring(0, dev_status.Length - 1);
            return get_dataponts(dev_status);
        }
        private string get_dataponts(string dev_status)
        {
            Chart_AlarmInfo = "";
            for (int iIndex = 1; iIndex < dev_status.Split(',').Length; iIndex++)
                Chart_AlarmInfo += "{ y:" + dev_status.Split(',')[iIndex].Split('^')[1] + " , label: '" + dev_status.Split('^')[iIndex].Split(',')[1].Replace('\n', ' ') + "' },";//20200131edit
            return Chart_AlarmInfo;
        }
        private void Get_MachType_List()//取type list
        {
            DropDownList_MachType.Items.Clear();
            dt_data = DataTableUtils.GetDataTable("select type_name from mach_type");
            if (dt_data != null && dt_data.Rows.Count != 0)
            {
                DropDownList_MachType.Items.Add("--Select--");
                for (int iIndex = 0; iIndex < dt_data.Rows.Count; iIndex++)
                    DropDownList_MachType.Items.Add(dt_data.Rows[iIndex]["type_name"].ToString());
            }
        }

        //event
        //這裡
        protected void Select_MachGroupClick(object sender, EventArgs e)    //執行運算
        {
            if (DropDownList_MachType.SelectedItem.Text != "--Select--")// && DropDownList_MachGroup.SelectedItem.Text != "--Select--")
            {
                b_Page_Load = false;
                string sqlcmd = "";
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                sqlcmd = "select * from mach_type where type_name = '" + DropDownList_MachType.SelectedItem.Text + "' ";
                //if (DropDownList_MachGroup.SelectedItem.Text != "--Select--")
                //    sqlcmd = sqlcmd + " and group_name = '" + DropDownList_MachGroup.SelectedItem.Text + "' ";
                DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
                if (dt.Rows.Count > 0)
                {
                    if (DropDownList_MachGroup.SelectedItem.Text == "--Select--")
                    {
                        string Group = DataTableUtils.toString(dt.Rows[0]["group_name"]);
                        if (Group != "")
                        {
                            string[] Machine_Group = Group.Split(',');
                            for (int i = 0; i < Machine_Group.Length; i++)
                                Read_Data(DataTableUtils.toString(Machine_Group[i]));
                        }
                        else
                            Read_Data("不存在");
             
                    }
                    else
                        Read_Data(DropDownList_MachGroup.SelectedItem.Text);
                }
            }
            else
                Response.Redirect("Analysis_alarm_count.aspx");
        }
        //這裡
        protected void button_select_Click(object sender, EventArgs e)  //時間篩選
        {
            List<string> ST_First_Last_Time = cNC_Class.get_search_time(DataTableUtils.toString(((Control)sender).ID.Split('_')[1]), "", "");
            FirstDay = DateTime.ParseExact(ST_First_Last_Time[0].Split(',')[0], "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
            LastDay = DateTime.ParseExact(ST_First_Last_Time[0].Split(',')[1], "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
            if (LastDay > DateTime.Now) LastDay = DateTime.Now;
            Type = ST_First_Last_Time[1];
            if (DropDownList_MachType.SelectedItem.Text != "--Select--")// && DropDownList_MachGroup.SelectedItem.Text != "--Select--")
            {
                b_Page_Load = false;
                string sqlcmd = "";
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                sqlcmd = "select * from mach_type where type_name = '" + DropDownList_MachType.SelectedItem.Text + "' ";
                //if (DropDownList_MachGroup.SelectedItem.Text != "--Select--")
                //    sqlcmd = sqlcmd + " and group_name = '" + DropDownList_MachGroup.SelectedItem.Text + "' ";
                DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
                if (dt.Rows.Count > 0)
                {
                    if (DropDownList_MachGroup.SelectedItem.Text == "--Select--")
                    {
                        string Group = DataTableUtils.toString(dt.Rows[0]["group_name"]);
                        if (Group != "")
                        {
                            string[] Machine_Group = Group.Split(',');
                            for (int i = 0; i < Machine_Group.Length; i++)
                                Read_Data(DataTableUtils.toString(Machine_Group[i]));
                        }
                        else
                            Read_Data("不存在");
                    }
                    else
                        Read_Data(DropDownList_MachGroup.SelectedItem.Text);
                }
            }
            else
                Read_Data("", Type);
        }
        protected void DropDownList_MachType_SelectedIndexChanged(object sender, EventArgs e)//cbx select事件//取group list
        {
            if (DropDownList_MachType.SelectedItem.Text != "--Select--")
            {
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                ls_data.Clear();
                DropDownList_MachGroup.Items.Clear();
                ls_data = DataTableUtils.GetDataTable("select group_name from mach_type where type_name = '" + DropDownList_MachType.SelectedItem.Text + "'").Rows[0].ItemArray[0].ToString().Split(',').ToList();
                DropDownList_MachGroup.Items.Add("--Select--");
                for (int iIndex = 0; iIndex < ls_data.Count; iIndex++)
                    DropDownList_MachGroup.Items.Add(ls_data[iIndex]);
            }
            else
            {
                DropDownList_MachGroup.Items.Clear();
                DropDownList_MachGroup.Items.Add(" ");
            }
        }
        protected void button_wrench_Click1(object sender, EventArgs e)
        {
            if (RadioButtonList_select_type.SelectedValue == "0")
            {
                TimeUnit = "(分鐘)";
            }
            else if (RadioButtonList_select_type.SelectedValue == "1")
            {
                TimeUnit = "(小時)";
            }
            set_page_content();
        }
    }
}