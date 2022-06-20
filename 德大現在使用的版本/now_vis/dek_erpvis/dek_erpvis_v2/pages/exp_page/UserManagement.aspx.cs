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
    public partial class UserManagement : System.Web.UI.Page
    {
        public string time_array = "";
        public string th = "";
        public string tr = "";
        public string col_data_Points = "";
        public string data_name = "";
        public string title_text = "";
        public string user_area_text = "使用者帳號列表";
        string TOP = "";
        string URL_NAME = "";
        string acc = "";

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
                Response.BufferOutput = false;
                set_table_title();
            }
            else
            {
                Response.Write("<script language='javascript'>alert('伺服器回應 : 無法載入資料，" + clsDB_Vis.ErrorMessage + " 請聯絡德科人員或檢查您的網路連線。');</script>");
                無資料處理();
            }
        }
        private void set_table_title()
        {
            string sqlcmd = 德大機械.帳號管理.使用者帳號();
            public_dt = clsDB_Vis.DataTable_GetTable(sqlcmd);
            clsDB_Vis.dbOpen(myclass.GetConnByDekVisErp);
            if (public_dt.Rows.Count > 0)
            {
                //title
                for (int i = 0; i < public_dt.Columns.Count; i++)
                {
                     string col_name = public_dt.Columns[i].ColumnName;
                     th += "<th>" + col_name + "</th>\n";
                }
                th += "<th> 登入紀錄 </th>\n";
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
            if (clsDB_Vis.IsConnected == true )//&& myclass.user_view_check(URL_NAME, acc) == true)
            {
                if (public_dt != null)
                {
                    if (public_dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in public_dt.Rows)
                        {
                            tr += "<tr>\n";
                            tr += "<td>" + DataTableUtils.toString(row["使用者名稱"]) + "</td>\n";
                            tr += "<td>" + DataTableUtils.toString(row["使用者帳號"]) + "</td>\n";
                            tr += "<td>" + DataTableUtils.toString(row["部門別"]) + "</td>\n";
                            tr += "<td>" + DataTableUtils.toString(row["信箱"]) + "</td>\n";
                            tr += "<td>" + DataTableUtils.toString(row["生日"]) + "</td>\n";
                            tr += "<td>" + DataTableUtils.toString(row["電話"]) + "</td>\n";
                            tr += "<td>" + DataTableUtils.toString(row["狀態"]) + "</td>\n";
                            tr += "<td>" + DataTableUtils.toString(row["ADM"]) + "</td>\n";
                            tr += "<td>" + DataTableUtils.toString(row["建立時間"]) + "</td>\n";
                            tr += "<td>" + DataTableUtils.toString(row["登入次數"]) + "</td>\n";
                            tr += "<td><u><a href='UserManagementDetail.aspx?USER_ACC=" + DataTableUtils.toString(row["使用者帳號"]) + "' target='_blank'> 查看 </u></td>\n";
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
                    public_dt.Dispose();
                    public_dt.Clear();
                }
                else
                {
                    Response.Redirect(myclass.logout_url);
                }
            }
            return "";
        }
        private void 無資料處理()
        {
            title_text = "'沒有資料'";
            th = "<th class='center'>沒有資料載入</th>";
            tr = "<tr class='even gradeX'> <td class='center'> no data </ td ></ tr >";
        }
        protected void button_select_Click(object sender, EventArgs e)
        {
            //get_search_time(DataTableUtils.toString(((Control)sender).ID.Split('_')[1]));
            //GotoCenn();
            txt_time_str.Value = "";
            txt_time_end.Value = "";
        }

    }
}