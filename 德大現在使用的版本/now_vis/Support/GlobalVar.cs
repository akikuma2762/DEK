using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Support
{
    public enum DB_NAME { DB_VisMach, DB_VisData, DB_VisAssm, DB_deta_sw };
    public class GlobalVar
    {
        public static string dbVisMach_ConnStr = "";
        public static string dbVisData_ConnStr = "";
        public static string dbVisAssm_ConnStr = "";
        public static string dbTrsSowon_ConnStr = "";
        //-------------------------------------------
        //宣告全域變數
        public static bool Login_status = true;
        public static bool Conn_status = false;
        public static bool LoginCheck = false;
        public static string UserID = "";
        public static string DBServer_IP = "";
        public static int DBServer_Port = 0;
        public static string user_name = "";
        public static string user_pass = "";

        public static void Open()
        {
            SetupIniIP ini = new SetupIniIP("Config.ini");

            dbVisMach_ConnStr = getDBConnStr(ini, "Con_Mach", "dekVis");
            dbVisData_ConnStr = getDBConnStr(ini, "Con_Data", "dekVisData");
            dbVisAssm_ConnStr = getDBConnStr(ini, "Con_Assm", "dekVisAssm");
            dbTrsSowon_ConnStr = getDBConnStr(ini, "Con_deta", "FJWSQL");
            //UseDB(DB_NAME.DB_VisMach);
        }


        public static void Close()
        {
        }
        private static string getDBConnStr(SetupIniIP ini, string key, string db_name, int timeout = 15)
        {
            string con_fmt = "Data Source={0},{1};Initial Catalog={2};Integrated Security=false;" +
                            "User Id={3};Password={4};MultipleActiveResultSets=True";
            if (timeout != 15)
            {
                con_fmt += string.Format(";Connection Timeout = {0}", timeout);
            }
            string conn_str = ini.ReadString("DB", key, "");
            if (conn_str == "")
                conn_str = string.Format(con_fmt, DBServer_IP, DBServer_Port, db_name, user_name, user_pass);
            else if (conn_str.Contains(";") != true)
                conn_str = Support.Encrypt.DecryptString(conn_str, db_name);
            return conn_str;
        }

        static string dbCurrent_ConnStr = "";
        public static string UseDB_getConnString() { return dbCurrent_ConnStr; }//...................
        public static void UseDB_setConnString(string conn_str) { dbCurrent_ConnStr = conn_str; }
        public static void UseDB(DB_NAME name)
        {
            if (name == DB_NAME.DB_VisMach) UseDB_setConnString(dbVisMach_ConnStr);
            else if (name == DB_NAME.DB_VisData) UseDB_setConnString(dbVisData_ConnStr);
            else if (name == DB_NAME.DB_VisAssm) UseDB_setConnString(dbVisAssm_ConnStr);
            else if (name == DB_NAME.DB_deta_sw) UseDB_setConnString(dbTrsSowon_ConnStr);
        }

    }
}
