using System;
using System.Collections.Generic;
using MDC;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Support;
using System.Linq;
using Utils;

namespace MTLinkiDB
{
    public class MTLinkiDB
    {
        MongoDatabase database;
        IMongoDatabase _database = null;
        IMongoCollection<BsonDocument> _Collection = null;

        public enum MongoSortType { Descending = -1, Ascending = 1 };

        public string ErrorMessage { get; set; }

        public bool DatatableToMongo(string Sql_cond, string Mongo_tablename)
        {
            bool is_ok = false;
            DataTableUtils.Conn_String = ConnStr.sql_str;
            BsonDocument bsons = MongoUtils.DataTableToBsonDocument(DataTableUtils.GetDataTable(Sql_cond));

            SelectDataTable_IMongo(Mongo_tablename);
            if (bsons.Count() > 0)
            {
                foreach (BsonDocument bson in bsons["rows"].AsBsonArray)
                    is_ok = Insert_MongoDB_Data_Bson(bson);
            }
            return is_ok;
        }

        public void GetConntionString_MongoDB(string db_name, string user, string password, string IP = "", string PORT = "")
        {
            if (IP == "") IP = "localhost";
            if (PORT == "") PORT = "27017";
            //IP = "192.168.1.114";
            //IP = "localhost";
            //PORT = "27017";
            //string connect_str = "mongodb://" + user + ":" + password + "@" + IP + ":27017/MTLINKi";
            string connect_str = "mongodb://" + user + ":" + password + "@" + IP + ":"+ PORT + "/MTLINKi";
            MongoClient client = new MongoClient(connect_str);
            var server = client.GetServer();
            database = server.GetDatabase(db_name);
            _database = client.GetDatabase(db_name);
        }

        public MongoCollection<BsonDocument> SelectDataTable(string collection_name)
        {
            return database.GetCollection<BsonDocument>(collection_name);
        }

        public IMongoCollection<BsonDocument> SelectDataTable_IMongo(string collection_name)
        {
            _Collection = _database.GetCollection<BsonDocument>(collection_name);
            return _Collection;
        }

        public List<string> Get_Machine_List()
        {
            return Get_DB_Info("L0_Setting", Query.NE("_id", 0), new string[] { "L0Name" });
        }

        public List<string> Get_Device_List()
        {
            return Get_DB_Info("L1_Setting", Query.NE("_id", 0), new string[] { "L1Name" });
        }

        public List<string> Get_DevGroup_List()
        {
            return Get_DB_Info("_dek_DevGroup", Query.NE("_id", 0), new string[] { "設備群組" });
        }

        public List<string> Get_Permission_List()
        {
            return Get_DB_Info("_dek_Permission", Query.NE("_id", 0), new string[] { "權限名稱" });
        }

        public List<string> Get_PermissionGroup_List()
        {
            return Get_DB_Info("_dek_PermissionGroup", Query.NE("_id", 0), new string[] { "權限群組名稱" });
        }

        public string Get_Mach_Name_Now(string dev_name, string mach_name = "")
        {
            return String.Join(",", Get_DB_Info("L1_Pool_Opened", Query.EQ("L1Name", dev_name), new string[] { "L1Name" }));
        }

        public string Get_Mach_Status_Now(string dev_name, string mach_name = "")
        {
            return String.Join(",", Get_DB_Info("L1_Pool_Opened", Query.EQ("L1Name", dev_name), new string[] { "value" }));
        }

        public string Get_Mach_Product_Count_Now(string dev_name, string mach_name = "")
        {
            return String.Join(",", Get_DB_Info("ProductResult_History_Active", Query.EQ("L1Name", dev_name), new string[] { "productresult_accumulate" }));
        }

        public List<string> Get_Mach_PreProduct_Count_Now(string dev_name, string mach_name)//用aps預計生產件數取代
        {
            var filter = Query.And(Query.EQ("L1Name", dev_name), Query.EQ("signalname", "PartsNumAll_path1_" + mach_name));
            return Get_DB_Info("L1Signal_Pool_Active", filter, new string[] { "value" });
        }

        public List<string> Get_Mach_MainProgram_Now(string dev_name, string mach_name)
        {
            var filter = Query.And(Query.EQ("L1Name", dev_name), Query.EQ("signalname", "MainProgram_path1_" + mach_name));
            return Get_ReverseProgName(Get_DB_Info("L1Signal_Pool_Active", filter, new string[] { "value" }));
        }

        public string Get_Mach_RunProgram_Now(string dev_name, string mach_name = "")
        {
            var filter = Query.And(Query.EQ("L1Name", dev_name), Query.EQ("signalname", "ActProgram_path1_" + dev_name));
            return String.Join(",", Get_ReverseProgName(Get_DB_Info("L1Signal_Pool_Active", filter, new string[] { "value" })));
        }

        public List<string> Get_ReverseProgName(List<string> ST_ProgName)
        {
            if (ST_ProgName.Count != 0)
            {
                char[] Prog_char = ST_ProgName[0].ToCharArray();
                Array.Reverse(Prog_char);
                Prog_char = new string(Prog_char).Split('/')[0].ToCharArray();
                Array.Reverse(Prog_char);
                string Prog = new string(Prog_char);
                ST_ProgName.Clear();
                ST_ProgName.Add(Prog);
                if (ST_ProgName.Count == 0)
                    ST_ProgName.Add("null");
            }
            return ST_ProgName;
        }
        public List<string> Get_Mach_PowOnTime_Now(string dev_name, string mach_name)
        {
            var filter = Query.And(Query.EQ("L1Name", dev_name), Query.EQ("signalname", "PowOnTime_path1_" + mach_name));
            return Get_DB_Info("L1Signal_Pool_Active", filter, new string[] { "value" });
        }

        public List<string> Get_Mach_RunTime_Now(string dev_name, string mach_name)
        {
            var filter = Query.And(Query.EQ("L1Name", dev_name), Query.EQ("signalname", "RunTime_path1_" + mach_name));
            return Get_DB_Info("L1Signal_Pool_Active", filter, new string[] { "value" });
        }

        public List<string> Get_Mach_CutTime_Now(string dev_name, string mach_name)
        {
            var filter = Query.And(Query.EQ("L1Name", dev_name), Query.EQ("signalname", "CutTime_path1_" + mach_name));
            return Get_DB_Info("L1Signal_Pool_Active", filter, new string[] { "value" });
        }

        public string Get_Mach_AlarmMesg_Now(string dev_name, string mach_name = "")
        {
            return String.Join(",", Get_DB_Info("Alarm_History", Query.And(Query.EQ("L1Name", dev_name), Query.EQ("timespan", 0)), new string[] { "message" }));
        }

