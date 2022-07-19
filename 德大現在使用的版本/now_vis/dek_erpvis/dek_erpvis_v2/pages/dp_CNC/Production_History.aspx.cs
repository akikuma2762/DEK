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
    public partial class Production_History_new : System.Web.UI.Page
    {
        public string y_text = "";
        public string font_color = "";
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
        string acc_power = "";
        public List<string> Session_List = new List<string>();
        //event
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                URL_NAME = "Production_History";
                color = HtmlUtil.change_color(acc);
                if (myclass.user_view_check(URL_NAME, acc) )
                {
                    acc_power = CNCUtils.Find_Group(HtmlUtil.Search_acc_Column(acc, "Belong_Factory"));
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
                            int weeknow = Convert.ToInt32(DateTime.Now.DayOfWeek);
                            int daydiff = (-1) * weeknow;

                            //本周第一天
                            txt_str.Text = DateTime.Now.AddDays(daydiff).ToString("yyyy-MM-dd");

                            daydiff = (7 - weeknow) - 1;
                            //本周最后一天
                            txt_end.Text = DateTime.Now.AddDays(daydiff).ToString("yyyy-MM-dd");
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



            DropDownList_factory.SelectedIndex = DropDownList_factory.Items.IndexOf(DropDownList_factory.Items.FindByValue(TextBox_MachTypeValue.Text));
            DropDownList_Group.Items.Clear();
            List<string> list = new List<string>(TextBox_MachTypeValue.Text.Split(','));
            for (int i = 0; i < list.Count - 1; i++)
            {
                ListItem listItem = new ListItem(list[i], list[i + 1]);
                DropDownList_Group.Items.Add(listItem);
                i++;
            }
            DropDownList_Group.SelectedIndex = DropDownList_Group.Items.IndexOf(DropDownList_Group.Items.FindByText(TextBox_MachGroupText.Text));




            if (DropDownList_Group.Items.Count != 0 && DropDownList_Group.SelectedItem.Value.Split('^')[0] == "1")
            {
                for (int i = 0; i < CheckBoxList_mach.Items.Count; i++)
                {
                    if (CheckBoxList_mach.Items[i].Selected == true && CheckBoxList_mach.Items[i].Value != "全部")
                        select_item.Add(CheckBoxList_mach.Items[i].Value);
                }

                show_producthistory(select_item);
            }
            else if (DropDownList_Group.Items.Count != 0 && DropDownList_Group.SelectedItem.Value.Split('^')[0] != "1")
            {
                show_factory();
                show_producthistory(select_item);
                Response.Redirect(DropDownList_Group.SelectedItem.Value.Split('^')[0], "_blank", "");
                DropDownList_Group.Items.Clear();
            }
            else
                Response.Redirect("Production_History.aspx");

        }




        private void show_factory()
        {


            string sql = "";
            DataTable dt_mach = new DataTable();
            string all_mach = "";
            string sqlcmd = "";
            List<string> machlist = new List<string>();


            ListItem listItem = new ListItem();
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            DropDownList_factory.Items.Clear();
            DataTable dt_data = DataTableUtils.GetDataTable("select distinct area_name from mach_group where area_name <> '全廠' and area_name <> '測試區'  ");
            if (HtmlUtil.Check_DataTable(dt_data))
            {
                DropDownList_factory.Items.Add("--Select--");
                string itemname = "";
                string acc_power = CNCUtils.Find_Group(HtmlUtil.Search_acc_Column(acc, "Belong_Factory"));
                if (acc_power == "")
                {
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                    sqlcmd = "select * from mach_group where area_name = '全廠'";
                    DataTable dt_all = DataTableUtils.GetDataTable(sqlcmd);
                    //全廠的部分
                    if (HtmlUtil.Check_DataTable(dt_all))
                    {
                        //找出全廠設備
                        GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                        sql = "SELECT mach_show_name FROM machine_info where area_name <> '測試區'";
                        dt_mach = DataTableUtils.GetDataTable(sql);

                        //取得所有機台
                        if (HtmlUtil.Check_DataTable(dt_mach))
                        {
                            foreach (DataRow rw in dt_mach.Rows)
                                all_mach += DataTableUtils.toString(rw["mach_show_name"]) + "^";
                        }
                        itemname = $"--Select--,1^,";
                        foreach (DataRow row in dt_all.Rows)
                        {
                            if (DataTableUtils.toString(row["web_address"]) != "")
                                itemname += $"{DataTableUtils.toString(row["group_name"])},{DataTableUtils.toString(row["web_address"])}{Request.FilePath},";
                            else
                                itemname += $"{DataTableUtils.toString(row["group_name"])},1^{all_mach},";
                        }
                        listItem = new ListItem("全廠", itemname);
                        DropDownList_factory.Items.Add(listItem);




                    }

                    //其他廠區的部分
                    if (HtmlUtil.Check_DataTable(dt_data))
                    {

                        foreach (DataRow row in dt_data.Rows)
                        {
                            itemname = "";
                            machlist.Clear();
                            all_mach = "";
                            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                            sqlcmd = $"select * from mach_group where area_name = '{DataTableUtils.toString(row["area_name"])}'";
                            dt_all = DataTableUtils.GetDataTable(sqlcmd);


                            foreach (DataRow rew in dt_all.Rows)
                            {

                                if (DataTableUtils.toString(rew["web_address"]) != "")
                                    itemname += $"{DataTableUtils.toString(rew["group_name"])},{DataTableUtils.toString(rew["web_address"])}{Request.FilePath},";
                                else
                                {
                                    all_mach = "";
                                    string[] mach = DataTableUtils.toString(rew["mach_name"]).Split(',');
                                    for (int i = 0; i < mach.Length; i++)
                                    {
                                        if (mach[i] != "")
                                        {
                                            all_mach += CNCUtils.MachName_translation(mach[i]) + "^";
                                            machlist.Add(CNCUtils.MachName_translation(mach[i]));
                                        }
                                    }

                                    itemname += $"{DataTableUtils.toString(rew["group_name"])},1^{all_mach}^,";

                                }



                            }
                            //machlist = machlist.Distinct().ToList();
                            //for (int i = 0; i < machlist.Count; i++)
                            //    all_mach += machlist[i] + "^";

                            itemname = $"--Select--,1^," + itemname;
                            listItem = new ListItem(DataTableUtils.toString(row["area_name"]), itemname);
                            DropDownList_factory.Items.Add(listItem);
                        }
                    }
                }
                //特定廠區
                else
                {
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                    sqlcmd = $"select * from mach_group where area_name = '{acc_power}'";
                    DataTable dt_all = DataTableUtils.GetDataTable(sqlcmd);
                    //全廠的部分
                    if (HtmlUtil.Check_DataTable(dt_all))
                    {
                        itemname = "";
                        machlist.Clear();
                        all_mach = "";

                        foreach (DataRow row in dt_all.Rows)
                        {
                            if (DataTableUtils.toString(row["web_address"]) != "")
                                itemname += $"{DataTableUtils.toString(row["group_name"])},{DataTableUtils.toString(row["web_address"])}{Request.FilePath},";
                            else
                            {
                                all_mach = "";
                                string[] mach = DataTableUtils.toString(row["mach_name"]).Split(',');
                                for (int i = 0; i < mach.Length; i++)
                                {
                                    if (mach[i] != "")
                                    {
                                        all_mach += CNCUtils.MachName_translation(mach[i]) + "^";
                                        machlist.Add(CNCUtils.MachName_translation(mach[i]));
                                    }
                                }

                                itemname += $"{DataTableUtils.toString(row["group_name"])},1^{all_mach}^,";

                            }


                        }

                        machlist = machlist.Distinct().ToList();
                        for (int i = 0; i < machlist.Count; i++)
                            all_mach += machlist[i] + "^";

                        itemname = $"--Select--,1^," + itemname;
                        listItem = new ListItem(acc_power, itemname);
                        DropDownList_factory.Items.Add(listItem);
                    }
                }
            }

            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            sqlcmd = "select distinct mach_name from aps_info";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
            List<string> list = new List<string>();
            if (HtmlUtil.Check_DataTable(dt))
            {
                foreach (DataRow row in dt.Rows)
                    list.Add(DataTableUtils.toString(row["mach_name"]));
                create_item(null, null, CheckBoxList_mach, "", "", list);
            }

        }
        private void create_item(DataTable dt = null, DropDownList dropDownList = null, CheckBoxList checkBoxList = null, string column = "", string value = "", List<string> list = null, bool special = false)
        {
            if (dt != null && dropDownList != null)
            {
                ListItem listItem = new ListItem();
                dropDownList.Items.Clear();
                dropDownList.Items.Add("--Select--");
                if (!special)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        listItem = new ListItem(DataTableUtils.toString(row[column]), DataTableUtils.toString(row[value]));
                        dropDownList.Items.Add(listItem);
                    }
                }
                else if (special)
                {
                    if (acc_power == "")
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            listItem = new ListItem(DataTableUtils.toString(row[column]), DataTableUtils.toString(row[value]));
                            dropDownList.Items.Add(listItem);
                        }
                    }
                    else
                    {
                        string sqlcmd = $"area_name ='{acc_power}'";
                        DataRow[] rows = dt.Select(sqlcmd);

                        for (int i = 0; i < rows.Length; i++)
                        {
                            listItem = new ListItem(DataTableUtils.toString(rows[i][column]), DataTableUtils.toString(rows[i][value]));
                            dropDownList.Items.Add(listItem);
                        }

                    }
                }

            }
            else if (list != null && checkBoxList != null)
            {
                checkBoxList.Items.Clear();
                ListItem listItem = new ListItem();
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
            string sqlcmd = "";
            DataTable dt = new DataTable();
            if (acc_power != "")
                acc_power = $" where area_name ='{acc_power}' ";
            else
                acc_power = $" where area_name <> '測試區' ";
            if (DropDownList_factory.SelectedItem.Text == "--Select--")
            {
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                sqlcmd = $"select * from machine_info {acc_power}";
                dt = DataTableUtils.GetDataTable(sqlcmd);
                if (HtmlUtil.Check_DataTable(dt))
                {
                    foreach (DataRow row in dt.Rows)
                        list.Add(DataTableUtils.toString(row["mach_name"]));
                }
            }
            else if (DropDownList_factory.SelectedItem.Text == "全廠")
            {
                if (DropDownList_Group.SelectedItem.Text == "--Select--")
                {
                    for (int i = 1; i < DropDownList_Group.Items.Count; i++)
                    {
                        if (DropDownList_Group.Items[i].Text == "全廠設備")
                        {
                            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                            sqlcmd = "select * from machine_info  where area_name <> '測試區'";
                            dt = DataTableUtils.GetDataTable(sqlcmd);
                            if (HtmlUtil.Check_DataTable(dt))
                            {
                                foreach (DataRow row in dt.Rows)
                                    list.Add(DataTableUtils.toString(row["mach_name"]));
                            }
                        }
                        else
                        {
                            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                            sqlcmd = $"select mach_name from mach_group where group_name = '{DropDownList_Group.Items[i].Text}'  and area_name <> '測試區'";
                            dt = DataTableUtils.GetDataTable(sqlcmd);
                            if (HtmlUtil.Check_DataTable(dt))
                            {
                                string[] str = DataTableUtils.toString(dt.Rows[0]["mach_name"]).Split(',');
                                for (int x = 0; x < str.Length; x++)
                                    list.Add(str[x]);
                            }
                        }

                    }
                }
                else if (DropDownList_Group.SelectedItem.Text == "全廠設備")
                {
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                    sqlcmd = "select * from machine_info  where area_name <> '測試區'";
                    dt = DataTableUtils.GetDataTable(sqlcmd);
                    if (HtmlUtil.Check_DataTable(dt))
                    {
                        foreach (DataRow row in dt.Rows)
                            list.Add(DataTableUtils.toString(row["mach_name"]));
                    }
                }
                else
                {
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                    sqlcmd = $"select mach_name from mach_group where group_name = '{DropDownList_Group.SelectedItem.Text}'  and area_name <> '測試區'";
                    dt = DataTableUtils.GetDataTable(sqlcmd);
                    if (HtmlUtil.Check_DataTable(dt))
                    {
                        string[] str = DataTableUtils.toString(dt.Rows[0]["mach_name"]).Split(',');
                        for (int i = 0; i < str.Length; i++)
                            list.Add(str[i]);
                    }
                }

            }
            else
            {
                list.Clear();
                if (DropDownList_Group.SelectedItem.Text == "--Select--")
                {

                    for (int i = 1; i < DropDownList_Group.Items.Count; i++)
                    {
                        GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                        sqlcmd = $"select mach_name from mach_group where group_name = '{DropDownList_Group.Items[i].Text}'  ";
                        dt = DataTableUtils.GetDataTable(sqlcmd);
                        if (HtmlUtil.Check_DataTable(dt))
                        {
                            string[] str = DataTableUtils.toString(dt.Rows[0]["mach_name"]).Split(',');
                            for (int x = 0; x < str.Length; x++)
                                list.Add(str[x]);
                        }
                    }
                }
                else
                {
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                    sqlcmd = $"select mach_name from mach_group where group_name = '{DropDownList_Group.SelectedItem.Text}'";
                    dt = DataTableUtils.GetDataTable(sqlcmd);
                    if (HtmlUtil.Check_DataTable(dt))
                    {
                        string[] str = DataTableUtils.toString(dt.Rows[0]["mach_name"]).Split(',');
                        for (int i = 0; i < str.Length; i++)
                            list.Add(str[i]);
                    }
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

            y_text = DropDownList_Y.SelectedItem.Text;
            //圖片部分
            int count = 0;
            string limit = "";
            if (CheckBox_All.Checked == false)
                limit = $" limit {txt_showCount.Text} ";

            //各機台生產數量
            if (DropDownList_Y.SelectedItem.Value == "")
                Response.Redirect("Production_History.aspx");
            if (DropDownList_x.SelectedItem.Value == "machine")
            {
                sqlcmd = $"SELECT  IFNULL(mach_show_name, program_history_info.mach_name) AS  {DropDownList_x.SelectedItem.Text}, COUNT(*) AS 數量, SUM(IFNULL(amount, 1)) AS 金額 FROM program_history_info,craft_info,machine_info WHERE update_time >= '{txt_str.Text.Replace("-", "")}000000' AND enddate_time <= '{txt_end.Text.Replace("-", "")}235959' AND ({condition}) and program_history_info.main_progflg = 'true' AND mach_show_name IS NOT NULL and program_history_info.mach_name = craft_info.mach_name AND SUBSTRING_INDEX(program_history_info.main_prog, '/', - 1) = craft_info.program and machine_info.mach_name = program_history_info.mach_name GROUP BY {DropDownList_x.SelectedItem.Text}  ORDER BY {DropDownList_Y.SelectedItem.Text} DESC {limit}";
                dt = DataTableUtils.GetDataTable(sqlcmd);
                if (HtmlUtil.Check_DataTable(dt))
                    col_data_Points = HtmlUtil.Set_Chart(dt, DropDownList_x.SelectedItem.Text, DropDownList_Y.SelectedItem.Text, "", out count,out _);
            }
            else
            {
                sqlcmd = $"SELECT  IFNULL(CONCAT(product_name, '-', craft_name), SUBSTRING_INDEX(program_history_info.main_prog, '/', - 1)) AS '{DropDownList_x.SelectedItem.Text.Split('(')[0]}', COUNT(*) AS 數量, SUM(IFNULL(amount, 1)) AS 金額 FROM program_history_info, craft_info, machine_info WHERE update_time >= '{txt_str.Text.Replace("-", "")}000000' AND enddate_time <= '{txt_end.Text.Replace("-", "")}235959' AND ({condition}) AND program_history_info.mach_name = craft_info.mach_name AND SUBSTRING_INDEX(program_history_info.main_prog, '/', - 1) = craft_info.program AND mach_show_name IS NOT NULL AND program_history_info.main_progflg = 'true' AND machine_info.mach_name = program_history_info.mach_name GROUP BY IFNULL(CONCAT(product_name, '-', craft_name), SUBSTRING_INDEX(program_history_info.main_prog, '/', - 1)) ORDER BY IFNULL(CONCAT(product_name, '-', craft_name), SUBSTRING_INDEX(program_history_info.main_prog, '/', - 1)) {limit}";
                dt = DataTableUtils.GetDataTable(sqlcmd);
                if (HtmlUtil.Check_DataTable(dt))
                    col_data_Points = HtmlUtil.Set_Chart(dt, DropDownList_x.SelectedItem.Text.Split('(')[0], DropDownList_Y.SelectedItem.Text, "", out count,out _);
            }
            string unit = "";

            if (DropDownList_Y.SelectedItem.Value != "Amount")
                title_text = "'生產履歷統計'";
            else
                title_text = $"'生產{DropDownList_Y.SelectedItem.Text}：{TransThousand(count)}'";

            timerange = txt_str.Text.Replace('-', '/') + "~" + txt_end.Text.Replace('-', '/');
            string title = "";
            string qty_amt = "";



            //表格部分
            if (DropDownList_Y.SelectedItem.Value == "Amount")
                qty_amt = $"   sum(IFNULL(amount, 1)) as {DropDownList_Y.SelectedItem.Text} ";
            else
                qty_amt = $" COUNT(*) AS {DropDownList_Y.SelectedItem.Text} ";

            sqlcmd = $"SELECT  machine_info.mach_show_name AS 設備名稱, IFNULL(CONCAT(product_name, '-', craft_name), SUBSTRING_INDEX(program_history_info.main_prog, '/', - 1)) AS '工藝名稱(運行程式)', {qty_amt} FROM program_history_info, craft_info, machine_info WHERE update_time >= '{txt_str.Text.Replace("-", "")}000000' AND enddate_time <= '{txt_end.Text.Replace("-", "")}235959' AND ({condition}) AND program_history_info.main_progflg = 'true' AND mach_show_name IS NOT NULL AND machine_info.mach_name = program_history_info.mach_name AND program_history_info.mach_name = craft_info.mach_name AND SUBSTRING_INDEX(program_history_info.main_prog, '/', - 1) = craft_info.program GROUP BY 設備名稱 , IFNULL(CONCAT(product_name, '-', craft_name), SUBSTRING_INDEX(program_history_info.main_prog, '/', - 1))";
            dt = DataTableUtils.GetDataTable(sqlcmd);
            if (HtmlUtil.Check_DataTable(dt))
            {
                th = HtmlUtil.Set_Table_Title(dt, out title);
                tr = HtmlUtil.Set_Table_Content(dt, title, Production_History_callback);
            }
            else
                HtmlUtil.NoData(out th, out tr);
        }

        protected void DropDownListx_SelectedIndexChanged(object sender, EventArgs e)
        {
            set_DropDownList(DropDownList_Y);
        }
        private void set_DropDownList(DropDownList downList)
        {
            downList.Items.Clear();
            ListItem listItem = new ListItem();

            listItem = new ListItem("數量", "Quantity");
            downList.Items.Add(listItem);


            if (德大機械.function_yn(acc, "生產金額") == "Y")
            {
                listItem = new ListItem("金額", "Amount");
                downList.Items.Add(listItem);
            }

        }

        private string Production_History_callback(DataRow row, string field_name)
        {
            string value = "";
            if (field_name == "數量")
            {
                string url = HtmlUtil.AttibuteValue("machine", DataTableUtils.toString(row["設備名稱"]), "") + "," +
                             $"product={DataTableUtils.toString(row["工藝名稱(運行程式)"])}," +
                             HtmlUtil.AttibuteValue("date_str", txt_str.Text.Replace("-", ""), "") + "," +
                             HtmlUtil.AttibuteValue("date_end", txt_end.Text.Replace("-", ""), "");
                string href = $"Production_History_details.aspx?key={WebUtils.UrlStringEncode(url)}";

                value = HtmlUtil.ToTag("u", HtmlUtil.ToHref(TransThousand(DataTableUtils.toString(row[field_name])), href));
            }
            else if (field_name == "金額")
            {
                string url = HtmlUtil.AttibuteValue("machine", DataTableUtils.toString(row["設備名稱"]), "") + "," +
             $"product={DataTableUtils.toString(row["工藝名稱(運行程式)"])}," +
             HtmlUtil.AttibuteValue("date_str", txt_str.Text.Replace("-", ""), "") + "," +
             HtmlUtil.AttibuteValue("date_end", txt_end.Text.Replace("-", ""), "");
                string href = $"Production_History_details.aspx?key={WebUtils.UrlStringEncode(url)}";

                value = HtmlUtil.ToTag("u", HtmlUtil.ToHref(TransThousand(DataTableUtils.toString(row[field_name])), href));
            }
            else if (field_name == "產品名稱")
                value = CNCUtils.change_productname(DataTableUtils.toString(row[field_name]));

            if (value == "")
                return value;
            else
                return "<td>" + value + "</td>";
        }
        private string TransThousand(object yValue)//金額，千分位轉換
        {
            int yValue_trans = DataTableUtils.toInt(DataTableUtils.toString(yValue));
            return DataTableUtils.toString(yValue_trans.ToString("N0"));
        }
    }
}
