using dek_erpvis_v2.cls;
using dek_erpvis_v2.webservice;
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
    public partial class shipment_details_New : System.Web.UI.Page
    {
        //-------------------------------------------------參數 OR 引用------------------------------------------------------------
        public string color = "";
        public StringBuilder th = new StringBuilder();
        public StringBuilder tr = new StringBuilder();
        public string cust_name = "";
        public string dt_st = "";
        public string dt_ed = "";
        string acc = "";
        string Factory = "";
        myclass myclass = new myclass();
        ERP_Sales SLS = new ERP_Sales();
        DataTable dt_monthtotal = new DataTable();

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
                    Response.Redirect("shipment.aspx", false);
            }
            else
                Response.Redirect("shipment.aspx", false);
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
            
            if (Request.QueryString["key"] != null)
            {
                Dictionary<string, string> keyValues = HtmlUtil.Return_dictionary(Request.QueryString["key"]);
                cust_name = HtmlUtil.Search_Dictionary(keyValues, "cust");
                dt_st = HtmlUtil.Search_Dictionary(keyValues, "date_str");
                dt_ed = HtmlUtil.Search_Dictionary(keyValues, "date_end");
                Factory = HtmlUtil.Search_Dictionary(keyValues, "type");
                //儲存cookie
                Response.Cookies.Add(HtmlUtil.Save_Cookies("shipment", cust_name));
            }
            else
                Response.Redirect("shipment.aspx", false);
        }

        //取得本月出貨數
        private void Get_MonthTotal()
        {
            dt_monthtotal = SLS.Shipment_Detail(dt_st, dt_ed, Factory);

            if (HtmlUtil.Check_DataTable(dt_monthtotal))
            {
                dt_monthtotal = myclass.Add_LINE_GROUP(dt_monthtotal).ToTable();
                dt_monthtotal.Columns["小計"].ColumnName = "數量";
            }
        }
        
        //設定表格
        private void Set_Table()
        {
            DataTable dt = dt_monthtotal;
            if (HtmlUtil.Check_DataTable(dt))
            {
                //移除產線代號
                dt.Columns.RemoveAt(0);
                dt = HtmlUtil.Get_InformationDataTable(dt, DataTableUtils.toString(dt.Columns[0]), cust_name);
                dt.Columns["產線群組"].SetOrdinal(1);

                //去重複部分
                DataView dt_distinct = dt.DefaultView;
                dt = dt_distinct.ToTable(true, "客戶簡稱", "產線群組", "品號", "品名規格");

                dt.Columns.Add("小計");

                string columns = "";
                th.Append(HtmlUtil.Set_Table_Title(dt, out columns));
                tr.Append(HtmlUtil.Set_Table_Content(dt, columns, ShipmentDetail_Callback));
            }
            else
                HtmlUtil.NoData(out th, out tr);
        }
        
        //例外處理
        private string ShipmentDetail_Callback(DataRow row, string field_name)
        {
            string value = "";
            double total = 0;
            if (field_name == "小計")
            {
                string sqlcmd = $"客戶簡稱='{row["客戶簡稱"]}' and 產線群組='{row["產線群組"]}' and 品號='{row["品號"]}' and 品名規格='{row["品名規格"]}'";
                DataRow[] rows = dt_monthtotal.Select(sqlcmd);
                if (rows != null && rows.Length > 0)
                {
                    for(int i=0;i<rows.Length;i++)
                        total += DataTableUtils.toDouble(DataTableUtils.toString(rows[i]["數量"]));
                }
                 
                value = $"<td data-toggle=\"modal\" data-target=\"#exampleModal\">" +
                            $"<u>" +
                                $"<a href=\"javascript:void(0)\" onclick=GetShipment_details(\"{dt_st}\",\"{dt_ed}\",\"{cust_name.Trim()}\",\"{DataTableUtils.toString(row["品號"]).Trim()}\",\"{Factory}\")>" +
                                    $"{total:0}" +
                                $"</a>" +
                            $"</u>" +
                        $"</td>";
            }
            return value == "" ? "" : value;
        }
    }
}