        public string Get_FinishTime(string dev_name)
        {
            //-->1.先判斷設備現在執行程式-->2.比對資料庫該程式的加工時間、換料時間多久、產品名稱-->3.產品名稱抓資料庫生產排程之生產件數(可能有誤)-->4.紀錄現在設備生產件數、目前時間-->5.計算生產件數差額*(加工&換料時間)-->6.完成
            MyWorkTime work = new MyWorkTime(IsHoliday);
            string Prog_Info = Get_DB_Info("_dek_Product", Query.And(Query.EQ("程式名稱", Get_Mach_RunProgram_Now(dev_name))), new string[] { "加工時間", "換料時間", "產品名稱" })[0];
            if (Prog_Info == "--") return "--";

            double Prod_Count = 0, TotalTime = 0;
            string Now_Time = DateTime.Now.ToString("yyyyMMddHHmmss");
            string Produce_Data = Get_DB_Info("_dek_Produce", Query.And(Query.EQ("產品名稱", Prog_Info.Split(',')[2]), Query.EQ("工作類型", "加工"), Query.EQ("設備名稱", dev_name), Query.LTE("上工時間", Now_Time), Query.GTE("完工時間", Now_Time)), new string[] { "預計生產量" })[0];
            if (Produce_Data == "--") return "--";
            else Prod_Count = DataTableUtils.toInt(Produce_Data);
            double Now_Count = DataTableUtils.toDouble(Get_Mach_Product_Count_Now(dev_name));
            if (Now_Count >= Prod_Count) return "生產完成";
            else TotalTime = (DataTableUtils.toInt(Prog_Info.Split(',')[0]) * 60 + DataTableUtils.toInt(Prog_Info.Split(',')[1]) * 60) * (Prod_Count - Now_Count);

            //work.工作時段_新增(8,0,12,0);  // 08:00 ~ 12:00
            //work.工作時段_新增(13, 0, 17, 0);
            //string curr_time = work.目標日期(DateTime.Now, TimeSpan.FromSeconds(TotalTime)).ToString();
            return work.目標日期(DateTime.Now, TimeSpan.FromSeconds(TotalTime)).ToString("MM/dd HH:mm:ss");
        }

        static bool IsHoliday(DateTime dt)
        {
            List<DateTime> holiday_list = new List<DateTime>();
            //判斷傳入的日期dt 是否為假日
            // 若是，傳回true
            DayOfWeek week = dt.DayOfWeek;
            if (week == DayOfWeek.Saturday || week == DayOfWeek.Sunday)
                return true;
            foreach (DateTime date in holiday_list)
                if (date.Date == dt.Date) return true;
            return false;
        }

        //稼動率相關
        public List<string> Get_Mach_OperRate_Day(string dev_name, int GetInfo = 1)
        {
            DateTime Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);    //当月的第一天//{2019/8/1 上午 12:00:00}
            return Get_StatusRate(dev_name, Date, 1, GetInfo);
        }

        public List<string> Get_Mach_OperRate_Week(string dev_name, int GetInfo = 1)//得到七天(7筆1天的資料)
        {
            DateTime Date = DateTime.Now.AddDays(-Convert.ToInt16((int)DateTime.Now.DayOfWeek) + 1);//取指定日期當週星期一日期
            return Get_StatusRate(dev_name, Date, 7, GetInfo);
        }

        public List<string> Get_Mach_OperRate_Month(string dev_name, string month = "", int GetInfo = 1)//2019/08//得到約30天(30筆1天的資料)/month = DateTime.Now.ToString("yyyy/MM");
        {   //當月
            return Get_Month_Season_Half_Year_OperRate(dev_name, month, 1, GetInfo);
        }

        public List<string> Get_Mach_OperRate_Season(string dev_name, string season = "", int GetInfo = 1)//2019/08//得到3個月(3筆1個月的資料)
        {
            List<string> Date = Get_Month_Season_Half_Year_OperRate(dev_name, season, 3, GetInfo);
            return Get_Mach_OperRate_Unit_Month(Date, season, 3, GetInfo);
        }

        public List<string> Get_Mach_OperRate_Half(string dev_name, string half = "", int GetInfo = 1)//2019/08//得到6個月(6筆1個月的資料)
        {
            List<string> Date = Get_Month_Season_Half_Year_OperRate(dev_name, half, 6, GetInfo);
            return Get_Mach_OperRate_Unit_Month(Date, half, 6, GetInfo);
        }

        public List<string> Get_Mach_OperRate_Year(string dev_name, string year = "", int GetInfo = 1)//2019/08//得到12個月(12筆1個月的資料)
        {
            List<string> Date = Get_Month_Season_Half_Year_OperRate(dev_name, year, 12, GetInfo);
            return Get_Mach_OperRate_Unit_Month(Date, year, 12, GetInfo);
        }

        public List<string> Get_Month_Season_Half_Year_OperRate(string dev_name, string Time = "", int MonthCount = 1, int GetInfo = 1)//2019/08    //GetInfo=0//date/operrate/->//1//operrate
        {
            DateTime dTime = Convert.ToDateTime(Time);                  //自訂月份//{2019/8/1 上午 12:00:00}
            DateTime firstday = dTime.AddMonths(0 - (dTime.Month - 1) % MonthCount).AddDays(1 - dTime.Day);  //本季度初
            DateTime lastday = firstday.AddMonths(MonthCount);                   //隔月的第一天//{2019/9/1 上午 12:00:00}
            int totalday = lastday.Subtract(firstday).Days;             //获取两个日期间的总天数  //31

            List<string> Data_Date = Get_StatusRate(dev_name, firstday, totalday, GetInfo);
            return Data_Date;
        }

        public List<string> Get_Mach_OperRate_Unit_Month(List<string> Data, string Time, int unit_month, int GetInfo = 1)
        {
            DateTime firstday = Convert.ToDateTime(Time).AddMonths(0 - (Convert.ToDateTime(Time).Month - 1) % unit_month).AddDays(1 - Convert.ToDateTime(Time).Day);  //本季度初
            int day_count = 0;
            List<string> Data_result = new List<string>();
            for (int iIndex = 0; iIndex < unit_month; iIndex++)
            {
                double disc_Rate = 0, stop_Rate = 0, alarm_Rate = 0, oper_rate = 0;
                int totalDay = DateTime.DaysInMonth(firstday.Year, firstday.Month);
                if (GetInfo == 0)
                {
                    for (int iIndex_1 = 0 + day_count; iIndex_1 < totalDay + day_count; iIndex_1++)
                    {
                        disc_Rate += DataTableUtils.toDouble(Data[iIndex_1].Split('-')[0].Split(',')[1]);
                        stop_Rate += DataTableUtils.toDouble(Data[iIndex_1].Split('-')[1].Split(',')[1]);
                        alarm_Rate += DataTableUtils.toDouble(Data[iIndex_1].Split('-')[2].Split(',')[1]);
                        oper_rate += DataTableUtils.toDouble(Data[iIndex_1].Split(',')[4]);
                    }
                    string Date = DataTableUtils.toInt(firstday.ToString("MM")).ToString() + "月,";
                    Data_result.Add(Date + Math.Round(disc_Rate / totalDay, 2, MidpointRounding.AwayFromZero).ToString() + "-" + Date + Math.Round(stop_Rate / totalDay, 2, MidpointRounding.AwayFromZero).ToString() + "-" + Date + Math.Round(alarm_Rate / totalDay, 2, MidpointRounding.AwayFromZero).ToString() + "-" + Date + Math.Round(oper_rate / totalDay, 2, MidpointRounding.AwayFromZero).ToString());
                }
                else
                {
                    for (int iIndex_1 = 0 + day_count; iIndex_1 < totalDay + day_count; iIndex_1++)
                        oper_rate += DataTableUtils.toDouble(Data[iIndex_1]);
                    Data_result.Add(Math.Round(oper_rate / totalDay, 2, MidpointRounding.AwayFromZero).ToString());
                }
                day_count += totalDay;
                firstday = firstday.AddMonths(1);
            }
            return Data_result;
        }

