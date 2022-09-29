using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using Support;

namespace dek_erpvis_v2.cls
{
    public class myclass
    {
        //---------------------------------------------------正式區--------------------------------------------------------------------------//
        ////首旺相關連線
        //public static string GetConnByDetaSowon = clsDB_Server.GetConntionString_MsSQL("192.168.1.210", "FJWSQL", "dek", "asus54886961");
        //public static string GetConnByDetaEip = clsDB_Server.GetConntionString_MsSQL("192.168.1.210", "Eip", "dek", "asus54886961");
        ////德科ERP的連線
        //public static string GetConnByDekERPDataTable = clsDB_Server.GetConntionString_MySQL("192.168.1.26", "erp", "jroot", "erp89886066");



        ////加工可視化連線
        //public static string GetConnByDekVisCnc_inside = clsDB_Server.GetConntionString_MySQL("192.168.1.221", "cnc_db", "erp", "erp89886066");
        //public static string GetConnByDekVisCnc_outside = clsDB_Server.GetConntionString_MySQL("192.168.1.221", "cnc_db", "erp", "erp89886066");

        ////MSSQL 用戶基本資料連線
        //public static string GetConnByDekVisErp = clsDB_Server.GetConntionString_MsSQL("192.168.1.46,5872", "dekVisErp", "sa", "asus54886961");
        ////立式廠連線
        //public static string GetConnByDekdekVisAssm = clsDB_Server.GetConntionString_MsSQL("192.168.1.46,5872", "dekVisAssm", "sa", "asus54886961");
        ////臥式廠連線
        //public static string GetConnByDekdekVisAssmHor = clsDB_Server.GetConntionString_MsSQL("192.168.1.46,5872", "detaVisHor", "sa", "asus54886961");
        ////電控產線連線
        //public static string GetConnByDetaELine = clsDB_Server.GetConntionString_MsSQL("192.168.1.46,5872", "DetaELine", "sa", "asus54886961");

        //---------------------------------------------------正式區結束--------------------------------------------------------------------------//


        //------------------------------------------------20220929新測試區&正式區----------------------------------------------------------------------//
        public static string test_mode = WebUtils.GetAppSettings("test_mode");
        //首旺相關連線
        public static string GetConnByDetaSowon = test_mode == "Y"? clsDB_Server.GetConntionString_MsSQL("192.168.1.210", "TEST", "dek", "asus54886961"): clsDB_Server.GetConntionString_MsSQL("192.168.1.210", "FJWSQL", "dek", "asus54886961");
        public static string GetConnByDetaEip = test_mode == "Y" ? clsDB_Server.GetConntionString_MsSQL("192.168.1.210", "Eip", "dek", "asus54886961"): clsDB_Server.GetConntionString_MsSQL("192.168.1.210", "Eip", "dek", "asus54886961");
        //德科ERP的連線
        public static string GetConnByDekERPDataTable = test_mode == "Y" ? clsDB_Server.GetConntionString_MySQL("192.168.1.26", "erp_new1", "jroot", "erp89886066"): clsDB_Server.GetConntionString_MySQL("192.168.1.26", "erp", "jroot", "erp89886066");


        //加工可視化連線
        public static string GetConnByDekVisCnc_inside = test_mode == "Y" ? clsDB_Server.GetConntionString_MySQL("192.168.1.221", "cnc_db", "erp", "erp89886066"): clsDB_Server.GetConntionString_MySQL("192.168.1.221", "cnc_db", "erp", "erp89886066");
        public static string GetConnByDekVisCnc_outside = test_mode == "Y" ? clsDB_Server.GetConntionString_MySQL("192.168.1.221", "cnc_db", "erp", "erp89886066") : clsDB_Server.GetConntionString_MySQL("192.168.1.221", "cnc_db", "erp", "erp89886066");

        //MSSQL 用戶基本資料連線
        public static string GetConnByDekVisErp = test_mode == "Y" ? clsDB_Server.GetConntionString_MsSQL("192.168.1.46,5872", "dekVisErp_VM", "sa", "asus54886961"): clsDB_Server.GetConntionString_MsSQL("192.168.1.46,5872", "dekVisErp", "sa", "asus54886961");
        //立式廠連線
        public static string GetConnByDekdekVisAssm = test_mode == "Y" ? clsDB_Server.GetConntionString_MsSQL("192.168.1.46,5872", "dekVisAssm_VM", "sa", "asus54886961"): clsDB_Server.GetConntionString_MsSQL("192.168.1.46,5872", "dekVisAssm", "sa", "asus54886961");
        //臥式廠連線
        public static string GetConnByDekdekVisAssmHor = test_mode == "Y" ? clsDB_Server.GetConntionString_MsSQL("192.168.1.46,5872", "detaVisHor_VM", "sa", "asus54886961"): clsDB_Server.GetConntionString_MsSQL("192.168.1.46,5872", "detaVisHor", "sa", "asus54886961");
        //電控產線連線
        public static string GetConnByDetaELine = test_mode == "Y" ? clsDB_Server.GetConntionString_MsSQL("192.168.1.46,5872", "DetaELine_VM", "sa", "asus54886961"): clsDB_Server.GetConntionString_MsSQL("192.168.1.46,5872", "DetaELine", "sa", "asus54886961");

