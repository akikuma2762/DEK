using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using dek_erpvis_v2.cls;
using System.Data;
using Support;
using System.Text;
using dekERP_dll;
using dekERP_dll.dekErp;

namespace dek_erpvis_v2.pages.dp_PD
{
    public partial class pick_list_New : System.Web.UI.Page
    {
        //-------------------------------------------------參數 OR 引用------------------------------------------------------------
        public StringBuilder th = new StringBuilder();
        public StringBuilder tr = new StringBuilder();
        public string color = "";
        public string all_div = "";
        public string all_js = "";
        DataTable dt_monthtotal = new DataTable();
        DataTable dt_monthtotal_title = new DataTable();
        List<string> avoid_again = new List<string>();
        ERP_Materials PCD = new ERP_Materials();
        string Now_Number = "";

        //----------------------------------------------------Event----------------------------------------------------------------------
        //載入事件
        protected void Page_Load(object sender, EventArgs e)
        {
            string acc = "";
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                color = HtmlUtil.change_color(acc);
                if (myclass.user_view_check("pick_list", acc))
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
        protected void Unnamed_ServerClick(object sender, EventArgs e)
        {
            MainProcess();
        }
        //----------------------------------------------------Function-------------------------------------------------------------------
        //需要執行副程式
        private void MainProcess()
        {
            Get_MonthTotal();
            Set_IndividualTable();
            Set_Table();
        }

        //取得輸入的領料總表
        private void Get_MonthTotal()
        {
            //產生領料表(全部)
            dt_monthtotal = PCD.pick_list_datatable(TextBox_Record.Text, dropdownlist_Factory.SelectedItem.Value);
            //產生領料表頭(全部)
            dt_monthtotal_title = PCD.pick_list_title(TextBox_Record.Text, dropdownlist_Factory.SelectedItem.Value);
        }

        //取得各別的領料表
        private void Set_IndividualTable()
        {
            List<string> Number_List = new List<string>(TextBox_Record.Text.Split('#'));
            for (int i = 0; i < Number_List.Count - 1; i++)
            {
                if (Number_List[i] != "")
                {
                    Now_Number = Number_List[i];

                    all_div += $"<div id=\"div_{Number_List[i]}\"></div>";
                    Set_Table();
                    all_js += $"create_tablecode_noshdrow_havesubtitle('div_{Number_List[i]}', '{Number_List[i]}領料單', '{Number_List[i]}', '{th}', '{tr}','{Set_Title(Now_Number)}');\n" +
                             $"set_Table('#{Number_List[i]}');\n" +
                             $"loadpage('', '');\n";

                    Now_Number = "";
                    th = new StringBuilder();
                    tr = new StringBuilder();
                    avoid_again.Clear();
                }

            }
        }

        //各別領料單的表頭
        private string Set_Title(string Number)
        {
            string return_string = "";
            if (HtmlUtil.Check_DataTable(dt_monthtotal_title))
            {
                string sqlcmd = $"製造批號='{Number}' OR 製令單號='{Number}' and 用途說明 <> '補領'";
                DataRow[] rows = dt_monthtotal_title.Select(sqlcmd);
                DataTable dt_clone = dt_monthtotal_title.Clone();
                if (rows != null && rows.Length > 0)
                    dt_clone.ImportRow(rows[0]);
                if (HtmlUtil.Check_DataTable(dt_clone))
                {
                    return_string = "<div class=\"col-md-12 col-xs-12 col-sm-12\">";
                    for (int i = 0; i < dt_clone.Columns.Count; i++)
                    {
                        return_string += "<div class=\"col-md-4 col-xs-6 col-sm-6\">";
                        return_string += $" {dt_clone.Columns[i]}:{dt_clone.Rows[0][dt_clone.Columns[i]]}";
                        return_string += "</div>";
                    }
                    return_string += "</div>";
                }
            }
            return return_string;
        }

        //總領料單 || 各別領料單
        private void Set_Table()
        {
            if (HtmlUtil.Check_DataTable(dt_monthtotal))
            {
                DataTable Item = dt_monthtotal.DefaultView.ToTable(true, new string[] { "品號", "品名規格", "倉位", "單位" });
                Item.Columns.Add("現有庫存");
                Item.Columns.Add("申請數量");
                Item.Columns.Add("領料數量");
                Item.Columns.Add("不足數量");
                Item.Columns.Add("狀況");
                string column = "";
                th.Append(HtmlUtil.Set_Table_Title(Item, out column));
                tr.Append(HtmlUtil.Set_Table_Content(Item, column, pick_list_callback));
            }
            else
                HtmlUtil.NoData(out th, out tr);
        }

        //例外處理
        private string pick_list_callback(DataRow row, string field_name)
        {
            if (avoid_again.IndexOf(DataTableUtils.toString(row["品號"]).Trim()) == -1)
            {
                string value = "";
                string sqlcmd = Now_Number == "" ? $"品號='{row["品號"]}'" : $"品號='{row["品號"]}' and 刀庫或製令='{Now_Number}'";
                DataRow[] rows = dt_monthtotal.Select(sqlcmd);
                double count = 0;

                if (field_name == "現有庫存")
                {
                    //找最後一個
                    if (rows != null && rows.Length > 0)
                    {
                        try
                        {
                            value = DataTableUtils.toDouble(DataTableUtils.toString(rows[rows.Length - 1][field_name])).ToString("0");
                        }
                        catch
                        {
                            value = "0";
                        }
                    }
                }
                else if (field_name == "申請數量" || field_name == "領料數量" || field_name == "不足數量")
                {
                    if (rows != null && rows.Length > 0)
                    {
                        for (int i = 0; i < rows.Length; i++)
                            count += DataTableUtils.toDouble(DataTableUtils.toString(rows[i][field_name]));
                    }
                    value = count.ToString("0");
                }

                else if (field_name == "狀況")
                {
                    bool ok = true;
                    if (rows != null && rows.Length > 0)
                    {
                        for (int i = 0; i < rows.Length; i++)
                        {
                            if (DataTableUtils.toString(rows[i]["狀況"]) != "結案")
                                ok = false;
                        }
                    }
                    value = ok ? "結案" : "未結";
                    avoid_again.Add(DataTableUtils.toString(row["品號"]).Trim());
                }
                return value == "" ? "" : $"<td>{value}</td>";
            }
            else
                return "1";
        }
    }
}