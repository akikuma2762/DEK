using dek_erpvis_v2.cls;
using Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace dek_erpvis_v2.pages.webservice
{
    public partial class stockanalysis : System.Web.UI.Page
    {
        public string color = "";
        public string nstock = "";
        public string ostock = "";
        public string _val總庫存 = "";
        public string _val一般庫存 = "";
        public string _val逾期庫存 = "";
        public string Table_Title = "逾期庫存數量";
        public string pie_data_points = "";
        public string col_data_points_nor = "";
        public string col_data_points_sply = "";
        public string th = "";
        public string tr = "";
        public string date_str = "";
        public string date_end = "";
        public string title_msg = "";
        public string title_msg_list = "";
        public string title_text = "";
        public string path = "";
        public string timerange = "";
        public string range = "";
        string title = "";
        DataTable all = new DataTable();
        string sql_condi = "";
        string URL_NAME = "";
        string acc = "";
        string[] str = null;
        int total = 30;
        int CUST_TOTAL;
        DataTable dw = null;
        DataTable public_dt = null;
        myclass myclass = new myclass();
        德大機械 德大機械 = new 德大機械();
        clsDB_Server clsDB_sw = new clsDB_Server("");
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
             
                path = 德大機械.get_title_web_path("WHE");
                //  path = path.Replace("</ol>", "<li><u><a href='../dp_WH/stockanalysis.aspx'>成品庫存分析</a></u></li></ol>");
                URL_NAME = "stockanalysis";
                color = HtmlUtil.change_color(acc);
                if (CheckBox_All.Checked == true)
                    total = 1;
                else
                    total = DataTableUtils.toInt(txt_showCount.Text);
                if (myclass.user_view_check(URL_NAME, acc) == true)
                {
                    string[] daterange = 德大機械.德大專用月份(acc).Split(',');
                    if (!IsPostBack)
                    {
                        get_SqlConndi();
                        GotoCenn();
                    }

                }
                else
                {
                    Response.Write("<script>alert('您無法瀏覽此頁面 請向該單位主管申請權限!');location.href='../index.aspx';</script>");
                    //Response.Redirect(myclass.logout_url);
                }

               
            }
            else

                Response.Redirect(myclass.logout_url);

        }
        //function
        private void GotoCenn()
        {



            clsDB_sw.dbOpen(myclass.GetConnByDataWin);
            if (clsDB_sw.IsConnected == true)
            {
                load_page_data();
            }
            else
            {
                Response.Write("<script language='javascript'>alert('伺服器回應 : 無法載入資料，" + clsDB_sw.ErrorMessage + " 請聯絡德科人員或檢查您的網路連線。');</script>");
                無資料處理();
            }
        }
        private void 無資料處理()
        {
            title_text = HtmlUtil.NoData(out th, out tr);
            _val總庫存 = "0";
            _val一般庫存 = "0";
            _val逾期庫存 = "0";
        }
        private void load_page_data()
        {
            //title_text = "''";
            Response.BufferOutput = false;
            set_col_value();
            Set_Html_Table();
        }
        private void set_col_value()
        {
            GlobalVar.UseDB_setConnString(myclass.GetConnByDataWin);
            string LINE_GROUP = "臥式";
            float 逾期數量 = 0, 一般庫存 = 0, 總數量 = 0;
            float TOTAL_逾期數量 = 0, TOTAL_一般庫存 = 0, TOTAL_總數量 = 0;
            int i = 0, j = 0;
            string sqlcmd = "SELECT Hd03 as 品號, da02 as 品名規格,  count(distinct Sf05) as 庫存數量 ,Sf05 as 刀庫編號,Sf04 as 入庫單號,peb03 as 訂單編號,ba03 as 客戶簡稱 FROM (     SELECT Sf05, Hd03, Qty=ISNULL(SUM(Qty),0),Sf04,peb03,ba03     FROM     (         SELECT Hd01, Hd02, Hd03, Qty=ISNULL(Hd05,0)+ISNULL(Hd08,0), SafeQty=ISNULL(Hd07,0) ,Sf05,Sf04,peb03,ba03         FROM  khm04 	left join ksm06 on ksm06.Sf02 = khm04.Hd02 and ksm06.Sf03 =   khm04.Hd03 and  ksm06.Sf06 >0 	left join ksm05 on ksm05.Se010 = 'M' and ksm06.Sf04 = ksm05.Se03 and ksm06.Sf05 = ksm05.Se04 and khm04.Hd03 = ksm05.Se02 	left join pem02 on pem02.peb011 = ksm05.Se011 and khm04.Hd03 =pem02.peb05 	LEFT JOIN kfm01 ON kfm01.Fa01 = pem02.peb03 	LEFT JOIN tbm01 ON tbm01.ba01 = kfm01.Fa061 WHERE isnull(Hd02,'')<>'' AND (Hd01 in (select ad02 from tam04 where ad01='H') or Hd01='H')   and (Hd02 between 'H01' and 'H01') and Hd03 IN(SELECT da01 FROM tdm01 WHERE 1=1  and (da55>='19110101' AND da55<='{0}')  )     ) Stk1      GROUP BY Hd01, Hd03,Sf05,Sf04,peb03,ba03 ) Stk2,tdm01  WHERE Hd03=da01 and Qty>0 GROUP BY Hd03, da02, da03, da04, da16, da24, da37, da39, da50 ,Sf05 ,Sf04,peb03,ba03 ORDER BY Hd03    ";

            string sql = "";
            sql = string.Format(sqlcmd, "99991231");
            DataTable dt = DataTableUtils.GetDataTable(sql);
            總數量 = dt.Rows.Count;

            sql = string.Format(sqlcmd, DateTime.Now.AddDays(-DataTableUtils.toInt(txt_showCount.Text)).ToString("yyyyMMdd"));
            dt = DataTableUtils.GetDataTable(sql);

            逾期數量 = dt.Rows.Count;

            一般庫存 = 總數量 - 逾期數量;

            col_data_points_sply += "{ y: " + 逾期數量 + ", label: '" + LINE_GROUP + "' },";
            col_data_points_nor += "{ y: " + 一般庫存 + ", label: '" + LINE_GROUP + "' },";



            set_pie_value(逾期數量, 一般庫存, 總數量);

        }
        private void set_pie_value(float 逾期數量, float 一般庫存, float 總數量)
        {
            //string dt1 = DateTime.Now.ToString("ss.fff");
            string _per一般庫存 = DataTableUtils.toString(一般庫存 / 總數量 * 100).Split('.')[0];
            string _per逾期庫存 = DataTableUtils.toString(逾期數量 / 總數量 * 100).Split('.')[0];

            pie_data_points = "{y:" + _per一般庫存 + ", name:'一般庫存 " + _per一般庫存 + "%' , label:'一般庫存',color:'#5b59ac'}," +
                              "{y:" + _per逾期庫存 + ",name:'逾期庫存 " + _per逾期庫存 + "%', label:'逾期庫存',color:'#ff4d4d',exploded: true}";
            nstock = _per一般庫存 + "%";
            ostock = _per逾期庫存 + "%";

            _val總庫存 = DataTableUtils.toString(總數量);
            _val一般庫存 = DataTableUtils.toString(一般庫存);
            _val逾期庫存 = DataTableUtils.toString(逾期數量);

        }
        private string Set_Html_Table()
        {
            //title
            th = "<th>客戶/產線</th>\n                                            ";
            th += "<th>臥式</th>\n";
            th += "<th>小計</th>\n                                            ";
            title = "客戶簡稱,臥式,小計,";
            str = title.Split(',');
            //月份
            if (date_str == "" && date_end == "")
            {

                date_str = DateTime.Now.AddDays(-DataTableUtils.toInt(txt_showCount.Text)).ToString("yyyyMMdd");
                date_end = "";
            }
            //Content
            string sqlcmd = "SELECT ba03 as 客戶簡稱,1000 as 臥式 FROM (     SELECT Sf05, Hd03, Qty=ISNULL(SUM(Qty),0),Sf04,peb03,ba03     FROM     (         SELECT Hd01, Hd02, Hd03, Qty=ISNULL(Hd05,0)+ISNULL(Hd08,0), SafeQty=ISNULL(Hd07,0) ,Sf05,Sf04,peb03,ba03         FROM  khm04 	left join ksm06 on ksm06.Sf02 = khm04.Hd02 and ksm06.Sf03 =   khm04.Hd03 and  ksm06.Sf06 >0 	left join ksm05 on ksm05.Se010 = 'M' and ksm06.Sf04 = ksm05.Se03 and ksm06.Sf05 = ksm05.Se04 and khm04.Hd03 = ksm05.Se02 	left join pem02 on pem02.peb011 = ksm05.Se011 and khm04.Hd03 =pem02.peb05 	LEFT JOIN kfm01 ON kfm01.Fa01 = pem02.peb03 	LEFT JOIN tbm01 ON tbm01.ba01 = kfm01.Fa061 WHERE isnull(Hd02,'')<>'' AND (Hd01 in (select ad02 from tam04 where ad01='H') or Hd01='H')   and (Hd02 between 'H01' and 'H01') and Hd03 IN(SELECT da01 FROM tdm01 WHERE 1=1  and (da55>='19110101' AND da55<='{0}')  )     ) Stk1      GROUP BY Hd01, Hd03,Sf05,Sf04,peb03,ba03 ) Stk2,tdm01  WHERE Hd03=da01 and Qty>0 GROUP BY Hd03, da02, da03, da04, da16, da24, da37, da39, da50 ,Sf05 ,Sf04,peb03,ba03 ORDER BY Hd03    ";
            all = clsDB_sw.DataTable_GetTable(string.Format(sqlcmd, DateTime.Now.AddDays(-DataTableUtils.toInt(txt_showCount.Text)).ToString("yyyyMMdd")));

            DataTable custom = all.DefaultView.ToTable(true, new string[] { "客戶簡稱" });

            tr = HtmlUtil.Set_Table_Content(custom, title, stockanalysis_callback);
            return "";
        }
        private string stockanalysis_callback(DataRow row, string field_name)//這裡之後要重新寫過，太吃效能了
        {
            string value = "";
            if (field_name == "臥式")
            {
                string sqlcmd = "客戶簡稱 ='" + DataTableUtils.toString(row["客戶簡稱"]) + "'";
                DataRow[] rows = all.Select(sqlcmd);
                value = "" + rows.Length;
                CUST_TOTAL = rows.Length;

            }

            //小計的地方需要用到超連結
            if (field_name == "小計")
            {
                //string url = HtmlUtil.AttibuteValue("cust_name", DataTableUtils.toString(row["客戶簡稱"]).Trim(), "") + "," +
                //     HtmlUtil.AttibuteValue("date_str", date_str, "") + "," +
                //     HtmlUtil.AttibuteValue("date_end", date_end, "");
                //string href = string.Format("stockanalysis_details.aspx?key={0}  ' ",
                //    WebUtils.UrlStringEncode(url)
                //     );

                value = DataTableUtils.toString(CUST_TOTAL);
                //     value = HtmlUtil.ToTag("u", HtmlUtil.ToHref(value, href));
            }
            if (field_name == "客戶簡稱")
            {
                value = DataTableUtils.toString(row["客戶簡稱"]);
            }
            return "<td>" + value + "</td>\n";
        }
        private void get_SqlConndi()
        {
            //2019/06/21，將天數轉換成日期字串，組合到SQL語法中(ru)
            //default condi => only one
            string[] s = 德大機械.德大專用月份(acc).Split(',');
            sql_condi = "and (SELECT MAX(ITEMIO.TRN_DATE) FROM ITEMIOS LEFT JOIN ITEMIO ON ITEMIOS.IO=ITEMIO.IO AND ITEMIOS.TRN_NO=ITEMIO.TRN_NO WHERE ITEMIOS.IO='2' AND ITEMIOS.MKORD_NO=MKORDSUB.TRN_NO AND ITEMIOS.MKORD_SN=MKORDSUB.SN AND ITEMIO.S_DESC< >'歸還' AND ITEMIO.MK_TYPE='成品入庫') <=" + myclass.date_trn(DataTableUtils.toString(total)) + "";
            title_text = "'庫存期限大於" + DataTableUtils.toString(total) + "天'";
            string date_s = HtmlUtil.changetimeformat(s[0]);
            string date_e = HtmlUtil.changetimeformat(s[1]);
            range = date_s + " ~ " + date_e;

            timerange = DataTableUtils.toString(total);
        }

        //event
        protected void button_select_Click(object sender, EventArgs e)
        {
            if (DataTableUtils.toString(((Control)sender).ID.Split('_')[1]) == "select")
            {
                date_str = txt_str.Text.Replace("-", "");
                date_end = txt_end.Text.Replace("-", "");
                if (RadioButtonList_Type.SelectedValue == "1")
                {
                    sql_condi = "and (SELECT MAX(ITEMIO.TRN_DATE) FROM ITEMIOS LEFT JOIN ITEMIO ON ITEMIOS.IO=ITEMIO.IO AND ITEMIOS.TRN_NO=ITEMIO.TRN_NO WHERE ITEMIOS.IO='2' AND ITEMIOS.MKORD_NO=MKORDSUB.TRN_NO AND ITEMIOS.MKORD_SN=MKORDSUB.SN AND ITEMIO.S_DESC< >'歸還' AND ITEMIO.MK_TYPE='成品入庫') >=" + date_str + "" +
             " and(SELECT MAX(ITEMIO.TRN_DATE) FROM ITEMIOS LEFT JOIN ITEMIO ON ITEMIOS.IO = ITEMIO.IO AND ITEMIOS.TRN_NO = ITEMIO.TRN_NO WHERE ITEMIOS.IO = '2' AND ITEMIOS.MKORD_NO = MKORDSUB.TRN_NO AND ITEMIOS.MKORD_SN = MKORDSUB.SN AND ITEMIO.S_DESC < > '歸還' AND ITEMIO.MK_TYPE = '成品入庫') <= " + date_end + "";
                    title_text = "'" + HtmlUtil.changetimeformat(date_str) + " ~ " + HtmlUtil.changetimeformat(date_end) + "'";
                    Table_Title = HtmlUtil.changetimeformat(date_str) + " ~ " + HtmlUtil.changetimeformat(date_end) + "  庫存數量";
                }
                else if (RadioButtonList_Type.SelectedValue == "0")
                {
                    sql_condi = "and (SELECT MAX(ITEMIO.TRN_DATE) FROM ITEMIOS LEFT JOIN ITEMIO ON ITEMIOS.IO=ITEMIO.IO AND ITEMIOS.TRN_NO=ITEMIO.TRN_NO WHERE ITEMIOS.IO='2' AND ITEMIOS.MKORD_NO=MKORDSUB.TRN_NO AND ITEMIOS.MKORD_SN=MKORDSUB.SN AND ITEMIO.S_DESC< >'歸還' AND ITEMIO.MK_TYPE='成品入庫') <=" + myclass.date_trn(DataTableUtils.toString(total)) + "";
                    title_text = "'庫存期限大於" + DataTableUtils.toString(total) + "天'";
                    Table_Title = "逾期庫存數量";
                }

                GotoCenn();
            }
            else
            {
                string[] s = 德大機械.德大專用月份(acc).Split(',');
                HtmlUtil.Button_Click(DataTableUtils.toString(((Control)sender).ID.Split('_')[1]), s, DataTableUtils.toString(txt_str.Text), DataTableUtils.toString(txt_end.Text), out date_str, out date_end);
                txt_str.Text = HtmlUtil.changetimeformat(date_str, "-");
                txt_end.Text = HtmlUtil.changetimeformat(date_end, "-");
            }


        }

        protected void DropDownList_dayval_SelectedIndexChanged(object sender, EventArgs e)
        {
            sql_condi = "and (SELECT MAX(ITEMIO.TRN_DATE) FROM ITEMIOS LEFT JOIN ITEMIO ON ITEMIOS.IO=ITEMIO.IO AND ITEMIOS.TRN_NO=ITEMIO.TRN_NO WHERE ITEMIOS.IO='2' AND ITEMIOS.MKORD_NO=MKORDSUB.TRN_NO AND ITEMIOS.MKORD_SN=MKORDSUB.SN AND ITEMIO.S_DESC< >'歸還' AND ITEMIO.MK_TYPE='成品入庫') <=" + myclass.date_trn(DataTableUtils.toString(total)) + "";
            title_text = "'庫存期限大於" + DataTableUtils.toString(total) + "天'";
            GotoCenn();
            if (DataTableUtils.toString(((Control)sender).ID.Split('_')[1]) != "select")
            {

            }
        }

        protected void RadioButtonList_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RadioButtonList_Type.SelectedValue == "0")
            {
                PlaceHolder_day.Visible = true;
                PlaceHolder_range.Visible = false;
            }
            else if (RadioButtonList_Type.SelectedValue == "1")
            {
                PlaceHolder_range.Visible = true;
                PlaceHolder_day.Visible = false;
            }
        }
        protected void CheckBox_All_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox_All.Checked == true)
                txt_showCount.Enabled = false;
            else
                txt_showCount.Enabled = true;
        }
    }
}