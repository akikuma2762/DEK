using Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace ERPVIS.i_TEC
{
    public static class iTechDB
    {
        private static string GetConnByDataWin = clsDB_Server.GetConntionString_MsSQL("192.168.3.4", "DATAWIN", "sa", "16899");
        //取得DataTable
        public static DataTable Get_DataTable(string Sqlcmd)
        {
            GlobalVar.UseDB_setConnString(GetConnByDataWin);
            DataTable dt = DataTableUtils.GetDataTable(Sqlcmd);
            return Error_DataTable(dt, Sqlcmd, GetConnByDataWin,DataTableUtils.ErrorMessage);
        }
        //把json轉成DataTable
        public static DataTable JsontoDataTable(string jsonStr)
        {
            BsonDocument bson = MongoUtils.JsonToBsonDocument(jsonStr);
            DataTable dr = MongoUtils.BsonArrayToDataTable(bson["rows"].AsBsonArray);

            return dr;
        }
        public static DataTable Error_DataTable(DataTable dt, string sqlcmd, string link = "", string error_msg = "")
        {
            if (dt != null && dt.Rows.Count > 0)
                return dt;
            else
            {
                string struction = "N";

                dt = new DataTable();
                dt.Columns.Add("語法");
                dt.Columns.Add("連接");
                dt.Columns.Add("錯誤訊息");
                dt.Columns.Add("結構是否存在");
                if (dt != null)
                    struction = "Y";
                else
                    struction = "N";
                dt.Rows.Add(sqlcmd, link, error_msg, struction);
                return dt;
            }


        }
    }
}
