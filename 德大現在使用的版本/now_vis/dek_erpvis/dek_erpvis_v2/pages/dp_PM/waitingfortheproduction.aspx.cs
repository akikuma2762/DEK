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
        public string th = "";
        public string tr = "";
        DataTable dt_本月應生產 = new DataTable();
        DataTable dt_未生產完成 = new DataTable();
        int total = 0;
        string Line = "";
        string condition = "";
        List<string> avoid_again = new List<string>();
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

                if (myclass.user_view_check(URL_NAME, acc))
                {
                    if (!IsPostBack)
                    {
                        var daterange = 德大機械.德大專用月份(acc).Split(',');
                        date_str = daterange[0];
                        date_end = daterange[1];
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
            Set_CheckBox();
            Set_SearchLine();
            Get_MonthTotal();
            Set_Chart();
            Set_Error();
            Set_Table();
            Set_ShowImage();
        }
        //顯示目前有的產線
        private void Set_CheckBox(string link = "sowon")
        {
            //確認有幾條產線
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisErp);
            string sqlcmd = "";
            if (link == "sowon")
                sqlcmd = "SELECT  ASSEMBLY_GROUP.GROUP_NAME,  ASSEMBLY_LINE.LINE_ID FROM ASSEMBLY_LINE  INNER JOIN ASSEMBLY_LINE_GROUP ON ASSEMBLY_LINE.LINE_ID = ASSEMBLY_LINE_GROUP.LINE_ID  INNER JOIN ASSEMBLY_GROUP ON ASSEMBLY_LINE_GROUP.GROUP_ID = ASSEMBLY_GROUP.GROUP_ID WHERE ASSEMBLY_LINE.LINE_STATUS = '1' and ASSEMBLY_GROUP.GROUP_NAME <> '臥式'  and ASSEMBLY_GROUP.GROUP_NAME <> 'NEW INTE'";
            else
                sqlcmd = "SELECT a.GROUP_NAME,b.LINE_ID FROM (SELECT ASSEMBLY_GROUP.GROUP_NAME GROUP_NAME, ASSEMBLY_LINE_GROUP.LINE_ID FROM ASSEMBLY_GROUP, ASSEMBLY_LINE_GROUP WHERE ASSEMBLY_LINE_GROUP.GROUP_ID = ASSEMBLY_GROUP.GROUP_ID and ISNUMERIC( ASSEMBLY_LINE_GROUP.LINE_ID)=1 ) a LEFT JOIN (SELECT OriLine 工作站編號,目標件數,TrsLine LINE_ID FROM detaVisHor.dbo.工作站型態資料表,detaVisHor.dbo.工作站對應資料表 WHERE 工作站是否使用中 = '1' and 工作站對應資料表.TrsLine = 工作站型態資料表.工作站編號 ) b ON cast(a.LINE_ID as int) = cast(b.工作站編號 as int) where 工作站編號 IS NOT NULL";
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
                condition += i == 0 ? $" 產線代號='{Line_Number[i]}' " : $" OR 產線代號='{Line_Number[i]}' ";

            condition = condition != "" ? $" and ( {condition} ) " : "";
        }
        //取得本月應做資料
        private void Get_MonthTotal()
        {
            timerange = $"'{HtmlUtil.changetimeformat(date_str, "/")}~{HtmlUtil.changetimeformat(date_end, "/")}'";
            dt_Finish = new DataTable();
            dt_NoFinish = new DataTable();
            DataTable dt_month = new DataTable();


            //立式廠 && 大圓盤資訊
           
                //找出本月需做之數量
                //--------------------------------------------------------德科部分-----------------------------------------------
                //從dek ERP撈取本月排程資料
                //20220728 大圓盤部分暫時全搜索,待從立式移除後,歸類於臥室
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekERPDataTable);
                string sqlcmd = "SELECT   " +
                                "cust.custnm2 AS 客戶簡稱, " +
                                "ws.PLINE_NO AS 產線代號, " +
                                "ws.pline_no AS 工作站編號,"+
                                "ws.lot_no AS 排程編號, " +
                                "ws.cord_no AS 訂單號碼, " +
                                "cord.cord_no AS 客戶訂單, " +
                                "mkordsub.item_no AS 品號," +
                                "DATE_FORMAT(cordsub.trn_date, '%Y%m%d') AS 訂單日期, " +
                                "DATE_FORMAT(mkordsub.str_date, '%Y%m%d') AS 預計開工日 " +
                                "FROM ws " +
                                "RIGHT JOIN mkordsub ON (ws.cord_no = mkordsub.cord_no AND ws.cord_sn = mkordsub.cord_sn AND (ws.lot_no = mkordsub.lot_no || ws.lot_no IS NULL || ws.lot_no = '')) " +
                                "LEFT JOIN item ON (mkordsub.item_no = item.item_no) " +
                                "LEFT JOIN cord ON (ws.cord_no = cord.trn_no) " +
                                "LEFT JOIN cordsub ON (ws.cord_no = cordsub.trn_no AND ws.cord_sn = cordsub.sn) " +
                                "LEFT JOIN invosub ON (ws.cord_no = invosub.cord_no AND ws.cord_sn = invosub.cord_sn AND ws.lot_no = invosub.lot_no) " +
                                "LEFT JOIN citem ON (citem.cust_no = mkordsub.cust_no AND citem.item_no = mkordsub.item_no) " +
                                "LEFT JOIN mkord ON (mkord.trn_no = mkordsub.trn_no) " +
                                "LEFT JOIN cust ON (cust.cust_no = mkord.cust_no) " +
                                "WHERE 1 = 1 AND ws.lot_no IS NOT NULL AND " +
                                "LENGTH(ws.lot_no) > 0 AND mkordsub.fclose IS NULL AND " +
                               $"DATE_FORMAT(mkordsub.str_date, '%Y%m%d') <= {date_end} AND DATE_FORMAT(mkordsub.str_date, '%Y%m%d') >= {HtmlUtil.StrToDate(date_str).AddMonths(-1):yyyyMMdd} AND "+
                               "ws.PLINE_NO <>''";

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

                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
                    sqlcmd = $" select " +
                             $"排程編號," +
                             $"進度," +
                             $"組裝日," +
                             $"狀態," +
                             $"組裝時間," +
                             $"SUBSTRING(實際完成時間,1,8) 實際完成時間  " +
                             $"from 工作站狀態資料表 " +
                             $"left join 立式工藝 on 立式工藝.機種編號 = (select top(1) value from STRING_SPLIT(工作站狀態資料表.排程編號, '-')) " +
                             $"where 工作站編號 = 11 {shcdulue} ";
                    DataTable dek_dtinformation = DataTableUtils.GetDataTable(sqlcmd);
                    DataTable dek_time = DataTableUtils.GetDataTable("select * from 立式工藝");



                    DataRow[] rows = null;
                    if (HtmlUtil.Check_DataTable(dek_dtinformation))
                    {
                        //填入資料
                        foreach (DataRow row in dek_dt.Rows)
                        {
                            if (row["排程編號"].ToString().Split('-')[0] == "R4MKV" && DataTableUtils.toInt(row["預計開工日"].ToString())>20220622)
                                sqlcmd = "";
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
            if (dropdownlist_Factory.SelectedItem.Value == "sowon")
            {
                //--------------------------------------------------------首旺部分-----------------------------------------------
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
                sqlcmd = "select " +
                         "a.*," +
                         "進度," +
                         "狀態," +
                         "組裝日," +
                         "a.預計開工日 預計完工日, " +
                         "SUBSTRING(實際完成時間,1,8) 實際完成時間 " +
                         "from ( " +
                         "      SELECT " +
                         "      CUSTNM2 客戶簡稱," +
                         "      FAB_USER 產線代號," +
                         "      FAB_USER 工作站編號," +
                         "      sw_MKORDSUB.LOT_NO 排程編號," +
                         "      A22_FAB.CORD_NO 訂單號碼," +
                         "      sw_CORD.CORD_NO  as 客戶訂單," +
                         "      sw_MKORDSUB.item_no as 品號," +
                         "      sw_item.itemnm as 品名規格, " +
                         "      sw_cordsub.trn_date as 訂單日期," +
                         "      A22_FAB.STR_DATE as 預計開工日," +
                         "      sw_MKORDSUB.SCLOSE 製令狀態 " +
                         "      FROM SW.FJWSQL.dbo.A22_FAB " +
                         "      LEFT JOIN SW.FJWSQL.dbo.CORD AS sw_CORD ON sw_CORD.trn_no = A22_FAB.cord_no  " +
                         "      LEFT JOIN SW.FJWSQL.dbo.CORDSUB AS sw_CORDSUB ON sw_CORDSUB.TRN_NO = A22_FAB.CORD_no AND sw_CORDSUB.SN = A22_FAB.CORD_SN " +
                         "      LEFT JOIN SW.FJWSQL.dbo.CUST AS sw_CUST ON sw_CUST.CUST_NO = sw_CORD.CUST_NO " +
                         "      LEFT JOIN SW.FJWSQL.dbo.MKORDSUB AS sw_MKORDSUB ON sw_MKORDSUB.CORD_NO = sw_CORDSUB.trn_no AND sw_MKORDSUB.CORD_SN = sw_CORDSUB.sn " +
                         "      LEFT JOIN SW.FJWSQL.dbo.citem AS sw_citem ON sw_CORDSUB.item_no = sw_citem.item_no " +
                         "      LEFT JOIN SW.FJWSQL.dbo.item_22 AS sw_item_22 ON sw_CORDSUB.item_no = sw_item_22.item_no " +
                         "      left join SW.FJWSQL.dbo.ITEM as sw_item on sw_item.ITEM_NO = sw_item_22.item_no " +
                         "      WHERE sw_MKORDSUB.FCLOSE <> 1 AND A22_FAB.STR_DATE > 20210101 AND " +
                        $"      A22_FAB.STR_DATE <= {date_end} and 1<=FAB_USER and FAB_USER<=99 ) a " +
                        $"left join 工作站狀態資料表 on 工作站狀態資料表.排程編號 = a.排程編號 and 工作站狀態資料表.工作站編號 = a.產線代號 " +
                        $"where (SUBSTRING(實際完成時間,1,8) >={date_str} OR 實際完成時間 is null OR 實際完成時間 = '') and ((a.製令狀態 = '結案' and 狀態 IS NOT NULL) OR (a.製令狀態 = '未結'))  " +
                        $"order by a.預計開工日";
                DataTable dt_sowon = DataTableUtils.GetDataTable(sqlcmd);
                if (HtmlUtil.Check_DataTable(dt_sowon))
                {
                    foreach (DataRow row in dt_sowon.Rows)
                        dt_month.ImportRow(row);
                }
            }

            //臥式廠部分
            else
            {
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);
                string condition_copy = condition.Replace("工作站編號", " a.工作站編號 ");
                sqlcmd = $"SELECT a.*,SUBSTRING(工作站狀態資料表.實際完成時間,1,8) 實際完成時間,工作站狀態資料表.狀態,(CASE a.產線代號 WHEN 1 THEN 組裝工時*60*60 WHEN 2 THEN 組裝工時*60*60 WHEN 4 THEN ((CAST(刀臂點數 AS float)+CAST(刀套點數 AS float))*60*60) WHEN 5 THEN (CAST(刀鍊點數 AS float)*60*60) WHEN 6 THEN (CAST(全油壓點數 AS float)*60*60) END) 標準工時 FROM (SELECT (case FAB_USER when '100' then '1' when '110' then '2' end)   產線代號, sw_MKORDSUB.LOT_NO 排程編號, A22_FAB.STR_DATE+'080000'  上線日,CUSTNM2 客戶簡稱, (case FAB_USER when '100' then '1' when '110' then '2' end) 工作站編號,sw_MKORDSUB.SCLOSE 製令狀態 FROM SW.FJWSQL.dbo.A22_FAB LEFT JOIN SW.FJWSQL.dbo.CORD AS sw_CORD ON sw_CORD.trn_no = A22_FAB.cord_no LEFT JOIN SW.FJWSQL.dbo.CORDSUB AS sw_CORDSUB ON sw_CORDSUB.TRN_NO = A22_FAB.CORD_no AND sw_CORDSUB.SN = A22_FAB.CORD_SN LEFT JOIN SW.FJWSQL.dbo.CUST AS sw_CUST ON sw_CUST.CUST_NO = sw_CORD.CUST_NO LEFT JOIN SW.FJWSQL.dbo.MKORDSUB AS sw_MKORDSUB ON sw_MKORDSUB.CORD_NO = sw_CORDSUB.trn_no AND sw_MKORDSUB.CORD_SN = sw_CORDSUB.sn LEFT JOIN SW.FJWSQL.dbo.citem AS sw_citem ON sw_CORDSUB.item_no = sw_citem.item_no LEFT JOIN SW.FJWSQL.dbo.item_22 AS sw_item_22 ON sw_CORDSUB.item_no = sw_item_22.item_no LEFT JOIN SW.FJWSQL.dbo.ITEM AS sw_item ON sw_item.ITEM_NO = sw_item_22.item_no WHERE sw_MKORDSUB.FCLOSE <> 1 AND A22_FAB.STR_DATE > 20210101 AND A22_FAB.STR_DATE <= {date_end} AND 100<=FAB_USER AND FAB_USER<=199 ) a LEFT JOIN 工作站狀態資料表 ON 工作站狀態資料表.排程編號 = a.排程編號 AND 工作站狀態資料表.工作站編號 = a.產線代號 LEFT JOIN 臥式工藝 ON 臥式工藝.機種編號 = SUBSTRING(a.排程編號, 1, CHARINDEX('-', a.排程編號) - 1) WHERE  ((組裝日>={HtmlUtil.StrToDate(date_str).AddMonths(-1):yyyyMMdd} AND 組裝日 <={date_end}) OR (實際完成時間 IS NULL OR LEN(實際完成時間) =0)) and LEN(客戶簡稱)>0 {condition_copy}  and   (SUBSTRING(實際完成時間,1, 8) >={date_str} OR 實際完成時間 IS NULL OR 實際完成時間 = '') AND ((a.製令狀態 = '結案' AND 狀態 IS NOT NULL) OR (a.製令狀態 = '未結')) ORDER BY 上線日";
                DataTable dt_sowon = DataTableUtils.GetDataTable(sqlcmd);
                //20220728 dt_sowon的標準工時在合併前須先轉換,否則後面會錯誤
                dt_sowon = sfun.Format_NowMonthTotal(dt_sowon);
                if (HtmlUtil.Check_DataTable(dt_sowon))
                {
                    foreach (DataRow row in dt_sowon.Rows)
                        dt_month.ImportRow(row);
                }
                //dt_month = DataTableUtils.GetDataTable(sqlcmd);
            }
            //---------------------------------------------------------------------------------合併------------------------------------------------------------
            //產生 本月應做 之前未做完但在本月做完 到目前為止皆未完成之資料表
            if (HtmlUtil.Check_DataTable(dt_month))
                dt_now = sfun.Return_NowMonthTotal(dt_month, date_str, date_end, out dt_Finish, out dt_NoFinish);

            if (dt_now != null)
            {
                dt_本月應生產 = dt_now.Clone();
                dt_now.PrimaryKey = new DataColumn[] { dt_now.Columns["排程編號"], dt_now.Columns["工作站編號"] };
                dt_本月應生產.PrimaryKey = new DataColumn[] { dt_本月應生產.Columns["排程編號"], dt_本月應生產.Columns["工作站編號"] };
                dt_本月應生產.Merge(dt_now);
            }
            if (HtmlUtil.Check_DataTable(dt_Finish) && DataTableUtils.toInt(DateTime.Now.ToString("yyyyMMdd")) >= DataTableUtils.toInt(date_str))
            {
                dt_Finish.PrimaryKey = new DataColumn[] { dt_Finish.Columns["排程編號"], dt_Finish.Columns["工作站編號"] };
                dt_本月應生產.Merge(dt_Finish, true, MissingSchemaAction.Ignore);
            }
            if (HtmlUtil.Check_DataTable(dt_NoFinish) && DataTableUtils.toInt(DateTime.Now.ToString("yyyyMMdd")) >= DataTableUtils.toInt(date_str))
            {
                dt_NoFinish.PrimaryKey = new DataColumn[] { dt_NoFinish.Columns["排程編號"], dt_NoFinish.Columns["工作站編號"] };
                dt_本月應生產.Merge(dt_NoFinish, true, MissingSchemaAction.Ignore);
            }
        }
        //顯示圖片
        private void Set_Chart()
        {
            DataTable dt_day = Get_Monthday(date_str, date_end);
            string sqlcmd = "";
            DataRow[] rows = null;
            for (int i = 0; i < dt_day.Rows.Count; i++)
            {
                //預計生產
                if (DataTableUtils.toInt(DateTime.Now.ToString("yyyyMMdd")) >= DataTableUtils.toInt(date_str))
                    sqlcmd = (i == 0) ? $"(預計完工日 <= '{dt_day.Rows[i]["日期"]}' OR 預計完工日='開發機') {condition} and (substring(實際完成時間, 1, 8)>='{date_str}'  OR 實際完成時間 IS NULL  OR 實際完成時間 ='' ) " : $"預計完工日 = '{dt_day.Rows[i]["日期"]}' {condition} and (substring(實際完成時間, 1, 8)>='{date_str}' OR 實際完成時間 IS NULL  OR 實際完成時間 ='') ";
                else
                    sqlcmd = $"預計完工日 = '{dt_day.Rows[i]["日期"]}' {condition} and (substring(實際完成時間, 1, 8)>='{date_str}' OR 實際完成時間 IS NULL  OR 實際完成時間 ='') ";

                rows = dt_本月應生產.Select(sqlcmd);
                預定生產_data += "{" + $"label:'{dt_day.Rows[i]["日期"].ToString().Substring(6, 2)}日',y:{rows.Length}" + "},";
                預定生產_data_y_max += rows.Length;

                //到今天為止應生產多少
                預計生產量_至今 += DataTableUtils.toInt(dt_day.Rows[i]["日期"].ToString()) <= DataTableUtils.toInt(DateTime.Now.ToString("yyyyMMdd")) ? rows.Length : 0;

                //實際生產
                sqlcmd = $" 實際完成時間  LIKE '%{dt_day.Rows[i]["日期"]}%' and 狀態 = '完成' {condition} ";
                rows = dt_本月應生產.Select(sqlcmd);
                if (DataTableUtils.toInt(DateTime.Now.ToString("yyyyMMdd")) >= DataTableUtils.toInt(date_str))
                    實際生產_data += DataTableUtils.toInt(DateTime.Now.ToString("yyyyMMdd")) >= DataTableUtils.toInt(DataTableUtils.toString(dt_day.Rows[i]["日期"])) ? "{" + $"label:'{dt_day.Rows[i]["日期"].ToString().Substring(6, 2)}日',y:{rows.Length} " + "}," : "";
                else
                    實際生產_data += "{" + $"label:'{dt_day.Rows[i]["日期"].ToString().Substring(6, 2)}日',y:{rows.Length} " + "},";

                實際生產_data_y_max += DataTableUtils.toInt(DateTime.Now.ToString("yyyyMMdd")) >= DataTableUtils.toInt(DataTableUtils.toString(dt_day.Rows[i]["日期"])) ? rows.Length : 0;
            }
            預定生產_data_y_max = 預定生產_data_y_max == 0 ? 1 : 預定生產_data_y_max;
            應有進度 = DataTableUtils.toDouble(預計生產量_至今 * 100 / 預定生產_data_y_max).ToString("0") + "%";
            實際進度 = DataTableUtils.toDouble(實際生產_data_y_max * 100 / 預定生產_data_y_max).ToString("0") + "%";
            差值 = (實際生產_data_y_max - 預計生產量_至今).ToString();
        }
        //顯示錯誤台數
        private void Set_Error()
        {
            //列出目前所有排程
            string Error_Number = "";
            bool ok = true;
            if (HtmlUtil.Check_DataTable(dt_本月應生產))
            {
                foreach (DataRow row in dt_本月應生產.Rows)
                {
                    Error_Number += ok ? $"  排程編號='{row["排程編號"]}' " : $" OR 排程編號='{row["排程編號"]}' ";
                    ok = false;
                }
                Error_Number = Error_Number != "" ? $" where ( {Error_Number} ) " : "";
            }

            DataTable dt_nosolve = new DataTable();
            if (dropdownlist_Factory.SelectedItem.Value == "sowon")
            {
                //取得目前未結案之數量
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
                string sqlcmd = $" SELECT " +
                                $" DISTINCT " +
                                $" 異常維護編號," +
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
            }
            else
            {
                Error_Number = Error_Number != "" ? $" {Error_Number} and (工作站異常維護資料表.工作站編號='1' OR 工作站異常維護資料表.工作站編號='2'  OR 工作站異常維護資料表.工作站編號='9' ) " : "";
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);
                string sqlcmd = $" SELECT DISTINCT 異常維護編號,排程編號,a.結案判定類型  FROM 工作站異常維護資料表 LEFT JOIN (SELECT max(異常維護編號) 編號,排程編號 排程號碼,父編號,結案判定類型 FROM 工作站異常維護資料表 where    結案判定類型 IS NOT NULL group by 父編號,排程編號,結案判定類型) a ON a.排程號碼 = 工作站異常維護資料表.排程編號 AND 工作站異常維護資料表.異常維護編號 = a.父編號 {Error_Number} AND (工作站異常維護資料表.父編號 IS NULL OR 工作站異常維護資料表.父編號=0) AND 工作站編號=1 AND a.結案判定類型 IS NULL ORDER BY 工作站異常維護資料表.排程編號";
                dt_nosolve = DataTableUtils.GetDataTable(sqlcmd);
            }
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
        private void Set_Table()
        {
            if (dropdownlist_Factory.SelectedItem.Value == "iTec")
            {
                dt_本月應生產.Columns["產線代號"].ReadOnly = false;
                dt_本月應生產.Columns["產線代號"].MaxLength = 15;
                foreach (DataRow row in dt_本月應生產.Rows)
                {
                    if (row["產線代號"].ToString() == "1")
                        row["產線代號"] = "100";
                    else if (row["產線代號"].ToString() == "2")
                        row["產線代號"] = "110";
                }
            }


            dt_本月應生產 = myclass.Add_LINE_GROUP(dt_本月應生產).ToTable();
            dt_未生產完成 = dt_本月應生產.Clone();
            string sqlcmd = dropdownlist_Factory.SelectedItem.Value == "iTec" ? $" (實際完成時間 IS NULL  OR 實際完成時間 ='') and (狀態 <> '完成' OR 狀態 IS NULL ) {condition.Replace("產線代號", "工作站編號")} " : $" (實際完成時間 IS NULL  OR 實際完成時間 ='') and (狀態 <> '完成' OR 狀態 IS NULL ) {condition} ";
            DataRow[] rows = dt_本月應生產.Select(sqlcmd);

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
    }
}
