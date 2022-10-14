using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using dek_erpvis_v2.cls;
using System.Data;
using Support;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.Configuration;

namespace dek_erpvis_v2.pages.dp_PM
{

    public partial class Asm_ErrorDetail : System.Web.UI.Page
    {
        public string CCS = "";
        string startdate = "";
        string enddate = "";
        string errortype = "";
        string db = "";
        public string UrlLink = "";
        public string replace_name = "";
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
        public string[] RowsDataArray = new string[5] { "", "", "", "", "" };
        public string[] ErrorTitleArray = new string[5] { "", "", "", "", "" };
        public string[] ErrorTitleDisplayType = new string[5] { "nodata", "nodata", "nodata", "nodata", "nodata", };
        public string[] ErrorTitleDisplayDep = new string[5] { "nodata", "nodata", "nodata", "nodata", "nodata", };
        public string[] ErrorTitleDisplayStatus = new string[5] { "nodata", "nodata", "nodata", "nodata", "nodata", };
        public string[] ErrorTitleDisplayStatusColor = new string[5] { "nodata", "nodata", "nodata", "nodata", "nodata", };
        string ErrorID = "";
        string MantID = "";
        string acc = "";
        string RW = "";
        string dpm = "";
        public string go_back = "";
        clsDB_Server clsDB_sw = new clsDB_Server("");
        ShareFunction SFun = new ShareFunction();
        myclass myclass = new myclass();
        public string word = "";
        /*----------------20200424留言板功能---------*/
        string Return_Image = "";
        /*----------------20200424留言板功能---------*/
        private void GotoCenn()
        {
            if (clsDB_sw.IsConnected == true)
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
            else
                Response.Redirect("Asm_ErrorDetail.aspx?key=" + Request.QueryString["key"]);
        }
        private void GetTableData(string ErrorID, string LineNum)
        {

            //public string[] ErrorTitleDisplayDep = new string[5];
            //public string[] ErrorTitleDisplayStatus = new string[5];
            //public string[] ErrorTitleDisplayStatusColor = new string[5];
            string[] result;
            string[] ErrorTitleShow = new string[5] { "", "", "", "", "" };

            ColumnsData = SFun.GetColumnName("Asm_ErrorDetail");
            result = SFun.GetMantRowsData(acc, ErrorID, LineNum, ref ErrorTitleShow, ref ErrorTitleDisplayDep, ref ErrorTitleDisplayStatus, "", MantID, startdate, enddate, errortype, db);
            if (ShareFunction.Last_Place(acc).ToLower() == "ver")
                CCS = ShareFunction.Get_CCS(myclass.GetConnByDekdekVisAssm, ErrorID);
            else
                CCS = ShareFunction.Get_CCS(myclass.GetConnByDekdekVisAssmHor, ErrorID);

            if (result[0] != "")
            {
                RowsDataArray = result;
                ErrorTitleArray = ErrorTitleShow;
            }
            for (int i = 0; i < ErrorTitleArray.Length; i++)
            {
                if (ErrorTitleArray[i] != "")
                {
                    ErrorTitleDisplayType[i] = "style = 'display:normal'";
                    if (ErrorTitleDisplayStatus[i].ToUpper() == "TRUE")
                    {
                        ErrorTitleDisplayStatus[i] = "結案";
                        ErrorTitleDisplayStatusColor[i] = "style = 'color:green'";
                    }
                    else
                    {
                        ErrorTitleDisplayStatus[i] = "處理中";
                        ErrorTitleDisplayStatusColor[i] = "style = 'color:red'";
                    }
                }
                else
                    ErrorTitleDisplayType[i] = "style = 'display:none'";
            }
        }
        private void GetDateToshow()
        {
            string[] date = new string[3];
            date = DateTime.Now.ToString("yyyy-MM-dd").ToString().Split('-');
            NowYear = date[0];
            NowMonth = date[1];
            NowDay = date[2];
        }
        private void GetDropDownList_Error()
        {
            if (HtmlUtil.Search_acc_Column(acc, "Last_Model").ToLower() == "ver")
                GlobalVar.UseDB_setConnString(SFun.GetConnByDekdekVisAssm);
            else
                GlobalVar.UseDB_setConnString(SFun.GetConnByDekdekVisAssmHor);
            //DropDownList_ErrorType.DataSource = SFun.CaseErrorType(SFun.GetConnByDekVisTmp);
            //DropDownList_ErrorType.DataBind();
            DropDownList_ErrorChild.Items.Clear();
            DropDownList_Errorfa.Items.Clear();
            DropDownList_Errorfa.DataSource = SFun.CaseErrorType(SFun.GetConnByDekVisTmp);
            DropDownList_Errorfa.DataBind();
            DropDownList_ErrorChild.DataSource = SFun.CaseErrorType(SFun.GetConnByDekVisTmp);
            DropDownList_ErrorChild.DataBind();

        }

