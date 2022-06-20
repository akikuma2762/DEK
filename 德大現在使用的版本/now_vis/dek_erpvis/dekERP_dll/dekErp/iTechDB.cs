using MongoDB.Bson;
using Support;
using Support.DB;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dekERP_dll.dekErp
{
    public class iTechDB
    {
        IniManager iniManager = new IniManager(ConfigurationManager.AppSettings["ini_road"]);

        string GetDBLink = "";
        //舊的方式(for 不合併情況)
        public DataTable Get_DataTable(string Sqlcmd)
        {
            //伺服器名稱
            string IP = "192.168.1.26";
            //資料庫名稱
            string DB = "erp";
            //伺服器帳號
            string ACC = "jroot";
            //伺服器密碼
            string PWD = "erp89886066";


            GetDBLink = clsDB_Server.GetConntionString_MySQL(IP, DB, ACC, PWD);

            GlobalVar.UseDB_setConnString(GetDBLink);
            DataTable dt = Error_DataTable(DataTableUtils.GetDataTable(Sqlcmd), Sqlcmd, GetDBLink, DataTableUtils.ErrorMessage);
            return dt;

        }

        //新的方式(for 合併方便)
        public DataTable Get_DataTable(string sqlcmd, string source)
        {
            //讀取相對應的INI
            iniManager = new IniManager($"{ConfigurationManager.AppSettings["ini_local"]}{source}Erp.ini");
            string code_name = dekSecure.dekHashCode.SHA256("dek54886961deta1115");
            string DB_Type = iniManager.ReadIniFile("db_name", "inikey", "");
            string GetDBLink = dekSecure.dekEncDec.Decrypt(iniManager.ReadIniFile(dekSecure.dekEncDec.Encrypt(DB_Type, code_name), "inikey1", ""), code_name);

            //伺服器名稱
            string IP = dekSecure.dekEncDec.Decrypt(iniManager.ReadIniFile(dekSecure.dekEncDec.Encrypt(DB_Type, code_name), "inikey2", ""), code_name);
            //資料庫名稱
            string DB = DB_Type;
            //伺服器帳號
            string ACC = dekSecure.dekEncDec.Decrypt(iniManager.ReadIniFile(dekSecure.dekEncDec.Encrypt(DB_Type, code_name), "inikey3", ""), code_name);
            //伺服器密碼
            string PWD = dekSecure.dekEncDec.Decrypt(iniManager.ReadIniFile(dekSecure.dekEncDec.Encrypt(DB_Type, code_name), "inikey4", ""), code_name);

            GetDBLink = GetDBLink == "MsSQL" ? clsDB_Server.GetConntionString_MsSQL(IP, DB, ACC, PWD) : clsDB_Server.GetConntionString_MySQL(IP, DB, ACC, PWD);
            GlobalVar.UseDB_setConnString(GetDBLink);

            return ConfigurationManager.AppSettings["Get_Normal"] == "1" ? Error_DataTable(DataTableUtils.GetDataTable(sqlcmd), sqlcmd, GetDBLink, DataTableUtils.ErrorMessage) : Error_DataTable(null, sqlcmd, GetDBLink, DataTableUtils.ErrorMessage);
        }

        //取得資料表失敗時，須回傳語法(驗證)，連接(確認)，錯誤訊息(獲得)，結構是否存在(判斷)
        public DataTable Error_DataTable(DataTable dt, string sqlcmd, string link = "", string error_msg = "")
        {
            if (dt != null && dt.Rows.Count > 0)
                return dt;
            else
            {
                dt = new DataTable();
                dt.Columns.Add("語法");
                dt.Columns.Add("連接");
                dt.Columns.Add("錯誤訊息");
                dt.Columns.Add("結構是否存在");
                dt.Rows.Add(sqlcmd, link, error_msg, dt != null ? "Y" : "N");
                return dt;
            }
        }
    }
}
