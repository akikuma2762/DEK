using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using dek_erpvis_v2.cls;
using System.Data;
using Support;

namespace dek_erpvis_v2.pages.dp_PM
{
    public partial class Asm_Cahrt_Error : System.Web.UI.Page
    {
        public string y_value = "";
        public string color = "";
        public string ChartData_Count = "";
        public string ChartData_Time = "";
        public string date_str = "";// new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyyMMdd");
        public string date_end = "";//
        public string timetype = "";
        public string ColumnsData = "";
        public string RowsData = "";
        public string SelectLine = "'ALL";
        public string chartName = "";
        public string chartName_Single = "";
        List<string> list = new List<string>();
        public string Choose_Line = "所有產線";
        public string LineNum = "";
        public string AxisSet = "";
        public string LineStatus = "";
        public string TimeTypeForSubTitle = DateTime.Now.ToString("yyyy-MM-dd");
        public string LineNameForSubTitle = "所有產線";
        public string time_area_text = "";
        public string TopStrForPieceCount = "0";
        public string TopStrForPieceTimes = "0";
        public string PieceUnit = ShareMemory.PieceUnit;
        public string TimeUnit = "";
        clsDB_Server clsDB_sw = new clsDB_Server("");
        ShareFunction SFun = new ShareFunction();
        myclass myclass = new myclass();
        HttpCookie userOpRec = new HttpCookie("Rec");// Request.Cookies["userOpRec"];
        string acc = "";
        bool First_load = true;
        德大機械 德大機械 = new 德大機械();

        private void GotoCenn()
        {
            clsDB_sw.dbOpen(SFun.GetConnByDekVisTmp);
            DataTableUtils.Conn_String = SFun.GetConnByDekVisTmp;
            if (clsDB_sw.IsConnected == true)
            {
                try
                {
                    y_value = dropdownlist_y.SelectedItem.Text;
                    load_page_data();
                }
                catch
                {
                    Response.Redirect("Asm_Cahrt_Error.aspx");
                }
            }

            else
            {
                Response.Write("<script language='javascript'>alert('伺服器回應 : 無法載入資料，" + clsDB_sw.ErrorMessage + " 請聯絡德科人員或檢查您的網路連線。');</script>");
                NodataProcess();
            }
        }
        private void NodataProcess()
        {
            HtmlUtil.NoData(out ColumnsData, out RowsData);
        }
        private void load_page_data()
        {
            if (GlobalVar.Conn_status == true) //資料庫連線成功要做的事()
                LoadData();
            else   //資料庫連線失敗要做的事()
            {
                Response.Write("<script language='javascript'>alert('伺服器回應 : 無法載入資料，" + " 請聯絡德科人員或檢查您的網路連線。');</script>");
                NodataProcess();
            }

        }
        private void LoadData()
        {
            GetErrorData();
        }

