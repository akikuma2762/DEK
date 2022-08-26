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
    public partial class waitingfortheproduction_details_New : System.Web.UI.Page
    {
        public string color = "";
        public string th = "";
        public string tr = "";
        public string cust_sname = "";
        public string date_str = "";
        public string date_end = "";
        string acc = "";
        public string path = "";
        myclass myclass = new myclass();

        DataTable dt_本月應生產 = new DataTable();
        DataTable dt_Finish = new DataTable();
        DataTable dt_NoFinish = new DataTable();
        DataTable dt_now = new DataTable();
        ShareFunction sfun = new ShareFunction();
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

        private void MainProcess()
        {

        }

        private void GetCondi()
        {

        }

        private void Get_MonthTotal()
        {

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
                    Line += i == 0 ? $" 產線代號='{Linelist[i]}' " : $" OR 產線代號='{Linelist[i]}' ";
                Line = Line != "" ? $" and ( {Line} ) " : "";
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
            //找出本月需做之數量
            //--------------------------------------------------------德科部分-----------------------------------------------
            //從dek ERP撈取本月排程資料
            //從dek ERP撈取本月排程資料
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
                           $"DATE_FORMAT(mkordsub.str_date, '%Y%m%d') <= {date_end} AND DATE_FORMAT(mkordsub.str_date, '%Y%m%d') >= {HtmlUtil.StrToDate(date_str).AddMonths(-1):yyyyMMdd} ";


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
                            row["預計完工日"] = sfun.Test_calculation_finish(row["預計開工日"].ToString(), rows[0]["組裝時間"].ToString() == "" ? "1" : rows[0]["組裝時間"].ToString(), true);
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
                                    row["預計完工日"] = sfun.Test_calculation_finish(row["預計開工日"].ToString(), rows[0]["組裝時間"].ToString() == "" ? "1" : rows[0]["組裝時間"].ToString(), true);

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

            //---------------------------------------------------------------------------------合併------------------------------------------------------------
            //產生 本月應做 之前未做完但在本月做完 到目前為止皆未完成之資料表
            if (HtmlUtil.Check_DataTable(dt_month))
                dt_now = sfun.Return_NowMonthTotal(dt_month, date_str, date_end, out dt_Finish, out dt_NoFinish);

            if (dt_now != null)
            {
                dt_本月應生產 = dt_now.Clone();
                //20220803 PrimaryKey 會錯誤,無發現用途,暫時拔除
                dt_now.PrimaryKey = new DataColumn[] { dt_now.Columns["排程編號"], dt_now.Columns["工作站編號"] };
                dt_本月應生產.PrimaryKey = new DataColumn[] { dt_本月應生產.Columns["排程編號"], dt_本月應生產.Columns["工作站編號"] };
                dt_本月應生產.Merge(dt_now);
            }
            if (HtmlUtil.Check_DataTable(dt_Finish))
            {
                dt_Finish.PrimaryKey = new DataColumn[] { dt_Finish.Columns["排程編號"], dt_Finish.Columns["工作站編號"] };
                dt_本月應生產.Merge(dt_Finish, true, MissingSchemaAction.Ignore);
            }
            if (HtmlUtil.Check_DataTable(dt_NoFinish))
            {
                dt_NoFinish.PrimaryKey = new DataColumn[] { dt_NoFinish.Columns["排程編號"], dt_NoFinish.Columns["工作站編號"] };
                dt_本月應生產.Merge(dt_NoFinish, true, MissingSchemaAction.Ignore);
            }

            string sql = $"客戶簡稱='{cust_sname}' {Line} and (狀態 <> '完成' OR 狀態 IS null)";
            rows = dt_本月應生產.Select(sql);

            DataTable dt_print = dt_本月應生產.Clone();
            for (int i = 0; i < rows.Length; i++)
                dt_print.ImportRow(rows[i]);

            if (HtmlUtil.Check_DataTable(dt_print))
            {
                //加入產線
                dt_print = myclass.Add_LINE_GROUP(dt_print).ToTable();

                //移除無須顯示項
                dt_print.Columns.Remove("產線代號");
                dt_print.Columns.Remove("實際完成時間");
                dt_print.Columns.Remove("預計完工日");
                dt_print.Columns["產線群組"].SetOrdinal(1);
                dt_print.Columns["排程編號"].ColumnName = "製造批號";
                string title = "";
                th = HtmlUtil.Set_Table_Title(dt_print, out title);
                tr = HtmlUtil.Set_Table_Content(dt_print, title, waitingfortheproduction_details_New_callback);
            }
        }
        private string waitingfortheproduction_details_New_callback(DataRow row, string field_name)
        {
            string value = "";
            value = field_name == "預計開工日" || field_name == "組裝日" ? HtmlUtil.changetimeformat(DataTableUtils.toString(row[field_name])) : "";
            return value == "" ? "" : $"<td>{value}</td>";

        }
    }
}