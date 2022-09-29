using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;

namespace dekERP_dll.dekErp
{
    public class ERP_Sales : Get_Sales
    {
        iTechDB iTech = new iTechDB();
        const string DateFormat = "yyyyMMdd";
        IniManager iniManager = new IniManager("");
        public DataTable Orders_Detail(string start, string end, OrderStatus status, string source, bool detail = false, string custom = "")
        {
            iniManager = new IniManager($"{ConfigurationManager.AppSettings["ini_local"]}{source}Erp.ini");
            string sqlcmd = GetOrders_Detail(start, end, status, detail);
            return iTech.Get_DataTable(sqlcmd, source);
        }

        public DataTable Orders_Detail(DateTime start, DateTime end, OrderStatus status, string source, bool detail = false, string custom = "")
        {
            return Orders_Detail(start.ToString(DateFormat), end.ToString(DateFormat), status, source, detail, custom);
        }

        public DataTable Orders_Over_Detail(string start, string source, bool detail = false, string custom = "")
        {
            iniManager = new IniManager($"{ConfigurationManager.AppSettings["ini_local"]}{source}Erp.ini");
            string sqlcmd = GetOrders_Over_Detail(start, detail);
            return iTech.Get_DataTable(sqlcmd, source);
        }

        //20220812 大圓盤逾期語法優化,新增判斷客戶名稱
        public DataTable Orders_Over_Detail_Customer(string start, string source, bool detail = false, string customer = "")
        {
            iniManager = new IniManager($"{ConfigurationManager.AppSettings["ini_local"]}{source}Erp.ini");
            string sqlcmd = GetOrders_Over_Detail_Customer(start, detail, customer);
            return iTech.Get_DataTable(sqlcmd, source);
        }

        public DataTable Orders_Over_Detail(DateTime start, string source, bool detail = false, string custom = "")
        {
            return Orders_Over_Detail(start.ToString(DateFormat), source, detail, custom);
        }

        public DataTable Shipment_Detail(string start, string end, string source, string custom = "")
        {
            iniManager = new IniManager($"{ConfigurationManager.AppSettings["ini_local"]}{source}Erp.ini");
            string sqlcmd = GetShipment_Detail(start, end);
            return iTech.Get_DataTable(sqlcmd, source);
        }

        public DataTable Shipment_Detail(DateTime start, DateTime end, string source, string custom = "")
        {
            return Shipment_Detail(start.ToString(DateFormat), end.ToString(DateFormat), source, custom);
        }

        public DataTable Get_Shipment(string start, string end, string custom, string item, string source)
        {
            iniManager = new IniManager($"{ConfigurationManager.AppSettings["ini_local"]}{source}Erp.ini");
            string sqlcmd = GetShipment(start, end, custom, item);
            return iTech.Get_DataTable(sqlcmd, source);
        }

        public DataTable Get_Shipment(DateTime start, DateTime end, string custom, string item, string source)
        {
            return Get_Shipment(start.ToString(DateFormat), end.ToString(DateFormat), custom, item, source);
        }

        public DataTable Transportrackstatistics(string acc, string source)
        {
            iniManager = new IniManager($"{ConfigurationManager.AppSettings["ini_local"]}{source}Erp.ini");
            string sqlcmd = GetTransportrackstatistics(acc);
            return iTech.Get_DataTable(sqlcmd, source);
        }

        public DataTable UntradedCustomer(string start, string end, string symbol, int day, string source)
        {
            iniManager = new IniManager($"{ConfigurationManager.AppSettings["ini_local"]}{source}Erp.ini");
            string sqlcmd = GetUntradedCustomer(start, end, symbol, day, source);
            return iTech.Get_DataTable(sqlcmd, source);
        }

        public DataTable UntradedCustomer(DateTime start, DateTime end, string symbol, int day, string source)
        {
            return UntradedCustomer(start.ToString(DateFormat), end.ToString(DateFormat), symbol, day, source);
        }

        public DataTable Recordsofchangetheorder_Details(string start, string end, string source)
        {
            iniManager = new IniManager($"{ConfigurationManager.AppSettings["ini_local"]}{source}Erp.ini");
            string sqlcmd = GetRecordsofchangetheorder_Details(start, end);
            return iTech.Get_DataTable(sqlcmd, source);
        }

        public DataTable Recordsofchangetheorder_Details(DateTime start, DateTime end, string source)
        {
            return Recordsofchangetheorder_Details(start.ToString(DateFormat), end.ToString(DateFormat), source);
        }

        public DataTable Orders_StartDay(string start, string end, string source)
        {
            iniManager = new IniManager($"{ConfigurationManager.AppSettings["ini_local"]}{source}Erp.ini");
            string sqlcmd = GetOrders_StartDay(start, end);
            return iTech.Get_DataTable(sqlcmd, source);
        }

