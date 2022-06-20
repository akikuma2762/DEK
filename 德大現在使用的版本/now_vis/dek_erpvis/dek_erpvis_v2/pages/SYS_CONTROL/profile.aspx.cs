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
    public partial class profile : BasePage//System.Web.UI.Page
    {
        public string color = "";
        string acc = "";
        public string th = "";
        public string tr = "";
        public string view_page = "";
        public string page_name = "個人檔案管理";
        myclass myclass = new myclass();
        clsDB_Server clsDB_sw = new clsDB_Server("");
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                color = HtmlUtil.change_color(acc);
                if (!IsPostBack)
                {
                    GotoCenn();
                    iniDropdownlist();
                }
            }
            else
                Response.Redirect(myclass.logout_url);
        }
        private void GotoCenn()
        {
            clsDB_sw.dbOpen(myclass.GetConnByDekVisErp);
            if (clsDB_sw.IsConnected == true)
            {
                load_page_data();
                Label_code.Text = new Random().Next(9999).ToString();
                TextBox_code.Text = "";
            }
            else
            {
                Response.Write($"<script language='javascript'>alert('伺服器回應 : 無法載入資料，{clsDB_sw.ErrorMessage} 請聯絡德科人員或檢查您的網路連線。');</script>");
                無資料處理();
            }
        }
        private void 無資料處理()
        {
            /*title_text = "'沒有資料載入'";
            th = "<th class='center'>沒有資料載入</th>";
            tr = "<tr class='even gradeX'> <td class='center'> no data </ td ></ tr >";*/
        }
        private void iniDropdownlist()
        {
            clsDB_sw.dbOpen(myclass.GetConnByDekVisErp);
            string sqlcmd = 德大機械.控制台_權限管理.取得部門("SYS");
            DataTable dt = clsDB_sw.GetDataTable(sqlcmd);
            if (HtmlUtil.Check_DataTable(dt))
            {
                sqlcmd = 德大機械.控制台_權限管理.取得用戶個人資訊(acc);
                DataTable dr = clsDB_sw.GetDataTable(sqlcmd);

                string dpm = DataTableUtils.toString(dr.Rows[0]["USER_DPM"]);
                ListItem li = new ListItem("--請選擇--", "NULL");
                DropDownList_dmt.Items.Add(li);
                foreach (DataRow row in dt.Rows)
                {
                    li = new ListItem(DataTableUtils.toString(row["DPM_NAME2"]), DataTableUtils.toString(row["DPM_NAME"]));
                    if (DataTableUtils.toString(row["DPM_NAME"]) == dpm)
                        li.Selected = true;
                    DropDownList_dmt.Items.Add(li);
                }
                dr.Clear();
                dr.Dispose();
            }
            dt.Clear();
            dt.Dispose();
        }
        private void load_page_data()
        {
            clsDB_sw.dbOpen(myclass.GetConnByDekVisErp);
            string sqlcmd = 德大機械.控制台_權限管理.取得用戶個人資訊(acc);
            DataTable dt = clsDB_sw.GetDataTable(sqlcmd);
            if (HtmlUtil.Check_DataTable(dt))
            {
                int j = 0;
                string name = DataTableUtils.toString(dt.Rows[0]["USER_NAME"]);
                string number = DataTableUtils.toString(dt.Rows[0]["USER_NUM"]); if (number == "") number = "---";
                string email = DataTableUtils.toString(dt.Rows[0]["USER_EMAIL"]); if (email == "") email = "---";
                //string birthday = DataTableUtils.toString(dt.Rows[0]["USER_BIRTHDAY"]); if (birthday == "") birthday = "---";
                string dpm = DataTableUtils.toString(dt.Rows[0]["DPM_NAME2"]); if (dpm == "") dpm = "---";
                string ADM = DataTableUtils.toString(dt.Rows[0]["ADM"]); if (ADM == "Y") { ADM = "(系統管理員)"; } else { ADM = "(一般會員)"; }

                Label_ADM.Text = ADM;
                Label_name.Text = name;
                Label_email.Text = email;
                //Label_birthday.Text = birthday;20191115總經理說不需要
                Label_dpm.Text = dpm;
                Label_number.Text = number;
                TextBox_fullname.Text = DataTableUtils.toString(dt.Rows[0]["USER_NAME"]);
                TextBox_Birth.Text = HtmlUtil.changetimeformat(DataTableUtils.toString(dt.Rows[0]["USER_BIRTHDAY"]), "-");
                //              TextBox_birthday.Value = ;
                TextBox_email.Text = DataTableUtils.toString(dt.Rows[0]["USER_EMAIL"]);
                TextBox_num.Text = DataTableUtils.toString(dt.Rows[0]["USER_NUM"]);

                sqlcmd = 德大機械.控制台_權限管理.取得部門_依網頁();
                dt = clsDB_sw.GetDataTable(sqlcmd);
                foreach (DataRow row in dt.Rows)
                {
                    sqlcmd = 德大機械.控制台_權限管理.取得選取用戶的權限詳細(acc, DataTableUtils.toString(row["WEB_DPM"]));
                    dt = clsDB_sw.GetDataTable(sqlcmd);
                    if (HtmlUtil.Check_DataTable(dt))
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string USER_ACC = DataTableUtils.toString(dt.Rows[i]["USER_ACC"]);
                            string VIEW_NY = DataTableUtils.toString(dt.Rows[i]["VIEW_NY"]);
                            string DPM_NAME2 = DataTableUtils.toString(row["DPM_NAME2"]);
                            string WEB_PAGENAME = DataTableUtils.toString(dt.Rows[i]["WEB_PAGENAME"]);
                            string WEB_URL = DataTableUtils.toString(dt.Rows[i]["WEB_URL"]);
                            string url = myclass.Base64Encode(acc + "," + WEB_URL);

                            tr += "<tr>\n";
                            if (USER_ACC != "" && VIEW_NY == "Y")
                            {
                                tr += "<td><button id= \"submit_btn" + (j++) + "\" type=\"button\" class=\"btn btn-round btn-default\" disabled>審核通過</button></td>\n";
                                tr += "<td>" + DPM_NAME2 + "</td>\n";
                                tr += "<td>" + WEB_PAGENAME + "</td>\n";
                            }
                            else
                            {
                                if (USER_ACC != "" && VIEW_NY == "N")
                                    tr += "<td style='color:red;'><button id= \"submit_btn" + (j++) + "\" type=\"button\" class=\"btn btn-round btn-warning disabled\">待審核中</button></td>\n";
                                else
                                    tr += "<td style='color:red;'><button id= \"submit_btn" + (j++) + "\" type=\"button\" class=\"btn btn-round btn-danger\" onclick=\"submit_click(this.id,'" + url + "')\">點擊申請</button></td>\n";
                                tr += "<td style='color:red;'><strong>" + DPM_NAME2 + "</strong></td>\n";
                                tr += "<td style='color:red;'><strong>" + WEB_PAGENAME + "</strong></td>\n";
                            }
                            tr += "</tr>\n";
                        }

                    }
                }
            }
        }

        protected void Button_info_Click(object sender, EventArgs e)
        {

            string tablename = "users";
            string sql_condi = "USER_ACC = '" + acc + "'";
            bool OK = false;
            clsDB_sw.dbOpen(myclass.GetConnByDekVisErp);
            if (clsDB_sw.IsConnected == true)
            {
                string sqlcmd = "SELECT * FROM USERS where USER_NUM = '" + TextBox_num.Text.ToString() + "' and USER_ACC != '" + acc + "'";
                DataTable dr = clsDB_sw.DataTable_GetTable(sqlcmd);
                if (dr.Rows.Count <= 0)
                {
                    DataTableUtils.Conn_String = myclass.GetConnByDekVisErp;
                    DataRow row = DataTableUtils.DataTable_GetDataRow(tablename, sql_condi);
                    if (row != null)
                    {
                        row["USER_NAME"] = TextBox_fullname.Text.Trim();
                        row["USER_BIRTHDAY"] = TextBox_Birth.Text.Replace("-", "");
                        row["USER_EMAIL"] = TextBox_email.Text.Trim();
                        row["USER_DPM"] = DropDownList_dmt.SelectedValue;
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
            }

            GotoCenn();
        }
        protected void Button_pwd_Click(object sender, EventArgs e)
        {

            string oldpwd = dekSecure.dekHashCode.SHA256(TextBox_oldpwd.Text);
            string newpwd = dekSecure.dekHashCode.SHA256(TextBox_pwd1.Text);
            string tablename = "users";
            string sql_condi = "USER_ACC = '" + acc + "'";
            bool OK = false;
            clsDB_sw.dbOpen(myclass.GetConnByDekVisErp);
            if (clsDB_sw.IsConnected == true)
            {
                DataTableUtils.Conn_String = myclass.GetConnByDekVisErp;
                DataRow row = DataTableUtils.DataTable_GetDataRow(tablename, sql_condi);
                if (row != null)
                {
                    if (DataTableUtils.toString(row["USER_PWD"]) == oldpwd)
                    {
                        row["USER_PWD"] = newpwd;
                        OK = DataTableUtils.Update_DataRow(tablename, sql_condi, row);
                        if (OK == true)
                        {
                            HttpCookie userInfo = Request.Cookies["userInfo"];
                            if (userInfo != null)
                            {
                                userInfo = new HttpCookie("userInfo");
                                userInfo.Expires = DateTime.Now.AddDays(-1d);
                                userInfo.Values.Clear();
                                Response.Cookies.Add(userInfo);
                            }
                            Response.Write("<script>alert('伺服器回應 : 密碼修改成功!請使用新密碼重新登入帳號!');window.location.reload()</script>");
                        }
                        else
                        {
                            Response.Write("<script language='javascript'>alert('伺服器回應 : 無法更新資料!請重新檢查您的項目或聯絡德科人員。');</script>");
                            GotoCenn();
                        }
                    }
                    else
                    {
                        Response.Write("<script language='javascript'>alert('伺服器回應 : 原密碼錯誤!請重新檢查您的項目或聯絡德科人員。');</script>");
                        GotoCenn();
                    }

                }
            }
        }
    }
}