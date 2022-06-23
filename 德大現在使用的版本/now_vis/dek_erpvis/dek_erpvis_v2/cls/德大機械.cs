using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Support;

namespace dek_erpvis_v2.cls
{
    public class 德大機械
    {
        public static string function_yn(string user_acc, string function_name)
        {
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisErp);
            //--------------------------------------------------------------------------
            string sqlcmd = 控制台_權限管理.特殊功能YN(user_acc, function_name);
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
            return HtmlUtil.Check_DataTable(dt)? DataTableUtils.toString(dt.Rows[0]["許可"]):"N";
        }

        public static string 德大專用月份(string acc)
        {
            //判斷順序 : 參數設定(優先) -> 預設值(未設定則取預設值)
            clsDB_Server clsDB_vis = new clsDB_Server(myclass.GetConnByDekVisErp);
            clsDB_vis.dbOpen(myclass.GetConnByDekVisErp);
            string SQLQuery = 德大機械.控制台_權限管理.是否新增過參數(acc);
            string startDate = "", endDate = "";
            DataTable dt = clsDB_vis.GetDataTable(SQLQuery);

            string start = DateTime.Now.AddMonths(-1).AddDays(-DateTime.Now.AddMonths(1).Day).ToString("dd");
            string last = DateTime.Now.AddMonths(1).AddDays(-DateTime.Now.AddMonths(1).Day).ToString("dd");


            if (HtmlUtil.Check_DataTable(dt))
            {
                startDate = "yyyyMM" + DataTableUtils.toString(dt.Rows[0]["DATE_STR"]).PadLeft(2, '0');
                endDate = "yyyyMM" + DataTableUtils.toString(dt.Rows[0]["DATE_END"]).PadLeft(2, '0');
            }
            else
            {
                startDate = "yyyyMM01";
                endDate = "yyyyMM31";
            }

            if (!檢查日期格式(startDate, -1))
                startDate = "yyyyMM" + start;
            if (!檢查日期格式(endDate, 0))
                endDate = "yyyyMM" + last;



            string date_str = "";
            string date_end = "";
            DateTime dt2 = DateTime.ParseExact(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString(endDate), "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
            if (DateTime.Compare(DateTime.Today, dt2) > 0) // 當日>結算日
            { // 有參數設定，當月起算，次月結算
                date_str = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString(startDate);
                date_end = new DateTime(DateTime.Now.AddMonths(1).Year, DateTime.Now.AddMonths(1).Month, 1).ToString(endDate);
            }
            else // 當日<結算日
            {
                if (HtmlUtil.Check_DataTable(dt)) //有參數設定，前月起算，當月結算
                {
                    string start_month = DateTime.Now.Month.ToString();
                    if (start_month == "1")
                        date_str = new DateTime(DateTime.Now.Year - 1, DateTime.Now.Month + 11, 1).ToString(startDate);
                    else
                    {
                        if (startDate == "yyyyMM01")
                            date_str = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString(startDate);
                        else
                            date_str = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, 1).ToString(startDate);
                    }

                    date_end = new DateTime(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).Year, DateTime.Now.Month, 1).ToString(endDate);
                }
                else //無參數設定，當月初起算，當月末結算
                {
                    date_str = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString(startDate);
                    date_end = new DateTime(DateTime.Now.AddMonths(1).Year, DateTime.Now.AddMonths(1).Month, 1).AddDays(-1).ToString(endDate);
                }

            }
            return date_str + "," + date_end;
        }
        //檢查日期格式是否正確
        public static bool 檢查日期格式(string date, int x)
        {
            bool ok = true;
            date = HtmlUtil.changetimeformat(date);
            date = DateTime.Now.AddMonths(x).ToString(date + " HH:mm:ss");
            DateTime time = DateTime.Now;
            if (DataTableUtils.toString(date) != "")
            {
                if (!DateTime.TryParse(date, out _))
                    ok = false;
            }
            return ok;
        }
        public static string get_title_web_path(string dp)
        {
            clsDB_Server clsDB_Server = new clsDB_Server(myclass.GetConnByDekVisErp);
            string html = "";
            if (clsDB_Server.IsConnected == true)
            {
                string sqlcmd = $"{德大機械.控制台_權限管理.取得部門("SYS")} and DPM_NAME = '{dp}'";
                DataTable dt = clsDB_Server.GetDataTable(sqlcmd);
                html = $"<ol class='breadcrumb_'>" +
                $"<li><u><a href='../index.aspx'>首頁 </a></u></li> " +
                $"<li><u><a href='../SYS_CONTROL/dp_fuclist.aspx?dp={DataTableUtils.toString(dt.Rows[0]["DPM_NAME"])}'>{ DataTableUtils.toString(dt.Rows[0]["DPM_NAME2"])}</a></u></li> " +
                $"</ol>";
            }
            return html;
        }
        public class 業務部_訂單變更紀錄
        {
            public static string 首旺加EIP客戶變更(string date_str, string date_end, string TOP = "")
            {
                if (TOP != "" && TOP != "0")
                    TOP = $"TOP({TOP})";

                return $"  select {TOP} alldata.客戶名稱,count(distinct alldata.訂單號碼) as 訂單次數,count(*) as 變更次數 from (  select ISNULL(a.客戶名稱,b.客戶名稱) as 客戶名稱,ISNULL(a.訂單號碼,b.訂單號碼) as 訂單號碼,ISNULL(a.SN,b.SN) as SN,ISNULL(a.變更欄位,b.變更欄位) as 變更欄位,ISNULL(a.變更日期, replace(b.變更日期,'/','')  ) as 變更日期,a.備註,a.變更前內容,a.變更後內容 from  (SELECT cust.CUSTNM2 AS 客戶名稱,CORD_CH_VERIFY.CORD_NO AS 訂單號碼,CORD_CH_VERIFY.CORD_SN AS SN,  CH_OBJECT AS 變更欄位,APPROVE_DATE AS 變更日期,CORD_CH_VERIFY.REMARK AS 備註,BEFORE_CH_CONTENT AS 變更前內容,SNO,  (CASE  WHEN LEN(AFTER_CH_D_DATE) > 0 THEN AFTER_CH_D_DATE  ELSE (CASE  WHEN LEN(AFTER_CH_ITEM_NO) > 0 THEN AFTER_CH_ITEM_NO  ELSE (CASE  WHEN AFTER_CH_QTY > 0 THEN CONVERT(varchar, AFTER_CH_QTY)  ELSE (CASE  WHEN LEN(AFTER_CH_SCLOSE) > 0 THEN AFTER_CH_SCLOSE  ELSE ''  END)  END)  END)  END) AS 變更後內容  FROM FJWSQL.dbo.CORD_CH_VERIFY  LEFT JOIN FJWSQL.dbo.CORDSUB ON CORDSUB.TRN_NO = CORD_CH_VERIFY.CORD_NO  AND CORDSUB.SN = CORD_CH_VERIFY.CORD_SN  LEFT JOIN FJWSQL.dbo.cust ON CUST.CUST_NO = CORDSUB.CUST_NO  WHERE APPROVE_DATE>={date_str}  AND APPROVE_DATE<={date_end}  AND CH_OBJECT IS NOT NULL  AND LEN(CH_OBJECT)>0 )  as a  full join (SELECT CUSTNM2 AS 客戶名稱,TRN_NO AS 訂單號碼 ,SN,變更欄位 ,convert(varchar, cc.UP_time, 111) AS 變更日期  FROM CordSub_chg_history AS cc  LEFT JOIN FJWSQL.dbo.CUST AS fjw ON cc.cust_no = fjw.CUST_NO  WHERE cc.UP_time BETWEEN Cast('{date_str}' AS DATETIME) AND Cast('{date_end}' AS DATETIME)) as b on a.客戶名稱 = b.客戶名稱 and a.訂單號碼 = b.訂單號碼 and a.SN  = b.SN where a.變更欄位 <> '單號.變') as alldata  group by alldata.客戶名稱 order by 變更次數 desc";
            }
            public static string 變更類型()
            {
                return "select distinct 變更欄位 from CordSub_chg_history";
            }

            public static string 客戶列表(string 日期起, string 日期訖, string TOP)
            {
                if (TOP != "" && TOP != "0")
                    TOP = $"TOP({TOP})";

                //此處日期訖須+1D
                DateTime NewDate = DateTime.ParseExact(日期訖, "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
                日期訖 = NewDate.ToString("yyyyMMdd");
                //string sqlcmd = "SELECT " + TOP + " fjw.CUSTNM2 as 客戶名稱, count(eip.TRN_NO) as 訂單筆數, sum(eip.cnt) as 變更次數 FROM Eip.dbo.Cord_cnt as eip left join FJWSQL.dbo.CUST as fjw on eip.cust_no = fjw.CUST_NO  where eip.UP_time BETWEEN CAST('" + 日期起 + "' AS DATETIME) AND CAST('" + 日期訖 + "' AS DATETIME) and eip.cnt > 0 group by fjw.CUSTNM2 having sum(eip.cnt) > 0 ORDER BY sum(eip.cnt) DESC";
                //20190516發布，訂單特定欄位異動，變更次數才需累計:Eip.dbo.Cord_chg_cnt

                return $"select  a.客戶名稱 as 客戶名稱,count(a.訂單號碼) as 訂單次數,sum(a.變更次數) as 變更次數 from ( select  fjw.CUSTNM2 as 客戶名稱,TRN_NO as 訂單號碼,count(DISTINCT  jsn) as 變更次數 from  CordSub_chg_history as cc left join FJWSQL.dbo.CUST as fjw on cc.cust_no = fjw.CUST_NO  WHERE  cc.UP_time BETWEEN Cast('{日期起}' AS DATETIME) AND Cast('{日期訖}' AS DATETIME)  group by  fjw.CUSTNM2 ,TRN_NO ) as a group by 客戶名稱 order by 變更次數 desc";
            }
            public static string 客戶訂單變更歷程(string 客戶名稱, string 日期起, string 日期訖)
            {
                //此處日期訖須+1D
                DateTime NewDate = DateTime.ParseExact(日期訖, "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
                日期訖 = NewDate.AddDays(-1).ToString("yyyyMMdd");
                //string sqlcmd = "SELECT fjw.CUSTNM2 as 客戶名稱,eip.TRN_NO as 訂單號碼,eip.Cord_No as 訂單備註,eip.cnt as 變更次數,eip.remm as 紀錄歷程 FROM Eip.dbo.Cord_cnt as eip left join FJWSQL.dbo.CUST as fjw on eip.cust_no = fjw.CUST_NO where eip.UP_time BETWEEN CAST('" + 日期起 + "' AS DATETIME) AND CAST('" + 日期訖 + "' AS DATETIME) and fjw.CUSTNM2 = '" + 客戶名稱 + "' and eip.cnt >0 ";
                //20190516發布，訂單特定欄位異動，變更次數才需累計:Eip.dbo.Cord_chg_cnt
                //string sqlcmd = "select * from (select cust.CUSTNM2 as 客戶名稱,cbSub.TRN_NO as 訂單號碼,cbSub.SN,sum(Cord_No_cnt) as 單號變更次數,sum(ITEM_NO_cnt) as 品號變更次數,sum(QUANTITY_cnt) as 數量變更次數,sum(D_DATE_cnt) as 日期變更次數,sum(REMARK_cnt) as 備註變更次數,sum(Cord_No_cnt+ITEM_NO_cnt+QUANTITY_cnt+D_DATE_cnt+REMARK_cnt) as 總計變更次數 from CordSub_chg_cnt as cbSub left join Cord_chg_cnt as cb on cbSub.TRN_NO = cb.TRN_NO left join FJWSQL.dbo.CUST as cust on cbSub.CUST_NO = cust.CUST_NO left join Eip.dbo.CordSub_chg_history as eiphis on cbSub.TRN_NO = eiphis.TRN_NO and eiphis.SN = cbSub.SN where eiphis.UP_time BETWEEN CAST('" + 日期起 + "' AS DATETIME) AND CAST('" + 日期訖 + "' AS DATETIME)  and cust.CUSTNM2 = '" + 客戶名稱 + "' group by cbSub.TRN_NO,cbSub.SN,cust.CUSTNM2) as A where A.總計變更次數 > 0";
                //20190529更正，變更日期依據，不採訂單日期，改採變更日期(UP_time2)
                //string sqlcmd = "SELECT * FROM(SELECT cust.CUSTNM2 AS 客戶名稱,cbSub.TRN_NO AS 訂單號碼,cbSub.SN,   sum(Cord_No_cnt) AS 客戶單號變更次數,sum(ITEM_NO_cnt) AS 品號變更次數,   sum(QUANTITY_cnt) AS 數量變更次數,sum(D_DATE_cnt) AS 交期變更次數,   sum(Cord_No_cnt+ITEM_NO_cnt+QUANTITY_cnt+D_DATE_cnt) AS 小計,   德大變更_cnt AS 德大變更,客戶變更_cnt AS 客戶變更 FROM CordSub_chg_cnt AS cbSub   LEFT JOIN Cord_chg_cnt AS cb ON cbSub.TRN_NO = cb.TRN_NO   LEFT JOIN FJWSQL.dbo.CUST AS cust ON cbSub.CUST_NO = cust.CUST_NO   WHERE cbSub.UP_time2 BETWEEN CAST('" + 日期起 + "' AS DATETIME) AND CAST('" + 日期訖 + "' AS DATETIME)     AND cust.CUSTNM2 = '" + 客戶名稱 + "'  GROUP BY cbSub.TRN_NO,cbSub.SN,cust.CUSTNM2,德大變更_cnt,客戶變更_cnt) AS A   WHERE 小計 >0";
                //新增更新日期

                return $"SELECT * FROM(SELECT cust.CUSTNM2 AS 客戶名稱,cbSub.TRN_NO AS 訂單號碼,cbSub.SN,   sum(Cord_No_cnt) AS 客戶單號變更次數,cbSub.up_time2    as 變更日期,sum(ITEM_NO_cnt) AS 品號變更次數,   sum(QUANTITY_cnt) AS 數量變更次數,sum(D_DATE_cnt) AS 交期變更次數,   sum(Cord_No_cnt+ITEM_NO_cnt+QUANTITY_cnt+D_DATE_cnt) AS 小計,   德大變更_cnt AS 德大變更,客戶變更_cnt AS 客戶變更 FROM CordSub_chg_cnt AS cbSub   LEFT JOIN Cord_chg_cnt AS cb ON cbSub.TRN_NO = cb.TRN_NO   LEFT JOIN FJWSQL.dbo.CUST AS cust ON cbSub.CUST_NO = cust.CUST_NO   WHERE cbSub.UP_time2 BETWEEN CAST('{日期起}' AS DATETIME) AND CAST('{日期訖}' AS DATETIME)     AND cust.CUSTNM2 = '{客戶名稱}'  GROUP BY cbSub.TRN_NO,cbSub.up_time2,cbSub.SN,cust.CUSTNM2,德大變更_cnt,客戶變更_cnt) AS A   WHERE 小計 >0";
            }
            public static string 客戶訂單變更(string 客戶名稱, string 日期起, string 日期訖)
            {
                return $"SELECT CUSTNM2 as 客戶名稱,TRN_NO as 訂單號碼 ,SN,變更欄位 ,convert(varchar, cc.UP_time, 111)  as 變更日期   FROM CordSub_chg_history AS cc   LEFT JOIN FJWSQL.dbo.CUST AS fjw ON cc.cust_no = fjw.CUST_NO   WHERE cc.UP_time BETWEEN Cast('{日期起}' AS DATETIME) AND Cast('{日期訖}' AS DATETIME)      AND fjw.CUSTNM2 = '{客戶名稱}'";
            }
            public static string 變更客戶(string 客戶名稱, string 日期起, string 日期訖)
            {
                return $"SELECT distinct CUSTNM2 as 客戶名稱,TRN_NO as 訂單號碼 ,SN , convert(varchar, cc.UP_time, 111)   as 變更日期  FROM CordSub_chg_history AS cc   LEFT JOIN FJWSQL.dbo.CUST AS fjw ON cc.cust_no = fjw.CUST_NO   WHERE cc.UP_time BETWEEN Cast('{日期起}' AS DATETIME) AND Cast('{日期訖}' AS DATETIME)      AND fjw.CUSTNM2 = '{客戶名稱}'";
            }
            public static string 首旺變更明細(string cust_name, string date_str, string date_end)
            {
                return $" SELECT cust.CUSTNM2 AS 客戶名稱,CORD_CH_VERIFY.CORD_NO AS 訂單號碼,CORD_CH_VERIFY.CORD_SN AS SN, CH_OBJECT AS 變更欄位,APPROVE_DATE AS 變更日期,CORD_CH_VERIFY.REMARK AS 備註,BEFORE_CH_CONTENT AS 變更前內容,SNO,  (CASE WHEN LEN(AFTER_CH_D_DATE) > 0 THEN AFTER_CH_D_DATE ELSE ( CASE WHEN LEN(AFTER_CH_ITEM_NO) > 0 THEN AFTER_CH_ITEM_NO ELSE ( CASE WHEN AFTER_CH_QTY > 0 THEN CONVERT(varchar, AFTER_CH_QTY)  ELSE ( CASE WHEN LEN(AFTER_CH_SCLOSE) > 0 THEN AFTER_CH_SCLOSE ELSE ''  END  ) END ) END )  END )  AS 變更後內容 FROM CORD_CH_VERIFY LEFT JOIN CORDSUB ON CORDSUB.TRN_NO = CORD_CH_VERIFY.CORD_NO AND CORDSUB.SN = CORD_CH_VERIFY.CORD_SN LEFT JOIN cust ON CUST.CUST_NO = CORDSUB.CUST_NO WHERE APPROVE_DATE>={date_str} AND APPROVE_DATE<={date_end} and CUSTNM2  = '{cust_name}' AND CH_OBJECT IS NOT NULL AND LEN(CH_OBJECT)>0";
            }
            public static string 訂單變更明細(string date_str, string date_end, string cust_name)
            {
                return $"  SELECT a.客戶名稱,a.訂單號碼,a.SN, a.變更欄位,a.變更日期,a.備註,a.變更前內容,a.變更後內容,mkordsub.item_no AS CCS, mkordsub.lot_no AS 機號,CITEM_NO as 客戶機種 FROM (SELECT a.客戶代號 as 客戶代號,ISNULL(a.客戶名稱,b.客戶名稱) AS 客戶名稱,ISNULL(a.訂單號碼,b.訂單號碼) AS 訂單號碼,ISNULL(a.SN, b.SN) AS SN, ISNULL(a.變更欄位,b.變更欄位) AS 變更欄位,ISNULL(a.變更日期, replace(b.變更日期,'/', '')) AS 變更日期,a.備註,a.變更前內容,a.變更後內容 FROM (SELECT cust.CUST_NO as 客戶代號, cust.CUSTNM2 AS 客戶名稱,CORD_CH_VERIFY.CORD_NO AS 訂單號碼,CORD_CH_VERIFY.CORD_SN AS SN, CH_OBJECT AS 變更欄位,APPROVE_DATE AS 變更日期,CORD_CH_VERIFY.REMARK AS 備註,BEFORE_CH_CONTENT AS 變更前內容,SNO, (CASE WHEN LEN(AFTER_CH_D_DATE) > 0 THEN AFTER_CH_D_DATE ELSE (CASE WHEN LEN(AFTER_CH_ITEM_NO) > 0 THEN AFTER_CH_ITEM_NO ELSE (CASE WHEN AFTER_CH_QTY > 0 THEN CONVERT(varchar, AFTER_CH_QTY) ELSE (CASE WHEN LEN(AFTER_CH_SCLOSE) > 0 THEN AFTER_CH_SCLOSE ELSE '' END) END) END) END) AS 變更後內容 FROM FJWSQL.dbo.CORD_CH_VERIFY LEFT JOIN FJWSQL.dbo.CORDSUB ON CORDSUB.TRN_NO = CORD_CH_VERIFY.CORD_NO AND CORDSUB.SN = CORD_CH_VERIFY.CORD_SN LEFT JOIN FJWSQL.dbo.cust ON CUST.CUST_NO = CORDSUB.CUST_NO WHERE APPROVE_DATE>={date_str} AND APPROVE_DATE<={date_end} AND CUSTNM2 = '{cust_name}' AND CH_OBJECT IS NOT NULL AND LEN(CH_OBJECT)>0 ) AS a FULL JOIN (SELECT CUSTNM2 AS 客戶名稱,TRN_NO AS 訂單號碼 ,SN,變更欄位 ,convert(varchar, cc.UP_time, 111) AS 變更日期 FROM CordSub_chg_history AS cc LEFT JOIN FJWSQL.dbo.CUST AS fjw ON cc.cust_no = fjw.CUST_NO WHERE cc.UP_time BETWEEN Cast('{date_str}' AS DATETIME) AND Cast('{date_end}' AS DATETIME) AND fjw.CUSTNM2 = '{cust_name}') AS b ON a.客戶名稱 = b.客戶名稱 AND a.訂單號碼 = b.訂單號碼 AND a.SN = b.SN WHERE a.變更欄位 <> '單號.變' ) AS a LEFT JOIN FJWSQL.dbo.mkordsub ON a.訂單號碼 = mkordsub.cord_no AND a.SN = mkordsub.cord_sn left join FJWSQL.dbo.CITEM on CITEM.CUST_NO  = a.客戶代號  and CITEM.ITEM_NO = mkordsub.item_no ";
            }
        }
        public class 業務部_訂單進度查詢
        {
            public static string 訂單列表(string 條件式)
            {
                return $"select cordsub.TRN_NO as 訂單單號,cordsub.SN as 明細序,cordsub.ITEM_NO as CCS,cordsub.ITEMNM as 品名規格, CUSTNM2 as 客戶名稱,cordsub.SCLOSE as 訂單狀態,MKORDSUB.LOT_NO as 刀庫編號,INVOSUB.TRN_DATE as 出貨日期 from cordsub left join CUST on cordsub.CUST_NO = cust.CUST_NO left join INVOSUB on invosub.CORD_NO = cordsub.TRN_NO and INVOSUB.CORD_SN = cordsub.SN left join MKORDSUB on mkordsub.CORD_NO = cordsub.TRN_NO and MKORDSUB.CORD_SN = cordsub.SN where {條件式}"; ;
            }
            public static string 依刀庫編號取得生產進度(string 條件式)
            {
                return $"SELECT 排程編號 as 刀庫編號,進度,狀態 FROM 工作站狀態資料表 where 排程編號 = '{條件式}' ";
            }
        }

        public class 業務部_成品庫存分析
        {
            public static string 數量詳細表(string 條件式)
            {
                return $"SELECT a.產線代號,a.總數量,(CASE WHEN b.逾期數量 IS NULL THEN 0 ELSE b.逾期數量	END) AS 逾期數量,(CASE	WHEN 逾期數量 > 0 THEN (a.總數量 - 逾期數量)	ELSE a.總數量	END)AS 一般數量	FROM	(SELECT A22_FAB.FAB_USER AS 產線代號, count(*) AS 總數量	FROM A22_FAB	LEFT JOIN CORDSUB AS cordsub ON A22_FAB.CORD_NO=cordsub.TRN_NO	AND A22_FAB.CORD_SN=cordsub.SN	LEFT JOIN MKORDSUB AS MKORDSUB ON mkordSUB.CORD_NO=CORDSUB.trn_no	AND mkordsub.CORD_SN=cordsub.sn	LEFT JOIN CUST AS CUST ON CUST.CUST_NO=CORDsub.CUST_NO	left join item  on    CORDSUB.ITEM_NO=item.ITEM_NO	left join item_22  on  CORDSUB.ITEM_NO=item_22.ITEM_NO	WHERE cordsub.SCLOSE !='結案'	AND item.class>='Z'	AND item.class<= 'ZR'	AND	(SELECT MAX(ITEMIO.TRN_DATE)	FROM ITEMIOS	LEFT JOIN ITEMIO ON ITEMIOS.IO=ITEMIO.IO	AND ITEMIOS.TRN_NO=ITEMIO.TRN_NO	WHERE ITEMIOS.IO='2'	AND ITEMIOS.MKORD_NO=MKORDSUB.TRN_NO	AND ITEMIOS.MKORD_SN=MKORDSUB.SN	AND ITEMIO.S_DESC< >'歸還'	AND ITEMIO.MK_TYPE='成品入庫') !=''	GROUP BY A22_FAB.FAB_USER) AS a	LEFT JOIN	(SELECT A22_FAB.FAB_USER AS 產線代號,(count(*)) AS 逾期數量	FROM A22_FAB	LEFT JOIN CORDSUB AS cordsub ON A22_FAB.CORD_NO=cordsub.TRN_NO	AND A22_FAB.CORD_SN=cordsub.SN	LEFT JOIN MKORDSUB AS MKORDSUB ON mkordSUB.CORD_NO=CORDSUB.trn_no	AND mkordsub.CORD_SN=cordsub.sn	LEFT JOIN CUST AS CUST ON CUST.CUST_NO=CORDsub.CUST_NO	left join item  on    CORDSUB.ITEM_NO=item.ITEM_NO	left join item_22  on  CORDSUB.ITEM_NO=item_22.ITEM_NO	WHERE cordsub.SCLOSE !='結案'   AND item.class>='Z'	AND item.class<= 'ZR'   {條件式}	 GROUP BY A22_FAB.FAB_USER) AS b ON a.產線代號 = b.產線代號";
            }
            public static string 產線群組列表(string 產線代號)
            {
                return $"SELECT ASSEMBLY_LINE.LINE_ID,ASSEMBLY_LINE.LINE_NAME,ASSEMBLY_GROUP.GROUP_NAME FROM ASSEMBLY_LINE inner join ASSEMBLY_LINE_GROUP on ASSEMBLY_LINE.LINE_ID = ASSEMBLY_LINE_GROUP.LINE_ID inner join ASSEMBLY_GROUP on ASSEMBLY_LINE_GROUP.GROUP_ID = ASSEMBLY_GROUP.GROUP_ID where ASSEMBLY_LINE.LINE_STATUS = '1' and ASSEMBLY_LINE.LINE_ID = '{產線代號}'";
            }
            public static string 客戶列表(string 條件式)
            {
                //return "use FJWSQL select distinct cust.custnm2 as 客戶簡稱 from A22_FAB left join CORDSUB as cordsub on A22_FAB.CORD_NO = cordsub.TRN_NO and A22_FAB.CORD_SN = cordsub.SN left join MKORDSUB as MKORDSUB on mkordSUB.CORD_NO = CORDSUB.trn_no and mkordsub.CORD_SN = cordsub.sn left join CUST as CUST on CUST.CUST_NO = CORDsub.CUST_NO where cordsub.SCLOSE != '結案' AND(SELECT MAX(ITEMIO.TRN_DATE) FROM ITEMIOS LEFT JOIN ITEMIO ON ITEMIOS.IO = ITEMIO.IO AND ITEMIOS.TRN_NO = ITEMIO.TRN_NO WHERE ITEMIOS.IO = '2' AND ITEMIOS.MKORD_NO = MKORDSUB.TRN_NO AND ITEMIOS.MKORD_SN = MKORDSUB.SN AND ITEMIO.S_DESC < > '歸還' AND ITEMIO.MK_TYPE = '成品入庫') != '' AND (SELECT MAX(ITEMIO.TRN_DATE) FROM ITEMIOS LEFT JOIN ITEMIO ON ITEMIOS.IO = ITEMIO.IO AND ITEMIOS.TRN_NO = ITEMIO.TRN_NO WHERE ITEMIOS.IO = '2' AND ITEMIOS.MKORD_NO = MKORDSUB.TRN_NO AND ITEMIOS.MKORD_SN = MKORDSUB.SN AND ITEMIO.S_DESC < > '歸還' AND ITEMIO.MK_TYPE = '成品入庫') <= " + 基準日期 + "";
                return $" SELECT DISTINCT cust.custnm2 AS 客戶簡稱 FROM A22_FAB LEFT JOIN CORDSUB AS cordsub ON A22_FAB.CORD_NO = cordsub.TRN_NO AND A22_FAB.CORD_SN = cordsub.SN LEFT JOIN MKORDSUB AS MKORDSUB ON mkordSUB.CORD_NO = CORDSUB.trn_no AND mkordsub.CORD_SN = cordsub.sn LEFT JOIN CUST AS CUST ON CUST.CUST_NO = CORDsub.CUST_NO LEFT JOIN item ON CORDSUB.ITEM_NO=item.ITEM_NO LEFT JOIN item_22  ON CORDSUB.ITEM_NO=item_22.ITEM_NO WHERE cordsub.SCLOSE != '結案'   AND item.class>='Z' AND item.class<= 'ZR'  AND  (SELECT MAX(ITEMIO.TRN_DATE) FROM ITEMIOS LEFT JOIN ITEMIO ON ITEMIOS.IO = ITEMIO.IO AND ITEMIOS.TRN_NO = ITEMIO.TRN_NO WHERE ITEMIOS.IO = '2' AND ITEMIOS.MKORD_NO = MKORDSUB.TRN_NO AND ITEMIOS.MKORD_SN = MKORDSUB.SN AND ITEMIO.S_DESC < > '歸還' AND ITEMIO.MK_TYPE = '成品入庫') != ''  {條件式}";
            }
            public static string 客戶庫存明細(string 客戶名稱, string 條件式)
            {
                return $"use FJWSQL select cust.custnm2 as 客戶簡稱,A22_FAB.FAB_USER as 產線代號,COUNT(A22_FAB.FAB_USER) as 小計 from A22_FAB left join CORDSUB as cordsub on A22_FAB.CORD_NO = cordsub.TRN_NO and A22_FAB.CORD_SN = cordsub.SN left join MKORDSUB as MKORDSUB on mkordSUB.CORD_NO = CORDSUB.trn_no and mkordsub.CORD_SN = cordsub.sn left join CUST as CUST on CUST.CUST_NO = CORDsub.CUST_NO where cordsub.SCLOSE != '結案' AND(SELECT MAX(ITEMIO.TRN_DATE) FROM ITEMIOS LEFT JOIN ITEMIO ON ITEMIOS.IO = ITEMIO.IO AND ITEMIOS.TRN_NO = ITEMIO.TRN_NO WHERE ITEMIOS.IO = '2' AND ITEMIOS.MKORD_NO = MKORDSUB.TRN_NO AND ITEMIOS.MKORD_SN = MKORDSUB.SN AND ITEMIO.S_DESC < > '歸還' AND ITEMIO.MK_TYPE = '成品入庫') != '' {條件式} and cust.custnm2='{客戶名稱}' group by cust.custnm2,A22_FAB.FAB_USER";
            }
            public static string 客戶庫存金額(string day = "")
            {
                if (day != "")
                    day = $" and a.入庫日 > {day} ";
                return $"  SELECT a.客戶簡稱,a.產線代號,a.製造批號,a.入庫日,DATEDIFF(DAY, Cast(a.入庫日 AS DATETIME), GETUTCDATE()) AS 庫存天數,a.庫存金額 FROM (SELECT cust.custnm2 AS 客戶簡稱,A22_FAB.FAB_USER AS 產線代號 ,mkordsub.LOT_NO AS 製造批號, (SELECT MAX(ITEMIO.TRN_DATE) FROM ITEMIOS, ITEMIO WHERE ITEMIOS.IO=ITEMIO.IO AND ITEMIOS.TRN_NO=ITEMIO.TRN_NO AND ITEMIOS.IO='2' AND ITEMIOS.MKORD_NO=MKORDSUB.TRN_NO AND ITEMIOS.MKORD_SN=MKORDSUB.SN AND ITEMIO.S_DESC< >'歸還' AND ITEMIO.MK_TYPE='成品入庫') AS 入庫日, cordsub.AMOUNT AS 庫存金額 FROM A22_FAB, CORDSUB, MKORDSUB, CUST,item_22,item WHERE A22_FAB.CORD_NO=cordsub.TRN_NO AND A22_FAB.CORD_SN=cordsub.SN AND mkordSUB.CORD_NO=CORDSUB.trn_no AND mkordsub.CORD_SN=cordsub.sn AND CUST.CUST_NO=CORDsub.CUST_NO AND cordsub.SCLOSE !='結案'     AND CUST.CUST_NO=CORDsub.CUST_NO AND  CORDSUB.ITEM_NO=item.ITEM_NO AND CORDSUB.ITEM_NO=item_22.ITEM_NO AND item.class>='Z' AND item.class<= 'ZR' ) AS a WHERE a.入庫日 <> '' {day} ";
            }
            public static string 庫存比例分析(string day)
            {
                return $"SELECT sum(ISNULL(a.逾期數量,0)) 逾期數量,sum(ISNULL(b.一般數量,0)) 一般數量,sum(ISNULL(a.逾期數量,0))+sum(ISNULL(b.一般數量,0)) 總數量 FROM (SELECT a.產線代號,count(a.製造批號) 逾期數量 FROM (SELECT cust.custnm2 客戶簡稱,A22_FAB.FAB_USER 產線代號 ,mkordsub.LOT_NO 製造批號, (SELECT MAX(ITEMIO.TRN_DATE) FROM ITEMIOS, ITEMIO WHERE ITEMIOS.IO=ITEMIO.IO AND ITEMIOS.TRN_NO=ITEMIO.TRN_NO AND ITEMIOS.IO='2' AND ITEMIOS.MKORD_NO=MKORDSUB.TRN_NO AND ITEMIOS.MKORD_SN=MKORDSUB.SN AND ITEMIO.S_DESC< >'歸還' AND ITEMIO.MK_TYPE='成品入庫') 入庫日, cordsub.AMOUNT 庫存金額 FROM A22_FAB, CORDSUB, MKORDSUB, CUST,item_22,item WHERE A22_FAB.CORD_NO=cordsub.TRN_NO AND A22_FAB.CORD_SN=cordsub.SN AND mkordSUB.CORD_NO=CORDSUB.trn_no AND mkordsub.CORD_SN=cordsub.sn AND CUST.CUST_NO=CORDsub.CUST_NO AND cordsub.SCLOSE !='結案'  AND  CORDSUB.ITEM_NO=item.ITEM_NO AND CORDSUB.ITEM_NO=item_22.ITEM_NO AND item.class>='Z' AND item.class<= 'ZR') a WHERE a.入庫日 <> '' AND DATEDIFF(DAY, Cast(a.入庫日 AS DATETIME), GETUTCDATE()) >={day} GROUP BY a.產線代號) a FULL JOIN (SELECT a.產線代號,count(a.製造批號) 一般數量 FROM (SELECT cust.custnm2 客戶簡稱,A22_FAB.FAB_USER 產線代號 ,mkordsub.LOT_NO 製造批號, (SELECT MAX(ITEMIO.TRN_DATE) FROM ITEMIOS, ITEMIO WHERE ITEMIOS.IO=ITEMIO.IO AND ITEMIOS.TRN_NO=ITEMIO.TRN_NO AND ITEMIOS.IO='2' AND ITEMIOS.MKORD_NO=MKORDSUB.TRN_NO AND ITEMIOS.MKORD_SN=MKORDSUB.SN AND ITEMIO.S_DESC< >'歸還' AND ITEMIO.MK_TYPE='成品入庫') 入庫日, cordsub.AMOUNT 庫存金額 FROM A22_FAB, CORDSUB, MKORDSUB, CUST,item_22,item WHERE A22_FAB.CORD_NO=cordsub.TRN_NO AND A22_FAB.CORD_SN=cordsub.SN AND mkordSUB.CORD_NO=CORDSUB.trn_no AND mkordsub.CORD_SN=cordsub.sn AND CUST.CUST_NO=CORDsub.CUST_NO AND cordsub.SCLOSE !='結案'  AND  CORDSUB.ITEM_NO=item.ITEM_NO AND CORDSUB.ITEM_NO=item_22.ITEM_NO AND item.class>='Z' AND item.class<= 'ZR') a WHERE a.入庫日 <> '' AND DATEDIFF(DAY, Cast(a.入庫日 AS DATETIME), GETUTCDATE()) <{day}  GROUP BY a.產線代號) b ON a.產線代號 = b.產線代號";
            }
            public static string 庫存長條圖統計(string text_x, string text_y, string day, string top)
            {
                if (text_x == "客戶" && top != "")
                    top = $" TOP({top}) ";
                else
                    top = "";

                if (text_x == "客戶")
                    text_x = "客戶簡稱";
                else
                    text_x = "產線代號";

                string value = "";
                if (text_y == "數量")
                    value = " count(a.製造批號) ";
                else
                    value = "sum(a.庫存金額)";



                return $" SELECT {top} ISNULL(a.{text_x},b.{text_x}) {text_x}, ISNULL(a.逾期{text_y},0) 逾期{text_y},ISNULL(b.一般{text_y},0) 一般{text_y}, ISNULL(a.逾期{text_y},0)+ISNULL(b.一般{text_y},0) 總{text_y} FROM (SELECT a.{text_x},{value} 逾期{text_y} FROM (SELECT cust.custnm2 客戶簡稱,A22_FAB.FAB_USER 產線代號 ,mkordsub.LOT_NO 製造批號, (SELECT MAX(ITEMIO.TRN_DATE) FROM ITEMIOS, ITEMIO WHERE ITEMIOS.IO=ITEMIO.IO AND ITEMIOS.TRN_NO=ITEMIO.TRN_NO AND ITEMIOS.IO='2' AND ITEMIOS.MKORD_NO=MKORDSUB.TRN_NO AND ITEMIOS.MKORD_SN=MKORDSUB.SN AND ITEMIO.S_DESC< >'歸還' AND ITEMIO.MK_TYPE='成品入庫') 入庫日, cordsub.AMOUNT 庫存金額 FROM A22_FAB, CORDSUB, MKORDSUB, CUST WHERE A22_FAB.CORD_NO=cordsub.TRN_NO AND A22_FAB.CORD_SN=cordsub.SN AND mkordSUB.CORD_NO=CORDSUB.trn_no AND mkordsub.CORD_SN=cordsub.sn AND CUST.CUST_NO=CORDsub.CUST_NO AND cordsub.SCLOSE !='結案') a WHERE a.入庫日 <> '' AND DATEDIFF(DAY, Cast(a.入庫日 AS DATETIME), GETUTCDATE()) >= {day} GROUP BY a.{text_x}) a FULL JOIN (SELECT a.{text_x},{value} 一般{text_y} FROM (SELECT cust.custnm2 客戶簡稱,A22_FAB.FAB_USER 產線代號 ,mkordsub.LOT_NO 製造批號, (SELECT MAX(ITEMIO.TRN_DATE) FROM ITEMIOS, ITEMIO WHERE ITEMIOS.IO=ITEMIO.IO AND ITEMIOS.TRN_NO=ITEMIO.TRN_NO AND ITEMIOS.IO='2' AND ITEMIOS.MKORD_NO=MKORDSUB.TRN_NO AND ITEMIOS.MKORD_SN=MKORDSUB.SN AND ITEMIO.S_DESC< >'歸還' AND ITEMIO.MK_TYPE='成品入庫') 入庫日, cordsub.AMOUNT 庫存金額 FROM A22_FAB, CORDSUB, MKORDSUB, CUST WHERE A22_FAB.CORD_NO=cordsub.TRN_NO AND A22_FAB.CORD_SN=cordsub.SN AND mkordSUB.CORD_NO=CORDSUB.trn_no AND mkordsub.CORD_SN=cordsub.sn AND CUST.CUST_NO=CORDsub.CUST_NO AND cordsub.SCLOSE !='結案') a WHERE a.入庫日 <> '' AND DATEDIFF(DAY, Cast(a.入庫日 AS DATETIME), GETUTCDATE()) <{day} GROUP BY a.{text_x}) b ON a.{text_x} = b.{text_x} order by ISNULL(a.逾期{text_y},0) desc,ISNULL(b.一般{text_y},0) desc";

            }
        }

        public class 業務部_成品庫存分析詳細
        {
            public static string 客戶逾期詳細列表(string 客戶代號, string 條件式)
            {
                return $"select cust.custnm2 as 客戶簡稱,A22_FAB.FAB_USER as 產線代號 ,cordsub.cust_no as 客戶編號,A22_FAB.CORD_NO as 訂單號碼,mkordsub.LOT_NO as 製造批號,(SELECT MAX(ITEMIO.TRN_DATE) FROM ITEMIOS LEFT JOIN ITEMIO ON ITEMIOS.IO=ITEMIO.IO AND ITEMIOS.TRN_NO=ITEMIO.TRN_NO WHERE ITEMIOS.IO='2' AND ITEMIOS.MKORD_NO=MKORDSUB.TRN_NO AND ITEMIOS.MKORD_SN=MKORDSUB.SN AND ITEMIO.S_DESC< >'歸還' AND ITEMIO.MK_TYPE='成品入庫') as 入庫日,'' as 庫存天數,A22_FAB.USER_FIELD08 as 庫存原因 from A22_FAB left join CORDSUB as cordsub on A22_FAB.CORD_NO=cordsub.TRN_NO and A22_FAB.CORD_SN=cordsub.SN left join MKORDSUB as MKORDSUB on mkordSUB.CORD_NO=CORDSUB.trn_no and mkordsub.CORD_SN=cordsub.sn left join CUST as CUST on CUST.CUST_NO=CORDsub.CUST_NO  where cordsub.SCLOSE !='結案' and (SELECT MAX(ITEMIO.TRN_DATE) FROM ITEMIOS LEFT JOIN ITEMIO ON ITEMIOS.IO=ITEMIO.IO AND ITEMIOS.TRN_NO=ITEMIO.TRN_NO WHERE ITEMIOS.IO='2' AND ITEMIOS.MKORD_NO=MKORDSUB.TRN_NO AND ITEMIOS.MKORD_SN=MKORDSUB.SN AND ITEMIO.S_DESC< >'歸還' AND ITEMIO.MK_TYPE='成品入庫') !='' {條件式} AND cust.custnm2 = '{客戶代號}'";
            }
        }

        public class 業務部_未生產分析
        {
            public static DataTable 實領料表(string date_str, string date_end, string line_condi)
            {
                //clsDB_Server clsDB_ = new clsDB_Server(myclass.GetConnByDekdekVisAssm);
                //if (clsDB_.IsConnected == false) clsDB_.dbOpen(myclass.GetConnByDekdekVisAssm);
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
                string sqlcmd = 德大機械.業務部_未生產分析.實際領料數量(date_str, date_end, line_condi);
                DataTable dt_實領料表 = DataTableUtils.GetDataTable(sqlcmd);
                //clsDB_.dbClose();
                return dt_實領料表;
            }
            public static DataTable 組裝日料表(string date_str, string date_end, string line_condi)
            {
                //clsDB_Server clsDB_ = new clsDB_Server(myclass.GetConnByDekdekVisAssm);
                //if (clsDB_.IsConnected == false) clsDB_.dbOpen(myclass.GetConnByDekdekVisAssm);
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
                string sqlcmd = 德大機械.業務部_未生產分析.組裝日數量(date_str, date_end, line_condi);
                DataTable dt_組裝日料表 = DataTableUtils.GetDataTable(sqlcmd);
                //clsDB_.dbClose();
                return dt_組裝日料表;
            }
            public static DataTable LINQ_JOIN實生產與應生產(DataTable dt_應生產表, DataTable dt_實領料表)
            {
                DataTable dtResult = new DataTable();
                dtResult.Columns.Add("日期", typeof(string));
                dtResult.Columns.Add("預定生產數量", typeof(int));
                dtResult.Columns.Add("領料數量", typeof(int));

                var results = from table1 in dt_應生產表.AsEnumerable()
                              join table2 in dt_實領料表.AsEnumerable()
                              on (string)table1["日期"] equals (string)table2["領料日期"] into rows
                              from row in rows.DefaultIfEmpty()
                              select dtResult.LoadDataRow(new object[]
                              {
                            table1.Field<string>("日期"),
                            table1.Field<int>("預定生產數量"),
                            row == null? 0 : row.Field<int>("領料數量")
                              }, false);

                int s = results.Count();

                return dtResult;
            }
            public static DataTable catchday(DataTable dt_應生產表, DataTable dt_實領料表)
            {
                DataTable dtResult = new DataTable();
                dtResult.Columns.Add("日期", typeof(string));

                var results = from table1 in dt_應生產表.AsEnumerable()
                              join table2 in dt_實領料表.AsEnumerable()
                              on (string)table1["日期"] equals (string)table2["領料日期"] into rows
                              from row in rows.DefaultIfEmpty()
                              select dtResult.LoadDataRow(new object[]
                              {
                            table1.Field<string>("日期")
                              }, false);

                int s = results.Count();

                return dtResult;
            }

            public static string 預計生產量_至今(string 日期起, string 今日, string 產線篩選)
            {
                if (產線篩選 != "")
                    產線篩選 = $" and ({產線篩選})";
                //原本的
                //return $"select sum(a.預定生產數量) as 至今累積量 from ( select count(STR_DATE) as 預定生產數量 from A22_FAB  where A22_FAB.STR_DATE >={日期起} and A22_FAB.STR_DATE <= {今日} {產線篩選}  group by STR_DATE ) as a"; ;
                //新版
                return $" SELECT sum(a.預定生產數量) AS 至今累積量 FROM  (SELECT count(A22_FAB.STR_DATE) AS 預定生產數量 FROM A22_FAB LEFT JOIN CORD AS sw_CORD ON sw_CORD.trn_no = A22_FAB.cord_no LEFT JOIN CORDSUB AS sw_CORDSUB ON sw_CORDSUB.TRN_NO = A22_FAB.CORD_no AND sw_CORDSUB.SN = A22_FAB.CORD_SN LEFT JOIN CUST AS sw_CUST ON sw_CUST.CUST_NO = sw_CORD.CUST_NO LEFT JOIN MKORDSUB AS sw_MKORDSUB ON sw_MKORDSUB.CORD_NO = sw_CORDSUB.trn_no AND sw_MKORDSUB.CORD_SN = sw_CORDSUB.sn LEFT JOIN citem AS sw_citem ON sw_CORDSUB.item_no = sw_citem.item_no LEFT JOIN item_22 AS sw_item_22 ON sw_CORDSUB.item_no = sw_item_22.item_no WHERE sw_MKORDSUB.FCLOSE <> 1 AND A22_FAB.STR_DATE >={日期起} AND A22_FAB.STR_DATE <= {今日} {產線篩選} GROUP BY A22_FAB.STR_DATE) AS a";
            }
            public static string 預定生產內容(string 條件式)
            {
                return $" USE FJWSQL select A22_FAB.STR_DATE as 日期 ,count(A22_FAB.STR_DATE) as 預定生產數量 FROM A22_FAB LEFT JOIN CORD AS sw_CORD ON sw_CORD.trn_no = A22_FAB.cord_no  LEFT JOIN CORDSUB AS sw_CORDSUB ON sw_CORDSUB.TRN_NO = A22_FAB.CORD_no   AND sw_CORDSUB.SN = A22_FAB.CORD_SN LEFT JOIN CUST AS sw_CUST ON sw_CUST.CUST_NO = sw_CORD.CUST_NO LEFT JOIN MKORDSUB AS sw_MKORDSUB ON sw_MKORDSUB.CORD_NO = sw_CORDSUB.trn_no AND sw_MKORDSUB.CORD_SN = sw_CORDSUB.sn LEFT JOIN citem AS sw_citem ON sw_CORDSUB.item_no = sw_citem.item_no LEFT JOIN item_22 AS sw_item_22 ON sw_CORDSUB.item_no = sw_item_22.item_no where sw_MKORDSUB.FCLOSE <> 1  {條件式} group by A22_FAB.STR_DATE order by A22_FAB.STR_DATE";
                //return "use FJWSQL select STR_DATE as 日期 ,count(STR_DATE) as 預定生產數量 from A22_FAB " + 條件式 + "  group by STR_DATE order by STR_DATE";
            }
            public static string 實際生產內容(string 日期起, string 日期訖, string 篩選產線)
            {
                //string sqlcmd = "select a.下架日,count(a.下架日) as 實際生產數量 from (  SELECT SUBSTRING(實際完成時間, 1, 8) as 下架日  FROM 工作站狀態資料表   where 實際完成時間 is not null and 實際完成時間 between '" + 日期起 + "' and  '" + 日期訖 + "' )  as a  group by a.下架日 order by  a.下架日 asc";
                return $"select a.下架日,count(a.下架日) as 實際生產數量 from (  SELECT SUBSTRING(實際完成時間, 1, 8) as 下架日  FROM 工作站狀態資料表   where 實際完成時間 is not null and 狀態 = '完成' {篩選產線} ) as a   where  a.下架日 >= {日期起}   and a.下架日 <= {日期訖} group by a.下架日 order by  a.下架日 asc"; ;
            }
            public static string 實際生產內容_new(string date_end, string condition)
            {
                return $" SELECT sw_db.客戶簡稱, sw_db.產線代號, sw_db.製造批號, sw_db.訂單號碼, sw_db.客戶訂單, sw_db.品號, sw_db.訂單日期, sw_db.預計開工日, vis_db.進度, vis_db.狀態, vis_db.組裝日 FROM (SELECT sw_CUST.custnm2 AS 客戶簡稱, sw_item_22.PLINE_NO AS 產線代號, sw_MKORDSUB.LOT_NO AS 製造批號, sw_a22_fab.cord_no AS 訂單號碼, sw_CORD.CORD_NO AS 客戶訂單, sw_MKORDSUB.item_no AS 品號, sw_item.itemnm AS 品名規格, sw_cordsub.trn_date AS 訂單日期, sw_A22_FAB.STR_DATE AS 預計開工日 FROM SW.FJWSQL.dbo.A22_FAB AS sw_A22_FAB LEFT JOIN SW.FJWSQL.dbo.CORD AS sw_CORD ON sw_CORD.trn_no = sw_A22_FAB.cord_no LEFT JOIN SW.FJWSQL.dbo.CORDSUB AS sw_CORDSUB ON sw_CORDSUB.TRN_NO = sw_A22_FAB.CORD_no AND sw_CORDSUB.SN = sw_A22_FAB.CORD_SN LEFT JOIN SW.FJWSQL.dbo.CUST AS sw_CUST ON sw_CUST.CUST_NO = sw_CORD.CUST_NO LEFT JOIN SW.FJWSQL.dbo.MKORDSUB AS sw_MKORDSUB ON sw_MKORDSUB.CORD_NO = sw_CORDSUB.trn_no AND sw_MKORDSUB.CORD_SN = sw_CORDSUB.sn LEFT JOIN SW.FJWSQL.dbo.citem AS sw_citem ON sw_CORDSUB.item_no = sw_citem.item_no LEFT JOIN SW.FJWSQL.dbo.item_22 AS sw_item_22 ON sw_CORDSUB.item_no = sw_item_22.item_no LEFT JOIN SW.FJWSQL.dbo.ITEM AS sw_item ON sw_item.ITEM_NO = sw_item_22.item_no LEFT JOIN SW.FJWSQL.dbo.ITEMIOS AS sw_ITEMIOS ON sw_ITEMIOS.MKORD_NO = sw_MKORDSUB.TRN_NO AND sw_ITEMIOS.MKORD_SN = sw_MKORDSUB.SN WHERE sw_CORDSUB.SCLOSE !='結案' AND sw_item.CLASS='Z' AND sw_item_22.PLINE_NO > 0 AND sw_A22_FAB.STR_DATE <= '{date_end}' {condition} AND (SELECT MAX(SW_ITEMIO.TRN_DATE) FROM SW.FJWSQL.dbo.ITEMIOS AS SW_ITEMIOS LEFT JOIN SW.FJWSQL.dbo.ITEMIO AS SW_ITEMIO ON SW_ITEMIOS.IO=SW_ITEMIO.IO AND SW_ITEMIOS.TRN_NO=SW_ITEMIO.TRN_NO WHERE SW_ITEMIOS.IO='2' AND SW_ITEMIOS.MKORD_NO=sw_MKORDSUB.TRN_NO AND SW_ITEMIOS.MKORD_SN=sw_MKORDSUB.SN AND SW_ITEMIO.S_DESC< >'歸還' AND SW_ITEMIO.MK_TYPE='成品入庫') IS NULL ) AS sw_db LEFT JOIN 工作站狀態資料表 AS vis_db ON sw_db.製造批號 = vis_db.排程編號 where vis_db.狀態 = '完成' ORDER BY vis_db.進度 DESC";
            }
            public static string 未生產列表欄位(string 條件式)
            {
                //"use FJWSQL select DISTINCT item_22.PLINE_NO as 產線代號 from CORDSUB left join CUST as CUST on CUST.CUST_NO=CORDsub.CUST_NO left join item_22 as item_22 on item_22.ITEM_NO=cordsub.item_no left join item as item on item.ITEM_NO=cordsub.item_no where CORDSUB.SCLOSE !='結案' and item.CLASS='Z' and item_22.PLINE_NO > 0 group by CUSTNM2,item_22.PLINE_NO"

                return $"use FJWSQL select distinct item_22.PLINE_NO as 產線代號 from A22_FAB left join CORD as CORD on CORD.trn_no=A22_FAB.cord_no left join CORDSUB as CORDSUB on CORDSUB.TRN_NO=A22_FAB.CORD_no and CORDSUB.SN=A22_FAB.CORD_SN left join CUST as CUST on CUST.CUST_NO=CORD.CUST_NO left join MKORDSUB as MKORDSUB on mkordSUB.CORD_NO=CORDSUB.trn_no and mkordsub.CORD_SN=cordsub.sn left join citem as citem on CORDSUB.item_no=citem.item_no left join item_22 as item_22 on CORDSUB.item_no=item_22.item_no left join ITEM as item on item.ITEM_NO=item_22.item_no left join ITEMIOS as ITEMIOS on ITEMIOS.MKORD_NO=MKORDSUB.TRN_NO AND ITEMIOS.MKORD_SN=MKORDSUB.SN {條件式} and CORDSUB.SCLOSE !='結案' and item.CLASS='Z' and item_22.PLINE_NO > 0 and (SELECT MAX(ITEMIO.TRN_DATE) FROM ITEMIOS LEFT JOIN ITEMIO ON ITEMIOS.IO=ITEMIO.IO AND ITEMIOS.TRN_NO=ITEMIO.TRN_NO WHERE ITEMIOS.IO='2' AND ITEMIOS.MKORD_NO=MKORDSUB.TRN_NO AND ITEMIOS.MKORD_SN=MKORDSUB.SN AND ITEMIO.S_DESC< >'歸還' AND ITEMIO.MK_TYPE='成品入庫') is null"; ;
            }
            public static string 未生產列表內容_客戶欄(string 條件式)
            {
                //"use FJWSQL select distinct cust.custnm2 as 客戶簡稱 from A22_FAB left join CORD as CORD on CORD.trn_no=A22_FAB.cord_no left join CORDSUB as CORDSUB on CORDSUB.TRN_NO=A22_FAB.CORD_no and CORDSUB.SN=A22_FAB.CORD_SN left join CUST as CUST on CUST.CUST_NO=CORD.CUST_NO   left join MKORDSUB as MKORDSUB on mkordSUB.CORD_NO=CORDSUB.trn_no and mkordsub.CORD_SN=cordsub.sn left join citem as citem on CORDSUB.item_no=citem.item_no left join item_22 as item_22 on CORDSUB.item_no=item_22.item_no left join ITEM as item on item.ITEM_NO=item_22.item_no left join ITEMIOS as ITEMIOS on ITEMIOS.MKORD_NO=MKORDSUB.TRN_NO AND ITEMIOS.MKORD_SN=MKORDSUB.SN where CORDSUB.SCLOSE !='結案' and item.CLASS='Z' and item_22.PLINE_NO > 0 and (SELECT MAX(ITEMIO.TRN_DATE) FROM ITEMIOS LEFT JOIN ITEMIO ON ITEMIOS.IO=ITEMIO.IO AND ITEMIOS.TRN_NO=ITEMIO.TRN_NO WHERE ITEMIOS.IO='2' AND ITEMIOS.MKORD_NO=MKORDSUB.TRN_NO AND ITEMIOS.MKORD_SN=MKORDSUB.SN AND ITEMIO.S_DESC< >'歸還' AND ITEMIO.MK_TYPE='成品入庫') is null "
                return $"use FJWSQL select distinct cust.custnm2 as 客戶簡稱 from A22_FAB left join CORD as CORD on CORD.trn_no=A22_FAB.cord_no left join CORDSUB as CORDSUB on CORDSUB.TRN_NO=A22_FAB.CORD_no and CORDSUB.SN=A22_FAB.CORD_SN left join CUST as CUST on CUST.CUST_NO=CORD.CUST_NO   left join MKORDSUB as MKORDSUB on mkordSUB.CORD_NO=CORDSUB.trn_no and mkordsub.CORD_SN=cordsub.sn left join citem as citem on CORDSUB.item_no=citem.item_no left join item_22 as item_22 on CORDSUB.item_no=item_22.item_no left join ITEM as item on item.ITEM_NO=item_22.item_no left join ITEMIOS as ITEMIOS on ITEMIOS.MKORD_NO=MKORDSUB.TRN_NO AND ITEMIOS.MKORD_SN=MKORDSUB.SN {條件式} and CORDSUB.SCLOSE !='結案' and item.CLASS='Z' and item_22.PLINE_NO > 0 and (SELECT MAX(ITEMIO.TRN_DATE) FROM ITEMIOS LEFT JOIN ITEMIO ON ITEMIOS.IO=ITEMIO.IO AND ITEMIOS.TRN_NO=ITEMIO.TRN_NO WHERE ITEMIOS.IO='2' AND ITEMIOS.MKORD_NO=MKORDSUB.TRN_NO AND ITEMIOS.MKORD_SN=MKORDSUB.SN AND ITEMIO.S_DESC< >'歸還' AND ITEMIO.MK_TYPE='成品入庫') is null ";
            }
            public static string 未生產列表內容_客戶未生產詳細(string 客戶簡稱, string 條件式)
            {
                //"use FJWSQL select cust.custnm2 as 客戶簡稱,item_22.PLINE_NO as 產線代號,count(item_22.PLINE_NO) as 小計 from A22_FAB left join CORD as CORD on CORD.trn_no=A22_FAB.cord_no left join CORDSUB as CORDSUB on CORDSUB.TRN_NO=A22_FAB.CORD_no and CORDSUB.SN=A22_FAB.CORD_SN left join CUST as CUST on CUST.CUST_NO=CORD.CUST_NO   left join MKORDSUB as MKORDSUB on mkordSUB.CORD_NO=CORDSUB.trn_no and mkordsub.CORD_SN=cordsub.sn left join citem as citem on CORDSUB.item_no=citem.item_no left join item_22 as item_22 on CORDSUB.item_no=item_22.item_no left join ITEM as item on item.ITEM_NO=item_22.item_no left join ITEMIOS as ITEMIOS on ITEMIOS.MKORD_NO=MKORDSUB.TRN_NO AND ITEMIOS.MKORD_SN=MKORDSUB.SN where CORDSUB.SCLOSE !='結案' and item.CLASS='Z' and item_22.PLINE_NO > 0 and cust.custnm2 = '"+客戶簡稱+"' and (SELECT MAX(ITEMIO.TRN_DATE) FROM ITEMIOS LEFT JOIN ITEMIO ON ITEMIOS.IO=ITEMIO.IO AND ITEMIOS.TRN_NO=ITEMIO.TRN_NO WHERE ITEMIOS.IO='2' AND ITEMIOS.MKORD_NO=MKORDSUB.TRN_NO AND ITEMIOS.MKORD_SN=MKORDSUB.SN AND ITEMIO.S_DESC< >'歸還' AND ITEMIO.MK_TYPE='成品入庫') is null group by item_22.PLINE_NO,cust.custnm2"
                return $"use FJWSQL select cust.custnm2 as 客戶簡稱,item_22.PLINE_NO as 產線代號,count(item_22.PLINE_NO) as 小計 from A22_FAB left join CORD as CORD on CORD.trn_no=A22_FAB.cord_no left join CORDSUB as CORDSUB on CORDSUB.TRN_NO=A22_FAB.CORD_no and CORDSUB.SN=A22_FAB.CORD_SN left join CUST as CUST on CUST.CUST_NO=CORD.CUST_NO   left join MKORDSUB as MKORDSUB on mkordSUB.CORD_NO=CORDSUB.trn_no and mkordsub.CORD_SN=cordsub.sn left join citem as citem on CORDSUB.item_no=citem.item_no left join item_22 as item_22 on CORDSUB.item_no=item_22.item_no left join ITEM as item on item.ITEM_NO=item_22.item_no left join ITEMIOS as ITEMIOS on ITEMIOS.MKORD_NO=MKORDSUB.TRN_NO AND ITEMIOS.MKORD_SN=MKORDSUB.SN {條件式} and CORDSUB.SCLOSE !='結案' and item.CLASS='Z' and item_22.PLINE_NO > 0 and cust.custnm2 = '{客戶簡稱}' and (SELECT MAX(ITEMIO.TRN_DATE) FROM ITEMIOS LEFT JOIN ITEMIO ON ITEMIOS.IO=ITEMIO.IO AND ITEMIOS.TRN_NO=ITEMIO.TRN_NO WHERE ITEMIOS.IO='2' AND ITEMIOS.MKORD_NO=MKORDSUB.TRN_NO AND ITEMIOS.MKORD_SN=MKORDSUB.SN AND ITEMIO.S_DESC< >'歸還' AND ITEMIO.MK_TYPE='成品入庫') is null group by item_22.PLINE_NO,cust.custnm2";

            }
            public static string 未生產列表內容_客戶未生產詳細_All(string 條件式)
            {
                //"use FJWSQL select cust.custnm2 as 客戶簡稱,item_22.PLINE_NO as 產線代號,count(item_22.PLINE_NO) as 小計 from A22_FAB left join CORD as CORD on CORD.trn_no=A22_FAB.cord_no left join CORDSUB as CORDSUB on CORDSUB.TRN_NO=A22_FAB.CORD_no and CORDSUB.SN=A22_FAB.CORD_SN left join CUST as CUST on CUST.CUST_NO=CORD.CUST_NO   left join MKORDSUB as MKORDSUB on mkordSUB.CORD_NO=CORDSUB.trn_no and mkordsub.CORD_SN=cordsub.sn left join citem as citem on CORDSUB.item_no=citem.item_no left join item_22 as item_22 on CORDSUB.item_no=item_22.item_no left join ITEM as item on item.ITEM_NO=item_22.item_no left join ITEMIOS as ITEMIOS on ITEMIOS.MKORD_NO=MKORDSUB.TRN_NO AND ITEMIOS.MKORD_SN=MKORDSUB.SN where CORDSUB.SCLOSE !='結案' and item.CLASS='Z' and item_22.PLINE_NO > 0 and cust.custnm2 = '"+客戶簡稱+"' and (SELECT MAX(ITEMIO.TRN_DATE) FROM ITEMIOS LEFT JOIN ITEMIO ON ITEMIOS.IO=ITEMIO.IO AND ITEMIOS.TRN_NO=ITEMIO.TRN_NO WHERE ITEMIOS.IO='2' AND ITEMIOS.MKORD_NO=MKORDSUB.TRN_NO AND ITEMIOS.MKORD_SN=MKORDSUB.SN AND ITEMIO.S_DESC< >'歸還' AND ITEMIO.MK_TYPE='成品入庫') is null group by item_22.PLINE_NO,cust.custnm2"
                //舊版
                // string sqlcmd = "use FJWSQL select cust.custnm2 as 客戶簡稱,item_22.PLINE_NO as 產線代號,count(item_22.PLINE_NO) as 小計 from A22_FAB left join CORD as CORD on CORD.trn_no=A22_FAB.cord_no left join CORDSUB as CORDSUB on CORDSUB.TRN_NO=A22_FAB.CORD_no and CORDSUB.SN=A22_FAB.CORD_SN left join CUST as CUST on CUST.CUST_NO=CORD.CUST_NO   left join MKORDSUB as MKORDSUB on mkordSUB.CORD_NO=CORDSUB.trn_no and mkordsub.CORD_SN=cordsub.sn left join citem as citem on CORDSUB.item_no=citem.item_no left join item_22 as item_22 on CORDSUB.item_no=item_22.item_no left join ITEM as item on item.ITEM_NO=item_22.item_no left join ITEMIOS as ITEMIOS on ITEMIOS.MKORD_NO=MKORDSUB.TRN_NO AND ITEMIOS.MKORD_SN=MKORDSUB.SN " + 條件式 + " and CORDSUB.SCLOSE !='結案' and item.CLASS='Z' and item_22.PLINE_NO > 0 and  (SELECT MAX(ITEMIO.TRN_DATE) FROM ITEMIOS LEFT JOIN ITEMIO ON ITEMIOS.IO=ITEMIO.IO AND ITEMIOS.TRN_NO=ITEMIO.TRN_NO WHERE ITEMIOS.IO='2' AND ITEMIOS.MKORD_NO=MKORDSUB.TRN_NO AND ITEMIOS.MKORD_SN=MKORDSUB.SN AND ITEMIO.S_DESC< >'歸還' AND ITEMIO.MK_TYPE='成品入庫') is null group by item_22.PLINE_NO,cust.custnm2";
                //新版
                return $" select 客戶簡稱,產線代號,count(客戶簡稱) as 小計 from ( SELECT sw_db.客戶簡稱, sw_db.產線代號, sw_db.製造批號, sw_db.訂單號碼, sw_db.客戶訂單, sw_db.品號, sw_db.訂單日期, sw_db.預計開工日, vis_db.進度, vis_db.狀態, vis_db.組裝日 FROM (SELECT sw_CUST.custnm2 AS 客戶簡稱, sw_item_22.PLINE_NO AS 產線代號, sw_MKORDSUB.LOT_NO AS 製造批號, sw_a22_fab.cord_no AS 訂單號碼, sw_CORD.CORD_NO AS 客戶訂單, sw_MKORDSUB.item_no AS 品號, sw_item.itemnm AS 品名規格, sw_cordsub.trn_date AS 訂單日期, sw_A22_FAB.STR_DATE AS 預計開工日 FROM SW.FJWSQL.dbo.A22_FAB AS sw_A22_FAB LEFT JOIN SW.FJWSQL.dbo.CORD AS sw_CORD ON sw_CORD.trn_no = sw_A22_FAB.cord_no LEFT JOIN SW.FJWSQL.dbo.CORDSUB AS sw_CORDSUB ON sw_CORDSUB.TRN_NO = sw_A22_FAB.CORD_no AND sw_CORDSUB.SN = sw_A22_FAB.CORD_SN LEFT JOIN SW.FJWSQL.dbo.CUST AS sw_CUST ON sw_CUST.CUST_NO = sw_CORD.CUST_NO LEFT JOIN SW.FJWSQL.dbo.MKORDSUB AS sw_MKORDSUB ON sw_MKORDSUB.CORD_NO = sw_CORDSUB.trn_no AND sw_MKORDSUB.CORD_SN = sw_CORDSUB.sn LEFT JOIN SW.FJWSQL.dbo.citem AS sw_citem ON sw_CORDSUB.item_no = sw_citem.item_no LEFT JOIN SW.FJWSQL.dbo.item_22 AS sw_item_22 ON sw_CORDSUB.item_no = sw_item_22.item_no LEFT JOIN SW.FJWSQL.dbo.ITEM AS sw_item ON sw_item.ITEM_NO = sw_item_22.item_no LEFT JOIN SW.FJWSQL.dbo.ITEMIOS AS sw_ITEMIOS ON sw_ITEMIOS.MKORD_NO = sw_MKORDSUB.TRN_NO AND sw_ITEMIOS.MKORD_SN = sw_MKORDSUB.SN WHERE sw_CORDSUB.SCLOSE !='結案' and sw_MKORDSUB.FCLOSE <> 1 AND sw_item.CLASS='Z' {條件式} AND sw_item_22.PLINE_NO > 0 AND (SELECT MAX(SW_ITEMIO.TRN_DATE) FROM SW.FJWSQL.dbo.ITEMIOS AS SW_ITEMIOS LEFT JOIN SW.FJWSQL.dbo.ITEMIO AS SW_ITEMIO ON SW_ITEMIOS.IO=SW_ITEMIO.IO AND SW_ITEMIOS.TRN_NO=SW_ITEMIO.TRN_NO WHERE SW_ITEMIOS.IO='2' AND SW_ITEMIOS.MKORD_NO=sw_MKORDSUB.TRN_NO AND SW_ITEMIOS.MKORD_SN=sw_MKORDSUB.SN AND SW_ITEMIO.S_DESC< >'歸還' AND SW_ITEMIO.MK_TYPE='成品入庫') IS NULL ) AS sw_db LEFT JOIN 工作站狀態資料表 AS vis_db ON sw_db.製造批號 = vis_db.排程編號 where 狀態 <> '完成' OR 狀態 IS NULL OR LEN(狀態) = 0) as a group by 客戶簡稱,產線代號";

            }
            public static string 取得德大產線群組()
            {
                return "SELECT distinct ASSEMBLY_GROUP.GROUP_NAME FROM ASSEMBLY_LINE inner join ASSEMBLY_LINE_GROUP on ASSEMBLY_LINE.LINE_ID = ASSEMBLY_LINE_GROUP.LINE_ID inner join ASSEMBLY_GROUP on ASSEMBLY_LINE_GROUP.GROUP_ID = ASSEMBLY_GROUP.GROUP_ID where ASSEMBLY_LINE.LINE_STATUS = '1' ";
            }
            public static string 取得產線群組與代號()
            {
                return "SELECT DISTINCT ASSEMBLY_GROUP.GROUP_NAME,ASSEMBLY_LINE.LINE_ID FROM ASSEMBLY_LINE INNER JOIN ASSEMBLY_LINE_GROUP ON ASSEMBLY_LINE.LINE_ID = ASSEMBLY_LINE_GROUP.LINE_ID INNER JOIN ASSEMBLY_GROUP ON ASSEMBLY_LINE_GROUP.GROUP_ID = ASSEMBLY_GROUP.GROUP_ID WHERE ASSEMBLY_LINE.LINE_STATUS = '1'";
            }
            public static string 取得德大產線群組_單一(string 群組名稱)
            {

                return $"SELECT ASSEMBLY_GROUP.GROUP_NAME, ASSEMBLY_LINE.LINE_ID FROM ASSEMBLY_LINE inner join ASSEMBLY_LINE_GROUP on ASSEMBLY_LINE.LINE_ID = ASSEMBLY_LINE_GROUP.LINE_ID inner join ASSEMBLY_GROUP on ASSEMBLY_LINE_GROUP.GROUP_ID = ASSEMBLY_GROUP.GROUP_ID where ASSEMBLY_LINE.LINE_STATUS = '1' and ASSEMBLY_GROUP.GROUP_NAME = '{群組名稱}' ";
            }

            public static string 取得產線群組()
            {
                return $"SELECT  distinct ASSEMBLY_GROUP.GROUP_NAME,ASSEMBLY_GROUP.GROUP_ID  FROM ASSEMBLY_LINE left join ASSEMBLY_LINE_GROUP on ASSEMBLY_LINE_GROUP.LINE_ID = ASSEMBLY_LINE.LINE_ID left join ASSEMBLY_GROUP on ASSEMBLY_GROUP.GROUP_ID = ASSEMBLY_LINE_GROUP.GROUP_ID where ASSEMBLY_LINE.LINE_STATUS  != 0";

            }

            public static string 實際領料數量(string 日期起, string 日期迄, string 篩選產線)
            {
                string col_name = "";
                if (篩選產線 != "")
                    col_name = ",工作站編號";

                return $"select 工作站狀態資料表.領料日期,SUM(工作站狀態資料表.領料數量) as 領料數量 from ( select 轉出時間 as 領料日期 , count(轉出時間) as 領料數量 {col_name} from 工作站狀態資料表 group by 轉出時間 {col_name}  )  as 工作站狀態資料表 where 領料數量 > 0 and 領料日期 >= {日期起} and 領料日期 <= {日期迄} {篩選產線} group by 工作站狀態資料表.領料日期  order by 領料日期   ";

            }
            public static string 組裝日數量(string 日期起, string 日期迄, string 篩選產線)
            {
                string col_name = "";
                if (篩選產線 != "")
                    col_name = ",工作站編號";

                return $"select 工作站狀態資料表.領料日期,SUM(工作站狀態資料表.領料數量) as 領料數量 from ( select REPLACE(REPLACE(組裝日, '/', ''), '-', '')   as 領料日期 , count(REPLACE(REPLACE(組裝日, '/', ''), '-', '')) as 領料數量 {col_name} from 工作站狀態資料表 group by 組裝日 {col_name}  )  as 工作站狀態資料表  where 領料數量 > 0 and 領料日期 >= {日期起} and 領料日期 <= {日期迄} {篩選產線} group by 工作站狀態資料表.領料日期  order by 領料日期   ";


            }

        }
        public class 業務部_未生產分析詳細
        {
            public static string 客戶未生產詳細表(string 客戶簡稱, string 條件式, string 選擇產線)
            {
                if (條件式 != "")
                    條件式 = $" AND {條件式} ";
                //string sqlcmd = "use FJWSQL select cust.custnm2 as 客戶簡稱,item_22.PLINE_NO as 產線代號,a22_fab.cord_no as 訂單號碼,a22_fab.cord_sn as 訂單明細序, CORD.CORD_NO  as 客戶訂單, MKORDSUB.LOT_NO as 製造批號, MKORDSUB.item_no as 品號, item.itemnm as 品名規格, cordsub.trn_date as 訂單日期, A22_FAB.STR_DATE as 預計開工日 from A22_FAB left join CORD as CORD on CORD.trn_no=A22_FAB.cord_no left join CORDSUB as CORDSUB on CORDSUB.TRN_NO=A22_FAB.CORD_no and CORDSUB.SN=A22_FAB.CORD_SN left join CUST as CUST on CUST.CUST_NO=CORD.CUST_NO left join MKORDSUB as MKORDSUB on mkordSUB.CORD_NO=CORDSUB.trn_no and mkordsub.CORD_SN=cordsub.sn left join citem as citem on CORDSUB.item_no=citem.item_no left join item_22 as item_22 on CORDSUB.item_no=item_22.item_no left join ITEM as item on item.ITEM_NO=item_22.item_no left join ITEMIOS as ITEMIOS on ITEMIOS.MKORD_NO=MKORDSUB.TRN_NO AND ITEMIOS.MKORD_SN=MKORDSUB.SN where CORDSUB.SCLOSE !='結案' and item.CLASS='Z' and item_22.PLINE_NO > 0 and (SELECT MAX(ITEMIO.TRN_DATE) FROM ITEMIOS LEFT JOIN ITEMIO ON ITEMIOS.IO=ITEMIO.IO AND ITEMIOS.TRN_NO=ITEMIO.TRN_NO WHERE ITEMIOS.IO='2' AND ITEMIOS.MKORD_NO=MKORDSUB.TRN_NO AND ITEMIOS.MKORD_SN=MKORDSUB.SN AND ITEMIO.S_DESC< >'歸還' AND ITEMIO.MK_TYPE='成品入庫') is null and cust.custnm2 = '" +客戶簡稱+"' "+條件式+"  order by a22_fab.cord_no asc ";
                return "select  " +
                                "sw_db.客戶簡稱, " +
                                "sw_db.產線代號, " +
                                "sw_db.製造批號, " +
                                "sw_db.訂單號碼, " +
                                "sw_db.客戶訂單, " +
                                "sw_db.品號, " +
                                "sw_db.訂單日期, " +
                                "sw_db.預計開工日, " +
                                "vis_db.進度, " +
                                "vis_db.狀態, " +
                                "vis_db.組裝日 " +
                                "from " +
                                "(" +
                                "select " +
                                "sw_CUST.custnm2 as 客戶簡稱, " +
                                "sw_item_22.PLINE_NO as 產線代號, " +
                                "sw_MKORDSUB.LOT_NO AS 製造批號, " +
                                "sw_a22_fab.cord_no as 訂單號碼, " +
                                "sw_CORD.CORD_NO  as 客戶訂單, " +
                                "sw_MKORDSUB.item_no as 品號, " +
                                "sw_item.itemnm as 品名規格, " +
                                "sw_cordsub.trn_date as 訂單日期, " +
                                "sw_A22_FAB.STR_DATE as 預計開工日 " +
                                "from " +
                                "SW.FJWSQL.dbo.A22_FAB AS sw_A22_FAB " +
                                "left join SW.FJWSQL.dbo.CORD as sw_CORD on sw_CORD.trn_no = sw_A22_FAB.cord_no " +
                                "left join SW.FJWSQL.dbo.CORDSUB as sw_CORDSUB on sw_CORDSUB.TRN_NO = sw_A22_FAB.CORD_no and sw_CORDSUB.SN = sw_A22_FAB.CORD_SN " +
                                "left join SW.FJWSQL.dbo.CUST as sw_CUST on sw_CUST.CUST_NO = sw_CORD.CUST_NO " +
                                "left join SW.FJWSQL.dbo.MKORDSUB as sw_MKORDSUB on sw_MKORDSUB.CORD_NO = sw_CORDSUB.trn_no and sw_MKORDSUB.CORD_SN = sw_CORDSUB.sn " +
                                "left join SW.FJWSQL.dbo.citem as sw_citem on sw_CORDSUB.item_no = sw_citem.item_no " +
                                "left join SW.FJWSQL.dbo.item_22 as sw_item_22 on sw_CORDSUB.item_no = sw_item_22.item_no " +
                                "left join SW.FJWSQL.dbo.ITEM as sw_item on sw_item.ITEM_NO = sw_item_22.item_no " +
                                "left join SW.FJWSQL.dbo.ITEMIOS as sw_ITEMIOS on sw_ITEMIOS.MKORD_NO = sw_MKORDSUB.TRN_NO AND sw_ITEMIOS.MKORD_SN = sw_MKORDSUB.SN " +
                                "where   " +
                                "sw_CORDSUB.SCLOSE !='結案' and " +
                                $"sw_item.CLASS='Z' {條件式}  and " +
                                $"sw_item_22.PLINE_NO > 0 {選擇產線} and " +
                                $"(SELECT MAX(SW_ITEMIO.TRN_DATE) FROM SW.FJWSQL.dbo.ITEMIOS AS SW_ITEMIOS LEFT JOIN SW.FJWSQL.dbo.ITEMIO AS SW_ITEMIO ON SW_ITEMIOS.IO=SW_ITEMIO.IO AND SW_ITEMIOS.TRN_NO=SW_ITEMIO.TRN_NO WHERE SW_ITEMIOS.IO='2' AND SW_ITEMIOS.MKORD_NO=sw_MKORDSUB.TRN_NO AND SW_ITEMIOS.MKORD_SN=sw_MKORDSUB.SN AND SW_ITEMIO.S_DESC< >'歸還' AND SW_ITEMIO.MK_TYPE='成品入庫') is null and sw_CUST.custnm2  = '{客戶簡稱}' ) as sw_db left join  工作站狀態資料表 as vis_db on sw_db.製造批號 = vis_db.排程編號 order by vis_db.進度 desc  ";



            }
        }

        //----------------------------------------------修改的這邊----------------------------------------------
        public class 業務部_出貨統計
        {
            public static string 各產線成品出貨數(string 日期起, string 日期訖)
            {
                string sqlcmd = "select item_22.PLINE_NO as 產線代號, sum(INVOSUB.QUANTITY) as 出貨數量  from INVOSUB  left join CUST as cust on cust.CUST_NO=INVOSUB.CUST_NO  left join CORDSUB as cordsub on cordsub.TRN_NO=INVOSUB.CORD_NO and cordsub.SN=INVOSUB.CORD_SN  left join item_22 as item_22 on INVOSUB.ITEM_NO=item_22.ITEM_NO  left join item as item on INVOSUB.ITEM_NO=item.ITEM_NO  where invosub.TRN_DATE>=" + 日期起 + " and invosub.TRN_DATE<=" + 日期訖 + " and item.class>='Z' and item.class<= 'ZR' and item_22.PLINE_NO > 0  group by item_22.PLINE_NO";
                return sqlcmd;
            }
            public static string 沒有產線代號(string 日期起, string 日期訖)
            {
                return "select item_22.PLINE_NO as 產線代號, sum(INVOSUB.QUANTITY) as 出貨數量  from INVOSUB  left join CUST as cust on cust.CUST_NO=INVOSUB.CUST_NO  left join CORDSUB as cordsub on cordsub.TRN_NO=INVOSUB.CORD_NO and cordsub.SN=INVOSUB.CORD_SN  left join item_22 as item_22 on INVOSUB.ITEM_NO=item_22.ITEM_NO  left join item as item on INVOSUB.ITEM_NO=item.ITEM_NO  where invosub.TRN_DATE>=" + 日期起 + " and invosub.TRN_DATE<=" + 日期訖 + " and item.class>='Z' and item.class<= 'ZR' and item_22.PLINE_NO is null group by item_22.PLINE_NO";
            }
            public static string 成品出貨總數(string 日期起, string 日期訖)
            {
                string sqlcmd = "select sum(INVOSUB.QUANTITY) as 出貨數量 from INVOSUB left join CUST as cust on cust.CUST_NO=INVOSUB.CUST_NO left join CORDSUB as cordsub on cordsub.TRN_NO=INVOSUB.CORD_NO and cordsub.SN=INVOSUB.CORD_SN left join item_22 as item_22 on INVOSUB.ITEM_NO=item_22.ITEM_NO left join item as item on INVOSUB.ITEM_NO=item.ITEM_NO where invosub.TRN_DATE>=" + 日期起 + " and invosub.TRN_DATE<=" + 日期訖 + " and item.class>='Z' and item.class<= 'ZR' and item_22.PLINE_NO > 0  ";
                return sqlcmd;
            }
            public static string 客戶列表(string 日期起, string 日期訖)
            {
                string sql_cmd = "select distinct cust.custnm2 as 客戶簡稱 from INVOSUB left join CUST as cust on cust.CUST_NO=INVOSUB.CUST_NO left join CORDSUB as cordsub on cordsub.TRN_NO=INVOSUB.CORD_NO and cordsub.SN=INVOSUB.CORD_SN left join item_22 as item_22 on INVOSUB.ITEM_NO=item_22.ITEM_NO left join item as item on INVOSUB.ITEM_NO=item.ITEM_NO where invosub.TRN_DATE>=" + 日期起 + " and invosub.TRN_DATE<=" + 日期訖 + " and item.class>='Z' and item.class<= 'ZR' and item_22.PLINE_NO > 0 ";
                return sql_cmd;
            }
            public static string 客戶_沒有產線代號(string 日期起, string 日期訖)
            {
                return "select distinct cust.custnm2 as 客戶簡稱 from INVOSUB left join CUST as cust on cust.CUST_NO=INVOSUB.CUST_NO left join CORDSUB as cordsub on cordsub.TRN_NO=INVOSUB.CORD_NO and cordsub.SN=INVOSUB.CORD_SN left join item_22 as item_22 on INVOSUB.ITEM_NO=item_22.ITEM_NO left join item as item on INVOSUB.ITEM_NO=item.ITEM_NO where invosub.TRN_DATE>=" + 日期起 + " and invosub.TRN_DATE<=" + 日期訖 + " and item.class>='Z' and item.class<= 'ZR' and item_22.PLINE_NO is null ";
            }
            public static string 客戶出貨明細(string 客戶名稱, string 日期起, string 日期訖)
            {
                string sql_cmd = "select cust.custnm2 as 客戶簡稱,item_22.PLINE_NO as 產線代號,sum(INVOSUB.QUANTITY) as 小計 from INVOSUB left join CUST as cust on cust.CUST_NO=INVOSUB.CUST_NO left join CORDSUB as cordsub on cordsub.TRN_NO=INVOSUB.CORD_NO and cordsub.SN=INVOSUB.CORD_SN left join item_22 as item_22 on INVOSUB.ITEM_NO=item_22.ITEM_NO left join item as item on INVOSUB.ITEM_NO=item.ITEM_NO where invosub.TRN_DATE>=" + 日期起 + " and invosub.TRN_DATE<=" + 日期訖 + " and item.class>='Z' and item.class<= 'ZR'  and cust.custnm2='" + 客戶名稱 + "' and item_22.PLINE_NO > 0 group by INVOSUB.QUANTITY,item_22.PLINE_NO,cust.custnm2 order by item_22.PLINE_NO";
                return sql_cmd;
            }
            public static string 出貨_沒有產線代號(string 客戶名稱, string 日期起, string 日期訖)
            {
                string sql_cmd = "select cust.custnm2 as 客戶簡稱,item_22.PLINE_NO as 產線代號,sum(INVOSUB.QUANTITY) as 小計 from INVOSUB left join CUST as cust on cust.CUST_NO=INVOSUB.CUST_NO left join CORDSUB as cordsub on cordsub.TRN_NO=INVOSUB.CORD_NO and cordsub.SN=INVOSUB.CORD_SN left join item_22 as item_22 on INVOSUB.ITEM_NO=item_22.ITEM_NO left join item as item on INVOSUB.ITEM_NO=item.ITEM_NO where invosub.TRN_DATE>=" + 日期起 + " and invosub.TRN_DATE<=" + 日期訖 + " and item.class>='Z' and item.class<= 'ZR'  and cust.custnm2='" + 客戶名稱 + "' and item_22.PLINE_NO is null group by INVOSUB.QUANTITY,item_22.PLINE_NO,cust.custnm2 order by item_22.PLINE_NO";
                return sql_cmd;

            }
        }
        public class 業務部_出貨詳細
        {
            public static string 客戶出貨詳細表(string 客戶名稱, string 日期起, string 日期訖)
            {
                string sql_cmd = "select cust.custnm2 as 客戶簡稱,item_22.PLINE_NO as 產線代號,INVOSUB.ITEM_NO as 品號,INVOSUB.itemnm as 品名規格,count(INVOSUB.QUANTITY) as 小計 from INVOSUB left join CUST as cust on cust.CUST_NO=INVOSUB.CUST_NO left join CORDSUB as cordsub on cordsub.TRN_NO=INVOSUB.CORD_NO and cordsub.SN=INVOSUB.CORD_SN left join item_22 as item_22 on INVOSUB.ITEM_NO=item_22.ITEM_NO left join item as item on INVOSUB.ITEM_NO=item.ITEM_NO where invosub.TRN_DATE>=" + 日期起 + " and invosub.TRN_DATE<=" + 日期訖 + " and item.class>='Z' and item.class<= 'ZR'  and cust.custnm2='" + 客戶名稱 + "' and item_22.PLINE_NO > 0 group by INVOSUB.QUANTITY,item_22.PLINE_NO,cust.custnm2,INVOSUB.ITEM_NO,INVOSUB.itemnm";
                return sql_cmd;
            }
            public static string 客戶出貨詳細表_webservice(string 客戶名稱, string 品號, string 日期起, string 日期訖)
            {
                string sql_cmd = "select INVOSUB.TRN_DATE as 出貨日期,INVOSUB.TRN_NO as 出貨單號,mkordsub.LOT_NO as 製造批號,mkordsub.ITEM_NO as CCS,CORDSUB.REMARK as 訂單備註 from INVOSUB left join CUST as cust on cust.CUST_NO=INVOSUB.CUST_NO left join CORDSUB as cordsub on cordsub.TRN_NO=INVOSUB.CORD_NO and cordsub.SN=INVOSUB.CORD_SN left join MKORDSUB as mkordsub on cordsub.TRN_NO=mkordsub.CORD_NO and cordsub.SN=mkordsub.CORD_SN left join item_22 as item_22 on INVOSUB.ITEM_NO=item_22.ITEM_NO left join item as item on INVOSUB.ITEM_NO=item.ITEM_NO where invosub.TRN_DATE>=" + 日期起 + " and invosub.TRN_DATE<=" + 日期訖 + " and item.class>='Z' and item.class<= 'ZR'  and cust.custnm2='" + 客戶名稱 + "' and INVOSUB.ITEM_NO = '" + 品號 + "' order by INVOSUB.TRN_DATE desc";
                return sql_cmd;
            }
        }
        public class 業務部_訂單統計
        {
            // 2019/06/28，訂單數量及金額統計
            public static string 產線代號列表(string 日期起, string 日期訖)
            {
                string sqlCmd_ = "SELECT                                                                                " +
                                " item_22.PLINE_NO                                                                     " +
                                //" COUNT(cordsub.trn_no) as 訂單數量,                                                     " +
                                //" LEFT(sum(CORDSUB.AMOUNT), CHARINDEX('.', sum(CORDSUB.AMOUNT)) - 1) AS 訂單金額 ,       " +
                                //" LEFT(sum(CORDSUB.SCOST), CHARINDEX('.', sum(CORDSUB.SCOST)) - 1)  as 成本              " +
                                " FROM CORDSUB AS cordsub                                                               " +
                                //" left join INVOSUB on cordsub.SN = INVOSUB.CORD_SN and cordsub.TRN_NO = INVOSUB.CORD_NO" +
                                //" LEFT JOIN CUST AS cust ON cust.CUST_NO = CORDSUB.CUST_NO                              " +
                                " LEFT JOIN item_22 AS item_22 ON item_22.ITEM_NO = cordsub.ITEM_NO                     " +
                                " WHERE item_22.PLINE_NO > 0                                                            " +
                                " and D_DATE >= " + 日期起 + " and D_DATE <= " + 日期訖 + " " +
                                " GROUP BY item_22.PLINE_NO";
                return sqlCmd_;
            }

            public class 訂單數量與金額
            {
                //訂單圖形數據 
                public string 取得訂單圖形數據(string TOP, string Columns, string OrderBy, string dt_st, string dt_ed, string 訂單狀態)
                {
                    string InA22_Fab = "";
                    string GroupBy = Columns;
                    string Scloce = 取得訂單狀態(ref InA22_Fab, 訂單狀態);
                    if (TOP != "") TOP = " TOP ( " + TOP + " ) ";
                    if (OrderBy != "") OrderBy = " order by " + OrderBy + " desc ";
                    string sqlCmd_ = " SELECT                                                                              " +
                                     " " + TOP + "                                                                         " +
                                     " sum(QUANTITY) as QUANTITY,                                                          " +
                                     " sum(AMOUNT) as AMOUNT,                                                              " +
                                     " " + Columns + "                                                                     " +
                                     " FROM CORDSUB                                                                        " +
                                     " left join ITEM_22 on ITEM_22.ITEM_NO = CORDSUB.ITEM_NO                              " +
                                     " left join CUST on  CUST.CUST_NO = CORDSUB.CUST_NO                                   " +
                                     " where ITEM_22.PLINE_NO > 0 and D_DATE >= " + dt_st + " and D_DATE <= " + dt_ed + "  " +
                                     " " + Scloce + "                                                                      " +
                                     " " + InA22_Fab + "                                                                   " +
                                     " group by " + GroupBy + "                                                            " +
                                       OrderBy;
                    return sqlCmd_;
                }

                //訂單表身，取得客戶清單
                public string 取得客戶列表(string TOP, string OrderBy, string dt_st, string dt_ed, string 訂單狀態)
                {
                    if (TOP != "") TOP = " TOP ( " + TOP + " ) ";
                    string InA22_Fab = "";
                    string Scloce = 取得訂單狀態(ref InA22_Fab, 訂單狀態);
                    if (OrderBy != "") OrderBy = " order by " + OrderBy + " desc ";
                    string sqlCmd_ = "SELECT                                                                                " +
                                     " distinct " + TOP + " CUST.CUSTNM2 as 客戶簡稱,                                        " +
                                     " sum(QUANTITY) as QUANTITY,                                                           " +
                                     " sum(AMOUNT) as AMOUNT                                                                " +
                                     " FROM CORDSUB                                                                         " +
                                     " LEFT JOIN item_22 ON cordsub.ITEM_NO = item_22.ITEM_NO                               " +
                                     " left join CUST on CUST.CUST_NO = CORDSUB.CUST_NO                                     " +
                                     " WHERE  ITEM_22.PLINE_NO > 0 and D_DATE >= " + dt_st + " and D_DATE <= " + dt_ed + "  " +
                                     " " + Scloce + "                                                                       " +
                                     " " + InA22_Fab + "                                                                    " +
                                     " GROUP BY CUST.CUSTNM2                                                                " +
                                       OrderBy;
                    return sqlCmd_;
                }

                //訂單表身，取得客戶訂單明細
                public string 取得客戶訂單明細(string cust, string dt_st, string dt_ed, string 訂單狀態)
                {
                    string InA22_Fab = "";
                    string Scloce = 取得訂單狀態(ref InA22_Fab, 訂單狀態);
                    string sqlCmd_ = " SELECT                                                                              " +
                                     " CUST.CUSTNM2 as 客戶名稱,                                                            " +
                                     " item_22.PLINE_NO,                                                                   " +
                                     " sum(cordsub.QUANTITY) as 數量,                                                       " +
                                     " LEFT(sum(CORDSUB.AMOUNT), CHARINDEX('.', sum(CORDSUB.AMOUNT)) - 1) AS 金額           " +
                                     //" sum(CORDSUB.SCOST) as 成本                                                         " +
                                     " FROM CORDSUB AS cordsub                                                              " +
                                     " LEFT JOIN CUST AS cust ON cust.CUST_NO = CORDSUB.CUST_NO                             " +
                                     " LEFT JOIN item_22 AS item_22 ON item_22.ITEM_NO = cordsub.ITEM_NO                    " +
                                     " WHERE item_22.PLINE_NO > 0 and D_DATE >= " + dt_st + " and D_DATE <= " + dt_ed + "   " +
                                     " " + Scloce + "                                                                       " +
                                     " " + InA22_Fab + "                                                                    " +
                                     "   and cust.CUSTNM2 = '" + cust + "'                                                  " +
                                     " GROUP BY cust.CUSTNM2,item_22.PLINE_NO";
                    return sqlCmd_;
                }

                public string 取得訂單詳細資訊(string cust, string start, string end, string 訂單狀態)
                {
                    string InA22_Fab = "";
                    string Scloce = 取得訂單狀態(ref InA22_Fab, 訂單狀態);
                    string sqlcmd = "SELECT cust.custnm2  AS 客戶簡稱,                                                " +
                                    "cordsub.trn_no AS 訂單號碼,                                                      " +
                                    "mkordsub.item_no AS CCS,                                                        " +
                                    "mkordsub.lot_no AS 製造批號,                                                     " +
                                    "item_22.pline_no AS 產線代號,                                                    " +
                                    "cordsub.sclose AS 訂單狀況,    EMPLOY.Name 業務員,                                                  " +
                                    "a22_fab.str_date AS 預計開工日,                                                  " +
                                    "(SELECT Max(itemio.trn_date)                                                    " +
                                    " FROM itemios                                                                   " +
                                    " LEFT JOIN itemio ON itemios.io = itemio.io AND itemios.trn_no = itemio.trn_no  " +
                                    " WHERE itemios.io = '2'                                                         " +
                                    " AND itemios.mkord_no = mkordsub.trn_no AND itemios.mkord_sn = mkordsub.sn      " +
                                    " AND itemio.s_desc < > '歸還'                                                   " +
                                    " AND itemio.mk_type = '成品入庫') AS 入庫日,INVOSUB.TRN_DATE 出貨日,                                     " +
                                    " CONVERT(VARCHAR(12), CONVERT(MONEY, cordsub.quantity), 1) AS 數量              " +
                                    //" cord.currency AS 幣別,                                                         " +
                                    //" CONVERT(VARCHAR(20), CONVERT(MONEY, cord.rate), 1)        AS 匯率,             " +
                                    //" CONVERT(VARCHAR(20), CONVERT(MONEY, cordsub.o_price), 1)  AS 外幣單價,         " +
                                    //" CONVERT(VARCHAR(20), CONVERT(MONEY, cordsub.amount), 1)   AS 台幣單價          " +
                                    " FROM cordsub AS cordsub                                                       " +
                                    "  LEFT JOIN a22_fab ON cordsub.trn_no = a22_fab.cord_no AND cordsub.sn = a22_fab.cord_sn " +
                                    "  LEFT JOIN mkordsub AS mkordsub ON cordsub.trn_no = mkordsub.cord_no AND cordsub.sn = mkordsub.cord_sn " +
                                    "  LEFT JOIN cust AS cust ON cust.cust_no = cordsub.cust_no                     " +
                                    "  LEFT JOIN item_22 AS item_22 ON item_22.item_no = cordsub.item_no            " +
                                    "  LEFT JOIN cord AS cord ON cord.trn_no = cordsub.trn_no  left join EMPLOY on EMPLOY.EMP_NO = CORD.USER_CODE    LEFT JOIN INVOSUB ON cordsub.TRN_NO=INVOSUB.CORD_NO AND cordsub.SN=INVOSUB.CORD_SN                 " +
                                    " WHERE item_22.pline_no > 0                                                    " +
                                    " " + Scloce + "                                                                " +
                                     " " + InA22_Fab + "                                                            " +
                                    "  AND cordsub.d_date >= " + start + " AND cordsub.d_date <= " + end + "         " +
                                    "  AND cust.custnm2 = '" + cust + "'                                            ";
                    return sqlcmd;
                }

                //(共用"取得訂單狀態"、"取得組進表數據")
                public string 取得訂單狀態(ref string InA22_Fab, string 訂單狀態)
                {
                    string Closed = " AND CORDSUB.SCLOSE !='未結' ";
                    string NotClosed = " AND CORDSUB.SCLOSE ='未結'  ";
                    string IN = " IN                          ";
                    string NotIN = " NOT IN                      ";
                    string Sclose = "";
                    switch (訂單狀態)
                    {
                        case "1":
                            //已結案訂單
                            Sclose = Closed;
                            break;
                        case "2":
                            //未結案訂單
                            Sclose = NotClosed;
                            break;
                        case "3":
                            //未結案訂單+已排程訂單
                            Sclose = NotClosed;
                            InA22_Fab = 取得組進表數據(IN);
                            break;
                        case "4":
                            //未結案訂單+未排程訂單
                            Sclose = NotClosed;
                            InA22_Fab = 取得組進表數據(NotIN);
                            break;
                    }
                    return Sclose;
                }
                public string 取得組進表數據(string Condition)
                {
                    return " AND (CORDSUB.TRN_NO + CORDSUB.SN)                                  " +
                           " " + Condition + "                                                  " +
                           " (SELECT A22_FAB.CORD_NO + A22_FAB.CORD_SN                          " +
                           " FROM A22_FAB AS A22_FAB                                            " +
                           " WHERE                                                              " +
                           " CORDSUB.TRN_NO = A22_FAB.CORD_NO AND CORDSUB.SN = A22_FAB.CORD_SN) ";
                }

            }
            public static string 訂單數量與金額圖形數據(string TOP, string Columns, string OrderBy, string dt_st, string dt_ed, string 訂單狀態)
            {
                訂單數量與金額 orders = new 訂單數量與金額();
                return orders.取得訂單圖形數據(TOP, Columns, OrderBy, dt_st, dt_ed, 訂單狀態);
            }
            public static string 訂單客戶列表(string TOP, string OrderBy, string dt_st, string dt_ed, string 訂單狀態)
            {
                訂單數量與金額 custs = new 訂單數量與金額();
                return custs.取得客戶列表(TOP, OrderBy, dt_st, dt_ed, 訂單狀態);
            }
            public static string 逾期訂單客戶列表(string start)
            {
                return $" SELECT CUST.CUSTNM2 AS 客戶簡稱,sum(cordsub.QUANTITY) AS over_qty, LEFT(sum(CORDSUB.AMOUNT), CHARINDEX('.', sum(CORDSUB.AMOUNT)) - 1) AS over_amt FROM CORDSUB AS cordsub LEFT JOIN CUST AS cust ON cust.CUST_NO = CORDSUB.CUST_NO  left join INVOSUB  on cordsub.TRN_NO=INVOSUB.CORD_NO and cordsub.SN=INVOSUB.CORD_SN  LEFT JOIN item_22 AS item_22 ON item_22.ITEM_NO = cordsub.ITEM_NO WHERE item_22.PLINE_NO > 0 AND D_DATE <{start}  and (  CORDSUB.SCLOSE ='未結' OR INVOSUB.TRN_DATE >={start}) GROUP BY cust.CUSTNM2";
            }
            public static string 訂單客戶明細(string cust, string dt_st, string dt_ed, string 訂單狀態)
            {
                訂單數量與金額 cust_details = new 訂單數量與金額();
                return cust_details.取得客戶訂單明細(cust, dt_st, dt_ed, 訂單狀態);
            }
            public static string 逾期訂單明細(string start, string cust)
            {
                return $" SELECT cust.custnm2 AS 客戶簡稱, cordsub.trn_no AS 訂單號碼, mkordsub.item_no AS CCS, mkordsub.lot_no AS 製造批號, item_22.pline_no AS 產線代號, cordsub.sclose AS 訂單狀況 ,EMPLOY.Name 業務員, a22_fab.str_date AS 預計開工日, (SELECT Max(itemio.trn_date) FROM itemios LEFT JOIN itemio ON itemios.io = itemio.io AND itemios.trn_no = itemio.trn_no WHERE itemios.io = '2' AND itemios.mkord_no = mkordsub.trn_no AND itemios.mkord_sn = mkordsub.sn AND itemio.s_desc < > '歸還' AND itemio.mk_type = '成品入庫') AS 入庫日,INVOSUB.TRN_DATE 出貨日, CONVERT(VARCHAR(12), CONVERT(MONEY, cordsub.quantity), 1) AS 數量 FROM cordsub AS cordsub LEFT JOIN a22_fab ON cordsub.trn_no = a22_fab.cord_no AND cordsub.sn = a22_fab.cord_sn LEFT JOIN mkordsub AS mkordsub ON cordsub.trn_no = mkordsub.cord_no AND cordsub.sn = mkordsub.cord_sn LEFT JOIN cust AS cust ON cust.cust_no = cordsub.cust_no LEFT JOIN INVOSUB ON cordsub.TRN_NO=INVOSUB.CORD_NO AND cordsub.SN=INVOSUB.CORD_SN  LEFT JOIN item_22 AS item_22 ON item_22.item_no = cordsub.item_no LEFT JOIN cord AS cord ON cord.trn_no = cordsub.trn_no LEFT JOIN EMPLOY ON  EMPLOY.EMP_NO  = CORD.USER_CODE WHERE item_22.pline_no > 0 AND cordsub.d_date < {start} AND (CORDSUB.SCLOSE ='未結' OR INVOSUB.TRN_DATE >={start}) AND cust.custnm2 = '{cust}'";
            }
            public static string 訂單詳細資訊(string cust, string start, string end, string 訂單狀態)
            {
                訂單數量與金額 order_details = new 訂單數量與金額();
                return order_details.取得訂單詳細資訊(cust, start, end, 訂單狀態);
            }
            public static string 逾期訂單總數統計(string start)
            {
                return $" SELECT sum(CORDSUB.QUANTITY) AS QUANTITY, sum(CORDSUB.AMOUNT) AS AMOUNT, item_22.PLINE_NO FROM CORDSUB LEFT JOIN ITEM_22 ON ITEM_22.ITEM_NO = CORDSUB.ITEM_NO LEFT JOIN CUST ON CUST.CUST_NO = CORDSUB.CUST_NO  left join INVOSUB  on cordsub.TRN_NO=INVOSUB.CORD_NO and cordsub.SN=INVOSUB.CORD_SN  WHERE ITEM_22.PLINE_NO > 0 AND D_DATE <{start}  and (  CORDSUB.SCLOSE ='未結' OR INVOSUB.TRN_DATE >={start}) GROUP BY item_22.PLINE_NO";
            }
            public static string 逾期客戶總數統計(string start)
            {
                return $" SELECT sum(CORDSUB.QUANTITY) AS QUANTITY, sum(CORDSUB.AMOUNT) AS AMOUNT, CUSTNM2 FROM CORDSUB LEFT JOIN ITEM_22 ON ITEM_22.ITEM_NO = CORDSUB.ITEM_NO  left join INVOSUB  on cordsub.TRN_NO=INVOSUB.CORD_NO and cordsub.SN=INVOSUB.CORD_SN  LEFT JOIN CUST ON CUST.CUST_NO = CORDSUB.CUST_NO WHERE ITEM_22.PLINE_NO > 0 AND D_DATE < {start} and (  CORDSUB.SCLOSE ='未結' OR INVOSUB.TRN_DATE >={start}) GROUP BY CUSTNM2 ORDER BY QUANTITY DESC";
            }

            public static string 取得日期語法(string 日期起, string 日期訖, string 資料表)
            {
                //組進表   : " and A22_FAB.STR_DATE >=" + date_str + " and A22_FAB.STR_DATE <=" + date_end + " ";
                //訂單明細 : " and CORDSUB.TRN_DATE >= " + 日期起 + " and CORDSUB.TRN_DATE <= " + 日期訖 + "    ";

                if (日期起 == "" && 日期訖 == "") return "";

                string 語法 = "";
                string 欄位 = "";
                switch (資料表)
                {
                    case "CORDSUB":
                        欄位 = " CORDSUB.D_DATE";    //20190531，依訂單明細的預交貨日查詢
                        break;

                    case "A22_FAB":
                        欄位 = "A22_FAB.STR_DATE";
                        break;

                    case "INVOSUB":
                        欄位 = "INVOSUB.TRN_DATE";
                        break;
                }

                語法 = "and " + 欄位 + " >= " + 日期起 + " and " + 欄位 + " <= " + 日期訖 + "";
                return 語法;
            }
            public static string 取得訂單狀態語法(string 條件式)
            {
                string sqlcmd = "and cordsub.SCLOSE " + 條件式 + "";
                return sqlcmd;
            }
            //訂單總數
            public static string 各產線訂單資料_訂單總數(string 日期起訖)
            {
                string sql_cmd = "USE FJWSQL                                            " +
                                " SELECT                                                                                " +
                                " item_22.PLINE_NO as 產線代號,                                                          " +
                                " COUNT(cordsub.trn_no) as 訂單數量,                                                        " +
                                " LEFT(sum(CORDSUB.AMOUNT), CHARINDEX('.', sum(CORDSUB.AMOUNT)) - 1) AS 訂單金額 , " +
                                " LEFT(sum(CORDSUB.SCOST), CHARINDEX('.', sum(CORDSUB.SCOST)) - 1)  as 成本 " +
                                " FROM CORDSUB AS cordsub                                                               " +
                                " left join INVOSUB on cordsub.SN = INVOSUB.CORD_SN and cordsub.TRN_NO = INVOSUB.CORD_NO" +
                                " LEFT JOIN CUST AS cust ON cust.CUST_NO = CORDSUB.CUST_NO                              " +
                                " LEFT JOIN item_22 AS item_22 ON item_22.ITEM_NO = cordsub.ITEM_NO                     " +
                                " WHERE item_22.PLINE_NO > 0                                                            " +
                                "   " + 日期起訖 + "                                                                      " +
                                " GROUP BY item_22.PLINE_NO";
                return sql_cmd;
            }
            public static string 客戶列表_訂單總數(string 日期起訖)
            {
                string sql_cmd = "USE FJWSQL                                                                             " +
                                " SELECT                                                                                 " +
                                " distinct CUST.CUSTNM2 as 客戶簡稱                                                       " +
                                " FROM CORDSUB AS cordsub                                                                " +
                                " LEFT JOIN CUST AS cust ON cust.CUST_NO = CORDSUB.CUST_NO                               " +
                                " LEFT JOIN item_22 AS item_22 ON item_22.ITEM_NO = cordsub.ITEM_NO                      " +
                                " WHERE item_22.PLINE_NO > 0                                                             " +
                                "   " + 日期起訖 + "                                                                         " +
                                " GROUP BY cust.CUSTNM2";
                return sql_cmd;
            }
            public static string 客戶訂單明細_訂單總數(string 客戶名稱, string 日期起訖)
            {
                string sql_cmd = "USE FJWSQL                                                                                " +
                                    " SELECT                                                                                " +
                                    " CUST.CUSTNM2 as 客戶名稱,                                                              " +
                                    " item_22.PLINE_NO as 產線代號,                                                          " +
                                    " COUNT(cordsub.trn_no) as 訂單數量,                                                        " +
                                    " sum(CORDSUB.AMOUNT) as 訂單金額,                                                       " +
                                    " sum(CORDSUB.SCOST) as 成本                                                            " +
                                    " FROM CORDSUB AS cordsub                                                               " +
                                    " LEFT JOIN CUST AS cust ON cust.CUST_NO = CORDSUB.CUST_NO                              " +
                                    " LEFT JOIN item_22 AS item_22 ON item_22.ITEM_NO = cordsub.ITEM_NO                     " +
                                    " WHERE item_22.PLINE_NO > 0                                                            " +
                                    "   " + 日期起訖 + "                                                                      " +
                                    "   and cust.CUSTNM2 = '" + 客戶名稱 + "'                                                 " +
                                    " GROUP BY cust.CUSTNM2,item_22.PLINE_NO";
                return sql_cmd;
            }
            public static string 訂單詳細表_訂單總數(string 客戶名稱, string 日期起訖)
            {
                string sql_cmd = "SELECT cust.CUSTNM2 AS 客戶簡稱,CORDSUB.TRN_NO AS 訂單號碼,mkordsub.ITEM_NO AS CCS,        " +
                                 "mkordsub.LOT_NO AS 製造批號,item_22.PLINE_NO AS 產線代號,cordsub.SCLOSE AS 訂單狀況,A22_FAB.STR_DATE AS 預計開工日," +
                                 "  (SELECT MAX(ITEMIO.TRN_DATE)                                                           " +
                                 "  FROM ITEMIOS                                                                           " +
                                 "   LEFT JOIN ITEMIO ON ITEMIOS.IO = ITEMIO.IO                                            " +
                                 "   AND ITEMIOS.TRN_NO = ITEMIO.TRN_NO                                                    " +
                                 "   WHERE ITEMIOS.IO = '2'                                                                " +
                                 "     AND ITEMIOS.MKORD_NO = MKORDSUB.TRN_NO                                              " +
                                 "     AND ITEMIOS.MKORD_SN = MKORDSUB.SN                                                  " +
                                 "     AND ITEMIO.S_DESC < > '歸還'                                                        " +
                                 "     AND ITEMIO.MK_TYPE = '成品入庫') AS 入庫日, Convert(Varchar(12), CONVERT(MONEY, cordsub.QUANTITY), 1) AS 數量, cord.CURRENCY AS 幣別,Convert(Varchar(20), CONVERT(MONEY, cord.RATE), 1) AS 匯率, Convert(Varchar(20), CONVERT(MONEY, CORDSUB.O_PRICE), 1)AS 外幣單價, Convert(Varchar(20), CONVERT(MONEY, CORDSUB.AMOUNT), 1)AS 台幣單價 " +
                                 "FROM CORDSUB AS cordsub                                                                  " +
                                 "LEFT JOIN A22_FAB ON cordsub.TRN_NO = A22_FAB.CORD_NO AND cordsub.sn = A22_FAB.CORD_SN   " +
                                 "LEFT JOIN MKORDSUB AS mkordsub ON cordsub.TRN_NO = mkordsub.CORD_NO AND cordsub.SN = mkordsub.CORD_SN " +
                                 "LEFT JOIN CUST AS cust ON cust.CUST_NO = CORDSUB.CUST_NO                                 " +
                                 "LEFT JOIN item_22 AS item_22 ON item_22.ITEM_NO = cordsub.ITEM_NO                        " +
                                 "LEFT JOIN CORD AS cord ON cord.TRN_NO = cordsub.TRN_NO                                   " +
                                 "LEFT JOIN INVOSUB ON cordsub.SN = INVOSUB.CORD_SN AND cordsub.TRN_NO = INVOSUB.CORD_NO   " +
                                 "WHERE item_22.PLINE_NO > 0                                                               " +
                                 "   " + 日期起訖 + "                                                                      " +
                                 "  AND cust.CUSTNM2 = '" + 客戶名稱 + "'";
                return sql_cmd;
            }

            //已結案
            public static string 各產線訂單資料_已結案(string 日期起訖, string 條件式)
            {
                //20190527
                //string sql_cmd = "use FJWSQL select item_22.PLINE_NO as 產線代號,COUNT(item_22.PLINE_NO) as 訂單數量,LEFT(sum(CORDSUB.AMOUNT), CHARINDEX('.', sum(CORDSUB.AMOUNT)) - 1) as 訂單金額 from A22_FAB left join CORDSUB as cordsub on cordsub.TRN_NO=A22_FAB.CORD_NO and cordsub.sn=A22_FAB.CORD_SN left join MKORDSUB as mkordsub on cordsub.TRN_NO=mkordsub.CORD_NO and cordsub.SN=mkordsub.CORD_SN  left join CUST as cust on cust.CUST_NO=CORDSUB.CUST_NO left join item_22 as item_22 on item_22.ITEM_NO=cordsub.ITEM_NO where item_22.PLINE_NO > 0  and A22_FAB.STR_DATE>=" + 日期起 + " and A22_FAB.STR_DATE<=" + 日期訖 + "  " + 條件式_已結or未結 + " group by item_22.PLINE_NO ";

                //20190528
                string sql_cmd = "USE FJWSQL                                            " +
                                " SELECT                                                                                " +
                                " item_22.PLINE_NO as 產線代號,                                                          " +
                                " COUNT(cordsub.trn_no) as 訂單數量,                                                        " +
                                " LEFT(sum(CORDSUB.AMOUNT), CHARINDEX('.', sum(CORDSUB.AMOUNT)) - 1) AS 訂單金額 , " +
                                " LEFT(sum(CORDSUB.SCOST), CHARINDEX('.', sum(CORDSUB.SCOST)) - 1)  as 成本 " +
                                " FROM CORDSUB AS cordsub                                                               " +
                                " left join INVOSUB on cordsub.SN = INVOSUB.CORD_SN and cordsub.TRN_NO = INVOSUB.CORD_NO" +
                                " LEFT JOIN CUST AS cust ON cust.CUST_NO = CORDSUB.CUST_NO                              " +
                                " LEFT JOIN item_22 AS item_22 ON item_22.ITEM_NO = cordsub.ITEM_NO                     " +
                                " WHERE item_22.PLINE_NO > 0                                                            " +
                                "   " + 條件式 + "                                                                        " +
                                "   " + 日期起訖 + "                                                                      " +
                                " GROUP BY item_22.PLINE_NO";
                return sql_cmd;
            }
            public static string 客戶列表_已結案(string 日期起訖, string 條件式)
            {
                string sql_cmd = "USE FJWSQL                                                                             " +
                                " SELECT                                                                                 " +
                                " distinct CUST.CUSTNM2 as 客戶簡稱                                                       " +
                                " FROM CORDSUB AS cordsub                                                                " +
                                " left join INVOSUB on cordsub.SN = INVOSUB.CORD_SN and cordsub.TRN_NO = INVOSUB.CORD_NO " +
                                " LEFT JOIN CUST AS cust ON cust.CUST_NO = CORDSUB.CUST_NO                               " +
                                " LEFT JOIN item_22 AS item_22 ON item_22.ITEM_NO = cordsub.ITEM_NO                      " +
                                " WHERE item_22.PLINE_NO > 0                                                             " +
                                "   " + 條件式 + "                                                                         " +
                                "   " + 日期起訖 + "                                                                         " +
                                " GROUP BY cust.CUSTNM2";
                return sql_cmd;
            }
            public static string 客戶訂單明細_已結案(string 客戶名稱, string 日期起訖, string 條件式)
            {
                string sql_cmd = "USE FJWSQL                                                                                " +
                                    " SELECT                                                                                " +
                                    " CUST.CUSTNM2 as 客戶名稱,                                                              " +
                                    " item_22.PLINE_NO as 產線代號,                                                          " +
                                    " COUNT(cordsub.trn_no) as 訂單數量,                                                        " +
                                    " sum(CORDSUB.AMOUNT) as 訂單金額,                                                       " +
                                    " sum(CORDSUB.SCOST) as 成本                                                            " +
                                    " FROM CORDSUB AS cordsub                                                               " +
                                    //" left join INVOSUB on cordsub.SN = INVOSUB.CORD_SN and cordsub.TRN_NO = INVOSUB.CORD_NO" +
                                    " LEFT JOIN CUST AS cust ON cust.CUST_NO = CORDSUB.CUST_NO                              " +
                                    " LEFT JOIN item_22 AS item_22 ON item_22.ITEM_NO = cordsub.ITEM_NO                     " +
                                    " WHERE item_22.PLINE_NO > 0                                                            " +
                                    "   " + 條件式 + "                                                      " +
                                    "   " + 日期起訖 + "                                                                      " +
                                    "   and cust.CUSTNM2 = '" + 客戶名稱 + "'                                                 " +
                                    " GROUP BY cust.CUSTNM2,item_22.PLINE_NO";
                return sql_cmd;
            }
            public static string 訂單詳細表_已結案(string 客戶名稱, string 日期起訖, string 訂單狀態)
            {
                string sql_cmd = "SELECT cust.CUSTNM2 AS 客戶簡稱,CORDSUB.TRN_NO AS 訂單號碼,mkordsub.ITEM_NO AS CCS,        " +
                                 "mkordsub.LOT_NO AS 製造批號,item_22.PLINE_NO AS 產線代號,cordsub.SCLOSE AS 訂單狀況,A22_FAB.STR_DATE AS 預計開工日," +
                                 "  (SELECT MAX(ITEMIO.TRN_DATE)                                                           " +
                                 "  FROM ITEMIOS                                                                           " +
                                 "   LEFT JOIN ITEMIO ON ITEMIOS.IO = ITEMIO.IO                                            " +
                                 "   AND ITEMIOS.TRN_NO = ITEMIO.TRN_NO                                                    " +
                                 "   WHERE ITEMIOS.IO = '2'                                                                " +
                                 "     AND ITEMIOS.MKORD_NO = MKORDSUB.TRN_NO                                              " +
                                 "     AND ITEMIOS.MKORD_SN = MKORDSUB.SN                                                  " +
                                 "     AND ITEMIO.S_DESC < > '歸還'                                                        " +
                                 "     AND ITEMIO.MK_TYPE = '成品入庫') AS 入庫日, Convert(Varchar(12), CONVERT(MONEY, cordsub.QUANTITY), 1) AS 數量, cord.CURRENCY AS 幣別,Convert(Varchar(20), CONVERT(MONEY, cord.RATE), 1) AS 匯率, Convert(Varchar(20), CONVERT(MONEY, CORDSUB.O_PRICE), 1)AS 外幣單價, Convert(Varchar(20), CONVERT(MONEY, CORDSUB.AMOUNT), 1)AS 台幣單價 " +
                                 "FROM CORDSUB AS cordsub                                                                  " +
                                 "LEFT JOIN A22_FAB ON cordsub.TRN_NO = A22_FAB.CORD_NO AND cordsub.sn = A22_FAB.CORD_SN   " +
                                 "LEFT JOIN MKORDSUB AS mkordsub ON cordsub.TRN_NO = mkordsub.CORD_NO AND cordsub.SN = mkordsub.CORD_SN " +
                                 "LEFT JOIN CUST AS cust ON cust.CUST_NO = CORDSUB.CUST_NO                                 " +
                                 "LEFT JOIN item_22 AS item_22 ON item_22.ITEM_NO = cordsub.ITEM_NO                        " +
                                 "LEFT JOIN CORD AS cord ON cord.TRN_NO = cordsub.TRN_NO                                   " +
                                 "LEFT JOIN INVOSUB ON cordsub.SN = INVOSUB.CORD_SN AND cordsub.TRN_NO = INVOSUB.CORD_NO   " +
                                 "WHERE item_22.PLINE_NO > 0                                                               " +
                                 "   " + 訂單狀態 + "                                                                      " +
                                 "   " + 日期起訖 + "                                                                      " +
                                 "  AND cust.CUSTNM2 = '" + 客戶名稱 + "'";
                return sql_cmd;
            }

            //未結案
            public static string 各產線訂單資料_未結案by已上線加未上線(string 日期起訖, string 條件式)
            {
                //20190527
                //string sql_cmd = "SELECT item_22.PLINE_NO as 產線代號 ,count(item_22.PLINE_NO) as 訂單數量 , sum(CORDSUB.AMOUNT) as 訂單金額 ,sum(CORDSUB.SCOST) as 成本 FROM CORDSUB left join item_22 on CORDSUB.ITEM_NO = item_22.ITEM_NO where item_22.PLINE_NO > 0 "+條件式+"  "+ 日期起訖 + " group by item_22.PLINE_NO";
                //string sql_cmd= "select a.產線代號,sum(訂單數量) as 訂單數量,sum(訂單金額) as 訂單金額,sum(a.成本) as 成本 from ( SELECT item_22.PLINE_NO as 產線代號 , count(item_22.PLINE_NO) as 訂單數量 ,  sum(CORDSUB.AMOUNT) as 訂單金額 , sum(CORDSUB.SCOST) as 成本  FROM CORDSUB  left join A22_FAB as A22_FAB on cordsub.TRN_NO=A22_FAB.CORD_NO and cordsub.sn=A22_FAB.CORD_SN  left join item_22 on CORDSUB.ITEM_NO = item_22.ITEM_NO  where item_22.PLINE_NO > 0 and cordsub.SCLOSE ='未結'    " + 日期起訖 + "group by item_22.PLINE_NO"+
                //                " union SELECT item_22.PLINE_NO as 產線代號,count(item_22.PLINE_NO) as 訂單數量, LEFT(sum(CORDSUB.AMOUNT), CHARINDEX('.', sum(CORDSUB.AMOUNT)) - 1) as 訂單金額, sum(SCOST) as 成本 FROM CORDSUB as cordsub LEFT JOIN item_22 AS item_22 ON cordsub.ITEM_NO = item_22.ITEM_NO WHERE NOT EXISTS(SELECT * FROM A22_FAB as a22_fab WHERE cordsub.TRN_NO = a22_fab.CORD_NO AND cordsub.SN = a22_fab.CORD_SN) and cordsub.SCLOSE = '未結' AND PLINE_NO > 0    group by item_22.PLINE_NO) a group by a.產線代號";

                //20190528
                string sql_cmd = "select" +
                                " PLINE_NO as 產線代號," +
                                " count(item_22.PLINE_NO) as 訂單數量," +
                                " LEFT(sum(CORDSUB.AMOUNT), CHARINDEX('.', sum(CORDSUB.AMOUNT)) - 1) AS 訂單金額 , " +
                                " LEFT(sum(CORDSUB.SCOST), CHARINDEX('.', sum(CORDSUB.SCOST)) - 1)  as 成本 " +
                                " from CORDSUB" +
                                " left join item_22 on CORDSUB.ITEM_NO = item_22.ITEM_NO" +
                                " where" +
                                " PLINE_NO > 0 " +
                                  日期起訖 +
                                  條件式 +
                                "and item_22.PLINE_NO > 0" +
                                " group by PLINE_NO";
                return sql_cmd;
            }
            public static string 客戶列表_未結案(string 日期起訖, string 條件式, string NoExists, string union)
            {
                //20190527
                //string union_cmd = " union select distinct cust.CUSTNM2 as 客戶簡稱 from CORDSUB as cordsub  left join CUST as cust on cust.CUST_NO = CORDSUB.CUST_NO  left join item_22 as item_22 on item_22.ITEM_NO = cordsub.ITEM_NO where  NOT EXISTS ( SELECT * FROM A22_FAB as a22_fab WHERE cordsub.TRN_NO = a22_fab.CORD_NO AND cordsub.SN = a22_fab.CORD_SN) and item_22.PLINE_NO > 0  and cordsub.SCLOSE ='未結' group by cust.CUSTNM2,item_22.PLINE_NO,item_22.PLINE_NO";
                //string sql_cmd = "use FJWSQL select distinct cust.CUSTNM2 as 客戶簡稱 from CORDSUB as cordsub left join A22_FAB on cordsub.TRN_NO = A22_FAB.CORD_NO and cordsub.sn = A22_FAB.CORD_SN left join MKORDSUB as mkordsub on cordsub.TRN_NO = mkordsub.CORD_NO and cordsub.SN = mkordsub.CORD_SN  left join CUST as cust on cust.CUST_NO = CORDSUB.CUST_NO left join item_22 as item_22 on item_22.ITEM_NO = cordsub.ITEM_NO where " + NoExists + "  item_22.PLINE_NO > 0 " + 日期起訖 + " " + 條件式 + " group by cust.CUSTNM2,item_22.PLINE_NO,item_22.PLINE_NO";
                //if (union != "")
                //{
                //    sql_cmd += union_cmd;
                //}

                //20190528
                string sql_cmd = "USE FJWSQL" +
                                 " SELECT distinct cust.CUSTNM2 AS 客戶簡稱" +
                                 " FROM CORDSUB AS cordsub" +
                                 " LEFT JOIN CUST AS cust ON cust.CUST_NO = CORDSUB.CUST_NO" +
                                 " LEFT JOIN item_22 AS item_22 ON item_22.ITEM_NO = cordsub.ITEM_NO" +
                                 " WHERE item_22.PLINE_NO > 0" +
                                   日期起訖 +
                                   條件式 +
                                 " GROUP BY cust.CUSTNM2, item_22.PLINE_NO";
                return sql_cmd;

            }
            public static string 客戶訂單明細_未結案(string 客戶名稱, string 日期起訖, string 條件式)
            {
                //20190527
                //string union_cmd = " union select cust.CUSTNM2 as 客戶簡稱,item_22.PLINE_NO as 產線代號,count(item_22.PLINE_NO) as 訂單數量 ,LEFT(sum(CORDSUB.AMOUNT),CHARINDEX('.',sum(CORDSUB.AMOUNT))-1) as 訂單金額  from CORDSUB as cordsub left join A22_FAB on cordsub.TRN_NO = A22_FAB.CORD_NO and cordsub.sn = A22_FAB.CORD_SN left join MKORDSUB as mkordsub on cordsub.TRN_NO = mkordsub.CORD_NO and cordsub.SN = mkordsub.CORD_SN  left join CUST as cust on cust.CUST_NO = CORDSUB.CUST_NO left join item_22 as item_22 on item_22.ITEM_NO = cordsub.ITEM_NO where " + NoExists + "  item_22.PLINE_NO > 0 and cust.CUSTNM2 = '" + 客戶名稱 + "'  " + 條件式 + " group by cust.CUSTNM2,item_22.PLINE_NO,item_22.PLINE_NO";
                //string sql_cmd = "use FJWSQL select cust.CUSTNM2 as 客戶簡稱,item_22.PLINE_NO as 產線代號,count(item_22.PLINE_NO) as 訂單數量 ,LEFT(sum(CORDSUB.AMOUNT),CHARINDEX('.',sum(CORDSUB.AMOUNT))-1) as 訂單金額  from CORDSUB as cordsub left join A22_FAB on cordsub.TRN_NO = A22_FAB.CORD_NO and cordsub.sn = A22_FAB.CORD_SN left join MKORDSUB as mkordsub on cordsub.TRN_NO = mkordsub.CORD_NO and cordsub.SN = mkordsub.CORD_SN  left join CUST as cust on cust.CUST_NO = CORDSUB.CUST_NO left join item_22 as item_22 on item_22.ITEM_NO = cordsub.ITEM_NO where item_22.PLINE_NO > 0 and cust.CUSTNM2 = '" + 客戶名稱 + "' " + 日期起訖 + " " + 條件式 + " group by cust.CUSTNM2,item_22.PLINE_NO,item_22.PLINE_NO";
                //string sql_cmd = "use FJWSQL select cust.CUSTNM2 as 客戶簡稱,item_22.PLINE_NO as 產線代號,count(item_22.PLINE_NO) as 訂單數量 ,LEFT(sum(CORDSUB.AMOUNT),CHARINDEX('.',sum(CORDSUB.AMOUNT))-1) as 訂單金額  from CORDSUB as cordsub left join A22_FAB on cordsub.TRN_NO = A22_FAB.CORD_NO and cordsub.sn = A22_FAB.CORD_SN left join MKORDSUB as mkordsub on cordsub.TRN_NO = mkordsub.CORD_NO and cordsub.SN = mkordsub.CORD_SN  left join CUST as cust on cust.CUST_NO = CORDSUB.CUST_NO left join item_22 as item_22 on item_22.ITEM_NO = cordsub.ITEM_NO where item_22.PLINE_NO > 0 and cust.CUSTNM2 = '" + 客戶名稱 + "' " + 日期起訖 + " " + 條件式 + " group by cust.CUSTNM2,item_22.PLINE_NO,item_22.PLINE_NO";
                //if (union != "")
                //{
                //    sql_cmd += union_cmd;
                //}
                //
                //if (status == "4")
                //{
                //    sql_cmd = "use FJWSQL select cust.CUSTNM2 as 客戶簡稱,item_22.PLINE_NO as 產線代號,count(item_22.PLINE_NO) as 訂單數量 ,LEFT(sum(CORDSUB.AMOUNT),CHARINDEX('.',sum(CORDSUB.AMOUNT))-1) as 訂單金額  from CORDSUB as cordsub left join A22_FAB on cordsub.TRN_NO = A22_FAB.CORD_NO and cordsub.sn = A22_FAB.CORD_SN left join MKORDSUB as mkordsub on cordsub.TRN_NO = mkordsub.CORD_NO and cordsub.SN = mkordsub.CORD_SN  left join CUST as cust on cust.CUST_NO = CORDSUB.CUST_NO left join item_22 as item_22 on item_22.ITEM_NO = cordsub.ITEM_NO where "+ NoExists + " item_22.PLINE_NO > 0 and cust.CUSTNM2 = '" + 客戶名稱 + "' " + 日期起訖 + " " + 條件式 + " group by cust.CUSTNM2,item_22.PLINE_NO,item_22.PLINE_NO";
                //}

                //20190528
                string sql_cmd = "USE FJWSQL" +
                                " SELECT cust.CUSTNM2 AS 客戶簡稱,                                   " +
                                " PLINE_NO as 產線代號,                                              " +
                                " count(item_22.PLINE_NO) as 訂單數量,                               " +
                                " sum(CORDSUB.AMOUNT) as 訂單金額 , sum(CORDSUB.SCOST) as 成本        " +
                                " FROM CORDSUB AS cordsub                                            " +
                                " LEFT JOIN CUST AS cust ON cust.CUST_NO = CORDSUB.CUST_NO           " +
                                " LEFT JOIN item_22 AS item_22 ON item_22.ITEM_NO = cordsub.ITEM_NO  " +
                                " WHERE item_22.PLINE_NO > 0                                         " +
                                "   AND cust.CUSTNM2 = '" + 客戶名稱 + "'                              " +
                                "   " + 日期起訖 + "                                                   " +
                                "   " + 條件式 + "                                                     " +
                                " group by PLINE_NO,cust.CUSTNM2     ";
                return sql_cmd;
            }
            public static string 訂單詳細表_未結案(string 客戶名稱, string 日期起訖, string 訂單狀態)
            {
                string sql_cmd = "SELECT cust.CUSTNM2 AS 客戶簡稱,CORDSUB.TRN_NO AS 訂單號碼,mkordsub.ITEM_NO AS CCS,        " +
                                 "mkordsub.LOT_NO AS 製造批號,item_22.PLINE_NO AS 產線代號,cordsub.SCLOSE AS 訂單狀況,A22_FAB.STR_DATE AS 預計開工日," +
                                 "  (SELECT MAX(ITEMIO.TRN_DATE)                                                           " +
                                 "  FROM ITEMIOS                                                                           " +
                                 "   LEFT JOIN ITEMIO ON ITEMIOS.IO = ITEMIO.IO                                            " +
                                 "   AND ITEMIOS.TRN_NO = ITEMIO.TRN_NO                                                    " +
                                 "   WHERE ITEMIOS.IO = '2'                                                                " +
                                 "     AND ITEMIOS.MKORD_NO = MKORDSUB.TRN_NO                                              " +
                                 "     AND ITEMIOS.MKORD_SN = MKORDSUB.SN                                                  " +
                                 "     AND ITEMIO.S_DESC < > '歸還'                                                        " +
                                 "     AND ITEMIO.MK_TYPE = '成品入庫') AS 入庫日, Convert(Varchar(12), CONVERT(MONEY, cordsub.QUANTITY), 1) AS 數量, cord.CURRENCY AS 幣別,Convert(Varchar(20), CONVERT(MONEY, cord.RATE), 1) AS 匯率, Convert(Varchar(20), CONVERT(MONEY, CORDSUB.O_PRICE), 1)AS 外幣單價, Convert(Varchar(20), CONVERT(MONEY, CORDSUB.AMOUNT), 1)AS 台幣單價 " +
                                 "FROM CORDSUB AS cordsub                                                                  " +
                                 "LEFT JOIN A22_FAB ON cordsub.TRN_NO = A22_FAB.CORD_NO AND cordsub.sn = A22_FAB.CORD_SN   " +
                                 "LEFT JOIN MKORDSUB AS mkordsub ON cordsub.TRN_NO = mkordsub.CORD_NO AND cordsub.SN = mkordsub.CORD_SN " +
                                 "LEFT JOIN CUST AS cust ON cust.CUST_NO = CORDSUB.CUST_NO                                 " +
                                 "LEFT JOIN item_22 AS item_22 ON item_22.ITEM_NO = cordsub.ITEM_NO                        " +
                                 "LEFT JOIN CORD AS cord ON cord.TRN_NO = cordsub.TRN_NO                                   " +
                                 "WHERE item_22.PLINE_NO > 0                                                               " +
                                 "   " + 訂單狀態 + "                                                                      " +
                                 "   " + 日期起訖 + "                                                                      " +
                                 "  AND cust.CUSTNM2 = '" + 客戶名稱 + "'";
                return sql_cmd;
            }

            //已排程(未結案)
            public static string 各產線訂單資料_已排程(string 日期起訖, string 條件式)
            {
                //20190527
                //將已排入組進表的訂單明細找出來，未結案的訂單
                //日期區間： STR_DATE (預計開工日)
                //使用訂單明細狀況 > 去組進表找 (SCLOSE!=結案) > 若有資料 = 有訂單 已上線 未完成
                //string sql_cmd = "SELECT  item_22.PLINE_NO AS 產線代號, COUNT(item_22.PLINE_NO) AS 訂單數量 , LEFT(sum(CORDSUB.AMOUNT), CHARINDEX('.', sum(CORDSUB.AMOUNT)) - 1) as 訂單金額,SUM(CORDSUB.SCOST) AS 成本 FROM CORDSUB LEFT JOIN A22_FAB ON CORDSUB.TRN_NO = A22_FAB.CORD_NO AND CORDSUB.SN = A22_FAB.CORD_SN LEFT JOIN item_22 ON CORDSUB.ITEM_NO = item_22.ITEM_NO  WHERE item_22.PLINE_NO > 0 " + 條件式 + " AND A22_FAB.STR_DATE is not null  " + 日期起訖 + "   GROUP BY item_22.PLINE_NO ";

                //20190528
                string sql_cmd = "SELECT item_22.PLINE_NO AS 產線代號,                                                   " +
                                "  COUNT(item_22.PLINE_NO) AS 訂單數量,                                                  " +
                                "   LEFT(sum(CORDSUB.AMOUNT), CHARINDEX('.', sum(CORDSUB.AMOUNT)) - 1) AS 訂單金額,      " +
                                "   LEFT(sum(CORDSUB.SCOST), CHARINDEX('.', sum(CORDSUB.SCOST)) - 1) AS 成本                                                          " +
                                " FROM CORDSUB                                                                          " +
                                " LEFT JOIN A22_FAB ON CORDSUB.TRN_NO = A22_FAB.CORD_NO AND CORDSUB.SN = A22_FAB.CORD_SN" +
                                " LEFT JOIN item_22 ON CORDSUB.ITEM_NO = item_22.ITEM_NO                                " +
                                " WHERE (CORDSUB.TRN_NO+CORDSUB.sn) IN (SELECT A22_FAB.CORD_NO+A22_FAB.CORD_SN FROM A22_FAB AS a22_fab WHERE cordsub.TRN_NO = a22_fab.CORD_NO AND cordsub.SN = a22_fab.CORD_SN) " +
                                "   and item_22.PLINE_NO > 0                                                            " +
                                "   " + 條件式 + "                                                                       " +
                                "   " + 日期起訖 + "                                                                     " +
                                " GROUP BY item_22.PLINE_NO";
                return sql_cmd;
            }
            public static string 客戶列表_已排程(string 日期起訖, string 條件式)
            {
                string sql_cmd = "SELECT                                                " +
                                " distinct cust.CUSTNM2 as 客戶簡稱                                                      " +
                                " FROM CORDSUB                                                                          " +
                                " LEFT JOIN A22_FAB ON CORDSUB.TRN_NO = A22_FAB.CORD_NO AND CORDSUB.SN = A22_FAB.CORD_SN" +
                                " LEFT JOIN item_22 ON CORDSUB.ITEM_NO = item_22.ITEM_NO                                " +
                                " left join CUST on cust.CUST_NO = CORDSUB.CUST_NO                                      " +
                                " WHERE (CORDSUB.TRN_NO+CORDSUB.sn) IN (SELECT A22_FAB.CORD_NO+A22_FAB.CORD_SN FROM A22_FAB AS a22_fab WHERE cordsub.TRN_NO = a22_fab.CORD_NO AND cordsub.SN = a22_fab.CORD_SN)" +
                                "   and item_22.PLINE_NO > 0                                                            " +
                                "   " + 條件式 + "                                                                         " +
                                "   " + 日期起訖 + "                                                                        " +
                                " GROUP BY item_22.PLINE_NO,cust.CUSTNM2";
                return sql_cmd;
            }
            public static string 客戶訂單明細_已排程(string 客戶名稱, string 日期起訖, string 條件式)
            {
                string sql_cmd = "SELECT                                                                                " +
                                " cust.CUSTNM2 as 客戶簡稱,                                                              " +
                                " item_22.PLINE_NO AS 產線代號,                                                          " +
                                "  COUNT(item_22.PLINE_NO) AS 訂單數量,                                                  " +
                                "   LEFT(sum(CORDSUB.AMOUNT), CHARINDEX('.', sum(CORDSUB.AMOUNT)) - 1) AS 訂單金額,      " +
                                "   SUM(CORDSUB.SCOST) AS 成本                                                           " +
                                " FROM CORDSUB                                                                          " +
                                " LEFT JOIN A22_FAB ON CORDSUB.TRN_NO = A22_FAB.CORD_NO AND CORDSUB.SN = A22_FAB.CORD_SN " +
                                " LEFT JOIN item_22 ON CORDSUB.ITEM_NO = item_22.ITEM_NO                                " +
                                " left join CUST on cust.CUST_NO = CORDSUB.CUST_NO                                      " +
                                " WHERE (CORDSUB.TRN_NO+CORDSUB.sn) IN (SELECT A22_FAB.CORD_NO+A22_FAB.CORD_SN FROM A22_FAB AS a22_fab WHERE cordsub.TRN_NO = a22_fab.CORD_NO AND cordsub.SN = a22_fab.CORD_SN)" +
                                "   and item_22.PLINE_NO > 0                                                            " +
                                "   " + 條件式 + "                                                                       " +
                                "   " + 日期起訖 + "                                                                       " +
                                "   and cust.CUSTNM2 = '" + 客戶名稱 + "'                                                  " +
                                " GROUP BY item_22.PLINE_NO,cust.CUSTNM2";
                return sql_cmd;
            }
            public static string 訂單詳細表_已排程(string 客戶名稱, string 日期起訖, string 訂單狀態)
            {
                string sql_cmd = "SELECT cust.CUSTNM2 AS 客戶簡稱,CORDSUB.TRN_NO AS 訂單號碼,mkordsub.ITEM_NO AS CCS,        " +
                                 "mkordsub.LOT_NO AS 製造批號,item_22.PLINE_NO AS 產線代號,cordsub.SCLOSE AS 訂單狀況,A22_FAB.STR_DATE AS 預計開工日," +
                                 "  (SELECT MAX(ITEMIO.TRN_DATE)                                                           " +
                                 "  FROM ITEMIOS                                                                           " +
                                 "   LEFT JOIN ITEMIO ON ITEMIOS.IO = ITEMIO.IO                                            " +
                                 "   AND ITEMIOS.TRN_NO = ITEMIO.TRN_NO                                                    " +
                                 "   WHERE ITEMIOS.IO = '2'                                                                " +
                                 "     AND ITEMIOS.MKORD_NO = MKORDSUB.TRN_NO                                              " +
                                 "     AND ITEMIOS.MKORD_SN = MKORDSUB.SN                                                  " +
                                 "     AND ITEMIO.S_DESC < > '歸還'                                                        " +
                                 "     AND ITEMIO.MK_TYPE = '成品入庫') AS 入庫日, Convert(Varchar(12), CONVERT(MONEY, cordsub.QUANTITY), 1) AS 數量, cord.CURRENCY AS 幣別,Convert(Varchar(20), CONVERT(MONEY, cord.RATE), 1) AS 匯率, Convert(Varchar(20), CONVERT(MONEY, CORDSUB.O_PRICE), 1)AS 外幣單價, Convert(Varchar(20), CONVERT(MONEY, CORDSUB.AMOUNT), 1)AS 台幣單價  " +
                                 "FROM CORDSUB AS cordsub                                                                  " +
                                 "LEFT JOIN A22_FAB ON cordsub.TRN_NO = A22_FAB.CORD_NO AND cordsub.sn = A22_FAB.CORD_SN   " +
                                 "LEFT JOIN MKORDSUB AS mkordsub ON cordsub.TRN_NO = mkordsub.CORD_NO AND cordsub.SN = mkordsub.CORD_SN " +
                                 "LEFT JOIN CUST AS cust ON cust.CUST_NO = CORDSUB.CUST_NO                                 " +
                                 "LEFT JOIN item_22 AS item_22 ON item_22.ITEM_NO = cordsub.ITEM_NO                        " +
                                 "LEFT JOIN CORD AS cord ON cord.TRN_NO = cordsub.TRN_NO                                   " +
                                 " WHERE (CORDSUB.TRN_NO+CORDSUB.sn) IN (SELECT A22_FAB.CORD_NO+A22_FAB.CORD_SN FROM A22_FAB AS a22_fab WHERE cordsub.TRN_NO = a22_fab.CORD_NO AND cordsub.SN = a22_fab.CORD_SN)" +
                                 "   and item_22.PLINE_NO > 0                                                            " +
                                 "   " + 訂單狀態 + "                                                                      " +
                                 "   " + 日期起訖 + "                                                                      " +
                                 "  AND cust.CUSTNM2 = '" + 客戶名稱 + "'";
                return sql_cmd;
            }

            //未排程
            public static string 各產線訂單資料_未排程(string 日期起訖, string 條件式)
            {
                //將未排入組進表的訂單明細資料找出來
                //日期區間 : 未排入組進表,未有預計開工日(STR_DATE)
                //使用訂單明細狀況 > 去組進表找 (SCLOSE!=結案) > 若找無資料 = 有訂單 未上線 未完成

                string sql_cmd = "SELECT " +
                    "item_22.PLINE_NO as 產線代號," +
                    "LEFT(sum(CORDSUB.AMOUNT), CHARINDEX('.', sum(CORDSUB.AMOUNT)) - 1) as 訂單金額," +
                    " count(item_22.PLINE_NO )as 訂單數量, LEFT(sum(CORDSUB.SCOST), CHARINDEX('.', sum(CORDSUB.SCOST)) - 1) as 成本" +
                    " FROM CORDSUB as cordsub " +
                    " LEFT JOIN item_22  AS item_22 ON cordsub.ITEM_NO = item_22.ITEM_NO " +
                    " WHERE (CORDSUB.TRN_NO+CORDSUB.sn) NOT IN (SELECT A22_FAB.CORD_NO+A22_FAB.CORD_SN FROM A22_FAB AS a22_fab WHERE cordsub.TRN_NO = a22_fab.CORD_NO AND cordsub.SN = a22_fab.CORD_SN) " +
                    " AND PLINE_NO > 0 " +
                    " " + 條件式 + " " +
                    " " + 日期起訖 + " " +
                    " group by item_22.PLINE_NO";
                return sql_cmd;

            }
            public static string 客戶列表_未排程(string 日期起訖, string 條件式)
            {
                string sql_cmd = "SELECT                                                                                                           " +
                                " distinct CUST.CUSTNM2 as 客戶簡稱                                                                                " +
                                " FROM CORDSUB                                                                                                    " +
                                " LEFT JOIN item_22 ON cordsub.ITEM_NO = item_22.ITEM_NO                                                          " +
                                " left join CUST on CUST.CUST_NO = CORDSUB.CUST_NO                                                                " +
                                " WHERE (CORDSUB.TRN_NO+CORDSUB.sn) NOT IN (SELECT A22_FAB.CORD_NO+A22_FAB.CORD_SN FROM A22_FAB AS a22_fab WHERE cordsub.TRN_NO = a22_fab.CORD_NO AND cordsub.SN = a22_fab.CORD_SN) " +
                                " AND PLINE_NO > 0                                                                                               " +
                                " " + 條件式 +
                                " " + 日期起訖 + "                                                                                                   " +
                                " GROUP BY CUST.CUSTNM2";
                return sql_cmd;
            }
            public static string 客戶訂單明細_未排程(string 客戶名稱, string 日期起訖, string 條件式)
            {
                string sql_cmd = "SELECT                                                                                                       " +
                                " CUST.CUSTNM2 as 客戶簡稱,                                                                                       " +
                                " item_22.PLINE_NO AS 產線代號,                                                                                   " +
                                " LEFT(sum(CORDSUB.AMOUNT),                                                                                      " +
                                " CHARINDEX('.', sum(CORDSUB.AMOUNT)) - 1) AS 訂單金額,                                                           " +
                                " count(item_22.PLINE_NO)AS 訂單數量,                                                                             " +
                                " sum(SCOST) AS 成本                                                                                             " +
                                " FROM CORDSUB                                                                                                   " +
                                " LEFT JOIN item_22 ON cordsub.ITEM_NO = item_22.ITEM_NO                                                         " +
                                " left join CUST on CUST.CUST_NO = CORDSUB.CUST_NO                                                               " +
                                //" WHERE NOT EXISTS(SELECT * FROM A22_FAB WHERE cordsub.TRN_NO = a22_fab.CORD_NO AND cordsub.SN = a22_fab.CORD_SN) " +
                                " WHERE (CORDSUB.TRN_NO+CORDSUB.sn) NOT IN (SELECT A22_FAB.CORD_NO+A22_FAB.CORD_SN FROM A22_FAB AS a22_fab WHERE cordsub.TRN_NO = a22_fab.CORD_NO AND cordsub.SN = a22_fab.CORD_SN) " +
                                " AND PLINE_NO > 0                                                                                               " +
                                " " + 條件式 +
                                " AND CUST.CUSTNM2 = '" + 客戶名稱 + "'                                                                                           " +
                                " " + 日期起訖 + "                                                                                                   " +
                                " GROUP BY CUST.CUSTNM2 , item_22.PLINE_NO";
                return sql_cmd;
            }
            public static string 訂單詳細表_未排程(string 客戶名稱, string 日期起訖, string 訂單狀態)
            {
                string sql_cmd = "SELECT cust.CUSTNM2 AS 客戶簡稱,CORDSUB.TRN_NO AS 訂單號碼,mkordsub.ITEM_NO AS CCS,        " +
                                 "mkordsub.LOT_NO AS 製造批號,item_22.PLINE_NO AS 產線代號,cordsub.SCLOSE AS 訂單狀況,A22_FAB.STR_DATE AS 預計開工日," +
                                 "  (SELECT MAX(ITEMIO.TRN_DATE)                                                           " +
                                 "  FROM ITEMIOS                                                                           " +
                                 "   LEFT JOIN ITEMIO ON ITEMIOS.IO = ITEMIO.IO                                            " +
                                 "   AND ITEMIOS.TRN_NO = ITEMIO.TRN_NO                                                    " +
                                 " WHERE NOT EXISTS(SELECT * FROM A22_FAB WHERE cordsub.TRN_NO = a22_fab.CORD_NO AND cordsub.SN = a22_fab.CORD_SN) " +
                                 "     AND ITEMIOS.IO = '2'                                                                " +
                                 "     AND ITEMIOS.MKORD_NO = MKORDSUB.TRN_NO                                              " +
                                 "     AND ITEMIOS.MKORD_SN = MKORDSUB.SN                                                  " +
                                 "     AND ITEMIO.S_DESC < > '歸還'                                                        " +
                                 "     AND ITEMIO.MK_TYPE = '成品入庫') AS 入庫日, Convert(Varchar(12), CONVERT(MONEY, cordsub.QUANTITY), 1) AS 數量, cord.CURRENCY AS 幣別,Convert(Varchar(20), CONVERT(MONEY, cord.RATE), 1) AS 匯率, Convert(Varchar(20), CONVERT(MONEY, CORDSUB.O_PRICE), 1)AS 外幣單價, Convert(Varchar(20), CONVERT(MONEY, CORDSUB.AMOUNT), 1)AS 台幣單價 " +
                                 "FROM CORDSUB AS cordsub                                                                  " +
                                 "LEFT JOIN A22_FAB ON cordsub.TRN_NO = A22_FAB.CORD_NO AND cordsub.sn = A22_FAB.CORD_SN   " +
                                 "LEFT JOIN MKORDSUB AS mkordsub ON cordsub.TRN_NO = mkordsub.CORD_NO AND cordsub.SN = mkordsub.CORD_SN " +
                                 "LEFT JOIN CUST AS cust ON cust.CUST_NO = CORDSUB.CUST_NO                                 " +
                                 "LEFT JOIN item_22 AS item_22 ON item_22.ITEM_NO = cordsub.ITEM_NO                        " +
                                 "LEFT JOIN CORD AS cord ON cord.TRN_NO = cordsub.TRN_NO                                   " +
                                " WHERE (CORDSUB.TRN_NO+CORDSUB.sn) NOT IN (SELECT A22_FAB.CORD_NO+A22_FAB.CORD_SN FROM A22_FAB AS a22_fab WHERE cordsub.TRN_NO = a22_fab.CORD_NO AND cordsub.SN = a22_fab.CORD_SN) " +
                                " AND PLINE_NO > 0                                                                                               " +
                                 "   " + 訂單狀態 + "                                                                      " +
                                 "   " + 日期起訖 + "                                                                      " +
                                 "  AND cust.CUSTNM2 = '" + 客戶名稱 + "'";
                return sql_cmd;
            }

            //-----------------------------------------------------------
            public static string 各產線訂單資料_全部(string 日期起, string 日期訖, string 條件式_已結or未結)
            {
                string sql_cmd = "SELECT A.產線代號, SUM(A.訂單數量) AS 訂單數量, SUM(A.訂單金額) AS 訂單金額,SUM(A.成本) AS 成本 FROM (select item_22.PLINE_NO as 產線代號,COUNT(item_22.PLINE_NO) as 訂單數量,LEFT(sum(CORDSUB.AMOUNT), CHARINDEX('.', sum(CORDSUB.AMOUNT)) - 1) as 訂單金額,sum(CORDSUB.SCOST) as 成本 from A22_FAB left join CORDSUB as cordsub on cordsub.TRN_NO=A22_FAB.CORD_NO and cordsub.sn=A22_FAB.CORD_SN left join MKORDSUB as mkordsub on cordsub.TRN_NO=mkordsub.CORD_NO and cordsub.SN=mkordsub.CORD_SN  left join CUST as cust on cust.CUST_NO=CORDSUB.CUST_NO left join item_22 as item_22 on item_22.ITEM_NO=cordsub.ITEM_NO where item_22.PLINE_NO > 0  and A22_FAB.STR_DATE>=20190501 and A22_FAB.STR_DATE<=20190531  and cordsub.SCLOSE !='未結'  group by item_22.PLINE_NO " +
                " UNION select a.產線代號,sum(訂單數量) as 訂單數量,sum(訂單金額) as 訂單金額,sum(a.成本) as 成本 from(SELECT item_22.PLINE_NO as 產線代號 , count(item_22.PLINE_NO) as 訂單數量 , sum(CORDSUB.AMOUNT) as 訂單金額 , sum(CORDSUB.SCOST) as 成本 FROM CORDSUB left join A22_FAB as A22_FAB on cordsub.TRN_NO= A22_FAB.CORD_NO and cordsub.sn= A22_FAB.CORD_SN  left join item_22 on CORDSUB.ITEM_NO = item_22.ITEM_NO  where item_22.PLINE_NO > 0 and cordsub.SCLOSE = '未結'     and A22_FAB.STR_DATE >=20190501 and A22_FAB.STR_DATE <=20190531 group by item_22.PLINE_NO union SELECT item_22.PLINE_NO as 產線代號, count(item_22.PLINE_NO) as 訂單數量, LEFT(sum(CORDSUB.AMOUNT), CHARINDEX('.', sum(CORDSUB.AMOUNT)) - 1) as 訂單金額, sum(SCOST) as 成本 FROM CORDSUB as cordsub LEFT JOIN item_22 AS item_22 ON cordsub.ITEM_NO = item_22.ITEM_NO WHERE NOT EXISTS(SELECT* FROM A22_FAB as a22_fab WHERE cordsub.TRN_NO = a22_fab.CORD_NO AND cordsub.SN = a22_fab.CORD_SN) and cordsub.SCLOSE = '未結' AND PLINE_NO > 0    group by item_22.PLINE_NO) a group by a.產線代號) A GROUP BY A.產線代號";
                return sql_cmd;
            }
            public static string 客戶列表()
            {
                string sql_cmd = "use FJWSQL select distinct cust.CUSTNM2 as 客戶簡稱 from A22_FAB left join CORDSUB as cordsub on cordsub.TRN_NO=A22_FAB.CORD_NO and cordsub.sn=A22_FAB.CORD_SN left join MKORDSUB as mkordsub on cordsub.TRN_NO=mkordsub.CORD_NO and cordsub.SN=mkordsub.CORD_SN  left join CUST as cust on cust.CUST_NO=CORDSUB.CUST_NO left join item_22 as item_22 on item_22.ITEM_NO=cordsub.ITEM_NO where cust.CUSTNM2 is not null";
                return sql_cmd;
            }
            //-----------------------------------------------------------
            public static string 未交易客戶(string 條件式)
            {
                string sql_cmd = "select * from(select cust.CUSTNM2 as 客戶簡稱,max(TRN_DATE) as 最後交易日,DATEDIFF(day,max(TRN_DATE),convert(varchar(10),getdate(),112)) as 未交易天數 from cord left join CUST as cust on cust.CUST_NO=cord.CUST_NO where cust.CUSTNM2 is not null group by cust.CUSTNM2) as A WHERE " + 條件式 + "  order by 未交易天數 desc";
                //string sql_cmd = "SELECT * FROM (select CUSTNM2 as 客戶簡稱,MAX(TRN_DATE) as 最後交易日 from cord left join CUST as cust on cust.CUST_NO=cord.CUST_NO where cust.CUSTNM2 is not null GROUP BY cust.CUSTNM2 ) AS A " + 條件式 +" ORDER BY 最後交易日 DESC";
                return sql_cmd;
            }
        }
        public class 業務部_訂單詳細
        {
            public static string 客戶訂單詳細(string 客戶名稱, string 日期起, string 日期訖, string 條件式)
            {
                string sql_cmd = "use FJWSQL select cust.CUSTNM2 as 客戶簡稱,CORDSUB.TRN_NO as 訂單號碼,mkordsub.ITEM_NO as CCS,mkordsub.LOT_NO as 製造批號,item_22.PLINE_NO as 產線代號,cordsub.SCLOSE as 訂單狀況,A22_FAB.STR_DATE as 預計開工日,(SELECT MAX(ITEMIO.TRN_DATE) FROM ITEMIOS LEFT JOIN ITEMIO ON ITEMIOS.IO=ITEMIO.IO AND ITEMIOS.TRN_NO=ITEMIO.TRN_NO WHERE ITEMIOS.IO='2' AND ITEMIOS.MKORD_NO=MKORDSUB.TRN_NO AND ITEMIOS.MKORD_SN=MKORDSUB.SN AND ITEMIO.S_DESC< >'歸還' AND ITEMIO.MK_TYPE='成品入庫') as 入庫日,Convert(Varchar(12),CONVERT(money,cordsub.QUANTITY),1) as 數量,cord.CURRENCY as 幣別,Convert(Varchar(20),CONVERT(money,cord.RATE),1) as 匯率,Convert(Varchar(20),CONVERT(money,CORDSUB.O_PRICE),1)as 外幣單價,Convert(Varchar(20),CONVERT(money,CORDSUB.AMOUNT),1)as 台幣單價 from A22_FAB left join CORDSUB as cordsub on cordsub.TRN_NO=A22_FAB.CORD_NO and cordsub.sn=A22_FAB.CORD_SN left join MKORDSUB as mkordsub on cordsub.TRN_NO=mkordsub.CORD_NO and cordsub.SN=mkordsub.CORD_SN left join CUST as cust on cust.CUST_NO=CORDSUB.CUST_NO left join item_22 as item_22 on item_22.ITEM_NO=cordsub.ITEM_NO left join CORD as cord on cord.TRN_NO = cordsub.TRN_NO where cust.CUSTNM2 = '" + 客戶名稱 + "' and A22_FAB.STR_DATE>=" + 日期起 + " and A22_FAB.STR_DATE<=" + 日期訖 + " " + 條件式 + " and item_22.PLINE_NO > 0";
                return sql_cmd;

            }
            //20190527
            public static string 客戶訂單詳細表(string 客戶名稱, string 日期起訖, string 條件式, string union_cmd, string NoExists)
            {
                string sql_cmd = "use FJWSQL select cust.CUSTNM2 as 客戶簡稱,CORDSUB.TRN_NO as 訂單號碼,mkordsub.ITEM_NO as CCS,mkordsub.LOT_NO as 製造批號,item_22.PLINE_NO as 產線代號,cordsub.SCLOSE as 訂單狀況,A22_FAB.STR_DATE as 預計開工日,(SELECT MAX(ITEMIO.TRN_DATE) FROM ITEMIOS LEFT JOIN ITEMIO ON ITEMIOS.IO=ITEMIO.IO AND ITEMIOS.TRN_NO=ITEMIO.TRN_NO WHERE ITEMIOS.IO='2' AND ITEMIOS.MKORD_NO=MKORDSUB.TRN_NO AND ITEMIOS.MKORD_SN=MKORDSUB.SN AND ITEMIO.S_DESC< >'歸還' AND ITEMIO.MK_TYPE='成品入庫') as 入庫日,Convert(Varchar(12),CONVERT(money,cordsub.QUANTITY),1) as 數量,cord.CURRENCY as 幣別,Convert(Varchar(20),CONVERT(money,cord.RATE),1) as 匯率,Convert(Varchar(20),CONVERT(money,CORDSUB.O_PRICE),1)as 外幣單價,Convert(Varchar(20),CONVERT(money,CORDSUB.AMOUNT),1)as 台幣單價 from CORDSUB as cordsub left join A22_FAB on cordsub.TRN_NO = A22_FAB.CORD_NO and cordsub.sn = A22_FAB.CORD_SN left join MKORDSUB as mkordsub on cordsub.TRN_NO=mkordsub.CORD_NO and cordsub.SN=mkordsub.CORD_SN left join CUST as cust on cust.CUST_NO=CORDSUB.CUST_NO left join item_22 as item_22 on item_22.ITEM_NO=cordsub.ITEM_NO left join CORD as cord on cord.TRN_NO = cordsub.TRN_NO where " + NoExists + 日期起訖 + " and cust.CUSTNM2 = '" + 客戶名稱 + "' and item_22.PLINE_NO > 0" + 條件式 + union_cmd;
                return sql_cmd;

            }
        }
        public class 業務部_運輸未歸還統計
        {
            public static string 異常運輸架品號列表(string 今日, string acc = "")
            {
                acc = acc == "visrd" || acc == "detawink" || acc == "" ? " or INVOSUB.ITEM_NO='MR4CM00ZZC07' " : "";
                return $"use FJWSQL select distinct INVOSUB.ITEM_NO as 運輸架品號 from INVOSUB left join CUST as cust on cust.CUST_NO=INVOSUB.cust_no where invosub.trn_date <= {今日} and INVOSUB.CORD_NO='' and (INVOSUB.ITEM_NO='MR4A-00ZZA08' or INVOSUB.ITEM_NO='MR5D-00ZZA06' or INVOSUB.ITEM_NO='MC4A-00MMA04' or INVOSUB.ITEM_NO='MC5A-00ZZB05' or INVOSUB.ITEM_NO='MC4BQ-0MMB04' {acc}) GROUP BY INVOSUB.ITEM_NO";
            }
            public static string 正常運輸架品號列表(string 今日,string acc="")
            {
                acc = acc == "visrd" || acc == "detawink" || acc == "" ? " or INVOSUB.ITEM_NO='MR4CM00ZZC07' " : "";
                return $"use FJWSQL select distinct INVOSUB.ITEM_NO as 運輸架品號 from INVOSUB where invosub.trn_date <= {今日} and INVOSUB.CORD_NO='' and (INVOSUB.ITEM_NO='MR4A-00ZZA08' or INVOSUB.ITEM_NO='MR5D-00ZZA06' or INVOSUB.ITEM_NO='MC4A-00MMA04' or INVOSUB.ITEM_NO='MC5A-00ZZB05' or INVOSUB.ITEM_NO='MC4BQ-0MMB04' {acc}) GROUP BY INVOSUB.ITEM_NO";
            }
            public static string 正常運輸架數量(string 運輸架品號, string 今日, string acc = "")
            {
                acc = acc == "visrd" || acc == "detawink" || acc == "" ? " or INVOSUB.ITEM_NO='MR4CM00ZZC07' " : "";
                return $"use FJWSQL select INVOSUB.ITEM_NO as 運輸架品號, sum(INVOSUB.QUANTITY) as 數量 from INVOSUB where invosub.trn_date <= {今日} and INVOSUB.CORD_NO='' and (INVOSUB.ITEM_NO='MR4A-00ZZA08' or INVOSUB.ITEM_NO='MR5D-00ZZA06' or INVOSUB.ITEM_NO='MC4A-00MMA04' or INVOSUB.ITEM_NO='MC5A-00ZZB05' or INVOSUB.ITEM_NO='MC4BQ-0MMB04' {acc}) and INVOSUB.ITEM_NO = '{運輸架品號}' GROUP BY INVOSUB.ITEM_NO HAVING SUM(INVOSUB.QUANTITY) > 0 order by sum(INVOSUB.QUANTITY) desc";
                
            }
            public static string 異常運輸架數量(string 運輸架品號, string 今日, string acc = "")
            {
                acc = acc == "visrd" || acc == "detawink" || acc == "" ? " or INVOSUB.ITEM_NO='MR4CM00ZZC07' " : "";
                return $"use FJWSQL select INVOSUB.ITEM_NO as 運輸架品號,sum(INVOSUB.QUANTITY) as 數量 from INVOSUB left join CUST as cust on cust.CUST_NO=INVOSUB.cust_no where invosub.trn_date <= {今日}  and INVOSUB.CORD_NO='' and (INVOSUB.ITEM_NO='MR4A-00ZZA08' or INVOSUB.ITEM_NO='MR5D-00ZZA06' or INVOSUB.ITEM_NO='MC4A-00MMA04' or INVOSUB.ITEM_NO='MC5A-00ZZB05' or INVOSUB.ITEM_NO='MC4BQ-0MMB04' {acc}) and INVOSUB.ITEM_NO ='{運輸架品號}'  GROUP BY cust.custnm2,INVOSUB.ITEM_NO HAVING SUM(INVOSUB.QUANTITY) < 0";
                 
            }
            public static string 在外運輸架總數(string 今日, string acc = "")
            {
                acc = acc == "visrd" || acc == "detawink" || acc == "" ? " or INVOSUB.ITEM_NO='MR4CM00ZZC07' " : "";
                return $"use FJWSQL select sum(INVOSUB.QUANTITY) as 總數量 from INVOSUB where invosub.trn_date <= {今日} and INVOSUB.CORD_NO = '' and(INVOSUB.ITEM_NO = 'MR4A-00ZZA08' or INVOSUB.ITEM_NO = 'MR5D-00ZZA06' or INVOSUB.ITEM_NO = 'MC4A-00MMA04' or INVOSUB.ITEM_NO = 'MC5A-00ZZB05' or INVOSUB.ITEM_NO = 'MC4BQ-0MMB04' {acc})";
           
            }
            public static string 表格欄位名稱(string 今日, string acc = "")
            {
                acc = acc == "visrd" || acc == "detawink" || acc == "" ? " or INVOSUB.ITEM_NO='MR4CM00ZZC07' " : "";
                return $"use FJWSQL select distinct INVOSUB.ITEM_NO as 運輸架品號 from INVOSUB where invosub.trn_date <= {今日} and INVOSUB.CORD_NO = '' and(INVOSUB.ITEM_NO = 'MR4A-00ZZA08' or INVOSUB.ITEM_NO = 'MR5D-00ZZA06' or INVOSUB.ITEM_NO = 'MC4A-00MMA04' or INVOSUB.ITEM_NO = 'MC5A-00ZZB05' or INVOSUB.ITEM_NO = 'MC4BQ-0MMB04' {acc}) GROUP BY INVOSUB.ITEM_NO order by INVOSUB.ITEM_NO ASC";
                 
            }
            public static string 未歸還運輸架客戶列表(string 今日, string acc = "")
            {
                acc = acc == "visrd" || acc == "detawink" || acc == "" ? " or INVOSUB.ITEM_NO='MR4CM00ZZC07' " : "";
                return  $"use FJWSQL select distinct cust.custnm2 as 客戶簡稱 from INVOSUB left join CUST as cust on cust.CUST_NO=INVOSUB.cust_no where invosub.trn_date <= {今日} and INVOSUB.CORD_NO='' and (INVOSUB.ITEM_NO='MR4A-00ZZA08' or INVOSUB.ITEM_NO='MR5D-00ZZA06' or INVOSUB.ITEM_NO='MC4A-00MMA04' or INVOSUB.ITEM_NO='MC5A-00ZZB05' or INVOSUB.ITEM_NO='MC4BQ-0MMB04' {acc}) GROUP BY cust.custnm2,INVOSUB.ITEM_NO ";
               
            }
            public static string 未歸還運輸架客戶詳細(string 客戶名稱, string 今日, string acc = "")
            {
                acc = acc == "visrd" || acc == "detawink" || acc == "" ? " or INVOSUB.ITEM_NO='MR4CM00ZZC07' " : "";
                return $"use FJWSQL select cust.custnm2 as 客戶簡稱,INVOSUB.ITEM_NO as 運輸架品號,sum(INVOSUB.QUANTITY) as 數量 from INVOSUB left join CUST as cust on cust.CUST_NO=INVOSUB.cust_no where invosub.trn_date <={今日} and INVOSUB.CORD_NO='' and (INVOSUB.ITEM_NO='MR4A-00ZZA08' or INVOSUB.ITEM_NO='MR5D-00ZZA06' or INVOSUB.ITEM_NO='MC4A-00MMA04' or INVOSUB.ITEM_NO='MC5A-00ZZB05' or INVOSUB.ITEM_NO='MC4BQ-0MMB04' {acc}) and cust.custnm2 = '{客戶名稱}' GROUP BY cust.custnm2,INVOSUB.ITEM_NO order by sum(INVOSUB.QUANTITY) ASC";
            }

        }

        public class 資材部_供應商達交率
        {
            public static string 供應商達交率(string 日期起, string 日期訖)
            {
                string sqlcmd = "select * from ( select A.供應商,COUNT(A.供應商) AS 採購次數, convert(numeric(17,0),sum (a.期限內已交數量))  as 期限內已交總數量, convert(numeric(17,0),sum (a.採購數量)) as 採購總數量, convert(numeric(17,2),convert(float,round(((convert(numeric(17,2),sum (a.期限內已交數量)))/convert(numeric(17,2),sum (a.採購數量)))*100,2))) as 達交率 from  ( SELECT SORD.TRN_NO as 採購單號 ,SORDSUB.SN as 採購明細序 ,FACT.FACTNM2 as 供應商 ,SORDSUB.ITEM_NO as 品號 ,SORDSUB.NAME as 品名規格 ,SORDSUB.QUANTITY as 採購數量 ,PURCSUB.Q_DELIED as 期限內已交數量 ,SORD.TRN_DATE as 採購單日期 ,SORDSUB.D1 as 預交日期 ,PURCSUB.TRN_DATE as 期限內最後進貨日期 FROM SORD left join SORDSUB on SORD.TRN_NO = SORDSUB.TRN_NO left join PURCSUB on PURCSUB.SORD_NO = SORD.TRN_NO left join FACT on fact.FACT_NO = SORD.FACT_NO  where SORDSUB.D1 >= " + 日期起 + "  and SORDSUB.D1 <= " + 日期訖 + "  and PURCSUB.SORD_SN = SORDSUB.SN  and SORDSUB.D1 >= PURCSUB.TRN_DATE ) as A group by a.供應商  ) AS B order by b.達交率    ";
                return sqlcmd;
            }
        }
        public class 資材部_供應商達交率明細表
        {
            public static string 供應商交貨明細表(string 供應商, string 日期起, string 日期訖)
            {
                string sqlcmd = "SELECT sord.trn_no  AS 採購單號, sordsub.sn  AS 採購明細序, fact.factnm2  AS 供應商名稱, sordsub.item_no AS 品號, sordsub.NAME AS 品名規格, sordsub.quantity AS 採購數量, purcsub.q_delied AS 期限內已交數量, sord.trn_date AS 採購單日期, sordsub.d1 AS 預交日期, purcsub.trn_date AS 期限內最後進貨日期, CONVERT(NUMERIC(17, 2), CONVERT(FLOAT, Round(( ( CONVERT(NUMERIC(17, 2), purcsub.q_delied) ) / CONVERT(NUMERIC(17, 2), sordsub.quantity) ) * 100, 2))) AS 達交率 FROM sord LEFT JOIN sordsub ON sord.trn_no = sordsub.trn_no LEFT JOIN purcsub ON purcsub.sord_no = sord.trn_no LEFT JOIN fact ON fact.fact_no = sord.fact_no WHERE  sordsub.d1 >= " + 日期起 + " AND sordsub.d1 <= " + 日期訖 + " AND purcsub.sord_sn = sordsub.sn AND sordsub.d1 >= purcsub.trn_date AND fact.factnm2 = '" + 供應商 + "'";
                return sqlcmd;
            }
        }
        public class 資材部_物料領用統計
        {
            public static string 用途說明(string 種類, string 條件式, string 日期起, string 日期訖)
            {
                string sql_cmd = "use FJWSQL select distinct(CASE WHEN CHARINDEX('(', a.用途說明 COLLATE Latin1_General_CS_AS) > 0 THEN SUBSTRING(a.用途說明, 0, CHARINDEX('(', a.用途說明)) ELSE a.用途說明 END) as 用途說明 from(select distinct ITEMIOS.S_DESC as 用途說明 from ITEMIOS left join item as item on ITEMIOS.ITEM_NO = item.ITEM_NO where " + 條件式 + " like '%" + 種類 + "%' and TRN_DATE >= " + 日期起 + " and TRN_DATE <= " + 日期訖 + " group by ITEMIOS.ITEM_NO, item.ITEMNM, ITEMIOS.S_DESC having sum(itemios.QTY2) > 0) as a";
                return sql_cmd;
            }
            public static string 品名總領料列表(string 種類, string 搜尋式, string 條件式, string 日期起, string 日期訖, int 位數)
            {
                string 品號擷取位數 = "";
                if (搜尋式 == "內含")
                {
                    搜尋式 = "like '%" + 種類 + "%'";
                }
                else if (搜尋式 == "等於")
                {
                    搜尋式 = " =  '" + 種類 + "'";
                }

                if (位數 > -1)
                {
                    品號擷取位數 = "SUBSTRING(a.品號,0," + 位數 + ")";
                }
                else if (位數 <= -1)
                {
                    品號擷取位數 = "a.品號";
                }
                //string s1ql_cmd = "use FJWSQL select distinct ITEMIOS.ITEM_NO as 品號,item.ITEMNM as 品名規格,sum(itemios.QTY2) as 總領料數  from ITEMIOS left join item as item on ITEMIOS.ITEM_NO=item.ITEM_NO where " + 條件式 + " "+ 搜尋式 + " and TRN_DATE>=" + 日期起 + " and TRN_DATE<=" + 日期訖 + "  group by ITEMIOS.ITEM_NO,item.ITEMNM having sum(itemios.QTY2) > 0 order by ITEMIOS.ITEM_NO asc";
                //原本的
                string s1ql_cmd = "select " + 品號擷取位數 + " as 品號,a.品名規格,sum(a.總領料數) as 總領料數 from (select distinct ITEMIOS.ITEM_NO as 品號, item.ITEMNM as 品名規格, sum(itemios.QTY2) as 總領料數 from ITEMIOS left join item as item on ITEMIOS.ITEM_NO=item.ITEM_NO where " + 條件式 + " " + 搜尋式 + " and TRN_DATE>=" + 日期起 + " and TRN_DATE<=" + 日期訖 + " group by ITEMIOS.ITEM_NO,item.ITEMNM having sum(itemios.QTY2) > 0 ) as a group by " + 品號擷取位數 + ",a.品名規格 ORDER BY " + 品號擷取位數 + " desc";
                //20191024新增客戶簡稱與數量
                //string s1ql_cmd = "select " + 品號擷取位數 + " as 品號,a.品名規格,sum(a.總領料數) as 總領料數, a.客戶簡稱及數量  AS 客戶簡稱及數量 from (select distinct ITEMIOS.ITEM_NO as 品號, item.ITEMNM as 品名規格, sum(itemios.QTY2) as 總領料數,CUST.CUSTNM2  As 客戶簡稱及數量  from ITEMIOS left join item as item on ITEMIOS.ITEM_NO=item.ITEM_NO LEFT JOIN CUST AS CUST  ON itemios.CUST_NO = CUST.CUST_NO  where " + 條件式 + " " + 搜尋式 + " and TRN_DATE>=" + 日期起 + " and TRN_DATE<=" + 日期訖 + " group by ITEMIOS.ITEM_NO,item.ITEMNM,CUST.CUSTNM2 having sum(itemios.QTY2) > 0 ) as a group by " + 品號擷取位數 + ",a.品名規格,a.客戶簡稱及數量 ORDER BY " + 品號擷取位數 + " desc";
                return s1ql_cmd;
            }
            //20191024新增查詢品項與客戶
            public static string 品號領料細項(string 品號, string 日期起, string 日期訖)
            {
                string sql_cmd = "use FJWSQL select a.品號,a.品名規格,a.領料數,a.用途說明 from (select ITEMIOS.ITEM_NO as 品號,item.ITEMNM as 品名規格,sum(itemios.QTY2) as 領料數,(CASE WHEN CHARINDEX ( '(',ITEMIOS.S_DESC COLLATE Latin1_General_CS_AS) > 0 THEN SUBSTRING( ITEMIOS.S_DESC,0,CHARINDEX('(',ITEMIOS.S_DESC)) ELSE ITEMIOS.S_DESC END ) as 用途說明 from ITEMIOS left join item as item on ITEMIOS.ITEM_NO=item.ITEM_NO where ITEMIOS.ITEM_NO = '" + 品號 + "' and TRN_DATE>=" + 日期起 + " and TRN_DATE<= " + 日期訖 + " group by ITEMIOS.ITEM_NO,item.ITEMNM,(CASE WHEN CHARINDEX ( '(',ITEMIOS.S_DESC COLLATE Latin1_General_CS_AS) > 0 THEN SUBSTRING( ITEMIOS.S_DESC,0,CHARINDEX('(',ITEMIOS.S_DESC)) ELSE ITEMIOS.S_DESC END ) having sum(itemios.QTY2) > 0 ) as a order by a.品號 asc";
                return sql_cmd;
            }
        }
        public class 資材部_物料領用統計詳細
        {
            // 2019/07/03，領料明細表
            public static string 用途說明_明細表(string 品號, string 日期起, string 日期訖)
            {
                string sqlcmd = "use FJWSQL select distinct a.用途說明 from (select (CASE WHEN CHARINDEX ( '(',ITEMIOS.S_DESC COLLATE Latin1_General_CS_AS) > 0 THEN SUBSTRING( ITEMIOS.S_DESC,0,CHARINDEX('(',ITEMIOS.S_DESC)) ELSE ITEMIOS.S_DESC END ) as 用途說明 ,sum(itemios.QTY2) as 領料數 from ITEMIOS left join item as item on ITEMIOS.ITEM_NO=item.ITEM_NO where ITEMIOS.ITEM_NO = '" + 品號 + "' and TRN_DATE>'" + 日期起 + "'and TRN_DATE<'" + 日期訖 + "' group by ITEMIOS.ITEM_NO,item.ITEMNM,ITEMIOS.S_DESC,TRN_DATE) as a group by a.領料數,a.用途說明 HAVING SUM(a.領料數) > 0";
                return sqlcmd;
            }
            public static string 領料月份(string 品號, string 日期起, string 日期訖)
            {
                string sqlcmd = "use FJWSQL select a.領料月份 from (select SUBSTRING(TRN_DATE,0,7) as 領料月份 , ITEMIOS.ITEM_NO as 品號,item.ITEMNM as 品名規格 from ITEMIOS left join item as item on ITEMIOS.ITEM_NO=item.ITEM_NO where ITEMIOS.ITEM_NO = '" + 品號 + "' and TRN_DATE>'" + 日期起 + "'and TRN_DATE<'" + 日期訖 + "' group by ITEMIOS.ITEM_NO,item.ITEMNM,ITEMIOS.S_DESC,TRN_DATE) as a group by a.品號,a.品名規格,a.領料月份 order by a.領料月份 ";
                return sqlcmd;
            }
            public static string 每月物料領用數量(string 品號, string 日期起, string 日期訖, string 用途說明, string 領料月份)
            {
                string sqlcmd = "use FJWSQL select a.領料月份,sum(a.領料數) as 領料數 ,a.用途說明 from (select SUBSTRING(TRN_DATE,0,7) as 領料月份 , ITEMIOS.ITEM_NO as 品號,item.ITEMNM as 品名規格,sum(itemios.QTY2) as 領料數,(CASE WHEN CHARINDEX ( '(',ITEMIOS.S_DESC COLLATE Latin1_General_CS_AS) > 0 THEN SUBSTRING( ITEMIOS.S_DESC,0,CHARINDEX('(',ITEMIOS.S_DESC)) ELSE ITEMIOS.S_DESC END ) as 用途說明 from ITEMIOS left join item as item on ITEMIOS.ITEM_NO=item.ITEM_NO where ITEMIOS.ITEM_NO = '" + 品號 + "' and TRN_DATE>'" + 日期起 + "'and TRN_DATE<'" + 日期訖 + "' group by ITEMIOS.ITEM_NO,item.ITEMNM,ITEMIOS.S_DESC,TRN_DATE) as a where a.用途說明 ='" + 用途說明 + "' and a.領料月份 ='" + 領料月份 + "'  group by a.品號,a.品名規格,a.領料月份,a.用途說明 HAVING SUM(a.領料數) > 0 order by a.領料月份 ";
                return sqlcmd;
            }
            public static string 品號領料紀錄(string 品號, string 日期起, string 日期訖)
            {
                string sql_cmd = "use FJWSQL select ITEMIOS.TRN_NO as 領料單號, ITEMIOS.TRN_date as 領料單日期, ITEMIOS.item_no as 領料單明細品號, ITEM.itemnm as 領料單明細品名規格, itemios.qty2 as 領料數量, (CASE WHEN CHARINDEX( '(',ITEMIOS.S_DESC COLLATE Latin1_General_CS_AS) > 0 THEN SUBSTRING(ITEMIOS.S_DESC,0,CHARINDEX('(', ITEMIOS.S_DESC)) ELSE ITEMIOS.S_DESC END ) as 用途說明, itemios.mkord_no as 製令單號,(select top 1 mkordsub.ITEM_NO from mkordsub where ITEMIOS.MKORD_NO = mkordsub.TRN_NO) as 製令明細品號,(select item.ITEMNM from item where item.ITEM_NO = (select top 1 mkordsub.ITEM_NO from mkordsub where ITEMIOS.MKORD_NO = mkordsub.TRN_NO)) as 製令明細品名規格, CUST.CUSTNM2 AS 使用客戶  from ITEMIOS       left join item as item on ITEMIOS.ITEM_NO = item.ITEM_NO 	   LEFT JOIN CUST AS CUST          ON itemios.CUST_NO = CUST.CUST_NO  where ITEMIOS.ITEM_NO  like '" + 品號 + "%' and TRN_DATE>= " + 日期起 + " and TRN_DATE<= " + 日期訖 + " and itemios.qty2 >0 order by CUST.CUSTNM2";
                return sql_cmd;
            }
            public static string 產生領料單表頭(string Number)
            {
                string condition = $"  ( itemio.LOT_NO='{Number}' OR itemio.mkord_no='{Number}' ) ";

                return $"select itemio.trn_no as 領料單號,itemio.trn_date as 領料日期,(select name from employ where itemio.user_code=employ.emp_no) as 領料人,itemio.mkord_no as 製令單號,(select cord_no from mkord where mkord.trn_no=itemio.mkord_no) as 訂單號碼, itemio.s_desc as 用途說明,(select name from employ where ITEMIO.STOCK=employ.emp_no) as 倉管,itemio.LOT_NO as 製造批號,(select CUST.CUSTNM2 from CUST where CUST.cust_no=(select cust_no from mkord where mkord.trn_no=itemio.mkord_no)) as 客戶簡稱, itemio.MITEM_NO as 產品組件, cast(itemio.BOM_QTY as numeric(20,2)) as 領料台數 from itemio where IO='1' and {condition} ";
            }
            public static string 產生領料表內容(string Number)
            {
                List<string> list = new List<string>(Number.Split('#'));
                string condition = "";
                bool ok = true;
                for (int i = 0; i < list.Count - 1; i++)
                {
                    if (list[i] != "")
                    {
                        condition += ok ? $" itemio.LOT_NO='{list[i]}' OR itemio.mkord_no='{list[i]}' " : $" OR itemio.LOT_NO='{list[i]}' OR itemio.mkord_no='{list[i]}' ";
                        ok = false;
                    }
                }
                condition = $" and ( {condition} ) ";
                return $"select a.品號,a.品名規格,a.倉位,a.單位,min(a.現有庫存) 現有庫存,sum(a.申請數量) 申請數量,sum(a.領料數量) 領料數量,sum(a.不足數量) 不足數量,a.狀況 from  ( SELECT itemios.item_no AS 品號, (SELECT itemnm FROM item WHERE item.item_no=itemios.item_no) AS 品名規格,  itemios.PLACE AS 倉位,  itemios.unit AS 單位,  cast(itemios.PQTY_H AS numeric(20, 2)) AS 現有庫存,  cast(itemios.QTY1 AS numeric(20, 2)) AS 申請數量,  cast(itemios.QTY2 AS numeric(20, 2)) AS 領料數量,  cast(itemios.QTYNG AS numeric(20, 2)) AS 不足數量, (SELECT SCLOSE FROM mkordis WHERE mkordis.trn_no=itemios.mkord_no AND mkordis.item_no=itemios.item_no) AS 狀況 FROM itemios LEFT JOIN itemio ON itemios.trn_no = itemio.trn_no WHERE itemio.IO='1' {condition} ) a group by a.品號,a.品名規格,a.倉位,a.單位,a.狀況";
            }
        }
        public class 資材部_供應商催料總表
        {
            public static string 最新催料表日期()
            {
                string sqlcmd = "select max(URGE_ITEM.TRN_DATE) as 催料表日期 from urge_item "; //20190710
                return sqlcmd;
            }
            public static string 催料表清單_依採購單(string supplier, string supplierName, string searchDate, string dt_st, string dt_ed, string itemNo, string 催料日期_MP)
            {
                string 日期條件 = "";
                string 品號條件 = "";
                string 供應商代碼 = "";
                string 供應商簡稱 = "";
                if (dt_st != "") 日期條件 = "AND d_date BETWEEN " + dt_st + " AND " + dt_ed + "";
                if (itemNo != "") 品號條件 = "AND item_no LIKE '%" + itemNo + "%'";
                if (supplier != "") 供應商代碼 = "AND URGE_ITEM.FACT_NO = '" + supplier + "'";
                if (supplierName != "") 供應商簡稱 = "AND factnm2 like '%" + supplierName + "%'";

                string sqlcmd = "  select URGE_ITEM.trn_no as 催料單號,                                         " +
                                "         ORIGIN_TRN_NO as 採購單號,                                            " +
                                "         fjwsql.dbo.SORD.TRN_DATE as 開單日期,                                 " +
                                "         URGE_ITEM.FACT_NO as 廠商,                                            " +
                                "         factnm2 AS 廠商簡稱,                                                  " +
                                "         ITEM_NO as 品號,                                                      " +
                                "         itemnm as 品名規格,                                                    " +
                                "         D_DATE as 催料預交日,                                                  " +
                                "         LEFT(sum(quantity), CHARINDEX('.', sum(quantity)) - 1) as 催料數量,    " +
                                "         '' as 未交量                                                           " +
                                "  from URGE_ITEM                                                               " +
                                "  LEFT JOIN fjwsql.dbo.fact ON URGE_ITEM.fact_no = fact.fact_no                " +
                                "  LEFT JOIN fjwsql.dbo.SORD ON urge_item.ORIGIN_TRN_NO = sord.TRN_NO           " +
                                "  where URGE_ITEM.TRN_DATE =" + 催料日期_MP + "   " +
                                "    " + 供應商代碼 + " " +
                                "    " + 供應商簡稱 + " " +
                                "    " + 日期條件 + " " +
                                "    " + 品號條件 + " " +

                                "    group by ORIGIN_TRN_NO, URGE_ITEM.FACT_NO,ITEM_NO,itemnm,D_DATE,factnm2,fjwsql.dbo.SORD.TRN_DATE,URGE_ITEM.trn_no  " +
                                "    order by ITEM_NO asc, D_DATE asc                                         ";
                return sqlcmd;
            }
            public static string 催料表清單_依收貨單(string supplier, string supplierName, string searchDateId, string 催料日期_RC)
            {
                string 供應商代碼 = "";
                string 供應商簡稱 = "";
                
                if (supplier != "") 供應商代碼 = "AND urge_item_b2b.FACT_NO = '" + supplier + "'";
               
                if (supplierName != "") 供應商簡稱 = "AND factnm2 like '%" + supplierName + "%'";
                
                string sqlcmd = "select  urge_trn_no,                                                  " +
                                "        urge_item_b2b.FACT_NO as 廠商,                                " +
                                "        ITEM_NO as 品號,                                              " +
                                "        sum(in_qty) as 收貨量                                 " +
                                " from urge_item_b2b                                                  " +
                                " LEFT JOIN fjwsql.dbo.fact ON urge_item_b2b.fact_no = fact.fact_no   " +
                                " where urge_trn_no = (select max(urge_trn_no) from urge_item_b2b)    " +
                                "       and urge_trn_no = '" + 催料日期_RC + "' " +
                                "    " + 供應商代碼 + " " +
                                "    " + 供應商簡稱 + " " +
                                " group by urge_item_b2b.FACT_NO, ITEM_NO, urge_trn_no                 " +
                                " having Sum(in_qty) is not null                               ";
                return sqlcmd;
                //" and (ITEM_NO = 'MC4BV0004B01-C1' or  ITEM_NO = 'C4FQJV003A05-C1') " +
            }
        }

        public class 倉管部_呆滯物料統計表
        {
            public static string 取得儲位列表(string 物料類別, string 條件式_期限日)
            {
                //string sql_cmd = "select distinct ITEM_GD.GDNO as 儲位  from ( select distinct ITEM.item_no as 品號, MAX(itemios.trn_date) as 最後領料日  from ITEM  left join itemios on itemios.ITEM_NO = ITEM.ITEM_NO  where  ITEM.PQTY_H > 0  and ITEM.CLASS  = '"+物料類別+"'  and ITEM.COQTY  = 0   and ITEM.ISQTY  = 0  and ITEM.SMQTY >= 0 and ITEM.SOQTY >= 0  group by ITEM.ITEM_NO,ITEM.ITEMNM,ITEM.PQTY_H,ITEM.PLACE ) a  left join ITEM_GD on ITEM_GD.ITEM_NO = a.品號  where (a.最後領料日 <= "+ 條件式_期限日 + " or a.最後領料日 is null) and ITEM_GD.QTY > 0   ";
                string sql_cmd = "SELECT distinct GDNO as 儲位 FROM ITEM_GD";
                return sql_cmd;

            }
            public static string 物料類別列表()
            {
                string sql_cmd = "SELECT CLASS,C_NAME FROM CLASS";
                return sql_cmd;

            }
            public static string 呆滯物料列表(string 物料類別, string 條件式_期限日, string 包括的儲位, CheckBoxList cbx)
            {
                //--ITEM_GD.GDNO = '總倉' OR ITEM_GD.GDNO = '售服倉'  
                //待出貨倉            ,售服倉              ,備料倉,發料倉              ,總倉                ,           
                string 儲位條件 = "";
                List<string> list = new List<string>(包括的儲位.Split(','));
                string condition = "";
                for (int i = 0; i < list.Count - 1; i++)
                {
                    儲位條件 += $"ITEM_GD.GDNO='{list[i]}'";
                    if (i < list.Count - 2)
                        儲位條件 += " OR ";
                }

                for (int i = 0; i < cbx.Items.Count; i++)
                {
                    if (cbx.Items[i].Selected)
                    {
                        if (condition == "")
                            condition += $" ITEM.CLASS = '{cbx.Items[i].Value}' ";
                        else
                            condition += $" OR ITEM.CLASS = '{cbx.Items[i].Value}' ";

                    }

                }
                if (condition != "")
                    condition = $" AND ({condition}) ";


                //string sql_cmd = "select(CASE WHEN a.最後領料日 is null THEN '---' ELSE a.最後領料日 END) as 最後領料日,  a.品號,  a.品名規格,  a.倉位  /*a.儲位 */  from(select distinct ITEM.item_no as 品號,  ITEM.ITEMNM as 品名規格,  ITEM.PQTY_H as 庫存數,  ITEM.PLACE as 倉位,  MAX(itemios.trn_date) as 最後領料日,  ITEM_GD.GDNO as 儲位  from ITEM left join itemios on itemios.ITEM_NO = ITEM.ITEM_NO  left join ITEM_GD on ITEM_GD.ITEM_NO = ITEM.ITEM_NO  where ITEM.PQTY_H > 0   and ITEM.CLASS = '"+ 物料類別 + "'  and ITEM.COQTY = 0  and ITEM.ISQTY = 0  and ITEM.SMQTY >= 0  and ITEM.SOQTY >= 0  and ITEM_GD.QTY > 0  and("+ 儲位條件 + ")  group by ITEM.ITEM_NO,ITEM.ITEMNM,ITEM.PQTY_H,ITEM.PLACE,ITEM_GD.GDNO) a where(a.最後領料日 <= "+ 條件式_期限日 + " or a.最後領料日 is null)";
                //錯誤的
                //  string sql_cmd = $"	select ISNULL(b.最後領料日,'---') as 最後領料日,a.品號,b.品名規格,b.倉位,ISNULL(a.領用數量,0) as 領用數量 ,ISNULL(a.領用比例,0) as 領用比例,b.標準成本 from	(SELECT item.item_no AS 品號,	(SELECT sum(ITEMIOS.QTY2)	FROM itemios	WHERE itemios.item_no=item.item_no	AND itemios.io='1'	AND itemios.trn_date>'{條件式_期限日}'	AND itemios.trn_date<'{DateTime.Now.AddDays(1).ToString("yyyyMMdd")}'	GROUP BY itemios.item_no)/(	(SELECT sum(ITEMIOS.QTY2)	FROM itemios	WHERE itemios.item_no=item.item_no	AND itemios.io='1'	AND itemios.trn_date>'{條件式_期限日}'	AND itemios.trn_date<'{DateTime.Now.AddDays(1).ToString("yyyyMMdd")}'	GROUP BY itemios.item_no)+ITEM.PQTY_H) AS 領用比例, ITEM.PQTY_H AS 庫存數量,	(SELECT sum(ITEMIOS.QTY2)	FROM itemios	WHERE itemios.item_no=item.item_no	AND itemios.io='1'	AND itemios.trn_date>'{條件式_期限日}'	AND itemios.trn_date<'{DateTime.Now.AddDays(1).ToString("yyyyMMdd")}'	GROUP BY itemios.item_no) AS 領用數量	FROM item	LEFT JOIN ITEM_GD ON ITEM_GD.ITEM_NO = ITEM.ITEM_NO	WHERE ITEM.PQTY_H>0    	AND ITEM.CLASS = '{物料類別}'	AND ITEM.COQTY = 0	AND ITEM.ISQTY = 0	AND ITEM.SMQTY >= 0	AND ITEM.SOQTY >= 0	AND ITEM_GD.QTY > 0	AND ({儲位條件})) as a	left join 	(SELECT DISTINCT ITEM.item_no AS 品號, ITEM.ITEMNM AS 品名規格, ITEM.PQTY_H AS 庫存數, ITEM.PLACE AS 倉位, MAX(itemios.trn_date) AS 最後領料日, ITEM_GD.GDNO AS 儲位 , ITEM.SCOST AS 標準成本	FROM ITEM	LEFT JOIN itemios ON itemios.ITEM_NO = ITEM.ITEM_NO	LEFT JOIN ITEM_GD ON ITEM_GD.ITEM_NO = ITEM.ITEM_NO	Left JOIN CLASS on CLASS.CLASS = ITEM.CLASS 	WHERE ITEM.PQTY_H > 0	AND ITEM.CLASS = '{物料類別}'	AND ITEM.COQTY = 0	AND ITEM.ISQTY = 0	AND ITEM.SMQTY >= 0	AND ITEM.SOQTY >= 0	AND ITEM_GD.QTY > 0	AND ({儲位條件})	GROUP BY ITEM.ITEM_NO,	ITEM.ITEMNM,	ITEM.PQTY_H,	ITEM.PLACE,	ITEM_GD.GDNO,	ITEM.SCOST	)as b on a.品號 = b.品號 and a.庫存數量 = b.庫存數	where b.最後領料日 <'{條件式_期限日}' OR b.最後領料日 is null	order by b.最後領料日 asc";
                //正確的
                string sql_cmd = $" use fjwsql select isnull(b.最後領料日,'---') AS 最後領料日,a.品號,b.品名規格,b.倉位,b.類別,a.領用數量,a.銷貨數量,a.入庫數量,a.庫存數量 ,((a.領用數量+a.銷貨數量)/(a.庫存數量+a.領用數量+a.銷貨數量-a.入庫數量)) as 領用比例 ,b.儲位,b.標準成本 , b.標準成本 * a.庫存數量 AS 總庫存成本 from (select item.item_no as 品號, ISNULL( ITEM.PQTY_H,0) as 庫存數量, ISNULL((select sum(ITEMIOS.QTY2) from itemios where itemios.item_no=item.item_no and itemios.io=1 and itemios.trn_date>{條件式_期限日} and itemios.trn_date<{DateTime.Now.AddDays(1).ToString("yyyyMMdd")} group by itemios.item_no),0) as 領用數量, ISNULL((select sum(ITEMIOS.QTY2) from itemios where itemios.item_no=item.item_no and itemios.io=2 and itemios.trn_date>{條件式_期限日} and itemios.trn_date<{DateTime.Now.AddDays(1).ToString("yyyyMMdd")} group by itemios.item_no),0) as 入庫數量, ISNULL((select sum(invosub.QUANTITY) from invosub where invosub.item_no=item.item_no and type=1 and invosub.trn_date>{條件式_期限日} and invosub.trn_date<{DateTime.Now.AddDays(1).ToString("yyyyMMdd")} group by invosub.item_no),0) as 銷貨數量 from item LEFT JOIN ITEM_GD ON ITEM_GD.ITEM_NO = ITEM.ITEM_NO WHERE ITEM.PQTY_H>0 {condition} AND ITEM.COQTY = 0 AND ITEM.ISQTY = 0 AND ITEM.SMQTY >= 0 AND ITEM.SOQTY >= 0 AND ITEM_GD.QTY > 0 AND ({儲位條件})) as a LEFT JOIN (SELECT DISTINCT ITEM.item_no AS 品號, ITEM.ITEMNM AS 品名規格, ITEM.PQTY_H AS 庫存數, ITEM.PLACE AS 倉位, MAX(itemios.trn_date) AS 最後領料日, ITEM_GD.GDNO AS 儲位 , ITEM.SCOST AS 標準成本,CLASS.C_NAME as 類別 FROM ITEM LEFT JOIN itemios ON itemios.ITEM_NO = ITEM.ITEM_NO LEFT JOIN ITEM_GD ON ITEM_GD.ITEM_NO = ITEM.ITEM_NO LEFT JOIN CLASS ON CLASS.CLASS = ITEM.CLASS WHERE  ITEM_GD.QTY > 0 GROUP BY ITEM.ITEM_NO, ITEM.ITEMNM, ITEM.PQTY_H, ITEM.PLACE, ITEM_GD.GDNO, ITEM.SCOST,CLASS.C_NAME) AS b ON a.品號 = b.品號 AND a.庫存數量 = b.庫存數  order by 儲位 DESC";

                return sql_cmd;
            }
            public static string 呆料數量_依儲位(string 物料類別, string 品號, string 條件式_期限日, string 儲位, CheckBoxList cbx)
            {
                string condition = "";
                for (int i = 0; i < cbx.Items.Count; i++)
                {
                    if (cbx.Items[i].Selected)
                    {
                        if (condition == "")
                            condition += $" ITEM.CLASS = '{cbx.Items[i].Value}' ";
                        else
                            condition += $" OR ITEM.CLASS = '{cbx.Items[i].Value}' ";

                    }

                }
                if (condition != "")
                    condition = $" AND ({condition}) ";

                //正確的
                string sql_cmd = $"select  a.品號,  ITEM_GD.GDNO as 儲位,  ITEM_GD.QTY as 儲位數量  from  ( select distinct ITEM.item_no as 品號,  ITEM.PQTY_H as 庫存數,  ITEM.PLACE as 倉位,  MAX(itemios.trn_date) as 最後領料日   from ITEM  left join itemios on itemios.ITEM_NO = ITEM.ITEM_NO  where  ITEM.PQTY_H > 0  {condition}  and ITEM.COQTY  = 0  and ITEM.ISQTY  = 0  and ITEM.SMQTY >= 0   and ITEM.SOQTY >= 0  and ITEM.ITEM_NO= '{品號}'  group by ITEM.ITEM_NO,ITEM.ITEMNM,ITEM.PQTY_H,ITEM.PLACE ) a  left join ITEM_GD on ITEM_GD.ITEM_NO = a.品號  where  ITEM_GD.QTY > 0 and ITEM_GD.GDNO like '%{儲位}%'  order by a.品號    ";

                //錯誤的
                // string sql_cmd = "select  a.品號,  ITEM_GD.GDNO as 儲位,  ITEM_GD.QTY as 儲位數量  from  ( select distinct ITEM.item_no as 品號,  ITEM.PQTY_H as 庫存數,  ITEM.PLACE as 倉位,  MAX(itemios.trn_date) as 最後領料日   from ITEM  left join itemios on itemios.ITEM_NO = ITEM.ITEM_NO  where  ITEM.PQTY_H > 0  and ITEM.CLASS  = '" + 物料類別 + "'  and ITEM.COQTY  = 0  and ITEM.ISQTY  = 0  and ITEM.SMQTY >= 0   and ITEM.SOQTY >= 0  and ITEM.ITEM_NO= '" + 品號 + "'  group by ITEM.ITEM_NO,ITEM.ITEMNM,ITEM.PQTY_H,ITEM.PLACE ) a  left join ITEM_GD on ITEM_GD.ITEM_NO = a.品號  where (a.最後領料日 <= " + 條件式_期限日 + " or a.最後領料日 is null) and ITEM_GD.QTY > 0 and ITEM_GD.GDNO like '%" + 儲位 + "%'  order by a.品號    ";
                return sql_cmd;
            }
            public static string 庫存數量(CheckBoxList cbx)
            {
                string condition = "";
                for (int i = 0; i < cbx.Items.Count; i++)
                {
                    if (cbx.Items[i].Selected)
                        condition += condition == "" ? $" ITEM_GD.GDNO = '{cbx.Items[i].Value}' " : $" OR ITEM_GD.GDNO = '{cbx.Items[i].Value}' ";
                }

                condition = condition != "" ? $" AND ({condition}) " : "";
                return $" SELECT item.item_no AS 品號,itemnm AS 品名規格,CLASS.C_NAME AS 物料類別, case item.NOUSE when 'Y' then '是' else  '' end 是否停用 , ITEM_GD.GDNO AS 倉位,ITEM_GD.qty AS 庫存數量,SCOST AS 庫存金額,(ITEM_GD.qty*SCOST) AS 總庫存金額 FROM item LEFT JOIN ITEM_GD ON ITEM_GD.ITEM_NO = ITEM.ITEM_NO AND ITEM_GD.QTY >0 LEFT JOIN CLASS ON CLASS.CLASS = ITEM.CLASS WHERE PQTY_H>0 {condition} ORDER BY item.item_no";
            }
        }
        public class 倉管部_報廢數量統計表
        {
            public static string 取得報廢人列表(string 日期起, string 日期訖)
            {
                string sql_cmd = "SELECT distinct ITEMIO.STOCKNAME as 報廢人 FROM ITEM AS ITEM  LEFT JOIN ITEMIOS AS ITEMIOS ON ITEM.ITEM_NO = ITEMIOS.ITEM_NO AND ITEMIOS.IO='3'  LEFT JOIN ITEMIO AS ITEMIO ON ITEMIOS.IO=ITEMIO.IO AND ITEMIOS.TRN_NO=ITEMIO.TRN_NO  LEFT JOIN DEP AS DEP ON ITEMIO.D_NO=DEP.S_NO WHERE ITEMIOS.IRQTY <> 0 AND ITEMIOS.WASTES_SW2='Y' AND ITEMIOS.TRN_DATE >= " + 日期起 + " AND ITEMIOS.TRN_DATE <= " + 日期訖 + "";
                return sql_cmd;
            }
            public static string 報廢數量統計表(string 日期起, string 日期訖, string 報廢人)
            {
                if (報廢人 != "")
                {
                    string 報廢人條件 = "";
                    for (int i = 0; i < 報廢人.Split(',').Length - 1; i++)
                    {
                        if (i + 1 == 報廢人.Split(',').Length - 1)
                        { 報廢人條件 += " ITEMIO.STOCKNAME = '" + 報廢人.Split(',')[i] + "'"; }
                        else
                        { 報廢人條件 += " ITEMIO.STOCKNAME = '" + 報廢人.Split(',')[i] + "' or "; }
                    }
                    報廢人 = "and(" + 報廢人條件 + ")";
                }
                string sql_cmd = "SELECT ITEMIO.STOCKNAME as 報廢者,ITEMIOS.TRN_NO as 單據號碼, ITEMIOS.TRN_DATE as 單據日期, ITEM.ITEM_NO as 品號,ITEM.ITEMNM as 品名規格, ITEM.UNIT as 單位, ITEM.SCOST as 標準成本 , ITEMIOS.IRQTY AS 報廢數量, (ITEM.SCOST*ITEMIOS.IRQTY) as 金額小計, ITEMIO.REMARK as 備註 FROM ITEM AS ITEM  LEFT JOIN ITEMIOS AS ITEMIOS ON ITEM.ITEM_NO = ITEMIOS.ITEM_NO AND ITEMIOS.IO='3'  LEFT JOIN ITEMIO AS ITEMIO ON ITEMIOS.IO=ITEMIO.IO AND ITEMIOS.TRN_NO=ITEMIO.TRN_NO  LEFT JOIN DEP AS DEP ON ITEMIO.D_NO=DEP.S_NO WHERE ITEMIOS.IRQTY <> 0 AND ITEMIOS.TRN_NO <> '090902085'      AND ITEMIOS.WASTES_SW2='Y' AND ITEMIOS.TRN_DATE >= " + 日期起 + " AND ITEMIOS.TRN_DATE <= " + 日期訖 + " " + 報廢人 + "";
                return sql_cmd;
            }
            public static string 取得報廢統計資料(string 日期起, string 日期訖, string 報廢人)
            {
                //string sql_cmd = "SELECT SUBSTRING(ITEMIOS.TRN_DATE,0, 7) as 單據日期, Replace(Convert(Varchar(12),CONVERT(money,sum((ITEM.SCOST*ITEMIOS.IRQTY))),1),'.00','') as 金額小計 FROM ITEM AS ITEM LEFT JOIN ITEMIOS AS ITEMIOS ON ITEM.ITEM_NO = ITEMIOS.ITEM_NO AND ITEMIOS.IO = '3' LEFT JOIN ITEMIO AS ITEMIO ON ITEMIOS.IO = ITEMIO.IO AND ITEMIOS.TRN_NO = ITEMIO.TRN_NO LEFT JOIN DEP AS DEP ON ITEMIO.D_NO = DEP.S_NO  WHERE ITEMIOS.IRQTY <> 0 AND ITEMIOS.WASTES_SW2 = 'Y' AND ITEMIOS.TRN_DATE >= " + 日期起 + " AND ITEMIOS.TRN_DATE <= " + 日期訖 + " Group by SUBSTRING(ITEMIOS.TRN_DATE, 0, 7)";
                if (報廢人 != "")
                {
                    string 報廢人條件 = "";
                    for (int i = 0; i < 報廢人.Split(',').Length - 1; i++)
                    {
                        if (i + 1 == 報廢人.Split(',').Length - 1)
                        {

                            報廢人條件 += " ITEMIO.STOCKNAME = '" + 報廢人.Split(',')[i] + "'";
                        }
                        else
                        {

                            報廢人條件 += " ITEMIO.STOCKNAME = '" + 報廢人.Split(',')[i] + "' or ";
                        }

                    }
                    報廢人 = "and(" + 報廢人條件 + ")";

                }
                string sql_cmd = "select b.不良原因,COUNT(b.不良原因)  as 次數,sum(b.報廢數量) as 報廢數量 ,SUM(b.總報廢金額) as 總報廢金額 from ( select  SUBSTRING(a.不良原因,0,CHARINDEX(';',a.不良原因)) as 不良原因, a.報廢數量 as 報廢數量,  (a.報廢數量*a.標準成本) as 總報廢金額 from ( SELECT  ITEM.SCOST as 標準成本 , ITEMIOS.IRQTY AS 報廢數量, SUBSTRING(ITEMIO.REMARK,CHARINDEX('因',ITEMIO.REMARK)+9,CHARINDEX('屬',ITEMIO.REMARK)) as 不良原因 FROM ITEM AS ITEM LEFT JOIN ITEMIOS AS ITEMIOS ON ITEM.ITEM_NO = ITEMIOS.ITEM_NO AND ITEMIOS.IO='3' LEFT JOIN ITEMIO AS ITEMIO ON ITEMIOS.IO=ITEMIO.IO AND ITEMIOS.TRN_NO=ITEMIO.TRN_NO LEFT JOIN DEP AS DEP ON ITEMIO.D_NO=DEP.S_NO WHERE ITEMIOS.IRQTY <> 0 AND ITEMIOS.WASTES_SW2='Y' AND ITEMIOS.TRN_DATE >= " + 日期起 + " AND ITEMIOS.TRN_DATE <= " + 日期訖 + " " + 報廢人 + " ) as a  group by SUBSTRING(a.不良原因,0,CHARINDEX(';',a.不良原因)),a.標準成本,a.報廢數量 ) as b group by b.不良原因 ";
                return sql_cmd;
            }
            public static string 取得報廢次數(string 日期起, string 日期訖, string 報廢人)
            {
                if (報廢人 != "")
                {
                    string 報廢人條件 = "";
                    for (int i = 0; i < 報廢人.Split(',').Length - 1; i++)
                    {
                        if (i + 1 == 報廢人.Split(',').Length - 1)
                        {

                            報廢人條件 += " ITEMIO.STOCKNAME = '" + 報廢人.Split(',')[i] + "'";
                        }
                        else
                        {

                            報廢人條件 += " ITEMIO.STOCKNAME = '" + 報廢人.Split(',')[i] + "' or ";
                        }

                    }
                    報廢人 = "and(" + 報廢人條件 + ")";

                }

                string sqlcmd = "select SUBSTRING(a.不良原因,0,CHARINDEX(';',a.不良原因)) as 不良原因, COUNT(SUBSTRING(a.不良原因,0,CHARINDEX(';',a.不良原因))) as 次數 from ( SELECT  SUBSTRING(ITEMIO.REMARK,CHARINDEX('因',ITEMIO.REMARK)+9,CHARINDEX('屬',ITEMIO.REMARK)) as 不良原因 FROM ITEM AS ITEM LEFT JOIN ITEMIOS AS ITEMIOS ON ITEM.ITEM_NO = ITEMIOS.ITEM_NO AND ITEMIOS.IO='3' LEFT JOIN ITEMIO AS ITEMIO ON ITEMIOS.IO=ITEMIO.IO AND ITEMIOS.TRN_NO=ITEMIO.TRN_NO LEFT JOIN DEP AS DEP ON ITEMIO.D_NO=DEP.S_NO WHERE ITEMIOS.IRQTY <> 0 AND ITEMIOS.WASTES_SW2='Y' AND ITEMIOS.TRN_DATE >= " + 日期起 + " AND ITEMIOS.TRN_DATE <= " + 日期訖 + " " + 報廢人 + " ) as a   group by SUBSTRING(a.不良原因,0,CHARINDEX(';',a.不良原因))order by 次數 desc ";
                return sqlcmd;

            }
            public static string 找最大者(string sql)
            {
                sql = sql + "order by (ITEM.SCOST*ITEMIOS.IRQTY) desc";
                return sql;
            }

        }

        public class 控制台_權限管理
        {
            public static string 特殊功能YN(string 帳號, string 功能名稱)
            {
                string sql_cmd = "SELECT  USER_ACC as 帳號 ,FUNC_DES as 功能描述 ,FUNC_YN as 許可 FROM SYSTEM_PMR where USER_ACC = '" + 帳號 + "' and FUNC_DES = '" + 功能名稱 + "'";
                return sql_cmd;
            }
            public static string 取得選取用戶的權限詳細(string selected_user_acc, string editer_dpm)
            {
                if (editer_dpm != "")
                {
                    editer_dpm = "where a.WEB_DPM = '" + editer_dpm + "'";
                }
                string sqlcmd = "select a.*,(CASE WHEN b.USER_ACC is null THEN '' ELSE b.USER_ACC END ) as USER_ACC , b.View_NY from (select WEB_PAGES.WEB_DPM, WEB_PAGES.WEB_PAGENAME, WEB_PAGES.WEB_URL,WEB_PAGES.WEB_PATH,WEB_PAGES.web_index,web_pages.web_open FROM WEB_PAGES) as a left join (SELECT WB_URL,USER_ACC,View_NY FROM WEB_USER where USER_ACC = '" + selected_user_acc + "') as b on a.WEB_URL = b.WB_URL " + editer_dpm + " and a.web_index > 0  AND web_open = 'Y'   and WEB_PAGENAME <> '電控盒產線'     and WEB_PAGENAME <> '電控盒產線細節' order by a.web_index";
                return sqlcmd;
            }
            public static string 取得部門_依網頁()
            {
                string sqlcmd = "SELECT distinct WEB_DPM,DEPARTMENT.DPM_NAME2 FROM WEB_PAGES left join DEPARTMENT on DEPARTMENT.DPM_NAME = WEB_PAGES.WEB_DPM where DEPARTMENT.DPM_GROUP is null";
                return sqlcmd;
            }
            public static string 取得部門(string 欲排除部門)
            {
                string sqlcmd = "SELECT DPM_NAME,DPM_NAME2 FROM DEPARTMENT where DPM_NAME != '" + 欲排除部門 + "'";
                return sqlcmd;
            }
            public static string 取得用戶個人資訊(string 帳號)
            {
                string sqlcmd = "SELECT USER_ACC,USER_NUM,USER_NAME,USER_PWD,DEPARTMENT.DPM_NAME2,USER_DPM,USER_EMAIL,USER_BIRTHDAY,ADM FROM USERS left join DEPARTMENT on DEPARTMENT.DPM_NAME = USERS.USER_DPM where USER_ACC = '" + 帳號 + "' OR USER_NUM = '" + 帳號 + "'";

                return sqlcmd;

            }
            public static string 取得申請瀏覽列表()
            {
                string slqcmd = "select a.用戶帳號,DEPARTMENT.DPM_NAME2 as 所屬部門,a.用戶名稱,a.申請頁面,a.頁面網址 from ( SELECT USERS.USER_ACC as 用戶帳號, 	USERS.USER_DPM as 所屬部門, 	  USERS.USER_NAME as 用戶名稱, 	  DEPARTMENT.DPM_NAME2 + '-' +WEB_PAGES.WEB_PAGENAME as 申請頁面, 	  WEB_USER.WB_URL as 頁面網址   FROM WEB_USER   left join WEB_PAGES on WEB_PAGES.WEB_URL = WEB_USER.WB_URL   left join DEPARTMENT on DEPARTMENT.DPM_NAME = WEB_PAGES.WEB_DPM   left join USERS on USERS.USER_ACC = WEB_USER.USER_ACC   where VIEW_NY = 'N' ) as a    left join DEPARTMENT on DEPARTMENT.DPM_NAME = a.所屬部門   ";
                return slqcmd;
            }
            public static int 是否為新用戶(string user_acc)
            {
                int count = 0;
                clsDB_Server clsDB_Server = new clsDB_Server(myclass.GetConnByDekVisErp);
                if (clsDB_Server.IsConnected == true)
                {
                    string slqcmd = "SELECT COUNT(USER_ACC) as 登入次數 FROM SYSTEM_USERSLOGIN_log where user_acc = '" + user_acc + "'";
                    count = DataTableUtils.toInt(DataTableUtils.toString(clsDB_Server.GetDataTable(slqcmd).Rows[0]["登入次數"]));
                }
                return count;
            }
            //20190605，參數設定，判斷是否新增過參數值
            public static string 是否新增過參數(string 帳號)
            {
                string sqlcmd = $"select * from SYSTEM_PARAMETER where USER_ACC = '{帳號}'";
                return sqlcmd;
            }
            public static string 取得起算結算日(string 帳號)
            {
                string sqlcmd = "select DATE_STR, DATE_END from SYSTEM_PARAMETER where USER_ACC = '" + 帳號 + "'  ";
                return sqlcmd;
            }

            //20190605，參數設定，起始/結算日期
            public static string 更新參數設定(string 起始日, string 結束日, string 帳號)
            {
                string sqlcmd = "update SYSTEM_PARAMETER set DATE_STR = '" + 起始日 + "', DATE_END = '" + 結束日 + "'  WHERE USER_ACC = '" + 帳號 + "'  ";
                return sqlcmd;
            }
            //20190605，參數設定，起始/結算日期
            public static string 新增參數設定(string ID, string 帳號, string 起始日, string 結束日)
            {
                string sqlcmd = "insert into SYSTEM_PARAMETER (SETID, USER_ACC, DATE_STR, DATE_END) values('" + ID + "','" + 帳號 + "', " + 起始日 + ", " + 結束日 + ")";
                return sqlcmd;
            }
        }
        //生產推移圖(2021/02/25)開始改版
        public class 生產部
        {
            //當月應生產之總數量
            public static string 首旺預計生產明細(string 起始日, string 結束日, string 條件式)
            {
                return $"SELECT sw_db.客戶簡稱, sw_db.產線代號, sw_db.製造批號 AS 排程編號, sw_db.預計開工日,sw_db.預計開工日 AS 預計完工日, vis_db.進度, vis_db.狀態,vis_db.實際完成時間,vis_db.組裝日,sw_db.訂單號碼,sw_db.客戶訂單,sw_db.品號,sw_db.訂單日期 FROM (SELECT DISTINCT sw_CUST.custnm2 AS 客戶簡稱, sw_A22_FAB.FAB_USER AS 產線代號 , sw_MKORDSUB.LOT_NO AS 製造批號, sw_a22_fab.cord_no AS 訂單號碼, sw_CORD.CORD_NO AS 客戶訂單, sw_MKORDSUB.item_no AS 品號, sw_item.itemnm AS 品名規格, sw_A22_FAB.STR_DATE AS 預計開工日,sw_cordsub.trn_date AS 訂單日期 FROM SW.FJWSQL.dbo.A22_FAB AS sw_A22_FAB LEFT JOIN SW.FJWSQL.dbo.CORD AS sw_CORD ON sw_CORD.trn_no = sw_A22_FAB.cord_no LEFT JOIN SW.FJWSQL.dbo.CORDSUB AS sw_CORDSUB ON sw_CORDSUB.TRN_NO = sw_A22_FAB.CORD_no AND sw_CORDSUB.SN = sw_A22_FAB.CORD_SN LEFT JOIN SW.FJWSQL.dbo.CUST AS sw_CUST ON sw_CUST.CUST_NO = sw_CORD.CUST_NO LEFT JOIN SW.FJWSQL.dbo.MKORDSUB AS sw_MKORDSUB ON sw_MKORDSUB.CORD_NO = sw_CORDSUB.trn_no AND sw_MKORDSUB.CORD_SN = sw_CORDSUB.sn LEFT JOIN SW.FJWSQL.dbo.item_22 AS sw_item_22 ON sw_CORDSUB.item_no = sw_item_22.item_no LEFT JOIN SW.FJWSQL.dbo.ITEM AS sw_item ON sw_item.ITEM_NO = sw_item_22.item_no WHERE sw_item_22.PLINE_NO > 0 AND sw_A22_FAB.STR_DATE >= {起始日} AND sw_A22_FAB.STR_DATE <= {結束日} AND sw_A22_FAB.STR_DATE>0 ) AS sw_db LEFT JOIN 工作站狀態資料表 AS vis_db ON sw_db.製造批號 = vis_db.排程編號 ORDER BY sw_db.產線代號";
            }
            public static string 首旺未如期下架明細(string 起始日, string 條件式)
            {
                return $"SELECT sw_db.客戶簡稱, sw_db.產線代號, sw_db.製造批號 AS 排程編號, sw_db.預計開工日,sw_db.預計開工日 AS 預計完工日, vis_db.進度, vis_db.狀態,vis_db.實際完成時間,sw_db.客戶訂單,sw_db.訂單日期,sw_db.訂單號碼,sw_db.品號,vis_db.組裝日 FROM (SELECT DISTINCT sw_CUST.custnm2 AS 客戶簡稱, sw_A22_FAB.FAB_USER AS 產線代號 , sw_MKORDSUB.LOT_NO AS 製造批號, sw_a22_fab.cord_no AS 訂單號碼, sw_CORD.CORD_NO AS 客戶訂單, sw_MKORDSUB.item_no AS 品號, sw_item.itemnm AS 品名規格, sw_A22_FAB.STR_DATE AS 預計開工日,sw_cordsub.trn_date AS 訂單日期 FROM SW.FJWSQL.dbo.A22_FAB AS sw_A22_FAB LEFT JOIN SW.FJWSQL.dbo.CORD AS sw_CORD ON sw_CORD.trn_no = sw_A22_FAB.cord_no LEFT JOIN SW.FJWSQL.dbo.CORDSUB AS sw_CORDSUB ON sw_CORDSUB.TRN_NO = sw_A22_FAB.CORD_no AND sw_CORDSUB.SN = sw_A22_FAB.CORD_SN LEFT JOIN SW.FJWSQL.dbo.CUST AS sw_CUST ON sw_CUST.CUST_NO = sw_CORD.CUST_NO LEFT JOIN SW.FJWSQL.dbo.MKORDSUB AS sw_MKORDSUB ON sw_MKORDSUB.CORD_NO = sw_CORDSUB.trn_no AND sw_MKORDSUB.CORD_SN = sw_CORDSUB.sn LEFT JOIN SW.FJWSQL.dbo.citem AS sw_citem ON sw_CORDSUB.item_no = sw_citem.item_no LEFT JOIN SW.FJWSQL.dbo.item_22 AS sw_item_22 ON sw_CORDSUB.item_no = sw_item_22.item_no LEFT JOIN SW.FJWSQL.dbo.ITEM AS sw_item ON sw_item.ITEM_NO = sw_item_22.item_no LEFT JOIN SW.FJWSQL.dbo.ITEMIOS AS sw_ITEMIOS ON sw_ITEMIOS.MKORD_NO = sw_MKORDSUB.TRN_NO AND sw_ITEMIOS.MKORD_SN = sw_MKORDSUB.SN WHERE sw_A22_FAB.FAB_USER > 0 AND sw_A22_FAB.STR_DATE <{起始日} AND sw_A22_FAB.STR_DATE>0 ) AS sw_db LEFT JOIN 工作站狀態資料表 AS vis_db ON sw_db.製造批號 = vis_db.排程編號 WHERE SUBSTRING(vis_db.實際完成時間, 1, 8) >={起始日} ORDER BY 產線代號 ";
            }
            public static string 首旺未完成明細(string 起始日, string 條件式)
            {
                return $"SELECT sw_db.客戶簡稱, sw_db.產線代號, sw_db.製造批號 AS 排程編號, sw_db.預計開工日,sw_db.預計開工日 AS 預計完工日, vis_db.進度, vis_db.狀態,vis_db.實際完成時間,sw_db.客戶訂單,sw_db.訂單日期,sw_db.訂單號碼,sw_db.品號,vis_db.組裝日 FROM (SELECT DISTINCT sw_CUST.custnm2 AS 客戶簡稱, sw_A22_FAB.FAB_USER AS 產線代號 , sw_MKORDSUB.LOT_NO AS 製造批號, sw_a22_fab.cord_no AS 訂單號碼, sw_CORD.CORD_NO AS 客戶訂單, sw_MKORDSUB.item_no AS 品號, sw_item.itemnm AS 品名規格, sw_A22_FAB.STR_DATE AS 預計開工日,sw_cordsub.trn_date AS 訂單日期 FROM SW.FJWSQL.dbo.A22_FAB AS sw_A22_FAB LEFT JOIN SW.FJWSQL.dbo.CORD AS sw_CORD ON sw_CORD.trn_no = sw_A22_FAB.cord_no LEFT JOIN SW.FJWSQL.dbo.CORDSUB AS sw_CORDSUB ON sw_CORDSUB.TRN_NO = sw_A22_FAB.CORD_no AND sw_CORDSUB.SN = sw_A22_FAB.CORD_SN LEFT JOIN SW.FJWSQL.dbo.CUST AS sw_CUST ON sw_CUST.CUST_NO = sw_CORD.CUST_NO LEFT JOIN SW.FJWSQL.dbo.MKORDSUB AS sw_MKORDSUB ON sw_MKORDSUB.CORD_NO = sw_CORDSUB.trn_no AND sw_MKORDSUB.CORD_SN = sw_CORDSUB.sn LEFT JOIN SW.FJWSQL.dbo.item_22 AS sw_item_22 ON sw_CORDSUB.item_no = sw_item_22.item_no LEFT JOIN SW.FJWSQL.dbo.ITEM AS sw_item ON sw_item.ITEM_NO = sw_item_22.item_no LEFT JOIN SW.FJWSQL.dbo.ITEMIOS AS sw_ITEMIOS ON sw_ITEMIOS.MKORD_NO = sw_MKORDSUB.TRN_NO AND sw_ITEMIOS.MKORD_SN = sw_MKORDSUB.SN WHERE sw_CORDSUB.SCLOSE !='結案' AND sw_item.CLASS='Z' AND sw_A22_FAB.STR_DATE >20200101 AND sw_A22_FAB.STR_DATE <{起始日} AND sw_item_22.PLINE_NO > 0 AND (SELECT MAX(SW_ITEMIO.TRN_DATE) FROM SW.FJWSQL.dbo.ITEMIOS AS SW_ITEMIOS LEFT JOIN SW.FJWSQL.dbo.ITEMIO AS SW_ITEMIO ON SW_ITEMIOS.IO=SW_ITEMIO.IO AND SW_ITEMIOS.TRN_NO=SW_ITEMIO.TRN_NO WHERE SW_ITEMIOS.IO='2' AND SW_ITEMIOS.MKORD_NO=sw_MKORDSUB.TRN_NO AND SW_ITEMIOS.MKORD_SN=sw_MKORDSUB.SN AND SW_ITEMIO.S_DESC< >'歸還' AND SW_ITEMIO.MK_TYPE='成品入庫') IS NULL AND sw_A22_FAB.STR_DATE>0 ) AS sw_db LEFT JOIN 工作站狀態資料表 AS vis_db ON sw_db.製造批號 = vis_db.排程編號 WHERE sw_db.預計開工日 IS NOT NULL AND 實際完成時間 IS NULL ORDER BY sw_db.產線代號 DESC ";
            }
            public static string 德科預計生產明細(string 起始日, string 結束日, string 條件式)
            {
                return $"SELECT  cust.custnm2 AS 客戶簡稱, ws.PLINE_NO AS 產線代號, item.item_no_bom AS 客戶機型, ws.lot_no AS 排程編號, DATE_FORMAT(mkordsub.str_date, '%Y%m%d') AS 預計開工日, ws.cord_no AS 訂單號碼, mkordsub.item_no AS 品號, cord.cord_no AS 客戶訂單, DATE_FORMAT( cordsub.trn_date, '%Y%m%d') AS 訂單日期 FROM ws RIGHT JOIN mkordsub ON (ws.cord_no = mkordsub.cord_no AND ws.cord_sn = mkordsub.cord_sn AND (ws.lot_no = mkordsub.lot_no || ws.lot_no IS NULL || ws.lot_no = '')) LEFT JOIN item ON (mkordsub.item_no = item.item_no) LEFT JOIN cord ON (ws.cord_no = cord.trn_no) LEFT JOIN cordsub ON (ws.cord_no = cordsub.trn_no AND ws.cord_sn = cordsub.sn) LEFT JOIN invosub ON (ws.cord_no = invosub.cord_no AND ws.cord_sn = invosub.cord_sn AND ws.lot_no = invosub.lot_no) LEFT JOIN citem ON (citem.cust_no = mkordsub.cust_no AND citem.item_no = mkordsub.item_no) LEFT JOIN mkord ON (mkord.trn_no = mkordsub.trn_no) LEFT JOIN cust ON (cust.cust_no = mkord.cust_no) WHERE 1 = 1 AND ws.lot_no IS NOT NULL AND LENGTH(ws.lot_no) > 0 AND DATE_FORMAT(mkordsub.str_date, '%Y%m%d') >= {起始日} AND DATE_FORMAT(mkordsub.str_date, '%Y%m%d') <= {結束日} ORDER BY mkordsub.str_date ASC";
            }
            public static string 德科未如期完成明細(string 起始日, string 條件式)
            {
                return $" SELECT  cust.custnm2 AS 客戶簡稱, ws.PLINE_NO AS 產線代號, item.item_no_bom AS 客戶機型, ws.lot_no AS 排程編號, DATE_FORMAT(mkordsub.str_date, '%Y%m%d') AS 預計開工日, ws.cord_no AS 訂單號碼, mkordsub.item_no AS 品號, cord.cord_no AS 客戶訂單, DATE_FORMAT(cordsub.trn_date, '%Y%m%d') AS 訂單日期 FROM ws RIGHT JOIN mkordsub ON (ws.cord_no = mkordsub.cord_no AND ws.cord_sn = mkordsub.cord_sn AND (ws.lot_no = mkordsub.lot_no || ws.lot_no IS NULL || ws.lot_no = '')) LEFT JOIN item ON (mkordsub.item_no = item.item_no) LEFT JOIN cord ON (ws.cord_no = cord.trn_no) LEFT JOIN cordsub ON (ws.cord_no = cordsub.trn_no AND ws.cord_sn = cordsub.sn) LEFT JOIN invosub ON (ws.cord_no = invosub.cord_no AND ws.cord_sn = invosub.cord_sn AND ws.lot_no = invosub.lot_no) LEFT JOIN citem ON (citem.cust_no = mkordsub.cust_no AND citem.item_no = mkordsub.item_no) LEFT JOIN mkord ON (mkord.trn_no = mkordsub.trn_no) LEFT JOIN cust ON (cust.cust_no = mkord.cust_no) WHERE 1 = 1 AND ws.lot_no IS NOT NULL AND LENGTH(ws.lot_no) > 0 AND DATE_FORMAT(mkordsub.str_date, '%Y%m%d') < {起始日} AND DATE_FORMAT(mkordsub.str_date, '%Y%m%d') > 20200901 ORDER BY mkordsub.str_date ASC";
            }
            public static string 取得德大產線()
            {
                return "SELECT distinct ASSEMBLY_GROUP.GROUP_NAME, ASSEMBLY_LINE.LINE_ID  FROM ASSEMBLY_LINE inner join ASSEMBLY_LINE_GROUP on ASSEMBLY_LINE.LINE_ID = ASSEMBLY_LINE_GROUP.LINE_ID inner join ASSEMBLY_GROUP on ASSEMBLY_LINE_GROUP.GROUP_ID = ASSEMBLY_GROUP.GROUP_ID where ASSEMBLY_LINE.LINE_STATUS = '1' ";
            }

        }


        //20190515
        public class 帳號管理
        {
            public static string 使用者帳號()
            {
                string sqlcmd = "SELECT USER_NAME as 使用者名稱,USERS.USER_ACC as 使用者帳號, dept.DPM_NAME2 as 部門別, USER_EMAIL as 信箱, USER_BIRTHDAY as 生日, USER_NUM as  電話, USERS.STATUS as 狀態, ADM, ADD_TIME as 建立時間 , count(SU_ID) as 登入次數 FROM USERS left join DEPARTMENT as dept on USERS.USER_DPM = dept.DPM_NAME left join SYSTEM_USERSLOGIN_log as USERSLOGIN on USERS.USER_ACC = USERSLOGIN.USER_ACC group by USER_NAME,USERS.USER_ACC,dept.DPM_NAME2,USER_EMAIL,USER_BIRTHDAY,USER_NUM,USERS.STATUS,ADM,ADD_TIME order by count(SU_ID) desc ";
                return sqlcmd;
            }
            public static string 使用登入紀錄(string 帳號)
            {
                string sqlcmd = "select USER_NAME as 使用者名稱,USERSLOGIN.USER_ACC  as 使用者帳號,  LOGIN_TIME as 登入時間, CLIENT_IP as IP位址 from SYSTEM_USERSLOGIN_log as USERSLOGIN left join USERS on USERSLOGIN .USER_ACC = USERS.USER_ACC where USERSLOGIN.USER_ACC = '" + 帳號 + "' and USERSLOGIN.STATUS > 0 order by LOGIN_TIME desc";
                return sqlcmd;
            }
        }
        public class 業務報告
        {
            public static string 報告時間查詢(string 關鍵字, string 部門, string 種類, string date_str, string date_end)
            {
                string sqlcmd = "SELECT " +
                                "Record_Report.id as 'id'," +
                                "Record_Report.part as '所屬部門'," +
                                "Record_Report.type as '類型'," +
                                "Record_Report.title as '標題'," +
                                "Record_Report.name as '撰寫人'," +
                                "Record_Report.updatetime as '上傳時間' " +
                                "FROM[Record_Report] " +
                                "where " + 關鍵字 + 部門 + 種類 +
                                "time  BETWEEN '" + date_str + "' AND '" + date_end + "'";
                return sqlcmd;
            }
            public static string 取得文本(string id)
            {
                string sqlcmd = "SELECT Record_Report.id as 'id',Record_Report.title as '標題',Record_Report.type as '種類',Record_Report.name as '撰寫人',Record_Report.updatetime as '上傳時間',Record_Report.part as '所屬部門' FROM [Record_Report] where id ='" + id + "'";
                return sqlcmd;
            }
        }
    }
}