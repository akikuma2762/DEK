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
    public partial class APSList_Details : System.Web.UI.Page
    {
        public string key = "";
        public string color = "";
        string acc = "";
        public string th = "";
        public string tr = "";
        myclass myclass = new myclass();
        //網頁載入事件
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                color = HtmlUtil.change_color(acc);
                if (Request.QueryString["key"] != null)
                {
                    if (!IsPostBack)
                        getdata(HtmlUtil.Return_str(Request.QueryString["key"])[1]);
                }
                else
                    Response.Redirect("APSList.aspx");
            }
            else
                Response.Redirect(myclass.logout_url);
        }
        private void getdata(string product_num)
        {
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisAps);

            string sql_cmd = "SELECT  " +
                                "status as 狀態, " +
                                // "Project as 送料單號, " +
                                "Job as 品名規格, " +
                                "TaskName as 工藝名稱,  " +
                                "Resource as 機台名稱, " +
                                "StartTime as 預定起始時間, " +
                                "EndTime as 預定結束時間, " +
                                "CurrentPiece as 累積數量, " +
                                "TargetPiece as 需求總數量, " +
                                "Note as 備註  " +
                                "FROM " + ShareMemory.SQLWorkHour +
                                " where Project = '" + product_num + "' ";
            DataTable dt = DataTableUtils.GetDataTable(sql_cmd);
            if (dt.Rows.Count > 0 && dt != null)
            {
                List<string> list = new List<string>();

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    th = th += "<th align='center'>" + dt.Columns[i] + "</th>";
                    list.Add(DataTableUtils.toString(dt.Columns[i]));
                }
                list.Add(" ");

                tr = HtmlUtil.Set_Table_Content(dt, list, apsDetail_callback);
            }
            else
                HtmlUtil.NoData(out th, out tr);
        }
        private string apsDetail_callback(DataRow row, string field_name)
        {
            string value = "";
            string Status = "";
            string CountNow = "0";
            if (field_name == "狀態")
            {
                if (DataTableUtils.toString(row["狀態"]) == "")
                    Status = "READY";                else
                    Status = DataTableUtils.toString(row["狀態"]);
                value = "<img class='img-rounded' src='../../assets/images/" + Status + ".PNG' alt='...' style='width:35px;height:35px;align-items:center;'>";
                value = "<td  style='width:7%'>" + value + "</td>\n";
            }
            if (field_name == "預定起始時間" || field_name == "預定結束時間")
            {
                DateTime dt = DateTime.ParseExact(DataTableUtils.toString(row[field_name]), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                value = dt.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture) + "&nbsp " + dt.ToString("HH:mm", System.Globalization.CultureInfo.CurrentCulture);
                value = "<td  style='width:15%'>" + value + "</td>\n";
            }
            if (field_name == "累積數量")
            {
                if (!string.IsNullOrEmpty(DataTableUtils.toString(row[field_name])))
                    CountNow = DataTableUtils.toString(row[field_name]);
                value = "<td>" + CountNow + "</td>\n";
            }
            return value;
        }
    }
}