        public DataTable Orders_StartDay(DateTime start, DateTime end, string source)
        {
            return Orders_StartDay(start.ToString(DateFormat), end.ToString(DateFormat), source);
        }

        public DataTable Orders_Month(string start, string end, string source, bool detail = false)
        {
            iniManager = new IniManager($"{ConfigurationManager.AppSettings["ini_local"]}{source}Erp.ini");
            string sqlcmd = GetOrders_Month(start, end, detail);
            return iTech.Get_DataTable(sqlcmd, source);
        }

        public DataTable Orders_Month(DateTime start, DateTime end, string source, bool detail = false)
        {
            return Orders_Month(start.ToString(DateFormat), end.ToString(DateFormat), source, detail);
        }

        public DataTable Change_Date_To_Now(string start, string end, string source)
        {
            iniManager = new IniManager($"{ConfigurationManager.AppSettings["ini_local"]}{source}Erp.ini");
            string sqlcmd = GetChange_Date_To_Now(start, end);
            return iTech.Get_DataTable(sqlcmd, source);
        }

        public DataTable Change_Date_To_Now(DateTime start, DateTime end, string source)
        {
            return Change_Date_To_Now(start.ToString(DateFormat), end.ToString(DateFormat), source);
        }

        public DataTable Change_Date_To_Other(string start, string end, string source)
        {
            iniManager = new IniManager($"{ConfigurationManager.AppSettings["ini_local"]}{source}Erp.ini");
            string sqlcmd = GetChange_Date_To_Other(start, end);
            return iTech.Get_DataTable(sqlcmd, source);
        }

        public DataTable Change_Date_To_Other(DateTime start, DateTime end, string source)
        {
            return Change_Date_To_Other(start.ToString(DateFormat), end.ToString(DateFormat), source);
        }

        public DataTable Change_Now_To_Now(string start, string end, string source)
        {
            iniManager = new IniManager($"{ConfigurationManager.AppSettings["ini_local"]}{source}Erp.ini");
            string sqlcmd = GetChange_Now_To_Now(start, end);
            return iTech.Get_DataTable(sqlcmd, source);
        }

        public DataTable Change_Now_To_Now(DateTime start, DateTime end, string source)
        {
            return Change_Now_To_Now(start.ToString(DateFormat), end.ToString(DateFormat), source);
        }

        //20220816生產推移圖立式資料
        public string Waitingfortheproduction_Assm_Table(string start, string end, string source)
        {
            iniManager = new IniManager($"{ConfigurationManager.AppSettings["ini_local"]}{source}Erp.ini");
            string sqlcmd = GET_Waitingfortheproduction_Assm_Table(start, end);
            return sqlcmd;
        }
        //20220816生產推移圖臥式資料
        public string Waitingfortheproduction_Hor_Table(string upper_month,string condition, string start, string end, string source)
        {
            iniManager = new IniManager($"{ConfigurationManager.AppSettings["ini_local"]}{source}Erp.ini");
            string sqlcmd = GET_Waitingfortheproduction_Hor_Table(upper_month, condition, start, end);
            return sqlcmd;
        }
        public string Waitingfortheproduction_BigDisc_Table(string upper_month, string start, string end, string source)
        {
            iniManager = new IniManager($"{ConfigurationManager.AppSettings["ini_local"]}{source}Erp.ini");
            string sqlcmd = GET_Waitingfortheproduction_BigDisc_Table(upper_month,start, end);
            return sqlcmd;
        }
        //----------------------------------------------------語法產生處---------------------------------------------------------
        //訂單數量與金額統計
        string GetOrders_Detail(string start, string end, OrderStatus status, bool detail)
        {
            StringBuilder sqlcmd = new StringBuilder();
            StringBuilder Condition = new StringBuilder();
            StringBuilder in_house = new StringBuilder();
            StringBuilder in_house_column = new StringBuilder();

            if (detail)
            {
                if (iniManager.ReadIniFile("Parameter", "in_house", "") != "")
                    in_house.Append(iniManager.ReadIniFile("Parameter", "in_house", ""));
                if (iniManager.ReadIniFile("Parameter", "in_house_column", "") != "")
                    in_house_column.Append(iniManager.ReadIniFile("Parameter", "in_house_column", ""));
            }

            if (status != OrderStatus.All && iniManager.ReadIniFile("Parameter", "Orders_" + status, "") != "")
                Condition.Append(iniManager.ReadIniFile("Parameter", "Orders_" + status, ""));

            if (iniManager.ReadIniFile("dekERPVIS", "Orders_Details", "") != "")
                sqlcmd.AppendFormat(iniManager.ReadIniFile("dekERPVIS", "Orders_Details", ""), start, end, Condition, in_house, in_house_column);

            return sqlcmd.ToString();
        }

