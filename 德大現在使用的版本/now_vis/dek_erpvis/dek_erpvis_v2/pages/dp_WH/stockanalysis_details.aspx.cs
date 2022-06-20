using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using dek_erpvis_v2.cls;
using dekERP_dll.dekErp;
using Support;

namespace dek_erpvis_v2.pages.dp_WH
{
    public partial class stockanalysis_details_New : System.Web.UI.Page
    {
        public string cust_name = "";
        public string color = "";
        public StringBuilder th = new StringBuilder();
        public StringBuilder tr = new StringBuilder();
        string acc = "";
        string datetime = "";
        string Factory = "";
        DataTable dt_monthtotal = new DataTable();
        ERP_House WHE = new ERP_House();
        myclass myclass = new myclass();

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
                    Response.Redirect("stockanalysis.aspx", false);
            }
            else
                Response.Redirect("stockanalysis.aspx", false);
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
                datetime = HtmlUtil.Search_Dictionary(keyValues, "datetime");
                Factory = HtmlUtil.Search_Dictionary(keyValues, "type");
                //儲存cookie
                Response.Cookies.Add(HtmlUtil.Save_Cookies("stockanalysis", cust_name));
            }
            else
                Response.Redirect("stockanalysis.aspx", false);
        }

        //取得逾期數量
        private void Get_MonthTotal()
        {
            dt_monthtotal = WHE.stockanalysis_Details(Factory);
            if (HtmlUtil.Check_DataTable(dt_monthtotal))
            {
                dt_monthtotal = myclass.Add_LINE_GROUP(dt_monthtotal).ToTable();
                //移除產線代號
                dt_monthtotal.Columns.Remove("產線代號");
                dt_monthtotal.Columns["產線群組"].SetOrdinal(1);

                //移除大於N天的
                DataRow[] rows = dt_monthtotal.Select($"庫存天數<'{datetime}'");
                for (int i = 0; i < rows.Length; i++)
                    rows[i].Delete();
                dt_monthtotal.AcceptChanges();
            }

        }

        //設定表格
        private void Set_Table()
        {
            DataTable dt = dt_monthtotal;
            if (HtmlUtil.Check_DataTable(dt))
            {
                dt.Columns.Remove("庫存金額");
                dt = HtmlUtil.Get_InformationDataTable(dt, DataTableUtils.toString(dt.Columns[0]), cust_name);
                string columns = "";
                th.Append(HtmlUtil.Set_Table_Title(dt, out columns));
                tr.Append(HtmlUtil.Set_Table_Content(dt, columns, stockanalysis_details_callback));
            }
            else
                HtmlUtil.NoData(out th, out tr);
        }

        //例外處理
        private string stockanalysis_details_callback(DataRow row, string field_name)
        {
            string value = "";
            if (field_name == "入庫日")
                value = HtmlUtil.changetimeformat(DataTableUtils.toString(row[field_name]));

            return value == "" ? "" : $"<td>{value}</td>";
        }
    }
}
