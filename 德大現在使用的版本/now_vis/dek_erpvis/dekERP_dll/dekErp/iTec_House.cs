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
    public class iTec_House : IdepHouse
    {
        iTechDB iTech = new iTechDB();
        const string DateFormat = "yyyyMMdd";
        IniManager iniManager = new IniManager(ConfigurationManager.AppSettings["ini_road"]);

        //成品庫存分析
        public DataTable stockanalysis(dekModel img_or_tbl, int day)
        {
            string sqlcmd = Getstockanalysis(img_or_tbl, day);
            DataTable dt = iTech.Get_DataTable(sqlcmd);
            return iTech.Error_DataTable(dt, sqlcmd);
        }
        //成品庫存分析-細節 
        public DataTable stockanalysis_Details(string start, string custom)
        {
            string sqlcmd = Getstockanalysis_Details(start, custom);
            DataTable dt = iTech.Get_DataTable(sqlcmd);
            return iTech.Error_DataTable(dt, sqlcmd);
        }

        public DataTable stockanalysis_Details(DateTime dt_st, string custom)
        {
            return stockanalysis_Details(dt_st.ToString(DateFormat), custom);
        }

        //呆滯物料統計表
        public DataTable InactiveInventory(string item_type, string warehouse, string day)
        {
            string sqlcmd = GetInactiveInventory(item_type, warehouse, day);
            DataTable dt = iTech.Get_DataTable(sqlcmd);
            return iTech.Error_DataTable(dt, sqlcmd);
        }

        //報廢數量統計表
        public DataTable Scrapped(string start, string end, string condition)
        {
            string sqlcmd = GetScrapped(start, end, condition);
            DataTable dt = iTech.Get_DataTable(sqlcmd);
            return iTech.Error_DataTable(dt, sqlcmd);
        }

        public DataTable Scrapped(DateTime dt_st, DateTime dt_ed, string condition)
        {
            return Scrapped(dt_st.ToString(DateFormat), dt_ed.ToString(DateFormat), condition);
        }

        //庫存數量列表
        public DataTable Inventory_Total_Amount(CheckBoxList cbx)
        {
            string sqlcmd = Get_Inventory_Total_Amount(cbx);
            DataTable dt = iTech.Get_DataTable(sqlcmd);
            return iTech.Error_DataTable(dt, sqlcmd);
        }



        //--------------------------------------------------SQL指令專區----------------------------------------------
        //成品庫存分析
        string Getstockanalysis(dekModel img_or_tbl, int day)
        {
            StringBuilder sqlcmd = new StringBuilder();
            if (iniManager.ReadIniFile("dekERPVIS", "stockanalysis_" + img_or_tbl, "") != "")
                sqlcmd.AppendFormat(iniManager.ReadIniFile("dekERPVIS", "stockanalysis_" + img_or_tbl, ""), day.ToString());
            return sqlcmd.ToString();
        }

        //成品庫存分析 細節
        string Getstockanalysis_Details(string start, string custom)
        {
            StringBuilder sqlcmd = new StringBuilder();
            if (iniManager.ReadIniFile("dekERPVIS", "stockanalysis_details", "") != "")
                sqlcmd.AppendFormat(iniManager.ReadIniFile("dekERPVIS", "stockanalysis_details", ""), start, custom);
            return sqlcmd.ToString();
        }

        //呆滯物料統計表
        string GetInactiveInventory(string item_type, string warehouse, string day)
        {
            StringBuilder sqlcmd = new StringBuilder();
            if (iniManager.ReadIniFile("dekERPVIS", "InactiveInventory", "") != "")
                sqlcmd.AppendFormat(iniManager.ReadIniFile("dekERPVIS", "InactiveInventory", ""), item_type, warehouse, day, DateTime.Now.AddDays(1).ToString("yyyyMMdd"));
            return sqlcmd.ToString();
        }
        //報廢數量統計表
        string GetScrapped(string start, string end, string condition)
        {
            StringBuilder sqlcmd = new StringBuilder();
            if (condition != "")
                condition = " AND ( " + condition + " )";
            if (iniManager.ReadIniFile("dekERPVIS", "Scrapped", "") != "")
                sqlcmd.AppendFormat(iniManager.ReadIniFile("dekERPVIS", "Scrapped", ""), start, end, condition);
            return sqlcmd.ToString();
        }

        //庫存數量列表
        string Get_Inventory_Total_Amount(CheckBoxList cbx)
        {
            StringBuilder sqlcmd = new StringBuilder();
            string condition = "";
            for (int i = 0; i < cbx.Items.Count; i++)
            {
                if (cbx.Items[i].Selected)
                    condition += condition == "" ? $" ITEM_GD.GDNO = '{cbx.Items[i].Value}' " : $" OR ITEM_GD.GDNO = '{cbx.Items[i].Value}' ";
            }
            condition = condition != "" ? $" AND ({condition}) " : "";

            if (iniManager.ReadIniFile("dekERPVIS", "Inventory_Total_Amount", "") != "")
                sqlcmd.AppendFormat(iniManager.ReadIniFile("dekERPVIS", "Inventory_Total_Amount", ""), condition);
            return sqlcmd.ToString();


        }
    }
}
