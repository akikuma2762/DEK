using dek_erpvis_v2.cls;
using Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using dekERP_dll.dekErp;
using System.Data;

namespace dek_erpvis_v2.pages.dp_WH
{
    public partial class InactiveInventory_New : System.Web.UI.Page
    {
        //-------------------------------------------------參數 OR 引用------------------------------------------------------------
        string acc = "";
        string YN = "";
        double total = 0;
        double use_total = 0;
        double cost = 0;
        public string color = "";
        public string path = "";
        public StringBuilder th = new StringBuilder();
        public StringBuilder tr = new StringBuilder();
        List<string> avoid_again = new List<string>();
        List<string> space = new List<string>();
        public List<string> persent = new List<string>();
        List<double> record_persent = new List<double>();
        ERP_Materials PCD = new ERP_Materials();
        ERP_House WHE = new ERP_House();
        DataTable dt_monthtotal = new DataTable();

        //----------------------------------------------------Event----------------------------------------------------------------------
        //載入事件
        protected void Page_Load(object sender, EventArgs e)
        {
            //先產生，避免後續出問題
            persent.Add("0");
            persent.Add("0");
            persent.Add("0");
            persent.Add("0");

            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                path = 德大機械.get_title_web_path("WHE");
                color = HtmlUtil.change_color(acc);
                if (myclass.user_view_check("InactiveInventory", acc))
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
            CheckBoxList_itemtype.Items.Clear();
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

        //設定CheckboxList內容
        private void Set_Checkbox()
        {
            ListItem listItem = new ListItem();
            //設定儲位
            if (CheckBoxList_spaces.Items.Count == 0)
            {
                DataTable dt_space = PCD.Item_DataTable("InactiveInventory_House", dropdownlist_Factory.SelectedItem.Value);
                if (HtmlUtil.Check_DataTable(dt_space))
                    HtmlUtil.Set_Element(dt_space, CheckBoxList_spaces, "儲位", "儲位", true, "總倉");
            }

            //設定物料類別
            if (CheckBoxList_itemtype.Items.Count == 0)
            {
                DataTable dt_class = PCD.Item_DataTable("InactiveInventory_Class", dropdownlist_Factory.SelectedItem.Value);
                if (HtmlUtil.Check_DataTable(dt_class))
                    HtmlUtil.Set_Element(dt_class, CheckBoxList_itemtype, "C_NAME", "CLASS", true, "零件");
            }
        }

        //取得呆滯物料總表
        private void Get_MonthTotal()
        {
            dt_monthtotal = WHE.InactiveInventory(CheckBoxList_itemtype, CheckBoxList_spaces, DateTime.Now.AddDays(-DataTableUtils.toInt(TextBoxdayval.Text)).ToString("yyyyMMdd"), dropdownlist_Factory.SelectedItem.Value);
        }

        //設定表格
        private void Set_Table()
        {

            YN = 德大機械.function_yn(acc, "呆料金額");
            if (HtmlUtil.Check_DataTable(dt_monthtotal))
            {
                DataTable Item = dt_monthtotal.DefaultView.ToTable(true, new string[] { "最後領料日", "品號", "品名規格", "倉位", "類別" });
                for (int i = 1; i < CheckBoxList_spaces.Items.Count; i++)
                {
                    if (CheckBoxList_spaces.Items[i].Selected)
                    {
                        Item.Columns.Add(CheckBoxList_spaces.Items[i].Text);
                        space.Add(CheckBoxList_spaces.Items[i].Text);
                    }
                }
                Item.Columns.Add("剩餘總庫存");
                Item.Columns.Add("領用數量");
                Item.Columns.Add("銷貨數量");
                Item.Columns.Add("期初庫存");
                Item.Columns.Add("領用比例");
                if (YN == "Y")
                {
                    Item.Columns.Add("標準成本");
                    Item.Columns.Add("金額小計");
                }
                string column = "";

                record_persent.Add(0);
                record_persent.Add(0);
                record_persent.Add(0);
                record_persent.Add(0);

                th.Append(HtmlUtil.Set_Table_Title(Item, out column));
                tr.Append(HtmlUtil.Set_Table_Content(Item, column, InactiveInventory_callback));

                persent[0] = HtmlUtil.Trans_Thousand(record_persent[0].ToString("0"));
                persent[1] = HtmlUtil.Trans_Thousand(record_persent[1].ToString("0"));
                persent[2] = HtmlUtil.Trans_Thousand(record_persent[2].ToString("0"));
                persent[3] = HtmlUtil.Trans_Thousand(record_persent[3].ToString("0"));
            }
            else
                HtmlUtil.NoData(out th, out tr);
        }

        //例外處理
        private string InactiveInventory_callback(DataRow row, string field_name)
        {
            if (avoid_again.IndexOf(DataTableUtils.toString(row["品號"]).Trim()) == -1)
            {
                string value = "";
                if (field_name == "最後領料日")
                    value = DataTableUtils.toString(row[field_name]) == "---" ? "---" : HtmlUtil.changetimeformat(DataTableUtils.toString(row[field_name]));
                else if (space.IndexOf(field_name) != -1)
                {
                    string count = "0";
                    string sqlcmd = $"最後領料日='{row["最後領料日"]}' and 品號='{row["品號"]}' and 類別='{row["類別"]}' and 儲位='{field_name}'";
                    DataRow[] rows = dt_monthtotal.Select(sqlcmd);
                    if (rows != null && rows.Length > 0)
                        count = DataTableUtils.toDouble(DataTableUtils.toString(rows[0]["剩餘總庫存"])).ToString("0");
                    value = HtmlUtil.Trans_Thousand(count);
                    total += DataTableUtils.toDouble(count);
                }
                else if (field_name == "剩餘總庫存")
                    value = HtmlUtil.Trans_Thousand(total);
                else if (field_name == "領用數量" || field_name == "銷貨數量")
                {
                    string count = "0";
                    string sqlcmd = $"最後領料日='{row["最後領料日"]}' and 品號='{row["品號"]}'  and 類別='{row["類別"]}' ";
                    DataRow[] rows = dt_monthtotal.Select(sqlcmd);
                    if (rows != null && rows.Length > 0)
                    {
                        for (int i = 0; i < rows.Length; i++)
                            count += DataTableUtils.toDouble(DataTableUtils.toString(rows[i][field_name])).ToString("0");
                    }
                    value = HtmlUtil.Trans_Thousand(count);
                    use_total += DataTableUtils.toDouble(count);

                }
                else if (field_name == "期初庫存")
                    value = (use_total + total).ToString("0");
                else if (field_name == "領用比例")
                {

                    string sqlcmd = $"最後領料日='{row["最後領料日"]}' and 品號='{row["品號"]}'  and 類別='{row["類別"]}' ";
                    DataRow[] rows = dt_monthtotal.Select(sqlcmd);
                    if (rows != null && rows.Length > 0)
                        cost = DataTableUtils.toDouble(DataTableUtils.toString(rows[0]["標準成本"]).Split('.')[0]);

                    double use_present = (use_total * 100 / (use_total + total));
                    value = $"{use_present:0}%";
                    if (YN == "Y")
                    {
                        div_present.Visible = true;

                        if (DataTableUtils.toInt(use_present.ToString("0")) <= 25)
                            record_persent[0] += total * cost;
                        else if (DataTableUtils.toInt(use_present.ToString("0")) > 25 && DataTableUtils.toInt(use_present.ToString("0")) <= 50)
                            record_persent[1] += total * cost;
                        else if (DataTableUtils.toInt(use_present.ToString("0")) > 50 && DataTableUtils.toInt(use_present.ToString("0")) <= 75)
                            record_persent[2] += total *cost;
                        else if (DataTableUtils.toInt(use_present.ToString("0")) > 75 && DataTableUtils.toInt(use_present.ToString("0")) <= 100)
                            record_persent[3] += total * cost;
                    }
                    else
                        div_present.Visible = false;
                    if (YN == "N")
                    {
                        total = 0;
                        use_total = 0;
                        avoid_again.Add(DataTableUtils.toString(row["品號"]).Trim());
                    }
                }
                else if (field_name == "標準成本")
                    value = HtmlUtil.Trans_Thousand(cost.ToString("0"));
                else if (field_name == "金額小計")
                {
                    value = HtmlUtil.Trans_Thousand((cost * total).ToString("0"));
                    if (YN == "Y")
                    {
                        total = 0;
                        use_total = 0;
                        cost = 0;
                        avoid_again.Add(DataTableUtils.toString(row["品號"]).Trim());
                    }
                }

                return value == "" ? "" : $"<td>{value}</td>";
            }
            else
                return "1";
        }
    }
}