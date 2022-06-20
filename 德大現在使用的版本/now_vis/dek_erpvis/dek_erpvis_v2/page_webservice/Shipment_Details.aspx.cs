using dek_erpvis_v2.cls;
using dek_erpvis_v2.webservice;
using Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace dek_erpvis_v2.page_webservice
{
    public partial class Shipment_Details : System.Web.UI.Page
    {
        public string color = "";
        public string th = "";
        public string tr = "";
        public string title = "";
        public string title_msg = "";
        public string title_msg_list = "";
        public string title_text = "";
        public string cust_name = "";
        public string date_str = "";
        public string date_end = "";
        string cust = "";
        string acc = "";
        myclass myclass = new myclass();
        Service service = new Service();
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                color = HtmlUtil.change_color(acc);
                if (acc != "")
                {
                    if (!IsPostBack)
                        load_page_data();
                }
                else
                    Response.Redirect("Shipment.aspx", false);
            }
            else
                Response.Redirect("Shipment.aspx", false);
        }

        private void load_page_data()
        {
            Response.BufferOutput = false;
            if (Request.QueryString["key"] != null)
            {
                string[] parameter = HtmlUtil.Return_str(Request.QueryString["key"]);
                cust_name = parameter[1];
                GlobalVar.UseDB_setConnString(myclass.GetConnByDataWin);
                string sqlcmd = "Select * from tbm01 where ba03='" + parameter[1] + "'";
                DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
                cust = DataTableUtils.toString(dt.Rows[dt.Rows.Count - 1]["ba01"]);
                date_str = parameter[3];
                date_end = parameter[5];
                Set_Html_Table();
            }
            else
                Response.Redirect("Shipment.aspx", false);
        }

        private void Set_Html_Table()
        {
            string Json = service.GetShipmentDetailInformation(date_str, date_end, cust);
            DataTable dt = ERPVIS.i_TEC.iTechDB.JsontoDataTable(Json);
            dt = myclass.Add_LINE_GROUP(dt).ToTable();
            dt.Columns.RemoveAt(0);
            dt.Columns["產線群組"].SetOrdinal(1);
            string title = "";
            th = HtmlUtil.Set_Table_Title(dt, out title);
            tr = HtmlUtil.Set_Table_Content(dt, title, ShipmentDetail_Callback);
        }
        private string ShipmentDetail_Callback(DataRow row, string field_name)
        {
            string value = "";
            if (field_name == "數量")
            {
                value = "<td data-toggle='modal' data-target='#exampleModal'>" +
                        "<u>" +
                        "<a href='javascript:void(0)' OnClick='GetShipment_details(" + '"' + date_str + '"' + "," + '"' + date_end + '"' + "," + '"' + cust + '"' + "," + '"' + DataTableUtils.toString(row["品號"]) + '"' + ")'>" +
                        DataTableUtils.toString(row["數量"]) +
                        "</a></u></td>\n";
            }

            if (value == "")
                return "";
            else
                return value;
        }
    }
}