        private void GetDropDownList_Status()
        {
            DropDownList_Status.DataSource = SFun.GetErrorProcessStatus();
            DropDownList_Status.DataBind();
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
            GetDateToshow();
            List<string> parameterList = new List<string>();
            if (Request.QueryString["ErrorID"] != null)
            {
                parameterList = new List<string>(HtmlUtil.Return_str(Request.QueryString["ErrorID"], "0"));
                parameterList.RemoveAt(3);
                parameterList.RemoveAt(1);
            }
            else if (Request.QueryString["key"] != null)
            {
                parameterList = new List<string>(HtmlUtil.Return_str(Request.QueryString["key"]));
                parameterList.RemoveAt(4);
                parameterList.RemoveAt(2);
                parameterList.RemoveAt(0);
            }

            if (parameterList.Count != 0)
            {
                if (parameterList.Count > 1)
                {
                    LineNum = parameterList[1];
                    LineName = parameterList[2];
                    SFun.GetConnByDekVisTmp = SFun.CheckConnectSting(parameterList);
                }
                Key = parameterList[0];
                GetDropDownList_Error();

                GetDropDownList_Status();
                GetTableData(Key, LineNum);
            }
            else
                Response.Redirect("Asm_LineTotalView.aspx");
        }
        //---------------------------Event----------------------------------------
        protected void Page_Load(object sender, EventArgs e)
        {
            string CompLoacation = "";
            //HttpCookie CompInfo = Request.Cookies["VisCompany"];
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                color = HtmlUtil.change_color(acc);
                CompLoacation = ShareFunction.Last_Place(acc);
                while (CompLoacation == "")
                {
                    CompLoacation = ShareFunction.Last_Place(acc);
                    if (CompLoacation != "")
                        break;
                }
                dpm = acc_dpm(acc);

                string save = "";
                /*此部分防呆用，如果沒有點選臥式就進入的話，新增的資料會跑到立式去，故加上此防呆*/
                if (Request.QueryString["key"] != null)
                {
                    Dictionary<string, string> Url_List = HtmlUtil.Return_dictionary(Request.QueryString["key"]);

                    if (Url_List.Count < 3)
                        Response.Redirect("Asm_LineTotalView.aspx");
                    string catchvalue = HtmlUtil.Search_Dictionary(Url_List, "ErrorLineName");
                    if (HtmlUtil.Search_Dictionary(Url_List, "history") == "1")
                        PlaceHolder_hidden.Visible = false;
                    MantID = HtmlUtil.Search_Dictionary(Url_List, "MantID");
                    startdate = HtmlUtil.Search_Dictionary(Url_List, "Date_str");
                    enddate = HtmlUtil.Search_Dictionary(Url_List, "Date_end");
                    errortype = HtmlUtil.Search_Dictionary(Url_List, "ErrorType");
                    db = HtmlUtil.Search_Dictionary(Url_List, "db");
                    SFun.GetConnByDekVisTmp = SFun.GetConnByDekdekVisAssmHor;
                    DataTableUtils.Conn_String = SFun.GetConnByDekdekVisAssmHor;//要加上這一行確定會連結到
                    DataTable ds = DataTableUtils.GetDataTable($"select * from 工作站型態資料表 where 工作站名稱 = '{catchvalue}'");
                    if (HtmlUtil.Check_DataTable(ds))
                    {
                        SFun.GetConnByDekVisTmp = SFun.GetConnByDekdekVisAssmHor;
                        save = "Hor";
                    }
                    else
                    {
                        SFun.GetConnByDekVisTmp = SFun.GetConnByDekdekVisAssm;
                        save = "Ver";
                    }

                    /*20200221修改這裡*/
                    ShareFunction.Last_Place(acc, save);
                    /*20200221修改這裡*/
                    UrlLink = "LineNum=" + HtmlUtil.Search_Dictionary(Url_List, "ErrorLineNum") + ",ReqType=Line";
                    UrlLink = WebUtils.UrlStringEncode(UrlLink);
                    if (MantID != "")
                    {
                        go_back = $"ErrorID={HtmlUtil.Search_Dictionary(Url_List, "ErrorID")},ErrorLineNum={HtmlUtil.Search_Dictionary(Url_List, "ErrorLineNum")},ErrorLineName={HtmlUtil.Search_Dictionary(Url_List, "ErrorLineName")}";
                        go_back = $"<li><u><a href=\"Asm_ErrorDetail.aspx?key={WebUtils.UrlStringEncode(go_back)}\">上一頁</a></u></li>";
                    }

                }
                else if (Request.QueryString["ErrorID"] != null)
                {
                    //-------------------------------------
                    string[] str = HtmlUtil.Return_str(Request.QueryString["ErrorID"], "0");
                    string catchvalue = str[4];
                    SFun.GetConnByDekVisTmp = SFun.GetConnByDekdekVisAssmHor;
                    DataTableUtils.Conn_String = SFun.GetConnByDekdekVisAssmHor;//要加上這一行確定會連結到
                    DataTable ds = DataTableUtils.GetDataTable("select * from 工作站型態資料表 where 工作站名稱 = '" + catchvalue + "'");
                    if (ds.Rows.Count > 0)
                    {
                        SFun.GetConnByDekVisTmp = SFun.GetConnByDekdekVisAssmHor;
                        save = "Hor";
                    }
                    else
                    {
                        SFun.GetConnByDekVisTmp = SFun.GetConnByDekdekVisAssm;
                        save = "Ver";
                    }

                    /*20200221修改這裡*/
                    ShareFunction.Last_Place(acc, save);
                    /*20200221修改這裡*/
                    UrlLink = "LineNum=" + str[2] + ",ReqType=Line";
                    UrlLink = WebUtils.UrlStringEncode(UrlLink);
                }
                else
                    Response.Redirect("Asm_LineTotalView.aspx");

                /*-----------------------------------------------------------------------*/

                if (!IsPostBack)
                {
                    if (CompLoacation.ToUpper().Contains("HOR"))
                        SFun.GetConnByDekVisTmp = SFun.GetConnByDekdekVisAssmHor;
                    GotoCenn();
                }
            }
            else
                Response.Redirect(myclass.logout_url);
        }
        protected void Unnamed_ServerClick(object sender, EventArgs e)
        {
            string CompLoacation = "";
            //HttpCookie CompInfo = Request.Cookies["VisCompany"];
            HttpCookie userInfo = Request.Cookies["userInfo"];
            string msg = "";
            acc = DataTableUtils.toString(userInfo["user_ACC"]);
            RW = DataTableUtils.toString(userInfo["USER_RW"]);
            if (acc != null)//acc!=null && RW=1
            {
                List<string> parameterList = new List<string>();
                if (Request.QueryString["ErrorID"] != null)
                {
                    parameterList = new List<string>(HtmlUtil.Return_str(Request.QueryString["ErrorID"], "0"));
                    parameterList.RemoveAt(3);
                    parameterList.RemoveAt(1);
                }
                else if (Request.QueryString["key"] != null)
                {
                    parameterList = new List<string>(HtmlUtil.Return_str(Request.QueryString["key"]));
                    parameterList.RemoveAt(4);
                    parameterList.RemoveAt(2);
                    parameterList.RemoveAt(0);
                }


                Key = parameterList[0];
                if (parameterList.Count > 1)
                    LineNum = parameterList[1];

                CompLoacation = ShareFunction.Last_Place(acc);
                if (CompLoacation.ToUpper().Contains("HOR"))
                    SFun.GetConnByDekVisTmp = SFun.GetConnByDekdekVisAssmHor;



                //建立問題上傳的檔案
                string Image_Save = "";
                Image_Save = HtmlUtil.FileUpload_Name(FileUpload_image, "Backup_Error_Image");


                if (DropDownList_Errorfa.SelectedItem.Text != "")
                {
                    if (!SFun.SetMantDataToDataTable(acc, Key, DropDownList_Errorfa.SelectedItem.Text, MantStr.Text, dpm, "處理", parameterList, RadioButtonList_Post.SelectedValue, Image_Save))
                        Response.Write("<script language='javascript'>alert('伺服器回應 : 新增異常，請洽管理者協助。');</script>");
                    else
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script>alert('伺服器回應 : 新增成功');location.href='Asm_ErrorDetail.aspx" + Request.Url.Query + "';</script>");
                }
                else
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script>alert('伺服器回應 : 請選擇異常類型');location.href='Asm_ErrorDetail.aspx" + Request.Url.Query + "';</script>");
            }
            else
                Response.Write("<script>alert('您無法瀏覽此頁面 請向該單位主管主管申請權限!');location.href='../index.aspx';</script>");
        }
        protected void bt_del_ServerClick(object sender, EventArgs e)
        {
            string Message = "";
            string CompLoacation = "";
            //HttpCookie CompInfo = Request.Cookies["VisCompany"];
            HttpCookie userInfo = Request.Cookies["userInfo"];
            List<string> parameterList = new List<string>();
            if (Request.QueryString["ErrorID"] != null)
            {
                parameterList = new List<string>(HtmlUtil.Return_str(Request.QueryString["ErrorID"], "0"));
                parameterList.RemoveAt(3);
                parameterList.RemoveAt(1);
            }
            else if (Request.QueryString["key"] != null)
            {
                parameterList = new List<string>(HtmlUtil.Return_str(Request.QueryString["key"]));
                parameterList.RemoveAt(4);
                parameterList.RemoveAt(2);
                parameterList.RemoveAt(0);
            }


            acc = DataTableUtils.toString(userInfo["user_ACC"]);
            Key = parameterList[0];



            CompLoacation = ShareFunction.Last_Place(acc);
            //if (CompInfo != null)
            //    CompLoacation = DataTableUtils.toString(CompInfo["Location"]);
            if (CompLoacation.ToUpper().Contains("HOR"))
                SFun.GetConnByDekVisTmp = SFun.GetConnByDekdekVisAssmHor;
            var num = TextBox_num.Text;
            if (num != null && num != "")
            {
                Message = SFun.ErrorDetailDeleteProcess(num, acc);
                // Response.Write("<script>alert('"+ Message + "!')");
                string url = "ErrorID=" + parameterList[0] + ",ErrorLineNum=" + parameterList[1] + ",ErrorLineName=" + parameterList[2];
                Response.Write("<script language='javascript'>alert('伺服器回應 : " + Message + "');location.href='" + "../dp_PM/Asm_ErrorDetail.aspx?key=" + WebUtils.UrlStringEncode(url) + "';</script>");
            }
            else
            {
                Message = "請先選擇刪除項目。";
                string url = "ErrorID=" + parameterList[0] + ",ErrorLineNum=" + parameterList[1] + ",ErrorLineName=" + parameterList[2];
                Response.Write("<script language='javascript'>alert('伺服器回應 : " + Message + "');location.href='" + "../dp_PM/Asm_ErrorDetail.aspx?key=" + WebUtils.UrlStringEncode(url) + "';</script>");
            }
        }
        /*----------------20200424留言板功能---------*/
        protected void AddContent_Click(object sender, EventArgs e)
        {
            //修改+回覆的檔案
            for (int i = 0; i < CheckBoxList_UpdateError.Items.Count; i++)
            {
                if (CheckBoxList_UpdateError.Items[i].Selected)
                    Return_Image += CheckBoxList_UpdateError.Items[i].Value + "\n";
            }

            Return_Image += HtmlUtil.FileUpload_Name(FileUpload_Content, "Backup_Error_Image");

            string close = "";
            //結案的檔案
            for (int i = 0; i < CheckBoxList_Close.Items.Count; i++)
            {
                if (CheckBoxList_Close.Items[i].Selected)
                    close += CheckBoxList_Close.Items[i].Value + "\n";

            }
            close += HtmlUtil.FileUpload_Name(FileUpload_Close, "Backup_File");

            string[] status = TextBox_textTemp.Text.Split('_');
            int ID = 0;

            if (status.Length == 2)
                ID = DataTableUtils.toInt(status[1]);


            // ------------------20200424------------------------
            if (DropDownList_Status.SelectedItem.Text == "處理")
            {
                if (status.Length == 2)
                {
                    if (SFun.Add_Content(RadioButtonList_Upost.SelectedValue, ID.ToString(), DropDownList_ErrorChild.SelectedItem.Text, TextBox_ErrorID.Text, acc, dpm, TextContent.Text, Return_Image, ID) == true)
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script>alert('修改成功');location.href='Asm_ErrorDetail.aspx" + Request.Url.Query + "';</script>");
                }

                else
                {
                    if (SFun.Add_Content(RadioButtonList_Upost.SelectedValue, TextBox_textTemp.Text, DropDownList_ErrorChild.SelectedItem.Text, TextBox_ErrorID.Text, acc, dpm, TextContent.Text, Return_Image) == true)
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script>alert('新增成功');location.href='Asm_ErrorDetail.aspx" + Request.Url.Query + "';</script>");
                }
            }
            else if (DropDownList_Status.SelectedItem.Text == "結案")
            {
                if (status.Length == 2)
                {
                    if (SFun.Add_Content(RadioButtonList_Upost.SelectedValue, ID.ToString(), DropDownList_ErrorChild.SelectedItem.Text, TextBox_ErrorID.Text, acc, dpm, TextContent.Text, Return_Image, ID, DropDownList_ErrorChild.SelectedItem.Text, TextBox_Report.Text, close) == true)
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script>alert('修改成功');location.href='Asm_ErrorDetail.aspx" + Request.Url.Query + "';</script>");
                }

                else
                {
                    if (SFun.Add_Content(RadioButtonList_Upost.SelectedValue, TextBox_textTemp.Text, DropDownList_ErrorChild.SelectedItem.Text, TextBox_ErrorID.Text, acc, dpm, TextContent.Text, Return_Image, 0, DropDownList_ErrorChild.SelectedItem.Text, TextBox_Report.Text, close) == true)
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script>alert('新增成功');location.href='Asm_ErrorDetail.aspx" + Request.Url.Query + "';</script>");
                }

            }

        }