        private void GetErrorData()
        {
            HttpCookie userOpRec = Request.Cookies["Rec"];
            //string[] timeSet;
            string[] timeSet = new string[2];
            string timeTypeSource = "";
            if (userOpRec["user_TimeType"] != null || userOpRec["user_TimeType"].ToString() != "")
                timeTypeSource = SFun.TrsDate(userOpRec["user_TimeType"].ToString());
            SelectLine = userOpRec["user_LineNum"].ToString();
            if (!string.IsNullOrEmpty(timeTypeSource))//timeTypeSource != null && timeTypeSource != "")
            {
                //SelectLine = "1";
                //timeSet = SFun.GetTimeType(timeTypeSource);
                //userOpRec["user_StartTime"] = timeSet[0];
                //userOpRec["user_EndTime"] = timeSet[1];
                timeSet[0] = date_str;
                timeSet[1] = date_end;
                Response.Cookies.Add(userOpRec);
                SetDropDownList();//pageload已載入,不重複設定checkbox 20220614
                if (SelectLine == "0" || string.IsNullOrEmpty(SelectLine))
                    GetDataInf(timeTypeSource, timeSet);
                else
                    GetDataInf(timeTypeSource, timeSet, SelectLine);
                //TimeTypeForSubTitle = timeSet[4];
                //if (Session["time_s"] != null && Session["time_e"] != null)
                //{
                //    TimeTypeForSubTitle = HtmlUtil.changetimeformat(Session["time_s"].ToString().Replace("010101", "")) + "~" + HtmlUtil.changetimeformat(Session["time_e"].ToString().Replace("235959", ""));
                //    Session.Remove("time_s");
                //    Session.Remove("time_e");

                //}
            }
            else
            {
                //if (SelectLine == "0" || SelectLine == null || SelectLine == "")
                //    GetDataInf(timeTypeSource, null);
                //else //First  -> show Today
                //{
                //    timeSet = SFun.GetTimeType("day");
                //    GetDataInf("day", timeSet, SelectLine);
                //    TimeTypeForSubTitle = timeSet[4];
                //    if (Session["time_s"] != null && Session["time_e"] != null)
                //    {
                //        TimeTypeForSubTitle = Session["time_s"].ToString() + "-" + Session["time_e"].ToString();
                //        Session.Remove("time_s");
                //        Session.Remove("time_e");
                //    }

                //}
            }
        }
        private void GetDataInf(string TimeType, string[] timeset, string SelectLine = "0")
        {
            string time_st;
            string time_ed;
            string LineStr = "";
            if (list.Count == 0)
            {
                list.Add("0");
                Choose_Line = "所有產線";
            }
            //if (Session["list"] != null)
            //    list = (List<string>)Session["list"];//.ToString().Split('、').ToList();
            //if (Session["line"] != null)
            //    Choose_Line = Session["line"].ToString();
            if (Session["timetype"] != null)
            {
                TimeType = Session["timetype"].ToString();
                TimeUnit = Session["timetype"].ToString();
            }
            else//沒有數值的時候，就讓他=分鐘
                TimeUnit = "(分鐘)";
            if (Session["time_s"] != null && Session["time_e"] != null)
            {
                time_st = Session["time_s"].ToString();
                time_ed = Session["time_e"].ToString();
            }
            else
            {
                time_st = timeset[0];
                time_ed = timeset[1];
            }
            string[] StrInf;
            if (timeset != null && SelectLine == "0")
                StrInf = SFun.GetErrorInf(CheckBoxList_Line, TimeUnit, TimeType, time_st, time_ed, list);
            else if (timeset != null && SelectLine != null)
                StrInf = SFun.GetErrorInf(CheckBoxList_Line, TimeUnit, TimeType, time_st, time_ed, list);
            else if (timeset == null && SelectLine == "0")
                StrInf = SFun.GetErrorInf(CheckBoxList_Line, TimeUnit, TimeType, "Today", "Today", list);
            else
                StrInf = SFun.GetErrorInf(CheckBoxList_Line, TimeUnit, TimeType, "Today", "Today", list);
            ChartData_Count = "";
            if (StrInf.Length != 0 && StrInf[4] != null)
            {

                if (dropdownlist_y.SelectedValue == "0")
                {
                    ChartData_Count = StrInf[0];
                    ChartData_Count = ChartData_Count.Replace("\n", "");
                    ChartData_Count = ChartData_Count.Replace("indexLabel", "label");

                }
                else
                {
                    ChartData_Count = StrInf[2];
                    ChartData_Count = ChartData_Count.Replace("\n", "");
                    ChartData_Count = ChartData_Count.Replace("indexLabel", "label");
                }
                y_value = dropdownlist_y.SelectedItem.Text;
                time_area_text = textbox_dt1.Text.Replace('-', '/') + "~" + textbox_dt2.Text.Replace('-', '/');
                RowsData = StrInf[3];
                TopStrForPieceCount = StrInf[4];
                TopStrForPieceTimes = StrInf[5];
            }
            else
                NodataProcess();
        }