        //------------------------------------------------20220929新測試區&正式區結束------------------------------------------------------------------//

        //------------------------------------------------------舊測試區------------------------------------------------------------------------------------//
        //////測試區(舊)->連到文心的電腦
        ////MSSQL 用戶基本資料連線
        //public static string GetConnByDekVisErp = clsDB_Server.GetConntionString_MsSQL("172.23.10.106,1433", "dekVisErpMirco", "sa", "dek1234");
        ////加工可視化連線
        //public static string GetConnByDekVisCnc_inside = clsDB_Server.GetConntionString_MySQL("172.23.10.106", "cnc_db", "root", "asus54886961");
        //public static string GetConnByDekVisCnc_outside = clsDB_Server.GetConntionString_MySQL("172.23.10.106", "cnc_db", "root", "asus54886961");
        ////立式廠連線
        //public static string GetConnByDekdekVisAssm = clsDB_Server.GetConntionString_MsSQL("172.23.10.106,1433", "dekVisAssm", "sa", "dek1234");
        ////臥式廠連線
        //public static string GetConnByDekdekVisAssmHor = clsDB_Server.GetConntionString_MsSQL("172.23.10.106,1433", "detaVisHor", "sa", "dek1234");
        ////電控產線連線
        //public static string GetConnByDetaELine = clsDB_Server.GetConntionString_MsSQL("172.23.10.106,1433", "DetaELine", "sa", "dek1234");
        //------------------------------------------------------舊測試區結束----------------------------------------------------------------------------------//


        //鉅茂的連線
        public static string GetConnByDataWin = clsDB_Server.GetConntionString_MsSQL("192.168.3.4", "DATAWIN", "sa", "16899");
        //APS連線
        public static string GetConnByDekVisCNC = clsDB_Server.GetConntionString_MySQL("172.23.9.109", "cnc_db", "APS", "54886961");
        public static string GetConnByDekVisAps = clsDB_Server.GetConntionString_MySQL("172.23.9.109", "dek_aps", "APS", "54886961");

        public static string logout_url = "../../login.aspx";

        public static string Base64Encode(string AStr)
        {   //編碼兩次
            AStr = Convert.ToBase64String(Encoding.UTF8.GetBytes(AStr));
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(AStr));
        }
        public static string Base64Decode(string ABase64)
        {
            //解碼兩次
            ABase64 = Encoding.UTF8.GetString(Convert.FromBase64String(ABase64));
            return Encoding.UTF8.GetString(Convert.FromBase64String(ABase64));
        }
        //判斷該帳號能否進入該網頁功能
        public static bool user_view_check(string URL_NAME, string user_ID)
        {
            GlobalVar.UseDB_setConnString(GetConnByDekVisErp);
            string sqlcmd = $"SELECT * FROM WEB_USER where WB_URL = '{URL_NAME}' and user_ACC = '{user_ID}' and VIEW_NY = 'Y'";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
            return HtmlUtil.Check_DataTable(dt);
        }
        //判斷是否為最高使用者
        public string check_user_power(string acc_)
        {
            GlobalVar.UseDB_setConnString(GetConnByDekVisErp);
            string sqlcmd = $"select * from USERS where USER_ACC = '{acc_}'";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
            return HtmlUtil.Check_DataTable(dt) ? DataTableUtils.toString(dt.Rows[0]["ADM"]) : "";
        }
        public int get_ran_id()
        {
            Random Rnd = new Random();
            return Rnd.Next(99999);
        }
        public DataTable user_login(string USER_ACC, string USER_PWD)
        {
            string cmd = "";
            if (Regex.IsMatch(USER_ACC, @"^09[0-9]{8}$") == true)
                cmd = $"user_num = '{USER_ACC}'";
            else
                cmd = $"user_acc = '{USER_ACC}'";
            GlobalVar.UseDB_setConnString(GetConnByDekVisErp);
            string sqlcmd = $"SELECT * FROM USERS where {cmd} and user_pwd = '{USER_PWD}' and STATUS = 'ON'";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
            return dt;
        }

