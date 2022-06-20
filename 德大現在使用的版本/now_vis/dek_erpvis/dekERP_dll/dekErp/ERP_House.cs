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
    public class ERP_House : Get_House
    {
        iTechDB iTech = new iTechDB();
        const string DateFormat = "yyyyMMdd";
        IniManager iniManager = new IniManager(ConfigurationManager.AppSettings["ini_road"]);

        public DataTable stockanalysis_Details(string source, string custom = "")
        {
            iniManager = new IniManager($"{ConfigurationManager.AppSettings["ini_local"]}{source}Erp.ini");
            string sqlcmd = Getstockanalysis_Details();
            return iTech.Get_DataTable(sqlcmd, source);
        }

        public DataTable Inventory_Total_Amount(CheckBoxList warehouse,CheckBoxList type, string source)
        {
            iniManager = new IniManager($"{ConfigurationManager.AppSettings["ini_local"]}{source}Erp.ini");
            string sqlcmd = GetInventory_Total_Amount(warehouse,type);
            return iTech.Get_DataTable(sqlcmd, source);
        }

        public DataTable InactiveInventory(CheckBoxList item_type, CheckBoxList warehouse, string day, string source)
        {
            iniManager = new IniManager($"{ConfigurationManager.AppSettings["ini_local"]}{source}Erp.ini");
            string sqlcmd = GetInactiveInventory(item_type, warehouse, day);
            return iTech.Get_DataTable(sqlcmd, source);
        }

        public DataTable Scrapped(string start, string end, CheckBoxList Scrapped_personnel, string source)
        {
            iniManager = new IniManager($"{ConfigurationManager.AppSettings["ini_local"]}{source}Erp.ini");
            string sqlcmd = GetScrapped(start, end, Scrapped_personnel);
            return iTech.Get_DataTable(sqlcmd, source);
        }

        public DataTable Scrapped(DateTime start, DateTime end, CheckBoxList Scrapped_personnel, string source)
        {
            return Scrapped(start.ToString(DateFormat), end.ToString(DateFormat), Scrapped_personnel, source);
        }

        //----------------------------------------------------語法產生處---------------------------------------------------------
        //成品庫存分析
        string Getstockanalysis_Details()
        {
            StringBuilder sqlcmd = new StringBuilder();
            if (iniManager.ReadIniFile("dekERPVIS", "stockanalysis_details", "") != "")
                sqlcmd.AppendFormat(iniManager.ReadIniFile("dekERPVIS", "stockanalysis_details", ""));
            return sqlcmd.ToString();
        }
        //庫存明細列表
        string GetInventory_Total_Amount(CheckBoxList warehouse,CheckBoxList type)
        {
            StringBuilder sqlcmd = new StringBuilder();
            string condition = "";
            string condition_type = "";
            string Total_warehouse = iniManager.ReadIniFile("Parameter", "Total_warehouse", "");
            string Total_type = iniManager.ReadIniFile("Parameter", "Total_type", "");
            
            //設定所選之倉庫位置
            if (Total_warehouse != "")
            {
                for (int i = 1; i < warehouse.Items.Count; i++)
                {
                    if (warehouse.Items[i].Selected)
                        condition += condition == "" ? $" {Total_warehouse} = '{warehouse.Items[i].Value}' " : $" OR {Total_warehouse} = '{warehouse.Items[i].Value}' ";
                }
                condition = condition != "" ? $" AND ({condition}) " : "";
            }

            //所選之類別
            if(Total_type != "")
            {
                for (int i = 1; i < type.Items.Count; i++)
                {
                    if (type.Items[i].Selected)
                        condition_type += condition_type == "" ? $" {Total_type} = '{type.Items[i].Value}' " : $" OR {Total_type} = '{type.Items[i].Value}' ";
                }
                condition_type = condition_type != "" ? $" AND ({condition_type}) " : "";
            }

            if (iniManager.ReadIniFile("dekERPVIS", "Inventory_Total_Amount", "") != "")
                sqlcmd.AppendFormat(iniManager.ReadIniFile("dekERPVIS", "Inventory_Total_Amount", ""), condition, condition_type);
            return sqlcmd.ToString();
        }
        //呆滯物料統計表
        string GetInactiveInventory(CheckBoxList item_type, CheckBoxList warehouse, string day)
        {
            StringBuilder sqlcmd = new StringBuilder();
            string class_name = iniManager.ReadIniFile("Parameter", "class_name", "");
            string InactiveInventory_warehouse = iniManager.ReadIniFile("Parameter", "InactiveInventory_warehouse", "");

            string class_condition = "";
            string warehouse_condition = "";

            if (class_name != "")
            {
                for (int i = 1; i < item_type.Items.Count; i++)
                {
                    if (item_type.Items[i].Selected)
                        class_condition += class_condition == "" ? $" {class_name}='{item_type.Items[i].Value}' " : $" or {class_name}='{item_type.Items[i].Value}' ";
                }
                class_condition = class_condition != "" ? $" and ( {class_condition} ) " : "";
            }

            if (InactiveInventory_warehouse != "")
            {
                for (int i = 1; i < warehouse.Items.Count; i++)
                {
                    if (warehouse.Items[i].Selected)
                        warehouse_condition += warehouse_condition == "" ? $" {InactiveInventory_warehouse}='{warehouse.Items[i].Value}' " : $" or {InactiveInventory_warehouse}='{warehouse.Items[i].Value}' ";
                }
                warehouse_condition = warehouse_condition != "" ? $" and ( {warehouse_condition} ) " : "";
            }

            if (iniManager.ReadIniFile("dekERPVIS", "InactiveInventory", "") != "" && class_condition != "" && warehouse_condition != "")
                sqlcmd.AppendFormat(iniManager.ReadIniFile("dekERPVIS", "InactiveInventory", ""), day, DateTime.Now.AddDays(1).ToString("yyyyMMdd"), class_condition, warehouse_condition);
            return sqlcmd.ToString();
        }
        //報廢數量統計表
        string GetScrapped(string start, string end, CheckBoxList Scrapped_personnel)
        {
            StringBuilder sqlcmd = new StringBuilder();

            string Scrapped_List = iniManager.ReadIniFile("Parameter", "Scrapped_personnel", "");
            string Scrapped_List_condition = "";

            if (Scrapped_List != "")
            {
                for (int i = 0; i < Scrapped_personnel.Items.Count; i++)
                {
                    if (Scrapped_personnel.Items[i].Selected)
                        Scrapped_List_condition += Scrapped_List_condition == "" ? $" {Scrapped_List}='{Scrapped_personnel.Items[i].Value}' " : $" or {Scrapped_List}='{Scrapped_personnel.Items[i].Value}' ";
                }
                Scrapped_List_condition = Scrapped_List_condition != "" ? $" and ( {Scrapped_List_condition} ) " : "";
            }

            if (iniManager.ReadIniFile("dekERPVIS", "Scrapped", "") != "" && Scrapped_List_condition != "")
                sqlcmd.AppendFormat(iniManager.ReadIniFile("dekERPVIS", "Scrapped", ""), start, end, Scrapped_List_condition);
            return sqlcmd.ToString();
        }
    }
}