        public List<string> Get_StatusRate(string dev_name, DateTime firstday, int totalday, int GetInfo = 1)//改全狀態
        {
            double disc_Rate = 0, stop_Rate = 0, alarm_Rate = 0, oper_Rate = 0;
            List<string> StatusRate = new List<string>();
            for (var iIndex = 0; iIndex < totalday; iIndex++)
            {
                var tempdt = firstday.Date.AddDays(iIndex);             //{2019/8/1 上午 12:00:00}
                string Rate_Data = Get_Mach_StatusRate(dev_name, tempdt);
                if (tempdt.DayOfWeek != DayOfWeek.Saturday && tempdt.DayOfWeek != DayOfWeek.Sunday)
                {
                    disc_Rate += DataTableUtils.toDouble(Rate_Data.Split(',')[0]);
                    stop_Rate += DataTableUtils.toDouble(Rate_Data.Split(',')[1]);
                    alarm_Rate += DataTableUtils.toDouble(Rate_Data.Split(',')[2]);
                    oper_Rate += DataTableUtils.toDouble(Rate_Data.Split(',')[3]);//計算當日稼動(會一直累加)                 
                }
                string Date = tempdt.ToString("MMdd") + ",";
                if (GetInfo == 0)
                    StatusRate.Add(Date + Rate_Data.Split(',')[0] + "-" + Date + Rate_Data.Split(',')[1] + "-" + Date + Rate_Data.Split(',')[2] + "-" + Date + Rate_Data.Split(',')[3]);
                else
                    StatusRate.Add(Rate_Data.Split(',')[3]);
            }
            return StatusRate;
        }

