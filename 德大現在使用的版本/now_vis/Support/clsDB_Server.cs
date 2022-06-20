using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//namespace Support
//{
//#if DEBUG != true
//    [LicenseProvider(typeof(dekLicenseProvider))]
//#endif
//    public class clsDB_Server
//    {
//        //////////////////////////////////////////////////////////////////////////////////////////////
//        /// 資料庫連結字串:
//        /// MYSQL: "Server=IP;Port=3306;Database=myDataBase;Uid=myUsername;Pwd=myPassword"
//        /// MSSQL: "Data Source=IP;Provider=SQLOLEDB;Initial Catalog=procdb;User ID=xxx;Password=xxx"
//        /// ACCESS: "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=access.mdb"
//        //////////////////////////////////////////////////////////////////////////////////////////////

//        OleDbConnection db_connection = null;

//        public static string GetConntionString_MsSQL(string mssql_ip, string dbname, string user, string password)
//        {
//            return string.Format("Data Source={0};Provider=SQLOLEDB;Initial Catalog={1};User ID={2};Password={3};Connection Timeout=3",
//                mssql_ip, dbname, user, password);
//        }

//        public static string GetConntionString_MySQL(string mysql_ip, string dbname, string user, string password)
//        {
//            return string.Format("Server={0};Port=3306;Database={1};Uid={2};Pwd={3}",
//                mysql_ip, dbname, user, password);
//        }

//        public static string GetConntionString_Access(string access_file)
//        {
//            return string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}", access_file);
//        }

//        public clsDB_Server(string strConnection)
//        {
//            LicenseManager.Validate(typeof(clsDB_Server), this);
//            dbOpen(strConnection);
//        }

//        public bool dbOpen(string strConn)
//        {
//            try
//            {
//                dbClose();
//                db_connection = new OleDbConnection(strConn);
//                db_connection.Open();
//                GlobalVar.Conn_status = true;
//            }
//            catch
//            {
//                GlobalVar.Conn_status = false;
//                db_connection = null;
//            }
//            return db_connection != null;
//        }

//        public void dbClose()
//        {
//            if (db_connection != null)
//            {
//                db_connection.Close();
//                db_connection = null;
//            }
//        }

//        private OleDbCommand getSQLCommand(string SqlCmd, DbParameter[] parameters)
//        {
//            if (db_connection == null) return null;
//            OleDbCommand cmd = new OleDbCommand(SqlCmd, db_connection);
//            if (parameters != null)
//            {
//                string str = SqlCmd;
//                foreach (DbParameter parameter in parameters)
//                    str = str.Replace(parameter.ParameterName, "'" + parameter.Value.ToString() + "'");
//                cmd.CommandText = str;
//            }
//            return cmd;
//        }

//        public int SQL_Exec(string SqlCmd, DbParameter[] parameters = null)
//        {
//            try
//            {
//                OleDbCommand cmd = getSQLCommand(SqlCmd, parameters);
//                int record_count = cmd.ExecuteNonQuery();
//                cmd.Dispose();
//                return record_count;
//            }
//            catch { }
//            return 0;
//        }

//        //=====================================================================
//        public string toString(object obj)
//        {
//            string str = "";
//            if (obj != null)
//            {
//                try { str = obj.ToString(); }
//                catch { str = ""; }
//            }
//            return str;
//        }

//        //-------------------------------------------------------------
//        public int RowCount(string SqlCmd, DbParameter[] parameters = null)
//        {
//            int rec_count = 0;
//            try
//            {
//                OleDbCommand cmd = getSQLCommand(SqlCmd, parameters);
//                OleDbDataReader dr = cmd.ExecuteReader();
//                while (dr.Read())
//                {
//                    rec_count++;
//                }
//                dr.Close();
//                cmd.Dispose();
//            }
//            catch { }
//            return rec_count;
//        }

//        public int RowCount(string table_name, string sql_cond)
//        {
//            string SqlCmd = "select count(*) from " + table_name;
//            if (sql_cond.Trim() != "") SqlCmd += " where " + sql_cond;
//            int rec_count;
//            try
//            {
//                OleDbCommand cmd = getSQLCommand(SqlCmd, null);
//                rec_count = (int)cmd.ExecuteScalar();
//                cmd.Dispose();
//            }
//            catch { rec_count = 0; }
//            return rec_count;
//        }

