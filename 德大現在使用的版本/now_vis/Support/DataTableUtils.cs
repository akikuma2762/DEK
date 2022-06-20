using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Support
{
    public class DataTableUtils
    {

        static Stack<string> conn_str_stack = new Stack<string>();

        public static string Conn_String
        {
            get { return GlobalVar.UseDB_getConnString(); }
            set { GlobalVar.UseDB_setConnString(value); }
        }
        
        public static string ErrorMessage { get; set; }

        public static string toSelectCommand(string table_name, string sql_condition)
        {
            string sql = "select * from " + table_name;
            if (sql_condition.Trim() != "") sql += " where " + sql_condition;
            return sql;
        }

        public static void UseDB_Restore()
        {
            if (conn_str_stack.Count>0)
                Conn_String = conn_str_stack.Pop();
        }
        public static void UseDB_Change(DB_NAME name)
        {
            conn_str_stack.Push(Conn_String);
            GlobalVar.UseDB(name);
        }
        //=========================================================================
        public static int ExecSQL(string SqlCmd)
        {
            clsDB_Server clsdb = new clsDB_Server(Conn_String);
            int count = 0;
            if (clsdb.IsConnected)
                count = clsdb.SQL_Exec(SqlCmd);
            clsdb.dbClose();
            ErrorMessage = clsdb.ErrorMessage;
            return count;
        }

        public static int RowCount(string SqlCmd)
        {
            clsDB_Server clsdb = new clsDB_Server(Conn_String);
            int count = 0;
            if (clsdb.IsConnected)
                count=clsdb.RowCount(SqlCmd);
            clsdb.dbClose();
            ErrorMessage = clsdb.ErrorMessage;
            return count;
        }

        public static int RowCount(string table_name,string sql_cond)
        {
            return RowCount(toSelectCommand(table_name, sql_cond));
        }

        public static DataTable GetDataTable(string SqlCmd)
        {
            clsDB_Server clsdb = new clsDB_Server(Conn_String);
            DataTable dt = null;
            if (clsdb.IsConnected)
            {
                dt = clsdb.GetDataTable(SqlCmd);
                clsdb.dbClose();
            }
            ErrorMessage = clsdb.ErrorMessage;
            return dt;
        }

        public static DataTable GetDataTable(string table_name, string sql_condition)
        {
            string sqlcmd = toSelectCommand(table_name, sql_condition);
            return GetDataTable(sqlcmd);
        }

        public static DataTable DataTable_GetRow(string SqlCmd)
        {
            clsDB_Server clsdb = new clsDB_Server(Conn_String);
            DataTable dt = null;
            if (clsdb.IsConnected)
            {
                dt = clsdb.DataTable_GetRow(SqlCmd);
                clsdb.dbClose();
            }
            ErrorMessage = clsdb.ErrorMessage;
            return dt;
        }

        public static DataTable DataTable_GetRow(string table_name, string sql_condition)
        {
            string sqlcmd = toSelectCommand(table_name, sql_condition);
            return DataTable_GetRow(sqlcmd);
        }

        public static DataRow DataRow_GetEmptyRow(string table_name)
        {
            DataTable table = DataTable_TableNoRow(table_name);
            DataRow row = table.NewRow();
            return row;
        }

        public static DataRow DataTable_GetDataRow(string SqlCmd)
        {
            DataTable dt = DataTable_GetRow(SqlCmd);
            if (dt.Rows.Count <= 0 || dt.Rows[0] == null) return null;
            return dt.Rows[0];
        }

        public static DataRow DataTable_GetDataRow(string table_name, string sql_condition)
        {
            string sqlcmd = toSelectCommand(table_name, sql_condition);
            return DataTable_GetDataRow(sqlcmd);
        }

        public static string DataTable_GetDataRow_toString(string table_name, string sql_condition,string column_name)
        {
            string sqlcmd = toSelectCommand(table_name, sql_condition);
            DataRow row = DataTable_GetDataRow(sqlcmd);
            if (row != null)
            {
                return DataTableUtils.toString(row[column_name]);
            }
            return "";
        }
        public static string DataTable_GetDataRow_toString(string sqlcmd, string column_name)
        {
            DataRow row = DataTable_GetDataRow(sqlcmd);
            if (row != null)
            {
                return DataTableUtils.toString(row[column_name]);
            }
            return "";
        }

        public static DataTable DataTable_TableNoRow(string table_name)
        {
            clsDB_Server clsdb = new clsDB_Server(Conn_String);
            DataTable table = null;
            if (clsdb.IsConnected)
            {
                table = clsdb.DataTable_TableNoRow(table_name);
                clsdb.dbClose();
            }
            ErrorMessage = clsdb.ErrorMessage;
            return table;
        }

        public static DataTable DataTable_GetDataTableEmpty(string table_name)
        {
            return DataTable_TableNoRow(table_name);
        }

        public static DataTable DataTable_GetRowHeader(string table_name)
        {
            clsDB_Server clsdb = new clsDB_Server(Conn_String);
            DataTable table = null;
            if (clsdb.IsConnected)
            {
                table = clsdb.DataTable_TableNoRow(table_name);
                if (table.Columns.Count <= 0)
                {
                    if (table != null) table.Dispose();
                    table = new DataTable();
                    string sql = string.Format("select column_name from INFORMATION_SCHEMA.COLUMNS where table_name = '{0}'", table_name);
                    DataTable dt = clsdb.DataTable_GetTable(sql);                    
                    string str;
                    foreach (DataRow row in dt.Rows)
                    {
                        str = DataTableUtils.DataRow_ColumntoString(row, "column_name");
                        if (str != "") table.Columns.Add(str);
                    }
                    dt.Dispose();
                }
                clsdb.dbClose();
            }
            ErrorMessage = clsdb.ErrorMessage;
            return table;
        }

        public static  DataTable DataTable_GetTable(string SqlCmd, int start_rec_no = 0, int max_records = 0)
        {
            clsDB_Server clsdb = new clsDB_Server(Conn_String);
            DataTable dt = null;
            if (clsdb.IsConnected)
            {
                dt = clsdb.DataTable_GetTable(SqlCmd, start_rec_no, max_records);
                clsdb.dbClose();
            }
            ErrorMessage = clsdb.ErrorMessage;
            return dt;
        }

        public static DataTable DataTable_GetTable(string table_name, string sql_condition, int start_rec_no = 0, int max_records = 0)
        {
            string sql = toSelectCommand(table_name, sql_condition);
            return DataTable_GetTable(sql, start_rec_no, max_records);
        }

        public static bool Insert_DataRow(string tablename, DataRow row)
        {
            clsDB_Server clsdb = new clsDB_Server(Conn_String);
            bool ok = false;
            if (clsdb.IsConnected)
            {
                ok = clsdb.Insert_DataRow(tablename, row);
                clsdb.dbClose();
            }
            ErrorMessage = clsdb.ErrorMessage;
            return ok;
        }
        public static int Insert_TableRows(string table_name, DataTable datatable)
        {
            clsDB_Server clsdb = new clsDB_Server(Conn_String);
            int count = 0;
            if (clsdb.IsConnected)
            {
                count = clsdb.Insert_TableRows(table_name, datatable);
                clsdb.dbClose();
            }
            ErrorMessage = clsdb.ErrorMessage;
            return count;
        }

        public static bool Delete_Record(string 資料表名稱, string SQL指令搜尋條件)
        {
            clsDB_Server clsdb = new clsDB_Server(Conn_String);
            bool ok = false;
            if (clsdb.IsConnected)
            {
                ok = clsdb.Delete_Record(資料表名稱, SQL指令搜尋條件);
                clsdb.dbClose();
            }
            ErrorMessage = clsdb.ErrorMessage;
            return ok;
        }

        public static bool Update_DataRow(string table_name, string SQL指令搜尋條件, DataRow row)
        {
            clsDB_Server clsdb = new clsDB_Server(Conn_String);
            bool ok = false;
            if (clsdb.IsConnected)
            {
                ok = clsdb.Update_DataRow(table_name, SQL指令搜尋條件, row);
                clsdb.dbClose();
            }
            ErrorMessage = clsdb.ErrorMessage;
            return ok;
        }

        public static int Update_DataTable(string table_name, string SQL指令搜尋條件, DataTable datatable)
        {
            clsDB_Server clsdb = new clsDB_Server(Conn_String);
            int count = 0;
            if (clsdb.IsConnected)
            {
                count = clsdb.Update_DataTable(table_name, SQL指令搜尋條件, datatable);
                clsdb.dbClose();
            }
            ErrorMessage = clsdb.ErrorMessage;
            return count;
        }

        //==============================================================================

        public static string toString(object obj,string def_str="")
        {
            string str = "";
            if (obj != null)
            {
                try { str = obj.ToString(); }
                catch { str = def_str; }
            }
            return str;
        }

        public static int toInt(string text, int def_value = 0)
        {
            int value;
            try { value = int.Parse(text); }
            catch { value = def_value; }
            return value;
        }

        public static long toLong(string text, long def_value = 0)
        {
            long value;
            try { value = long.Parse(text); }
            catch { value = def_value; }
            return value;
        }

        public static double toDouble(object text, double def_value = 0)
        {
            double value;
            try { value = double.Parse(text.ToString()); }
            catch { value = def_value; }
            return value;
        }

        public static void DataTable_AddColumns(DataTable dt, string field_comma_text)
        {
            if (dt == null) return;
            string[] name_tab = field_comma_text.Split(',');
            foreach (string str in name_tab)
            {
                dt.Columns.Add(name_tab[0]);
            }
        }

        public static void DataTable_AddColumns(DataGridView dgv, string comma_text_with_display_index)
        {
            if (dgv == null) return;
            DataTable dt = (DataTable)dgv.DataSource;
            if (dt.Rows.Count==0) return;
            string[] name_tab = comma_text_with_display_index.Split(',');
            foreach (string str in name_tab)
            {
                string[] str_tab = str.Split('=');
                if (str_tab.Length <= 0) continue;
                string name = str_tab[0];
                int index = dt.Columns.IndexOf(name);
                if (index<0) dt.Columns.Add(name);
                if (str_tab.Length > 1)
                {
                    index = toInt(str_tab[1]);
                    if (index >= 0)                      
                         dgv.Columns[name].DisplayIndex = index;
                    else dgv.Columns[name].Visible = false;
                }
            }
        }

        public static void DataTable_ColumnsOrder(DataGridView dgv, string comma_text_with_display_index)
        {
            DataTable dt = (DataTable)dgv.DataSource;
            if (dt.Rows.Count == 0) return;
            string[] name_tab = comma_text_with_display_index.Split(',');

            for (int j = name_tab.Length-1; j >= 0; j--)
            {
                string[] str_tab = name_tab[j].Split('=');

                string col_name = str_tab[0];
                int col_indrx = toInt(str_tab[1]);

                if (col_indrx >= 0)
                {
                    dt.Columns[col_name].SetOrdinal(col_indrx);
                }
                else if (col_indrx < 0)
                {
                    dgv.Columns[col_name].Visible = false;
                }
            }
        }
        public static string DataRow_ColumntoString(DataRow row, int index, string def_str = "")
        {
            if (row != null && index >= 0 && index < row.Table.Columns.Count)
                return toString(row[index], def_str);
            return def_str;
        }

        public static string DataRow_ColumntoString(DataRow row, string column_name, string def_str = "")
        {
            if (row == null) return def_str;
            return DataRow_ColumntoString(row, row.Table.Columns.IndexOf(column_name), def_str);
        }

        public static int DataRow_ColumntoInt(DataRow row, int index,int def_value=0)
        {
            string str = DataRow_ColumntoString(row, index);
            return toInt(str, def_value);
        }

        public static int DataRow_ColumntoInt(DataRow row, string column_name, int def_value = 0)
        {
            string str = DataRow_ColumntoString(row, column_name);
            return toInt(str, def_value);
        }

        public static double DataRow_ColumntoDouble(DataRow row, string column_name, int def_value = 0)
        {
            string str = DataRow_ColumntoString(row, column_name);
            return toDouble(str, def_value);
        }

        public static string DataRow_ColumntoString(DataTable table, int row_index, int col_index, string def_str = "")
        {
            if (table == null || row_index < 0 || row_index >= table.Rows.Count) return def_str;
            DataRow row = table.Rows[row_index];
            return DataRow_ColumntoString(row, col_index, def_str);
        }

        public static string DataRow_ColumntoString(DataTable table, int row_index, string column_name, string def_str = "")
        {
            if (table == null || row_index < 0 || row_index >= table.Rows.Count) return def_str;
            DataRow row = table.Rows[row_index];
            return DataRow_ColumntoString(row, row.Table.Columns.IndexOf(column_name), def_str);
        }

        public static void DataRow_ToControl(DataRow row, Dictionary<string, Control> list)
        {
            Control ctrl;
            string value;
            foreach (string key in list.Keys)
            {
                value = DataRow_ColumntoString(row,key);
                ctrl = (Control)list[key];
                CtrlUtils.setControlText(ctrl, value);
            }
        }

        public static void DataRow_FromControl(DataRow row, Dictionary<string, Control> list)
        {
            Control ctrl;
            string value = "";
            foreach (string key in list.Keys)
            {
                ctrl = (Control)list[key];
                value=CtrlUtils.getControlText(ctrl);
                row[key] = value;
            }
        }
        //---------------------------------------------------------------
        public static string toCommaText(DataRow row)
        {
            string str = "";
            for (int index = 0; index < row.Table.Columns.Count; index++)
            {
                if (index > 0) str += ",";
                str += toString(row[index]);
            }
            return str;
        }

        public static int ComboBox_LoadFieldValue(ComboBox cbx, string table_name, string field_name)
        {
            if (cbx == null) return 0;
            cbx.Items.Clear();
            string sql = string.Format("select distinct {0} from {1}", field_name, table_name);
            DataTable dt = DataTable_GetTable(sql);
            foreach (DataRow row in dt.Rows)
                cbx.Items.Add(toCommaText(row));
            return cbx.Items.Count;
        }

        public static int ComboBox_LoadFieldValue(ComboBox cbx, string sql)
        {
            if (cbx == null) return 0;
            cbx.Items.Clear();
            DataTable dt = DataTable_GetTable(sql);
            foreach (DataRow row in dt.Rows)
                cbx.Items.Add(toCommaText(row));
            return cbx.Items.Count;
        }

    }
}
