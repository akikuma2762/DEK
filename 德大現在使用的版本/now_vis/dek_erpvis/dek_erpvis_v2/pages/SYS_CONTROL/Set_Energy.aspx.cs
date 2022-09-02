using dek_erpvis_v2.cls;
using dek_erpvis_v2.webservice;
using dekERP_dll.dekErp;
using Support;
using Support.DB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace dek_erpvis_v2.pages.SYS_CONTROL
{
    public partial class Set_Energy : System.Web.UI.Page
    {
        //-------------------------------------------------參數 OR 引用------------------------------------------------------------
        public string color = "";
        public string path = "";
        public StringBuilder th = new StringBuilder();
        public StringBuilder tr = new StringBuilder();
        string acc = "";
        DataTable dt_monthtotal = new DataTable();
        //----------------------------------------------------Event----------------------------------------------------------------------
        //載入事件
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                color = HtmlUtil.change_color(acc);

                if (HtmlUtil.Search_acc_Column(acc) == "Y")
                {
                    if (!IsPostBack)
                    {
                        if (WebUtils.GetAppSettings("Show_dek") == "1" || HtmlUtil.Search_acc_Column(acc, "Can_dek") == "Y")
                            PlaceHolder_hide.Visible = true;
                        else
                            PlaceHolder_hide.Visible = false;
                        MainProcess();
                    }
                }
                else
                    Response.Write("<script>alert('您無法瀏覽此頁面 請向該單位主管申請權限!');location.href='../index.aspx';</script>");
            }
            else
                Response.Redirect(myclass.logout_url);
        }

        //查詢事件
        protected void button_select_Click(object sender, EventArgs e)
        {
            MainProcess();
        }

        //修改事件
        protected void Button_Add_Click(object sender, EventArgs e)
        {
            string sqlcmd = "";
            DataTable dt = new DataTable();
            bool ok = false;
            switch (Label_Save.Text)
            {
                //20220823 立式&大圓盤分離,獨立連線
                case "sowon":
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
                    sqlcmd = $"select * from 工作站型態資料表 where 工作站編號 ='{TextBox_Number.Text}'";
                    dt = DataTableUtils.GetDataTable(sqlcmd);
                    if (HtmlUtil.Check_DataTable(dt))
                    {
                        DataRow row = dt.NewRow();
                        row["工作站編號"] = dt.Rows[0]["工作站編號"];
                        row["目標件數"] = TextBox_Qty.Text;
                        GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
                        ok = DataTableUtils.Update_DataRow("工作站型態資料表", $"工作站編號 ='{TextBox_Number.Text}'", row);
                    }
                    break;
                case "dek":
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);//20220811 大圓盤改連臥式資料庫
                    sqlcmd = $"select * from 工作站型態資料表 where 工作站編號 ='{TextBox_Number.Text}'";
                    dt = DataTableUtils.GetDataTable(sqlcmd);
                    if (HtmlUtil.Check_DataTable(dt))
                    {
                        DataRow row = dt.NewRow();
                        row["工作站編號"] = dt.Rows[0]["工作站編號"];
                        row["目標件數"] = TextBox_Qty.Text;
                        GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);//20220811 大圓盤改連臥式資料庫
                        ok = DataTableUtils.Update_DataRow("工作站型態資料表", $"工作站編號 ='{TextBox_Number.Text}'", row);
                    }
                    break;
                case "iTec":
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);
                    sqlcmd = $"select * from 工作站型態資料表 where 工作站編號 ='{TextBox_Number.Text}'";
                    dt = DataTableUtils.GetDataTable(sqlcmd);
                    if (HtmlUtil.Check_DataTable(dt))
                    {
                        DataRow row = dt.NewRow();
                        row["工作站編號"] = dt.Rows[0]["工作站編號"];
                        row["目標件數"] = TextBox_Qty.Text;
                        GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);
                        ok = DataTableUtils.Update_DataRow("工作站型態資料表", $"工作站編號 ='{TextBox_Number.Text}'", row);
                    }
                    break;
            }

            if (ok)
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", $"<script>alert('修改成功');</script>");
            else
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", $"<script>alert('修改失敗');</script>");
            MainProcess();

        }
        [WebMethod]
        public static object postData(string _data) {
            Dictionary<string, string> myData = new Dictionary<string, string>();
            DataTable Rp_data = JsonToDataTable.JsonStringToDataTable(_data);
            ShareFunction SF = new ShareFunction();
            Set_Energy Energy = new Set_Energy();
            ArrayList everyday = new ArrayList();
            ArrayList holiday = new ArrayList();
            ArrayList weekday = new ArrayList();
            ArrayList workstation_All = new ArrayList();
            DataTable dt_Holiday = new DataTable();
            DataTable dt = new DataTable();
            DateTime d_start;
            DateTime d_end;
            DataRow dt_Row;
            string sqlcmd = "";
            string factory = "";
            string date_str = "";
            string date_end = "";
            string mode = "";
            string month = "";
            string conditiion = "";
            object data = new { };
            object obj_date ;
            object workstation_info;

            foreach (DataRow dr in Rp_data.Rows)
            {
                foreach (DataColumn dc in Rp_data.Columns)
                {
                    string dc_Name = dc.ToString();
                    myData.Add(dc_Name, dr[dc_Name].ToString());
                }
            }
            factory = myData["Factory"];
            mode = myData["Mode"];
            obj_date = mode == "Month" ? SF.monthInterval(myData["Month"] + "01", myData["User_Acc"]): SF.monthInterval(myData["Day"], myData["User_Acc"]);
            

                if (myData["Workstation"] == "全部")
                {
                Energy.set_Connect(factory);
                sqlcmd = Energy.set_Sqlcmd(factory);
                dt = DataTableUtils.GetDataTable(sqlcmd);
                    foreach (DataRow row in dt.Rows)
                    {
                        conditiion += $" OR 工作站編號='{row["工作站編號"]}'";
                        workstation_info = new { 工作站編號 = row["工作站編號"], 工作站名稱 = row["工作站名稱"] };
                        workstation_All.Add(workstation_info);
                    }
                }
                else 
                {
                if (factory == "sowon")
                {
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
                }
                else if (factory == "iTec")
                {
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);
                }
                else if (factory == "dek")
                {
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);
                }
                conditiion += $" and 工作站編號='{myData["Workstation_Num"]}'";
                    workstation_info = new { 工作站編號 = myData["Workstation_Num"], 工作站名稱 = myData["Workstation"]};
                    workstation_All.Add(workstation_info);
                }

                
                date_str = obj_date.GetType().GetProperty("startDay").GetValue(obj_date).ToString();
                date_end = obj_date.GetType().GetProperty("endDay").GetValue(obj_date).ToString();
                d_start = DateTime.ParseExact(date_str, "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
                d_end = DateTime.ParseExact(date_end, "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces);

            if (mode == "Month")
            {
                sqlcmd = $"Select * From 人員工時表 where 日期>='{date_str}' and 日期<='{date_end}'{conditiion}";
                dt = DataTableUtils.GetDataTable(sqlcmd);
                month = d_end.Month.ToString();
                if (!HtmlUtil.Check_DataTable(dt))
                {
                    sqlcmd = $"Select * From [WorkTime_Holiday] where PK_Holiday >='{date_str}' and PK_Holiday<='{date_end}'";
                    dt_Holiday = DataTableUtils.GetDataTable(sqlcmd);
                    //紀錄假日日期
                    foreach (DataRow Row in dt_Holiday.Rows)
                    {
                        holiday.Add(Row["PK_Holiday"]);
                    }

                    //存取每日日期
                    int count = 0;
                    while (d_start.Date.CompareTo(d_end.Date) < 0)
                    {
                        d_start = d_start.AddDays(+count);
                        string a = d_start.ToString("yyyyMMdd");
                        count = 1;
                        everyday.Add(a);
                    }

                    //篩選平日日期
                    for (int i = 0; i < everyday.Count; i++)
                    {
                        if (holiday.IndexOf(everyday[i]) == -1)
                        {
                            weekday.Add(everyday[i]);
                        }
                    }

                    dt = DataTableUtils.DataTable_TableNoRow("人員工時表");
                    for (int j = 0; j < workstation_All.Count; j++) 
                    {
                        for (int i = 0; i < weekday.Count; i++)
                        {
                            dt_Row = dt.NewRow();
                            dt_Row["日期"] = weekday[i];
                            dt_Row["工作站編號"] = workstation_All[j].GetType().GetProperty("工作站編號").GetValue(workstation_All[j]).ToString();
                            dt_Row["工作站名稱"] = workstation_All[j].GetType().GetProperty("工作站名稱").GetValue(workstation_All[j]).ToString();
                            dt_Row["工作人數"] = myData["Working_People"];
                            dt_Row["工作時數"] = myData["Work_Time"];
                            dt_Row["所屬月份"] = month;
                            dt.Rows.Add(dt_Row);
                        }
                    }

                    int success = DataTableUtils.Insert_TableRows("人員工時表", dt);
                    if (success > 0)
                    {
                        data = new { status = "新增成功!" };

                    }
                    else
                    {
                        data = new { status = "新增失敗!" };
                    }

                }
                else 
                {
                    data = new { status = "日期重複,新增失敗!" };
                }
                   
            }
             else if (mode == "Day")
            {
                if (int.Parse(myData["Day"]) > int.Parse(date_str) && int.Parse(myData["Day"]) <= int.Parse(date_end))
                {
                    month = d_end.Month.ToString();
                }
                else 
                {
                    month = d_start.Month.ToString();
                }

                sqlcmd = $"Select * From 人員工時表 where 日期='{myData["Day"]}'{conditiion}";
                dt = DataTableUtils.GetDataTable(sqlcmd);
                if (!HtmlUtil.Check_DataTable(dt))
                {
                    dt = DataTableUtils.DataTable_TableNoRow("人員工時表");
                    for (int j = 0; j < workstation_All.Count; j++)
                    {
                        dt_Row = dt.NewRow();
                        dt_Row["日期"] = myData["Day"];
                        dt_Row["工作站編號"] = workstation_All[j].GetType().GetProperty("工作站編號").GetValue(workstation_All[j]).ToString();
                        dt_Row["工作站名稱"] = workstation_All[j].GetType().GetProperty("工作站名稱").GetValue(workstation_All[j]).ToString();
                        dt_Row["工作人數"] = myData["Working_People"];
                        dt_Row["工作時數"] = myData["Work_Time"];
                        dt_Row["所屬月份"] = month;
                        dt.Rows.Add(dt_Row);
                    }
                    int success = DataTableUtils.Insert_TableRows("人員工時表", dt);
                    if (success > 0)
                    {
                        data = new { status = "新增成功!" };

                    }
                    else
                    {
                        data = new { status = "新增失敗!" };
                    }
                }
                else {
                    data = new { status = "日期重複,新增失敗!" };
                }
                
            }

            
            return data;
        }
        public object getMonth_Rare(string acc) {
            object data = new { };
            string[] date = 德大機械.德大專用月份(acc).Split(',');
            string dt_str = date[0];
            string dt_end = date[1];
            string date_str = HtmlUtil.changetimeformat(date[0], "-");
            string date_end = HtmlUtil.changetimeformat(date[1], "-");
            data = new { dt_str = dt_str, dt_end= dt_end };
            return data;
        }
        //-----------------------------------------------------Function---------------------------------------------------------------------      

        private void set_Connect(string factory) {
            if (factory == "sowon")
            {
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
            }
            else if (factory == "iTec")
            {
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);
            }
            else if (factory == "dek")
            {
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);
            }

        }
        private string set_Sqlcmd(string factory)
        {
            string sqlcmd = "";

            if (factory == "sowon")
            {
                sqlcmd = "select 工作站編號, 工作站名稱 from 工作站型態資料表   where 工作站是否使用中 = '1'";
            }
            else if (factory == "iTec")
            {
                sqlcmd = "select 工作站編號, 工作站名稱 from 工作站型態資料表   where 工作站是否使用中 = '1' and 工作站編號 <> '11'";//大圓盤獨立顯示
            }
            else if (factory == "dek")
            {
                sqlcmd = "select 工作站編號, 工作站名稱 from 工作站型態資料表   where 工作站是否使用中='1'  and 工作站編號 = '11'";
            }

            return sqlcmd;
        }


        //需要執行的程式
        private void MainProcess()
        {
            set_DropDownlist();
            Get_MonthTotal();
            Set_Table();
        }

        private void set_DropDownlist()
        {
            string sqlcmd = "";
            if (dropdownlist_Factory.SelectedItem.Value.ToLower() == "sowon")
            {
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
                sqlcmd = "select 工作站編號, 工作站名稱 from 工作站型態資料表   where 工作站是否使用中 = '1'";
            }
            else if (dropdownlist_Factory.SelectedItem.Value == "iTec") 
            {
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);
                sqlcmd = "select 工作站編號, 工作站名稱 from 工作站型態資料表   where 工作站是否使用中 = '1' and 工作站編號 <> '11'";//大圓盤獨立顯示
            } 
            else if (dropdownlist_Factory.SelectedItem.Value.ToLower() == "dek") {
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);
                 sqlcmd = "select 工作站編號, 工作站名稱 from 工作站型態資料表   where 工作站是否使用中='1'  and 工作站編號 = '11'";
            }

             
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);

            if (HtmlUtil.Check_DataTable(dt))
            {
                    ListItem listItem = new ListItem();
                    Workstation.Items.Clear();
                Day_Workstation.Items.Clear();
                    Workstation.Items.Add("全部");
                Day_Workstation.Items.Add("全部");
                foreach (DataRow row in dt.Rows)
                    {
                        listItem = new ListItem(DataTableUtils.toString(row["工作站名稱"]), DataTableUtils.toString(row["工作站編號"]));
                        Workstation.Items.Add(listItem);
                    Day_Workstation.Items.Add(listItem);
                }
                
            }

        }


        private void Get_MonthTotal()
        {
            string sqlcmd = "";
            Label_Save.Text = dropdownlist_Factory.SelectedItem.Value;
            switch (dropdownlist_Factory.SelectedItem.Value)
            {
                case "sowon":
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
                    sqlcmd = "select 工作站編號 AS 編輯產能,工作站名稱,目標件數 AS 每日產能 from 工作站型態資料表 where 工作站是否使用中 = '1'";
                    dt_monthtotal = DataTableUtils.GetDataTable(sqlcmd);
                    break;
                case "dek":
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);//20220811 大圓盤改連臥式資料庫
                    sqlcmd = "select 工作站編號 AS 編輯產能,工作站名稱,目標件數 AS 每月產能 from 工作站型態資料表 where 工作站是否使用中 = '1' and 工作站編號 = '11'";
                    dt_monthtotal = DataTableUtils.GetDataTable(sqlcmd);
                    break;
                case "iTec":
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);
                    sqlcmd = "select 工作站編號 AS 編輯產能,工作站名稱,目標件數 AS 每月產能 from 工作站型態資料表 where 工作站是否使用中 = '1' and 工作站編號<>'11' ";
                    dt_monthtotal = DataTableUtils.GetDataTable(sqlcmd);
                    break;
            }
        }

        //設定表格
        private void Set_Table()
        {
            DataTable dt = dt_monthtotal;
            if (HtmlUtil.Check_DataTable(dt))
            {
                string titlename = "";
                dt.Columns.Add("工人工時編輯");
                dt.Columns["工人工時編輯"].SetOrdinal(1);
                th.Append(HtmlUtil.Set_Table_Title(dt, out titlename));
                tr.Append(HtmlUtil.Set_Table_Content(dt, titlename, Set_Energy_callback));
            }
            else
                HtmlUtil.NoData(out th, out tr);
        }

        private string Set_Energy_callback(DataRow row, string field_name)
        {
            string value = "";
            string capacity = "";
            capacity = dropdownlist_Factory.SelectedItem.Value == "sowon" ? row["每日產能"].ToString() : row["每月產能"].ToString();


            if (field_name == "編輯產能")
            {
                value = $"<td>" +
                            $"<u>" +
                                $"<a href=\"javascript:void(0)\" onclick=Set_Value(\"{row[field_name]}\",\"{capacity}\")  data-toggle=\"modal\" data-target=\"#exampleModal_information\">" +
                                    $"編輯" +
                                $"</a>" +
                            $"</u>" +
                        $"</td>";
            }
            else if (field_name == "工人工時編輯")
            {
                string url = $"Set_Month_WorkTime.aspx?key={WebUtils.UrlStringEncode($"workstation={row["工作站名稱"]},workstation_Num={row["編輯產能"]}")}";
                value = $"<td>" +
                                $"<u>" +
                                    $"<a href=\"{url}\">" +
                                        $"工人工時編輯" +
                                    $"</a>" +
                                $"</u>" +
                            $"</td>";
            }
            return value;
        }
    }
}