using System;
using System.Collections.Generic;
using MTLinkiDB;
using System.Linq;
using System.Web;
using Support;
using System.Data;
using System.Net;
using System.Text;
using System.IO;
using System.Timers;
using System.Globalization;

namespace dek_erpvis_v2.cls
{
    public class CNC_Web_Data
    {
        public DataTable dt_data = null;
        public DataTable dt_data_1 = null;
        public DataTable dt_data_2 = null;
        public List<string> ls_data = new List<string>();
        public string Mysql_conn_str = myclass.GetConnByDekVisCnc_inside;
        private static Timer AlarmTimer;
        //取得目前排程資料，若沒有則表示空
        public DataTable Get_MachInfo(string Dev_Name)
        {
            List<string> list = new List<string>();
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            //Pre_dt
            dt_data_1 = DataTableUtils.GetDataTable($"select * from aps_info where mach_name = '{Dev_Name}'  and order_number > '0'  and start_time <= '{DateTime.Now:yyyyMMddHHmmss}' and end_time >= '{DateTime.Now:yyyyMMddHHmmss}' ");
            if (HtmlUtil.Check_DataTable(dt_data_1))
                return dt_data_1;
            else if (dt_data_1 == null)
                return Get_MachInfo(Dev_Name);
            else if (dt_data_1 != null && dt_data_1.Rows.Count < 1)
            {
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                dt_data_1 = DataTableUtils.GetDataTable($"select * from aps_info where mach_name = '{Dev_Name}'");
                if (HtmlUtil.Check_DataTable(dt_data_1))
                {
                    foreach (DataRow dr in dt_data_1.Rows) // search whole table
                    {
                        dr["check_staff"] = "";
                        dr["work_staff"] = "";
                        dr["manu_id"] = "";
                        dr["custom_name"] = "";
                        dr["product_name"] = "";
                        dr["product_number"] = "";
                        dr["craft_name"] = "";
                        dr["product_count"] = "";
                        dr["product_rate_day"] = "";
                        dr["finish_time"] = "";
                        dr["complete_time"] = "";
                        //20220811 生產進度新增NAN判斷 不設空值
                        //dr["exp_product_count_day"] = ""; 

                        //20220811 生產進度新增NAN判斷 不設空值
                        //dr["product_count_day"] = "";

                        //20220811 前人殘存未知欄位,暫不刪除
                        // dr["exp_product_count"] = ""; 
                    }
                    return dt_data_1;
                }
                return null;
            }
            else
                return null;
        }
        //報工，然後覆寫下一個排程
        public string Next_MachInfo(string Dev_Name, string order_number, string now_time = "")
        {
            //先找到當下的資料
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            string sqlcmd = $"select * from aps_info where mach_name = '{Dev_Name}'";
            DataTable dt_now = DataTableUtils.GetDataTable(sqlcmd);

            if (HtmlUtil.Check_DataTable(dt_now))
                order_number = dt_now.Rows[0]["order_number"].ToString();
            else
                order_number = "0";

            //先找出當下的
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            sqlcmd = $"select * from aps_info where mach_name = '{Dev_Name}' and CAST(order_number AS double)  = {order_number}";
            DataTable machine_info = DataTableUtils.GetDataTable(sqlcmd);
            //找出下一筆比他大的
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            sqlcmd = $"select * from aps_list_info where mach_name = '{Dev_Name}' and CAST(order_number AS double)  > {order_number}  order by CAST(order_number AS double) asc limit 1 ";
            DataTable Next_DataTable = DataTableUtils.GetDataTable(sqlcmd);

            if (order_number == "0")
                return "工單已完結，請由可視化主控台指派";
            else
            {
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                sqlcmd = "select * from aps_info_report ";
                DataTable dt_report = DataTableUtils.GetDataTable(sqlcmd);

                //該排程與報工皆存在時
                if (HtmlUtil.Check_DataTable(machine_info) && dt_report != null)
                {
                    //建立例外存取之清單
                    List<string> list = new List<string>();
                    list.Add("end_time");
                    list.Add(DateTime.Now.ToString("yyyyMMddHHmmss"));
                    //將該排程存入aps_info_report
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                    bool ok = insert_Row(dt_report, machine_info, "aps_info_report", list);
                    if (HtmlUtil.Check_DataTable(Next_DataTable))
                    {
                        list.Clear();
                        //寫入派工資料表
                        GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                        sqlcmd = "select * from aps_info_dispatch";
                        DataTable dt_dispatch = DataTableUtils.GetDataTable(sqlcmd);
                        list.Add("end_time");
                        list.Add("");
                        list.Add("list_id");
                        list.Add(DataTableUtils.toString(Next_DataTable.Rows[0]["_id"]));
                        GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                        ok = insert_Row(dt_dispatch, Next_DataTable, "aps_info_dispatch", list);

                        //紀錄回報時間
                        Update_Starttime("aps_list_info", Next_DataTable);

                        Update_Starttime("aps_list_info", Next_DataTable, machine_info);

                        list.Clear();
                        list.Add("start_time");
                        list.Add(now_time);
                        list.Add("list_id");
                        list.Add(DataTableUtils.toString(Next_DataTable.Rows[0]["_id"]));
                        list.Add("end_time");
                        list.Add(ShareFunction.StrToDate(DataTableUtils.toString(Next_DataTable.Rows[0]["end_time"])).AddYears(10).ToString("yyyyMMddHHmmss"));
                        GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                        return Update_Row(Dev_Name, machine_info, Next_DataTable, "儲存", false, list, true);
                    }
                    else
                    {
                        GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                        return Update_Row(Dev_Name, machine_info, Next_DataTable, "儲存", true);
                    }

                }
                else
                    return "儲存失敗";
            }
        }
        //修改目前排程
        public string Update_Row(string machine_name, DataTable Pre_dt, DataTable Next_dt, string action, bool null_value = false, List<string> fixed_list = null, bool check = false)
        {
            List<string> list = new List<string>();
            if (Pre_dt == null || Next_dt == null)
                return $"{action}失敗";
            else
            {
                list = Return_List(Pre_dt, Next_dt, "_id");
                DataRow row = Pre_dt.NewRow();
                row["_id"] = Pre_dt.Rows[0]["_id"];
                if (!null_value)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (fixed_list.IndexOf(list[i]) == -1)
                            row[list[i]] = Next_dt.Rows[0][list[i]];
                    }
                    if (fixed_list != null)
                    {
                        for (int i = 0; i < fixed_list.Count; i++)
                        {
                            row[fixed_list[i]] = fixed_list[i + 1];
                            i++;
                        }
                    }

                }
                else
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i] == "order_number")
                            row[list[i]] = "0";
                        else if (fixed_list != null && fixed_list.IndexOf(list[i]) != -1)
                            row[list[i]] = fixed_list[fixed_list.IndexOf(list[i]) + 1];
                        else if (list[i] != "mach_name")
                            row[list[i]] = "";
                    }
                }
                if (check)
                    row["check_time"] = "False";

                GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                if (DataTableUtils.Update_DataRow("aps_info", $"mach_name = '{machine_name}'", row))
                    return $"已完成{action},請記得清空機台數量";
                else
                {
                    string errormsg = DataTableUtils.ErrorMessage;
                    return $"{action}失敗";
                }

            }
        }
        //新增報工跟派工資料表
        public bool insert_Row(DataTable Pre_dt, DataTable Next_dt, string Table_Name, List<string> export_list = null)
        {
            List<string> list = new List<string>();
            if (Pre_dt == null || Next_dt == null)
                return false;
            else
            {
                list = Return_List(Pre_dt, Next_dt);
                DataRow row = Pre_dt.NewRow();

                for (int i = 0; i < list.Count; i++)
                {
                    if (export_list.IndexOf(list[i]) == -1)
                        row[list[i]] = Next_dt.Rows[0][list[i]];
                }
                if (export_list != null)
                {
                    for (int i = 0; i < export_list.Count; i++)
                    {
                        row[export_list[i]] = export_list[i + 1];
                        i++;
                    }
                }
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                bool ok = DataTableUtils.Insert_DataRow(Table_Name, row);
                string ss = DataTableUtils.ErrorMessage;
                return ok;
            }
        }

        //填入回報時間
        public void Update_Starttime(string tablename, DataTable dt, DataTable dt_pre = null)
        {
            if(dt_pre == null)
            {
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                DataRow row = dt.NewRow();
                row["_id"] = dt.Rows[0]["_id"];
                row["report_start_time"] = DateTime.Now.ToString("yyyyMMddHHmmss");
                DataTableUtils.Update_DataRow(tablename, $"_id='{dt.Rows[0]["_id"]}'", row);
            }
            else
            {
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                DataRow row = dt.NewRow();
                row["_id"] = dt_pre.Rows[0]["list_id"];
                row["order_status"] = "已完成報工";
                DataTableUtils.Update_DataRow(tablename, $"_id='{dt_pre.Rows[0]["list_id"]}'", row);
            }

        }

        //回傳資料表的欄位陣列
        public List<string> Return_List(DataTable dt, DataTable ds, string Ex_Column = "")
        {
            List<string> list = new List<string>();
            List<string> ex = new List<string>(Ex_Column.Split(','));
            if (dt != null && ds != null)
            {
                //抓出相同的欄位
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    for (int j = 0; j < ds.Columns.Count; j++)
                    {
                        if (DataTableUtils.toString(dt.Columns[i]) == DataTableUtils.toString(ds.Columns[j]))
                        {
                            if (ex.IndexOf(DataTableUtils.toString(dt.Columns[i])) == -1)
                                list.Add(DataTableUtils.toString(dt.Columns[i]));
                        }
                    }
                }

            }
            return list;
        }
        public string Get_CheckStaff(DataTable dt_Mach_Info)
        {
            return Aviod_NoValue(dt_Mach_Info, "check_staff");
        }
        public string Get_WorkStaff(DataTable dt_Mach_Info)
        {
            //if (HtmlUtil.Check_DataTable(dt_Mach_Info) && Aviod_NoValue(dt_Mach_Info, "work_staff") != "")
            //{
            //    GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            //    string sqlcmd = $"select WorkMan from record_worktime where WORK_MACHINE = '{dt_Mach_Info.Rows[0]["mach_name"]}' and start_time = '{dt_Mach_Info.Rows[0]["start_time"]}' and end_time = '{dt_Mach_Info.Rows[0]["end_time"]}' order by NOW_TIME desc";
            //    DataTable dt = DataTableUtils.GetDataTable(sqlcmd);

            //    if (HtmlUtil.Check_DataTable(dt))
            //        return DataTableUtils.toString(dt.Rows[0]["WorkMan"]);
            //    else
            //        return Aviod_NoValue(dt_Mach_Info, "work_staff");//加工人員           
            //}
            //else
            return Aviod_NoValue(dt_Mach_Info, "work_staff");//加工人員           
        }
        public string Get_MachName(DataTable dt_Mach_Info, string machine_name)
        {
            return Aviod_NoValue(dt_Mach_Info, "mach_name", machine_name);//設備名稱           
        }
        public string Get_MachShowName(DataTable dt_Mach_Info, string machine_name)
        {
            return CNCUtils.MachName_translation(Aviod_NoValue(dt_Mach_Info, "mach_name", machine_name));//顯示名稱           
        }
        public string Get_CustomName(DataTable dt_Mach_Info)
        {
            return Aviod_NoValue(dt_Mach_Info, "custom_name");//客戶名稱         
        }
        public string Get_Spindle_shock(DataTable dt_Mach_Info)
        {
            return Aviod_NoValue(dt_Mach_Info, "spindle_shock");//主軸震動         
        }
        public string Get_ManuID(DataTable dt_Mach_Info)
        {
            return Aviod_NoValue(dt_Mach_Info, "manu_id");//製令單號         
        }
        public string Get_ProductName(DataTable dt_Mach_Info)
        {
            return Aviod_NoValue(dt_Mach_Info, "product_name");//產品(料件)名稱        
        }
        public string Get_ProductNumber(DataTable dt_Mach_Info)
        {
            return Aviod_NoValue(dt_Mach_Info, "product_number");//產品(料件)編號        
        }
        public string Get_CraftName(DataTable dt_Mach_Info)
        {
            return Aviod_NoValue(dt_Mach_Info, "craft_name");//工藝名稱       
        }
        public string Get_CountTotal(DataTable dt_Mach_Info)//設備生產總數
        {
            return Aviod_NoValue(dt_Mach_Info, "product_count");
        }
        public string Get_CountToday(DataTable dt_Mach_Info)//今日生產總數
        {
            return Aviod_NoValue(dt_Mach_Info, "product_count_day");
        }
        public string Get_ExpCountToday(DataTable dt_Mach_Info)//預計今日生產總數
        {
            if (Aviod_NoValue(dt_Mach_Info, "exp_product_count_day") == "")
                return "0";
            else
                return Aviod_NoValue(dt_Mach_Info, "exp_product_count_day");
        }
        public string Get_CountTodayRate(DataTable dt_Mach_Info)//今日生產進度
        {
            if (HtmlUtil.Check_DataTable(dt_Mach_Info))
                return Aviod_NoValue(dt_Mach_Info, "product_rate_day");
            else
                return "0";
        }
        public string Get_FinishTime(DataTable dt_Mach_Info)//預計完成時間
        {
            return Aviod_NoValue(dt_Mach_Info, "finish_time");
        }
        public string Get_Operate_Rate(DataTable dt_Mach_Info, string mach = "")//今日目前稼動率
        {
            if (HtmlUtil.Check_DataTable(dt_Mach_Info))
            {
                //如果出現NaN，則顯示為0
                if (dt_Mach_Info.Rows[0]["operate_rate"].ToString() == "NaN" || dt_Mach_Info.Rows[0]["operate_rate"].ToString() == "非數值" || Double.IsInfinity(DataTableUtils.toDouble(dt_Mach_Info.Rows[0]["operate_rate"].ToString())))
                    return "0";
                else
                    return Aviod_NoValue(dt_Mach_Info, "operate_rate");
            }
            else
            {
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                string sqlcmd = $"select * from aps_info where mach_name =  '{mach}'";
                DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
                if (HtmlUtil.Check_DataTable(dt))
                    return DataTableUtils.toString(dt.Rows[0]["operate_rate"]);
                else
                    return "0";
            }
        }
        public string Get_ProgramRun(DataTable dt_Mach_Info, string machine_name)//目前執行程式
        {
            return Aviod_NoValue(dt_Mach_Info, "prog_run", machine_name);
        }
        public string Get_ProgramMain(DataTable dt_Mach_Info)//目前主程式
        {
            return Aviod_NoValue(dt_Mach_Info, "prog_main");
        }
        public string Get_AlarmMesg(DataTable dt_Mach_Info)//目前警報
        {
            return Aviod_NoValue(dt_Mach_Info, "alarm_mesg");
        }
        public string Get_MachStatus(DataTable dt_Mach_Info, string machine_name)//目前狀態
        {
            return Aviod_NoValue(dt_Mach_Info, "mach_status", machine_name);
        }

        //進給率 主軸轉速 主軸溫度 主軸速度 主軸負載 切削時間 通電時間 運轉時間 主程式 主程式註解 運行程式註解 用
        public string Get_Information(DataTable dt_Mach_Info, string column)
        {
            return Aviod_NoValue(dt_Mach_Info, column);
        }
        //避免出錯的方式
        private string Aviod_NoValue(DataTable dt_Mach_Info, string value, string mach_name = "")
        {
            try
            {
                if (value == "run_time" || value == "cut_time" || value == "poweron_time")
                {
                    string dt_value = dt_Mach_Info.Rows[0][value].ToString();
                    dt_value = dt_value.Replace("H", ":").Replace("M", ":").Replace("S", ":");
                    List<string> time = new List<string>(dt_value.Split(':'));
                    int day = DataTableUtils.toInt(time[0]) / 24;
                    int hour = DataTableUtils.toInt(time[0]) % 24;
                    if (WebUtils.GetAppSettings("time_type") == "0")
                        return $"{day}天 {hour}小時 {time[1]}分鐘 {time[2]}秒";
                    else if (WebUtils.GetAppSettings("time_type") == "1")
                        return $"{day}天 {hour}小時 {time[1]}分鐘";
                    else if (WebUtils.GetAppSettings("time_type") == "2")
                        return $"{day}天 {hour}小時";
                    else if (WebUtils.GetAppSettings("time_type") == "3")
                        return $"{time[0]}小時";
                    else
                        return $"{day}天 {hour}小時 {time[1]}分鐘 {time[2]}秒";
                }
                else if (value == "prog_main" || value == "prog_run")
                {
                    List<string> progarm = new List<string>(dt_Mach_Info.Rows[0][value].ToString().Split('/'));
                    return progarm[progarm.Count - 1];
                }
                else
                    return dt_Mach_Info.Rows[0][value].ToString();
            }
            catch
            {
                if (mach_name != "" && value == "mach_status")
                {
                    GlobalVar.UseDB_setConnString(Mysql_conn_str);
                    string sqlcmd = $"select * from cnc_db.status_currently_info where mach_name = '{mach_name}'";
                    DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
                    try
                    {
                        return dt.Rows[0]["status"].ToString();
                    }
                    catch
                    {
                        return "";
                    }
                }
                else if (mach_name != "" && value == "prog_run")
                {
                    GlobalVar.UseDB_setConnString(Mysql_conn_str);
                    string sqlcmd = $"select * from aps_info where mach_name = '{mach_name}'";
                    DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
                    try
                    {
                        List<string> progarm = new List<string>(DataTableUtils.toString(dt.Rows[0]["prog_run"]).Split('/'));
                        return progarm[progarm.Count - 1];
                    }
                    catch
                    {
                        return "";
                    }

                }
                else if (mach_name != "" && value != "mach_status")
                    return mach_name;
                else
                    return "";
            }
        }
        public List<string> Get_Operate_Rate(string s_time, string e_time, string MachName)//稼動率//統計分析用
        {
            List<string> Operate_Rate = new List<string>();
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            DataTable dt_operate_rate = DataTableUtils.GetDataTable($"select * from status_realtime_info where mach_name = '{MachName}'and work_date >= '{s_time}' and work_date <= '{e_time}'");
            if (HtmlUtil.Check_DataTable(dt_operate_rate))
            {
                s_time = HtmlUtil.changetimeformat(s_time);
                DateTime st = DateTime.Parse(s_time.ToString());//取得該區間開始時間
                e_time = HtmlUtil.changetimeformat(e_time);
                DateTime end = DateTime.Parse(e_time.ToString());//取得該區間結束時間(+1式)
                DateTime noon = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd") + " 下午 12:00:00");
                List<string> list = new List<string>();//將時間放置於此
                List<string> list_num = new List<string>();
                TimeSpan ts = end - st;//日期相減
                TimeSpan distance = end - noon;
                int day = 1;
                day += Int16.Parse(ts.TotalDays.ToString("00"));//取得整數 

                //當月到目前為止非假日的時間點
                for (int i = 0; i < day; i++)
                {
                    if (st.AddDays(i).ToString() != "")
                        list.Add(st.AddDays(i).ToString("yyyyMMdd"));
                }
                //機台所有非假日時間點
                for (int iIndex = 0; iIndex < dt_operate_rate.Rows.Count; iIndex++)
                {
                    if (HtmlUtil.changetimeformat(dt_operate_rate.Rows[iIndex]["work_date"].ToString()) != "")
                        list_num.Add(dt_operate_rate.Rows[iIndex]["work_date"].ToString() + ":" +
                                     dt_operate_rate.Rows[iIndex]["work_time"].ToString() + "," +
                                     dt_operate_rate.Rows[iIndex]["disc_time"].ToString() + "," +
                                     dt_operate_rate.Rows[iIndex]["idle_time"].ToString() + "," +
                                     dt_operate_rate.Rows[iIndex]["alarm_time"].ToString() + "," +
                                     dt_operate_rate.Rows[iIndex]["operate_time"].ToString() + "," +
                                    Avoid_Null(dt_operate_rate.Rows[iIndex]["emergency_time"].ToString()) + "," +
                                     Avoid_Null(dt_operate_rate.Rows[iIndex]["suspend_time"].ToString()) + "," +
                                      Avoid_Null(dt_operate_rate.Rows[iIndex]["manual_time"].ToString()) + "," +
                                     Avoid_Null(dt_operate_rate.Rows[iIndex]["warmup_time"].ToString()));
                }

                for (int i = 0; i < list.Count; i++)
                {
                    bool isok = false;
                    for (int j = 0; j < list_num.Count; j++)
                    {
                        if (isok == false)
                        {
                            //有存在
                            if (list_num[j].IndexOf(list[i]) != -1)
                            {
                                Operate_Rate.Add(list_num[j]);
                                isok = true;
                            }
                        }
                    }
                    if (isok == false)
                        Operate_Rate.Add(list[i] + ":" + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0 + "," + 0);
                }
            }
            return Operate_Rate;
        }
        public string Avoid_Null(string str)
        {
            if (str == "" || str == null)
                return "0";
            else
                return str;
        }
        public DateTime EndTime(DateTime end_time)
        {
            if (end_time >= DateTime.Now) end_time = DateTime.Now;
            return end_time;
        }
    }

    public class CNCUtils
    {
        private string str_FirstDay = "";
        private string str_LastDay = "";
        MTLinkiDB.MTLinkiDB MTLinki_DB = new MTLinkiDB.MTLinkiDB();

        public List<string> Status_Bar_Info(DataTable DT_Data, DateTime FirstDay)
        {
            string Update_time_date, Start_time_min, Cycle_time_min, Status, Start_time_line, End_time_line;
            DateTime Start_time, End_time;
            List<string> status_web = new List<string>();
            if (DT_Data != null && DT_Data.Rows.Count != 0)
            {
                for (int iIndex_1 = 0; iIndex_1 < DT_Data.Rows.Count; iIndex_1++)
                {
                    Update_time_date = "Update_time=" + DT_Data.Rows[iIndex_1]["update_time"].ToString().Split('.')[0];
                    Start_time = DateTime.ParseExact(DT_Data.Rows[iIndex_1]["update_time"].ToString(), "yyyyMMddHHmmss.f", null, DateTimeStyles.AllowWhiteSpaces);
                    Start_time_min = "Start_time=" + Math.Round(Start_time.Subtract(FirstDay).Duration().TotalMinutes, 2, MidpointRounding.AwayFromZero).ToString();
                    End_time = DateTime.ParseExact(DT_Data.Rows[iIndex_1]["enddate_time"].ToString(), "yyyyMMddHHmmss.f", null, DateTimeStyles.AllowWhiteSpaces);
                    Cycle_time_min = "Cycle_time=" + Math.Round(End_time.Subtract(Start_time).Duration().TotalMinutes, 2, MidpointRounding.AwayFromZero).ToString();
                    Status = "Nc_Status=" + DT_Data.Rows[iIndex_1]["status"].ToString();
                    Start_time_line = "Start_time_line=" + DT_Data.Rows[iIndex_1]["update_time"].ToString().Substring(4, 10);
                    End_time_line = "End_time_line=" + DT_Data.Rows[iIndex_1]["enddate_time"].ToString().Substring(4, 10);

                    status_web.Add(Update_time_date + "," + Start_time_min + "," + Cycle_time_min + "," + Status + "," + Start_time_line + "," + End_time_line);
                }
            }
            return status_web;
        }
        //機台英文名稱→中文
        public static string MachName_translation(string id)
        {
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            string sqlcmd = $"select * from machine_info where mach_name = '{id}'";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);

            if (HtmlUtil.Check_DataTable(dt))
                id = DataTableUtils.toString(dt.Rows[0]["mach_show_name"]);

            return id;
        }

        static Dictionary<CNCStatusCode, string> CNCStatusColor_list = new Dictionary<CNCStatusCode, string>()
        {
            { CNCStatusCode.OPERATE,"#04ba26"}, { CNCStatusCode.SUSPEND, "#808000"},//運轉//暫停
            { CNCStatusCode.MANUAL,"#00EFEF"}, { CNCStatusCode.WARMUP,"#890F27"},//手動//暖機
            { CNCStatusCode.ALARM,"#f73939"}, { CNCStatusCode.EMERGENCY, "#f73939"},//警告//警報
            { CNCStatusCode.STOP,"#ff9900"},//閒置
            { CNCStatusCode.DISCONNECT, "#737373"},//離線
            { CNCStatusCode.NONE, "#737373"}//關機//先用none代替
        };

        public string mach_status_Color(CNCStatusCode status)
        {
            string color;
            try
            {
                color = CNCStatusColor_list[status];
            }
            catch
            {
                color = "0x000000";
            }
            return color;
        }
        public string mach_status_Color(string status)
        {
            string color = "";
            switch (status)
            {
                case "OPERATE":
                    color = "#00b400";
                    break;
                case "DISCONNECT":
                    color = "#a9a9a9";
                    break;
                case "ALARM":
                    color = "#ff0000";
                    break;
                case "EMERGENCY":
                    color = "#ff00ff";
                    break;
                case "SUSPEND":
                    color = "#ffff00";
                    break;
                case "STOP":
                    color = "#ff9b32";
                    break;
                case "MANUAL":
                    color = "#02cdfc";
                    break;
                case "WARMUP":
                    color = "#b22222";
                    break;
            }
            return color;
        }
        public string mach_font_Color(string status)
        {
            string color = "";
            if (status == "閒置" || status == "待機" || status == "離線" || status == "暫停") color = "black";
            else color = "black";
            return color;
        }
        public string mach_background_Color(string status)
        {
            string color = "";
            switch (status)
            {
                case "運轉":
                case "OPERATE":
                    color = "#00b400";
                    break;
                case "離線":
                case "DISCONNECT":
                    color = "#a9a9a9";
                    break;
                case "警報":
                case "ALARM":
                    color = "#ff0000";
                    break;
                case "警告":
                case "EMERGENCY":
                    color = "#ff00ff";
                    break;
                case "暫停":
                case "SUSPEND":
                    color = "#ffff00";
                    break;
                case "待機":
                case "STOP":
                    color = "#ff9b32";
                    break;
                case "手動":
                case "MANUAL":
                    color = "#02cdfc";
                    break;
                case "暖機":
                case "WARMUP":
                    color = "#b22222";
                    break;
            }
            return color;
        }
        public string mach_status_EN2CH(string status_en)
        {
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            string sqlcmd = $"select * from status_change where status_english = '{status_en}'";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
            if (HtmlUtil.Check_DataTable(dt))
                return DataTableUtils.toString(dt.Rows[0]["status_chinese"]);
            else
                return status_en;
        }
        public List<string> get_search_time(string btnID, string str_time = "", string end_time = "")
        {
            List<string> ST_First_Last_Time = new List<string>();
            switch (btnID)
            {
                case "day":
                    str_FirstDay = DateTime.Today.ToString("yyyyMMddHHmmss");
                    str_LastDay = DateTime.Today.AddDays(1).AddSeconds(-1).ToString("yyyyMMddHHmmss");
                    break;
                case "week":
                    str_FirstDay = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek).ToString("yyyyMMddHHmmss");  //單位：周
                    str_LastDay = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + 7).AddSeconds(-1).ToString("yyyyMMddHHmmss");
                    break;
                case "month":
                    str_FirstDay = DateTime.Now.AddMonths(0).Date.AddDays(1 - DateTime.Now.Day).ToString("yyyyMMddHHmmss");
                    str_LastDay = DateTime.Now.AddDays(1 - DateTime.Now.Day).Date.AddMonths(1).AddSeconds(-1).ToString("yyyyMMddHHmmss");
                    break;
                case "season":
                    DateTime dTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM"));                  //自訂月份//{2019/8/1 上午 12:00:00}
                    DateTime FirstDay = dTime.AddMonths(0 - (dTime.Month - 1) % 3).AddDays(1 - dTime.Day);  //本季度初
                    DateTime LastDay = FirstDay.AddMonths(3).AddSeconds(-1);
                    str_FirstDay = FirstDay.ToString("yyyyMMddHHmmss");
                    str_LastDay = LastDay.ToString("yyyyMMddHHmmss");
                    break;
                case "firsthalf":
                    str_FirstDay = new DateTime(DateTime.Now.Year, 1, 1).ToString("yyyyMMddHHmmss");  //2019-01-01
                    str_LastDay = new DateTime(DateTime.Now.Year, 7, 1).AddSeconds(-1).ToString("yyyyMMddHHmmss"); //2019-12-31
                    break;
                case "lasthalf":
                    str_FirstDay = new DateTime(DateTime.Now.Year, 7, 1).ToString("yyyyMMddHHmmss");  //2019-01-01
                    str_LastDay = new DateTime(DateTime.Now.Year, 12, 1).AddMonths(1).AddSeconds(-1).ToString("yyyyMMddHHmmss"); //2019-12-31
                    break;
                case "year":
                    str_FirstDay = new DateTime(DateTime.Now.Year, 1, 1).ToString("yyyyMMddHHmmss");  //2019-01-01
                    str_LastDay = new DateTime(DateTime.Now.Year, 12, 1).AddMonths(1).AddDays(-1).AddSeconds(-1).ToString("yyyyMMddHHmmss"); //2019-12-31
                    break;
                case "select":
                    if (DaysBetween(ret_date(str_time), ret_date(end_time)) > 730)
                    {
                        HttpContext.Current.Response.Write("<script language='javascript'>alert('伺服器回應 : 日期搜尋範圍不得大於 730 天 !');</script>");
                        str_FirstDay = DateTime.Now.AddMonths(0).Date.AddDays(1 - DateTime.Now.Day).ToString("yyyyMMddHHmmss");
                        str_LastDay = DateTime.Now.AddDays(1 - DateTime.Now.Day).Date.AddMonths(1).AddSeconds(-1).ToString("yyyyMMddHHmmss");
                    }
                    else
                    {
                        str_FirstDay = ret_date(str_time).ToString("yyyyMMddHHmmss");
                        str_LastDay = ret_date(end_time).ToString("yyyyMMddHHmmss");
                    }
                    break;
                case "selectdate"://status_bar
                    str_FirstDay = ret_date(str_time).ToString("yyyyMMddHHmmss");
                    end_time = DateTime.ParseExact(str_FirstDay, "yyyyMMddHHmmss", null, DateTimeStyles.AllowWhiteSpaces).AddDays(1).ToString("yyyyMMddHHmmss");
                    str_LastDay = ret_date(end_time.Substring(0, 8)).ToString("yyyyMMddHHmmss");
                    break;
            }
            ST_First_Last_Time.Add(str_FirstDay + "," + str_LastDay);
            ST_First_Last_Time.Add(btnID);
            return ST_First_Last_Time;
        }
        public int DaysBetween(DateTime d1, DateTime d2)
        {
            TimeSpan span = d2.Subtract(d1);
            return (int)span.TotalDays;
        }
        public DateTime ret_date(string date)
        {
            DateTime NewDate = new DateTime();
            var isContain = date.IndexOf(",", StringComparison.OrdinalIgnoreCase);
            if (isContain != -1 && date.Split(',')[1] == "PM")
                NewDate = (DateTime.ParseExact(date.Split(',')[0], "yyyyMMddhhmm", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces)).AddHours(12);
            else if (isContain != -1 && date.Split(',')[1] == "AM")
                NewDate = DateTime.ParseExact(date.Split(',')[0], "yyyyMMddhhmm", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
            else
                NewDate = DateTime.ParseExact(date + "0000".Split(',')[0], "yyyyMMddhhmm", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
            return NewDate;
        }
        public double Math_Round(double value_1, double value_2, int point)
        {
            return Math.Round(value_1 / value_2, point, MidpointRounding.AwayFromZero);
        }
        public static string date_Substring(string date)
        {
            try
            {
                return date.Substring(0, 11);
            }
            catch
            {
                return date;
            }

        }
        //加工程式→工藝名稱
        public static string change_productname(string product)
        {
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            string sqlcmd = $"select * from product_info where craft_name = '{product}'";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);

            if (HtmlUtil.Check_DataTable(dt))
                product = DataTableUtils.toString(dt.Rows[0]["cnc_craft_name"]);

            return product;
        }
        //找到有攝影機的機台
        public static string Have_CameraLink(string machine)
        {
            string value = "";
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            string sqlcmd = $"select * from machine_info where mach_name = '{machine}'";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);

            if (HtmlUtil.Check_DataTable(dt))
                value = DataTableUtils.toString(dt.Rows[0]["camera_address"]);

            if (value == "")
                return "";
            else
                return value;
        }
        //找到對應的群組
        public static string Find_Group(string group)
        {
            if (group == "All" || group == "全部" || group == "" || group == "全廠")
                return "";
            else
                return group;
        }

    }

}