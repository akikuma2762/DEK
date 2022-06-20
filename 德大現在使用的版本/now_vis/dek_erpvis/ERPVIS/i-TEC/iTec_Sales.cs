using ERPVIS.i_TEC;
using Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPVIS
{
    public class iTec_Sales : IdepSales
    {
        const string DateFormat = "yyyyMMdd";
        IniManager iniManager = new IniManager("D:/DATAWINCommand.ini");
        //訂單數量及金額統計
        public DataTable Orders(string start, string end, OrderStatus status, dekModel img_or_tbl, OrderLineorCust line_or_custom, OrderType qty_or_amt, int max_records)
        {
            string sqlcmd = GetOrders(start, end, status, img_or_tbl, line_or_custom, qty_or_amt, max_records);
            DataTable dt = iTechDB.Get_DataTable(sqlcmd);
          //  dt = Special_Process.Order_AddColumn(dt, Get_List(dt));
            return dt;
        }

        public DataTable Orders(DateTime dt_start, DateTime dt_end, OrderStatus status, dekModel img_or_tbl, OrderLineorCust line_or_custom, OrderType qty_or_amt, int max_records)
        {
            return Orders(dt_start.ToString(DateFormat), dt_end.ToString(DateFormat), status, img_or_tbl, line_or_custom, qty_or_amt, max_records);
        }

        //訂單詳細表
        public DataTable Orders_Details(string start, string end, string custom, OrderStatus status)
        {
            string sqlcmd = GetOrders_Details(start, end, custom, status);
            DataTable dt = iTechDB.Get_DataTable(sqlcmd);
          //  dt = Special_Process.OrdersDetail_Table(dt, Get_List(dt));
            return dt;
        }

        public DataTable Orders_Details(DateTime dt_start, DateTime dt_end, string custom, OrderStatus status)
        {
            return Orders_Details(dt_start.ToString(DateFormat), dt_end.ToString(DateFormat), custom, status);
        }

        //出貨統計表
        public DataTable Shipment(string start, string end, dekModel img_or_tbl)
        {
            string sqlcmd = GetShipment(start, end, img_or_tbl);
            DataTable dt = iTechDB.Get_DataTable(sqlcmd);

            //if (img_or_tbl == dekModel.Image)
            //    dt = Special_Process.Shipment_Image(dt, Get_List(dt));
            //else if (img_or_tbl == dekModel.Table)
            //    dt = Special_Process.Shipment_Table(dt, Get_List(dt));
            return dt;
        }

        public DataTable Shipment(DateTime dt_start, DateTime dt_end, dekModel img_or_tbl)
        {
            return Shipment(dt_start.ToString(DateFormat), dt_end.ToString(DateFormat), img_or_tbl);
        }

        //出貨詳細表
        public DataTable Shipment_Detail(string start, string end, string custom)
        {
            string sqlcmd = GetShipment_Detail(start, end, custom);
            DataTable dt = iTechDB.Get_DataTable(sqlcmd);
            //dt = Special_Process.ShipmentDetails_Table(dt, Get_List(dt));
            return dt;
        }

        public DataTable Shipment_Detail(DateTime dt_start, DateTime dt_end, string custom)
        {
            return Shipment_Detail(dt_start.ToString(DateFormat), dt_end.ToString(DateFormat), custom);
        }

        //出貨詳細表 小計
        public DataTable Get_Shipment(string start, string end, string custom, string item)
        {
            string sqlcmd = GetShipment_Data(start, end, custom,item);
            DataTable dt = iTechDB.Get_DataTable(sqlcmd);
            return dt;
        }

        public DataTable Get_Shipment(DateTime dt_start, DateTime dt_end, string custom, string item)
        {
            return Get_Shipment(dt_start.ToString(DateFormat), dt_end.ToString(DateFormat), custom, item);
        }

        //運輸架未歸還紀錄
        public DataTable Transportrackstatistics(dekModel img_or_tbl)
        {
            string sqlcmd = Get_Transportrackstatistics(img_or_tbl);
            DataTable dt = iTechDB.Get_DataTable(sqlcmd);
            return dt;
        }

        //未交易客戶
        public DataTable UntradedCustomer(string start, string end, string symbol, int day)
        {
            string sqlcmd = Get_UntradedCustomer(start, end, symbol, day);
            DataTable dt = iTechDB.Get_DataTable(sqlcmd);
            return dt;
        }

        public DataTable UntradedCustomer(DateTime dt_start, DateTime dt_end, string symbol, int day)
        {
            return UntradedCustomer(dt_start.ToString(DateFormat), dt_end.ToString(DateFormat), symbol, day);
        }

        //----------------------------------------------------------------------------SQL指令-------------------------------------------------------------------------------------

        //訂單金額及數量統計
        string GetOrders(string start, string end, OrderStatus status, dekModel img_or_tbl, OrderLineorCust line_or_custom, OrderType qty_or_amt, int max_records)
        {
            StringBuilder sqlcmd = new StringBuilder();
            StringBuilder Condition = new StringBuilder();
            StringBuilder max = new StringBuilder();

            if (status != OrderStatus.All && iniManager.ReadIniFile("DATAWIN", "Orders_" + status, "") != "")
                Condition.Append(iniManager.ReadIniFile("DATAWIN", "Orders_" + status, ""));

            if (img_or_tbl == dekModel.Image)
            {
                if (line_or_custom == OrderLineorCust.Line)
                {
                    if (iniManager.ReadIniFile("DATAWIN", "Orders_" + line_or_custom, "") != "")
                        sqlcmd.AppendFormat(iniManager.ReadIniFile("DATAWIN", "Orders_" + line_or_custom, ""), start, end, Condition);
                }
                else if (line_or_custom == OrderLineorCust.Custom)
                {
                    if (max_records != 0)
                        max.Append($" TOP({max_records}) ");
                    if (iniManager.ReadIniFile("DATAWIN", "Orders_" + line_or_custom, "") != "")
                        sqlcmd.AppendFormat(iniManager.ReadIniFile("DATAWIN", "Orders_" + line_or_custom, ""), start, end, Condition, qty_or_amt, max);
                }
            }
            else if (img_or_tbl == dekModel.Table)
            {
                if (iniManager.ReadIniFile("DATAWIN", "Orders_" + img_or_tbl, "") != "")
                    sqlcmd.AppendFormat(iniManager.ReadIniFile("DATAWIN", "Orders_" + img_or_tbl, ""), start, end, Condition);
            }


            return sqlcmd.ToString();
        }

        //訂單詳細表
        string GetOrders_Details(string start, string end, string custom, OrderStatus status)
        {
            StringBuilder sqlcmd = new StringBuilder();
            StringBuilder Condition = new StringBuilder();
            if (status != OrderStatus.All && iniManager.ReadIniFile("DATAWIN", "Orders_" + status, "") != "")
                Condition.Append(iniManager.ReadIniFile("DATAWIN", "Orders_" + status, ""));

            if (iniManager.ReadIniFile("DATAWIN", "Orders_Details", "") != "")
                sqlcmd.AppendFormat(iniManager.ReadIniFile("DATAWIN", "Orders_Details", ""), start, end, custom, Condition);
            return sqlcmd.ToString();
        }

        //出貨統計表
        string GetShipment(string start, string end, dekModel img_or_tbl)
        {

            StringBuilder sqlcmd = new StringBuilder();
            if (iniManager.ReadIniFile("DATAWIN", "Shipment_" + img_or_tbl, "") != "")
                sqlcmd.AppendFormat(iniManager.ReadIniFile("DATAWIN", "Shipment_" + img_or_tbl, ""), start, end);
            return sqlcmd.ToString();
        }

        //出貨詳細表
        string GetShipment_Detail(string start, string end, string custom)
        {
            StringBuilder sqlcmd = new StringBuilder();
            if (iniManager.ReadIniFile("DATAWIN", "Shipment_Details", "") != "")
                sqlcmd.AppendFormat(iniManager.ReadIniFile("DATAWIN", "Shipment_Details", ""), start, end, custom);
            return sqlcmd.ToString();
        }

        //出貨詳細表 小計
        string GetShipment_Data(string start, string end, string custom, string item)
        {
            string SQLCommand = iniManager.ReadIniFile("DATAWIN", "GetShipmentData", "");
            if (SQLCommand != "")
                SQLCommand = string.Format(SQLCommand, start, end, custom, item);
            return SQLCommand;
        }

        //運輸架未歸還統計
        string Get_Transportrackstatistics(dekModel img_or_tbl)
        {
            string Condition = "";
            string Groupby = "";

            string SQLCommand = iniManager.ReadIniFile("DATAWIN", $"transportrackstatistics_{img_or_tbl}", "");
            if (SQLCommand != "")
                SQLCommand = string.Format(SQLCommand, Condition, Groupby);
            return SQLCommand;

        }

        //未交易客戶
        string Get_UntradedCustomer(string start, string end, string symbol, int day)
        {
            //取得時間區間
            string Condition = "";
            if (start != "" && end != "")
            {
                Condition = iniManager.ReadIniFile("Parameter", "UntradedCustomer_DateRange", "");
                if (Condition != "")
                    Condition = string.Format(Condition, start, end);
            }
            //取得完整SQL指令
            string SQLCommand = iniManager.ReadIniFile("DATAWIN", "UntradedCustomer", "");
            if (SQLCommand != "")
                SQLCommand = string.Format(SQLCommand, Condition, symbol, day);

            return SQLCommand;
        }

        //回傳list
        List<string> Get_List(DataTable dt)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < dt.Columns.Count; i++)
                list.Add(DataTableUtils.toString(dt.Columns[i]));
            return list;
        }
    }
}









