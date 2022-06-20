using dek_erpvis_v2.cls;
using dek_erpvis_v2.webservice;
using dekERP_dll.dekErp;
using Support;
using Support.DB;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace dek_erpvis_v2.pages.dp_SD
{
    public partial class Previous_Shipment : System.Web.UI.Page
    {
        //-------------------------------------------------參數 OR 引用------------------------------------------------------------
        public string color = "";
        public string path = "";
        public StringBuilder th = new StringBuilder();
        public StringBuilder tr = new StringBuilder();
        string acc = "";
        DataTable dt_monthtotal = new DataTable();
        clsDB_Server cls_erp = new clsDB_Server(myclass.GetConnByDekVisErp);

        //----------------------------------------------------Event----------------------------------------------------------------------
        //載入事件
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                color = HtmlUtil.change_color(acc);
                path = 德大機械.get_title_web_path("SLS");
                if (true || myclass.user_view_check("Previous_Shipment", acc))
                {
                    if (!IsPostBack)
                    {
                        string[] date = 德大機械.德大專用月份(acc).Split(',');
                        txt_str.Text = HtmlUtil.changetimeformat(date[0], "-");
                        txt_end.Text = HtmlUtil.changetimeformat(date[1], "-");
                        MainProcess();
                    }
                }
                else
                    Response.Write("<script>alert('您無法瀏覽此頁面 請向該單位主管申請權限!');location.href='../index.aspx';</script>");
            }
            else
                Response.Redirect(myclass.logout_url);
        }

        //查詢事件
        protected void button_select_Click(object sender, EventArgs e)
        {
            MainProcess();
        }

        //刪除事件
        protected void Button_Delete_Click(object sender, EventArgs e)
        {
            string sqlcmd = $"select * from Shipment_Remark where id={TextBox_Delete.Text}";
            DataTable dt = cls_erp.GetDataTable(sqlcmd);

            if (HtmlUtil.Check_DataTable(dt))
            {
                if (cls_erp.Delete_Record("Shipment_Remark", $"id={TextBox_Delete.Text}"))
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", $"<script>alert('刪除成功');location.href='Previous_Shipment.aspx';</script>");
                else
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", $"<script>alert('刪除失敗');location.href='Previous_Shipment.aspx';</script>");
            }
            else
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", $"<script>alert('刪除失敗');location.href='Previous_Shipment.aspx';</script>");

        }

        //更新事件
        protected void Button_Update_Click(object sender, EventArgs e)
        {
            string sqlcmd = $"select * from Shipment_Remark where id={TextBox_Update.Text}";
            DataTable dt = cls_erp.GetDataTable(sqlcmd);

            if (HtmlUtil.Check_DataTable(dt))
            {
                DataRow row = dt.NewRow();
                row["id"] = dt.Rows[0]["id"];
                row["date"] = TextBox_Date.Text.Replace("-", "");
                row["custom"] = TextBox_Custom.Text;
                row["machine_order"] = TextBox_MachineOrder.Text;
                row["machine_remark"] = TextBox_MachineRemark.Text;
                row["driver"] = DropDownList_driver.SelectedItem.Value;
                row["driver_remark"] = TextBox_DriverRemark.Text;
                row["trip_number"] = DropDownList_period.SelectedItem.Value;
                row["last_updatetime"] = DateTime.Now.ToString("yyyyMMddHHmmss");
                row["last_updateacc"] = acc;
                if (cls_erp.Update_DataRow("Shipment_Remark", $"id={TextBox_Update.Text}", row))
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", $"<script>alert('更新成功');location.href='Previous_Shipment.aspx';</script>");
                else
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", $"<script>alert('更新失敗');location.href='Previous_Shipment.aspx';</script>");
            }
            else
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", $"<script>alert('更新失敗');location.href='Previous_Shipment.aspx';</script>");
        }
        //-----------------------------------------------------Function---------------------------------------------------------------------      
        //需要執行的程式
        private void MainProcess()
        {
            Set_DriverDropDownList();
            Set_PeriodDropDownList();
            Get_MonthTotal();
            Set_Table();
        }

        //設定司機的下拉選單
        private void Set_DriverDropDownList()
        {
            List<string> list = new List<string>(WebUtils.GetAppSettings("driver_name").Split(','));
            if (list.Count > 0 && list[0] != "" && DropDownList_driver.Items.Count == 0)
            {
                foreach (string item in list)
                {
                    ListItem listItem = new ListItem(item, item);
                    DropDownList_driver.Items.Add(listItem);
                }
            }
        }

        //設定時段的下拉選單
        private void Set_PeriodDropDownList()
        {
            List<string> list = new List<string>(WebUtils.GetAppSettings("period").Split(','));
            if (list.Count > 0 && list[0] != "" && DropDownList_period.Items.Count == 0)
            {
                foreach (string item in list)
                {
                    ListItem listItem = new ListItem(item, item);
                    DropDownList_period.Items.Add(listItem);
                }
            }
        }

        //取得未交易數
        private void Get_MonthTotal()
        {
            string sqlcmd = $"select id 刪除,id 編輯,date 出貨日期,custom 客戶簡稱,machine_order 機號,machine_remark 機號備註,driver 司機,driver_remark 司機備註,trip_number 時段  from Shipment_Remark where {txt_str.Text.Replace("-", "")} <= date and date <= {txt_end.Text.Replace("-", "")}";
            dt_monthtotal = cls_erp.GetDataTable(sqlcmd);
        }

        //設定表格
        private void Set_Table()
        {
            DataTable dt = dt_monthtotal;
            if (HtmlUtil.Check_DataTable(dt))
            {
                List<string> list = new List<string>(WebUtils.GetAppSettings("update_preshipment").Split(','));
                if (list.Count == 0 || list.IndexOf(acc) != -1)
                {

                }
                else
                {
                    dt.Columns.Remove("編輯");
                    dt.Columns.Remove("刪除");
                }

                string titlename = "";
                th.Append(HtmlUtil.Set_Table_Title(dt, out titlename));
                tr.Append(HtmlUtil.Set_Table_Content(dt, titlename, Write_ShipmentInformation_callback).Replace(Environment.NewLine, "</br>"));
            }
            else
                HtmlUtil.NoData(out th, out tr);
        }

        //例外處理
        private string Write_ShipmentInformation_callback(DataRow row, string field_name)
        {
            string value = "";
            if (field_name == "出貨日期")
                value = HtmlUtil.changetimeformat(row[field_name].ToString());

            else if (field_name == "編輯")
            {
                string message = $"{HtmlUtil.changetimeformat(row["出貨日期"].ToString(), "-")}Ω{row["客戶簡稱"]}Ω{row["機號"]}Ω{row["機號備註"]}Ω{row["司機"]}Ω{row["司機備註"]}Ω{row["時段"]}Ω";
                value = $"<a href=\"javascript:void()\"  data-toggle = \"modal\" data-target = \"#exampleModal\" onclick=update_row(\"{row[field_name]}\",\"{message.Replace(Environment.NewLine, "Θ")}\")><u>編輯</u></a>";
            }
            else if (field_name == "刪除")
                value = $"<a href=\"javascript:void()\" onclick=delete_row(\"{row[field_name]}\")><u>刪除</u></a>";

            return value == "" ? "" : $"<td>{value}</td>";
        }
    }
}