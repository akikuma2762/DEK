using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace dekERP_dll.dekErp
{
    public class ERP_Materials : Get_Material
    {
        iTechDB iTech = new iTechDB();
        const string DateFormat = "yyyyMMdd";
        IniManager iniManager = new IniManager("");

        public DataTable MaxOrder(string source)
        {
            iniManager = new IniManager($"{ConfigurationManager.AppSettings["ini_local"]}{source}Erp.ini");
            string sqlcmd = GetMaxOrder();
            return iTech.Get_DataTable(sqlcmd, source);
        }

       public DataTable Shortage(string source)
        {
            iniManager = new IniManager($"{ConfigurationManager.AppSettings["ini_local"]}{source}Erp.ini");
            string sqlcmd = GetShortage();
            return iTech.Get_DataTable(sqlcmd, source);
        }

        public DataTable SupplierShortage_Urge(string supplier, string supplierName, string start, string end, string itemno, string maxorder, string source)
        {
            iniManager = new IniManager($"{ConfigurationManager.AppSettings["ini_local"]}{source}Erp.ini");
            string sqlcmd = GetSupplierShortage_Urge(supplier, supplierName, start, end, itemno, maxorder);
            return iTech.Get_DataTable(sqlcmd, source);
        }

        public DataTable SupplierShortage_Urge(string supplier, string supplierName, DateTime start, DateTime end, string itemno, string maxorder, string source)
        {
            return SupplierShortage_Urge(supplier, supplierName, start.ToString(DateFormat), end.ToString(DateFormat), itemno, maxorder, source);
        }

        public DataTable SupplierShortage_Delivery(List<string> orders, string source)
        {
            iniManager = new IniManager($"{ConfigurationManager.AppSettings["ini_local"]}{source}Erp.ini");
            string sqlcmd = GetSupplierShortage_Delivery(orders);
            return iTech.Get_DataTable(sqlcmd, source);
        }

        public DataTable Supplierscore_Detail(string start, string end, string source, string custom = "")
        {
            iniManager = new IniManager($"{ConfigurationManager.AppSettings["ini_local"]}{source}Erp.ini");
            string sqlcmd = GetSupplierscore_Detail(start, end);
            return iTech.Get_DataTable(sqlcmd, source);
        }

        public DataTable Supplierscore_Detail(DateTime start, DateTime end, string source, string custom = "")
        {
            return Supplierscore_Detail(start.ToString(DateFormat), end.ToString(DateFormat), source, custom);
        }

        public DataTable materialrequirementplanning_Detail(string start, string end, RadioButtonList rbx, string item, string source)
        {
            iniManager = new IniManager($"{ConfigurationManager.AppSettings["ini_local"]}{source}Erp.ini");
            string sqlcmd = Getmaterialrequirementplanning_Detail(start, end, rbx, item);
            return iTech.Get_DataTable(sqlcmd, source);
        }

        public DataTable materialrequirementplanning_Detail(DateTime start, DateTime end, RadioButtonList rbx, string item, string source)
        {
            return materialrequirementplanning_Detail(start.ToString(DateFormat), end.ToString(DateFormat), rbx, item, source);
        }

        public DataTable pick_list_title(string Number, string source)
        {
            iniManager = new IniManager($"{ConfigurationManager.AppSettings["ini_local"]}{source}Erp.ini");
            string sqlcmd = Getpick_list_title(Number);
            return iTech.Get_DataTable(sqlcmd, source);
        }

        public DataTable pick_list_datatable(string Number, string source)
        {
            iniManager = new IniManager($"{ConfigurationManager.AppSettings["ini_local"]}{source}Erp.ini");
            string sqlcmd = Getpick_list_datatable(Number);
            return iTech.Get_DataTable(sqlcmd, source);
        }

        public DataTable Item_DataTable(string ini_Name, string source, string start = "", string end = "")
        {
            iniManager = new IniManager($"{ConfigurationManager.AppSettings["ini_local"]}{source}Erp.ini");
            string sqlcmd = GetItem_DataTable(ini_Name, start, end);
            return iTech.Get_DataTable(sqlcmd, source);
        }
        //----------------------------------------------------語法產生處---------------------------------------------------------

        //取得當前單號
        string GetMaxOrder()
        {
            StringBuilder sqlcmd = new StringBuilder();
            if (iniManager.ReadIniFile("Get_Item", "MaxOrder", "") != "")
                sqlcmd.AppendFormat(iniManager.ReadIniFile("Get_Item", "MaxOrder", ""));
            return sqlcmd.ToString();
        }

        //取得催料日期
        string GetShortage()
        {
            StringBuilder sqlcmd = new StringBuilder();
            if (iniManager.ReadIniFile("Get_Item", "Shortage", "") != "")
                sqlcmd.AppendFormat(iniManager.ReadIniFile("Get_Item", "Shortage", ""));
            return sqlcmd.ToString();
        }

        //取得當前催料表
        string GetSupplierShortage_Urge(string supplier, string supplierName, string start, string end, string itemno, string maxorder)
        {
            StringBuilder sqlcmd = new StringBuilder();

            string supplier_no = iniManager.ReadIniFile("Parameter", "supplier_no", "");
            string supplier_name = iniManager.ReadIniFile("Parameter", "supplier_name", "");
            string item_no = iniManager.ReadIniFile("Parameter", "item_no", "");
            string SupplierShortage_date = iniManager.ReadIniFile("Parameter", "SupplierShortage_date", "");

            supplier = supplier != "" && supplier_no != "" ? string.Format(supplier_no, supplier) : "";
            supplierName = supplierName != "" && supplier_name != "" ? string.Format(supplier_name, supplierName) : "";
            itemno = itemno != "" && item_no != "" ? string.Format(item_no, itemno) : "";
            SupplierShortage_date = SupplierShortage_date != "" && start != "" && end != "" ? string.Format(SupplierShortage_date, start, end) : "";

            if (iniManager.ReadIniFile("dekERPVIS", "SupplierShortage_Urge", "") != "")
                sqlcmd.AppendFormat(iniManager.ReadIniFile("dekERPVIS", "SupplierShortage_Urge", ""), maxorder, supplier, supplierName, itemno, SupplierShortage_date);
            return sqlcmd.ToString();
        }

        //取得對應的採購/加工單
        string GetSupplierShortage_Delivery(List<string> orders)
        {
            string SupplierShortage_Column = iniManager.ReadIniFile("Parameter", "SupplierShortage_Column", "");
            string Condition = "";
            if (SupplierShortage_Column != "" && orders.Count > 0)
                for (int i = 0; i < orders.Count; i++)
                    Condition += Condition == "" ? $" {SupplierShortage_Column}='{orders[i]}' " : $" OR {SupplierShortage_Column}='{orders[i]}' ";
            Condition = Condition != "" ? $" where ({Condition}) " : ""; 

            StringBuilder sqlcmd = new StringBuilder();
            if (iniManager.ReadIniFile("dekERPVIS", "SupplierShortage_Delivery", "") != "")
                sqlcmd.AppendFormat(iniManager.ReadIniFile("dekERPVIS", "SupplierShortage_Delivery", ""), Condition);
            return sqlcmd.ToString();
        }

        //供應商達交率
        string GetSupplierscore_Detail(string start, string end)
        {
            StringBuilder sqlcmd = new StringBuilder();
            if (iniManager.ReadIniFile("dekERPVIS", "Supplierscore_Detail", "") != "")
                sqlcmd.AppendFormat(iniManager.ReadIniFile("dekERPVIS", "Supplierscore_Detail", ""), start, end);
            return sqlcmd.ToString();
        }

        //物料領用統計表
        string Getmaterialrequirementplanning_Detail(string start, string end, RadioButtonList rbx, string item)
        {
            StringBuilder sqlcmd = new StringBuilder();
            StringBuilder Condition = new StringBuilder();
            string sql = rbx.SelectedItem.Value == "1" ? "item" : rbx.SelectedItem.Value == "4" ? "dtl" : "Name";
            //看要查詢的是品號還是品名
            if (item != "" && iniManager.ReadIniFile("Parameter", $"materialrequirementplanning_{sql}", "") != "")
                Condition.AppendFormat(iniManager.ReadIniFile("Parameter", $"materialrequirementplanning_{sql}", ""), item);

            if (iniManager.ReadIniFile("dekERPVIS", "materialrequirementplanning_Detail", "") != "")
                sqlcmd.AppendFormat(iniManager.ReadIniFile("dekERPVIS", "materialrequirementplanning_Detail", ""), start, end, Condition);

            return sqlcmd.ToString();
        }

        //取得下拉選單 核取方塊 單選框來源
        string GetItem_DataTable(string ini_name, string start = "", string end = "")
        {
            StringBuilder sqlcmd = new StringBuilder();
            if (iniManager.ReadIniFile("Get_Item", ini_name, "") != "")
            {
                if (start != "" && end != "")
                    sqlcmd.AppendFormat(iniManager.ReadIniFile("Get_Item", ini_name, ""), start, end);
                else
                    sqlcmd.Append(iniManager.ReadIniFile("Get_Item", ini_name, ""));
            }
            return sqlcmd.ToString();
        }

        //產生領料表表頭
        string Getpick_list_title(string Number)
        {
            List<string> list = new List<string>(Number.Split('#'));
            string lot_no = iniManager.ReadIniFile("Parameter", "lot_no", "");
            string mkord_no = iniManager.ReadIniFile("Parameter", "mkord_no", "");
            string condition = "";
            for (int i = 0; i < list.Count - 1; i++)
            {
                if (list[i] != "")
                    condition += condition == "" ? $" {lot_no}='{list[i]}' OR {mkord_no}='{list[i]}' " : $" OR {lot_no}='{list[i]}' OR {mkord_no}='{list[i]}' ";
            }
            condition = condition == "" ? "" : $" and ( {condition} ) ";

            StringBuilder sqlcmd = new StringBuilder();
            if (iniManager.ReadIniFile("dekERPVIS", "pick_list_title", "") != "" && condition != "")
                sqlcmd.AppendFormat(iniManager.ReadIniFile("dekERPVIS", "pick_list_title", ""), condition);

            return sqlcmd.ToString();
        }

        //產生領料表表格
        string Getpick_list_datatable(string Number)
        {
            List<string> list = new List<string>(Number.Split('#'));
            string condition = "";
            string lot_no = iniManager.ReadIniFile("Parameter", "lot_no", "");
            string mkord_no = iniManager.ReadIniFile("Parameter", "mkord_no", "");
            for (int i = 0; i < list.Count - 1; i++)
            {
                if (list[i] != "")
                    condition += condition == "" ? $" {lot_no}='{list[i]}' OR {mkord_no}='{list[i]}' " : $" OR {lot_no}='{list[i]}' OR {mkord_no}='{list[i]}' ";
            }
            condition = condition == "" ? "" : $" and ( {condition} ) ";

            StringBuilder sqlcmd = new StringBuilder();
            if (iniManager.ReadIniFile("dekERPVIS", "pick_list_datatable", "") != "" && condition != "")
                sqlcmd.AppendFormat(iniManager.ReadIniFile("dekERPVIS", "pick_list_datatable", ""), condition);
            return sqlcmd.ToString();

        }

    }
}
