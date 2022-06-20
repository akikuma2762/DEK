using System;
using Support;
using dek_erpvis_v2.cls;
using System.Collections.Generic;
using System.Web.UI;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;

namespace dek_erpvis_v2.pages.dp_CNC
{
    public partial class Analysis : System.Web.UI.Page
    {
        public string color = "";
        public string timerange = "";
        public DateTime FirstDay = new DateTime();
        public DateTime LastDay = new DateTime();
        public string Type = "";
        public string dev_name = "";
        public string StatusInfo_th = "";
        public string StatusInfo_tr = "";
        public string js_ = "";
        public string js = "";
        public string OperRate_Str = "";            //折線圖
        public string Chart_Percent = "";           //圓餅圖   
        public string StatusRate_Str = "";          //圓餅圖      
        string acc = "", URL_NAME = "";
        public bool b_Page_Load = true;
        public List<string> ls_data = new List<string>();
        public bool is_ok = false;
        public string s_data = null;
        public DataTable dt_data = null;
        CNC_Web_Data Web_Data = new CNC_Web_Data();
        myclass myclass = new myclass();
        CNCUtils cNC_Class = new CNCUtils();
        List<string> Record_Machine = new List<string>();
        public string devName;
        //porcess
        protected void Page_Load(object sender, EventArgs e)
        {
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            FirstDay = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);  //單位：周
            LastDay = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + 7).AddSeconds(-1);
            if (LastDay > DateTime.Now) LastDay = DateTime.Now;

            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                URL_NAME = "Analysis_oper_rate";
                color = HtmlUtil.change_color(acc);

                if (textbox_st.Text == "" && textbox_ed.Text == "")
                {
                    int weeknow = Convert.ToInt32(DateTime.Now.DayOfWeek);
                    int daydiff = (-1) * weeknow;

                    //本周第一天
                    textbox_st.Text = DateTime.Now.AddDays(daydiff).ToString("yyyy-MM-dd");

                    daydiff = (7 - weeknow) - 1;
                    //本周最后一天
                    textbox_ed.Text = DateTime.Now.AddDays(daydiff).ToString("yyyy-MM-dd");

                }


