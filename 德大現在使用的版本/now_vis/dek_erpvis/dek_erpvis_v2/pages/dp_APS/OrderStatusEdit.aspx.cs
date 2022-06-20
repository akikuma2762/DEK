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
    public partial class OrderStatusEdit : System.Web.UI.Page
    {
        public string color = "";
        string acc = "";
        public string Order = "";
        myclass myclass = new myclass();
        //-------------------------------------------事件-------------------------------------
        //網頁載入事件
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                color = HtmlUtil.change_color(acc);
                string URL_NAME = "APSList";
                if (myclass.user_view_check(URL_NAME, acc) == true)
                {
                    if (!IsPostBack)
                    {
                        ini_cbx();
                        Read_Data();
                    }
                }
                else
                    Response.Write("<script>alert('您無法瀏覽此頁面 請向該單位主管申請權限!');location.href='../index.aspx';</script>");
            }
            else
                Response.Redirect(myclass.logout_url);
        }
        //儲存事件
        protected void Button_Save_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["OrderNum"] != null)
                Order = Request.QueryString["OrderNum"].Split(',')[0];

            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisAps);
            string sqlcmd = "SELECT * FROM " + ShareMemory.SQLWorkHour_Order + " where Project='" + Order + "'";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
            if (dt != null && dt.Rows.Count > 0 && dt.Rows[0]["Status"].ToString() != DropDownList_Status.Text)
            {
                DataRow row = dt.NewRow();
                row["ID"] = DataTableUtils.toString(dt.Rows[0]["ID"]);
                row["Status"] = DropDownList_Status.Text;
                if (DataTableUtils.Update_DataRow(ShareMemory.SQLWorkHour_Order, "Project = '" + Order + "'", row))
                {
                    //Updata WorkHour
                    ShareFunction_APS.UpdataWorkHourData(Order, "", WorkHourEditSource.訂單, DropDownList_Status.Text);
                    //Updata CNC Vis
                    ShareFunction_APS.UpdataCNCVisStatus(Order, "", DropDownList_Status.Text, WorkHourEditSource.訂單);
                    //Updata  WorkHourDetail
                    Response.Write("<script>alert('修改完畢!');location.href='OrderList.aspx'; </script>");
                }
            }
            else
                Response.Write("<script>alert('資料異常或無變更狀態!');location.href='OrderList.aspx'; </script>");
        }
        //----------------------------------------方法-----------------------------------------------
        //把資料塞入下拉式選單
        private void ini_cbx()
        {
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisAps);
            string sqlcmd = "SELECT * FROM " + ShareMemory.SQLWorkHour_OrderStatus;
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);

            DropDownList_Status.Items.Clear();
            ListItem list = null;
            foreach (DataRow row in dt.Rows)
            {
                list = new ListItem(DataTableUtils.toString(row["Status"]), DataTableUtils.toString(row["Status"]));
                DropDownList_Status.Items.Add(list);
            }
        }
        //讀取相對應的資料
        private void Read_Data()
        {
            if (Request.QueryString["OrderNum"] != null)
                Order = Request.QueryString["OrderNum"].Split(',')[0];
            else
                Response.Redirect("OrderList.aspx");

            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisAps);
            string sqlcmd = "SELECT * FROM " + ShareMemory.SQLWorkHour_Order;
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);

            if (dt.Rows.Count > 0)
                DropDownList_Status.SelectedValue = DataTableUtils.toString(dt.Rows[0]["Status"]);
        }
    }
}