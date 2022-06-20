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
    public partial class PMD_Upload : System.Web.UI.Page
    {

        public string Location = "";
        public string information = "";
        public string get_js = "";
        public string color = "";
        string date_str = "";
        string date_end = "";
        string acc = "";
        public string th = "";
        public string tr = "";
        myclass myclass = new myclass();
        德大機械 德大機械 = new 德大機械();
        //-------------------------------事件----------------------------
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                color = HtmlUtil.change_color(acc);

                if (HtmlUtil.Search_acc_Column(acc) == "Y" )
                {
                    try
                    {
                        if (TextBox_textTemp.Text != "")
                        {
                            if (Request.QueryString["key"] != null)
                            {
                                string[] type = HtmlUtil.Return_str(Request.QueryString["key"]);
                                string[] value = TextBox_textTemp.Text.Split('_');
                                if (value[0] == "ed")
                                    all_information(type[1], value[1] + "-" + value[2], value[3]);
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    Check_Request();
                }
                else
                    Response.Write("<script>alert('您無法瀏覽此頁面 請向該單位主管申請權限!');location.href='../index.aspx';</script>");
            }
            else Response.Redirect(myclass.logout_url);
        }
        //後端產生的按鈕事件
        protected void button_search_Click(object sender, EventArgs e)
        {
            string[] value = TextBox_textTemp.Text.Split('_');
            //跳出小視窗給使用者編輯
            if (value[0] == "ed")
            {
            }
            //刪除
            else
            {
                string[] type = HtmlUtil.Return_str(Request.QueryString["key"]);
                delete_information(type[1], value[1] + "-" + value[2], value[3]);
            }
        }
        //搜尋日期&變更廠區
        protected void button_select_Click(object sender, EventArgs e)
        {
            string[] s = 德大機械.德大專用月份(acc).Split(',');
            HtmlUtil.Button_Click(DataTableUtils.toString(((Control)sender).ID.Split('_')[1]), s, DataTableUtils.toString(txt_time_str.Value), DataTableUtils.toString(txt_time_end.Value), out date_str, out date_end);
            if (DataTableUtils.toString(((Control)sender).ID.Split('_')[1]) != "select")
            {
                txt_time_str.Value = "";
                txt_time_end.Value = "";
            }
            string url = "type=" + RadioButtonList_Type.SelectedValue + ",date_str=" + date_str + ",date_end=" + date_end;
            Response.Redirect("PMD_Upload_old.aspx?key=" + WebUtils.UrlStringEncode(url));
        }
        //更新資料
        protected void Button_Update_Click(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            if (PlaceHolder_information.Controls.Count > 0)
            {
                foreach (Control tbx in this.PlaceHolder_information.Controls)
                {
                    if (tbx is TextBox)
                    {
                        TextBox tx = tbx as TextBox;
                        list.Add(tx.Text);
                    }
                }
                if (Request.QueryString["key"] != null)
                {
                    string[] type = HtmlUtil.Return_str(Request.QueryString["key"]);
                    string[] value = TextBox_textTemp.Text.Split('_');
                    update_information(type[1], value[1] + "-" + value[2], value[3], list);
                }
            }
        }
        //--------------------------------------方法----------------------------
        //用於檢查?後面的文字
        private void Check_Request()
        {
            if (Request.QueryString["key"] != null)
            {
                string[] str = HtmlUtil.Return_str(Request.QueryString["key"]);
                string[] date = new string[2];
                if (str[1] == "Ver")
                    Location = "立式";
                else if (str[1] == "Hor")
                    Location = "臥式";
                if (str.Length == 2)
                {
                    date[0] = DateTime.Now.Date.AddDays(-(int)(DateTime.Now.DayOfWeek) + 1).ToString("yyyyMMdd");//当前周的开始日期
                    date[1] = DateTime.Now.Date.AddDays(7 - (int)(DateTime.Now.DayOfWeek)).ToString("yyyyMMdd");//当前周的结束日期
                }
                else
                {
                    date[0] = str[3];
                    date[1] = str[5];
                }
                Read_DataTable(str[1], date);
            }
            //如果沒有文字，自動導入立式
            else
            {
                string url = "type=Ver";
                Response.Redirect("PMD_Upload_old.aspx?key=" + WebUtils.UrlStringEncode(url));
            }
        }
        //建立表格
        private void Read_DataTable(string type, string[] date)
        {
            //判斷該開啟哪個資料庫
            if (type == "Ver")
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
            else if (type == "Hor")
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);

            string sql_cmd = "Select 組裝編號,排程編號,進度,狀態,工作站編號,組裝日,實際組裝時間 From 工作站狀態資料表 where 組裝日>=" + date[0] + " and 組裝日<=" + date[1];
            DataTable dt = DataTableUtils.GetDataTable(sql_cmd);

            //增加編輯與刪除之欄位
            dt.Columns.Add("編輯");
            dt.Columns.Add("刪除");
            //把這兩個欄位移至最前面
            dt.Columns["編輯"].SetOrdinal(0);
            dt.Columns["刪除"].SetOrdinal(1);
            string th = "";
            //表格資料
            string tr = "";
            List<string> list = new List<string>();
            //依照欄位名稱跑回圈
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                th += "<th>" + dt.Columns[i] + "</th>";
                list.Add(DataTableUtils.toString(dt.Columns[i]));
            }
            list.Add("");
            //產生表格內容
            tr = HtmlUtil.Set_Table_Content(dt, list, PMD_Uploadcallback);

            Create_Table(th, tr);
        }
        //產生給前端表格
        private void Create_Table(string th, string tr)
        {
            PlaceHolder_ShowImformation.Controls.Add(new LiteralControl("<div class=\"row\">\n"));
            PlaceHolder_ShowImformation.Controls.Add(new LiteralControl("<table id=\"datatable-buttons\" class=\"table table-bordered nowrap table-ts\" cellspacing=\"0\" width=\"100%\">\n"));
            PlaceHolder_ShowImformation.Controls.Add(new LiteralControl("<thead>\n"));
            PlaceHolder_ShowImformation.Controls.Add(new LiteralControl("<tr style=\"background-color: #FFFFFF\" >\n"));
            PlaceHolder_ShowImformation.Controls.Add(new LiteralControl(th));
            PlaceHolder_ShowImformation.Controls.Add(new LiteralControl("</tr>\n"));
            PlaceHolder_ShowImformation.Controls.Add(new LiteralControl("</thead>\n"));
            PlaceHolder_ShowImformation.Controls.Add(new LiteralControl("<tbody>\n"));
            PlaceHolder_ShowImformation.Controls.Add(new LiteralControl(tr));
            PlaceHolder_ShowImformation.Controls.Add(new LiteralControl(" </tbody>\n"));
            PlaceHolder_ShowImformation.Controls.Add(new LiteralControl("</table>\n"));
            PlaceHolder_ShowImformation.Controls.Add(new LiteralControl("</div>\n"));
        }
        //callback執行處
        private string PMD_Uploadcallback(DataRow row, string field_name)
        {
            string value = "";
            string 排程編號 = DataTableUtils.toString(row["排程編號"]).Replace('-', '_');
            if (field_name == "編輯")
            {
                value = "<td style='width:7%'><b><u><a class=\"ed\" id=ed_" + 排程編號 + "_" + DataTableUtils.toString(row["工作站編號"]) + " data-toggle = \"modal\" data-target = \"#exampleModal_information\">編輯</a></u></b></td>";
                get_js += "$(\"#ed_" + 排程編號 + "_" + DataTableUtils.toString(row["工作站編號"]) + "\").click(function () {\n" +
                 "$('#ContentPlaceHolder1_TextBox_textTemp').val(\"ed_" + 排程編號 + "_" + DataTableUtils.toString(row["工作站編號"]) + "\");\n" +
                 "document.getElementById('ContentPlaceHolder1_button_search').click();\n" +
               "});\n";
            }

            if (field_name == "刪除")
            {
                value = "<td style='width:7%'><b><u><a class=\"de\" id=de_" + 排程編號 + "_" + DataTableUtils.toString(row["工作站編號"]) + ">刪除</a></u></b></td>";
                get_js += "$(\"#de_" + 排程編號 + "_" + DataTableUtils.toString(row["工作站編號"]) + "\").click(function () {\n" +
                           "var r =confirm(\"確定要刪除嗎\");" +
                           "if(r == true){" +
                            "$('#ContentPlaceHolder1_TextBox_textTemp').val(\"de_" + 排程編號 + "_" + DataTableUtils.toString(row["工作站編號"]) + "\");\n" +
                            "document.getElementById('ContentPlaceHolder1_button_delete').click();\n" +
                          "}" +
                            "});\n";

            }
            return value;
        }
        //顯示所有資訊
        private void all_information(string type, string num, string worknum)
        {
            int total = PlaceHolder_information.Controls.Count;
            //判斷該開啟哪個資料庫
            if (type == "Ver")
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
            else if (type == "Hor")
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);
            string sqlcmd = "Select 組裝編號,排程編號,進度,狀態,工作站編號,組裝日,實際組裝時間 From 工作站狀態資料表 where 排程編號='" + num + "' and 工作站編號 = '" + worknum + "'";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
            string str = "";
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        PlaceHolder_information.Controls.Add(new LiteralControl("<div class=\"col-md-12 col-sm-12 col-xs-12\">\n"));

                        Label lb = new Label();
                        lb.Text = DataTableUtils.toString(dt.Columns[i]);

                        lb.ID = DataTableUtils.toString(dt.Columns[i]);

                        PlaceHolder_information.Controls.Add(lb);
                        PlaceHolder_information.Controls.Add(new LiteralControl("</div>\n"));

                        PlaceHolder_information.Controls.Add(new LiteralControl("<div class=\"col-md-12 col-sm-12 col-xs-12\">\n"));

                        TextBox tx = new TextBox();
                        tx.Text = DataTableUtils.toString(row[dt.Columns[i]]);
                        if (str == DataTableUtils.toString(row[dt.Columns[i]]))
                            tx.ID = DataTableUtils.toString(row[dt.Columns[i]]) + 1;
                        else
                            tx.ID = DataTableUtils.toString(row[dt.Columns[i]]);
                        PlaceHolder_information.Controls.Add(tx);
                        PlaceHolder_information.Controls.Add(new LiteralControl("</div>\n"));
                        str = DataTableUtils.toString(row[dt.Columns[i]]);
                    }
                }
            }

        }
        //刪除選中的資訊
        private void delete_information(string type, string num, string worknum)
        {
            //判斷該開啟哪個資料庫
            if (type == "Ver")
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
            else if (type == "Hor")
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);

            string sqlcmd = "select * from 工作站狀態資料表  where 排程編號='" + num + "' and 工作站編號 = '" + worknum + "'";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);

            string url = "type=" + type;
            url = WebUtils.UrlStringEncode(url);
            if (dt.Rows.Count > 0)
            {
                if (DataTableUtils.Delete_Record("工作站狀態資料表", "排程編號='" + num + "' and 工作站編號 = '" + worknum + "'"))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script>alert('刪除成功');location.href='PMD_Upload_old.aspx?key=" + url + "';</script>");
                    TextBox_textTemp.Text = "";
                }
            }
        }
        //更新選中的資料
        private void update_information(string type, string num, string worknum, List<string> list)
        {
            //判斷該開啟哪個資料庫
            if (type == "Ver")
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
            else if (type == "Hor")
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);

            string sqlcmd = "select 組裝編號,排程編號,進度,狀態,工作站編號,組裝日,實際組裝時間 from 工作站狀態資料表  where 排程編號='" + num + "' and 工作站編號 = '" + worknum + "'";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);

            string url = "type=" + type;
            url = WebUtils.UrlStringEncode(url);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.NewRow();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    row[dt.Columns[i]] = list[i];
                }

                if (DataTableUtils.Update_DataRow("工作站狀態資料表", "排程編號='" + num + "' and 工作站編號 = '" + worknum + "'", row))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script>alert('更新成功');location.href='PMD_Upload_old.aspx?key=" + url + "';</script>");
                    TextBox_textTemp.Text = "";
                }
            }
        }
    }
}
