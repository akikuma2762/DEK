using dek_erpvis_v2.cls;
using Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace dek_erpvis_v2.pages.dp_APS
{
    public partial class WorkHourDetailEdit : System.Web.UI.Page
    {

        public string pagename = "";
        public string Super_Link = "";
        public string color = "";
        string acc = "";
        public string P_Order = "";
        public string G_Order = "";
        public string O_Order = "";
        public string T_Order = "";
        public string CurrentPiece = "";
        public string TargetPiece = "";
        myclass myclass = new myclass();
        public string status_button = "";
        //-------------------------------------------------事件------------------------------------
        //網頁載入事件
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                string URL_NAME = "OrderList";
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                color = HtmlUtil.change_color(acc);
                if (myclass.user_view_check(URL_NAME, acc) == true)
                {
                    if (!IsPostBack)
                    {
                        create_button();
                        Read_Data();

                    }
                }
                else  Response.Write("<script>alert('您無法瀏覽此頁面 請向該單位主管申請權限!');location.href='../index.aspx';</script>"); 
            }
            else Response.Redirect(myclass.logout_url);
        }
        //儲存與修改事件
        protected void Button_Save_Click(object sender, EventArgs e)
        {
            string[] value = HtmlUtil.Return_str(Request.QueryString["key"]);
            if (value.Length == 2)
                Save_Data(value[1], TextBox_status.Text, TextBox_Date.Text, TextBox_Time.Text, TextBox_Count.Text, TextBox_Man.Text);
            else
                Add_Data(value, TextBox_status.Text, TextBox_Date.Text, TextBox_Time.Text, TextBox_Count.Text, TextBox_Man.Text);
        }
        //------------------------------------------------方法-------------------------------------
        //讀取?後面文字
        private void Read_Data()
        {
            DataRow dr_WH;
            //ini_cbx();
            if (Request.QueryString["key"] != null)
            {
                string[] value = HtmlUtil.Return_str(Request.QueryString["key"]);
                if (value.Length == 2)
                {
                    Show_Data(value[1]);
                    pagename = "修改報工明細";
                }
                else if (value.Length == 6)
                {
                    G_Order = value[1];
                    O_Order = value[3];
                    T_Order = value[5];
                    dr_WH = DataTableUtils.DataTable_GetDataRow(ShareMemory.SQLWorkHour, "Project=" + "'" + O_Order + "'" + " AND " + "TaskName=" + "'" + T_Order + "'");
                    if (dr_WH != null)
                    {
                        P_Order = dr_WH["Job"].ToString();
                        CurrentPiece = string.IsNullOrEmpty(dr_WH["CurrentPiece"].ToString()) == true ? "0" : dr_WH["CurrentPiece"].ToString();
                        TargetPiece = dr_WH["TargetPiece"].ToString();
                    }
                    Button_Save.Text = "新增";
                    pagename = "新增報工明細";
                    TextBox_Date.Text = DateTime.Now.ToString("yyyy-MM-dd");
                    TextBox_Time.Text = DateTime.Now.ToString("HH:mm");
                    TextBox_Man.Text = ShareFunction_APS.GetAccName(acc);
                }
            }
            else
                Response.Redirect("OrderList.aspx");

            Super_Link = "<ol class=\"breadcrumb_\">" +
                            "<li><u><a href=\"../index.aspx\">首頁 </a></u></li> " +
                            "<li><u><a href=\"../dp_APS/OrderList.aspx\"> 報工清單 </a></u></li> " +
                            "<li><u><a href=\"../dp_APS/WorkHourList.aspx?key=" + WebUtils.UrlStringEncode("OrderNum=" + O_Order) + "\"> 報工列表 </a></u></li> " +
                            "<li><u><a href=\"../dp_APS/WorkHourDetail.aspx?key=" + WebUtils.UrlStringEncode("WorkHourID=" + G_Order + ",Project=" + O_Order + ",TaskName=" + T_Order) + "\"> 報工明細 </a></u></li>" +
                            "<li>" + pagename + "</li>" +
                        "</ol>";
        }
        //把ID相關資料放入TEXTBOX內，以供使用者修改
        private void Show_Data(string ID)
        {
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisErp);
            string sqlcmd = "SELECT * FROM " + ShareMemory.SQLWorkHour_Detail + " where ID = '" + ID + "'";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisAps);
            status_button = "";
            create_button(DataTableUtils.toString(dt.Rows[0]["Status"]));
            foreach (DataRow row in dt.Rows)
            {
                G_Order = DataTableUtils.toString(row["WorkHourID"]);
                O_Order = DataTableUtils.toString(row["Project"]);
                T_Order = DataTableUtils.toString(row["TaskName"]);
                TextBox_Man.Text = DataTableUtils.toString(row["staffname"]);
                TextBox_Date.Text = DataTableUtils.toString(row["workDate"]);
                TextBox_Count.Text = DataTableUtils.toString(row["Piece"]);
                TextBox_Time.Text = DataTableUtils.toString(row["WorkTime"]);
            }

        }
       
        //修改後存入資料庫
        private void Save_Data(string ID, string Status, string Date, string Time, string Count, string Man = "")
        {
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisAps);
            string sqlcmd = "SELECT * FROM " + ShareMemory.SQLWorkHour_Detail + " where ID = '" + ID + "'";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
            DataRow dr_status = DataTableUtils.DataTable_GetDataRow(ShareMemory.SQLWorkHour_MachStatus, "Status=" + "'" + Status + "'");
            bool OK = false;
            //避免刷新後資料消失，導致跳頁後沒有資訊
            foreach (DataRow row in dt.Rows)
            {
                G_Order = DataTableUtils.toString(row["WorkHourID"]);
                O_Order = DataTableUtils.toString(row["Project"]);
                T_Order = DataTableUtils.toString(row["TaskName"]);
            }
            string alert = ShareFunction_APS.Calculate_QTY(O_Order, T_Order, Count, 1);
            if (string.IsNullOrEmpty(alert))
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.NewRow();
                    row["ID"] = ID;
                    row["Status"] = dr_status["StatusEn"];
                    row["WorkDate"] = Date;
                    row["WorkTime"] = Time;
                    row["StaffName"] = Man;
                    row["piece"] = Count;
                    if (DataTableUtils.Update_DataRow(ShareMemory.SQLWorkHour_Detail, "ID = '" + ID + "'", row))
                    {
                        OK = true;
                        //UPdata order APS List Status 
                        ShareFunction_APS.UpdataWorkHourData(O_Order, T_Order, WorkHourEditSource.報工明細, Status);
                        //Updata CNC VIS Status
                        ShareFunction_APS.UpdataCNCVisStatus(O_Order, T_Order, Status, WorkHourEditSource.報工明細);
                        //Order Status 
                        ShareFunction_APS.UpdataWorkHourOrderData(O_Order, T_Order, WorkHourEditSource.報工明細, Status);
                        Response.Write("<script>location.href='WorkHourDetail.aspx?key=" + WebUtils.UrlStringEncode("WorkHourID=" + G_Order + ",Project=" + O_Order + ",TaskName=" + T_Order) + " '; </script>");
                    }
                }
            }
            else
                Response.Write("<script>alert('數量比目標多" + alert + "件，請重新輸入!');location.href='WorkHourDetailEdit.aspx" + Request.Url.Query + "'; </script>");
        }
        //新增進入資料庫
        private void Add_Data(string[] value, string Status, string Date, string Time, string Count, string Man = "")
        {
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisAps);
            DataTable dt = DataTableUtils.DataTable_GetRowHeader(ShareMemory.SQLWorkHour_Detail);
            DataRow dr_status = DataTableUtils.DataTable_GetDataRow(ShareMemory.SQLWorkHour_MachStatus, "Status=" + "'" + Status + "'");
            //確定按鈕按下之後還會存在
            G_Order = value[1];
            O_Order = value[3];
            T_Order = value[5];
            string alert = ShareFunction_APS.Calculate_QTY(O_Order, T_Order, Count, 2);
            if (string.IsNullOrEmpty(alert) && dt != null && !string.IsNullOrEmpty(Status))
            {
                DataRow row = dt.NewRow();
                row["piece"] = Count;
                row["Status"] = dr_status["StatusEn"];
                row["Project"] = O_Order;
                row["TaskName"] = T_Order;
                row["WorkHourID"] = G_Order;
                row["WorkDate"] = Date;
                row["WorkTime"] = Time;
                row["StaffName"] = Man;
                if (DataTableUtils.Insert_DataRow(ShareMemory.SQLWorkHour_Detail, row))
                {
                    //Updata order APS List Status 
                    ShareFunction_APS.UpdataWorkHourData(O_Order, T_Order, WorkHourEditSource.報工明細, Status);
                    //Updata CNC VIS Status
                    ShareFunction_APS.UpdataCNCVisStatus(O_Order, T_Order, Status, WorkHourEditSource.報工明細);
                    //Order Status 
                    ShareFunction_APS.UpdataWorkHourOrderData(O_Order, T_Order, WorkHourEditSource.報工明細, Status);
                    Response.Redirect("WorkHourDetail.aspx?key=" + WebUtils.UrlStringEncode("WorkHourID=" + G_Order + ",Project=" + O_Order + ",TaskName=" + T_Order));
                }
                else
                    Response.Redirect("WorkHourDetail.aspx?key=" + WebUtils.UrlStringEncode("WorkHourID=" + G_Order + ",Project=" + O_Order + ",TaskName=" + T_Order));

            }
            else
            {
                if (string.IsNullOrEmpty(Status))
                    Response.Redirect("WorkHourDetail.aspx?key=" + WebUtils.UrlStringEncode("WorkHourID=" + G_Order + ",Project=" + O_Order + ",TaskName=" + T_Order));
                else
                    Response.Redirect("WorkHourDetail.aspx?key=" + WebUtils.UrlStringEncode("WorkHourID=" + G_Order + ",Project=" + O_Order + ",TaskName=" + T_Order));
            }
        }

        private string Calculate_QTY(string ID, string QTY, string nowID = "")
        {
            int total = 0;
            int count = 0;
            if (nowID != "")
                nowID = " and b.ID <> '" + nowID + "' ";
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisAps);
            //先統計完全部的數量
            string sqlcmd = "SELECT a.ID as 序號," +
                            " a.TargetPiece as 目標件數," +
                            " sum(b.piece) as 目前件數 " +
                            " FROM " + ShareMemory.SQLWorkHour + " as a " +
                            " left join " + ShareMemory.SQLWorkHour_Detail + " as b on a.id = b.WorkHourID " +
                            " where b.WorkHourID = '" + ID + "'" + nowID;
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
            total = DataTableUtils.toInt(DataTableUtils.toString(dt.Rows[0]["目標件數"]));
            count = DataTableUtils.toInt(DataTableUtils.toString(dt.Rows[0]["目前件數"])) + DataTableUtils.toInt(QTY);

            if (count > total && total != 0)
                return (count - total).ToString();
            else
                return "";
        }
        //產生按鈕
        private void create_button(string status = "")
        {
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisAps);
            string sqlcmd = "select * from workhour_machstatus where work_show = 'Y'";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);

            foreach (DataRow row in dt.Rows)
            {
                if (status == DataTableUtils.toString(row["StatusEn"]))
                {
                    status_button += "<label class=\"btn btn-default active\" onclick=\"status('" + DataTableUtils.toString(row["Status"]) + "')\">" +
"<input type=\"radio\" name=\"" + DataTableUtils.toString(row["Status"]) + "\" id=\"" + DataTableUtils.toString(row["Status"]) + "\" class=\"radio\" value=\"" + DataTableUtils.toString(row["Status"]) + "\">" +
"" + DataTableUtils.toString(row["Status"]) + "" +
"</label>";
                    TextBox_status.Text = DataTableUtils.toString(row["Status"]);
                }

                else
                    status_button += "<label class=\"btn btn-default\" onclick=\"status('" + DataTableUtils.toString(row["Status"]) + "')\">" +
                                     "<input type=\"radio\" name=\"" + DataTableUtils.toString(row["Status"]) + "\" id=\"" + DataTableUtils.toString(row["Status"]) + "\" class=\"radio\" value=\"" + DataTableUtils.toString(row["Status"]) + "\">" +
                                     "" + DataTableUtils.toString(row["Status"]) + "" +
                                     "</label>";
            }

        }
    }
}