//        public int RowCount(string SqlCmd)
//        {
//            /*
//            SELECT [DISTINCT] column, column, .... 
//            FROM tablename
//            WHERE condition
//            GROUP BY column, column, .....
//            HAVING condition
//            ORDER BY column [ASC | DESC], column [ASC | DESC],  .... ;
//            */
//            SqlCmd = SqlCmd.ToLower();
//            int index = SqlCmd.IndexOf("from ");
//            if (index <= 0) return 0;
//            string table = SqlCmd.Substring(index + 5).Trim();
//            string sqlcond = "";
//            index = table.IndexOf(" ");
//            if (index > 1)
//            {
//                SqlCmd = table.Substring(index + 1).Trim();
//                table = table.Substring(0, index);
//                index = SqlCmd.IndexOf("where ");
//                if (index >= 0)
//                {
//                    index += 6;
//                    sqlcond = SqlCmd.Substring(index).Trim();
//                    int pos = sqlcond.IndexOf("group ");
//                    if (pos <= index)
//                        pos = sqlcond.IndexOf("having ");
//                    if (pos <= index)
//                        pos = sqlcond.IndexOf("order ");
//                    if (pos > index)
//                        sqlcond = sqlcond.Substring(0, pos);
//                }
//            }
//            return RowCount(table, sqlcond);
//        }

//        //------------------------------------------------------------------------
//        private int DataReader_SetColumns(OleDbDataReader dr, DataTable table)
//        {
//            if (table == null) return 0;
//            table.Columns.Clear();
//            for (int index = 0; index < dr.FieldCount; index++)
//            {
//                table.Columns.Add(dr.GetName(index));
//            }
//            return table.Columns.Count;
//        }

//        private bool DataReader_AddRow(OleDbDataReader dr, DataTable table)
//        {
//            if (dr.HasRows && table != null)
//            {
//                if (dr.Read() != true) return false;
//                if (table.Columns.Count <= 0)
//                    DataReader_SetColumns(dr, table);
//                DataRow row = table.NewRow();
//                string fieldname, value;
//                for (int index = 0; index < table.Columns.Count; index++)
//                {
//                    fieldname = toString(table.Columns[index]);
//                    if (fieldname != "")
//                    {
//                        value = toString(dr[fieldname]);
//                        row[fieldname] = value;
//                    }
//                }
//                table.Rows.Add(row);
//                return table.Columns.Count > 0;
//            }
//            return false;
//        }

//        private DataTable DataReader_GetEmptyTable(OleDbDataReader dr)
//        {
//            DataTable table = new DataTable();
//            for (int index = 0; index < dr.FieldCount; index++)
//            {
//                table.Columns.Add(dr.GetName(index));
//            }
//            return table;
//        }

//        //--------------------------------------------------------
//        public DataTable DataTable_GetDataTableEmpty(string table_name)
//        {
//            string sql = "select * from " + table_name;
//            DataTable table = null;
//            try
//            {
//                OleDbCommand cmd = getSQLCommand(sql, null);
//                OleDbDataReader dr = cmd.ExecuteReader();
//                table = DataReader_GetEmptyTable(dr);
//                dr.Close();
//                cmd.Dispose();
//            }
//            catch { }
//            return table;
//        }

//        public DataTable DataTable_GetRow(string sql)
//        {
//            DataTable table = null;
//            try
//            {
//                OleDbCommand cmd = getSQLCommand(sql, null);
//                OleDbDataReader dr = cmd.ExecuteReader();
//                if (dr.HasRows != true)
//                    table = DataReader_GetEmptyTable(dr);
//                else
//                {
//                    table = new DataTable();
//                    DataReader_AddRow(dr, table);
//                }
//                dr.Close();
//                cmd.Dispose();
//            }
//            catch { }
//            return table;
//        }

//        public DataTable GetDataTable(string SqlCmd, DbParameter[] parameters = null)
//        {
//            DataTable dt = null;
//            try
//            {
//                OleDbCommand cmd = getSQLCommand(SqlCmd, parameters);
//                OleDbDataReader dr = cmd.ExecuteReader();
//                if (dr.HasRows)
//                {
//                    dt = new DataTable();
//                    dt.Load(dr);
//                }
//                else dt = DataReader_GetEmptyTable(dr);
//                dr.Close();
//                cmd.Dispose();
//            }
//            catch (Exception ex) { string str = ex.Message; }
//            return dt;
//        }

