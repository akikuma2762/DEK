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
    public partial class OrderList : System.Web.UI.Page
    {

        public string color = "";
        string acc = "";
        public string Close_th = "";
        public string Close_tr = "";
        public string Open_th = "";
        public string Open_tr = "";
        public string Failure_th = "";
        public string Failure_tr = "";
        public string title = "";
        public string content = "";
        string check_rightStr = "";
        myclass myclass = new myclass();
        DataTable dt_machineStatus;
        //-------------------------------------------------------------事件---------------------------------------
        //網頁載入事件
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                //如果進入網頁沒有文字的話，就幫他補上
                if (string.IsNullOrEmpty(TextBox_Start.Text))
                {
                    TextBox_Start.Text = DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd");// "2019-01-01";
                    TextBox_End.Text = DateTime.Now.AddDays(100).ToString("yyyy-MM-dd");
                }
                string URL_NAME = "Orders";
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                color = HtmlUtil.change_color(acc);
                if (myclass.user_view_check(URL_NAME, acc) == true)
                {
                    if (!IsPostBack)
                        Read_Data(TextBox_Start.Text, TextBox_End.Text);
                }
                else
                    Response.Write("<script>alert('您無此權限!');location.href='../index.aspx';</script>");
            }
            else
                Response.Redirect(myclass.logout_url);
        }
        //搜尋事件
        protected void Button_Search_Click(object sender, EventArgs e)
        {
            Read_Data(TextBox_Start.Text, TextBox_End.Text);
        }
        //-------------------------------------------------------------方法----------------------------------------
        //依照起始日期跟結束日期讀取資料庫資料
        private void Read_Data(string start, string end)
        {
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisAps);
            double Start_Time = DataTableUtils.toDouble(start.Replace("-", "") + "000000");
            double End_Time = DataTableUtils.toDouble(end.Replace("-", "") + "235959");
            string sql_cmd = "SELECT a.ID as 編號,a.Project as 送料單號,dek_aps.workhour.Job as 品名規格,a.Status as 狀態"
                + " from " + ShareMemory.SQLWorkHour_Order + " as a "
                + " inner join " + ShareMemory.SQLWorkHour
                + " on a.jobid=" + ShareMemory.SQLWorkHour + ".jobid"
                + " where ((a.StartTime >= '" + Start_Time + "' and a.StartTime <= '" + End_Time + "')"
                + " or (a.EndTime >= '" + Start_Time + "' and a.EndTime <= '" + End_Time + "'))"
                + " Group by a.id";
            DataTable dt_all = DataTableUtils.GetDataTable(sql_cmd);
            if (dt_all != null && dt_all.Rows.Count != 0)
            {
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisAps);
                string sqlcmd = "SELECT * FROM " + ShareMemory.SQLWorkHour_OrderStatus;
                dt_machineStatus = DataTableUtils.GetDataTable(sqlcmd);
                check_rightStr = ShareFunction_APS.check_right(acc);
                //
                Set_Table(dt_all, "OPEN", out Open_th, out Open_tr);
                Set_Table(dt_all, "CLOSE", out Close_th, out Close_tr);
                Set_Table(dt_all, "FAILURE", out Failure_th, out Failure_tr);
            }
        }

        private void Set_Table(DataTable dt_all, string status, out string th, out string tr)
        {
            th = "";
            tr = "";
            var dt_open = dt_all.AsEnumerable().Where(w => w.Field<string>("狀態") == status);
            if (dt_open != null && dt_open.Count() != 0)
            {
                DataTable dt = dt_open.CopyToDataTable();
                if (dt != null && dt.Rows.Count != 0)
                {
                    dt.Columns.Add("進入報工");
                    List<string> list = new List<string>();
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        th += "<th>" + dt.Columns[i] + "</th>\n";
                        list.Add(DataTableUtils.toString(dt.Columns[i]));
                    }
                    list.Add(" ");
                    dt.Columns.Add("");
                    tr = HtmlUtil.Set_Table_Content(dt, list, Orderlist_callback);
                }
            }
        }

        //callback執行的地方
        private string Orderlist_callback(DataRow row, string field_name)
        {
            string value = "";
            if (field_name == "狀態")
            {
                value = "<td style='width:10%'>" +
                            "<select name=\"ctl00$ContentPlaceHolder1$DropDownList_" + DataTableUtils.toString(row["送料單號"]) + "\" onchange=func('" + DataTableUtils.toString(row["送料單號"]) + "')  id =\"ContentPlaceHolder1_DropDownList_" + DataTableUtils.toString(row["送料單號"]) + "\"" + check_rightStr + " >";
                for (int i = 0; i < dt_machineStatus.Rows.Count; i++)
                {
                    if (DataTableUtils.toString(dt_machineStatus.Rows[i]["Status_En"]) == DataTableUtils.toString(row[field_name]))
                        value += "	<option selected=\"selected\" value=" + DataTableUtils.toString(dt_machineStatus.Rows[i]["Status"]) + ">" + DataTableUtils.toString(dt_machineStatus.Rows[i]["Status"]) + "</option>";
                    else
                        value += "<option value=" + DataTableUtils.toString(dt_machineStatus.Rows[i]["Status"]) + ">" + DataTableUtils.toString(dt_machineStatus.Rows[i]["Status"]) + "</option>";
                }
                value += "</select>" + "</td>";
            }
            if (field_name == "進入報工")
            {
                if (DataTableUtils.toString(row["狀態"]) != "FAILURE")
                {
                    string url = string.Format("WorkHourList.aspx?key={0}",
                           WebUtils.UrlStringEncode(HtmlUtil.AttibuteValue("OrderNum", DataTableUtils.toString(row["送料單號"]), "")));
                    url = HtmlUtil.ToTag("u", HtmlUtil.ToHref("報工", url));
                    value = "<td style='width:10%'>" +
                                url +
                            "</td>";
                }
                else
                    value = "<td style='width:10%'>報工</td>";
            }
            if (field_name == "編號")
                value = "<td style='width:5%'>" + DataTableUtils.toString(row["編號"]) + "</td>";
            return value;
        }
        //更新下拉式選單的值
        protected void Button_change_Click(object sender, EventArgs e)
        {
            string[] str = TextBox_textTemp.Text.Split('^');
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisAps);
            string Order = str[0];
            string sqlcmd = "SELECT * FROM " + ShareMemory.SQLWorkHour_Order + " where Project='" + Order + "'";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.NewRow();
                row["ID"] = DataTableUtils.toString(dt.Rows[0]["ID"]);
                //
                DataRow dr_status = DataTableUtils.DataTable_GetDataRow(ShareMemory.SQLWorkHour_OrderStatus, "Status=" + "'" + str[1] + "'");
                if (dr_status != null)
                    row["Status"] = dr_status["Status_En"].ToString();
                if (DataTableUtils.Update_DataRow(ShareMemory.SQLWorkHour_Order, " Project='" + Order + "'", row))
                {
                    //Updata WorkHour
                    ShareFunction_APS.UpdataWorkHourData(Order, "", WorkHourEditSource.訂單, str[1]);
                    //Updata CNC Vis
                    ShareFunction_APS.UpdataCNCVisStatus(Order, "", str[1], WorkHourEditSource.訂單);
                    //Updata  WorkHourDetail
                    Response.Write("<script>alert('更新完畢')</script>");
                    Read_Data(TextBox_Start.Text, TextBox_End.Text);
                }
                else
                    Response.Write("<script>alert('更新失敗')</script>");

            }
        }
    }
}