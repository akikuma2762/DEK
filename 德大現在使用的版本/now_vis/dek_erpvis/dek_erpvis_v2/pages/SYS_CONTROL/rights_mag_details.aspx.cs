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
    public partial class rights_mag_details : BasePage
    {
        public string color = "";
        string acc = "";
        string dpm = "";
        string adm = "";
        public string selected_user_name = "";
        string selected_user_acc = "";
        myclass myclass = new myclass();
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                DataTableUtils.Conn_String = myclass.GetConnByDekVisErp;
                GlobalVar.Open();

                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                color = HtmlUtil.change_color(acc);
                dpm = DataTableUtils.toString(userInfo["user_DPM"]);
                adm = check_user_power(acc);
                string url = DataTableUtils.toString(HttpContext.Current.Request.QueryString["password_"]);
                if (adm == "Y")
                {
                    //if (!IsPostBack)
                    //{
                    if (url != "")
                    {
                        selected_user_acc = DataTableUtils.toString(myclass.Base64Decode(DataTableUtils.toString(HttpContext.Current.Request.QueryString["password_"]))).Split(',')[0].Split('=')[1];
                        iniDropdownlist();
                        GotoCenn();
                    }
                    else
                        Response.Redirect("rights_mag.aspx");

                    //}
                }
                else
                    Response.Redirect("rights_mag.aspx");

                if (acc == "visrd")
                    Panel1.Visible = true;
            }
            else
                Response.Redirect("rights_mag.aspx", false);


            int i = 0;
            foreach (Control ctl in this.Panel_contr.Controls)
            {
                if (ctl is CheckBoxList)
                    i++;
            }
        }
        private void GotoCenn()
        {
            Set_Checkboxlist();
            load_page_data();
            Label_code.Text = new Random().Next(9999).ToString();
            TextBox_code.Text = "";
        }
        private void iniDropdownlist()
        {
            if (DropDownList_dmt.Items.Count == 0)
            {
                DropDownList_dmt.Items.Clear();
                string sqlcmd = 德大機械.控制台_權限管理.取得部門("SYS");
                DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
                if (HtmlUtil.Check_DataTable(dt))
                {
                    sqlcmd = 德大機械.控制台_權限管理.取得用戶個人資訊(selected_user_acc);
                    DataTable dr = DataTableUtils.GetDataTable(sqlcmd);
                    string dpm = DataTableUtils.toString(dr.Rows[0]["USER_DPM"]);
                    ListItem li = new ListItem("--請選擇--", "NULL");
                    DropDownList_dmt.Items.Add(li);
                    foreach (DataRow row in dt.Rows)
                    {
                        li = new ListItem(DataTableUtils.toString(row["DPM_NAME2"]), DataTableUtils.toString(row["DPM_NAME"]));

                        DropDownList_dmt.Items.Add(li);
                    }
                    dr.Clear();
                    dr.Dispose();
                }
                dt.Clear();
                dt.Dispose();
            }

        }
        private void load_page_data()
        {
            Response.BufferOutput = false;
            string passwrod = DataTableUtils.toString(HttpContext.Current.Request.QueryString["password_"]);
            if (passwrod != "")
            {
                string url = DataTableUtils.toString(myclass.Base64Decode(passwrod));
                if (url != "")
                {
                    selected_user_acc = url.Split(',')[0].Split('=')[1];
                    selected_user_name = HtmlUtil.Search_acc_Column(selected_user_acc, "USER_NAME");
                    set_control_panel();
                    set_selected_user_data();
                }
                else
                    Response.Redirect("rights_mag.aspx");
            }
            else
                Response.Redirect("rights_mag.aspx");
        }
        private void set_control_panel()
        {
            Panel_contr.Controls.Clear();
            int i = 0;
            string sql_cmd = 德大機械.控制台_權限管理.取得部門_依網頁();
            DataTable dt = DataTableUtils.GetDataTable(sql_cmd);
            Panel_contr.Controls.Add(new LiteralControl("<div class=\"row\">\n"));
            foreach (DataRow row in dt.Rows)
            {
                i += 2;
                if (i % 12 == 0)
                {
                    Panel_contr.Controls.Add(new LiteralControl("<div class=\"row\">\n"));
                    Panel_contr_Add(row);
                    Panel_contr.Controls.Add(new LiteralControl("</div>\n"));
                }
                else
                    Panel_contr_Add(row);
            }
            Panel_contr.Controls.Add(new LiteralControl("</div>\n"));
        }
        private void set_selected_user_data()
        {
            if (!IsPostBack)
            {
                string sqlcmd = 德大機械.控制台_權限管理.取得用戶個人資訊(selected_user_acc);
                DataRow row = DataTableUtils.DataTable_GetDataRow(sqlcmd);
                if (row != null)
                {
                    TextBox_fullname.Text = DataTableUtils.toString(row["USER_NAME"]);
                    TextBox_num.Text = DataTableUtils.toString(row["USER_NUM"]);
                    TextBox_Birth.Text = HtmlUtil.changetimeformat(DataTableUtils.toString(row["USER_BIRTHDAY"]), "-");
                    //    TextBox_birthday.Value = DataTableUtils.toString(row["USER_BIRTHDAY"]);
                    TextBox_email.Text = DataTableUtils.toString(row["USER_EMAIL"]);
                    Label_psd.Text = DataTableUtils.toString(row["USER_PWD"]);
                    DropDownList_dmt.SelectedValue = DataTableUtils.toString(row["USER_DPM"]);
                }
            }


        }
        private void Panel_contr_Add(DataRow row)
        {
            Panel_contr.Controls.Add(new LiteralControl("<div class=\"col-md-3 col-sm-4 col-xs-12\">\n"));
            Panel_contr.Controls.Add(new LiteralControl("<label class=\"control-label \">" + DataTableUtils.toString(row["DPM_NAME2"]) + " " + DataTableUtils.toString(row["WEB_DPM"]) + "</label>\n"));
            Panel_contr.Controls.Add(new LiteralControl("<div class=\"\">\n"));
            Panel_contr.Controls.Add(new LiteralControl("<label>\n"));
            set_createCheckBoxList(DataTableUtils.toString(row["WEB_DPM"]));
            Panel_contr.Controls.Add(new LiteralControl("</label>\n"));
            Panel_contr.Controls.Add(new LiteralControl("</div>\n"));
            Panel_contr.Controls.Add(new LiteralControl("</div>\n"));
        }
        private void set_createCheckBoxList(string dep_item)
        {
            CheckBoxList checkBoxList = new CheckBoxList();
            checkBoxList.ID = "checkBoxList" + dep_item;
            checkBoxList.CssClass = "table-striped";
            get_checkBoxList_Item(checkBoxList, dep_item);
            Panel_contr.Controls.Add(checkBoxList);
        }
        private void get_checkBoxList_Item(CheckBoxList CheckBoxList, string dep_item)
        {
            ListItem listItem = null;
            //string sql_cmd = "select a.*, (CASE WHEN b.USER_ACC is null THEN '' ELSE b.USER_ACC END ) as USER_ACC from (select WEB_PAGES.WEB_DPM, WEB_PAGES.WEB_PAGENAME, WEB_PAGES.WEB_URL ,web_pages.web_open FROM WEB_PAGES) as a left join (SELECT WB_URL,USER_ACC FROM WEB_USER where USER_ACC = '" + selected_user_acc + "') as b on a.WEB_URL = b.WB_URL  where a.WEB_DPM = '" + dep_item + "' and a.WEB_open = 'Y'  ";
            string sql_cmd = "select a.*, (CASE WHEN b.USER_ACC is null THEN '' ELSE b.USER_ACC END ) as USER_ACC,b.VIEW_NY from (select WEB_PAGES.WEB_DPM, WEB_PAGES.WEB_PAGENAME, WEB_PAGES.WEB_URL ,web_pages.web_open FROM WEB_PAGES) as a left join (SELECT * FROM WEB_USER where USER_ACC = '" + selected_user_acc + "') as b on a.WEB_URL = b.WB_URL  where a.WEB_DPM = '" + dep_item + "' and a.WEB_open = 'Y'";
            DataTable dt = DataTableUtils.GetDataTable(sql_cmd);
            foreach (DataRow row in dt.Rows)
            {
                listItem = new ListItem(DataTableUtils.toString(row["WEB_PAGENAME"]), DataTableUtils.toString(row["WEB_URL"]));
                string View_NY = row["VIEW_NY"].ToString();//20221228修改-增加判斷view_ny,確定審核過才能打勾,不是有審核就打勾
                if (DataTableUtils.toString(row["USER_ACC"]) != "" && View_NY=="Y")
                    listItem.Selected = true;
                else
                    listItem.Selected = false;
                CheckBoxList.Items.Add(listItem);
            }


        }
        private string check_user_power(string acc_)
        {
            DataRow row = DataTableUtils.DataTable_GetDataRow("USERS", "USER_ACC = '" + acc_ + "'");
            if (row != null)
                return DataTableUtils.toString(row["ADM"]);
            else
                return "";
        }
        private void Set_Checkboxlist()
        {
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisErp);
            string sqlcmd = "select * from High_Power";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);

            if (HtmlUtil.Check_DataTable(dt))
            {
                if (CheckBoxList_Power.Items.Count == 0)
                {
                    ListItem listItem = new ListItem();
                    foreach (DataRow row in dt.Rows)
                    {
                        listItem = new ListItem(DataTableUtils.toString(row["Pages"]) + "-" + DataTableUtils.toString(row["Chinese_Name"]), DataTableUtils.toString(row["Chinese_Name"]));
                        CheckBoxList_Power.Items.Add(listItem);
                    }

                    if (CheckBoxList_Power.Items.Count > 0)
                    {
                        for (int i = 0; i < CheckBoxList_Power.Items.Count; i++)
                        {
                            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisErp);
                            sqlcmd = $"select * from SYSTEM_PMR where USER_ACC='{selected_user_acc}' and FUNC_DES='{CheckBoxList_Power.Items[i].Value}' and FUNC_YN = 'Y' ";
                            dt = DataTableUtils.GetDataTable(sqlcmd);
                            if (HtmlUtil.Check_DataTable(dt))
                                CheckBoxList_Power.Items[i].Selected = true;

                        }
                    }
                }

            }
        }
        protected void Button_right_Click(object sender, EventArgs e)
        {

            string msg = "";
            string sqlcmd = "";
            int ran = myclass.get_ran_id();
            DataTableUtils.Conn_String = myclass.GetConnByDekVisErp;
            DataTableUtils.Delete_Record("WEB_USER", "user_acc = '" + selected_user_acc + "'");
            DataTableUtils.Conn_String = myclass.GetConnByDekVisErp;
            DataTableUtils.Delete_Record("show_page", "account = '" + selected_user_acc + "'");
            int i = 0;
            DataTable dt = DataTableUtils.GetDataTable($"select * from web_user where USER_ACC = '{selected_user_acc}'");

            if (dt != null)
            {
                foreach (Control cbxlist in this.Panel_contr.Controls)
                {
                    if (cbxlist is CheckBoxList)
                    {
                        foreach (ListItem li in ((CheckBoxList)cbxlist).Items)
                        {
                            if (li.Selected == true)
                            {

                                ran = ran + 1;
                                DataRow row = dt.NewRow();
                                row["WU_ID"] = "WU" + ran;
                                row["WB_URL"] = DataTableUtils.toString(li.Value);
                                row["USER_ACC"] = selected_user_acc;
                                row["VIEW_NY"] = "Y";
                                GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisErp);
                                if (DataTableUtils.Insert_DataRow("web_user", row))
                                    i++;

                                GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisErp);
                                //新增至show_page
                                sqlcmd = "select * from show_page ";
                                DataTable ds = DataTableUtils.GetDataTable(sqlcmd);
                                if (ds != null)
                                {
                                    //20221202 show_page 不使用空白Tablerow 會自動產生ID,導致寫入錯誤 by 秋雄
                                    DataTable ds_NoRow = DataTableUtils.DataTable_TableNoRow("show_page");
                                    DataRow rsw = ds_NoRow.NewRow();
                                    rsw["URL"] = DataTableUtils.toString(li.Value);
                                    rsw["account"] = selected_user_acc;
                                    rsw["Allow"] = "Y";
                                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisErp);
                                    DataTableUtils.Insert_DataRow("show_page", rsw);

                                }

                            }
                        }
                    }
                }

            }




            for (int x = 0; x < CheckBoxList_Power.Items.Count; x++)
            {
                //先取得ID
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisErp);
                sqlcmd = $"select * from SYSTEM_PMR order by WP_ID desc  ";
                dt = DataTableUtils.GetDataTable(sqlcmd);
                int ID = DataTableUtils.toInt(DataTableUtils.toString(dt.Rows[0]["WP_ID"]).Split('_')[1]) + 1;


                GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisErp);
                sqlcmd = $"select * from SYSTEM_PMR where USER_ACC = '{selected_user_acc}' and FUNC_DES = '{CheckBoxList_Power.Items[x].Value}' ";
                dt = DataTableUtils.GetDataTable(sqlcmd);
                DataRow row = dt.NewRow();
                //裡面有資料的情況
                if (dt != null && dt.Rows.Count > 0)
                {
                    row["WP_ID"] = DataTableUtils.toString(dt.Rows[0]["WP_ID"]);
                    switch (CheckBoxList_Power.Items[x].Selected)
                    {
                        case true:
                            row["FUNC_YN"] = "Y";
                            break;
                        case false:
                            row["FUNC_YN"] = "N";
                            break;
                    }
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisErp);
                    bool ok = DataTableUtils.Update_DataRow("SYSTEM_PMR", $" WP_ID='{DataTableUtils.toString(row["WP_ID"])}' ", row);
                }
                else if (dt != null && dt.Rows.Count == 0)
                {
                    row["WP_ID"] = "WP_" + ID;
                    row["USER_ACC"] = selected_user_acc;
                    row["FUNC_DES"] = CheckBoxList_Power.Items[x].Value;
                    switch (CheckBoxList_Power.Items[x].Selected)
                    {
                        case true:
                            row["FUNC_YN"] = "Y";
                            break;
                        case false:
                            row["FUNC_YN"] = "N";
                            break;
                    }
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisErp);
                    bool ok = DataTableUtils.Insert_DataRow("SYSTEM_PMR", row);

                }
            }



            msg = selected_user_acc + "已異動! 可瀏覽頁面數 : " + i + "頁";
            Response.Write("<script language='javascript'>alert('伺服器回應 : " + msg + "');</script>");
            //20221228 送出後重整避免使用者瀏覽器重整,重複動作
            string password = DataTableUtils.toString(HttpContext.Current.Request.QueryString["password_"]);
            string url = $"rights_mag_details.aspx?password_={password}";
            Server.Transfer(url);
            //load_page_data();
        }
        protected void Button_info_Click(object sender, EventArgs e)
        {

            string tablename = "users";
            string sql_condi = $"USER_ACC = '{selected_user_acc}'";
            bool OK = false;
            string sqlcmd = $"SELECT * FROM USERS where USER_NUM = '{TextBox_num.Text}' and USER_ACC != '{selected_user_acc}'";
            DataTable dr = DataTableUtils.DataTable_GetTable(sqlcmd);
            if (dr.Rows.Count <= 0)
            {
                DataTableUtils.Conn_String = myclass.GetConnByDekVisErp;
                DataRow row = DataTableUtils.DataTable_GetDataRow(tablename, sql_condi);
                if (row != null)
                {
                    row["USER_NAME"] = TextBox_fullname.Text.Trim();
                    row["USER_BIRTHDAY"] = TextBox_Birth.Text.Replace("-", "");
                    row["USER_EMAIL"] = TextBox_email.Text.Trim();
                    row["USER_DPM"] = DropDownList_dmt.SelectedItem.Value;
                    row["USER_NUM"] = TextBox_num.Text.Trim();
                    OK = DataTableUtils.Update_DataRow(tablename, sql_condi, row);
                    if (OK == true)
                        Response.Write("<script language='javascript'>alert('伺服器回應 : 基本資料修改成功!');</script>");
                    else
                        Response.Write("<script language='javascript'>alert('伺服器回應 : 無法更新資料!請重新檢查您的項目或聯絡德科人員。');</script>");
                }
            }
            else
                Response.Write("<script language='javascript'>alert('伺服器回應 : 此手機號碼已被登錄註冊! 請輸入其它號碼!');</script>");
            load_page_data();
        }
        protected void Button_pwd_Click(object sender, EventArgs e)
        {

            string newpwd = dekSecure.dekHashCode.SHA256(TextBox_pwd1.Text);
            string tablename = "users";
            string sql_condi = "USER_ACC = '" + selected_user_acc + "'";
            bool OK = false;
            DataTableUtils.Conn_String = myclass.GetConnByDekVisErp;
            DataRow row = DataTableUtils.DataTable_GetDataRow(tablename, sql_condi);
            if (row != null)
            {
                row["USER_PWD"] = newpwd;
                OK = DataTableUtils.Update_DataRow(tablename, sql_condi, row);
                if (OK == true)
                    Response.Write("<script>alert('伺服器回應 : 密碼修改成功!請使用新密碼重新登入帳號!');</script>");
                else
                    Response.Write("<script language='javascript'>alert('伺服器回應 : 無法更新資料!請重新檢查您的項目或聯絡德科人員。');</script>");
            }
            else
                Response.Write("<script language='javascript'>alert('伺服器回應 : 原密碼錯誤!請重新檢查您的項目或聯絡德科人員。');</script>");
            load_page_data();
        }
    }
}