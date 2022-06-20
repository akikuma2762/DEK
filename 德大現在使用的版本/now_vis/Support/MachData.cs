using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Support
{
    public class MachData
    {
        public static string DateTime_String(DateTime dt)
        {
            return dt.ToString("yyyyMMddHHmmss");
        }
        public static string DateTime_Now()
        {
            return DateTime_String(DateTime.Now);
        }

        public static DateTime DateTime_FormString(string time_str)
        {
            try
            {
                return DateTime.ParseExact(time_str, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                return DateTime.Now;
            }
        }
        //--------------------------------------------------------------

        public static Dictionary<string, string> TransToCN(string where, string what)
        {
            Dictionary<string, string> list = new Dictionary<string, string>();
            //廠區轉中文
            try
            {
                if (where == "All") where = "整廠";
                else
                {
                    DataTableUtils.UseDB_Change(DB_NAME.DB_VisMach);
                    string strCmd = "SELECT * FROM Factory_Group where FAC_GP_ID = '" + where + "'";
                    DataTable dt = DataTableUtils.GetDataTable(strCmd);
                    where = dt.Rows[0]["fac_gp_name"].ToString();
                    dt.Dispose();
                    DataTableUtils.UseDB_Restore();
                }
                list.Add("where", where);
                //統計圖轉中文
                switch (what)
                {
                    case "OEE":
                        what = "稼動率";
                        break;
                    case "PCM":
                        what = "完工率";
                        break;
                    case "WGR":
                        what = "警告率";
                        break;
                }
                list.Add("what", what);
            }
            catch
            {

            }
            return list;
        }

        //===========================================================
        public static DataTable GetTableRow(string table, string sql_condition)
        {
            DataTableUtils.UseDB_Change(DB_NAME.DB_VisMach);
            string sql = "select * from " + table;
            if (sql_condition.Trim() != "") sql += " where " + sql_condition;
            DataTable dt = DataTableUtils.DataTable_GetRow(sql);
            DataTableUtils.UseDB_Restore();
            return dt;
        }

        public static List<string> MachIDList_取得機台群組編號串列(string 機台群組編號 = "") //空字串代表全部機台
        {
            DataTableUtils.UseDB_Change(DB_NAME.DB_VisMach);
            List<string> id_list = new List<string>();
            string sqlcmd = "SELECT * FROM Mach_Info";
            if (機台群組編號 != "")
            {
                sqlcmd += " where FAC_GP_ID ='" + 機台群組編號 + "'";
            }
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
            //"FB20171122110922-96255,東181"
            //"FB20171122111323-30296,東185"
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sqlcmd = dt.Rows[i]["Mach_ID"].ToString() + "," + dt.Rows[i]["Mach_ShortName"].ToString();
                id_list.Add(sqlcmd);
            }
            dt.Dispose(); // !!free DataTable!!
            DataTableUtils.UseDB_Restore();
            return id_list;
        }
        
        public static DataTable MachInfo_機台資料(string machine_id)
        {
            return GetTableRow("Mach_Info", "Mach_ID = '" + machine_id + "'");
        }

        public static DataTable MachInfo_機台作業狀態(string machine_id)
        {
            return GetTableRow("NC_Register", "Mach_ID = '" + machine_id + "'");
        }

        public static DataTable MachInfo_機台生產資料(string machine_id)
        {
            return GetTableRow("MakeOrder_Process_Detail", "Mach_ID = '" + machine_id + "'"); ;
        }

        public static string MachInfo_機台保養資料(string machine_id,string 資料欄位名稱)
        {
            //Mach_PM_Rrecord: [PMR_Index],[Mach_ID],[PM_Item_ThisTIme],
            //                 [ThisTIme],[PM_Item_NextTime],[NextTime],[update_dt]

            DataTable dt =GetTableRow("Mach_PM_Rrecord", "Mach_ID = '" + machine_id + "'");
            if (dt.Rows.Count <= 0) return "";
            string str = DataTableUtils.DataRow_ColumntoString(dt, 0, 資料欄位名稱, "");
            if (str=="") str = "尚未排定";
            else str = long.Parse(str).ToString("0000/00/00 00:00");
            dt.Dispose();
            return str;
        }
        //----------------------------------------------------------
        public static string MachInfo_稼動率(string mach_ID, string start_time, string end_time)
        {
            DataTableUtils.UseDB_Change(DB_NAME.DB_VisMach);
            string str,table_name = "NC_WorkProcess_Detail";
            //start_time = "20170101000000";
            //-----------------------------------
            //查詢run time 和 idle time
            string sqlcmd =
                string.Format("select Mach_ID, SUM(NC_Run_Sec) as RunTime, SUM(NC_Idle_Sec) as IdleTime" +
                            " from {0}" +
                            " where Start_DT between '{1}' and  '{2}'" +
                            " and End_DT != '' and Mach_ID='{3}'" +
                            " GROUP BY Mach_ID",
                            table_name, start_time, end_time, mach_ID);
            DataTable dt = DataTableUtils.DataTable_GetRow(sqlcmd);
            long 加工時間 = 0;
            long 上下料時間 = 0;
            if (dt.Rows.Count != 0)
            {
                str = DataTableUtils.DataRow_ColumntoString(dt.Rows[0], "RunTime");//加工時間 (秒)
                加工時間 = DataTableUtils.toLong(str);
                str = DataTableUtils.DataRow_ColumntoString(dt.Rows[0], "IdleTime");//上下料 (秒)
                上下料時間 = DataTableUtils.toLong(str);
            }
            dt.Dispose();
            DateTime 起始時間 = DateTime_FormString(start_time);
            DateTime 結束時間 = DateTime_FormString(end_time);
            TimeSpan tmsp = 結束時間 - 起始時間;
            int 稼動率 = 0;
            if (tmsp.TotalSeconds > 0)
                稼動率 = (int)((加工時間 + 上下料時間) * 1000 / tmsp.TotalSeconds);
            DataTableUtils.UseDB_Restore();
            return (稼動率/10.0).ToString();
        }

        public static string MachInfo_警告率(string mach_ID, string start_time, string end_time)
        {
            DataTableUtils.UseDB_Change(DB_NAME.DB_VisMach);
            string table = "NC_Register_log";
            string sql_cond=string.Format("Mach_ID = '{0}' " +
                        "and update_dt between '{1}' and '{2}'",
                        mach_ID, start_time, end_time);
            //---------------------------------------------------
            int All = DataTableUtils.RowCount(table, sql_cond);
            string warning = "0";
            if (All != 0)
            {
                int erorr = DataTableUtils.RowCount(table, sql_cond + " and Nc_Status = '2'");
                warning = (erorr * 100 / All).ToString(); //警告率百分比
            }
            DataTableUtils.UseDB_Restore();
            return warning;
        }

        

        //====================================================
        public static string[] GetValue(List<string> mach_id_list, string what, DateTime time_start, DateTime time_end)
        {
            string time_start_str = MachData.DateTime_String(time_start);
            string time_end_str = MachData.DateTime_String(time_end);
            string[] values = new string[mach_id_list.Count];
            string[] str_tab;
            for (int i = 0; i < mach_id_list.Count; i++)
            {
                //[0] = "FB20171122110922-96255,東181"
                str_tab = mach_id_list[i].Split(',');
                if (what == "OEE")
                    values[i] = str_tab[1] + "," + MachData.MachInfo_稼動率(str_tab[0], time_start_str, time_end_str);
                else
                    values[i] = str_tab[1] + "," + MachData.MachInfo_警告率(str_tab[0], time_start_str, time_end_str);
            }
            return values;

        }
    }
}
