using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using dek_erpvis_v2.cls;
using Support;

namespace dek_erpvis_v2.pages.SYS_CONTROL
{
    public partial class Add_Report : System.Web.UI.Page
    {
        public string inup = "";
        public string acc = "";
        public string name = "";
        public string part = "";
        public string time;
        string URL_NAME = "";
        string update_id = "";
        string permissioncheck = "";
        myclass myclass = new myclass();
        clsDB_Server clsDB_sw = new clsDB_Server("");
        //載入要用的事件
        protected void Page_Load(object sender, EventArgs e)
        {
            Label_code.Text = new Random().Next(9999).ToString();
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                if (!IsPostBack)
                {
                    InitDropDown();
                    iniCbx();
                    load_data();
                }
            }
            else
            {
                Response.Redirect(myclass.logout_url);
            }
        }
        //載入該帳號的名稱及部門
        private void mainprocess()
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            name = DataTableUtils.toString(userInfo["user_ACC"]);
            part = DataTableUtils.toString(userInfo["user_DPM"]);
        }
        //執行新增及修改事件
        protected void button_add_Click(object sender, EventArgs e)
        {
            clsDB_sw.dbOpen(myclass.GetConnByDekVisErp);
            //去抓網址是否有ID
            if (Request.QueryString["id"] != null)
            {
                update_id = Request.QueryString["id"].Split(',')[0];
            }

            mainprocess();
            string title = TextBox_Title.Text;
            string type = DropDownList_Type.Text;
            string inputname = name;
            string time = DateTime.Now.ToString("yyyyMMdd");
            string inputpart = part;
            string detail = TextBox_textTemp.Text.ToString();
            string updatetime = DateTime.Now.ToString("yyyy/MM/dd");
            permissioncheck = "";
            //新增的動作
            if (update_id == "")
            {
                string id = DataTableUtils.toString(myclass.get_ran_id());

                //紀錄所點選的核取方塊
                for (int i = 0; i < CheckBoxList_SYS.Items.Count; i++)
                {
                    if (CheckBoxList_SYS.Items[i].Selected)
                    {
                        permissioncheck += CheckBoxList_SYS.Items[i].Value + ",";
                    }
                }
                if (title != "")
                {
                    Add_New_Report(id, title, type, inputname, time, inputpart, detail, updatetime, permissioncheck);
                }
                else
                {
                    Response.Write("<script language='javascript'>alert('伺服器回應 : 請填入標題以便搜尋喔')</script>");
                }
            }
            //更新的動作
            else
            {
                //紀錄所點選的核取方塊
                for (int i = 0; i < CheckBoxList_SYS.Items.Count; i++)
                {
                    if (CheckBoxList_SYS.Items[i].Selected)
                    {
                        permissioncheck += CheckBoxList_SYS.Items[i].Value + ",";
                    }
                }

                updatedata(title, type, inputname, time, inputpart, detail, updatetime, permissioncheck);
            }


        }
        //執行新增的動作
        private void Add_New_Report(string id, string title, string type, string inputname, string time, string inputpart, string detial, string updatetime, string permission)
        {
            clsDB_sw.dbOpen(myclass.GetConnByDekVisErp);
            string sqlcmd = "SELECT * FROM Record_Report where id = '" + id + "'";
            DataTable dt = clsDB_sw.DataTable_GetTable(sqlcmd);
            if (dt.Rows.Count <= 0)
            {
                DataRow row = dt.NewRow();
                row["id"] = id;
                row["title"] = title;
                row["type"] = type;
                row["name"] = name;
                row["time"] = time;
                row["part"] = part;
                row["detail"] = detial;
                row["updatetime"] = updatetime;
                row["permission"] = permission;
                if (clsDB_sw.Insert_DataRow("Record_Report", row) == true)
                {
                    Response.Write("<script>alert('伺服器回應 : 以新增成功!');location.href='../SYS_CONTROL/Business_Report_detail.aspx?id=" + id + "';</script>");
                }
            }
        }
        //取消並返回上一頁
        protected void button_cancel_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] != null)
            {
                update_id = Request.QueryString["id"].Split(',')[0];
            }
            if (update_id == "")
            {
                Response.Redirect("Business_Report.aspx");
            }
            else
            {
                Response.Redirect("Business_Report_detail.aspx?id=" + update_id);
            }
        }
        //匯入資料庫內的所有部門
        private void iniCbx()
        {
            clsDB_sw.dbOpen(myclass.GetConnByDekVisErp);
            if (clsDB_sw.IsConnected == true)
            {
                DataTable dt = clsDB_sw.GetDataTable("SELECT distinct DPM_NAME,DPM_NAME2 FROM DEPARTMENT where DPM_NAME2 != '系統'");
                if (dt.Rows.Count > 0)
                {
                    ListItem list = new ListItem("全部 &nbsp&nbsp&nbsp", "");
                    CheckBoxList_SYS.Items.Add(list);

                    foreach (DataRow row in dt.Rows)
                    {
                        //如果為德科，則補上空白(縮排等高用)
                        if (DataTableUtils.toString(row["DPM_NAME2"]) == "德科")
                        {
                            list = new ListItem(DataTableUtils.toString(row["DPM_NAME2"]) + " &nbsp&nbsp&nbsp", DataTableUtils.toString(row["DPM_NAME"]));
                        }
                        else
                        {
                            list = new ListItem(DataTableUtils.toString(row["DPM_NAME2"]) + " &nbsp", DataTableUtils.toString(row["DPM_NAME"]));
                        }
                        CheckBoxList_SYS.Items.Add(list);
                    }
                }
            }
        }
        //清空目前內容
        protected void button_clear_Click(object sender, EventArgs e)
        {
            //文本編輯器的話，只要觸發重新整理就會清除
            TextBox_Title.Text = "";
            DropDownList_Type.SelectedIndex = -1;
            foreach (ListItem li in CheckBoxList_SYS.Items)
            {
                li.Selected = false;
            }

        }
        //更新載入ID，以及給使用者知道目前是處於更新還是新增
        private void load_data()
        {
            Response.BufferOutput = false;
            if (Request.QueryString["id"] != null)
            {
                update_id = Request.QueryString["id"].Split(',')[0];
                update_data(update_id);
                inup = "更新";
            }
            else
            {
                mainprocess();
                inup = "新增";
            }
        }
        //更新時載入資料
        private void update_data(string update_id)
        {
            clsDB_sw.dbOpen(myclass.GetConnByDekVisErp);
            string sqlcmd = "SELECT * FROM Record_Report where id= '" + update_id + "'";
            DataTable dt = clsDB_sw.DataTable_GetTable(sqlcmd);
            if (dt.Rows.Count > 0)
            {
                TextBox_Title.Text = DataTableUtils.toString(dt.Rows[0]["title"]);
                DropDownList_Type.SelectedValue = DataTableUtils.toString(dt.Rows[0]["type"]);
                name = DataTableUtils.toString(dt.Rows[0]["name"]);
                time = DataTableUtils.toString(dt.Rows[0]["updatetime"]);
                part = DataTableUtils.toString(dt.Rows[0]["part"]);
                TextBox_textTemp.Text = DataTableUtils.toString(dt.Rows[0]["detail"]);
                string iniboxselect = DataTableUtils.toString(dt.Rows[0]["permission"]);

                //進行字串分割
                string[] str = iniboxselect.Split(',');
                //分割完畢之後，先印出所有部門
                for (int i = 0; i < CheckBoxList_SYS.Items.Count; i++)
                {
                    //把分割完畢的字串跑迴圈(因為最後一個是""，所以要-1，不然全部會被打勾)
                    for (int j = 0; j < str.Length - 1; j++)
                    {
                        //如果相同的話，就讓核取方塊打勾
                        if (CheckBoxList_SYS.Items[i].Value == str[j])
                        {
                            CheckBoxList_SYS.Items[i].Selected = true;
                        }
                    }
                }
            }
        }
        //進行更新的動作
        private void updatedata(string title, string type, string inputname, string time, string inputpart, string detial, string updatetime, string permission)
        {
            clsDB_sw.dbOpen(myclass.GetConnByDekVisErp);
            string sqlcmd = "SELECT * FROM Record_Report Where id = " + "'" + update_id + "'";
            DataTable dt = clsDB_sw.DataTable_GetTable(sqlcmd);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.NewRow();
                row["title"] = title;
                row["type"] = type;
                row["name"] = inputname;
                row["time"] = time;
                row["part"] = inputpart;
                row["detail"] = detial;
                row["updatetime"] = updatetime;
                row["permission"] = permission;
                //因為寫好了，所以只需要資料表、更新條件以及你所寫好的row
                if (clsDB_sw.Update_DataRow("Record_Report", "id= '" + update_id + "'", row) == true)
                {
                    Response.Write("<script>alert('伺服器回應 : 以修改成功!');location.href='../SYS_CONTROL/Business_Report_detail.aspx?id=" + update_id + "';</script>");
                }
                else
                {
                    Response.Write("<script>alert('伺服器回應 : 失敗了!');location.href='../SYS_CONTROL/Business_Report.aspx';</script>");
                }
            }
        }
        //從資料庫載入報告類型
        private void InitDropDown()
        {
            clsDB_sw.dbOpen(myclass.GetConnByDekVisErp);
            if (clsDB_sw.IsConnected == true)
            {
                //去尋找Type_Repoert資料庫裡面的報告類型
                DataTable dt = clsDB_sw.GetDataTable("SELECT Report_Type FROM Type_Report");
                if (dt.Rows.Count > 0)
                {
                    ListItem list = new ListItem();
                    //把它印出來直到沒有為止
                    foreach (DataRow row in dt.Rows)
                    {
                        list = new ListItem(DataTableUtils.toString(row["Report_Type"]), DataTableUtils.toString(row["Report_Type"]));
                        DropDownList_Type.Items.Add(list);
                    }
                }
            }
        }

    }
}