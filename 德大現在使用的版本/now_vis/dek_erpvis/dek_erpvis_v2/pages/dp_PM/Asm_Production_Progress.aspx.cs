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
        ProductionProgressChart PPC = new ProductionProgressChart();
        ERP_Sales SLS = new ERP_Sales();

        public string Hor_th = "";
        public string Hor_tr = "";
        public string Assm_th = "";
        public string Assm_tr = "";

        DataTable Assm_month = new DataTable();
        DataTable Hor_month = new DataTable();
        DataTable Hor_Error = new DataTable();
        DataTable Assm_Error = new DataTable();
        DataTable Hor_Staff = new DataTable();
        DataTable Assm_Staff = new DataTable();
        DataTable Hor_Capacity = new DataTable();
        DataTable Assm_Capacity= new DataTable();
        List<string> avoid_again = new List<string>();
        public StringBuilder th_month_capacity = new StringBuilder();
        public StringBuilder tr_month_capacity = new StringBuilder();
        
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
            Assm_month = PPC.Get_ProductionDataTable(date_str, date_end,"","sowon");
            Assm_Capacity = PPC.Get_CapacityDatable("sowon");
            Assm_month = PPC.DataTableMerge(Assm_month, date_str, date_end);
            Assm_Error = PPC.Get_AbbormalTable(Assm_month,"", "sowon");
            Assm_Staff = PPC.GetWorkerWorkTimeDataTable(date_str, date_end, "sowon");
            Set_ProductionProgressTable(Assm_month, Assm_Staff, Assm_Error, Assm_Capacity,acc,date_str,date_end,"", "sowon");

            Hor_month = PPC.Get_ProductionDataTable(date_str, date_end,"","iTec");
            Hor_Capacity = PPC.Get_CapacityDatable("iTec");
            Hor_month = PPC.DataTableMerge(Hor_month, date_str, date_end);
            Hor_Error = PPC.Get_AbbormalTable(Hor_month,"" ,"iTec");
            Hor_Staff = PPC.GetWorkerWorkTimeDataTable(date_str, date_end, "iTec");
            Set_ProductionProgressTable(Hor_month, Hor_Staff, Hor_Error, Hor_Capacity, acc, date_str, date_end,"", "iTec");
        }
        //--------------排程資料--------------------//
        

        
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

        private void Set_ProductionProgressTable(DataTable dt,DataTable Staff, DataTable error_unsolved,DataTable capacityTable,string acc,string date_start,string date_end,string condition ,string factory)
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
                productionProgress.Add(new ProductionProgress() {
                    FactoryFloor = factory,
                    Account=acc,
                    Date_Start= date_start,
                    Date_End=date_end,
                    Condition=condition,
                    FactoryDataTable = dt, 
                    StaffDataTable = Staff, 
                    ErrorUnsolvedDataTable = error_unsolved,
                    CapacitydDataTable=capacityTable 
                });
                if (factory == "sowon")
                {
                    Assm_th=HtmlUtil.Set_Table_Title(table, out columns, "", "style=\"text-align:center;background-color:#2A3F54;color:white;\"");
                    Assm_tr=HtmlUtil.DaTaTable_To_HtmlTable(table, productionProgress, columns, PPC.ProductionProgressTable_CallBack, "style=\"text-align:center;background-color:#2A3F54;color:white;\"");
                }
                else
                {
                    Hor_th = HtmlUtil.Set_Table_Title(table, out columns, "", "style=\"text-align:center;background-color:#2A3F54;color:white;\"");
                    Hor_tr = HtmlUtil.DaTaTable_To_HtmlTable(table, productionProgress, columns, PPC.ProductionProgressTable_CallBack, "style=\"text-align:center;background-color:#2A3F54;color:white;\"");
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
        //public string ExpectedProduction_CMD(string expected_fn_day, string today, string date_str, bool first_day) {
        //    string sqlcmd = "";
        //        //預計生產 20221209 
        //        if (DataTableUtils.toInt(today) >= DataTableUtils.toInt(date_str))
        //        //中間的 substring(實際完成時間, 1, 8)>='{date_str}' 是為了將逾期完成,單在本月完成後的資料抓出,否則完成前會出現,完成後則抓不到
        //        //1.不顯示已提早在上期完成的數量
        //        //sqlcmd = first_day ? $"((預計完工日 <= '{expected_fn_day}' and (substring(實際完成時間, 1, 8)>='{date_str}' OR 實際完成時間 IS NULL  OR 實際完成時間 =''))  OR 預計完工日='開發機' ) " : $"預計完工日 = '{expected_fn_day}' and (substring(實際完成時間, 1, 8)>='{date_str}' OR 實際完成時間 IS NULL  OR 實際完成時間 ='')";
        //        //2.顯示已在上期提早完成的數量
        //        sqlcmd = first_day ? $"((預計完工日 <= '{expected_fn_day}' and (substring(實際完成時間, 1, 8)>='{date_str}' OR substring(實際完成時間, 1, 8)<='{date_str}' OR 實際完成時間 IS NULL  OR 實際完成時間 =''))  OR 預計完工日='開發機' ) " : $"預計完工日 = '{expected_fn_day}' and (substring(實際完成時間, 1, 8)>='{date_str}' OR 實際完成時間 IS NULL  OR 實際完成時間 ='')";

        //        else
        //        sqlcmd = $"預計完工日 = '{expected_fn_day}'";
        //    return sqlcmd;
        //}

        //20221216 昨日實際生產總計篩選條件 by秋雄
        //public string ProductionFullCountForYesterday_CMD(string start, string end, string yesterday)
        //{
        //    string sqlcmd = "";
        //    //不顯示已在上期完成的數量
        //    //sqlcmd = $"(預計完工日<={end} and (實際完成時間 >='{start}' and 實際完成時間<={yesterday}) and 狀態 = '完成')";
        //    //顯示已在上期完成的數量
        //    sqlcmd = $"(預計完工日<={end} and ( 實際完成時間<={yesterday}) and 狀態 = '完成')";
        //    return sqlcmd;
        //}
        

     
        
        ////20221212 生產推移圖-上期未生產生產篩選條件 by秋雄
        //public string ProductionDelay_CMD(string date_str)
        //{
        //    string sqlcmd = "";
        //    //延遲預計生產 20221214 
            
        //        sqlcmd =  $"((預計完工日 <= '{date_str}' and (substring(實際完成時間, 1, 8)>='{date_str}' OR 實際完成時間 IS NULL  OR 實際完成時間 =''))  OR 預計完工日='開發機' ) ";
            
        //    return sqlcmd;
        //}
        //20221212 生產推移圖-下期提前生產篩選條件 by秋雄
        //public string ProductionAhead_CMD(string date_end)
        //{
        //    string sqlcmd = "";
        //    //提前生產 20221214 
        //    sqlcmd = $"((預計完工日 > '{date_end}' OR 預計完工日='開發機') and (substring(實際完成時間, 1, 8)<='{date_end}')) ";

        //    return sqlcmd;
        //}

    }
}