//        public DataTable DataTable_GetTable(string SqlCmd, int start_rec_no = 0, int max_records = 0)
//        {
//            DataTable table = null;
//            try
//            {
//                OleDbCommand cmd = getSQLCommand(SqlCmd, null);
//                OleDbDataReader dr = cmd.ExecuteReader();
//                bool ok = dr.HasRows;
//                if (!ok)
//                    table = DataReader_GetEmptyTable(dr);
//                else
//                {
//                    table = new DataTable();
//                    while (ok && --start_rec_no >= 0)
//                    {
//                        try { ok = dr.Read(); }
//                        catch { ok = false; }
//                    }
//                    while (ok)
//                    {
//                        ok = DataReader_AddRow(dr, table);
//                        if (max_records > 0)
//                        {
//                            if (--max_records <= 0) break;
//                        }
//                    }
//                }
//                dr.Close();
//            }
//            catch { }
//            return table;
//        }

//        //========================================================================
//        public bool Insert_DataRow(string tablename, DataRow row)
//        {
//            string sql_fields = "", sql_values = "";
//            string name, value;
//            for (int index = 0; index < row.ItemArray.Length; index++)
//            {
//                //INSERT INTO 表格名 (欄位1, 欄位2, ...)  VALUES('值1', '值2', ...) 
//                name = row.Table.Columns[index].ColumnName;
//                value = toString(row[name]);
//                if (name != "" && value != "")
//                {
//                    if (sql_fields == "")
//                    {
//                        sql_fields = "(";
//                        sql_values = "Values(";
//                    }
//                    else { sql_fields += ","; sql_values += ","; }
//                    sql_fields += name;
//                    sql_values += "'" + value + "'";
//                }
//            }
//            if (sql_fields != "")
//            {
//                string SQL指令 = "Insert into " + tablename + " " +
//                           sql_fields + ") " + sql_values + ")";
//                if (SQL_Exec(SQL指令, null) > 0) return true;
//            }
//            return false;
//        }

//        public int Insert_TableRows(string table_name, DataTable datatable)
//        {
//            int rec_count = 0;
//            foreach (DataRow row in datatable.Rows)
//            {
//                if (!Insert_DataRow(table_name, row)) break;
//                rec_count++;
//            }
//            return rec_count;
//        }

//        //==========================================================
//        public bool Delete_Record(string 資料表名稱, string SQL指令搜尋條件)
//        //-------------------------------------------------------------------------------
//        {
//            string SQL指令 = "delete from " + 資料表名稱;
//            if (SQL指令搜尋條件.Trim() != "") SQL指令 += " where " + SQL指令搜尋條件;
//            return SQL_Exec(SQL指令, null) > 0;
//        }

//        //==========================================================
//        public bool Update_DataRow(string table_name, string SQL指令搜尋條件, DataRow row)
//        {
//            string SQL指令 = "select * from " + table_name;
//            if (SQL指令搜尋條件.Trim() != "") SQL指令 += " where " + SQL指令搜尋條件;
//            DataTable rec_tab = DataTable_GetRow(SQL指令);
//            if (rec_tab.Rows.Count <= 0) return false;
//            string name, value, old_val;
//            //UPDATE "table_name" SET column_1 = [value1], column_2 = [value2] WHERE {SQL指令搜尋條件} 
//            SQL指令 = "";
//            DataColumnCollection columns = row.Table.Columns;
//            for (int index = 0; index < columns.Count; index++)
//            {
//                name = columns[index].ColumnName;
//                try
//                {
//                    object obj = row[name];
//                    if (obj == DBNull.Value) continue;
//                    value = toString(obj);
//                    old_val = toString(rec_tab.Rows[0][name]);
//                }
//                catch { continue; }
//                if (name != "" && old_val != value)
//                {
//                    if (SQL指令 == "") SQL指令 = "SET ";
//                    else SQL指令 += ", ";
//                    SQL指令 += name + "='" + value + "'";
//                }
//            }
//            if (SQL指令 != "")
//            {
//                SQL指令 = "UPDATE " + table_name + " " + SQL指令;
//                if (SQL指令搜尋條件.Trim() != "") SQL指令 += " where " + SQL指令搜尋條件;
//                if (SQL_Exec(SQL指令, null) <= 0) return false;
//            }
//            return true;
//        }

//        public int Update_DataTable(string table_name, string SQL指令搜尋條件, DataTable datatable)
//        {
//            int count = 0;
//            foreach (DataRow row in datatable.Rows)
//            {
//                if (!Update_DataRow(table_name, SQL指令搜尋條件, row)) break;
//                count++;
//            }
//            return count;
//        }

