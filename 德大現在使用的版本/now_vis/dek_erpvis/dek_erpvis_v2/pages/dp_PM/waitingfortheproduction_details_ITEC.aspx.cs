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
    public partial class waitingfortheproduction_details_ITEC : System.Web.UI.Page
    {
        public string color = "";
        public string th = "";
        public string tr = "";
        public string cust_sname = "";
        public string date_str = "";
        public string date_end = "";
        string acc = "";
        public string path = "";
        myclass myclass = new cls.myclass();
        ShareFunction sFun = new ShareFunction();
        DataTable dt_本月應生產 = new DataTable();
        DataTable dt_Finish = new DataTable();
        
        DataTable dt_now = new DataTable();
        DataTable dt_NoFinish = new DataTable();
        string Line = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                path = 德大機械.get_title_web_path("PMD");
                color = HtmlUtil.change_color(acc);
                if (acc != "")
                {
                    if (!IsPostBack)
                        load_page_data();
                }
                else
                    Response.Redirect("waitingfortheproduction.aspx", false);
            }
            else
                Response.Redirect("waitingfortheproduction.aspx", false);
        }

        private void load_page_data()
        {
            Response.BufferOutput = false;
            if (Request.QueryString["key"] != null)
            {
                //確認參數是否存在
                Dictionary<string, string> keyValues = HtmlUtil.Return_dictionary(Request.QueryString["key"]);
                cust_sname = HtmlUtil.Search_Dictionary(keyValues, "cust_name");
                date_str = HtmlUtil.Search_Dictionary(keyValues, "date_str");
                date_end = HtmlUtil.Search_Dictionary(keyValues, "date_end");
                List<string> Linelist = new List<string>(HtmlUtil.Search_Dictionary(keyValues, "line").Split('#'));
                for (int i = 0; i < Linelist.Count - 1; i++)
                    Line += i == 0 ? $" 產線代號={Linelist[i]} " : $" OR 產線代號={Linelist[i]} ";
                Line = Line != "" ? $" and ( {Line} ) ":"";
                //儲存cookie
                Response.Cookies.Add(HtmlUtil.Save_Cookies("waitingfortheproduction", cust_sname));

                Set_Table();
            }
            else
                Response.Redirect("waitingfortheproduction.aspx", false);
        }
        //印出表格
        private void Set_Table()
        {
            DataRow[] rows = null;
            dt_Finish = new DataTable();
            dt_NoFinish = new DataTable();
            DataTable dt_month = new DataTable();
            
            //-20220728新增-------------------------------------------------------德科大圓盤部分-----------------------------------------------
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekERPDataTable);
            string sqlcmd = "SELECT   " +
                                "cust.custnm2 AS 客戶簡稱, " +
                                "ws.PLINE_NO AS 產線代號, " +
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
                               $"DATE_FORMAT(mkordsub.str_date, '%Y%m%d') <= {date_end} AND DATE_FORMAT(mkordsub.str_date, '%Y%m%d') >= {HtmlUtil.StrToDate(date_str).AddMonths(-1):yyyyMMdd} AND " +
                               "ws.PLINE_NO <>'' AND ws.PLINE_NO IS NOT NULL";


            DataTable dek_dt = DataTableUtils.GetDataTable(sqlcmd);
            //從組裝資料表撈取相對應資料
            if (HtmlUtil.Check_DataTable(dek_dt))
            {
                bool ok = true;
                string condition = "";
                foreach (DataRow row in dek_dt.Rows)
                {
                    condition += ok ? $" 排程編號='{row["排程編號"]}' " : $" OR 排程編號='{row["排程編號"]}' ";
                    ok = false;
                }
                condition = condition != "" ? $" and ( {condition} ) " : "";

                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
                sqlcmd = $" select " +
                         $"排程編號," +
                         $"進度," +
                         $"組裝日," +
                         $"狀態," +
                         $"組裝時間," +
                         $"SUBSTRING(實際完成時間,1,8) 實際完成時間  " +
                         $"from 工作站狀態資料表 " +
                         $"left join 組裝工藝 on 組裝工藝.機種編號 = (select top(1) value from STRING_SPLIT(工作站狀態資料表.排程編號, '-')) " +
                         $"where 工作站編號 = 11 {condition} ";
                DataTable dek_dtinformation = DataTableUtils.GetDataTable(sqlcmd);
                DataTable dek_time = DataTableUtils.GetDataTable("select * from 組裝工藝");
                rows = null;
                if (HtmlUtil.Check_DataTable(dek_dtinformation))
                {
                    //新增欄位
                    dek_dt.Columns.Add("進度");
                    dek_dt.Columns.Add("狀態");
                    dek_dt.Columns.Add("組裝日");
                    dek_dt.Columns.Add("預計完工日");
                    dek_dt.Columns.Add("實際完成時間");

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
                            row["預計完工日"] = sFun.Test_calculation_finish(row["預計開工日"].ToString(), rows[0]["組裝時間"].ToString() == "" ? "1" : rows[0]["組裝時間"].ToString(), true);
                        }
                        //沒有資料的情況
                        else
                        {
                            sqlcmd = $"機種編號='{DataTableUtils.toString(row["排程編號"]).Split('-')[0]}'";
                            rows = dek_time.Select(sqlcmd);
                            //20220727 增加判斷 機種是否存在於組裝工藝中 ex:WDB不存在
                            if (rows.Length == 0)
                            {
                                row["預計完工日"] = "";
                            }
                            else
                            {
                                row["預計完工日"] = sFun.Test_calculation_finish(row["預計開工日"].ToString(), rows[0]["組裝時間"].ToString() == "" ? "1" : rows[0]["組裝時間"].ToString(), true);

                            }
                        }

                    }
                }

                //選取屬於本月應生產之項目
                dt_month = dek_dt.Clone();
                sqlcmd = $"(實際完成時間>={date_str} and 預計完工日>={date_str}) or 實際完成時間 IS NULL";
                rows = dek_dt.Select(sqlcmd);
                if (rows != null && rows.Length > 0)
                {
                    for (int i = 0; i < rows.Length; i++)
                        dt_month.ImportRow(rows[i]);
                }
            }
            else {
                dek_dt.Columns.Add("進度");
                dek_dt.Columns.Add("狀態");
                dek_dt.Columns.Add("組裝日");
                dek_dt.Columns.Add("預計完工日");
                dek_dt.Columns.Add("實際完成時間");
                dt_month = dek_dt.Clone();
            }

