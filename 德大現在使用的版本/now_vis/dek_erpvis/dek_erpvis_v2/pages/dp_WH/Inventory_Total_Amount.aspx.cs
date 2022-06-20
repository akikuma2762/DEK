using dek_erpvis_v2.cls;
using Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using dekERP_dll.dekErp;

namespace dek_erpvis_v2.pages.dp_WH
{
    public partial class Inventory_Total_Amount_New : System.Web.UI.Page
    {
        //-------------------------------------------------參數 OR 引用------------------------------------------------------------
        public string color = "";
        public string path = "";
        public StringBuilder th = new StringBuilder();
        public StringBuilder tr = new StringBuilder();
        string acc = "";
        string YN = "";
        DataTable dt_monthtotal = new DataTable();
        ERP_House WHE = new ERP_House();
        ERP_Materials PCD = new ERP_Materials();
        List<string> avoid_again = new List<string>();
        //----------------------------------------------------Event----------------------------------------------------------------------
        //載入事件
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                path = 德大機械.get_title_web_path("WHE");
                color = HtmlUtil.change_color(acc);
                if (myclass.user_view_check("Inventory_Total_Amount", acc))
                {
                    if (!IsPostBack)
                    {
                        if (WebUtils.GetAppSettings("Show_dek") == "1" || HtmlUtil.Search_acc_Column(acc, "Can_dek") == "Y")
                            PlaceHolder_hide.Visible = true;
                        else
                            PlaceHolder_hide.Visible = false;

                        MainProcess();
                    }

                }
                else
                    Response.Write("<script>alert('您無此權限!');location.href='../index.aspx';</script>");
            }
            else
                Response.Redirect(myclass.logout_url);
        }

        //查詢事件
        protected void button_select_Click(object sender, EventArgs e)
        {
            MainProcess();
        }

        //廠區切換
        protected void dropdownlist_Factory_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckBoxList_spaces.Items.Clear();
            CheckBoxList_type.Items.Clear();
            Set_Checkbox();
        }

        //-----------------------------------------------------Function---------------------------------------------------------------------
        //需要執行的程式
        private void MainProcess()
        {
            Set_Checkbox();
            Get_MonthTotal();
            Set_Table();
        }

        //設定倉位的核取方塊
        private void Set_Checkbox()
        {
            if (CheckBoxList_spaces.Items.Count == 0)
            {
                DataTable dt_warehouse = PCD.Item_DataTable("InactiveInventory_House", dropdownlist_Factory.SelectedItem.Value);
                if (HtmlUtil.Check_DataTable(dt_warehouse))
                    HtmlUtil.Set_Element(dt_warehouse, CheckBoxList_spaces, "儲位", "", true, "總倉");
            }
            if (CheckBoxList_type.Items.Count == 0)
            {
                DataTable dt_type = PCD.Item_DataTable("InactiveInventory_Class", dropdownlist_Factory.SelectedItem.Value);
                if (HtmlUtil.Check_DataTable(dt_type))
                    HtmlUtil.Set_Element(dt_type, CheckBoxList_type, "C_NAME", "CLASS", true, "零件");
            }
        }

        //取得所有的庫存明細
        private void Get_MonthTotal()
        {
            dt_monthtotal = WHE.Inventory_Total_Amount(CheckBoxList_spaces, CheckBoxList_type, dropdownlist_Factory.SelectedItem.Value);
        }

        //設定表格
        private void Set_Table()
        {
            if (HtmlUtil.Check_DataTable(dt_monthtotal))
            {
                YN = 德大機械.function_yn(acc, "呆料金額");
                if (YN != "Y")
                {
                    dt_monthtotal.Columns.Remove("庫存金額");
                    dt_monthtotal.Columns.Remove("總庫存金額");
                }
                string column = "";
                th.Append(HtmlUtil.Set_Table_Title(dt_monthtotal, out column));
                tr.Append(HtmlUtil.Set_Table_Content(dt_monthtotal, column, Inventory_Total_Amount_New_callback));
            }
            else
                HtmlUtil.NoData(out th, out tr);
        }

        //例外處理
        private string Inventory_Total_Amount_New_callback(DataRow row, string field_name)
        {
            if (avoid_again.IndexOf(DataTableUtils.toString(row["品號"]).Trim()) == -1)
            {
                string value = "";
                if (field_name == "庫存數量" || field_name == "庫存金額" || field_name == "總庫存金額")
                {
                    value = HtmlUtil.Trans_Thousand(DataTableUtils.toDouble(DataTableUtils.toString(row[field_name])).ToString("0"));
                    if (YN == "N" && field_name == "庫存數量")
                        avoid_again.Add(DataTableUtils.toString(row["品號"]).Trim());
                    else if (YN == "Y" && field_name == "總庫存金額")
                        avoid_again.Add(DataTableUtils.toString(row["品號"]).Trim());
                }
                else if (field_name == "品名規格")
                    value = DataTableUtils.toString(row[field_name]).Replace("\r\n", "~").Replace("'", "^");

                return value != "" ? $"<td>{value}</td>" : "";
            }
            else
                return "1";
        }
    }
}