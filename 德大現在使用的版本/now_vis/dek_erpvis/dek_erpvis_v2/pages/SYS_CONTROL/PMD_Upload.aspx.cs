using dek_erpvis_v2.cls;
using Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace dek_erpvis_v2.pages.SYS_CONTROL
{
    public partial class PMD_Upload1 : System.Web.UI.Page
    {
        public string color = "";
        public string th = "";
        public string tr = "";
        string acc = "";
        string Link = "";
        string condition = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                color = HtmlUtil.change_color(acc);
                if (HtmlUtil.Search_acc_Column(acc) == "Y")
                {
                    if (txt_str.Text == "" && txt_end.Text == "")
                    {
                        txt_str.Text = DateTime.Now.Date.AddDays(-(int)(DateTime.Now.DayOfWeek) + 1).ToString("yyyy-MM-dd");//当前周的开始日期
                        txt_end.Text = DateTime.Now.Date.AddDays(7 - (int)(DateTime.Now.DayOfWeek)).ToString("yyyy-MM-dd");//当前周的结束日期
                    }
                    load_data();
                }
                else
                    Response.Write("<script>alert('您無法瀏覽此頁面 請向該單位主管主管申請權限!');location.href='../index.aspx';</script>");
            }
            else
                Response.Redirect(myclass.logout_url);
        }
        //修改
        protected void Button_Save_Click(object sender, EventArgs e)
        {
            if (Link == "ver")
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
            else if (Link == "hor")
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);
            string sqlcmd = $"Select * From 工作站狀態資料表 where  組裝編號= '{TextBox_OrderNum.Text}' and  排程編號= '{TextBox_Schedule.Text}' and 工作站編號= '{TextBox_WorkNumber.Text}'";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
            if (HtmlUtil.Check_DataTable(dt))
            {
                DataRow row = dt.NewRow();
                row["組裝編號"] = TextBox_Order.Text;
                row["排程編號"] = TextBox_Number.Text;
                row["進度"] = DropDownList_Percent.SelectedItem.Text.Trim('%');
                row["狀態"] = DropDownList_Status.SelectedItem.Value;
                row["工作站編號"] = DropDownList_Work.SelectedItem.Value;
                row["組裝日"] = TextBox_Date.Text.Replace("-", "");
                row["實際組裝時間"] = TextBox_Truedate.Text.Replace("-", "");
                if(DropDownList_Status.SelectedItem.Value=="未動工")
                {
                    row["進度"] ="0";
                    row["實際啟動時間"] = "";
                    row["再次啟動時間"] = "";
                    row["暫停時間"] = "";
                    row["實際完成時間"] = "";
                }

                if (Link == "ver")
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
                else if (Link == "hor")
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);
                if (DataTableUtils.Update_DataRow("工作站狀態資料表", $" 組裝編號= '{TextBox_OrderNum.Text}' and  排程編號= '{TextBox_Schedule.Text}' and 工作站編號= '{TextBox_WorkNumber.Text}'", row))
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script>alert('修改成功');</script>");
                else
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script>alert('修改失敗');</script>");
            }
            load_data();
        }
        //刪除
        protected void Button_Delete_Click(object sender, EventArgs e)
        {
            if (Link == "ver")
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
            else if (Link == "hor")
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);

            string sqlcmd = $"Select 組裝編號,排程編號,進度,狀態,工作站編號,組裝日,實際組裝時間 From 工作站狀態資料表 where  組裝編號= '{TextBox_OrderNum.Text}' and  排程編號= '{TextBox_Schedule.Text}' and 工作站編號= '{TextBox_WorkNumber.Text}'";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);

            if (HtmlUtil.Check_DataTable(dt))
            {
                if (Link == "ver")
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
                else if (Link == "hor")
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);
                if (DataTableUtils.Delete_Record("工作站狀態資料表", $" 組裝編號= '{TextBox_OrderNum.Text}' and  排程編號= '{TextBox_Schedule.Text}' and 工作站編號= '{TextBox_WorkNumber.Text}'"))
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script>alert('刪除成功');</script>");
                else
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script>alert('刪除失敗');</script>");
            }
            load_data();
        }
        //查詢
        protected void button_select_Click(object sender, EventArgs e)
        {
            string sqlcmd = $"type={DropDownList_Type.SelectedItem.Value},date_str={txt_str.Text},date_end={txt_end.Text},Product={DropDownList_Product.SelectedItem.Value},keynum={TextBox_keyWord.Text}";
            Response.Redirect($"PMD_Upload.aspx?key={WebUtils.UrlStringEncode(sqlcmd)}");
        }
        //確認連結
        private void load_data()
        {
            //有的話，導向該資料庫
            if (Request.QueryString["key"] != null)
            {
                Dictionary<string, string> keyValues = HtmlUtil.Return_dictionary(Request.QueryString["key"]);
                Link = HtmlUtil.Search_Dictionary(keyValues, "type").ToLower();
                string dt_st = "";
                string dt_ed = "";
                if (keyValues.Count == 1)
                {
                    dt_st = txt_str.Text;
                    dt_ed = txt_end.Text;
                }
                else if (keyValues.Count == 5)
                {
                    dt_st = HtmlUtil.Search_Dictionary(keyValues, "date_str");
                    dt_ed = HtmlUtil.Search_Dictionary(keyValues, "date_end");
                    if (!IsPostBack)
                    {
                        DropDownList_Type.SelectedValue = HtmlUtil.Search_Dictionary(keyValues, "type");
                        set_DropDownlist();
                        TextBox_keyWord.Text = HtmlUtil.Search_Dictionary(keyValues, "keynum");
                        txt_str.Text = HtmlUtil.Search_Dictionary(keyValues, "date_str");
                        txt_end.Text = HtmlUtil.Search_Dictionary(keyValues, "date_end");
                        DropDownList_Product.SelectedValue = HtmlUtil.Search_Dictionary(keyValues, "Product");
                    }
                }
                set_DropDownlist();
                if (DropDownList_Product.Items.Count > 0 && DropDownList_Product.SelectedItem.Value != "全部")
                    condition += $" and 工作站狀態資料表.工作站編號 = '{DropDownList_Product.SelectedItem.Value}' ";
                if (TextBox_keyWord.Text != "")
                    condition += $" and 工作站狀態資料表.排程編號 like '%{TextBox_keyWord.Text}%' ";


                Set_DataTable(Link, dt_st.Replace("-", ""), dt_ed.Replace("-", ""), condition);
                Set_Percent();
                Set_Status();
                Set_Work();
            }
            //沒有的話，先導向立式
            else
                Response.Redirect($"PMD_Upload.aspx?key={WebUtils.UrlStringEncode($"type=" + DropDownList_Type.SelectedItem.Value)}");
        }
        //產生資料表
        private void Set_DataTable(string Link, string start, string end, string sqlcmd)
        {
            if (Link == "ver")
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
            else if (Link == "hor")
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);

            string sql_cmd = $"Select 工作站名稱,組裝日,組裝編號,排程編號,進度,狀態,實際組裝時間 From 工作站狀態資料表  left join 工作站型態資料表 on 工作站型態資料表.工作站編號 = 工作站狀態資料表.工作站編號 where 組裝日>={start} and 組裝日<={end} {sqlcmd}";
            DataTable dt = DataTableUtils.GetDataTable(sql_cmd);

            if (HtmlUtil.Check_DataTable(dt))
            {
                //增加編輯與刪除之欄位
                dt.Columns.Add("編輯");
                dt.Columns.Add("刪除");
                //把這兩個欄位移至最前面
                dt.Columns["編輯"].SetOrdinal(0);
                dt.Columns["刪除"].SetOrdinal(1);

                string title = "";
                th = HtmlUtil.Set_Table_Title(dt, out title);
                tr = HtmlUtil.Set_Table_Content(dt, title, PMD_Uploadcallback);
            }
            else
                HtmlUtil.NoData(out th, out tr);

        }
        //表格例外處理
        private string PMD_Uploadcallback(DataRow row, string field_name)
        {
            string value = "";

            if (field_name == "編輯")
                value = $"<td style='width:7%'><b><u><a href=\"javascript: void()\" onclick=Set_Value('{DataTableUtils.toString(row["組裝編號"])}'," +
                                                                        $"'{DataTableUtils.toString(row["排程編號"])}'," +
                                                                        $"'{DataTableUtils.toString(row["進度"])}'," +
                                                                        $"'{DataTableUtils.toString(row["狀態"])}'," +
                                                                        $"'{Change_WorkID(DataTableUtils.toString(row["工作站名稱"]))}'," +
                                                                        $"'{HtmlUtil.changetimeformat(DataTableUtils.toString(row["組裝日"]), "-")}'," +
                                                                        $"'{HtmlUtil.changetimeformat(DataTableUtils.toString(row["實際組裝時間"]), "-")}') data-toggle = \"modal\" data-target = \"#exampleModal\">編輯</a></u></b></td>";
            else if (field_name == "刪除")
                value = $"<td style='width:7%'><b><u><a href=\"javascript: void()\" onclick=Delete_Value('{DataTableUtils.toString(row["組裝編號"])}','{DataTableUtils.toString(row["排程編號"])}','{Change_WorkID(DataTableUtils.toString(row["工作站名稱"]))}')>刪除</a></u></b></td>";
            else if (field_name == "組裝日" || field_name == "實際組裝時間")
                value = $"<td>{HtmlUtil.changetimeformat(DataTableUtils.toString(row[field_name]))}</td>";

            if (value == "")
                return "";
            else
                return value;
        }
        //設定進度
        private void Set_Percent()
        {
            if (DropDownList_Percent.Items.Count == 0)
            {
                DropDownList_Percent.Items.Clear();
                for (int i = 0; i < 100; i = i + 10)
                    DropDownList_Percent.Items.Add(i.ToString() + "%");
                DropDownList_Percent.Items.Add("99" + "%");
                DropDownList_Percent.Items.Add("100" + "%");
            }

        }
        //設定狀態
        private void Set_Status()
        {
            if (DropDownList_Status.Items.Count == 0)
            {
                DropDownList_Status.Items.Clear();
                DropDownList_Status.Items.Add("未動工");
                DropDownList_Status.Items.Add("啟動");
                DropDownList_Status.Items.Add("暫停");
                DropDownList_Status.Items.Add("完成");
            }

        }
        //設定工作站編號
        private void Set_Work()
        {
            if (Link == "ver")
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
            else if (Link == "hor")
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);
            string sqlcmd = "select 工作站編號,工作站名稱 from 工作站型態資料表 where 工作站是否使用中='1'";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);

            if (HtmlUtil.Check_DataTable(dt))
            {
                if (DropDownList_Work.Items.Count == 0)
                {
                    DropDownList_Work.Items.Clear();
                    ListItem list = new ListItem();
                    foreach (DataRow row in dt.Rows)
                    {
                        list = new ListItem(DataTableUtils.toString(row["工作站名稱"]), DataTableUtils.toString(row["工作站編號"]));
                        DropDownList_Work.Items.Add(list);
                    }
                }

            }
        }
        //變更工作站編號
        private string Change_WorkID(string Number)
        {
            if (Link == "ver")
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
            else if (Link == "hor")
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);
            string sqlcmd = $"select 工作站編號,工作站名稱 from 工作站型態資料表 where 工作站是否使用中='1' and 工作站名稱 = '{Number}' ";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);

            if (HtmlUtil.Check_DataTable(dt))
                return DataTableUtils.toString(dt.Rows[0]["工作站編號"]);
            else
                return "";
        }
        //設定下拉選單的值
        private void set_DropDownlist(bool Refalsh = false)
        {
            if (DropDownList_Type.SelectedItem.Value.ToLower() == "ver")
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
            else if (DropDownList_Type.SelectedItem.Value.ToLower() == "hor")
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);

            string sqlcmd = "select 工作站編號, 工作站名稱 from 工作站型態資料表   where 工作站是否使用中='1' ";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);

            if (HtmlUtil.Check_DataTable(dt))
            {
                if (Refalsh || DropDownList_Product.Items.Count == 0)
                {
                    ListItem listItem = new ListItem();
                    DropDownList_Product.Items.Clear();
                    listItem = new ListItem("全部", "全部");
                    DropDownList_Product.Items.Add(listItem);

                    foreach (DataRow row in dt.Rows)
                    {
                        listItem = new ListItem(DataTableUtils.toString(row["工作站名稱"]), DataTableUtils.toString(row["工作站編號"]));
                        DropDownList_Product.Items.Add(listItem);
                    }
                }
            }

        }
        protected void DropDownList_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            set_DropDownlist(true);
        }
    }
}