        public DataView Add_LINE_GROUP(DataTable dt)
        {
            //有結果的存放於此，才不用一直開關資料庫
            List<string> list = new List<string>();

            DataTable dr = dt;
            string LINE_ID = "";
            string LINE_GROUP = "";
            dt.Columns.Add("產線群組", typeof(string));

            foreach (DataRow row in dt.Rows)
            {
                if (list.Count == 0 || list.IndexOf(DataTableUtils.toString(row["產線代號"])) == -1)
                {
                    LINE_ID = DataTableUtils.toString(row["產線代號"]);
                    LINE_GROUP = "";
                    GlobalVar.UseDB_setConnString(GetConnByDekVisErp);//切換至可視化資料庫
                    string sqlcmd = 德大機械.業務部_成品庫存分析.產線群組列表(LINE_ID);
                    DataTable ds = DataTableUtils.GetDataTable(sqlcmd);
                    if (HtmlUtil.Check_DataTable(ds))
                        LINE_GROUP = DataTableUtils.toString(ds.Rows[0]["GROUP_NAME"]);
                    else
                        LINE_GROUP = "";
                    list.Add(LINE_ID);
                    list.Add(LINE_GROUP);
                }
                else
                {
                    LINE_ID = list[list.IndexOf(DataTableUtils.toString(row["產線代號"]))];
                    LINE_GROUP = list[list.IndexOf(DataTableUtils.toString(row["產線代號"])) + 1];
                }

                if (LINE_GROUP == "")
                    row["產線群組"] = "沒有產線代號";
                else
                    row["產線群組"] = LINE_GROUP;

            }
            DataView dv = new DataView(dt);
            dv.Sort = "產線群組 asc";
            dv.ToTable();
            return dv;

        }
        public DataView Add_LINE_GROUP(DataTable dt, string Type)
        {
            //有結果的存放於此，才不用一直開關資料庫
            List<string> list = new List<string>();

            DataTable dr = dt;
            string LINE_ID = "";
            string LINE_GROUP = "";
            dt.Columns.Add("產線群組", typeof(string));

            foreach (DataRow row in dt.Rows)
            {
                if (list.Count == 0 || list.IndexOf(DataTableUtils.toString(row["產線代號"])) == -1)
                {
                    LINE_ID = DataTableUtils.toString(row["產線代號"]);
                    LINE_GROUP = "";
                    GlobalVar.UseDB_setConnString(GetConnByDekdekVisAssmHor);//切換至可視化資料庫
                    string sqlcmd = $"select * from 工作站型態資料表 where 工作站編號='{LINE_ID}'";
                    DataTable ds = DataTableUtils.GetDataTable(sqlcmd);
                    if (HtmlUtil.Check_DataTable(ds))
                        LINE_GROUP = DataTableUtils.toString(ds.Rows[0]["工作站名稱"]);
                    else
                        LINE_GROUP = "";

                    list.Add(LINE_ID);
                    list.Add(LINE_GROUP);
                }
                else
                {
                    LINE_ID = list[list.IndexOf(DataTableUtils.toString(row["產線代號"]))];
                    LINE_GROUP = list[list.IndexOf(DataTableUtils.toString(row["產線代號"])) + 1];
                }

                if (LINE_GROUP == "")
                    row["產線群組"] = "沒有產線代號";
                else
                    row["產線群組"] = LINE_GROUP;

            }
            DataView dv = new DataView(dt);
            dv.Sort = "產線群組 asc";
            dv.ToTable();

            return dv;
        }
        public DataView Add_LINE_GROUP(DataTable dt, string 群組欄位名稱, string 產線欄位名稱)
        {
            GlobalVar.UseDB_setConnString(GetConnByDekVisErp);
            DataTable dr = dt;
            dt.Columns.Add(群組欄位名稱, typeof(string));
            foreach (DataRow row in dt.Rows)
            {
                string LINE_ID = DataTableUtils.toString(row[產線欄位名稱]);
                string LINE_GROUP = DataTableUtils.toString(row[群組欄位名稱]);
                if (LINE_GROUP == "")
                {
                    string sqlcmd = 德大機械.業務部_成品庫存分析.產線群組列表(LINE_ID);
                    DataRow row_ = null;
                    try
                    {
                        row_ = DataTableUtils.DataTable_GetDataRow(sqlcmd);
                        row[群組欄位名稱] = DataTableUtils.toString(row_["GROUP_NAME"]);
                    }
                    catch
                    {
                        dr.Columns.RemoveAt(dr.Columns.Count - 1);
                        Add_LINE_GROUP(dr, 群組欄位名稱, 產線欄位名稱);
                    }
                }
            }
            DataView dv = new DataView(dt);
            dv.Sort = $"{群組欄位名稱} asc";
            dv.ToTable();
            return dv;
        }

        public string date_trn(string days)
        {
            Double val = DataTableUtils.toDouble(days);
            return DataTableUtils.toString(DateTime.Now.AddDays(-val).ToString("yyyyMMdd"));
        }
        //增加查詢條件
        public static string Insert_Condition(string field_name, List<string> list, string or_and)
        {
            if (list.Count > 0)
            {
                StringBuilder Condition = new StringBuilder();
                for (int i = 0; i < list.Count; i++)
                {
                    Condition.Append($" {field_name}='{list[i]}' ");
                    if (i < list.Count - 1)
                        Condition.Append($" {or_and} ");
                }
                return Condition.ToString();
            }
            else
                return "";
        }

    }
}