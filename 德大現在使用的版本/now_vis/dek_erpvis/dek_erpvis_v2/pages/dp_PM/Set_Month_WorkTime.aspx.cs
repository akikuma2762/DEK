using dek_erpvis_v2.cls;
using Support;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace dek_erpvis_v2.pages.dp_PM
{
    public partial class Set_Month_WorkTime : System.Web.UI.Page
    {
        public string color = "";
        string acc = "";
        string condition = "";
        public string date_str = "";
        public string date_end = "";
        public string request_WorkStation = "";
        public string request_WorkStation_Num = "";
        Dictionary<string,string> obj_date =new Dictionary<string, string>();
        public string th = "";
        public string tr = "";
        ShareFunction SF = new ShareFunction();
        DataTable user_Acc = new DataTable();
        DataTable dataTable = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                color = HtmlUtil.change_color(acc);
                DataTableUtils.Conn_String = myclass.GetConnByDekVisErp;
                user_Acc = DataTableUtils.DataTable_GetTable($"SELECT * FROM SYSTEM_PARAMETER WHERE USER_ACC='{acc}'");
                    if (!IsPostBack)
                    {
                        string[] date = 德大機械.德大專用月份(acc).Split(',');
                        date_str = HtmlUtil.changetimeformat(date[0], "-");
                        date_end = HtmlUtil.changetimeformat(date[1], "-");
                        Select_Month.Text = date_end.Substring(0, date_end.Length - 3);
                        GetCondi();
                        set_DropDownlist();
                        Get_DataTable();
                        set_Table();
                    }
            }
            else
                Response.Redirect(myclass.logout_url);
        }

        //接收AJAX資料
        [WebMethod]
        public static object postData(string _data)
        {
            Dictionary<string, string> myData = new Dictionary<string, string>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            Dictionary<string, object> obj_data = new Dictionary<string, object>();
            DataTable Rp_data = JsonToDataTable.JsonStringToDataTable(_data);
            Set_Month_WorkTime SMW = new Set_Month_WorkTime();
            //object data = new { };
            
            foreach (DataRow dr in Rp_data.Rows)
            {
                foreach (DataColumn dc in Rp_data.Columns)
                {
                    string dc_Name = dc.ToString();
                    myData.Add(dc_Name, dr[dc_Name].ToString());
                }
            }

            switch (myData["click_Type"]) {
                case "Insert":
                    data = SMW.insert(myData);
                    obj_data.Add("data", data);
                    break;
                case "Update":
                    data = SMW.update(myData);
                    obj_data.Add("data", data);
                    break;
                case "Delete":
                    data = SMW.delete(myData);
                    obj_data.Add("data", data);
                    break;
            }
            return obj_data;
        }
        //計算月份區間
        public object getMonth_Rare(string acc)
        {
            object data = new { };
            string[] date = 德大機械.德大專用月份(acc).Split(',');
            string dt_str = date[0];
            string dt_end = date[1];
            string date_str = HtmlUtil.changetimeformat(date[0], "-");
            string date_end = HtmlUtil.changetimeformat(date[1], "-");
            data = new { dt_str = dt_str, dt_end = dt_end };
            return data;
        }
        public Dictionary<string, string> insert(Dictionary<string,string> myData) {
            ShareFunction SF = new ShareFunction();
            ArrayList everyday = new ArrayList();
            ArrayList holiday = new ArrayList();
            ArrayList weekday = new ArrayList();
            ArrayList workstation_All = new ArrayList();
            DataTable dt = new DataTable();
            DateTime d_start;
            DateTime d_end;
            DataRow dt_Row;
            string sqlcmd = "";
            string month = "";
            string conditiion = "";
            //object data = new { };
            Dictionary<string, string> obj_date = new Dictionary<string, string>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            object workstation_info;
            string factory = myData["Factory"];
            string insert_Type = myData["Insert_Type"];
            string date = myData["Day"];
            //計算日期區間
            obj_date = insert_Type == "Month" ? SF.monthInterval(myData["Month"] + "01", myData["User_Acc"]) : SF.monthInterval(date, myData["User_Acc"]);
          
            if (myData["WorkStation_Num"] == "全部")
            {
                set_Connect(factory);
                sqlcmd = set_Sqlcmd(factory);
                dt = DataTableUtils.GetDataTable(sqlcmd);
                foreach (DataRow row in dt.Rows)
                {
                    conditiion += $"工作站編號='{row["工作站編號"]}' OR ";
                    workstation_info = new { 工作站編號 = row["工作站編號"], 工作站名稱 = row["工作站名稱"] };
                    workstation_All.Add(workstation_info);
                }
                conditiion = conditiion.Substring(0, conditiion.Length - 4);
            }
            else
            {
                set_Connect(factory);
                conditiion += $"工作站編號='{myData["WorkStation_Num"]}'";
                workstation_info = new { 工作站編號 = myData["WorkStation_Num"], 工作站名稱 = myData["WorkStation_Name"] };
                workstation_All.Add(workstation_info);
            }


            string date_str = obj_date["startDay"];
            string date_end = obj_date["endDay"];
            d_start = DateTime.ParseExact(date_str, "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
            d_end = DateTime.ParseExact(date_end, "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces);

            if (insert_Type == "Month")
            {
                sqlcmd = $"Select * From 人員工時表 where 日期>='{date_str}' and 日期<='{date_end}' and ({conditiion})";
                dt = DataTableUtils.GetDataTable(sqlcmd);
                month = d_end.Month.ToString();
                if (!HtmlUtil.Check_DataTable(dt))
                {
                    sqlcmd = $"Select * From [WorkTime_Holiday] where PK_Holiday >='{date_str}' and PK_Holiday<='{date_end}'";
                    DataTable  dt_Holiday = DataTableUtils.GetDataTable(sqlcmd);
                    //紀錄假日日期
                    foreach (DataRow Row in dt_Holiday.Rows)
                    {
                        holiday.Add(Row["PK_Holiday"]);
                    }

                    //存取每日日期
                    //int count = 0;
                    while (d_start.Date.CompareTo(d_end.Date) <= 0)
                    {
                        string date_string = d_start.ToString("yyyyMMdd");
                        d_start = d_start.AddDays(+1);
                        everyday.Add(date_string);
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
                            dt_Row["應到人數"] = myData["Excepted_People"];
                            dt_Row["實到人數"] = myData["Working_People"];
                            dt_Row["工作時數"] = myData["Work_Time"];
                            dt_Row["所屬月份"] = month;
                            dt.Rows.Add(dt_Row);
                        }
                    }

                    int success = DataTableUtils.Insert_TableRows("人員工時表", dt);
                    if (success > 0)
                    {
                        data = set_Table(myData, "新增成功");

                    }
                    else
                    {
                        //data = new { status = "新增失敗!" };
                        data.Add("status", "新增失敗");
                    }

                }
                else
                {
                    //data = new { status = "日期重複,新增失敗!" };
                    data.Add("status", "日期重複,新增失敗!");
                }

            }
            else if (insert_Type == "Day")
            {
                //判斷所屬月份
                if (int.Parse(date) >= int.Parse(date_str) && int.Parse(date) <= int.Parse(date_end))
                {
                    month = d_end.Month.ToString();
                }
                else if (int.Parse(date) > int.Parse(date_end)) 
                {
                    month = d_end.AddMonths(1).Month.ToString();
                }
                else
                {
                    month = d_start.AddMonths(1).Month.ToString();
                }

                sqlcmd = $"Select * From 人員工時表 where 日期='{date}' and ({conditiion})";
                dt = DataTableUtils.GetDataTable(sqlcmd);
                if (!HtmlUtil.Check_DataTable(dt))
                {
                    dt = DataTableUtils.DataTable_TableNoRow("人員工時表");
                    for (int j = 0; j < workstation_All.Count; j++)
                    {
                        dt_Row = dt.NewRow();
                        dt_Row["日期"] = date;
                        dt_Row["工作站編號"] = workstation_All[j].GetType().GetProperty("工作站編號").GetValue(workstation_All[j]).ToString();
                        dt_Row["工作站名稱"] = workstation_All[j].GetType().GetProperty("工作站名稱").GetValue(workstation_All[j]).ToString();
                        dt_Row["應到人數"] = myData["Excepted_People"];
                        dt_Row["實到人數"] = myData["Working_People"];
                        dt_Row["工作時數"] = myData["Work_Time"];
                        dt_Row["所屬月份"] = month;
                        dt.Rows.Add(dt_Row);
                    }
                    int success = DataTableUtils.Insert_TableRows("人員工時表", dt);
                    if (success > 0)
                    {
                        data = set_Table(myData, "新增成功");
                    }
                    else
                    {
                        data.Add("status", "新增失敗");
                    }
                }
                else
                {
                    data.Add("status", "日期重複,新增失敗!");
                }

            }
            return data;
        }
        public Dictionary<string, string> delete(Dictionary<string, string> myData)
        {
            //object data = new { };
            Dictionary<string, string> data = new Dictionary<string, string>();
            string factory = myData["Factory"];
            string workstation_Num = myData["WorkStation_Num"];
            string date = myData["Day"];
            set_Connect(factory);
            string sqlcmd = $"Select * From 人員工時表 Where 工作站編號='{workstation_Num}' and 日期='{date}'";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
            if (HtmlUtil.Check_DataTable(dt)) 
            {
                if (DataTableUtils.Delete_Record("人員工時表", $"工作站編號='{workstation_Num}' and 日期='{date}'"))
                {
                    data= set_Table(myData,"刪除成功");
                }
                else 
                {
                    data.Add("status", "刪除失敗");
                }
            }
            else
            {
                data.Add("status", "刪除失敗");
            }


            return data;
        }

        public Dictionary<string, string> update(Dictionary<string, string> myData)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            string factory = myData["Factory"];
            string workstation_Num = myData["WorkStation_Num"];
            string workstation_Name = myData["WorkStation_Name"];
            string excepted_People = myData["Excepted_People"];
            string working_People = myData["Working_People"];
            string work_Time = myData["Work_Time"];
            string date = myData["Original_Date"];
            string new_date = myData["New_Date"];
            string sqlcmd = "";
            DataTable dt;
            object workstation_info;
            ArrayList workstation_All = new ArrayList();
            string conditiion = "";
            if (myData["WorkStation_Num"] == "全部")
            {
                set_Connect(factory);
                sqlcmd = set_Sqlcmd(factory);
                dt = DataTableUtils.GetDataTable(sqlcmd);
                foreach (DataRow row in dt.Rows)
                {
                    conditiion += $"工作站編號='{row["工作站編號"]}' OR ";
                    workstation_info = new { 工作站編號 = row["工作站編號"], 工作站名稱 = row["工作站名稱"] };
                    workstation_All.Add(workstation_info);
                }
                conditiion = conditiion.Substring(0, conditiion.Length - 4);
            }
            else
            {
                set_Connect(factory);
                conditiion += $"工作站編號='{myData["WorkStation_Num"]}'";
                workstation_info = new { 工作站編號 = myData["WorkStation_Num"], 工作站名稱 = myData["WorkStation_Name"] };
                workstation_All.Add(workstation_info);
            }


             sqlcmd = $"Select * From 人員工時表 Where 日期='{date}' and ({conditiion})";
             dt = DataTableUtils.GetDataTable(sqlcmd);
            if (HtmlUtil.Check_DataTable(dt))
            {
                dt = DataTableUtils.DataTable_TableNoRow("人員工時表");
                for (int j = 0; j < workstation_All.Count; j++)
                {
                    DataRow row = dt.NewRow();
                    row["日期"] = new_date;
                    row["工作站編號"] = workstation_All[j].GetType().GetProperty("工作站編號").GetValue(workstation_All[j]).ToString();
                    row["工作站名稱"] = workstation_All[j].GetType().GetProperty("工作站名稱").GetValue(workstation_All[j]).ToString();
                    row["應到人數"] = excepted_People;
                    row["實到人數"] = working_People;
                    row["工作時數"] = work_Time;
                    //row["所屬月份"] = TextBox_Date.Text.Replace("-", "");
                    dt.Rows.Add(row);
                }
                int success_count = DataTableUtils.Update_DataTable("人員工時表", $"日期='{date}' and ({conditiion})", dt);
                if (success_count > 0)
                {
                    data = set_Table(myData, "修改成功");
                }
                else
                {
                    data.Add("status", "修改失敗");
                }
            }
            else
            {
                data.Add("status", "修改失敗");
            }


            return data;
        }


        private void set_Connect(string factory)
        {
            if (factory == "sowon")
            {
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
            }
            else
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
            else if (factory == "hor")
            {
                sqlcmd = "select 工作站編號, 工作站名稱 from 工作站型態資料表   where 工作站是否使用中 = '1' and 工作站編號 <> '11'";//大圓盤獨立顯示
            }
            else if (factory == "dek")
            {
                sqlcmd = "select 工作站編號, 工作站名稱 from 工作站型態資料表   where 工作站是否使用中='1'  and 工作站編號 = '11'";
            }

            return sqlcmd;
        }

        private void set_DropDownlist()
        {
            string factory = DropDownList_Factory.SelectedItem.Value;
            if (DropDownList_WorkStation.SelectedItem != null)
                request_WorkStation_Num = DropDownList_WorkStation.SelectedItem.Value;

            set_Connect(factory);
            string sqlcmd =set_Sqlcmd(factory);

            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);

            if (HtmlUtil.Check_DataTable(dt))
            {
                ListItem listItem = new ListItem();
                DropDownList_WorkStation.Items.Clear();
                Month_WorkStation.Items.Clear();
                Day_WorkStation.Items.Clear();
                Update_WorkStation.Items.Clear();
                Month_WorkStation.Items.Add("全部");
                Day_WorkStation.Items.Add("全部");
                DropDownList_WorkStation.Items.Add("全部");
                //Update_WorkStation.Items.Add("全部");
                foreach (DataRow row in dt.Rows)
                {
                    listItem = new ListItem(DataTableUtils.toString(row["工作站名稱"]), DataTableUtils.toString(row["工作站編號"]));
                    Month_WorkStation.Items.Add(listItem);
                    Day_WorkStation.Items.Add(listItem);
                    DropDownList_WorkStation.Items.Add(listItem);
                    Update_WorkStation.Items.Add(listItem);
                }
                //設定預設選項
                if (request_WorkStation_Num != null || request_WorkStation_Num != "")
                {
                    for (int i = 0; i < DropDownList_WorkStation.Items.Count; i++)
                    {
                        if (DropDownList_WorkStation.Items[i].Value == request_WorkStation_Num)
                        {
                            DropDownList_WorkStation.Items[i].Selected = true;
                        }
                    }
                }

            }


        }


        private void MainProcess()
        {
            Get_DataTable();
            set_DropDownlist();
            set_Table();

        }
        //修改
        protected void Button_Save_Click(object sender, EventArgs e)
        {
        }
        //刪除
        protected void Button_Delete_Click(object sender, EventArgs e)
        {
        }
        //查詢
        protected void button_select_Click(object sender, EventArgs e)
        {

            MainProcess();
        }
        

        //設定數值
        private void GetCondi()
        {
            Response.Buffer = false;
            if (Request.QueryString["key"] != null)
            {
                Dictionary<string, string> keyValues = HtmlUtil.Return_dictionary(Request.QueryString["key"]);
                request_WorkStation_Num = HtmlUtil.Search_Dictionary(keyValues, "workstation_Num");
                request_WorkStation = HtmlUtil.Search_Dictionary(keyValues, "workstation");
                DropDownList_Factory.SelectedValue = HtmlUtil.Search_Dictionary(keyValues, "factory"); ;
            }
            else
                Response.Redirect("Set_Energy.aspx", false);
        }

        protected void DropDownList_Factory_SelectedIndexChanged(object sender, EventArgs e)
        {
            set_DropDownlist(true);
        }

        private void set_DropDownlist(bool Refalsh = false)
        {
            string sqlcmd = "";
            string factory = DropDownList_Factory.SelectedItem.Value;
            set_Connect(factory);
            sqlcmd = set_Sqlcmd(factory);

            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);

            if (HtmlUtil.Check_DataTable(dt))
            {
                if (Refalsh || DropDownList_WorkStation.Items.Count == 0)
                {
                    ListItem listItem = new ListItem();
                    DropDownList_WorkStation.Items.Clear();
                    DropDownList_WorkStation.Items.Add("全部");
                    foreach (DataRow row in dt.Rows)
                    {
                        listItem = new ListItem(DataTableUtils.toString(row["工作站名稱"]), DataTableUtils.toString(row["工作站編號"]));
                        DropDownList_WorkStation.Items.Add(listItem);
                    }
                }
            }
        }
        private void Get_DataTable()
        {
            string sqlcmd = "";
            string factory = DropDownList_Factory.SelectedItem.Value;
            string date = (Select_Month.Text.Replace("-", "")) + "01";
            obj_date = SF.monthInterval(date, acc);
            date_str = obj_date["startDay"];
            date_end = obj_date["endDay"];
            condition = DropDownList_WorkStation.SelectedItem.Value == "全部" ? "" : $"and 工作站編號={DropDownList_WorkStation.SelectedItem.Value}";
            switch (factory)
            {
                case "sowon":
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
                    sqlcmd = $"select 工作站編號 AS 編輯,日期,所屬月份,工作站名稱,應到人數,實到人數,工作時數 from 人員工時表 where 日期>={date_str} and 日期<={date_end} {condition} order by 工作站名稱,日期";
                    dataTable = DataTableUtils.GetDataTable(sqlcmd);
                    break;
                case "dek":
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);//20220811 大圓盤改連臥式資料庫
                    sqlcmd = $"select 工作站編號 AS 編輯,日期,所屬月份,工作站名稱,應到人數,實到人數,工作時數 from 人員工時表 where 日期>={date_str} and 日期<={date_end} and 工作站編號='11' order by 工作站名稱,日期";
                    dataTable = DataTableUtils.GetDataTable(sqlcmd);
                    break;
                case "hor":
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);
                    sqlcmd = $"select 工作站編號 AS 編輯,日期,所屬月份,工作站名稱,應到人數,實到人數,工作時數 from 人員工時表 where 日期>={date_str} and 日期<={date_end} and 工作站編號<>'11' {condition} order by 工作站名稱,日期";
                    dataTable = DataTableUtils.GetDataTable(sqlcmd);
                    break;
            }
        }

        //一般table
        private void set_Table()
        {
            DataTable dt = dataTable;
            if (HtmlUtil.Check_DataTable(dt))
            {
                string titlename = "";
                dt.Columns.Add("刪除", typeof(string));
                dt.Columns["刪除"].SetOrdinal(1);
                th = HtmlUtil.Set_Table_Title(dt, out titlename);
                tr = HtmlUtil.Set_Table_Content(dt, titlename, Set_Energy_callback);
            }
            else
                HtmlUtil.NoData(out th, out tr);
        }

        //CRUD後重載Table
        private Dictionary<string, string> set_Table(Dictionary<string, string> myData, string status)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            //object data = new { };
            string sqlcmd = "";
            string factory = myData["Factory"];
            string serch_WorkStation = myData["Serch_WorkStation"];
            string date = myData["Serch_Month"] + "01";
            string acc = myData["User_Acc"];
            obj_date = SF.monthInterval(date, acc);
            date_str = obj_date["startDay"];
            date_end = obj_date["endDay"];
            condition = serch_WorkStation == "全部" ? "" : $"and 工作站編號={serch_WorkStation}";
            switch (factory)
            {
                case "sowon":
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
                    sqlcmd = $"select 工作站編號 AS 編輯,日期,所屬月份,工作站名稱,應到人數,實到人數,工作時數 from 人員工時表 where 日期>={date_str} and 日期<={date_end} {condition} order by 工作站名稱,日期";
                    dataTable = DataTableUtils.GetDataTable(sqlcmd);
                    break;
                case "dek":
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);//20220811 大圓盤改連臥式資料庫
                    sqlcmd = $"select 工作站編號 AS 編輯,日期,所屬月份,工作站名稱,應到人數,實到人數,工作時數 from 人員工時表 where 日期>={date_str} and 日期<={date_end} and 工作站編號='11' order by 工作站名稱,日期";
                    dataTable = DataTableUtils.GetDataTable(sqlcmd);
                    break;
                case "hor":
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);
                    sqlcmd = $"select 工作站編號 AS 編輯,日期,所屬月份,工作站名稱,應到人數,實到人數,工作時數 from 人員工時表 where 日期>={date_str} and 日期<={date_end} and 工作站編號<>'11' {condition} order by 工作站名稱,日期";
                    dataTable = DataTableUtils.GetDataTable(sqlcmd);
                    break;
            }

            DataTable dt = dataTable;
            if (HtmlUtil.Check_DataTable(dt))
            {
                string titlename = "";

                dt.Columns.Add("刪除", typeof(string));
                dt.Columns["刪除"].SetOrdinal(1);
                th = HtmlUtil.Set_Table_Title(dt, out titlename);
                tr = HtmlUtil.Set_Table_Content(dt, titlename, Set_Energy_callback);
                
                data.Add("th", th);
                data.Add("tr", tr);
                data.Add("status", status);

            }
            else
            {
                HtmlUtil.NoData(out th, out tr);
                //data = new { th = th, tr = tr, status = status };
                data.Add("th", th);
                data.Add("tr", tr);
                data.Add("status", status);
            }
            return data;
        }

        private string Set_Energy_callback(DataRow row, string field_name)
        {
            string value = "";
            string workStion_Num = DataTableUtils.toString(row["編輯"]);
            string date = DataTableUtils.toString(row["日期"]);
            date=DateTime.ParseExact(date, "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces).ToString("yyyy-MM-dd");
            string working_People = DataTableUtils.toString(row["實到人數"]);
            string excepted_People = DataTableUtils.toString(row["應到人數"]);
            string workTime = DataTableUtils.toString(row["工作時數"]);
            if (field_name == "編輯")
            {
                value = $"<td>" +
                            $"<u>" +
                                $"<a href=\"javascript:void(0)\" onclick=Update_Value(\"{workStion_Num}\",\"{date}\",\"{excepted_People}\",\"{working_People}\",\"{workTime}\")  data-toggle=\"modal\" data-target=\"#Update_Modal\">" +
                                    $"編輯" +
                                $"</a>" +
                            $"</u>" +
                        $"</td>";
            }
            else if (field_name == "刪除")
            {
                value = $"<td><b><u><a href=\"javascript: void()\" onclick=Delete_Value(\"{workStion_Num}\",\"{date}\")>刪除</a></u></b></td>";
            }
            return value;
        }

    }
}