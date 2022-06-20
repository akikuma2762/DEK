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

namespace dek_erpvis_v2.pages.dp_SD
{
    public partial class transportrackstatistics_New : System.Web.UI.Page
    {
        //-------------------------------------------------參數 OR 引用------------------------------------------------------------
        string acc = "";
        public string color = "";
        public string path = "";
        public string title = "";
        public string col_data_points_sply = "";
        public string col_data_points_nor = "";
        public StringBuilder th = new StringBuilder();
        public StringBuilder tr = new StringBuilder();
        DataTable dt_monthtotal = new DataTable();
        ERP_Sales SLS = new ERP_Sales();
        List<string> avoid_again = new List<string>();
        List<string> transport_Number = new List<string>();
        double total = 0;
        //----------------------------------------------------Event----------------------------------------------------------------------
        //載入事件
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                path = 德大機械.get_title_web_path("SLS");
                color = HtmlUtil.change_color(acc);
                if (myclass.user_view_check("transportrackstatistics", acc))
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
        //-----------------------------------------------------Function---------------------------------------------------------------------
        //需要執行的程式
        private void MainProcess()
        {
            Get_MonthTotal();
            Set_Chart();
            Set_Table();
        }
        //取得所有未歸還數量
        private void Get_MonthTotal()
        {
            dt_monthtotal = SLS.Transportrackstatistics(acc, dropdownlist_Factory.SelectedItem.Value);
            if (HtmlUtil.Check_DataTable(dt_monthtotal))
                //變更名稱
                foreach (DataRow row in dt_monthtotal.Rows)
                    row["運輸架品號"] = get_trn_name(DataTableUtils.toString(row["運輸架品號"]));
        }
        //設定圖片
        private void Set_Chart()
        {
            if (HtmlUtil.Check_DataTable(dt_monthtotal))
            {
                DataTable dt_normal = HtmlUtil.PrintChart_DataTable(dt_monthtotal, "運輸架品號", "正常數量");
                DataTable dt_abnormal = HtmlUtil.PrintChart_DataTable(dt_monthtotal, "運輸架品號", "異常數量");
                int total = 0;
                int ab_total = 0;
                col_data_points_sply = HtmlUtil.Set_Chart(dt_normal, "運輸架品號", "正常數量", "", out total);
                col_data_points_nor = HtmlUtil.Set_Chart(dt_abnormal, "運輸架品號", "異常數量", "", out ab_total);
            }
        }
        //設定表格
        private void Set_Table()
        {
            if (HtmlUtil.Check_DataTable(dt_monthtotal))
            {
                DataTable transport = dt_monthtotal.DefaultView.ToTable(true, new string[] { "運輸架品號" });
                DataTable Custom = dt_monthtotal.DefaultView.ToTable(true, new string[] { "客戶簡稱" });

                //新增產線
                foreach (DataRow row in transport.Rows)
                {
                    Custom.Columns.Add(DataTableUtils.toString(row["運輸架品號"]));
                    transport_Number.Add(DataTableUtils.toString(row["運輸架品號"]));
                }
                //新增小計
                Custom.Columns.Add("小計");
                string columns = "";
                th.Append(HtmlUtil.Set_Table_Title(Custom, out columns));
                tr.Append(HtmlUtil.Set_Table_Content(Custom, columns, transportrackstatistics_callback));
            }
            else
                 HtmlUtil.NoData(out th, out tr);
        }

        //例外處理
        private string transportrackstatistics_callback(DataRow row, string field_name)
        {
            if (avoid_again.IndexOf(DataTableUtils.toString(row["客戶簡稱"]).Trim()) == -1)
            {
                string value = "";
                //產線的處理
                if (transport_Number.IndexOf(field_name) != -1)
                {
                    if (transport_Number.IndexOf(field_name) == 0)
                        total = 0;

                    string sqlcmd = $"客戶簡稱='{DataTableUtils.toString(row["客戶簡稱"])}' and 運輸架品號='{field_name}'";
                    DataRow[] rows = dt_monthtotal.Select(sqlcmd);
                    if (rows != null && rows.Length > 0)
                    {
                        double count = 0;
                        for (int i = 0; i < rows.Length; i++)
                            count += DataTableUtils.toDouble(DataTableUtils.toString(rows[i]["正常數量"]));

                        value = $" align=\"right\">{count:0}</a></u> ";
                        total += count;
                    }
                    else
                        value = $" align=\"right\">0</a></u> ";
                }
                else if (field_name == "小計")
                {
                    if (total != 0)
                        value = $" align=\"right\"> {HtmlUtil.Trans_Thousand(total.ToString("0"))}</u> ";
                    else
                        value = $" align=\"right\">0";
                    avoid_again.Add(DataTableUtils.toString(row["客戶簡稱"]).Trim());
                }
                return value == "" ? "" : $"<td {value} </td>";
            }
            else
                return "1";
        }

        //名稱轉換
        private string get_trn_name(string trn_code)
        {
            switch (trn_code)
            {
                case "MC4A-00MMA04":
                    trn_code = $"40側掛 {trn_code}";
                    break;
                case "MC4BQ-0MMB04":
                    trn_code = $"40鍊下鎖 {trn_code}";
                    break;
                case "MC5A-00ZZB05":
                    trn_code = $"50側掛 {trn_code}";
                    break;
                case "MR4A-00ZZA08":
                    trn_code = $"40盤 {trn_code}";
                    break;
                case "MR5D-00ZZA06":
                    trn_code = $"50盤 {trn_code}";
                    break;
                case "MR4CM00ZZC07":
                    trn_code = $"{trn_code}";
                    break;
            }
            return trn_code;
        }
    }
}