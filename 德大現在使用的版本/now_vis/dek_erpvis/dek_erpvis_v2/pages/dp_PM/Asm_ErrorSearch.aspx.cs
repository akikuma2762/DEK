using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using dek_erpvis_v2.cls;
using System.Data;
using Support;

namespace dek_erpvis_v2.pages.dp_PM
{
    public partial class Asm_ErrorSearch : System.Web.UI.Page
    {
        public string color = "";
        public string ColumnsData = "<th class='center'>沒有資料載入</th>";
        public string RowsData = "<tr class='even gradeX'> <td class='center'> no data </ td ></ tr >";
        public string NowYear = "";
        public string NowMonth = "";
        public string NowDay = "";
        public string Key = "";
        public string ErrorType = "";
        public string LineNum = "";
        public string LineName = "";
        clsDB_Server clsDB_sw = new clsDB_Server("");
        ShareFunction SFun = new ShareFunction();
        myclass myclass = new myclass();
        private void GotoCenn()
        {
            clsDB_sw.dbOpen(SFun.GetConnByDekVisTmp);
            DataTableUtils.Conn_String = SFun.GetConnByDekVisTmp;
            if (clsDB_sw.IsConnected == true)
            {
                load_page_data();
            }
            else
            {
                Response.Write("<script language='javascript'>alert('伺服器回應 : 無法載入資料，" + clsDB_sw.ErrorMessage + " 請聯絡德科人員或檢查您的網路連線。');</script>");
                //SFun.NOdataProcess();
            }
        }
        private void load_page_data()
        {
            if (GlobalVar.Conn_status == true) //資料庫連線成功要做的事()
                LoadData();
            else   //資料庫連線失敗要做的事()
                Response.Write("<script language='javascript'>alert('伺服器回應 : 無法載入資料，" + " 請聯絡德科人員或檢查您的網路連線。');</script>");
        }
        private void LoadData()
        {
            GetDropDownList_LineName();
            GetDropDownList_ErrorType();
            // GetTableData()
        }
        private void GetTableData(string ErrorID, ListItem Line, string ErrotType)
        {
            string msg = "";
            string[] result = new string[2];
            GetDropDownList_LineName();
            GetDropDownList_ErrorType();
            ColumnsData = SFun.GetColumnName("Asm_ErrorSearch");
            result = SFun.GetErrorRowsData(ErrorID, Line, ErrotType);
            if (result[1] != null)
                RowsData = result[1];
            else
            {
                msg = "查無此資料，確認輸入資訊正確";
                Response.Write("<script language='javascript'>alert('伺服器回應 : " + msg + "');</script>");
                NoData();
            }
        }
        private void GetDropDownList_LineName()
        {
            DropDownList_Line.DataTextField = "LineName";//default show Text
            DropDownList_Line.DataValueField = "LineID";
            DropDownList_Line.DataSource = SFun.GetLineList();
            DropDownList_Line.DataBind();
            DropDownList_Line.Items.Insert(0, new ListItem("--Select--", "0"));
        }
        private void GetDropDownList_ErrorType()
        {
            DropDownList_ErrorType.DataSource = SFun.GetErrorType(null,null);
            DropDownList_ErrorType.DataBind();
            DropDownList_ErrorType.Items.Insert(0, "--Select--");
        }
        private void NoData()
        {
            ColumnsData = "<th class='center'>沒有資料載入</th>";
            RowsData = "<tr class='even gradeX'> <td class='center'> no data </ td ></ tr >";
        }
        //======================Event=============================
        protected void Page_Load(object sender, EventArgs e)
        {
            string CompLoacation = "", URL_NAME = "", acc = "";
            //HttpCookie CompInfo = Request.Cookies["VisCompany"];
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
               
                URL_NAME = "Asm_ErrorSearch";
                color = HtmlUtil.change_color(acc);
                if (Session["Location"] != null)
                    CompLoacation = Session["Location"].ToString();
                if (myclass.user_view_check(URL_NAME, acc) == true)
                {
                    if (!IsPostBack)
                    {

                        if (CompLoacation.ToUpper().Contains("HOR"))
                            SFun.GetConnByDekVisTmp = SFun.GetConnByDekdekVisAssmHor;
                        GotoCenn();
                    }
                }
                else
                {
                    Response.Write("<script>alert('您無法瀏覽此頁面 請向該單位主管主管申請權限!');location.href='../index.aspx';</script>");
                    NoData();
                }

              
            }
            else
            {
                Response.Redirect(myclass.logout_url);
            }
        }

        protected void Unnamed_ServerClick(object sender, EventArgs e)
        {
            string CompLoacation = "";

            if (Mant_Search.Value == "" && DropDownList_Line.SelectedItem.Text == "--Select--" && DropDownList_ErrorType.SelectedItem.Text == "--Select--")
            {
                Response.Write("<script>alert('請至少輸入一項資訊!');location.href='Asm_ErrorSearch.aspx';</script>");
                NoData();
            }
            else
            {
                // HttpCookie CompInfo = Request.Cookies["VisCompany"];
                if (Session["Location"] != null)
                    CompLoacation = Session["Location"].ToString();
                if (CompLoacation.ToUpper().Contains("HOR"))
                    SFun.GetConnByDekVisTmp = SFun.GetConnByDekdekVisAssmHor;
                GetTableData(Mant_Search.Value, DropDownList_Line.SelectedItem, DropDownList_ErrorType.SelectedItem.Text);
            }
        }

        protected void bt_Ver_ServerClick(object sender, EventArgs e)
        {
            Control bt = (Control)sender;
            //HttpCookie cook = new HttpCookie("VisCompany");
            HttpCookie userInfo = Request.Cookies["userInfo"];
            string acc = DataTableUtils.toString(userInfo["user_ACC"]);//user_RW
            if (bt.ID.Contains("Hor") )//&& (acc == "detatina" || acc == "lin@deta.com.tw" || acc == "dora" || acc == "eerp" || acc == "visrd"))
            {
                SFun.GetConnByDekVisTmp = SFun.GetConnByDekdekVisAssmHor;
                //cook["Location"] = "Hor";
                Session["Location"] = "Hor";
            }
            else
            {
                SFun.GetConnByDekVisTmp = SFun.GetConnByDekdekVisAssm;
                Session["Location"] = "Ver";
                //cook["Location"] = "Ver";
            }
            //Response.Cookies.Add(cook);
            GotoCenn();

        }
    }
}