using dek_erpvis_v2.cls;
using Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace dek_erpvis_v2
{
    public partial class masterpage : System.Web.UI.MasterPage
    {
        public string model = "nav-md footer_fixed";
        public string cookieValue = "";
        public string ELine = "";
        public string test = "";
        public string changevalue = "";
        public string btn = "";
        public string pagename = "";
        public string html_js = "";
        public string color = "#8d9be3";//顏色修改相關
        public string button_color = "#4655cc";//側欄底下四個按鈕顏色修改相關
        string user_css = "";
        public string user_name = "";
        public string keys = "";
        string acc = "";
        public string give_js = "";
        myclass myclass = new myclass();
        clsDB_Server clsDB_sw = new clsDB_Server("");
        HttpApplication httpApplication = new HttpApplication();
        protected void Page_Init(object sender, EventArgs e)
        {

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                user_name = DataTableUtils.toString(HttpUtility.UrlDecode(Request.Cookies["userInfo"]["user_NAME"]));
                keys = myclass.Base64Encode(DataTableUtils.toString(HttpUtility.UrlDecode(Request.Cookies["userInfo"]["user_ACC"])));

                //----------顏色修改相關
                clsDB_sw.dbOpen(myclass.GetConnByDekVisErp);
                string sqlcmd = $"SELECT * FROM Account_Image where Account_name = '{acc}' and Image_link is null";
                DataTable dt = clsDB_sw.DataTable_GetTable(sqlcmd);
                if (HtmlUtil.Check_DataTable(dt))
                    user_css = DataTableUtils.toString(dt.Rows[0]["background"]);
                else
                    user_css = "custom";
                //---------顏色修改相關
                load_data();
                color_value(acc);
                //給使用者測試顏色用
                if (System.IO.Path.GetFileName(Request.PhysicalPath) == "Color_Change.aspx")
                    pagename = "<li><a id=\"colortest\" href=\"/pages/SYS_CONTROL/Color_Change.aspx\"><img src=\"../../assets/images/icon white-24.png\" width=\"26px\" height=\"26px\"><label id=\"SomeLabel100\" style=\"font-size:100%\">&nbsp; 顏色設定</label><span id=\"\"></span></a></li>";
                else
                {
                    btn = "<div class=\"sidebar-footer hidden-small\" style=\"height:40px; \">" +
                          "<a style=\"background-color:<%= button_color %>\" id = \"Btncolor1\" ></a>" +
                          "<a style = \"background-color:<%= button_color %> \" id = \"Btncolor2\">" +
                            "<span class=\"\" aria-hidden=\"true\"></span>" +
                          "</a>" +
                          "<a style = \"background-color: <%= button_color %> \" id=\"Btncolor3\"></a>" +
                          "<a style = \"background-color: <%= button_color %> \" id=\"Btncolor4\"></a>" +
                          "<div style = \"color: #9dd0ed;\" >" +
                          "<td align=\"left\"></td>" +
                          "</div>" +
                          "</div>";
                }
                //開放權限
                if (HtmlUtil.Search_acc_Column(acc) == "Y")
                    changevalue = $"<li style=\"background-color: {color}\"><a href=\"/pages/SYS_CONTROL/PMD_Upload.aspx\">03變更資料</a></li>" +
                                  $"<li style=\"background-color: {color}\"><a href=\"/pages/SYS_CONTROL/Set_Energy.aspx\">04產能變更</a></li>";
            }
        }
        protected void Button_logout_Click(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                userInfo = new HttpCookie("userInfo");
                userInfo.Expires = DateTime.Now.AddDays(-1d);
                userInfo.Values.Clear();
                Response.Cookies.Add(userInfo);
            }
            Response.Redirect("~/login.aspx");
        }
        private void load_data()
        {
            left_List();
            show_all();
            show_IMG();
            show_CompanyName();
        }
        private void left_List() {
            if (HttpContext.Current.Request.Cookies["clickhistory"] != null)
            {
                cookieValue = HttpContext.Current.Request.Cookies["clickhistory"].Value;
                if (cookieValue == "fa fa-chevron-right")
                {
                    model = "nav-sm footer_fixed";
                }
                else
                {
                    model = "nav-md footer_fixed";
                }
            }
            else {
                return;
            }
        }
        //顯示圖片
        private void show_IMG()
        {
            //顯示個人圖片
            clsDB_sw.dbOpen(myclass.GetConnByDekVisErp);
            string sqlcmd = $"SELECT * FROM Account_Image where Account_name = '{acc}'";
            DataTable dt = clsDB_sw.DataTable_GetTable(sqlcmd);
            if (HtmlUtil.Check_DataTable(dt))
                Image_user.ImageUrl = DataTableUtils.toString(dt.Rows[0]["Image_link"]);
            else
                Image_user.ImageUrl = "~/assets/images/user.png";

            //顯示公司圖片
            sqlcmd = "SELECT * FROM Account_Image where Account_name = 'companyimage'";
            dt = clsDB_sw.DataTable_GetTable(sqlcmd);
            if (HtmlUtil.Check_DataTable(dt))
                Image_company.ImageUrl = DataTableUtils.toString(dt.Rows[0]["Image_link"]);
        }
        //顯示公司名稱
        private void show_CompanyName()
        {
            string sql_cmd = "";
            DataTable dt = null;

            sql_cmd = "SELECT * FROM Account_Image WHERE Account_name = 'companyname'";
            dt = clsDB_sw.DataTable_GetTable(sql_cmd);
            if (HtmlUtil.Check_DataTable(dt))
            {
                foreach (DataRow row in dt.Rows)
                    PlaceHolder_companyname.Controls.Add(new LiteralControl($"<a href=\"/pages/index.aspx\"><p style=\"font-size: 18px;line-height:2.7; text-valign:center; color: #ffffff;height:26px;width:151.08px\">{DataTableUtils.toString(row["Image_link"])}</p></a>"));
            }
        }
        //顯示所以部門的資料在側欄上
        private void show_all()
        {
            int i = 0;
            string sql_cmd = "";
            DataTable dt = null;
            DataTable dw = null;
            //由後臺決定icon圖案
            //先判斷有幾個部門
            sql_cmd = "SELECT DISTINCT DEPARTMENT.DPM_ID,web_dpm, DEPARTMENT.DPM_NAME2,DEPARTMENT.DPM_NAME FROM   WEB_PAGES LEFT JOIN DEPARTMENT ON DEPARTMENT.DPM_NAME= WEB_PAGES.WEB_DPM WHERE  DEPARTMENT.DPM_GROUP IS NULL ";
            DataTableUtils.Conn_String = myclass.GetConnByDekVisErp;//要加上這一行確定會連結到
            dw = clsDB_sw.DataTable_GetTable(sql_cmd);
            if (HtmlUtil.Check_DataTable(dw))
            {
                foreach (DataRow raw in dw.Rows)
                {
                    //判斷使用者有沒有取消特定的頁面部顯示
                    sql_cmd = $"SELECT * FROM show_page where account = '{acc}'";
                    dt = clsDB_sw.DataTable_GetTable(sql_cmd);
                    if (HtmlUtil.Check_DataTable(dt))
                    {                //先判斷部門內是否有資料，有就印出
                        sql_cmd = $"SELECT * FROM WEB_PAGES as wp left join Show_Page as sp on wp.WEB_URL = sp.URL WHERE WEB_DPM = '{DataTableUtils.toString(raw["DPM_NAME"])}' and WEB_OPEN = 'Y' and Allow = 'Y' and account = '{acc}'";
                        dt = clsDB_sw.DataTable_GetTable(sql_cmd);
                        if (HtmlUtil.Check_DataTable(dt))
                        {
                            //先印部門名稱
                            sql_cmd = $"SELECT * FROM DEPARTMENT WHERE DPM_NAME = '{DataTableUtils.toString(raw["DPM_NAME"])}'";
                            dt = clsDB_sw.DataTable_GetTable(sql_cmd);
                            if (HtmlUtil.Check_DataTable(dt))
                            {
                                foreach (DataRow row in dt.Rows)
                                {
                                    PlaceHolder_AllPart.Controls.Add(new LiteralControl($"<li id=\"{"id" + i}\"><a id=\"{  "Receive" + i}\"><img src=\"../../assets/images/{ DataTableUtils.toString(row["DPM_NAME"])}.png\" width=\"26px\" height=\"26px\"><label> &ensp; {DataTableUtils.toString(row["DPM_NAME2"])} </label><span class={"\"fa fa-chevron-down\""} style=\"font-size:10px\"></span></a>"));
                                    i++;
                                }
                            }
                            //在印部門底下各頁面
                            sql_cmd = $"SELECT * FROM WEB_PAGES as wp left join Show_Page as sp on sp.URL = wp.WEB_URL WHERE WEB_DPM = '{ DataTableUtils.toString(raw["DPM_NAME"])}' and WEB_OPEN = 'Y' and judge='1' and Allow = 'Y' and account = '{acc}' order by WB_ID asc";
                            dt = clsDB_sw.DataTable_GetTable(sql_cmd);
                            if (HtmlUtil.Check_DataTable(dt))
                            {
                                PlaceHolder_AllPart.Controls.Add(new LiteralControl("<ul class=\"nav child_menu\" style=\"left:70px\">"));
                                if (user_css == "custom_old")
                                {
                                    color = "";
                                    button_color = "#2A3F54";
                                }
                                else if (user_css == "custom_person")
                                {
                                    color = "";
                                    button_color = "";
                                }
                                foreach (DataRow row in dt.Rows)
                                    PlaceHolder_AllPart.Controls.Add(new LiteralControl($"<li id=\"hide1\" style=\"background-color:{color}\";><a onclick=show_loading()   href=/pages/{ DataTableUtils.toString(row["WEB_PATH"])}/{  DataTableUtils.toString(row["WEB_URL"])}.aspx>{DataTableUtils.toString(row["WEB_PAGENAME"])}</a></li>"));
                              
                                PlaceHolder_AllPart.Controls.Add(new LiteralControl("</ul>"));
                            }
                            PlaceHolder_AllPart.Controls.Add(new LiteralControl("</li>"));
                        }
                    }
                    else
                    {
                        //先判斷部門內是否有資料，有就印出
                        sql_cmd = $"SELECT * FROM WEB_PAGES WHERE WEB_DPM = '{DataTableUtils.toString(raw["DPM_NAME"])}' and WEB_OPEN = 'Y'";
                        dt = clsDB_sw.DataTable_GetTable(sql_cmd);
                        if (HtmlUtil.Check_DataTable(dt))
                        {
                            //先印部門名稱
                            sql_cmd = $"SELECT * FROM DEPARTMENT WHERE DPM_NAME = '{DataTableUtils.toString(raw["DPM_NAME"])}'";
                            dt = clsDB_sw.DataTable_GetTable(sql_cmd);
                            if (HtmlUtil.Check_DataTable(dt))
                            {
                                foreach (DataRow row in dt.Rows)
                                {                     
                                    PlaceHolder_AllPart.Controls.Add(new LiteralControl($"<li id=\"{"id" + i}\"><a id=\"{ "Receive" + i}\"><img src=\"../../assets/images/{ DataTableUtils.toString(row["DPM_NAME"])}.png\" width=\"26px\" height=\"26px\"><label> &ensp; {DataTableUtils.toString(row["DPM_NAME2"])} </label><span class={"\"fa fa-chevron-down\""} style=\"font-size:10px\"></span></a>"));
                                    i++;
                                }
                            }

                            //在印部門底下各頁面
                            sql_cmd = $"SELECT * FROM WEB_PAGES  WHERE WEB_DPM = '{DataTableUtils.toString(raw["DPM_NAME"])}' and WEB_OPEN = 'Y' and judge='1'";
                            dt = clsDB_sw.DataTable_GetTable(sql_cmd);
                            if (dt.Rows.Count > 0)
                            {
                                PlaceHolder_AllPart.Controls.Add(new LiteralControl("<ul class=\"nav child_menu\">"));
                                if (user_css == "custom_old")
                                {
                                    color = "";
                                    button_color = "#2A3F54";
                                }
                                else if (user_css == "custom_person")
                                {
                                    color = "";
                                    button_color = "";
                                }
                                foreach (DataRow row in dt.Rows)
                                    PlaceHolder_AllPart.Controls.Add(new LiteralControl($"<li id=\"hide1\" style=\"background-color:{color}\";><a onclick=show_loading()   href=/pages/{ DataTableUtils.toString(row["WEB_PATH"])}/{  DataTableUtils.toString(row["WEB_URL"])}.aspx>{DataTableUtils.toString(row["WEB_PAGENAME"])}</a></li>"));


                                PlaceHolder_AllPart.Controls.Add(new LiteralControl("</ul>"));
                            }
                            PlaceHolder_AllPart.Controls.Add(new LiteralControl("</li>"));
                        }
                    }
                }
            }
        }
        //前端JS顯示 20220630沒用到,停用
        private string js_code(int i)
        {
            return "if (id" + i + ".className == 'active') {document.getElementById(\"Receive" + i + "\").click();}\n";
        }
        //從資料庫抓值後放到前端-------------
        private void color_value(string acc)
        {
            string sql_cmd = "";
            DataTable dt = null;
            string back = "";
            sql_cmd = $"SELECT * FROM Account_Image WHERE Account_name = '{acc}' and background = 'custom_person' and RGB_Set <> ' '";
            dt = clsDB_sw.DataTable_GetTable(sql_cmd);
            string[] name = {"Sidebar_Color","SidebarText_Color",
                             "Label_Color","Background_Color",
                             "Column_Color","ColumnText_Color",
                             "DataTable_Color","DataTableText_Color"};
            if (HtmlUtil.Check_DataTable(dt))//有資料就讀資料
            {
                string value = DataTableUtils.toString(dt.Rows[0]["RGB_Set"]);
                string[] str = value.Split(';');
                for (int i = 0; i < name.Length; i++)
                {
                    for (int j = 0; j < str.Length; j++)
                    {
                        if (name[i] == str[j].Split(',')[0])
                        {
                            back += "'" + str[j].Split(',')[1] + "',";
                            break;
                        }
                    }
                }
                give_js = $"colorvalue = [{back}];";
            }
            else//沒資料就讀cookie
            {
                give_js = "var x = document.cookie.split(';');\n" +
                          "var colorvalue = [];\n" +
                          "for (var i = 0; i < x.length; i++)\n" +
                          "{\n" +
                                "var item = x[i].trim();\n" +
                                "for (var j = 0; j < colorname.length; j++)\n" +
                                "{\n" +
                                    "if (item.indexOf(colorname[j]) == 0)\n" +
                                        "colorvalue[j] = item.substring(colorname[j].length, item.length);\n" +
                                "}\n" +
                          "}\n";
            }
        }
    }
}