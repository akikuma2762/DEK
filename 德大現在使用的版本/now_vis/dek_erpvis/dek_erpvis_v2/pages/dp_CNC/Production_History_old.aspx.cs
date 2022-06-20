using dek_erpvis_v2.cls;
using Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace dek_erpvis_v2.pages.dp_CNC
{
    public partial class Production_History : System.Web.UI.Page
    {
        public string th = "";
        public string color = "";
        public string tr = "";
        public string title_text = "''";
        public string x_value = "";
        public string timerange = "";
        public string col_data_Points = "";
        string acc = "";
        string URL_NAME = "";
        myclass myclass = new myclass();
        string date_str, date_end;
        德大機械 德大機械 = new 德大機械();
        List<string> select_item = new List<string>();
        //event
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                URL_NAME = "Production_History";
                color = HtmlUtil.change_color(acc);
                if (myclass.user_view_check(URL_NAME, acc) == true)
                {
                    string[] daterange = null;
                    if (!IsPostBack)
                    {
                        daterange = 德大機械.德大專用月份(acc).Split(',');
                        date_str = daterange[0];
                        date_end = daterange[1];
                        set_DropDownList(DropDownList_Y);
                        show_factory();
                        if (txt_str.Text == "" && txt_end.Text == "")
                        {
                            txt_str.Text = HtmlUtil.changetimeformat(daterange[0], "-");
                            txt_end.Text = HtmlUtil.changetimeformat(daterange[1], "-");
                        }
                        show_producthistory(select_item);
                    }

                }
                else
                    Response.Write("<script>alert('您無法瀏覽此頁面 請向該單位主管申請權限!');location.href='../index.aspx';</script>");
            }
            else
                Response.Redirect(myclass.logout_url);
        }
        protected void button_select_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < CheckBoxList_mach.Items.Count; i++)
            {
                if (CheckBoxList_mach.Items[i].Selected == true && CheckBoxList_mach.Items[i].Value != "全部")
                    select_item.Add(CheckBoxList_mach.Items[i].Value);
            }

            show_producthistory(select_item);
        }
        protected void DropDownList_factory_SelectedIndexChanged(object sender, EventArgs e)
        {
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            //取得設備
            if (DropDownList_factory.SelectedItem.Text != "--Select--")
            {
                string[] str = DropDownList_factory.SelectedItem.Value.Split(',');
                string sqlcmd = "";
                DataTable dt = new DataTable();
                //多個群組合併用
                DataTable ds = new DataTable();
                for (int i = 0; i < str.Length; i++)
                {
                    sqlcmd = $"select * from mach_group where  group_name = '{str[i]}'";
                    if (i == 0)
                        dt = DataTableUtils.GetDataTable(sqlcmd);
                    else
                        ds = DataTableUtils.GetDataTable(sqlcmd);

                    if (dt != null && ds != null)
                        dt.Merge(ds);
                }
                create_item(dt, DropDownList_Group, null, "group_name", "group_name", null);

                /*可在優化過*/
                str = DropDownList_factory.SelectedItem.Value.Split(',');
                sqlcmd = "";
                dt = new DataTable();
                string mach = "";
                for (int i = 0; i < str.Length; i++)
                {
                    sqlcmd = $"select mach_name from mach_group where group_name = '{str[i]}'";
                    dt = DataTableUtils.GetDataTable(sqlcmd);
                    if (i != str.Length - 1)
                        mach += DataTableUtils.toString(dt.Rows[0]["mach_name"]) + ",";
                    else
                        mach += DataTableUtils.toString(dt.Rows[0]["mach_name"]);
                }
                List<string> list = new List<string>(mach.Split(','));
                create_item(null, null, CheckBoxList_mach, "", "", list);
            }
            else
                DropDownList_Group.Items.Clear();
        }
        protected void DropDownList_Group_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownList_factory.SelectedItem.Value == "--Select--")
                CheckBoxList_mach.Items.Clear();
            else
            {
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                //選擇單一群組時
                if (DropDownList_Group.SelectedItem.Value != "--Select--")
                {
                    string sqlcmd = $"select mach_name from mach_group where group_name = '{DropDownList_Group.SelectedItem.Value}'";
                    DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
                    string mach = DataTableUtils.toString(dt.Rows[0]["mach_name"]);
                    List<string> list = new List<string>(mach.Split(','));
                    create_item(null, null, CheckBoxList_mach, "", "", list);
                }
                //選擇--Select--時，底下全顯示
                else
                {
                    string[] str = DropDownList_factory.SelectedItem.Value.Split(',');
                    string sqlcmd = "";
                    DataTable dt = new DataTable();
                    string mach = "";
                    for (int i = 0; i < str.Length; i++)
                    {
                        sqlcmd = $"select mach_name from mach_group where group_name = '{str[i]}'";
                        dt = DataTableUtils.GetDataTable(sqlcmd);
                        if (i != str.Length - 1)
                            mach += DataTableUtils.toString(dt.Rows[0]["mach_name"]) + ",";
                        else
                            mach += DataTableUtils.toString(dt.Rows[0]["mach_name"]);
                    }
                    List<string> list = new List<string>(mach.Split(','));
                    create_item(null, null, CheckBoxList_mach, "", "", list);
                }
            }
        }
        protected void CheckBoxList_mach_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void CheckBox_All_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox_All.Checked == true)
                txt_showCount.Enabled = false;
            else
                txt_showCount.Enabled = true;
        }
        private void show_factory()
        {
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            //取得廠區
            string sqlcmd = "select * from mach_type";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
            create_item(dt, DropDownList_factory, null, "type_name", "group_name");

            sqlcmd = "select distinct mach_name from aps_info";
            dt = DataTableUtils.GetDataTable(sqlcmd);
            List<string> list = new List<string>();
            foreach (DataRow row in dt.Rows)
                list.Add(DataTableUtils.toString(row["mach_name"]));
            create_item(null, null, CheckBoxList_mach, "", "", list);
        }
        private void create_item(DataTable dt = null, DropDownList dropDownList = null, CheckBoxList checkBoxList = null, string column = "", string value = "", List<string> list = null)
        {
            if (dt != null && dropDownList != null)
            {
                ListItem listItem = new ListItem();
                dropDownList.Items.Clear();
                dropDownList.Items.Add("--Select--");
                foreach (DataRow row in dt.Rows)
                {
                    listItem = new ListItem(DataTableUtils.toString(row[column]), DataTableUtils.toString(row[value]));
                    dropDownList.Items.Add(listItem);
                }
            }
            else if (list != null && checkBoxList != null)
            {
                checkBoxList.Items.Clear();
                ListItem listItem = new ListItem();
                //  checkBoxList.Items.Add("全部");
                for (int i = 0; i < list.Count; i++)
                {
                    listItem = new ListItem(CNCUtils.MachName_translation(list[i]), list[i]);
                    CheckBoxList_mach.Items.Add(listItem);
                }
            }
        }
        //顯示生產履歷
        private void show_producthistory(List<string> list)
        {
            x_value = DropDownList_x.SelectedItem.Text;
            string condition = "";
            if (list.Count == 0)
            {
                list.Clear();
                for (int i = 0; i < CheckBoxList_mach.Items.Count; i++)
                {
                    if (CheckBoxList_mach.Items[i].Value != "全部")
                        list.Add(CheckBoxList_mach.Items[i].Value);
                }
            }
            for (int i = 0; i < list.Count; i++)
            {
                if (i != 0)
                    condition += $" OR machine_info.mach_name='{list[i]}' ";
                else
                    condition += $" machine_info.mach_name='{list[i]}' ";
            }
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);


            //圖片部分
            int count = 0;
            string limit = "";
            if (CheckBox_All.Checked == false)
                limit = $" limit {txt_showCount.Text} ";
            string sqlcmd = "";
            DataTable dt = new DataTable();
            //各機台生產數量
            if (DropDownList_Y.SelectedItem.Value == "")
                Response.Redirect("Production_History.aspx");
            if (DropDownList_x.SelectedItem.Value == "machine")
            {
                sqlcmd = $"SELECT mach_show_name AS 機台名稱, COUNT(*) AS 數量, sum(IFNULL(product_info.amount, 1)) AS 金額 FROM product_history_info left join machine_info on machine_info.mach_name = product_history_info.mach_name       LEFT JOIN    product_info ON product_info.craft_name = product_history_info.productname WHERE update_time >=  { txt_str.Text.Replace("-", "") + "000000"} AND enddate_time <= { txt_end.Text.Replace("-", "") + "235959"} and mach_show_name is not null and ({condition}) GROUP BY product_history_info.mach_name order by {DropDownList_Y.SelectedItem.Text}  desc {limit} ";
                dt = DataTableUtils.GetDataTable(sqlcmd);
                col_data_Points = HtmlUtil.Set_Chart(dt, "機台名稱", DropDownList_Y.SelectedItem.Text, "", out count, out _);
            }
            else
            {
                sqlcmd = $"SELECT   IFNULL(product_info.cnc_craft_name,productname) AS 產品名稱, COUNT(*) AS 數量, sum(IFNULL(product_info.amount, 1)) AS 金額  FROM product_history_info left join machine_info on machine_info.mach_name = product_history_info.mach_name   LEFT JOIN  product_info ON product_info.craft_name = product_history_info.productname  WHERE update_time >=  { txt_str.Text.Replace("-", "") + "000000"} AND enddate_time <= { txt_end.Text.Replace("-", "") + "235959"} and mach_show_name is not null and ({condition}) GROUP BY product_history_info.productname ORDER BY {DropDownList_Y.SelectedItem.Text} DESC {limit} ";
                dt = DataTableUtils.GetDataTable(sqlcmd);
                col_data_Points = HtmlUtil.Set_Chart(dt, "產品名稱", DropDownList_Y.SelectedItem.Text, "", out count, out _);
            }
            string unit = "";
            if (DropDownList_Y.SelectedItem.Value == "Amount")
                unit = "NTD";
            else
                unit = "件";

            title_text = $"'總生產{DropDownList_Y.SelectedItem.Text}({unit})：{TransThousand(count)}'";


            timerange = txt_str.Text.Replace('-', '/') + "~" + txt_end.Text.Replace('-', '/');
            string title = "";
            string qty_amt = "";



            //表格部分
            if (DropDownList_Y.SelectedItem.Value == "Amount")
                qty_amt = $" sum(IFNULL(product_info.amount, 1)) AS {DropDownList_Y.SelectedItem.Text} ";
            else
                qty_amt = $" COUNT(*) AS {DropDownList_Y.SelectedItem.Text} ";

            sqlcmd = $"SELECT     machine_info.mach_show_name as 機台名稱,    productname as 產品名稱, {qty_amt}     FROM  product_history_info    left join machine_info on machine_info.mach_name = product_history_info.mach_name  LEFT JOIN  product_info ON product_info.craft_name = product_history_info.productname     WHERE update_time >=  { txt_str.Text.Replace("-", "") + "000000"}    AND enddate_time <= { txt_end.Text.Replace("-", "") + "235959"} and mach_show_name is not null and ({condition})  group by 機台名稱,產品名稱";
            dt = DataTableUtils.GetDataTable(sqlcmd);
            th = HtmlUtil.Set_Table_Title(dt, out title);
            tr = HtmlUtil.Set_Table_Content(dt, title, Production_History_callback);
        }

        protected void DropDownListx_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DropDownList_Y.Items.Clear();
            set_DropDownList(DropDownList_Y);
        }
        private void set_DropDownList(DropDownList downList)
        {
            downList.Items.Clear();
            ListItem listItem = new ListItem();
            if (DropDownList_x.SelectedItem.Value == "product")
            {
                listItem = new ListItem("數量", "Quantity");
                downList.Items.Add(listItem);
            }

            if (德大機械.function_yn(acc, "生產金額") == "Y")
            {
                listItem = new ListItem("金額", "Amount");
                downList.Items.Add(listItem);
            }
            else if (德大機械.function_yn(acc, "生產金額") != "Y" && DropDownList_x.SelectedItem.Value != "product")
            {
                listItem = new ListItem("", "");
                downList.Items.Add(listItem);
            }

        }

        private string Production_History_callback(DataRow row, string field_name)
        {
            string value = "";
            if (field_name == DropDownList_Y.SelectedItem.Text)
            {
                string url = HtmlUtil.AttibuteValue("machine", DataTableUtils.toString(row["機台名稱"]), "") + "," +
                             "product=" + DataTableUtils.toString(row["產品名稱"]) + "," +
                             HtmlUtil.AttibuteValue("date_str", txt_str.Text.Replace("-", ""), "") + "," +
                             HtmlUtil.AttibuteValue("date_end", txt_end.Text.Replace("-", ""), "");
                string href = $"Production_History_details.aspx?key={WebUtils.UrlStringEncode(url)} ";

                value = HtmlUtil.ToTag("u", HtmlUtil.ToHref(TransThousand(DataTableUtils.toString(row[field_name])), href));
            }
            else if (field_name == "產品名稱")
                value = CNCUtils.change_productname(DataTableUtils.toString(row[field_name]));

            if (value == "")
                return value;
            else
                return "<td>" + value + "</td>\n";
        }
        private string TransThousand(object yValue)//金額，千分位轉換
        {
            int yValue_trans = DataTableUtils.toInt(DataTableUtils.toString(yValue));
            return DataTableUtils.toString(yValue_trans.ToString("N0"));
        }
    }
}
