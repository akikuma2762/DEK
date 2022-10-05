using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace dekERP_dll.dekErp
{
    public class iTec_Materials : IdepMaterial
    {
        iTechDB iTech = new iTechDB();
        const string DateFormat = "yyyyMMdd";
        IniManager iniManager = new IniManager(ConfigurationManager.AppSettings["ini_road"]);

        //供應商達交率
        public DataTable Supplierscore(string start, string end)
        {
            string sqlcmd = GetSupplierscore(start, end);
            DataTable dt = iTech.Get_DataTable(sqlcmd);
            return iTech.Error_DataTable(dt, sqlcmd);
        }

        public DataTable Supplierscore(DateTime dt_st, DateTime dt_ed)
        {
            return Supplierscore(dt_st.ToString(DateFormat), dt_ed.ToString(DateFormat));
        }

        //供應商達交率細項
        public DataTable Supplierscore_Detail(string start, string end, string cust)
        {
            string sqlcmd = GetSupplierscore_Detail(start, end, cust);
            DataTable dt = iTech.Get_DataTable(sqlcmd);
            return iTech.Error_DataTable(dt, sqlcmd);
        }

        public DataTable Supplierscore_Detail(DateTime dt_st, DateTime dt_ed, string cust)
        {
            return Supplierscore_Detail(dt_st.ToString(DateFormat), dt_ed.ToString(DateFormat), cust);
        }

        //歷史用料統計表
        public DataTable materialrequirementplanning(string start, string end, string type, string item)
        {
            string sqlcmd = Getmaterialrequirementplanning(start, end, type, item);
            DataTable dt = iTech.Get_DataTable(sqlcmd);
            return iTech.Error_DataTable(dt, sqlcmd);
        }

        public DataTable materialrequirementplanning(DateTime dt_st, DateTime dt_ed, string type, string item)
        {
            return materialrequirementplanning(dt_st.ToString(DateFormat), dt_ed.ToString(DateFormat), type, item);
        }

        //歷史用料統計表-細項
        public DataTable materialrequirementplanning_Detail(string item, string start, string end, dekModel model)
        {
            string sqlcmd = Getmaterialrequirementplanning_Detail(item, start, end, model);
            DataTable dt = iTech.Get_DataTable(sqlcmd);
            return iTech.Error_DataTable(dt, sqlcmd);
        }

        public DataTable materialrequirementplanning_Detail(string item, DateTime dt_st, DateTime dt_ed, dekModel model)
        {
            return materialrequirementplanning_Detail(item, dt_st.ToString(DateFormat), dt_ed.ToString(DateFormat), model);
        }

        //供應商催料總表
        public DataTable SupplierShortage(SupplierShortageType type, string supplier, string supplierName, string start, string end, string itemNo, string Reminder_Date)
        {
            string sqlcmd = GetSupplierShortage(type, supplier, supplierName, start, end, itemNo, Reminder_Date);
            DataTable dt = iTech.Get_DataTable(sqlcmd);
            return iTech.Error_DataTable(dt, sqlcmd);
        }

        //產生核取方塊(下拉選單)用
        public DataTable Item_DataTable(string ini_Name, string start = "", string end = "")
        {
            string sqlcmd = GetItem_DataTable(ini_Name, start, end);
            DataTable dt = iTech.Get_DataTable(sqlcmd);
            return iTech.Error_DataTable(dt, sqlcmd);
        }

        //產生領料表表頭
        public DataTable pick_list_title(string Number)
        {

            string sqlcmd = Getpick_list_title(Number);
            DataTable dt = iTech.Get_DataTable(sqlcmd);
            return iTech.Error_DataTable(dt, sqlcmd);
        }

        //產生領料表表格
        public DataTable pick_list_datatable(string Number)
        {

            string sqlcmd = Getpick_list_datatable(Number);
            DataTable dt = iTech.Get_DataTable(sqlcmd);
            return iTech.Error_DataTable(dt, sqlcmd);
        }


        //-----------------------------------SQL語法專區----------------------------------------------------------
        //供應商達交率
        string GetSupplierscore(string start, string end)
        {
            StringBuilder sqlcmd = new StringBuilder();
            if (iniManager.ReadIniFile("dekERPVIS", "supplierscore", "") != "")
                sqlcmd.AppendFormat(iniManager.ReadIniFile("dekERPVIS", "supplierscore", ""), start, end);

            return sqlcmd.ToString();
        }

        //供應商達交率細項
        string GetSupplierscore_Detail(string start, string end, string cust)
        {
            StringBuilder sqlcmd = new StringBuilder();
            if (iniManager.ReadIniFile("dekERPVIS", "supplierscore_Details", "") != "")
                sqlcmd.AppendFormat(iniManager.ReadIniFile("dekERPVIS", "supplierscore_Details", ""), start, end, cust);
            return sqlcmd.ToString();
        }

        //歷史用料統計表
        string Getmaterialrequirementplanning(string start, string end, string type, string item)
        {
            StringBuilder sqlcmd = new StringBuilder();
            if (iniManager.ReadIniFile("dekERPVIS", "materialrequirementplanning", "") != "")
                sqlcmd.AppendFormat(iniManager.ReadIniFile("dekERPVIS", "materialrequirementplanning", ""), start, end, type, item);
            return sqlcmd.ToString();
        }

        //歷史用料統計表細項
        string Getmaterialrequirementplanning_Detail(string item, string start, string end, dekModel model)
        {
            StringBuilder sqlcmd = new StringBuilder();
            if (iniManager.ReadIniFile("dekERPVIS", "materialrequirementplanning", "") != "")
                sqlcmd.AppendFormat(iniManager.ReadIniFile("dekERPVIS", "materialrequirementplanning_Details_" + model, ""), item, start, end);
            return sqlcmd.ToString();
        }

        //供應商催料總表
        string GetSupplierShortage(SupplierShortageType type, string supplier, string supplierName, string start, string end, string itemNo, string Reminder_Date)
        {
            StringBuilder sqlcmd = new StringBuilder();
            if (type == SupplierShortageType.收貨單)
            {
                supplier = supplier != "" ? $"  and  urge_item.fact_no  = '{supplier}' " : "";
                supplierName = supplierName != "" ? $" and D_ADDR = '{supplierName}' " : "";

                if (iniManager.ReadIniFile("dekERPVIS", "SupplierShortage_Receive", "") != "")
                    sqlcmd.AppendFormat(iniManager.ReadIniFile("dekERPVIS", "SupplierShortage_Receive", ""), Reminder_Date, supplier, supplierName);
                return sqlcmd.ToString();
            }
            else
            {
                start = start != "" && end != "" ? $" 	and DATE_FORMAT( D_DATE, \"%Y%m%d\" ) >= {start}     and DATE_FORMAT( D_DATE, \"%Y%m%d\" ) <= {end} " : "";
                itemNo = itemNo != "" ? $"   and item_no = '{itemNo}' " : "";
                supplier = supplier != "" ? $"  and  urge_item.fact_no  = '{supplier}' " : "";
                supplierName = supplierName != "" ? $" and D_ADDR = '{supplierName}' " : "";

                if (iniManager.ReadIniFile("dekERPVIS", "SupplierShortage_Purchase", "") != "")
                    sqlcmd.AppendFormat(iniManager.ReadIniFile("dekERPVIS", "SupplierShortage_Purchase", ""), Reminder_Date, start, itemNo, supplier, supplierName);
                return sqlcmd.ToString();
            }
        }

        //取得內容物
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
            string condition = "";
            for (int i = 0; i < list.Count - 1; i++)
            {
                if (list[i] != "")
                    condition += condition == "" ? $" itemio.LOT_NO='{list[i]}' OR itemio.mkord_no='{list[i]}' " : $" OR itemio.LOT_NO='{list[i]}' OR itemio.mkord_no='{list[i]}' ";
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

            for (int i = 0; i < list.Count - 1; i++)
            {
                if (list[i] != "")
                    condition += condition == "" ? $" itemio.LOT_NO='{list[i]}' OR itemio.mkord_no='{list[i]}' " : $" OR itemio.LOT_NO='{list[i]}' OR itemio.mkord_no='{list[i]}' ";
            }
            condition = condition == "" ? "" : $" and ( {condition} ) ";

            StringBuilder sqlcmd = new StringBuilder();
            if (iniManager.ReadIniFile("dekERPVIS", "pick_list_datatable", "") != "" && condition != "")
                sqlcmd.AppendFormat(iniManager.ReadIniFile("dekERPVIS", "pick_list_datatable", ""), condition);
            return sqlcmd.ToString();

        }


    }
}
