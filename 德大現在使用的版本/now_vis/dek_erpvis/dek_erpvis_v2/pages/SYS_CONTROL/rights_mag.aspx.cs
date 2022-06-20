using dek_erpvis_v2.cls;
using Support;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace dek_erpvis_v2.pages.sys_control
{
    public partial class rights_mag : System.Web.UI.Page
    {
        public string color = "";
        public string th = "";
        public string tr = "";
        public string title_text = "";
        public string type = "";
        public string type_code = "";
        public string title = "";
        public string search_condi = "";
        public string safty_text = "";
        public string min_text = "";
        public string chart_card_text = "";
        string acc = "";
        string adm = "";
        clsDB_Server clsDB_sw = new clsDB_Server("");
        myclass myclass = new myclass();
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                color = HtmlUtil.change_color(acc);
                adm = myclass.check_user_power(acc);
                if (adm =="Y")
                    GotoCenn();
                else
                    Response.Write("<script>alert('您無法瀏覽此頁面 請向該單位主管申請權限!');location.href='../index.aspx';</script>");
            }
            else
                Response.Redirect(myclass.logout_url);
        }
        private void GotoCenn()
        {
            clsDB_sw.dbOpen(myclass.GetConnByDekVisErp);
            if (clsDB_sw.IsConnected == true)
                load_page_data();
            else
            {
                Response.Write($"<script language='javascript'>alert('伺服器回應 : 無法載入資料，{ clsDB_sw.ErrorMessage} 請聯絡德科人員或檢查您的網路連線。');</script>");
                無資料處理();
            }
        }
        private void 無資料處理()
        {
            title_text = "'沒有資料載入'";
            th = "<th class='center'>沒有資料載入</th>";
            tr = "<tr class='even gradeX'> <td class='center'> no data </td></tr>";
        }
        private void load_page_data()
        {
            Response.BufferOutput = false;
            set_table_title();
            clsDB_sw.dbOpen(myclass.GetConnByDekVisErp);
        }
        private void set_table_title()
        {
            th = "<th>用戶名稱</th>\n<th>所屬部門</th>\n<th>用戶帳號</th>\n<th>狀態</th><th>建立時間</th><th>操作</th>\n";
        }

        public string set_table_content()
        {
            clsDB_sw.dbOpen(myclass.GetConnByDekVisErp);
            if (clsDB_sw.IsConnected == true && adm == "Y")
            {
                clsDB_sw.dbOpen(myclass.GetConnByDekVisErp);
                string sqlcmd = "SELECT USER_ACC,USER_NAME,DEPARTMENT.DPM_NAME2,STATUS,ADD_TIME FROM USERS left join DEPARTMENT on USERS.USER_DPM = DEPARTMENT.DPM_NAME";
                DataTable dt = clsDB_sw.DataTable_GetTable(sqlcmd);
                if (HtmlUtil.Check_DataTable(dt))
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        string url = myclass.Base64Encode("user_acc=" + DataTableUtils.toString(row["USER_ACC"]) + ",user_name=" + DataTableUtils.toString(row["USER_NAME"]) + "");
                        tr += "<tr>\n";
                        tr += "<td><u><a href='rights_mag_details.aspx?password_="+ url + "' target='_blank'>" + DataTableUtils.toString(row["USER_NAME"]) + " </u></td>\n";
                        tr += "<td>" + DataTableUtils.toString(row["DPM_NAME2"]) + "</td>\n";
                        tr += "<td>" + DataTableUtils.toString(row["USER_ACC"]) + "</td>\n";
                        tr += "<td>" + DataTableUtils.toString(row["STATUS"]) + "</td>\n";
                        tr += "<td>" + DataTableUtils.toString(row["ADD_TIME"]) + "</td>\n";
                        //tr += "<td><button id='"+ DataTableUtils.toString(row["USER_NAME"]) + "' type=\"button\" class=\"btn btn-default\" onclick=\"button_delete()\">刪除</button></ td>\n";

                        tr += "<td><button id='" + DataTableUtils.toString(row["USER_NAME"]) + "' type=\"button\" class=\"btn btn-danger btn-s dt-delete\" onclick=\"button_delete('" + myclass.Base64Encode(adm +","+ acc + "," +DataTableUtils.toString(row["USER_ACC"])) + "')\"><span class =\"glyphicon glyphicon-remove\" aria-hidden='true'></span> 刪除</button></ td>\n";

                        tr += "</tr>\n";
                    }
                    Response.Write(tr);
                    Response.Flush();
                    Response.Clear();
                    tr = "";
                }
                dt.Dispose();
            }
            else
                無資料處理();
            return "";
        }
    }

}