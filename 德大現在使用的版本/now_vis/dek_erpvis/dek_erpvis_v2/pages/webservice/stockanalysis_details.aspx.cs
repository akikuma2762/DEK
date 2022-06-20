﻿using dek_erpvis_v2.cls;
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
    public partial class stockanalysis_details : System.Web.UI.Page
    {
        public string color = "";
        public string th = "";
        public string tr = "";
        public string title = "";
        public string title_msg = "";
        public string title_msg_list = "";
        public string title_text = "";
        public string cust_name = "";
        public string date_str = "";
        public string date_end = "";
        string sql_condi = "";
        string acc = "";
        DataTable public_dt = null;
        clsDB_Server clsDB_sw = new clsDB_Server("");
        myclass myclass = new myclass();
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
               
                color = HtmlUtil.change_color(acc);
                if (acc != "")
                {
                    if (!IsPostBack)
                    {
                        get_SqlConndi();
                        GotoCenn();
                    }
                }
                else
                {
                    Response.Redirect("stockanalysis.aspx", false);
                }


            }
            else

                Response.Redirect("stockanalysis.aspx", false);
   
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
                title_text = HtmlUtil.NoData(out th, out tr);
            }
        }
        private void load_page_data()
        {
            Response.BufferOutput = false;
            if (Request.QueryString["key"] != null)
            {
                string[] str = HtmlUtil.Return_str(Request.QueryString["key"]);
                cust_name = str[1];
                date_str = str[3];
                date_end = str[5];
                get_SqlConndi();
                Set_Html_Table();
            }
            else
            {
                Response.Redirect("stockanalysis.aspx", false);
            }
        }
        private void Set_Html_Table()
        {
            //title
            string strCmd = 德大機械.業務部_成品庫存分析詳細.客戶逾期詳細列表(cust_name, sql_condi);
            public_dt = myclass.Add_LINE_GROUP(clsDB_sw.DataTable_GetTable(strCmd)).ToTable("Table", false, "客戶簡稱", "產線群組", "訂單號碼", "製造批號", "入庫日", "庫存原因");

            string titlename = "";
            th = HtmlUtil.Set_Table_Title(public_dt, out titlename);
            tr = HtmlUtil.Set_Table_Content(public_dt, titlename, stockanalysis_details_callback);
        }
        private string stockanalysis_details_callback(DataRow row, string field_name)//這裡之後要重新寫過，太吃效能了
        {
            string value = "";
            if (field_name == "入庫日")
                value = HtmlUtil.changetimeformat(DataTableUtils.toString(row[field_name]));
            return "<td>" + value + "</td>\n";
        }

        private void get_SqlConndi()
        {
            if (date_end == "")
            {
                //This is a default sql condition  => Just only 1 line.
                sql_condi = "and (SELECT MAX(ITEMIO.TRN_DATE) FROM ITEMIOS LEFT JOIN ITEMIO ON ITEMIOS.IO=ITEMIO.IO AND ITEMIOS.TRN_NO=ITEMIO.TRN_NO WHERE ITEMIOS.IO='2' AND ITEMIOS.MKORD_NO=MKORDSUB.TRN_NO AND ITEMIOS.MKORD_SN=MKORDSUB.SN AND ITEMIO.S_DESC< >'歸還' AND ITEMIO.MK_TYPE='成品入庫') <=" + date_str + "";
            }
            else if (date_end != "")
            {
                //Before selected time array => sql conditions are 2 line.
                sql_condi = "and (SELECT MAX(ITEMIO.TRN_DATE) FROM ITEMIOS LEFT JOIN ITEMIO ON ITEMIOS.IO=ITEMIO.IO AND ITEMIOS.TRN_NO=ITEMIO.TRN_NO WHERE ITEMIOS.IO='2' AND ITEMIOS.MKORD_NO=MKORDSUB.TRN_NO AND ITEMIOS.MKORD_SN=MKORDSUB.SN AND ITEMIO.S_DESC< >'歸還' AND ITEMIO.MK_TYPE='成品入庫') >=" + date_str + "" +
                            "and(SELECT MAX(ITEMIO.TRN_DATE) FROM ITEMIOS LEFT JOIN ITEMIO ON ITEMIOS.IO = ITEMIO.IO AND ITEMIOS.TRN_NO = ITEMIO.TRN_NO WHERE ITEMIOS.IO = '2' AND ITEMIOS.MKORD_NO = MKORDSUB.TRN_NO AND ITEMIOS.MKORD_SN = MKORDSUB.SN AND ITEMIO.S_DESC < > '歸還' AND ITEMIO.MK_TYPE = '成品入庫') <= " + date_end + "";
            }
        }
    }
}