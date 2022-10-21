using System;
using Support;
using dek_erpvis_v2.cls;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI;
using System.Text;

namespace dek_erpvis_v2.pages.dp_CNC
{
    public partial class Machine_list_info : System.Web.UI.Page
    {
        public string workmans = "";
        public string color = "";
        double mach_count = 0;//機台總數
        double machine_count = 0;
        public int run = 0;//運轉
        public int rest = 0;//待機
        public int alert = 0;//警告
        public int noline = 0;//離線
        public int other = 0;
        public double percent = 0;//稼動率
        public double progress = 0;//生產進度
        public string js_code = "";
        //丟到前端的資料
        public DateTime FirstDay = new DateTime();
        public DateTime LastDay = new DateTime();
        public string str_First_Day = "";
        public string str_Last_Day = "";
        public string str_Dev_Status = "";
        public string dev_name = "";
        public List<string> dev_list = null;
        public List<string> ls_data = new List<string>();
        public bool is_ok = false;
        public DataTable dt_data = null;
        CNC_Web_Data Web_Data = new CNC_Web_Data();
        public string Refresh_Time = "";
        public StringBuilder th = new StringBuilder();
        public StringBuilder tr = new StringBuilder();
        public StringBuilder area = new StringBuilder();
        public StringBuilder Calculation_td = new StringBuilder();
        public string acc = "";
        string order_num = "";
        public string str_status = "";
        public string str_Dev_Name = "";
        string acc_power = "";
        public bool b_Page_Load = true;
        CNCUtils cNC_Class = new CNCUtils();
        public string Gantt_Data = "";
        public string machine_list = "";
        List<string> list = new List<string>();
        public string show_gantt_image = "";
        Dictionary<string, string> keyValues = new Dictionary<string, string>();
        DataTable dt_staff = new DataTable();
        List<string> Record_Machine = new List<string>();
        DataTable dt_next = new DataTable();
        DataTable dt_machurl = new DataTable();
        List<string> list_machname = new List<string>();
        enum classcolor { ganttRed, ganttGreen, ganttOrange, ganttBlue, ganttPurple };
        protected void Page_Load(object sender, EventArgs e)
        {
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            str_First_Day = DateTime.Now.ToString("yyyy/MM/dd");
            str_Last_Day = DateTime.Now.AddDays(1).ToString("yyyy/MM/dd");

            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);