//        //======================================================================

//    }
//}

namespace Support//NEW//MYSQL OK, OTHER NO
{
    public enum DBConnectType { OLEDB = 1, SQLDB, ODBC };

#if DEBUG != true
    [LicenseProvider(typeof(dekLicenseProvider))]
#endif
    public class clsDB_Server
    {
        //////////////////////////////////////////////////////////////////////////////////////////////
        /// 資料庫連結字串:
        /// MYSQL: "Server=IP;Port=3306;Database=myDataBase;Uid=myUsername;Pwd=myPassword"
        /// MSSQL: "Data Source=IP;Provider=SQLOLEDB;Initial Catalog=procdb;User ID=xxx;Password=xxx"
        /// ACCESS: "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=access.mdb"
        //////////////////////////////////////////////////////////////////////////////////////////////

        //public DBConnectType DB_Connect_Type = DBConnectType.SQLDB;
        public DBConnectType DB_Connect_Type = DBConnectType.ODBC;      //只有這個改
        public string ErrorMessage { get; set; }

        DbConnection db_connection = null;
        public clsDB_Server(string strConnection, DBConnectType db_con_type = DBConnectType.SQLDB)
        {
            LicenseManager.Validate(typeof(clsDB_Server), this);
            ErrorMessage = "";
            IsConnected = false;
            dbOpen(strConnection);
        }

        public bool dbOpen(string strConn)
        {
            try
            {
                dbClose();
                if (DB_Connect_Type == DBConnectType.SQLDB)
                    db_connection = new SqlConnection(strConn);
                else if (DB_Connect_Type == DBConnectType.ODBC)
                    db_connection = new OdbcConnection(strConn);
                else if (DB_Connect_Type == DBConnectType.OLEDB)
                    db_connection = new OleDbConnection(strConn);
                db_connection.Open();
                IsConnected = true;
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                dbClose();
            }
            return db_connection != null;
        }

        public void dbClose()
        {
            if (db_connection != null)
                db_connection.Close();
            db_connection = null;
            IsConnected = false;
        }

        private DbCommand getSQLCommand(string SqlCmd, DbParameter[] parameters)
        {
            if (db_connection == null) return null;
            DbCommand cmd = null;
            if (db_connection is SqlConnection)
                cmd = new SqlCommand(SqlCmd, (SqlConnection)db_connection);
            else if (db_connection is OdbcConnection)
                cmd = new OdbcCommand(SqlCmd, (OdbcConnection)db_connection);
            else if (db_connection is OleDbConnection)
                cmd = new OleDbCommand(SqlCmd, (OleDbConnection)db_connection);
            if (cmd != null && parameters != null)
            {
                string str = SqlCmd;
                foreach (DbParameter parameter in parameters)
                    str = str.Replace(parameter.ParameterName, "'" + parameter.Value.ToString() + "'");
                cmd.CommandText = str;
            }
            return cmd;
        }

        //------------------------------------------------------
        public bool IsConnected
        {
            get { return GlobalVar.Conn_status; }
            set { GlobalVar.Conn_status = value; }
        }