        private void SetDropDownList()
        {

            CheckBoxList_Line.DataTextField = "LineName";//default show Text
            CheckBoxList_Line.DataValueField = "LineID";
            CheckBoxList_Line.DataSource = SFun.GetLineList();
            CheckBoxList_Line.DataBind();
            //Creat Table Column
            ColumnsData = SFun.GetCharstColumnName_Error("0", CheckBoxList_Line, "Asm_Cahrt_Error");
            if (RowsData == "<tr class='even gradeX'> <td class='center'> no data </td ></tr>")
                ColumnsData = "<th class='center'>沒有資料載入</th>";
            //add all
            CheckBoxList_Line.Items.Insert(0, new ListItem("全部", ""));
            //送出後加回已選擇選項 20220614
            if (!First_load)
            {
                for (var i = 0; i < CheckBoxList_Line.Items.Count; i++)
                {
                    if (CheckBoxList_Line.Items[i].Selected == true)
                    {
                        CheckBoxList_Line.Items[i].Selected = true;
                    }
                }
            }
            else if(First_load) {
                for (var i = 0; i < CheckBoxList_Line.Items.Count; i++)
                {
                    CheckBoxList_Line.Items[i].Selected = true;
                }
                First_load = false;
            }
        }
        public string GetPieceForSeries(int NumTh, string Type)
        {
            string[] TopPiece;
            HttpCookie userOpRec = Request.Cookies["Rec"];
            if (TopStrForPieceTimes != null && TopStrForPieceCount != null)
            {
                if (Type == "Count")
                    TopPiece = TopStrForPieceCount.Split(',');
                else
                    TopPiece = TopStrForPieceTimes.Split(',');
                //
                if (TopPiece.Length >= NumTh)
                {
                    if (TopPiece[NumTh - 1] != "" && TopPiece[NumTh - 1] != null)
                    {
                        if (Type == "Count")
                            return TopPiece[NumTh - 1];
                        else
                            return SFun.TrsTime(TopPiece[NumTh - 1], userOpRec["user_unit"].ToString());
                    }
                    else
                        return "0";
                }
                else
                    return "0";
            }
            else
                return "0";
        }
        private string timeUnitForshow(string time)
        {
            if (time.Contains('m'))
                return "(分鐘)";
            else
                return "(小時)";
        }
        //--------------------------------------Event--------------------------------------
        protected void Page_Load(object sender, EventArgs e)
        {

            string CompLoacation = "";
            string acc = "";
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);

                string URL_NAME = "Asm_Cahrt_Error";
                color = HtmlUtil.change_color(acc);
                string ped = DataTableUtils.toString(userInfo["user_PWD"]);

