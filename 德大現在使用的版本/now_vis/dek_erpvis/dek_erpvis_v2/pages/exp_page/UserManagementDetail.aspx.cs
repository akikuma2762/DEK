using dek_erpvis_v2.cls;
using Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace dek_erpvis_v2.pages.exp_page
{
    public partial class UserManagementDetail : System.Web.UI.Page
    {
        public string time_array = "";
        public string th = "";
        public string tr = "";
        public string col_data_Points = "";
        public string data_name = "";
        public string title_text = "";
        public string user_area_text = "";
        string TOP = "";
        string URL_NAME = "";
        string acc = "";
        string Account = "";

        DataTable public_dt = null;
        clsDB_Server clsDB_Vis = new clsDB_Server("");
        myclass myclass = new myclass();
        德大機械 德大機械 = new 德大機械();
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                URL_NAME = "UserManagement";
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                //if (myclass.user_view_check(URL_NAME, acc) == true)
                //{
                if (!IsPostBack)
                {
                    GotoCenn();
                }
                //}
                //else
                //{
                //    Response.Write("<script>alert('您無法瀏覽此頁面 請向該單位主管申請權限!');location.href='../index.aspx';</script>");
                //Response.Redirect(myclass.logout_url);
                //}
            }
            else
            {
                Response.Redirect(myclass.logout_url);
            }
        }
        private void GotoCenn()
        {
            clsDB_Vis.dbOpen(myclass.GetConnByDekVisErp);
            if (clsDB_Vis.IsConnected == true)
            {
                load_page_data();
            }
            else
            {
                Response.Write("<script language='javascript'>alert('伺服器回應 : 無法載入資料，" + clsDB_Vis.ErrorMessage + " 請聯絡德科人員或檢查您的網路連線。');</script>");
                無資料處理();
            }
        }
        private void load_page_data()
        {
            Response.BufferOutput = false;
            if (Request.QueryString["USER_ACC"] != null)
            {
                Account = Request.QueryString["USER_ACC"];                
                Response.BufferOutput = false;
                //set_col_value();
                set_table_title();
            }
            else
            {
                Response.Redirect("UserManagement.aspx", false);
            }
        }
        private void set_table_title()
        {
            string sqlcmd = 德大機械.帳號管理.使用登入紀錄(Account);
            public_dt = clsDB_Vis.DataTable_GetTable(sqlcmd);
            clsDB_Vis.dbOpen(myclass.GetConnByDekVisErp);
            user_area_text = "使用者 " + DataTableUtils.toString(public_dt.Rows[0]["使用者名稱"]) + " 登入列表";
            if (public_dt.Rows.Count > 0)
            {
                //title
                for (int i = 0; i < public_dt.Columns.Count; i++)
                {
                    string col_name = public_dt.Columns[i].ColumnName;
                    th += "<th>" + col_name + "</th>\n";
                }
            }
            else
            {
                無資料處理();
            }
            public_dt.Dispose();
        }
        public string set_table_content()
        {
            tr = "";
            //content
            if (public_dt != null)
            {
                if (public_dt.Rows.Count > 0)
                {
                    foreach (DataRow row in public_dt.Rows)
                    {
                        tr += "<tr>\n";
                        tr += "<td>" + DataTableUtils.toString(row["使用者名稱"]) + "</td>\n";
                        tr += "<td>" + DataTableUtils.toString(row["使用者帳號"]) + "</td>\n";
                        tr += "<td>" + DataTableUtils.toString(row["登入時間"]) + "</td>\n";
                        tr += "<td>" + DataTableUtils.toString(row["IP位址"]) + "</td>\n";
                        tr += "</tr>\n";
                        Response.Write(tr);
                        Response.Flush();
                        Response.Clear();
                        tr = "";
                    }
                }
                else
                {
                    無資料處理();
                }
            }
            else
            {
                Response.Redirect("materialrequirementplanning.aspx", false);
            }
            return "";
        }
        private void 無資料處理()
        {
            title_text = "'沒有資料'";
            th = "<th class='center'>沒有資料載入</th>";
            tr = "<tr class='even gradeX'> <td class='center'> no data </ td ></ tr >";
        }
    }
}