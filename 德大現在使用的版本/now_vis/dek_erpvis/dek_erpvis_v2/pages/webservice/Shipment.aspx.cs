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

namespace dek_erpvis_v2.pages.webservice
{
    public partial class Shipment : System.Web.UI.Page
    {
        public string color = "";
        public string dt_st = "";
        public string dt_ed = "";
        public string select_year = "";
        public string title_text = "沒有資料載入";
        public string title_text_cust = "沒有資料載入";
        public string col_data_Points = "";
        public string col_data_Points_cust = "";
        public string time_area_text = "";
        public string th = "";
        public string tr = "";
        public string timerange = "";
        public string path = "";
        string URL_NAME = "";
        string acc = "";
        int CUST_TOTAL;
        string[] str = null;
        DataTable dt = new DataTable();
        DataTable custom = new DataTable();
        DataTable Line = new DataTable();
        myclass myclass = new myclass();
        德大機械 德大機械 = new 德大機械();
        Service service = new Service();
        //--------------------------------------EVENT-----------------------------------------------
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                path = 德大機械.get_title_web_path("SLS");
                path = path.Replace("</ol>", "<li><u><a href='../webservice/shipment.aspx'>出貨統計表</a></u></li></ol>");
                URL_NAME = "shipment";
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                color = HtmlUtil.change_color(acc);
                //if (myclass.user_view_check(URL_NAME, acc) == true)
                if (HtmlUtil.Search_acc_Column(acc) == "A" || HtmlUtil.Search_acc_Column(acc) == "B")
                {
                    if (!IsPostBack)
                    {
                        string[] daterange = 德大機械.德大專用月份(acc).Split(',');
                        dt_st = daterange[0];
                        dt_ed = daterange[1];
                        GotoCenn();
                    }
                }
                else
                    //  Response.Write("<script>alert('您無法瀏覽此頁面 請向該單位主管申請權限!');location.href='../index.aspx';</script>");
                    Response.Write("<script>alert('目前尚未開放權限!');location.href='../index.aspx';</script>");
            }
            else
                Response.Redirect(myclass.logout_url);
        }
        protected void button_select_Click(object sender, EventArgs e)
        {
            string[] s = 德大機械.德大專用月份(acc).Split(',');
            HtmlUtil.Button_Click(DataTableUtils.toString(((Control)sender).ID.Split('_')[1]), s, DataTableUtils.toString(txt_time_str.Value), DataTableUtils.toString(txt_time_end.Value), out dt_st, out dt_ed);


            if (DataTableUtils.toString(((Control)sender).ID.Split('_')[1]) != "select")
            {
                txt_time_str.Value = "";
                txt_time_end.Value = "";
            }
            GotoCenn();
        }
        //--------------------------------------FUCTION---------------------------------------------
        private void GotoCenn()
        {
            string date_s = HtmlUtil.changetimeformat(dt_st);
            string date_e = HtmlUtil.changetimeformat(dt_ed);
            timerange = date_s + " ~ " + date_e;
            set_col_value();
            set_table_title();
        }
        private void set_col_value()
        {
            string Json = service.GetShipmentInformation(dt_st, dt_ed, ERPVIS.dekModel.Image);
            DataTable dt = ERPVIS.i_TEC.iTechDB.JsontoDataTable(Json);

            if (HtmlUtil.Check_DataTable(dt))
            {           
                dt = myclass.Add_LINE_GROUP(dt).ToTable();
                int Total_Quantity;
                col_data_Points = HtmlUtil.Set_Chart(dt, "產線群組", "數量", "台", out Total_Quantity, out _);
                title_text = "' " + dt_st + "~" + dt_ed + " 總出貨數量 : " + Total_Quantity + "'";
            }
            else
            {
                col_data_Points = "";
                title_text = "'此區間內尚無資料'";
            }
            dt.Dispose();
        }
        private void set_table_title()
        {

            string Json = service.GetShipmentInformation(dt_st, dt_ed, ERPVIS.dekModel.Table);
            dt = ERPVIS.i_TEC.iTechDB.JsontoDataTable(Json);
            if (HtmlUtil.Check_DataTable(dt))
            {
                dt = myclass.Add_LINE_GROUP(dt).ToTable();
                dt.Columns.RemoveAt(1);
                dt.Columns["產線群組"].SetOrdinal(1);
                ////測試用資料
                //dt.Rows.Add("東台", "5", "立式");
                //dt.Rows.Add("東台", "10", "怕式");
                //dt.Rows.Add("百百", "5", "立式");

                //取得所有產線
                Line = dt.DefaultView.ToTable(true, new string[] { "產線群組" });
                //取得所有客戶
                custom = dt.DefaultView.ToTable(true, new string[] { "客戶簡稱" });

                string title = "";
                th = "<th>客戶簡稱</th>\n";
                th += HtmlUtil.Set_Table_Title(Line, out title, "產線群組");
                th += "<th>小計</th>\n";
                title = "客戶簡稱," + title + "小計,";
                str = title.Split(',');

                tr = HtmlUtil.Set_Table_Content(custom, title, Shipment_callback);
                set_col_value_cust(demo_vertical2.Value);
            }
            else
                HtmlUtil.NoData(out th, out tr);
        }
        private void set_col_value_cust(string count)
        {
            title_text_cust = "' " + dt_st + "~" + dt_ed + " 客戶出貨數量排行前 " + demo_vertical2.Value + " 名'";
            string Json = service.GetShipmentInformation(dt_st, dt_ed, ERPVIS.dekModel.Table);
            DataTable dt = ERPVIS.i_TEC.iTechDB.JsontoDataTable(Json);
            DataTable dr = new DataTable();
            dr.Columns.Add("客戶簡稱");
            dr.Columns.Add("數量", typeof(int));
            if (HtmlUtil.Check_DataTable(dt))
            {
                DataTable custom = dt.DefaultView.ToTable(true, new string[] { "客戶簡稱" });
                foreach (DataRow row in custom.Rows)
                {
                    int x = 0;
                    DataRow[] rows = dt.Select($"客戶簡稱='{row["客戶簡稱"]}'");
                    if (rows != null && rows.Length > 0)
                    {
                        for (int i = 0; i < rows.Length; i++)
                            x += DataTableUtils.toInt(rows[i]["數量"].ToString());
                    }
                    dr.Rows.Add(row["客戶簡稱"].ToString(), x);
                }
                DataView dv_mant = new DataView(dr);
                dv_mant.Sort = "數量 desc";
                dr = dv_mant.ToTable();
                int max = 0;                                                  //2.決定長條圖筆數(ru)

                if (dr.Rows.Count >= DataTableUtils.toInt(count))
                    max = DataTableUtils.toInt(count);
                else if (dr.Rows.Count < DataTableUtils.toInt(count))
                    max = dr.Rows.Count;

                for (int i = 0; i < max; i++)                                 //3.利用迴圈組合長條圖資料(ru)
                    col_data_Points_cust += "{ y: " + dr.Rows[i]["數量"].ToString() + ", label:'" + dr.Rows[i]["客戶簡稱"].ToString() + "',indexLabel:'" + dr.Rows[i]["數量"].ToString() + "台'  },";

                dt.Dispose();
                dt.Clear();
            }

        }
        private string Shipment_callback(DataRow row, string field_name)
        {
            string value = "";
            if (field_name == "小計")
            {
                string url = HtmlUtil.AttibuteValue("cust_name", DataTableUtils.toString(row["客戶簡稱"]).Trim(), "") + "," +
                                                                        HtmlUtil.AttibuteValue("date_str", dt_st, "") + "," +
                                                                        HtmlUtil.AttibuteValue("date_end", dt_ed, "");
                string href = string.Format("Shipment_Details.aspx?key={0}", WebUtils.UrlStringEncode(url));
                value = CUST_TOTAL.ToString();
                value = HtmlUtil.ToTag("u", HtmlUtil.ToHref(value, href));
            }
            else if (field_name != "客戶簡稱" && field_name != "小計")
            {
                //進入第一個產線後，將上一個總和歸0
                if (field_name == str[1])
                    CUST_TOTAL = 0;
                string sqlcmd = "客戶簡稱 ='" + DataTableUtils.toString(row["客戶簡稱"]) + "' and 產線群組 = '" + field_name + "'";
                DataRow[] rows = dt.Select(sqlcmd);
                int LINE_TOTAL = 0;
                if (rows.Length == 0)
                { }
                else
                {
                    for (int i = 0; i < rows.Length; i++)
                    {
                        for (int j = 0; j < Line.Rows.Count; j++)
                        {
                            if (field_name == DataTableUtils.toString(rows[i]["產線群組"]) && field_name == DataTableUtils.toString(Line.Rows[j]["產線群組"]))
                                LINE_TOTAL += DataTableUtils.toInt(DataTableUtils.toString(rows[i]["數量"]));
                        }
                    }
                }
                value = DataTableUtils.toString(LINE_TOTAL);
                CUST_TOTAL += LINE_TOTAL;
            }
            if (value == "")
                return "";
            else
                return "<td align='right'>" + value + "</td>";
        }
    }
}