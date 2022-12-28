using dek_erpvis_v2.cls;
using dekERP_dll.dekErp;
using Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace dek_erpvis_v2.pages.dp_PM
{
    public partial class waitingfortheproduction_New : System.Web.UI.Page
    {
        //-----------------------------參數 OR 引用------------------------------------------------
        public string path = "";
        public string color = "";
        string acc = "";
        string date_str = "";
        string date_end = "";
        string URL_NAME = "";
        myclass myclass = new myclass();
        DataTable dt_Finish = new DataTable();
        DataTable dt_NoFinish = new DataTable();
        DataTable dt_now = new DataTable();
        ShareFunction sfun = new ShareFunction();
        ProductionProgressChart PPC = new ProductionProgressChart();
        ERP_Sales SLS = new ERP_Sales();
        public int 預定生產_data_y_max = 0;
        public int 預計生產量_至今 = 0;
        public string 應有進度 = "3";
        public int 實際生產_data_y_max = 0;
        public string 實際進度 = "3";
        public string 差值 = "";
        public string sovling = "0";
        public string timerange = "";
        public string hide_image = "1";
        public string 預定生產_data = "";
        public string 實際生產_data = "";
        public string 實領料數_data = "";
        public string 本期未生產_persent = "";
        public int 上期尚未生產 = 0;
        public int 下期提前生產 = 0;
        public string th = "";
        public string tr = "";
        DataTable dt_本月應生產 = new DataTable();
        DataTable dt_未生產完成 = new DataTable();
        DataTable month_Capacity = new DataTable();
        DataTable dt_WorkerWorkTime = new DataTable();
        DataTable dt_nosolve = new DataTable();
        DataTable dt_Production = new DataTable();
        DataTable dt_Capacity = new DataTable();
        DataTable dt_Error = new DataTable();
        DataTable dt_Staff = new DataTable();
        int total = 0;
        string Line = "";
        string condition = "";
        List<string> avoid_again = new List<string>();
        public StringBuilder th_month_capacity = new StringBuilder();
        public StringBuilder tr_month_capacity = new StringBuilder();
        Dictionary<string, string> dc_MonthInterval = new Dictionary<string, string>();
        //-----------------------------------------------------EVENT-----------------------------------------------------------------------------
        //載入事件
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                path = 德大機械.get_title_web_path("PMD");
                URL_NAME = "waitingfortheproduction";
                color = HtmlUtil.change_color(acc);
                //string date = DateTime.Now.ToString("yyyyMMdd");
                

                if (myclass.user_view_check(URL_NAME, acc))
                {
                    if (!IsPostBack)
                    {
                        var daterange = 德大機械.德大專用月份(acc).Split(',');
                        date_str = daterange[0];
                        date_end = daterange[1];
                        dc_MonthInterval = sfun.monthInterval(date_end, acc);
                        if (txt_str.Text == "" && txt_end.Text == "")
                        {
                            txt_str.Text = HtmlUtil.changetimeformat(date_str, "-");
                            txt_end.Text = HtmlUtil.changetimeformat(date_end, "-");
                        }
                        MainProcess();
                    }
                }
                else
                    Response.Write("<script>alert('您無此權限!');location.href='../index.aspx';</script>");
            }
            else
                Response.Redirect(myclass.logout_url);

        }

        //把起始與結束時間設定成本月 || 搜尋事件
        protected void button_select_Click(object sender, EventArgs e)
        {
            if (DataTableUtils.toString(((Control)sender).ID.Split('_')[1]) == "select")
            {
                date_str = txt_str.Text.Replace("-", "");
                date_end = txt_end.Text.Replace("-", "");
                dc_MonthInterval = sfun.monthInterval(date_end, acc);
                var daterange = 德大機械.德大專用月份(acc).Split(',');
                MainProcess();
            }
            else
            {
                string[] daterange = 德大機械.德大專用月份(acc).Split(',');
                txt_str.Text = HtmlUtil.changetimeformat(daterange[0], "-");
                txt_end.Text = HtmlUtil.changetimeformat(daterange[1], "-");
            }
        }

        //廠區切換事件
        protected void dropdownlist_Factory_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkBoxList_LINE.Items.Clear();
            Set_CheckBox(dropdownlist_Factory.SelectedItem.Value);
        }

        //------------------------------------------------------------Function-------------------------------------------------
        //需要執行副程式
        private void MainProcess()
        {
            timerange = $"'{HtmlUtil.changetimeformat(date_str, "/")}~{HtmlUtil.changetimeformat(date_end, "/")}'";
            Set_CheckBox(dropdownlist_Factory.SelectedItem.Value);
            Set_SearchLine();
            dt_Production = PPC.Get_ProductionDataTable(date_str, date_end, condition, dropdownlist_Factory.SelectedItem.Value);
            dt_Capacity = PPC.Get_CapacityDatable(dropdownlist_Factory.SelectedItem.Value);
            dt_Production = PPC.DataTableMerge(dt_Production, date_str, date_end);
            dt_Error = PPC.Get_AbbormalTable(dt_Production, condition, dropdownlist_Factory.SelectedItem.Value);
            dt_Staff = PPC.GetWorkerWorkTimeDataTable(date_str, date_end, dropdownlist_Factory.SelectedItem.Value);
            Set_ProductionProgressTable(dt_Production, dt_Staff, dt_Error, dt_Capacity, acc, date_str, date_end, condition, dropdownlist_Factory.SelectedItem.Value);

            dt_Error=Get_AbbormalTable(dt_Production);
            Set_Chart(dt_Production);
            Set_Error(dt_Error);
            Set_Table(dt_Production);
            Set_ShowImage();
            
        }
        //顯示目前有的產線
        private void Set_CheckBox(string link = "sowon")
        {
            //確認有幾條產線
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisErp);
            string sqlcmd = "";
            if (link == "sowon")
                sqlcmd = "SELECT  ASSEMBLY_GROUP.GROUP_NAME,  ASSEMBLY_LINE.LINE_ID FROM ASSEMBLY_LINE  INNER JOIN ASSEMBLY_LINE_GROUP ON ASSEMBLY_LINE.LINE_ID = ASSEMBLY_LINE_GROUP.LINE_ID  INNER JOIN ASSEMBLY_GROUP ON ASSEMBLY_LINE_GROUP.GROUP_ID = ASSEMBLY_GROUP.GROUP_ID WHERE ASSEMBLY_LINE.LINE_STATUS = '1' and ASSEMBLY_LINE.LINE_ID < 100 and ASSEMBLY_LINE.LINE_ID <> 11";
            else
                sqlcmd = "SELECT a.GROUP_NAME,b.LINE_ID FROM (SELECT ASSEMBLY_GROUP.GROUP_NAME GROUP_NAME, ASSEMBLY_LINE_GROUP.LINE_ID FROM ASSEMBLY_GROUP, ASSEMBLY_LINE_GROUP WHERE ASSEMBLY_LINE_GROUP.GROUP_ID = ASSEMBLY_GROUP.GROUP_ID and ISNUMERIC( ASSEMBLY_LINE_GROUP.LINE_ID)=1 ) a LEFT JOIN (SELECT OriLine 工作站編號,目標件數,TrsLine LINE_ID FROM detaVisHor.dbo.工作站型態資料表,detaVisHor.dbo.工作站對應資料表 WHERE 工作站是否使用中 = '1' and 工作站對應資料表.TrsLine = 工作站型態資料表.工作站編號 ) b ON cast(a.LINE_ID as int) = cast(b.工作站編號 as int) where 工作站編號 IS NOT NULL ";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);

            if (HtmlUtil.Check_DataTable(dt) && checkBoxList_LINE.Items.Count == 0)
            {
                ListItem list = new ListItem();
                list = new ListItem("全部", "");
                checkBoxList_LINE.Items.Add(list);
                string Number = "";
                DataTable LINE = dt.DefaultView.ToTable(true, new string[] { "GROUP_NAME" });
                foreach (DataRow row in LINE.Rows)
                {
                    Number = "";
                    DataRow[] rows = dt.Select($"GROUP_NAME='{row["GROUP_NAME"]}'");
                    if (rows != null && rows.Length > 0)
                    {
                        for (int i = 0; i < rows.Length; i++)
                            Number += $"{rows[i]["LINE_ID"]},";
                    }
                    list = new ListItem(DataTableUtils.toString(row["GROUP_NAME"]), Number);

                    checkBoxList_LINE.Items.Add(list);
                }
            }
        }
        //使用者設定的產線
        private void Set_SearchLine()
        {
            condition = "";
            Line = "";

            //先集中所有產線
            for (int i = 0; i < checkBoxList_LINE.Items.Count; i++)
                Line += checkBoxList_LINE.Items[i].Selected ? checkBoxList_LINE.Items[i].Value : "";
            //轉換字元
            Line = Line.Replace(',', '#');

            //分割字串
            List<string> Line_Number = new List<string>(Line.Split('#'));
            for (int i = 0; i < Line_Number.Count - 1; i++)
                condition += i == 0 ? $" 工作站編號='{Line_Number[i]}' " : $" OR 工作站編號='{Line_Number[i]}' ";

            condition = condition != "" ? $" and ( {condition} ) " : "";
        }
        //取得本月應做資料
        private DataTable Get_MonthTotal(string date_str,string date_end)
        {
            timerange = $"'{HtmlUtil.changetimeformat(date_str, "/")}~{HtmlUtil.changetimeformat(date_end, "/")}'";
            dt_Finish = new DataTable();
            dt_NoFinish = new DataTable();
            DataTable dt_month = new DataTable();
            DataTable dt_return= new DataTable();
            DataTable dt_BigDisck = new DataTable();
            string sqlcmd = ""; 

            if (dropdownlist_Factory.SelectedItem.Value == "sowon")
            {
                //--------------------------------------------------------首旺部分-----------------------------------------------

                //20221125 新規則-將預計開工於下個月但在這個月完成的資料抓出來
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
                DateTime datetime = DateTime.ParseExact(date_end, "yyyyMMdd", null);
                datetime = datetime.AddMonths(1);
                string upper_Month = (HtmlUtil.StrToDate(date_str).AddMonths(-1)).ToString("yyyyMMdd");
                string next_MonthEnd = datetime.ToString("yyyyMMdd");
                sqlcmd = SLS.Waitingfortheproduction_Assm_Table(upper_Month,date_str, next_MonthEnd, dropdownlist_Factory.SelectedItem.Value);
                DataTable dt_sowon = DataTableUtils.GetDataTable(sqlcmd);
                dt_month = dt_sowon;

                //舊規則
                //GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
                //sqlcmd = SLS.Waitingfortheproduction_Assm_Table(date_end, date_str, dropdownlist_Factory.SelectedItem.Value);
                //DataTable dt_sowon = DataTableUtils.GetDataTable(sqlcmd);
                //dt_month = dt_sowon;

                //20220822 增加月產能表格
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
                month_Capacity = DataTableUtils.GetDataTable("select 工作站編號 AS 產線代號,工作站名稱,目標件數 AS 日產能 FROM 工作站型態資料表 WHERE 工作站是否使用中='1'");
                month_Capacity = myclass.Add_LINE_GROUP(month_Capacity).ToTable();
               

            }

            //臥式廠部分
            else
            {

                if (condition.Contains("11") || string.IsNullOrEmpty(condition))
                {
                    dt_BigDisck = get_大圓盤();
                }

                //20221125 新規則-將預計開工於下個月但在這個月完成的資料抓出來
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);
                string condition_copy = condition.Replace("工作站編號", " a.工作站編號 ");
                string upper_Month = (HtmlUtil.StrToDate(date_str).AddMonths(-1)).ToString("yyyyMMdd");
                DateTime datetime = DateTime.ParseExact(date_end, "yyyyMMdd", null);
                datetime = datetime.AddMonths(1);
                string next_MonthEnd = datetime.ToString("yyyyMMdd");
                sqlcmd = SLS.Waitingfortheproduction_Hor_Table(upper_Month, condition_copy, date_str, next_MonthEnd, dropdownlist_Factory.SelectedItem.Value);

                //舊規則
                //GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);
                //string condition_copy = condition.Replace("工作站編號", " a.工作站編號 ");
                //string upper_Month = (HtmlUtil.StrToDate(date_str).AddMonths(-1)).ToString("yyyyMMdd");
                //sqlcmd = SLS.Waitingfortheproduction_Hor_Table(upper_Month, condition_copy, date_str, date_end, dropdownlist_Factory.SelectedItem.Value);
                DataTable dt_sowon = DataTableUtils.GetDataTable(sqlcmd);

                //20220822 增加月產能表格,大圓盤&臥式統一在此取得
                month_Capacity = DataTableUtils.GetDataTable("select 工作站編號 AS 產線代號,工作站名稱,目標件數 AS 月產能 FROM 工作站型態資料表 WHERE 工作站是否使用中='1'");

                //20221208 補上防呆,空資料會錯誤  by秋雄
                if (HtmlUtil.Check_DataTable(dt_BigDisck)) 
                {
                    if (HtmlUtil.Check_DataTable(dt_sowon))
                    {
                        //20220728 dt_sowon的標準工時在合併前須先轉換,否則後面會錯誤
                        dt_sowon = sfun.Format_NowMonthTotal(dt_sowon);

                        foreach (DataRow row in dt_sowon.Rows)
                            dt_BigDisck.ImportRow(row);
                        dt_month = dt_BigDisck;
                    }
                    else {
                        dt_month = dt_BigDisck;
                    }
                } 
                else 
                {
                    if (HtmlUtil.Check_DataTable(dt_sowon))
                    {
                        //20220728 dt_sowon的標準工時在合併前須先轉換,否則後面會錯誤 
                        dt_sowon = sfun.Format_NowMonthTotal(dt_sowon);
                        dt_month = dt_sowon;
                    }
                    else {
                        dt_sowon.Columns.Add("預計完工日");
                        dt_BigDisck.Merge(dt_sowon);
                        dt_month = dt_BigDisck;

                    }
                }
               
            }
            //---------------------------------------------------------------------------------合併------------------------------------------------------------
            //產生 本月應做 之前未做完但在本月做完 到目前為止皆未完成之資料表
            if (HtmlUtil.Check_DataTable(dt_month))
                dt_now = sfun.Return_NowMonthTotal(dt_month, date_str, date_end, out dt_Finish, out dt_NoFinish);

            if (HtmlUtil.Check_DataTable(dt_now))
            {
                dt_return = dt_now.Clone();

                //20220826 PrimaryKey Merge時如無主鍵,資料會重複
                dt_now.PrimaryKey = new DataColumn[] { dt_now.Columns["排程編號"], dt_now.Columns["工作站編號"] };
                dt_return.PrimaryKey = new DataColumn[] { dt_return.Columns["排程編號"], dt_return.Columns["工作站編號"] };
                dt_return.Merge(dt_now);
            }
            else {
                dt_return = dt_month.Clone();
            }
            if (HtmlUtil.Check_DataTable(dt_Finish) && DataTableUtils.toInt(DateTime.Now.ToString("yyyyMMdd")) >= DataTableUtils.toInt(date_str))
            {
                dt_Finish.PrimaryKey = new DataColumn[] { dt_Finish.Columns["排程編號"], dt_Finish.Columns["工作站編號"] };
                dt_return.Merge(dt_Finish, true, MissingSchemaAction.Ignore);
            }
            if (HtmlUtil.Check_DataTable(dt_NoFinish) && DataTableUtils.toInt(DateTime.Now.ToString("yyyyMMdd")) >= DataTableUtils.toInt(date_str))
            {
                dt_NoFinish.PrimaryKey = new DataColumn[] { dt_NoFinish.Columns["排程編號"], dt_NoFinish.Columns["工作站編號"] };
                dt_return.Merge(dt_NoFinish, true, MissingSchemaAction.Ignore);
            }
            return dt_return;
        }

       //單獨取的大圓盤資料
        private DataTable get_大圓盤() {
            DataTable dt_month = new DataTable();
            string sqlcmd = "";
            //立式廠 && 大圓盤資訊
            //找出本月需做之數量
            //--------------------------------------------------------德科部分-----------------------------------------------
            //從dek ERP撈取本月排程資料
            //20220728 大圓盤部分暫時全搜索,待從立式移除後,歸類於臥室

            //20221125 新規則-將預計開工於下個月但在這個月完成的資料抓出來
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekERPDataTable);
            string upper_Month = (HtmlUtil.StrToDate(date_str).AddMonths(-1)).ToString("yyyyMMdd");
            DateTime datetime = DateTime.ParseExact(date_end, "yyyyMMdd", null);
            datetime = datetime.AddMonths(1);
            string next_MonthEnd = datetime.ToString("yyyyMMdd");
            sqlcmd = SLS.Waitingfortheproduction_BigDisc_Table(upper_Month, date_str, next_MonthEnd, "dek");

            //20221125 舊規則-
            //GlobalVar.UseDB_setConnString(myclass.GetConnByDekERPDataTable);
            //string upper_Month = (HtmlUtil.StrToDate(date_str).AddMonths(-1)).ToString("yyyyMMdd");
            //sqlcmd = SLS.Waitingfortheproduction_BigDisc_Table(upper_Month, date_str, date_end, "dek");


            DataTable dek_dt = DataTableUtils.GetDataTable(sqlcmd);
            //從組裝資料表撈取相對應資料
            if (HtmlUtil.Check_DataTable(dek_dt))
            {
                //新增欄位
                dek_dt.Columns.Add("進度");
                dek_dt.Columns.Add("狀態");
                dek_dt.Columns.Add("組裝日");
                dek_dt.Columns.Add("預計完工日");
                dek_dt.Columns.Add("實際完成時間");

                bool ok = true;
                string shcdulue = "";
                foreach (DataRow row in dek_dt.Rows)
                {
                    shcdulue += ok ? $" 排程編號='{row["排程編號"]}' " : $" OR 排程編號='{row["排程編號"]}' ";
                    ok = false;
                }
                shcdulue = shcdulue != "" ? $" and ( {shcdulue} ) " : "";
                //20220815 大圓盤立式移至臥式
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);
                sqlcmd = $" select " +
                         $"排程編號," +
                         $"進度," +
                         $"組裝日," +
                         $"狀態," +
                         $"組裝時間," +
                         $"SUBSTRING(實際完成時間,1,8) 實際完成時間  " +
                         $"from 工作站狀態資料表 " +
                         $"left join 組裝工藝 on 組裝工藝.機種編號 = (select top(1) value from STRING_SPLIT(工作站狀態資料表.排程編號, '-')) " +
                         $"where 工作站編號 = 11 {shcdulue} ";
                DataTable dek_dtinformation = DataTableUtils.GetDataTable(sqlcmd);
                DataTable dek_time = DataTableUtils.GetDataTable("select * from 組裝工藝");


                DataRow[] rows = null;
                if (HtmlUtil.Check_DataTable(dek_dtinformation))
                {
                    //填入資料
                    foreach (DataRow row in dek_dt.Rows)
                    {
                        sqlcmd = $"排程編號='{row["排程編號"]}'";
                        rows = dek_dtinformation.Select(sqlcmd);
                        //有資料的情況
                        if (rows != null && rows.Length > 0)
                        {
                            row["進度"] = rows[0]["進度"];
                            row["狀態"] = rows[0]["狀態"];
                            row["組裝日"] = rows[0]["組裝日"];
                            row["實際完成時間"] = rows[0]["實際完成時間"];
                            row["預計完工日"] = sfun.Test_calculation_finish(row["預計開工日"].ToString(), rows[0]["組裝時間"].ToString() == "" ? "1" : rows[0]["組裝時間"].ToString(), true);
                        }
                        //沒有資料的情況
                        else
                        {
                            sqlcmd = $"機種編號='{DataTableUtils.toString(row["排程編號"]).Split('-')[0]}'";
                            try
                            {
                                rows = dek_time.Select(sqlcmd);
                                row["預計完工日"] = sfun.Test_calculation_finish(row["預計開工日"].ToString(), rows[0]["組裝時間"].ToString() == "" ? "1" : rows[0]["組裝時間"].ToString(), true);

                                //20221007 如果erp有預計完成時間,則使用erp的預計完成時間
                                string fnh_day = row["組裝預計完工日"].ToString();
                                row["預計完工日"] = !string.IsNullOrEmpty(fnh_day) ? row["組裝預計完工日"].ToString() : row["預計完工日"].ToString();
                            }
                            catch
                            {
                                row["預計完工日"] = sfun.Test_calculation_finish(row["預計開工日"].ToString(), "1", true);
                            }
                        }
                    }
                }
                //沒有資料的情況
                else
                {
                    //填入資料
                    foreach (DataRow row in dek_dt.Rows)
                    {
                        sqlcmd = $"機種編號='{DataTableUtils.toString(row["排程編號"]).Split('-')[0]}'";
                        rows = dek_time.Select(sqlcmd);
                        if (rows.Length > 0)
                            row["預計完工日"] = sfun.Test_calculation_finish(row["預計開工日"].ToString(), rows[0]["組裝時間"].ToString(), true);
                        else
                            row["預計完工日"] = sfun.Test_calculation_finish(row["預計開工日"].ToString(), "1", true);
                    }
                }

                //選取屬於本月應生產之項目
                dt_month = dek_dt.Clone();
                sqlcmd = $"(實際完成時間>={date_str} OR 預計完工日>={date_str}) or 實際完成時間 IS NULL";
                rows = dek_dt.Select(sqlcmd);
                if (rows != null && rows.Length > 0)
                {
                    for (int i = 0; i < rows.Length; i++)
                        dt_month.ImportRow(rows[i]);
                }
            }
            else {
                //20221208 補上防呆,空資料會錯 by秋雄
                dt_month = dek_dt;
            }
            return dt_month;
        }

        //顯示圖片
        private void Set_Chart(DataTable dt_Production)
        {
            DataTable dt_day = Get_Monthday(date_str, date_end);
            string sqlcmd = "";
            DataRow[] rows = null;
            string today = DateTime.Now.ToString("yyyyMMdd");
            string expected_fn_day = "";
            for (int i = 0; i < dt_day.Rows.Count; i++)
            {
                expected_fn_day = dt_day.Rows[i]["日期"].ToString();
                //20221128 新預計生產條件
                sqlcmd = i == 0 ? PPC.ExpectedProduction_CMD(expected_fn_day, today, date_str, true) : PPC.ExpectedProduction_CMD(expected_fn_day, today, date_str, false);
                sqlcmd += condition;
                rows = dt_Production.Select(sqlcmd);
                //預計生產圖表
                預定生產_data += ExpectedProduction_Chart(expected_fn_day, rows.Length);
                //預計生產數量
                
                預定生產_data_y_max += rows.Length;

                //到今天為止應生產多少
                預計生產量_至今 += ExpectedProduction_Qty(expected_fn_day, today, date_str, rows.Length);

                //實際生產條件
                string d_end = dc_MonthInterval["endDay"];
                sqlcmd = i == 0 ? ActuallyProduction_CMD(expected_fn_day, d_end, true): ActuallyProduction_CMD(expected_fn_day, d_end, false);
                sqlcmd += condition;
                rows = dt_Production.Select(sqlcmd);
                //實際生產圖表
                實際生產_data += ActuallyProduction_Chart(expected_fn_day, today, date_str, rows.Length);
                //實際生產數量
                實際生產_data_y_max += ActuallyProduction_Qty(expected_fn_day, today, rows.Length);
            }
            預定生產_data_y_max = 預定生產_data_y_max == 0 ? 0 : 預定生產_data_y_max;
            應有進度 = 預定生產_data_y_max==0? 0+"%":DataTableUtils.toDouble(預計生產量_至今 * 100 / 預定生產_data_y_max).ToString("0") + "%";
            實際進度 = 預定生產_data_y_max == 0 ? 0+"%":DataTableUtils.toDouble(實際生產_data_y_max * 100 / 預定生產_data_y_max).ToString("0") + "%";
            差值 = (實際生產_data_y_max - 預計生產量_至今).ToString();
            本期未生產_persent= 預定生產_data_y_max == 0 ? 0+"%":((Math.Abs(Int32.Parse(預定生產_data_y_max.ToString())) - Math.Abs(Int32.Parse(實際生產_data_y_max.ToString()))) * 100 / 預定生產_data_y_max).ToString()+"%";
        }
        private DataTable Get_AbbormalTable(DataTable dt_Production ) {
            DataTable dt_nosolve = new　DataTable();
            //列出目前所有排程
            string Error_Number = "";
            bool ok = true;
            if (HtmlUtil.Check_DataTable(dt_Production))
            {
                foreach (DataRow row in dt_Production.Rows)
                {
                    Error_Number += ok ? $"  排程編號='{row["排程編號"]}' " : $" OR 排程編號='{row["排程編號"]}' ";
                    ok = false;
                }
                Error_Number = Error_Number != "" ? $" where ( {Error_Number} ) " : "";
            }

            
            if (dropdownlist_Factory.SelectedItem.Value == "sowon")
            {
                //取得目前未結案之數量
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
                string sqlcmd = $" SELECT " +
                                $" DISTINCT " +
                                $" 異常維護編號," +
                                $"工作站編號 as 產線代號,"+
                                $" 排程編號," +
                                $" a.結案判定類型  " +
                                $" FROM 工作站異常維護資料表 " +
                                $" LEFT JOIN (" +
                                $"             SELECT max(異常維護編號) 編號,排程編號 排程號碼,父編號,結案判定類型 " +
                                $"             FROM 工作站異常維護資料表 " +
                                $"             where    結案判定類型 IS NOT NULL " +
                                $"             group by 父編號,排程編號,結案判定類型" +
                                $"           ) a ON a.排程號碼 = 工作站異常維護資料表.排程編號 AND 工作站異常維護資料表.異常維護編號 = a.父編號 " +
                                $"           {Error_Number} " +
                                $"           AND (工作站異常維護資料表.父編號 IS NULL OR 工作站異常維護資料表.父編號=0) " +
                                $"           AND a.結案判定類型 IS NULL {condition.Replace("產線代號", "工作站異常維護資料表.工作站編號")} ORDER BY 工作站異常維護資料表.排程編號";
                dt_nosolve = DataTableUtils.GetDataTable(sqlcmd);
                dt_nosolve = myclass.Add_LINE_GROUP(dt_nosolve).ToTable();
            }
            else
            {
                Error_Number = Error_Number != "" ? $" {Error_Number} and (工作站異常維護資料表.工作站編號='1' OR 工作站異常維護資料表.工作站編號='2'  OR 工作站異常維護資料表.工作站編號='9' ) " : "";
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);
                string sqlcmd = $" SELECT DISTINCT 異常維護編號,(CASE when 工作站編號='1' then '100' when 工作站編號='2' then'110' else 工作站編號 End) AS 產線代號,排程編號,a.結案判定類型  FROM 工作站異常維護資料表 LEFT JOIN (SELECT max(異常維護編號) 編號,排程編號 排程號碼,父編號,結案判定類型 FROM 工作站異常維護資料表 where    結案判定類型 IS NOT NULL group by 父編號,排程編號,結案判定類型) a ON a.排程號碼 = 工作站異常維護資料表.排程編號 AND 工作站異常維護資料表.異常維護編號 = a.父編號 {Error_Number} AND (工作站異常維護資料表.父編號 IS NULL OR 工作站異常維護資料表.父編號=0) AND 工作站編號=1 AND a.結案判定類型 IS NULL ORDER BY 工作站異常維護資料表.排程編號";

                //20221212 無資料不抓異常
                if (Error_Number == "")
                    sqlcmd = "";

                dt_nosolve = DataTableUtils.GetDataTable(sqlcmd);
                dt_nosolve = myclass.Add_LINE_GROUP(dt_nosolve).ToTable();
            }
            return dt_nosolve;
        }


        //顯示錯誤台數
        private void Set_Error(DataTable dt_nosolve)
        {
            if (HtmlUtil.Check_DataTable(dt_nosolve))
            {
                //存入目前未結案的排程編號
                List<string> list_schdule = new List<string>();
                for (int i = 0; i < dt_nosolve.Rows.Count; i++)
                    list_schdule.Add(dt_nosolve.Rows[i]["排程編號"].ToString());
                //排除重複項
                list_schdule = list_schdule.Distinct().ToList();
                //寫入排程編號跟維護編號
                string nosovle = "";
                foreach (DataRow row in dt_nosolve.Rows)
                    nosovle += $"{row["排程編號"]}!{row["異常維護編號"]}*";
                //導向網址
                sovling = dropdownlist_Factory.SelectedItem.Value == "sowon" ? $"<u><a href=\"Asm_NoSolve.aspx?key={WebUtils.UrlStringEncode($"mach={nosovle}")}\">{list_schdule.Count}</a></u>" : $"<u><a href=\"Asm_NoSolve_ITEC.aspx?key={WebUtils.UrlStringEncode($"mach={nosovle}")}\">{list_schdule.Count}</a></u>"; ;
            }
            else
                sovling = "0";
        }
        //顯示表格
        private void Set_Table(DataTable dt_Production)
        {
            if (dropdownlist_Factory.SelectedItem.Value == "iTec")
            {
                dt_Production.Columns["產線代號"].ReadOnly = false;
                dt_Production.Columns["產線代號"].MaxLength = 15;
                foreach (DataRow row in dt_Production.Rows)
                {
                    if (row["產線代號"].ToString() == "1")
                        row["產線代號"] = "100";
                    else if (row["產線代號"].ToString() == "2")
                        row["產線代號"] = "110";
                    
                }
            }
            //dt_Production = myclass.Add_LINE_GROUP(dt_Production).ToTable();
            dt_未生產完成 = dt_Production.Clone();
            string sqlcmd = dropdownlist_Factory.SelectedItem.Value == "iTec" ? $" (實際完成時間 IS NULL  OR 實際完成時間 ='') and (狀態 <> '完成' OR 狀態 IS NULL ) {condition.Replace("產線代號", "工作站編號")} " : $" (實際完成時間 IS NULL  OR 實際完成時間 ='') and (狀態 <> '完成' OR 狀態 IS NULL ) {condition} ";
            DataRow[] rows = dt_Production.Select(sqlcmd);

            if (rows != null && rows.Length > 0)
                for (int i = 0; i < rows.Length; i++)
                    dt_未生產完成.ImportRow(rows[i]);

            //列出目前所有客戶
            DataTable custom = dt_未生產完成.DefaultView.ToTable(true, new string[] { "客戶簡稱" });
            //列出目前所有產線
            DataTable Line = dt_未生產完成.DefaultView.ToTable(true, new string[] { "產線群組" });
            //確定有客戶再列出
            if (HtmlUtil.Check_DataTable(custom))
            {
                foreach (DataRow row in Line.Rows)
                    custom.Columns.Add(row["產線群組"].ToString());
                custom.Columns.Add("小計");
                string title = "";
                th = HtmlUtil.Set_Table_Title(custom, out title);
                tr = HtmlUtil.Set_Table_Content(custom, title, waitingfortheproduction_New_callback);
            }
            //沒有客戶不列出
            else
                HtmlUtil.NoData(out th, out tr);
        }
        //欄位處理用
        private string waitingfortheproduction_New_callback(DataRow row, string field_name)
        {
            string value = "";
            if (avoid_again.IndexOf(DataTableUtils.toString(row["客戶簡稱"]).Trim()) == -1)
            {
                if (field_name != "客戶簡稱" && field_name != "小計")
                {
                    string sqlcmd = $"客戶簡稱='{row["客戶簡稱"]}' and 產線群組='{field_name}'";
                    DataRow[] rows = dt_未生產完成.Select(sqlcmd);
                    DataTable test = dt_未生產完成.Clone();
                    for (int i = 0; i < rows.Length; i++)
                        test.ImportRow(rows[i]);
                    value = rows.Length.ToString();
                    total += rows.Length;
                }
                else if (field_name == "小計")
                {
                    //產生連結
                    string url = $"cust_name={row["客戶簡稱"]},date_str={date_str},date_end={date_end},line={Line}";
                    url = WebUtils.UrlStringEncode(url);
                    value = dropdownlist_Factory.SelectedItem.Value == "sowon" ? $"<u><a href=waitingfortheproduction_details.aspx?key={url} >{total} </a></u>" : $"<u><a href=waitingfortheproduction_details_ITEC.aspx?key={url} >{total} </a></u>";
                    //把到目前為止的統計規零
                    total = 0;
                    avoid_again.Add(DataTableUtils.toString(row["客戶簡稱"]).Trim());
                }
                return value == "" ? "" : $"<td>{value}</td>";
            }
            else
                return "1";
        }
        //設定應顯示推移圖 || 領料圖
        private void Set_ShowImage()
        {
            if (dropdownlist_type.SelectedItem.Text == "生產推移圖")
                hide_image = "hidediv";
            else
                hide_image = "hidepercent";
        }
        //列出當月每一天
        private DataTable Get_Monthday(string start, string end)
        {
            DateTime start_time = ShareFunction.StrToDate(start);
            DateTime end_time = ShareFunction.StrToDate(end);
            TimeSpan span = end_time - start_time;
            DataTable dt = new DataTable();
            dt.Columns.Add("日期", typeof(string));
            for (int i = 0; i < Int16.Parse(span.TotalDays.ToString()) + 1; i++)
                dt.Rows.Add(start_time.AddDays(i).ToString("yyyyMMdd"));
            return dt;
        }



       
        private void Set_ProductionProgressTable(DataTable dt, DataTable Staff, DataTable error_unsolved, DataTable capacityTable, string acc, string date_start, string date_end, string condition, string factory)
        {
            if (HtmlUtil.Check_DataTable(dt))
            {
                List<ProductionProgress> productionProgress = new List<ProductionProgress>();
                DataTable dt_Copy = dt.Clone();

                //清空陣列
                avoid_again.Clear();
                DataTable target = dt.DefaultView.ToTable(true, new string[] { "產線群組" });
                DataTable table = new DataTable();
                table.Columns.Add(" ");
                foreach (DataRow dataRow in target.Rows)
                {
                    table.Columns.Add(dataRow[0].ToString());
                }


                //新增總計
                table.Rows.Add("月產能");
                table.Rows.Add("本期計畫生產");
                //table.Rows.Add("目前累計應有進度");
                //table.Rows.Add("目前累計實際進度");
                //table.Rows.Add("差異量");
                table.Rows.Add("昨日應有進度");
                table.Rows.Add("昨日實際進度");
                table.Rows.Add("上期尚未生產");
                table.Rows.Add("下期提前生產");
                table.Rows.Add("昨日人力(實到/應到)");
                table.Rows.Add("本期異常累計");
                string columns = "";
                //20220729 新稱月產能
                productionProgress.Clear();
                productionProgress.Add(new ProductionProgress()
                {
                    FactoryFloor = factory,
                    Account = acc,
                    Date_Start = date_start,
                    Date_End = date_end,
                    Condition = condition,
                    FactoryDataTable = dt,
                    StaffDataTable = Staff,
                    ErrorUnsolvedDataTable = error_unsolved,
                    CapacitydDataTable = capacityTable
                });
                //上期未生產
                string sqlcmd = PPC.ProductionDelay_CMD(date_start);
                DataRow[] rows= dt.Select(sqlcmd);
                上期尚未生產 = rows.Length;
                //下期提前生產
                sqlcmd = PPC.ProductionAhead_CMD(date_end);
                rows = dt.Select(sqlcmd);
                下期提前生產 = rows.Length;
                    th_month_capacity.Append(HtmlUtil.Set_Table_Title(table, out columns, "", "style=\"text-align:center;background-color:#2A3F54;color:white;\""));
                    tr_month_capacity.Append(HtmlUtil.DaTaTable_To_HtmlTable(table, productionProgress, columns, PPC.ProductionProgressTable_CallBack, "style=\"text-align:center;background-color:#2A3F54;color:white;\""));
                

            }
            else
            {
                HtmlUtil.NoData(out th_month_capacity, out tr_month_capacity);

            }

        }
        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>舊架構<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//
        private void Set_Month_Capacity_col()
        {
            if (HtmlUtil.Check_DataTable(dt_本月應生產))
            {
                DataTable dt_Copy = dt_本月應生產.Clone();
                //清空陣列
                //Line_Name.Clear();
                avoid_again.Clear();
                DataTable target = dt_本月應生產.DefaultView.ToTable(true, new string[] { "產線群組" });
                DataTable test = new DataTable();
                test.Columns.Add(" ");
                foreach (DataRow dataRow in target.Rows)
                {
                    test.Columns.Add(dataRow[0].ToString());

                }
                //新增總計
                test.Rows.Add("月產能");
                test.Rows.Add("本期計畫生產");
                //test.Rows.Add("目前累計應有進度");
                //test.Rows.Add("目前累計實際進度");
                //test.Rows.Add("差異量");
                test.Rows.Add("昨日應有進度");
                test.Rows.Add("昨日實際進度");
                test.Rows.Add("上期尚未生產");
                test.Rows.Add("下期提前生產");
                test.Rows.Add("昨日人力(實到/應到)");
                test.Rows.Add("異常");
                string columns = "";
                //20220729 新稱月產能
                th_month_capacity.Append(HtmlUtil.Set_Table_Title(test, out columns,"", "style=\"text-align:center;background-color:#2A3F54;color:white;\""));
                tr_month_capacity.Append(HtmlUtil.Set_Table_Content(test, columns, capacity_CallBack2, "style=\"text-align:center;background-color:#2A3F54;color:white;\""));
            }
            else
                HtmlUtil.NoData(out th_month_capacity, out tr_month_capacity);
        }


        private string capacity_CallBack2(DataRow row, string field_name)
        {
            //此有兩組日期使用,一組為月初月尾,一組為選擇的日期
            DataTable dt_day = Get_Monthday(date_str, date_end);
            string sqlcmd = "";
            int _預定生產 = 0;
            int _預計生產量 = 0;
            int _實際生產 = 0;
            int _昨日預定生產 = 0;
            int _昨日實際生產 = 0;
            int total = 0;
            int 月產能 = 0;
            string 差異量 = "";
            DataRow[] rows = null;
            string LineGroup = field_name;
            string today = DateTime.Now.ToString("yyyyMMdd");
            string excepted_fn_day = "";
            string value = "";
            int 應到人數 = 0;
            int 實到人數 = 0;
            string _異常量 = "";
            string a = row[field_name].ToString();
            string row_name = row[0].ToString();
            string d_end = dc_MonthInterval["endDay"];
            string d_start = dc_MonthInterval["startDay"];
            string yesterday = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");
            int this_month = int.Parse(DateTime.Now.Month.ToString());
            int past_month = int.Parse(DateTime.ParseExact(d_end, "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces).Month.ToString());
            if (field_name!=" ") 
            {


                switch (row_name)
                {
                    case "月產能":
                        if (dropdownlist_Factory.SelectedItem.Value == "sowon")
                        {

                            object dd = month_Capacity.Compute("sum(日產能)", $"產線群組='{field_name}'");
                            int workDay = int.Parse(month_WorkDay(date_str, date_end));
                            月產能 = (int.Parse(dd.ToString()) * workDay);
                        }
                        else
                        {
                            string line_Group = field_name == "NEW INTE" ? line_Group = "NEW_INTE" : line_Group = field_name;
                            object dd = month_Capacity.Compute("sum(月產能)", $"工作站名稱='{line_Group}'");
                            月產能 = int.Parse(dd.ToString());
                        }
                        break;
                    case "本期計畫生產":
                        //預計生產
                        sqlcmd = PPC.ExpectedProduction_CMD(date_end, today, date_str, true);
                        sqlcmd += $" AND 產線群組='{LineGroup}'";
                        rows = dt_本月應生產.Select(sqlcmd);
                        _預定生產 = rows.Length;
                        break;
                    case "目前累計應有進度":
                        sqlcmd = PPC.ExpectedProduction_CMD(today, today, date_str, true);
                        sqlcmd += $" AND 產線群組='{LineGroup}'";
                        rows = dt_本月應生產.Select(sqlcmd);
                        //到今天為止應生產多少
                        _預計生產量 = rows.Length;
                        break;
                    case "目前累計實際進度":
                        //實際生產

                        sqlcmd = sqlcmd = $"(預計完工日<={d_end} and 狀態 = '完成')";
                        sqlcmd += $" and 產線群組='{LineGroup}'";
                        rows = dt_本月應生產.Select(sqlcmd);

                        _實際生產 = rows.Length;

                        break;
                    case "差異量":

                        sqlcmd = sqlcmd = $"(預計完工日<={d_end} and 狀態 = '完成')";
                        sqlcmd += $" and 產線群組='{LineGroup}'";
                        rows = dt_本月應生產.Select(sqlcmd);

                        _實際生產 = rows.Length;

                        sqlcmd = PPC.ExpectedProduction_CMD(today, today, date_str, true);
                        sqlcmd += $" AND 產線群組='{LineGroup}'";
                        rows = dt_本月應生產.Select(sqlcmd);
                        //到今天為止應生產多少
                        _預計生產量 = rows.Length;

                        差異量 = (_預計生產量 - _實際生產).ToString();
                        break;
                    case "昨日應有進度":

                        //for (int i = 0; i < dt_day.Rows.Count; i++)
                        //{
                        //    excepted_fn_day = dt_day.Rows[i]["日期"].ToString();
                        //    sqlcmd = i == 0 ? ExpectedProduction_CMD(excepted_fn_day, today, date_str, true) : ExpectedProduction_CMD(excepted_fn_day, today, date_str, false);
                        //    sqlcmd += $" AND 產線群組='{LineGroup}'";
                        //    rows = dt_本月應生產.Select(sqlcmd);
                        //    total += ExpectedProduction_Qty(excepted_fn_day, today, date_str, rows.Length);
                        //    //昨日預計生產
                        //    if (i != 0 && int.Parse(excepted_fn_day) < int.Parse(today))
                        //    {
                        //        _昨日預定生產 = total;
                        //    }

                        //}

                        //if (this_month > past_month) 
                        //{
                        //    if (int.Parse(today) > int.Parse(date_end))
                        //    {
                        //        //上個月昨天
                        //        string last_month_yesterday = DateTime.ParseExact(date_end, "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces).AddDays(-1).ToString("yyyyMMdd");
                        //        sqlcmd = $"(預計完工日<={last_month_yesterday} and (實際完成時間 >='{date_str}' AND 實際完成時間<={d_end}) and 狀態 = '完成')";
                        //    }

                        //} 
                        //else 
                        //{

                        //    if (int.Parse(today) > int.Parse(date_end))
                        //    {
                        //        //上個月昨天
                        //        sqlcmd = $"(預計完工日<={_yesterday} and (實際完成時間 >='{d_start}' and 實際完成時間<={_yesterday}) and 狀態 = '完成')";
                        //    }
                        //    else
                        //    {
                        //        //本月昨天
                        //        //sqlcmd = ExpectedProduction_CMD(yesterday, today, date_str, true);
                        if (int.Parse(today) > int.Parse(date_end))
                        {
                            yesterday = DateTime.ParseExact(date_end, "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces).AddDays(-1).ToString("yyyyMMdd");
                            sqlcmd = PPC.ExpectedProduction_CMD(yesterday, today, date_str, true);
                        }
                        else {
                            yesterday = DateTime.ParseExact(today, "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces).AddDays(-1).ToString("yyyyMMdd");
                            sqlcmd = PPC.ExpectedProduction_CMD(yesterday, today, date_str, true);

                        }

                        //    }
                        //}


                        //sqlcmd = ExpectedProduction_CMD(yesterday, today, date_str, true);
                        sqlcmd += $" AND 產線群組='{LineGroup}'";
                        rows = dt_本月應生產.Select(sqlcmd);
                        DataTable dtss = dt_本月應生產.Clone();
                        for (int i = 0; i < rows.Length; i++)
                            dtss.ImportRow(rows[i]);
                        _昨日預定生產 = rows.Length;
                        break;

                    case "昨日實際進度":
                        //舊,數字正常
                        //for (int i = 0; i < dt_day.Rows.Count; i++)
                        //{
                        //    excepted_fn_day = dt_day.Rows[i]["日期"].ToString();
                        //    //實際生產
                        //    sqlcmd = i == 0 ? ActuallyProduction_CMD(excepted_fn_day, d_end, true) : ActuallyProduction_CMD(excepted_fn_day, d_end, false);
                        //    sqlcmd += $" and 產線群組='{LineGroup}'";
                        //    rows = dt_本月應生產.Select(sqlcmd);
                        //    if (int.Parse(today) > int.Parse(date_end))
                        //    {
                        //        total += ExpectedProduction_Qty(excepted_fn_day, date_end, date_str, rows.Length);
                        //    }
                        //    else
                        //    {
                        //        total += ExpectedProduction_Qty(excepted_fn_day, today, date_str, rows.Length);
                        //    }

                        //    if (i != 0 && int.Parse(excepted_fn_day) < int.Parse(today))
                        //    {
                        //        _昨日實際生產 = total;
                        //    }
                        //}

                        //新
                        //if (this_month > past_month)
                        //{
                        //    if (int.Parse(today) > int.Parse(date_end))
                        //    {
                        //        //上個月昨天
                        //        string last_month_yesterday = DateTime.ParseExact(date_end, "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces).AddDays(-1).ToString("yyyyMMdd");
                        //        sqlcmd = $"(預計完工日<={date_end} and (實際完成時間 >='{d_start}' and 實際完成時間<={last_month_yesterday}) and 狀態 = '完成')";
                        //    }

                        //}
                        //else
                        //{
                        //    if (int.Parse(today) > int.Parse(date_end))
                        //    {
                        //        //上個月昨天
                        //        string _yesterday = DateTime.ParseExact(date_end, "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces).AddDays(-1).ToString("yyyyMMdd");
                        //        sqlcmd = $"(預計完工日<={d_end} and (實際完成時間 >='{d_start}' and 實際完成時間<={_yesterday}) and 狀態 = '完成')";
                        //    }
                        //    else
                        //    {
                        //        //本月昨天
                        //        sqlcmd = $"(預計完工日<={d_end} and (實際完成時間 >='{d_start}' and 實際完成時間<={yesterday}) and 狀態 = '完成')";
                        //    }
                        //}

                        if (int.Parse(today) > int.Parse(date_end))
                        {
                            yesterday = DateTime.ParseExact(date_end, "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces).AddDays(-1).ToString("yyyyMMdd");
                            sqlcmd = PPC.ProductionFullCountForYesterday_CMD(d_start, d_end, yesterday);
                        }
                        else
                        {
                            yesterday = DateTime.ParseExact(today, "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces).AddDays(-1).ToString("yyyyMMdd");
                            sqlcmd = PPC.ProductionFullCountForYesterday_CMD(d_start, d_end, yesterday);
                        }

                            sqlcmd += $" and 產線群組='{LineGroup}'";
                        rows = dt_本月應生產.Select(sqlcmd);
                        DataTable dts = dt_本月應生產.Clone();
                        for (int i = 0; i < rows.Length; i++)
                            dts.ImportRow(rows[i]);
                        _昨日實際生產 = rows.Length;
                        break;

                    case "昨日人力(實到/應到)":

                        if (HtmlUtil.Check_DataTable(dt_WorkerWorkTime))
                        {
                            
                            sqlcmd = $"日期='{yesterday}' and 產線群組='{LineGroup}'";
                            rows = dt_WorkerWorkTime.Select(sqlcmd);

                            foreach (DataRow dataRow in rows)
                            {
                                string excepted_People_count = dataRow["應到人數"].ToString() == "" ? "0" : dataRow["應到人數"].ToString();
                                string working_People_count = dataRow["實到人數"].ToString() == "" ? "0" : dataRow["實到人數"].ToString();
                                應到人數 += int.Parse(excepted_People_count);
                                實到人數 += int.Parse(working_People_count);
                            }
                        }

                        break;

                    case "異常":


                        if (HtmlUtil.Check_DataTable(dt_nosolve))
                        {
                            sqlcmd = $"產線群組='{LineGroup}'";
                            rows = dt_nosolve.Select(sqlcmd);
                            if (rows.Length > 0)
                            {
                                //存入目前未結案的排程編號
                                List<string> list_schdule = new List<string>();
                                foreach (DataRow dataRow in rows)
                                    list_schdule.Add(dataRow["排程編號"].ToString());
                                //排除重複項
                                list_schdule = list_schdule.Distinct().ToList();
                                //寫入排程編號跟維護編號
                                string nosovle = "";
                                foreach (DataRow dataRow1 in rows)
                                    nosovle += $"{dataRow1["排程編號"]}!{dataRow1["異常維護編號"]}*";
                                //導向網址
                                _異常量 = dropdownlist_Factory.SelectedItem.Value == "sowon" ? $"<u><a href=\"Asm_NoSolve.aspx?key={WebUtils.UrlStringEncode($"mach={nosovle}")}\">{rows.Length}</a></u>" : $"<u><a href=\"Asm_NoSolve_ITEC.aspx?key={WebUtils.UrlStringEncode($"mach={nosovle}")}\">{rows.Length}</a></u>";
                            }
                            else
                            {
                                _異常量 = "0";
                            }

                        }
                        else
                        {
                            _異常量 = "0";
                        }
                        break;
                    case "上期尚未生產":
                        sqlcmd = PPC.ProductionDelay_CMD(date_str);
                        sqlcmd += $" and 產線群組='{LineGroup}'";
                        rows = dt_本月應生產.Select(sqlcmd);
                        上期尚未生產 = rows.Length;
                        break;
                    case "下期提前生產":
                        sqlcmd = PPC.ProductionAhead_CMD(d_end);
                        sqlcmd += $" and 產線群組='{LineGroup}'";
                        rows = dt_本月應生產.Select(sqlcmd);
                        下期提前生產 = rows.Length;
                        break;
                }

                value = "";
                if (row_name == "本期計畫生產")
                {
                    _預定生產 = _預定生產 == 0 ? 0 : _預定生產;
                    value = _預定生產.ToString();
                }
                if (row_name == "目前累計應有進度")
                    value = _預計生產量.ToString();
                if (row_name == "目前累計實際進度")
                    value = _實際生產.ToString();
                if (row_name == "差異量")
                {
                    
                    value = 差異量;
                }

                if (row_name == "昨日應有進度")
                    value = _昨日預定生產.ToString();
                if (row_name == "昨日實際進度")
                    value = _昨日實際生產.ToString();
                if (row_name == "昨日人力(實到/應到)")
                    value = $"{實到人數}/{應到人數}";
                if (row_name == "異常")
                    value = _異常量.ToString();
                if (row_name == "月產能")
                    value = 月產能.ToString();
                if (row_name == "上期尚未生產")
                    value = 上期尚未生產.ToString();
                if (row_name == "下期提前生產")
                    value = 下期提前生產.ToString();

            }
            return value == "" ? "" : $"<td style=\"text-align:center;\"> {value} </td>";
        }
        //20220822 計算工作天,生產推移圖專用
        private string month_WorkDay(string date_str, string date_end)
        {
            string[] arr = monthInterval(date_str, date_end);
            string startDay_Interval = arr[0];
            string endDay_Interval = arr[1];
            int day = int.Parse(arr[2]);
            string sqlcmd = $"SELECT count(IsDay)月假日 FROM WorkTime_Holiday where PK_Holiday between {startDay_Interval} and {endDay_Interval}";
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
            DataTable dt_Holiday = DataTableUtils.GetDataTable(sqlcmd);
            int Holiday = int.Parse(DataTableUtils.toString(dt_Holiday.Rows[0]["月假日"]));
            string work_Day = (day - Holiday).ToString();
            string value = work_Day;
            return value;
        }
        //20220822 計算月區間,生產推移圖專用
        private string[] monthInterval(string date_str, string date_end)
        {
            string[] arr = new string[3];
            //轉換日期型態
            DateTime endDay = DateTime.ParseExact(date_end, "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
            DateTime startDay = DateTime.ParseExact(date_str, "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
            arr[0] = startDay.ToString("yyyyMMdd");
            arr[1] = endDay.ToString("yyyyMMdd");
            arr[2] = (((endDay - startDay).Days) + 1).ToString();
            return arr;
        }

        public void GetWorkerWorkTimeDataTable(string factory_floor)
        {

            string sqlcmd = "";
            if (factory_floor == "sowon")
            {
                sqlcmd = "select 日期,工作站編號 as 產線代號,工作站名稱,應到人數,實到人數 from 人員工時表";
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
            }

            else
            {
                sqlcmd = "select 日期,(CASE when 工作站編號='1' then '100' when 工作站編號='2' then'110' else 工作站編號 End) as 產線代號,工作站名稱,應到人數,實到人數 from 人員工時表";
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);
            }
            dt_WorkerWorkTime = DataTableUtils.GetDataTable(sqlcmd);
            if (HtmlUtil.Check_DataTable(dt_WorkerWorkTime))
            {
                dt_WorkerWorkTime = myclass.Add_LINE_GROUP(dt_WorkerWorkTime).ToTable();
            }
        }

        public void GetWorkerWorkTime(string excepted_fn_day, string today)
        {

            string sqlcmd = $"日期='{excepted_fn_day}'";
            if (dropdownlist_Factory.SelectedItem.Value == "sowon")
            {
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
            }

            else
            {
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);
            }
            dt_WorkerWorkTime = DataTableUtils.GetDataTable(sqlcmd);
        }
        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>舊架構End<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//

        //20221208 生產推移圖-預計生產折線圖資料 by秋雄
        public string ExpectedProduction_Chart(string expected_fn_day, int qty)
        {
            string value = "";
            value = "{" + $"label:'{expected_fn_day.Substring(6, 2)}日',y:{qty}" + "},";
            return value;
        }
        //20221208 生產推移圖-預計生產數量 by秋雄
        public int ExpectedProduction_Qty(string expected_fn_day,string today,string date_str, int qty)
        {
            int value = 0;
            value = DataTableUtils.toInt(expected_fn_day) <= DataTableUtils.toInt(today) ? qty : 0;

            if (DataTableUtils.toInt(date_str) > DataTableUtils.toInt(today))
            {
                value = DataTableUtils.toInt(expected_fn_day) == DataTableUtils.toInt(date_str) ? qty : 0;
            }
            return value;
        }
        //20221208 生產推移圖-實際生產篩選條件 by秋雄
        public string ActuallyProduction_CMD(string expected_fn_day, string date_end,bool first_day)
        {
            string sqlcmd = "";
            if (first_day)
            {   
                //不顯示提早完成
                //sqlcmd = $"(預計完工日<={date_end} and 實際完成時間  LIKE '%{expected_fn_day}%' and 狀態 = '完成' )";
                //顯示提早完成
                sqlcmd = $"(預計完工日<={date_end} and (實際完成時間  LIKE '%{expected_fn_day}%' OR 實際完成時間<='{expected_fn_day}') and 狀態 = '完成' )";
            }
            else
            {
                sqlcmd = $"預計完工日<={date_end} and 實際完成時間  LIKE '%{expected_fn_day}%' and 狀態 = '完成'";
            }

            return sqlcmd;
        }
        //20221208 生產推移圖-實際生產折線圖資料 by秋雄
        public string ActuallyProduction_Chart(string expected_fn_day, string today,string date_str,int qty)
        {
            string value = "";
            if (DataTableUtils.toInt(today) >= DataTableUtils.toInt(date_str))
                value = DataTableUtils.toInt(today) >= DataTableUtils.toInt(DataTableUtils.toString(expected_fn_day)) ? "{" + $"label:'{expected_fn_day.Substring(6, 2)}日',y:{qty} " + "}," : "";
            else
                value = "{" + $"label:'{expected_fn_day.Substring(6, 2)}日',y:{qty} " + "},";
            return value;
        }

        //20221208 生產推移圖-實際生產數量 by秋雄
        public int ActuallyProduction_Qty(string expected_fn_day, string today, int qty)
        {
            int value = 0;
            //實際生產數量
            value=DataTableUtils.toInt(today) >= DataTableUtils.toInt(expected_fn_day) ? qty : 0;
            return value;
        }

    }
}
