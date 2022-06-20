using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using dek_erpvis_v2.cls;
using System.Data;
using Support;


namespace dek_erpvis_v2.pages.dp_PD
{
    public partial class Asm_LineTotalView : System.Web.UI.Page
    {
        public int no_solved = 0;
        public int behind = 0;
        public string color = "";
        public string ColumnsData = "<th class='center'>沒有資料載入</th>";
        public string RowsData = "<tr class='even gradeX'> <td class='center'> no data </ td ></ tr >";
        public string NowYear = "";
        public string NowMonth = "";
        public string TotalTagetPiece = "0";
        public string TotalTagetPerson = "0";
        public string TotalFinishPiece = "0";
        public string TotalOnLinePiece = "0";
        public string TotalErrorPiece = "0";
        public string td_TotalFinishPiece = "0";
        public string td_TotalOnLinePiece = "0";
        public string td_TotalErrorPiece = "0";
        public string NowDay = "";
        public string PieceUnit = ShareMemory.PieceUnit;
        clsDB_Server clsDB_sw = new clsDB_Server("");
        ShareFunction SFun = new ShareFunction();
        myclass myclass = new myclass();
        bool TestMode = true;
        private void GotoCenn()
        {

            clsDB_sw.dbOpen(SFun.GetConnByDekVisTmp);
            DataTableUtils.Conn_String = SFun.GetConnByDekVisTmp;
            if (clsDB_sw.IsConnected == true)
            {

                load_page_data();

            }
            else
                Response.Write("<script language='javascript'>alert('伺服器回應 : 無法載入資料，" + clsDB_sw.ErrorMessage + " 請聯絡德科人員或檢查您的網路連線。');</script>");

        }
        private void load_page_data()
        {
            if (GlobalVar.Conn_status == true)
            {
                //資料庫連線成功要做的事()
                string[] date = new string[6];
                ColumnsData = SFun.GetColumnName("Asm_LineTotalView");
                date = SFun.GetLineTotal(6, ref no_solved, ref behind); // columns 6 //
                RowsData = date[1];
                TotalTagetPiece = SFun.cTotalTagetPiece;
                TotalTagetPerson = SFun.cTotalTagetPerson;
                TotalFinishPiece = SFun.cTotalFinishPiece;
                TotalOnLinePiece = SFun.cTotalOnLinePiece;
                TotalErrorPiece = SFun.cTotalErrorPiece;
                td_TotalFinishPiece = SFun.td_cTotalFinishPiece;
                td_TotalOnLinePiece = SFun.td_cTotalOnLinePiece;
                td_TotalErrorPiece = SFun.td_cTotalErrorPiece;
            }
            else
            {
                Response.Write("<script language='javascript'>alert('伺服器回應 : 無法載入資料，" + " 請聯絡德科人員或檢查您的網路連線。');</script>");
                //資料庫連線失敗要做的事()
            }
        }
        //============Event=======================
        protected void Page_Load(object sender, EventArgs e)
        {
            string CompLoacation = "";
            HttpCookie userInfo = Request.Cookies["userInfo"];
            string acc = "";
            // HttpCookie CompInfo = Request.Cookies["VisCompany"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                string URL_NAME = "Asm_LineTotalView";
                color = HtmlUtil.change_color(acc);
                CompLoacation = ShareFunction.Last_Place(acc);
                ShareFunction.Last_Place(acc, CompLoacation);
                /*20200221修改這裡*/
                if (myclass.user_view_check(URL_NAME, acc) == true)
                {
                    if (!IsPostBack)
                    {
                        if (CompLoacation.ToUpper().Contains("HOR"))
                        {
                            SFun.GetConnByDekVisTmp = SFun.GetConnByDekdekVisAssmHor;
                            dropdownlist_X.SelectedValue = "1";
                        }
                        GotoCenn();
                    }

                }
                else
                    Response.Write("<script>alert('您無此權限!');location.href='../index.aspx';</script>");
            }
            else
                Response.Redirect(myclass.logout_url);
        }

        protected void bt_Hor_ServerClick(object sender, EventArgs e)
        {
            Control bt = (Control)sender;
            HttpCookie userInfo = Request.Cookies["userInfo"];
            string acc = DataTableUtils.toString(userInfo["user_ACC"]);//user_RW
            if (dropdownlist_X.SelectedValue == "1") //&& (acc == "detatina" || acc == "lin@deta.com.tw" || acc == "dora" || acc == "eerp" || acc == "visrd"))
            {
                SFun.GetConnByDekVisTmp = SFun.GetConnByDekdekVisAssmHor;
                ShareFunction.Last_Place(acc, "Hor");
            }
            else
            {
                SFun.GetConnByDekVisTmp = SFun.GetConnByDekdekVisAssm;
                ShareFunction.Last_Place(acc, "Ver");
            }
            GotoCenn();
        }

        protected void Button_Jump_Click(object sender, EventArgs e)
        {
            string url_link = "Asm_LineOverView.aspx?key=" + TextBox_textTemp.Text;
            Response.Redirect(url_link);
        }
    }
}