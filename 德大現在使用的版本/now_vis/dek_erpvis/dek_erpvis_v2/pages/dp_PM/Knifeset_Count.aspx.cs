using dek_erpvis_v2.cls;
using Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using dekERP_dll.dekErp;

namespace dek_erpvis_v2.pages.dp_PM
{
    public partial class Knifeset_Count : System.Web.UI.Page
    {
        string acc = "";
        string date_str = "";
        string date_end = "";
        public StringBuilder th = new StringBuilder();
        public StringBuilder tr = new StringBuilder();
        public StringBuilder th_date = new StringBuilder();
        public StringBuilder tr_date = new StringBuilder();
        public string color = "";
        DataTable dt_monthtotal = new DataTable();
        ERP_Product PMD = new ERP_Product();
        List<string> date_list = new List<string>();

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
                    {
                        var daterange = 德大機械.德大專用月份(acc).Split(',');
                        date_str = daterange[0];
                        date_end = daterange[1];
                        if (txt_str.Text == "" && txt_end.Text == "")
                        {
                            txt_str.Text = HtmlUtil.changetimeformat(date_str, "-");
                            txt_end.Text = HtmlUtil.changetimeformat(date_end, "-");
                        }
                        MainProcess();
                    }

                }
                else
                    Response.Write("<script>alert('您無此權限!');location.href='../index.aspx';</script>");
            }
            else
                Response.Redirect(myclass.logout_url);
        }

        protected void Unnamed_ServerClick(object sender, EventArgs e)
        {
            date_str = txt_str.Text.Replace("-", "");
            date_end = txt_end.Text.Replace("-", "");
            MainProcess();
        }
        private void MainProcess()
        {
            Get_MonthTotal();
            Set_Table(th, tr);
            Set_Table(th_date, tr_date, true);
        }

        private void Get_MonthTotal()
        {
            dt_monthtotal = PMD.KnifeSet(date_str, date_end, dropdownlist_Factory.SelectedItem.Value);
            if (HtmlUtil.Check_DataTable(dt_monthtotal))
            {
                //先把類別不含刀套的去除
                DataRow[] rows = dt_monthtotal.Select("類別 not like '%刀套%' ");
                for (int i = 0; i < rows.Length; i++)
                    rows[i].Delete();
                dt_monthtotal.AcceptChanges();

                //排序
                DataView dv_mant = new DataView(dt_monthtotal);
                dv_mant.Sort = "預計開工日 asc";
                dt_monthtotal = dv_mant.ToTable();
                dt_monthtotal.Columns["預計開工日"].MaxLength = 15;

                //變更日期格式
                foreach (DataRow row in dt_monthtotal.Rows)
                    row["預計開工日"] = HtmlUtil.changetimeformat(DataTableUtils.toString(row["預計開工日"]));
                dt_monthtotal.AcceptChanges();
            }
        }

        private void Set_Table(StringBuilder th, StringBuilder tr, bool ok = false)
        {
            if (HtmlUtil.Check_DataTable(dt_monthtotal))
            {
                DataTable Item = dt_monthtotal.DefaultView.ToTable(true, new string[] { "母件品名", "子件品號", "子件品名", "位置" });
                if (ok)
                {
                    DataTable date = dt_monthtotal.DefaultView.ToTable(true, new string[] { "預計開工日" });
                    foreach (DataRow row in date.Rows)
                    {
                        Item.Columns.Add(DataTableUtils.toString(row["預計開工日"]));
                        date_list.Add(DataTableUtils.toString(row["預計開工日"]));
                    }
                }
                Item.Columns.Add("需求量");
                string column = "";
                th.Append(HtmlUtil.Set_Table_Title(Item, out column));
                tr.Append(HtmlUtil.Set_Table_Content(Item, column, Knifeset_Count_callback));
            }
        }
        private string Knifeset_Count_callback(DataRow row, string field_name)
        {
            string value = "";
            if (field_name == "需求量")
            {
                string sqlcmd = row["母件品名"].ToString() == "" ? $"子件品號='{row["子件品號"]}' and 子件品名='{row["子件品名"]}' and 母件品名 IS NULL" : $"子件品號='{row["子件品號"]}' and 子件品名='{row["子件品名"]}' and 母件品名='{row["母件品名"]}'";
                DataRow[] rows = dt_monthtotal.Select(sqlcmd);
                double count = 0;
                if (rows != null && rows.Length > 0)
                {
                    for (int i = 0; i < rows.Length; i++)
                        count += DataTableUtils.toDouble(DataTableUtils.toString(rows[i]["需求量"]));
                }
                value = count.ToString("0");
            }
            else if (date_list.IndexOf(field_name) != -1)
            {
                string sqlcmd = row["母件品名"].ToString() == "" ? $"子件品號='{row["子件品號"]}' and 子件品名='{row["子件品名"]}' and 母件品名 IS NULL and 預計開工日='{field_name}'": $"子件品號='{row["子件品號"]}' and 子件品名='{row["子件品名"]}' and 母件品名='{row["母件品名"]}' and 預計開工日='{field_name}'";
                DataRow[] rows = dt_monthtotal.Select(sqlcmd);
                double count = 0;
                if (rows != null && rows.Length > 0)
                {
                    for (int i = 0; i < rows.Length; i++)
                        count += DataTableUtils.toDouble(DataTableUtils.toString(rows[i]["需求量"]));
                }
                value = count.ToString("0");
            }
            return value == "" ? "" : $"<td>{value}</td>";
        }
    }
}