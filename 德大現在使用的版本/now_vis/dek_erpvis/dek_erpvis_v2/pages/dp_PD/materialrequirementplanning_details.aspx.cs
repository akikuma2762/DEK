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
    public partial class materialrequirementplanning_details_New : System.Web.UI.Page
    {
        //-------------------------------------------------參數 OR 引用------------------------------------------------------------
        public string color = "";
        public string item_name = "";
        public string date_str = "";
        public string date_end = "";
        public string pie_data_points = "";
        public string col_data_points = "";
        string Factory = "";
        string acc = "";
        public StringBuilder th = new StringBuilder();
        public StringBuilder tr = new StringBuilder();
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
                    Response.Redirect("materialrequirementplanning.aspx", false);
            }
            else
                Response.Redirect("materialrequirementplanning.aspx", false);
        }

        //-----------------------------------------------------Function---------------------------------------------------------------------
        //需要執行的程式
        private void MainProcess()
        {
            GetCondi();
            Get_MonthTotal();
            Set_PieChart();
            Set_ColumnChart();
            Set_Table();
        }

        //設定數值
        private void GetCondi()
        {
            Response.Buffer = false;
            if (Request.QueryString["key"] != null)
            {
                Dictionary<string, string> keyValues = HtmlUtil.Return_dictionary(Request.QueryString["key"]);
                item_name = HtmlUtil.Search_Dictionary(keyValues, "item_code");
                date_str = HtmlUtil.Search_Dictionary(keyValues, "date_str");
                date_end = HtmlUtil.Search_Dictionary(keyValues, "date_end");
                Factory = HtmlUtil.Search_Dictionary(keyValues, "type");
                //儲存cookie
                Response.Cookies.Add(HtmlUtil.Save_Cookies("materialrequirementplanning", item_name));
            }
            else
                Response.Redirect("materialrequirementplanning.aspx", false);
        }

        //取得本月領料資訊
        private void Get_MonthTotal()
        {
            dt_monthtotal = PCD.materialrequirementplanning_Detail(date_str, date_end, RadioButtonList_select_type, item_name, Factory);
        }

        //設定圓餅圖
        private void Set_PieChart()
        {
            DataTable pie_data = HtmlUtil.PrintChart_DataTable(dt_monthtotal, "用途說明", "領料數量");
            foreach (DataRow row in pie_data.Rows)
            {
                if (pie_data_points == "")
                    pie_data_points += "{" +
                                        $"y:{row["領料數量"]},name:'{row["用途說明"]} {row["領料數量"]}', label:'{row["用途說明"]}',exploded: true" +
                                        "},";
                else
                    pie_data_points += "{" +
                                        $"y:{row["領料數量"]},name:'{row["用途說明"]} {row["領料數量"]}', label:'{row["用途說明"]}' " +
                                        "},";
            }
        }

        //設定直方圖
        private void Set_ColumnChart()
        {
            col_data_points = HtmlUtil.PrintChart_String(dt_monthtotal);
        }

        //設定表格
        private void Set_Table()
        {
            if (HtmlUtil.Check_DataTable(dt_monthtotal))
            {
                string column = "";
                th.Append(HtmlUtil.Set_Table_Title(dt_monthtotal, out column));
                tr.Append(HtmlUtil.Set_Table_Content(dt_monthtotal, column, materialrequirementplanning_details_callback));
            }
            else
                HtmlUtil.NoData(out th, out tr);
        }
        
        //例外處理
        private string materialrequirementplanning_details_callback(DataRow row, string field_name)
        {
            string value = "";

            if (field_name == "領料單日期")
                value = HtmlUtil.changetimeformat(DataTableUtils.toString(row[field_name]));
            else if (field_name == "領料數量")
                value = DataTableUtils.toString(row[field_name]).Split('.')[0];

            return value == "" ? "" : $"<td>{value}</td>";
        }
    }
}
