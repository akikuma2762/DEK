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

namespace dek_erpvis_v2.pages.SYS_CONTROL
{
    public partial class Set_Month_WorkTime : System.Web.UI.Page
    {
        public string color = "";
        string acc = "";
        string Link = "";
        string condition = "";
        string dt_str = "";
        string dt_end = "";
        public string date_str = "";
        public string date_end = "";
        public string workstation = "";
        public string workstation_Num = "";
        object obj_date;
        public string th = "";
        public string tr = "";
        ShareFunction SF = new ShareFunction();
        DataTable user_Acc = new DataTable();
        DataTable dataTable = new DataTable();
        //public string dt_st = "";
        //public string dt_ed = "";
        //public string Get_SOStatus = "";
        //string Factory = "";
        //string mode = "";
        //public string dt_Row = "";
        //string selectItem_X = "";
        //public string table_Title = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                color = HtmlUtil.change_color(acc);
                DataTableUtils.Conn_String = myclass.GetConnByDekVisErp;
                user_Acc = DataTableUtils.DataTable_GetTable($"SELECT * FROM SYSTEM_PARAMETER WHERE USER_ACC='{acc}'");
                if (HtmlUtil.Search_acc_Column(acc) == "Y" || true)
                {
                    if (!IsPostBack)
                    {
                        string[] date = 德大機械.德大專用月份(acc).Split(',');
                        dt_str = date[0];
                        dt_end = date[1];
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
                    Response.Write("<script>alert('您無法瀏覽此頁面 請向該單位主管主管申請權限!');location.href='../index.aspx';</script>");
            }
            else
                Response.Redirect(myclass.logout_url);
        }

        [WebMethod]
        public static object postData(string _data)
        {
            Dictionary<string, string> myData = new Dictionary<string, string>();
            DataTable Rp_data = JsonToDataTable.JsonStringToDataTable(_data);
            Set_Month_WorkTime SMW = new Set_Month_WorkTime();
            string mode = "";
            object data = new { };


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
                    data= SMW.insert(myData);
                    break;
                case "Update": break;
                case "Delete":
                    data = SMW.delete(myData);
                    
                    break;
            }
            


            return data;
        }
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
        public object insert(Dictionary<string,string> myData) {
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
            object data = new { };
            object obj_date;
            object workstation_info;
            string factory = myData["Factory"];
            string insert_Type = myData["Inser_Type"];
            obj_date = insert_Type == "Month" ? SF.monthInterval(myData["Month"] + "01", myData["User_Acc"]) : SF.monthInterval(myData["Day"], myData["User_Acc"]);


            if (myData["Workstation"] == "全部")
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
                conditiion += $"工作站編號='{myData["Workstation_Num"]}'";
                workstation_info = new { 工作站編號 = myData["Workstation_Num"], 工作站名稱 = myData["Workstation"] };
                workstation_All.Add(workstation_info);
            }


