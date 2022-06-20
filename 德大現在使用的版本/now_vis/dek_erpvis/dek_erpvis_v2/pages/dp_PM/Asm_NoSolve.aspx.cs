using dek_erpvis_v2.cls;
using Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace dek_erpvis_v2.pages.dp_PM
{
    public partial class Asm_NoSolve : System.Web.UI.Page
    {
        //-------------------------------------------------參數 OR 引用------------------------------------------------------------    
        public string th = "";
        public string tr = "";
        public string color = "";
        string acc = "";
        List<string> abnormal = new List<string>();
        ShareFunction sfun = new ShareFunction();
        DataTable dt_monthtotal = new DataTable();
        //-----------------------------------------------------EVENT-----------------------------------------------------------------------------
        //載入事件
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                color = HtmlUtil.change_color(acc);
                if (!IsPostBack)
                    MainProcess();
            }
            else
                Response.Redirect(myclass.logout_url);
        }

        //-----------------------------------------------------Function---------------------------------------------------------------------
        //需要執行的程式
        private void MainProcess()
        {
            GetCondi();
            Get_MonthTotal();
            Set_Table();
        }

        //設定數值
        private void GetCondi()
        {
            if (Request.QueryString["key"] != null)
            {
                Dictionary<string, string> keyValues = HtmlUtil.Return_dictionary(Request.QueryString["key"]);
                abnormal = new List<string>(HtmlUtil.Search_Dictionary(keyValues, "mach").Replace('*', '!').Split('!'));
            }
            else
                Response.Redirect("waitingfortheproduction.aspx", false);

        }

        //取得本月異常
        private void Get_MonthTotal()
        {
            dt_monthtotal = sfun.GetNosovle(abnormal);
        }

        //設定表格
        private void Set_Table()
        {
            DataTable dt = dt_monthtotal;
            if (HtmlUtil.Check_DataTable(dt))
            {
                string titlename = "";
                th = HtmlUtil.Set_Table_Title(dt, out titlename, "", "style='text-align:center'");
                tr = HtmlUtil.Set_Table_Content(dt, titlename, Asm_NoSolve_callback, "style='text-align:center'");
            }
            else
                HtmlUtil.NoData(out th, out tr);
        }

        //例外處理
        private string Asm_NoSolve_callback(DataRow row, string field_name)
        {
            string value = "";

            if (field_name == "上線日")
                value = HtmlUtil.StrToDate(DataTableUtils.toString(row[field_name])).ToString("MM/dd");
            else if (field_name == "預定進度")
            {

                string linenum = "";
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
                string sqlcmd = $"SELECT  工作站編號 from 工作站型態資料表 where 工作站名稱 = '{row["產線"]}'";
                DataTable dt = DataTableUtils.GetDataTable(sqlcmd);

                if (HtmlUtil.Check_DataTable(dt))
                    linenum = dt.Rows[0]["工作站編號"].ToString();

                string result = sfun.percent_calculation(DataTableUtils.toString(row["編號"]), DataTableUtils.toString(row["進度"]), ref sqlcmd, linenum)[1].ToString();
                if (result == "非數值")
                    value = "開發機";
                else
                {
                    if (DataTableUtils.toDouble(result) * 100 > 100)
                        value = "100%";
                    else
                        value = (DataTableUtils.toDouble(result) * 100).ToString("0") + "%";
                }
            }
            else if (field_name == "進度")
                value = row[field_name].ToString() + "%";
            else if (field_name == "未解決數量")
            {
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
                string sqlcmd = $"SELECT  工作站編號 from 工作站型態資料表 where 工作站名稱 = '{row["產線"]}'";
                DataTable dt = DataTableUtils.GetDataTable(sqlcmd);

                if (HtmlUtil.Check_DataTable(dt))
                {
                    string url = $"ErrorID={row["編號"]},ErrorLineNum={dt.Rows[0]["工作站編號"]},ErrorLineName={row["產線"]}";
                    value = $"<u><a href=\"Asm_ErrorDetail.aspx?key={WebUtils.UrlStringEncode(url)}\">{row[field_name]}</a></u>";
                }
            }

            if (value == "")
                return "";
            else
                return $"<td style=\"text-align:center\">{value}</td>";
        }
    }
}