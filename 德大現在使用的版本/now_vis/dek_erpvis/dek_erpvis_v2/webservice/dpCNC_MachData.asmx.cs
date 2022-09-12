using dek_erpvis_v2.cls;
using MongoDB.Driver.Builders;
using Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Services;
using System.Xml;
using System.Globalization;
using System.Net;

namespace dek_erpvis_v2.webservice
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
    [System.Web.Script.Services.ScriptService]
    public class dpCNC_MachData : System.Web.Services.WebService
    {
        CNCUtils cNC_Class = new CNCUtils();
        CNC_Web_Data Web_Data = new CNC_Web_Data();

        //得到機台資料
        [WebMethod]
        public XmlNode GetMachineData(string acc, string machine)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string acc_power = HtmlUtil.Search_acc_Column(acc, "Belong_Factory");
            string condition = "";
            if (acc_power == "" || acc_power == "All" || acc_power == "全部" || acc_power == "全廠")
                condition = "  where area_name <> '測試區' ";
            else
                condition = $" where area_name = '{acc_power}' ";
            string machine_sqlcmd = "";

            if (machine != "")
            {
                List<string> machine_list = new List<string>(WebUtils.UrlStringDecode(machine).Split(','));
                for (int i = 0; i < machine_list.Count - 1; i++)
                {
                    if (i == 0)
                        machine_sqlcmd += $" mach_name = '{machine_list[i]}' ";
                    else
                        machine_sqlcmd += $" OR mach_name = '{machine_list[i]}' ";
                }
                if (condition == "")
                    machine_sqlcmd = $" where {machine_sqlcmd} ";
                else
                    machine_sqlcmd = $" and ({machine_sqlcmd}) ";
            }


            DataTable dt_data = null;
            List<string> ls_data = new List<string>();
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            dt_data = DataTableUtils.GetDataTable($"select mach_name from machine_info {condition} {machine_sqlcmd} ");
            if (HtmlUtil.Check_DataTable(dt_data))
            {
                foreach (DataRow row in dt_data.Rows)
                    ls_data.Add(row.ItemArray[0].ToString());
            }


            XmlDocument xmlDoc = new XmlDocument();

            if (ls_data.Count > 0)
            {
                XmlElement xmlElem = xmlDoc.CreateElement("ROOT_MACH");
                //--------------------------------------
                if (ls_data != null)
                {
                    if (ls_data.Count <= 0)
                        xmlElem.SetAttribute("count", "0");
                    else
                    {
                        xmlElem.SetAttribute("count", DataTableUtils.toString(ls_data.Count));
                        xmlDoc.AppendChild(xmlElem);
                        for (int iIndex = 0; iIndex < ls_data.Count; iIndex++)
                        {
                            XmlElement xmlElemA = xmlDoc.CreateElement("Group");
                            dt_data = Web_Data.Get_MachInfo(ls_data[iIndex]);
                            if (dt_data != null)
                            {
                                string CheckStaff, WorkStaff, MachName, CustomName, ManuId, ProductName, ProductNumber, CraftName, CountTotal, CountToday, ExpCountToday, CountTodayRate, FinishTime, OperRate, MachStatus, AlarmMesg, ProgramRun;
                                CheckStaff = Web_Data.Get_CheckStaff(dt_data);
                                WorkStaff = Web_Data.Get_WorkStaff(dt_data);
                                MachName = Web_Data.Get_MachName(dt_data, ls_data[iIndex]);
                                CustomName = Web_Data.Get_CustomName(dt_data);
                                ManuId = Web_Data.Get_ManuID(dt_data);
                                ProductName = Web_Data.Get_ProductName(dt_data);
                                ProductNumber = Web_Data.Get_ProductNumber(dt_data);
                                CraftName = Web_Data.Get_CraftName(dt_data);
                                CountTotal = Web_Data.Get_CountTotal(dt_data);
                                CountToday = Web_Data.Get_CountToday(dt_data);
                                ExpCountToday = Web_Data.Get_ExpCountToday(dt_data);
                                CountTodayRate = Web_Data.Get_CountTodayRate(dt_data);
                                FinishTime = Web_Data.Get_FinishTime(dt_data);
                                OperRate = Web_Data.Get_Operate_Rate(dt_data, MachName);
                                MachStatus = Web_Data.Get_MachStatus(dt_data, ls_data[iIndex]);
                                AlarmMesg = Web_Data.Get_AlarmMesg(dt_data);
                                ProgramRun = Web_Data.Get_ProgramRun(dt_data, ls_data[iIndex]);
                                
                                //20201201新增
                                string acts = Web_Data.Get_Information(dt_data, "acts");//主軸轉速
                                string spindleload = Web_Data.Get_Information(dt_data, "spindleload");//主軸負載
                                string spindlespeed = Web_Data.Get_Information(dt_data, "spindlespeed");//主軸速度
                                string spindletemp = Web_Data.Get_Information(dt_data, "spindletemp");//主軸溫度
                                string prog_main = Web_Data.Get_Information(dt_data, "prog_main"); ;//主程式
                                string prog_main_cmd = Web_Data.Get_Information(dt_data, "prog_main_cmd");//主程式註解
                                string prog_run_cmd = Web_Data.Get_Information(dt_data, "prog_run_cmd");//運行程式註解
                                string overrides = Web_Data.Get_Information(dt_data, "override");//進給率
                                string run_time = Web_Data.Get_Information(dt_data, "run_time");//運轉時間
                                string cut_time = Web_Data.Get_Information(dt_data, "cut_time");//切削時間
                                string poweron_time = Web_Data.Get_Information(dt_data, "poweron_time");//通電時間
                                //20220622新增
                                string spindle_shock = Web_Data.Get_Information(dt_data, "spindle_shock");//主軸震動
                                string spindle_side_temp = Web_Data.Get_Information(dt_data, "spindle_side_temp");//主軸震動
                                string daoku_motor_electric = Web_Data.Get_Information(dt_data, "daoku_motor_electric");//刀庫馬達電流_損壞預警
                                string spindle_koudao_shock = Web_Data.Get_Information(dt_data, "spindle_koudao_shock");//主軸扣刀_震動監視
                                string spindle_position = Web_Data.Get_Information(dt_data, "spindle_position");//主軸定位_在位確認
                                string spindle_ladao_position = Web_Data.Get_Information(dt_data, "spindle_ladao_position");//主軸拉刀_在位確認
                                string oil_level = Web_Data.Get_Information(dt_data, "oil_level");//潤滑油_油位檢知
                                string ball_screw_hightemp = Web_Data.Get_Information(dt_data, "ball_screw_hightemp");//ball_screw_高溫監視
                                string tool_oil_temp = Web_Data.Get_Information(dt_data, "tool_oil_temp");//治具油壓_溫度監視
                                string tool_oil_pressure = Web_Data.Get_Information(dt_data, "tool_oil_pressure");//治具油壓_壓力監視
                                string qiexieye_concentration = Web_Data.Get_Information(dt_data, "qiexieye_concentration");//切屑液_濃度檢知
                                string qiexieye_temp = Web_Data.Get_Information(dt_data, "qiexieye_temp");//切屑液_溫度檢知
                                string air_pressure = Web_Data.Get_Information(dt_data, "air_pressure");//氣壓源_壓力檢知

                                //20220912新增
                                string Spindle_temp = Web_Data.Get_Information(dt_data, "Spindle_temp");//主軸溫升
                                string Tank_Hydraulic_Temp = Web_Data.Get_Information(dt_data, "Tank_Hydraulic_Temp");//油箱油溫
                                string MainClamp_Hydraulic_Temp = Web_Data.Get_Information(dt_data, "MainClamp_Hydraulic_Temp");//主夾迴路油溫
                                string AngularPositioning_Hydraulic_Temp = Web_Data.Get_Information(dt_data, "AngularPositioning_Hydraulic_Temp");//角向定位迴路油溫
                                string Brake_Hydraulic_Temp_4th = Web_Data.Get_Information(dt_data, "4th_Brake_Hydraulic_Temp");//剎車迴路油溫
                                string Tank_Hydraulic_Pressure = Web_Data.Get_Information(dt_data, "Tank_Hydraulic_Pressure");//主軸溫升
                                string MainClamp_Hydraulic_Pressure = Web_Data.Get_Information(dt_data, "MainClamp_Hydraulic_Pressure");//主軸溫升
                                string AngularPositioning_Hydraulic_Pressure = Web_Data.Get_Information(dt_data, "AngularPositioning_Hydraulic_Pressure");//角向定位迴路油壓
                                string Brake_Hydraulic_Pressure_4th = Web_Data.Get_Information(dt_data, "4th_Brake_Hydraulic_Pressure");//剎車迴路油壓
                                string Coolant_Concentration = Web_Data.Get_Information(dt_data, "Coolant_Concentration");//切削液濃度
                                string Coolant_Temp = Web_Data.Get_Information(dt_data, "Coolant_Temp");//切削液溫度
                                string Air_Temp = Web_Data.Get_Information(dt_data, "Air_Temp");//空壓源溫度

                                //20210105新增
                                string complete_time = Web_Data.Get_Information(dt_data, "complete_time");//通電時間

                                //20210111新增
                                string order_number = Web_Data.Get_Information(dt_data, "order_number");//order_num

                                //計算後面還有多少排程
                                GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                                string sqlcmd = $"select * from aps_list_info where mach_name = '{MachName}' and CAST(order_number AS UNSIGNED)  > {order_number} ";
                                DataTable ds = DataTableUtils.GetDataTable(sqlcmd);
                                string count = "0";
                                if (HtmlUtil.Check_DataTable(ds))
                                    count = "" + ds.Rows.Count;
                                //計算目前時間與當前排程開始時間
                                double Date_Now = DataTableUtils.toDouble(DateTime.Now.ToString("yyyyMMddHHmmss"));
                                double Date_Start = DataTableUtils.toDouble(dt_data.Rows[0]["start_time"].ToString());
                                string can_next = "";
                                if ((Date_Now - Date_Start) >= 0)
                                    can_next = "can";

                                string now_detailstatus = "";
                                if (HtmlUtil.Check_DataTable(dt_data))
                                {

                                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                                    sqlcmd = $"select * from record_worktime where WORK_MACHINE = '{MachName}' and start_time = '{dt_data.Rows[0]["start_time"]}' and end_time = '{dt_data.Rows[0]["end_time"]}' order by NOW_TIME desc limit 1";
                                    DataTable dt_status = DataTableUtils.GetDataTable(sqlcmd);

                                    if (HtmlUtil.Check_DataTable(dt_status))
                                        now_detailstatus = dt_status.Rows[0]["WORKMAN_STATUS"].ToString();
                                }



                                xmlElemA.SetAttribute("Dev_Name", MachName);
                                xmlElemA.SetAttribute("checkMachStaff", CheckStaff);
                                xmlElemA.SetAttribute("prodCustomName", CustomName);
                                xmlElemA.SetAttribute("prodNo", ProductNumber);
                                xmlElemA.SetAttribute("curParts", CountTotal);//總件數
                                xmlElemA.SetAttribute("prod_count", CountToday);//今日生產件數
                                xmlElemA.SetAttribute("tarParts", ExpCountToday);//預計生產件數
                                xmlElemA.SetAttribute("partsRate", CountTodayRate);
                                xmlElemA.SetAttribute("operRate", OperRate);
                                xmlElemA.SetAttribute("alarmMesg", AlarmMesg);
                                xmlElemA.SetAttribute("status", MachStatus);
                                xmlElemA.SetAttribute("workStaff", WorkStaff);
                                xmlElemA.SetAttribute("craftName", CraftName);
                                xmlElemA.SetAttribute("prodName", ProductName);
                                xmlElemA.SetAttribute("progRun", ProgramRun);
                                xmlElemA.SetAttribute("partsTime", FinishTime);
                                xmlElemA.SetAttribute("manuId", ManuId);

                                xmlElemA.SetAttribute("acts", acts);
                                xmlElemA.SetAttribute("spindleload", spindleload);
                                xmlElemA.SetAttribute("spindlespeed", spindlespeed);
                                xmlElemA.SetAttribute("spindletemp", spindletemp);
                                xmlElemA.SetAttribute("prog_main", prog_main);
                                xmlElemA.SetAttribute("prog_main_cmd", prog_main_cmd);
                                xmlElemA.SetAttribute("prog_run_cmd", prog_run_cmd);
                                xmlElemA.SetAttribute("override", overrides);
                                xmlElemA.SetAttribute("run_time", run_time);
                                xmlElemA.SetAttribute("cut_time", cut_time);
                                xmlElemA.SetAttribute("poweron_time", poweron_time);
                                xmlElemA.SetAttribute("complete_time", complete_time);
                                xmlElemA.SetAttribute("order_number", order_number.Trim());
                                xmlElemA.SetAttribute("count", count);
                                xmlElemA.SetAttribute("can_next", can_next);
                                xmlElemA.SetAttribute("now_detailstatus", now_detailstatus);
                                //2022新增選項
                                xmlElemA.SetAttribute("spindle_shock", spindle_shock);
                                xmlElemA.SetAttribute("spindle_side_temp", spindle_side_temp);
                                xmlElemA.SetAttribute("daoku_motor_electric", daoku_motor_electric);
                                xmlElemA.SetAttribute("spindle_koudao_shock", spindle_koudao_shock);
                                xmlElemA.SetAttribute("spindle_position", spindle_position);
                                xmlElemA.SetAttribute("spindle_ladao_position", spindle_ladao_position);
                                xmlElemA.SetAttribute("oil_level", oil_level);
                                xmlElemA.SetAttribute("ball_screw_hightemp", ball_screw_hightemp);
                                xmlElemA.SetAttribute("tool_oil_temp", tool_oil_temp);
                                xmlElemA.SetAttribute("tool_oil_pressure", tool_oil_pressure);
                                xmlElemA.SetAttribute("qiexieye_concentration", qiexieye_concentration);
                                xmlElemA.SetAttribute("qiexieye_temp", qiexieye_temp);
                                xmlElemA.SetAttribute("air_pressure", air_pressure);

                                xmlElemA.SetAttribute("Spindle_temp", Spindle_temp);
                                xmlElemA.SetAttribute("Tank_Hydraulic_Temp", Tank_Hydraulic_Temp);
                                xmlElemA.SetAttribute("MainClamp_Hydraulic_Temp", MainClamp_Hydraulic_Temp);
                                xmlElemA.SetAttribute("AngularPositioning_Hydraulic_Temp", AngularPositioning_Hydraulic_Temp);
                                xmlElemA.SetAttribute("Brake_Hydraulic_Temp_4th", Brake_Hydraulic_Temp_4th);
                                xmlElemA.SetAttribute("Tank_Hydraulic_Pressure", Tank_Hydraulic_Pressure);
                                xmlElemA.SetAttribute("MainClamp_Hydraulic_Pressure", MainClamp_Hydraulic_Pressure);
                                xmlElemA.SetAttribute("AngularPositioning_Hydraulic_Pressure", AngularPositioning_Hydraulic_Pressure);
                                xmlElemA.SetAttribute("Brake_Hydraulic_Pressure_4th", Brake_Hydraulic_Pressure_4th);
                                xmlElemA.SetAttribute("Coolant_Concentration", Coolant_Concentration);
                                xmlElemA.SetAttribute("Coolant_Temp", Coolant_Temp);
                                xmlElemA.SetAttribute("Air_Temp", Air_Temp);


                                string now_list = $"{CNCUtils.MachName_translation(MachName)}^{WorkStaff}^{ManuId}^{CustomName}^{ProductName}^{ProductNumber}^{CraftName}^{DataTableUtils.toInt(CountToday)}^{DataTableUtils.toInt(ExpCountToday)}^";
                                xmlElemA.SetAttribute("now_list", now_list.Replace(' ', '*'));

                                //次筆排程資料
                                GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                                sqlcmd = $"select * from aps_list_info where mach_name = '{MachName}' and CAST(order_number AS double)  > {order_number}  order by CAST(order_number AS double) asc limit 1 ";
                                DataTable Next_DataTable = DataTableUtils.GetDataTable(sqlcmd);

                                string List_next = "";
                                if (HtmlUtil.Check_DataTable(Next_DataTable))
                                    List_next = $"{CNCUtils.MachName_translation(MachName)}^{Next_DataTable.Rows[0]["work_staff"]}^{Next_DataTable.Rows[0]["manu_id"]}^{Next_DataTable.Rows[0]["custom_name"]}^{Next_DataTable.Rows[0]["product_name"]}^{Next_DataTable.Rows[0]["product_number"]}^{Next_DataTable.Rows[0]["craft_name"]}^{"0"}^{Next_DataTable.Rows[0]["exp_product_count_day"]}^";

                                xmlElemA.SetAttribute("next_list", List_next.Replace(' ', '*'));


                                string staff = "";
                                GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                                sqlcmd = $"select staff_name from staff_info left join machine_info on machine_info.area_name = staff_info.area_name where machine_info.mach_name = '{MachName}'";
                                DataTable dt_staff = DataTableUtils.GetDataTable(sqlcmd);
                                if (HtmlUtil.Check_DataTable(dt_staff))
                                {
                                    foreach (DataRow row in dt_staff.Rows)
                                        staff += $"{row["staff_name"]}^";
                                }
                                xmlElemA.SetAttribute("staff", staff.Replace(' ', '*'));
                            }
                            xmlDoc.DocumentElement.AppendChild(xmlElemA);
                        }
                        return xmlDoc.DocumentElement;
                    }
                }
                else
                    xmlElem.SetAttribute("Value", "-1");
                //------------------------------------------------------------------------
            }
            return xmlDoc.DocumentElement;
        }
        //得到整天的狀態BAR
        [WebMethod]
        public XmlNode GetMachStatus(string Mach_ID, string First_Day, string Last_Day)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            XmlDocument xmlDoc = new XmlDocument();
            DateTime FirstDay = Convert.ToDateTime(First_Day);
            DateTime LastDay = Convert.ToDateTime(Last_Day);
            LastDay = Web_Data.EndTime(LastDay);

            List<string> status_web = new List<string>();
            List<string> ST_Data_1 = new List<string>();
            List<string> ST_Data_2 = new List<string>();
            List<string> ST_Data_3 = new List<string>();
            string Update_time_date, Start_time_min, Cycle_time_min, Status, Start_time_line, End_time_line;
            DateTime Start_time, End_time;
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            string sqlcmd = $"select * from status_history_info where '{FirstDay:yyyyMMddHHmmss}' >= update_time and enddate_time >= '{FirstDay:yyyyMMddHHmmss}' and enddate_time <= '{LastDay:yyyyMMddHHmmss}' and mach_name = '{Mach_ID}'  ";
            DataTable dt1 = DataTableUtils.GetDataTable(sqlcmd);
            if (dt1 != null && dt1.Rows.Count != 0)
            {
                End_time = DateTime.ParseExact(dt1.Rows[0]["enddate_time"].ToString(), "yyyyMMddHHmmss.f", null, DateTimeStyles.AllowWhiteSpaces);
                Cycle_time_min = "Cycle_time=" + Math.Round(End_time.Subtract(FirstDay).Duration().TotalMinutes, 2, MidpointRounding.AwayFromZero).ToString();
                Status = "Nc_Status=" + dt1.Rows[0]["status"].ToString();

                Update_time_date = "Update_time=" + FirstDay.ToString("yyyyMMddHHmmss");
                Start_time_min = "Start_time=0";
                Start_time_line = "Start_time_line=" + FirstDay.ToString("MMddHHmmss");
                End_time_line = "End_time_line=" + End_time.ToString("MMddHHmmss");

                ST_Data_1.Add($"{Update_time_date},{Start_time_min},{Cycle_time_min},{Status},{Start_time_line},{End_time_line}");
            }
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            sqlcmd = $"select * from status_history_info where  '{FirstDay:yyyyMMddHHmmss}' <= update_time and enddate_time <= '{LastDay:yyyyMMddHHmmss}' and mach_name = '{Mach_ID}' ";
            DataTable dt2 = DataTableUtils.GetDataTable(sqlcmd);
            ST_Data_2 = cNC_Class.Status_Bar_Info(dt2, FirstDay);
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            sqlcmd = $"select * from status_currently_info where mach_name = '{Mach_ID}'";
            DataTable dt3 = DataTableUtils.GetDataTable(sqlcmd);
            if (dt3 != null && dt3.Rows.Count != 0)
            {
                Start_time = DateTime.ParseExact(dt3.Rows[0]["update_time"].ToString(), "yyyyMMddHHmmss.f", null, DateTimeStyles.AllowWhiteSpaces);

                if (Start_time > FirstDay)
                {
                    Update_time_date = "Update_time=" + dt3.Rows[0]["update_time"].ToString().Split('.')[0];
                    Start_time_min = "Start_time=" + Math.Round(Start_time.Subtract(FirstDay).Duration().TotalMinutes, 2, MidpointRounding.AwayFromZero).ToString();
                    Cycle_time_min = "Cycle_time=" + Math.Round(LastDay.Subtract(Start_time).Duration().TotalMinutes, 2, MidpointRounding.AwayFromZero).ToString();
                    Start_time_line = "Start_time_line=" + dt3.Rows[0]["update_time"].ToString().Substring(4, 10);
                }
                else
                {
                    Update_time_date = "Update_time=" + FirstDay.ToString("yyyyMMddHHmmss");
                    Start_time_min = "Start_time=0";
                    Cycle_time_min = "Cycle_time=" + Math.Round(LastDay.Subtract(FirstDay).Duration().TotalMinutes, 2, MidpointRounding.AwayFromZero).ToString();
                    Start_time_line = "Start_time_line=" + FirstDay.ToString("MMddHHmmss");
                }
                Status = "Nc_Status=" + dt3.Rows[0]["status"].ToString();

                End_time_line = "End_time_line=" + LastDay.ToString("yyyyMMddHHmmss").Substring(4, 10);

                ST_Data_3.Add($"{Update_time_date},{Start_time_min},{Cycle_time_min},{Status},{Start_time_line},{End_time_line}");
            }

            status_web = ST_Data_1.Concat(ST_Data_2).ToList<string>().Concat(ST_Data_3).ToList<string>();

            DataTable dt = new DataTable();
            dt.Columns.Add("Update_time");
            dt.Columns.Add("Start_time");
            dt.Columns.Add("Cycle_time");
            dt.Columns.Add("Nc_Status");
            dt.Columns.Add("Start_time_line");
            dt.Columns.Add("End_time_line");

            foreach (string val in status_web)
            {
                string data_str = "";
                DataRow row = dt.NewRow();
                data_str = val;
                row["Update_time"] = data_str.Split(',')[0].Split('=')[1];
                row["Start_time"] = data_str.Split(',')[1].Split('=')[1];
                row["Cycle_time"] = data_str.Split(',')[2].Split('=')[1];
                row["Nc_Status"] = data_str.Split(',')[3].Split('=')[1];
                row["Start_time_line"] = "開始時間：" + data_str.Split(',')[4].Split('=')[1].Substring(0, 2) + "/" + data_str.Split(',')[4].Split('=')[1].Substring(2, 2) + " " +
                                         data_str.Split(',')[4].Split('=')[1].Substring(4, 2) + ":" + data_str.Split(',')[4].Split('=')[1].Substring(6, 2) + ":" + data_str.Split(',')[4].Split('=')[1].Substring(8, 2);
                row["End_time_line"] = "結束時間：" + data_str.Split(',')[5].Split('=')[1].Substring(0, 2) + "/" + data_str.Split(',')[5].Split('=')[1].Substring(2, 2) + " " +
                                       data_str.Split(',')[5].Split('=')[1].Substring(4, 2) + ":" + data_str.Split(',')[5].Split('=')[1].Substring(6, 2) + ":" + data_str.Split(',')[5].Split('=')[1].Substring(8, 2);
                dt.Rows.Add(row);
            }

            XmlElement xmlElem = xmlDoc.CreateElement("ROOT");
            if (dt != null)
            {
                if (dt.Rows.Count == 0)
                {
                    xmlElem.SetAttribute("Value", "0");
                    xmlDoc.AppendChild(xmlElem);
                    return xmlDoc.DocumentElement;
                }
                xmlElem.SetAttribute("Value", dt.Rows.Count.ToString());

                xmlDoc.AppendChild(xmlElem);
                foreach (DataRow dr in dt.Rows)
                {
                    XmlElement xmlElemA = xmlDoc.CreateElement("Group");
                    xmlElemA.SetAttribute("Nc_Status", dr["Nc_Status"].ToString());
                    xmlElemA.SetAttribute("Cycle_time", dr["Cycle_time"].ToString());
                    xmlElemA.SetAttribute("Start_time", dr["Start_time"].ToString());
                    xmlElemA.SetAttribute("Update_time", dr["Update_time"].ToString());
                    xmlElemA.SetAttribute("Start_time_line", dr["Start_time_line"].ToString());
                    xmlElemA.SetAttribute("End_time_line", dr["End_time_line"].ToString());
                    xmlDoc.DocumentElement.AppendChild(xmlElemA);
                }
                return xmlDoc.DocumentElement;
            }
            else
            {
                xmlElem.SetAttribute("Value", "-1");
            }
            xmlDoc.AppendChild(xmlElem);
            return xmlDoc.DocumentElement;
        }
        //得到早上和下午的狀態BAR
        [WebMethod]
        public XmlNode GetMachStatus_bar(string Mach_ID, string First_Day, string Last_Day, string condition = "")
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            XmlDocument xmlDoc = new XmlDocument();
            DateTime FirstDay = Convert.ToDateTime(First_Day);
            DateTime LastDay = Convert.ToDateTime(Last_Day);
            LastDay = Web_Data.EndTime(LastDay);

            List<string> status_web = new List<string>();
            List<string> ST_Data_1 = new List<string>();
            List<string> ST_Data_2 = new List<string>();
            List<string> ST_Data_3 = new List<string>();
            string Update_time_date, Start_time_min, Cycle_time_min, Status, Start_time_line, End_time_line;
            DateTime Start_time, End_time;
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            string sqlcmd = $"SELECT * FROM status_history_info , status_change WHERE '{FirstDay:yyyyMMddHHmmss}' >=  update_time  AND  enddate_time  >= '{FirstDay:yyyyMMddHHmmss}' AND enddate_time <= '{LastDay:yyyyMMddHHmmss}' and mach_name = '{Mach_ID}' {WebUtils.UrlStringDecode(condition)} AND  status_change.status_english = status_history_info.status   ";
            DataTable dt1 = DataTableUtils.GetDataTable(sqlcmd);
            if (dt1 != null && dt1.Rows.Count != 0)
            {
                End_time = DateTime.ParseExact(dt1.Rows[0]["enddate_time"].ToString(), "yyyyMMddHHmmss.f", null, DateTimeStyles.AllowWhiteSpaces);
                Cycle_time_min = "Cycle_time=" + Math.Round(End_time.Subtract(FirstDay).Duration().TotalMinutes, 2, MidpointRounding.AwayFromZero).ToString();
                Status = "Nc_Status=" + dt1.Rows[0]["status"].ToString();

                Update_time_date = "Update_time=" + FirstDay.ToString("yyyyMMddHHmmss");
                Start_time_min = "Start_time=0";
                Start_time_line = "Start_time_line=" + FirstDay.ToString("MMddHHmmss");
                End_time_line = "End_time_line=" + End_time.ToString("MMddHHmmss");

                ST_Data_1.Add(Update_time_date + "," + Start_time_min + "," + Cycle_time_min + "," + Status + "," + Start_time_line + "," + End_time_line);
            }
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            sqlcmd = $"SELECT * FROM status_history_info,status_change WHERE  '{FirstDay:yyyyMMddHHmmss}' <= update_time AND enddate_time <= '{LastDay:yyyyMMddHHmmss}'   and mach_name = '{Mach_ID}'  {WebUtils.UrlStringDecode(condition)}  and status_change.status_english = status_history_info.status";
            DataTable dt2 = DataTableUtils.GetDataTable(sqlcmd);

            int count = dt2.Rows.Count;
            //持續超過一天以上
            if (dt2 != null && count == 0)
            {
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                sqlcmd = $"SELECT * FROM status_history_info,status_change WHERE '{FirstDay:yyyyMMddHHmmss}' >=  update_time  AND  enddate_time  >= '{LastDay:yyyyMMddHHmmss}' and  mach_name = '{Mach_ID}'  {WebUtils.UrlStringDecode(condition)}  and  status_change.status_english = status_history_info.status ";
                dt2 = DataTableUtils.GetDataTable(sqlcmd);

                foreach (DataRow dr in dt2.Rows) // search whole table
                {
                    dr["update_time"] = FirstDay.ToString("yyyyMMddHHmmss") + ".0";
                    dr["enddate_time"] = LastDay.ToString("yyyyMMddHHmmss") + ".0";
                }
            }
            //從上午跨到下午的部分
            if (First_Day.Contains("00:00:00") && Last_Day.Contains("11:59:59") && count != 0)
            {
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                sqlcmd = $" SELECT  status_history_info._id, mach_name, status, update_time update_time, enddate_time, timespan, upload_time, status_chinese FROM   status_history_info, status_change,(select max(_id) _id from status_history_info where  '{FirstDay:yyyyMMddHHmmss}' <= update_time AND update_time <= '{LastDay:yyyyMMddHHmmss}' AND mach_name = '{Mach_ID}' {WebUtils.UrlStringDecode(condition).Replace("status_chinese", "status")} 	AND CAST(timespan AS SIGNED) > 0) a WHERE '{FirstDay:yyyyMMddHHmmss}' <= update_time AND update_time <= '{LastDay:yyyyMMddHHmmss}' AND mach_name = '{Mach_ID}' {WebUtils.UrlStringDecode(condition)}  	AND CAST(timespan AS SIGNED) > 0	AND status_change.status_english = status_history_info.status	and a._id = status_history_info._id GROUP BY mach_name";
                DataTable dt_moon = DataTableUtils.GetDataTable(sqlcmd);
                double moon = HtmlUtil.Check_DataTable(dt_moon) ? DataTableUtils.toDouble(DataTableUtils.toString(dt_moon.Rows[0]["enddate_time"])) : 0;
                double last_time = DataTableUtils.toDouble(LastDay.ToString("yyyyMMddHHmmss.f"));
                if (moon > last_time)
                {
                    foreach (DataRow dr in dt_moon.Rows) // search whole table
                        dr["enddate_time"] = LastDay.ToString("yyyyMMddHHmmss") + ".0";
                }
                try
                {
                    dt2.Merge(dt_moon, true, MissingSchemaAction.Ignore);
                }
                catch
                {

                }
            }
            string ss = DateTime.Today.ToString("yyyy/MM/dd");
            bool o2k = First_Day.Contains(DateTime.Today.ToString("yyyy/MM/dd"));

            //抓取當天的最後一筆
            if (!First_Day.Contains(DateTime.Today.ToString("yyyy/MM/dd")) && (count != 0 || WebUtils.UrlStringDecode(condition).Contains("離線")))
            {
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                sqlcmd = $"SELECT max(status_history_info._id)  _id,mach_name,status,update_time,enddate_time,timespan,upload_time,status_chinese FROM status_history_info,status_change WHERE  update_time <= '{FirstDay:yyyyMMdd235959}'  and   mach_name = '{Mach_ID}' AND status_change.status_english = status_history_info.status  group by mach_name";
                DataTable dt_last = DataTableUtils.GetDataTable(sqlcmd);

                foreach (DataRow dr in dt_last.Rows) // search whole table
                    dr["enddate_time"] = LastDay.ToString("yyyyMMdd235959.0");

                bool ok = WebUtils.UrlStringDecode(condition).Contains(DataTableUtils.toString(dt_last.Rows[0]["status_chinese"]));
                bool test = WebUtils.UrlStringDecode(condition).Contains("status_chinese");

                if (WebUtils.UrlStringDecode(condition).Contains(DataTableUtils.toString(dt_last.Rows[0]["status_chinese"])) || !WebUtils.UrlStringDecode(condition).Contains("status_chinese"))
                    dt2.Merge(dt_last, true, MissingSchemaAction.Ignore);
            }
            ST_Data_2 = cNC_Class.Status_Bar_Info(dt2, FirstDay);
            double First_Time = 0, Now_Time = 0;
            First_Time = DataTableUtils.toDouble(FirstDay.ToString("yyyyMMdd120000"));
            Now_Time = DataTableUtils.toDouble(DateTime.Now.ToString("yyyyMMddHHmmss"));

            if (First_Time > Now_Time && First_Day.Contains("00:00:00") && Last_Day.Contains("11:59:59"))
                First_Time = DataTableUtils.toDouble(FirstDay.ToString("yyyyMMdd000000"));

            //如果是今天
            if (First_Day.Contains(DateTime.Today.ToString("yyyy/MM/dd")) && Now_Time >= First_Time)
            {
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                sqlcmd = $"select * from(SELECT     _id,    mach_name,    status,    update_time,status_chinese,   {DateTime.Now.ToString("yyyyMMddHHmmss.f")} as enddate_time,    (CAST({DateTime.Now.ToString("yyyyMMddHHmmss")} AS double)  - CAST(update_time AS double)) as timespan FROM    status_currently_info    Left join status_change on status_change.status_english = status_currently_info.status    ) as a where a.mach_name = '" + Mach_ID + "' "
                        + WebUtils.UrlStringDecode(condition).Replace("timespan", "a.timespan").Replace("status_chinese", "a.status_chinese");
                DataTable dt3 = DataTableUtils.GetDataTable(sqlcmd);
                if (dt3 != null && dt3.Rows.Count != 0)
                {
                    Start_time = DateTime.ParseExact(dt3.Rows[0]["update_time"].ToString(), "yyyyMMddHHmmss.f", null, DateTimeStyles.AllowWhiteSpaces);

                    if (Start_time > FirstDay)
                    {
                        Update_time_date = "Update_time=" + dt3.Rows[0]["update_time"].ToString().Split('.')[0];
                        Start_time_min = "Start_time=" + Math.Round(Start_time.Subtract(FirstDay).Duration().TotalMinutes, 2, MidpointRounding.AwayFromZero).ToString();
                        Cycle_time_min = "Cycle_time=" + Math.Round(LastDay.Subtract(Start_time).Duration().TotalMinutes, 2, MidpointRounding.AwayFromZero).ToString();
                        Start_time_line = "Start_time_line=" + dt3.Rows[0]["update_time"].ToString().Substring(4, 10);
                    }
                    else
                    {
                        Update_time_date = "Update_time=" + FirstDay.ToString("yyyyMMddHHmmss");
                        Start_time_min = "Start_time=0";
                        Cycle_time_min = "Cycle_time=" + Math.Round(LastDay.Subtract(FirstDay).Duration().TotalMinutes, 2, MidpointRounding.AwayFromZero).ToString();
                        Start_time_line = "Start_time_line=" + FirstDay.ToString("MMddHHmmss");
                    }
                    Status = "Nc_Status=" + dt3.Rows[0]["status"].ToString();

                    End_time_line = "End_time_line=" + LastDay.ToString("yyyyMMddHHmmss").Substring(4, 10);

                    ST_Data_3.Add(Update_time_date + "," + Start_time_min + "," + Cycle_time_min + "," + Status + "," + Start_time_line + "," + End_time_line);
                }
                status_web = ST_Data_1.Concat(ST_Data_2).ToList<string>().Concat(ST_Data_3).ToList<string>();
            }
            else
                status_web = ST_Data_1.Concat(ST_Data_2).ToList<string>();
            DataTable dt = new DataTable();
            dt.Columns.Add("Update_time");
            dt.Columns.Add("Start_time");
            dt.Columns.Add("Cycle_time");
            dt.Columns.Add("Nc_Status");
            dt.Columns.Add("Start_time_line");
            dt.Columns.Add("End_time_line");

            foreach (string val in status_web)
            {
                string data_str = "";
                DataRow row = dt.NewRow();
                data_str = val;
                row["Update_time"] = data_str.Split(',')[0].Split('=')[1];
                row["Start_time"] = data_str.Split(',')[1].Split('=')[1];
                row["Cycle_time"] = data_str.Split(',')[2].Split('=')[1];
                row["Nc_Status"] = data_str.Split(',')[3].Split('=')[1];
                row["Start_time_line"] = "開始時間：" + data_str.Split(',')[4].Split('=')[1].Substring(0, 2) + "/" + data_str.Split(',')[4].Split('=')[1].Substring(2, 2) + " " +
                                         data_str.Split(',')[4].Split('=')[1].Substring(4, 2) + ":" + data_str.Split(',')[4].Split('=')[1].Substring(6, 2) + ":" + data_str.Split(',')[4].Split('=')[1].Substring(8, 2);
                row["End_time_line"] = "結束時間：" + data_str.Split(',')[5].Split('=')[1].Substring(0, 2) + "/" + data_str.Split(',')[5].Split('=')[1].Substring(2, 2) + " " +
                                       data_str.Split(',')[5].Split('=')[1].Substring(4, 2) + ":" + data_str.Split(',')[5].Split('=')[1].Substring(6, 2) + ":" + data_str.Split(',')[5].Split('=')[1].Substring(8, 2);
                dt.Rows.Add(row);
            }
            XmlElement xmlElem = xmlDoc.CreateElement("ROOT");
            if (dt != null)
            {
                if (dt.Rows.Count == 0)
                {
                    xmlElem.SetAttribute("Value", "0");
                    xmlDoc.AppendChild(xmlElem);
                    return xmlDoc.DocumentElement;
                }
                xmlElem.SetAttribute("Value", dt.Rows.Count.ToString());

                xmlDoc.AppendChild(xmlElem);
                foreach (DataRow dr in dt.Rows)
                {
                    XmlElement xmlElemA = xmlDoc.CreateElement("Group");
                    xmlElemA.SetAttribute("Nc_Status", dr["Nc_Status"].ToString());
                    xmlElemA.SetAttribute("Cycle_time", dr["Cycle_time"].ToString());
                    xmlElemA.SetAttribute("Start_time", dr["Start_time"].ToString());
                    xmlElemA.SetAttribute("Update_time", dr["Update_time"].ToString());
                    xmlElemA.SetAttribute("Start_time_line", dr["Start_time_line"].ToString());
                    xmlElemA.SetAttribute("End_time_line", dr["End_time_line"].ToString());
                    xmlDoc.DocumentElement.AppendChild(xmlElemA);
                }
                return xmlDoc.DocumentElement;
            }
            else
                xmlElem.SetAttribute("Value", "-1");
            xmlDoc.AppendChild(xmlElem);
            return xmlDoc.DocumentElement;
        }
        //得到該機台可派工之DATATABLE
        [WebMethod]
        public XmlNode GetCanDispatchList(string Machine)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            string sqlcmd = $"select * from aps_list_info where mach_name='{Machine}' ";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);

            XmlDocument xmlDoc = new XmlDocument();
            XmlElement xmlElem = xmlDoc.CreateElement("ROOT_PIE");
            if (HtmlUtil.Check_DataTable(dt))
            {
                xmlElem.SetAttribute("Value", DataTableUtils.toString(dt.Rows.Count));
                xmlElem.SetAttribute("Machine", Machine);
                xmlDoc.AppendChild(xmlElem);
                foreach (DataRow row in dt.Rows)
                {
                    XmlElement xmlElemA = xmlDoc.CreateElement("Group");
                    xmlElemA.SetAttribute("序列", $"<center><input  style='width:150%' type ='radio' name='items' value='{DataTableUtils.toString(row["_id"])}' id ='check-all' class='flat'> \n </center>");
                    xmlElemA.SetAttribute("機台名稱", $"<center>{CNCUtils.MachName_translation(DataTableUtils.toString(row["mach_name"]))}</center>");
                    xmlElemA.SetAttribute("排程派工", $"<center>{CNCUtils.MachName_translation(DataTableUtils.toString(row["mach_name"]))}  排程派工</center>");
                    xmlElemA.SetAttribute("加工人員", $"<center>{DataTableUtils.toString(row["work_staff"])}</center>");
                    xmlElemA.SetAttribute("製令單號", $"<center>{DataTableUtils.toString(row["manu_id"])}</center>");
                    xmlElemA.SetAttribute("客戶名稱", $"<center>{DataTableUtils.toString(row["custom_name"])}</center>");
                    xmlElemA.SetAttribute("產品名稱", $"<center>{DataTableUtils.toString(row["product_name"])}</center>");
                    xmlElemA.SetAttribute("料件編號", $"<center>{DataTableUtils.toString(row["product_number"])}</center>");
                    xmlElemA.SetAttribute("工藝名稱", $"<center>{DataTableUtils.toString(row["craft_name"])}</center>");
                    xmlElemA.SetAttribute("開始時間", $"<center>{HtmlUtil.StrToDate(DataTableUtils.toString(row["start_time"]))}</center>");
                    xmlElemA.SetAttribute("結束時間", $"<center>{HtmlUtil.StrToDate(DataTableUtils.toString(row["end_time"]))}</center>");
                    xmlDoc.DocumentElement.AppendChild(xmlElemA);
                }
                return xmlDoc.DocumentElement;
            }
            else
                xmlElem.SetAttribute("Value", "-1");
            xmlDoc.AppendChild(xmlElem);
            dt.Dispose();
            return xmlDoc.DocumentElement;
        }

        [WebMethod]
        public XmlNode Get_NextTask(string Machine, string order)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            string sqlcmd = $"select * from aps_list_info where mach_name='{Machine}'  and CAST(order_number AS double) > {order}  limit 1";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);

            XmlDocument xmlDoc = new XmlDocument();
            XmlElement xmlElem = xmlDoc.CreateElement("ROOT_PIE");
            if (HtmlUtil.Check_DataTable(dt))
            {
                xmlElem.SetAttribute("Value", DataTableUtils.toString(dt.Rows.Count));
                xmlElem.SetAttribute("Machine", Machine);
                xmlDoc.AppendChild(xmlElem);
                foreach (DataRow row in dt.Rows)
                {
                    XmlElement xmlElemA = xmlDoc.CreateElement("Group");
                    xmlElemA.SetAttribute("機台名稱", $"<center>{CNCUtils.MachName_translation(DataTableUtils.toString(row["mach_name"]))}</center>");
                    xmlElemA.SetAttribute("排程派工", $"<center>{CNCUtils.MachName_translation(DataTableUtils.toString(row["mach_name"]))}  下一筆工藝內容</center>");
                    xmlElemA.SetAttribute("加工人員", $"<center>{DataTableUtils.toString(row["work_staff"])}</center>");
                    xmlElemA.SetAttribute("製令單號", $"<center>{DataTableUtils.toString(row["manu_id"])}</center>");
                    xmlElemA.SetAttribute("客戶名稱", $"<center>{DataTableUtils.toString(row["custom_name"])}</center>");
                    xmlElemA.SetAttribute("產品名稱", $"<center>{DataTableUtils.toString(row["product_name"])}</center>");
                    xmlElemA.SetAttribute("料件編號", $"<center>{DataTableUtils.toString(row["product_number"])}</center>");
                    xmlElemA.SetAttribute("工藝名稱", $"<center>{DataTableUtils.toString(row["craft_name"])}</center>");
                    xmlElemA.SetAttribute("預計生產數量", $"<center>{DataTableUtils.toString(row["exp_product_count_day"])}</center>");
                    xmlElemA.SetAttribute("開始時間", $"<center>{HtmlUtil.StrToDate(DataTableUtils.toString(row["start_time"]))}</center>");
                    xmlElemA.SetAttribute("結束時間", $"<center>{HtmlUtil.StrToDate(DataTableUtils.toString(row["end_time"]))}</center>");
                    xmlDoc.DocumentElement.AppendChild(xmlElemA);
                }
                return xmlDoc.DocumentElement;
            }
            else
                xmlElem.SetAttribute("Value", "-1");
            xmlDoc.AppendChild(xmlElem);

            return xmlDoc.DocumentElement;
        }
        [WebMethod]
        public XmlNode Get_NowTask(string Machine, string order)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            string sqlcmd = $"select * from aps_info where mach_name='{Machine}'  and CAST(order_number AS double) = {order}  limit 1";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);

            XmlDocument xmlDoc = new XmlDocument();
            XmlElement xmlElem = xmlDoc.CreateElement("ROOT_PIE");
            if (HtmlUtil.Check_DataTable(dt))
            {
                xmlElem.SetAttribute("Value", DataTableUtils.toString(dt.Rows.Count));
                xmlElem.SetAttribute("Machine", Machine);
                xmlDoc.AppendChild(xmlElem);
                foreach (DataRow row in dt.Rows)
                {
                    XmlElement xmlElemA = xmlDoc.CreateElement("Group");
                    xmlElemA.SetAttribute("機台名稱", $"<center>{CNCUtils.MachName_translation(DataTableUtils.toString(row["mach_name"]))}</center>");
                    xmlElemA.SetAttribute("排程派工", $"<center>{CNCUtils.MachName_translation(DataTableUtils.toString(row["mach_name"]))}  下一筆工藝內容</center>");
                    xmlElemA.SetAttribute("加工人員", $"<center>{DataTableUtils.toString(row["work_staff"])}</center>");
                    xmlElemA.SetAttribute("製令單號", $"<center>{DataTableUtils.toString(row["manu_id"])}</center>");
                    xmlElemA.SetAttribute("客戶名稱", $"<center>{DataTableUtils.toString(row["custom_name"])}</center>");
                    xmlElemA.SetAttribute("產品名稱", $"<center>{DataTableUtils.toString(row["product_name"])}</center>");
                    xmlElemA.SetAttribute("料件編號", $"<center>{DataTableUtils.toString(row["product_number"])}</center>");
                    xmlElemA.SetAttribute("工藝名稱", $"<center>{DataTableUtils.toString(row["craft_name"])}</center>");
                    xmlElemA.SetAttribute("預計生產數量", $"<center>{DataTableUtils.toString(row["exp_product_count_day"])}</center>");
                    xmlElemA.SetAttribute("開始時間", $"<center>{HtmlUtil.StrToDate(DataTableUtils.toString(row["start_time"]))}</center>");
                    xmlElemA.SetAttribute("結束時間", $"<center>{HtmlUtil.StrToDate(DataTableUtils.toString(row["end_time"]))}</center>");
                    xmlDoc.DocumentElement.AppendChild(xmlElemA);
                }
                return xmlDoc.DocumentElement;
            }
            else
                xmlElem.SetAttribute("Value", "-1");
            xmlDoc.AppendChild(xmlElem);

            return xmlDoc.DocumentElement;
        }


    }
}