        //查詢所屬的部門
        private string acc_dpm(string acc)
        {
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisErp);
            string sqlcmd = "select DPM_NAME2 from users left join DEPARTMENT on USERS.USER_DPM = DEPARTMENT.DPM_NAME where USER_ACC = '" + acc + "'";
            DataRow row = DataTableUtils.DataTable_GetDataRow(sqlcmd);
            if (row != null)
                return DataTableUtils.toString(row["DPM_NAME2"]);
            else
                return "";
        }
        protected void Button_Update_Click(object sender, EventArgs e)
        {
            CheckBoxList_Close.Items.Clear();
            CheckBoxList_UpdateError.Items.Clear();
            DropDownList_Status.Enabled = true;
            GlobalVar.UseDB_setConnString(SFun.GetConnByDekVisTmp);
            string sqlcmd = "select * from 工作站異常維護資料表 where 異常維護編號 = '" + TextBox_textTemp.Text.Split('_')[1] + "'";
            DataRow row = DataTableUtils.DataTable_GetDataRow(sqlcmd);
            if (row != null)
            {
                // PlaceHolder_image.Visible = false;
                
                DropDownList_Status.SelectedIndex = -1;
                try
                {
                    DropDownList_ErrorChild.SelectedValue = DataTableUtils.toString(row["異常原因類型"]);
                }
                catch
                {
                    DropDownList_ErrorChild.SelectedValue = "其他";
                }
                //他是子項目
                if (DataTableUtils.toString(row["父編號"]) != "")
                {
                    TextBox_Report.Text = DataTableUtils.toString(row["結案內容"]);
                    word = DataTableUtils.toString(row["維護內容"]);
                    if (DataTableUtils.toString(row["結案判定類型"]) != "")
                    {
                        DropDownList_Status.SelectedValue = "處理";
                        DropDownList_Status.SelectedValue = "結案";
                        //   DropDownList_ErrorType.SelectedValue = DataTableUtils.toString(row["結案判定類型"]);
                        //PlaceHolder_Close.Visible = true;
                        TextBox_Report.Enabled = true;
                    }
                    else
                    {
                        //PlaceHolder_Close.Visible = false;
                        DropDownList_Status.SelectedValue = "處理";
                    }

                }
                //他是父項目
                else
                {
                    DropDownList_Status.Enabled = false;
                    word = DataTableUtils.toString(row["維護內容"]);
                    sqlcmd = "select * from 工作站異常維護資料表 where 異常維護編號 = '" + TextBox_textTemp.Text.Split('_')[1] + "' order by 異常維護編號 desc";
                    DataRow rew = DataTableUtils.DataTable_GetDataRow(sqlcmd);
                    if (rew != null)
                    {
                        try
                        {
                            DropDownList_ErrorChild.SelectedValue = DataTableUtils.toString(rew["異常原因類型"]);
                        }
                        catch
                        {

                        }
                        //Text_Content.Value = DataTableUtils.toString(rew["維護內容"]);
                        if (DataTableUtils.toString(rew["結案判定類型"]) != "")
                        {
                            DropDownList_Status.SelectedValue = "結案";
                            TextBox_Report.Text = DataTableUtils.toString(rew["結案內容"]);
                            TextBox_Report.Enabled = false;
                            //  DropDownList_ErrorType.SelectedValue = DataTableUtils.toString(rew["結案判定類型"]);
                          //  PlaceHolder_Close.Visible = true;
                        }
                        else
                        {
                           // PlaceHolder_Close.Visible = false;
                            DropDownList_Status.SelectedValue = "處理";
                        }
                    }

                }
                TextContent.Text = word;

                Print_Checkbox(CheckBoxList_UpdateError, DataTableUtils.toString(row["圖片檔名"]));
                Print_Checkbox(CheckBoxList_Close, DataTableUtils.toString(row["結案附檔"]));

                button_select.Text = "修改";
            }
        }
        protected void Button_Add_Click(object sender, EventArgs e)
        {
            CheckBoxList_Close.Items.Clear();
            CheckBoxList_UpdateError.Items.Clear();
            //20221014 先按編輯再會回復會被關閉,改進入回復時打開
            DropDownList_Status.Enabled = true;
            TextContent.Text = "";
            //要先找子項目，再找父項目
            if (HtmlUtil.Search_acc_Column(acc, "Last_Model").ToLower() == "ver")
                GlobalVar.UseDB_setConnString(SFun.GetConnByDekdekVisAssm);
            else
                GlobalVar.UseDB_setConnString(SFun.GetConnByDekdekVisAssmHor);

            string sqlcmd = $"select top(1)   * from 工作站異常維護資料表 where 父編號={TextBox_textTemp.Text} order by 異常維護編號 desc ";
            DataTable ds = DataTableUtils.GetDataTable(sqlcmd);
          
            //他有子項目，因此選擇子項目最新的異常原因類型
            if (HtmlUtil.Check_DataTable(ds))
                DropDownList_ErrorChild.SelectedValue = DataTableUtils.toString(ds.Rows[0]["異常原因類型"]);
            //他是父項目
            else
            {
                sqlcmd = $"select top(1) * from 工作站異常維護資料表 where 異常維護編號={TextBox_textTemp.Text} order by 異常維護編號 desc";
                ds = DataTableUtils.GetDataTable(sqlcmd);
                
                if (HtmlUtil.Check_DataTable(ds))
                    DropDownList_ErrorChild.SelectedValue = DataTableUtils.toString(ds.Rows[0]["異常原因類型"]);
            }

            DropDownList_Status.SelectedValue = "結案";
            DropDownList_Status.SelectedValue = "處理";

            if (WebConfigurationManager.AppSettings["Lock_Close"] == "1")
            {
                if (HtmlUtil.Search_acc_Column(acc, "USER_NAME") != TextBox_acc.Text && HtmlUtil.Search_acc_Column(acc, "Can_Close") != "Y")
                    DropDownList_Status.Enabled = false;
            }

            PlaceHolder_image.Visible = true;
         //   PlaceHolder_Close.Visible = false;
            TextBox_Report.Text = "";
            button_select.Text = "新增";
        }

        private void Print_Checkbox(CheckBoxList cbx, string url)
        {
            string[] file_url = url.Split('\n');
            //測試編輯檔案
            ListItem item = new ListItem();
            for (int i = 0; i < file_url.Length - 1; i++)
            {
                if (file_url[i].Split('.')[1].ToLower() == "mp4")
                    item = new ListItem($"<video src=\"{file_url[i]}\" style=\"width:200px;height:200px;background:white\" controls=\"\" href=\"javascript: void()\" />", file_url[i]);
                else
                    item = new ListItem($"<img src=\"{file_url[i]}\" style=\"width:200px;height:200px;background:white\" />", file_url[i]);
                cbx.Items.Add(item);
            }

            for (int i = 0; i < cbx.Items.Count; i++)
                cbx.Items[i].Selected = true;

        }
    }
}