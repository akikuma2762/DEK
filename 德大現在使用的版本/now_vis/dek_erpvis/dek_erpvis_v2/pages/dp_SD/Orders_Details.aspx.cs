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
    public partial class Orders_Details_New : System.Web.UI.Page
    {   
        //-------------------------------------------------參數 OR 引用------------------------------------------------------------
        public string color = "";
        public string cust_name = "";
        public string dt_st = "";
        public string dt_ed = "";
        public string Get_SOStatus = "";
        string acc = "";
        string Factory = "";
        string mode = "";
        public string dt_Row = "";
        string selectItem_X = "";
        public string table_Title = "";
        public StringBuilder tr = new StringBuilder();
        public StringBuilder th = new StringBuilder();
        myclass myclass = new myclass();
        ERP_Sales SLS = new ERP_Sales();
        DataTable dt_monthtotal = new DataTable();
        DataTable dt_Overdue = new DataTable();

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
                    Response.Redirect("Orders.aspx", false);
            }
            else
                Response.Redirect("Orders.aspx", false);
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
                //cust_name = HtmlUtil.Search_Dictionary(keyValues, "cust");
                dt_st = HtmlUtil.Search_Dictionary(keyValues, "date_str");
                dt_ed = HtmlUtil.Search_Dictionary(keyValues, "date_end");
                Get_SOStatus = HtmlUtil.Search_Dictionary(keyValues, "condi");
                //--新增超連結 參數 20220615--//
                Factory = HtmlUtil.Search_Dictionary(keyValues, "type");
                mode = HtmlUtil.Search_Dictionary(keyValues, "mode");
                dt_Row = HtmlUtil.Search_Dictionary(keyValues, "dt_Row");
                selectItem_X = HtmlUtil.Search_Dictionary(keyValues, "selectItem_X");
                table_Title = dt_Row;
                if (mode.Contains("order_month_Overdue")) table_Title += "逾期";
                //---------------------------//
                //儲存cookie
                Response.Cookies.Add(HtmlUtil.Save_Cookies("Order", table_Title));
            }
            else
                Response.Redirect("Orders.aspx", false);
        }

        //取得本月訂單數量跟逾期數量
        private void Get_MonthTotal()
        {
            dekERP_dll.OrderStatus status = new dekERP_dll.OrderStatus();
            switch (Get_SOStatus)
            {
                case "0":
                    status = dekERP_dll.OrderStatus.All;
                    break;
                case "1":
                    status = dekERP_dll.OrderStatus.Finished;
                    break;
                case "2":
                    status = dekERP_dll.OrderStatus.Unfinished;
                    break;
                case "3":
                    status = dekERP_dll.OrderStatus.Scheduled;
                    break;
                case "4":
                    status = dekERP_dll.OrderStatus.Unscheduled;
                    break;
            }

            dt_monthtotal = SLS.Orders_Detail(dt_st, dt_ed, status, Factory,true);

            //20220812 大圓盤語法優化,新增判斷客戶,立式臥式待調整
            if(Factory=="dek")
            dt_Overdue = selectItem_X!="產線"? SLS.Orders_Over_Detail_Customer(dt_st, Factory, true,dt_Row) : SLS.Orders_Over_Detail(dt_st, Factory, true);
            else
                dt_Overdue=SLS.Orders_Over_Detail(dt_st, Factory, true);
            if (HtmlUtil.Check_DataTable(dt_monthtotal))
                dt_monthtotal = myclass.Add_LINE_GROUP(dt_monthtotal).ToTable();

            if (HtmlUtil.Check_DataTable(dt_Overdue))
                dt_Overdue = myclass.Add_LINE_GROUP(dt_Overdue).ToTable();
        }

        //設定表格
        private void Set_Table()
        {
            DataTable dt = new DataTable();
            if (dt_ed != "")
                dt = dt_monthtotal;
            else
                dt = dt_Overdue;

            List<string> columns_list = HtmlUtil.Get_ColumnsList(acc, "Orders_Details");
            if (HtmlUtil.Check_DataTable(dt))
            {
                //移除產線代號
                dt.Columns.RemoveAt(0);
                dt.Columns.Remove("金額");
                try
                {
                    dt.Columns.Remove("計算月份");
                }
                catch
                {

                }
                //--新增模式判斷20220615--//
                if (mode == "order")
                {
                    dt = HtmlUtil.Get_InformationDataTable(dt, DataTableUtils.toString(dt.Columns[0]), dt_Row);
                }
                else if (mode == "order_Overdue")
                {
                    dt = HtmlUtil.Get_InformationDataTable(dt, DataTableUtils.toString(dt.Columns[0]), dt_Row);
                }
                else if (mode == "order_month")
                {
                    if(selectItem_X=="客戶") dt = HtmlUtil.Get_InformationDataTable(dt, DataTableUtils.toString(dt.Columns[0]), dt_Row);
                    else dt = HtmlUtil.Get_InformationDataTable(dt, DataTableUtils.toString(dt.Columns[10]), dt_Row);
                }
                else if (mode == "order_month_Overdue")
                {
                    if (selectItem_X == "客戶") dt = HtmlUtil.Get_InformationDataTable(dt, DataTableUtils.toString(dt.Columns[0]), dt_Row);
                    else dt = HtmlUtil.Get_InformationDataTable(dt, DataTableUtils.toString(dt.Columns[10]), dt_Row);
                }
                else if (mode == "order_month_Overdue_Total")
                {
                    DataTable dt_clone = dt.Clone();
                    string sqlcmd = $"{dt.Columns[10]}<>'T1'";
                    DataRow[] rows = dt.Select(sqlcmd);
                    if (rows != null && rows.Length > 0)
                    {
                        for (int i = 0; i < rows.Length; i++)
                        {
                            dt_clone.ImportRow(rows[i]);
                        } 
                    }
                    dt_clone.AcceptChanges();
                    dt = dt_clone;
                }
                else if (mode == "order_month_capacity")
                {
                    if (selectItem_X == "客戶") dt = HtmlUtil.Get_MonthCapacity_InformationDataTable(dt, DataTableUtils.toString(dt.Columns[0]), dt_Row, dt_st,dt_ed, "month_capacity");
                    else dt = HtmlUtil.Get_MonthCapacity_InformationDataTable(dt, DataTableUtils.toString(dt.Columns[10]), dt_Row, dt_st, dt_ed, "month_capacity");
                }
                else if (mode == "order_month_capacity_total")
                {
                    if (selectItem_X == "客戶") dt = HtmlUtil.Get_MonthCapacity_InformationDataTable(dt, DataTableUtils.toString(dt.Columns[0]), dt_Row, dt_st, dt_ed, "month_capacity_total");
                    else dt = HtmlUtil.Get_MonthCapacity_InformationDataTable(dt, DataTableUtils.toString(dt.Columns[10]), dt_Row, dt_st, dt_ed, "month_capacity_total");
                }
                //----------------------//

                dt.Columns["產線群組"].SetOrdinal(1);
                //如果資料庫存在資料的話
                if (columns_list.Count > 0)
                {
                    th.Append(HtmlUtil.Set_Table_Title(columns_list));
                    tr.Append(HtmlUtil.Set_Table_Content(dt, columns_list, orderDetails_callback));
                }
                else
                {
                    string columns = "";
                    th.Append(HtmlUtil.Set_Table_Title(dt, out columns));
                    tr.Append(HtmlUtil.Set_Table_Content(dt, columns, orderDetails_callback));
                }
            }
            else
                HtmlUtil.NoData(out th, out tr);
        }

        //例外處理
        private string orderDetails_callback(DataRow row, string field_name)
        {
            string value = "";
            if (field_name == "預計開工日" || field_name == "入庫日" || field_name == "出貨日")
                value = HtmlUtil.changetimeformat(DataTableUtils.toString(row[field_name]));

            if (value == "")
                return "";
            else
                return $"<td>{value}</td>";
        }
    }
}