//-----------------------------------------------------------------------------------------------------------------------------

//----------------------------------------臥式---------------------------------------------------------------------------------
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);
             sqlcmd = $"SELECT a.*,a.製造批號 as 排程編號,工作站狀態資料表.進度, 工作站狀態資料表.狀態, 工作站狀態資料表.組裝日, a.預計開工日+'080000' 上線日, SUBSTRING(工作站狀態資料表.實際完成時間,1, 8) 實際完成時間,  (CASE a.產線代號 WHEN 1 THEN 組裝工時*60*60 WHEN 2 THEN 組裝工時*60*60 WHEN 4 THEN ((CAST(刀臂點數 AS float)+CAST(刀套點數 AS float))*60*60) WHEN 5 THEN (CAST(刀鍊點數 AS float)*60*60) WHEN 6 THEN (CAST(全油壓點數 AS float)*60*60) END) 標準工時 FROM (SELECT (CASE FAB_USER WHEN '100' THEN '1' WHEN '110' THEN '2' END) 產線代號,CUSTNM2 客戶簡稱,sw_MKORDSUB.LOT_NO 製造批號, A22_FAB.CORD_NO 訂單號碼,sw_MKORDSUB.item_no AS 品號, sw_cordsub.trn_date AS 訂單日期, sw_CORD.CORD_NO AS 客戶訂單,A22_FAB.STR_DATE 預計開工日, sw_MKORDSUB.SCLOSE 製令狀態 FROM SW.FJWSQL.dbo.A22_FAB LEFT JOIN SW.FJWSQL.dbo.CORD AS sw_CORD ON sw_CORD.trn_no = A22_FAB.cord_no LEFT JOIN SW.FJWSQL.dbo.CORDSUB AS sw_CORDSUB ON sw_CORDSUB.TRN_NO = A22_FAB.CORD_no AND sw_CORDSUB.SN = A22_FAB.CORD_SN LEFT JOIN SW.FJWSQL.dbo.CUST AS sw_CUST ON sw_CUST.CUST_NO = sw_CORD.CUST_NO LEFT JOIN SW.FJWSQL.dbo.MKORDSUB AS sw_MKORDSUB ON sw_MKORDSUB.CORD_NO = sw_CORDSUB.trn_no AND sw_MKORDSUB.CORD_SN = sw_CORDSUB.sn LEFT JOIN SW.FJWSQL.dbo.citem AS sw_citem ON sw_CORDSUB.item_no = sw_citem.item_no LEFT JOIN SW.FJWSQL.dbo.item_22 AS sw_item_22 ON sw_CORDSUB.item_no = sw_item_22.item_no LEFT JOIN SW.FJWSQL.dbo.ITEM AS sw_item ON sw_item.ITEM_NO = sw_item_22.item_no WHERE sw_MKORDSUB.FCLOSE <> 1 AND A22_FAB.STR_DATE > 20210101 AND A22_FAB.STR_DATE<={date_end} AND 100<=FAB_USER AND FAB_USER<=199 ) a LEFT JOIN 工作站狀態資料表 ON 工作站狀態資料表.排程編號 = a.製造批號 AND 工作站狀態資料表.工作站編號 = a.產線代號 LEFT JOIN 臥式工藝 ON 臥式工藝.機種編號 = SUBSTRING(a.製造批號, 1, CHARINDEX('-', a.製造批號) - 1) WHERE ((組裝日>={HtmlUtil.StrToDate(date_str).AddMonths(-1):yyyyMMdd} AND 組裝日 <={date_end}) OR (實際完成時間 IS NULL OR LEN(實際完成時間) =0)) AND LEN(客戶簡稱)>0  AND (SUBSTRING(實際完成時間,1, 8) >=20220126 OR 實際完成時間 IS NULL OR 實際完成時間 = '') AND ((a.製令狀態 = '結案' AND 狀態 IS NOT NULL) OR (a.製令狀態 = '未結')) AND 客戶簡稱 = '{cust_sname}' {Line}  ORDER BY 客戶簡稱";
            DataTable dt_sowon = DataTableUtils.GetDataTable(sqlcmd);
            dt_sowon = sFun.Format_NowMonthTotal(dt_sowon);
            if (HtmlUtil.Check_DataTable(dt_sowon))
            {
                foreach (DataRow row in dt_sowon.Rows)
                    dt_month.ImportRow(row);
            }


            if (HtmlUtil.Check_DataTable(dt_month))
            {
                if (HtmlUtil.Check_DataTable(dt_month))
                    dt_now = sFun.Return_NowMonthTotal(dt_month, date_str, date_end, out _, out dt_NoFinish);
                //合併
                if (dt_now != null)
                {
                    dt_本月應生產 = dt_now.Clone();
                    //20220803 PrimaryKey 會錯誤,無發現用途,暫時拔除
                    //dt_now.PrimaryKey = new DataColumn[] { dt_now.Columns["排程編號"], dt_now.Columns["工作站編號"] };
                    //dt_本月應生產.PrimaryKey = new DataColumn[] { dt_本月應生產.Columns["排程編號"], dt_本月應生產.Columns["工作站編號"] };
                    dt_本月應生產.Merge(dt_now);
                }
                if (HtmlUtil.Check_DataTable(dt_NoFinish))
                {
                    dt_NoFinish.PrimaryKey = new DataColumn[] { dt_NoFinish.Columns["排程編號"], dt_NoFinish.Columns["工作站編號"] };
                    dt_本月應生產.Merge(dt_NoFinish, true, MissingSchemaAction.Ignore);
                }

                //20220803 修改反邏輯查詢語法
                string sql = $"客戶簡稱='{cust_sname}' {Line} and (狀態 <> '完成' OR 狀態 IS null)";
                rows = dt_本月應生產.Select(sql);
                DataTable dt_print = dt_本月應生產.Clone();
                for (int i = 0; i < rows.Length; i++)
                {
                    dt_print.ImportRow(rows[i]);
                }
                dt_本月應生產 = dt_print;
                if (HtmlUtil.Check_DataTable(dt_本月應生產))
                {
                    //加入產線
                    dt_本月應生產 = myclass.Add_LINE_GROUP(dt_本月應生產,"hor").ToTable();

                    //移除無須顯示項
                    dt_本月應生產.Columns.Remove("產線代號");
                    dt_本月應生產.Columns.Remove("實際完成時間");
                    dt_本月應生產.Columns.Remove("預計完工日");

                    //20220728合併後已消失欄位
                    //dt_本月應生產.Columns.Remove("上線日");
                    //dt_本月應生產.Columns.Remove("標準工時");

                    dt_本月應生產.Columns["產線群組"].SetOrdinal(1);
                    string title = "";
                    th = HtmlUtil.Set_Table_Title(dt_本月應生產, out title);
                    tr = HtmlUtil.Set_Table_Content(dt_本月應生產, title, waitingfortheproduction_details_ITEC_callback);

                }
                else
                    HtmlUtil.NoData(out th, out tr);
            }
            else
                HtmlUtil.NoData(out th, out tr);
        }
        private string waitingfortheproduction_details_ITEC_callback(DataRow row, string field_name)
        {
            string value = "";
            value = field_name == "預計開工日" || field_name == "組裝日" ? HtmlUtil.changetimeformat(DataTableUtils.toString(row[field_name])) : "";
            return  value == "" ? "" : $"<td>{value}</td>";
        }
    }
}