using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using dek_erpvis_v2.cls;
using System.Data;
using Support;
using System.Text;
using dekERP_dll;
using dekERP_dll.dekErp;

namespace dek_erpvis_v2.pages.dp_PM
{
    public partial class Lost_Material : System.Web.UI.Page
    {
        //-------------------------------------------------參數 OR 引用------------------------------------------------------------
        public StringBuilder th = new StringBuilder();
        public StringBuilder tr = new StringBuilder();
        public string color = "";
        public string all_div = "";
        public string all_js = "";
        ERP_Product PMD = new ERP_Product();
        DataTable dt_monthtotal = new DataTable();
        List<string> avoid_again = new List<string>();
        List<string> custom_list = new List<string>();
        public int custom_count = 0;

        //----------------------------------------------------Event----------------------------------------------------------------------
        //載入事件
        protected void Page_Load(object sender, EventArgs e)
        {
            string acc = "";
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                color = HtmlUtil.change_color(acc);
                if (myclass.user_view_check("Lost_Material", acc))
                {
                    if (!IsPostBack)
                    {
                        string[] date = 德大機械.德大專用月份(acc).Split(',');
                        txt_str.Text = HtmlUtil.changetimeformat(date[0], "-");
                        txt_end.Text = HtmlUtil.changetimeformat(date[1], "-");
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
        //----------------------------------------------------Function-------------------------------------------------------------------
        //需要執行副程式
        private void MainProcess()
        {
            Get_MonthTotal();
            Set_IndividualTable();
        }

        //取得當前欠料表資料
        private void Get_MonthTotal()
        {
            dt_monthtotal = PMD.Lost_Material(txt_str.Text.Replace("-",""), txt_end.Text.Replace("-", ""), dropdownlist_Factory.SelectedItem.Value);
        }

        //取得個別欠料表
        private void Set_IndividualTable()
        {
            DataTable dt_number = dt_monthtotal.DefaultView.ToTable(true, new string[] { "刀庫編號", "客戶名稱" });
            string Now_Number = "";
           
            foreach (DataRow row in dt_number.Rows)
            {
                if (DataTableUtils.toString(row["刀庫編號"]) != "")
                {
                    Now_Number = DataTableUtils.toString(row["刀庫編號"]);

                    all_div += $"<div id=\"div_{DataTableUtils.toString(row["客戶名稱"])}{custom_list.Count()}\"></div>";
                    Set_Table(DataTableUtils.toString(row["刀庫編號"]));
                    all_js += $"create_tablecode_noshdrow_havesubtitle('div_{DataTableUtils.toString(row["客戶名稱"])}{custom_list.Count()}', '{DataTableUtils.toString(row["刀庫編號"])}  欠料單', '{DataTableUtils.toString(row["刀庫編號"])}', '{th}', '{tr}','');\n" +
                              $"set_Table('#{DataTableUtils.toString(row["刀庫編號"])}');\n" +
                              $"loadpage('', '');\n";
                    Now_Number = "";
                    th = new StringBuilder();
                    tr = new StringBuilder();
                    avoid_again.Clear();
                    custom_list.Add(DataTableUtils.toString(row["客戶名稱"]));
                }
            }
        }
        //總領料單
        private void Set_Table(string Number)
        {
            if (HtmlUtil.Check_DataTable(dt_monthtotal))
            {
                DataTable Item = new DataTable();
                var dt_number = dt_monthtotal.AsEnumerable().Where(w => w.Field<string>("刀庫編號") == Number);
                if (dt_number.FirstOrDefault() != null)
                    Item = dt_number.CopyToDataTable();
                if (HtmlUtil.Check_DataTable(Item))
                {
                    string column = "";
                    th.Append(HtmlUtil.Set_Table_Title(Item, out column));
                    tr.Append(HtmlUtil.Set_Table_Content(Item, column, Lost_Material_callback));
                }
                else
                    HtmlUtil.NoData(out th, out tr);
            }
            else
                HtmlUtil.NoData(out th, out tr);
        }

        private string Lost_Material_callback(DataRow row ,string field_name)
        {
            string value = "";
            if (field_name == "下單日" || field_name == "預計開工日")
                if (row[field_name].ToString() != "")
                    value = HtmlUtil.changetimeformat(row[field_name].ToString());

            return value == "" ? "" : $"<td>{value}</td>";
        }

    }
}