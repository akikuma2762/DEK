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
    public partial class Business_Report_detail : System.Web.UI.Page
    {
        public string update_id = "";
        public string see_name = "";
        public string title = "";
        public string type = "";
        public string name = "";
        public string time = "";
        public string part = "";
        public string detail = "";
        public string th = "";
        public string tr = "";
        public string title_text = "";
        public string title_name = "";
        string seeman = "";
        string userpart = "";
        string seepower = "";
        myclass myclass = new myclass();
        clsDB_Server clsDB_sw = new clsDB_Server("");

        protected void Page_Load(object sender, EventArgs e)
        {
            
            HttpCookie userInfo = Request.Cookies["userInfo"];
            
            if (userInfo != null)
            {
                see_name = DataTableUtils.toString(userInfo["user_ACC"]);
                userpart = DataTableUtils.toString(userInfo["user_DPM"]);
                if (see_name != "")
                {
                    if (!IsPostBack)
                    {
                        GotoCenn();
                    }
                }
                else

                {
                    Response.Redirect("Business_Report.aspx", false);
                }
            }
            else
            {
                Response.Redirect("Business_Report.aspx", false);
            }
        }
        //資料庫連結
        private void GotoCenn()
        {
            clsDB_sw.dbOpen(myclass.GetConnByDekVisErp);
            if (clsDB_sw.IsConnected == true)
            {
                load_page_data();
            }
            else
            {
                Response.Write("<script language='javascript'>alert('伺服器回應 : 無法載入資料，" + clsDB_sw.ErrorMessage + " 請聯絡德科人員或檢查您的網路連線。');</script>");
                無資料處理();
            }
        }
        //沒資料的處理辦法
        private void 無資料處理()
        {
            title_text = "'沒有資料'";
            th = "<th class='center'>沒有資料載入</th>";
            tr = "<tr class='even gradeX'> <td class='center'> no data </ td ></ tr >";
        }
        //連結到資料庫後載入資料(若該ID沒有資料，則返回上一頁)
        private void load_page_data()
        {
            Response.BufferOutput = false;
            if (Request.QueryString["id"] != null)
            {
                update_id = Request.QueryString["id"].Split(',')[0];
                string allowok = allowsee(update_id);
                string[] str = allowok.Split(',');//做字串分割
                string ADM = catchpower(see_name);//找到權限
                 
                if(ADM == "Y")//如果我有最高權限的話，可以進行修改
                {
                    PlaceHolder_hidden.Visible = true;
                    search_data(update_id);
                }
                else
                {
                    if (str[0] == userpart)//如果沒有的話，就看部門
                    {
                        PlaceHolder_hidden.Visible = true;
                    }
                    else
                    {
                        PlaceHolder_hidden.Visible = false;
                    }

                    int a = str.ToList().IndexOf(userpart);//直接搜尋分割完畢之後，使用者部門在陣列內的位置

                    if (str[1] == "")//如果為全部的話，每個人都可以看
                    {
                        search_data(update_id);
                    }
                    if (a == -1)//如果不再指定部門內，則無法看到
                    {
                        Response.Write("<script>alert('您目前沒有權限觀看此報告!');location.href='../SYS_CONTROL/Business_Report.aspx';</script>");
                    }
                    if (str[a] == userpart)//如果在指定部門內，則可以看到
                    {
                        search_data(update_id);
                    }
                    if (userpart == "MNG")//如果登入的部門為管理部，則全部皆可以看到
                    {
                        search_data(update_id);
                    }
                }                
            }
            else
            {
                Response.Redirect("Business_Report.aspx", false);
            }
        }
        //進行刪除的事件
        protected void button_delete_Click(object sender, EventArgs e)
        {
            clsDB_sw.dbOpen(myclass.GetConnByDekVisErp);
            load_page_data();

            Response.Write(deletedata(title_name));
        }
        //進行刪除的動作
        private string deletedata(string title_name)
        {
            clsDB_sw.dbOpen(myclass.GetConnByDekVisErp);
            string sqlcmd = "SELECT * FROM Record_Report Where id = '" + update_id + "'";
            DataTable dt = clsDB_sw.DataTable_GetTable(sqlcmd);
            string d1 = "";
            if (dt.Rows.Count > 0)
            {
                if (clsDB_sw.Delete_Record("Record_Report", "id = '" + update_id + "'") == true)
                {
                    d1 = "<script>alert('伺服器回應 : 以刪除成功!');location.href='../SYS_CONTROL/Business_Report.aspx';</script>";
                }
                else
                {
                    d1 = "<script>alert('伺服器回應 : 失敗了!');location.href='../SYS_CONTROL/Business_Report.aspx';</script>";
                }
            }

            return d1;

        } 
        //把資料載入到一開始登入時，將資料載入前端
        private void search_data(string update_id)
        {
            clsDB_sw.dbOpen(myclass.GetConnByDekVisErp);
            string sqlcmd = "SELECT * FROM Record_Report where id= '" + update_id + "'";
            DataTable dt = clsDB_sw.DataTable_GetTable(sqlcmd);
            if (dt.Rows.Count > 0)
            {
                title_name = DataTableUtils.toString(dt.Rows[0]["title"]);
                type = DataTableUtils.toString(dt.Rows[0]["type"]);
                name = DataTableUtils.toString(dt.Rows[0]["name"]);
                time = DataTableUtils.toString(dt.Rows[0]["updatetime"]);
                part = DataTableUtils.toString(dt.Rows[0]["part"]);
                detail = DataTableUtils.toString(dt.Rows[0]["detail"]);
            }
        }
        //返回上一頁
        protected void button_return_Click(object sender, EventArgs e)
        {
            Response.Redirect("Business_Report.aspx");
        }
        //只載入登入身分的部門與能看的權限
        private string allowsee(string update_id)
        {
            clsDB_sw.dbOpen(myclass.GetConnByDekVisErp);
            string sqlcmd = "SELECT * FROM Record_Report where id= '" + update_id + "'";
            DataTable dt = clsDB_sw.DataTable_GetTable(sqlcmd);
            if (dt.Rows.Count > 0)
            {
                //確認輸入者的部門
                part = DataTableUtils.toString(dt.Rows[0]["part"]);
                //確認權限
                seeman = DataTableUtils.toString(dt.Rows[0]["permission"]);
                seeman = part + "," + seeman;
            }
            return seeman;
        }
        //跳頁至新增頁面，並同時把ID傳輸出去
        protected void button_update_Click(object sender, EventArgs e)
        {
            load_page_data();
            Response.Redirect("Add_Report.aspx?id=" + update_id);
        }             
        //抓使用者權限
        private string catchpower(string see_name)
        {
            clsDB_sw.dbOpen(myclass.GetConnByDekVisErp);
            string sqlcmd = "SELECT * FROM USERS where USER_ACC= '" + see_name + "'";
            DataTable dt = clsDB_sw.DataTable_GetTable(sqlcmd);
            string power = "";
            if (dt.Rows.Count > 0)
            {
                //確認輸入者的部門權限
                power = DataTableUtils.toString(dt.Rows[0]["ADM"]);
            }
            return power;
        }
    }
}