            string date_str = obj_date.GetType().GetProperty("startDay").GetValue(obj_date).ToString();
            string date_end = obj_date.GetType().GetProperty("endDay").GetValue(obj_date).ToString();
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
                    int count = 0;
                    while (d_start.Date.CompareTo(d_end.Date) < 0)
                    {
                        d_start = d_start.AddDays(+count);
                        string date_string = d_start.ToString("yyyyMMdd");
                        count = 1;
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
                            dt_Row["工作人數"] = myData["Working_People"];
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
                        data = new { status = "新增失敗!" };
                    }

                }
                else
                {
                    data = new { status = "日期重複,新增失敗!" };
                }

            }
            else if (insert_Type == "Day")
            {
                if (int.Parse(myData["Day"]) > int.Parse(date_str) && int.Parse(myData["Day"]) <= int.Parse(date_end))
                {
                    month = d_end.Month.ToString();
                }
                else
                {
                    month = d_start.Month.ToString();
                }

                sqlcmd = $"Select * From 人員工時表 where 日期='{myData["Day"]}' and ({conditiion})";
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
                        data = set_Table(myData, "新增成功");

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
            return data;
        }
        public object delete(Dictionary<string, string> myData)
        {
            object data = new { };
            string factory = myData["Factory"];
            string workstation_Num = myData["Workstation_Num"];
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
                else {
                    data = new { status = "失敗" };
                }
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
            string sqlcmd = "";
            string select_value = "";
            string factory = DropDownList_Factory.SelectedItem.Value;
            if (DropDownList_Product.SelectedItem != null)
                workstation_Num = DropDownList_Product.SelectedItem.Value;

            set_Connect(factory);
            sqlcmd=set_Sqlcmd(factory);

            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);

            if (HtmlUtil.Check_DataTable(dt))
            {
                ListItem listItem = new ListItem();
                DropDownList_Product.Items.Clear();
                Month_Workstation.Items.Clear();
                Day_Workstation.Items.Clear();
                Month_Workstation.Items.Add("全部");
                Day_Workstation.Items.Add("全部");
                DropDownList_Product.Items.Add("全部");
                foreach (DataRow row in dt.Rows)
                {
                    listItem = new ListItem(DataTableUtils.toString(row["工作站名稱"]), DataTableUtils.toString(row["工作站編號"]));
                    Month_Workstation.Items.Add(listItem);
                    Day_Workstation.Items.Add(listItem);
                    DropDownList_Product.Items.Add(listItem);

                }
                if (workstation_Num != null || workstation_Num != "")
                {
                    for (int i = 0; i < DropDownList_Product.Items.Count; i++)
                    {
                        if (DropDownList_Product.Items[i].Value == workstation_Num)
                        {
                            DropDownList_Product.Items[i].Selected = true;
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
        private void set_Table()
        {
            DataTable dt = dataTable;
            if (HtmlUtil.Check_DataTable(dt))
            {
                string titlename = "";
               
                dt.Columns.Add("刪除", typeof(string));
                //dt.Columns.Add("編輯");
                //dt.Columns.Add("刪除");
                
                dt.Columns["刪除"].SetOrdinal(1);
                th=HtmlUtil.Set_Table_Title(dt, out titlename);
                tr=HtmlUtil.Set_Table_Content(dt, titlename, Set_Energy_callback);
            }
            else
                HtmlUtil.NoData(out th, out tr);
        }

        private object set_Table(Dictionary<string,string> myData ,string status)
        {
            object data = new { };
            string sqlcmd = "";
            string factory = myData["Factory"];
            string workStation_Num= myData["Workstation_Num"];
            string date = myData["Serch_Month"] + "01";
            string acc = myData["User_Acc"];
            obj_date = SF.monthInterval(date, acc);
            date_str = obj_date.GetType().GetProperty("startDay").GetValue(obj_date).ToString();
            date_end = obj_date.GetType().GetProperty("endDay").GetValue(obj_date).ToString();
            condition = workStation_Num == "全部" ? "" : $"and 工作站編號={workStation_Num}";
            switch (factory)
            {
                case "sowon":
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
                    sqlcmd = $"select 工作站編號 AS 編輯,日期,所屬月份,工作站名稱,工作人數,工作時數 from 人員工時表 where 日期>={date_str} and 日期<={date_end} {condition} order by 工作站名稱,日期";
                    dataTable = DataTableUtils.GetDataTable(sqlcmd);
                    break;
                case "dek":
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);//20220811 大圓盤改連臥式資料庫
                    sqlcmd = $"select 工作站編號 AS 編輯,日期,所屬月份,工作站名稱,工作人數,工作時數 from 人員工時表 where 日期>={date_str} and 日期<={date_end} and 工作站編號='11' order by 工作站名稱,日期";
                    dataTable = DataTableUtils.GetDataTable(sqlcmd);
                    break;
                case "hor":
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);
                    sqlcmd = $"select 工作站編號 AS 編輯,日期,所屬月份,工作站名稱,工作人數,工作時數 from 人員工時表 where 日期>={date_str} and 日期<={date_end} and 工作站編號<>'11' {condition} order by 工作站名稱,日期";
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
                data = new { th = th, tr = tr, status = status };

            }
            else
            {
                HtmlUtil.NoData(out th, out tr);
                data = new { th = th, tr = tr, status = status };
            }
            return data;
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
                if (Refalsh || DropDownList_Product.Items.Count == 0)
                {
                    ListItem listItem = new ListItem();
                    DropDownList_Product.Items.Clear();
                    DropDownList_Product.Items.Add("全部");
                    foreach (DataRow row in dt.Rows)
                    {
                        listItem = new ListItem(DataTableUtils.toString(row["工作站名稱"]), DataTableUtils.toString(row["工作站編號"]));
                        DropDownList_Product.Items.Add(listItem);
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
            date_str = obj_date.GetType().GetProperty("startDay").GetValue(obj_date).ToString();
            date_end = obj_date.GetType().GetProperty("endDay").GetValue(obj_date).ToString();
            condition = DropDownList_Product.SelectedItem.Value == "全部" ? "" : $"and 工作站編號={DropDownList_Product.SelectedItem.Value}";
            switch (factory)
            {
                case "sowon":
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
                    sqlcmd = $"select 工作站編號 AS 編輯,日期,所屬月份,工作站名稱,工作人數,工作時數 from 人員工時表 where 日期>={date_str} and 日期<={date_end} {condition} order by 工作站名稱,日期";
                    dataTable = DataTableUtils.GetDataTable(sqlcmd);
                    break;
                case "dek":
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);//20220811 大圓盤改連臥式資料庫
                    sqlcmd = $"select 工作站編號 AS 編輯,日期,所屬月份,工作站名稱,工作人數,工作時數 from 人員工時表 where 日期>={date_str} and 日期<={date_end} and 工作站編號='11' order by 工作站名稱,日期";
                    dataTable = DataTableUtils.GetDataTable(sqlcmd);
                    break;
                case "hor":
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);
                    sqlcmd = $"select 工作站編號 AS 編輯,日期,所屬月份,工作站名稱,工作人數,工作時數 from 人員工時表 where 日期>={date_str} and 日期<={date_end} and 工作站編號<>'11' {condition} order by 工作站名稱,日期";
                    dataTable = DataTableUtils.GetDataTable(sqlcmd);
                    break;
            }
        }
       

        //設定數值
        private void GetCondi()
        {
            Response.Buffer = false;
            if (Request.QueryString["key"] != null)
            {

                Dictionary<string, string> keyValues = HtmlUtil.Return_dictionary(Request.QueryString["key"]);
                workstation_Num = HtmlUtil.Search_Dictionary(keyValues, "workstation_Num");
                workstation = HtmlUtil.Search_Dictionary(keyValues, "workstation");
                DropDownList_Factory.SelectedValue = HtmlUtil.Search_Dictionary(keyValues, "factory"); ;
            }
            else
                Response.Redirect("Set_Energy.aspx", false);
        }

        private string Set_Energy_callback(DataRow row, string field_name)
        {
            string value = "";
            string capacity = "";
            string workStion_Num = DataTableUtils.toString(row["編輯"]);
            string date = DataTableUtils.toString(row["日期"]);
            date=DateTime.ParseExact(date, "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces).ToString("yyyy-MM-dd");
            string working_People = DataTableUtils.toString(row["工作人數"]);
            string workTime = DataTableUtils.toString(row["工作時數"]);


            if (field_name == "編輯")
            {
                value = $"<td>" +
                            $"<u>" +
                                $"<a href=\"javascript:void(0)\" onclick=Update_Value(\"{workStion_Num}\",\"{date}\",\"{working_People}\",\"{workTime}\")  data-toggle=\"modal\" data-target=\"#Update_Modal\">" +
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