                Refresh_Time = set_time(acc);
                color = HtmlUtil.change_color(acc);
                if (myclass.user_view_check("Machine_list_info", acc))
                {
                    if (!IsPostBack)
                        set_page_content();
                }
                else
                    Response.Write("<script>alert('您無法瀏覽此頁面 請向該單位主管申請權限!');location.href='../index.aspx';</script>");
            }
            else
                Response.Redirect(myclass.logout_url);
            div_button.Attributes["style"] = "display:block";
        }
        private void set_page_content()
        {
            show_checkbox();//0.2s
            Read_Data();//10S
            Get_MachType_List();//3S
            Mach_Info_List("");
        }
        //fuction         
        public void Read_Data(string machgroup = "")
        {
            //列出加工人員的資料表
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            string sqlcmd = $"select staff_name,mach_name from staff_info,machine_info where machine_info.area_name = staff_info.area_name";
            dt_staff = DataTableUtils.GetDataTable(sqlcmd);

            //列出每筆排程的資料表
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            sqlcmd = $"select * from aps_list_info order by order_number asc";
            dt_next = DataTableUtils.GetDataTable(sqlcmd);

            acc_power = CNCUtils.Find_Group(HtmlUtil.Search_acc_Column(acc, "Belong_Factory"));

            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            sqlcmd = "select * from realtime_info ";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
            if (HtmlUtil.Check_DataTable(dt))
            {
                keyValues.Clear();
                foreach (DataRow row in dt.Rows)
                    keyValues.Add(DataTableUtils.toString(row["info_name"]), DataTableUtils.toString(row["info_chinese"]));
            }

            if (WebUtils.GetAppSettings("show_CNCgantt") == "1")
            {
                show_gantt_image = "<li role=\"presentation\" class=\"\" style=\"box-shadow: 3px 3px 9px gray;\"><a href=\"#tab_content3\" role=\"tab\" id=\"profile-tab2\" onclick=\"gantt_show()\" data-toggle=\"tab\" aria-expanded=\"false\">排程清單</a></li>";
                show_Gantt();
            }
            string condition = acc_power == "" ? " where area_name <> '測試區' " : $" where area_name = '{acc_power}' ";

            //找出可以被選取的欄位
            if (th.ToString() == "")
                th = set_column();
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            ls_data.Clear();
            if (b_Page_Load || DropDownList_MachType.Text == "--Select--")//|| DropDownList_MachGroup.Text == "--Select--")
            {
                DataTable dt_mach = DataTableUtils.GetDataTable($"select mach_name from machine_info {condition}");
                if (HtmlUtil.Check_DataTable(dt_mach))
                {
                    foreach (DataRow row in dt_mach.Rows)
                        ls_data.Add(row["mach_name"].ToString());
                }
            }
            else
            {
                if (machgroup != "不存在")
                {
                    if (DropDownList_MachType.SelectedItem.Text == "全廠" && (DropDownList_MachGroup.SelectedItem.Text == "全廠設備" || DropDownList_MachGroup.SelectedItem.Text == "--Select--"))
                    {
                        GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                        DataTable ds = DataTableUtils.GetDataTable("select mach_name from machine_info where area_name <> '測試區' order by _id desc");
                        if (HtmlUtil.Check_DataTable(ds))
                        {
                            foreach (DataRow dataRow in ds.Rows)
                                ls_data.Add(DataTableUtils.toString(dataRow["mach_name"]));
                        }
                    }
                    else
                    {
                        GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                        ls_data = DataTableUtils.GetDataTable($"select mach_name from mach_group where group_name = '{machgroup}'").Rows[0].ItemArray[0].ToString().Split(',').ToList();
                    }
                }
                else
                    ls_data = null;
            }
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            dt_machurl = DataTableUtils.GetDataTable("select  mach_name,mach_show_name,img_url,camera_address,camera_address_2 from machine_info");

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

                mach_count = ls_data.Count;
                machine_count = ls_data.Count;
                for (int iIndex = 0; iIndex < ls_data.Count; iIndex++)
                {
                    machine_list += ls_data[iIndex] + ",";
                    dt_data = Web_Data.Get_MachInfo(ls_data[iIndex]);
                    dev_name = ls_data[iIndex];
                    if (dt_data != null)
                    {
                        string CheckStaff, WorkStaff, MachName, CustomName, ManuId, ProductName, ProductNumber, CraftName, CountTotal, CountToday, ExpCountToday, CountTodayRate, FinishTime, StatusBar, OperRate, MachStatus, AlarmMesg, ProgramRun, ProgramMain;
                        CheckStaff = Web_Data.Get_CheckStaff(dt_data);//Y校機人員2
                        WorkStaff = Web_Data.Get_WorkStaff(dt_data);//Y加工人員3
                        MachName = Web_Data.Get_MachName(dt_data, ls_data[iIndex]);//設備名稱
                        CustomName = Web_Data.Get_CustomName(dt_data);//Y客戶名稱5
                        ManuId = Web_Data.Get_ManuID(dt_data);//Y製令單號6
                        ProductName = Web_Data.Get_ProductName(dt_data);//Y產品名稱7
                        ProductNumber = Web_Data.Get_ProductNumber(dt_data);//Y產品編號8
                        CraftName = Web_Data.Get_CraftName(dt_data);//Y工藝名稱9
                        CountTotal = Web_Data.Get_CountTotal(dt_data);//Y生產總數11             //註解
                        CountToday = Web_Data.Get_CountToday(dt_data);//Y生產件數(當日)12         //註解
                        ExpCountToday = Web_Data.Get_ExpCountToday(dt_data);//Y目標件數(當日)13
                        CountTodayRate = Web_Data.Get_CountTodayRate(dt_data);//Y生產進度(當日)14       //註解
                        FinishTime = Web_Data.Get_FinishTime(dt_data);//ok                 //註解  
                        StatusBar = ls_data[iIndex];//狀態Bar
                        OperRate = Web_Data.Get_Operate_Rate(dt_data, MachName);
                        MachStatus = Web_Data.Get_MachStatus(dt_data, ls_data[iIndex]);
                        AlarmMesg = Web_Data.Get_AlarmMesg(dt_data);//Y異警資訊16               //註解
                        ProgramRun = Web_Data.Get_ProgramRun(dt_data, ls_data[iIndex]);//主程式                 //註解     
                        
                        //20201201新增
                        string acts = Web_Data.Get_Information(dt_data, "acts");//主軸轉速
                        string spindleload = Web_Data.Get_Information(dt_data, "spindleload");//主軸負載
                        string spindlespeed = Web_Data.Get_Information(dt_data, "spindlespeed");//主軸速度
                        string spindletemp = Web_Data.Get_Information(dt_data, "spindletemp");//主軸溫度
                        string prog_main = Web_Data.Get_Information(dt_data, "prog_main"); //主程式
                        string prog_main_cmd = Web_Data.Get_Information(dt_data, "prog_main_cmd");//主程式註解
                        string prog_run_cmd = Web_Data.Get_Information(dt_data, "prog_run_cmd");//運行程式註解
                        string overrides = Web_Data.Get_Information(dt_data, "override");//進給率

                        //20220622新增 20220928移除
                        //string spindle_shock = Web_Data.Get_Information(dt_data, "spindle_shock");//主軸震動
                        //string spindle_side_temp = Web_Data.Get_Information(dt_data, "spindle_side_temp");// 主軸側溫度
                        //string daoku_motor_electric = Web_Data.Get_Information(dt_data, "daoku_motor_electric");// 刀庫馬達電流_損壞預警
                        //string spindle_koudao_shock = Web_Data.Get_Information(dt_data, "spindle_koudao_shock");//主軸扣刀_震動監視
                        //string spindle_position = Web_Data.Get_Information(dt_data, "spindle_position");//主軸定位_在位確認
                        //string spindle_ladao_position = Web_Data.Get_Information(dt_data, "spindle_ladao_position");//主軸拉刀_在位確認
                        //string oil_level = Web_Data.Get_Information(dt_data, "oil_level");//潤滑油_油位檢知
                        //string ball_screw_hightemp = Web_Data.Get_Information(dt_data, "ball_screw_hightemp");//ball_screw_高溫監視
                        //string tool_oil_temp = Web_Data.Get_Information(dt_data, "tool_oil_temp");//治具油壓_溫度監視
                        //string tool_oil_pressure = Web_Data.Get_Information(dt_data, "tool_oil_pressure");//治具油壓_壓力監視
                        //string qiexieye_concentration = Web_Data.Get_Information(dt_data, "qiexieye_concentration");//切屑液_濃度檢知
                        //string qiexieye_temp = Web_Data.Get_Information(dt_data, "qiexieye_temp");//切屑液_溫度檢知
                        //string air_pressure = Web_Data.Get_Information(dt_data, "air_pressure");//氣壓源_壓力檢知

                        //20220912新增
                        string Spindle_temp = Web_Data.Get_Information(dt_data, "Spindle_temp");//主軸溫升
                        //string Tank_Hydraulic_Temp = Web_Data.Get_Information(dt_data, "Tank_Hydraulic_Temp");//油箱油溫
                        //string MainClamp_Hydraulic_Temp = Web_Data.Get_Information(dt_data, "MainClamp_Hydraulic_Temp");//主夾迴路油溫
                        //string AngularPositioning_Hydraulic_Temp = Web_Data.Get_Information(dt_data, "AngularPositioning_Hydraulic_Temp");//角向定位迴路油溫
                        //string Brake_Hydraulic_Temp_4th = Web_Data.Get_Information(dt_data,"4th_Brake_Hydraulic_Temp");//剎車迴路油溫
                        //string Tank_Hydraulic_Pressure = Web_Data.Get_Information(dt_data, "Tank_Hydraulic_Pressure");//主軸溫升
                        //string MainClamp_Hydraulic_Pressure = Web_Data.Get_Information(dt_data, "MainClamp_Hydraulic_Pressure");//主軸溫升
                        //string AngularPositioning_Hydraulic_Pressure = Web_Data.Get_Information(dt_data, "AngularPositioning_Hydraulic_Pressure");//角向定位迴路油壓
                        //string Brake_Hydraulic_Pressure_4th = Web_Data.Get_Information(dt_data, "4th_Brake_Hydraulic_Pressure");//剎車迴路油壓
                        string Coolant_Concentration = Web_Data.Get_Information(dt_data, "Coolant_Concentration");//切削液濃度
                        string Coolant_Temp = Web_Data.Get_Information(dt_data, "Coolant_Temp");//切削液溫度
                        string Air_Temp = Web_Data.Get_Information(dt_data, "Air_Temp");//空壓源溫度

                        //20221004新增
                        string Air_Pressure = Web_Data.Get_Information(dt_data, "Air_Pressure");//空壓源壓力
                        string Spindle_loading = Web_Data.Get_Information(dt_data, "Spindle_loading");//主軸負載(Deep)
                        string Servo_loading_X = Web_Data.Get_Information(dt_data, "Servo_loading_X");//伺服軸負載(X)
                        string Servo_loading_Y = Web_Data.Get_Information(dt_data, "Servo_loading_Y");//伺服軸負載(Y)
                        string Servo_loading_Z = Web_Data.Get_Information(dt_data, "Servo_loading_Z");//伺服軸負載(Z)
                        string Servo_loading_B = Web_Data.Get_Information(dt_data, "Servo_loading_B");//伺服軸負載(B)


                        string run_time = Web_Data.Get_Information(dt_data, "run_time");//運轉時間
                        string cut_time = Web_Data.Get_Information(dt_data, "cut_time");//切削時間
                        string poweron_time = Web_Data.Get_Information(dt_data, "poweron_time");//通電時間
                        //20210105新增
                        string complete_time = Web_Data.Get_Information(dt_data, "complete_time");//完成時間
                        order_num = Web_Data.Get_Information(dt_data, "order_number");
                        //判斷是否可以繼續下個排程
                        double Date_Now = DataTableUtils.toDouble(DateTime.Now.ToString("yyyyMMddHHmmss"));
                        double Date_Start = DataTableUtils.toDouble(dt_data.Rows[0]["start_time"].ToString());
                        string can_next = "";
                        if ((Date_Now - Date_Start) >= 0)
                            can_next = "can";

                        int count = 0;
                        DataRow[] rews = dt_next.Select($"order_number > '{order_num}' and mach_name = '{MachName}'");

                        if (rews != null && rews.Length != 0)
                            count = rews.Length;

                        string now_detailstatus = "";
                        if (HtmlUtil.Check_DataTable(dt_data))
                        {
                            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                            sqlcmd = $"select * from record_worktime where start_time = '{dt_data.Rows[0]["start_time"]}' and end_time = '{dt_data.Rows[0]["end_time"]}' and mach_name = '{MachName}'   order by NOW_TIME desc limit 1";
                            DataTable dt_status = DataTableUtils.GetDataTable(sqlcmd);

                            if (HtmlUtil.Check_DataTable(dt_status))
                                now_detailstatus = dt_status.Rows[0]["WORKMAN_STATUS"].ToString();
                        }
                        

                        設定圖塊(MachName, CheckStaff, WorkStaff, MachStatus, OperRate, StatusBar, ManuId, CustomName, ProductName, ProductNumber, ProgramRun, CountTotal, ExpCountToday, FinishTime, "0", CountTodayRate, AlarmMesg, CountToday, CraftName, acts, spindleload, spindlespeed, spindletemp, prog_main, prog_main_cmd, prog_run_cmd, overrides, run_time, cut_time, poweron_time, complete_time);
                        設定表格(MachName, CheckStaff, WorkStaff, MachStatus, OperRate, StatusBar, ManuId, CustomName, ProductName, ProductNumber, ProgramRun, CountTotal, ExpCountToday, FinishTime, "0", CountTodayRate, AlarmMesg, CountToday, CraftName, acts, spindleload, spindlespeed, spindletemp, prog_main, prog_main_cmd, prog_run_cmd, overrides, run_time, cut_time, poweron_time, complete_time, Spindle_temp, Coolant_Concentration, Coolant_Temp, Air_Temp, Air_Pressure, Spindle_loading, Servo_loading_X, Servo_loading_Y, Servo_loading_Z, Servo_loading_B, order_num, count, can_next, now_detailstatus);



                        if (iIndex < ls_data.Count - 1) str_Dev_Status += MachStatus + ", ";
                        else str_Dev_Status += MachStatus;
                    }
                }
                machine_list = WebUtils.UrlStringEncode(machine_list);
            }
            else if ((ls_data == null || ls_data.Count == 0) && th == null && tr == null)
                HtmlUtil.NoData(out th, out tr);
        }
        private void 設定表格(string 設備名稱, string 校機人員, string 加工人員, string 設備狀態, string 設備稼動, string 設備稼動_長條圖, string 製令單號, string 客戶名稱, string 產品名稱, string 料件編號, string 加工程式, string 生產件數, string 預計生產件數, string 預計完工時間, string 問題回報, string 生產進度, string 異警資訊, string 今日生產件數, string 工藝名稱, string 主軸轉速, string 主軸負載, string 主軸速度, string 主軸溫度, string 主程式, string 主程式註解, string 運行程式註解, string 進給率, string 運轉時間, string 切削時間, string 通電時間, string 應完工時間, string 主軸溫升, string 切削液濃度, string 切削液溫度, string 空壓源溫度,string 空壓源壓力, string 主軸負載Deep, string 伺服軸負載X, string 伺服軸負載Y, string 伺服軸負載Z, string 伺服軸負載B, string 工藝, int next_count = 0, string 能否繼續 = "", string 明細狀態 = "")
        {
            string total_Value = "";
            string 設備狀態_色彩 = cNC_Class.mach_status_Color(設備狀態);
            設備狀態 = cNC_Class.mach_status_EN2CH(設備狀態);

            if (工藝 == "0" || 工藝 == "")
                工藝 = "disabled=\"disabled\" value=\"完工\"";
            else
                工藝 = "value=\"完工\"";

            if (能否繼續 != "can")
                工藝 = "disabled=\"disabled\" value=\"完工\"";


            tr.Append($"<tr style='color:white; background-color:{設備狀態_色彩};' id='{設備名稱}'>");
            string old_name = 設備名稱;
            //這部分由於空白會造成換行的情況，因此做此措施
            設備名稱 = list_machname[list_machname.IndexOf(old_name) + 1];

            switch (設備狀態)
            {
                case "運轉":
                    run++;
                    break;
                case "待機":

                    rest++;
                    break;
                case "警報":

                    alert++;
                    break;
                case "離線":
                    noline++;
                    break;
                case "手動":
                case "暖機":
                case "暫停":
                case "警告":
                    other++;
                    break;
            }

            percent += Math.Round(DataTableUtils.toDouble(設備稼動) / machine_count, 1);
            progress += Math.Round(DataTableUtils.toDouble(生產進度) / machine_count, 1);


            if (WebUtils.GetAppSettings("switch_cnc") == "0")
            {
                if (工藝 == "value=\"完工\"")
                    tr.Append(show_table(old_name, "next_button", $" <input type=\"button\" style=\"width:100%;height:100%;background-color:#10A0FF;border-radius:15px\"  onclick=Next_Craft('{old_name}','{order_num}','{今日生產件數}','{預計生產件數}')  {工藝} > ", "1"));
                else
                    tr.Append(show_table(old_name, "next_button", $" <input type=\"button\" style=\"width:100%;height:100%;background-color:#BEBEBE;border-radius:15px	\"  {工藝} > ", "1"));
            }
            else
            {
                if (工藝 == "value=\"完工\"")
                {
                    //當下排程資料
                    string List_now = $"{設備名稱}^{加工人員}^{製令單號}^{客戶名稱}^{產品名稱}^{料件編號}^{工藝名稱}^{DataTableUtils.toInt(今日生產件數)}^{DataTableUtils.toInt(預計生產件數)}^";
                    DataRow[] row_next = dt_next.Select($"mach_name = '{old_name}' and order_number  > '{order_num}'");

                    string List_next = "";
                    if (row_next != null && row_next.Length > 0)
                        List_next = $"{設備名稱}^{row_next[0]["work_staff"]}^{row_next[0]["manu_id"]}^{row_next[0]["custom_name"]}^{row_next[0]["product_name"]}^{row_next[0]["product_number"]}^{row_next[0]["craft_name"]}^{"0"}^{row_next[0]["exp_product_count_day"]}^";

                    List<string> man_list = new List<string>();
                    string staff = "";
                    DataRow[] rows = dt_staff.Select($"mach_name='{old_name}'");
                    if (rows != null && rows.Length > 0)
                    {
                        for (int i = 0; i < rows.Length; i++)
                        {
                            if (man_list.IndexOf(rows[i]["staff_name"].ToString().Trim()) == -1)
                            {
                                staff += $"{rows[i]["staff_name"]}^";
                                man_list.Add(rows[i]["staff_name"].ToString().Trim());
                            }
                        }
                    }
                    tr.Append(show_table(old_name, "next_button", $"<a href=\"javascript:void(0)\"><img src=\"../../assets/images/canclick.png\"  width=\"50px\" height=\"50px\" data-toggle = \"modal\" data-target = \"#Next_Task\" onclick=Next_Task('{old_name}','{order_num.Trim()}','{明細狀態}','{今日生產件數}','{預計生產件數}','{List_now.Replace(' ', '*')}','{List_next.Replace(' ', '*')}','{staff.Replace(' ', '*')}')  /></a>", "1"));
                }
                else
                    tr.Append(show_table(old_name, "next_button", $" <a href=\"javascript:void(0)\"><img src=\"../../assets/images/unclick.png\" width=\"50px\" height=\"50px\" onclick=\"alert('工單已完結，請由可視化主控台指派')\" /></a> ", "1"));
            }

            //20220811 生產進度新增NAN判斷
            if (DataTableUtils.toInt(今日生產件數) > 0 && DataTableUtils.toInt(預計生產件數) == 0)
            {
                 total_Value = $" NAN ( {DataTableUtils.toInt(今日生產件數)} / {DataTableUtils.toInt(預計生產件數)} )";
            }
            else {
                total_Value = DataTableUtils.toInt(生產進度) + $" %  ( {DataTableUtils.toInt(今日生產件數)} / {DataTableUtils.toInt(預計生產件數)} )";
            }

            tr.Append(show_table(old_name, "mach_name", $"<a href=\"javascript:void(0)\" onclick=jump('{WebUtils.UrlStringEncode($"machine={old_name}")}')><u style=\"color:blue\">{設備名稱}</u></a>"));
            tr.Append(show_table(old_name, "check_staff", 校機人員));
            tr.Append(show_table(old_name, "work_staff", 加工人員));
            tr.Append(show_table(old_name, "mach_status", 設備狀態));
            tr.Append(show_table(old_name, "operate_rate", DataTableUtils.toInt(設備稼動) + " %"));
            tr.Append(show_table(old_name, "manu_id", 製令單號));
            tr.Append(show_table(old_name, "custom_name", 客戶名稱));
            tr.Append(show_table(old_name, "product_name", 產品名稱));
            tr.Append(show_table(old_name, "product_number", 料件編號));
            tr.Append(show_table(old_name, "craft_name", 工藝名稱));
            tr.Append(show_table(old_name, "count_today_rate", total_Value));
            tr.Append(show_table(old_name, "complete_time", CNCUtils.date_Substring(應完工時間)));
            tr.Append(show_table(old_name, "finish_time", CNCUtils.date_Substring(預計完工時間)));
            tr.Append(show_table(old_name, "acts", 主軸轉速));
            tr.Append(show_table(old_name, "spindleload", 主軸負載));
            tr.Append(show_table(old_name, "spindlespeed", 主軸速度));
            tr.Append(show_table(old_name, "spindletemp", 主軸溫度));
            tr.Append(show_table(old_name, "prog_main", 主程式));
            tr.Append(show_table(old_name, "prog_main_cmd", 主程式註解));
            tr.Append(show_table(old_name, "program_run", 加工程式));
            tr.Append(show_table(old_name, "prog_run_cmd", 運行程式註解));
            tr.Append(show_table(old_name, "override", 進給率));
            tr.Append(show_table(old_name, "run_time", 運轉時間));
            tr.Append(show_table(old_name, "cut_time", 切削時間));
            tr.Append(show_table(old_name, "poweron_time", 通電時間));
            tr.Append(show_table(old_name, "alarm_mesg", 異警資訊));
            //tr.Append(show_table(old_name, "spindle_shock", 主軸轉動));
            //tr.Append(show_table(old_name, "spindle_side_temp", 主軸側溫度));
            //tr.Append(show_table(old_name, "daoku_motor_electric", 刀庫馬達電流_損壞預警));
            //tr.Append(show_table(old_name, "spindle_koudao_shock", 主軸扣刀_震動監視));
            //tr.Append(show_table(old_name, "spindle_position", 主軸定位_在位確認));
            //tr.Append(show_table(old_name, "spindle_ladao_position", 主軸拉刀_在位確認));
            //tr.Append(show_table(old_name, "oil_level", 潤滑油_油位檢知));
            //tr.Append(show_table(old_name, "ball_screw_hightemp", ball_screw_高溫監視));
            //tr.Append(show_table(old_name, "tool_oil_temp", 治具油壓_溫度監視));
            //tr.Append(show_table(old_name, "tool_oil_pressure", 治具油壓_壓力監視));
            //tr.Append(show_table(old_name, "qiexieye_concentration", 切屑液_濃度檢知));
            //tr.Append(show_table(old_name, "qiexieye_temp", 切屑液_溫度檢知));
            //tr.Append(show_table(old_name, "air_pressure", 氣壓源_壓力檢知));

            tr.Append(show_table(old_name, "Spindle_temp", 主軸溫升));
            //tr.Append(show_table(old_name, "Tank_Hydraulic_Temp", 油箱油溫));
            //tr.Append(show_table(old_name, "MainClamp_Hydraulic_Temp", 主夾迴路油溫));
            //tr.Append(show_table(old_name, "AngularPositioning_Hydraulic_Temp", 角向定位迴路油溫));
            //tr.Append(show_table(old_name, "Brake_Hydraulic_Temp_4th", 剎車迴路油溫));
            //tr.Append(show_table(old_name, "Tank_Hydraulic_Pressure", 油箱油壓));
            //tr.Append(show_table(old_name, "MainClamp_Hydraulic_Pressure", 主夾迴路油壓));
            //tr.Append(show_table(old_name, "AngularPositioning_Hydraulic_Pressure", 角向定位迴路油壓));
            //tr.Append(show_table(old_name, "Brake_Hydraulic_Pressure_4th", 剎車迴路油壓));
            tr.Append(show_table(old_name, "Coolant_Concentration", 切削液濃度));
            tr.Append(show_table(old_name, "Coolant_Temp", 切削液溫度));
            tr.Append(show_table(old_name, "Air_Temp", 空壓源溫度));

            tr.Append(show_table(old_name, "Air_Pressure", 空壓源壓力));
            tr.Append(show_table(old_name, "Spindle_loading", 主軸負載Deep));
            tr.Append(show_table(old_name, "Servo_loading_X", 伺服軸負載X));
            tr.Append(show_table(old_name, "Servo_loading_Y", 伺服軸負載Y));
            tr.Append(show_table(old_name, "Servo_loading_Z", 伺服軸負載Z));
            tr.Append(show_table(old_name, "Servo_loading_B", 伺服軸負載B));

            tr.Append("</tr>");
        }
        //設定表格欄位
        private StringBuilder set_column()
        {
            StringBuilder sb = new StringBuilder();
            list.Clear();
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            //個人設定之欄位
            DataTable dt = DataTableUtils.GetDataTable($"select * from realtime_info,show_column  where show_status='True' and show_check='Y' and Account = '{acc}' and Allow = 'True'  and show_column.Column_Name = realtime_info.info_name");

            //沒設定過→依據系統
            if (dt != null && dt.Rows.Count == 0)
            {
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                dt = DataTableUtils.DataTable_GetTable("select * from realtime_info where show_status='True' and show_check='Y'");
            }

            if (HtmlUtil.Check_DataTable(dt))
            {
                //同時存入欄位英文與中文名
                foreach (DataRow row in dt.Rows)
                {
                    list.Add(DataTableUtils.toString(row["info_name"]));
                    if (DataTableUtils.toString(row["info_chinese"]) == "生產進度")
                        list.Add("生產進度(目前/預計)");
                    else
                        list.Add(DataTableUtils.toString(row["info_chinese"]));
                }
            }
            return sb;
        }
        public string show_table(string machine, string column, string value, string ex = "")
        {
            if (ex != "")
                ex = "width:100%;height:40px; ";
            string background = "";

            //依據目前的第一個來動作->目前第一個是工程報工
            if (column == "next_button")
            {
                th = new StringBuilder();
                if (WebUtils.GetAppSettings("switch_cnc") != "0")
                    background = "background-color:#ffffff;";
            }

            if (list.IndexOf(column) != -1)
            {
                th.Append($"<th style=\"vertical-align:middle;text-align:center\">{list[list.IndexOf(column) + 1]}</th>");
                return $"<td align=\"center\" style=\"vertical-align:middle;color:black;{background}\" ><label id=\"{machine}_{column}\" style=\"font-weight:normal;{ex}\" >{value}</label></td>";
            }
            else
                return "";
        }
        private void 設定圖塊(string 設備名稱, string 校機人員, string 加工人員, string 設備狀態_燈號, string 設備稼動, string 設備稼動_長條圖, string 製令單號, string 客戶名稱, string 產品名稱, string 料件編號, string 加工程式, string 生產件數, string 預計生產件數, string 預計完工時間, string 問題回報, string 生產進度, string 異警資訊, string 今日生產件數, string 工藝名稱, string 主軸轉速, string 主軸負載, string 主軸速度, string 主軸溫度, string 主程式, string 主程式註解, string 運行程式註解, string 進給率, string 運轉時間, string 切削時間, string 通電時間, string 應完工時間)
        {
            string machine = "mach" + mach_count;
            mach_count++;
            string 設備狀態 = cNC_Class.mach_status_EN2CH(設備狀態_燈號);
            string 字體顏色 = cNC_Class.mach_font_Color(設備狀態);
            string 背景顏色 = cNC_Class.mach_background_Color(設備狀態);
            string mach = 設備名稱;
            area.Append("<div class=\"item\">");
            area.Append($"<div id='chart_{設備名稱}' class='col-md-12 col-sm-12 col-xs-12' >\n");
            area.Append("   <div class='dashboard_graph x_panel'  style=\" border: 2px solid #1f1f1f \" >\n");
            area.Append("       <div class='col-md-12 col-sm-12 col-xs-12' style='padding:0px 0px;'>\n");
            string[] str = CNCUtils.MachName_translation(設備名稱).Split(' ');
            if (str.Length > 1)
            {
                for (int i = 0; i < str.Length; i++)
                {
                    if (i == 0)
                        設備名稱 = str[i];
                    else
                        設備名稱 += "&nbsp" + str[i];
                }
            }
            else
                設備名稱 = str[0];

            string address2 = get_machineinformation(mach, "camera_address_2");
            int camera_count = string.IsNullOrEmpty(address2) ? 1 : 2;

            list_machname.Add(mach);
            list_machname.Add(設備名稱);
            area.Append("           <div class='left col-md-12 col-sm-12 col-xs-12' style='font-size:19px'>\n");
            area.Append("               <div class ='col-md-12 col-sm-12 col-xs-12' >\n");
            area.Append("                   <div class ='col-md-4 col-sm-12 col-xs-12' style='text-align:center;'>\n");
            //20221014新增攝影機按鈕
            area.Append("<div style=\"display:flex;flex-warp:nowarp\">");
            for (int i = 1; i <= camera_count; i++) {
                string camera_column = "camera_address";
                if (i == 2) camera_column = "camera_address_2";
                area.Append("<div style=\"display:flex;width:100%;\">");
                area.Append($"            <button onclick=\"gotocamera('{ get_machineinformation(mach, camera_column)}')\" type=\"button\" id=\"exportChart\" title=\"前往攝影機畫面_{i}\" style=\"width:90%;margin-top:6px; text-align:center; text-valign:center;\">");
                area.Append("<img src=\"../../assets/images/camera.png\" style=\"width:28px; height: 28px;\">");
                area.Append("</button>");
                area.Append("</div>");
            }
            area.Append("</div>");

            area.Append($"                     <img  class='img-rounded' src='/pages/dp_CNC/CNC_Image/{get_machineinformation(mach, "img_url")}.jpg' alt='...' style='width:100%;height:240px;align-items:center;'>");
            area.Append("                   </div>\n");
            area.Append("                   <div class ='col-md-8 col-sm-12 col-xs-12'>\n");
            area.Append(Image_typesetting("mach_status", $"               <div id = 'ma_canvas_status{mach}' class='_mdTitle' style='font-size:20px;text-align:center;background-color:{背景顏色};color:{字體顏色}; '>{設備狀態}</div>", "1"));
            area.Append(Image_typesetting("mach_name", 設備名稱, "1"));
            area.Append(Image_typesetting("count_today_rate", DataTableUtils.toInt(生產進度) + $" % ( {DataTableUtils.toInt(今日生產件數)} / {DataTableUtils.toInt(預計生產件數)} )", "1"));
            area.Append(Image_typesetting("operate_rate", DataTableUtils.toInt(設備稼動) + " % ", "1"));
            area.Append(Image_typesetting("complete_time", CNCUtils.date_Substring(應完工時間), "1"));
            area.Append(Image_typesetting("finish_time", CNCUtils.date_Substring(預計完工時間), "1"));
            area.Append("                   </div>\n");
            area.Append("               </div>\n");
            area.Append("           <div  class='col-xs-12 col-sm-12' >\n");
            area.Append(顯示資訊("status_bar", 設備稼動_長條圖));
            area.Append("           </div>\n");

            area.Append("               <div class ='col-md-9 col-sm-9 col-xs-8'>\n");
            area.Append("                   <div class ='col-md-12 col-sm-12 col-xs-9'>\n");
            area.Append($"                      <a data-toggle=\"collapse\" data-parent=\"#accordion\"  href=\"#{machine}\" onclick=\"change_icon('{machine}1','{machine}2')\"  >\n");
            area.Append("                           <div class ='col-md-12 col-sm-12 col-xs-12' style='background:#8D9BE3;text-align:center' >\n");
            area.Append($"                              <b id=\"{machine}2\"  style=\"color:black\" >展開詳細資料   </b>  <i id=\"{machine}1\"  class=\"fa fa-chevron-circle-down\" style=\"color:black;width:3%;font-size: 1.4em;\"></i>\n");
            area.Append($"                          </div>\n");
            area.Append("                       </a>\n");
            area.Append($"                  </div>\n");
            area.Append($"               </div>\n");
            area.Append("               <div class ='col-md-3 col-sm- col-xs-4'>\n");
            area.Append($"                   <button type=\"button\" class=\"btn btn-info btn-sm ButtonStyle\" style='width:93%;position:absolute; right:1px;text-align:center;background-color:#8D9BE3;color:black' onclick=jump('{WebUtils.UrlStringEncode("machine=" + mach)}') ><b>問題回報</b></button>");
            area.Append($"               </div>\n");
            area.Append($" <div id = \"{machine}\" class=\"panel-collapse collapse  \">\n");
            area.Append("<div class=\"panel-body\">\n");

            // 加工程式 = string_split(加工程式);
            area.Append("               <div class ='col-md-12 col-sm-12 col-xs-12 div_height' >\n");
            area.Append("               </div>\n");
            area.Append("               <div class ='col-md-6 col-sm-6 col-xs-12'>\n");
            area.Append(Image_typesetting("acts", 主軸轉速, "2"));
            area.Append(Image_typesetting("spindlespeed", 主軸速度, "2"));
            area.Append(Image_typesetting("prog_main", 主程式, "2"));
            area.Append(Image_typesetting("program_run", 加工程式, "2"));
            area.Append(Image_typesetting("override", 進給率, "2"));
            //area.Append(Image_typesetting("check_staff", 校機人員, "2"));
            area.Append(Image_typesetting("work_staff", 加工人員, "2"));
            area.Append(Image_typesetting("manu_id", 製令單號, "2"));
            area.Append(Image_typesetting("product_number", 料件編號, "2"));
            area.Append(Image_typesetting("craft_name", 工藝名稱, "2"));
            area.Append("               </div>\n");
            area.Append("               <div class ='col-md-6 col-sm-6 col-xs-12'>\n");
            area.Append(Image_typesetting("spindleload", 主軸負載, "2"));
            area.Append(Image_typesetting("spindletemp", 主軸溫度, "2"));
            area.Append(Image_typesetting("prog_main_cmd", 主程式註解, "2"));
            area.Append(Image_typesetting("prog_run_cmd", 運行程式註解, "2"));
            area.Append(Image_typesetting("run_time", 運轉時間, "2"));
            area.Append(Image_typesetting("poweron_time", 通電時間, "2"));
            area.Append(Image_typesetting("cut_time", 切削時間, "2"));
            area.Append(Image_typesetting("custom_name", 客戶名稱, "2"));
            area.Append(Image_typesetting("product_name", 產品名稱, "2"));
            area.Append("               </div>\n");
            area.Append("               <div class ='col-md-12 col-sm-12 col-xs-12'>\n");
            area.Append(Image_typesetting("alarm_mesg", 異警資訊, "3"));
            area.Append("               </div>\n");
            area.Append("               <div class ='col-md-12 col-sm-12 col-xs-12 div_height' >\n");
            area.Append("               </div>\n");
            area.Append("           </div>\n");
            area.Append("       </div>\n");
            area.Append("           </div>\n");
            area.Append("       </div>\n");
            area.Append("   </div>\n");
            area.Append("</div>\n");
            area.Append("</div>\n");
            //網頁開啟後兩秒,顯示狀態bar
            js_code += "mTimer_status = setTimeout(function () { draw_Axial('" + 設備稼動_長條圖 + "'); }, 2000);\n";

        }
        public StringBuilder 顯示資訊(string Info, string Value, string status = "")
        {
            //因為BAR通常會顯示
            StringBuilder sb = new StringBuilder();
            if (Info == "status_bar")
            {
                sb.Append($"           <div id = 'ma_div_{Value}' class='' >");
                sb.Append($"<canvas id = 'ma_canvas_{Value}' width = '1200' height = '40'></canvas>");
                sb.Append("</div>");
            }
            return sb;
        }
        public StringBuilder Image_typesetting(string Info, string value, string type)
        {
            Info = HtmlUtil.Search_Dictionary(keyValues, Info);
            StringBuilder Image = new StringBuilder();
            //0.設備狀態、1.設備名稱、2.設備稼動率、3.今日生產進度(今日/預計)、4.應完工時間、5.預計完工時間
            if (type == "1")
            {
                Image.Append("<div class ='col-md-7 col-sm-6 col-xs-6' style='background:#EAEAEA;text-align:center;height:34px;font-size:20px;margin-top: 7px; margin-bottom: 7px;'>\n");
                if (Info != "生產進度")
                    Image.Append(Info + "\n");
                else
                    Image.Append("生產進度(目前/預計) \n");
                Image.Append("</div>\n");
                Image.Append("<div class ='col-md-5 col-sm-6 col-xs-6' style='text-align:center;font-size:20px; margin-top: 7px; margin-bottom: 7px;'>\n");
                if (value == "")
                    value = " <br /> ";
                Image.Append($"<span> {value}</span>\n");
                Image.Append("<hr class=\"_hr2\"/>");
                Image.Append("</div>\n");
            }
            //6.主軸轉速、7.轉軸速度、8.主程式、9.運行程式、10.進給率、11.校機人員、12.加工人員、13.製令單號、14.料件編號、15.工藝名稱
            //16.主軸負載、17.主軸溫度、18.主程式註解、19.運行程式註解、20.運轉時間、21.通電時間、22.切削時間、23.客戶名稱、24.產品名稱
            else if (type == "2")
            {
                Image.Append("<div class ='col-md-4 col-sm-6 col-xs-6' style='background:#EAEAEA;text-align:center;font-size:15px; margin-top: 7px; '>\n");
                Image.Append(Info + "\n");
                Image.Append("</div>\n");
                Image.Append("<div class ='col-md-8 col-sm-6 col-xs-6' style='text-align:center;font-size:15px; margin-top: 7px; '>\n");
                if (value == "")
                    value = " <br /> ";
                Image.Append($"<span> {value}</span>\n");
                Image.Append("<hr class=\"_hr2\"/>");
                Image.Append("</div>\n");
            }
            //25.異警資訊
            else if (type == "3")
            {
                Image.Append("<div class ='col-md-2 col-sm-2 col-xs-2' style='background:#EAEAEA;text-align:center;font-size:15px; margin-top: 7px; '>\n");
                Image.Append(Info + "\n");
                Image.Append("</div>\n");
                Image.Append("<div class ='col-md-10 col-sm-10 col-xs-10' style='text-align:center;font-size:15px; margin-top: 7px; '>\n");
                if (value == "")
                    value = " <br /> ";
                Image.Append($"<span> {value}</span>\n");
                Image.Append("<hr class=\"_hr2\"/>");
                Image.Append("</div>\n");
            }
            return Image;
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
                //20220823 select主選項不合理,暫時移除
                //DropDownList_MachType.Items.Add("--Select--");
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
                        //20220823 select主選項不合理,雖然能搜索全場資料但不是正確搜尋參數,暫時拔除
                        // itemname = $"--Select--,1^,";
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
                                            all_mach += get_machineinformation(mach[i], "mach_show_name") + "^";
                                            machlist.Add(get_machineinformation(mach[i], "mach_show_name"));
                                        }
                                    }
                                    itemname += $"{DataTableUtils.toString(rew["group_name"])},1^{all_mach}^,";
                                }
                            }
                            //20220823 select副選項不合理,不是搜尋參數且導致甘特圖錯誤,暫時拔除
                           // itemname = $"--Select--,1^," + itemname;
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
                                        all_mach += get_machineinformation(mach[i], "mach_show_name") + "^";
                                        machlist.Add(get_machineinformation(mach[i], "mach_show_name"));
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
        public void Mach_Info_List(string MachGroup = "")    //畫status_bar使用
        {
            if (ls_data != null)
                ls_data.Clear();
            string condition = acc_power == "" ? " where area_name <> '測試區' " : $" where area_name = '{acc_power}' ";
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            if (MachGroup == "")
            {
                foreach (DataRow row in DataTableUtils.GetDataTable($"select mach_name from machine_info {condition}").Rows)
                    ls_data.Add(row.ItemArray[0].ToString());
            }
            else
            {
                if (MachGroup != "不存在")
                {
                    if (DropDownList_MachType.SelectedItem.Text == "全廠" && (DropDownList_MachGroup.SelectedItem.Text == "全廠設備" || DropDownList_MachGroup.SelectedItem.Text == "--Select--"))
                    {
                        GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                        DataTable ds = DataTableUtils.GetDataTable("select mach_name from machine_info  order by _id desc");
                        if (HtmlUtil.Check_DataTable(ds))
                        {
                            foreach (DataRow dataRow in ds.Rows)
                                ls_data.Add(DataTableUtils.toString(dataRow["mach_name"]));
                        }
                    }
                    else
                        ls_data = DataTableUtils.GetDataTable("select mach_name from mach_group where group_name = '" + MachGroup + "'").Rows[0].ItemArray[0].ToString().Split(',').ToList();
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
                for (int iIndex = 0; iIndex < ls_data.Count; iIndex++)
                {
                    Record_Machine.Add(ls_data[iIndex]);
                    if (iIndex < ls_data.Count - 1) str_Dev_Name += ls_data[iIndex] + ",";
                    else str_Dev_Name += ls_data[iIndex];
                }
            }
        }
        //顯示個人設定的刷新時間
        private string set_time(string acc)
        {
            string second = HtmlUtil.Search_acc_Column(acc, "Set_Time");
            return second == "" ? "60000" : $"{second}000";
        }
        //event   
        protected void Select_MachGroupClick(object sender, EventArgs e)    //執行運算
        {
            //先做checkbox的儲存
            save_column(acc);
            Change_Status();
            show_checkbox();

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
                //20220823 德上頁面重新導向後,甘特圖資料異常,重新搜索正確資料
                show_Gantt();
            }

            else if (DropDownList_MachType.SelectedItem.Text != "--Select--")//&& DropDownList_MachGroup.SelectedItem.Text != "--Select--")
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
                            {
                                Read_Data(DataTableUtils.toString(row["group_name"]));
                                Mach_Info_List(DataTableUtils.toString(row["group_name"]));
                            }
                        }
                        else
                        {
                            Read_Data("不存在");
                            Mach_Info_List("不存在");
                        }
                    }
                    else
                    {
                        Read_Data(DropDownList_MachGroup.SelectedItem.Text);
                        Mach_Info_List(DropDownList_MachGroup.SelectedItem.Text);
                    }
                }
            }
            else
                Response.Redirect("Machine_list_info.aspx");
        }
        private void save_column(string acc)
        {
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            string sqlcmd = $"select * from show_column where Account = '{acc}'";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
            if (HtmlUtil.Check_DataTable(dt))
            {
            }
            else if (dt != null && dt.Rows.Count == 0)
            {
                DataRow rew = dt.NewRow();
                rew["Column_Name"] = "mach_name";
                rew["Account"] = acc;
                rew["Allow"] = "True";
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                DataTableUtils.Insert_DataRow("show_column", rew);
                for (int i = 0; i < CheckBoxList_cloumn.Items.Count; i++)
                {

                    DataRow row = dt.NewRow();
                    row["Column_Name"] = DataTableUtils.toString(CheckBoxList_cloumn.Items[i].Value);
                    row["Account"] = acc;
                    row["Allow"] = CheckBoxList_cloumn.Items[i].Selected;
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                    DataTableUtils.Insert_DataRow("show_column", row);
                }

            }
        }
        //改變checkbox的狀態
        private void Change_Status()
        {
            //先確定有哪些
            for (int i = 0; i < CheckBoxList_cloumn.Items.Count; i++)
            {
                if (CheckBoxList_cloumn.Items[i].Selected == true)
                    list.Add(CheckBoxList_cloumn.Items[i].Value);
            }
            list.Add("mach_name");
            //找到整個DataTable
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            string sqlcmd = $"select* from show_column where Account = '{acc}'";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
            if (HtmlUtil.Check_DataTable(dt))
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow rew = dt.NewRow();
                    rew["id"] = row["id"];
                    if (list.IndexOf(DataTableUtils.toString(row["Column_Name"])) != -1)
                        rew["Allow"] = "True";
                    else
                        rew["Allow"] = "False";
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                    bool ok = DataTableUtils.Update_DataRow("show_column", $" ID={DataTableUtils.toInt(DataTableUtils.toString(row["id"]))}", rew);
                }
            }

        }
        //搜尋下一個工藝，並把他複製過去
        protected void Search_save_Click(object sender, EventArgs e)
        {
            if (WebUtils.GetAppSettings("switch_cnc") == "0")
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", $"<script>alert('{Web_Data.Next_MachInfo(TextBox_Machine.Text, TextBox_Number.Text, DateTime.Now.ToString("yyyyMMddHHmmss"))}');location.href='Machine_list_info.aspx';</script>");
            //人員汰換
            else if (WebUtils.GetAppSettings("switch_cnc") == "1")
            {
                //單純記錄人員，以及替換人員
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                string sqlcmd = $"select * from aps_info,machine_info   where aps_info.mach_name = '{TextBox_MachNext.Text}' and  machine_info.mach_name = aps_info.mach_name";
                DataTable ds = DataTableUtils.GetDataTable(sqlcmd);
                //更新人員
                if (HtmlUtil.Check_DataTable(ds))
                {
                    if (DataTableUtils.toString(ds.Rows[0]["work_staff"]) != TextBox_work.Text)
                    {
                        DataRow row = ds.NewRow();
                        row["_id"] = ds.Rows[0]["_id"].ToString();
                        row["check_staff"] = TextBox_check.Text;
                        row["work_staff"] = TextBox_work.Text;

                        GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                        DataTableUtils.Update_DataRow("aps_info", $"mach_name = '{TextBox_MachNext.Text}'", row);
                    }
                    else
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", $"<script>alert('操作人員已由可視化主控台變更為 \"{DataTableUtils.toString(ds.Rows[0]["work_staff"])}\" ');location.href='Machine_list_info.aspx';</script>");

                }
                if (DataTableUtils.toString(ds.Rows[0]["work_staff"]) != TextBox_work.Text)
                {
                    int id_max = 1;
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                    string sql = "select _id from record_worktime order by _id desc limit 1";
                    DataTable dt_max = DataTableUtils.GetDataTable(sql);

                    if (HtmlUtil.Check_DataTable(dt_max))
                        id_max = DataTableUtils.toInt(DataTableUtils.toString(dt_max.Rows[0]["_id"]));
                    //紀錄
                    bool ok = false;
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                    sqlcmd = $"select * from record_worktime where  start_time='{DataTableUtils.toString(ds.Rows[0]["start_time"])}' and end_time = '{DataTableUtils.toString(ds.Rows[0]["end_time"])}' and mach_name = '{TextBox_MachNext.Text}'  order by NOW_TIME desc,_id desc";
                    DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
                    string ss = DataTableUtils.ErrorMessage;
                    if (HtmlUtil.Check_DataTable(dt))
                    {
                        string status = dt.Rows[0]["workman_status"].ToString();
                        DataRow row = dt.NewRow();
                        //已經有資料的情況->前一筆:出站  後一筆:入站            
                        row["_id"] = id_max + 1;
                        row["mach_name"] = TextBox_MachNext.Text;

                        if (status == "入站")
                        {
                            row["workman_status"] = "出站";
                            row["work_staff"] = ds.Rows[0]["work_staff"].ToString(); //TextBox_workman.Text;
                            row["check_staff"] = ds.Rows[0]["check_staff"].ToString(); //TextBox_workman.Text;
                        }
                        else
                        {
                            row["workman_status"] = "入站";
                            row["work_staff"] = TextBox_work.Text; //TextBox_workman.Text;
                            row["check_staff"] = TextBox_check.Text; //TextBox_workman.Text;
                        }
                        row["now_time"] = DateTime.Now.ToString("yyyyMMddHHmmss");
                        row["report_source"] = "vis";
                        row["add_account"] = acc;

                        row["qty_status"] = "良品";

                        if (HtmlUtil.Check_DataTable(ds))
                        {
                            row["area_name"] = ds.Rows[0]["area_name"];
                            row["report_qty"] = (DataTableUtils.toInt(ds.Rows[0]["product_count_day"].ToString()) - DataTableUtils.toInt(dt.Rows[0]["now_qty"].ToString())).ToString();
                            row["now_qty"] = ds.Rows[0]["product_count_day"].ToString();
                            row["start_time"] = DataTableUtils.toString(ds.Rows[0]["start_time"]);
                            row["end_time"] = DataTableUtils.toString(ds.Rows[0]["end_time"]);
                            row["manu_id"] = DataTableUtils.toString(ds.Rows[0]["manu_id"]);
                            row["product_number"] = DataTableUtils.toString(ds.Rows[0]["product_number"]);
                            row["custom_name"] = DataTableUtils.toString(ds.Rows[0]["custom_name"]);
                            row["product_name"] = DataTableUtils.toString(ds.Rows[0]["product_name"]);
                            row["craft_name"] = DataTableUtils.toString(ds.Rows[0]["craft_name"]);
                        }
                        GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                        ok = DataTableUtils.Insert_DataRow("record_worktime", row);
                        ss = DataTableUtils.ErrorMessage;

                        DataRow rew = dt.NewRow();
                        if (status == "入站")
                        {
                            row["_id"] = id_max + 2;
                            rew["mach_name"] = TextBox_MachNext.Text;
                            rew["workman_status"] = "入站";
                            rew["work_staff"] = TextBox_work.Text; //TextBox_workman.Text;
                            rew["check_staff"] = TextBox_check.Text; //TextBox_workman.Text;
                            rew["now_time"] = DateTime.Now.ToString("yyyyMMddHHmmss");
                            rew["report_source"] = "vis";
                            rew["add_account"] = acc;
                            rew["qty_status"] = "良品";

                            rew["area_name"] = ds.Rows[0]["area_name"];
                            rew["report_qty"] = "0";
                            rew["now_qty"] = ds.Rows[0]["product_count_day"].ToString();
                            rew["start_time"] = DataTableUtils.toString(ds.Rows[0]["start_time"]);
                            rew["end_time"] = DataTableUtils.toString(ds.Rows[0]["end_time"]);
                            rew["manu_id"] = DataTableUtils.toString(ds.Rows[0]["manu_id"]);
                            rew["product_number"] = DataTableUtils.toString(ds.Rows[0]["product_number"]);
                            rew["custom_name"] = DataTableUtils.toString(ds.Rows[0]["custom_name"]);
                            rew["product_name"] = DataTableUtils.toString(ds.Rows[0]["product_name"]);
                            rew["craft_name"] = DataTableUtils.toString(ds.Rows[0]["craft_name"]);
                        }
                        GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                        ok = DataTableUtils.Insert_DataRow("record_worktime", rew);
                        ss = DataTableUtils.ErrorMessage;
                    }
                    //第一次儲存->只有入站情況
                    else if (dt != null)
                    {
                        //該排程第一次儲存
                        GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                        sqlcmd = "select * from record_worktime";
                        dt = DataTableUtils.GetDataTable(sqlcmd);
                        if (dt != null)
                        {
                            DataRow row = dt.NewRow();
                            row["_id"] = id_max + 1;
                            row["mach_name"] = TextBox_MachNext.Text;
                            row["workman_status"] = "入站";
                            row["qty_status"] = "良品";
                            row["work_staff"] = TextBox_work.Text; //TextBox_workman.Text;
                            row["check_staff"] = TextBox_check.Text; //TextBox_workman.Text;
                            row["now_time"] = DateTime.Now.ToString("yyyyMMddHHmmss");
                            row["report_source"] = "vis";
                            row["add_account"] = acc;

                            if (HtmlUtil.Check_DataTable(ds))
                            {
                                row["area_name"] = ds.Rows[0]["area_name"];
                                row["report_qty"] = ds.Rows[0]["product_count_day"].ToString();
                                row["now_qty"] = ds.Rows[0]["product_count_day"].ToString();
                                row["start_time"] = DataTableUtils.toString(ds.Rows[0]["start_time"]);
                                row["end_time"] = DataTableUtils.toString(ds.Rows[0]["end_time"]);
                                row["manu_id"] = DataTableUtils.toString(ds.Rows[0]["manu_id"]);
                                row["product_number"] = DataTableUtils.toString(ds.Rows[0]["product_number"]);
                                row["custom_name"] = DataTableUtils.toString(ds.Rows[0]["custom_name"]);
                                row["product_name"] = DataTableUtils.toString(ds.Rows[0]["product_name"]);
                                row["craft_name"] = DataTableUtils.toString(ds.Rows[0]["craft_name"]);
                            }
                            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                            ok = DataTableUtils.Insert_DataRow("record_worktime", row);
                        }
                    }
                    div_button.Attributes["style"] = "display:none";
                    if (ok)
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", $"<script>alert('儲存成功');location.href='Machine_list_info.aspx';</script>");
                    else if (!ok)
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", $"<script>alert('儲存失敗');location.href='Machine_list_info.aspx';</script>");
                }

            }
            //紀錄工時
            else
            {

                GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                string sqlcmd = $"select * from aps_info,machine_info  where aps_info.mach_name = '{TextBox_MachNext.Text}' and  machine_info.mach_name = aps_info.mach_name";
                DataTable ds = DataTableUtils.GetDataTable(sqlcmd);
                //更新人員
                if (HtmlUtil.Check_DataTable(ds))
                {
                    DataRow row = ds.NewRow();
                    row["work_staff"] = TextBox_work.Text;

                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                    DataTableUtils.Update_DataRow("aps_info", $"mach_name = '{TextBox_MachNext.Text}'", row);
                }

                //紀錄
                bool ok = false;
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                sqlcmd = $"select * from record_worktime where start_time='{DataTableUtils.toString(ds.Rows[0]["start_time"])}' and end_time = '{DataTableUtils.toString(ds.Rows[0]["end_time"])}' and  mach_name = '{TextBox_MachNext.Text}' order by NOW_TIME desc,_id desc";
                DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
                if (HtmlUtil.Check_DataTable(dt))
                {
                    DataRow row = dt.NewRow();
                    row["mach_name"] = TextBox_MachNext.Text;
                    row["work_staff"] = TextBox_work.Text;
                    row["now_time"] = DateTime.Now.ToString("yyyyMMddHHmmss");
                    row["report_source"] = "vis";
                    row["add_account"] = acc;

                    if (HtmlUtil.Check_DataTable(ds))
                    {
                        int history_count = 0;
                        foreach (DataRow rsw in dt.Rows)
                            history_count += DataTableUtils.toInt(rsw["Report_QTY"].ToString());
                        row["area_name"] = ds.Rows[0]["area_name"];
                        row["report_qty"] = (DataTableUtils.toInt(ds.Rows[0]["product_count_day"].ToString()) - history_count).ToString();
                        row["now_qty"] = ds.Rows[0]["product_count_day"].ToString();
                        row["start_time"] = DataTableUtils.toString(ds.Rows[0]["start_time"]);
                        row["end_time"] = DataTableUtils.toString(ds.Rows[0]["end_time"]);
                        row["manu_id"] = DataTableUtils.toString(ds.Rows[0]["manu_id"]);
                        row["product_number"] = DataTableUtils.toString(ds.Rows[0]["product_number"]);
                        row["custom_name"] = DataTableUtils.toString(ds.Rows[0]["custom_name"]);
                        row["product_name"] = DataTableUtils.toString(ds.Rows[0]["product_name"]);
                        row["craft_name"] = DataTableUtils.toString(ds.Rows[0]["craft_name"]);
                    }
                    ok = DataTableUtils.Insert_DataRow("record_worktime", row);
                }
                else if (dt != null)
                {
                    //該排程第一次儲存
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                    sqlcmd = "select * from record_worktime";
                    dt = DataTableUtils.GetDataTable(sqlcmd);
                    if (dt != null)
                    {
                        DataRow row = dt.NewRow();
                        row["mach_name"] = TextBox_MachNext.Text;

                        row["work_staff"] = TextBox_work.Text; //TextBox_workman.Text;
                                                               //       row["check_staff"] = TextBox_check.Text; //TextBox_workman.Text;
                        row["now_time"] = DateTime.Now.ToString("yyyyMMddHHmmss");
                        row["report_source"] = "vis";
                        row["add_account"] = acc;

                        if (HtmlUtil.Check_DataTable(ds))
                        {
                            row["area_name"] = ds.Rows[0]["area_name"];
                            row["report_qty"] = DataTableUtils.toString(ds.Rows[0]["product_count_day"]);
                            row["now_qty"] = ds.Rows[0]["product_count_day"].ToString();
                            row["start_time"] = DataTableUtils.toString(ds.Rows[0]["start_time"]);
                            row["end_time"] = DataTableUtils.toString(ds.Rows[0]["end_time"]);
                            row["manu_id"] = DataTableUtils.toString(ds.Rows[0]["manu_id"]);
                            row["product_number"] = DataTableUtils.toString(ds.Rows[0]["product_number"]);
                            row["custom_name"] = DataTableUtils.toString(ds.Rows[0]["custom_name"]);
                            row["product_name"] = DataTableUtils.toString(ds.Rows[0]["product_name"]);
                            row["craft_name"] = DataTableUtils.toString(ds.Rows[0]["craft_name"]);
                        }
                        ok = DataTableUtils.Insert_DataRow("record_worktime", row);
                    }
                }
            }

        }
        protected void Search_Next_Click(object sender, EventArgs e)
        {
            string now_time = DateTime.Now.ToString("yyyyMMddHHmmss");

            int id_max = 1;
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            string sql = "select max(_id) _id from record_worktime ";
            DataTable dt_max = DataTableUtils.GetDataTable(sql);

            if (HtmlUtil.Check_DataTable(dt_max))
                id_max = DataTableUtils.toInt(DataTableUtils.toString(dt_max.Rows[0]["_id"]));

            if (WebUtils.GetAppSettings("switch_cnc") != "0")
            {
                //寫入當下那筆工單
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                string sqlcmd = $"select * from aps_info,machine_info  where aps_info.mach_name = '{TextBox_MachNext.Text}' and machine_info.mach_name = aps_info.mach_name";
                DataTable ds = DataTableUtils.GetDataTable(sqlcmd);
                bool ok = false;
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                sqlcmd = $"select * from record_worktime where start_time='{DataTableUtils.toString(ds.Rows[0]["start_time"])}' and end_time = '{DataTableUtils.toString(ds.Rows[0]["end_time"])}' and mach_name = '{TextBox_MachNext.Text}'   order by NOW_TIME desc,_id desc";
                DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
                if (HtmlUtil.Check_DataTable(dt))
                {
                    DataRow row = dt.NewRow();
                    row["_id"] = id_max + 1;
                    row["mach_name"] = TextBox_MachNext.Text;
                    row["check_staff"] = TextBox_check.Text; //TextBox_workman.Text;
                    row["workman_status"] = "出站";
                    row["qty_status"] = "良品";
                    row["now_time"] = now_time;
                    row["report_source"] = "vis";
                    row["add_account"] = acc;

                    if (HtmlUtil.Check_DataTable(ds))
                    {
                        row["work_staff"] = ds.Rows[0]["work_staff"].ToString();
                        row["area_name"] = ds.Rows[0]["area_name"].ToString();
                        row["report_qty"] = (DataTableUtils.toInt(ds.Rows[0]["product_count_day"].ToString()) - DataTableUtils.toInt(dt.Rows[0]["now_qty"].ToString())).ToString();
                        row["now_qty"] = ds.Rows[0]["product_count_day"].ToString();
                        row["start_time"] = DataTableUtils.toString(ds.Rows[0]["start_time"]);
                        row["end_time"] = DataTableUtils.toString(ds.Rows[0]["end_time"]);

                        row["manu_id"] = DataTableUtils.toString(ds.Rows[0]["manu_id"]);
                        row["product_number"] = DataTableUtils.toString(ds.Rows[0]["product_number"]);
                        row["custom_name"] = DataTableUtils.toString(ds.Rows[0]["custom_name"]);
                        row["product_name"] = DataTableUtils.toString(ds.Rows[0]["product_name"]);
                        row["craft_name"] = DataTableUtils.toString(ds.Rows[0]["craft_name"]);
                    }
                    ok = DataTableUtils.Insert_DataRow("record_worktime", row);
                }
                else if (dt != null)
                {
                    //該排程第一次儲存
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                    sqlcmd = "select * from record_worktime";
                    dt = DataTableUtils.GetDataTable(sqlcmd);
                    if (dt != null)
                    {
                        DataRow row = dt.NewRow();
                        row["_id"] = id_max + 1;

                        row["mach_name"] = TextBox_MachNext.Text;
                        row["workman_status"] = "出站";

                        row["check_staff"] = TextBox_check.Text; //TextBox_workman.Text;
                        row["qty_status"] = "良品";
                        row["now_time"] = now_time;
                        row["report_source"] = "vis";
                        row["add_account"] = acc;

                        if (HtmlUtil.Check_DataTable(ds))
                        {
                            row["work_staff"] = ds.Rows[0]["work_staff"].ToString();
                            row["area_name"] = ds.Rows[0]["area_name"];
                            row["report_qty"] = ds.Rows[0]["product_count_day"].ToString();
                            row["now_qty"] = ds.Rows[0]["product_count_day"].ToString();
                            row["start_time"] = DataTableUtils.toString(ds.Rows[0]["start_time"]);
                            row["end_time"] = DataTableUtils.toString(ds.Rows[0]["end_time"]);
                            row["manu_id"] = DataTableUtils.toString(ds.Rows[0]["manu_id"]);
                            row["product_number"] = DataTableUtils.toString(ds.Rows[0]["product_number"]);
                            row["custom_name"] = DataTableUtils.toString(ds.Rows[0]["custom_name"]);
                            row["product_name"] = DataTableUtils.toString(ds.Rows[0]["product_name"]);
                            row["craft_name"] = DataTableUtils.toString(ds.Rows[0]["craft_name"]);
                        }
                        ok = DataTableUtils.Insert_DataRow("record_worktime", row);
                    }
                }
                if (WebUtils.GetAppSettings("switch_cnc") == "1")
                {
                    //填入次筆工單狀態
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                    sqlcmd = $"SELECT * FROM aps_list_info,machine_info where aps_list_info.mach_name = '{TextBox_MachNext.Text}' and  CAST(order_number AS double)  > {TextBox_Order.Text} and aps_list_info.mach_name = machine_info.mach_name order by CAST(order_number AS double) asc limit 1";
                    DataTable dt_next = DataTableUtils.GetDataTable(sqlcmd);
                    if (HtmlUtil.Check_DataTable(dt_next) && dt != null)
                    {
                        DataRow row = dt.NewRow();
                        row["_id"] = id_max + 1;
                        row["mach_name"] = TextBox_MachNext.Text;
                        row["work_staff"] = dt_next.Rows[0]["work_staff"].ToString();
                        row["check_staff"] = dt_next.Rows[0]["check_staff"].ToString();
                        row["workman_status"] = "入站";
                        row["report_qty"] = "0";
                        row["now_qty"] = "0";
                        row["qty_status"] = "良品";
                        row["now_time"] = now_time;
                        row["start_time"] = now_time;
                        row["end_time"] =  dt_next.Rows[0]["end_time"].ToString();
                        row["area_name"] = dt_next.Rows[0]["area_name"].ToString();
                        row["add_account"] = acc;
                        row["report_source"] = "vis";
                        row["manu_id"] = DataTableUtils.toString(dt_next.Rows[0]["manu_id"]);
                        row["product_number"] = DataTableUtils.toString(dt_next.Rows[0]["product_number"]);
                        row["custom_name"] = DataTableUtils.toString(dt_next.Rows[0]["custom_name"]);
                        row["product_name"] = DataTableUtils.toString(dt_next.Rows[0]["product_name"]);
                        row["craft_name"] = DataTableUtils.toString(dt_next.Rows[0]["craft_name"]);
                        GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                        ok = DataTableUtils.Insert_DataRow("record_worktime", row);
                    }
                }
            }
            div_button.Attributes["style"] = "display:none";
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", $"<script>alert('{Web_Data.Next_MachInfo(TextBox_MachNext.Text, TextBox_Order.Text, now_time)}');location.href='Machine_list_info.aspx';</script>");
        }
        //顯示checkbox
        private void show_checkbox()
        {
            Calculation_td = new StringBuilder();
            list.Clear();
            CheckBoxList_cloumn.Items.Clear();
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            string sqlcmd = $"select * from realtime_info,show_column  where show_check = 'Y' and info_name <> 'mach_name' and Account = '{acc}' and show_column.Column_Name = realtime_info.info_name";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
            //已經存在過的
            if (HtmlUtil.Check_DataTable(dt))
            {
                ListItem item = new ListItem();

                foreach (DataRow row in dt.Rows)
                {
                    item = new ListItem(DataTableUtils.toString(row["info_chinese"]), DataTableUtils.toString(row["info_name"]));
                    item.Selected = Boolean.Parse(DataTableUtils.toString(row["Allow"]).ToLower());
                    CheckBoxList_cloumn.Items.Add(item);
                }
            }
            //沒有存在過的
            else if (dt != null && dt.Rows.Count == 0)
            {
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                sqlcmd = "select * from realtime_info  where show_check = 'Y' and info_name <> 'mach_name'";
                dt = DataTableUtils.GetDataTable(sqlcmd);

                if (HtmlUtil.Check_DataTable(dt))
                {
                    ListItem item = new ListItem();

                    foreach (DataRow row in dt.Rows)
                    {
                        item = new ListItem(DataTableUtils.toString(row["info_chinese"]), DataTableUtils.toString(row["info_name"]));
                        item.Selected = Boolean.Parse(DataTableUtils.toString(row["show_status"]).ToLower());
                        CheckBoxList_cloumn.Items.Add(item);
                    }
                }

            }

            for (int i = 0; i < CheckBoxList_cloumn.Items.Count; i++)
            {
                if (CheckBoxList_cloumn.Items[i].Selected == true)
                    list.Add(CheckBoxList_cloumn.Items[i].Value);
            }
            list.Add("mach_name");
            list.Add("status_bar");
            // show_tdData(list);
        }
        ////顯示甘特圖
        private void show_Gantt()
        {
            string factory = acc_power == "" ?"": $"  where area_name = '{acc_power}' ";    
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            string sqlcmd = $"select mach_name from mach_group {factory}";
            DataTable data = DataTableUtils.GetDataTable(sqlcmd);
            factory = "";
            foreach (DataRow row in data.Rows)
                factory += DataTableUtils.toString(row["mach_name"]) + ",";
            List<string> mach_name = new List<string>(factory.Split(','));
            List<string> mach_show = new List<string>();
            string condition = "";
            mach_show = DropDownList_MachGroup.Items.Count > 0 ? new List<string>(DropDownList_MachGroup.SelectedItem.Value.Split('^')) : new List<string>();

            if (mach_show.Count > 0)
            {
                for (int i = 0; i < mach_show.Count; i++)
                    condition += condition == "" ? $"  machine_info.mach_show_name = '{mach_show[i]}' " : $" OR machine_info.mach_show_name = '{mach_show[i]}' ";
                condition = condition != "" ? $" and ( {condition} ) " : "";
            }

            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            sqlcmd = $" SELECT  * FROM aps_info_dispatch, (SELECT  MAX(_id) A_id, list_id Alist_id FROM aps_info_dispatch WHERE list_id IS NOT NULL GROUP BY list_id) AS a, (SELECT  mach_name B_machname, mach_show_name FROM machine_info) AS b,machine_info WHERE LENGTH(start_time) = 14 AND LENGTH(end_time) = 14  {condition}  AND a.A_id = aps_info_dispatch._id AND aps_info_dispatch.list_id = a.Alist_id AND b.B_machname = aps_info_dispatch.mach_name and aps_info_dispatch.mach_name = machine_info.mach_name and machine_info.area_name <> '測試區' ORDER BY machine_info.area_name desc ,machine_info.mach_name,aps_info_dispatch.end_time asc";
            DataTable dt_派工 = DataTableUtils.GetDataTable(sqlcmd);

            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            sqlcmd = $" SELECT  * FROM aps_info_report, (SELECT  MAX(_id) A_id, list_id Alist_id FROM aps_info_report WHERE list_id IS NOT NULL GROUP BY list_id) AS a, (SELECT  mach_name B_machname, mach_show_name FROM machine_info) AS b, machine_info WHERE LENGTH(start_time) = 14 AND LENGTH(end_time) = 14  {condition} AND a.A_id = aps_info_report._id AND aps_info_report.list_id = a.Alist_id AND b.B_machname = aps_info_report.mach_name AND aps_info_report.mach_name = machine_info.mach_name AND machine_info.area_name <> '測試區' ORDER BY machine_info.area_name DESC , machine_info.mach_name,aps_info_report.end_time asc";
            DataTable dt_報工 = DataTableUtils.GetDataTable(sqlcmd);

            if (HtmlUtil.Check_DataTable(dt_派工))
            {
                DataTable dt_mach = dt_派工.DefaultView.ToTable(true, new string[] { "mach_name" });

                foreach (DataRow row in dt_mach.Rows)
                {
                    if (mach_name.IndexOf(DataTableUtils.toString(row["mach_name"])) != -1)
                    {
                        DataTable dt_copy_派工 = new DataTable();
                        if (HtmlUtil.Check_DataTable(dt_派工))
                        {
                            dt_copy_派工 = dt_派工.Clone();
                            sqlcmd = $"mach_name = '{DataTableUtils.toString(row["mach_name"])}'";
                            DataRow[] rows = dt_派工.Select(sqlcmd);
                            for (int i = 0; i < rows.Length; i++)
                                dt_copy_派工.ImportRow(rows[i]);
                        }

                        DataTable dt_copy_報工 = new DataTable();
                        if (HtmlUtil.Check_DataTable(dt_報工))
                        {
                            dt_copy_報工 = dt_報工.Clone();
                            sqlcmd = $"mach_name = '{DataTableUtils.toString(row["mach_name"])}'";
                            DataRow[] rows = dt_報工.Select(sqlcmd);
                            for (int i = 0; i < rows.Length; i++)
                                dt_copy_報工.ImportRow(rows[i]);
                        }
                        Gantt_Data += Get_GanttData(dt_copy_派工, CNCUtils.MachName_translation(DataTableUtils.toString(row["mach_name"])), dt_copy_報工);
                    }
                }
            }
        }
        private string Get_GanttData(DataTable dt, string machine, DataTable dt_exception)
        {
            List<string> record_color = new List<string>();
            List<string> record_id = new List<string>();
            int i = 0;
            string gantt_value = "";
            gantt_value += "{";
            gantt_value += $"name:'{machine}',";
            gantt_value += "desc:'',";
            gantt_value += "values:[";

            i = 0;
            foreach (DataRow row in dt.Rows)
            {
                if (record_color.IndexOf(row["list_id"].ToString()) == -1)
                {
                    gantt_value += "{ " +
                $"from:'/Date({HtmlUtil.GetTimeStamp(HtmlUtil.StrToDate(DataTableUtils.toString(row["start_time"])))})/', " +
                $"  to:'/Date({HtmlUtil.GetTimeStamp(HtmlUtil.StrToDate(DataTableUtils.toString(row["end_time"])))})/',      " +
                $"  customClass: \"{Enum.GetName(typeof(classcolor), i % 5)}\"," +
                $"  desc: \"<b>設備名稱：{machine}{add_listid(row)}</b> <br> " +
                $"          <b>校機人員：{DataTableUtils.toString(row["check_staff"]).Replace(@"""", "")}</b> <br>" +
                $"          <b>加工人員：{DataTableUtils.toString(row["work_staff"]).Replace(@"""", "")}</b> <br>" +
                $"          <b>客戶名稱：{DataTableUtils.toString(row["custom_name"]).Replace(@"""", "")}</b> <br>" +
                $"          <b>製令單號：{DataTableUtils.toString(row["manu_id"]).Replace(@"""", "")}</b> <br>" +
                $"          <b>產品名稱：{DataTableUtils.toString(row["product_name"]).Replace(@"""", "")}</b> <br> " +
                $"          <b>料件編號：{DataTableUtils.toString(row["product_number"]).Replace(@"""", "")}</b> <br>" +
                $"          <b>工藝名稱：{DataTableUtils.toString(row["craft_name"]).Replace(@"""", "")}</b> <br>" +
                $"          <b>預計工單生產數量：{DataTableUtils.toString(row["exp_product_count_day"]).Replace(@"""", "")}</b> <br>" +
                $"          <b>開始時間：{HtmlUtil.StrToDate(DataTableUtils.toString(row["start_time"]))}</b> <br>" +
                $"          <b>結束時間：{HtmlUtil.StrToDate(DataTableUtils.toString(row["end_time"]))}</b> <br>\" " +

                "},";

                    record_color.Add(row["list_id"].ToString());
                    record_color.Add(Enum.GetName(typeof(classcolor), i % 5));
                    i++;
                }

            }

            foreach (DataRow row in dt_exception.Rows)
            {
                if (record_color.IndexOf(row["list_id"].ToString()) != -1 && record_id.IndexOf(row["list_id"].ToString()) == -1)
                {
                    gantt_value += "{ " +
               $"from:'/Date({HtmlUtil.GetTimeStamp(HtmlUtil.StrToDate(DataTableUtils.toString(row["start_time"])))})/', " +
               $"  to:'/Date({HtmlUtil.GetTimeStamp(HtmlUtil.StrToDate(DataTableUtils.toString(row["end_time"])))})/',      " +
               $"  customClass: \"{ check_color(record_color, row["list_id"].ToString(), Enum.GetName(typeof(classcolor), i % 5))}Dark\"," +
               $"  desc: \"<b>設備名稱：{machine}{add_listid(row)}</b> <br> " +
               $"          <b>校機人員：{DataTableUtils.toString(row["check_staff"]).Replace(@"""", "")}</b> <br>" +
               $"          <b>加工人員：{DataTableUtils.toString(row["work_staff"]).Replace(@"""", "")}</b> <br>" +
               $"          <b>客戶名稱：{DataTableUtils.toString(row["custom_name"]).Replace(@"""", "")}</b> <br>" +
               $"          <b>製令單號：{DataTableUtils.toString(row["manu_id"]).Replace(@"""", "")}</b> <br>" +
               $"          <b>產品名稱：{DataTableUtils.toString(row["product_name"]).Replace(@"""", "")}</b> <br> " +
               $"          <b>料件編號：{DataTableUtils.toString(row["product_number"]).Replace(@"""", "")}</b> <br>" +
               $"          <b>工藝名稱：{DataTableUtils.toString(row["craft_name"]).Replace(@"""", "")}</b> <br>" +
               $"          <b>工單生產數量：{DataTableUtils.toString(row["product_count_day"]).Replace(@"""", "")}</b> <br>" +
               $"          <b>開始時間：{HtmlUtil.StrToDate(DataTableUtils.toString(row["start_time"]))}</b> <br>" +
               $"          <b>結束時間：{HtmlUtil.StrToDate(DataTableUtils.toString(row["end_time"]))}</b> <br>\" " +

               "},";
                    record_id.Add(row["list_id"].ToString());
                }

            }

            gantt_value += "]";
            gantt_value += "},";


            return gantt_value;
        }
        private string check_color(List<string> list, string id, string now_color)
        {
            if (list == null)
                return now_color;
            else if (list != null && list.IndexOf(id) == -1)
                return now_color;
            else if (list != null && list.IndexOf(id) != -1)
                return list[list.IndexOf(id) + 1];
            else
                return now_color;
        }
        private string get_machineinformation(string mach, string column)
        {
            if (mach != "")
            {
                string sqlcmd = $"mach_name='{mach}'";
                DataRow[] row = dt_machurl.Select(sqlcmd);
                if (row != null && row.Length > 0)
                    mach = DataTableUtils.toString(row[0][column]);
                }
            return mach;
        }
        private string add_listid(DataRow row)
        {
            return WebUtils.GetAppSettings("show_id") == "1" ? $"~~~{row["list_id"]}" : "";
        }

    }
}