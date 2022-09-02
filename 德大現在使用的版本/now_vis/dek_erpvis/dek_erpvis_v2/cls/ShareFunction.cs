using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using Support;
using System.Text.RegularExpressions;
using System.Configuration;

namespace dek_erpvis_v2.cls
{
    public class ShareFunction
    {


        public enum departmentSelect { 個人, 列表 };
        enum ErrorProcessStatus { 處理, 結案 };
        //
        clsDB_Server clsDB_Switch = new clsDB_Server("");
        //connect String
        public string GetConnByDekdekVisAssm = myclass.GetConnByDekdekVisAssm;
        public string GetConnByDekdekVisAssmHor = myclass.GetConnByDekdekVisAssmHor;
        public string GetConnByDekVisErp = myclass.GetConnByDekVisErp;
        public string GetConnByDekVisTmp = myclass.GetConnByDekdekVisAssm;
        //取得DekErp的資料-臥式-圓盤的資料(臥式廠)
        // public static string GetConnByDekERPDataTable = clsDB_Server.GetConntionString_MySQL("192.168.3.4", "erp", "erp", "erp89886066");
        //取得DekErp的資料-臥式-圓盤的資料(立式廠)
        public static string GetConnByDekERPDataTable = myclass.GetConnByDekERPDataTable;
        static List<DateTime> holiday_list = new List<DateTime>();
        public MyWorkTime work = new MyWorkTime(IsHoliday);
        static DataTable dt_holiday = dt_work();

        private static DataTable dt_work(bool ok = false)
        {
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisErp);
            dt_holiday = ok ? DataTableUtils.DataTable_GetTable("WorkTime_Holiday", $"Line_Number = '11' and Factory='Ver'") : DataTableUtils.DataTable_GetTable("WorkTime_Holiday", $"Line_Number = '1' and Factory='Ver'");
            return dt_holiday;
        }

