using System;
using Support;
using dek_erpvis_v2.cls;
using System.Collections.Generic;
using System.Web.UI;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;
using System.Text;

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
        public string unit = "次數";
        public string dev_name = "";
        public string[] Top_Count = { "0", "0", "0" };
        public string[] Top_Time = { "0", "0", "0" };
        string[] followcount = null;
        public string error_AlarmInfo_th = "";
        public StringBuilder error_count_tr = new StringBuilder();
        public string error_time_tr = "";
        public string error_unit = "次";
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
        List<string> Record_Machine = new List<string>();
        public int dt_count = 0;

        //porcess
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);

                URL_NAME = "Analysis_alarm_count";
                color = HtmlUtil.change_color(acc);
                if (myclass.user_view_check(URL_NAME, acc) == true)
                {
                    if (!IsPostBack)
                    {
                        if (textbox_st.Text == "" && textbox_ed.Text == "")
                        {
                            FirstDay = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);  //單位：周
                            LastDay = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + 7).AddSeconds(-1);
                            if (LastDay > DateTime.Now) LastDay = DateTime.Now;

                            textbox_st.Text = FirstDay.ToString("yyyy-MM-dd");
                            textbox_ed.Text = LastDay.ToString("yyyy-MM-dd");
                        }
                        set_page_content();
                    }
                }
                else
                    Response.Write("<script>alert('您無此權限!');location.href='../index.aspx';</script>");
            }
            else
                Response.Redirect(myclass.logout_url);

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
            string condition = "";
            string factory = CNCUtils.Find_Group(HtmlUtil.Search_acc_Column(acc, "Belong_Factory"));
            if (factory == "")
                condition = " where area_name <> '測試區' ";
            else
                condition = $" where area_name = '{factory}' ";
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            List<string> ST_AlarmInfo = new List<string>();//取所有可能異警資訊
            ls_data.Clear();
            if (b_Page_Load || DropDownList_MachType.Text == "--Select--")
            {
                foreach (DataRow row in DataTableUtils.GetDataTable($"select mach_name from machine_info {condition} order by _id desc").Rows)
                    ls_data.Add(row.ItemArray[0].ToString());
            }
            else
            {
                if (machgroup != "不存在")
                {
                    if (DropDownList_MachType.SelectedItem.Text == "全廠" && (DropDownList_MachGroup.SelectedItem.Text == "全廠設備" || DropDownList_MachGroup.SelectedItem.Text == "--Select--"))
                    {
                        GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                        DataTable ds = DataTableUtils.GetDataTable("select mach_name from machine_info where  area_name <> '測試區'  order by _id desc");

                        if (HtmlUtil.Check_DataTable(ds))
                        {
                            foreach (DataRow dataRow in ds.Rows)
                                ls_data.Add(DataTableUtils.toString(dataRow["mach_name"]));
                        }
                    }
                    else
                        ls_data = DataTableUtils.GetDataTable("select mach_name from mach_group where group_name = '" + machgroup + "' order by _id desc").Rows[0].ItemArray[0].ToString().Split(',').ToList();
                }
                else
                    ls_data = null;

            }
            //第一階段，過濾空值
            if (ls_data != null && ls_data.Count != 0)
            {
                for (int i = 0; i < ls_data.Count; i++)
                {
                    if (DataTableUtils.toString(ls_data[i]) == "")
                    {
                        ls_data.RemoveAt(i);
                        i--;
                    }
                }
            }

            //第二階段，過濾相同
            if (Record_Machine != null && ls_data != null && Record_Machine.Count != 0 && ls_data.Count != 0)
            {
                for (int i = 0; i < Record_Machine.Count; i++)
                {
                    if (ls_data.IndexOf(Record_Machine[i]) != -1)
                        ls_data.RemoveAt(ls_data.IndexOf(Record_Machine[i]));
                }
            }

            if (ls_data != null && ls_data.Count != 0)
            {
                timerange = textbox_st.Text.Replace('-', '/') + "~" + textbox_ed.Text.Replace('-', '/');
                FirstDay = DateTime.Parse(textbox_st.Text.Replace('-', '/') + "上午 00:00:00");
                LastDay = DateTime.Parse(textbox_ed.Text.Replace('-', '/') + "下午 23:59:59");

                string sqlcmd = $"select * from alarm_history_info where ( update_time > '{ FirstDay.ToString("yyyyMMddHHmmss")}' and update_time <= '{LastDay.ToString("yyyyMMddHHmmss")}' ) union select * from alarm_currently_info  where  update_time > '{FirstDay.ToString("yyyyMMddHHmmss")}'";
                dt_data = DataTableUtils.GetDataTable(sqlcmd);
                DateTime updatetime;
                DateTime endtime;
                string uptime = "", edtime = ""; ;

                if (dt_data != null && dt_data.Rows.Count != 0)
                {
                    foreach (DataRow row in dt_data.Rows)
                    {

                        if (ls_data.IndexOf(DataTableUtils.toString(row["mach_name"])) >= 0)
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

                unit = DropDownList_Y.SelectedItem.Text;
                //異常原因
                if (DropDownList_X.SelectedValue == "Error_Season")
                {
                    if (DropDownList_Y.SelectedValue == "count")
                    {
                        //次數(圖)
                        error_unit = "次";
                        Alarm_Data = Get_Alarm_Count(AlarmMesg);
                        Chart_Count = getChartData_Count(combine_str("count", Alarm_Data));
                    }
                    else
                    {
                        error_unit = "分";
                        //時間(圖)
                        Alarm_Data = Get_Alarm_Time(AlarmMesg, AlarmMesg_Time);
                        Chart_Count = getChartData_Count(combine_str("time", Alarm_Data, "分鐘"));
                    }
                }
                //機台名稱
                else
                    Get_Machine_Alarm(dt_data, DropDownList_Y.SelectedValue);

                //(表格)
                Alarm_Table();
            }
        }
        //X軸為機台時，統計次數跟時間
        private void Get_Machine_Alarm(DataTable Error_DataTable, string type)
        {


            if (type == "count")
                error_unit = "次";
            else
                error_unit = "分";
            //有進行排序的
            //先設定一個DataTable，用他進行讀取的動作
            DataTable dt = new DataTable();
            dt.Columns.Add("machine");
            dt.Columns.Add("count", typeof(int));
            dt.Columns.Add("time", typeof(double));

            int count = 0;
            double time = 0;
            int out_number = 0;
            DataTable Machine_name = Error_DataTable.DefaultView.ToTable(true, new string[] { "mach_name" });
            //重整後排序
            foreach (DataRow row in Machine_name.Rows)
            {
                if (ls_data.IndexOf(DataTableUtils.toString(row["mach_name"])) >= 0)
                {
                    count = 0;
                    time = 0;
                    string sqlcmd = "mach_name = '" + DataTableUtils.toString(row["mach_name"]) + "'";
                    DataRow[] rows = Error_DataTable.Select(sqlcmd);
                    for (int i = 0; i < rows.Length; i++)
                    {
                        time += DataTableUtils.toDouble(rows[i]["timespan"]) / 60.0;
                        count++;
                    }
                    dt.Rows.Add(CNCUtils.MachName_translation(DataTableUtils.toString(row["mach_name"])), count, Math.Round(time, 0, MidpointRounding.AwayFromZero));
                }
            }
            DataView dv_mant = new DataView(dt);
            dv_mant.Sort = DropDownList_Y.SelectedValue + " desc";
            dt = dv_mant.ToTable();
            Chart_Count = HtmlUtil.Set_Chart(dt, "machine", DropDownList_Y.SelectedValue, "", out out_number,out _);

        }
        private string combine_str(string type, string[] value, string timetype = "")
        {
            int count = 0;
            if (CheckBox_All.Checked == true)
                count = value.Length;
            else
            {
                count = DataTableUtils.toInt(txt_showCount.Text);
                if (value.Length < count)
                    count = value.Length;
            }

            //目前捨棄秒數
            for (int i = 0; i < count; i++)
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
                    //四捨五入後
                    com[1] = Math.Round(a, 0, MidpointRounding.AwayFromZero).ToString();
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
        //次數用
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
        //時間用
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
            //跟隨大小排序
            return Get_Alarm_Data(i_AlarmTime, AlarmTimeData, "time");
            //跟隨次數排序
            //return Get_Alarm_Time_Data(followcount, AlarmTimeData, "time");
        }
        //次數用
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
            string sqlcmd = "";
            if (DataTableUtils.toInt(DateTime.Now.ToString("yyyyMMdd")) >= DataTableUtils.toInt(FirstDay.ToString("yyyyMMddHHmmss")) && DataTableUtils.toInt(DateTime.Now.ToString("yyyyMMdd")) <= DataTableUtils.toInt(LastDay.ToString("yyyyMMddHHmmss")))
                sqlcmd = $"select * from alarm_history_info where update_time >= '{FirstDay.ToString("yyyyMMddHHmmss")}' union select * from alarm_currently_info  where  update_time >= '{FirstDay.ToString("yyyyMMddHHmmss")}' ORDER BY mach_name asc";
            else
                sqlcmd = $"select * from alarm_history_info where update_time >= '{FirstDay.ToString("yyyyMMddHHmmss")}' and enddate_time <='{LastDay.ToString("yyyyMMddHHmmss")}'  ORDER BY mach_name asc";


            if (Record_Machine.Count == 0)
                error_count_tr.Clear();
            dt_data = DataTableUtils.GetDataTable(sqlcmd);
            List<string> machine_list = new List<string>();
            string machinename = "";
            string sql = "";
            string workstaff = "";
            DataTable ds = new DataTable();
            DateTime updatetime, endtime;
            if (dt_data != null && dt_data.Rows.Count != 0)
            {
                dt_count = 2;
                error_AlarmInfo_th = "<th>設備名稱</th>\n<th>異常訊息</th>\n<th>起始時間</th>\n<th>持續時間</th>\n<th>作業人員</th>";
                foreach (DataRow row in dt_data.Rows)
                {
                    if (ls_data.IndexOf(DataTableUtils.toString(row["mach_name"])) >= 0)
                    {
                        updatetime = HtmlUtil.StrToDate(DataTableUtils.toString(row["update_time"]).Split('.')[0]);
                        if (DataTableUtils.toString(row["enddate_time"]) == "null") endtime = DateTime.Now;
                        else endtime = DateTime.ParseExact(DataTableUtils.toString(row["enddate_time"]), "yyyyMMddHHmmss.f", System.Globalization.CultureInfo.CurrentCulture);

                        if (machine_list.IndexOf(DataTableUtils.toString(row["mach_name"])) == -1)
                        {
                            //加入MDC的機器名稱
                            machine_list.Add(DataTableUtils.toString(row["mach_name"]));
                            //加入轉換的機器名稱
                            machine_list.Add(CNCUtils.MachName_translation(DataTableUtils.toString(row["mach_name"])));
                            machinename = CNCUtils.MachName_translation(DataTableUtils.toString(row["mach_name"]));
                            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                            sql = $"select work_staff from aps_info where mach_name = '{DataTableUtils.toString(row["mach_name"])}' and ({updatetime.ToString("yyyyMMddHHmmss")}<= start_time OR end_time >= {endtime.ToString("yyyyMMddHHmmss")})";
                            ds = DataTableUtils.GetDataTable(sql);
                            if (HtmlUtil.Check_DataTable(ds))
                                workstaff = DataTableUtils.toString(ds.Rows[0]["work_staff"]);
                            else
                                workstaff = "";
                            //加入加工人員
                            machine_list.Add(workstaff);
                            Record_Machine.Add(DataTableUtils.toString(row["mach_name"]));
                        }
                        else
                        {
                            machinename = machine_list[machine_list.IndexOf(DataTableUtils.toString(row["mach_name"])) + 1];
                            workstaff = machine_list[machine_list.IndexOf(DataTableUtils.toString(row["mach_name"])) + 2];
                        }

                        TimeSpan alarm_continue_time = TimeSpan.FromSeconds(DataTableUtils.toDouble(DataTableUtils.toString(row["timespan"])));
                        error_count_tr.Append("<tr>");
                        error_count_tr.Append($"<td style=\"width:15%\">{machinename}</td>");
                        error_count_tr.Append($"<td style=\"width:35%\">{DataTableUtils.toString(row["alarm_type"])}-{DataTableUtils.toString(row["alarm_num"])}-{DataTableUtils.toString(row["alarm_mesg"])}</td>");
                        error_count_tr.Append($"<td style=\"width:20%\">{Convert.ToDateTime(updatetime).ToString("yyyy/MM/dd HH:mm:ss")}</td>");
                        error_count_tr.Append($"<td style=\"width:15%\">{alarm_continue_time.Hours}:{alarm_continue_time.Minutes}:{alarm_continue_time.Seconds}</td>\n");
                        error_count_tr.Append($"<td style=\"width:15%\">{workstaff}</td>");
                        error_count_tr.Append("</tr>");
                    }
                }
            }
            else
            {
                dt_count = 0;
                string tr = "";
                HtmlUtil.NoData(out error_AlarmInfo_th, out tr);
                error_count_tr.Append(tr);
            }
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
            string sql = "";
            DataTable dt_mach = new DataTable();
            string all_mach = "";

            ListItem listItem = new ListItem();
            List<string> machlist = new List<string>();


            DropDownList_MachType.Items.Clear();
            dt_data = DataTableUtils.GetDataTable("select distinct area_name from mach_group where area_name <> '全廠' and area_name <> '測試區'  ");
            if (HtmlUtil.Check_DataTable(dt_data))
            {
                DropDownList_MachType.Items.Add("--Select--");
                string itemname = "";
                string acc_power = CNCUtils.Find_Group(HtmlUtil.Search_acc_Column(acc, "Belong_Factory"));
                if (acc_power == "")
                {
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                    string sqlcmd = "select * from mach_group where area_name = '全廠'";
                    DataTable dt_all = DataTableUtils.GetDataTable(sqlcmd);
                    //全廠的部分
                    if (HtmlUtil.Check_DataTable(dt_all))
                    {
                        //找出全廠設備
                        GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                        sql = "SELECT mach_show_name FROM machine_info where area_name <> '測試區'";
                        dt_mach = DataTableUtils.GetDataTable(sql);

                        //取得所有機台
                        if (HtmlUtil.Check_DataTable(dt_mach))
                        {
                            foreach (DataRow rw in dt_mach.Rows)
                                all_mach += DataTableUtils.toString(rw["mach_show_name"]) + "^";
                        }
                        itemname = $"--Select--,1^,";
                        foreach (DataRow row in dt_all.Rows)
                        {
                            if (DataTableUtils.toString(row["web_address"]) != "")
                                itemname += $"{DataTableUtils.toString(row["group_name"])},{DataTableUtils.toString(row["web_address"])}{Request.FilePath},";
                            else
                                itemname += $"{DataTableUtils.toString(row["group_name"])},1^{all_mach},";
                        }
                        listItem = new ListItem("全廠", itemname);
                        DropDownList_MachType.Items.Add(listItem);
                    }

                    //其他廠區的部分
                    if (HtmlUtil.Check_DataTable(dt_data))
                    {
                        foreach (DataRow row in dt_data.Rows)
                        {
                            itemname = "";
                            machlist.Clear();
                            all_mach = "";
                            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                            sqlcmd = $"select * from mach_group where area_name = '{DataTableUtils.toString(row["area_name"])}'";
                            dt_all = DataTableUtils.GetDataTable(sqlcmd);


                            foreach (DataRow rew in dt_all.Rows)
                            {

                                if (DataTableUtils.toString(rew["web_address"]) != "")
                                    itemname += $"{DataTableUtils.toString(rew["group_name"])},{DataTableUtils.toString(rew["web_address"])}{Request.FilePath},";
                                else
                                {
                                    all_mach = "";
                                    string[] mach = DataTableUtils.toString(rew["mach_name"]).Split(',');
                                    for (int i = 0; i < mach.Length; i++)
                                    {
                                        if (mach[i] != "")
                                        {
                                            all_mach += CNCUtils.MachName_translation(mach[i]) + "^";
                                            machlist.Add(CNCUtils.MachName_translation(mach[i]));
                                        }
                                    }

                                    itemname += $"{DataTableUtils.toString(rew["group_name"])},1^{all_mach}^,";

                                }



                            }
                            //machlist = machlist.Distinct().ToList();
                            //for (int i = 0; i < machlist.Count; i++)
                            //    all_mach += machlist[i] + "^";

                            itemname = $"--Select--,1^," + itemname;
                            listItem = new ListItem(DataTableUtils.toString(row["area_name"]), itemname);
                            DropDownList_MachType.Items.Add(listItem);
                        }
                    }
                }
                //特定廠區
                else
                {
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                    string sqlcmd = $"select * from mach_group where area_name = '{acc_power}'";
                    DataTable dt_all = DataTableUtils.GetDataTable(sqlcmd);
                    //全廠的部分
                    if (HtmlUtil.Check_DataTable(dt_all))
                    {
                        itemname = "";
                        machlist.Clear();
                        all_mach = "";

                        foreach (DataRow row in dt_all.Rows)
                        {
                            if (DataTableUtils.toString(row["web_address"]) != "")
                                itemname += $"{DataTableUtils.toString(row["group_name"])},{DataTableUtils.toString(row["web_address"])}{Request.FilePath},";
                            else
                            {
                                all_mach = "";
                                string[] mach = DataTableUtils.toString(row["mach_name"]).Split(',');
                                for (int i = 0; i < mach.Length; i++)
                                {
                                    if (mach[i] != "")
                                    {
                                        all_mach += CNCUtils.MachName_translation(mach[i]) + "^";
                                        machlist.Add(CNCUtils.MachName_translation(mach[i]));
                                    }
                                }

                                itemname += $"{DataTableUtils.toString(row["group_name"])},1^{all_mach}^,";

                            }


                        }

                        machlist = machlist.Distinct().ToList();
                        for (int i = 0; i < machlist.Count; i++)
                            all_mach += machlist[i] + "^";

                        itemname = $"--Select--,1^," + itemname;
                        listItem = new ListItem(acc_power, itemname);
                        DropDownList_MachType.Items.Add(listItem);
                    }
                }
            }

        }
        //event
        protected void Select_MachGroupClick(object sender, EventArgs e)    //執行運算
        {
            DropDownList_MachType.SelectedIndex = DropDownList_MachType.Items.IndexOf(DropDownList_MachType.Items.FindByValue(TextBox_MachTypeValue.Text));
            DropDownList_MachGroup.Items.Clear();
            List<string> list = new List<string>(TextBox_MachTypeValue.Text.Split(','));
            for (int i = 0; i < list.Count - 1; i++)
            {
                ListItem listItem = new ListItem(list[i], list[i + 1]);
                DropDownList_MachGroup.Items.Add(listItem);
                i++;
            }
            DropDownList_MachGroup.SelectedIndex = DropDownList_MachGroup.Items.IndexOf(DropDownList_MachGroup.Items.FindByText(TextBox_MachGroupText.Text));



            if (DropDownList_MachGroup.Items.Count != 0 && DropDownList_MachGroup.SelectedItem.Value.Split('^')[0] != "1")
            {
                set_page_content();
                Response.Redirect(DropDownList_MachGroup.SelectedItem.Value.Split('^')[0], "_blank", "");
                DropDownList_MachGroup.Items.Clear();
            }

            else if (DropDownList_MachType.SelectedItem.Text != "--Select--")
            {
                Record_Machine.Clear();
                b_Page_Load = false;
                string sqlcmd = "";
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                sqlcmd = $"select * from mach_group where area_name = '{DropDownList_MachType.SelectedItem.Text}' ";

                DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
                if (HtmlUtil.Check_DataTable(dt))
                {
                    if (DropDownList_MachGroup.SelectedItem.Text == "--Select--")
                    {
                        if (HtmlUtil.Check_DataTable(dt))
                        {
                            foreach (DataRow row in dt.Rows)
                                Read_Data(DataTableUtils.toString(row["group_name"]));
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
            textbox_st.Text = FirstDay.ToString("yyyy-MM-dd");
            textbox_ed.Text = LastDay.ToString("yyyy-MM-dd");


        }




    }
}