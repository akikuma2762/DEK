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


namespace dek_erpvis_v2.pages.SYS_CONTROL
{
    public partial class Write_ShipmentInformation : System.Web.UI.Page
    {

        //-------------------------------------------------參數 OR 引用------------------------------------------------------------
        public string color = "";
        string acc = "";
        //----------------------------------------------------Event----------------------------------------------------------------------
        //載入事件
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                color = HtmlUtil.change_color(acc);
                List<string> list = new List<string>(WebUtils.GetAppSettings("insert_preshipment").Split(','));

                if ((list.Count == 1 && list[0] == "") || list.IndexOf(acc) != -1)
                {

                }
                else
                    Response.Write("<script>alert('您無法瀏覽此頁面 請向該單位主管申請權限!');location.href='../index.aspx';</script>");
            }
            else
                Response.Redirect(myclass.logout_url);
        }
        //存入事件
        protected void Button_Add_Click(object sender, EventArgs e)
        {
            //取得所寫之內容
            List<string> list = new List<string>(TextBox_Content.Text.Split('Ω'));

            //取得所寫之數量
            int total = (list.Count - 1) / 7;

            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisErp);
            string sqlcmd = "select * from Shipment_Remark order by id desc";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);

            DataTable dt_clone = dt.Clone();
            if (dt != null)
            {
                int max = HtmlUtil.Check_DataTable(dt) ? DataTableUtils.toInt(dt.Rows[0]["id"].ToString()) + 1 : 1;

                for (int i = 0; i < total; i++)
                {
                    DataRow row = dt_clone.NewRow();
                    row["id"] = max;
                    row["date"] = list[i * 7 + 0].Replace("-", "");
                    row["custom"] = list[i * 7 + 1];
                    row["machine_order"] = list[i * 7 + 2];
                    row["machine_remark"] = list[i * 7 + 3];
                    row["driver"] = list[i * 7 + 4];
                    row["driver_remark"] = list[i * 7 + 5];
                    row["trip_number"] = list[i * 7 + 6];
                    row["add_time"] = DateTime.Now.ToString("yyyyMMddHHmmss");
                    row["add_acc"] = acc;
                    dt_clone.Rows.Add(row);
                    max++;
                }
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisErp);
                if (DataTableUtils.Insert_TableRows("Shipment_Remark", dt_clone) == total)
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", $"<script>alert('儲存成功');location.href='Write_ShipmentInformation.aspx';</script>");
                else
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", $"<script>alert('儲存失敗');location.href='Write_ShipmentInformation.aspx';</script>");
            }
            else
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", $"<script>alert('儲存失敗');location.href='Write_ShipmentInformation.aspx';</script>");
        }
    }
}