                if (myclass.user_view_check(URL_NAME, acc) == true)
                {
                    if (!IsPostBack)
                        set_page_content();
                }
                else
                    Response.Write("<script>alert('您無此權限!');location.href='../index.aspx';</script>");
            }
            else
                Response.Redirect(myclass.logout_url);
        }
        private void set_page_content()
        {
            Read_Data();
            Get_MachType_List();
        }
        //fuction
        public void Read_Data(string MachGroup = "", string button_name = "")
        {
            if (button_name != "")
                Type = button_name;
            string data_first = "", oper_date = "";
            string[] date_week = { "0", "0", "0", "0", "0", "0", "0" };
            double[] operate_rate_week = { 0, 0, 0, 0, 0, 0, 0 };//紀錄預設當周每日稼動率
            double[] operate_time_week = { 0, 0, 0, 0, 0, 0, 0 };//紀錄預設當周每日總運轉工時
            double[] work_time_week = { 0, 0, 0, 0, 0, 0, 0 };//紀錄預設當周每日總工時
            double[] status_rate_day = { 0, 0, 0, 0, 0, 0, 0, 0 };
            double[] status_time = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            double[] status_time_table = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            double[] status_rate = { 0, 0, 0, 0, 0, 0, 0, 0 };
            List<string> Status_Rate = new List<string>();
            List<string> Status_Value = new List<string>();


            List<double> mach_work = new List<double>();
            List<double> mach_operate = new List<double>();
            List<string> mach_date = new List<string>();
            List<double> mach_rate = new List<double>();

            string condition = "";
            string factory = CNCUtils.Find_Group(HtmlUtil.Search_acc_Column(acc, "Belong_Factory"));
            if (factory == "")
                condition = " where area_name <> '測試區' ";
            else
                condition = $" where area_name = '{factory}' ";

            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);

            ls_data.Clear();
            if (b_Page_Load || DropDownList_MachType.Text == "--Select--")
            {

                GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);

                foreach (DataRow row in DataTableUtils.GetDataTable($"select mach_name from machine_info {condition} order by _id desc").Rows)
                    ls_data.Add(row.ItemArray[0].ToString());
            }
            else
            {
                if (MachGroup != "不存在")
                {
                    if (DropDownList_MachType.SelectedItem.Text == "全廠" && (DropDownList_MachGroup.SelectedItem.Text == "全廠設備" || DropDownList_MachGroup.SelectedItem.Text == "--Select--"))
                    {
                        GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                        DataTable ds = DataTableUtils.GetDataTable("select mach_name from machine_info where area_name <> '測試區'  order by _id desc");
                        if (HtmlUtil.Check_DataTable(ds))
                        {
                            foreach (DataRow dataRow in ds.Rows)
                                ls_data.Add(DataTableUtils.toString(dataRow["mach_name"]));
                        }
                    }
                    else
                        ls_data = DataTableUtils.GetDataTable("select mach_name from mach_group where group_name = '" + MachGroup + "' order by _id desc").Rows[0].ItemArray[0].ToString().Split(',').ToList();
                }
                else
                    ls_data = null;
            }
            //第一階段，過濾空值
            if (ls_data != null && ls_data.Count != 0)
            {
                for (int i = 0; i < ls_data.Count; i++)
                {
                    if (DataTableUtils.toString(ls_data[i]) == "")
                    {
                        ls_data.RemoveAt(i);
                        i--;
                    }
                }
            }

            //第二階段，過濾相同
            if (Record_Machine != null && ls_data != null && Record_Machine.Count != 0 && ls_data.Count != 0)
            {
                for (int i = 0; i < Record_Machine.Count; i++)
                {
                    if (ls_data.IndexOf(Record_Machine[i]) != -1)
                        ls_data.RemoveAt(ls_data.IndexOf(Record_Machine[i]));
                }
            }


            if (ls_data != null && ls_data.Count != 0)
            {
                ls_data = ls_data.Distinct().ToList();
                for (int iIndex = 0; iIndex < ls_data.Count; iIndex++)
                {
                    Record_Machine.Add(ls_data[iIndex]);
                    string data_sec = "";
                    FirstDay = DateTime.ParseExact(textbox_st.Text.Replace("-", "") + "000000", "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                    //最後一天若大於當天，則都不顯示
                    if (Int32.Parse(textbox_ed.Text.Replace("-", "")) > Int32.Parse(DateTime.Now.ToString("yyyyMMdd")))
                        LastDay = DateTime.ParseExact(DateTime.Now.ToString("yyyyMMddHHmmss"), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                    else
                        LastDay = DateTime.ParseExact(textbox_ed.Text.Replace("-", "") + "235959", "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);

                    Status_Value = Web_Data.Get_Operate_Rate(FirstDay.ToString("yyyyMMdd"), LastDay.ToString("yyyyMMdd"), ls_data[iIndex]);
                    for (int iIndex_1 = 0; iIndex_1 < Status_Value.Count; iIndex_1++)
                    {
                        for (int iIndex_3 = 0; iIndex_3 < status_time.Length; iIndex_3++)
                        {
                            status_time[iIndex_3] += DataTableUtils.toDouble(Status_Value[iIndex_1].Split(':')[1].Split(',')[iIndex_3]);
                            status_time_table[iIndex_3] += DataTableUtils.toDouble(Status_Value[iIndex_1].Split(':')[1].Split(',')[iIndex_3]);
                        }

                        if (b_Page_Load || DropDownList_Show.SelectedItem.Value == "0")//第一次
                        {
                            var Mach = ls_data[iIndex];

                            if (mach_operate.Count == Status_Value.Count)
                            {
                                mach_work[iIndex_1] += DataTableUtils.toDouble(Status_Value[iIndex_1].Split(':')[1].Split(',')[0]);
                                mach_operate[iIndex_1] += DataTableUtils.toDouble(Status_Value[iIndex_1].Split(':')[1].Split(',')[4]);
                            }
                            else
                            {

                                mach_work.Add(DataTableUtils.toDouble(Status_Value[iIndex_1].Split(':')[1].Split(',')[0]));
                                mach_operate.Add(DataTableUtils.toDouble(Status_Value[iIndex_1].Split(':')[1].Split(',')[4]));
                                mach_date.Add(Status_Value[iIndex_1].Split(':')[0].Substring(6, 2) + "號");
                            }


                            if (iIndex == ls_data.Count - 1 && iIndex_1 == Status_Value.Count - 1)
                            {
                                for (int iIndex_2 = 0; iIndex_2 < Status_Value.Count; iIndex_2++)
                                {
                                    mach_rate.Add(cNC_Class.Math_Round(mach_operate[iIndex_2], mach_work[iIndex_2], 2) * 100);
                                    if (double.IsNaN(mach_rate[iIndex_2])) mach_rate[iIndex_2] = 0;//避免=非數值
                                    data_first += "{ label:'" + mach_date[iIndex_2] + "' , y:" + mach_rate[iIndex_2] + " },";
                                }
                                if (MachGroup == "")
                                    MachGroup = "全廠";
                                OperRate_Str += "{type: 'line',showInLegend: true,name: '" + MachGroup + " - 平均稼動率',indexLabel: '{y}',dataPoints: [" + data_first + "]},";  //畫第一次折線圖                                
                            }

                            //work_time_week[iIndex_1] += DataTableUtils.toDouble(Status_Value[iIndex_1].Split(':')[1].Split(',')[0]);
                            //operate_time_week[iIndex_1] += DataTableUtils.toDouble(Status_Value[iIndex_1].Split(':')[1].Split(',')[4]);
                            //date_week[iIndex_1] = Status_Value[iIndex_1].Split(':')[0].Substring(6, 2) + "號";
                            //if (iIndex == ls_data.Count - 1 && iIndex_1 == Status_Value.Count - 1)
                            //{
                            //    for (int iIndex_2 = 0; iIndex_2 < Status_Value.Count; iIndex_2++)
                            //    {
                            //        operate_rate_week[iIndex_2] = cNC_Class.Math_Round(operate_time_week[iIndex_2], work_time_week[iIndex_2], 2) * 100;
                            //        if (double.IsNaN(operate_rate_week[iIndex_2])) operate_rate_week[iIndex_2] = 0;//避免=非數值
                            //        data_first += "{ label:'" + date_week[iIndex_2] + "' , y:" + operate_rate_week[iIndex_2] + " },";
                            //    }
                            //    OperRate_Str += "{type: 'line',showInLegend: true,name: '平均稼動率',indexLabel: '{y}',dataPoints: [" + data_first + "]},";  //畫第一次折線圖                                
                            //}

                            if (iIndex_1 == Status_Value.Count - 1)
                            {
                                status_rate_day[0] = cNC_Class.Math_Round(status_time_table[1], status_time_table[0], 2) * 100;
                                status_rate_day[1] = cNC_Class.Math_Round(status_time_table[2], status_time_table[0], 2) * 100;
                                status_rate_day[2] = cNC_Class.Math_Round(status_time_table[3], status_time_table[0], 2) * 100;
                                status_rate_day[3] = cNC_Class.Math_Round(status_time_table[4], status_time_table[0], 2) * 100;
                                status_rate_day[4] = cNC_Class.Math_Round(status_time_table[5], status_time_table[0], 2) * 100;
                                status_rate_day[5] = cNC_Class.Math_Round(status_time_table[6], status_time_table[0], 2) * 100;
                                status_rate_day[6] = cNC_Class.Math_Round(status_time_table[7], status_time_table[0], 2) * 100;
                                status_rate_day[7] = cNC_Class.Math_Round(status_time_table[8], status_time_table[0], 2) * 100;
                                Status_Rate.Add($"Dev_Name:{ls_data[iIndex]},DISCONNECT:{status_rate_day[0]},STOP:{status_rate_day[1]},ALARM:{status_rate_day[2]},OPERATE:{status_rate_day[3]},EMERGEMCY:{status_rate_day[4]},SUSPEND:{status_rate_day[5]},MANUAL:{status_rate_day[6]},WARMUP:{status_rate_day[7]},SHUTDOWN:0");
                                for (int iIndex_5 = 0; iIndex_5 < status_time_table.Length; iIndex_5++)
                                    status_time_table[iIndex_5] = 0;
                            }

                        }
                        else
                        {
                            double work_time_now = DataTableUtils.toDouble(Status_Value[iIndex_1].Split(':')[1].Split(',')[0]);
                            double oper_time_now = DataTableUtils.toDouble(Status_Value[iIndex_1].Split(':')[1].Split(',')[4]);
                            oper_date = Status_Value[iIndex_1].Split(':')[0].Substring(6, 2) + "號";

                            status_rate_day[3] = cNC_Class.Math_Round(oper_time_now, work_time_now, 2) * 100;//operate_time
                            if (double.IsNaN(status_rate_day[3])) status_rate_day[3] = 0;//避免=非數值



                            data_sec += "{ label:'" + oper_date + "' , y:" + status_rate_day[3] + " },";  //畫表(單獨線)


                            if (iIndex_1 == Status_Value.Count - 1)
                            {
                                ls_data[iIndex] = CNCUtils.MachName_translation(ls_data[iIndex]);

                                OperRate_Str += "{type: 'line',showInLegend: true,name: '" + ls_data[iIndex] + "',indexLabel: '{y}',dataPoints: [" + data_sec + "]},\n";//畫折線
                                                                                                                                                                        //算 Table    
                                status_rate_day[0] = cNC_Class.Math_Round(status_time_table[1], status_time_table[0], 2) * 100;
                                status_rate_day[1] = cNC_Class.Math_Round(status_time_table[2], status_time_table[0], 2) * 100;
                                status_rate_day[2] = cNC_Class.Math_Round(status_time_table[3], status_time_table[0], 2) * 100;
                                status_rate_day[3] = cNC_Class.Math_Round(status_time_table[4], status_time_table[0], 2) * 100;
                                status_rate_day[4] = cNC_Class.Math_Round(status_time_table[5], status_time_table[0], 2) * 100;
                                status_rate_day[5] = cNC_Class.Math_Round(status_time_table[6], status_time_table[0], 2) * 100;
                                status_rate_day[6] = cNC_Class.Math_Round(status_time_table[7], status_time_table[0], 2) * 100;
                                status_rate_day[7] = cNC_Class.Math_Round(status_time_table[8], status_time_table[0], 2) * 100;
                                Status_Rate.Add($"Dev_Name:{ls_data[iIndex]},DISCONNECT:{status_rate_day[0]},STOP:{status_rate_day[1]},ALARM:{status_rate_day[2]},OPERATE:{status_rate_day[3]},EMERGEMCY:{status_rate_day[4]},SUSPEND:{status_rate_day[5]},MANUAL:{status_rate_day[6]},WARMUP:{status_rate_day[7]},SHUTDOWN:0");
                                for (int iIndex_4 = 0; iIndex_4 < status_time_table.Length; iIndex_4++)
                                    status_time_table[iIndex_4] = 0;
                                //算 Table 
                            }
                        }
                    }
                }
                Status_Table(Status_Rate);  //表格資料
                status_rate[0] = cNC_Class.Math_Round(status_time[1], status_time[0], 2) * 100;
                status_rate[1] = cNC_Class.Math_Round(status_time[2], status_time[0], 2) * 100;
                status_rate[2] = cNC_Class.Math_Round(status_time[3], status_time[0], 2) * 100;
                status_rate[3] = cNC_Class.Math_Round(status_time[4], status_time[0], 2) * 100;
                status_rate[4] = cNC_Class.Math_Round(status_time[5], status_time[0], 2) * 100;
                status_rate[5] = cNC_Class.Math_Round(status_time[6], status_time[0], 2) * 100;
                status_rate[6] = cNC_Class.Math_Round(status_time[7], status_time[0], 2) * 100;
                status_rate[7] = cNC_Class.Math_Round(status_time[8], status_time[0], 2) * 100;
                StatusRate_Str = $"Device Name: ,DISCONNECT:{status_rate[0]},STOP:{status_rate[1]},ALARM:{status_rate[2] },OPERATE:{status_rate[3]},EMERGENCY:{status_rate[4]},SUSPEND:{status_rate[5]},MANUAL:{status_rate[6]},WARMUP:{status_rate[7]},SHUTDOWN:0";//圓餅圖
                string date_s = FirstDay.ToString("yyyy/MM/dd");
                string date_e = LastDay.ToString("yyyy/MM/dd");
                timerange = DateTime.Parse(date_s).ToString("yyyy/MM/dd") + " ~ " + DateTime.Parse(date_e).ToString("yyyy/MM/dd");
            }

        }
        private void Status_Table(List<string> Status_Rate)//表格資料
        {
            if (Status_Rate.Count != 0)
            {
                StatusInfo_th = "<th>設備名稱</th>\n" +
                "<th>運轉(%)</th>\n" +
                "<th>待機(%)</th>\n" +
                "<th>警報(%)</th>\n" +
                "<th>離線(%)</th>\n" +
                "<th>警告(%)</th>\n" +
                "<th>暫停(%)</th>\n" +
                "<th>手動(%)</th>\n" +
                "<th>暖機(%)</th>\n";
                string devicename = "";
                foreach (string Status_List in Status_Rate)
                {
                    if (Status_List != "--")
                    {
                        devicename = CNCUtils.MachName_translation(Status_List.Split(',')[0].Split(':')[1]);
                        StatusInfo_tr += "<tr>";
                        StatusInfo_tr += $"<td>{devicename}</td>";//devname
                        StatusInfo_tr += $"<td>{Status_List.Split(',')[4].Split(':')[1]}</td>";//operate
                        StatusInfo_tr += $"<td>{Status_List.Split(',')[2].Split(':')[1]}</td>";//stop
                        StatusInfo_tr += $"<td>{Status_List.Split(',')[3].Split(':')[1]}</td>";//emergency
                        StatusInfo_tr += $"<td>{Status_List.Split(',')[1].Split(':')[1]}</td>";//disconnect
                        StatusInfo_tr += $"<td>{Status_List.Split(',')[5].Split(':')[1]}</td>";//disconnect
                        StatusInfo_tr += $"<td>{Status_List.Split(',')[6].Split(':')[1]}</td>";//disconnect
                        StatusInfo_tr += $"<td>{Status_List.Split(',')[7].Split(':')[1]}</td>";//disconnect
                        StatusInfo_tr += $"<td>{Status_List.Split(',')[8].Split(':')[1]}</td>";//disconnect
                        StatusInfo_tr += "</tr>";
                    }
                }
            }
            else
                HtmlUtil.NoData(out StatusInfo_th, out StatusInfo_tr);
        }
        private void Get_MachType_List()//取type list
        {
            string sql = "";
            DataTable dt_mach = new DataTable();
            string all_mach = "";

            ListItem listItem = new ListItem();
            List<string> machlist = new List<string>();


            DropDownList_MachType.Items.Clear();
            dt_data = DataTableUtils.GetDataTable("select distinct area_name from mach_group where area_name <> '全廠' and area_name <> '測試區'  ");
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
        public string get_MachList_html(string dev_name)
        {
            string html_ = "'<div class=\"col-md-12 col-sm-12 col-xs-12\">" +
                                "<div id = \"" + dev_name + "\" class=\"mach_list_Pei\">" +
                                "</div>" +
                           "</div>';";//-23
            return js_ += "document.getElementById('mach_list').innerHTML += " + html_ + "";
        }
        public void get_javascrtpt_pie(string dev_status)
        {
            devName = dev_status.Split(':')[1].Split(',')[0];    //MTLINKi設備命名限制如宣告命名方式
            js += "var " + devName + "_ch" + " = new CanvasJS.Chart(\"" + devName + "\", {" +
           "animationEnabled: true," +
           "title:{text: \"" + "狀態比例統計" + "\" ,   fontFamily: 'NotoSans',fontWeight: 'bolder',textAlign: \"center\",     fontSize: 37,}," +
           "subtitles: [{text: '" + timerange + "',fontFamily: 'NotoSans',fontWeight: 'bolder',textAlign: \"center\",fontSize: 15,}]," +
           "legend: {  fontSize: 20,  cursor: \"pointer\", fontFamily: \"NotoSans\",fontWeight: \"bolder\",itemclick: toggleDataSeries,}," +
           "data: [{ showInLegend: true,type: \"pie\",toolTipContent: \"{name}: <strong>{y}%</strong>\",indexLabel: \"{name} - {y}%\",dataPoints: [" + get_dataponts(dev_status) + "]}]" +
           "}); " + devName + "_ch.render();";

        }
        private string get_dataponts(string dev_status)
        {
            Chart_Percent = "";
            for (int iIndex = 1; iIndex < dev_status.Split(',').Length - 1; iIndex++)
            {
                string dev_status_title = dev_status.Split(',')[iIndex].Split(':')[0];
                if (dev_status_title == "SHUTDOWN") dev_status_title = "NONE";//關機先用none取代
                string dev_value = dev_status.Split(',')[iIndex].Split(':')[1];
                Chart_Percent += "{ y:" + dev_value + " , name: \"" + cNC_Class.mach_status_EN2CH(dev_status_title) + "\",color: '" + cNC_Class.mach_status_Color(dev_status_title) + "' },";
            }
            return Chart_Percent;
        }
        //event      
        protected void Select_MachGroupClick(object sender, EventArgs e)    //執行運算
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
                set_page_content();
                Response.Redirect(DropDownList_MachGroup.SelectedItem.Value.Split('^')[0], "_blank", "");
                DropDownList_MachGroup.Items.Clear();
            }


            else if (DropDownList_MachType.SelectedItem.Text != "--Select--")// && DropDownList_MachGroup.SelectedItem.Text != "--Select--")
            {
                Record_Machine.Clear();
                b_Page_Load = false;
                string sqlcmd = "";
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                sqlcmd = $"select * from mach_group where area_name = '{DropDownList_MachType.SelectedItem.Text}' ";
                DataTable dt = DataTableUtils.GetDataTable(sqlcmd);

                if (HtmlUtil.Check_DataTable(dt))
                {
                    if (DropDownList_MachGroup.SelectedItem.Text == "--Select--")
                    {
                        if (HtmlUtil.Check_DataTable(dt))
                        {
                            foreach (DataRow row in dt.Rows)
                                Read_Data(DataTableUtils.toString(row["group_name"]));
                        }
                        else
                            Read_Data("不存在");

                    }
                    else
                        Read_Data(DropDownList_MachGroup.SelectedItem.Text);
                }
            }
            else
                Response.Redirect("Analysis_oper_rate.aspx");
        }

    }
}