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
    public partial class Set_Month_WorkTime : System.Web.UI.Page
    {
        public string color = "";
        public string th = "";
        public string tr = "";
        string acc = "";
        string Link = "";
        string condition = "";

        public string workstation = "";
        public string workstation_Num = "";
        //public string dt_st = "";
        //public string dt_ed = "";
        //public string Get_SOStatus = "";
        //string Factory = "";
        //string mode = "";
        //public string dt_Row = "";
        //string selectItem_X = "";
        //public string table_Title = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                color = HtmlUtil.change_color(acc);
                if (HtmlUtil.Search_acc_Column(acc) == "Y" || true)
                {
                    if (!IsPostBack)
                    {
                        GetCondi();
                        set_DropDownlist();
                        set_Table();
                    }
                }
                else
                    Response.Write("<script>alert('您無法瀏覽此頁面 請向該單位主管主管申請權限!');location.href='../index.aspx';</script>");
            }
            else
                Response.Redirect(myclass.logout_url);
        }

        private void set_Table()
        {

        }
        protected void DropDownList_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            set_DropDownlist(true);
        }

        private void set_DropDownlist(bool Refalsh = false)
        {
            if (DropDownList_Type.SelectedItem.Value.ToLower() == "ver")
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
            else if (DropDownList_Type.SelectedItem.Value.ToLower() == "hor")
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);

            string sqlcmd = "select 工作站編號, 工作站名稱 from 工作站型態資料表   where 工作站是否使用中='1' ";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);

            if (HtmlUtil.Check_DataTable(dt))
            {
                if (Refalsh || DropDownList_Product.Items.Count == 0)
                {
                    ListItem listItem = new ListItem();
                    DropDownList_Product.Items.Clear();

                    foreach (DataRow row in dt.Rows)
                    {
                        listItem = new ListItem(DataTableUtils.toString(row["工作站名稱"]), DataTableUtils.toString(row["工作站編號"]));
                        DropDownList_Product.Items.Add(listItem);
                    }
                }
            }

        }

        //設定數值
        private void GetCondi()
        {
            Response.Buffer = false;
            if (Request.QueryString["key"] != null)
            {

                Dictionary<string, string> keyValues = HtmlUtil.Return_dictionary(Request.QueryString["key"]);
                workstation_Num = HtmlUtil.Search_Dictionary(keyValues, "workstation_Num");
                workstation = HtmlUtil.Search_Dictionary(keyValues, "workstation");
                //dt_ed = HtmlUtil.Search_Dictionary(keyValues, "date_end");
                //Get_SOStatus = HtmlUtil.Search_Dictionary(keyValues, "condi");
                ////--新增超連結 參數 20220615--//
                //Factory = HtmlUtil.Search_Dictionary(keyValues, "type");
                //mode = HtmlUtil.Search_Dictionary(keyValues, "mode");
                //dt_Row = HtmlUtil.Search_Dictionary(keyValues, "dt_Row");
                //cust_name = dt_Row;
                //selectItem_X = HtmlUtil.Search_Dictionary(keyValues, "selectItem_X");
                //table_Title = dt_Row;
                //if (mode.Contains("order_month_Overdue")) table_Title += "逾期";
                ////---------------------------//
                ////儲存cookie
                //Response.Cookies.Add(HtmlUtil.Save_Cookies("Order", table_Title));
            }
            else
                Response.Redirect("Set_Month_WorkTime.aspx", false);
        }

    }
}