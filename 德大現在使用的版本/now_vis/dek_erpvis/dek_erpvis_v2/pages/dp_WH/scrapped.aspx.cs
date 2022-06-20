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
    public partial class scrapped_New : System.Web.UI.Page
    {
        //-------------------------------------------------參數 OR 引用------------------------------------------------------------
        public string color = "";
        public string path = "";
        public string date_str = "";
        public string date_end = "";
        string dt_str = "";
        string dt_end = "";
        public StringBuilder th = new StringBuilder();
        public StringBuilder tr = new StringBuilder();
        string acc = "";
        public string 總報廢金額 = "";
        public string 最大金額品名規格 = "";
        ERP_Materials PCD = new ERP_Materials();
        DataTable dt_monthtotal = new DataTable();
        ERP_House WHE = new ERP_House();
        string YN = "";
        double total = 0;
        List<string> Record_Note = new List<string>();
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
                if (myclass.user_view_check("scrapped", acc))
                {
                    if (!IsPostBack)
                    {
                        string[] date = 德大機械.德大專用月份(acc).Split(',');
                        dt_str = date[0];
                        dt_end = date[1];
                        date_str = HtmlUtil.changetimeformat(date[0], "-");
                        date_end = HtmlUtil.changetimeformat(date[1], "-");
                        txt_str.Text = HtmlUtil.changetimeformat(date[0], "-");
                        txt_end.Text = HtmlUtil.changetimeformat(date[1], "-");
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
            string[] date = 德大機械.德大專用月份(acc).Split(',');
            date_str = HtmlUtil.changetimeformat(date[0], "-");
            date_end = HtmlUtil.changetimeformat(date[1], "-");
            dt_str = txt_str.Text.Replace("-", "");
            dt_end = txt_end.Text.Replace("-", "");
            MainProcess();
        }

        //廠區切換
        protected void dropdownlist_Factory_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckBoxList_staff.Items.Clear();
            dt_str = txt_str.Text.Replace("-", "");
            dt_end = txt_end.Text.Replace("-", "");
            Set_Checkbox();
        }

        //報廢人員切換
        protected void txt_date_TextChanged(object sender, EventArgs e)
        {
            dt_str = txt_str.Text.Replace("-", "");
            dt_end = txt_end.Text.Replace("-", "");
            CheckBoxList_staff.Items.Clear();
            Set_Checkbox();
            if (CheckBoxList_staff.Items.Count > 0)
                PlaceHolder_hide.Visible = true;
            else
                PlaceHolder_hide.Visible = false;
        }
        //-----------------------------------------------------Function---------------------------------------------------------------------
        //需要執行的程式
        private void MainProcess()
        {
            Set_Checkbox();
            Get_MonthTotal();
            Set_Table();
        }

        //設定報廢人員的核取方塊
        private void Set_Checkbox()
        {
            if (CheckBoxList_staff.Items.Count == 0)
            {
                DataTable dt_staff = PCD.Item_DataTable("Scrapped_Man", dropdownlist_Factory.SelectedItem.Value, dt_str, dt_end);
                if (HtmlUtil.Check_DataTable(dt_staff))
                {
                    HtmlUtil.Set_Element(dt_staff, CheckBoxList_staff, "報廢人", "報廢人", false, "", true);
                    PlaceHolder_hide.Visible = true;
                }
                else
                    PlaceHolder_hide.Visible = false;
            }
        }

        //產生本月報廢總表
        private void Get_MonthTotal()
        {
            dt_monthtotal = WHE.Scrapped(dt_str, dt_end, CheckBoxList_staff, dropdownlist_Factory.SelectedItem.Value);
        }

        //設定表格
        private void Set_Table()
        {
            if (HtmlUtil.Check_DataTable(dt_monthtotal))
            {
                YN = 德大機械.function_yn(acc, "報廢金額");
                DataTable Item = dt_monthtotal.DefaultView.ToTable(true, new string[] { "報廢者", "單據號碼", "單據日期", "品號", "品名規格", "備註" });
                Item.Columns.Add("報廢數量");
                if (YN == "Y")
                {
                    Item.Columns.Add("標準成本");
                    Item.Columns.Add("金額小計");
                    PlaceHolder1.Visible = true;
                }
                else
                    PlaceHolder1.Visible = false;
                Item.Columns.Add("覆驗單號");
                Item.Columns.Add("不良原因");
                Item.Columns.Add("責任歸屬");

                Item.Columns["備註"].SetOrdinal(Item.Columns.Count - 1);

                string column;
                th.Append(HtmlUtil.Set_Table_Title(Item, out column));
                tr.Append(HtmlUtil.Set_Table_Content(Item, column, scrapped_callback));
                總報廢金額 = HtmlUtil.Trans_Thousand(total.ToString("0"));
            }
            else
                HtmlUtil.NoData(out th, out tr);
        }

        //例外處理
        private string scrapped_callback(DataRow row, string field_name)
        {
            string value = "";
            if (field_name == "報廢者")
                Record_Note.Clear();
            else if (field_name == "單據日期")
                value = HtmlUtil.changetimeformat(DataTableUtils.toString(row[field_name]));
            else if (field_name == "品名規格")
            {
                value = DataTableUtils.toString(row[field_name]).Replace("\r\n", "~").Replace("'", "^");
                最大金額品名規格 = 最大金額品名規格 == "" ? DataTableUtils.toString(row[field_name]) : 最大金額品名規格;

            }
            else if (field_name == "報廢數量")
            {
                double count = 0;
                string sqlcmd = $"報廢者='{row["報廢者"]}' and 單據號碼='{row["單據號碼"]}' and 單據日期='{row["單據日期"]}' and 品號='{row["品號"]}'";
                DataRow[] rows = dt_monthtotal.Select(sqlcmd);
                if (rows != null && rows.Length > 0)
                {
                    for (int i = 0; i < rows.Length; i++)
                        count += DataTableUtils.toDouble(DataTableUtils.toString(rows[i]["報廢數量"]));
                    value = count.ToString("0") + DataTableUtils.toString(rows[0]["單位"]);
                    Record_Note = new List<string>(DataTableUtils.toString(row["備註"]).Split(';'));
                }
            }
            else if (field_name == "標準成本")
            {
                string sqlcmd = $"報廢者='{row["報廢者"]}' and 單據號碼='{row["單據號碼"]}' and 單據日期='{row["單據日期"]}' and 品號='{row["品號"]}'";
                DataRow[] rows = dt_monthtotal.Select(sqlcmd);
                if (rows != null && rows.Length > 0)
                    value = HtmlUtil.Trans_Thousand(DataTableUtils.toDouble(DataTableUtils.toString(rows[0]["標準成本"])).ToString("0"));
            }
            else if (field_name == "金額小計")
            {
                double count = 0;
                string sqlcmd = $"報廢者='{row["報廢者"]}' and 單據號碼='{row["單據號碼"]}' and 單據日期='{row["單據日期"]}' and 品號='{row["品號"]}'";
                DataRow[] rows = dt_monthtotal.Select(sqlcmd);
                if (rows != null && rows.Length > 0)
                    for (int i = 0; i < rows.Length; i++)
                        count += DataTableUtils.toDouble(DataTableUtils.toString(rows[i]["金額小計"]));
                value = HtmlUtil.Trans_Thousand(count.ToString("0"));
                total += count;
            }
            else if (field_name == "覆驗單號")
            {
                if (Record_Note.Count == 4 || Record_Note.Count == 5)
                {
                    try
                    {
                        value = Record_Note[0].Split(':')[1].Replace("\r\n", "~").Replace("'", "^");
                    }
                    catch
                    {
                        value = " ";
                    }
                }
            }
            else if (field_name == "不良原因")
            {
                if (Record_Note.Count == 4 || Record_Note.Count == 5)
                {
                    try
                    {
                        value = Record_Note[1].Split(':')[1].Replace("\r\n", "~").Replace("'", "^");
                    }
                    catch
                    {
                        value = " ";
                    }
                }
            }
            else if (field_name == "責任歸屬")
            {
                if (Record_Note.Count == 4 || Record_Note.Count == 5)
                {
                    try
                    {
                        value = Record_Note[2].Split(':')[1].Replace("\r\n", "~").Replace("'", "^");
                    }
                    catch
                    {
                        value = " ";
                    }
                }
            }
            else if (field_name == "備註")
            {
                if (Record_Note.Count == 4 || Record_Note.Count == 5)
                {
                    try
                    {
                        value = Record_Note[3].Split(':')[1].Replace("\r\n", "~").Replace("'", "^");
                    }
                    catch
                    {
                        value = " ";
                    }
                }
                else
                {
                    string sqlcmd = $"報廢者='{row["報廢者"]}' and 單據號碼='{row["單據號碼"]}' and 單據日期='{row["單據日期"]}' and 品號='{row["品號"]}'";
                    DataRow[] rows = dt_monthtotal.Select(sqlcmd);
                    if (rows != null && rows.Length > 0)
                        value = DataTableUtils.toString(rows[0]["備註"]).Replace("\r\n", "~").Replace("'", "^");
                }
            }

            return $"<td>{value}</td>";
        }
    }
}