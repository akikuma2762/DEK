using dek_erpvis_v2.cls;
using dekERP_dll.dekErp;
using Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace dek_erpvis_v2.pages.dp_SD
{
    public partial class recordsofchangetheorder_details_New : System.Web.UI.Page
    {//-------------------------------------------------參數 OR 引用------------------------------------------------------------
        public string color = "";
        public StringBuilder th = new StringBuilder();
        public StringBuilder tr = new StringBuilder();
        public string cust_name = "";
        public string dt_st = "";
        public string dt_ed = "";
        string acc = "";
        string Factory = "";
        string genre = "";
        myclass myclass = new myclass();
        ERP_Sales SLS = new ERP_Sales();
        DataTable dt_monthtotal = new DataTable();
        double total = 0;
        public int HTML_交期變更總次數 = 0;
        public int HTML_數量變更總次數 = 0;
        public int HTML_品號變更總次數 = 0;
        public int HTML_客戶單號變更總次數 = 0;
        public int HTML_單價變更次數 = 0;
        public int HTML_外幣單價變更次數 = 0;

        //----------------------------------------------------Event----------------------------------------------------------------------
        //載入事件
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                color = HtmlUtil.change_color(acc);
                if (acc != "")
                {
                    if (!IsPostBack)
                        MainProcess();
                }
                else
                    Response.Redirect("recordsofchangetheorder.aspx", false);
            }
            else
                Response.Redirect("recordsofchangetheorder.aspx", false);
        }
        //-----------------------------------------------------Function---------------------------------------------------------------------
        //需要執行的程式
        private void MainProcess()
        {
            GetCondi();
            Get_MonthTotal();
            Set_Table();
        }

        //設定數值
        private void GetCondi()
        {
            Response.Buffer = false;
            if (Request.QueryString["key"] != null)
            {
                Dictionary<string, string> keyValues = HtmlUtil.Return_dictionary(Request.QueryString["key"]);
                cust_name = HtmlUtil.Search_Dictionary(keyValues, "cust");
                dt_st = HtmlUtil.Search_Dictionary(keyValues, "date_str");
                dt_ed = HtmlUtil.Search_Dictionary(keyValues, "date_end");
                Factory = HtmlUtil.Search_Dictionary(keyValues, "type");
                genre = HtmlUtil.Search_Dictionary(keyValues, "genre");
                //儲存cookie
                Response.Cookies.Add(HtmlUtil.Save_Cookies("recordsofchangetheorder", cust_name));
            }
            else
                Response.Redirect("recordsofchangetheorder.aspx", false);
        }

        //取得本月變更數
        private void Get_MonthTotal()
        {
            switch(genre)
            {
                case "":
                    dt_monthtotal = SLS.Recordsofchangetheorder_Details(dt_st, dt_ed, Factory);
                    break;
                case "1":
                    dt_monthtotal = SLS.Change_Date_To_Now(dt_st, dt_ed, Factory);
                    break;
                case "2":
                    dt_monthtotal = SLS.Change_Date_To_Other(dt_st, dt_ed, Factory);
                    break;
                case "3":
                    dt_monthtotal = SLS.Change_Now_To_Now(dt_st, dt_ed, Factory);
                    break;
            }
        }

        //設定表格
        private void Set_Table()
        {
            DataTable dt = HtmlUtil.Get_InformationDataTable(dt_monthtotal, DataTableUtils.toString(dt_monthtotal.Columns[0]), cust_name);
            if (HtmlUtil.Check_DataTable(dt))
            {
                DataView db_MP = dt.DefaultView;
                 dt = db_MP.ToTable(true, "客戶名稱", "訂單號碼", "SN", "變更日期", "變更欄位", "備註", "變更前內容", "變更後內容", "CCS", "機號", "客戶機種");

                string columns = "";
                th.Append("<th>客戶名稱</th>");
                th.Append("<th>訂單號碼</th>");
                th.Append("<th>項次</th>");
                th.Append("<th>變更日期</th>");
                th.Append("<th>強迫結案次數</th>");
                th.Append("<th>品號變更次數</th>");
                th.Append("<th>數量變更次數</th>");
                th.Append("<th>交期變更次數</th>");
                th.Append("<th>單價變更次數</th>");
                th.Append("<th>外幣單價變更次數</th>");
                th.Append("<th>小計</th>");
                th.Append("<th>德大變更</th>");
                th.Append("<th>客戶變更</th>");
                th.Append("<th>船期變更</th>");
                th.Append("<th>備註</th>");
                th.Append("<th>變更前內容</th>");
                th.Append("<th>變更後內容</th>");
                th.Append("<th>CCS</th>");
                th.Append("<th>機號</th>");
                th.Append("<th>客戶品號</th>");
                columns = "客戶名稱,訂單號碼,SN,變更日期,強迫結案次數,品號變更次數,數量變更次數,交期變更次數,單價變更次數,外幣單價變更次數,小計,德大變更,客戶變更,船期變更,備註,變更前內容,變更後內容,CCS,機號,客戶機種,";
                tr.Append(HtmlUtil.Set_Table_Content(dt, columns, recordsofchangetheorder_details_Callback));
            }
            else
                HtmlUtil.NoData(out th, out tr);
        }

        //例外處理
        private string recordsofchangetheorder_details_Callback(DataRow row, string field_name)
        {
            string value = "";
            if (field_name == "變更日期")
                value = HtmlUtil.changetimeformat(DataTableUtils.toString(row[field_name]));
            else if (field_name == "強迫結案次數" || field_name == "品號變更次數" || field_name == "數量變更次數" || field_name == "交期變更次數"|| field_name == "單價變更次數"|| field_name == "外幣單價變更次數")
            {
                if (field_name == "強迫結案次數")
                    total = 0;

                string sqlcmd = "";
                if (DataTableUtils.toString(row["變更後內容"]) != "")
                    sqlcmd = $"訂單號碼='{DataTableUtils.toString(row["訂單號碼"])}' and SN='{DataTableUtils.toString(row["SN"])}' and  變更日期='{DataTableUtils.toString(row["變更日期"])}' and {sub(field_name)} and 備註='{DataTableUtils.toString(row["備註"])}' and 變更前內容='{DataTableUtils.toString(row["變更前內容"])}' and 變更後內容='{DataTableUtils.toString(row["變更後內容"])}'";
                else
                    sqlcmd = $"訂單號碼='{DataTableUtils.toString(row["訂單號碼"])}' and SN='{DataTableUtils.toString(row["SN"])}' and  變更日期='{DataTableUtils.toString(row["變更日期"])}' and {sub(field_name)} ";

                DataRow[] rows = dt_monthtotal.Select(sqlcmd);

                value = "" + rows.Length;
                total += rows.Length;
                add(field_name, rows.Length);
            }

            else if (field_name == "小計")
                value = "" + total;
            else if (field_name == "德大變更")
            {
                if (DataTableUtils.toString(row["變更後內容"]) != "")
                {
                    string sqlcmd = $"訂單號碼='{DataTableUtils.toString(row["訂單號碼"])}' and SN='{DataTableUtils.toString(row["SN"])}' and  變更日期='{DataTableUtils.toString(row["變更日期"])}' and 變更日期='{DataTableUtils.toString(row["變更日期"])}'  and 變更前內容='{DataTableUtils.toString(row["變更前內容"])}' and 變更後內容='{DataTableUtils.toString(row["變更後內容"])}' and 備註 like '%德大變更%'";
                    DataRow[] rows = dt_monthtotal.Select(sqlcmd);
                    value = "" + rows.Length;
                }
                else
                {
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDetaEip);
                    string sqlcmd = $"select * from CordSub_chg_cnt where TRN_NO = '{DataTableUtils.toString(row["訂單號碼"])}'";
                    DataTable dt = DataTableUtils.GetDataTable(sqlcmd);

                    if (DataTableUtils.toInt(DataTableUtils.toString(dt.Rows[0]["德大變更_cnt"])) > DataTableUtils.toInt(DataTableUtils.toString(dt.Rows[0]["客戶變更_cnt"])))
                        value = "" + total;
                    else
                        value = "0";
                }

            }
            else if (field_name == "船期變更")
            {
                string sqlcmd = $"訂單號碼='{DataTableUtils.toString(row["訂單號碼"])}' and SN='{DataTableUtils.toString(row["SN"])}' and  變更日期='{DataTableUtils.toString(row["變更日期"])}' and 變更日期='{DataTableUtils.toString(row["變更日期"])}'  and 變更前內容='{DataTableUtils.toString(row["變更前內容"])}' and 變更後內容='{DataTableUtils.toString(row["變更後內容"])}' and 備註 like '%船期變更%'";
                DataRow[] rows = dt_monthtotal.Select(sqlcmd);
                value = "" + rows.Length;
            }
            else if (field_name == "客戶變更")
            {
                if (DataTableUtils.toString(row["變更後內容"]) != "")
                {
                    string sqlcmd = $"訂單號碼='{DataTableUtils.toString(row["訂單號碼"])}' and SN='{DataTableUtils.toString(row["SN"])}' and  變更日期='{DataTableUtils.toString(row["變更日期"])}' and 變更欄位='{DataTableUtils.toString(row["變更欄位"])}'  and 變更前內容='{DataTableUtils.toString(row["變更前內容"])}' and 變更後內容='{DataTableUtils.toString(row["變更後內容"])}' and 備註 like '%客戶變更%'";
                    DataRow[] rows = dt_monthtotal.Select(sqlcmd);
                    value = "" + rows.Length;
                }
                else
                {
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDetaEip);
                    string sqlcmd = $"select * from CordSub_chg_cnt where TRN_NO = '{DataTableUtils.toString(row["訂單號碼"])}'";
                    DataTable dt = DataTableUtils.GetDataTable(sqlcmd);

                    if (DataTableUtils.toInt(DataTableUtils.toString(dt.Rows[0]["德大變更_cnt"])) <= DataTableUtils.toInt(DataTableUtils.toString(dt.Rows[0]["客戶變更_cnt"])))
                        value = "" + total;
                    else
                        value = "0";
                }
            }

            else if (field_name == "變更前內容" || field_name == "變更後內容" || field_name == "備註")
            {
                string sqlcmd = "";
                if (DataTableUtils.toString(row["變更後內容"]) != "")
                    sqlcmd = $"訂單號碼='{DataTableUtils.toString(row["訂單號碼"])}' and SN='{DataTableUtils.toString(row["SN"])}' and  變更日期='{DataTableUtils.toString(row["變更日期"])}' and 變更欄位='{DataTableUtils.toString(row["變更欄位"])}' and 備註='{DataTableUtils.toString(row["備註"])}' and 變更前內容='{DataTableUtils.toString(row["變更前內容"])}' and 變更後內容='{DataTableUtils.toString(row["變更後內容"])}'";
                else
                    sqlcmd = $"訂單號碼='{DataTableUtils.toString(row["訂單號碼"])}' and SN='{DataTableUtils.toString(row["SN"])}' and  變更日期='{DataTableUtils.toString(row["變更日期"])}' and 變更欄位='{DataTableUtils.toString(row["變更欄位"])}' ";

                DataRow[] rows = dt_monthtotal.Select(sqlcmd);
                if (rows != null && rows.Length > 0)
                {
                    if (field_name == "備註")
                        value = DataTableUtils.toString(rows[0][field_name]) + " ";
                    else if (DataTableUtils.toString(row["變更欄位"]) == "預交日")
                        value = HtmlUtil.changetimeformat(DataTableUtils.toString(rows[0][field_name]));
                    else if (DataTableUtils.toString(row["變更欄位"]) == "數量")
                        value = DataTableUtils.toString(rows[0][field_name]).Split('.')[0];
                    else
                        value = DataTableUtils.toString(rows[0][field_name]);
                }
                else
                    value = " ";
            }

            if (value == "")
                return "";
            else
                return $"<td>{value.Replace("\r\n", "")}</td>";
        }

        //名稱轉換
        private string sub(string field)
        {
            switch (field)
            {
                case "交期變更次數":
                    return "(變更欄位='預交日' OR 變更欄位 like '%D_DATE%')";
                case "數量變更次數":
                    return "(變更欄位='數量' OR 變更欄位 like '%qty%')";
                case "品號變更次數":
                    return "(變更欄位='品號' OR 變更欄位 like '%品號%')";
                case "強迫結案次數":
                    return "變更欄位='強迫結案'";
                case "單價變更次數":
                    return "變更欄位='單價'";
                case "外幣單價變更次數":
                    return "變更欄位='外幣單價'";
            }
            return "";
        }

        //依據類型新增數量
        private void add(string field, int qty)
        {
            switch (field)
            {
                case "交期變更次數":
                    HTML_交期變更總次數 += qty;
                    break;
                case "數量變更次數":
                    HTML_數量變更總次數 += qty;
                    break;
                case "品號變更次數":
                    HTML_品號變更總次數 += qty;
                    break;
                case "強迫結案次數":
                    HTML_客戶單號變更總次數 += qty;
                    break;
                case "單價變更次數":
                    HTML_單價變更次數 += qty;
                    break;
                case "外幣單價變更次數":
                    HTML_外幣單價變更次數 += qty;
                    break;
            }
        }
    }
}