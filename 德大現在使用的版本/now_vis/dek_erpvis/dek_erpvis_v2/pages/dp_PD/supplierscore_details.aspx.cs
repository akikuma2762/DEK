using dek_erpvis_v2.cls;
using dek_erpvis_v2.webservice;
using Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using dekERP_dll;
using dekERP_dll.dekErp;
using System.Text;

namespace dek_erpvis_v2.pages.dp_PD
{
    public partial class supplierscore_details_New : System.Web.UI.Page
    {
        //-------------------------------------------------參數 OR 引用------------------------------------------------------------
        public string color = "";
        public string sup_sname = "";
        public string date_str = "";
        public string date_end = "";
        string Factory = "";
        public StringBuilder th = new StringBuilder();
        public StringBuilder tr = new StringBuilder();
        public string acc = "";
        ERP_Materials PCD = new ERP_Materials();
        DataTable dt_monthtotal = new DataTable();

        //-----------------------------------------------------EVENT-----------------------------------------------------------------------------
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
                    Response.Redirect("supplierscore.aspx", false);
            }
            else
                Response.Redirect("supplierscore.aspx", false);
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
                sup_sname = HtmlUtil.Search_Dictionary(keyValues, "sup_sname");
                date_str = HtmlUtil.Search_Dictionary(keyValues, "date_str");
                date_end = HtmlUtil.Search_Dictionary(keyValues, "date_end");
                Factory = HtmlUtil.Search_Dictionary(keyValues, "type");
                //儲存cookie
                Response.Cookies.Add(HtmlUtil.Save_Cookies("supplierscore", sup_sname));
            }
            else
                Response.Redirect("supplierscore.aspx", false);
        }

        //取得本月達交率
        private void Get_MonthTotal()
        {
            dt_monthtotal = PCD.Supplierscore_Detail(date_str, date_end, Factory);
        }

        //設定表格
        private void Set_Table()
        {
            DataTable dt = dt_monthtotal;
            if (HtmlUtil.Check_DataTable(dt))
            {
                dt = HtmlUtil.Get_InformationDataTable(dt, DataTableUtils.toString(dt.Columns[0]), sup_sname);

                string columns = "";
                th.Append(HtmlUtil.Set_Table_Title(dt, out columns));
                tr.Append(HtmlUtil.Set_Table_Content(dt, columns, supplierscore_details_callback));
            }
            else
                HtmlUtil.NoData(out th, out tr);
        }

        //例外處理
        private string supplierscore_details_callback(DataRow row, string field_name)
        {
            string value = "";
            if (field_name == "採購數量")
                value = $"<td>{DataTableUtils.toString(row["採購數量"]).Split('.')[0]}</td>";
            if (field_name == "期限內已交數量")
                value = $"<td>{DataTableUtils.toString(row["期限內已交數量"]).Split('.')[0]}</td>"; ;
            if (field_name == "採購單日期")
                value = $"<td>{HtmlUtil.changetimeformat(DataTableUtils.toString(row["採購單日期"]))}</td>"; ;
            if (field_name == "預交日期")
                value = $"<td>{HtmlUtil.changetimeformat(DataTableUtils.toString(row["預交日期"]))}</td>"; ;
            if (field_name == "期限內最後進貨日期")
                value = $"<td>{HtmlUtil.changetimeformat(DataTableUtils.toString(row["期限內最後進貨日期"]))}</td>"; ;
            return value;
        }
    }
}