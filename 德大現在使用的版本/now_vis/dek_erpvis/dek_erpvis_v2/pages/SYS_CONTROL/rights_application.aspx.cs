﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Support;
using dek_erpvis_v2.cls;
using System.Data;

namespace dek_erpvis_v2.pages.SYS_CONTROL
{
    public partial class rights_application : BasePage//System.Web.UI.Page
    {
        public string color = "";
        public string page_name = "權限申請列表";
        public string notice = "您可以在此審核用戶提出的瀏覽申請。";
        public string title_text = "";
        public string th = "";
        public string tr = "";
        string acc = "";
        string adm = "";
        clsDB_Server clsDB_sw = new clsDB_Server("");
        myclass myclass = new myclass();
        DataTable dt = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                color = HtmlUtil.change_color(acc);
                adm = myclass.check_user_power(acc);
                if (adm == "Y")
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
                Response.Write("<script language='javascript'>alert('伺服器回應 : 無法載入資料，" + clsDB_sw.ErrorMessage + " 請聯絡德科人員或檢查您的網路連線。');</script>");
                無資料處理();
            }
        }
        private void 無資料處理()
        {
            title_text = "'沒有資料載入'";
            th = "<th class='center'>沒有資料載入</th>";
            tr = "<tr class='even gradeX'> <td class='center'> no data </ td ></ tr >";
        }
        private void load_page_data()
        {

            clsDB_sw.dbOpen(myclass.GetConnByDekVisErp);
            string sqlcmd = 德大機械.控制台_權限管理.取得申請瀏覽列表();
            dt = clsDB_sw.DataTable_GetTable(sqlcmd);
            if (dt.Rows.Count <= 0)
                notice = "目前尚無會員提出瀏覽申請。";
        }
        public string set_table_content()
        {
            clsDB_sw.dbOpen(myclass.GetConnByDekVisErp);
            if (clsDB_sw.IsConnected == true && adm == "Y")
            {
                int i = 0;
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        string dpm = DataTableUtils.toString(row["所屬部門"]);
                        string name = DataTableUtils.toString(row["用戶名稱"]);
                        string acc = DataTableUtils.toString(row["用戶帳號"]);
                        string page = DataTableUtils.toString(row["申請頁面"]);
                        string url = DataTableUtils.toString(row["頁面網址"]);
                        tr += get_table_content(dpm, name, acc, page, url, i++);
                    }
                    Response.Write(tr);
                    Response.Flush();
                    Response.Clear();
                    tr = "";
                }
                else
                    無資料處理();
                dt.Dispose();
            }
            else
                無資料處理();
            return "";
        }

        private string get_table_content(string dpm, string name, string acc, string page, string url, int count)
        {
            //content
            string id = acc + "&" + url;
            string num_type = "even";
            if (count % 2 == 0) num_type = "odd";
            string tr = "<tr class='" + num_type + "pointer'>";
            tr += "<td class=\"a-center \">";
            tr += "<input id='" + id + "' name='table_records' value='" + id + "'   type=\"checkbox\" class=\"flat\">";
            tr += "</td>";
            tr += "<td>" + dpm + "</td>";
            tr += "<td>" + name + "</td>";
            tr += "<td>" + acc + "</td>";
            tr += "<td>" + page + "</td>";
            tr += "</tr>";

            return tr;
        }

        protected void Button_submit_Click(object sender, EventArgs e)
        {           
            int available = 0;
            string tabl_name = "WEB_USER";
            string seleted_item = Request.Form["table_records"];
            DataTableUtils.Conn_String = myclass.GetConnByDekVisErp;

            for (int i = 0; i < seleted_item.Split(',').Length; i++)
            {
                string id = seleted_item.Split(',')[i].Split('&')[0];
                string url = seleted_item.Split(',')[i].Split('&')[1];
                string condition = "USER_ACC ='" + id + "' and WB_URL = '" + url + "' ";

                DataRow row = DataTableUtils.DataTable_GetDataRow(tabl_name, condition);
                row["VIEW_NY"] = "Y";
                if (DataTableUtils.Update_DataRow(tabl_name, condition, row) == true)
                    available++;

                GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisErp);

                //新增至show_page //20220816 無法insert,新版帳號無須inser show_page暫不修正
                //舊版-資料庫為空的話,永遠無法新增資料
                //string sqlcmd = "select * from show_page ";
                //DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
                //if (dt != null)
                //{
                //    DataTable dt_NoRow = DataTableUtils.DataTable_TableNoRow("show_page");
                //    DataRow rsw = dt_NoRow.NewRow();
                //    rsw["URL"] = url;
                //    rsw["account"] = id;
                //    rsw["Allow"] = "Y";
                //    GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisErp);
                //    bool success=DataTableUtils.Insert_DataRow("show_page", rsw);

                //}
                //20221228 新-不判斷資料庫直接直接新增
                DataTable dt_NoRow = DataTableUtils.DataTable_TableNoRow("show_page");
                DataRow rsw = dt_NoRow.NewRow();
                rsw["URL"] = url;
                rsw["account"] = id;
                rsw["Allow"] = "Y";
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisErp);
                bool success = DataTableUtils.Insert_DataRow("show_page", rsw);
            }
            Response.Write("<script language='javascript'>alert('伺服器回應 : 已審核 " + available + " 筆申請資料。');</script>");
            //20221228 送出後重整避免使用者瀏覽器重整,重複動作
            Response.Redirect("rights_application.aspx", false);
            //GotoCenn();

        }

        protected void Button_delete_Click(object sender, EventArgs e)
        {

            int available = 0;
            string tabl_name = "WEB_USER";
            string seleted_item = Request.Form["table_records"];
            DataTableUtils.Conn_String = myclass.GetConnByDekVisErp;

            for (int i = 0; i < seleted_item.Split(',').Length; i++)
            {
                string id = seleted_item.Split(',')[i].Split('&')[0];
                string url = seleted_item.Split(',')[i].Split('&')[1];
                string condition = "USER_ACC ='" + id + "' and WB_URL = '" + url + "' ";
                if (DataTableUtils.Delete_Record(tabl_name, condition) == true)
                    available++;
            }
            Response.Write("<script language='javascript'>alert('伺服器回應 : 已刪除 " + available + " 筆申請資料。');</script>");
            //20221228 送出後重整避免使用者瀏覽器重整,重複動作
            Response.Redirect("rights_application.aspx", false);
            //GotoCenn();
        }

    }
}