        string change = "";
        string _cTotalTagetPiece;
        public string cTotalTagetPiece { get { return _cTotalTagetPiece; } set { _cTotalTagetPiece = value; } }
        string _cTotalTagetPerson;
        public string cTotalTagetPerson { get { return _cTotalTagetPerson; } set { _cTotalTagetPerson = value; } }
        string _cTotalFinishPiece;
        public string cTotalFinishPiece { get { return _cTotalFinishPiece; } set { _cTotalFinishPiece = value; } }
        string _cTotalErrorPiece;
        public string cTotalErrorPiece { get { return _cTotalErrorPiece; } set { _cTotalErrorPiece = value; } }
        string _cTotalOnLinePiece;
        public string cTotalOnLinePiece { get { return _cTotalOnLinePiece; } set { _cTotalOnLinePiece = value; } }
        //==========
        string _td_cTotalFinishPiece;
        public string td_cTotalFinishPiece { get { return _td_cTotalFinishPiece; } set { _td_cTotalFinishPiece = value; } }
        string _td_cTotalErrorPiece;
        public string td_cTotalErrorPiece { get { return _td_cTotalErrorPiece; } set { _td_cTotalErrorPiece = value; } }
        string _td_cTotalOnLinePiece;
        public string td_cTotalOnLinePiece { get { return _td_cTotalOnLinePiece; } set { _td_cTotalOnLinePiece = value; } }
        //======================================================================================
        //計算臥式的預計完成日
        public static string Get_CCS(string link, string Number)
        {
            GlobalVar.UseDB_setConnString(link);
            string sqlcmd = "";
            if (link.Contains("Hor"))
                sqlcmd = $"select item_no ccs from 組裝資料表 where 排程編號='{Number}' and item_no is not null and Len(item_no)>0";
            else
                sqlcmd = $"select item_no ccs from 組裝資料表 where 排程編號='{Number}' and item_no is not null and Len(item_no)>0";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
            if (HtmlUtil.Check_DataTable(dt))
                return dt.Rows[0]["ccs"].ToString();
            else
                return "";
        }
        //20220727新增 轉換臥式標準工時
        public DataTable Format_NowMonthTotal(DataTable dt)
        {
            dt.Columns.Add("預計完工日");
            work.工作時段_新增(8, 0, 12, 0);
            work.工作時段_新增(13, 0, 17, 0);
            DateTime stand_endtime = new DateTime();

            int standard_work = 0;
            foreach (DataRow row in dt.Rows)
            {
                standard_work = DataTableUtils.toInt(DataTableUtils.toString(row["標準工時"]));
                standard_work = standard_work == 0 ? 1 : standard_work;

                if (row["組裝預計完工日"].ToString() != "")
                    stand_endtime = DateTime.ParseExact(row["組裝預計完工日"].ToString(), "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
                else
                    stand_endtime = work.目標日期(StrToDate(row["上線日"].ToString()), new TimeSpan(0, 0, standard_work));
                row["預計完工日"] = stand_endtime.ToString("yyyyMMdd");
            }
            return dt;
        }
        public DataTable Return_NowMonthTotal(DataTable dt, string start_date, string end_date, out DataTable Finished, out DataTable NoFinish)
        {
            try
            {

                dt.Columns.Add("預計完工日");
                work.工作時段_新增(8, 0, 12, 0);
                work.工作時段_新增(13, 0, 17, 0);

                int standard_work = 0;
                foreach (DataRow row in dt.Rows)
                {
                    standard_work = DataTableUtils.toInt(DataTableUtils.toString(row["標準工時"]));
                    standard_work = standard_work == 0 ? 1 : standard_work;

                    DateTime stand_endtime = work.目標日期(StrToDate(row["上線日"].ToString()), new TimeSpan(0, 0, standard_work));
                    row["預計完工日"] = stand_endtime.ToString("yyyyMMdd");
                }

            }
            catch
            {

            }

            //撈出預計下架日在本月的, 與預計完工在下個月,但在本月提早完成的訂單
            DataTable dt_now = dt.Clone();
            DataTable dt_now_link = dt.Clone();
            string sqlcmd = $" 預計完工日>='{start_date}' and 預計完工日<='{end_date}' OR(預計完工日>'{end_date}' and (實際完成時間>='{start_date}' and '{end_date}'>=實際完成時間)) ";
            DataRow[] rows = dt.Select(sqlcmd);
            if (rows != null && rows.Length > 0)
                for (int i = 0; i < rows.Length; i++)
                    dt_now.ImportRow(rows[i]); 

            //-----Linq寫法-----//
            //var dt_1 = dt.AsEnumerable().Where(w => 
            //                                      DataTableUtils.toDouble(w.Field<string>("預計完工日")) >= DataTableUtils.toDouble($"{start_date}") &&
            //                                       DataTableUtils.toDouble(w.Field<string>("預計完工日")) <= DataTableUtils.toDouble($"{end_date}"));
            //    if (dt_1.FirstOrDefault() != null)
            //        {
            //            foreach (DataRow row in dt_1.CopyToDataTable().Rows) {
            //               dt_now_link.ImportRow(row);
            //           } 
            //       }
            //------------------//

                //撈出預計下架日在上個月，但在本月完成
                DataTable dt_Finish = dt.Clone();
            sqlcmd = $"預計完工日<'{start_date}' and (實際完成時間>='{start_date}000000' OR 實際完成時間>='{start_date}')";
            rows = dt.Select(sqlcmd);
            if (rows != null && rows.Length > 0)
                for (int i = 0; i < rows.Length; i++)
                    dt_Finish.ImportRow(rows[i]);

            //撈出到目前為止皆未完成的
            DataTable dt_NoFinish = dt.Clone();
            if (DataTableUtils.toInt(start_date) <= DataTableUtils.toInt(DateTime.Now.ToString("yyyyMMdd")))
            {
                sqlcmd = $"預計完工日<='{start_date}' and (實際完成時間 IS null OR 實際完成時間='' ) ";
                rows = dt.Select(sqlcmd);
                if (rows != null && rows.Length > 0)
                    for (int i = 0; i < rows.Length; i++)
                        dt_NoFinish.ImportRow(rows[i]);
            }

            Finished = dt_Finish;
            NoFinish = dt_NoFinish;

            return dt_now;
        }
        public List<string> AnsQueryString(string QueryString)
        {
            string[] Qstr;
            string[] QSubStr = new string[2] { "0", "0" };
            List<string> StrParList = new List<string>();
            if (!string.IsNullOrEmpty(QueryString))
            {
                Qstr = QueryString.Split(',');
                StrParList.Add(Qstr[0]); // First  ?AA = 1
                if (Qstr.Length > 1)
                {
                    foreach (string Str in Qstr)
                    {
                        QSubStr = Str.Split('=');
                        if (QSubStr.Length > 1)
                            StrParList.Add(QSubStr[1]);
                    }
                }
                return StrParList;
            }
            else
                return StrParList;
        }


        //找尋父編號
        private int Search_fatherID(string ID, string Link)
        {
            DataTableUtils.Conn_String = Link;
            string sqlcmd = $"select * from 工作站異常維護資料表 where 異常維護編號 = '{ID}'";
            DataRow row = DataTableUtils.DataTable_GetDataRow(sqlcmd);

            if (row != null)
            {
                if (DataTableUtils.toString(row["父編號"]) != "")
                    return DataTableUtils.toInt(DataTableUtils.toString(row["父編號"]));
                else
                    return DataTableUtils.toInt(ID);
            }
            else
                return DataTableUtils.toInt(ID);


        }
        //設定啟動時間 暫停時間 完成時間
        public bool change_status(string acc, string Link, DataTable dt, string tablename, string condition, string status, string Report, string percent = "")
        {
            acc = HtmlUtil.Search_acc_Column(acc, "USER_NAME");
            if (dt != null)
            {
                DataRow row = dt.NewRow();
                string now_status = DataTableUtils.toString(dt.Rows[0]["狀態"]);
                if (percent != "100")
                {
                    switch (status)
                    {
                        case "啟動":
                            row["狀態"] = status;
                            row["進度"] = percent;
                            if (now_status == "未動工")
                                row["實際啟動時間"] = DateTime.Now.ToString("yyyyMMddHHmmss");
                            else if (now_status == "暫停")
                                row["再次啟動時間"] = DateTime.Now.ToString("yyyyMMddHHmmss");
                            break;
                        case "暫停":
                            row["狀態"] = status;
                            row["進度"] = percent;
                            if (DataTableUtils.toString(dt.Rows[0]["實際啟動時間"]) == "")
                                row["實際啟動時間"] = DateTime.Now.ToString("yyyyMMddHHmmss");
                            row["暫停時間"] = DateTime.Now.ToString("yyyyMMddHHmmss");
                            break;
                        case "跑合":
                            row["狀態"] = "啟動";
                            row["進度"] = 99;
                            if (DataTableUtils.toString(dt.Rows[0]["實際啟動時間"]) == "")
                                row["實際啟動時間"] = DateTime.Now.ToString("yyyyMMddHHmmss");
                            row["再次啟動時間"] = DateTime.Now.ToString("yyyyMMddHHmmss");
                            break;
                        case "完成":
                            row["狀態"] = status;
                            row["進度"] = 100;
                            if (DataTableUtils.toString(dt.Rows[0]["實際啟動時間"]) == "")
                                row["實際啟動時間"] = DateTime.Now.ToString("yyyyMMddHHmmss");
                            row["實際完成時間"] = DateTime.Now.ToString("yyyyMMddHHmmss");
                            break;
                    }
                }
                else
                {
                    row["狀態"] = "完成";
                    row["進度"] = 100;
                    if (DataTableUtils.toString(dt.Rows[0]["實際啟動時間"]) == "")
                        row["實際啟動時間"] = DateTime.Now.ToString("yyyyMMddHHmmss");
                    row["實際完成時間"] = DateTime.Now.ToString("yyyyMMddHHmmss");
                }
                row["組裝者"] = acc;
                row["問題回報"] = Report;
                GlobalVar.UseDB_setConnString(Link);
                return DataTableUtils.Update_DataRow(tablename, condition, row);
            }
            else
                return false;
        }
        //增加或是修改子項目
        public bool Add_Content(string post, string ID, string Error, string ErrorID, string Account, string Department, string Content, string ImageLink = "", int number = 0, string ErrorType = "", string close_content = "", string close_file = "")
        {

            DataTableUtils.Conn_String = GetConnByDekVisTmp;
            string sqlcmd = "";
            DataTable dt = new DataTable();
            sqlcmd = $"select * from 工作站異常維護資料表 where 異常維護編號 = '{number}'";
            dt = DataTableUtils.GetDataTable(sqlcmd);
            int num = 0;

            DataRow dt_Name = GetAccInf(Account);
            if (HtmlUtil.Check_DataTable(dt))
            {
                num = Search_fatherID(number.ToString(), GetConnByDekVisTmp);
                DataRow row = dt.NewRow();
                row["維護內容"] = Content;
                if (ImageLink != "")
                    row["圖片檔名"] = ImageLink;
                if (DataTableUtils.toString(dt.Rows[0]["父編號"]) != "")
                {

                    row["結案判定類型"] = ErrorType;
                    row["結案內容"] = close_content;
                    if (close_file != "")
                        row["結案附檔"] = close_file;
                }
                if (post == "1")
                    row["是否有發送LINE"] = "Y";
                else
                    row["是否有發送LINE"] = "N";
                if (DataTableUtils.Update_DataRow("工作站異常維護資料表", $"異常維護編號 = '{number}'", row) == true)
                {
                    if (post == "1")
                        LineNote(DataTableUtils.toInt(dt.Rows[0]["工作站編號"].ToString()), ErrorID, "", Content, GetConnByDekVisTmp, num.ToString(), dt_Name["USER_NAME"].ToString());
                }

                return DataTableUtils.Update_DataRow("工作站異常維護資料表", $"異常維護編號 = '{number}'", row);
            }
            else
            {
                sqlcmd = $"select * from 工作站異常維護資料表 where 異常維護編號 = '{ID}' and 排程編號 = '{ErrorID}'";
                dt = DataTableUtils.GetDataTable(sqlcmd);
                if (HtmlUtil.Check_DataTable(dt))
                {

                    DataRow row = dt.NewRow();
                    sqlcmd = "select top(1) * from 工作站異常維護資料表 order by 異常維護編號 desc ";
                    DataTable dr = DataTableUtils.GetDataTable(sqlcmd);
                    if (number == 0)
                    {
                        num = DataTableUtils.toInt(DataTableUtils.toString(dr.Rows[0]["異常維護編號"])) + 1;
                        row["異常維護編號"] = num;
                        number = num;
                    }
                    else
                        row["異常維護編號"] = number;
                    row["排程編號"] = ErrorID;
                    row["異常原因類型"] = Error;
                    row["維護人員姓名"] = dt_Name["USER_NAME"].ToString();
                    row["維護人員單位"] = Department;
                    row["維護內容"] = Content;
                    row["時間紀錄"] = DateTime.Now.ToString("yyyyMMddHHmmss");
                    row["圖片檔名"] = ImageLink;
                    row["父編號"] = ID;
                    row["結案判定類型"] = ErrorType;
                    row["結案內容"] = close_content;
                    row["結案附檔"] = close_file;
                    //  row["工作站編號"] = dt.Rows[0]["工作站編號"];
                    if (post == "1")
                        row["是否有發送LINE"] = "Y";
                    else
                        row["是否有發送LINE"] = "N";
                    if (DataTableUtils.Insert_DataRow("工作站異常維護資料表", row) == true)
                    {
                        if (post == "1")
                            LineNote(DataTableUtils.toInt(dt.Rows[0]["工作站編號"].ToString()), ErrorID, "", Content, GetConnByDekVisTmp, number.ToString(), dt_Name["USER_NAME"].ToString());
                        return true;
                    }
                    else
                    {
                        string aa = DataTableUtils.ErrorMessage;
                        return false;
                    }

                }
                else
                    return false;
            }
        }
        public List<string> CaseErrorType(string Link)
        {
            List<string> list = new List<string>();
            GlobalVar.UseDB_setConnString(Link);
            string sqlcmd = "select 備註內容 from 工作站結案異常類型資料表";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
            if (dt != null && dt.Rows.Count > 0)
            {
                list.Add("");
                foreach (DataRow row in dt.Rows)
                    list.Add(DataTableUtils.toString(row["備註內容"]));
            }
            return list;
        }
        public int SetErrorDataToDataTable(string LineNum, string Pk, string ErrorType)
        {
            bool OK = false;
            int ErrorNum = 0;
            DataTableUtils.Conn_String = GetConnByDekVisTmp;
            DataTable dt_er = DataTableUtils.DataTable_GetRowHeader(ShareMemory.SQLAsm_WorkStation_Error);
            DataRow dr = dt_er.NewRow();
            ErrorNum = GetSeriesNum(ShareMemory.SQLAsm_WorkStation_Error, 0);
            dr["異常排除時間"] = "Time";
            dr["異常編號"] = ErrorNum;
            dr["異常起始時間"] = DateTime.Now.ToString("yyyyMMddHHmmss");
            dr["異常原因"] = ErrorType;
            dr["工作站編號"] = LineNum;
            dr["排程編號"] = Pk;
            OK = DataTableUtils.Insert_DataRow(ShareMemory.SQLAsm_WorkStation_Error, dr);
            return ErrorNum;
        }
        public bool SetMantDataToDataTable(string Account, string key, string ErrorType, string InputStr, string dep, string Status, List<string> StationInf, string value, string Image_Save = "")
        {
            //if (Account == "visrd")
            //    value = "0";
            InputStr = InputStr.Replace("'", " ");
            string backman = "";
            ////Mant
            bool OK = false;
            bool ProcessOK = false;
            DataTableUtils.Conn_String = GetConnByDekVisTmp;
            string condition = "";
            string SqlStr = "select Top 1 * From " + ShareMemory.SQLAsm_WorkStation_ErrorMant + " where 排程編號 = " + "'" + key + "'" + " ORDER BY " + "異常維護編號" + " desc";
            //int UserLevel = GetACCLevel(Account);
            DataTable dt_er = DataTableUtils.DataTable_GetTable(SqlStr);
            DataRow dt_Name = GetAccInf(Account);
            if (dt_er == null)
                dt_er = DataTableUtils.DataTable_GetRowHeader(ShareMemory.SQLAsm_WorkStation_ErrorMant);
            if (Status == ErrorProcessStatus.結案.ToString())
                ProcessOK = true;
            //
            DataRow dr = dt_er.NewRow();
            int ErrorNum = GetSeriesNum(ShareMemory.SQLAsm_WorkStation_ErrorMant, 0);
            dr["異常維護編號"] = ErrorNum;
            dr["排程編號"] = key;
            dr["時間紀錄"] = DateTime.Now.ToString("yyyyMMddHHmmss");
            dr["維護人員單位"] = dep;
            dr["維護內容"] = InputStr;
            dr["異常原因類型"] = ErrorType;
            dr["處理狀態"] = ProcessOK;
            dr["圖片檔名"] = Image_Save;
            if (value == "1")
                dr["是否有發送LINE"] = "Y";
            else
                dr["是否有發送LINE"] = "N";
            if (dt_Name != null)
            {
                dr["維護人員姓名"] = dt_Name["USER_NAME"].ToString();
                backman = dt_Name["USER_NAME"].ToString();
            }
            if (GetConnByDekVisTmp.Contains("dekVisAssm"))
            {
                dr[ShareMemory.WorkStationNum] = StationInf[1].ToString();
                condition = ShareMemory.PrimaryKey + "=" + "'" + key + "'";
            }
            else
            {
                dr[ShareMemory.WorkStationNum] = StationInf[1].ToString();
                condition = ShareMemory.PrimaryKey + "=" + "'" + key + "'" + " AND " + ShareMemory.WorkStationNum + "=" + "'" + StationInf[1].ToString() + "'";
            }
            // Mantan Table
            bool ok = DataTableUtils.Insert_DataRow(ShareMemory.SQLAsm_WorkStation_ErrorMant, dr);
            // Status Table
            DataRow dr_status = DataTableUtils.DataTable_GetDataRow(ShareMemory.SQLAsm_WorkStation_State, condition);
            if (dr_status != null)
            {
                dr_status["維護"] = InputStr + " " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                dr_status["異常"] = ErrorType;
                OK = DataTableUtils.Update_DataRow(ShareMemory.SQLAsm_WorkStation_State, condition, dr_status);
                //Updata Flag
                Note_MachineID_Line_Updata(dr_status["工作站編號"].ToString());
                if (value == "1")
                    LineNote(DataTableUtils.toInt(dr_status["工作站編號"].ToString()), key, ErrorType, InputStr, GetConnByDekVisTmp, ErrorNum.ToString(), backman);
                return OK;
            }
            else
                return OK;
        }
        public void Set_MachineID_Line_Updata(string LineNum)
        {
            bool OK = false;
            DataTableUtils.Conn_String = GetConnByDekVisTmp;
            DataTable dt = DataTableUtils.GetDataTable(ShareMemory.SQLAsm_MachineID_Line, "機台產線代號" + "=" + "'" + LineNum + "'");
            foreach (DataRow dr in dt.Rows)
            {
                if (!(dr["是否有更新資料現場"].ToString().ToUpper() == "TRUE" || dr["是否有更新資料現場"].ToString().ToUpper() == "1"))
                {
                    dr["是否有更新資料現場"] = true;
                    //insert or updata
                    if (DataTableUtils.RowCount(ShareMemory.SQLAsm_MachineID_Line, "機台產線代號" + "=" + "'" + LineNum + "'") != 0)
                        OK = DataTableUtils.Update_DataRow(ShareMemory.SQLAsm_MachineID_Line, "機台編號 =" + "'" + dr["機台編號"].ToString() + "'", dr);
                }
            }
        }
        public DataRow Set_NewStatus(DataRow dr, string radio_Select, string Progress)
        {
            string NowTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            if (dr != null)
            {
                //string radio_Select = RadioButtonList_select_type.SelectedValue;
                switch (radio_Select)
                {
                    case "0"://啟動
                        dr["狀態"] = "啟動";
                        if (dr["實際啟動時間"].ToString().ToUpper() == "NULL")
                            dr["實際啟動時間"] = NowTime;
                        dr["再次啟動時間"] = NowTime;
                        dr["進度"] = Progress.Trim('%');
                        break;
                    case "1"://暫停
                        dr["狀態"] = "暫停";
                        dr["暫停時間"] = NowTime;
                        dr["異常狀態號"] = SetErrorDataToDataTable(dr["工作站編號"].ToString(), dr[ShareMemory.PrimaryKey].ToString(), null);
                        dr["進度"] = Progress.Trim('%');
                        break;
                    case "2"://完成
                        dr["狀態"] = "完成";
                        dr["實際完成時間"] = NowTime;
                        dr["進度"] = "100";
                        break;
                }
            }
            return dr;
        }
        public string[] GetLineTotal(int infCount, ref int no_detail, ref int behind)
        {
            List<string> list = new List<string>();
            int count = 0;
            int nosolved = 0;
            string td = "";
            int totalTaget = 0;
            int totalTagetPerson = 0;
            int totalFinish = 0;
            int totalError = 0;
            int totalOnLine = 0;
            int td_totalFinish = 0;
            int td_totalError = 0;
            int td_totalOnLine = 0;
            DataTableUtils.Conn_String = GetConnByDekVisTmp;
            string[] str = new string[2];//0:finish 1: //clsDB_sw
            DataTable dt_line = null;
            int alarm_count = 0;

            dt_line = DataTableUtils.DataTable_GetTable("select * from " + ShareMemory.SQLAsm_WorkStation_Type + " where 工作站是否使用中 = 1");

            //DataTable dt_line = DataTableUtils.DataTable_GetTable("select * from " + ShareMemory.SQLAsm_WorkStation_Type);
            //DataTable dt_line = clsDB_sw.DataTable_GetTable("select * from 工作站型態資料表", 0, 0);
            if (HtmlUtil.Check_DataTable(dt_line))
            {

                DataTable dt_select = tableColumnSelectForTotalLine(dt_line);
                //Rows
                if (HtmlUtil.Check_DataTable(dt_select))
                {
                    for (int i = 0; i < dt_select.Rows.Count; i++)
                    {
                        int x = 0, y = 0;
                        //計算落後的
                        Calculation_Alarm_Or_Behind(DataTableUtils.toString(dt_select.Rows[i]["工作站編號"]), ref nosolved, ref count);
                        td += "<tr class='gradeX'> \n";
                        for (int j = 0; j < dt_select.Columns.Count; j++)
                        {
                            if (dt_select.Columns[j].ColumnName != "工作站是否使用中" && dt_select.Columns[j].ColumnName != "工作站編號" && dt_select.Columns[j].ColumnName != "人數配置")
                            {
                                if (DataTableUtils.toInt(dt_select.Rows[i]["暫停"].ToString()) != 0)
                                {
                                    if (dt_select.Columns[j].ColumnName == "工作站名稱")
                                    {
                                        string url = "LineNum=" + DataTableUtils.toString(dt_select.Rows[i]["工作站編號"]) + "," + "ReqType=Line";
                                        td += "<td style='text-align:center;'>" +
                                                  "<u>" +
                                                      "<a onclick=jump_AsmLineOverView('" + WebUtils.UrlStringEncode(url) + "')  href=\"javascript: void()\">    " +
                                                          "<div style=\"height:100%;width:100%\">" +
                                                            DataTableUtils.toString(dt_select.Rows[i][j]) +
                                                          "</div>" +
                                                      "</a>" +
                                                  "</u>" +
                                              "</td> \n";
                                    }
                                    else if (dt_select.Columns[j].ColumnName == "暫停")
                                    {
                                        string url = "LineNum=" + DataTableUtils.toString(dt_select.Rows[i]["工作站編號"]) + "," + "ReqType=Error";
                                        td += "<td style='text-align:center;background-color:lightcoral;color:white;font-size:20px;'>" +
                                                  "<u>" +
                                                      "<a onclick=jump_AsmLineOverView('" + WebUtils.UrlStringEncode(url) + "') style='color:white;' href=\"javascript: void()\">  " +
                                                          "<div style=\"height:100%;width:100%\">" +
                                                              DataTableUtils.toString(dt_select.Rows[i][j]) +
                                                          "</div>" +
                                                      "</a>" +
                                                  "</u>" +
                                              "</td> \n";
                                    }
                                    else if (dt_select.Columns[j].ColumnName == "目標件數")
                                    {
                                    }
                                    else if (dt_select.Columns[j].ColumnName == "今日完成" || dt_select.Columns[j].ColumnName == "今日暫停" || dt_select.Columns[j].ColumnName == "今日生產中")
                                    {

                                    }
                                    else
                                        td += "<td style='text-align:center;'>" + DataTableUtils.toString(dt_select.Rows[i][j]) + "</td> \n";
                                }
                                else if (dt_select.Columns[j].ColumnName == "今日完成" || dt_select.Columns[j].ColumnName == "今日暫停" || dt_select.Columns[j].ColumnName == "今日生產中")
                                {

                                }
                                else
                                {
                                    if (dt_select.Columns[j].ColumnName == "工作站名稱")//.ASPX?+parameter(Ex:LineNum=1)
                                    {
                                        string url = "LineNum=" + DataTableUtils.toString(dt_select.Rows[i]["工作站編號"]) + "," + "ReqType=Line";
                                        td += "<td style='text-align:center;'>" +
                                                  "<u>" +
                                                      "<a onclick=jump_AsmLineOverView('" + WebUtils.UrlStringEncode(url) + "') href=\"javascript: void()\" >" +
                                                          "  <div style=\"height:100%;width:100%\">" +
                                                             DataTableUtils.toString(dt_select.Rows[i][j]) +
                                                          "</div>" +
                                                      "</a>" +
                                                  "</u>" +
                                              "</td>\n";

                                    }
                                    else if (dt_select.Columns[j].ColumnName == "目標件數")
                                    {
                                    }
                                    else if (dt_select.Columns[j].ColumnName == "今日完成" || dt_select.Columns[j].ColumnName == "今日暫停" || dt_select.Columns[j].ColumnName == "今日生產中")
                                    {
                                    }
                                    else
                                        td += "<td style='text-align:center;'>" + DataTableUtils.toString(dt_select.Rows[i][j]) + "</td>\n";
                                }
                            }
                            else
                                continue;
                        }
                        //放今日落後用的
                        list = Calculation_Alarm_Or_Behind(DataTableUtils.toString(dt_select.Rows[i]["工作站編號"]), ref x, ref y, true);

                        td += "<td style='text-align:center;'>" + y + "</td>\n";
                        //計算未解決的機台數量
                        alarm_count += list.Count;

                        // Count Piece
                        totalTaget += DataTableUtils.toInt(dt_select.Rows[i]["目標件數"].ToString());
                        totalTagetPerson += DataTableUtils.toInt(dt_select.Rows[i]["人數配置"].ToString());
                        totalFinish += DataTableUtils.toInt(dt_select.Rows[i]["完成"].ToString());
                        totalError += DataTableUtils.toInt(dt_select.Rows[i]["暫停"].ToString());
                        totalOnLine += DataTableUtils.toInt(dt_select.Rows[i]["生產中"].ToString());
                        td_totalFinish += DataTableUtils.toInt(dt_select.Rows[i]["今日完成"].ToString());
                        td_totalError += DataTableUtils.toInt(dt_select.Rows[i]["今日暫停"].ToString());
                        td_totalOnLine += DataTableUtils.toInt(dt_select.Rows[i]["今日生產中"].ToString());
                        td += "</tr> \n";
                    }
                    str[1] = td;
                    _cTotalTagetPiece = totalTaget.ToString();
                    _cTotalTagetPerson = totalTagetPerson.ToString();
                    _cTotalFinishPiece = totalFinish.ToString();
                    _cTotalErrorPiece = totalError.ToString();
                    _cTotalOnLinePiece = totalOnLine.ToString();
                    _td_cTotalFinishPiece = td_totalFinish.ToString();
                    _td_cTotalErrorPiece = td_totalError.ToString();
                    _td_cTotalOnLinePiece = td_totalOnLine.ToString();

                    no_detail = alarm_count;
                    behind = count;
                    return str;
                }
                else
                {
                    no_detail = alarm_count;
                    behind = count;
                    return str;
                }
            }
            else
            {
                no_detail = alarm_count;
                behind = count;
                str[0] = " no data";
                return str;
            }
        }
        //計算未解決跟落後用，回傳未解決的機台編號
        private List<string> Calculation_Alarm_Or_Behind(string LineNum, ref int alarm, ref int count, bool recover = false)
        {
            //LineNum→產線編號 alarm→未解決數量 count→落後數量 recover→要不要重新計算
            List<double> list = new List<double>();
            string Condition1 = "";
            string Condition2 = "";
            string Condition3 = "";
            string Condition = "";
            List<string> alarm_num = new List<string>();

            Condition1 = "工作站編號 = " + "'" + LineNum.ToString() + "'" + " AND " + "實際組裝時間 =" + "'" + DateTime.Now.ToString("yyyyMMdd").ToString() + "'";
            Condition2 = "工作站編號 = " + "'" + LineNum.ToString() + "'" + " AND " + "實際組裝時間 <=" + "'" + DateTime.Now.ToString("yyyyMMdd").ToString() + "'" + " AND " + "狀態!=" + "'" + "完成" + "'";
            Condition3 = "工作站編號 = '" + LineNum.ToString() + "' AND 實際完成時間 <='" + DateTime.Now.ToString("yyyyMMdd") + "235959' AND 實際完成時間 >='" + DateTime.Now.ToString("yyyyMMdd") + "010101'";
            Condition = Condition1 + " OR " + Condition2 + " OR " + Condition3 + " order by " + "組裝日" + "," + "組裝編號";
            DataTable dt = DataTableUtils.DataTable_GetTable(ShareMemory.SQLAsm_WorkStation_State, Condition);
            //暫停
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (recover)
                        alarm = 0;
                    try
                    {
                        check_case(DataTableUtils.toString(row["排程編號"]), GetConnByDekVisTmp, LineNum.ToString(), ref alarm);
                    }
                    catch
                    {

                    }

                    try
                    {
                        string no_mean = "";
                        list = percent_calculation(DataTableUtils.toString(row["排程編號"]), DataTableUtils.toString(row["進度"]), ref no_mean, LineNum);
                        bool delay = false;
                        Comparison_Schedule(list[0], list[1], ref count, ref delay);
                    }
                    catch
                    {

                    }
                    if (alarm > 0 && alarm_num.IndexOf(DataTableUtils.toString(row["排程編號"])) == -1)
                        alarm_num.Add(DataTableUtils.toString(row["排程編號"]));
                }
            }
            return alarm_num;

        }
        public string[] GetErrorRowsData(string ErrorID, ListItem Line, string ErrorType)
        {
            string td = "";
            string[] str = new string[4];//0:un 1:html cmd
            DataTableUtils.Conn_String = GetConnByDekVisTmp;
            string Condition = GetErrorSearchCondition(ErrorID, Line.Value, ErrorType);
            Condition = Condition + " AND 異常原因 is not null";
            //string Condition = "排程編號 = " + "'" + ErrorID + "'";
            // string Condition = "排程編號 like " + "'" +"%"+ ErrorID +"%"+ "'";
            string Sql = "select * from 工作站異常紀錄資料表 INNER JOIN 工作站型態資料表 ON 工作站異常紀錄資料表.工作站編號 = 工作站型態資料表.工作站編號" + " where " + Condition;
            DataTable dt = DataTableUtils.GetDataTable(Sql);

            if (HtmlUtil.Check_DataTable(dt))
            {
                DataTable dt_select = tableColumnSelectForErrorSearchList(dt);
                // "排程編號", "異常編號", "工作站編號", "異常原因"
                //Rows
                if (HtmlUtil.Check_DataTable(dt_select))
                {
                    for (int i = 0; i < dt_select.Rows.Count; i++)
                    {
                        td += "<tr class='gradeX'> \n";
                        for (int j = 0; j < dt_select.Columns.Count; j++)
                        {
                            if (dt_select.Columns[j].ColumnName == "工作站編號")//維護人員姓名                              
                                td += "<td style='text-align:center;'>" + DataTableUtils.toString(dt_select.Rows[i]["工作站名稱"]) + "</td> \n";
                            else if (dt_select.Columns[j].ColumnName == "異常原因")//維護人員姓名   
                            {
                                if (dt_select.Rows[i]["異常起始時間"].ToString() != "" && dt_select.Rows[i]["異常起始時間"].ToString() != null && dt_select.Rows[i]["異常起始時間"].ToString().ToUpper() != "NULL")
                                {
                                    string url = "ErrorSearch_Detail" + "," + "LineNum=" + dt.Rows[i]["工作站編號"].ToString() + "," + "Value1=" + DataTableUtils.toString(dt_select.Rows[i]["排程編號"]) + "," + "Value2=" + dt.Rows[i]["異常原因"].ToString();
                                    td += "<td style='text-align:center;'>" +
                                        "<u><a href='Asm_Cahrt_Detail.aspx?Key=" + WebUtils.UrlStringEncode(url) + "'" + ">" + "</u>" + " " + DataTableUtils.toString(dt_select.Rows[i][j]) + " <br >"
                                         + "<span style='font-size:4px ;text-align:right;color:gray;'> " + DateTime.ParseExact(dt_select.Rows[i]["異常起始時間"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture).ToString("yyyy-MM-dd HH:mm:ss") + " <br ></ span >"
                                        + "</a></td> \n";
                                }
                                else
                                {
                                    string url = "ErrorID=" + DataTableUtils.toString(dt_select.Rows[i]["排程編號"]) + "," + "ErrorLineNum=" + dt.Rows[i]["工作站編號"].ToString() + "," + "ErrorType=" + dt.Rows[i]["異常原因"].ToString();
                                    td += "<td style='text-align:center;'><a href='Asm_ErrorSearchDetail.aspx?key=" + WebUtils.UrlStringEncode(url) + "'" + ">" + DataTableUtils.toString(dt_select.Rows[i][j]) + "</a></td> \n";


                                }
                            }
                            else if (dt_select.Columns[j].ColumnName == "異常起始時間")//維護人員姓名   
                            {
                                //none
                            }
                            else if (dt_select.Columns[j].ColumnName == "工作站名稱")//維護人員姓名   
                            {
                                //none
                            }
                            else
                                td += "<td style='text-align:center;'>" + DataTableUtils.toString(dt_select.Rows[i][j]) + "</td> \n";
                        }
                        td += "</tr> \n";
                    }
                    str[1] = td;
                    return str;
                }
                else
                {
                    return str;
                }
            }
            else
            {
                str[0] = " no data";
                return str;
            }
        }
        public string[] GetHistoryRowsData(string ErrorID, ListItem Line, string start = "", string end = "", List<string> Number = null, string LineNum = "", string db = "", string Datestart = "", string Dateend = "", string errortype = "")
        {
            string td = "";
            string[] str = new string[4];//0:un 1:html cmd
            DataTableUtils.Conn_String = GetConnByDekVisTmp;

            DataTable dt = new DataTable();
            DateTime time = new DateTime();
            if (Number == null && LineNum == "")
            {

                string Condition = GetHistorySearchCondition(ErrorID, Line.Value);

                string daterange = "";
                if (start != "" && end != "")
                    daterange = $" and 實際完成時間 >= {start.Replace("-", "")+"000000"} and 實際完成時間 <= {end.Replace("-", "")+"000000"} ";//20220825 組裝時間變更為實際完成時間

                string Sql = $"select TOP(500) * from {ShareMemory.SQLAsm_WorkStation_State} INNER JOIN 工作站型態資料表 ON 工作站狀態資料表.工作站編號 = 工作站型態資料表.工作站編號  where {Condition} {daterange} order by 組裝日 desc";
                dt = DataTableUtils.GetDataTable(Sql);

            }
            else if (Number != null && LineNum != "")
            {
                if (db == "ver")
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
                else if (db == "hor")
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);

                string schdule_number = "";
                for (int i = 0; i < Number.Count - 1; i++)
                {
                    if (i == 0)
                        schdule_number += $" and (  排程編號 ='{Number[i]}' ";
                    else
                        schdule_number += $" OR  排程編號 ='{Number[i]}' ";
                }
                schdule_number = schdule_number + ")";
                string Sql = $"select  * from {ShareMemory.SQLAsm_WorkStation_State} INNER JOIN 工作站型態資料表 ON 工作站狀態資料表.工作站編號 = 工作站型態資料表.工作站編號  where 工作站狀態資料表.工作站編號='{LineNum}' {schdule_number}  order by 組裝日 desc";
                dt = DataTableUtils.GetDataTable(Sql);
            }


            if (HtmlUtil.Check_DataTable(dt))
            {
                DataTable dt_select = tableColumnSelectForHistorySearchList(dt);
                dt_select.Columns.Add("異常歷程");
                //  "排程編號", "工作站名稱", "實際啟動時間", "實際完成時間", "組裝累積時間");
                //Rows
                if (HtmlUtil.Check_DataTable(dt_select))
                {
                    for (int i = 0; i < dt_select.Rows.Count; i++)
                    {
                        td += "<tr class='gradeX'>";
                        for (int j = 0; j < dt_select.Columns.Count; j++)
                        {
                            if (dt_select.Columns[j].ColumnName == "排程編號")//維護人員姓名                              
                                td += $"<td style='text-align:center;'>{DataTableUtils.toString(dt_select.Rows[i]["排程編號"])}</td>";
                            else if (dt_select.Columns[j].ColumnName == "工作站名稱")//維護人員姓名   
                                td += $"<td style='text-align:center;'>{DataTableUtils.toString(dt_select.Rows[i]["工作站名稱"])}</td>";
                            else if (dt_select.Columns[j].ColumnName == "實際啟動時間")//維護人員姓名 
                            {
                                time = StrToDateTime(DataTableUtils.toString(dt_select.Rows[i]["實際啟動時間"]), "yyyyMMddHHmmss");
                                td += $"<td style='text-align:center;'>{time.ToString("yyyy/MM/dd HH:mm:ss")}</td>";
                            }
                            else if (dt_select.Columns[j].ColumnName == "實際完成時間")//維護人員姓名  
                            {
                                time = StrToDateTime(DataTableUtils.toString(dt_select.Rows[i]["實際完成時間"]), "yyyyMMddHHmmss");
                                td += $"<td style='text-align:center;'>{time.ToString("yyyy/MM/dd HH:mm:ss")}</td>";
                            }
                            else if (dt_select.Columns[j].ColumnName == "異常歷程")
                            {
                                string date = "";
                                if (db == "")
                                    DataTableUtils.Conn_String = GetConnByDekVisTmp;
                                else
                                {
                                    if (db == "ver")
                                        GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
                                    else if (db == "hor")
                                        GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);
                                    date = $" and '{Datestart}' <= 時間紀錄 and 時間紀錄 <= '{Dateend}' and 結案判定類型 = '{errortype}' ";

                                }
                                string sqlcmd = $"select * from 工作站異常維護資料表 where (父編號 is null OR 父編號 = '0') and 排程編號 = '{ DataTableUtils.toString(dt_select.Rows[i]["排程編號"])}' and 工作站編號 = '{DataTableUtils.toString(dt_select.Rows[i]["工作站編號"])}' ";
                                DataTable ds = DataTableUtils.GetDataTable(sqlcmd);
                                int count = 0;
                                if (HtmlUtil.Check_DataTable(ds))
                                {
                                    if (db != "")
                                    {
                                        foreach (DataRow data in ds.Rows)
                                        {
                                            sqlcmd = $"select * from 工作站異常維護資料表 where 父編號 = '{DataTableUtils.toString(data["異常維護編號"])}' {date} ";
                                            DataTable de = DataTableUtils.GetDataTable(sqlcmd);
                                            if (de != null)
                                                count += de.Rows.Count;
                                        }

                                    }
                                    else
                                        count = ds.Rows.Count;

                                }

                                string urlpre = "ErrorID=" + DataTableUtils.toString(dt_select.Rows[i]["排程編號"]) + ",ErrorLineNum=" + DataTableUtils.toString(dt_select.Rows[i]["工作站編號"]) + ",ErrorLineName=" + DataTableUtils.toString(dt_select.Rows[i]["工作站名稱"]) + ",history=1";
                                if (db != "")
                                    urlpre = urlpre + ",Date_str=" + Datestart + ",Date_end=" + Dateend + ",ErrorType=" + errortype + ",db=" + db;

                                string url = WebUtils.UrlStringEncode(urlpre);
                                td += "<td style='text-align:center;'><a href='Asm_ErrorDetail.aspx?key=" + url + "'><div style='height:100%;width:100%'>" + count + "</div></a></td>";
                            }
                            else if (dt_select.Columns[j].ColumnName == "工作站編號")
                            {

                            }
                            else//組裝累積時間
                                td += "<td style='text-align:center;'>" + (DataTableUtils.toInt(dt_select.Rows[i][j].ToString()) / 60) + "</td>";
                        }
                        td += "</tr>";
                    }
                    str[1] = td;
                    return str;
                }
                else
                    return str;
            }
            else
            {
                str[0] = " no data";
                return str;
            }
        }
        public string[] GetMantRowsData(string acc, string ErrorID, string LineNum, ref string[] _errorTitleShow, ref string[] _dep, ref string[] _status, string f_type = "", string MantID = "", string startdate = "", string enddate = "", string errortype = "", string db = "")
        {
            string type = HtmlUtil.Search_acc_Column(acc, "Last_Model");
            string YN = HtmlUtil.Search_acc_Column(acc, "Can_Close");
            acc = HtmlUtil.Search_acc_Column(acc, "USER_NAME");

            string update = "";
            string delete = "";
            string td = "";
            string[] str = new string[6];//0:un 1:html cmd
            string[] strTime;
            string Condition = "";
            //------------------------------------20200424新增-----------------------------------

            if (f_type == "")
            {
                //  DataTableUtils.Conn_String = GetConnByDekVisTmp;
                if (type.ToLower() == "ver")
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
                else
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);

                if (MantID != "")
                {
                    //先找該ID的父項目
                    string sql = $"select * from 工作站異常維護資料表 where 異常維護編號 = {MantID}";
                    DataTable dt_row = DataTableUtils.GetDataTable(sql);
                    // DataRow row = DataTableUtils.DataTable_GetDataRow(sql);

                    if (dt_row != null && dt_row.Rows.Count > 0 && DataTableUtils.toString(dt_row.Rows[0]["父編號"]) != "" && DataTableUtils.toString(dt_row.Rows[0]["父編號"]) != "0")
                    {
                        MantID = DataTableUtils.toString(dt_row.Rows[0]["父編號"]);
                        MantID = $" and 異常維護編號={MantID} ";
                    }
                    else
                        MantID = $" and 異常維護編號={MantID} ";
                }


                if (type.ToLower() == "ver")
                {
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
                    type = "Ver";
                    Condition = "排程編號=" + "'" + ErrorID + "'  and (父編號 is null OR 父編號 = 0)" + MantID;
                }
                else
                {
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);
                    type = "Hor";
                    Condition = "排程編號=" + "'" + ErrorID + "'" + " AND " + "工作站編號=" + "'" + LineNum + "'  and (父編號 is null OR 父編號 = 0)" + MantID;
                }
            }
            else
            {
                if (type.ToLower() == "ver")
                {
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
                    type = "Ver";
                    Condition = "排程編號=" + "'" + ErrorID + "'  and (父編號 is null OR 父編號 = 0)" + MantID;
                }
                else
                {
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);
                    type = "Hor";
                    Condition = "排程編號=" + "'" + ErrorID + "'" + " AND " + "工作站編號=" + "'" + LineNum + "'  and (父編號 is null OR 父編號 = 0)" + MantID;
                }
            }

            //------------------------------------20200424新增-----------------------------------

            int ErrorTitleCount = 0;
            List<string> Errors = new List<string>();
            if (type.ToLower() == "ver")
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
            else
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);
            DataTable dt = DataTableUtils.DataTable_GetTable(ShareMemory.SQLAsm_WorkStation_ErrorMant, Condition);

            if (db != "")
            {
                List<string> schdule_list = new List<string>();
                foreach (DataRow datarow in dt.Rows)
                {
                    if (type.ToLower() == "ver")
                        GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
                    else
                        GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);

                    string sqlcmd = $"select * from 工作站異常維護資料表 where 父編號 = '{DataTableUtils.toString(datarow["異常維護編號"])}' and '{startdate}' <= 時間紀錄 and 時間紀錄 <= '{enddate}' and 結案判定類型 = '{errortype}' ";
                    DataTable de = DataTableUtils.GetDataTable(sqlcmd);
                    if (de != null && de.Rows.Count == 0)
                        schdule_list.Add(DataTableUtils.toString(datarow["異常維護編號"]));
                }

                for (int x = 0; x < schdule_list.Count; x++)
                {
                    DataRow[] rows = dt.Select($"異常維護編號='{schdule_list[x]}'");
                    for (int i = 0; i < rows.Length; i++)
                        rows[i].Delete();
                }
            }


            if (HtmlUtil.Check_DataTable(dt))
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Errors.Add("異常");
                    _errorTitleShow[0] = "異常";
                }
                DataTable dt_select = EtableColumnSelectForLineDetail(dt);
                string new_ID = "";
                //Rows
                if (HtmlUtil.Check_DataTable(dt_select))
                {
                    for (int i = 0; i < dt_select.Rows.Count; i++)
                    {
                        if (type.ToLower() == "ver")
                            GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
                        else
                            GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);

                        //得到子項目的datatable
                        string sqlcmd = "select * from 工作站異常維護資料表 where 父編號 = " + DataTableUtils.toString(dt_select.Rows[i]["異常維護編號"]) + " and 排程編號 = '" + ErrorID + "' order by 時間紀錄 asc";
                        DataTable ds = DataTableUtils.GetDataTable(sqlcmd);
                        if (!string.IsNullOrEmpty(dt_select.Rows[i]["異常原因類型"].ToString()))
                        {
                            td += "<tr class='gradeX'> \n";

                            for (int j = 0; j < dt_select.Columns.Count; j++)
                            {
                                if (dt_select.Columns[j].ColumnName == "維護人員姓名")//維護人員姓名
                                {
                                    string part = "";
                                    if (DataTableUtils.toString(dt_select.Rows[i][j]) != "")
                                    {
                                        if (_dep[0] == "nodata")
                                            _dep[0] = DataTableUtils.toString(dt_select.Rows[i]["維護人員單位"]);
                                        part = DataTableUtils.toString(dt_select.Rows[i]["維護人員單位"]);
                                    }
                                    else
                                    {
                                        if (_dep[0] == "nodata")
                                            _dep[0] = "生產部";
                                        part = "生產部";
                                    }


                                    strTime = dt_select.Rows[i]["時間紀錄"].ToString().Split(' ');

                                    if (strTime.Length > 2)
                                    {
                                        string[] st = strTime[0].Split('/');
                                        if (Int16.Parse(st[1]) < 10)
                                            st[1] = "0" + st[1];
                                        if (Int16.Parse(st[2]) < 10)
                                            st[2] = "0" + st[2];
                                        strTime[0] = st[0] + "/" + st[1] + "/" + st[2] + strTime[1] + " " + strTime[2];
                                    }
                                    else
                                        strTime[0] = StrToDate(strTime[0]).ToString("yyyy/MM/dd tt hh:mm:ss");

                                    td += $"<td style='text-align:center;width:15%'>" +
                                                $"<span style='font-size:8px ;text-align:right;color:red;'> " +
                                                    $"<b>{strTime[0]}</b>" +
                                                "</span>" +
                                                "<br>" +
                                                    $"[{part}]" + DataTableUtils.toString(dt_select.Rows[i][j]) +
                                                "<br>" +
                                          "</td> \n";
                                }
                                else if (dt_select.Columns[j].ColumnName == "時間紀錄")
                                {
                                    //none
                                }
                                else if (dt_select.Columns[j].ColumnName == "異常原因類型")
                                {
                                    //none
                                }
                                else if (dt_select.Columns[j].ColumnName == "維護人員單位")
                                {

                                }
                                else if (dt_select.Columns[j].ColumnName == "處理狀態")
                                    _status[0] = DataTableUtils.toString(dt_select.Rows[i][j]);
                                else if (dt_select.Columns[j].ColumnName == "異常維護編號")
                                {
                                }
                                else if (dt_select.Columns[j].ColumnName == "圖片檔名")
                                {
                                }
                                else if (dt_select.Columns[j].ColumnName == "結案判定類型")
                                {
                                    if (type.ToLower() == "ver")
                                        GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
                                    else
                                        GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);

                                    string sql = "select * from 工作站異常維護資料表 where 父編號 = '" + dt_select.Rows[i]["異常維護編號"] + "' order by 時間紀錄 desc";
                                    DataTable dt_row = DataTableUtils.GetDataTable(sql);
                                    //    DataRow row = DataTableUtils.DataTable_GetDataRow(sql);
                                    if (HtmlUtil.Check_DataTable(dt_row))
                                    {
                                        if (DataTableUtils.toString(dt_row.Rows[0]["結案判定類型"]) != "")
                                            td += "<td style='text-align:center;min-width:45px;max-width:25%;'>" + "<span style='color:green'>結案</span><br>" + DataTableUtils.toString(dt_row.Rows[0]["結案判定類型"]) + "<br></td> \n";
                                        else
                                            td += "<td style='text-align:center;min-width:45px;max-width:25%;'>" + "<span style='color:red'>處理中</span></td> \n";
                                    }
                                    else
                                        td += "<td style='text-align:center;min-width:45px;max-width:25%;'>" + "<span style='color:red'>處理中</span></td> \n";
                                }

                                //------------------------------------20200424新增-----------------------------------
                                else if (dt_select.Columns[j].ColumnName == "維護內容")
                                {
                                    string status = "";
                                    if (ds != null && ds.Rows.Count > 0)
                                        status = DataTableUtils.toString(ds.Rows[ds.Rows.Count - 1]["結案判定類型"]);
                                    string file = "";
                                    string Message_count = "";
                                    string Message = "";
                                    string Message_Open = "";
                                    if (ds != null && ds.Rows.Count > 0)
                                    {
                                        if (MantID != "")
                                            new_ID = " in ";
                                        else
                                            new_ID = "";

                                        string hr = "";
                                        for (int a = 0; a < ds.Rows.Count; a++)
                                        {
                                            strTime = ds.Rows[a]["時間紀錄"].ToString().Split(' ');

                                            string[] st = strTime[0].Split('/');
                                            if (st.Length > 2)
                                            {
                                                if (Int16.Parse(st[1]) < 10)
                                                    st[1] = "0" + st[1];
                                                if (Int16.Parse(st[2]) < 10)
                                                    st[2] = "0" + st[2];
                                                strTime[0] = st[0] + "/" + st[1] + "/" + st[2] + strTime[1] + " " + strTime[2];
                                            }
                                            else
                                                strTime[0] = StrToDate(strTime[0]).ToString("yyyy/MM/dd tt hh:mm:ss");

                                            //子項目附檔
                                            string videos = "";
                                            file = "";
                                            file = Return_fileurl(DataTableUtils.toString(ds.Rows[a]["圖片檔名"]), out videos);
                                            //回覆人員 訊息 時間
                                            string End_Report = "";
                                            string close_file = "";
                                            string close_video = "";
                                            if (type.ToLower() == "ver")
                                                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
                                            else
                                                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);

                                            string sql = "select * from 工作站異常維護資料表 where 父編號 = '" + dt_select.Rows[i]["異常維護編號"] + "' order by 時間紀錄 desc";
                                            DataTable dt_row = DataTableUtils.GetDataTable(sql);
                                            //   DataRow row = DataTableUtils.DataTable_GetDataRow(sql);
                                            if (HtmlUtil.Check_DataTable(dt_row))
                                                close_file = Return_fileurl(DataTableUtils.toString(dt_row.Rows[0]["結案附檔"]), out close_video);


                                            if (a == ds.Rows.Count - 1 && status != "")
                                                End_Report = "<div class=\"_EndDescripfa fa-chevron-circle-downtion\"><span style='color:green'>[結案說明]" + DataTableUtils.toString(ds.Rows[a]["維護人員姓名"]) + "</span>:" + br_text(DataTableUtils.toString(ds.Rows[a]["結案內容"])) + "<br>" + close_file + close_video + " </div>";

                                            delete = "";
                                            update = "";
                                            if (YN == "Y" || acc == DataTableUtils.toString(ds.Rows[a]["維護人員姓名"]))
                                            {
                                                update = " <div><a href='javascript:void(0)' onclick=updates('up_" + DataTableUtils.toString(ds.Rows[a]["異常維護編號"]) + "','" + ErrorID + "','" + type + "') data-toggle = \"modal\" data-target = \"#exampleModal\"><u>編輯</u></a></div>";
                                                delete = "<a href='javascript:void(0)' onclick=deletes('" + DataTableUtils.toString(ds.Rows[a]["異常維護編號"]) + "','" + ErrorID + "','" + "" + "') ><u>刪除</u></a>";
                                            }
                                            else
                                            {
                                                update = "<div>編輯</div>";
                                                delete = "刪除";
                                            }
                                            Message += $"<div class=\"_EndDescription\">" +
                                                        $"<span style='color:blue'>[{DataTableUtils.toString(ds.Rows[a]["維護人員單位"])}] {DataTableUtils.toString(ds.Rows[a]["維護人員姓名"])}</span>:{br_text(DataTableUtils.toString(ds.Rows[a]["維護內容"]))} {file}" +
                                                        "</div>" +
                                                        videos +
                                                        "<div id=\"_Response\" class=\"col-12 col-xs-12 text-right\" style=\"margin-right:-6px;\">" +
                                                            $"<h6 style=\"text-align:right;\">{strTime[0]}</h6>" +
                                                            $"<div class='_update'>{update}</div>" +
                                                            $"<div class='_delete'>{delete}</div><hr class=\"_hr\"/>" +
                                                        "</div>" +
                                                        End_Report;
                                        }
                                        string icon = "fa fa-folder-o";
                                        if (MantID != "")
                                            icon = "fa fa-folder-open-o";

                                        Message_Open = "<div class=\"col-md-1 col-xs-12\" style=\"width: 3%; margin:0px 10px 0px 0px \">" +
                                                            "<u>" +
                                                                $" <a data-toggle=\"collapse\" data-parent=\"#accordion\"  href=\"#collapse{dt_select.Rows[i]["異常維護編號"].ToString()}\" >" +
                                                                    $" <i id=\"Open{dt_select.Rows[i]["異常維護編號"].ToString()}\" onclick=Click_Num('Open{dt_select.Rows[i]["異常維護編號"].ToString()}') class='{icon}'  style='color:black;width:3%;font-size: 1.6em;'>" +
                                                                     "</i>" +
                                                                 "</a>" +
                                                            "</u>" +
                                                        "</div>";
                                    }


                                    //父項目的檔案
                                    string video = "";
                                    file = "";
                                    file = Return_fileurl(DataTableUtils.toString(dt_select.Rows[i]["圖片檔名"]), out video);

                                    if (status == "")
                                        status = "   <div><a href='javascript:void(0)' onclick=Get_ErrorDetails('" + DataTableUtils.toString(dt_select.Rows[i]["異常維護編號"]) + "','" + ErrorID + "','" + type + "','" + DataTableUtils.toString(dt_select.Rows[i]["維護人員姓名"]) + "') data-toggle = \"modal\" data-target = \"#exampleModal\"><u>回覆</u></a></div>";
                                    else
                                        status = "   <div><a href='javascript:void(0)' onclick=alert('該回文已結案')><u>回覆</u></a></div>";

                                    update = "";
                                    delete = "";
                                    if (YN == "Y" || acc == DataTableUtils.toString(dt_select.Rows[i]["維護人員姓名"]))
                                    {
                                        update = "   <div><a href='javascript:void(0)' onclick=updates('up_" + DataTableUtils.toString(dt_select.Rows[i]["異常維護編號"]) + "','" + ErrorID + "','" + type + "') data-toggle = \"modal\" data-target = \"#exampleModal\"><u>編輯</u></a></div>";
                                        delete = "<a href='javascript:void(0)' onclick=deletes('" + DataTableUtils.toString(dt_select.Rows[i]["異常維護編號"]) + "','" + ErrorID + "','" + "" + "') ><u>刪除</u></a>";
                                    }
                                    else
                                    {
                                        update = "<div>編輯</div>";
                                        delete = "刪除";
                                    }



                                    //父項目內容(子項目收縮在裡面)
                                    td += $"<td style='text-align:left;max-width:55%'>{Message_Open} { br_text(DataTableUtils.toString(dt_select.Rows[i][j]).Replace(';', '\n'))} {file}  {video} " +
                                            $"<div id=\"_Middle\" class=\"col-md-12 col-xs-12 text-right\" style=\"height:30px;\">" +
                                                //回復
                                                $"<div class=\"_status\">{status}</div>" +
                                                //編輯
                                                $"<div class=\"_update\">{update}</div>" +
                                                //刪除
                                                $"<div class=\"_delete\" style=\"margin:0px 0px 0px 3px\">{delete}</div>" +
                                            $"</div>" +
                                            Message_count +
                                            $"<div id=\"collapse{dt_select.Rows[i]["異常維護編號"].ToString()}\" class=\"panel-collapse collapse {new_ID} \">" +
                                                $"<div class=\"panel-body\">" + Message +
                                                $"</div>" +
                                            $"</div>" +
                                     $"</td> \n";
                                }
                                else
                                    td += "<td style='text-align:left;max-width:60%'>" + DataTableUtils.toString(dt_select.Rows[i][j]) + "</td> \n";

                            }
                            td += "</tr> \n";
                        }
                    }
                    str[0] = td;
                    td = "";
                    //}
                    return str;
                }
                else
                    return str;
            }
            else
            {
                str[0] = "";
                return str;
            }
            //------------------------------------20200424新增-----------------------------------
            //-------------------------------------20200505-------------------------------------
        }
        //把文字分行
        public string br_text(string word)
        {
            string back = "";
            if (word.Trim() == "")
                return "無內容";
            else
            {
                List<string> list = new List<string>(word.Split('\n'));
                for (int i = 0; i < list.Count; i++)
                    back += list[i] + " <br/> ";

                return back;
            }
        }
        //pdf，excel，ppt，word用文字方式呈現
        public string Return_fileurl(string file_name, out string image_mp4, string height = "248")
        {
            string file = "";
            string Return_imge = "";
            if (file_name == "")
            {
                image_mp4 = Return_imge;
                return "";
            }
            else
            {
                string[] name_list = file_name.Split('\n');
                if (name_list.Length == 0)
                {
                    image_mp4 = Return_imge;
                    return "";
                }
                else
                {
                    for (int x = 0; x < name_list.Length - 1; x++)
                    {
                        int num = x + 1;
                        string[] sp = name_list[x].Split('.');
                        if (sp[sp.Length - 1].ToLower() == "xls" || sp[sp.Length - 1].ToLower() == "xlsx")
                            file += "<u><a href = '" + name_list[x] + "'  href=\"javascript: void()\" >EXCEL表" + num + "</a></u>  ";
                        else if (sp[sp.Length - 1].ToLower() == "pdf")
                            file += "<u><a href = '" + name_list[x] + "'  href=\"javascript: void()\" target='_blank' >PDF檔" + num + "</a></u>  ";
                        else if (sp[sp.Length - 1].ToLower() == "mp4")//判斷是影片或是圖片
                            Return_imge += Return_file(name_list[x], height, "mp4");
                        else
                            Return_imge += Return_file(name_list[x], height);
                    }
                    if (Return_imge != "")
                        image_mp4 = "<div class=\"col-md-12\">" + Return_imge + "</div>";
                    else
                        image_mp4 = "";

                    if (file != "")
                        return "(" + file + ")";
                    else
                        return file;
                }
            }
        }
        //回傳圖片跟影片的html碼
        public string Return_file(string file, string height, string type = "")
        {
            if (type.ToLower() == "mp4")
                file = "<a onclick=show_image('" + file + "','video')  data-toggle=\"modal\" data-target=\"#exampleModal_Image\"  href=\"javascript: void()\"><video  width=\"97%\" height=\"" + height + "px\" src=" + file + " controls=\"\"></video></a>";
            else
                //file = "<a href = '" + file + "'><img src=" + file + " alt=\"...\" width=\"97%\" height=\"248px\"></a>";
                file = "<a onclick=show_image('" + file + "','image') data-toggle=\"modal\" data-target=\"#exampleModal_Image\"  href=\"javascript: void()\"><img src=" + file + " alt=\"...\" width=\"97%\" height=\"" + height + "px\"></a>";
            return "<div class=\"col-md-4 col-xs-4 gradeX_Img\" style=\"margin-bottom:8px;padding:0\">" + file + "</div>";

        }
        //取得圓盤產線的狀態
        public DataTable Get_Status(DataTable dt)
        {
            DataTable ds = new DataTable();
            ds.Columns.Add("排程編號");
            foreach (DataRow row in dt.Rows)
            {
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
                string sqlcmd = $"select 狀態 from 工作站狀態資料表 where 排程編號 = '{DataTableUtils.toString(row["排程編號"])}'";
                DataTable dw = DataTableUtils.GetDataTable(sqlcmd);
                if (HtmlUtil.Check_DataTable(dw) && DataTableUtils.toString(dw.Rows[0]["狀態"]) != "完成")
                    ds.Rows.Add(DataTableUtils.toString(row["排程編號"]));
                else if (dw != null && dw.Rows.Count == 0)
                    ds.Rows.Add(DataTableUtils.toString(row["排程編號"]));
            }
            return ds;
        }
        //測試圓盤預計完成日
        /// <summary>
        /// ok=false->不計算開發機 ok=true->計算開發機
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ok"></param>
        /// <returns></returns>
        public DataTable Test_GetEndday(DataTable dt, bool ok = false)
        {
            DataTable ds = new DataTable();
            ds.Columns.Add("客戶機型");
            ds.Columns.Add("排程編號");
            ds.Columns.Add("預計開工日");
            ds.Columns.Add("預計完工日");
            string standard = "";
            string finish = "";
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
            string sqlcmd = "select * from 組裝工藝";
            DataTable dt_time = DataTableUtils.GetDataTable(sqlcmd);

            if (HtmlUtil.Check_DataTable(dt) && HtmlUtil.Check_DataTable(dt_time))
            {
                foreach (DataRow row in dt.Rows)
                {

                    sqlcmd = $"機種編號='{DataTableUtils.toString(row["客戶機型"])}'";
                    DataRow[] rew = dt_time.Select(sqlcmd);
                    if (rew != null && rew.Length > 0)
                        standard = DataTableUtils.toString(rew[0]["組裝時間"]);

                    finish = Test_calculation_finish(DataTableUtils.toString(row["預計開工日"]), standard);
                    if (ok)
                        ds.Rows.Add(DataTableUtils.toString(row["客戶機型"]), DataTableUtils.toString(row["排程編號"]), DataTableUtils.toString(row["預計開工日"]), finish);
                    else
                    {
                        if (standard != "" && DataTableUtils.toInt(finish) < DataTableUtils.toInt(DateTime.Now.ToString("yyyyMMdd")))
                            ds.Rows.Add(DataTableUtils.toString(row["客戶機型"]), DataTableUtils.toString(row["排程編號"]), DataTableUtils.toString(row["預計開工日"]), finish);
                    }
                    //為開發機，沒有標準工時
                }
            }
            return ds;
        }
        public string Test_calculation_finish(string date, string worktime, bool ok = false)
        {
            if (ok)
                dt_work(ok);

            if (date != "")
                date = date + "080000";

            int standard_worktime = DataTableUtils.toInt(worktime);
            work.工作時段_新增(8, 0, 12, 0);
            work.工作時段_新增(13, 0, 17, 0);
            if (date != "")
            {
                DateTime stand_endtime = work.目標日期(StrToDate(date), new TimeSpan(0, 0, standard_worktime));
                return stand_endtime.ToString("yyyyMMdd");
            }
            else
                return "";
        }
        //得到圓盤產線的預計完工日
        public DataTable Get_FinishDay(DataTable dt, string start, string end)
        {
            string sqlcmd = "";
            string standard = "";

            DataTable dr = new DataTable();
            dr.Columns.Add("預計開工日");
            dr.Columns.Add("預定生產數量");
            dr.Columns.Add("排程編號");
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
            sqlcmd = "select * from 組裝工藝";
            DataTable dt_time = DataTableUtils.GetDataTable(sqlcmd);
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
            sqlcmd = "select SUBSTRING(實際完成時間, 1, 8) as 完成日,排程編號 from 工作站狀態資料表 where 工作站編號 = 11";
            DataTable dt_schdule = DataTableUtils.GetDataTable(sqlcmd);
            DataRow[] rew = null;
            DataRow[] rsw = null;

            if (HtmlUtil.Check_DataTable(dt) && HtmlUtil.Check_DataTable(dt_time))
            {
                foreach (DataRow row in dt.Rows)
                {

                    sqlcmd = $"機種編號 ='{DataTableUtils.toString(row["客戶機型"])}'";
                    rsw = dt_time.Select(sqlcmd);
                    if (rsw != null && rsw.Length > 0)
                        standard = DataTableUtils.toString(rsw[0]["組裝時間"]);
                    string time = calculation_finish(DataTableUtils.toString(row["預計開工日"]), standard, start, end);

                    //GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
                    //sqlcmd = $"select SUBSTRING(實際完成時間, 1, 8) as 完成日 from 工作站狀態資料表 where 排程編號 = '{DataTableUtils.toString(row["排程編號"])}' and SUBSTRING(實際完成時間, 1, 8)<'{start}' and (SUBSTRING(實際完成時間, 1, 8) is not null and Len(SUBSTRING(實際完成時間, 1, 8))>0)";
                    //rew = DataTableUtils.DataTable_GetDataRow(sqlcmd);
                    sqlcmd = $"排程編號 = '{DataTableUtils.toString(row["排程編號"])}' and 完成日<'{start}' and 完成日 is not null and Len(完成日)>0";
                    rew = dt_schdule.Select(sqlcmd);

                    if (time != "" && rew != null && rew.Length == 0)
                        dr.Rows.Add(time, DataTableUtils.toString(row["預定生產數量"]), DataTableUtils.toString(row["排程編號"]));
                }
            }

            DataTable return_dt = new DataTable();
            return_dt.Columns.Add("日期");
            return_dt.Columns.Add("預定生產數量");

            if (HtmlUtil.Check_DataTable(dr))
            {
                DataTable Line = dr.DefaultView.ToTable(true, new string[] { "預計開工日" });
                DataView dv_mant = new DataView(Line);
                dv_mant.Sort = "預計開工日 asc";
                Line = dv_mant.ToTable();

                int count = 0;
                foreach (DataRow row in Line.Rows)
                {
                    count = 0;
                    sqlcmd = $"預計開工日 = '{DataTableUtils.toString(row["預計開工日"])}' ";
                    DataRow[] rows = dr.Select(sqlcmd);

                    if (rows.Length > 0)
                    {
                        for (int i = 0; i < rows.Length; i++)
                            count += DataTableUtils.toInt(DataTableUtils.toString(rows[i]["預定生產數量"]));
                    }
                    return_dt.Rows.Add(DataTableUtils.toString(row["預計開工日"]), count);
                }
            }
            return return_dt;
        }
        //確認該排程是否已經完成
        public static DataTable Check_Schdule(DataTable dt)
        {
            DataTable dt_return = new DataTable();
            dt_return.Columns.Add("客戶簡稱");
            dt_return.Columns.Add("產線代號");
            dt_return.Columns.Add("小計");
            DataTable ds = dt.Clone();
            if (HtmlUtil.Check_DataTable(dt))
            {
                foreach (DataRow row in dt.Rows)
                {
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
                    string sqlcmd = $"select 狀態,實際完成時間 from 工作站狀態資料表 where 排程編號 = '{row["排程編號"]}' and 工作站編號={row["產線代號"]} ";
                    DataTable dt_status = DataTableUtils.GetDataTable(sqlcmd);
                    if (HtmlUtil.Check_DataTable(dt_status) && DataTableUtils.toString(dt_status.Rows[0]["狀態"]) != "完成" && DataTableUtils.toString(dt_status.Rows[0]["實際完成時間"]) == "")
                        ds.ImportRow(row);
                }
                DataTable dt_Cust = ds.DefaultView.ToTable(true, new string[] { "客戶簡稱" });
                foreach (DataRow row in dt_Cust.Rows)
                {
                    string sqlcmd = $"客戶簡稱 = '{row["客戶簡稱"]}'";
                    DataRow[] rows = ds.Select(sqlcmd);
                    if (rows != null && rows.Length > 0)
                        dt_return.Rows.Add(DataTableUtils.toString(row["客戶簡稱"]), DataTableUtils.toString(rows[0]["產線代號"]), rows.Length.ToString());
                }
            }
            return dt_return;
        }
        //計算圓盤產線預計完成日
        public string calculation_finish(string date, string work_time, string start = "", string end = "", bool cal_nofinish = false)
        {

            if (date != "")
                date = date + "080000";

            int standard_worktime = DataTableUtils.toInt(work_time);
            work.工作時段_新增(8, 0, 12, 0);
            work.工作時段_新增(13, 0, 17, 0);
            if (date != "")
            {
                DateTime stand_endtime = work.目標日期(StrToDate(date), new TimeSpan(0, 0, standard_worktime));
                if (!cal_nofinish)
                {
                    if (DataTableUtils.toInt(stand_endtime.ToString("yyyyMMdd")) >= DataTableUtils.toInt(start) && DataTableUtils.toInt(stand_endtime.ToString("yyyyMMdd")) <= DataTableUtils.toInt(end))
                        return stand_endtime.ToString("yyyyMMdd");
                    else if (start == "" && end == "")
                        return stand_endtime.ToString("yyyyMMdd");
                    else
                        return "";
                }
                else if (cal_nofinish && DataTableUtils.toDouble(DataTableUtils.toString(stand_endtime.ToString("yyyyMMdd"))) < DataTableUtils.toDouble(DataTableUtils.toString(end)))
                    return stand_endtime.ToString("yyyyMMddHHmmss");
                else
                    return "";
            }
            else
                return "";

        }
        //計算圓盤加上工時後上個月未完工數量
        public int Get_NoFinish(string st_day)
        {
            //先得到目前為止，圓盤未完成之數量
            GlobalVar.UseDB_setConnString(GetConnByDekERPDataTable);
            string sqlcmd = $"SELECT " +
                            $"item.item_no_bom AS 客戶機型,  " +
                            "       ws.lot_no as 排程編號, " +
                            $"DATE_FORMAT(mkordsub.str_date, '%Y%m%d')  AS 預計開工日 " +
                            $"FROM  " +
                            $"ws " +
                            $"RIGHT JOIN mkordsub ON (  ws.cord_no=mkordsub.cord_no  AND  ws.cord_sn=mkordsub.cord_sn   AND  ( ws.lot_no=mkordsub.lot_no || ws.lot_no IS NULL || ws.lot_no='')) " +
                            $"LEFT JOIN  item ON  ( mkordsub.item_no=item.item_no) " +
                            //$"LEFT JOIN  cord ON ( ws.cord_no=cord.trn_no) " +
                            //$"LEFT JOIN  cordsub ON ( ws.cord_no=cordsub.trn_no AND ws.cord_sn=cordsub.sn ) " +
                            //$"LEFT JOIN  invosub ON ( ws.cord_no=invosub.cord_no AND ws.cord_sn=invosub.cord_sn AND    ws.lot_no=invosub.lot_no) " +
                            //$"LEFT JOIN  citem ON  ( citem.cust_no=mkordsub.cust_no AND citem.item_no=mkordsub.item_no) " +
                            //$"LEFT JOIN  mkord ON ( mkord.trn_no=mkordsub.trn_no) " +
                            //$"LEFT JOIN  cust ON ( cust.cust_no=mkord.cust_no) " +
                            $"WHERE " +
                             //$"1=1 and " +
                             //$"ws.lot_no is not null and  length(ws.lot_no)>0 and " +
                             $"length(ws.lot_no)>0 and mkordsub.fclose  IS NULL and " +
                            $"DATE_FORMAT(mkordsub.str_date, '%Y%m%d') <= {st_day} and " +
                            $"DATE_FORMAT(mkordsub.str_date, '%Y%m%d') >= 20200901 " +
                            // $"and  cordsub.sclose != '結案' " +
                            $"ORDER BY    mkordsub.str_date asc";
            DataTable dt_圓盤 = DataTableUtils.GetDataTable(sqlcmd);
            dt_圓盤 = Test_GetEndday(dt_圓盤, true);

            if (HtmlUtil.Check_DataTable(dt_圓盤))
            {
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
                sqlcmd = "select 排程編號,實際完成時間 from 工作站狀態資料表 where 工作站編號 = 11";
                DataTable dt_狀態表 = DataTableUtils.DataTable_GetTable(sqlcmd);

                dt_圓盤.PrimaryKey = new DataColumn[] { dt_圓盤.Columns["排程編號"] };
                dt_狀態表.PrimaryKey = new DataColumn[] { dt_狀態表.Columns["排程編號"] };

                dt_圓盤.Merge(dt_狀態表, false, MissingSchemaAction.Add);
                //完成時間在上個月之前，預計完工日在起始時間之後
                DataRow[] rows = dt_圓盤.Select($"預計開工日 IS null OR (實際完成時間 <= '{st_day}000000' AND 實際完成時間 IS NOT NULL) OR  預計完工日 >= {st_day}");
                for (int i = 0; i < rows.Length; i++)
                {
                    if (DataTableUtils.toString(rows[i]["實際完成時間"]).Length != 0 ||
                        DataTableUtils.toString(rows[i]["預計開工日"]) == "" ||
                        DataTableUtils.toInt(DataTableUtils.toString(rows[i]["預計完工日"])) >= DataTableUtils.toInt(st_day))
                        rows[i].Delete();
                }
                return dt_圓盤.Rows.Count;
            }
            else
                return 0;
        }
        //圓盤線未完工表格
        public DataTable Get_NoFinishDataTable(DataTable dt, string start, string end)
        {
            DataTable dr = new DataTable();
            dr.Columns.Add("客戶簡稱");
            dr.Columns.Add("產線代號");
            dr.Columns.Add("排程編號");
            string standard = "";
            string sqlcmd = "";
            if (HtmlUtil.Check_DataTable(dt))
            {
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
                sqlcmd = $"select * from 組裝工藝  ";
                DataTable dt_time = DataTableUtils.GetDataTable(sqlcmd);

                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
                sqlcmd = $"select * from 工作站狀態資料表 where 工作站編號 = 11 and 狀態 <> '完成'";
                DataTable dt_status = DataTableUtils.GetDataTable(sqlcmd);

                foreach (DataRow row in dt.Rows)
                {

                    DataRow[] rew = dt_time.Select($"機種編號 ='{DataTableUtils.toString(row["客戶機型"])}'");
                    if (rew != null && rew.Length > 0)
                        standard = DataTableUtils.toString(rew[0]["組裝時間"]);

                    string time = calculation_finish(DataTableUtils.toString(row["預計開工日"]), standard, start, end);

                    DataRow[] rsw = dt_status.Select($"排程編號 = '{DataTableUtils.toString(row["製造批號"])}'");

                    string status = "";
                    if (rsw != null && rsw.Length > 0)
                        status = DataTableUtils.toString(rsw[0]["狀態"]);

                    if (time != "" && status != "完成")
                        dr.Rows.Add(DataTableUtils.toString(row["客戶簡稱"]), DataTableUtils.toString(row["產線代號"]), DataTableUtils.toString(row["製造批號"]));
                }

                DataTable ds = new DataTable();
                ds.Columns.Add("客戶簡稱");
                ds.Columns.Add("產線代號");
                ds.Columns.Add("小計");

                DataTable custom = dt.DefaultView.ToTable(true, new string[] { "客戶簡稱" });
                foreach (DataRow row in custom.Rows)
                {
                    sqlcmd = $"客戶簡稱 = '{DataTableUtils.toString(row["客戶簡稱"])}'";
                    DataRow[] rows = dr.Select(sqlcmd);
                    if (rows.Length != 0)
                        ds.Rows.Add(DataTableUtils.toString(rows[0]["客戶簡稱"]), DataTableUtils.toString(rows[0]["產線代號"]), DataTableUtils.toString(rows.Length));

                }
                return ds;
            }
            else
                return null;
        }
        //圓盤線細節列表
        public DataTable Get_DetailsDataTable(DataTable dt, string start, string end)
        {
            string sqlcmd = "";
            string standard = "";
            if (HtmlUtil.Check_DataTable(dt))
            {
                DataTable dr = new DataTable();
                dr.Columns.Add("客戶簡稱");
                dr.Columns.Add("產線代號");
                dr.Columns.Add("製造批號");
                dr.Columns.Add("訂單號碼");
                dr.Columns.Add("客戶訂單");
                dr.Columns.Add("品號");
                dr.Columns.Add("訂單日期");
                dr.Columns.Add("預計開工日");
                dr.Columns.Add("組裝日");

                foreach (DataRow row in dt.Rows)
                {

                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
                    //   sqlcmd = $"select 組裝時間,狀態 from 工作站狀態資料表 left join 組裝工藝 on 組裝工藝.機種編號 = SUBSTRING(排程編號, 1, CHARINDEX('-', 排程編號) - 1) where 工作站編號 = '11' and 排程編號 = '{DataTableUtils.toString(row["製造批號"])}' and 機種編號 ='{DataTableUtils.toString(row["客戶機型"])}'";
                    sqlcmd = $"select * from 組裝工藝 where 機種編號 ='{DataTableUtils.toString(row["客戶機型"])}' ";
                    DataRow rew = DataTableUtils.DataTable_GetDataRow(sqlcmd);
                    if (rew != null)
                        standard = DataTableUtils.toString(rew["組裝時間"]);
                    string time = calculation_finish(DataTableUtils.toString(row["預計開工日"]), standard, start, end);

                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
                    sqlcmd = $"select * from 工作站狀態資料表 where 工作站編號 = '11' and 排程編號 = '{DataTableUtils.toString(row["製造批號"])}'";
                    rew = DataTableUtils.DataTable_GetDataRow(sqlcmd);
                    string status = "";
                    if (rew != null)
                        status = DataTableUtils.toString(rew["狀態"]);

                    if (time != "" && status != "完成")
                        dr.Rows.Add(DataTableUtils.toString(row["客戶簡稱"]),
                            DataTableUtils.toString(row["產線代號"]),
                            DataTableUtils.toString(row["製造批號"]),
                            DataTableUtils.toString(row["訂單號碼"]),
                            DataTableUtils.toString(row["客戶訂單"]),
                            DataTableUtils.toString(row["品號"]),
                            DataTableUtils.toString(row["訂單日期"]),
                            DataTableUtils.toString(row["預計開工日"]),
                            DataTableUtils.toString(row["組裝日"]));
                }

                return dr;


            }
            else
                return null;
        }
        //大圓盤計算完預計完成時間+狀態、進度、預計完工日(Ver.2用)
        public DataTable Get_dekInformation(DataTable dt, string start, string end, int type)
        {
            //先取得工時
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
            string sqlcmd = "select 機種編號,組裝時間 from 組裝工藝";
            DataTable dt_工時 = DataTableUtils.GetDataTable(sqlcmd);
            //取得進度 狀態 完成時間
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
            sqlcmd = "select 排程編號,進度,狀態,實際完成時間,組裝日 from 工作站狀態資料表 where 工作站編號 = 11";
            DataTable dt_組裝資訊 = DataTableUtils.GetDataTable(sqlcmd);

            DataRow[] rows;
            if (!HtmlUtil.Check_DataTable(dt))
                return null;
            else
            {
                dt.Columns.Add("預計完工日", typeof(string));
                dt.Columns.Add("進度", typeof(int));
                dt.Columns.Add("狀態", typeof(string));
                dt.Columns.Add("實際完成時間", typeof(string));
                dt.Columns.Add("組裝日", typeof(string));
                foreach (DataRow row in dt.Rows)
                {
                    //加入預計完工日
                    sqlcmd = $"機種編號='{DataTableUtils.toString(row["客戶機型"])}'";
                    if (HtmlUtil.Check_DataTable(dt_工時))
                    {
                        rows = dt_工時.Select(sqlcmd);
                        if (rows.Length > 0)
                            row["預計完工日"] = Test_calculation_finish(DataTableUtils.toString(row["預計開工日"]), DataTableUtils.toString(rows[0]["組裝時間"]));
                        else
                            row["預計完工日"] = "開發機";
                    }
                    else
                        row["預計完工日"] = "開發機";

                    //加入進度、狀態、實際完成時間
                    sqlcmd = $"排程編號='{DataTableUtils.toString(row["排程編號"])}'";
                    if (HtmlUtil.Check_DataTable(dt_組裝資訊))
                    {
                        rows = dt_組裝資訊.Select(sqlcmd);
                        if (rows.Length > 0)
                        {
                            row["進度"] = DataTableUtils.toInt(DataTableUtils.toString(rows[0]["進度"]));
                            row["狀態"] = DataTableUtils.toString(rows[0]["狀態"]);
                            row["實際完成時間"] = DataTableUtils.toString(rows[0]["實際完成時間"]);
                            row["組裝日"] = DataTableUtils.toString(rows[0]["組裝日"]);
                        }
                    }
                }
                //只留下本月應做的
                if (type == 1)
                    sqlcmd = $"預計完工日<{start} OR 預計完工日>{end}";
                //留下上個月應做完，但是在這個月做完
                else if (type == 2)
                    sqlcmd = $"(預計完工日>='{start}' and 預計完工日 <> '開發機') OR 實際完成時間<'{start}000000'";
                //到目前完止皆未完成
                else if (type == 3)
                    sqlcmd = $"(預計完工日>='{start}' and 預計完工日 <> '開發機')  OR 狀態='完成'";

                rows = dt.Select(sqlcmd);
                for (int i = 0; i < rows.Length; i++)
                    dt.Rows.Remove(rows[i]);

                //除去非必要欄位
                DataView dv = new DataView(dt);
                dv.Sort = "預計完工日 asc";
                dt = dv.ToTable(true, "客戶簡稱", "產線代號", "排程編號", "預計開工日", "預計完工日", "進度", "狀態", "實際完成時間", "組裝日", "訂單號碼", "客戶訂單", "品號", "訂單日期");
                return dt;
            }
        }
        public string[] GetTagetPiece(int LineNum)
        {
            string[] str = new string[3];//0:piece 1:LineName
            DataTableUtils.Conn_String = GetConnByDekVisTmp;
            string StrCmd = "工作站編號=" + LineNum;
            DataRow dr_taget = DataTableUtils.DataTable_GetDataRow(ShareMemory.SQLAsm_WorkStation_Type, StrCmd);


            if (dr_taget != null)
            {
                str[0] = dr_taget["目標件數"].ToString();
                str[1] = dr_taget["工作站名稱"].ToString();
                str[2] = dr_taget["人數配置"].ToString();
                return str;
            }
            else
            {
                str[0] = "Null";
                return str;
            }
        }
        public string[] GetOverViewData(int LineNum, string LineName, string Acc, string _reqType, ref int total, ref int alarm_total, string judge = "")
        {
            string td = "";
            string[] str = new string[7] { "0", "0", "0", "0", "0", "0", "0" };//0:all   1:finsh   2:Stop  3:all   4:td_finish   5:td_Stop  7:Data
            string[] ErrorStr;
            string color = "";
            string SubError = "";
            string Condition = "";
            string Condition1 = "";
            string Condition2 = "";
            string Condition3 = "";
            string PredictionTimeStatus = "";
            List<double> percent = new List<double>();
            int nosolved = 0;
            List<string> list = new List<string>();
            List<string> alarm_list = new List<string>();
            int count = 0;
            int PredictionProgress = 0;
            DataTableUtils.Conn_String = GetConnByDekVisTmp;
            if (_reqType == "Line")
            {
                Condition1 = $"工作站編號 = '{LineNum}' AND 實際組裝時間 ='{DateTime.Now:yyyyMMdd}' ";
                Condition2 = $"工作站編號 = '{LineNum}' AND 實際組裝時間 <='{DateTime.Now:yyyyMMdd}' AND 狀態!='完成' ";
                Condition3 = $"工作站編號 = '{LineNum}' AND 實際完成時間 <='{DateTime.Now:yyyyMMdd}235959' AND 實際完成時間 >='{DateTime.Now:yyyyMMdd}010101' ";
                Condition = $" {Condition1} OR {Condition2} OR {Condition3} order by 組裝日,組裝編號 ";
            }
            else
            {
                Condition1 = $"工作站編號 = '{LineNum}' AND 實際組裝時間 ='{DateTime.Now:yyyyMMdd}' AND 狀態='暫停' ";
                Condition2 = $"工作站編號 = '{LineNum}' AND 實際組裝時間 <='{DateTime.Now:yyyyMMdd}' AND 狀態='暫停' ";
                Condition3 = $"工作站編號 = '{LineNum}' AND 實際完成時間 <='{DateTime.Now:yyyyMMdd}235959' AND 實際完成時間 >='{DateTime.Now:yyyyMMdd}010101'";
                Condition = $" ({Condition1}) OR ({Condition2}) order by 組裝日,組裝編號 ";
            }
            DataTable dt = null;

            int x = 0, y = 0;
            alarm_list = Calculation_Alarm_Or_Behind(LineNum.ToString(), ref x, ref y, true);

            if (_reqType == "Line")
                dt = DataTableUtils.DataTable_GetTable(ShareMemory.SQLAsm_WorkStation_State, Condition);
            else
            {
                DataTableUtils.Conn_String = GetConnByDekVisTmp;
                for (int a = 0; a < alarm_list.Count; a++)
                {
                    if (a == 0)
                        Condition = $"工作站編號 = '{LineNum}' and 狀態 <> '完成' and (排程編號 = '{alarm_list[a]}'";
                    else
                        Condition += $" OR 排程編號 = '{alarm_list[a]}'";
                }
                Condition += ")";
                dt = DataTableUtils.DataTable_GetTable(ShareMemory.SQLAsm_WorkStation_State, Condition);
            }

            if (HtmlUtil.Check_DataTable(dt))
            {

                DataTable dt_select = tableColumnSelectForLineDetail(dt, LineNum.ToString());
                str = GetEachPiece(dt);

                if (HtmlUtil.Check_DataTable(dt_select))
                {

                    for (int i = 0; i < dt_select.Rows.Count; i++)
                    {
                        td += "<tr class='gradeX'> \n";
                        for (int j = 0; j < dt_select.Columns.Count; j++)
                        {
                            if (dt_select.Columns[j].ColumnName == "狀態")
                            {
                                if (dt.Rows[i]["狀態"].ToString() == "暫停")
                                    td += $"<td style='text-align:center;width:10%;vertical-align: middle'><a href='javascript:void(0)' onclick=SetValue('{dt.Rows[i]["排程編號"]}','{dt.Rows[i]["進度"]}%','{dt.Rows[i]["問題回報"]}') data-toggle='modal' data-target='#exampleModal'> <span style=color:red><u>{dt.Rows[i]["進度"]}%</u></span></a></td> \n";
                                else if (dt.Rows[i]["狀態"].ToString() == "完成")
                                    td += $"<td style='text-align:center;width:10%;vertical-align: middle'><a href='javascript:void(0)' onclick=SetValue('{dt.Rows[i]["排程編號"]}','{dt.Rows[i]["進度"]}%','{dt.Rows[i]["問題回報"]}') data-toggle='modal' data-target='#exampleModal'><span style=color:green><u>{dt.Rows[i]["進度"]}%</u></span></a></td> \n";
                                else if (dt.Rows[i]["狀態"].ToString() == "啟動")
                                    td += $"<td style='text-align:center;width:10%;vertical-align: middle'><a href='javascript:void(0)' onclick=SetValue('{dt.Rows[i]["排程編號"]}','{dt.Rows[i]["進度"]}%','{dt.Rows[i]["問題回報"]}') data-toggle='modal' data-target='#exampleModal'><span style=color:blue><u>{dt.Rows[i]["進度"]}%</u></span></a></td> \n";
                                else
                                    td += $"<td style='text-align:center;width:10%;vertical-align: middle'><a href='javascript:void(0)' onclick=SetValue('{dt.Rows[i]["排程編號"]}','{dt.Rows[i]["進度"]}%','{dt.Rows[i]["問題回報"]}') data-toggle='modal' data-target='#exampleModal'><span style=color:black><u>0%</u></span></a></td> \n";

                                color = "";

                                //預定進度
                                try
                                {
                                    if (DataTableUtils.toDouble(DataTableUtils.toString(dt_select.Rows[i]["組裝日"])) < DataTableUtils.toDouble(DateTime.Now.ToString("yyyyMMdd")) && DataTableUtils.toString(dt_select.Rows[i]["實際啟動時間"]) == "" && DataTableUtils.toString(dt_select.Rows[i]["異常"]) == "")
                                        color = "red";

                                    string no_mean = "";
                                    percent = percent_calculation(DataTableUtils.toString(dt_select.Rows[i]["排程編號"]), DataTableUtils.toString(dt_select.Rows[i]["進度"]), ref no_mean, LineNum.ToString());

                                    bool delay = false;
                                    string target = Comparison_Schedule(percent[0], percent[1], ref count, ref delay);
                                    if (delay)
                                        color = "red";
                                    td += $"<td style='text-align:center;width:10%;vertical-align: middle'><b style='color:{color}'>{target}</b> </td> \n";
                                }
                                catch
                                {
                                    if (DataTableUtils.toDouble(DataTableUtils.toString(dt_select.Rows[i]["組裝日"])) < DataTableUtils.toDouble(DateTime.Now.ToString("yyyyMMdd")) && DataTableUtils.toString(dt_select.Rows[i]["實際啟動時間"]) == "" && DataTableUtils.toString(dt_select.Rows[i]["異常"]) == "")
                                        color = "red";

                                    td += $"<td style='text-align:center;width:10%;vertical-align: middle' ><b style='color:{color}'>0%</b> </td> \n";
                                }

                            }
                            else if (dt_select.Columns[j].ColumnName == "進度")
                            {

                            }
                            else if (dt_select.Columns[j].ColumnName == "問題回報")
                            {

                            }
                            else if (dt_select.Columns[j].ColumnName == "實際啟動時間")
                            {
                            }
                            else if (dt_select.Columns[j].ColumnName == "組裝日")
                                td += $"<td style='text-align:center;width:10%;vertical-align: middle'>{DataTableUtils.toString(dt_select.Rows[i][j].ToString().Substring(4, 2))}/{DataTableUtils.toString(dt_select.Rows[i][j].ToString().Substring(6, 2))}</td> \n";
                            else if (dt_select.Columns[j].ColumnName == "異常")//<a href="#">
                            {
                                if (dt_select.Rows[i][j].ToString() != "") //  
                                {
                                    SubError = "";
                                    ErrorStr = DataTableUtils.toString(dt_select.Rows[i][j]).Split(',');
                                    for (int k = 1; k < ErrorStr.Length - 2; k++)
                                        SubError += " " + ErrorStr[k];
                                    list = check_case(DataTableUtils.toString(dt_select.Rows[i]["排程編號"]), GetConnByDekVisTmp, LineNum.ToString(), ref nosolved);
                                    // content
                                    string url = $"ErrorID={DataTableUtils.toString(dt_select.Rows[i]["排程編號"])},ErrorLineNum={dt.Rows[0]["工作站編號"]},ErrorLineName={LineName}";
                                    if (ConfigurationManager.AppSettings["show_function"] == "1")
                                        td += $"<td style='text-align:center;width:48%;vertical-align: middle'>" +
                                                  $"<a onclick=jump_Asm_ErrorDetail('{WebUtils.UrlStringEncode(url)}')  href=\"javascript: void()\">" +
                                                      $"<div  style='height:100%;width:100%;font-size:20px;vertical-align: middle'>{list[0]} \t " +
                                                        $"<u> " +
                                                            $"[{list[1]}] " +
                                                        $"</u>" +
                                                      $"</div>" +
                                                  $"</a>" +
                                              $"</td> \n";
                                    else
                                        td += $"<td style='text-align:center;width:48%'><a onclick=jump_Asm_ErrorDetail('{WebUtils.UrlStringEncode(url)}')  href=\"javascript: void()\"><div style='height:100%;width:100%'>異常歷程</div></a></td> \n";

                                }
                                else
                                {
                                    list = check_case(DataTableUtils.toString(dt_select.Rows[i]["排程編號"]), GetConnByDekVisTmp, LineNum.ToString(), ref nosolved);
                                    string url = $"ErrorID={DataTableUtils.toString(dt_select.Rows[i]["排程編號"])},ErrorLineNum={dt.Rows[0]["工作站編號"]},ErrorLineName={LineName}";
                                    if (ConfigurationManager.AppSettings["show_function"] == "1")
                                        td += $"<td style='text-align:center;width:48%;vertical-align: middle'><a onclick=jump_Asm_ErrorDetail('{WebUtils.UrlStringEncode(url)}')  href=\"javascript: void()\"><div style='height:100%;width:100%;font-size:20px;vertical-align: middle'>{list[0]} \t <u> [{list[1]}]</u></div></a></td> \n";
                                    else
                                        td += $"<td style='text-align:center;width:48%'><u><a onclick=jump_Asm_ErrorDetail('{WebUtils.UrlStringEncode(url)}')  href=\"javascript: void()\"><div style='height:100%;width:100%'>" + "編輯" + "</div></a></u></td> \n";
                                }
                            }
                            else if (dt_select.Columns[j].ColumnName == "排程編號")
                            {
                                /*20200221修改這裡*/
                                //圖片
                                string image = Search_Image(DataTableUtils.toString(dt_select.Rows[i]["排程編號"]), GetConnByDekVisTmp);
                                if (image != "")
                                    image = $"<img src='{image}' alt='...' width='200px' height='200px'>";
                                //啟動時間
                                string start_time = "";
                                if (DataTableUtils.toString(dt_select.Rows[i]["實際啟動時間"]) != "")
                                    start_time = $"實際啟動時間： <br/> {StrToDate(DataTableUtils.toString(dt_select.Rows[i]["實際啟動時間"]))} <br/> ";
                                //預計完成時間
                                string finish_time = "";
                                percent_calculation(dt_select.Rows[i]["排程編號"].ToString(), "0", ref finish_time, LineNum.ToString(), true);
                                if (finish_time != "")
                                    finish_time = $"預計完成時間： <br/> {finish_time}  <br/> ";

                                GlobalVar.UseDB_setConnString(GetConnByDekVisTmp);
                                string sqlcmd = "";
                                if (GetConnByDekVisTmp.Contains("Hor"))
                                    sqlcmd = $"select sales,trn_date,d_date,designer,組裝者,CCS  from 組裝資料表,工作站狀態資料表 where 組裝資料表.排程編號 = 工作站狀態資料表.排程編號 and  工作站狀態資料表.排程編號 = '{DataTableUtils.toString(dt_select.Rows[i]["排程編號"])}' and CCS IS NOT NULL and Len(CCS) >0";
                                else
                                    sqlcmd = $"select sales,trn_date,d_date,designer,組裝者,item_no CCS from 組裝資料表,工作站狀態資料表 where 組裝資料表.排程編號 = 工作站狀態資料表.排程編號 and  工作站狀態資料表.排程編號 = '{DataTableUtils.toString(dt_select.Rows[i]["排程編號"])}' and item_no IS NOT NULL and Len(item_no) >0";
                                DataTable dt_imformation = DataTableUtils.GetDataTable(sqlcmd);
                                string ss = DataTableUtils.ErrorMessage;


                                //取得設計者
                                string designer = HtmlUtil.Check_DataTable(dt_imformation) && dt_imformation.Rows[0]["designer"].ToString() != "" ? $"  設計者:{dt_imformation.Rows[0]["designer"]} <br/> " : "";
                                //取得組裝者
                                string worker = HtmlUtil.Check_DataTable(dt_imformation) && dt_imformation.Rows[0]["組裝者"].ToString() != "" ? $"  組裝者:{dt_imformation.Rows[0]["組裝者"]} <br/>" : "";
                                //取得業務員
                                string saler = HtmlUtil.Check_DataTable(dt_imformation) && dt_imformation.Rows[0]["sales"].ToString() != "" ? $"  業務員:{dt_imformation.Rows[0]["sales"]} <br/>" : "";
                                //取得下單日
                                string trn_date = HtmlUtil.Check_DataTable(dt_imformation) && dt_imformation.Rows[0]["trn_date"].ToString() != "" ? $" 下單日:{HtmlUtil.changetimeformat(dt_imformation.Rows[0]["trn_date"].ToString())} <br/>" : "";
                                //取得預交日
                                string d_date = HtmlUtil.Check_DataTable(dt_imformation) && dt_imformation.Rows[0]["d_date"].ToString() != "" ? $" 預交日:{HtmlUtil.changetimeformat(dt_imformation.Rows[0]["d_date"].ToString())} <br/>" : "";
                                //取得ccs
                                string ccs = HtmlUtil.Check_DataTable(dt_imformation) && dt_imformation.Rows[0]["ccs"].ToString() != "" ? $"  CCS: <br/> {HtmlUtil.changetimeformat(dt_imformation.Rows[0]["CCS"].ToString())} <br/>" : "";

                                //tooltip
                                string tooltip = "";
                                if (image != "" || start_time != "" || finish_time != "" || designer != "" || worker != "" || saler != "" || trn_date != "" || d_date != "" || ccs != "")
                                    tooltip = $"data-toggle=\"tooltip\" data-html=\"true\" data-placement=\"left\"  data-html=\"true\" title=\"\" data-original-title=\" {start_time} {finish_time}  {designer} {worker} {saler} {trn_date} {d_date} {image} {ccs} \"";

                                if (judge == "")
                                    td += $"<td style='text-align:center;width:12%;vertical-align: middle' {tooltip} >{DataTableUtils.toString(dt_select.Rows[i][j])}</td> \n";

                                else
                                    td += $"<td style='text-align:center;width:12%;vertical-align: middle' {tooltip} >{DataTableUtils.toString(dt_select.Rows[i][j])}</td> \n";
                            }
                            else
                                td += $"<td style='text-align:center;width:10%;vertical-align: middle'>{DataTableUtils.toString(dt_select.Rows[i][j])}</td> \n";
                        }
                        td += "</tr> \n";
                    }
                    str[6] = td;
                    total = count;
                    alarm_total = alarm_list.Count;
                    return str;
                }
                else
                {
                    alarm_total = alarm_list.Count;
                    total = count;
                    return str;
                }
            }
            else
            {
                total = count;
                alarm_total = alarm_list.Count;
                str[6] = " no data";
                return str;
            }
        }
        //取得異常頁面
        public DataTable GetNosovle(List<string> list)
        {
            if (list.Count <= 1)
                return null;
            else
            {
                DataTable dt = new DataTable();
                List<string> copylist = new List<string>();
                for (int i = 0; i < list.Count - 1; i++)
                {
                    if (i % 2 == 0)
                        copylist.Add(list[i]);
                }
                copylist = copylist.Distinct().ToList();

                dt.Columns.Add("上線日");
                dt.Columns.Add("產線");
                dt.Columns.Add("客戶");
                dt.Columns.Add("編號");
                dt.Columns.Add("進度");
                dt.Columns.Add("預定進度");
                dt.Columns.Add("未解決數量");

                for (int i = 0; i < copylist.Count; i++)
                {
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
                    string sqlcmd = $"select  組裝日 as 上線日,工作站名稱 as 產線名稱,CUSTNM as 客戶, 工作站狀態資料表.排程編號 as 排程編號,進度 ,實際啟動時間 as 預定進度 " +
                                    $" from 工作站狀態資料表 " +
                                    $" left join 組裝資料表 on 組裝資料表.排程編號 = 工作站狀態資料表.排程編號 " +
                                    $" left join 工作站型態資料表 on 工作站狀態資料表.工作站編號 = 工作站型態資料表.工作站編號 " +
                                    $" where 工作站狀態資料表.排程編號 = '{copylist[i]}'";
                    DataTable dt_detail = DataTableUtils.GetDataTable(sqlcmd);

                    if (HtmlUtil.Check_DataTable(dt_detail))
                        dt.Rows.Add(DataTableUtils.toString(dt_detail.Rows[0]["上線日"]),
                                    DataTableUtils.toString(dt_detail.Rows[0]["產線名稱"]),
                                    DataTableUtils.toString(dt_detail.Rows[0]["客戶"]),
                                    DataTableUtils.toString(dt_detail.Rows[0]["排程編號"]),
                                    DataTableUtils.toString(dt_detail.Rows[0]["進度"]),
                                    DataTableUtils.toString(dt_detail.Rows[0]["預定進度"]),
                                    list.AsEnumerable().Where(w => w == copylist[i]).Count());


                }
                return dt;
            }

        }
        //取得異常頁面
        public DataTable GetNosovle_ITEC(List<string> list)
        {
            if (list.Count <= 1)
                return null;
            else
            {
                DataTable dt = new DataTable();
                List<string> copylist = new List<string>();
                for (int i = 0; i < list.Count - 1; i++)
                {
                    if (i % 2 == 0)
                        copylist.Add(list[i]);
                }
                copylist = copylist.Distinct().ToList();

                dt.Columns.Add("上線日");
                dt.Columns.Add("產線");
                dt.Columns.Add("客戶");
                dt.Columns.Add("編號");
                dt.Columns.Add("進度");
                dt.Columns.Add("預定進度");
                dt.Columns.Add("未解決數量");

                for (int i = 0; i < copylist.Count; i++)
                {
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);
                    string sqlcmd = $"select  組裝日 as 上線日,工作站名稱 as 產線名稱,(case when 組裝資料表.CUSTNM IS null then  組裝資料表.客戶  when 組裝資料表.CUSTNM ='' then  組裝資料表.客戶 else 組裝資料表.CUSTNM end ) 客戶, 工作站狀態資料表.排程編號 as 排程編號,進度 ,實際啟動時間 as 預定進度 " +
                                    $" from 工作站狀態資料表 " +
                                    $" left join 組裝資料表 on 組裝資料表.排程編號 = 工作站狀態資料表.排程編號 " +
                                    $" left join 工作站型態資料表 on 工作站狀態資料表.工作站編號 = 工作站型態資料表.工作站編號 " +
                                    $" where 工作站狀態資料表.排程編號 = '{copylist[i]}' ";
                    DataTable dt_detail = DataTableUtils.GetDataTable(sqlcmd);

                    if (HtmlUtil.Check_DataTable(dt_detail))
                        dt.Rows.Add(DataTableUtils.toString(dt_detail.Rows[0]["上線日"]),
                                    DataTableUtils.toString(dt_detail.Rows[0]["產線名稱"]),
                                    DataTableUtils.toString(dt_detail.Rows[0]["客戶"]),
                                    DataTableUtils.toString(dt_detail.Rows[0]["排程編號"]),
                                    DataTableUtils.toString(dt_detail.Rows[0]["進度"]),
                                    DataTableUtils.toString(dt_detail.Rows[0]["預定進度"]),
                                    list.AsEnumerable().Where(w => w == copylist[i]).Count());
                }
                return dt;
            }

        }
        //查詢機台的照片
        private string Search_Image(string Machine, string link)
        {
            GlobalVar.UseDB_setConnString(link);
            string sqlcmd = $"select * from Machine_Image where Ｍachine_Name = '{Machine.Split('-')[0]}' ";
            DataRow row = DataTableUtils.DataTable_GetDataRow(sqlcmd);
            if (row != null)
            {
                try
                {
                    return DataTableUtils.toString(row["Machine_ImageUrl"]);
                }
                catch
                {
                    return "";
                }
            }
            else
                return "";
        }
        //計算預定%數跟實際%數
        public List<double> percent_calculation(string schedule_number, string percent, ref string prediction_finish, string LineNum = "", bool fix = false)
        {


            work.工作時段_新增(8, 0, 12, 0);
            work.工作時段_新增(13, 0, 17, 0);
            List<double> list = new List<double>();
            string sqlcmd = "";
            DataTable dr = new DataTable();
            double real_percent = 0, predict_percent = 0;

            string finishTime = "";
            //預定進度
            GlobalVar.UseDB_setConnString(GetConnByDekVisTmp);
            //立式場用
            if (GetConnByDekVisTmp.ToLower().Contains("assm"))
                sqlcmd = $" SELECT 組裝日, 實際啟動時間, (case  when 選用通用工時 = '0' Then 標準工時  when 選用通用工時 <> '0' Then isnull( 組裝時間 ,標準工時) end ) as 標準工時 FROM 工作站狀態資料表 LEFT JOIN 工藝名稱資料表 ON 工藝名稱資料表.工作站編號 = 工作站狀態資料表.工作站編號 LEFT JOIN 組裝工藝 ON SUBSTRING(排程編號, 1, CHARINDEX('-', 排程編號) - 1) = 組裝工藝.機種編號 LEFT JOIN 工作站型態資料表 ON 工作站型態資料表.工作站編號 = 工作站狀態資料表.工作站編號 WHERE 工作站狀態資料表.排程編號 = '{schedule_number}' and (組裝時間 is not null OR 標準工時 is not null) ";
            else if (GetConnByDekVisTmp.ToLower().Contains("hor"))
            {
                if (LineNum == "1")
                    sqlcmd = $"select 實際啟動時間,(組裝工時*60*60) as 標準工時 from 工作站狀態資料表 left join 臥式工藝 on SUBSTRING(排程編號, 1, CHARINDEX('-', 排程編號) - 1) = 臥式工藝.機種編號 where 排程編號 = '{schedule_number}' and 工作站編號 = '{LineNum}' ";
                else if (LineNum == "2")
                    sqlcmd = $"select 實際啟動時間,(組裝工時*60*60) as 標準工時 from 工作站狀態資料表 left join 臥式工藝 on SUBSTRING(排程編號, 1, CHARINDEX('-', 排程編號) - 1) = 臥式工藝.機種編號 where 排程編號 = '{schedule_number}' and 工作站編號 = '{LineNum}' ";
                else if (LineNum == "14")
                    sqlcmd = $"select 實際啟動時間,(組裝工時*60*60) as 標準工時 from 工作站狀態資料表 left join 臥式工藝 on SUBSTRING(排程編號, 1, CHARINDEX('-', 排程編號) - 1) = 臥式工藝.機種編號 where 排程編號 = '{schedule_number}' and 工作站編號 = '{LineNum}' ";
                else if (LineNum == "4")
                    sqlcmd = $"select 實際啟動時間,((CAST(刀臂點數 as float)+CAST(刀套點數 as float))*60*60) as 標準工時 from 工作站狀態資料表 left join 臥式工藝 on SUBSTRING(排程編號, 1, CHARINDEX('-', 排程編號) - 1) = 臥式工藝.機種編號 where 排程編號 = '{schedule_number}' and 工作站編號 = '{LineNum}' ";
                else if (LineNum == "5")
                    sqlcmd = $"select 實際啟動時間,(CAST(刀鍊點數 as float)*60*60) as 標準工時 from 工作站狀態資料表 left join 臥式工藝 on SUBSTRING(排程編號, 1, CHARINDEX('-', 排程編號) - 1) = 臥式工藝.機種編號  where 排程編號 = '{schedule_number}' and 工作站編號 = '{LineNum}' ";
                else if (LineNum == "6")
                    sqlcmd = $"select 實際啟動時間,(CAST( 全油壓點數  as float)*60*60) as 標準工時 from 工作站狀態資料表 left join 臥式工藝 on SUBSTRING(排程編號, 1, CHARINDEX('-', 排程編號) - 1) = 臥式工藝.機種編號  where 排程編號 = '{schedule_number}' and 工作站編號 = '{LineNum}' ";
                else if (LineNum=="11")
                    sqlcmd = $"SELECT 實際啟動時間,(組裝時間) AS 標準工時 FROM 工作站狀態資料表 LEFT JOIN 組裝工藝 ON Substring(排程編號, 1, Charindex('-', 排程編號) - 1) = 組裝工藝.機種編號 WHERE 排程編號 ='{schedule_number}' AND 工作站編號 = '{LineNum}'";
            }
            dr = DataTableUtils.GetDataTable(sqlcmd);

            if (HtmlUtil.Check_DataTable(dr))
            {
                if (DataTableUtils.toString(dr.Rows[0]["標準工時"]) == "")
                    predict_percent = double.NaN;
                int standard_worktime = DataTableUtils.toInt(DataTableUtils.toString(dr.Rows[0]["標準工時"]));

                DateTime start_time;

                string timestart = "";
                //20220810 暫時移除
                //if (LineNum == "11" && !fix)
                //    timestart = DataTableUtils.toString(dr.Rows[0]["組裝日"]).Trim() + "080000";

                //else
                    timestart = DataTableUtils.toString(dr.Rows[0]["實際啟動時間"]);

                if (timestart != "")
                {
                    start_time = HtmlUtil.StrToDate(timestart);
                    DateTime stand_endtime = work.目標日期(start_time, new TimeSpan(0, 0, standard_worktime));//預計完成時間
                    finishTime = stand_endtime.ToString();
                    DateTime end_time = DateTime.Now;
                    TimeSpan prediction_time = work.工作時數(start_time, end_time);
                    real_percent = DataTableUtils.toDouble(percent) / 100;
                    predict_percent = prediction_time.TotalSeconds / standard_worktime;

                }

            }
            prediction_finish = finishTime;

            predict_percent = dr == null || DataTableUtils.toString(dr.Rows[0]["標準工時"]) == "" ? double.NaN : predict_percent;



            //都沒有抓到的話，就會回傳0
            list.Add(real_percent);
            list.Add(predict_percent);
            return list;
        }
        //比對進度是否落後
        public string Comparison_Schedule(double real_percent, double perdict_percent, ref int count, ref bool delay)
        {
            int x = 1;

            delay = false;
            if (Math.Round(perdict_percent * 100) > Math.Round(real_percent * 100) && real_percent < 1 && Double.IsInfinity(perdict_percent) == false && Math.Round(real_percent * 100) < 99)
            {
                delay = true;
                count += x;
            }
            if (perdict_percent.ToString() == "非數值" || Double.IsInfinity(perdict_percent))
                return "開發機";
            else if (perdict_percent > 1)
                return 100 + "%";
            else
                return Math.Round(perdict_percent * 100) + "%";
        }
        //找尋該排程編號的是否含有超過4小時未處理的案子
        private List<string> check_case(string scheduleID, string link, string LineNum, ref int alarm_nosolved)
        {
            GlobalVar.UseDB_setConnString(link);
            string sqlcmd = $"select * from   工作站異常維護資料表 where 排程編號 = '{scheduleID}' and 工作站編號='{LineNum}' and (父編號 is null OR 父編號 = 0)";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);

            int alarm_time = 15;
            DataRow rew = null;
            //找出該工作站的異警時間
            sqlcmd = $"select 異警時間 from 工作站型態資料表 where 工作站編號 = {LineNum}";
            rew = DataTableUtils.DataTable_GetDataRow(sqlcmd);

            alarm_time = rew != null ? DataTableUtils.toInt(DataTableUtils.toString(rew["異警時間"])) : 15;

            int total = 0, not_sloved = 0;
            string alarm = "", total_text = "", not_sloved_text = "";
            List<string> list = new List<string>();
            if (HtmlUtil.Check_DataTable(dt))
            {
                total = dt.Rows.Count;
                foreach (DataRow row in dt.Rows)
                {
                    sqlcmd = $"select * from 工作站異常維護資料表 where 父編號={DataTableUtils.toInt(DataTableUtils.toString(row["異常維護編號"]))} and 結案判定類型 is not null and LEN(結案判定類型) > 0 order by 時間紀錄 desc";
                    rew = DataTableUtils.DataTable_GetDataRow(sqlcmd);
                    //如果等null，則表示該問題尚未解決
                    if (rew == null)
                    {
                        not_sloved++;
                        sqlcmd = $"select * from 工作站異常維護資料表 where 父編號={DataTableUtils.toInt(DataTableUtils.toString(row["異常維護編號"]))}  order by 時間紀錄 desc";
                        rew = DataTableUtils.DataTable_GetDataRow(sqlcmd);
                        DateTime start_time = rew == null ? StrToDate(DataTableUtils.toString(row["時間紀錄"])) : StrToDate(DataTableUtils.toString(rew["時間紀錄"]));
                        TimeSpan span = work.工作時數(start_time, DateTime.Now);
                        if (Math.Abs(span.TotalMinutes) >= Math.Abs(alarm_time))
                            alarm = "<img src=\"../../assets/images/shutdown.gif\" width=\"26px\" height=\"26px\">";
                    }
                }
            }
            //沒有發生過事情
            if (alarm == "")
                list.Add("<img src=\"../../assets/images/normal.png\" width=\"26px\" height=\"26px\">");
            else
                list.Add(alarm);

            list.Add($"  {not_sloved}  /  {total}  ");
            alarm_nosolved += not_sloved;
            return list;
        }
        public string[] GetData(string number, string Acc)
        {
            string[] str = new string[7] { "0", "0", "0", "0", "0", "0", "0" };//0:all   1:finsh   2:Stop  3:all   4:td_finish   5:td_Stop  7:Data
            string td = "";
            string[] ErrorStr;
            string SubError = "";
            string Condition = "";
            string Condition1 = "";
            string Condition2 = "";
            DataTableUtils.Conn_String = GetConnByDekVisTmp;

            Condition1 = "排程編號 = " + "'" + number + "'" + " AND " + "實際組裝時間 =" + "'" + DateTime.Now.ToString("yyyyMMdd").ToString() + "'";
            Condition2 = "排程編號 = " + "'" + number + "'" + " AND " + "實際組裝時間 <=" + "'" + DateTime.Now.ToString("yyyyMMdd").ToString() + "'";
            Condition = Condition1 + " OR " + Condition2 + " order by " + "組裝日" + "," + "組裝編號";
            DataTable dt = DataTableUtils.DataTable_GetTable(ShareMemory.SQLAsm_WorkStation_State, Condition);

            if (HtmlUtil.Check_DataTable(dt))
            {
                DataTable dt_select = tableColumnSelectForLineDetail(dt, number.ToString(), ",工作站型態資料表.工作站名稱  ", " INNER JOIN 工作站型態資料表 ON 工作站狀態資料表.工作站編號 = 工作站型態資料表.工作站編號 ");
                str = GetEachPiece(dt);
                //Columns --目前固定的
                //Rows
                if (HtmlUtil.Check_DataTable(dt_select))
                {
                    for (int i = 0; i < dt_select.Rows.Count; i++)
                    {
                        td += "<tr class='gradeX'> \n";
                        for (int j = 0; j < dt_select.Columns.Count; j++)
                        {
                            if (dt_select.Columns[j].ColumnName == "狀態")
                            {
                                //20191125 加上style='width:?%' 以維持資料表單欄位寬度
                                // style="text-align:center;"
                                if (dt.Rows[i]["狀態"].ToString() == "暫停")
                                    td += "<td style='color: red;text-align:center;width:10%'> " + dt.Rows[i]["進度"].ToString() + "%" + " </td> \n";
                                else if (dt.Rows[i]["狀態"].ToString() == "完成")
                                    td += "<td style='color: green;text-align:center;width:10%'> " + dt.Rows[i]["進度"].ToString() + "%" + " </td> \n";
                                else if (dt.Rows[i]["狀態"].ToString() == "啟動")
                                    td += "<td style='color: blue;text-align:center;width:10%'> " + dt.Rows[i]["進度"].ToString() + "%" + " </td> \n";
                                else
                                    td += "<td style='color:black;text-align:center;width:10%'> " + dt.Rows[i]["進度"].ToString() + "%" + " </td> \n";

                            }
                            else if (dt_select.Columns[j].ColumnName == "進度")
                            {
                                //td += "<td style='color: black;text-align:center;'> " + DataTableUtils.toString(dt_select.Rows[i][j]) + "%" + " </td> \n";
                            }
                            else if (dt_select.Columns[j].ColumnName == "組裝日")
                                td += "<td style='text-align:center;width:10%'>" + DataTableUtils.toString(dt_select.Rows[i][j].ToString().Substring(4, 2)) + "/" + DataTableUtils.toString(dt_select.Rows[i][j].ToString().Substring(6, 2)) + "</td> \n";
                            else if (dt_select.Columns[j].ColumnName == "異常")//<a href="#">
                            {
                                if (dt_select.Rows[i][j].ToString() != "") //  
                                {
                                    SubError = "";
                                    ErrorStr = DataTableUtils.toString(dt_select.Rows[i][j]).Split(',');
                                    for (int k = 1; k < ErrorStr.Length - 2; k++)
                                        SubError += " " + ErrorStr[k];

                                    string Error1 = "";
                                    string Error2 = "";
                                    if (ErrorStr.Length > 3)
                                    {
                                        Error1 = ErrorStr[ErrorStr.Length - 2];
                                        Error2 = ErrorStr[ErrorStr.Length - 1];
                                    }
                                    /*
                                    string s5 = ErrorStr[ErrorStr.Length - 2];
                                    string s7 = ErrorStr[ErrorStr.Length - 1];*/
                                    // content
                                    td += "<td style='text-align:left;width:55%'>"
                                           + "<span style='font-size:10px; text-align:left;'> " + ErrorStr[0] + "<br></span>"
                                           + "<span style='color:red; text-align:center;'>" + SubError + "<br></span>"
                                           + "<span style='font-size:10px ;text-align:right;color:gray;'> " + Error1 + " " + Error2 + " <br ></ span >"
                                           + "</td> \n"; // type(UP) ,  ErrorStr (Center), yyyy/MM/dd HH:mm:SS(down):
                                }
                                else
                                    td += "<td style='text-align:center;width:55%'><u>" + "" + "</a></u></td> \n";
                            }
                            else if (dt_select.Columns[j].ColumnName == "排程編號")
                            {
                                //LayOut
                                /*20200221修改這裡*/
                                td += "<td style='text-align:center;width:20%'>" + " " + DataTableUtils.toString(dt_select.Rows[i][j]) + "</td> \n";
                                /*20200221修改這裡*/
                            }

                            else
                                td += "<td style='text-align:center;width:10%'>" + DataTableUtils.toString(dt_select.Rows[i][j]) + "</td> \n";
                        }
                        td += "</tr> \n";
                    }
                    str[6] = td;
                    return str;
                }
                else
                    return str;
            }
            else
            {
                str[6] = " no data";
                return str;
            }
        }
        public bool GetRW(string _acc)
        {
            bool Write = false;
            clsDB_Switch.dbOpen(GetConnByDekVisErp);
            DataTableUtils.Conn_String = GetConnByDekVisErp;
            if (clsDB_Switch.IsConnected == true)
            {
                DataRow dr_rw = null;
                try
                {
                    dr_rw = DataTableUtils.DataTable_GetDataRow("select FUNC_YN from  SYSTEM_PMR where USER_ACC=" + "'" + _acc + "'");
                }
                catch
                {
                    GetRW(_acc);
                }


                if (dr_rw != null && dr_rw[0].ToString().ToUpper() == "Y")
                {
                    Write = true;
                    //back
                    clsDB_Switch.dbOpen(GetConnByDekVisTmp);
                    DataTableUtils.Conn_String = GetConnByDekVisTmp;
                }
            }
            else
                DataTableUtils.Conn_String = GetConnByDekVisTmp;
            return Write;
        }
        public DataRow GetAccInf(string _acc)
        {
            DataRow dr_rw = null;
            clsDB_Switch.dbOpen(GetConnByDekVisErp);
            DataTableUtils.Conn_String = GetConnByDekVisErp;
            if (clsDB_Switch.IsConnected == true)
            {
                try
                {
                    dr_rw = DataTableUtils.DataTable_GetDataRow("select * from  USERS where USER_ACC=" + "'" + _acc + "'");
                }
                catch
                {
                    GetAccInf(_acc);
                }


                clsDB_Switch.dbOpen(GetConnByDekVisTmp);
                DataTableUtils.Conn_String = GetConnByDekVisTmp;
            }
            else
                DataTableUtils.Conn_String = GetConnByDekVisTmp;
            return dr_rw;
        }
        public List<string> Getdepartment(departmentSelect _Select, ref string _acc)
        {
            List<string> dep = new List<string>();
            string condition = " where USER_ACC = " + "'" + _acc + "'";
            clsDB_Switch.dbOpen(GetConnByDekVisErp);
            DataTableUtils.Conn_String = GetConnByDekVisErp;
            if (clsDB_Switch.IsConnected == true)
            {
                DataRow dr_dep = DataTableUtils.DataTable_GetDataRow("select USER_ACC,USER_DPM,DEPARTMENT.DPM_NAME2 from USERS  INNER JOIN DEPARTMENT ON USERS.USER_DPM = DEPARTMENT.DPM_NAME" + condition);
                if (_Select == departmentSelect.個人)
                {
                    if (dr_dep != null)
                        dep.Add(dr_dep["DPM_NAME2"].ToString());
                }
                else
                {
                    DataTable dt_dep = DataTableUtils.DataTable_GetTable("select DPM_NAME2 from DEPARTMENT", 0, 0);
                    if (dt_dep != null)
                    {
                        foreach (DataRow dr in dt_dep.Rows)
                        {
                            if (dr["DPM_NAME2"].ToString() != "德科" && dr["DPM_NAME2"].ToString() != "系統")
                                dep.Add(dr["DPM_NAME2"].ToString());
                        }
                        //dep.Add("採購部");
                    }

                    if (dr_dep != null)
                        _acc = dr_dep["DPM_NAME2"].ToString();
                    else
                        _acc = dep[0];
                }
                clsDB_Switch.dbOpen(GetConnByDekVisTmp);
                DataTableUtils.Conn_String = GetConnByDekVisTmp;
            }
            else
                DataTableUtils.Conn_String = GetConnByDekVisTmp;
            return dep;
        }
        public List<string> GetErrorProcessStatus()
        {
            List<string> ErrorStatus = new List<string>();
            DataTableUtils.Conn_String = GetConnByDekVisTmp;
            DataTable dt_ErrorStatus = DataTableUtils.DataTable_GetTable("select 狀態名稱  from " + ShareMemory.SQLAsm_WorkStation_ErrorProcessStatus, 0, 0);

            if (HtmlUtil.Check_DataTable(dt_ErrorStatus))
            {
                foreach (DataRow dr in dt_ErrorStatus.Rows)
                    ErrorStatus.Add(dr["狀態名稱"].ToString());
            }
            return ErrorStatus;
        }
        public List<string> GetErrorType(string key, string LineNum)
        {
            string Condition = "";
            List<string> Errors = new List<string>();
            DataTableUtils.Conn_String = GetConnByDekVisTmp;
            //List<ErrorType> Errors = new List<ErrorType>();//special Math
            if (!GetConnByDekVisTmp.Contains("Hor"))
                Condition = "排程編號=" + "'" + key + "'";
            else
                Condition = "排程編號=" + "'" + key + "'" + " AND " + "工作站編號=" + "'" + LineNum + "'";

            if (key == null)
            {

                DataTable dt_Error = DataTableUtils.DataTable_GetTable("select 備註編號,備註內容 from " + ShareMemory.SQLAsm_WorkStation_Note + " where  備註型態 = " + "'" + "異常" + "'", 0, 0);

                if (HtmlUtil.Check_DataTable(dt_Error))
                {
                    foreach (DataRow dr in dt_Error.Rows)
                        Errors.Add(dr["備註內容"].ToString());
                    //Errors.Add(new ErrorType() { ErrorId = DataTableUtils.toInt(dr["備註編號"].ToString()), ErrorName = dr["備註內容"].ToString() });
                }
            }
            else
            {
                //List<string> Errors_mant = new List<string>();
                DataTable dt_Error = DataTableUtils.DataTable_GetTable("select 異常維護編號,異常原因類型 from " + ShareMemory.SQLAsm_WorkStation_ErrorMant + " where " + Condition, 0, 0);


                //DataTable dt_Error = DataTableUtils.DataTable_GetTable("select 異常維護編號,異常原因類型 from " + ShareMemory.SQLAsm_WorkStation_ErrorMant,Condition, 0, 0);
                if (HtmlUtil.Check_DataTable(dt_Error))
                {
                    foreach (DataRow dr in dt_Error.Rows)
                    {
                        if (!Errors.Contains(dr["異常原因類型"].ToString()))
                            Errors.Add(dr["異常原因類型"].ToString());
                    }
                }

            }
            return Errors;
        }
        public int GetSeriesNum(string tableName, int ColumnIndex)
        {
            int Count = 1;
            DataTableUtils.Conn_String = GetConnByDekVisTmp;

            DataTable dr_hear = DataTableUtils.DataTable_GetRowHeader(tableName);
            DataTable dt = DataTableUtils.DataTable_GetTable("select " + dr_hear.Columns[ColumnIndex] + " from " + tableName + " order by " + dr_hear.Columns[ColumnIndex] + " desc");
            string field_name = DataTableUtils.toString(dr_hear.Columns[ColumnIndex]);

            if (HtmlUtil.Check_DataTable(dt))
            {
                Count = DataTableUtils.toInt(DataTableUtils.toString(dt.Rows[0][field_name])) + 1;
                return Count;
            }
            else
                return GetSeriesNum(tableName, ColumnIndex);
        }
        public Dictionary<string, int> GetWorkPointInf(string LineNum = null)
        {
            DataTableUtils.Conn_String = GetConnByDekVisTmp;
            Dictionary<string, int> WorkStationInf_List = new Dictionary<string, int>();
            DataTable dt;
            if (LineNum == null)
                dt = DataTableUtils.DataTable_GetTable("select 工作站名稱,工作站編號 from  " + ShareMemory.SQLAsm_WorkStation_Type);
            else
                dt = DataTableUtils.DataTable_GetTable(ShareMemory.SQLAsm_WorkStation_Type, "工作站編號=" + LineNum, 0, 0);
            foreach (DataRow dr in dt.Rows)
            {
                if (!WorkStationInf_List.ContainsKey(dr["工作站名稱"].ToString()))
                    WorkStationInf_List.Add(dr["工作站名稱"].ToString(), DataTableUtils.toInt(dr["工作站編號"].ToString()));//name,ID
            }
            return WorkStationInf_List;
        }
        public int GetTagetPiece(string TimeType, int LineNumber, string startTime, string endTime, string WhoSet)
        {
            int SubTagetPiece = 0;
            string Condition = "";
            DataTableUtils.Conn_String = GetConnByDekVisTmp;
            DataTable dt;
            if (WhoSet == "Set" || TimeType == null)
            {
                Condition = "工作站編號 = " + "'" + LineNumber + "'";
                dt = DataTableUtils.DataTable_GetTable(ShareMemory.SQLAsm_WorkStation_Type, Condition);
                if (dt.Rows.Count != 0)
                    SubTagetPiece = DataTableUtils.toInt(dt.Rows[0]["目標件數"].ToString());
            }
            else if (TimeType == "week" || TimeType == "month" || TimeType == "define") // 實際組裝時間
            {
                Condition = "工作站編號 = " + "'" + LineNumber + "'" + " AND " + "實際組裝時間>=" + "'" + startTime + "'" + " AND " + "實際組裝時間<=" + "'" + endTime + "'";
                dt = DataTableUtils.DataTable_GetTable(ShareMemory.SQLAsm_WorkStation_State, Condition);
                SubTagetPiece = dt.Rows.Count;
            }
            else
            {
                Condition = "工作站編號 = " + "'" + LineNumber + "'" + " AND " + "實際組裝時間>=" + "'" + startTime + "'" + " AND " + "實際組裝時間<=" + "'" + endTime + "'";
                dt = DataTableUtils.DataTable_GetTable(ShareMemory.SQLAsm_WorkStation_State, Condition);
                SubTagetPiece = dt.Rows.Count;
            }
            return SubTagetPiece;
        }
        public int GetRealPiece(int LineNumber, string StartDate = "Today", string EndDate = "Today")
        {
            int SubTagetPiece = 0;
            string Condition = "";
            DataTableUtils.Conn_String = GetConnByDekVisTmp;
            if (EndDate == "Today")// not Input
                Condition = "工作站編號 = " + "'" + LineNumber + "'" + " AND " + "實際完成時間 <" + "'" + DateTime.Now.ToString("yyyyMMddhhmmss").ToString() + "'" + " AND " + "實際完成時間 >" + "'" + DateTime.Now.ToString("yyyyMMdd").ToString() + "010101" + "'";
            else
                Condition = "工作站編號 = " + "'" + LineNumber + "'" + " AND " + "實際完成時間 <" + "'" + EndDate + "'" + " AND " + "實際完成時間 >" + "'" + StartDate + "'";
            DataTable dt = DataTableUtils.DataTable_GetTable(ShareMemory.SQLAsm_WorkStation_State, Condition);
            if (dt.Rows.Count != 0)
                SubTagetPiece = dt.Rows.Count;
            return SubTagetPiece;
        }
        public int GetOnTagetPiece(string TimeType, int LineNumber, string startTime, string endTime, ref DataTable dt)
        {
            int SubOnTagetPiece = 0;
            // dt.Clear();
            string Condition = "工作站編號 = " + "'" + LineNumber + "'" + " AND " + "實際組裝時間>=" + "'" + startTime + "'" + " AND " + "實際組裝時間<=" + "'" + endTime + "'" + " AND " + "狀態=" + "'" + "完成" + "'";
            DataTable dt_sub = DataTableUtils.DataTable_GetTable(ShareMemory.SQLAsm_WorkStation_State, Condition);
            SubOnTagetPiece = CountOnTaget(dt_sub);
            dt.Merge(dt_sub, false);
            return SubOnTagetPiece;
        }
        public string[] GetTimeType(string Str)
        {
            string[] time = new string[7];
            if (Str == "day")
            {
                time[0] = DateTime.Now.ToString("yyyyMMdd").ToString() + "010101";
                time[1] = DateTime.Now.ToString("yyyyMMdd").ToString() + "235959";
                time[3] = "HH:mm";
                // time[4] = "產線狀態 " + "(" + "今日" + ")";
                //time[4] = "(" + "本日" + ")";
                time[4] = "(" + DateTime.Now.ToString("yyyy-MM-dd").ToString() + ")";
                //time[4] = "產線狀態 " + "(" + DateTime.Now.ToString("yyyy/MM/dd").ToString() + ")";
                time[5] = "1";
                time[6] = "hour";
            }
            else if (Str == "week")
            {
                time[0] = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + 1).ToString("yyyyMMdd");
                time[1] = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + 5).ToString("yyyyMMdd");
                time[3] = "DD MM YYYY";
                //time[4] = "產線狀態 " + "( 一周)";
                //time[4] = "( 一周)";
                time[4] = "(" + DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + 1).ToString("MM/dd") + "-" + DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + 5).ToString("MM/dd") + ")";
                time[5] = "1";
                time[6] = "day";
                //time[0] = DateTime.Now.AddDays(-9).ToString("yyyyMMddmmhhss").ToString();
                //time[1] = DateTime.Now.ToString("yyyyMMddmmhhss").ToString();
            }
            else if (Str == "month")
            {
                time[0] = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyyMMdd");
                time[1] = new DateTime(DateTime.Now.AddMonths(1).Year, DateTime.Now.AddMonths(1).Month, 1).AddDays(-1).ToString("yyyyMMdd");
                time[3] = "DD";
                //time[4] = "產線狀態 " + "(" + DateTime.Now.ToString("yyyy/MM").ToString() + ")";
                //time[4] = "產線狀態 " + "(" + "本月" + ")";
                //time[4] = "(" + "本月" + ")";
                time[4] = "(" + new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy年MM月") + ")";
                time[5] = "1";
                time[6] = "day";
                //time[0] = DateTime.Now.AddMonths(-1).ToString("yyyyMMddmmhhss").ToString();
                //time[1] = DateTime.Now.ToString("yyyyMMddmmhhss").ToString();
            }
            else if (Str == "season")
            {
                time[0] = DateTime.Now.AddMonths(-3).ToString("yyyyMMdd").ToString();
                time[1] = DateTime.Now.ToString("yyyyMMdd").ToString();
                time[3] = "MMM";
                time[4] = "產線狀態 " + "(本季)";
                time[5] = "1";
                time[6] = "month";
            }
            else if (Str == "fyear")
            {
                time[0] = DateTime.Now.ToString("yyyy0101");
                time[1] = DateTime.Now.ToString("yyyy0630");
                time[3] = "MMM";
                //time[4] = "產線狀態 " + "(前半年度)";
                time[4] = "(1-6月)";
                time[5] = "1";
                time[6] = "month";
                //time[0] = DateTime.Now.AddMonths(-6).ToString("yyyyMMddmmhhss").ToString();
                //time[1] = DateTime.Now.ToString("yyyyMMddmmhhss").ToString();
            }
            else if (Str == "byear")
            {
                time[0] = DateTime.Now.ToString("yyyy0701");
                time[1] = DateTime.Now.ToString("yyyy1231");
                time[3] = "MMM";
                //time[4] = "產線狀態 " + "(後半年度)";
                time[4] = "(7-12月)";
                time[5] = "1";
                time[6] = "month";
                //time[0] = DateTime.Now.AddMonths(-6).ToString("yyyyMMddmmhhss").ToString();
                //time[1] = DateTime.Now.ToString("yyyyMMddmmhhss").ToString();
            }
            else if (Str == "year")
            {
                time[0] = DateTime.Now.ToString("yyyy0101");
                time[1] = DateTime.Now.ToString("yyyy1231");
                time[3] = "MMM YYYY";
                // time[4] = "產線狀態 " + "(全年度)";
                // time[4] = "(全年度)";
                time[4] = DateTime.Now.ToString("yyyy") + "年";
                time[5] = "1";
                time[6] = "month";
                // time[0] = DateTime.Now.AddYears(-1).ToString("yyyyMMddmmhhss").ToString();
                // time[1] = DateTime.Now.ToString("yyyyMMddmmhhss").ToString();
            }
            else
            {
                time[0] = DateTime.Now.ToString("yyyyMMdd").ToString() + "010101";
                time[1] = DateTime.Now.ToString("yyyyMMdd").ToString() + "235959";
                time[3] = "hh:mm";
                //time[4] = "產線狀態 " + "(" + DateTime.Now.ToString("yyyy/MM/dd").ToString() + ")";
                time[4] = "(" + DateTime.Now.ToString("yyyy-MM-dd").ToString() + ")";
            }
            time[2] = Str;
            return time;
        }
        private List<string> TrsDateList(string TimeType, string StartDay = "Today", string EndDay = "Today")
        {
            TimeSpan ts;
            List<string> date = new List<string>();
            // day
            if (TimeType == "day")
            {
                //for (int i = 0; i < 24; i++)
                //    date.Add(StartDay.Substring(0, 8) + i.ToString().PadLeft(2, '0'));
                date.Add(StartDay);
                return date;
            }
            // other
            var today = DateTime.ParseExact(StartDay, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
            var someday = DateTime.ParseExact(EndDay, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
            ts = someday - today;
            if (TimeType == "week" || TimeType == "month" || (TimeType == "define" && ts.TotalDays < 30))
            {
                foreach (var day in DateTransfor.EachDay(today, someday))//string.Format("{0:yyyyMMdd}", dt);　
                    date.Add(string.Format("{0:yyyyMMdd}", day));
            }
            else //month,season ,fhalf-year ,bhalf-year, year (unit :month)
            {
                foreach (var day in DateTransfor.EachMonthTo(today, someday))//month　
                    date.Add(string.Format("{0:yyyyMMdd}", day));
            }
            return date;
        }
        public object GetLineList()
        {
            DataTableUtils.Conn_String = GetConnByDekVisTmp;
            string condition = " where 工作站是否使用中='1'";
            //string condition = "";
            //string condition = "";
            DataTable dt_line = DataTableUtils.DataTable_GetTable("select 工作站編號,工作站名稱 from " + ShareMemory.SQLAsm_WorkStation_Type + condition, 0, 0);
            List<LineData> Lines = new List<LineData>();//special Math
            if (dt_line != null)
            {
                foreach (DataRow dr in dt_line.Rows)
                    Lines.Add(new LineData() { LineId = DataTableUtils.toInt(dr["工作站編號"].ToString()), LineName = dr["工作站名稱"].ToString() });
            }
            return Lines;
        }
        public string GetColumnName(string PageName)
        {
            string ColumnName = "";
            string[] muilt;
            //string[] Colweight;
            switch (PageName)
            {

                case "Asm_LineTotalView":
                    //  muilt = new string[4] { "產線名稱", "在線", "完成", "異常"};
                    muilt = new string[5] { "產線名稱", "在線", "完成", "異常", "落後" };
                    break;
                case "Asm_LineOverView":
                    if (ConfigurationManager.AppSettings["show_function"] == "1")
                        muilt = new string[6] { "上線日", "客戶", "編號", "進度", "預定進度", "未解決/全部" };
                    else
                        muilt = new string[6] { "上線日", "客戶", "編號", "進度", "預定進度", "異常歷程" };
                    //if (ConfigurationManager.AppSettings["show_function"] == "1")
                    //    muilt = new string[5] { "上線日", "客戶", "編號", "進度", "未解決/全部" };
                    //else
                    //    muilt = new string[5] { "上線日", "客戶", "編號", "進度",  "異常歷程" };
                    break;
                /*20200221修改這裡*/
                case "Asm_NumView":
                    muilt = new string[6] { "上線日", "工作站名稱", "客戶", "編號", "進度", "備註" };
                    break;
                /*20200221修改這裡*/
                case "Asm_ErrorSearch":
                    muilt = new string[4] { "編號", "排程編號", "站名", "原因" };
                    //muilt = new string[5] { "時間", "人員", "類型", "內容", "圖片" };
                    break;
                case "Asm_ErrorDetail":
                    //muilt = new string[5] { "時間", "人員", "類型", "內容", "圖片" };
                    //muilt = new string[4] { "編號", "人員", "單位", "內容" };
                    muilt = new string[3] { "人員", "內容", "狀態" };
                    break;
                case "ErrorChat":
                    muilt = new string[4] { "排程編號", "起始時間", "排除時間", "處理時間" };
                    break;
                case "Product_Detail":
                    muilt = new string[4] { "工藝時間", "起始時間", "完成時間", "預計完成" };
                    break;
                case "Asm_history":
                    muilt = new string[6] { "排程編號", "產線名稱", "起始時間", "完成時間", "組裝時間[分]", "異常件數" };
                    break;
                default:
                    muilt = new string[5] { "產線編號", "產線名稱", "排程產能", "實際產量", "達成率(%)" };
                    break;
            }
            //    muilt = new string[5] { "產線編號", "產線名稱", "排程產能", "實際產量", "達成率(%)" };
            //    Colweight = new string[5] { "10", "30", "20", "20", "20" };
            //ColumnName = "<tr class= 'column' style='text-align:center;background-color:dimgray;color: beige;' >\n";
            ColumnName = "<tr id='tr_row'>";
            // ColumnName+= "<th>\n";
            //if (PageName == "Asm_ErrorDetail")
            //{
            //    //ColumnName += "<th>\n";
            //    ColumnName += " <th><input type='checkbox' id='check - all' class='flat'></th> \n";
            //    //ColumnName += "</th>\n";
            //}
            // ColumnName += "</th>\n";
            for (int i = 0; i < muilt.Length; i++)
                ColumnName += "<th style='text-align:center;'>" + muilt[i] + "</th>";
            ColumnName += "</tr>";
            return ColumnName;
        }
        public Dictionary<string, string> AnsQueryStringChart(string QueryString)
        {
            string[] Qstr;
            string[] QSubStr;
            Dictionary<string, string> dc_PraStr = new Dictionary<string, string>();
            if (QueryString != null && QueryString != "")
            {
                Qstr = QueryString.Split(',');
                dc_PraStr.Add("Key", Qstr[0]); // First  ?AA = 1
                if (Qstr.Length > 1)
                {
                    foreach (string Str in Qstr)
                    {
                        QSubStr = Str.Split('=');
                        if (QSubStr.Length > 1)
                            dc_PraStr.Add(QSubStr[0], QSubStr[1]);
                    }
                }
                return dc_PraStr;
            }
            else
                return dc_PraStr;
        }
        public string[] GetChartColumnName(string PageName, string TableFormat)
        {
            string[] ColumnName = new string[2] { " ", " " };
            string[] muilt;
            //string[] Colweight;
            switch (PageName)
            {
                case "ErrorChat":
                    muilt = new string[5] { "排程編號", "起始時間", "排除時間", "處理時間", "異常歷程" };
                    //ColumnName[1] = "dt-responsive nowrap";
                    break;
                case "ErrorChatDetail":
                    muilt = new string[4] { "維護人員", "處理單位", "異常內容", "處理狀態" };
                    //muilt = new string[5] { "時間", "人員", "類型", "內容", "圖片" };
                    break;
                case "Product_Detail":
                    muilt = new string[9] { "工藝名稱", "組裝人員", "標準工時", "組裝累積", "異常累積", "實際|預估", "啟動時間", "預估完成", "實際完成" };
                    ColumnName[1] = "dt-responsive nowrap";
                    break;
                //case "Product_Det1ail":
                //    muilt = new string[9] { "工藝名稱", "組裝人員", "標準工時", "組裝累積", "異常累積", "實際|預估", "啟動時間", "預估完成", "實際完成" };
                //    ColumnName[1] = "dt-responsive nowrap";
                //    break;
                case "ErrorSearch_Detail":
                    muilt = new string[4] { "維護人員", "處理單位", "異常內容", "處理狀態" };
                    //muilt = new string[5] { "維護人員", "處理單位", "異常內容", "處理'狀態", "累積時間" };
                    //ColumnName[1] = "dt-responsive nowrap";
                    break;
                case "Asm_LineOverView_FinishSpecialFunction":
                    muilt = new string[4] { "客戶", "排程編號", "送貨日期", "送貨時段" };
                    //muilt = new string[5] { "維護人員", "處理單位", "異常內容", "處理'狀態", "累積時間" };
                    //ColumnName[1] = "dt-responsive nowrap";
                    break;
                default:
                    muilt = new string[5] { "產線編號", "產線名稱", "排程產能", "實際產量", "達成率(%)" };
                    break;
            }
            ColumnName[0] = "<tr id=\"tr_row\">\n";
            for (int i = 0; i < muilt.Length; i++)
                ColumnName[0] += "<th style='text-align:center;'>" + muilt[i] + "</th>\n";
            ColumnName[0] += "</tr>";
            return ColumnName;
        }
        public string GetCharstColumnName_Error(string SelectLine, CheckBoxList ColumnNameList, string PageName)
        {
            string ColumnName = "";
            string FirstTitle = "";
            if (PageName == "Asm_Cahrt_Error")
                FirstTitle = "錯誤類型";
            else if (PageName == "Asm_Compliance_rate")
                FirstTitle = "時間列表";
            else
                FirstTitle = "未定義";
            ColumnName = "";

            if (SelectLine == "0")
            {
                ColumnName += "<th>" + FirstTitle + "</th>";
                foreach (ListItem Name in ColumnNameList.Items)
                    ColumnName += "<th>" + Name.Text + "</th>";
            }

            return ColumnName;
        }
        public string CheckConnectSting(List<string> ErrorInf)
        {
            //先判斷有無字串[有字串表示來自於VIS網頁,無字串表示來自於Line Error直接連結]
            //無字串 給Vec 字串測試
            if (!string.IsNullOrEmpty(GetConnByDekVisTmp))
                return GetConnectStrFromWorkStation(ErrorInf);
            else
            {
                GetConnByDekVisTmp = GetConnByDekdekVisAssm;
                return GetConnectStrFromWorkStation(ErrorInf);
            }
        }
        private string GetConnectStrFromWorkStation(List<string> StationINf)
        {
            //先判斷 是不是 vec
            //在判斷 是不是 hor
            //Vec(hor) 有值 表示現在是 Vec(hor) 且回傳是Vec類error
            //Vec(hor) 無值 表示現在是 Vec(hor) 類error
            string condition = ShareMemory.WorkStationName + "=" + "'" + StationINf[2].ToString() + "'" + "AND " + ShareMemory.WorkStationNum + "=" + "'" + StationINf[1].ToString() + "'";
            if (GetConnByDekVisTmp.Contains("dekVisAssm"))//Vec
            {
                if (DataTableUtils.RowCount(ShareMemory.SQLAsm_WorkStation_Type, condition) == 0)
                    GetConnByDekVisTmp = GetConnByDekdekVisAssmHor;
            }
            else //Hor
            {
                if (DataTableUtils.RowCount(ShareMemory.SQLAsm_WorkStation_Type, condition) == 0)
                    GetConnByDekVisTmp = GetConnByDekdekVisAssm;
            }
            return GetConnByDekVisTmp;
        }
        /*20200221修改這裡*/
        public static string Last_Place(string acc, string factory = "")
        {
            string str = "";
            clsDB_Server clsDB = new clsDB_Server("");
            clsDB.dbOpen(myclass.GetConnByDekVisErp);
            DataTableUtils.Conn_String = myclass.GetConnByDekVisErp;

            string sql_cmd = $"select * from users where USER_ACC = '{acc}'";
            DataTable dt = clsDB.GetDataTable(sql_cmd);

            if (HtmlUtil.Check_DataTable(dt))
            {
                if (factory == "")
                {
                    if (DataTableUtils.toString(dt.Rows[0]["Last_Model"]) != "")
                        return DataTableUtils.toString(dt.Rows[0]["Last_Model"]);
                    else
                        return "Ver";
                }
                else
                {
                    if (dt.Rows.Count > 0)
                    {
                        DataRow row = dt.NewRow();

                        row["Last_Model"] = factory;

                        if (clsDB.Update_DataRow("users", $"USER_ACC = '{acc}'", row))
                        {
                            str = "";
                        }
                    }
                }
            }

            return str;
        }
        /*20200221修改這裡*/
        //=========================Tool===============================================
        public string TrsDate(string Source)
        {
            string[] date;
            if (Source.Contains('_'))
            {
                date = Source.Split('_');
                return date[1];
            }
            else
                return "month";
        }
        public string TrsTime(string Src, string Unit)
        {
            string TrsTime = "0";
            string tmp = "";
            if (DataTableUtils.toInt(Src) >= 60)
            {
                TimeSpan ts = new TimeSpan(0, 0, DataTableUtils.toInt(Src));
                if (Unit.Contains('m'))
                    TrsTime = (ts.Days * 24 * 60 + ts.Hours * 60 + ts.Minutes).ToString();//Minutes
                else
                {
                    tmp = (DataTableUtils.toDouble(ts.Minutes.ToString()) / 60).ToString();
                    if (tmp.Length >= 3)
                    {
                        tmp = tmp.Remove(0, 1).Substring(0, 2);
                        if (!tmp.Contains('0'))
                            TrsTime = (ts.Days * 24 + ts.Hours).ToString() + tmp; //Minutes
                        else
                            TrsTime = (ts.Days * 24 + ts.Hours).ToString();
                    }
                    else
                        TrsTime = (ts.Days * 24 + ts.Hours).ToString(); //Minutes
                }
            }
            else
            {
                if (Src != "0")
                    TrsTime = "> 1";
            }
            return TrsTime;
        }
        //==========================Chart===============================================
        public string[] SubGetChartInf(string TimeType, string StartDate = "Today", string EndDate = "Today")
        {
            int Taget = 0;
            int Real = 0;
            int count = 0;
            string RateSubStr = "";
            string td = "";
            string[] infStr = new string[4];
            Dictionary<string, int> WorkStationInf_List = new Dictionary<string, int>();
            WorkStationInf_List = GetWorkPointInf();
            //{ x: 10, y: 6, label: "Apple" },
            foreach (KeyValuePair<string, int> LineInf in WorkStationInf_List)
            {
                count++;
                Taget = GetTagetPiece(TimeType, LineInf.Value, StartDate, EndDate, null);
                Real = GetRealPiece(LineInf.Value, StartDate, EndDate);
                //table
                td += "<tr class='gradeX'> \n";
                td += "<td style='text-align:center;'>" + LineInf.Value + "</td> \n";
                td += "<td style='text-align:center;'>" + LineInf.Key + "</td> \n";
                td += "<td style='text-align:center;'>" + Taget + "</td> \n";
                td += "<td style='text-align:center;'>" + Real + "</td> \n";
                //chart
                infStr[0] = "{x:" + (10 * count).ToString() + "," + "y:" + DataTableUtils.toString(Taget) + ", label:" + "'" + LineInf.Key + "'" + "}" + "," + infStr[0];
                infStr[1] = "{x:" + (10 * count).ToString() + "," + "y:" + DataTableUtils.toString(Real) + ", label:" + "'" + LineInf.Key + "'" + "}" + "," + infStr[1];
                if (Taget != 0)
                    RateSubStr = (DataTableUtils.toDouble(Real.ToString()) / DataTableUtils.toDouble(Taget.ToString()) * 100).ToString("F2");
                else
                    RateSubStr = "0";
                td += "<td style='text-align:center;'>" + RateSubStr + "</td> \n";
                infStr[2] = "{x:" + (10 * count).ToString() + "," + "y:" + RateSubStr + ", label:" + "'" + LineInf.Key + "'" + ",indexLabel: " + "'" + RateSubStr + "%" + "'" + "}" + "," + infStr[2];
                td += "</tr> \n";
            }
            infStr[3] = td;
            return infStr;
        }
        public string[] SubGetChartInf(string TimeType, string StartDate = "Today", string EndDate = "Today", string SelectLine = null)
        {
            int Taget = 0;
            int Real = 0;
            string RateSubStr = "";
            string td = "";
            string StartDateDay = "";
            string EndDateDay = "";
            string yearStr = "";
            string MonthStr = "";
            string dayStr = "";
            string hourStr = "";
            string[] infStr = new string[4];
            Dictionary<string, int> WorkStationInf_List = new Dictionary<string, int>();
            List<string> dateList = new List<string>();
            WorkStationInf_List = GetWorkPointInf(SelectLine);
            if (SelectLine != null) //one line
            {
                if (TimeType == "other")
                {
                    return infStr;
                }
                else if (TimeType == "week" || TimeType == "month" || TimeType == "define")
                {
                    StartDateDay = StartDate.Substring(0, 8);
                    EndDateDay = EndDate.Substring(0, 8);
                    foreach (KeyValuePair<string, int> LineInf in WorkStationInf_List)
                    {
                        dateList = TrsDateList(TimeType, StartDateDay, EndDateDay);
                        foreach (string date in dateList)
                        {
                            Taget = GetTagetPiece(TimeType, LineInf.Value, date.ToString(), date.ToString(), null);
                            Real = GetRealPiece(LineInf.Value, date.ToString() + "010101", date.ToString() + "235959");
                            //table
                            td += "<tr class='gradeX'> \n";
                            td += "<td style='text-align:center;'>" + date.ToString() + "</td> \n";
                            td += "<td style='text-align:center;'>" + LineInf.Value + "</td> \n";
                            td += "<td style='text-align:center;'>" + LineInf.Key + "</td> \n";
                            td += "<td style='text-align:center;'>" + Taget + "</td> \n";
                            td += "<td style='text-align:center;'>" + Real + "</td> \n";
                            //chart    { x: new Date(2012, 00, 1), y: 450 },
                            yearStr = date.ToString().Substring(0, 4);
                            //MonthStr = (DataTableUtils.toInt(date.ToString().Substring(4, 2)) ).ToString();
                            MonthStr = (DataTableUtils.toInt(date.ToString().Substring(4, 2)) - 1).ToString();
                            dayStr = date.ToString().Substring(6, 2);
                            infStr[0] = "{x:new Date(" + yearStr + "," + MonthStr + "," + dayStr + ")," + "y:" + DataTableUtils.toString(Taget) + "}" + "," + infStr[0];
                            infStr[1] = "{x:new Date(" + yearStr + "," + MonthStr + "," + dayStr + ")," + "y:" + DataTableUtils.toString(Real) + "}" + "," + infStr[1];
                            if (Taget != 0)
                                RateSubStr = (DataTableUtils.toDouble(Real.ToString()) / DataTableUtils.toDouble(Taget.ToString()) * 100).ToString("F2");
                            else
                                RateSubStr = "0";
                            td += "<td style='text-align:center;'>" + RateSubStr + "</td> \n";
                            infStr[2] = "{x:new Date(" + yearStr + "," + MonthStr + "," + dayStr + ")," + "y:" + RateSubStr + ",indexLabel: " + "'" + RateSubStr + "%" + "'" + "}" + "," + infStr[2];
                            td += "</tr> \n";
                        }
                    }
                    infStr[3] = td;
                    return infStr;
                }
                else if (TimeType == "day")
                {

                    StartDateDay = StartDate.Substring(0, 14);
                    EndDateDay = EndDate.Substring(0, 14);
                    foreach (KeyValuePair<string, int> LineInf in WorkStationInf_List)
                    {
                        dateList = TrsDateList(TimeType, StartDateDay, EndDateDay);
                        foreach (string date in dateList)
                        {
                            Taget = GetTagetPiece(TimeType, LineInf.Value, StartDate, date.ToString(), null);  //待修正   一日的每個小時目標幾個!!
                            Real = GetRealPiece(LineInf.Value, date.ToString().Substring(0, 10) + "0101", date.ToString().Substring(0, 10) + "5959");
                            //table
                            td += "<tr class='gradeX'> \n";
                            td += "<td style='text-align:center;'>" + date.ToString().Substring(8, 2) + ":00" + "</td> \n";
                            td += "<td style='text-align:center;'>" + LineInf.Value + "</td> \n";
                            td += "<td style='text-align:center;'>" + LineInf.Key + "</td> \n";
                            td += "<td style='text-align:center;'>" + Taget + "</td> \n";
                            td += "<td style='text-align:center;'>" + Real + "</td> \n";
                            //chart    { x: new Date(2012, 00, 1), y: 450 },
                            yearStr = date.ToString().Substring(0, 4);
                            //MonthStr = (DataTableUtils.toInt(date.ToString().Substring(4, 2))).ToString();
                            MonthStr = (DataTableUtils.toInt(date.ToString().Substring(4, 2)) - 1).ToString();
                            dayStr = date.ToString().Substring(6, 2);
                            hourStr = date.ToString().Substring(8, 2);
                            infStr[0] = "{x:new Date(" + yearStr + "," + MonthStr + "," + dayStr + "," + hourStr + ")," + "y:" + DataTableUtils.toString(Taget) + "}" + "," + infStr[0];
                            infStr[1] = "{x:new Date(" + yearStr + "," + MonthStr + "," + dayStr + "," + hourStr + ")," + "y:" + DataTableUtils.toString(Real) + "}" + "," + infStr[1];
                            if (Taget != 0)
                                RateSubStr = (DataTableUtils.toDouble(Real.ToString()) / DataTableUtils.toDouble(Taget.ToString()) * 100).ToString("F2");
                            else
                                RateSubStr = "0";
                            td += "<td style='text-align:center;'>" + RateSubStr + "</td> \n";
                            infStr[2] = "{x:new Date(" + yearStr + "," + MonthStr + "," + dayStr + "," + hourStr + ")," + "y:" + RateSubStr + ",indexLabel: " + "'" + RateSubStr + "%" + "'" + "}" + "," + infStr[2];
                            td += "</tr> \n";
                        }
                    }
                    infStr[3] = td;
                    return infStr;
                }
                else //month,season ,fhalf-year ,bhalf-year, year (unit :month)
                {
                    StartDateDay = StartDate.Substring(0, 8);
                    EndDateDay = EndDate.Substring(0, 8);
                    foreach (KeyValuePair<string, int> LineInf in WorkStationInf_List)
                    {
                        dateList = TrsDateList(TimeType, StartDateDay, EndDateDay);
                        foreach (string date in dateList)
                        {
                            Taget = GetTagetPiece(TimeType, LineInf.Value, date.ToString().Substring(0, 6) + "01", date.ToString().Substring(0, 6) + "31", null);
                            Real = GetRealPiece(LineInf.Value, date.ToString().Substring(0, 6) + "01", date.ToString().Substring(0, 6) + "31");
                            //table
                            td += "<tr class='gradeX'> \n";
                            td += "<td style='text-align:center;'>" + date.ToString().Substring(0, 6) + "</td> \n";
                            td += "<td style='text-align:center;'>" + LineInf.Value + "</td> \n";
                            td += "<td style='text-align:center;'>" + LineInf.Key + "</td> \n";
                            td += "<td style='text-align:center;'>" + Taget + "</td> \n";
                            td += "<td style='text-align:center;'>" + Real + "</td> \n";
                            //chart    { x: new Date(2012, 00, 1), y: 450 },
                            yearStr = date.ToString().Substring(0, 4);
                            //MonthStr = (DataTableUtils.toInt(date.ToString().Substring(4, 2))).ToString();
                            MonthStr = (DataTableUtils.toInt(date.ToString().Substring(4, 2)) - 1).ToString();
                            infStr[0] = "{x:new Date(" + yearStr + "," + MonthStr + ")," + "y:" + DataTableUtils.toString(Taget) + "}" + "," + infStr[0];
                            infStr[1] = "{x:new Date(" + yearStr + "," + MonthStr + ")," + "y:" + DataTableUtils.toString(Real) + "}" + "," + infStr[1];
                            if (Taget != 0)
                                RateSubStr = (DataTableUtils.toDouble(Real.ToString()) / DataTableUtils.toDouble(Taget.ToString()) * 100).ToString("F2");
                            else
                                RateSubStr = "0";
                            td += "<td style='text-align:center;'>" + RateSubStr + "</td> \n";
                            infStr[2] = "{x:new Date(" + yearStr + "," + MonthStr + ")," + "y:" + RateSubStr + ",indexLabel: " + "'" + RateSubStr + "%" + "'" + "}" + "," + infStr[2];
                            td += "</tr> \n";
                        }
                    }
                    infStr[3] = td;
                    return infStr;
                }
            }
            else
            {
                infStr[3] = td;
                return infStr;
            }
        }
        public string[] GetErrorInf(CheckBoxList LineList, string timetype, string TimeType, string StartDate = "Today", string EndDate = "Today", List<string> SelectLine = null)
        {
            //ALL Line

            double ErrorTimeSum = 0;
            string td = "";
            string ChartDataStr_Count = "";
            string ChartDataStr_Time = "";
            string[] infStr = new string[6];////0:ChartDataStr 1:ErrorCount 2:   3:td content:
            string[] ErrorType;
            string condition = "";
            string ViewCondition = "";
            string sortname = "";
            string CountCondition = "";
            string FinisCondition = "";
            string Location = "";
            int BreakCount = 1;
            int TimeCount = 0;
            int ErrorCount = 0;
            string ErrorStr = "";
            string url = "";
            Dictionary<string, int> ErrorType_ListD = new Dictionary<string, int>();
            Dictionary<string, double> ErrorTime_ListD = new Dictionary<string, double>();
            List<string> ErrorType_List = new List<string>();
            DataTable dt_er_sort;
            DataTable dT_errorFather;
            DataTable dT_errorSon;
            DataRow dr_son;
            DataTable dt_Line = DataTableUtils.DataTable_GetTable(ShareMemory.SQLAsm_WorkStation_Type, "工作站是否使用中=" + "'" + "1" + "'");
            //Line
            //if (SelectLine != null && SelectLine[0] != "0" && !string.IsNullOrEmpty(SelectLine[0]))
            //{
            //    for (int i = 0; i < SelectLine.Count; i++)
            //    {
            //        //if (i == SelectLine.Count - 1)
            //        //{
            //        //    if (StartDate == "Today" && EndDate == "Today")
            //        //        condition += "工作站編號=" + SelectLine[i] + " AND  時間紀錄>=" + "'" + DateTime.Now.ToString("yyyyMMdd") + "010101" + "'" + " AND " + "時間紀錄 <= " + "'" + DateTime.Now.ToString("yyyyMMdd") + "235959" + "'";
            //        //    else
            //        //        condition += "工作站編號=" + SelectLine[i] + " AND 時間紀錄>=" + "'" + StartDate + "'" + " AND " + "時間紀錄 <= " + "'" + EndDate + "'";
            //        //}
            //        //else
            //        //{
            //        //    if (StartDate == "Today" && EndDate == "Today")
            //        //        condition += "工作站編號=" + SelectLine[i] + " and 時間紀錄 >=" + "'" + DateTime.Now.ToString("yyyyMMdd") + "010101" + "'" + " AND " + "時間紀錄 <= " + "'" + DateTime.Now.ToString("yyyyMMdd") + "235959" + "' or ";
            //        //    else
            //        //        condition += "工作站編號=" + SelectLine[i] + " and 時間紀錄>=" + "'" + StartDate + "'" + " AND " + "時間紀錄 <= " + "'" + EndDate + "' or ";
            //        //}
            //        //if (i == 0)
            //        //{
            //        //    condition = "Where " + condition;
            //        //}
            //        if(i==0)
            //            condition = $" 工作站編號={SelectLine[i]} ";
            //        else
            //            condition += $" OR 工作站編號={SelectLine[i]} ";
            //    }
            //    condition = $" where ({condition}) and  Len( 結案判定類型 )>0 and 結案判定類型 is not null and 時間紀錄 >='{StartDate}' and 時間紀錄<='{EndDate}' ";
            //}
            ////condition += "工作站編號=" + SelectLine + " AND ";
            //else
            //{
            //}
            // time
            if (StartDate == "Today" && EndDate == "Today")
                condition = "where " + condition + " 時間紀錄>=" + "'" + DateTime.Now.ToString("yyyyMMdd") + "010101" + "'" + " AND " + "時間紀錄 <= " + "'" + DateTime.Now.ToString("yyyyMMdd") + "235959" + "'";
            else
                condition = "where " + condition + " 時間紀錄>=" + "'" + StartDate + "010101" + "'" + " AND " + "時間紀錄 <= " + "'" + EndDate + "235959" + "' and  Len( 結案判定類型 )>0 and 結案判定類型 is not null";

            //
            // Get Datatable
            DataTableUtils.Conn_String = GetConnByDekVisTmp;
            //string aa = "select * from " + ShareMemory.SQLAsm_WorkStation_ErrorMant + " " + condition;
            dT_errorFather = DataTableUtils.DataTable_GetTable("select * from " + ShareMemory.SQLAsm_WorkStation_ErrorMant + " " + condition, 0, 0);
            if (SelectLine != null)
            {
                if (dT_errorFather != null)
                {
                    dT_errorSon = dT_errorFather.Clone();
                    foreach (DataRow dr_fa in dT_errorFather.Rows)
                    {
                        dr_son = DataTableUtils.DataTable_GetDataRow($"select tb.異常維護編號,  ta.工作站編號,ta.排程編號,ta.時間紀錄 as 維護內容,tb.時間紀錄,tb.結案判定類型,tb.父編號 from 工作站異常維護資料表 as ta left join 工作站異常維護資料表 as tb on tb.父編號 = ta.異常維護編號  where ta.異常維護編號='{dr_fa["父編號"].ToString()}' and tb.結案判定類型 is not null and LEN(tb.結案判定類型) >0");
                        if (dr_son != null)
                        {
                            if (SelectLine.IndexOf("0") != -1)
                                dT_errorSon.ImportRow(dr_son);
                            else if (SelectLine.IndexOf(DataTableUtils.toString(dr_son["工作站編號"])) != -1)
                                dT_errorSon.ImportRow(dr_son);
                        }

                    }
                }
                else
                    dT_errorSon = null;
            }
            else
                dT_errorSon = null;

            if (dT_errorSon != null && dT_errorSon.Rows.Count != 0)
            {
                DataView dV_error = dT_errorSon.DefaultView;
                DataView dV_error_Count = dT_errorSon.DefaultView;
                var ErrorTyped = dT_errorSon.AsEnumerable().GroupBy(g => g.Field<string>("結案判定類型")).Where(w => !string.IsNullOrEmpty(w.Key)).Select(s => s.Key);
                foreach (string key in ErrorTyped)
                    ErrorType_ListD.Add(key, dT_errorSon.AsEnumerable().Where(w => w.Field<string>("結案判定類型") == key).Count());

                var dicSort = from objDic in ErrorType_ListD orderby objDic.Value descending select objDic;//dec descending
                BreakCount = 1;
                // TimeCount = (ErrorType_ListD.Count > ShareMemory.ShowErrorCount) ? ShareMemory.ShowErrorCount : ErrorType_ListD.Count;
                //20191115開會決定由原本的3筆，改成匯出全部
                TimeCount = ErrorType_ListD.Count;
                foreach (KeyValuePair<string, int> kvp in dicSort)
                // Ans
                // foreach (string ErrorKind in ErrorType_List)//ErrorKind
                {
                    if (kvp.Key == "" || kvp.Key.ToUpper() == "NOUSE") continue;
                    //Init
                    ErrorTimeSum = 0;
                    //Table
                    td += "<tr class='gradeX'style='text-align:center;'>";
                    td += "<td style='text-align:center;'>" + kvp.Key + "</td>";//Error Type
                    foreach (ListItem item in LineList.Items) //Station
                    {
                        if (item.Value == "0"|| item.Value=="") continue; //20220614 全部給空,多判斷空值
                        //ViewCondition = "工作站編號" + " = " + "'" + item.Value + "'" + " AND " + "異常原因類型" + " Like " + "'" + "%" + kvp.Key + "%" + "'";
                        ViewCondition = "結案判定類型" + " Like " + "'" + "%" + kvp.Key + "%" + "'";
                        dV_error.RowFilter = ViewCondition;//有結案判定類型的
                        if (dV_error.Count != 0)
                        {
                            ErrorCount = 0;
                            ErrorStr = "";
                            foreach (DataRow dr in dV_error.ToTable().Rows)
                            {
                                if (dt_Line != null && dt_Line.Rows.Count != 0)
                                {
                                    var LineSN = dt_Line.AsEnumerable().Where(w => w.Field<string>("工作站名稱") == item.Text).Select(s => s.Field<string>("工作站編號")).FirstOrDefault();
                                    var liveError = dT_errorSon.AsEnumerable().Where(w => dr["異常維護編號"].ToString() == w.Field<string>("異常維護編號") && w.Field<string>("工作站編號") == LineSN.ToString()).FirstOrDefault();
                                    if (liveError != null)
                                    {
                                        ErrorCount++;
                                        if (dV_error.ToTable().Rows.IndexOf(dr) == dV_error.ToTable().Rows.Count - 1)
                                            ErrorStr += liveError["排程編號"].ToString();
                                        else
                                            ErrorStr += liveError["排程編號"].ToString() + "$";
                                    }
                                }
                            }
                            if (!string.IsNullOrEmpty(ErrorStr))
                            {
                                var LineSnn = dt_Line.AsEnumerable().Where(w => w.Field<string>("工作站名稱") == item.Text).Select(s => s.Field<string>("工作站編號")).FirstOrDefault();
                                Location = GetConnByDekVisTmp.Contains("Hor") ? "Hor" : "Ver";
                                url = "Local=" + Location + ",ErrorLineNum=" + LineSnn + ",Errorkey=" + ErrorStr + ",Date_str=" + StartDate + ",Date_end=" + EndDate + ",ErrorType=" + kvp.Key;
                                if (ConfigurationManager.AppSettings["URL_ENCODE"] == "1")
                                    url = WebUtils.UrlStringEncode(url);
                                td += "<td style='text-align:center;'><a href='Asm_history.aspx?key=" + url + "'><div style='height:100%;width:100%'>" + ErrorCount + "</div></a></td>";
                            }
                            else
                            {
                                td += "<td style='text-align:center;'>" + ErrorCount + "</td>";
                            }
                        }
                        else
                        {
                            td += "<td style='text-align:center;'>" + dV_error.Count.ToString() + "</td>";
                        }
                    }
                    td += "</tr>";
                    //Chart
                    //chart format {  y: Num, indexLabel: Name },
                    dV_error_Count = dT_errorSon.DefaultView;
                    CountCondition = "結案判定類型" + " Like " + "'" + "%" + kvp.Key + "%" + "'";
                    dV_error_Count.RowFilter = CountCondition;
                    if (BreakCount <= TimeCount)
                    {
                        ChartDataStr_Count += "{y:" + (dV_error_Count.Count).ToString() + "," + "indexLabel:" + "'" + kvp.Key + "'" + "}" + ",";
                        change += kvp.Key + ",";
                        infStr[4] += kvp.Value + ",";
                    }
                    //時間計算
                    dt_er_sort = dV_error.ToTable();
                    //
                    //WorkTime
                    //work.工作時數()
                    //ErrorTimeSum = 結束-起始
                    foreach (DataRow dr in dt_er_sort.Rows)// some error type son
                    {
                        //                   var Starttimesrc = dt_er_sort.AsEnumerable().Where(w => w.Field<string>("異常維護編號") == dr["父編號"].ToString()).Select(s => s.Field<string>("時間紀錄")).FirstOrDefault();

                        work.工作時段_新增(8, 0, 12, 0);
                        work.工作時段_新增(13, 0, 17, 0);
                        DateTime StratTime = StrToDateTime(dr["維護內容"].ToString(), "yyyyMMddHHmmss");
                        DateTime Endtime = StrToDateTime(dr["時間紀錄"].ToString(), "yyyyMMddHHmmss");
                        TimeSpan tsp = work.工作時數(StratTime, Endtime);
                        ErrorTimeSum += tsp.TotalSeconds;
                        // if (!ErrorTime_ListD.ContainsKey(dr["結案判定類型"].ToString()))
                        //     ErrorTime_ListD.Add(kvp.Key, ErrorTimeSum);
                        //ErrorTimeSum += DataTableUtils.toDouble(dr["異常累積時間"].ToString());
                    }
                    if (BreakCount <= TimeCount)
                    {
                        if (BreakCount != TimeCount)
                            ErrorTime_ListD.Add(kvp.Key, ErrorTimeSum);
                        else
                        {
                            ErrorTime_ListD.Add(kvp.Key, ErrorTimeSum);
                            var dicTimeSort = from objDic in ErrorTime_ListD orderby objDic.Value descending select objDic;//dec descending
                            {
                                foreach (KeyValuePair<string, double> kvp_time in dicTimeSort)//時間
                                {
                                    sortname += kvp_time.Value + "," + kvp_time.Key + ":";
                                    infStr[5] += kvp_time.Value.ToString() + ",";
                                }
                                ChartDataStr_Time = sortresult(change, sortname, timetype);
                            }
                        }

                        //ChartDataStr_Time += "{y:" + ErrorTimeSum.ToString() + "," + "indexLabel:" + "'" + kvp.Key + ":" + ErrorTimeSum.ToString() + "秒" + "'" + "}" + "," + "\n";
                        BreakCount++;
                    }
                    dt_er_sort.Clear();
                }
                if (ChartDataStr_Count != "" && td != "")
                {
                    infStr[0] = ChartDataStr_Count.Remove(ChartDataStr_Count.LastIndexOf(","), 1);
                    //int a = ChartDataStr_Time.LastIndexOf(",");
                    //if(a != -1)
                    infStr[2] = ChartDataStr_Time.Remove(ChartDataStr_Time.LastIndexOf(","), 1);
                    infStr[3] = td;
                }
            }
            return infStr;
        }
        //讓時間的標籤跟隨次數
        public string sortresult(string change, string sortname, string timetype)
        {
            if (timetype == "")
                timetype = "(分鐘)";

            string[] chsplit = change.Split(',');
            string[] sosplit = sortname.Split(':');
            int i;
            string[] all = new string[chsplit.Length - 1];

            for (int a = 0; a < sosplit.Length - 1; a++)
            {
                i = 0;
                for (int j = 0; j < chsplit.Length - 1; j++)
                {
                    if (chsplit[j] == sosplit[a].Split(',')[1])
                    {
                        if (timetype == "(分鐘)")
                        {
                            double times = double.Parse(sosplit[a].Split(',')[0]);
                            double minutes = times / 60;
                            all[i] = "{y:" + minutes + "," + "indexLabel:" + "'" + sosplit[a].Split(',')[1] + "'" + "}" + "," + "\n";
                        }
                        if (timetype == "(小時)")
                        {
                            double times = double.Parse(sosplit[a].Split(',')[0]);
                            double minutes = times / 3600;
                            all[i] = "{y:" + minutes + "," + "indexLabel:" + "'" + sosplit[a].Split(',')[1] + "'" + "}" + "," + "\n";
                        }

                        break;
                    }
                    else
                        i++;
                }
            }
            string result = "";
            for (int b = 0; b < all.Length; b++)
            {
                result += all[b];
            }
            return result;
        }
        public string[] GetComplianceInf(DropDownList LineList, string TimeType, string StartDate = "Today", string EndDate = "Today", string SelectLine = null)
        {
            int onTaget = 0;
            int unTaget = 0;
            int onTagetSum = 0;
            string RateSubStr = "";
            string td = "";
            string StartDateDay = "";
            string EndDateDay = "";
            string yearStr = "";
            string MonthStr = "";
            string dayStr = "";
            string hourStr = "";
            string[] infStr = new string[4];
            Dictionary<string, int> WorkStationInf_List = new Dictionary<string, int>();
            List<string> dateList = new List<string>();
            DataTable dt_OnTime = new DataTable();
            if (SelectLine == "0")
                SelectLine = null;
            WorkStationInf_List = GetWorkPointInf(SelectLine);
            if (WorkStationInf_List.Count > 1) //all line
            {
                if (TimeType == "day")
                {
                    StartDateDay = DateTime.Now.ToString("yyyyMMdd");
                    EndDateDay = DateTime.Now.ToString("yyyyMMdd");
                }
                else
                {
                    StartDateDay = StartDate.Substring(0, 8);//unit day
                    EndDateDay = EndDate.Substring(0, 8);//unit day
                }
                dateList = TrsDateList(TimeType, StartDateDay, EndDateDay);
                foreach (string date in dateList)
                {
                    td += "<tr class='gradeX'> \n";
                    td += "<td style='text-align:center;'>" + date.ToString() + "</td> \n";
                    foreach (KeyValuePair<string, int> LineInf in WorkStationInf_List)
                    {
                        onTaget = GetOnTagetPiece(TimeType, LineInf.Value, date.ToString(), date.ToString(), ref dt_OnTime);
                        onTagetSum += onTaget;
                        td += "<td style='text-align:center;'>" + onTaget + "</td> \n";
                    }
                    td += "</tr> \n";
                }
                infStr[0] = SeparationOnTaget(dt_OnTime, onTimeStatus.超前, WorkStationInf_List);
                infStr[1] = SeparationOnTaget(dt_OnTime, onTimeStatus.落後, WorkStationInf_List);
                infStr[3] = td;
                return infStr;
            }
            else //single
            {
                infStr[3] = td;
                return infStr;
            }
        }
        public string DispatchData(Dictionary<string, string> _dc_parameter, Dictionary<string, string> _dc_PageInf)
        {
            string td = "";
            string Condition = "";
            string[] strTime;
            int PredictionProgress = 0;
            bool Advance = false;
            DataTableUtils.Conn_String = GetConnByDekVisTmp;
            DateTime Now = DateTime.Now;
            DateTime Start;
            TimeSpan ts;
            DataRow Dr_Craft;
            if (_dc_parameter.Count != 0)
            {
                switch (_dc_parameter["Key"])
                {
                    case "ErrorChat":
                        //Pageinf
                        _dc_PageInf.Add("KeyTitle", "異常明細");
                        _dc_PageInf.Add("SubKeyTitle", "產線名稱");
                        _dc_PageInf.Add("ValueTitle", "異常類別");
                        _dc_PageInf.Add("Value", _dc_parameter["Value"].ToString());
                        _dc_PageInf.Add("FromPageAspx", "Asm_Cahrt_Error.aspx");
                        _dc_PageInf.Add("FromPageTitle", "產線異常原因");
                        //Ans Dc
                        if (_dc_parameter["StartTime"] == "Today" && _dc_parameter["EndTime"] == "Today")

                            Condition = "where " + "工作站異常紀錄資料表.工作站編號=" + _dc_parameter["LineNum"] + " AND " + " 異常起始時間>=" + "'" + DateTime.Now.ToString("yyyyMMdd") + "010101" + "'" + " AND " + "異常起始時間 <= " + "'" + DateTime.Now.ToString("yyyyMMdd") + "235959" + "'" + " AND " + "異常原因" + " Like " + "'" + "%" + _dc_parameter["Value"] + "%" + "'";
                        else
                            Condition = "where " + "工作站異常紀錄資料表.工作站編號=" + _dc_parameter["LineNum"] + " AND " + " 異常起始時間>=" + "'" + _dc_parameter["StartTime"] + "'" + " AND " + "異常起始時間 <= " + "'" + _dc_parameter["EndTime"] + "'" + " AND " + "異常原因" + " Like " + "'" + "%" + _dc_parameter["Value"] + "%" + "'";
                        // Get Datatable
                        string ss = "select 工作站型態資料表.工作站名稱,工作站異常紀錄資料表.工作站編號,排程編號,異常起始時間,異常排除時間,異常累積時間 from " + ShareMemory.SQLAsm_WorkStation_Error + "  INNER JOIN 工作站型態資料表 ON 工作站異常紀錄資料表.工作站編號 = 工作站型態資料表.工作站編號 " + Condition;
                        DataTable dT_error = DataTableUtils.DataTable_GetTable("select 工作站型態資料表.工作站名稱,工作站異常紀錄資料表.工作站編號,排程編號,異常起始時間,異常排除時間,異常累積時間 from " + ShareMemory.SQLAsm_WorkStation_Error + "  INNER JOIN 工作站型態資料表 ON 工作站異常紀錄資料表.工作站編號 = 工作站型態資料表.工作站編號 " + Condition, 0, 0);
                        // Ans Data //組裝資料表.CUSTNM FROM 工作站狀態資料表  INNER JOIN 組裝資料表 ON 工作站狀態資料表.排程編號 = 組裝資料表.排程編號
                        dT_error.Columns.Add("異常歷程");
                        if (dT_error != null)
                        {
                            _dc_PageInf.Add("SubKey", dT_error.Rows[0]["工作站名稱"].ToString());
                            //
                            foreach (DataRow dr in dT_error.Rows)
                            {
                                ts = new TimeSpan(0, 0, DataTableUtils.toInt(dr["異常累積時間"].ToString()));
                                td += "<tr class='gradeX'> \n";
                                //排程編號
                                td += "<td style='text-align:center;'>" + dr["排程編號"].ToString() + "</td> \n";
                                //==========================異常起始時間
                                if (DataTableUtils.toLong(dr["異常起始時間"].ToString()) != 0)
                                {
                                    Start = DateTime.ParseExact(DataTableUtils.toString(dr["異常起始時間"]), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                                    td += "<td style='text-align:center;'>" + Start.ToString("yyyy-MM-dd HH:mm:ss") + "</td> \n";
                                }
                                else
                                    td += "<td style='text-align:center;'>" + "0" + "</td> \n";
                                //==========================
                                if (DataTableUtils.toLong(dr["異常排除時間"].ToString()) != 0)
                                {
                                    //異常排除時間
                                    td += "<td style='text-align:center;'>" + DateTime.ParseExact(DataTableUtils.toString(dr["異常排除時間"]), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture).ToString("yyyy-MM-dd HH:mm:ss") + "</td> \n";
                                    //異常累積時間
                                    if (ts.Days != 0)
                                        td += "<td style='text-align:center;'>" + ts.Days.ToString() + "日" + ts.Hours.ToString().PadLeft(2, '0') + "時" + ts.Minutes.ToString().PadLeft(2, '0') + "分" + ts.Seconds.ToString().PadLeft(2, '0') + "秒" + "</td> \n";
                                    else if (ts.Hours != 0)
                                        td += "<td style='text-align:center;'>" + ts.Hours.ToString().PadLeft(2, '0') + "時" + ts.Minutes.ToString().PadLeft(2, '0') + "分" + ts.Seconds.ToString().PadLeft(2, '0') + "秒" + "</td> \n";
                                    else
                                        td += "<td style='text-align:center;'>" + ts.Minutes.ToString().PadLeft(2, '0') + "分" + ts.Seconds.ToString().PadLeft(2, '0') + "秒" + "</td> \n";
                                }
                                else
                                {
                                    //異常排除時間
                                    td += "<td style='text-align:center;'>" + "處理中..." + "</td> \n";
                                    //異常累積時間
                                    if (DataTableUtils.toLong(dr["異常起始時間"].ToString()) != 0)
                                    {
                                        Start = DateTime.ParseExact(DataTableUtils.toString(dr["異常起始時間"]), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                                        ts = Now - Start;
                                        td += "<td style='text-align:center;'>" + ts.Days.ToString() + "日" + ts.Hours.ToString().PadLeft(2, '0') + "時" + ts.Minutes.ToString().PadLeft(2, '0') + "分" + ts.Seconds.ToString().PadLeft(2, '0') + "秒" + "</td> \n";
                                    }
                                    else
                                        td += "<td style='text-align:center;'>" + "0" + "</td> \n";
                                }
                                //==========================
                                if (dr["異常歷程"].ToString() == "")
                                {
                                    string url = "Key=ErrorChatDetail" + ","
                                       + "LineNum=" + dT_error.Rows[0]["工作站編號"].ToString() + ","
                                       + "Value1=" + dr["排程編號"].ToString() + ","
                                       + "Value2=" + _dc_parameter["Value"].ToString() + ","
                                       + "SrcApsx=" + _dc_parameter["Key"].ToString() + "-" + _dc_parameter["LineNum"].ToString() + "-" + _dc_parameter["Value"].ToString() + "-" + _dc_parameter["StartTime"].ToString() + "-" + _dc_parameter["EndTime"].ToString();

                                    td += "<td style='text-align:center;'>" + "<u><a href='Asm_Cahrt_Detail.aspx?key=" + WebUtils.UrlStringEncode(url) + "'>" + "歷程" + "</a></u>" + " " + "</td> \n";
                                }

                                else
                                    td += "<td style='text-align:center;'>" + "空白" + "</td> \n";
                                //==========================
                                if (dr["工作站編號"].ToString() == "")
                                {
                                    //none
                                }
                                //==========================
                                td += "</tr> \n";
                            }
                        }
                        break;
                    case "ErrorChatDetail":
                        //Pageinf
                        if (_dc_parameter.ContainsKey("SrcApsx") && _dc_parameter["SrcApsx"].ToString() != "")
                            _dc_PageInf.Add("FromPageAspx", "Asm_Cahrt_Detail.aspx?" + GetSrcAspStr("Asm_Cahrt_Detail", _dc_parameter["SrcApsx"].ToString()));
                        else
                            _dc_PageInf.Add("FromPageAspx", "Asm_Cahrt_Detail.aspx");
                        _dc_PageInf.Add("FromPageTitle", "異常分類清單");
                        _dc_PageInf.Add("KeyTitle", "異常歷程");
                        _dc_PageInf.Add("SubKeyTitle", "排程編號");
                        _dc_PageInf.Add("SubKey", _dc_parameter["Value1"].ToString());
                        _dc_PageInf.Add("ValueTitle", "異常類別");
                        _dc_PageInf.Add("Value", _dc_parameter["Value2"].ToString());//key
                                                                                     //_dc_PageInf.Add("Value2", _dc_parameter["Value2"].ToString());//errortype
                        Condition = "排程編號=" + "'" + _dc_parameter["Value1"].ToString() + "'" + " AND " + "異常原因類型=" + "'" + _dc_parameter["Value2"].ToString() + "'";
                        //Ans Dc
                        // Get Datatable
                        DataTable dt_ErrorDetail = DataTableUtils.DataTable_GetTable("select 維護人員姓名,維護人員單位,維護內容,時間紀錄,處理狀態 from " + ShareMemory.SQLAsm_WorkStation_ErrorMant + " where " + Condition, 0, 0);
                        //// Ans Data //組裝資料表.CUSTNM FROM 工作站狀態資料表  INNER JOIN 組裝資料表 ON 工作站狀態資料表.排程編號 = 組裝資料表.排程編號
                        //
                        if (dt_ErrorDetail != null)
                        {
                            //dt_ErrorDetail.Columns.Add("累積時間");
                            //_dc_PageInf.Add("SubKey", dt_ErrorDetail.Rows[0]["工作站名稱"].ToString());
                            //

                            for (int i = 0; i < dt_ErrorDetail.Rows.Count; i++)
                            {
                                td += "<tr class='gradeX'> \n";

                                for (int j = 0; j < dt_ErrorDetail.Columns.Count; j++)
                                {
                                    if (dt_ErrorDetail.Columns[j].ColumnName == "維護人員姓名")//維護人員姓名
                                    {
                                        //td += "<td style='text-align:center;'>" + DataTableUtils.toString(dt_ErrorDetail.Rows[i][j]) + "</td> \n";
                                        strTime = dt_ErrorDetail.Rows[i]["時間紀錄"].ToString().Split(' ');
                                        td += "<td style='text-align:center;'>" + DataTableUtils.toString(dt_ErrorDetail.Rows[i][j]) + "<br>"
                                            + "<span style='font-size:2px ;text-align:right;color:gray;'> " + strTime[0] + "<br>" + strTime[1] + " " + strTime[2] + "</ span ></td> \n";
                                    }
                                    else if (dt_ErrorDetail.Columns[j].ColumnName == "時間紀錄")
                                    {
                                        //none
                                    }
                                    else if (dt_ErrorDetail.Columns[j].ColumnName == "處理狀態")
                                    {
                                        if (DataTableUtils.toString(dt_ErrorDetail.Rows[i]["處理狀態"]).ToUpper() != "TRUE")
                                            td += "<td style='text-align:center;color:red;'>" + "處理" + "</td> \n";
                                        else
                                            td += "<td style='text-align:center;color:green;'>" + "結案" + "</td> \n";
                                    }
                                    //else if (dt_ErrorDetail.Columns[j].ColumnName == "累積時間")
                                    //{
                                    //}
                                    else
                                        td += "<td style='text-align:center;'>" + DataTableUtils.toString(dt_ErrorDetail.Rows[i][j]) + "</td> \n";
                                }
                                td += "</tr> \n";
                            }

                        }
                        break;
                    case "Product_Detail"://{ "工藝名稱", "組裝人員", "標準工時", "起始時間", "完成時間", "預計完成" };
                                          // page inf
                        _dc_PageInf.Add("KeyTitle", "組裝明細");
                        _dc_PageInf.Add("SubKeyTitle", "產線名稱");
                        _dc_PageInf.Add("ValueTitle", "排程編號");
                        _dc_PageInf.Add("Value", _dc_parameter["Value"].ToString());
                        //Data Process
                        Condition = "排程編號=" + "'" + _dc_parameter["Value"].ToString() + "'";
                        // DataTable dt = DataTableUtils.DataTable_GetTable("select 工作站型態資料表.工作站名稱,工作站型態資料表.工藝工程編號,執行工藝,實際啟動時間,實際完成時間,組裝累積時間,派工狀態,工作站狀態資料表.工作站編號 from " + ShareMemory.SQLAsm_WorkStation_State + " INNER JOIN 工作站型態資料表 ON 工作站狀態資料表.工作站編號 = 工作站型態資料表.工作站編號 " + Condition);
                        DataTable dt = DataTableUtils.DataTable_GetTable("select 工作站型態資料表.工作站名稱,工作站型態資料表.工藝流程點編號,執行工藝,派工狀態,工作站狀態資料表.工作站編號 ,狀態,進度,實際啟動時間,實際完成時間,暫停時間,組裝累積時間,異常累積時間 from " + ShareMemory.SQLAsm_WorkStation_State + " INNER JOIN 工作站型態資料表 ON 工作站狀態資料表.工作站編號 = 工作站型態資料表.工作站編號 " + " where " + Condition);
                        DataTable dt_log = DataTableUtils.DataTable_GetTable("select 員工資料表.員工姓名,登入人員,啟動時間,停止時間,完成時間 from " + ShareMemory.SQLAsm_WorkStation_Log + " INNER JOIN 員工資料表 ON 工作站歷程資料表.登入人員 = 員工資料表.員工編號 " + " where " + Condition);
                        //GetPredictionTimeStatus
                        if (dt.Rows.Count != 0 && dt.Rows[0]["狀態"].ToString() != "未動工")
                            Advance = GetPredictionTimeStatus(_dc_parameter["Value"].ToString(), ref PredictionProgress);
                        //"工藝名稱"================================================ 
                        if (dt.Rows.Count != 0 && dt_log != null)
                        {
                            _dc_PageInf.Add("SubKey", dt.Rows[0]["工作站名稱"].ToString());
                            _dc_PageInf.Add("FromPageTitle", dt.Rows[0]["工作站名稱"].ToString());
                            _dc_PageInf.Add("FromPageAspx", "Asm_LineOverView.aspx?LineNum=" + dt.Rows[0]["工作站編號"].ToString() + ",ReqType=Line");
                            foreach (DataRow dr in dt.Rows)
                            {
                                td += "<tr class='gradeX'> \n";
                                //工藝名稱================================================ 
                                if (dt.Rows[0]["執行工藝"].ToString() != null && dt.Rows[0]["執行工藝"].ToString() != "" && dt.Rows[0]["執行工藝"].ToString().ToUpper() != "NULL")
                                    td += "<td style='text-align:center;'>" + dt.Rows[0]["執行工藝"] + "</td> \n";
                                else if (dt.Rows[0]["工藝流程點編號"].ToString() != null && dt.Rows[0]["工藝流程點編號"].ToString() != "" && dt.Rows[0]["工藝流程點編號"].ToString().ToUpper() != "NULL")
                                {
                                    Condition = "工作流程序號=" + "'" + dt.Rows[0]["工藝流程點編號"].ToString() + "'";
                                    Dr_Craft = DataTableUtils.DataTable_GetDataRow(ShareMemory.SQLAsm_WorkPoint, Condition);
                                    if (Dr_Craft != null)
                                        td += "<td style='text-align:center;'>" + Dr_Craft["工作流程名稱"] + "</td> \n";
                                    else
                                        td += "<td style='text-align:center;'>" + "未定義" + "</td> \n";
                                }
                                else
                                    td += "<td style='text-align:center;'>" + "未定義" + "</td> \n";
                                //"組裝人員"================================================ 
                                if (dt.Rows[0]["派工狀態"].ToString() != null && dt.Rows[0]["派工狀態"].ToString() != "" && dt.Rows[0]["派工狀態"].ToString().ToUpper() != "NULL")
                                    td += "<td style='text-align:center;'>" + dt.Rows[0]["派工狀態"] + "</td> \n";
                                else
                                {
                                    if (dt_log.Rows.Count != 0)
                                        td += "<td style='text-align:center;'>" + dt_log.Rows[0]["員工姓名"].ToString() + "</td> \n";
                                    else
                                        td += "<td style='text-align:center;'>" + "未定義" + "</td> \n";
                                }
                                //"標準工時,平均工時"================================================ 
                                Condition = "工作流程序號=" + "'" + dt.Rows[0]["工藝流程點編號"].ToString() + "'";
                                Dr_Craft = DataTableUtils.DataTable_GetDataRow("select 工藝流程點資料表.工作流程名稱,工藝名稱資料表.最大工時,工藝名稱資料表.最小工時,工藝名稱資料表.目前工時,工藝名稱資料表.標準工時 from " + ShareMemory.SQLAsm_WorkPoint + " INNER JOIN 工藝名稱資料表 ON 工藝流程點資料表.工作流程名稱 = 工藝名稱資料表.工藝名稱 " + " where " + Condition);
                                //Dr_Craft = DataTableUtils.DataTable_GetDataRow(ShareMemory.SQLAsm_WorkCraft_Name, Condition);
                                if (Dr_Craft != null)
                                {
                                    ts = new TimeSpan(0, 0, DataTableUtils.toInt(Dr_Craft["標準工時"].ToString()));
                                    td += "<td style='text-align:center;'>" + (int)ts.TotalMinutes + "分" + "</td> \n";
                                    //ts = new TimeSpan(0, 0, DataTableUtils.toInt(Dr_Craft["目前工時"].ToString()));
                                    //td += "<td style='text-align:center;'>" + (int)ts.TotalMinutes + "分" + "</td> \n";
                                }
                                else
                                {
                                    td += "<td style='text-align:center;'>" + "未定義" + "</td> \n";
                                    td += "<td style='text-align:center;'>" + "未定義" + "</td> \n";
                                }
                                //目前工時-已使用時間================================================ 
                                //ts = new TimeSpan(0, 0, DataTableUtils.toInt(Dr_Craft["標準工時"].ToString()));
                                //td += "<td style='text-align:center;'>" + PredictionProgress * (int)ts.TotalMinutes / 100 + "分" + "</td> \n";
                                //組裝累積時間================================================ 
                                if (dt.Rows[0]["組裝累積時間"].ToString() != null && dt.Rows[0]["組裝累積時間"].ToString() != "" && dt.Rows[0]["組裝累積時間"].ToString().ToUpper() != "NULL")
                                {
                                    ts = new TimeSpan(0, 0, DataTableUtils.toInt(dt.Rows[0]["組裝累積時間"].ToString()));
                                    td += "<td style='text-align:center;'>" + (int)ts.TotalMinutes + "分" + "</td> \n";
                                }
                                else
                                    td += "<td style='text-align:center;'>" + "-" + "</td> \n";
                                //異常累積時間================================================ 
                                if (dt.Rows[0]["異常累積時間"].ToString() != null && dt.Rows[0]["異常累積時間"].ToString() != "" && dt.Rows[0]["異常累積時間"].ToString().ToUpper() != "NULL")
                                {
                                    ts = new TimeSpan(0, 0, DataTableUtils.toInt(dt.Rows[0]["異常累積時間"].ToString()));
                                    td += "<td style='text-align:center;'>" + (int)ts.TotalMinutes + "分" + "</td> \n";
                                }
                                else
                                    td += "<td style='text-align:center;'>" + "-" + "</td> \n";
                                //預定進度
                                // td += "<td style='text-align:center;'>" + PredictionProgress + "%" + "</td> \n";
                                //實際進度================================================ 
                                if ((PredictionProgress > DataTableUtils.toInt(dt.Rows[0]["進度"].ToString())) || !Advance)
                                    td += "<td style='text-align:center;color:red;'>" + dt.Rows[0]["進度"].ToString() + "%" + " || " + PredictionProgress + "%" + "</td> \n";
                                else
                                    td += "<td style='text-align:center;color:green;'>" + dt.Rows[0]["進度"].ToString() + "%" + " || " + PredictionProgress + "%" + "</td> \n";
                                //"起始時間"================================================ 
                                if (dr["實際啟動時間"].ToString() != null && dr["實際啟動時間"].ToString() != "" && dr["實際啟動時間"].ToString().ToUpper() != "NULL")
                                    td += "<td style='text-align:center;'>" + DateTime.ParseExact(DataTableUtils.toString(dr["實際啟動時間"]), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture).ToString("MM月dd日-HH:mm:ss") + "</td> \n";
                                else
                                    td += "<td style='text-align:center;'>" + "未動工" + "</td> \n";
                                //"預計完成"================================================ 
                                if (Dr_Craft != null && dr["實際啟動時間"].ToString() != null && dr["實際啟動時間"].ToString() != "" && dr["實際啟動時間"].ToString().ToUpper() != "NULL")
                                    td += "<td style='text-align:center;'>" + GetPredictionTime(dr["實際啟動時間"].ToString(), Dr_Craft["標準工時"].ToString(), "早班", WorkType.人).ToString("MM月dd日-HH:mm:ss") + "</td> \n";
                                else
                                    td += "<td style='text-align:center;'>" + "" + "</td> \n";
                                //"停止時間", 
                                // if (dr["暫停時間"].ToString() != null && dr["暫停時間"].ToString() != "" && dr["暫停時間"].ToString().ToUpper() != "NULL")
                                //     td += "<td style='text-align:center;'>" + DateTime.ParseExact(DataTableUtils.toString(dr["暫停時間"]), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture).ToString("dd-HH:m:ss") + "</td> \n";
                                // else
                                //     if (dt.Rows[0]["狀態"].ToString() != "未動工")
                                //     td += "<td style='text-align:center;'>" + "組裝中..." + "</td> \n";
                                // else
                                //     td += "<td style='text-align:center;'>" + "未動工" + "</td> \n";
                                //"實際完成"================================================ 
                                if (dr["實際完成時間"].ToString() != null && dr["實際完成時間"].ToString() != "" && dr["實際完成時間"].ToString().ToUpper() != "NULL")
                                    td += "<td style='text-align:center;'>" + DateTime.ParseExact(DataTableUtils.toString(dr["實際完成時間"]), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture).ToString("MM月dd日-HH:mm:ss") + "</td> \n";
                                else
                                    td += "<td style='text-align:center;'>" + "未完成" + "</td> \n";
                                td += "</tr> \n";
                            }
                        }
                        break;
                    case "ErrorSearch_Detail":
                        //Pageinf
                        _dc_PageInf.Add("FromPageAspx", "Asm_ErrorSearch.aspx");
                        _dc_PageInf.Add("FromPageTitle", "維護歷程搜尋");
                        _dc_PageInf.Add("KeyTitle", "異常明細");
                        _dc_PageInf.Add("SubKeyTitle", "排程編號");
                        _dc_PageInf.Add("SubKey", _dc_parameter["Value1"].ToString());
                        _dc_PageInf.Add("ValueTitle", "異常類別");
                        _dc_PageInf.Add("Value", _dc_parameter["Value2"].ToString());//key
                                                                                     //_dc_PageInf.Add("Value2", _dc_parameter["Value2"].ToString());//errortype
                        Condition = "排程編號=" + "'" + _dc_parameter["Value1"].ToString() + "'" + " AND " + "異常原因類型=" + "'" + _dc_parameter["Value2"].ToString() + "'";
                        //Ans Dc
                        // Get Datatable
                        DataTable dt_ChartErrorDetail = DataTableUtils.DataTable_GetTable("select 維護人員姓名,維護人員單位,維護內容,時間紀錄,處理狀態 from " + ShareMemory.SQLAsm_WorkStation_ErrorMant + " where " + Condition, 0, 0);
                        //// Ans Data //組裝資料表.CUSTNM FROM 工作站狀態資料表  INNER JOIN 組裝資料表 ON 工作站狀態資料表.排程編號 = 組裝資料表.排程編號
                        //
                        if (dt_ChartErrorDetail != null)
                        {
                            //dt_ErrorDetail.Columns.Add("累積時間");
                            //_dc_PageInf.Add("SubKey", dt_ErrorDetail.Rows[0]["工作站名稱"].ToString());
                            //
                            for (int i = 0; i < dt_ChartErrorDetail.Rows.Count; i++)
                            {
                                td += "<tr class='gradeX'> \n";

                                for (int j = 0; j < dt_ChartErrorDetail.Columns.Count; j++)
                                {
                                    if (dt_ChartErrorDetail.Columns[j].ColumnName == "維護人員姓名")//維護人員姓名
                                    {
                                        //td += "<td style='text-align:center;'>" + DataTableUtils.toString(dt_ErrorDetail.Rows[i][j]) + "</td> \n";
                                        strTime = dt_ChartErrorDetail.Rows[i]["時間紀錄"].ToString().Split(' ');
                                        td += "<td style='text-align:center;'>" + DataTableUtils.toString(dt_ChartErrorDetail.Rows[i][j]) + "<br>"
                                            + "<span style='font-size:2px ;text-align:right;color:gray;'> " + strTime[0] + "<br>" + strTime[1] + " " + strTime[2] + "</ span ></td> \n";
                                    }
                                    else if (dt_ChartErrorDetail.Columns[j].ColumnName == "時間紀錄")
                                    {
                                        //none
                                    }
                                    else if (dt_ChartErrorDetail.Columns[j].ColumnName == "處理狀態")
                                    {
                                        if (DataTableUtils.toString(dt_ChartErrorDetail.Rows[i]["處理狀態"]).ToUpper() != "TRUE")
                                            td += "<td style='text-align:center;color:red;'>" + "處理" + "</td> \n";
                                        else
                                            td += "<td style='text-align:center;color:green;'>" + "結案" + "</td> \n";
                                    }
                                    //else if (dt_ErrorDetail.Columns[j].ColumnName == "累積時間")
                                    //{
                                    //}
                                    else
                                        td += "<td style='text-align:center;'>" + DataTableUtils.toString(dt_ChartErrorDetail.Rows[i][j]) + "</td> \n";
                                }
                                td += "</tr> \n";
                            }
                        }
                        break;
                    case "Asm_LineOverView_FinishSpecialFunction":
                        //Pageinf
                        _dc_PageInf.Add("FromPageAspx", "Asm_LineOverView.aspx?LineNum=" + _dc_parameter["LineNum"].ToString() + ",ReqType=Line");
                        _dc_PageInf.Add("FromPageTitle", "一般");
                        _dc_PageInf.Add("KeyTitle", "[鍊一廠]出貨預估");
                        _dc_PageInf.Add("SubKeyTitle", "午前到貨");
                        //_dc_PageInf.Add("SubKey", "8");
                        _dc_PageInf.Add("ValueTitle", "午後到貨");
                        //_dc_PageInf.Add("Value", "9");//key LineNum
                        string Condition1 = "工作站編號 = " + "'" + _dc_parameter["LineNum"].ToString() + "'" + " AND " + "實際完成時間 >=" + "'" + DateTime.Now.ToString("yyyyMMdd000000").ToString() + "'" + " AND " + "狀態=" + "'" + "完成" + "'";
                        Condition = Condition1;
                        DataTable dt_finish = DataTableUtils.DataTable_GetTable("select 組裝資料表.CUSTNM,組裝資料表.排程編號,實際完成時間 from " + ShareMemory.SQLAsm_WorkStation_State + " INNER JOIN 組裝資料表 ON 工作站狀態資料表.排程編號 = 組裝資料表.排程編號 " + " where " + Condition);

                        //// Ans Data //組裝資料表.CUSTNM FROM 工作站狀態資料表  INNER JOIN 組裝資料表 ON 工作站狀態資料表.排程編號 = 組裝資料表.排程編號
                        DataView dv_finsh = dt_finish.DefaultView;
                        dv_finsh.RowFilter = "實際完成時間 < " + "'" + DateTime.Now.ToString("yyyyMMdd") + "120000" + "'";
                        _dc_PageInf.Add("SubKey", dv_finsh.Count.ToString() + " 件");
                        dv_finsh = dt_finish.DefaultView;
                        dv_finsh.RowFilter = "實際完成時間 >= " + "'" + DateTime.Now.ToString("yyyyMMdd") + "120000" + "'";
                        _dc_PageInf.Add("Value", dv_finsh.Count.ToString() + " 件");//key LineNum
                        if (dt_finish != null)
                        {
                            foreach (DataRow dr_finish in dt_finish.Rows)
                            {
                                td += "<tr class='gradeX'> \n";
                                //======
                                if (dt_finish.Rows[0]["CUSTNM"].ToString() != null && dt_finish.Rows[0]["CUSTNM"].ToString() != "" && dt_finish.Rows[0]["CUSTNM"].ToString().ToUpper() != "NULL")
                                {
                                    td += "<td style='text-align:center;'>" + dr_finish["CUSTNM"].ToString() + "</td> \n";
                                }
                                else
                                    td += "<td style='text-align:center;'>" + "-" + "</td> \n";
                                //======
                                if (dt_finish.Rows[0]["排程編號"].ToString() != null && dt_finish.Rows[0]["排程編號"].ToString() != "" && dt_finish.Rows[0]["排程編號"].ToString().ToUpper() != "NULL")
                                {
                                    td += "<td style='text-align:center;'>" + dr_finish["排程編號"].ToString() + "</td> \n";
                                }
                                else
                                    td += "<td style='text-align:center;'>" + "-" + "</td> \n";
                                //======
                                if (dt_finish.Rows[0]["實際完成時間"].ToString() != null && dt_finish.Rows[0]["實際完成時間"].ToString() != "" && dt_finish.Rows[0]["實際完成時間"].ToString().ToUpper() != "NULL")
                                {
                                    Start = DateTime.ParseExact(dr_finish["實際完成時間"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);//  dr_finish["實際完成時間"].ToString()
                                                                                                                                                                   // td += "<td style='text-align:center;'>" + Start.ToString("MM月dd日HH時mm分ss秒") + "</td> \n";
                                                                                                                                                                   // send date
                                    td += "<td style='text-align:center;'>" + Start.AddDays(1).ToString("MM月dd日") + "</td> \n";
                                    // send time
                                    if (DataTableUtils.toInt(dr_finish["實際完成時間"].ToString().Substring(8, 2)) < 12)
                                        td += "<td style='text-align:center;'>" + "上午" + "</td> \n";
                                    else
                                        td += "<td style='text-align:center;'>" + "下午" + "</td> \n";
                                }
                                else
                                    td += "<td style='text-align:center;'>" + "-" + "</td> \n";
                                //======
                            }
                            td += "</tr> \n";

                        }
                        break;
                }
            }
            else
                td = NoDataProcess();
            return td;
        }
        public bool GetPredictionTimeStatus(string Key, ref int _PredictionProgress)
        {
            bool AdvanceStr = false;
            if (GetConnByDekVisTmp.IndexOf("DetaVisHor") < 0)
            {
                List<string> PredictionTimeInf = new List<string>();
                AdvanceStr = false;
                string Condition = "排程編號=" + "'" + Key + "'";
                DataTableUtils.Conn_String = GetConnByDekVisTmp;
                // DataTable dt = DataTableUtils.DataTable_GetTable("select 工作站型態資料表.工作站名稱,工作站型態資料表.工藝工程編號,執行工藝,實際啟動時間,實際完成時間,組裝累積時間,派工狀態,工作站狀態資料表.工作站編號 from " + ShareMemory.SQLAsm_WorkStation_State + " INNER JOIN 工作站型態資料表 ON 工作站狀態資料表.工作站編號 = 工作站型態資料表.工作站編號 " + Condition);
                DataRow dr_status = DataTableUtils.DataTable_GetDataRow("select 工作站型態資料表.工作站名稱,工作站型態資料表.工藝流程點編號,執行工藝,派工狀態,工作站狀態資料表.工作站編號 ,進度,狀態,實際啟動時間,實際完成時間,組裝累積時間 from " + ShareMemory.SQLAsm_WorkStation_State + " INNER JOIN 工作站型態資料表 ON 工作站狀態資料表.工作站編號 = 工作站型態資料表.工作站編號 " + " where " + Condition);
                DataRow Dr_Craft = null;
                DateTime PredictionTime;
                DateTime FinishTime;
                DateTime StartTime;
                DateTime RestTime;
                DateTime Result_StartTime;
                double ProcessSec;
                TimeSpan ts;
                //"工藝名稱", 
                if (dr_status != null)
                {
                    //"標準工時", 
                    Condition = "工作流程序號=" + "'" + dr_status["工藝流程點編號"].ToString() + "'";

                    Dr_Craft = DataTableUtils.DataTable_GetDataRow("select 工藝流程點資料表.工作流程名稱,工藝名稱資料表.最大工時,工藝名稱資料表.最小工時,工藝名稱資料表.目前工時,工藝名稱資料表.標準工時 from " + ShareMemory.SQLAsm_WorkPoint + " INNER JOIN 工藝名稱資料表 ON 工藝流程點資料表.工作流程名稱 = 工藝名稱資料表.工藝名稱 " + " where " + Condition);

                    //Dr_Craft = DataTableUtils.DataTable_GetDataRow(ShareMemory.SQLAsm_WorkCraft_Name, Condition);
                    if (Dr_Craft != null)
                    {
                        //DateTime.ParseExact(DataTableUtils.toString(dr_status["實際完成時間"]), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture).ToString("dd日HH時mm分ss秒");
                        //  GetPredictionTime(dr_status["啟動時間"].ToString(), Dr_Craft["標準工時"].ToString(), "早班", WorkType.人).ToString("dd日HH時mm分ss秒");
                        //"實際完成", 
                        if (dr_status["實際完成時間"].ToString() != null && dr_status["實際完成時間"].ToString() != "" && dr_status["實際完成時間"].ToString().ToUpper() != "NULL")
                        {
                            FinishTime = DateTime.ParseExact(DataTableUtils.toString(dr_status["實際完成時間"]), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                            PredictionTime = GetPredictionTime(dr_status["實際啟動時間"].ToString(), Dr_Craft["目前工時"].ToString(), "早班", WorkType.人);
                            _PredictionProgress = 100;
                            //PredictionTime = GetPredictionTime(dr_status["啟動時間"].ToString(), Dr_Craft["標準工時"].ToString(), "早班", WorkType.人);
                            if (FinishTime < PredictionTime)
                                return AdvanceStr = true;

                        }
                        else
                        {
                            if (DateTime.TryParseExact(dr_status["實際啟動時間"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.AssumeLocal, out Result_StartTime))
                            {
                                StartTime = Result_StartTime;
                                //StartTime = DateTime.ParseExact(dr_status["實際啟動時間"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                                RestTime = DateTime.ParseExact(dr_status["實際啟動時間"].ToString().Substring(0, 8) + "120000", "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                                //if ((StartTime < RestTime && DateTime.Now < RestTime) || (StartTime > RestTime && DateTime.Now > RestTime))//morning to  today morning or  afternoon to  today afternoon
                                //    ts = DateTime.Now - StartTime;
                                //else 
                                if ((StartTime < RestTime && DateTime.Now < RestTime) || (StartTime > RestTime && DateTime.Now > RestTime))// //morning to  morning  or afternoon to  afternoon
                                    ts = DateTime.Now - StartTime;
                                else if (StartTime < RestTime && DateTime.Now > RestTime)// //morning to  afternoon
                                    ts = DateTime.Now.AddHours(-1) - StartTime;
                                else if ((StartTime < RestTime && DateTime.Now < RestTime.AddDays(1)) || (StartTime > RestTime && DateTime.Now > RestTime.AddDays(1)))//morning to  tomorrow morning or afternoon to  tomorrow afternoon
                                    ts = DateTime.Now.AddHours(-16) - StartTime;
                                else if (StartTime > RestTime && DateTime.Now < RestTime.AddDays(1)) //afternoon to  tomorrow morning
                                    ts = DateTime.Now.AddHours(-15) - StartTime;
                                else if (StartTime < RestTime && DateTime.Now > RestTime.AddDays(1))///morning to  tomorrow afternoon
                                    ts = DateTime.Now.AddHours(-17) - StartTime;
                                else
                                    ts = DateTime.Now - StartTime;
                                if (DataTableUtils.toInt(Dr_Craft["目前工時"].ToString()) != 0)
                                {
                                    ProcessSec = ts.TotalSeconds / DataTableUtils.toDouble(Dr_Craft["目前工時"].ToString()) * 100;
                                    if ((int)ProcessSec <= 100)
                                        _PredictionProgress = (int)ProcessSec;
                                    else
                                        _PredictionProgress = 100;
                                    PredictionTimeInf.Add(ProcessSec.ToString());
                                    if (ProcessSec < DataTableUtils.toInt(dr_status["進度"].ToString()))
                                        return AdvanceStr = true;
                                }
                            }
                        }
                    }
                }
            }

            return AdvanceStr;
        }
        public static void LineNote(int LineNum, string PK, string ErrorMsgType, string ErrorMsg, string ConnByDekVisTmp, string MantID = "", string backman = "")
        {
            GlobalVar.UseDB_setConnString(ConnByDekVisTmp);

            string IP_Port = "57958";
            //Get Group and Token
            string CCSStr = "";
            string sql = "select 群組序號,工作站編號,工作站異常通知群組資料表.群組編號,工作站名稱,工作站異常通知資料表.群組編碼 from " + ShareMemory.SQLAsm_WorkStation_ErrorRingGroup + " INNER JOIN 工作站異常通知資料表 on 工作站異常通知群組資料表.群組編號 = 工作站異常通知資料表.群組編號";
            string condition = " where 工作站編號=" + "'" + LineNum + "'";
            DataRow dr_ccs;
            GlobalVar.UseDB_setConnString(ConnByDekVisTmp);
            DataRow dr_line = DataTableUtils.DataTable_GetDataRow(sql + condition);
            GlobalVar.UseDB_setConnString(ConnByDekVisTmp);
            DataRow dr_line_Name = DataTableUtils.DataTable_GetDataRow("select 工作站名稱 from " + ShareMemory.SQLAsm_WorkStation_Type + condition);
            //  if (ConnByDekVisTmp.Contains("DetaVisHor"))
            if (ConnByDekVisTmp.Contains("detaVisHor"))
            {
                CCSStr = "CCS";
                IP_Port = "57958";
            }
            else
            {
                CCSStr = "ITEM_NO";
                IP_Port = "57958";
            }
            GlobalVar.UseDB_setConnString(ConnByDekVisTmp);
            string sqlcmds = "select " + CCSStr + " from " + ShareMemory.SQLAsm_RowsData + " where " + "排程編號=" + "'" + PK + "'";
            dr_ccs = DataTableUtils.DataTable_GetDataRow(sqlcmds);
            if (dr_line != null && dr_line_Name != null && dr_ccs != null)
            {
                string url = "ErrorID=" + PK + ",ErrorLineNum=" + LineNum.ToString() + ",ErrorLineName=" + dr_line_Name["工作站名稱"].ToString() + ",MantID=" + MantID;

                //這裡有修改
                url = "http://vis.deta.com.tw:" + IP_Port + "/pages/dp_PM/Asm_ErrorDetail.aspx?key=" + WebUtils.UrlStringEncode(url);
                //   url = "http://172.23.10.106:8485/pages/dp_PM/Asm_ErrorDetail.aspx?key=" + WebUtils.UrlStringEncode(url);

                ErrorMsg = ErrorMsg.Replace('&', '，').Replace('+', '，').Replace('<', '，').Replace('>', '，').Replace('"', '，').Replace("'", "，").Replace('%', '，');
                lineNotify(dr_line["群組編碼"].ToString(), "\r\n" + "[產線名稱]:" + dr_line_Name["工作站名稱"].ToString() + "\r\n" + "[回覆人員]:" + backman + "\r\n" + "[排程編號]:" + PK + "\r\n" + "[排程品號]:" + dr_ccs[CCSStr].ToString() + "\r\n" + "[異常內容]:" + ErrorMsg + "\r\n" + "[連結]:" + url);
            }
        }
        public static void lineNotify(string token, string msg)
        {
            //token = "Z9dDvOaQAKbQZ1ASNWmALCkFOXPIt9wqnrtoVa1JZFz";//deta assm_01
            // token = "hYXiaLkQTaIcPKQ4GOpbGZFkpQS6yfYbv4s66qEG7J7";//deta assm_02
            // token = "QhiAa5v7oQngCXu7wWpx1U9YazEjE6CDc4G0DxilNsF";//專門測試用
            try
            {
                var request = (HttpWebRequest)WebRequest.Create("https://notify-api.line.me/api/notify");
                var postData = string.Format("message={0}", msg);
                var data = Encoding.UTF8.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                request.Headers.Add("Authorization", "Bearer " + token);

                using (var stream = request.GetRequestStream()) stream.Write(data, 0, data.Length);
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public string ErrorDetailDeleteProcess(string ErrorNum, string acc, string judge = "")
        {
            // check acc can delete this Mant rec
            string deleteFailNum = "";
            string message = "";
            int LineNum = 0;
            DataTable dr_mant = ErrorDetail_CheckIdMappping(ErrorNum, acc);
            if (dr_mant != null)
            {
                // delete rec 
                if (!ErrorDetailDeleteActive(dr_mant, ref deleteFailNum))
                    message = "刪除維護編號" + deleteFailNum + "失敗!";
                if (dr_mant.Rows[0]["父編號"].ToString() == null && dr_mant.Rows[0]["父編號"].ToString() == "0")
                {
                    // updata winForm && webform Vis 
                    LineNum = reCorrectMantToStationStatus(dr_mant.Rows[0][ShareMemory.PrimaryKey].ToString());
                    // updata machine_ID
                    Set_MachineID_Line_Updata(LineNum.ToString());
                }
                message = "維護編號" + ErrorNum + "刪除完成!";
            }
            else
            {
                message = "不能刪除非該帳號建立的維護訊息!";
            }
            return message;
        }
        public static DateTime StrToDate(string _date)
        {
            if (_date != null && _date.Length < 14 && _date.Substring(0, 1) != "0")
                _date = _date + "080000";
            DateTime Trs;
            Trs = StrToDateTime(_date, "yyyyMMddHHmmss");

            return Trs;
        }
        public static DateTime StrToDateTime(string time, string Sourceformat)
        {
            try
            {
                return DateTime.ParseExact(time, Sourceformat, System.Globalization.CultureInfo.CurrentCulture);
            }
            catch
            {
                return new DateTime();
            }
        }
        public static void record_user(string acc, string factory)
        {
            clsDB_Server clsDB = new clsDB_Server(myclass.GetConnByDekVisErp);
            DataTableUtils.Conn_String = myclass.GetConnByDekVisErp;

            string sql_cmd = "Select * from USERS where USER_ACC = '" + acc + "'";
            DataTable dt = DataTableUtils.GetDataTable(sql_cmd);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.NewRow();
                row["LastVIewModel"] = factory;
                if (clsDB.Update_DataRow("USERS", "USER_ACC = '" + acc + "'", row))
                {

                }
            }

        }
        public static string GetTimeStamp(DateTime time)
        {
            //  DateTime time = DateTime.Now;
            long ts = ConvertDateTimeToInt(time);
            return ts.ToString();
        }
        private static long ConvertDateTimeToInt(DateTime time)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位      
            return t;
        }
        //==========================private==============================================
        private string GetSrcAspStr(string AspPage, string Parameter)
        {
            string[] srcAsp = Parameter.Split('-');
            string SrcAspStr = "";
            if (AspPage == "Asm_Cahrt_Detail")
            {
                SrcAspStr = "Key=" + srcAsp[0] + ",LineNum=" + srcAsp[1] + ",Value=" + srcAsp[2] + ",StartTime=" + srcAsp[3] + ",EndTime=" + srcAsp[4];
                //Key = ErrorChat,LineNum = 2,Value = 其他,StartTime = 20190401,EndTime = 20190430
            }
            return SrcAspStr;
        }
        private DataTable ErrorDetail_CheckIdMappping(string ErrorNum, string acc)
        {
            string Acc_Name = "";
            string[] ErrorNumAry = ErrorNum.Split(',');
            DataRow dr_Acc_Name = GetAccInf(acc);
            DataTable dt_mant;
            DataView dv_mant;
            if (dr_Acc_Name != null)
            {
                Acc_Name = dr_Acc_Name["USER_NAME"].ToString();
                // linq
                // dataview
                dt_mant = GetMantDataFromNum(ErrorNumAry);
                dv_mant = new DataView(dt_mant);
                if (dr_Acc_Name["Power"].ToString().ToUpper() != "Y")
                {
                    dv_mant.RowFilter = "維護人員姓名=" + "'" + Acc_Name + "'";
                    if (dv_mant.Count == 0)
                        return null;
                    else
                        return dt_mant;
                }
                else
                    return dt_mant;
            }
            return null;
        }
        private bool ErrorDetailDeleteActive(DataTable dt_mant, ref string num)
        {
            bool OK = false;
            foreach (DataRow dr in dt_mant.Rows)
            {
                if (DataTableUtils.Delete_Record(ShareMemory.SQLAsm_WorkStation_ErrorMant, "異常維護編號=" + "'" + dr["異常維護編號"].ToString() + "'"))
                    OK = true;
                else
                {
                    num = dr["異常維護編號"].ToString();
                    return false;
                }
            }
            return OK;
        }
        private int reCorrectMantToStationStatus(string key)
        {
            bool ok = false;
            DataTable dt_mant = DataTableUtils.DataTable_GetTable(ShareMemory.SQLAsm_WorkStation_ErrorMant, ShareMemory.PrimaryKey + "=" + "'" + key + "'", 0, 0);
            DataView dv_mant = new DataView(dt_mant);
            dv_mant.Sort = "異常維護編號 desc";
            DataRow dr_mant = dv_mant.ToTable().Rows[0];
            //
            DataRow dr_Key = DataTableUtils.DataTable_GetDataRow(ShareMemory.SQLAsm_WorkStation_State, ShareMemory.PrimaryKey + "=" + "'" + dr_mant[ShareMemory.PrimaryKey].ToString() + "'");
            //if (dr_Key["異常"].ToString() == dr_mant["異常原因類型"].ToString()) //same type
            //{
            dr_Key["維護"] = dr_mant["維護內容"].ToString() + " " + Convert.ToDateTime(dr_mant["時間紀錄"].ToString()).ToString("yyyy/MM/dd HH:mm:ss");
            ok = DataTableUtils.Update_DataRow(ShareMemory.SQLAsm_WorkStation_State, ShareMemory.PrimaryKey + "=" + "'" + dr_mant[ShareMemory.PrimaryKey].ToString() + "'", dr_Key);
            //}
            //
            return DataTableUtils.toInt(dr_Key["工作站編號"].ToString());
        }
        private int LineNumFromKey(string key)
        {
            int lineNum = 0;
            DataRow dr_line = DataTableUtils.DataTable_GetDataRow(ShareMemory.SQLAsm_WorkStation_State, ShareMemory.PrimaryKey = "'" + key + "'");
            if (dr_line != null)
                lineNum = DataTableUtils.toInt(dr_line["工作站編號"].ToString());
            return lineNum;
        }
        private string[] GetEachPiece(DataTable dt)
        {
            string[] str = new string[7] { "0", "0", "0", "0", "0", "0", "0" };//0:all   1:finsh   2:Stop  3:all   4:td_finish   5:td_Stop  7:Data
                                                                               //全部
            DataView dt_fin = new DataView(dt);
            str[0] = dt_fin.Count.ToString();
            //完成
            dt_fin = dt.DefaultView;
            dt_fin.RowFilter = "狀態 = '完成'";
            if (dt.Rows[0]["工作站編號"].ToString() != "5" && dt.Rows[0]["工作站編號"].ToString() != "6" && dt.Rows[0]["工作站編號"].ToString() != "7")
                str[1] = dt_fin.Count.ToString();
            else // special for deta
                str[1] = " <a href = 'Asm_Cahrt_Detail.aspx?Key=Asm_LineOverView_FinishSpecialFunction,LineNum=" + dt.Rows[0]["工作站編號"].ToString() + "' >" + dt_fin.Count.ToString() + "</a>";

            //暫停
            dt_fin = dt.DefaultView;
            dt_fin.RowFilter = "狀態 = '暫停'";
            str[2] = dt_fin.Count.ToString();

            //今日全部
            dt_fin = dt.DefaultView;
            dt_fin.RowFilter = "組裝日 =" + "'" + DateTime.Now.ToString("yyyyMMdd").ToString() + "'";
            str[3] = dt_fin.Count.ToString();

            //今日完成
            dt_fin = dt.DefaultView;
            dt_fin.RowFilter = "狀態 = '完成'" + " AND " + "組裝日 =" + "'" + DateTime.Now.ToString("yyyyMMdd").ToString() + "'";
            str[4] = dt_fin.Count.ToString();

            //今日暫停
            dt_fin = dt.DefaultView;
            dt_fin.RowFilter = "狀態 = '暫停'" + " AND " + "組裝日 =" + "'" + DateTime.Now.ToString("yyyyMMdd").ToString() + "'";
            str[5] = dt_fin.Count.ToString();


            return str;
        }
        private string GetErrorSearchCondition(string key, string LineNum, string ErrorType)
        {
            //=============
            string condition = "";
            string condition_Key = "排程編號  " + " Like " + "'" + "%" + key + "%" + "'";
            string condition_LineNum = "工作站異常紀錄資料表.工作站編號" + " = " + "'" + LineNum + "'";
            string condition_ErrorType = "異常原因  =" + "'" + ErrorType + "'";
            //=============
            if (key != "" && LineNum != "0" && ErrorType != "--Select--")//111
                condition = condition_Key + " AND " + condition_LineNum + " AND " + condition_ErrorType;
            else if (key != "" && LineNum != "0" && ErrorType == "--Select--")//110
                condition = condition_Key + " AND " + condition_LineNum;
            else if (key != "" && LineNum == "0" && ErrorType != "--Select--")//101
                condition = condition_Key + " AND " + condition_ErrorType;
            else if (key != "" && LineNum == "0" && ErrorType == "--Select--")//100
                condition = condition_Key;
            else if (key == "" && LineNum != "0" && ErrorType != "--Select--")//011
                condition = condition_LineNum + " AND " + condition_ErrorType;
            else if (key == "" && LineNum != "0" && ErrorType == "--Select--")//010
                condition = condition_LineNum;
            else //if (key == "" && LineNum == "0" && ErrorType != "--Select--")//001
                condition = condition_ErrorType;
            return condition;
        }
        private string GetHistorySearchCondition(string key, string LineNum)
        {
            List<string> list = new List<string>(key.Split('#'));
            //=============
            string condition = "";

            string condition_Key = "";
            for (int i = 0; i < list.Count - 1; i++)
            {
                if (list[i] != "")
                    condition_Key += i == 0 ? $" 排程編號  Like '%{list[i].Trim()}%' " : $" OR 排程編號  Like '%{list[i].Trim()}%' ";

            }

            condition_Key = condition_Key != "" ? $" ( {condition_Key} ) " : "";

            string condition_LineNum = $"工作站狀態資料表.工作站編號 = '{LineNum}'";
            //=============
            if (condition_Key != "" && LineNum != "0")//11
                condition = condition_Key + " AND " + condition_LineNum;
            else if (condition_Key != "" && LineNum == "0")//10
                condition = condition_Key;
            else if (condition_Key == "" && LineNum != "0")//01
                condition = condition_LineNum;
            else //if (key == "" && LineNum == "0" && ErrorType != "--Select--")//00
                condition = condition_Key;
            return condition;
        }
        private string NoDataProcess()
        {
            //th = "<th class='center'>沒有資料載入</th>";
            string td = "<tr> <td class='center'> no data </ td ></ tr >";
            //title_text = "'沒有資料'";
            return td;
        }
        private void Note_MachineID_Line_Updata(string LineNum)
        {
            bool ok = false;
            DataTableUtils.Conn_String = GetConnByDekVisTmp;
            DataTable dt = DataTableUtils.GetDataTable(ShareMemory.SQLAsm_MachineID_Line, "機台產線代號" + "=" + "'" + LineNum + "'");
            foreach (DataRow dr in dt.Rows)
            {
                if (!(dr["是否有更新資料現場"].ToString().ToUpper() == "TRUE" || dr["是否有更新資料現場"].ToString().ToUpper() == "1"))
                {
                    dr["是否有更新資料現場"] = true;
                    ok = Support.DataTableUtils.Update_DataRow(ShareMemory.SQLAsm_MachineID_Line, "機台編號 =" + "'" + dr["機台編號"].ToString() + "'", dr);
                }
            }
        }
        private DataTable tableColumnSelectForLineDetail(DataTable src, string LineNum, string field = "", string interjoin = "")
        {
            string[] StrError;
            string CustomStr = "CUSTNM";
            DataTableUtils.Conn_String = GetConnByDekVisTmp;
            // string[] StrErrorTime;
            System.Data.DataView view = new System.Data.DataView(src);
            System.Data.DataTable selected = view.ToTable("Selected", false, "組裝日", "排程編號", "進度", "異常", "維護", "實際啟動時間", "問題回報");
            DataTable tmp;
            //"add customer data'
            string Condition1 = "";
            string Condition2 = "";
            string Condition3 = "";
            if (field == "")
            {
                Condition1 = "工作站編號 = " + "'" + LineNum.ToString() + "'" + " AND " + "實際組裝時間 =" + "'" + DateTime.Now.ToString("yyyyMMdd").ToString() + "'";
                Condition2 = "工作站編號 = " + "'" + LineNum.ToString() + "'" + " AND " + "實際組裝時間 <=" + "'" + DateTime.Now.ToString("yyyyMMdd").ToString() + "'" + " AND " + "狀態!=" + "'" + "完成" + "'";
                Condition3 = "OR 工作站編號 = '" + LineNum.ToString() + "' AND 實際完成時間 <='" + DateTime.Now.ToString("yyyyMMdd") + "235959' AND 實際完成時間 >='" + DateTime.Now.ToString("yyyyMMdd") + "010101'";
            }
            else
            {
                Condition1 = "工作站狀態資料表.排程編號 = " + "'" + LineNum.ToString() + "'" + " AND " + "實際組裝時間 =" + "'" + DateTime.Now.ToString("yyyyMMdd").ToString() + "'";
                Condition2 = "工作站狀態資料表.排程編號 = " + "'" + LineNum.ToString() + "'" + " AND " + "實際組裝時間 <=" + "'" + DateTime.Now.ToString("yyyyMMdd").ToString() + "'";
                Condition3 = "";
            }
            string Condition = Condition1 + " OR " + Condition2 + Condition3;
            DataTable Cust_Name = new DataTable();
            if (GetConnByDekVisTmp.Contains("Hor"))
                Cust_Name = DataTableUtils.GetDataTable("SELECT 工作站狀態資料表.排程編號,(case when 組裝資料表.CUSTNM IS null then  組裝資料表.客戶  when 組裝資料表.CUSTNM ='' then  組裝資料表.客戶 else 組裝資料表.CUSTNM end ) CUSTNM FROM 工作站狀態資料表  INNER JOIN 組裝資料表 ON 工作站狀態資料表.排程編號 = 組裝資料表.排程編號 where " + Condition);
            else
                Cust_Name = DataTableUtils.GetDataTable("SELECT 工作站狀態資料表.排程編號,組裝資料表.CUSTNM FROM 工作站狀態資料表  INNER JOIN 組裝資料表 ON 工作站狀態資料表.排程編號 = 組裝資料表.排程編號 where " + Condition);
            if (Cust_Name == null)
            {
                Cust_Name = DataTableUtils.GetDataTable("SELECT 工作站狀態資料表.排程編號,組裝資料表.客戶 " + field + "  FROM 工作站狀態資料表  INNER JOIN 組裝資料表 ON 工作站狀態資料表.排程編號 = 組裝資料表.排程編號  " + interjoin + "  where " + Condition + " order by 工作站狀態資料表.工作站編號 desc");
                CustomStr = "客戶";
            }
            System.Data.DataView view_Name = new System.Data.DataView(Cust_Name);
            //DataTable Cust_Name = DataTableUtils.GetDataTable("SELECT 工作站狀態資料表.排程編號,組裝資料表.CUSTNM FROM 工作站狀態資料表  INNER JOIN 組裝資料表 ON 工作站狀態資料表.排程編號 = 組裝資料表.排程編號 where 工作站編號= " + "'" + LineNum + "'" + " AND 實際組裝時間 =" + "'" + DateTime.Now.ToString("yyyyMMdd").ToString() + "'");
            //DataTable Error_Name = DataTableUtils.GetDataTable("SELECT 工作站狀態資料表.排程編號, 工作站異常紀錄資料表.異常原因,工作站異常紀錄資料表.異常起始時間 FROM 工作站狀態資料表 INNER JOIN 工作站異常紀錄資料表 ON 工作站狀態資料表.排程編號 = 工作站異常紀錄資料表.排程編號");

            selected.Columns.Add("狀態");
            selected.Columns.Add("客戶");
            selected.Columns["狀態"].SetOrdinal(3);//series
            selected.Columns["客戶"].SetOrdinal(1);
            if (field != "")
            {
                selected.Columns.Add("工作站名稱");
                selected.Columns["工作站名稱"].SetOrdinal(1);
            }

            try
            {
                for (int i = 0; i < selected.Rows.Count; i++)
                {
                    view_Name = Cust_Name.DefaultView;
                    view_Name.RowFilter = "排程編號 = " + "'" + selected.Rows[i]["排程編號"].ToString() + "'";
                    tmp = view_Name.ToTable();
                    selected.Rows[i]["客戶"] = tmp.Rows.Count > 0 ? tmp.Rows[0][CustomStr].ToString() : "無客戶";
                    if (field != "")
                        selected.Rows[i]["工作站名稱"] = tmp.Rows[i]["工作站名稱"].ToString();
                    if (selected.Rows[i]["異常"].ToString() != "")
                    {
                        StrError = selected.Rows[i]["維護"].ToString().Split(' ');
                        foreach (string Er in StrError)
                            selected.Rows[i]["異常"] += "," + Er;
                    }
                }
            }
            catch
            {
                return selected;
            }
            selected.Columns.Remove("維護");
            return selected;
        }
        private int CountOnTaget(DataTable dt_OnTarget)
        {
            int refProgress = 0;
            int count = 0;
            dt_OnTarget.Columns.Add("onTime");
            for (int i = 0; i < dt_OnTarget.Rows.Count; i++)
            {
                if (GetPredictionTimeStatus(dt_OnTarget.Rows[i][ShareMemory.PrimaryKey].ToString(), ref refProgress))
                {
                    dt_OnTarget.Rows[i]["onTime"] = "1";
                    count++;
                }
                else
                    dt_OnTarget.Rows[i]["onTime"] = "0";
            }
            return count;
        }
        private string SeparationOnTaget(DataTable dt_onTarget, onTimeStatus _onTimeStatus, Dictionary<string, int> _WorkStationInf_List)
        {
            string condition = "";
            string ChartStr = "";
            DataView dv = new DataView(dt_onTarget);
            // "{" + "y:" + DataTableUtils.toString(onTaget) + "," + "label:" + "'" + LineInf.Key + "'" + "}" + "," + infStr[0];
            if (_onTimeStatus == onTimeStatus.超前)
                condition = "1";
            else
                condition = "0";
            //
            foreach (KeyValuePair<string, int> LineInf in _WorkStationInf_List)
            {
                dv.RowFilter = "onTime =" + condition + " AND " + "工作站編號=" + "'" + LineInf.Value + "'";
                ChartStr = "{" + "y:" + DataTableUtils.toString(dv.Count) + "," + "label:" + "'" + LineInf.Key + "'" + "}" + "," + ChartStr;
            }
            return ChartStr;

        }
        //這裡要修改
        private DataTable EtableColumnSelectForLineDetail(DataTable src)
        {
            System.Data.DataView view = new System.Data.DataView(src);
            view.Sort = "時間紀錄 asc";
            System.Data.DataTable selected = new DataTable();
            try
            {
                selected = view.ToTable("Selected", false, "異常維護編號", "時間紀錄", "維護人員姓名", "維護人員單位", "異常原因類型", "維護內容", "處理狀態", "圖片檔名", "結案判定類型");
            }
            catch (Exception ex)
            {
                string esc = ex.Message;
            }
            return selected;
        }
        private DataTable tableColumnSelectForErrorSearchList(DataTable src)
        {
            System.Data.DataView view = new System.Data.DataView(src);
            //System.Data.DataTable selected = view.ToTable("Selected", false, "排程編號", "時間紀錄", "維護人員姓名", "維護人員單位", "異常原因類型", "維護內容", "處理狀態");
            System.Data.DataTable selected = view.ToTable("Selected", false, "異常編號", "排程編號", "工作站名稱", "工作站編號", "異常原因", "異常起始時間");
            return selected;
        }
        private DataTable tableColumnSelectForHistorySearchList(DataTable src)
        {
            System.Data.DataView view = new System.Data.DataView(src);
            //System.Data.DataTable selected = view.ToTable("Selected", false, "排程編號", "時間紀錄", "維護人員姓名", "維護人員單位", "異常原因類型", "維護內容", "處理狀態");
            System.Data.DataTable selected = view.ToTable("Selected", false, "排程編號", "工作站名稱", "實際啟動時間", "實際完成時間", "組裝累積時間", "工作站編號");
            return selected;
        }
        private DataTable tableColumnSelectForTotalLine(DataTable src)
        {
            List<string> list = new List<string>();
            int x, y;
            System.Data.DataView view = new System.Data.DataView(src);
            System.Data.DataTable selected = view.ToTable("Selected", false, "工作站名稱", "目標件數", "工作站是否使用中", "工作站編號", "人數配置");
            //add Finish Count
            selected.Columns.Add("生產中");
            selected.Columns.Add("完成");
            selected.Columns.Add("暫停");
            selected.Columns.Add("今日生產中");
            selected.Columns.Add("今日完成");
            selected.Columns.Add("今日暫停");
            //selected.Columns.Add("資訊");
            //Get piece 
            for (int i = 0; i < selected.Rows.Count; i++)
            {
                list.Clear();
                x = 0;
                y = 0;
                selected.Rows[i]["生產中"] = GetLinePieceCount(DataTableUtils.toInt(selected.Rows[i]["工作站編號"].ToString()), "產線").ToString();
                selected.Rows[i]["完成"] = GetLinePieceCount(DataTableUtils.toInt(selected.Rows[i]["工作站編號"].ToString()), "完成").ToString();
                //  selected.Rows[i]["暫停"] = GetLinePieceCount(DataTableUtils.toInt(selected.Rows[i]["工作站編號"].ToString()), "暫停").ToString();
                //計算裡面未解決的案件
                list = Calculation_Alarm_Or_Behind(selected.Rows[i]["工作站編號"].ToString(), ref x, ref y, true);
                selected.Rows[i]["暫停"] = list.Count.ToString();
                selected.Rows[i]["今日生產中"] = GetLinePieceCount(DataTableUtils.toInt(selected.Rows[i]["工作站編號"].ToString()), "產線", "td").ToString();
                selected.Rows[i]["今日完成"] = GetLinePieceCount(DataTableUtils.toInt(selected.Rows[i]["工作站編號"].ToString()), "完成", "td").ToString();
                selected.Rows[i]["今日暫停"] = GetLinePieceCount(DataTableUtils.toInt(selected.Rows[i]["工作站編號"].ToString()), "暫停", "td").ToString();
                //selected.Rows[i]["資訊"] = "內容";
            }
            return selected;
        }
        private int GetLinePieceCount(int LineNum, string Status, string td = null)
        {
            string Condition1 = "";
            string Condition2 = "";
            string Condition3 = "";
            string Condition = "";

            string CountCondition = "";
            Condition1 = "工作站編號 = " + "'" + LineNum + "'" + " AND " + "實際組裝時間 =" + "'" + DateTime.Now.ToString("yyyyMMdd").ToString() + "' ";
            Condition2 = "工作站編號 = " + "'" + LineNum + "'" + " AND " + "實際組裝時間 <=" + "'" + DateTime.Now.ToString("yyyyMMdd").ToString() + "' " + " AND " + "狀態!=" + "'" + "完成" + "'";
            Condition3 = $"工作站編號 = '{LineNum}' AND 實際完成時間>='{DateTime.Now.ToString("yyyyMMdd").ToString()}000000' AND 實際完成時間 <='{DateTime.Now.ToString("yyyyMMdd").ToString()}235959'  AND 狀態 ='完成'";
            if (td != "td")
                Condition = "(" + Condition1 + ")" + " OR " + "(" + Condition2 + ")" + " OR " + "(" + Condition3 + ")";
            else
                Condition = "工作站編號 = " + "'" + LineNum + "'" + " AND " + "組裝日 =" + "'" + DateTime.Now.ToString("yyyyMMdd").ToString() + "' ";
            DataTableUtils.Conn_String = GetConnByDekVisTmp;
            DataTable dt = DataTableUtils.DataTable_GetTable("工作站狀態資料表", Condition);
            DataView dV_Status_Count = dt.DefaultView;
            if (Status == "產線" || dt.Rows.Count == 0)
                return dt.Rows.Count;
            else
            {
                dV_Status_Count = dt.DefaultView;
                CountCondition = "狀態" + " Like " + "'" + "%" + Status + "%" + "'";
                dV_Status_Count.RowFilter = CountCondition;
                return dV_Status_Count.Count;
            }
        }
        private DateTime GetPredictionTime(string StartTimeStr, string StandardTime, string WorkClass, WorkType _WorkType)
        {
            DateTime PredictionTime;
            DataTableUtils.Conn_String = GetConnByDekVisTmp;
            try
            {
                DataRow dr_class = DataTableUtils.DataTable_GetDataRow(ShareMemory.SQLAsm_WorkClass, "班次名稱=" + "'" + WorkClass + "'");
                string EndTime = dr_class["結束時間"].ToString();
                DateTime StartTime = DateTime.ParseExact(StartTimeStr, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                DateTime EarlyClassTime = DateTime.ParseExact(StartTimeStr.Substring(0, 8) + dr_class["結束時間"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                DateTime RestTime = DateTime.ParseExact(StartTimeStr.Substring(0, 8) + "120000", "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);

                PredictionTime = StartTime.AddSeconds(DataTableUtils.toDouble(StandardTime));//StartTime+CraftStandardTime
                                                                                             //can not use mix work  class (human or work)
                if (StartTime < RestTime && PredictionTime < RestTime && PredictionTime < EarlyClassTime)//morning && not cross RestTime && corss early class
                    return PredictionTime;
                else if (StartTime < RestTime && PredictionTime > RestTime && PredictionTime < EarlyClassTime)//morning && not cross RestTime && corss early class
                {
                    if (PredictionTime.AddHours(1) > EarlyClassTime)//cross early class
                        return PredictionTime.AddHours(16);
                    else
                        return PredictionTime.AddHours(1);
                }
                else if (StartTime < RestTime && PredictionTime > RestTime && PredictionTime > EarlyClassTime)//morning && not cross RestTime && corss early class
                {
                    if (PredictionTime.AddHours(15) > RestTime.AddDays(1)) //cross tomorrow restTime
                        return PredictionTime.AddHours(17);
                    else
                        return PredictionTime.AddHours(16);
                }
                else if (StartTime > RestTime && PredictionTime < EarlyClassTime)//afternoon && not corss early class
                    return PredictionTime;
                else if (StartTime > RestTime && PredictionTime > EarlyClassTime)//afternoon && corss early class
                {
                    if (PredictionTime.AddHours(15) > RestTime.AddDays(1)) //cross tomorrow restTime
                        return PredictionTime.AddHours(16);
                    else
                        return PredictionTime.AddHours(15);
                }
                else
                    return PredictionTime;
            }
            catch
            {
                return DateTime.Now;
            }
        }
        private DataRow GetMantDataFromNum(string Num)
        {
            DataRow dr_mant = DataTableUtils.DataTable_GetDataRow(ShareMemory.SQLAsm_WorkStation_ErrorMant, "異常維護編號=" + "'" + Num + "'");
            return dr_mant;
        }
        private DataTable GetMantDataFromNum(string[] Mant_Num)
        {
            string condition = "";
            foreach (string num in Mant_Num)
            {
                if (num != Mant_Num.Last())
                    condition += "異常維護編號=" + "'" + num + "'" + " OR ";
                else
                    condition += "異常維護編號=" + "'" + num + "'";
            }
            DataTable dt_mant = DataTableUtils.DataTable_GetTable(ShareMemory.SQLAsm_WorkStation_ErrorMant, condition, 0, 0);
            return dt_mant;
        }
        static bool IsHoliday(DateTime dt)
        {
            if (dt_holiday != null && dt_holiday.Rows.Count > 0)
            {
                if (dt_holiday.AsEnumerable().Where(d => d.Field<string>("PK_Holiday") == dt.ToString("yyyyMMdd")).Count() != 0)
                    return true;
                foreach (DateTime date in holiday_list)
                    if (date.Date == dt.Date) return true;
            }
            else
            {
                DayOfWeek week = dt.DayOfWeek;
                if (week == DayOfWeek.Saturday || week == DayOfWeek.Sunday)
                    return true;
            }

            return false;

        }

        //計算起開始月份及結束月份
        public object monthInterval(string date,string acc)
        {
            object obj = new { };
            DataTableUtils.Conn_String = myclass.GetConnByDekVisErp;
            DataTable user_Acc = DataTableUtils.DataTable_GetTable($"SELECT * FROM SYSTEM_PARAMETER WHERE USER_ACC='{acc}'");
            //轉換日期型態
            DateTime endDay = DateTime.ParseExact(date, "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
            DateTime startDay = endDay.AddMonths(-1);

            //結合帳號月區間重新組合日期
            endDay = new DateTime(endDay.Year, endDay.Month, int.Parse(DataTableUtils.toString(user_Acc.Rows[0]["DATE_END"])));
            startDay = new DateTime(startDay.Year, startDay.Month, int.Parse(DataTableUtils.toString(user_Acc.Rows[0]["DATE_STR"])));
            obj =new{ startDay= startDay.ToString("yyyyMMdd"),endDay = endDay.ToString("yyyyMMdd"), interval= (((endDay - startDay).Days) + 1).ToString()};
            return obj;
        }
    }
    //=====================Class==============================================================
    class LineData
    {
        public int LineId { get; set; }
        public string LineName { get; set; }
    }
    public static class DateTransfor
    {
        public static IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }
        public static IEnumerable<DateTime> EachMonth(DateTime from, DateTime thru)
        {
            for (var month = from.Date; month.Date <= thru.Date || month.Month == thru.Month; month = month.AddMonths(1))
                yield return month;
        }
        public static IEnumerable<DateTime> EachDayTo(this DateTime dateFrom, DateTime dateTo)
        {
            return EachDay(dateFrom, dateTo);
        }
        public static IEnumerable<DateTime> EachMonthTo(this DateTime dateFrom, DateTime dateTo)
        {
            return EachMonth(dateFrom, dateTo);
        }
    }

    public class MyTimePeriod
    {
        public int minutes_start, // 起始時間 new-2020/01/20
                   minutes_end;   // 結束時間
        /*
                        00:00          10:15             24:00  
                          [--------------------------------]
         以分為計算單位: 0*60+0--------10*60+15----------24*60+0
        */
        public MyTimePeriod()
        {
            minutes_start = minutes_end = 0;
        }
        public MyTimePeriod(int start_hour, int start_minute, int end_hour, int end_minute)
        {
            SetTimePeriod(start_hour, start_minute, end_hour, end_minute);
        }

        public void SetTimePeriod(int start_hour, int start_minute, int end_hour, int end_minute)
        {
            minutes_start = start_hour * 60 + start_minute;
            minutes_end = end_hour * 60 + end_minute;
            if (minutes_start > minutes_end)
            {
                // 起始時間大於結束時間: 交換時間順序
                int t = minutes_start;
                minutes_start = minutes_end;
                minutes_end = t;
            }
        }

        void ToTimeValue(int minutes_total, out int hour, out int min)
        {
            // 將分為計算單位的值轉換為小時與分鐘
            hour = minutes_total / 60;
            min = minutes_total % 60;
        }

        public void GetStartTime(out int hour, out int min) // 取得開始時間
        {
            ToTimeValue(minutes_start, out hour, out min);
        }

        public void GetEndTime(out int hour, out int min)  // 取得結束時間
        {
            ToTimeValue(minutes_end, out hour, out min);
        }

        public override string ToString()
        {
            return ToText();
        }

        public string ToText() // 將工作時間範圍轉成文字
        {
            int min = TotalMinutes();
            return String.Format("{0:00}:{1:00}~{2:00}:{3:00}(時數={4}:{5})",
                minutes_start / 60, minutes_start % 60,
                minutes_end / 60, minutes_end % 60,
                min / 60, min % 60
                );
        }

        public int TotalMinutes() // 工作時間範圍的工作時數: 分
        {
            return minutes_end - minutes_start;
        }

        public bool IsEqual(MyTimePeriod mt)
        {
            if (mt == null) return false;
            return minutes_start == mt.minutes_start && minutes_end == mt.minutes_end;
        }

        public bool IsOverlap(MyTimePeriod mt) // 指定的時間範圍物件是否與自己有重疊時間
        {
            if (minutes_start < mt.minutes_start)
            {
                /* minutes_start  minutes_end 
                     [-------------]
                 mt:   [---]
                          [-----------]
                                     [-------]  X
                 */
                if (minutes_end <= mt.minutes_start) return false;
            }
            else if (minutes_start > mt.minutes_start)
            {
                /* minutes_start   minutes_end 
                             [-------------]
                 mt:   [---] x
                          [-----------]
                                     [-------]
                 */
                if (mt.minutes_end <= minutes_start) return false;
            }
            return true;
        }

    }

    //----------------------------------------------------------
    public class MyWorkTime
    {
        public List<MyTimePeriod> WorkHoursList = new List<MyTimePeriod>();
        public int DayWork_TotalMinutes = 24 * 60; // default 24 hours
        Func<DateTime, bool> IsHoliday;
        public MyWorkTime(Func<DateTime, bool> IsHolidayFunction)
        {
            IsHoliday = IsHolidayFunction;
        }

        int 工作時段_compare(MyTimePeriod mtp1, MyTimePeriod mtp2)
        {
            return mtp1.minutes_start - mtp2.minutes_start;
        }
        public bool 工作時段_新增(MyTimePeriod mt)
        {
            int total_min = 0;
            foreach (MyTimePeriod cur_mt in WorkHoursList)
            {
                if (cur_mt.IsOverlap(mt)) return false;
                total_min += cur_mt.TotalMinutes();
            }
            DayWork_TotalMinutes = total_min + mt.TotalMinutes();
            WorkHoursList.Add(mt);
            if (WorkHoursList.Count > 1)
                WorkHoursList.Sort(工作時段_compare);
            return true;
        }

        public bool 工作時段_新增(int start_hour, int start_min, int end_hour, int end_min)
        {
            return 工作時段_新增(new MyTimePeriod(start_hour, start_min, end_hour, end_min));
        }

        public bool 工作時段_刪除(MyTimePeriod mt)
        {
            foreach (MyTimePeriod cur_mt in WorkHoursList)
            {
                if (cur_mt.IsEqual(mt))
                {
                    DayWork_TotalMinutes -= cur_mt.TotalMinutes();
                    WorkHoursList.Remove(cur_mt);
                    return true;
                }
            }
            return false;
        }

        public bool 工作時段_刪除(int start_hour, int start_min, int end_hour, int end_min)
        {
            return 工作時段_刪除(new MyTimePeriod(start_hour, start_min, end_hour, end_min));
        }

        public string 工作時段_ToText()
        {
            string text = "";
            foreach (MyTimePeriod period in WorkHoursList)
            {
                if (text != "") text += ",";
                text += period.ToText();
            }
            return text + string.Format(" 全日工時={0:00}:{1:00}", DayWork_TotalMinutes / 60, DayWork_TotalMinutes % 60);
        }

        public TimeSpan 工作時段_工時(MyTimePeriod mtp)
        {
            int minutes = 0;
            if (WorkHoursList.Count <= 0)
                minutes = mtp.TotalMinutes();
            else
            {
                foreach (MyTimePeriod cur_mtp in WorkHoursList)
                {
                    if (cur_mtp.IsOverlap(mtp) != true) continue;
                    if (mtp.minutes_end <= cur_mtp.minutes_end)
                    {
                        /*
                        cur_mtp:   [-------------]
                        mtp:   [--------------]
                                   [----------]
                            ===>   [----------]
                        */
                        if (mtp.minutes_start < cur_mtp.minutes_start)
                            mtp.minutes_start = cur_mtp.minutes_start;
                        minutes += mtp.TotalMinutes();
                        break;
                    }
                    else
                    {
                        /*
                        cur_mtp:   [-------------]
                        mtp:   [-----------------------]
                                      [----------------]
                            ====>  [-------------]
                                               + [-----]  
                        */
                        if (mtp.minutes_start < cur_mtp.minutes_start)
                            mtp.minutes_start = cur_mtp.minutes_start;
                        int temp = mtp.minutes_end;
                        mtp.minutes_end = cur_mtp.minutes_end;
                        minutes += mtp.TotalMinutes();
                        mtp.minutes_start = cur_mtp.minutes_end;
                        mtp.minutes_end = temp;
                    }
                }
            }
            return new TimeSpan(0, minutes, 0);
        }
        public TimeSpan 工作時數(DateTime start_dt, DateTime end_dt)
        {
            bool is_minus = false;
            if (start_dt > end_dt)
            {
                DateTime dt = start_dt;
                start_dt = end_dt;
                end_dt = dt;
                is_minus = true;
            }
            TimeSpan total_time = new TimeSpan(0, 0, 0);
            DateTime cur_dt = start_dt;
            MyTimePeriod mtp = new MyTimePeriod();
            //-------------------------------
            if (IsHoliday(cur_dt) != true) // 起始日期
            {
                int end_hour = 24, end_min = 0;
                if (cur_dt.Date == end_dt.Date)
                {
                    end_hour = end_dt.Hour;
                    end_min = end_dt.Minute;
                }
                mtp.SetTimePeriod(cur_dt.Hour, cur_dt.Minute, end_hour, end_min);
                total_time += 工作時段_工時(mtp);
            }
            while (true)
            {
                cur_dt = cur_dt.AddDays(1);
                if (cur_dt.Date >= end_dt.Date) break;
                if (IsHoliday(cur_dt) != true)
                    total_time += new TimeSpan(0, DayWork_TotalMinutes, 0);
            }
            if (cur_dt.Day == end_dt.Day && IsHoliday(cur_dt) != true) // 結束日期
            {
                mtp.SetTimePeriod(0, 0, end_dt.Hour, end_dt.Minute);
                total_time += 工作時段_工時(mtp);
            }
            //-------------------------------
            if (is_minus) total_time = -total_time;
            return total_time;
        }

        public DateTime 目標日期(DateTime cur_date, TimeSpan span)
        {
            int min_total = (int)span.TotalMinutes;
            if (WorkHoursList.Count <= 0) // 沒有設定工作時間 ==> 24hours
            {
                while (min_total > 0)
                {
                    if (IsHoliday(cur_date) != true)
                    {
                        MyTimePeriod mtp = new MyTimePeriod(cur_date.Hour, cur_date.Minute, 24, 0);
                        int today_min = mtp.TotalMinutes();
                        if (min_total <= today_min)
                        {
                            cur_date = cur_date.AddMinutes(min_total);
                            break;
                        }
                        else
                        {
                            min_total -= today_min;
                        }
                    }
                    cur_date = new DateTime(cur_date.Year, cur_date.Month, cur_date.Day,
                                        0, 0, 0).AddDays(1);
                }
            }
            else
            {
                int hour, min, min_period;
                while (min_total > 0)
                {
                    if (IsHoliday(cur_date) != true)
                    {
                        MyTimePeriod mtp = new MyTimePeriod(cur_date.Hour, cur_date.Minute, 24, 0);
                        foreach (MyTimePeriod period in WorkHoursList)
                        {
                            if (period.IsOverlap(mtp) != true) continue;
                            min_period = period.TotalMinutes();
                            if (min_total >= min_period)
                            {
                                period.GetEndTime(out hour, out min);
                                mtp.SetTimePeriod(cur_date.Hour, cur_date.Minute, hour, min);
                                /*
                                  period_min        ---------
                                  min_total:        ---------------
                                                 HH:MM              24:00 
                                  mtp:             [------------------]
                                                startTime    EndTime
                                  period:          [-----------]
                                  mtp:       1. [--------------] 
                                             ==>    -----------     period_min
                                             2.       [--------] 
                                             ==>       --------     period_min = mtp.TotalMinutes();

                                */
                                // case 2 ??
                                if (mtp.minutes_start > period.minutes_start)
                                    min_period = mtp.TotalMinutes();
                                if (hour == 24)  // a new date ??
                                {
                                    min_total -= min_period;
                                    break;
                                }
                                cur_date = new DateTime(cur_date.Year, cur_date.Month, cur_date.Day,
                                    hour, min, 0);
                            }
                            else
                            {
                                /*
                                  period_min   ----------------
                                  min_total    ----------
                                                startTime    EndTime
                                  period:          [-----------] 24:00
                                  mtp:     1.   [------------------]
                                                   [-----------]
                                               ==>  -----
                                           2.         [------------] 
                                                      [--------]
                                               ==>     -----
                                           3.              [-------]
                                                           [---]
                                               ==>          ---   
                                */
                                if (mtp.minutes_start < period.minutes_start)
                                    mtp.minutes_start = period.minutes_start;
                                mtp.minutes_end = period.minutes_end;
                                min_period = mtp.TotalMinutes();
                                if (min_period > min_total)
                                    min_period = min_total;
                                mtp.GetStartTime(out hour, out min);
                                cur_date = new DateTime(cur_date.Year, cur_date.Month, cur_date.Day,
                                    hour, min, 0).AddMinutes(min_period);
                            }
                            min_total -= min_period;
                            if (min_total <= 0) return cur_date;
                            mtp.SetTimePeriod(cur_date.Hour, cur_date.Minute, 24, 0);
                        }
                    }
                    cur_date = new DateTime(cur_date.Year, cur_date.Month, cur_date.Day,
                                        0, 0, 0).AddDays(1);
                }
            }
            return cur_date;
        }
    }


}