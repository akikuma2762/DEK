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
    public partial class Asm_LineOverView : System.Web.UI.Page
    {
        public string X_Data = "";
        public string _Data = "";
        public string Select_Data = "";
        public string power = "";
        public string color = "";
        public string TagetPiece = "0";
        public string TagetPerson = "0";
        public string FinishPiece = "0";
        public string OnLinePiece = "0";

        public string ErrorPiece = "0";
        public string td_TagetPiece = "0";
        public string td_TagetPerson = "0";
        public string td_FinishPiece = "0";
        public string td_OnLinePiece = "0";
        public string td_ErrorPiece = "0";
        public string LineName = "";
        public string thCol = "";
        public string ColumnsData = "<th class='center'>沒有資料載入</th>";
        public string RowsData = "<tr class='even gradeX'> <td class='center'> no data </ td ></ tr >";
        public string NowYear = "";
        public string NowMonth = "";
        public string NowDay = "";
        public string StandaTime = "";
        public string StartTime = "";
        public string preFinishTime = "";
        public string Story_Columns = "";
        public string Story_content = "";
        string acc = "";
        public int behind = 0;
        public int alarm_total = 0;
        clsDB_Server clsDB_sw = new clsDB_Server("");
        ShareFunction SFun = new ShareFunction();
        myclass myclass = new myclass();
        public string PieceUnit = ShareMemory.PieceUnit;

        private void GotoCenn()
        {
            if (Request.QueryString["key"] != null)
            {
                clsDB_sw.dbOpen(SFun.GetConnByDekVisTmp);
                if (clsDB_sw.IsConnected == true)
                {
                    DataTableUtils.Conn_String = SFun.GetConnByDekVisTmp;
                    if (clsDB_sw.IsConnected == true)
                    {
                        try
                        {
                            AddProgressList();
                            load_page_data();
                            check_power(acc);
                        }
                        catch
                        {
                            GotoCenn();
                        }


                    }
                    else
                        Response.Write("<script language='javascript'>alert('伺服器回應 : 無法載入資料，" + clsDB_sw.ErrorMessage + " 請聯絡德科人員或檢查您的網路連線。');</script>");
                }
                else
                    Response.Redirect("Asm_LineOverView.aspx?" + Request.QueryString["key"]);

            }
            else
                Response.Redirect("Asm_LineTotalView.aspx");


        }
        private void load_page_data()
        {
            if (GlobalVar.Conn_status == true) //資料庫連線成功要做的事()
                LoadData();
            else   //資料庫連線失敗要做的事()
                Response.Write("<script language='javascript'>alert('伺服器回應 : 無法載入資料，" + " 請聯絡德科人員或檢查您的網路連線。');</script>");
        }
        private void GetDateToshow()
        {
            string[] date = new string[3];
            date = DateTime.Now.ToString("yyyy-MM-dd").ToString().Split('-');
            NowYear = date[0];
            NowMonth = date[1];
            NowDay = date[2];
        }
        private void UpdataHtmlVlaue(int LineNum)
        {
            string[] result = new string[2];
            result = SFun.GetTagetPiece(LineNum);
            if (result[0].ToString().ToUpper() != "NULL")
                TagetPiece = result[0];
            LineName = result[1];
            TagetPerson = result[2];
            //TagetPiece = fun.GetTagetPiece(DataTableUtils.toInt(Request.Params["Name"]));
        }
        private void GetTableData(int LineNum, string reqType)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            acc = DataTableUtils.toString(userInfo["user_ACC"]);
            string[] result;
            string[] reqStr = new string[2];
            //
            if (reqType != "")
                reqStr = reqType.Split('=');

            if (acc != null)
            {
                ColumnsData = SFun.GetColumnName("Asm_LineOverView");
                string judge = "";
                if (ShareFunction.Last_Place(acc) == "Hor")
                    judge = ShareFunction.Last_Place(acc);
                result = SFun.GetOverViewData(LineNum, LineName, acc, reqStr[1], ref behind, ref alarm_total, judge);
                OnLinePiece = result[0];
                FinishPiece = result[1];
                ErrorPiece = result[2];
                td_OnLinePiece = result[3];
                td_FinishPiece = result[4];
                td_ErrorPiece = result[5];
                RowsData = result[6];

                //20221005 第二重檢測資料是否正常
                _Data = result[7];
                Select_Data = result[8];
                for (int k = 0; k < result.Length; k++)
                {
                    if (k == 6 && result[6] != "no data")
                    {
                        X_Data += $"id{k},HTML table有資料 ";
                    }
                    else
                    {
                        X_Data += $"id{k} data:{result[k]}; ";
                    }
                }

            }
            else
                Response.Redirect(myclass.logout_url);
        }
        private void LoadData()
        {
            string[] QueryStr;
            GetDateToshow();
            /*20200221修改這裡*/
            if (Request.QueryString["key"] != null)
            {
                QueryStr = HtmlUtil.Return_str(Request.QueryString["key"]);

                if (QueryStr.Length == 4)
                {
                    UpdataHtmlVlaue(DataTableUtils.toInt(QueryStr[1]));
                    GetTableData(DataTableUtils.toInt(QueryStr[1]), QueryStr[2] + "=" + QueryStr[3]);
                }

            }

            /*20200221修改這裡*/
        }
        private void GetData(string str)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            acc = DataTableUtils.toString(userInfo["user_ACC"]);
            string[] result;

            if (acc != null)
            {
                ColumnsData = SFun.GetColumnName("Asm_NumView");
                /*20200221修改這裡*/
                result = SFun.GetData(str, acc);

                /*20200221修改這裡*/
                OnLinePiece = result[0];
                FinishPiece = result[1];
                ErrorPiece = result[2];
                td_OnLinePiece = result[3];
                td_FinishPiece = result[4];
                td_ErrorPiece = result[5];
                RowsData = result[6];
            }
            else
                Response.Redirect(myclass.logout_url);
        }
        private void AddProgressList()
        {
            for (int i = 0; i < 100; i = i + 10)
                DropDownList_progress.Items.Add(i.ToString() + "%");
            DropDownList_progress.Items.Add("99" + "%");
            DropDownList_progress.Items.Add("100" + "%");
        }
        private void check_power(string acc)
        {
            bool ok = SFun.GetRW(acc);
            string dep = HtmlUtil.Search_acc_Column(acc, "USER_DPM");

            if (ok == true || dep == "PMD")
            {
                RadioButtonList_select_type.Enabled = true;
                DropDownList_progress.Enabled = true;
                TextBox_Report.Enabled = true;
                power = "PMD";
            }
            else
            {
                RadioButtonList_select_type.Enabled = false;
                DropDownList_progress.Enabled = false;
                TextBox_Report.Enabled = false;
                power = "";
            }
        }
        //--------------------Event-------------------------------------------
        protected void Page_Load(object sender, EventArgs e)
        {
            string CompLoacation = "";
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                color = HtmlUtil.change_color(acc);
                CompLoacation = ShareFunction.Last_Place(acc);
                ShareFunction.Last_Place(acc, CompLoacation);
                if (!IsPostBack)
                {
                    if (CompLoacation.ToUpper().Contains("HOR"))
                        SFun.GetConnByDekVisTmp = SFun.GetConnByDekdekVisAssmHor;
                    GotoCenn();
                }
                //效能測試
            }
            else
                Response.Redirect(myclass.logout_url);
        }
        protected void button_select_Click(object sender, EventArgs e)
        {
            string msg = "";
            string condition = "";
            string CORD_NO = "";
            string CORD_SN = "";
            string Key = TextBox_show.Text;

            string CompLoacation = "";
            string[] QueryStr;
            QueryStr = HtmlUtil.Return_str(Request.QueryString["key"]);
            if (Key != "")
            {

                CompLoacation = ShareFunction.Last_Place(acc);
                SFun.GetConnByDekVisTmp = SFun.GetConnByDekdekVisAssm;
                if (CompLoacation.ToUpper().Contains("HOR"))
                    SFun.GetConnByDekVisTmp = SFun.GetConnByDekdekVisAssmHor;

                if (QueryStr != null)
                    condition = "排程編號=" + "'" + Key + "'" + " AND " + "工作站編號=" + "'" + QueryStr[1] + "'";
                else
                    condition = "排程編號=" + "'" + Key + "'";

                DataTableUtils.Conn_String = SFun.GetConnByDekVisTmp;
                DataRow dr = DataTableUtils.DataTable_GetDataRow(ShareMemory.SQLAsm_WorkStation_State, condition);

                DataRow dr_select = SFun.Set_NewStatus(dr, RadioButtonList_select_type.SelectedValue, DropDownList_progress.SelectedItem.Text);
                //
                if (dr_select != null)
                {

                    CORD_NO = dr_select["鍵編號"].ToString();
                    CORD_SN = dr_select["鍵序號"].ToString();
                }

                //bool UpdataOK = DataTableUtils.Update_DataRow(ShareMemory.SQLAsm_WorkStation_State, condition, dr_select);
                DataTable dt = DataTableUtils.GetDataTable(ShareMemory.SQLAsm_WorkStation_State, condition);
                bool updataok = SFun.change_status(acc,SFun.GetConnByDekVisTmp, dt, ShareMemory.SQLAsm_WorkStation_State, condition, RadioButtonList_select_type.SelectedItem.Text, TextBox_Report.Text.Replace("'", "^").Replace('"', '#').Replace(" ", "$").Replace("\r\n", "@"), DropDownList_progress.SelectedItem.Text.Trim('%'));
                SFun.Set_MachineID_Line_Updata(dr["工作站編號"].ToString());
                if (RadioButtonList_select_type.SelectedItem.Text == "完成")
                {
                    if (!Page.ClientScript.IsStartupScriptRegistered("alert"))
                    {
                        Page.ClientScript.RegisterStartupScript
                            (this.GetType(), "alert", "invokeMeMaster(" + "'" + CORD_NO + "'" + "," + "'" + CORD_SN + "'" + ");", true);
                    }
                    //updata sowan database 

                    DataTableUtils.Conn_String = myclass.GetConnByDetaSowon;
                    DataRow dr_sowan = DataTableUtils.DataTable_GetDataRow("select * from FJWSQL.DBO.A22_FAB WHERE CORD_NO=" + "'" + CORD_NO + "'" + " AND CORD_SN=" + "'" + CORD_SN + "'");
                    if (dr_sowan != null)
                    {
                        dr_sowan["OL_CLOSE"] = "已完工";
                        DataTableUtils.Conn_String = myclass.GetConnByDetaSowon;
                        bool ok = DataTableUtils.Update_DataRow("FJWSQL.DBO.A22_FAB", "CORD_NO = " + "'" + CORD_NO + "'" + " AND CORD_SN = " + "'" + CORD_SN + "'", dr_sowan);
                    }
                    DataTableUtils.Conn_String = SFun.GetConnByDekVisTmp;
                }
            }
            else
            {
                msg = "輸入資訊異常!";
                Response.Write("<script language='javascript'>alert('伺服器回應 : " + msg + "');</script>");
            }
            string[] str = HtmlUtil.Return_str(Request.QueryString["key"]);
            List<string> parameterList = SFun.AnsQueryString(Server.UrlDecode(str[1] + "," + str[2] + "=" + str[3]));
            string url = WebUtils.UrlStringEncode("LineNum=" + parameterList[0] + ",ReqType=" + parameterList[1]);
            if (parameterList.Count != 0)
                Response.Redirect("../dp_PM/Asm_LineOverView.aspx?key=" + url);

        }
        protected void Button_Jump_Click(object sender, EventArgs e)
        {
            Response.Redirect("Asm_ErrorDetail.aspx?key=" + TextBox_textTemp.Text);
        }

    }
}