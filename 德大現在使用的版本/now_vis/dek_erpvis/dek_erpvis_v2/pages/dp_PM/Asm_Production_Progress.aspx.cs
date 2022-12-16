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
    public partial class Asm_Production_Progress : System.Web.UI.Page
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
        ShareFunction sfun = new ShareFunction();
        ERP_Sales SLS = new ERP_Sales();
        public int 預定生產_data_y_max = 0;
        public int 預計生產量_至今 = 0;
        public string 應有進度 = "3";
        public int 實際生產_data_y_max = 0;
        public string 實際進度 = "3";
        public string 差值 = "";
        public string timerange = "";
        public string hide_image = "1";
        public string 預定生產_data = "";
        public string 實際生產_data = "";
        public string 實領料數_data = "";
        public string 本期未生產_persent = "";
        public int 上期尚未生產 = 0;
        public int 下期提前生產 = 0;
        public string Hor_th = "";
        public string Hor_tr = "";
        public string Assm_th = "";
        public string Assm_tr = "";

        DataTable month_Assm = new DataTable();
        DataTable month_Hor = new DataTable();
        DataTable Hor_Error = new DataTable();
        DataTable Assm_Error = new DataTable();
        DataTable Hor_Worker = new DataTable();
        DataTable Assm_Worker = new DataTable();
        DataTable month_Capacity = new DataTable();
        DataTable Hor_Capacity = new DataTable();
        DataTable Assm_Capacity= new DataTable();

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
                        MainProcess();
                    }
                }
                else
                    Response.Write("<script>alert('您無此權限!');location.href='../index.aspx';</script>");
            }
            else
                Response.Redirect(myclass.logout_url);

        }

        //------------------------------------------------------------Function-------------------------------------------------
        //需要執行副程式
        private void MainProcess()
        {
            month_Assm = Get_AssmDataTable(date_str, date_end);
            month_Assm = DataTableMerge(month_Assm, date_str, date_end);
            Assm_Error = Get_AbbormalTable(month_Assm, "sowon");
            Assm_Worker = GetWorkerWorkTimeDataTable(date_str, date_end, "sowon");
            Set_Month_Capacity_col(month_Assm, Assm_Worker, Assm_Error, Assm_Capacity, "sowon");
            month_Hor = Get_HorDataTable(date_str, date_end);
            month_Hor = DataTableMerge(month_Hor, date_str, date_end);
            Hor_Error =Get_AbbormalTable(month_Hor,"iTec");
            Hor_Worker = GetWorkerWorkTimeDataTable(date_str, date_end, "iTec");
            Set_Month_Capacity_col(month_Hor, Hor_Worker, Hor_Error, Hor_Capacity, "iTec");
        }

        //取得本月應做資料
        private DataTable Get_AssmDataTable(string date_str, string date_end)
        {
            DataTable dt_month = new DataTable();
            string sqlcmd = "";
            //20221125 新規則-將預計開工於下個月但在這個月完成的資料抓出來
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
            DateTime datetime = DateTime.ParseExact(date_end, "yyyyMMdd", null);
            datetime = datetime.AddMonths(1);
            string upper_Month = (HtmlUtil.StrToDate(date_str).AddMonths(-1)).ToString("yyyyMMdd");
            string next_MonthEnd = datetime.ToString("yyyyMMdd");
            sqlcmd = SLS.Waitingfortheproduction_Assm_Table(upper_Month, date_str, next_MonthEnd, "sowon");
            DataTable dt_sowon = DataTableUtils.GetDataTable(sqlcmd);
            dt_month = dt_sowon;

            //舊規則
            //GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
            //sqlcmd = SLS.Waitingfortheproduction_Assm_Table(date_end, date_str, dropdownlist_Factory.SelectedItem.Value);
            //DataTable dt_sowon = DataTableUtils.GetDataTable(sqlcmd);
            //dt_month = dt_sowon;

            //20220822 增加月產能表格
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
            Assm_Capacity = DataTableUtils.GetDataTable("select 工作站編號 AS 產線代號,工作站名稱,目標件數 AS 日產能 FROM 工作站型態資料表 WHERE 工作站是否使用中='1'");
            Assm_Capacity = myclass.Add_LINE_GROUP(Assm_Capacity).ToTable();
            return dt_month;
        }
        private DataTable Get_HorDataTable(string date_str, string date_end)
        {
            DataTable dt_month = new DataTable();
            DataTable dt_BigDisck = new DataTable();
            string sqlcmd = "";
            
            dt_BigDisck = get_大圓盤(date_str, date_end);

            //20221125 新規則-將預計開工於下個月但在這個月完成的資料抓出來
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);
            string condition_copy = condition.Replace("工作站編號", " a.工作站編號 ");
            string upper_Month = (HtmlUtil.StrToDate(date_str).AddMonths(-1)).ToString("yyyyMMdd");
            DateTime datetime = DateTime.ParseExact(date_end, "yyyyMMdd", null);
            datetime = datetime.AddMonths(1);
            string next_MonthEnd = datetime.ToString("yyyyMMdd");
            sqlcmd = SLS.Waitingfortheproduction_Hor_Table(upper_Month, condition_copy, date_str, next_MonthEnd, "iTec");

            //舊規則
            //GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);
            //string condition_copy = condition.Replace("工作站編號", " a.工作站編號 ");
            //string upper_Month = (HtmlUtil.StrToDate(date_str).AddMonths(-1)).ToString("yyyyMMdd");
            //sqlcmd = SLS.Waitingfortheproduction_Hor_Table(upper_Month, condition_copy, date_str, date_end, dropdownlist_Factory.SelectedItem.Value);
            DataTable dt_sowon = DataTableUtils.GetDataTable(sqlcmd);

            //20220822 增加月產能表格,大圓盤&臥式統一在此取得
            Hor_Capacity = DataTableUtils.GetDataTable("select (CASE when 工作站編號='1' then '100' when 工作站編號='2' then'110' else 工作站編號 End) AS 產線代號,工作站名稱,目標件數 AS 月產能 FROM 工作站型態資料表 WHERE 工作站是否使用中='1'");

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
                else
                {
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
                else
                {
                    dt_sowon.Columns.Add("預計完工日");
                    dt_BigDisck.Merge(dt_sowon);
                    dt_month = dt_BigDisck;

                }
            }

            return dt_month;
        }
        public DataTable DataTableMerge(DataTable dt ,string date_str,string date_end) {
            DataTable dt_return = new DataTable();
            DataTable dt_now = new DataTable();
            //產生 本月應做 之前未做完但在本月做完 到目前為止皆未完成之資料表
            if (HtmlUtil.Check_DataTable(dt))
                dt_now = sfun.Return_NowMonthTotal(dt, date_str, date_end, out dt_Finish, out dt_NoFinish);

            if (HtmlUtil.Check_DataTable(dt_now))
            {
                dt_return = dt_now.Clone();

                //20220826 PrimaryKey Merge時如無主鍵,資料會重複
                dt_now.PrimaryKey = new DataColumn[] { dt_now.Columns["排程編號"], dt_now.Columns["工作站編號"] };
                dt_return.PrimaryKey = new DataColumn[] { dt_return.Columns["排程編號"], dt_return.Columns["工作站編號"] };
                dt_return.Merge(dt_now);
            }
            else
            {
                dt_return = dt.Clone();
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
        private DataTable get_大圓盤(string date_str,string date_end) {
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

        private DataTable Get_AbbormalTable(DataTable dt,string factory) {
            //列出目前所有排程
            DataTable dt_unsolved = new DataTable();
            string Error_Number = "";
            bool ok = true;
            if (HtmlUtil.Check_DataTable(dt))
            {
                foreach (DataRow row in dt.Rows)
                {
                    Error_Number += ok ? $"  排程編號='{row["排程編號"]}' " : $" OR 排程編號='{row["排程編號"]}' ";
                    ok = false;
                }
                Error_Number = Error_Number != "" ? $" where ( {Error_Number} ) " : "";
            }

            
            if (factory == "sowon")
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
                dt_unsolved = DataTableUtils.GetDataTable(sqlcmd);
                dt_unsolved = myclass.Add_LINE_GROUP(dt_unsolved).ToTable();
            }
            else
            {
                Error_Number = Error_Number != "" ? $" {Error_Number} and (工作站異常維護資料表.工作站編號='1' OR 工作站異常維護資料表.工作站編號='2'  OR 工作站異常維護資料表.工作站編號='9' ) " : "";
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);
                string sqlcmd = $" SELECT DISTINCT 異常維護編號,(CASE when 工作站編號='1' then '100' when 工作站編號='2' then'110' else 工作站編號 End) AS 產線代號,排程編號,a.結案判定類型  FROM 工作站異常維護資料表 LEFT JOIN (SELECT max(異常維護編號) 編號,排程編號 排程號碼,父編號,結案判定類型 FROM 工作站異常維護資料表 where    結案判定類型 IS NOT NULL group by 父編號,排程編號,結案判定類型) a ON a.排程號碼 = 工作站異常維護資料表.排程編號 AND 工作站異常維護資料表.異常維護編號 = a.父編號 {Error_Number} AND (工作站異常維護資料表.父編號 IS NULL OR 工作站異常維護資料表.父編號=0) AND 工作站編號=1 AND a.結案判定類型 IS NULL ORDER BY 工作站異常維護資料表.排程編號";

                //20221212 無資料不抓異常
                if (Error_Number == "")
                    sqlcmd = "";

                dt_unsolved = DataTableUtils.GetDataTable(sqlcmd);


                dt_unsolved = myclass.Add_LINE_GROUP(dt_unsolved).ToTable();
            }
            return dt_unsolved;
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

        private void Set_Month_Capacity_col(DataTable dt,DataTable worker,DataTable error_unsolved,DataTable capacityTable, string factory)
        {
            if (HtmlUtil.Check_DataTable(dt))
            {
                List<ProductionProgress> productionProgress = new List<ProductionProgress>();
                if (factory == "iTec")
                {
                    
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["產線代號"].ToString() == "1")
                            row["產線代號"] = "100";
                        else if (row["產線代號"].ToString() == "2")
                            row["產線代號"] = "110";

                    }
                    dt = myclass.Add_LINE_GROUP(dt).ToTable();
                }
                else {
                    dt = myclass.Add_LINE_GROUP(dt).ToTable();
                }
                
                DataTable dt_Copy = dt.Clone();

                //清空陣列
                //Line_Name.Clear();
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
                table.Rows.Add("異常");
                string columns = "";
                //20220729 新稱月產能
                productionProgress.Clear();
                productionProgress.Add(new ProductionProgress() { FactoryFloor = factory, FactoryDataTable = dt, WorkerDataTable = worker, ErrorUsolvedDataTable = error_unsolved,CapacitydDataTable=capacityTable });
                if (factory == "sowon")
                {
                    Assm_th=HtmlUtil.Set_Table_Title(table, out columns, "", "style=\"text-align:center;background-color:#2A3F54;color:white;\"");
                    Assm_tr=HtmlUtil.DaTaTable_To_HtmlTable(table, productionProgress, columns, ProductionProcessTable_CallBack, "style=\"text-align:center;background-color:#2A3F54;color:white;\"");
                }
                else
                {
                    Hor_th = HtmlUtil.Set_Table_Title(table, out columns, "", "style=\"text-align:center;background-color:#2A3F54;color:white;\"");
                    Hor_tr = HtmlUtil.DaTaTable_To_HtmlTable(table, productionProgress, columns, ProductionProcessTable_CallBack, "style=\"text-align:center;background-color:#2A3F54;color:white;\"");
                }

            }
            else {
                if (factory == "sowon")
                {
                    HtmlUtil.NoData(out Assm_th, out Assm_tr);
                }
                else
                {
                    HtmlUtil.NoData(out Hor_th, out Hor_tr);
                }

            }
                
        }




        private string ProductionProcessTable_CallBack(List<ProductionProgress> productionProgresses, DataRow row, string field_name)
        {
            
            DataTable dt_factory = new DataTable();
            DataTable dt_worker = new DataTable();
            DataTable error_unsolved = new DataTable();
            DataTable dt_capacity = new DataTable();
            string factoryFloor = ""; 
            foreach (ProductionProgress productionProgress in productionProgresses)
            {
                dt_factory = productionProgress.FactoryDataTable;
                factoryFloor = productionProgress.FactoryFloor;
                dt_worker = productionProgress.WorkerDataTable;
                error_unsolved = productionProgress.ErrorUsolvedDataTable;
                dt_capacity = productionProgress.CapacitydDataTable;
            }
            DataTable dt_day = Get_Monthday(date_str, date_end);
            string sqlcmd = "";
            int _預定生產 = 0;
            int _預計生產量 = 0;
            int _實際生產 = 0;
            int _昨日預定生產 = 0;
            int _昨日實際生產 = 0;
            int 月產能 = 0;
            string 差異量 = "";
            DataRow[] rows = null;
            string LineGroup = field_name;
            string today = DateTime.Now.ToString("yyyyMMdd");
            string value = "";
            int 應到人數 = 0;
            int 實到人數 = 0;
            string _異常量 = "";
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
                        if (factoryFloor == "sowon")
                        {

                            object dd = dt_capacity.Compute("sum(日產能)", $"產線群組='{field_name}'");
                            int workDay = int.Parse(month_WorkDay(date_str, date_end));
                            月產能 = (int.Parse(dd.ToString()) * workDay);
                        }
                        else
                        {
                            string line_Group = field_name == "NEW INTE" ? "NEW_INTE" : field_name;
                            object dd = dt_capacity.Compute("sum(月產能)", $"工作站名稱='{line_Group}'");
                            月產能 = int.Parse(dd.ToString());
                        }
                        break;
                    case "本期計畫生產":
                        //預計生產
                        sqlcmd = ExpectedProduction_CMD(date_end, today, date_str, true);
                        sqlcmd += $" AND 產線群組='{LineGroup}'";
                        rows = dt_factory.Select(sqlcmd);
                        _預定生產 = rows.Length;
                        break;
                    case "目前累計應有進度":
                        sqlcmd = ExpectedProduction_CMD(today, today, date_str, true);
                        sqlcmd += $" AND 產線群組='{LineGroup}'";
                        rows = dt_factory.Select(sqlcmd);
                        //到今天為止應生產多少
                        _預計生產量 = rows.Length;
                        break;
                    case "目前累計實際進度":
                        //實際生產
                        sqlcmd = $"(預計完工日<={d_end} and 狀態 = '完成')";
                        sqlcmd += $" and 產線群組='{LineGroup}'";
                        rows = dt_factory.Select(sqlcmd);

                        _實際生產 = rows.Length;

                        break;
                    case "差異量":

                        sqlcmd = $"(預計完工日<={d_end} and 狀態 = '完成')";
                        sqlcmd += $" and 產線群組='{LineGroup}'";
                        rows = dt_factory.Select(sqlcmd);

                        _實際生產 = rows.Length;

                        sqlcmd = ExpectedProduction_CMD(today, today, date_str, true);
                        sqlcmd += $" AND 產線群組='{LineGroup}'";
                        rows = dt_factory.Select(sqlcmd);
                        //到今天為止應生產多少
                        _預計生產量 = rows.Length;

                        差異量 = (_預計生產量 - _實際生產).ToString();
                        break;
                    case "昨日應有進度":
                        if (today.Contains("26"))
                        {
                            string startDay = "";
                            string endDay = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");
                            Dictionary<string, string> dc = new Dictionary<string, string>();
                            dc = sfun.monthInterval(endDay, acc);
                            startDay = dc["startDay"];
                            endDay = dc["endDay"];
                            switch (factoryFloor)
                            {
                                case "sowon":
                                    DataTable last_month_Assm = Get_AssmDataTable(startDay, endDay);
                                    dt_factory = DataTableMerge(last_month_Assm, startDay, endDay);
                                   
                                    dt_factory = myclass.Add_LINE_GROUP(dt_factory).ToTable();
                                    
                                    sqlcmd = ExpectedProduction_CMD("20221125", today, startDay, true);
                                    break;
                                case "iTec":
                                    DataTable last_month_Hor = Get_HorDataTable(startDay, endDay);
                                    dt_factory = DataTableMerge(last_month_Hor, startDay, endDay);
                                    

                                        foreach (DataRow factoryRow in dt_factory.Rows)
                                        {
                                            if (factoryRow["產線代號"].ToString() == "1")
                                            factoryRow["產線代號"] = "100";
                                            else if (factoryRow["產線代號"].ToString() == "2")
                                            factoryRow["產線代號"] = "110";
                                        }
                                    dt_factory = myclass.Add_LINE_GROUP(dt_factory).ToTable();
                                    
                                    sqlcmd = ExpectedProduction_CMD("20221125", today, startDay, true);
                                    break;
                            }

                        }
                        else 
                        {
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
                            //        string _yesterday = DateTime.ParseExact(date_end, "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces).AddDays(-1).ToString("yyyyMMdd");
                            //        sqlcmd = $"(預計完工日<={_yesterday} and (實際完成時間 >='{d_start}' and 實際完成時間<={_yesterday}) and 狀態 = '完成')";
                            //    }
                            //    else
                            //    {
                            //        //本月昨天
                                    sqlcmd = ExpectedProduction_CMD(yesterday, today, date_str, true);
                            //    }
                            //}
                        }
                        
                        sqlcmd += $" AND 產線群組='{LineGroup}'";
                        rows = dt_factory.Select(sqlcmd);
                        DataTable dtss = dt_factory.Clone();
                        for (int i = 0; i < rows.Length; i++)
                            dtss.ImportRow(rows[i]);
                        _昨日預定生產 = rows.Length;
                        break;

                    case "昨日實際進度":
                        if (today.Contains("26"))
                        {
                            string startDay = "";
                            string endDay = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");
                            Dictionary<string, string> dc = new Dictionary<string, string>();
                            dc = sfun.monthInterval(endDay, acc);
                            startDay = dc["startDay"];
                            endDay = dc["endDay"];
                            switch (factoryFloor)
                            {
                                case "sowon":
                                    DataTable last_month_Assm = Get_AssmDataTable(startDay, endDay);
                                    dt_factory = DataTableMerge(last_month_Assm, startDay, endDay);

                                    dt_factory = myclass.Add_LINE_GROUP(dt_factory).ToTable();

                                    //sqlcmd = ProductionFullCountForYesterday_CMD(startDay, endDay, "20221125");
                                    sqlcmd = ProductionFullCount_CMD(endDay);
                                    break;
                                case "iTec":
                                    DataTable last_month_Hor = Get_HorDataTable(startDay, endDay);
                                    dt_factory = DataTableMerge(last_month_Hor, startDay, endDay);


                                    foreach (DataRow factoryRow in dt_factory.Rows)
                                    {
                                        if (factoryRow["產線代號"].ToString() == "1")
                                            factoryRow["產線代號"] = "100";
                                        else if (factoryRow["產線代號"].ToString() == "2")
                                            factoryRow["產線代號"] = "110";
                                    }
                                    dt_factory = myclass.Add_LINE_GROUP(dt_factory).ToTable();

                                    //sqlcmd = ProductionFullCountForYesterday_CMD(startDay,endDay,"20221125");
                                    sqlcmd= ProductionFullCount_CMD(endDay);
                                    break;
                            }

                        }
                        else
                        {
                            ////新
                            //if (this_month > past_month)
                            //{
                            //    if (int.Parse(today) > int.Parse(date_end))
                            //    {
                            //        //上個月昨天
                            //        string last_month_yesterday = DateTime.ParseExact(date_end, "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces).AddDays(-1).ToString("yyyyMMdd");
                            //        sqlcmd = $"(預計完工日<={date_end} and (實際完成時間 >='{d_start}' and 實際完成時間<={last_month_yesterday}) and 狀態 = '完成')";
                            //        sqlcmd = ProductionFullCountForYesterday_CMD(d_start, date_end, last_month_yesterday);
                            //    }

                            //}
                            //else
                            //{
                            //    if (int.Parse(today) > int.Parse(date_end))
                            //    {
                            //        //上個月昨天
                            //        string _yesterday = DateTime.ParseExact(date_end, "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces).AddDays(-1).ToString("yyyyMMdd");
                            //        sqlcmd = $"(預計完工日<={d_end} and (實際完成時間 >='{d_start}' and 實際完成時間<={_yesterday}) and 狀態 = '完成')";
                            //        sqlcmd = ProductionFullCountForYesterday_CMD(d_start, d_end, _yesterday);
                            //    }
                            //    else
                            //    {
                                    sqlcmd = ProductionFullCountForYesterday_CMD(d_start, d_end, yesterday);
                            //    }
                            //}
                        }
                            
                        sqlcmd += $" and 產線群組='{LineGroup}'";
                        rows = dt_factory.Select(sqlcmd);
                        DataTable dts = dt_factory.Clone();
                        for (int i = 0; i < rows.Length; i++)
                            dts.ImportRow(rows[i]);
                        _昨日實際生產 = rows.Length;
                        break;

                    case "昨日人力(實到/應到)":

                        if (HtmlUtil.Check_DataTable(dt_worker))
                        {

                            sqlcmd = $"日期='{yesterday}' and 產線群組='{LineGroup}'";
                            rows = dt_worker.Select(sqlcmd);

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
                        _異常量 = ErrorCountForSingleLine(error_unsolved, LineGroup, factoryFloor);
                        break;
                    case "上期尚未生產":
                        sqlcmd = ProductionDelay_CMD(date_str);
                        sqlcmd += $" and 產線群組='{LineGroup}'";
                        rows = dt_factory.Select(sqlcmd);
                        上期尚未生產 = rows.Length;
                        break;
                    case "下期提前生產":
                        sqlcmd = ProductionAhead_CMD(d_end);
                        sqlcmd += $" and 產線群組='{LineGroup}'";
                        rows = dt_factory.Select(sqlcmd);
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

        //20221216 顯示錯誤台數單獨產線計算
        private string ErrorCountForSingleLine(DataTable ErrorUnsolved,string LineGroup,string FactoryFloor)
        {
            string sqlcmd = "";
            string value = "";
            if (HtmlUtil.Check_DataTable(ErrorUnsolved))
            {
                sqlcmd = $"產線群組='{LineGroup}'";
                DataRow[] rows = ErrorUnsolved.Select(sqlcmd);
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
                    value = FactoryFloor == "sowon" ? $"<u><a href=\"Asm_NoSolve.aspx?key={WebUtils.UrlStringEncode($"mach={nosovle}")}\">{rows.Length}</a></u>" : $"<u><a href=\"Asm_NoSolve_ITEC.aspx?key={WebUtils.UrlStringEncode($"mach={nosovle}")}\">{rows.Length}</a></u>";
                }
                else
                {
                    value = "0";
                }

            }
            else
            {
                value = "0";
            }
            return value;
        }






        //20220822 計算工作天,生產推移圖專用
        private string month_WorkDay(string date_str,string date_end)
        {   
            string[] arr = monthInterval(date_str,date_end);
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
        private string[] monthInterval(string date_str,string date_end)
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


        //20221208 生產推移圖-預計生產篩選條件 by秋雄
        public string ExpectedProduction_CMD(string expected_fn_day, string today, string date_str, bool first_day) {
            string sqlcmd = "";
                //預計生產 20221209 
                if (DataTableUtils.toInt(today) >= DataTableUtils.toInt(date_str))
                //中間的 substring(實際完成時間, 1, 8)>='{date_str}' 是為了將逾期完成,單在本月完成後的資料抓出,否則完成前會出現,完成後則抓不到
                //1.不顯示已提早在上期完成的數量
                //sqlcmd = first_day ? $"((預計完工日 <= '{expected_fn_day}' and (substring(實際完成時間, 1, 8)>='{date_str}' OR 實際完成時間 IS NULL  OR 實際完成時間 =''))  OR 預計完工日='開發機' ) " : $"預計完工日 = '{expected_fn_day}' and (substring(實際完成時間, 1, 8)>='{date_str}' OR 實際完成時間 IS NULL  OR 實際完成時間 ='')";
                //2.顯示已在上期提早完成的數量
                sqlcmd = first_day ? $"((預計完工日 <= '{expected_fn_day}' and (substring(實際完成時間, 1, 8)>='{date_str}' OR substring(實際完成時間, 1, 8)<='{date_str}' OR 實際完成時間 IS NULL  OR 實際完成時間 =''))  OR 預計完工日='開發機' ) " : $"預計完工日 = '{expected_fn_day}' and (substring(實際完成時間, 1, 8)>='{date_str}' OR 實際完成時間 IS NULL  OR 實際完成時間 ='')";

                else
                sqlcmd = $"預計完工日 = '{expected_fn_day}'";
            return sqlcmd;
        }

        //20221216 昨日實際生產總計篩選條件 by秋雄
        public string ProductionFullCountForYesterday_CMD(string start, string end, string yesterday)
        {
            string sqlcmd = "";
            //不顯示已在上期完成的數量
            //sqlcmd = $"(預計完工日<={end} and (實際完成時間 >='{start}' and 實際完成時間<={yesterday}) and 狀態 = '完成')";
            //顯示已在上期完成的數量
            sqlcmd = $"(預計完工日<={end} and ( 實際完成時間<={yesterday}) and 狀態 = '完成')";
            return sqlcmd;
        }
        //20221216 實際生產總計篩選條件 by秋雄
        public string ProductionFullCount_CMD(string end)
        {
            string sqlcmd = "";
            //不顯示已在上期完成的數量
            //sqlcmd = $"(預計完工日<={end} and (實際完成時間 >='{start}' and 實際完成時間<={yesterday}) and 狀態 = '完成')";
            //顯示已在下期提早完成數量
            sqlcmd = $"((預計完工日<={end} OR 預計完工日>={end}) and 實際完成時間<='{end}' and 狀態 = '完成')";
            return sqlcmd;
        }

        public DataTable GetWorkerWorkTimeDataTable(string date_str,string date_end,string factory) {
            DataTable dt = new DataTable();
            string sqlcmd = "";
            if (factory == "sowon") 
            {
                 sqlcmd = $"select 日期,工作站編號 as 產線代號,工作站名稱,應到人數,實到人數 from 人員工時表 where 日期>='{date_str}' and 日期<='{date_end}'";
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
            }

            else
            {
                 sqlcmd = $"select 日期,(CASE when 工作站編號='1' then '100' when 工作站編號='2' then'110' else 工作站編號 End) as 產線代號,工作站名稱,應到人數,實到人數 from 人員工時表 where 日期>='{date_str}' and 日期<='{date_end}'";
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);
            }
            dt = DataTableUtils.GetDataTable(sqlcmd);
            if (HtmlUtil.Check_DataTable(dt)) 
            {
                dt = myclass.Add_LINE_GROUP(dt).ToTable();
            }
            return dt;
        }

        
        //20221212 生產推移圖-上期未生產生產篩選條件 by秋雄
        public string ProductionDelay_CMD(string date_str)
        {
            string sqlcmd = "";
            //延遲預計生產 20221214 
            
                sqlcmd =  $"((預計完工日 <= '{date_str}' and (substring(實際完成時間, 1, 8)>='{date_str}' OR 實際完成時間 IS NULL  OR 實際完成時間 =''))  OR 預計完工日='開發機' ) ";
            
            return sqlcmd;
        }
        //20221212 生產推移圖-下期提前生產篩選條件 by秋雄
        public string ProductionAhead_CMD(string date_end)
        {
            string sqlcmd = "";
            //提前生產 20221214 
            sqlcmd = $"((預計完工日 > '{date_end}' OR 預計完工日='開發機') and (substring(實際完成時間, 1, 8)<='{date_end}')) ";

            return sqlcmd;
        }

    }
}
