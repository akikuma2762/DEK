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
    public partial class Orders_DayTotal_Details : System.Web.UI.Page
    {
        public string color = "";
        public string date_st = "";
        public string date_ed = "";
        public string workday = "";
        public string LineName = "";
        string Factory = "";
        string custom = "";
        string date = "";
        string acc = "";
        public StringBuilder tr = new StringBuilder();
        public StringBuilder th = new StringBuilder();
        myclass myclass = new myclass();
        ERP_Sales SLS = new ERP_Sales();
        DataTable dt_monthtotal = new DataTable();
        Dictionary<string, string> keyValues = new Dictionary<string, string>();
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
                    Response.Redirect("Orders_ProductTotal.aspx", false);
            }
            else
                Response.Redirect("Orders_ProductTotal.aspx", false);
        }
        //-----------------------------------------------------Function---------------------------------------------------------------------
        //需要執行的程式
        private void MainProcess()
        {
            GetCondi();
            Get_MonthTotal();
            Set_Table();
            workday = HtmlUtil.changetimeformat(workday);
        }

        //設定數值
        private void GetCondi()
        {
            Response.Buffer = false;
            if (Request.QueryString["key"] != null)
            {
                keyValues = HtmlUtil.Return_dictionary(Request.QueryString["key"]);
                if (keyValues.Count == 5)
                {
                    date_st = HtmlUtil.Search_Dictionary(keyValues, "date_str");
                    date_ed = HtmlUtil.Search_Dictionary(keyValues, "date_end");
                    workday = HtmlUtil.Search_Dictionary(keyValues, "workday");
                    LineName = HtmlUtil.Search_Dictionary(keyValues, "Line");
                    Factory = HtmlUtil.Search_Dictionary(keyValues, "type");
                }
                else if (keyValues.Count == 6)
                {
                    date_st = HtmlUtil.Search_Dictionary(keyValues, "date_str");
                    date_ed = HtmlUtil.Search_Dictionary(keyValues, "date_end");
                    custom = HtmlUtil.Search_Dictionary(keyValues, "custom");
                    date = HtmlUtil.Search_Dictionary(keyValues, "Date");
                    Factory = HtmlUtil.Search_Dictionary(keyValues, "type");
                }

                //儲存cookie
                Response.Cookies.Add(HtmlUtil.Save_Cookies("Orders_DayTotal", HtmlUtil.changetimeformat(workday)));
            }
            else
                Response.Redirect("Orders_ProductTotal.aspx", false);
        }

        //取得本月出貨數
        private void Get_MonthTotal()
        {
            dt_monthtotal = SLS.Orders_StartDay(date_st, date_ed, Factory);

            if (HtmlUtil.Check_DataTable(dt_monthtotal))
            {
                dt_monthtotal = myclass.Add_LINE_GROUP(dt_monthtotal).ToTable();
                dt_monthtotal.Columns.Remove("產線代號");
                
                dt_monthtotal.Columns["產線群組"].SetOrdinal(0);
            }
        }

        //設定表格
        private void Set_Table()
        {
            DataTable dt = dt_monthtotal;
            if (HtmlUtil.Check_DataTable(dt))
            {
                string sqlcmd = "";
                if(keyValues.Count == 5)
                {
                    sqlcmd = LineName == "所有" ? "" : $" and  產線群組='{LineName}'";
                    dt = HtmlUtil.Get_InformationDataTable(dt, $" 預計開工日='{workday}' {sqlcmd}");
                    dt.Columns.Remove("計算月份");
                }
                else
                {
                    custom = custom == "" ? "" : $"客戶簡稱='{custom}'";
                    date = date == "" ? "" : $"計算月份='{date}'";
                    if (custom != "" && date != "")
                        sqlcmd = $"{custom} and {date}";
                    else if (custom != "")
                        sqlcmd = custom;
                    else if (date != "")
                        sqlcmd = date;
                    dt = HtmlUtil.Get_InformationDataTable(dt, sqlcmd);
                    dt.Columns.Remove("計算月份");
                }
                string columns = "";
                th.Append(HtmlUtil.Set_Table_Title(dt, out columns));
                tr.Append(HtmlUtil.Set_Table_Content(dt, columns, Orders_ProductTotal_Details_Callback).Replace("\r\n", ""));
            }
            else
                HtmlUtil.NoData(out th, out tr);
        }
        //例外處理
        private string Orders_ProductTotal_Details_Callback(DataRow row, string field_name)
        {
            string value = "";

            if (field_name == "預計開工日")
                value = HtmlUtil.changetimeformat(row[field_name].ToString());
            return value == "" ? "" : $"<td>{value}</td>";
        }
    }
}