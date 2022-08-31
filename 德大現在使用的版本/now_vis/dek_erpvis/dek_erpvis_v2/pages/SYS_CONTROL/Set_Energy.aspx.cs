using dek_erpvis_v2.cls;
using dek_erpvis_v2.webservice;
using dekERP_dll.dekErp;
using Support;
using Support.DB;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
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
        //-----------------------------------------------------Function---------------------------------------------------------------------      
        //需要執行的程式
        private void MainProcess()
        {
            set_DropDownlist();
            Get_MonthTotal();
            Set_Table();
        }

        private void set_DropDownlist()
        {
            if (dropdownlist_Factory.SelectedItem.Value.ToLower() == "sowon")
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
            else if (dropdownlist_Factory.SelectedItem.Value.ToLower() == "iTec" || dropdownlist_Factory.SelectedItem.Value.ToLower() == "dek")
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);

            string sqlcmd = "select 工作站編號, 工作站名稱 from 工作站型態資料表   where 工作站是否使用中='1' ";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);

            if (HtmlUtil.Check_DataTable(dt))
            {
                if (Workstation.Items.Count == 0)
                {
                    ListItem listItem = new ListItem();
                    Workstation.Items.Clear();
                    Workstation.Items.Add("全部");
                    foreach (DataRow row in dt.Rows)
                    {
                        listItem = new ListItem(DataTableUtils.toString(row["工作站名稱"]), DataTableUtils.toString(row["工作站編號"]));
                        Workstation.Items.Add(listItem);
                    }
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
                    sqlcmd = "select 工作站編號 AS 編輯產能,工作站名稱,目標件數 AS 每日產能 from 工作站型態資料表 where 工作站是否使用中 = '1' and 工作站編號 <> '11'";
                    dt_monthtotal = DataTableUtils.GetDataTable(sqlcmd);
                    break;
                case "dek":
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);//20220811 大圓盤改連臥式資料庫
                    sqlcmd = "select 工作站編號 AS 編輯產能,工作站名稱,目標件數 AS 每月產能 from 工作站型態資料表 where 工作站是否使用中 = '1' and 工作站編號 = '11'";
                    dt_monthtotal = DataTableUtils.GetDataTable(sqlcmd);
                    break;
                case "iTec":
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);
                    sqlcmd = "select 工作站編號 AS 編輯產能,工作站名稱,目標件數 AS 每月產能 from 工作站型態資料表 where 工作站是否使用中 = '1' and ( 工作站編號 = '1' OR 工作站編號 = '2') ";
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