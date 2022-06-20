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
    public partial class Worktime_Total : System.Web.UI.Page
    {
        public string color = "";
        string condition = "";
        public string tr = "";
        public string order = "";
        public string th = "";
        string acc = "";
        德大機械 德大機械 = new 德大機械();
        myclass myclass = new myclass();
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                color = HtmlUtil.change_color(acc);
                if (myclass.user_view_check("Worktime_Total", acc) || true)
                {
                    if (!IsPostBack)
                    {
                        string[] s = 德大機械.德大專用月份(acc).Split(',');
                        TextBox_Start.Text = HtmlUtil.changetimeformat(s[0], "-");
                        TextBox_End.Text = HtmlUtil.changetimeformat(s[1], "-");
                        Set_Dropdownlist();
                        Set_Html_Table();
                    }

                }
                else
                    Response.Write("<script>alert('您無法瀏覽此頁面 請向該單位主管申請權限!');location.href='../index.aspx';</script>");
            }
            else
                Response.Write("<script>alert('您無法瀏覽此頁面 請向該單位主管申請權限!');location.href='../index.aspx';</script>");
        }
        private void Set_Dropdownlist()
        {
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            string sql = "";
            DataTable dt_mach = new DataTable();
            string all_mach = "";

            ListItem listItem = new ListItem();
            List<string> machlist = new List<string>();


            DropDownList_MachType.Items.Clear();
            DataTable dt_data = DataTableUtils.GetDataTable("select distinct area_name from mach_group where area_name <> '全廠' and area_name <> '測試區'  ");
            if (HtmlUtil.Check_DataTable(dt_data))
            {
                DropDownList_MachType.Items.Add("--Select--");
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
                        DropDownList_MachType.Items.Add(listItem);
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
                            DropDownList_MachType.Items.Add(listItem);
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
                        DropDownList_MachType.Items.Add(listItem);
                    }
                }
            }

        }
        private void Set_Html_Table()
        {
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            string sqlcmd = $"SELECT  mach_show_name 機台名稱, work_staff 加工人員, manu_id 製令單號, custom_name 客戶名稱, product_number 料號, product_name 產品名稱, craft_name 工藝名稱, now_time 進站時間,start_time 開始時間,end_time 結束時間 FROM record_worktime, machine_info WHERE  machine_info.area_name <> '測試區' and workman_status = '入站'  {condition}   and substring(now_time,1,8) >={TextBox_Start.Text.Replace("-", "")}     and substring(now_time,1,8) <= {TextBox_End.Text.Replace("-", "")} AND work_staff IS NOT NULL AND record_worktime.mach_name = machine_info.mach_name";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);

            if (HtmlUtil.Check_DataTable(dt))
            {
                dt.Columns["進站時間"].ColumnName = "進站時間(開始時間)";
                dt.Columns.Add("出站時間(結束時間)");
                dt.Columns.Add("生產時間");
                dt.Columns.Add("完工數量");
                dt.Columns.Add("時間/PCS");

                //填入資料區
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);

                sqlcmd = $"SELECT  mach_show_name 機台名稱, now_time 出站時間, report_qty 完工數量, start_time 開始時間, end_time 結束時間 FROM record_worktime, machine_info WHERE machine_info.area_name <> '測試區' AND workman_status = '出站' AND work_staff IS NOT NULL AND record_worktime.mach_name = machine_info.mach_name";
                DataTable ds = DataTableUtils.GetDataTable(sqlcmd);


                if (HtmlUtil.Check_DataTable(ds))
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        sqlcmd = $" 機台名稱='{row["機台名稱"]}' and 開始時間='{row["開始時間"]}' and 結束時間='{row["結束時間"]}' and 出站時間>'{row["進站時間(開始時間)"]}'";
                        DataRow[] rows = ds.Select(sqlcmd);
                        //先填入出站時間
                        if (rows != null && rows.Length > 0)
                        {
                            row["出站時間(結束時間)"] = rows[0]["出站時間"].ToString();
                            row["完工數量"] = rows[0]["完工數量"].ToString();
                        }
                    }
                }

                dt.Columns.Remove("開始時間");
                dt.Columns.Remove("結束時間");

                string title = "";
                th = HtmlUtil.Set_Table_Title(dt, out title);
                tr = HtmlUtil.Set_Table_Content(dt, title, Worktime_TotalListback);
                order = ", [7, \"desc\"] ";
            }
            else
                HtmlUtil.NoData(out th, out tr);
        }
        private string Worktime_TotalListback(DataRow row, string fieldname)
        {

            if (fieldname == "進站時間(開始時間)" || fieldname == "出站時間(結束時間)")
            {
                if (DataTableUtils.toString(row[fieldname]) != "")
                    return $"<td>{HtmlUtil.StrToDate(DataTableUtils.toString(row[fieldname])):yyyy/MM/dd HH:mm:ss}</td>";
                else return "";
            }
            else if (fieldname == "生產時間")
            {
                if (DataTableUtils.toString(row["出站時間(結束時間)"]) != "" && DataTableUtils.toString(row["進站時間(開始時間)"]) != "")
                {
                    TimeSpan span = HtmlUtil.StrToDate(DataTableUtils.toString(row["出站時間(結束時間)"])) - HtmlUtil.StrToDate(DataTableUtils.toString(row["進站時間(開始時間)"]));                    
                    return $"<td>{span.Days}天{span.Hours}小時{span.Minutes}分{span.Seconds}秒</td>";
                }
                else return "";
            }
            else if (fieldname == "時間/PCS")
            {
                if (DataTableUtils.toString(row["出站時間(結束時間)"]) != "" && DataTableUtils.toString(row["進站時間(開始時間)"]) != "")
                {
                    TimeSpan span = HtmlUtil.StrToDate(DataTableUtils.toString(row["出站時間(結束時間)"])) - HtmlUtil.StrToDate(DataTableUtils.toString(row["進站時間(開始時間)"]));

                    if (DataTableUtils.toDouble(DataTableUtils.toString(row["完工數量"])) != 0)
                    {
                        double spantime = Math.Round(span.TotalSeconds / DataTableUtils.toDouble(DataTableUtils.toString(row["完工數量"])), 1, MidpointRounding.AwayFromZero);

                        span = TimeSpan.FromSeconds(spantime);
                        return $"<td>{span.Days}天{span.Hours}小時{span.Minutes}分{span.Seconds}秒</td>";

                    }
                    else
                        return "<td>0</td>";
                }
                else return "";
            }
            else if (fieldname == "除外時間(分)")
            {
                List<string> list = new List<string>(DataTableUtils.toString(row["除外時間(分)"]).Split(','));
                Double ex_time = 0;
                for (int i = 0; i < list.Count - 1; i++)
                {
                    if (list[i + 1] != "" && list[i] != "")
                    {
                        TimeSpan span = HtmlUtil.StrToDate(list[i + 1]) - HtmlUtil.StrToDate(list[i]);
                        ex_time += span.TotalMinutes;
                        i += 1;
                    }
                }
                return $"<td>{ex_time:0.00}</td>";
            }

            else
                return "";
        }
        protected void button_select_Click(object sender, EventArgs e)
        {

            DropDownList_MachType.SelectedIndex = DropDownList_MachType.Items.IndexOf(DropDownList_MachType.Items.FindByValue(TextBox_MachTypeValue.Text));
            DropDownList_MachGroup.Items.Clear();
            List<string> list = new List<string>(TextBox_MachTypeValue.Text.Split(','));
            for (int i = 0; i < list.Count - 1; i++)
            {
                ListItem listItem = new ListItem(list[i], list[i + 1]);
                DropDownList_MachGroup.Items.Add(listItem);
                i++;
            }
            DropDownList_MachGroup.SelectedIndex = DropDownList_MachGroup.Items.IndexOf(DropDownList_MachGroup.Items.FindByText(TextBox_MachGroupText.Text));


            if (DropDownList_MachGroup.Items.Count != 0 && DropDownList_MachGroup.SelectedItem.Value.Split('^')[0] != "1")
            {
                Set_Html_Table();
                Response.Redirect(DropDownList_MachGroup.SelectedItem.Value.Split('^')[0], "_blank", "");
                DropDownList_MachGroup.Items.Clear();
            }

            else if (DropDownList_MachType.SelectedItem.Text != "--Select--")
            {
                condition = "";
                condition = " and (";
                list = new List<string>(TextBox_Machines.Text.Split(','));
                for (int i = 0; i < list.Count - 1; i++)
                {
                    if (i == 0)
                        condition += $" mach_show_name = '{list[i]}' ";
                    else
                        condition += $" or mach_show_name = '{list[i]}' ";
                }
                condition += " ) ";


                Set_Html_Table();
            }
            else
                Response.Redirect("Worktime_Total.aspx");



        }
    }
}