        //訂單逾期數量
        string GetOrders_Over_Detail(string start, bool detail)
        {
            StringBuilder sqlcmd = new StringBuilder();
            StringBuilder in_house = new StringBuilder();
            StringBuilder in_house_column = new StringBuilder();

            if (detail)
            {
                if (iniManager.ReadIniFile("Parameter", "in_house", "") != "")
                    in_house.Append(iniManager.ReadIniFile("Parameter", "in_house", ""));
                if (iniManager.ReadIniFile("Parameter", "in_house_column", "") != "")
                    in_house_column.Append(iniManager.ReadIniFile("Parameter", "in_house_column", ""));
            }

            if (iniManager.ReadIniFile("dekERPVIS", "Orders_Over_Detail", "") != "")
                sqlcmd.AppendFormat(iniManager.ReadIniFile("dekERPVIS", "Orders_Over_Detail", ""), start, in_house, in_house_column);

            return sqlcmd.ToString();
        }
        //20220812 大圓盤逾期語法優化,新增判斷客戶名稱
        string GetOrders_Over_Detail_Customer(string start, bool detail,string customer)
        {
            StringBuilder sqlcmd = new StringBuilder();
            StringBuilder customer_name = new StringBuilder();

            if (detail)
                if (iniManager.ReadIniFile("Parameter", "Over_Detail_Customer", "") != "")
                    customer_name.AppendFormat(iniManager.ReadIniFile("Parameter", "Over_Detail_Customer", ""), customer);

            if (iniManager.ReadIniFile("dekERPVIS", "Orders_Over_Detail_Customer", "") != "")
                sqlcmd.AppendFormat(iniManager.ReadIniFile("dekERPVIS", "Orders_Over_Detail_Customer", ""), start, customer_name);

            return sqlcmd.ToString();
        }

        //出貨統計表
        string GetShipment_Detail(string start, string end)
        {
            StringBuilder sqlcmd = new StringBuilder();
            if (iniManager.ReadIniFile("dekERPVIS", "Shipment_Detail", "") != "")
                sqlcmd.AppendFormat(iniManager.ReadIniFile("dekERPVIS", "Shipment_Detail", ""), start, end);
            return sqlcmd.ToString();
        }

        //出貨小計
        string GetShipment(string start, string end, string custom, string item)
        {
            StringBuilder sqlcmd = new StringBuilder();
            if (iniManager.ReadIniFile("dekERPVIS", "Get_Shipment", "") != "")
                sqlcmd.AppendFormat(iniManager.ReadIniFile("dekERPVIS", "Get_Shipment", ""), start, end, custom, item);
            return sqlcmd.ToString();
        }

        //未交易客戶
        string GetUntradedCustomer(string start, string end, string symbol, int day, string source)
        {

            StringBuilder sqlcmd = new StringBuilder();
            StringBuilder Condition = new StringBuilder();

            if (start != "" && end != "" && iniManager.ReadIniFile("Parameter", "UntradedCustomer_Time", "") != "")
                Condition.AppendFormat(iniManager.ReadIniFile("Parameter", "UntradedCustomer_Time", ""), start, end);

            if (iniManager.ReadIniFile("dekERPVIS", "UntradedCustomer", "") != "")
            {
                if (source == "dek")
                    sqlcmd.AppendFormat(iniManager.ReadIniFile("dekERPVIS", "UntradedCustomer", ""), symbol, day, Condition, DateTime.Now.ToString(DateFormat));
                else
                    sqlcmd.AppendFormat(iniManager.ReadIniFile("dekERPVIS", "UntradedCustomer", ""), symbol, day, Condition);
            }


            return sqlcmd.ToString();
        }

        //訂單變更紀錄
        string GetRecordsofchangetheorder_Details(string start, string end)
        {
            StringBuilder sqlcmd = new StringBuilder();
            if (iniManager.ReadIniFile("dekERPVIS", "recordsofchangetheorder_details", "") != "")
                sqlcmd.AppendFormat(iniManager.ReadIniFile("dekERPVIS", "recordsofchangetheorder_details", ""), start, end);
            return sqlcmd.ToString();
        }


        //20220816生產推移圖立式語法格式化
        string GET_Waitingfortheproduction_Assm_Table(string start, string end)
        {
            StringBuilder sqlcmd = new StringBuilder();
            if (iniManager.ReadIniFile("dekERPVIS", "Waitingfortheproduction", "") != "")
                sqlcmd.AppendFormat(iniManager.ReadIniFile("dekERPVIS", "Waitingfortheproduction", ""), start, end);
            return sqlcmd.ToString();
        }

