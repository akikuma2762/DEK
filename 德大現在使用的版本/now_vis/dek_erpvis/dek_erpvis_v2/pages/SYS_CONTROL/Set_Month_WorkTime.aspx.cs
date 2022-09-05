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
        public StringBuilder th = new StringBuilder();
        public StringBuilder tr = new StringBuilder();
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
                        set_DropDownlist();
                        GetCondi();
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
            ShareFunction SF = new ShareFunction();
            Set_Month_WorkTime SMW = new Set_Month_WorkTime();
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
            object obj_date;
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
            obj_date = mode == "Month" ? SF.monthInterval(myData["Month"] + "01", myData["User_Acc"]) : SF.monthInterval(myData["Day"], myData["User_Acc"]);


            if (myData["Workstation"] == "全部")
            {
                SMW.set_Connect(factory);
                sqlcmd = SMW.set_Sqlcmd(factory);
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
                workstation_info = new { 工作站編號 = myData["Workstation_Num"], 工作站名稱 = myData["Workstation"] };
                workstation_All.Add(workstation_info);
            }


            date_str = obj_date.GetType().GetProperty("startDay").GetValue(obj_date).ToString();
            date_end = obj_date.GetType().GetProperty("endDay").GetValue(obj_date).ToString();
            d_start = DateTime.ParseExact(date_str, "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
            d_end = DateTime.ParseExact(date_end, "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces);

            if (mode == "Month")
            {
                sqlcmd = $"Select * From 人員工時表 where 日期>='{date_str}' and 日期<='{date_end}' and ({conditiion})";
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

        private void set_Connect(string factory)
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

        private void set_DropDownlist()
        {
            string sqlcmd = "";
            string select_value = "";
            if (DropDownList_Product.SelectedItem != null)
                select_value = DropDownList_Product.SelectedItem.Value;
            if (DropDownList_Factory.SelectedItem.Value.ToLower() == "sowon")
            {
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
                sqlcmd = "select 工作站編號, 工作站名稱 from 工作站型態資料表   where 工作站是否使用中 = '1'";
            }
            else if (DropDownList_Factory.SelectedItem.Value == "iTec")
            {
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);
                sqlcmd = "select 工作站編號, 工作站名稱 from 工作站型態資料表   where 工作站是否使用中 = '1' and 工作站編號 <> '11'";//大圓盤獨立顯示
            }
            else if (DropDownList_Factory.SelectedItem.Value.ToLower() == "dek")
            {
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);
                sqlcmd = "select 工作站編號, 工作站名稱 from 工作站型態資料表   where 工作站是否使用中='1'  and 工作站編號 = '11'";
            }


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
                if (select_value != null || select_value != "")
                {
                    for (int i = 0; i < DropDownList_Product.Items.Count; i++)
                    {
                        if (DropDownList_Product.Items[i].Value == select_value)
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
                th.Append(HtmlUtil.Set_Table_Title(dt, out titlename));
                tr.Append(HtmlUtil.Set_Table_Content(dt, titlename, Set_Energy_callback));
            }
            else
                HtmlUtil.NoData(out th, out tr);
        }
        protected void DropDownList_Factory_SelectedIndexChanged(object sender, EventArgs e)
        {
            set_DropDownlist(true);
        }

        private void set_DropDownlist(bool Refalsh = false)
        {
            string sqlcmd = "";
            if (DropDownList_Factory.SelectedItem.Value == "sowon")
            {
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
                sqlcmd = "select 工作站編號, 工作站名稱 from 工作站型態資料表   where 工作站是否使用中='1' ";
            }
            else if (DropDownList_Factory.SelectedItem.Value == "iTec")
            {
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);
                sqlcmd = "select 工作站編號, 工作站名稱 from 工作站型態資料表   where 工作站是否使用中='1' and 工作站編號<> '11'";
            }
            else if (DropDownList_Factory.SelectedItem.Value == "dek")
            {
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);
                sqlcmd = "select 工作站編號, 工作站名稱 from 工作站型態資料表   where 工作站是否使用中='1' and 工作站編號='11' ";
            }

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
            string date = (Select_Month.Text.Replace("-", "")) + "01";
            obj_date = SF.monthInterval(date, acc);
            date_str = obj_date.GetType().GetProperty("startDay").GetValue(obj_date).ToString();
            date_end = obj_date.GetType().GetProperty("endDay").GetValue(obj_date).ToString();
            condition = DropDownList_Product.SelectedItem.Value == "全部" ? "" : $"and 工作站編號={DropDownList_Product.SelectedItem.Value}";
            switch (DropDownList_Factory.SelectedItem.Value)
            {
                case "sowon":
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
                    sqlcmd = $"select 工作站編號 AS 編輯,日期,所屬月份,工作站名稱,工作人數,工作時數 from 人員工時表 where 日期>{date_str} and 日期<={date_end} {condition}";
                    dataTable = DataTableUtils.GetDataTable(sqlcmd);
                    break;
                case "dek":
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);//20220811 大圓盤改連臥式資料庫
                    sqlcmd = $"select 工作站編號 AS 編輯,日期,所屬月份,工作站名稱,工作人數,工作時數 from 人員工時表 where 日期>{date_str} and 日期<={date_end} and 工作站編號='11'";
                    dataTable = DataTableUtils.GetDataTable(sqlcmd);
                    break;
                case "iTec":
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);
                    sqlcmd = $"select 工作站編號 AS 編輯,日期,所屬月份,工作站名稱,工作人數,工作時數 from 人員工時表 where 日期>{date_str} and 日期<={date_end} and 工作站編號<>'11' {condition}";
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
                DropDownList_Product.SelectedValue = HtmlUtil.Search_Dictionary(keyValues, "workstation_Num");
                //dt_ed = HtmlUtil.Search_Dictionary(keyValues, "date_end");
                //Get_SOStatus = HtmlUtil.Search_Dictionary(keyValues, "condi");
                ////--新增超連結 參數 20220615--//
                //Factory = HtmlUtil.Search_Dictionary(keyValues, "type");
                //mode = HtmlUtil.Search_Dictionary(keyValues, "mode");
                //dt_Row = HtmlUtil.Search_Dictionary(keyValues, "dt_Row");
                //cust_name = dt_Row;
                //selectItem_X = HtmlUtil.Search_Dictionary(keyValues, "selectItem_X");
                //table_Title = dt_Row;
                //if (mode.Contains("order_month_Overdue")) table_Title += "逾期";
                ////---------------------------//
                ////儲存cookie
                //Response.Cookies.Add(HtmlUtil.Save_Cookies("Order", table_Title));
            }
            else
                Response.Redirect("Set_Energy.aspx", false);
        }

        private string Set_Energy_callback(DataRow row, string field_name)
        {
            string value = "";
            string capacity = "";
            


            if (field_name == "編輯")
            {
                value = $"<td>" +
                            $"<u>" +
                                $"<a href=\"javascript:void(0)\" onclick=Set_Value(\"{row["編輯"]}\",\"{row["日期"]}\")  data-toggle=\"modal\" data-target=\"#exampleModal_information\">" +
                                    $"編輯" +
                                $"</a>" +
                            $"</u>" +
                        $"</td>";
            }
            return value;
        }

    }
}