        public static string GetConntionString_MsSQL(string mssql_ip, string dbname, string user, string password)
        {
            return "DRIVER={SQL SERVER};" + string.Format("Server={0};Database={1};Uid={2};Pwd={3};providerName=System.Data.Odb",
                mssql_ip, dbname, user, password);
        }
        public static string GetConntionString_MySQL(string mysql_ip, string dbname, string user, string password)
        {
            return "DRIVER={MySQL ODBC 8.0 Unicode Driver};" + string.Format("Server={0};Port=3306;Database={1};Uid={2};Pwd={3}",
                mysql_ip, dbname, user, password);
        }
        public static string GetConntionString_Access(string access_file)
        {
            return string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}", access_file);
        }
        //===============================================================================
        public int SQL_Exec(string SqlCmd, DbParameter[] parameters = null)
        {
            try
            {
                DbCommand cmd = getSQLCommand(SqlCmd, parameters);
                int record_count = cmd.ExecuteNonQuery();
                cmd.Dispose();
                return record_count;
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return 0;
        }

        //=====================================================================
        public string toString(object obj)
        {
            string str = "";
            if (obj != null)
            {
                try { str = obj.ToString(); }
                catch { str = ""; }
            }
            return str;
        }

        //-------------------------------------------------------------
        public int RowCount(string SqlCmd, DbParameter[] parameters = null)
        {
            int rec_count = 0;
            try
            {
                DbCommand cmd = getSQLCommand(SqlCmd, parameters);
                DbDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    rec_count++;
                }
                dr.Close();
                cmd.Dispose();
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return rec_count;
        }

        public int RowCount(string table_name, string sql_cond)
        {
            string SqlCmd = "select count(*) from " + table_name;
            if (sql_cond.Trim() != "") SqlCmd += " where " + sql_cond;
            int rec_count;
            try
            {
                DbCommand cmd = getSQLCommand(SqlCmd, null);
                rec_count = (int)cmd.ExecuteScalar();
                cmd.Dispose();
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                rec_count = 0;
            }
            return rec_count;
        }

        public int RowCount(string SqlCmd)
        {
            /*
            SELECT [DISTINCT] column, column, .... 
            FROM tablename
            WHERE condition
            GROUP BY column, column, .....
            HAVING condition
            ORDER BY column [ASC | DESC], column [ASC | DESC],  .... ;
            */
            SqlCmd = SqlCmd.ToLower();
            int index = SqlCmd.IndexOf("from ");
            if (index <= 0) return 0;
            string table = SqlCmd.Substring(index + 5).Trim();
            string sqlcond = "";
            index = table.IndexOf(" ");
            if (index > 1)
            {
                SqlCmd = table.Substring(index + 1).Trim();
                table = table.Substring(0, index);
                index = SqlCmd.IndexOf("where ");
                if (index >= 0)
                {
                    index += 6;
                    sqlcond = SqlCmd.Substring(index).Trim();
                    int pos = sqlcond.IndexOf("group ");
                    if (pos <= index)
                        pos = sqlcond.IndexOf("having ");
                    if (pos <= index)
                        pos = sqlcond.IndexOf("order ");
                    if (pos > index)
                        sqlcond = sqlcond.Substring(0, pos);
                }
            }
            return RowCount(table, sqlcond);
        }

        //------------------------------------------------------------------------
        private int DataReader_SetColumns(DbDataReader dr, DataTable table)
        {
            if (table == null) return 0;
            table.Columns.Clear();
            for (int index = 0; index < dr.FieldCount; index++)
            {
                table.Columns.Add(dr.GetName(index));
            }
            return table.Columns.Count;
        }

        private bool DataReader_AddRow(DbDataReader dr, DataTable table)
        {
            if (dr.HasRows && table != null)
            {
                if (dr.Read() != true) return false;
                if (table.Columns.Count <= 0)
                    DataReader_SetColumns(dr, table);
                DataRow row = table.NewRow();
                string fieldname, value;
                for (int index = 0; index < table.Columns.Count; index++)
                {
                    fieldname = toString(table.Columns[index]);
                    if (fieldname != "")
                    {
                        value = toString(dr[fieldname]);
                        row[fieldname] = value;
                    }
                }
                table.Rows.Add(row);
                return table.Columns.Count > 0;
            }
            return false;
        }

        private DataTable DataReader_TableNoRow(DbDataReader dr)
        {
            DataTable table = new DataTable();
            for (int index = 0; index < dr.FieldCount; index++)
            {
                table.Columns.Add(dr.GetName(index));
            }
            return table;
        }

        //--------------------------------------------------------
        public DataTable DataTable_TableNoRow(string table_name)
        {
            string sql = "select * from " + table_name;
            DataTable table = null;
            try
            {
                DbCommand cmd = getSQLCommand(sql, null);
                DbDataReader dr = cmd.ExecuteReader();
                table = DataReader_TableNoRow(dr);
                dr.Close();
                cmd.Dispose();
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return table;
        }

        public DataTable DataTable_GetDataTableEmpty(string table_name)
        {
            return DataTable_TableNoRow(table_name);
        }

        public DataTable DataTable_GetRow(string sql)
        {
            DataTable table = null;
            try
            {
                DbCommand cmd = getSQLCommand(sql, null);
                DbDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows != true)
                    table = DataReader_TableNoRow(dr);
                else
                {
                    table = new DataTable();
                    DataReader_AddRow(dr, table);
                }
                dr.Close();
                cmd.Dispose();
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return table;
        }

        public DataTable GetDataTable(string SqlCmd, DbParameter[] parameters = null)
        {
            DataTable dt = null;
            try
            {
                DbCommand cmd = getSQLCommand(SqlCmd, parameters);
                DbDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dt = new DataTable();
                    dt.Load(dr);
                }
                else dt = DataReader_TableNoRow(dr);
                dr.Close();
                cmd.Dispose();
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return dt;
        }

        public DataTable DataTable_GetTable(string SqlCmd, int start_rec_no = 0, int max_records = 0)
        {
            DataTable table = null;
            try
            {
                DbCommand cmd = getSQLCommand(SqlCmd, null);
                DbDataReader dr = cmd.ExecuteReader();
                bool ok = dr.HasRows;
                if (!ok)
                    table = DataReader_TableNoRow(dr);
                else
                {
                    table = new DataTable();
                    while (ok && --start_rec_no >= 0)
                    {
                        try { ok = dr.Read(); }
                        catch { ok = false; }
                    }
                    while (ok)
                    {
                        ok = DataReader_AddRow(dr, table);
                        if (max_records > 0)
                        {
                            if (--max_records <= 0) break;
                        }
                    }
                }
                dr.Close();
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            return table;
        }

        //========================================================================
        public bool Insert_DataRow(string tablename, DataRow row)
        {
            string sql_fields = "", sql_values = "";
            string name, value;
            for (int index = 0; index < row.ItemArray.Length; index++)
            {
                //INSERT INTO 表格名 (欄位1, 欄位2, ...)  VALUES('值1', '值2', ...) 
                name = row.Table.Columns[index].ColumnName;
                value = toString(row[name]);
                if (name != "" && value != "")
                {
                    if (sql_fields == "")
                    {
                        sql_fields = "(";
                        sql_values = "Values(";
                    }
                    else { sql_fields += ","; sql_values += ","; }
                    sql_fields += name;
                    sql_values += "'" + value + "'";
                }
            }
            if (sql_fields != "")
            {
                string SQL指令 = "Insert into " + tablename + " " +
                           sql_fields + ") " + sql_values + ")";
                if (SQL_Exec(SQL指令, null) > 0) return true;
            }
            return false;
        }

        public int Insert_TableRows(string table_name, DataTable datatable)
        {
            int rec_count = 0;
            foreach (DataRow row in datatable.Rows)
            {
                if (!Insert_DataRow(table_name, row)) break;
                rec_count++;
            }
            return rec_count;
        }

        //==========================================================
        public bool Delete_Record(string 資料表名稱, string SQL指令搜尋條件)
        //-------------------------------------------------------------------------------
        {
            string SQL指令 = "delete from " + 資料表名稱;
            if (SQL指令搜尋條件.Trim() != "") SQL指令 += " where " + SQL指令搜尋條件;
            return SQL_Exec(SQL指令, null) > 0;
        }

        //==========================================================
        public bool Update_DataRow(string table_name, string SQL指令搜尋條件, DataRow row)
        {
            string SQL指令 = "select * from " + table_name;
            if (SQL指令搜尋條件.Trim() != "") SQL指令 += " where " + SQL指令搜尋條件;
            DataTable rec_tab = DataTable_GetRow(SQL指令);
            if (rec_tab.Rows.Count <= 0) return false;
            string name, value, old_val;
            //UPDATE "table_name" SET column_1 = [value1], column_2 = [value2] WHERE {SQL指令搜尋條件} 
            SQL指令 = "";
            DataColumnCollection columns = row.Table.Columns;
            for (int index = 0; index < columns.Count; index++)
            {
                name = columns[index].ColumnName;
                try
                {
                    object obj = row[name];
                    if (obj == DBNull.Value) continue;
                    value = toString(obj);
                    old_val = toString(rec_tab.Rows[0][name]);
                }
                catch { continue; }
                if (name != "" && old_val != value)
                {
                    if (SQL指令 == "") SQL指令 = "SET ";
                    else SQL指令 += ", ";
                    SQL指令 += name + "='" + value + "'";
                }
            }
            if (SQL指令 != "")
            {
                SQL指令 = "UPDATE " + table_name + " " + SQL指令;
                if (SQL指令搜尋條件.Trim() != "") SQL指令 += " where " + SQL指令搜尋條件;
                if (SQL_Exec(SQL指令, null) <= 0) return false;
            }
            return true;
        }

        public int Update_DataTable(string table_name, string SQL指令搜尋條件, DataTable datatable)
        {
            int count = 0;
            foreach (DataRow row in datatable.Rows)
            {
                if (!Update_DataRow(table_name, SQL指令搜尋條件, row)) break;
                count++;
            }
            return count;
        }
        //======================================================================
    }
}
