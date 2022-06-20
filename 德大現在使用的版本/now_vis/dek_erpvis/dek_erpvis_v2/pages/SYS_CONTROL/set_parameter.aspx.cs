using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using dek_erpvis_v2.cls;
using Support;

namespace dek_erpvis_v2.pages.SYS_CONTROL
{
    public partial class set_parameter : System.Web.UI.Page
    {
        public string color = "";
        public string page_name = "參數設定";
        public string startdate = "";
        public string enddate = "";
        string acc = "";
        public string page_names = "";
        List<string> list = new List<string>();

        DataTable dx = null;
        DataTable public_dt = null;
        myclass myclass = new myclass();
        clsDB_Server clsDB_vis = new clsDB_Server("");
        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime start = DateTime.Now;
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                color = HtmlUtil.change_color(acc);

                if (acc == "visrd")//visrd為最高使用者
                {
                    set_control_panel(Panel_foruser);
                    Panel_foruser.Visible = true;
                }


                load_page_data();
                if (!IsPostBack)
                {
                    if (Show_Style(acc) == "custom_old")
                        RadioButtonList1.Items.FindByValue("custom_old").Selected = true;
                    else if (Show_Style(acc) == "custom")
                        RadioButtonList1.Items.FindByValue("custom").Selected = true;
                    else
                        RadioButtonList1.Items.FindByValue("custom_person").Selected = true;
                }
                if (Session["back"] == null)
                {
                    if (Session["Radio"] != null)
                    {
                        RadioButtonList1.SelectedValue = Session["Radio"].ToString();
                        Session.Remove("Radio");
                    }
                }
                else
                    RadioButtonList1.SelectedValue = "custom";
                if (Session["check"] != null)
                    jump.Visible = true;
                Session.Remove("check");
                Session.Remove("back");
                Session.Remove("Radio");
            }
            else
                Response.Redirect(myclass.logout_url);

        }
        private void load_page_data()
        {
            clsDB_vis.dbOpen(myclass.GetConnByDekVisErp);
            if (clsDB_vis.IsConnected == true)
            {
                string strSQL = 德大機械.控制台_權限管理.是否新增過參數(acc);
                public_dt = clsDB_vis.GetDataTable(strSQL);

                if (!IsPostBack)
                {
                    if (HtmlUtil.Check_DataTable(public_dt)) //有新增過，將設定值取出                                                  
                    {
                        string SQLQuery = 德大機械.控制台_權限管理.取得起算結算日(acc);
                        DataTable dt_set = clsDB_vis.GetDataTable(SQLQuery);
                        date_str.Value = DataTableUtils.toString(dt_set.Rows[0]["DATE_STR"]);
                        date_end.Value = DataTableUtils.toString(dt_set.Rows[0]["DATE_END"]);
                        dt_set.Dispose();
                    }
                    else
                    {
                        date_str.Value = "1";
                        date_end.Value = "31";
                    }
                    clsDB_vis.dbOpen(myclass.GetConnByDekVisErp);
                    show_companyname();
                    // show_time(acc);
                }
                //visrd為最高使用者
                clsDB_vis.dbOpen(myclass.GetConnByDekVisErp);
                set_control_panel(Panel_people);
            }
        }
        protected void button_select_Click(object sender, EventArgs e)
        {
            string sqlcmd = "";
            startdate = date_str.Value;
            enddate = date_end.Value;

            clsDB_vis.dbOpen(myclass.GetConnByDekVisErp);
            if (clsDB_vis.IsConnected == true)
            {
                string strSQL = 德大機械.控制台_權限管理.是否新增過參數(acc);
                public_dt = clsDB_vis.GetDataTable(strSQL);

                if (HtmlUtil.Check_DataTable(public_dt)) //有新增過，用update
                    sqlcmd = 德大機械.控制台_權限管理.更新參數設定(startdate, enddate, acc);
                else //首次新增，用insert
                {
                    //2019/06/06，Set TableId = SET(3碼)+亂數流水號(5碼)
                    string id = "SET" + myclass.get_ran_id();
                    sqlcmd = 德大機械.控制台_權限管理.新增參數設定(id, acc, startdate, enddate);
                }
                DataTable dt = clsDB_vis.GetDataTable(sqlcmd);
                upload_image_user();
                save_css();

                Allow_Showpage();
                change_time(acc);
                Panel_people.Controls.Clear();
                if (acc == "visrd")
                {
                    list = new List<string>(TextBox_page.Text.Split('^'));
                    clsDB_vis.dbOpen(myclass.GetConnByDekVisErp);
                    update_companyname();
                    upload_companyimage();
                    change_allow_page();
                    //修正部門名稱
                    foreach (DataRow row in dx.Rows)
                        change_dep_name(row);
                    //修正頁面名稱
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisErp);

                    DataTable dc = DataTableUtils.GetDataTable("select * from WEB_PAGES where judge = '1'");
                    foreach (DataRow row in dc.Rows)
                        change_page_name(row);
                }
                dt.Dispose();
                Response.Write("<script>alert('資料已更新!');location.href='set_parameter.aspx'; </script>");
            }
            else
                Response.Write($"<script language='javascript'>alert('伺服器回應 : 無法載入資料，{clsDB_vis.ErrorMessage} 請聯絡德科人員或檢查您的網路連線。');</script>");

        }


        //------------------------------------20191115開會後新增----------------------------------------------------
        //列出頁面的狀態(能否進入)、目前的方法是讓每間公司的其中一個人決定要打開的頁面
        private void set_control_panel(Panel pn)
        {

            clsDB_vis.dbOpen(myclass.GetConnByDekVisErp);
            int i = 0;
            if (clsDB_vis.IsConnected == true)
            {
                string sql_cmd = "SELECT DISTINCT DEPARTMENT.DPM_ID,web_dpm, DEPARTMENT.DPM_NAME2 FROM   WEB_PAGES LEFT JOIN DEPARTMENT ON DEPARTMENT.DPM_NAME= WEB_PAGES.WEB_DPM WHERE  DEPARTMENT.DPM_GROUP IS NULL";
                DataTableUtils.Conn_String = myclass.GetConnByDekVisErp;//要加上這一行確定會連結到
                dx = DataTableUtils.GetDataTable(sql_cmd);
                if (HtmlUtil.Check_DataTable(dx))
                {
                    pn.Controls.Add(new LiteralControl("<div class=\"row\">\n"));
                    foreach (DataRow row in dx.Rows)
                    {
                        i += 2;
                        if (i % 12 == 0)
                        {
                            pn.Controls.Add(new LiteralControl("<div class=\"row\">\n"));
                            Panel_contr_Add(row, pn);
                            pn.Controls.Add(new LiteralControl("</div>\n"));
                        }
                        else
                            Panel_contr_Add(row, pn);
                    }
                    pn.Controls.Add(new LiteralControl("</div>\n"));

                    dx.Dispose();

                }

            }
        }
        private void Panel_contr_Add(DataRow row, Panel pn)
        {
            pn.Controls.Add(new LiteralControl("<div class=\"col-md-3 col-sm-4 col-xs-12\">\n"));
            //Label標籤
            if (acc == "visrd" && pn.ID == "Panel_foruser")
            {
                TextBox tx = new TextBox();
                tx.Text = DataTableUtils.toString(row["DPM_NAME2"]);
                tx.ID = DataTableUtils.toString(row["web_dpm"]);
                tx.Attributes.Add("style", "margin-top:5px;margin-bottom:5px;margin-left:5%");
                pn.Controls.Add(tx);
            }
            else
                pn.Controls.Add(new LiteralControl($"<label class=\"control-label \">[{DataTableUtils.toString(row["DPM_NAME2"])}]</label>\n"));

            pn.Controls.Add(new LiteralControl("<div class=\"\">\n"));
            pn.Controls.Add(new LiteralControl("<label>\n"));
            set_createCheckBoxList(DataTableUtils.toString(row["web_dpm"]), pn);
            if (DataTableUtils.toString(row["web_dpm"]) == "CNC")
            {
                if (pn.ID == "Panel_people")
                {
                    Label lb = new Label();
                    lb.Text = "監控面板刷新頻率<br>";
                    pn.Controls.Add(lb);

                    TextBox tx = new TextBox();
                    tx.Text = show_time(acc);
                    tx.TextMode = TextBoxMode.Number;
                    tx.ID = "timeset";
                    pn.Controls.Add(tx);

                    Label lb2 = new Label();
                    lb2.Text = "秒";
                    pn.Controls.Add(lb2);

                }
            }
            pn.Controls.Add(new LiteralControl("</label>\n"));
            pn.Controls.Add(new LiteralControl("</div>\n"));
            pn.Controls.Add(new LiteralControl("</div>\n"));
        }
        private void set_createCheckBoxList(string dep_item, Panel pn)
        {
            CheckBoxList checkBoxList = new CheckBoxList();
            if (acc == "visrd" && pn.ID == "Panel_foruser")
                checkBoxList.ID = "checkBoxList" + dep_item + acc;
            else
                checkBoxList.ID = "checkBoxList" + dep_item;
            checkBoxList.RepeatColumns = 1;
            checkBoxList.CssClass = "table-striped";
            get_checkBoxList_Item(checkBoxList, dep_item, pn);
            pn.Controls.Add(checkBoxList);
        }
        private void get_checkBoxList_Item(CheckBoxList CheckBoxList, string dep_item, Panel pn)
        {

            string sql_cmd = "";
            ListItem listItem = null;
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisErp);
            //visrd為最高使用者
            if (pn.ID == "Panel_foruser" && acc == "visrd")
                sql_cmd = $"SELECT a.*, ( CASE WHEN b.user_acc IS NULL THEN ''  ELSE b.user_acc  END ) AS USER_ACC FROM   (SELECT WEB_PAGES.WEB_DPM, WEB_PAGES.WEB_PAGENAME, WEB_PAGES.WEB_URL, WEB_PAGES.WEB_OPEN,WEB_PAGES.judge FROM   WEB_PAGES) AS a  left join (SELECT wb_url,   user_acc    FROM   WEB_USER    WHERE  user_acc = 'visrd') AS b   ON a.WEB_URL = b.wb_url WHERE  a.WEB_DPM = '{dep_item}' and a.judge = '1'";
            else

                sql_cmd = $"SELECT * FROM WEB_PAGES as wp left join Show_Page as sp on sp.URL = wp.WEB_URL where WEB_OPEN = 'Y' and account = '{acc}' and web_DPM = '{dep_item}' order by WB_ID asc";

            DataTable dt = DataTableUtils.GetDataTable(sql_cmd);
            if (HtmlUtil.Check_DataTable(dt))
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (pn.ID == "Panel_foruser" && acc == "visrd")
                    {
                        listItem = new ListItem($"<input name=\"ctl00$ContentPlaceHolder1${DataTableUtils.toString(row["WEB_URL"])}\" type=\"text\" value=\"{DataTableUtils.toString(row["WEB_PAGENAME"])}\" id=\"ContentPlaceHolder1_{DataTableUtils.toString(row["WEB_URL"])}\" style=\"margin-top:5px;margin-bottom:5px;margin-left:5%\">", DataTableUtils.toString(row["WEB_URL"]));
                        page_names += $"pagename += '{DataTableUtils.toString(row["WEB_URL"])}^' + document.getElementById('ContentPlaceHolder1_{DataTableUtils.toString(row["WEB_URL"])}').value + '^'; \n";
                    }

                    else
                        listItem = new ListItem(DataTableUtils.toString(row["WEB_PAGENAME"]), DataTableUtils.toString(row["WEB_URL"]));

                    if (pn.ID == "Panel_foruser" && acc == "visrd")
                    {
                        if (DataTableUtils.toString(row["WEB_OPEN"]) != "N")
                            listItem.Selected = true;
                        else
                            listItem.Selected = false;
                    }
                    else
                    {
                        if (DataTableUtils.toString(row["allow"]) != "N")
                            listItem.Selected = true;
                        else
                            listItem.Selected = false;
                    }
                    CheckBoxList.Items.Add(listItem);
                }
            }
            else
            {
                sql_cmd = $"SELECT * FROM WEB_PAGES where web_Open = 'Y' and Web_DPM = '{dep_item}' and judge = '1'";
                dt = DataTableUtils.GetDataTable(sql_cmd);
                if (HtmlUtil.Check_DataTable(dt))
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        listItem = new ListItem(DataTableUtils.toString(row["WEB_PAGENAME"]), DataTableUtils.toString(row["WEB_URL"]));
                        listItem.Selected = true;
                        CheckBoxList.Items.Add(listItem);
                    }
                }

            }

        }
        //設定能顯示的頁面
        private void change_allow_page()
        {
            DataTableUtils.Conn_String = myclass.GetConnByDekVisErp;
            foreach (Control cbxlist in this.Panel_foruser.Controls)
            {
                if (cbxlist is CheckBoxList)
                {
                    foreach (ListItem li in ((CheckBoxList)cbxlist).Items)
                    {
                        if (li.Selected == false)
                            update_right(li, "0");

                        else
                            update_right(li, "1");
                    }
                }
            }
        }
        private void update_right(ListItem li, string judge)
        {

            string text = DataTableUtils.toString(li.Value);

            clsDB_vis.dbOpen(myclass.GetConnByDekVisErp);
            string sqlcmd = $"SELECT * FROM WEB_PAGES Where WEB_URL = '{text}'";
            DataTable dt = clsDB_vis.DataTable_GetTable(sqlcmd);
            if (HtmlUtil.Check_DataTable(dt))
            {
                DataRow row = dt.NewRow();
                if (judge == "0")
                    row["WEB_OPEN"] = "N";
                else
                    row["WEB_OPEN"] = "Y";


                //因為寫好了，所以只需要資料表、更新條件以及你所寫好的row
                if (clsDB_vis.Update_DataRow("WEB_PAGES", $"WEB_URL= '{text}'", row))
                {

                }
            }
        }
        //新增||修改公司照片
        private void upload_companyimage()
        {
            if (FileUpload_companyimage.FileName != "")
            {
                string name = "";
                string path2 = "D:\\Backup_Error_Image\\";
                string path = Server.MapPath("~/assets/images/");
                string ext = Path.GetExtension(FileUpload_companyimage.FileName).ToLower();
                if (ext != "")
                {
                    if (ext == ".jpg" || ext == ".png" || ext == ".JPG" || ext == ".PNG")
                    {
                        FileUpload_companyimage.SaveAs(path + FileUpload_companyimage.FileName);
                        FileUpload_companyimage.SaveAs(path2 + FileUpload_companyimage.FileName);
                        name = "~/assets/images/" + FileUpload_companyimage.FileName;
                    }
                }
                clsDB_vis.dbOpen(myclass.GetConnByDekVisErp);

                string sqlcmd = "SELECT * FROM Account_Image where Account_name = 'companyimage'";
                DataTable dt = clsDB_vis.DataTable_GetTable(sqlcmd);
                //沒有新增過圖片
                if (dt.Rows.Count <= 0)
                {
                    sqlcmd = $"SELECT * FROM Account_Image where  Image_link = '{name}'";
                    dt = clsDB_vis.DataTable_GetTable(sqlcmd);
                    if (dt.Rows.Count <= 0)
                    {
                        DataRow row = dt.NewRow();
                        row["Account_name"] = "companyimage";
                        row["Image_link"] = name;
                        if (clsDB_vis.Insert_DataRow("Account_Image", row))
                        {
                        }
                    }
                    else
                        Response.Write("<script>alert('該圖片名稱已被使用，請更換圖片名稱!');location.href='set_parameter.aspx';</script>");
                }
                //有新增過圖片，就要轉成修改
                if (HtmlUtil.Check_DataTable(dt))
                {
                    sqlcmd = $"SELECT * FROM Account_Image where Image_link = '{name}'";
                    dt = clsDB_vis.DataTable_GetTable(sqlcmd);
                    if (dt.Rows.Count <= 0)
                    {
                        DataRow row = dt.NewRow();
                        row["Image_link"] = name;
                        //因為寫好了，所以只需要資料表、更新條件以及你所寫好的row
                        if (clsDB_vis.Update_DataRow("Account_Image", "Account_name = 'companyimage'", row))
                        {
                        }
                    }
                    else
                        Response.Write("<script>alert('該圖片名稱已被使用，請更換圖片名稱!');location.href='set_parameter.aspx';</script>");
                }
            }
        }
        //變更部門名稱
        private void change_dep_name(DataRow raw)
        {
            TextBox tz = new TextBox();
            tz = (TextBox)this.Panel_foruser.FindControl(DataTableUtils.toString(raw["WEB_DPM"]));
            string a = tz.Text;
            string sqlcmd = $"SELECT * FROM DEPARTMENT where DPM_NAME = '{DataTableUtils.toString(raw["WEB_DPM"])}'";
            DataTable dg = clsDB_vis.DataTable_GetTable(sqlcmd);
            if (HtmlUtil.Check_DataTable(dg))
            {
                DataRow row = dg.NewRow();
                row["DPM_NAME2"] = a;
                //因為寫好了，所以只需要資料表、更新條件以及你所寫好的row
                if (clsDB_vis.Update_DataRow("DEPARTMENT", $"DPM_NAME = '{DataTableUtils.toString(raw["WEB_DPM"])}'", row))
                {
                }
            }
        }
        //新增||修改公司名稱名稱
        private void update_companyname()
        {
            if (textbox_companyname.Text != null)
            {
                clsDB_vis.dbOpen(myclass.GetConnByDekVisErp);

                string sqlcmd = "SELECT * FROM Account_Image where Account_name = 'companyname'";
                DataTable dt = clsDB_vis.DataTable_GetTable(sqlcmd);
                if (dt.Rows.Count <= 0)
                {
                    sqlcmd = $"SELECT * FROM Account_Image where Image_link = '{textbox_companyname.Text}'";
                    dt = clsDB_vis.DataTable_GetTable(sqlcmd);
                    if (dt.Rows.Count <= 0)
                    {
                        DataRow row = dt.NewRow();
                        row["Account_name"] = "companyname";
                        row["Image_link"] = textbox_companyname.Text;
                        if (clsDB_vis.Insert_DataRow("Account_Image", row))
                        {
                        }
                    }
                }

                if (HtmlUtil.Check_DataTable(dt))
                {
                    sqlcmd = $"SELECT * FROM Account_Image where Image_link = '{textbox_companyname.Text}'";
                    dt = clsDB_vis.DataTable_GetTable(sqlcmd);
                    if (dt.Rows.Count <= 0)
                    {
                        DataRow row = dt.NewRow();
                        row["Image_link"] = textbox_companyname.Text;
                        //因為寫好了，所以只需要資料表、更新條件以及你所寫好的row
                        if (clsDB_vis.Update_DataRow("Account_Image", "Account_name= 'companyname'", row))
                        {
                        }
                    }
                }
            }
        }
        //新增||修改個人照片
        private void upload_image_user()
        {
            if (FileUpload_userimage.FileName != "")
            {
                string name = "";
                string path = Server.MapPath("~/assets/images/");
                string path2 = "D:\\Backup_Error_Image\\";
                string ext = Path.GetExtension(FileUpload_userimage.FileName);
                if (ext != "")
                {
                    if (ext == ".jpg" || ext == ".png" || ext == ".JPG" || ext == ".PNG")
                    {
                        FileUpload_userimage.SaveAs(path + FileUpload_userimage.FileName);
                        FileUpload_userimage.SaveAs(path2 + FileUpload_userimage.FileName);
                        name = "~/assets/images/" + FileUpload_userimage.FileName;
                    }
                }
                clsDB_vis.dbOpen(myclass.GetConnByDekVisErp);

                string sqlcmd = $"SELECT * FROM Account_Image where Account_name = '{acc}'";
                DataTable dt = clsDB_vis.DataTable_GetTable(sqlcmd);
                //沒有新增過圖片
                if (dt.Rows.Count <= 0)
                {
                    sqlcmd = $"SELECT * FROM Account_Image where and Image_link = '{name}'";
                    dt = clsDB_vis.DataTable_GetTable(sqlcmd);
                    if (dt.Rows.Count <= 0)
                    {
                        DataRow row = dt.NewRow();
                        row["Account_name"] = acc;
                        row["Image_link"] = name;
                        if (clsDB_vis.Insert_DataRow("Account_Image", row))
                        {
                        }
                    }
                    else
                        Response.Write("<script>alert('該圖片名稱已被使用，請更換圖片名稱!');location.href='set_parameter.aspx';</script>");
                }
                //有新增過圖片，就要轉成修改
                if (HtmlUtil.Check_DataTable(dt))
                {
                    sqlcmd = $"SELECT * FROM Account_Image where Image_link = '{name}'";
                    dt = clsDB_vis.DataTable_GetTable(sqlcmd);
                    if (dt.Rows.Count <= 0)
                    {
                        DataRow row = dt.NewRow();
                        row["Image_link"] = name;
                        //因為寫好了，所以只需要資料表、更新條件以及你所寫好的row
                        if (clsDB_vis.Update_DataRow("Account_Image", $"Account_name= '{acc}'", row))
                        {
                        }
                    }
                    else
                        Response.Write("<script>alert('該圖片名稱已被使用，請更換圖片名稱!');location.href='set_parameter.aspx';</script>");
                }
            }
        }
        //顯示公司名稱在TEXTBOX上(方便修改而已)
        private void show_companyname()
        {
            string sql_cmd = "";
            DataTable dt = null;

            sql_cmd = "SELECT * FROM Account_Image WHERE Account_name = 'companyname'";
            dt = clsDB_vis.DataTable_GetTable(sql_cmd);
            if (HtmlUtil.Check_DataTable(dt))
                textbox_companyname.Text = DataTableUtils.toString(dt.Rows[0]["Image_link"]);
        }
        //將個人主板存入資料庫
        private void save_css()
        {
            string sqlcmd = "select TOP(1)  UID from Account_Image order by UID desc ";
            DataTable dt = clsDB_vis.DataTable_GetTable(sqlcmd);
            int count = Int32.Parse(DataTableUtils.toString(dt.Rows[0]["Uid"])) + 1;
            sqlcmd = "SELECT * FROM Account_Image where Account_name  = '" + acc + "' and image_link is null";
            dt = clsDB_vis.DataTable_GetTable(sqlcmd);

            //沒有選擇過的，用新增

            if (dt.Rows.Count <= 0)
            {
                string user_style = RadioButtonList1.SelectedItem.Value;
                DataRow row = dt.NewRow();
                row["UID"] = count;
                row["Account_name"] = acc;
                row["background"] = user_style;
                //-------------------------------------------------把Cookie存到資料庫用
                if (cookie_value() != "" && user_style == "custom_person")
                    row["RGB_Set"] = cookie_value();
                else
                    row["RGB_Set"] = " ";
                //-------------------------------------------------把Cookie存到資料庫用
                if (clsDB_vis.Insert_DataRow("Account_Image", row))
                {
                }
            }
            //有選擇過的，用修改
            else
            {
                string user_style = RadioButtonList1.SelectedItem.Value;
                DataRow row = dt.NewRow();
                row["Account_name"] = acc;
                row["background"] = user_style;
                //-------------------------------------------------把Cookie存到資料庫用
                if (cookie_value() != "" && user_style == "custom_person")
                    row["RGB_Set"] = cookie_value();
                else
                    row["RGB_Set"] = " ";
                //-------------------------------------------------把Cookie存到資料庫用
                if (clsDB_vis.Update_DataRow("Account_Image", $"Account_name= '{acc}' and background = '{DataTableUtils.toString(dt.Rows[0]["background"])}'", row))
                {
                }
            }
        }
        //顯示個人採用的主板
        private string Show_Style(string acc)
        {
            string Return_str = "";
            string sql_cmd = $"SELECT * FROM Account_Image where Account_name  = '{acc}' and image_link is null";
            DataTable dt = clsDB_vis.DataTable_GetTable(sql_cmd);
            if (HtmlUtil.Check_DataTable(dt))
            {
                Return_str = DataTableUtils.toString(dt.Rows[0]["background"]);
                if (DataTableUtils.toString(dt.Rows[0]["background"]) == "custom_person")
                    jump.Visible = true;
            }
            else
                Return_str = "custom";

            return Return_str;
        }
        //個人頁面顯示部分
        private void Allow_Showpage()
        {
            DataTableUtils.Conn_String = myclass.GetConnByDekVisErp;
            foreach (Control cxlist in this.Panel_people.Controls)
            {
                if (cxlist is CheckBoxList)
                {
                    foreach (ListItem li in ((CheckBoxList)cxlist).Items)
                    {
                        if (li.Selected == false)
                            change_right(li, "0");
                        else
                            change_right(li, "1");
                    }
                }
            }
        }
        //沒有資料就新增，有資料則修改
        private void change_right(ListItem li, string judge)
        {
            string text = DataTableUtils.toString(li.Value);
            clsDB_vis.dbOpen(myclass.GetConnByDekVisErp);
            string sqlcmd = $"SELECT * FROM Show_Page Where account = '{acc}' and url = '{text}'";
            DataTable dt = clsDB_vis.DataTable_GetTable(sqlcmd);
            if (HtmlUtil.Check_DataTable(dt))
            {
                DataRow row = dt.NewRow();
                if (judge == "0")
                    row["Allow"] = "N";
                else
                    row["Allow"] = "Y";
                //因為寫好了，所以只需要資料表、更新條件以及你所寫好的row
                if (clsDB_vis.Update_DataRow("Show_Page", $" account = '{acc}' and url = '{text}'", row))
                {

                }
            }
            else
            {
                DataRow row = dt.NewRow();
                row["url"] = text;
                row["account"] = acc;
                if (judge == "0")
                    row["Allow"] = "N";
                else
                    row["Allow"] = "Y";
                //因為寫好了，所以只需要資料表、更新條件以及你所寫好的row
                if (clsDB_vis.Insert_DataRow("Show_Page", row))
                {

                }
            }
        }
        //Radiobuttonlist 事件
        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RadioButtonList1.SelectedValue == "custom_person")
                jump.Visible = true;
            else
                jump.Visible = false;
        }
        //得到cookie內的數值，以便存到資料庫
        private string cookie_value()
        {
            string cookievalue = "";
            HttpCookieCollection cookie = Request.Cookies;
            List<string> alphabet = new List<string>();
            //要去抓cookie的value
            alphabet.Add("Sidebar_Color");
            alphabet.Add("SidebarText_Color");
            alphabet.Add("Label_Color");
            alphabet.Add("Background_Color");
            alphabet.Add("Column_Color");
            alphabet.Add("ColumnText_Color");
            alphabet.Add("DataTable_Color");
            alphabet.Add("DataTableText_Color");

            for (int i = 0; i < cookie.Count; i++)
            {
                for (int j = 0; j < alphabet.Count; j++)
                {
                    if (cookie[i].Name == alphabet[j])
                    {
                        cookievalue += cookie[i].Name + "," + cookie[i].Value + ";";
                        break;
                    }
                }
            }
            return cookievalue;
        }
        //跳到顏色設定
        protected void jump_Click(object sender, EventArgs e)
        {
            Session["Radio"] = RadioButtonList1.SelectedValue;
            Response.Write("<script>alert('即將進入顏色設定畫面!');location.href='Color_Change.aspx'; </script>");
        }
        //變更刷新秒數(加工可視化用)
        private void change_time(string acc)
        {
            string time = "";
            foreach (Control text in this.Panel_people.Controls)
            {
                if (text is TextBox)
                    time = ((TextBox)text).Text;
            }
            string sqlcmd = $"select * from Users where USER_ACC = '{acc}'";
            DataTable dt = clsDB_vis.DataTable_GetTable(sqlcmd);

            if (HtmlUtil.Check_DataTable(dt))
            {
                DataRow row = dt.NewRow();
                row["USER_ID"] = DataTableUtils.toString(dt.Rows[0]["USER_ID"]);
                row["Set_Time"] = time;
                if (clsDB_vis.Update_DataRow("Users", $"USER_ACC = '{acc}'", row) == true)
                {

                }
            }
        }
        //顯示刷新秒數(加工可視化用)
        private string show_time(string acc)
        {
            string second = HtmlUtil.Search_acc_Column(acc, "Set_Time");
            if (second != "")
                return second;
            else
                return "60";
        }
        //變更頁面名稱
        private void change_page_name(DataRow row)
        {
            string page = DataTableUtils.toString(row["WEB_URL"]);
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisErp);
            string sqlcmd = $"SELECT * FROM WEB_PAGES where WEB_URL = '{page}'";
            DataTable dg = clsDB_vis.DataTable_GetTable(sqlcmd);
            if (HtmlUtil.Check_DataTable(dg))
            {
                DataRow raw = dg.NewRow();
                raw["WEB_PAGENAME"] = list[list.IndexOf(page) + 1];
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisErp);
                if (clsDB_vis.Update_DataRow("WEB_PAGES", $"WEB_URL = '{page}'", raw))
                {
                }
            }
        }
    }
}