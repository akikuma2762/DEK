using dek_erpvis_v2.cls;
using dek_erpvis_v2.webservice;
using ERPVIS;
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
    public partial class transportrackstatistics : System.Web.UI.Page
    {
        public string color = "";
        public string th = "";
        public string tr = "";
        public string title_text = "";
        public string time_area_text = "";
        public string col_data_points_nor = "";
        public string col_data_points_sply = "";
        public string path = "";
        DataTable Line = new DataTable();
        DataTable custom = new DataTable();
        DataTable dt = new DataTable();
        string[] str = null;
        int Total = 0;
        myclass myclass = new myclass();
        string URL_NAME = "";
        string acc = "";
        Service service = new Service();
        //---------------------------EVENT-------------------------------------------------------
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                path = 德大機械.get_title_web_path("SLS");
                path = path.Replace("</ol>", "<li><u><a href='../dp_SD/transportrackstatistics.aspx'>運輸架未歸還統計</a></u></li></ol>");
                URL_NAME = "transportrackstatistics";
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                color = HtmlUtil.change_color(acc);
                if (myclass.user_view_check(URL_NAME, acc) == true)
                {
                    if (!IsPostBack)
                        MainProcess();
                }
                else
                    Response.Write("<script>alert('您無法瀏覽此頁面 請向該單位主管申請權限!');location.href='../index.aspx';</script>");
            }
            else
                Response.Redirect(myclass.logout_url);
        }
        //---------------------------FUNCTION----------------------------------------------------
        private void MainProcess()
        {
            Set_Image();
            Set_HtmlTable();
        }
        private void Set_Image()
        {
            string Json = service.GettransportrackstatisticsInformation(dekModel.Image);
            dt = ERPVIS.i_TEC.iTechDB.JsontoDataTable(Json);
            int count = 0;
            col_data_points_nor = HtmlUtil.Set_Chart(dt, "品號", "數量", "", out count, out _);
            title_text = "運輸架在外數量：" + count;
        }
        private void Set_HtmlTable()
        {
            string Json = service.GettransportrackstatisticsInformation(dekModel.Table);
            dt = ERPVIS.i_TEC.iTechDB.JsontoDataTable(Json);
            Line = dt.DefaultView.ToTable(true, new string[] { "品號" });
            custom = dt.DefaultView.ToTable(true, new string[] { "客戶簡稱" });

            string titlename = "";
            th = "<th>客戶簡稱</th>\n";
            th += HtmlUtil.Set_Table_Title(Line, out titlename, "品號");
            th += "<th>小計</th>\n";
            titlename = "客戶簡稱," + titlename + "小計,";
            str = titlename.Split(',');

            tr = HtmlUtil.Set_Table_Content(custom, titlename, transportrackstatistics_callback);
        }
        private string transportrackstatistics_callback(DataRow row, string field_name)
        {
            string value = "";
            if (field_name != "客戶簡稱" && field_name != "小計")
            {
                //進入第一個產線後，將上一個總和歸0
                if (field_name == str[1])
                    Total = 0;
                string sqlcmd = "客戶簡稱 ='" + DataTableUtils.toString(row["客戶簡稱"]) + "' and 品號 = '" + field_name + "'";
                DataRow[] rows = dt.Select(sqlcmd);
                int LINE_TOTAL = 0;
                if (rows.Length == 0)
                {

                }
                else
                {
                    for (int i = 0; i < rows.Length; i++)
                    {
                        for (int j = 0; j < Line.Rows.Count; j++)
                        {
                            if (field_name == DataTableUtils.toString(rows[i]["品號"]) && field_name == DataTableUtils.toString(Line.Rows[j]["品號"]))
                                LINE_TOTAL += DataTableUtils.toInt(DataTableUtils.toString(rows[i]["數量"]));
                        }
                    }
                }
                value = DataTableUtils.toString(LINE_TOTAL);
                Total += LINE_TOTAL;
            }
            else if (field_name == "小計")
                value = Total.ToString();
            if (value == "")
                return "";
            else
                return "<td>" + value + "</td>";
        }
    }
}