        //20220816生產推移圖臥式語法格式化
        string GET_Waitingfortheproduction_Hor_Table(string upper_month,string condition, string start, string end)
        {
            StringBuilder sqlcmd = new StringBuilder();
            if (iniManager.ReadIniFile("dekERPVIS", "Waitingfortheproduction_Hor", "") != "")
            sqlcmd.AppendFormat(iniManager.ReadIniFile("dekERPVIS", "Waitingfortheproduction_Hor", ""), upper_month, condition, start, end);
            return sqlcmd.ToString();
        }

        //20220816生產推移圖大圓盤語法格式化
        string GET_Waitingfortheproduction_BigDisc_Table(string upper_month,string start, string end)
        {
            StringBuilder sqlcmd = new StringBuilder();
            if (iniManager.ReadIniFile("dekERPVIS", "Waitingfortheproduction_BigDisc", "") != "")
                sqlcmd.AppendFormat(iniManager.ReadIniFile("dekERPVIS", "Waitingfortheproduction_BigDisc", ""), upper_month, end);
            return sqlcmd.ToString();
        }

        //運輸架未歸還統計
        string GetTransportrackstatistics(string acc)
        {
            StringBuilder sqlcmd = new StringBuilder();
            List<string> Transport_rack = new List<string>();
            string condition = "";
            if (iniManager.ReadIniFile("Get_Item", "Item_No", "") != "")
                Transport_rack = new List<string>(iniManager.ReadIniFile("Get_Item", "Item_No", "").Split(','));
            string columns = iniManager.ReadIniFile("Parameter", "Transport", "");
            int x = acc == "visrd" || acc == "detawink" || acc == "detating" || acc == "" ? 0 : 1;

            for (int i = 0; i < Transport_rack.Count - x; i++)
                condition += condition == "" ? $" {columns}='{Transport_rack[i]}' " : $" or  {columns}='{Transport_rack[i]}' ";

            condition = condition != "" ? $" and ( {condition} ) " : "";

            if (iniManager.ReadIniFile("dekERPVIS", $"Transportrackstatistics", "") != "" && condition != "")
                sqlcmd.AppendFormat(iniManager.ReadIniFile("dekERPVIS", $"Transportrackstatistics", ""), condition);

            return sqlcmd.ToString();

        }

        //該訂單之預計開工日
        string GetOrders_StartDay(string start, string end)
        {
            StringBuilder sqlcmd = new StringBuilder();
            if (iniManager.ReadIniFile("dekERPVIS", "Orders_StartDay", "") != "")
                sqlcmd.AppendFormat(iniManager.ReadIniFile("dekERPVIS", "Orders_StartDay", ""), start, end);
            return sqlcmd.ToString();
        }

        //每位客戶各月之訂單
        string GetOrders_Month(string start, string end, bool detail = false)
        {
            StringBuilder sqlcmd = new StringBuilder();
            StringBuilder in_house = new StringBuilder();

            if (detail)
                if (iniManager.ReadIniFile("Parameter", "in_house", "") != "")
                    in_house.Append(iniManager.ReadIniFile("Parameter", "in_house", ""));

            if (iniManager.ReadIniFile("dekERPVIS", "Orders_Month", "") != "")
                sqlcmd.AppendFormat(iniManager.ReadIniFile("dekERPVIS", "Orders_Month", ""), start, end, in_house);

            return sqlcmd.ToString();
        }

        string GetChange_Date_To_Now(string start, string end)
        {
            StringBuilder sqlcmd = new StringBuilder();
            if (iniManager.ReadIniFile("dekERPVIS", "Change_Date_To_Now", "") != "")
                sqlcmd.AppendFormat(iniManager.ReadIniFile("dekERPVIS", "Change_Date_To_Now", ""), start, end);
            return sqlcmd.ToString();
        }
        string GetChange_Date_To_Other(string start, string end)
        {
            StringBuilder sqlcmd = new StringBuilder();
            if (iniManager.ReadIniFile("dekERPVIS", "Change_Date_To_Other", "") != "")
                sqlcmd.AppendFormat(iniManager.ReadIniFile("dekERPVIS", "Change_Date_To_Other", ""), start, end);
            return sqlcmd.ToString();
        }
        string GetChange_Now_To_Now(string start, string end)
        {
            StringBuilder sqlcmd = new StringBuilder();
            if (iniManager.ReadIniFile("dekERPVIS", "Change_Now_To_Now", "") != "")
                sqlcmd.AppendFormat(iniManager.ReadIniFile("dekERPVIS", "Change_Now_To_Now", ""), start, end);
            return sqlcmd.ToString();
        }
    }
}
