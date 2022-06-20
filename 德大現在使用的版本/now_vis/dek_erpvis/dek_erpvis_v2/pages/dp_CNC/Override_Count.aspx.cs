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
    public partial class Override_Count : System.Web.UI.Page
    {
        string acc = "";
        string URL_NAME = "";
        public string color = "";
        public string path = "";
        public string th = "";
        public string tr = "";
        myclass myclass = new myclass();
        public int dt_count = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                URL_NAME = "Override_Count";
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                color = HtmlUtil.change_color(acc);
                if (myclass.user_view_check(URL_NAME, acc) == true)
                {
                    if (!IsPostBack)
                    {
                        if (txt_date.Text == "")
                            txt_date.Text = DateTime.Now.ToString("yyyy-MM-dd");
                        load_data();
                    }
                }
                else
                    Response.Write("<script>alert('您無此權限!');location.href='../index.aspx';</script>");
            }
            else
                Response.Redirect(myclass.logout_url);
        }
        private void load_data()
        {
            Set_Factory();
            Set_Table();
        }
        private void Set_Table()
        {
            string symbol = "";
            string condition = "";
            string mach = "";
            string acc_power = CNCUtils.Find_Group(HtmlUtil.Search_acc_Column(acc, "Belong_Factory"));
            if (acc_power != "")
                acc_power = $" where area_name ='{acc_power}' ";
            else
                acc_power = $" where area_name <> '測試區' ";
            string sql = "";
            DataTable ds = new DataTable();

            if (DropDownList_Group.Items.Count > 0 && DropDownList_Group.SelectedItem.Value != "")
            {
                condition = "";
                if (DropDownList_factory.SelectedItem.Text == "--Select--")
                {

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
                                sql = "select * from machine_info where area_name <> '測試區'";
                                ds = DataTableUtils.GetDataTable(sql);
                                if (HtmlUtil.Check_DataTable(ds))
                                {
                                    foreach (DataRow row in ds.Rows)
                                        mach += DataTableUtils.toString(row["mach_name"]) + ",";
                                }
                            }
                            else
                            {
                                GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                                sql = $"select mach_name from mach_group where group_name = '{DropDownList_Group.Items[i].Text}' and area_name <> '測試區'";
                                ds = DataTableUtils.GetDataTable(sql);
                                if (HtmlUtil.Check_DataTable(ds))
                                {
                                    string[] str = DataTableUtils.toString(ds.Rows[0]["mach_name"]).Split(',');
                                    for (int x = 0; x < str.Length; x++)
                                        mach += str[x] + ",";
                                }
                            }
                        }
                    }
                    else if (DropDownList_Group.SelectedItem.Text == "全廠設備")
                    {
                        GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                        sql = "select * from machine_info where area_name <> '測試區'";
                        ds = DataTableUtils.GetDataTable(sql);
                        if (HtmlUtil.Check_DataTable(ds))
                        {
                            foreach (DataRow row in ds.Rows)
                                mach += DataTableUtils.toString(row["mach_name"]) + ",";
                        }
                    }
                    else
                    {
                        GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                        sql = $"select mach_name from mach_group where group_name = '{DropDownList_Group.SelectedItem.Text}' and area_name <> '測試區'";
                        ds = DataTableUtils.GetDataTable(sql);
                        if (HtmlUtil.Check_DataTable(ds))
                        {
                            string[] str = DataTableUtils.toString(ds.Rows[0]["mach_name"]).Split(',');
                            for (int i = 0; i < str.Length; i++)
                                mach += str[i] + ",";
                        }
                    }

                }
                else
                {
                    mach = "";
                    if (DropDownList_Group.SelectedItem.Text == "--Select--")
                    {

                        for (int i = 1; i < DropDownList_Group.Items.Count; i++)
                        {
                            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                            sql = $"select mach_name from mach_group where group_name = '{DropDownList_Group.Items[i].Text}' ";
                            ds = DataTableUtils.GetDataTable(sql);
                            if (HtmlUtil.Check_DataTable(ds))
                            {
                                string[] str = DataTableUtils.toString(ds.Rows[0]["mach_name"]).Split(',');
                                for (int x = 0; x < str.Length; x++)
                                    mach += str[x] + ",";
                            }
                        }
                    }
                    else
                    {
                        GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                        sql = $"select mach_name from mach_group where group_name = '{DropDownList_Group.SelectedItem.Text}'";
                        ds = DataTableUtils.GetDataTable(sql);
                        if (HtmlUtil.Check_DataTable(ds))
                        {
                            string[] str = DataTableUtils.toString(ds.Rows[0]["mach_name"]).Split(',');
                            for (int i = 0; i < str.Length; i++)
                                mach += str[i] + ",";
                        }
                    }
                }
                List<string> list = new List<string>(mach.Split(','));
                for (int i = 0; i < list.Count; i++)
                {
                    if (i == 0)
                        condition += $" and ( override_history_info.mach_name = '{list[i]}' ";
                    else
                        condition += $" OR override_history_info.mach_name = '{list[i]}' ";
                }
                condition = condition + ")";



                GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                if (DropDownList_Symbol.SelectedItem.Value == ">")
                    symbol = " >= ";
                else
                    symbol = " <= ";

                string sqlcmd = $"SELECT mach_show_name as 機台名稱,{DropDownList_Type.SelectedItem.Value} as {DropDownList_Type.SelectedItem.Text},update_time as 開始時間,enddate_time as 結束時間,timespan as 持續時間 FROM override_history_info left join machine_info on override_history_info.mach_name =  machine_info.mach_name where update_time>='{txt_date.Text.Replace("-", "")}000000' and  update_time<='{txt_date.Text.Replace("-", "")}235959' and    CAST({DropDownList_Type.SelectedItem.Value} AS UNSIGNED) {symbol}'{TextBox_Range.Text}' {condition} ";
                DataTable dt = DataTableUtils.GetDataTable(sqlcmd);


                if (HtmlUtil.Check_DataTable(dt))
                {
                    dt_count = 2;
                    string title = "";
                    th = HtmlUtil.Set_Table_Title(dt, out title);
                    tr = HtmlUtil.Set_Table_Content(dt, title, OverrideCount_callback);
                }
                else
                {
                    HtmlUtil.NoData(out th, out tr);
                    dt_count = 0;
                }

            }
            else
            {
                HtmlUtil.NoData(out th, out tr);
                dt_count = 0;
            }

        }
        private string OverrideCount_callback(DataRow row, string field_name)
        {
            string value = "";

            if (field_name == "開始時間" || field_name == "結束時間")
                value = Convert.ToDateTime(DateTime.ParseExact(DataTableUtils.toString(row[field_name]), "yyyyMMddHHmmss.f", System.Globalization.CultureInfo.CurrentCulture)).ToString("yyyy/MM/dd HH:mm:ss");
            else if (field_name == "持續時間")
            {
                int seconds = DataTableUtils.toInt(DataTableUtils.toString(row[field_name]).Split('.')[0]);
                var timespan = TimeSpan.FromSeconds(seconds);
                string day = "";
                if (timespan.ToString("%d") != "0")
                    day = timespan.ToString("%d") + " 天  ";
                value = day + timespan.ToString(@"hh\:mm\:ss");
            }
            else if (field_name == "進給率")
                value = DataTableUtils.toDouble(DataTableUtils.toString(row[field_name])).ToString("0");

            if (value == "")
                return "";
            else
                return $"<td>{value}</td>";
        }
        private void Set_Factory()
        {

            string sql = "";
            DataTable dt_mach = new DataTable();
            string all_mach = "";

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
                    string sqlcmd = "select * from mach_group where area_name = '全廠'";
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
                    string sqlcmd = $"select * from mach_group where area_name = '{acc_power}'";
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
                Set_Table();
            else if (DropDownList_Group.Items.Count != 0 && DropDownList_Group.SelectedItem.Value.Split('^')[0] != "1")
            {
                Set_Factory();
                Set_Table();
                Response.Redirect(DropDownList_Group.SelectedItem.Value.Split('^')[0], "_blank", "");
                DropDownList_Group.Items.Clear();
            }
            else
                Response.Redirect("Override_Count.aspx");


        }

    }
}