        public string Get_Mach_StatusRate(string dev_name, DateTime today)//給設備要計算狀態比例的日期
        {
            if (DateTime.Compare(today, DateTime.Now) > 0)//輸入日期大於今天日期--->就會沒資料
                return "100,0,0,0";//顯示離線
            DateTime start_time, end_time;
            string work_time = "", work_time_cal = "", date = today.ToString("yyyyMMdd");
            int div_count = 0;
            double discRate = 0, stopRate = 0, alarmRate = 0, operRate = 0;

            if (today.DayOfWeek == DayOfWeek.Monday || today.DayOfWeek == DayOfWeek.Tuesday || today.DayOfWeek == DayOfWeek.Wednesday || today.DayOfWeek == DayOfWeek.Thursday || today.DayOfWeek == DayOfWeek.Friday)
                work_time_cal = Get_DB_Info("_dek_WorkTime", Query.EQ("工時定義", "長日班"), new string[] { "上午起始時間", "上午結束時間", "下午起始時間", "下午結束時間", "晚上起始時間", "晚上結束時間" })[0];
            else if (today.DayOfWeek == DayOfWeek.Saturday || today.DayOfWeek == DayOfWeek.Sunday)
                work_time_cal = Get_DB_Info("_dek_WorkTime", Query.EQ("工時定義", "假日班"), new string[] { "上午起始時間", "上午結束時間", "下午起始時間", "下午結束時間", "晚上起始時間", "晚上結束時間" })[0];
            if (work_time_cal != "--" && work_time_cal != "")
            {
                work_time = Get_Work_Time(work_time_cal, date);
                if (work_time != "")
                {
                    for (int iIndex = 0; iIndex < work_time.Split(',').Length; iIndex++)
                    {
                        work_time_cal = System.Text.RegularExpressions.Regex.Replace(work_time_cal, ":", "");
                        if (iIndex % 2 == 0)
                        {
                            if (DataTableUtils.toDouble(work_time_cal.Split(',')[iIndex].Substring(0, 4)) == 0 || DataTableUtils.toDouble(work_time_cal.Split(',')[iIndex + 1].Substring(0, 4)) == 0)
                                div_count++;
                            else
                            {
                                start_time = DateTime.ParseExact(work_time.Split(',')[iIndex], "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
                                end_time = DateTime.ParseExact(work_time.Split(',')[iIndex + 1], "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
                                string Rate_Data = Get_All_Status_Percent(start_time, end_time, dev_name);
                                if (Rate_Data.Split(',')[1].Split(':')[1] == "0" && Rate_Data.Split(',')[2].Split(':')[1] == "0" && Rate_Data.Split(',')[3].Split(':')[1] == "0" && Rate_Data.Split(',')[4].Split(':')[1] == "0")
                                    div_count++;
                                discRate += DataTableUtils.toDouble(Rate_Data.Split(',')[1].Split(':')[1]);
                                stopRate += DataTableUtils.toDouble(Rate_Data.Split(',')[2].Split(':')[1]);
                                alarmRate += DataTableUtils.toDouble(Rate_Data.Split(',')[3].Split(':')[1]);
                                operRate += DataTableUtils.toDouble(Rate_Data.Split(',')[4].Split(':')[1]);
                            }
                        }
                    }
                    if (div_count == 3) div_count = 2;//避免除0產生錯誤
                    discRate = ((Math.Round(discRate / (3 - div_count), 2, MidpointRounding.AwayFromZero)));
                    stopRate = ((Math.Round(stopRate / (3 - div_count), 2, MidpointRounding.AwayFromZero)));
                    alarmRate = ((Math.Round(alarmRate / (3 - div_count), 2, MidpointRounding.AwayFromZero)));
                    operRate = ((Math.Round(operRate / (3 - div_count), 2, MidpointRounding.AwayFromZero)));
                }
            }
            return discRate.ToString() + "," + stopRate.ToString() + "," + alarmRate.ToString() + "," + operRate.ToString();
        }

        public string Get_Work_Time(string work_time_cal, string date)
        {
            string work_time = "";
            work_time_cal = System.Text.RegularExpressions.Regex.Replace(work_time_cal, ":", "");
            for (int iIndex = 0; iIndex < work_time_cal.Split(',').Length; iIndex++)
            {
                if (iIndex < work_time_cal.Split(',').Length - 1)
                    work_time += date + work_time_cal.Split(',')[iIndex] + "00" + ",";
                else
                    work_time += date + work_time_cal.Split(',')[iIndex] + "00";
            }
            return work_time;
        }

        public string Get_All_Status_Percent(DateTime start_time, DateTime end_time, string dev_name)//四種狀態比例
        {
            string[] Mach_Status = { "DISCONNECT", "STOP", "EMERGENCY", "OPERATE", "SUSPEND", "ALARM", "MANUAL", "WARMUP" };
            double[] Sum_Time = { 0, 0, 0, 0, 0, 0, 0, 0 };
            string[] Mach_Status_cal = { "DISCONNECT", "STOP", "EMERGENCY", "OPERATE" };
            double[] Sum_Time_cal = { 0, 0, 0, 0 };

            List<string> ST_Status_Percent = new List<string>();
            TimeSpan ts;
            for (int iIndex = 0; iIndex < Mach_Status.Length; iIndex++)
            {   //----L1_Pool-->ok
                List<string> ST_Data_First = Get_DB_Info("L1_Pool", Query.And(Query.GTE("updatedate", start_time), Query.LTE("enddate", end_time), Query.EQ("value", Mach_Status[iIndex]), Query.EQ("L1Name", dev_name)), new string[] { "timespan" });
                if (ST_Data_First[0] != "--")
                {
                    for (int iIndex_1 = 0; iIndex_1 < ST_Data_First.Count(); iIndex_1++)
                        Sum_Time[iIndex] += DataTableUtils.toDouble(ST_Data_First[iIndex_1]);
                }
                List<string> ST_Data_Sec = Get_DB_Info("L1_Pool", Query.And(Query.LTE("updatedate", start_time), Query.GTE("enddate", start_time), Query.LTE("enddate", end_time), Query.EQ("value", Mach_Status[iIndex]), Query.EQ("L1Name", dev_name)), new string[] { "timespan", "enddate" });
                if (ST_Data_Sec[0] != "--")
                {
                    ts = Convert.ToDateTime(ST_Data_Sec[0].Split(',')[1]) - start_time;
                    Sum_Time[iIndex] += DataTableUtils.toDouble(ts.TotalSeconds.ToString());
                }
                List<string> ST_Data_Third = Get_DB_Info("L1_Pool", Query.And(Query.GTE("updatedate", start_time), Query.LTE("updatedate", end_time), Query.GTE("enddate", end_time), Query.EQ("value", Mach_Status[iIndex]), Query.EQ("L1Name", dev_name)), new string[] { "timespan", "updatedate" });
                if (ST_Data_Third[0] != "--")
                {
                    ts = end_time - Convert.ToDateTime(ST_Data_Third[0].Split(',')[1]);
                    Sum_Time[iIndex] += DataTableUtils.toDouble(ts.TotalSeconds.ToString());
                }
                List<string> ST_Data_Fourth = Get_DB_Info("L1_Pool", Query.And(Query.LTE("updatedate", start_time), Query.GTE("enddate", end_time), Query.EQ("value", Mach_Status[iIndex]), Query.EQ("L1Name", dev_name)), new string[] { "timespan" });
                if (ST_Data_Fourth[0] != "--")
                {
                    ts = end_time - start_time;
                    Sum_Time[iIndex] += DataTableUtils.toDouble(ts.TotalSeconds.ToString());
                }
            }
            for (int iIndex_1 = 0; iIndex_1 < Mach_Status.Length; iIndex_1++)
            {   //----L1_Pool_Open-->ok
                end_time = EndTime(end_time);
                List<string> ST_Data_Last_First = Get_DB_Info("L1_Pool_Opened", Query.And(Query.GTE("updatedate", start_time), Query.LTE("updatedate", end_time), Query.EQ("value", Mach_Status[iIndex_1]), Query.EQ("L1Name", dev_name)), new string[] { "updatedate" });
                if (ST_Data_Last_First[0] != "--")
                {
                    ts = end_time - Convert.ToDateTime(ST_Data_Last_First[0]);
                    Sum_Time[iIndex_1] += DataTableUtils.toDouble(ts.TotalSeconds.ToString());
                }
                //下列狀況應該是不會發生
                if (end_time > start_time)
                {
                    List<string> ST_Data_Last_Fourth = Get_DB_Info("L1_Pool_Opened", Query.And(Query.LTE("updatedate", start_time), Query.EQ("value", Mach_Status[iIndex_1]), Query.EQ("L1Name", dev_name)), new string[] { "timespan" });
                    if (ST_Data_Last_Fourth[0] != "--")
                    {
                        ts = end_time - start_time;//不再工作時間會有問題
                        Sum_Time[iIndex_1] += DataTableUtils.toDouble(ts.TotalSeconds.ToString());
                    }
                }
            }
            Sum_Time_cal[0] = Sum_Time[0];//DISCONNECT
            Sum_Time_cal[1] = Sum_Time[1];//STOP
            Sum_Time_cal[2] = Sum_Time[2] + Sum_Time[5];//EMERGENCY + ALARM
            Sum_Time_cal[3] = Sum_Time[3] + Sum_Time[4] + Sum_Time[6] + Sum_Time[7];//OPERATE + SUSPEND + MANUAL + WARMUP
            double Sum_Status_Time = Sum_Time_cal[0] + Sum_Time_cal[1] + Sum_Time_cal[2] + Sum_Time_cal[3];
            if (Sum_Status_Time == 0) return "Device Name:" + dev_name + ",DISCONNECT:0,STOP:0,EMERGENCY:0,OPERATE:0";

            string mach_status_Percent = "Device Name:" + dev_name + ",";
            for (int iIndex_3 = 0; iIndex_3 < Sum_Time_cal.Length; iIndex_3++)
            {
                if (iIndex_3 == Sum_Time_cal.Length - 1)
                    mach_status_Percent += Mach_Status_cal[iIndex_3] + ":" + ((Math.Round(Sum_Time_cal[iIndex_3] / Sum_Status_Time, 4, MidpointRounding.AwayFromZero)) * 100).ToString();
                else
                    mach_status_Percent += Mach_Status_cal[iIndex_3] + ":" + ((Math.Round(Sum_Time_cal[iIndex_3] / Sum_Status_Time, 4, MidpointRounding.AwayFromZero)) * 100).ToString() + ",";
            }
            return mach_status_Percent;
        }

        public string Get_Check_MachStaff_Now(string dev_name, string mach_name = "")
        {
            string Now_Time = DateTime.Now.ToString("yyyyMMddHHmmss");
            return String.Join(",", Get_DB_Info("_dek_Produce", Query.And(Query.LTE("上工時間", Now_Time), Query.GTE("完工時間", Now_Time), Query.EQ("設備名稱", dev_name), Query.EQ("工作類型", "校機")), new string[] { "執行人員" }));
        }

        public string Get_CustomName_Now(string dev_name, string mach_name = "")
        {
            string Now_Time = DateTime.Now.ToString("yyyyMMddHHmmss");
            return String.Join(",", Get_DB_Info("_dek_Produce", Query.And(Query.LTE("上工時間", Now_Time), Query.GTE("完工時間", Now_Time), Query.EQ("設備名稱", dev_name), Query.EQ("工作類型", "加工")), new string[] { "客戶名稱" }));
        }

        public string Get_prodNo_Now(string dev_name, string mach_name = "")
        {
            string Now_Time = DateTime.Now.ToString("yyyyMMddHHmmss");
            return String.Join(",", Get_DB_Info("_dek_Produce", Query.And(Query.LTE("上工時間", Now_Time), Query.GTE("完工時間", Now_Time), Query.EQ("設備名稱", dev_name), Query.EQ("工作類型", "加工")), new string[] { "料件編號" }));
        }

        public string Get_tarParts_Now(string dev_name, string mach_name = "")
        {
            string Now_Time = DateTime.Now.ToString("yyyyMMddHHmmss");
            return String.Join(",", Get_DB_Info("_dek_Produce", Query.And(Query.LTE("上工時間", Now_Time), Query.GTE("完工時間", Now_Time), Query.EQ("設備名稱", dev_name), Query.EQ("工作類型", "加工")), new string[] { "預計生產量" }));
        }

        public string Get_workStaff_Now(string dev_name, string mach_name = "")
        {
            string Now_Time = DateTime.Now.ToString("yyyyMMddHHmmss");
            return String.Join(",", Get_DB_Info("_dek_Produce", Query.And(Query.LTE("上工時間", Now_Time), Query.GTE("完工時間", Now_Time), Query.EQ("設備名稱", dev_name), Query.EQ("工作類型", "加工")), new string[] { "執行人員" }));
        }

        public string Get_prodName_Now(string dev_name, string mach_name = "")
        {
            string Now_Time = DateTime.Now.ToString("yyyyMMddHHmmss");
            return String.Join(",", Get_DB_Info("_dek_Produce", Query.And(Query.LTE("上工時間", Now_Time), Query.GTE("完工時間", Now_Time), Query.EQ("設備名稱", dev_name), Query.EQ("工作類型", "加工")), new string[] { "產品名稱" }));
        }

        public string Get_returnQuestion_Now()
        {
            return String.Join(",", Get_DB_Info("_dek_RealTime_Info", Query.EQ("顯示資訊", "問題回報"), new string[] { "顯示與否" })[0]);
        }

        public List<string> Get_Alarm_List(DateTime start_time, DateTime end_time, string dev_name, string error_mesg)//取異警歷程
        {
            List<string> ST_DB_List = new List<string>();
            end_time = EndTime(end_time);

            var filter = Query.And(Query.NE("_id", 0), Query.GTE("enddate", start_time), Query.LTE("enddate", end_time), Query.EQ("L1Name", dev_name), Query.EQ("message", error_mesg));
            string[] col_name = { "L1Name", "L0Name", "type", "number", "message", "updatedate", "enddate", "timespan" };
            ST_DB_List = Get_DB_Info("Alarm_History", filter, col_name);
            //DateTime? dtnull = null;
            //filter = Query.And(Query.NE("_id", 0), Query.GTE("enddate", start_time), Query.EQ("enddate", ((BsonValue)dtnull).AsNullableDateTime), Query.EQ("L1Name", dev_name), Query.EQ("message", error_mesg));
            //ST_DB_List = Get_DB_Info("Alarm_History", filter, col_name);

            string Work_Staff = Get_DB_Info("_dek_Produce", Query.And(Query.NE("_id", 0), Query.LTE("上工時間", DateTime.Now.ToString("yyyyMMddHHmmss")), Query.EQ("設備名稱", dev_name), Query.EQ("工作類型", "加工")), new string[] { "執行人員" })[0];
            if (ST_DB_List[0] == "--")
                return ST_DB_List;
            if (Work_Staff == "--")
                Work_Staff = "";
            for (int iIndex = 0; iIndex < ST_DB_List.Count; iIndex++)
            {
                ST_DB_List[iIndex] += "," + Work_Staff;
            }
            return ST_DB_List;
        }

        public List<string> Get_AlarmInfo(DateTime start_time, DateTime end_time, string dev_name)//取異警歷程
        {   //有判斷null可修正用來運轉百分比判斷
            List<string> ST_Data_Total = new List<string>();
            end_time = EndTime(end_time);

            DateTime? dtnull = null;
            List<string> ST_DB_Data = Get_DB_Info("Alarm_History", Query.And(Query.GTE("updatedate", start_time), Query.LTE("enddate", end_time), Query.EQ("L1Name", dev_name)), new string[] { "L1Name", "type", "number", "message", "updatedate", "enddate", "timespan" });
            ST_DB_Data.AddRange(Get_DB_Info("Alarm_History", Query.And(Query.LTE("updatedate", start_time), Query.GTE("enddate", start_time), Query.LTE("enddate", end_time), Query.EQ("L1Name", dev_name)), new string[] { "L1Name", "type", "number", "message", "updatedate", "enddate", "timespan" }));

            List<string> ST_Data_Third = Get_DB_Info("Alarm_History", Query.And(Query.GTE("updatedate", start_time), Query.LTE("updatedate", end_time), Query.EQ("enddate", ((BsonValue)dtnull).AsNullableDateTime), Query.EQ("L1Name", dev_name)), new string[] { "L1Name", "type", "number", "message", "updatedate", "enddate", "timespan" });
            if (ST_Data_Third[0] != "--" && ST_Data_Third[0].Split(',')[5] == "null")
            {
                string ts = Math_Round(DataTableUtils.toDouble((DateTime.Now - Convert.ToDateTime(ST_Data_Third[0].Split(',')[4])).TotalSeconds.ToString()), 0).ToString();
                ST_Data_Third[0] = ST_Data_Third[0].Substring(0, ST_Data_Third[0].Length - 1).Replace("null", DateTime.Now.ToString()) + ts;
            }
            ST_DB_Data.AddRange(ST_Data_Third);

            ST_DB_Data.AddRange(Get_DB_Info("Alarm_History", Query.And(Query.LTE("updatedate", start_time), Query.GTE("enddate", end_time), Query.EQ("L1Name", dev_name)), new string[] { "L1Name", "type", "number", "message", "updatedate", "enddate", "timespan" }));

            foreach (string myStringList in ST_DB_Data)
            {
                if (myStringList == "--") continue;
                else
                    ST_Data_Total.Add(myStringList);
            }
            return ST_Data_Total;
        }

        public List<string> Get_DB_Info(string collection, IMongoQuery filter, string[] col_name)
        {
            List<string> ST_Info = new List<string>();
            var collection_Name = SelectDataTable(collection);
            var document = collection_Name.Find(filter);
            ST_Info = GetDataTable(document, col_name);
            if (ST_Info.Count == 0)
                ST_Info.Add("--");//用意：格式相同，防止第一次沒資料//表格使用//+-+
            return ST_Info;
        }

        public List<string> Get_DB_Info(string collection, MongoCursor<BsonDocument> document, string[] col_name)
        {
            List<string> ST_Info = new List<string>();
            var collection_Name = SelectDataTable(collection);
            ST_Info = GetDataTable(document, col_name);
            return ST_Info;
        }

        public List<string> Get_DB_Info_Web(string collection, IMongoQuery filter, string[] col_name)
        {
            List<string> ST_Info = new List<string>();
            var collection_Name = SelectDataTable(collection);
            var document = collection_Name.Find(filter);
            ST_Info = GetDataTable(document, col_name);
            return ST_Info;
        }

        public List<string> GetDataTable(MongoCursor<BsonDocument> document, string[] col_name)   //取多個 or 多個
        {
            List<string> ST_Data = new List<string>();
            foreach (var data_list in document)
            {
                string value = "";
                for (int iIndex = 0; iIndex < col_name.Length; iIndex++)
                {
                    if (col_name[iIndex] == "updatedate" || col_name[iIndex] == "enddate")
                    {
                        string dt_time = "";
                        string d_time = data_list.GetElement(col_name[iIndex]).Value.ToString();
                        if (d_time == "BsonNull")
                            dt_time = "null";
                        else if (d_time.Length >= 22)
                        {
                            string dt_strlength = "";
                            for (int iIndex_Num = 22; iIndex_Num <= d_time.Length; iIndex_Num++)
                                dt_strlength += "f";    //小數幾位補"f"
                            dt_time = DateTime.ParseExact(d_time, "yyyy-MM-ddTHH:mm:ss." + dt_strlength + "Z", System.Globalization.CultureInfo.CurrentCulture).ToString();
                        }
                        else
                            dt_time = DateTime.ParseExact(d_time, "yyyy-MM-ddTHH:mm:ssZ", System.Globalization.CultureInfo.CurrentCulture).ToString();
                        if (iIndex == col_name.Length - 1)
                            value += dt_time;
                        else
                            value += dt_time + ",";
                    }
                    else
                    {
                        if (col_name[iIndex] == "value" && data_list.GetElement(col_name[iIndex]).Value.ToString() == "BsonNull")
                        {
                            if (iIndex == col_name.Length - 1)
                                value += "--";
                            else
                                value += "--,";
                        }
                        else
                        {
                            if (iIndex == col_name.Length - 1)
                                value += data_list.GetElement(col_name[iIndex]).Value.ToString();
                            else
                                value += data_list.GetElement(col_name[iIndex]).Value.ToString() + ",";
                        }
                    }
                }
                ST_Data.Add(value);
            }
            return ST_Data;
        }

        public IMongoQuery Cond_filter(string[] name, string cond, string[] value)
        {
            IMongoQuery Cond_query = null, filter_query = null;
            for (int iIndex = 0; iIndex < name.Length; iIndex++)
            {
                if (cond == "=") Cond_query = Query.EQ(name[iIndex], value[iIndex]);
                else if (cond == "!=") Cond_query = Query.NE(name[iIndex], value[iIndex]);
                else if (cond == ">") Cond_query = Query.GT(name[iIndex], value[iIndex]);
                else if (cond == ">=") Cond_query = Query.GTE(name[iIndex], value[iIndex]);
                else if (cond == "<") Cond_query = Query.LT(name[iIndex], value[iIndex]);
                else if (cond == "<=") Cond_query = Query.LTE(name[iIndex], value[iIndex]);
                if (filter_query != null)
                    filter_query = Query.And(filter_query, Cond_query);
                else
                    filter_query = Query.And(Cond_query);
            }
            return filter_query;
        }

        public IMongoQuery Cond_filter(string[] name, string cond, int[] value)
        {
            IMongoQuery Cond_query = null, filter_query = null;
            for (int iIndex = 0; iIndex < name.Length; iIndex++)
            {
                if (cond == "=") Cond_query = Query.EQ(name[iIndex], value[iIndex]);
                else if (cond == "!=") Cond_query = Query.NE(name[iIndex], value[iIndex]);
                else if (cond == ">") Cond_query = Query.GT(name[iIndex], value[iIndex]);
                else if (cond == ">=") Cond_query = Query.GTE(name[iIndex], value[iIndex]);
                else if (cond == "<") Cond_query = Query.LT(name[iIndex], value[iIndex]);
                else if (cond == "<=") Cond_query = Query.LTE(name[iIndex], value[iIndex]);
                if (filter_query != null)
                    filter_query = Query.And(filter_query, Cond_query);
                else
                    filter_query = Query.And(Cond_query);
            }
            return filter_query;
        }

        public IMongoQuery Cond_filter(string[] name, string cond, double[] value)
        {
            IMongoQuery Cond_query = null, filter_query = null;
            for (int iIndex = 0; iIndex < name.Length; iIndex++)
            {
                if (cond == "=") Cond_query = Query.EQ(name[iIndex], value[iIndex]);
                else if (cond == "!=") Cond_query = Query.NE(name[iIndex], value[iIndex]);
                else if (cond == ">") Cond_query = Query.GT(name[iIndex], value[iIndex]);
                else if (cond == ">=") Cond_query = Query.GTE(name[iIndex], value[iIndex]);
                else if (cond == "<") Cond_query = Query.LT(name[iIndex], value[iIndex]);
                else if (cond == "<=") Cond_query = Query.LTE(name[iIndex], value[iIndex]);
                if (filter_query != null)
                    filter_query = Query.And(filter_query, Cond_query);
                else
                    filter_query = Query.And(Cond_query);
            }
            return filter_query;
        }

        public IMongoQuery Cond_filter(string[] name, string cond, DateTime[] value)
        {
            IMongoQuery Cond_query = null, filter_query = null;
            for (int iIndex = 0; iIndex < name.Length; iIndex++)
            {
                if (cond == "=") Cond_query = Query.EQ(name[iIndex], value[iIndex]);
                else if (cond == "!=") Cond_query = Query.NE(name[iIndex], value[iIndex]);
                else if (cond == ">") Cond_query = Query.GT(name[iIndex], value[iIndex]);
                else if (cond == ">=") Cond_query = Query.GTE(name[iIndex], value[iIndex]);
                else if (cond == "<") Cond_query = Query.LT(name[iIndex], value[iIndex]);
                else if (cond == "<=") Cond_query = Query.LTE(name[iIndex], value[iIndex]);
                if (filter_query != null)
                    filter_query = Query.And(filter_query, Cond_query);
                else
                    filter_query = Query.And(Cond_query);
            }
            return filter_query;
        }

        public DateTime EndTime(DateTime end_time)
        {
            if (end_time >= DateTime.Now)
                end_time = DateTime.Now;
            return end_time;
        }

        public List<string> Get_Status_List_Web(DateTime start_time, DateTime end_time, string dev_name)    //畫status bar
        {
            //判斷歷程
            string Start_time, Mach_status, Cycle_time, Update_time, Start_time_line, End_time_line;
            List<string> ST_Status = new List<string>();
            List<string> ST_Status_Curry = new List<string>();
            List<string> ST_Data = new List<string>();
            ST_Data = Get_First_Data(start_time, dev_name);

            end_time = EndTime(end_time);

            var filter = Query.And(Query.NE("_id", 0), Query.GTE("updatedate", start_time), Query.LTE("updatedate", end_time), Query.EQ("L1Name", dev_name));
            string[] col_name = { "L1Name", "value", "updatedate", "enddate", "timespan" };
            ST_Status = Get_DB_Info_Web("L1_Pool", filter, col_name);

            for (int iIndex = 0; iIndex < ST_Status.Count; iIndex++)
            {
                Update_time = Get_Update_Time_Web(ST_Status[iIndex]);
                Start_time = Get_Start_Time_Web(Update_time);
                Cycle_time = Get_Cycle_Time_Web(ST_Status[iIndex]);
                Mach_status = Get_Mach_Status_Web(ST_Status[iIndex]);
                Start_time_line = Update_time.Substring(4, 10);
                DateTime Dt_Start_time_line = DateTime.ParseExact(Update_time, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                End_time_line = Dt_Start_time_line.AddSeconds(int.Parse(ST_Status[iIndex].Split(',')[4].Split('.')[0])).ToString("yyyyMMddHHmmss").Substring(4, 10);

                ST_Data.Add("Update_time=" + Update_time + ",Start_time=" + Start_time + ",Cycle_time=" + Cycle_time + ",Nc_Status=" + Mach_status + ",Start_time_line=" + Start_time_line + ",End_time_line=" + End_time_line);
            }
            //判斷現在
            ST_Status_Curry = Get_DB_Info_Web("L1_Pool_Opened", filter, col_name);
            if (ST_Status_Curry.Count != 0)
            {
                Update_time = Get_Update_Time_Web(ST_Status_Curry[0]);
                Start_time = Get_Start_Time_Web(Update_time);
                DateTime dt_lasttime = Convert.ToDateTime(ST_Status_Curry[0].Split(',')[2]);
                Cycle_time = Math_Round_Minutes(Math_Round(end_time.Subtract(dt_lasttime).Duration().TotalSeconds, 0)).ToString();
                Mach_status = Get_Mach_Status_Web(ST_Status_Curry[0]);
                Start_time_line = Update_time.Substring(4, 10);
                DateTime Dt_Start_time_line = DateTime.ParseExact(Update_time, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                End_time_line = Dt_Start_time_line.AddSeconds(int.Parse(ST_Status_Curry[0].Split(',')[4].Split('.')[0])).ToString("yyyyMMddHHmmss").Substring(4, 10);
                if (End_time_line == Start_time_line)
                    End_time_line = end_time.ToString("MMddHHmmss");
                ST_Data.Add("Update_time=" + Update_time + ",Start_time=" + Start_time + ",Cycle_time=" + Cycle_time + ",Nc_Status=" + Mach_status + ",Start_time_line=" + Start_time_line + ",End_time_line=" + End_time_line);
            }
            else//若只有一筆，且持續發生
            {
                filter = Query.And(Query.NE("_id", 0), Query.LTE("updatedate", start_time), Query.EQ("L1Name", dev_name));
                ST_Status_Curry = Get_DB_Info_Web("L1_Pool_Opened", filter, col_name);
                if (ST_Status_Curry.Count != 0)
                {
                    string cycletimeSpan = "Cycle_time=" + Math_Round(end_time.Subtract(start_time).Duration().TotalMinutes, 2).ToString();
                    string ncstatus = ST_Status_Curry[0].Split(',')[1];
                    string data_str = "Update_time=" + start_time.ToString("yyyyMMddHHmmss") + ",Start_time=0," + cycletimeSpan + ",Nc_Status=" + ncstatus + ",Start_time_line=" + start_time.ToString("MMddHHmmss") + ",End_time_line=" + end_time.ToString("MMddHHmmss");
                    ST_Data.Add(data_str);
                }
            }
            return ST_Data;
        }

        public List<string> Get_First_Data(DateTime start_time, string dev_name)
        {
            string Update_time, Mach_status, Cycle_time_sec;
            List<string> ST_Status = new List<string>();
            List<string> ST_Data = new List<string>();

            var collection = SelectDataTable("L1_Pool");
            var filter = Query.And(Query.NE("_id", 0), Query.GTE("enddate", start_time), Query.LTE("updatedate", start_time), Query.EQ("L1Name", dev_name));
            MongoCursor<BsonDocument> document = collection.Find(filter).SetSortOrder(SortBy.Descending("enddate"));
            string[] col_name = { "L1Name", "value", "updatedate", "enddate", "timespan" };
            ST_Status = Get_DB_Info("L1_Pool", document, col_name);

            if (ST_Status.Count != 0)
            {
                Update_time = start_time.ToString("yyyyMMddHHmmss");
                Cycle_time_sec = Math_Round(Convert.ToDateTime(ST_Status[0].Split(',')[3]).Subtract(start_time).Duration().TotalSeconds, 0).ToString();
                Mach_status = ST_Status[0].Split(',')[1];

                ST_Data.Add("Cycle_time=" + Cycle_time_sec + ",Nc_Status=" + Mach_status + ",update_dt=" + Update_time);
            }
            return ST_Data;
        }

        public string Get_Update_Time_Web(string Data)
        {
            return Convert.ToDateTime(Data.Split(',')[2]).ToString("yyyyMMddHHmmss");
        }

        public string Get_Start_Time_Web(string Data)
        {
            int second = int.Parse(Data.Substring(12, 2)) + int.Parse(Data.Substring(10, 2)) * 60 + int.Parse(Data.Substring(8, 2)) * 60 * 60;
            return Math_Round_Minutes(second).ToString();
        }

        public string Get_Cycle_Time_Web(string Data)
        {
            int second = int.Parse(Data.Split(',')[4].Split('.')[0]);
            return Math_Round_Minutes(second).ToString();
        }

        public string Get_Mach_Status_Web(string Data)
        {
            return Data.Split(',')[1];
        }

        public double Math_Round(double Val, int Point)
        {
            return Math.Round(Val, Point, MidpointRounding.AwayFromZero);
        }

        public double Math_Round_Minutes(double Sec)
        {
            return Math.Round(new TimeSpan(0, 0, DataTableUtils.toInt(Sec.ToString())).TotalMinutes, 2, MidpointRounding.AwayFromZero);
        }

        //public void update_MongoDB_Data(string collection_name, IMongoQuery filter, UpdateBuilder update)//條件相同只更新一筆
        //{
        //    var filter1 = Query.And(Query.EQ("GroupName", "洗床"));
        //    var update1 = MongoDB.Driver.Builders.Update.Set("ProductName", "test");
        //    SelectDataTable(collection_name).Update(filter, update);
        //}

        public void RemoveAll_MongoDB_Data(string collection_name)
        {
            SelectDataTable(collection_name).RemoveAll();
        }

        public void Remove_MongoDB_Data(string collection_name)//條件相同全刪除(demo)
        {
            var filter = Query.EQ("GroupName", "洗床");
            SelectDataTable(collection_name).Remove(filter);
        }

        //---------------以下顧問方法
        public long Update_MongoDB_Data(string filter_json, string updata_json, bool first_match_only = true)
        {
            BsonDocument filter_data = getFilter(filter_json);
            BsonDocument updata_data = getNewData(updata_json);
            return Update_MongoDB_Data_Bson(filter_data, updata_data, first_match_only);
        }

        public long Update_MongoDB_Data_Bson(BsonDocument filter, BsonDocument doc, bool first_match_only = true) //true = 只改第一筆
        {
            if (_Collection != null)
            {
                ErrorMessage = "";
                try
                {
                    BsonDocument data = new BsonDocument("$set", doc);
                    UpdateOptions opts = new UpdateOptions { IsUpsert = false };
                    UpdateResult result;
                    if (first_match_only)
                        result = _Collection.UpdateOne(filter, data, opts);
                    else result = _Collection.UpdateMany(filter, data, opts);
                    return result.ModifiedCount;
                }
                catch (Exception ex) { ErrorMessage = ex.Message; }
            }
            return 0;
        }

        public void Delete_MongoDB_AllData(string collection_name)//wswang 20190716++
        {
            MongoCollection collection = database.GetCollection<BsonDocument>(collection_name);
            collection.RemoveAll();
        }

        public long Delete_MongoDB_Data(string filter_json, bool first_match_only = true)
        {
            BsonDocument filter_data = getFilter(filter_json);
            return Delete_MongoDB_Data_Bson(filter_data, first_match_only);
        }

        public long Delete_MongoDB_Data_Bson(BsonDocument filter, bool first_match_only = true)
        {
            if (_Collection != null)
            {
                ErrorMessage = "";
                try
                {
                    DeleteResult result;
                    if (first_match_only)
                        result = _Collection.DeleteOne(filter);
                    else result = _Collection.DeleteMany(filter);
                    return result.DeletedCount;
                }
                catch (Exception ex) { ErrorMessage = ex.Message; }
            }
            return 0;
        }

        public bool Insert_MongoDB_Data_Bson(BsonDocument doc)
        {
            if (_Collection != null)
            {
                ErrorMessage = "";
                try
                {
                    _Collection.InsertOne(doc);
                    return true;
                }
                catch (Exception ex) { ErrorMessage = ex.Message; }
            }
            return false;
        }

        public List<string> Find_MongoDB_Data(string filter_json)
        {
            BsonDocument filter_data = getFilter(filter_json);
            List<BsonDocument> list = Find_MongoDB_Data_Bson(filter_data);
            return ListRecords(list);
        }

        public List<BsonDocument> Find_MongoDB_Data_Bson(BsonDocument filter, BsonDocument sort_by = null, int from_index = 0, int max_count = 0)
        {
            List<BsonDocument> result = new List<BsonDocument>();
            if (_Collection != null)
            {
                ErrorMessage = "";
                try
                {
                    if (filter == null) filter = new BsonDocument();
                    var query = _Collection.Find(filter);
                    if (from_index > 0) query = query.Skip(from_index);
                    if (max_count > 0) query = query.Limit(max_count);
                    if (sort_by != null) query = query.Sort(sort_by);
                    return getQueryList(query.ToCursor());
                }
                catch (Exception ex) { ErrorMessage = ex.Message; }
            }
            return result;
        }

        public List<BsonDocument> FindAll_MongoDB_Data_Bson()
        {   //資料表全找
            BsonDocument sort = new BsonDocument();
            sort.Set("name", MongoSortType.Ascending);//先排name,遞增
            sort.Set("age", MongoSortType.Descending);//後排age,遞減            
            BsonDocument filter = null;//new BsonDocument();//空的內容，表示全部列出
            return Find_MongoDB_Data_Bson(filter, sort);
        }

        List<BsonDocument> getQueryList(IAsyncCursor<BsonDocument> cursor)
        {
            List<BsonDocument> result = new List<BsonDocument>();
            if (cursor != null)
            {
                while (cursor.MoveNext())
                {
                    result.AddRange(cursor.Current);
                }
            }
            return result;
        }

        public List<string> ListRecords(List<BsonDocument> list)
        {
            List<string> ST_DB_List = new List<string>();
            foreach (var obj in list)
                ST_DB_List.Add(obj.ToString());
            return ST_DB_List;
        }

        public BsonDocument getFilter(string filter_data)
        {
            string json = filter_data.Trim();
            BsonDocument filter = JsonToBsonDocument(json);
            return filter;
        }

        public BsonDocument getNewData(string update_data)
        {
            string json = update_data.Trim();
            BsonDocument doc = JsonToBsonDocument(json);
            return doc;
        }

        public static BsonDocument JsonToBsonDocument(string json)
        {
            try
            {
                return MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(json);
            }
            catch { }
            return new BsonDocument();
        }

        public string Bson_Merge_Account(string Account = "", string Password = "", string Inherit_PermGroup = "", string LoginTime = "", string Add_Account = "", string InsertTime = "", string UpdateTime = "")
        {
            string Bson_str = "{'帳號':'" + Account + "','密碼':'" + Password + "','權限群組':'" + Inherit_PermGroup + "','登入時間':'" + LoginTime + "','新增人員':'" + Add_Account + "','新增時間':'" + InsertTime + "','更新時間':'" + UpdateTime + "'}";
            return Bson_str;
        }

        public string Bson_Merge_Dev(string DevType = "", string DevGroup = "", string Add_Account = "", string InsertTime = "", string UpdateTime = "")
        {
            string Bson_str = "{'設備類型':'" + DevType + "','設備群組':'" + DevGroup + "','新增人員':'" + Add_Account + "','新增時間':'" + InsertTime + "','更新時間':'" + UpdateTime + "'}";
            return Bson_str;
        }

        public string Bson_Merge_DevGroup(string DevGroup = "", string DevName = "", string Add_Account = "", string InsertTime = "", string UpdateTime = "")
        {
            string Bson_str = "{'設備群組':'" + DevGroup + "','設備名稱':'" + DevName + "','新增人員':'" + Add_Account + "','新增時間':'" + InsertTime + "','更新時間':'" + UpdateTime + "'}";
            return Bson_str;
        }

        public string Bson_Merge_Permission(string Permission_Name = "", string Add_Account = "", string InsertTime = "", string UpdateTime = "")
        {
            string Bson_str = "{'權限名稱':'" + Permission_Name + "','新增人員':'" + Add_Account + "','新增時間':'" + InsertTime + "','更新時間':'" + UpdateTime + "'}";
            return Bson_str;
        }

        public string Bson_Merge_PermissionGroup(string PermissionGroup_Name = "", string Permission_Name = "", string Add_Account = "", string InsertTime = "", string UpdateTime = "")
        {
            string Bson_str = "{'權限群組名稱':'" + PermissionGroup_Name + "','權限名稱':'" + Permission_Name + "','新增人員':'" + Add_Account + "','新增時間':'" + InsertTime + "','更新時間':'" + UpdateTime + "'}";
            return Bson_str;
        }

        public string Bson_Merge_Custom(string CustomName = "", string CustomTel = "", string CustomFax = "", string CustomMail = "", string CustomAddr = "", string Add_Account = "", string InsertTime = "", string UpdateTime = "")
        {
            string Bson_str = "{'客戶名稱':'" + CustomName + "','聯繫電話':'" + CustomTel + "','傳真電話':'" + CustomFax + "','電子郵件':'" + CustomMail + "','聯繫地址':'" + CustomAddr + "','新增人員':'" + Add_Account + "','新增時間':'" + InsertTime + "','更新時間':'" + UpdateTime + "'}";
            return Bson_str;
        }

        public string Bson_Merge_WorkTime(string WorkTimeName = "", string MorningStart = "", string MorningEnd = "", string AfternoonStart = "", string AfternoonEnd = "", string NightStart = "", string NightEnd = "", string Execute_Date = "", string Add_Account = "", string InsertTime = "", string UpdateTime = "")
        {
            string Bson_str = "{'工時定義':'" + WorkTimeName + "','上午起始時間':'" + MorningStart + "','上午結束時間':'" + MorningEnd + "','下午起始時間':'" + AfternoonStart + "','下午結束時間':'" + AfternoonEnd + "','晚上起始時間':'" + NightStart + "','晚上結束時間':'" + NightEnd + "','執行日':'" + Execute_Date + "','新增人員':'" + Add_Account + "','新增時間':'" + InsertTime + "','更新時間':'" + UpdateTime + "'}";
            return Bson_str;
        }

        public string Bson_Merge_Product(string ProductName = "", string ProgramName = "", string WorkMinTime = "", string WorkMaxTime = "", string FeedTime = "", string WorkCash = "", string Cash = "", string DevGroup = "", string DevType = "", string DevName = "", string Add_Account = "", string InsertTime = "", string UpdateTime = "", string ProdNo = "")
        {
            string Bson_str = "{'產品名稱':'" + ProductName + "','程式名稱':'" + ProgramName + "','加工時間':'" + WorkMinTime + "','校模時間':'" + WorkMaxTime + "','換料時間':'" + FeedTime + "','加工成本':'" + WorkCash + "','單價':'" + Cash + "','設備群組':'" + DevGroup + "','設備類型':'" + DevType + "','設備名稱':'" + DevName + "','新增人員':'" + Add_Account + "','新增時間':'" + InsertTime + "','更新時間':'" + UpdateTime + "','料件編號':'" + ProdNo + "'}";
            return Bson_str;
        }

        public string Bson_Merge_Produce(string Account = "", string WorkType = "", string DevGroup = "", string DevType = "", string DevName = "", string CustomName = "", string ProductName = "", string StartTime = "", string EndTime = "", string PreProductCount = "", string Add_Account = "", string InsertTime = "", string UpdateTime = "", string ProdNo = "")
        {
            string Bson_str = "{'執行人員':'" + Account + "','工作類型':'" + WorkType + "','設備群組':'" + DevGroup + "','設備類型':'" + DevType + "','設備名稱':'" + DevName + "','客戶名稱':'" + CustomName + "','產品名稱':'" + ProductName + "','上工時間':'" + StartTime + "','完工時間':'" + EndTime + "','預計生產量':'" + PreProductCount + "','新增人員':'" + Add_Account + "','新增時間':'" + InsertTime + "','更新時間':'" + UpdateTime + "','料件編號':'" + ProdNo + "'}";
            return Bson_str;
        }

        public string Bson_Merge_LineScepter(string LineScepter = "", string AskTime = "", string Is_Update = "", string Add_Account = "", string InsertTime = "", string UpdateTime = "")
        {
            string Bson_str = "{'權杖編碼':'" + LineScepter + "','輪詢時間':'" + AskTime + "','是否輪詢':'" + Is_Update + "','新增人員':'" + Add_Account + "','新增時間':'" + InsertTime + "','更新時間':'" + UpdateTime + "'}";
            return Bson_str;
        }
        
        public List<string> Get_Mach_Type()
        {
            return Get_DB_Info("_dek_Dev", Query.NE("_id", 0), new string[] { "設備類型" });
        }
        public List<string> Get_Mach_Group(string MachType)
        {
            return Get_DB_Info("_dek_Dev", Query.And(Query.NE("_id", 0), Query.EQ("設備類型", MachType)), new string[] { "設備群組" })[0].Split(',').ToList();
        }

        public List<string> Get_Mach_List(string Group)
        {
            return Get_DB_Info("_dek_DevGroup", Query.EQ("設備群組", Group), new string[] { "設備名稱" });
        }

        public List<string> Get_Mach_List()
        {
            return Get_DB_Info("_dek_Dev_Group", Query.NE("_id", 0), new string[] { "設備名稱" });
        }

        public void Insert_Dev_GroupName(string DevType, string DevGroup, string DevName, string InsertTime, bool ReadData)
        {
            bool is_ok = false;
            SelectDataTable_IMongo("_dek_Dev_Group");
            string json = "{'設備類型':'" + DevType + "','設備群組':'" + DevGroup + "','設備名稱':'" + DevName + "','新增時間':'" + InsertTime + "','讀取資料':'" + ReadData + "'}";

            BsonDocument insert_data = getNewData(json);
            if (insert_data.Count() > 0)
                is_ok = Insert_MongoDB_Data_Bson(insert_data);
        }
    }
}