                CompLoacation = ShareFunction.Last_Place(acc);
                while (CompLoacation == "")
                {
                    CompLoacation = ShareFunction.Last_Place(acc);
                    if (CompLoacation != "")
                        break;
                }
                ShareFunction.Last_Place(acc, CompLoacation);
                if (myclass.user_view_check(URL_NAME, acc) == true)
                {
                    if (!IsPostBack)
                    {
                        string[] daterange = 德大機械.德大專用月份(acc).Split(',');
                        if (textbox_dt1.Text == "" && textbox_dt2.Text == "")
                        {
                            textbox_dt1.Text = HtmlUtil.changetimeformat(daterange[0], "-");
                            textbox_dt2.Text = HtmlUtil.changetimeformat(daterange[1], "-");
                        }
                        date_str = daterange[0];
                        date_end = daterange[1];
                        userOpRec["user_unit"] = ShareMemory.TimeUnit;
                        userOpRec["user_TimeType"] = "type_month";
                        userOpRec["user_LineNum"] = "0";
                        //userOpRec["user_StartTime"] = DateTime.Now.ToString("yyyyMM" + "01010101");
                        //userOpRec["user_EndTime"] = DateTime.Now.ToString("yyyyMM" + "30235959");
                        userOpRec["user_StartTime"] = daterange[0].ToString()+"010101";
                        userOpRec["user_EndTime"] = daterange[1].ToString()+"235959";
                        userOpRec.Expires = DateTime.Now.AddDays(1);
                        Response.Cookies.Add(userOpRec);
                        TimeUnit = timeUnitForshow(ShareMemory.TimeUnit);
                        //SetDropDownList();
                        if (dropdownlist_X.SelectedValue == "0")
                            SFun.GetConnByDekVisTmp = SFun.GetConnByDekdekVisAssm;
                        else
                            SFun.GetConnByDekVisTmp = SFun.GetConnByDekdekVisAssmHor;
                        GotoCenn();
                    }

                }
                else
                    Response.Write("<script>alert('您無此權限!');location.href='../index.aspx';</script>");

            }
            else
                Response.Redirect(myclass.logout_url);
        }

        protected void LinkButton_day_Click(object sender, EventArgs e)
        {
            string dt_st = "";
            string dt_ed = "";
            HttpCookie userInfo = Request.Cookies["userInfo"];
            acc = DataTableUtils.toString(userInfo["user_ACC"]);
            string[] daterange = 德大機械.德大專用月份(acc).Split(',');
            HtmlUtil.Button_Click(DataTableUtils.toString(((Control)sender).ID.Split('_')[1]), daterange, DataTableUtils.toString(textbox_dt1.Text), DataTableUtils.toString(textbox_dt2.Text), out dt_st, out dt_ed);
            textbox_dt1.Text = HtmlUtil.changetimeformat(dt_st, "-");
            textbox_dt2.Text = HtmlUtil.changetimeformat(dt_ed, "-");
        }
        protected void button_select_Click(object sender, EventArgs e)
        {
            string[] timeSet = new string[2];
            string LineSelect = "";
            //
            Choose_Line = "";
            list.Clear();
            for (int i = 0; i < CheckBoxList_Line.Items.Count; i++)
            {
                if (CheckBoxList_Line.Items[i].Selected)
                {
                    list.Add(CheckBoxList_Line.Items[i].Value);
                    Choose_Line += CheckBoxList_Line.Items[i].Text + "、";

                }
            }
            foreach (string line in list)
            {
                if (list.IndexOf(line) == list.Count - 1)
                    LineSelect += line;
                else
                    LineSelect += line + "、";
            }
            //
            HttpCookie userOpRec = Request.Cookies["Rec"];
            if (textbox_dt1.Text != "")
            {
                timeSet[0] = textbox_dt1.Text.Replace("-", String.Empty) + "010101";//add mmhhss           
                timeSet[1] = textbox_dt2.Text.Replace("-", String.Empty) + "235959";//
                Session["time_s"] = timeSet[0];
                Session["time_e"] = timeSet[1];
            }
            else
            {
                timeSet[0] = textbox_dt1.Text.Replace("-", String.Empty) + "010101";//add mmhhss           
                timeSet[1] = textbox_dt2.Text.Replace("-", String.Empty) + "235959";//
                Session["time_s"] = timeSet[0];
                Session["time_e"] = timeSet[1];
            }



            if (dropdownlist_X.SelectedValue == "0")
                SFun.GetConnByDekVisTmp = SFun.GetConnByDekdekVisAssm;
            else
                SFun.GetConnByDekVisTmp = SFun.GetConnByDekdekVisAssmHor;

            Session["list"] = list;
            Session["line"] = Choose_Line;
            First_load = false;
            GotoCenn();
        }


        protected void btn_cbx_Click(object sender, EventArgs e)
        {

            if (Session["Location"] != null)
            {
                if (Session["Location"].ToString() == "Hor")
                    SFun.GetConnByDekVisTmp = SFun.GetConnByDekdekVisAssmHor;
                else
                    SFun.GetConnByDekVisTmp = SFun.GetConnByDekdekVisAssm;
            }
            GotoCenn();

        }


        protected void dropdownlist_X_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dropdownlist_X.SelectedValue == "0")
                SFun.GetConnByDekVisTmp = SFun.GetConnByDekdekVisAssm;
            else
                SFun.GetConnByDekVisTmp = SFun.GetConnByDekdekVisAssmHor;

            SetDropDownList();
        }

    }
}