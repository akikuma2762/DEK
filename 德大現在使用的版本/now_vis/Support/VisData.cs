using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Support;

namespace Support
{
    public class VisData
    {
        public static string 廠商資料表 = "廠商資料表";

        public static DataTable 廠商資料_Load(string 廠商編號)
        {
            DataTableUtils.UseDB_Change(DB_NAME.DB_VisData);
            string sql_cond = "";
            if (廠商編號 != "") sql_cond = string.Format("廠商編號='{0}'", 廠商編號);
            DataTable dt = DataTableUtils.DataTable_GetTable(廠商資料表, sql_cond);
            DataTableUtils.UseDB_Restore();
            return dt;
        }

        public static bool 廠商資料_Update(string 廠商編號, DataRow row)
        {
            if (廠商編號 == "") return false;
            DataTableUtils.UseDB_Change(DB_NAME.DB_VisData);
            string sql_cond = string.Format("廠商編號='{0}'", 廠商編號);
            bool ok = DataTableUtils.Update_DataRow(廠商資料表, sql_cond, row);
            DataTableUtils.UseDB_Restore();
            return ok;
        }

        public static bool 廠商資料_Insert(DataRow row)
        {
            DataTableUtils.UseDB_Change(DB_NAME.DB_VisData);
            bool ok = DataTableUtils.Insert_DataRow(廠商資料表, row);
            DataTableUtils.UseDB_Restore();
            return ok;
        }

        public static bool 廠商資料_Delete(string 廠商編號)
        {
            if (廠商編號 == "") return false;
            DataTableUtils.UseDB_Change(DB_NAME.DB_VisData);
            string sql_cond = string.Format("廠商編號='{0}'", 廠商編號);
            bool ok = DataTableUtils.Delete_Record(廠商資料表, sql_cond);
            DataTableUtils.UseDB_Restore();
            return ok;
        }
    }
}
