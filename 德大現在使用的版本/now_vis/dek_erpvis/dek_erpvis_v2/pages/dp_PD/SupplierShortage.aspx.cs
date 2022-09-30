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

namespace dek_erpvis_v2.pages.dp_PD
{
    public partial class SupplierShortage_New : System.Web.UI.Page
    {
        //-------------------------------------------------參數 OR 引用------------------------------------------------------------
        public string color = "";
        public string path = "";
        public string urge_order = "";
        string dt_str = "";
        string dt_end = "";
        string Shortage = "";
        public int 未交量總計 = 0;
        public StringBuilder th = new StringBuilder();
        public StringBuilder tr = new StringBuilder();
        List<string> ERP_Source = new List<string>();
        DataTable dt_monthtotal = new DataTable();//催料單
        DataTable dt_delivery = new DataTable();//收貨單
        ERP_Materials PCD = new ERP_Materials();
        
        //----------------------------------------------------事件-------------------------------------------------------------------
        //載入事件
        protected void Page_Load(object sender, EventArgs e)
        {
            string acc = "";
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                color = HtmlUtil.change_color(acc);
                if (myclass.user_view_check("SupplierShortage", acc))
                {
                    if (!IsPostBack)
                    {
                        string[] date = 德大機械.德大專用月份(acc).Split(',');
                        dt_str = date[0];
                        dt_end = date[1];
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
        //廠區變更事件
        protected void dropdownlist_Factory_SelectedIndexChanged(object sender, EventArgs e)
        {
            Split_Source();
            textbox_BillNo.Text = "";
            Get_Maxorder();
        }

        //查詢事件
        protected void button_select_Click(object sender, EventArgs e)
        {
            dt_str = txt_str.Text.Replace("-", "");
            dt_end = txt_end.Text.Replace("-", "");
            urge_order = textbox_BillNo.Text;
            MainProcess();

        }
        //----------------------------------------------------Function-------------------------------------------------------------------
        //需要執行副程式
        private void MainProcess()
        {
            Split_Source();
            Get_Maxorder();
            Get_MonthTotal();
            Get_Delivery();
            Set_Table();
        }

        //分割來源
        private void Split_Source()
        {
            //0->採購單來源 1->進貨單來源
            ERP_Source = new List<string>(dropdownlist_Factory.SelectedItem.Value.Split(','));
        }

        //取得目前最大的催料單號
        private void Get_Maxorder()
        {
            if (textbox_BillNo.Text == "")
            {
                DataTable dt = PCD.MaxOrder(ERP_Source[1]);
                urge_order = HtmlUtil.Check_DataTable(dt) ? DataTableUtils.toString(dt.Rows[0]["催料表日期"]) : "";
                textbox_BillNo.Text = urge_order;
            }
        }

        //取得催料表
        private void Get_MonthTotal()
        {
            dt_monthtotal = urge_order != "" ? PCD.SupplierShortage_Urge(textbox_dt1.Text, textbox_dt2.Text, dt_str, dt_end, textbox_item.Text, textbox_BillNo.Text, ERP_Source[0]) : null;
            DataTable dt = PCD.Shortage(ERP_Source[0]);
            Shortage = HtmlUtil.Check_DataTable(dt) ? DataTableUtils.toString(dt.Rows[0]["繳交日期"]) : "";
        }

        //依據催料表取得相關採購/加工單號
        private void Get_Delivery()
        {
            if (HtmlUtil.Check_DataTable(dt_monthtotal))
            {
                List<string> orders_list = new List<string>();
                orders_list.Add(Shortage);
                dt_delivery = PCD.SupplierShortage_Delivery(orders_list, ERP_Source[1]);
            }
        }

        //設定前端顯示之表格
        private void Set_Table()
        {
            if (HtmlUtil.Check_DataTable(dt_monthtotal))
            {
                //先過濾第一次避免重複ROW出現
                DataView db_MP = dt_monthtotal.DefaultView;
                dt_monthtotal = db_MP.ToTable(true, "催料單號", "採購/加工單", "開單日期", "廠商代碼", "廠商簡稱", "品號", "品名規格", "催料預交日", "催料數量", "未交量", "加工代號");

                //變更未交量欄位修改與否/長度
                dt_monthtotal.Columns["未交量"].ReadOnly = false;
                dt_monthtotal.Columns["未交量"].MaxLength = 30;

                //有進貨要相減
                if (HtmlUtil.Check_DataTable(dt_delivery))
                {
                    foreach (DataRow row in dt_delivery.Rows)
                    {
                        string item = DataTableUtils.toString(row["品號"]);
                        string fact = DataTableUtils.toString(row["廠商代碼"]);
                        string Number = DataTableUtils.toString(row["加工代號"]);
                        int NoGive = DataTableUtils.toInt(DataTableUtils.toString(row["收貨量"]));
                        foreach (DataRow rew in dt_monthtotal.Rows)
                        {
                            if (DataTableUtils.toString(rew["品號"]) == item && DataTableUtils.toString(rew["廠商代碼"]) == fact && DataTableUtils.toString(rew["加工代號"]) == Number)
                            {
                                int last = NoGive - DataTableUtils.toInt(DataTableUtils.toString(rew["催料數量"]));
                                rew["未交量"] = last >= 0 ? 0 : Math.Abs(last);
                                NoGive = last >= 0 ? last : 0;
                            }
                        }
                    }
                }
                //沒有未交量的，補上催料數量
                foreach (DataRow row in dt_monthtotal.Rows)
                    if (DataTableUtils.toString(row["未交量"]) == "")
                        row["未交量"] = row["催料數量"];

                //計算所有未交量
                foreach (DataRow row in dt_monthtotal.Rows)
                    未交量總計 += DataTableUtils.toInt(DataTableUtils.toString(row["未交量"]));

                //移除無須顯示之欄位
                dt_monthtotal.Columns.Remove("催料單號");
                dt_monthtotal.Columns.Remove("廠商代碼");
                dt_monthtotal.Columns.Remove("加工代號");

                //先把未交量=0的資料移除
                DataRow[] rows = dt_monthtotal.Select("未交量='0'");
                for (int i = 0; i < rows.Length; i++)
                    rows[i].Delete();

                if (HtmlUtil.Check_DataTable(dt_monthtotal))
                {
                    string column = "";
                    th.Append(HtmlUtil.Set_Table_Title(dt_monthtotal, out column));
                    tr.Append(HtmlUtil.Set_Table_Content(dt_monthtotal, column, SupplierShortage_callback));
                }
                else
                    HtmlUtil.NoData(out th, out tr);
            }
            else
                HtmlUtil.NoData(out th, out tr);
        }

        //例外處理
        private string SupplierShortage_callback(DataRow row, string field_name)
        {
            string value = "";

            DateTime 催料預交日 = DateTime.ParseExact(row["催料預交日"].ToString(), "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
            DateTime 今天 = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            string field = field_name;
            if (field_name == field)
                value = $">{DataTableUtils.toString(row[field_name])}</td>";
            if (field_name == "催料數量")
                value = $"align=\"right\" >{DataTableUtils.toString(row[field_name])}</td>";
            if (field_name == "未交量")
                value = $"align=\"right\" >{DataTableUtils.toString(row["未交量"])}</td>";

            if (field_name == "開單日期")
                value = $">{HtmlUtil.changetimeformat(DataTableUtils.toString(row[field_name]))}</td>";
            if (field_name == "催料預交日")
                value = $">{HtmlUtil.changetimeformat(DataTableUtils.toString(row[field_name])) }</td>";

            if (DateTime.Compare(催料預交日, 今天) < 0)
                value = $"<td style=\"color: #ff0000;\" {value}";
            else
                value = $"<td {value}";
            return value;
        }
    }
}