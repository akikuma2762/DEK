﻿using dek_erpvis_v2.cls;
using Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace dek_erpvis_v2.pages.dp_SD
{
    public partial class waitingfortheproduction_details : System.Web.UI.Page
    {
        public string th = "";
        public string tr = "";
        public string cust_sname = "";
        public string date_str = "";
        public string date_end = "";
        string sql_condi = "";
        string acc = "";
        myclass myclass = new cls.myclass();
        clsDB_Server clsDB_sw = new clsDB_Server("");
        DataTable public_dt = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                if (acc != "")
                {
                    if (!IsPostBack)
                    {
                        GotoCenn();
                    }
                }
                else
                {
                    Response.Redirect("waitingfortheproduction.aspx.aspx", false);
                }
            }
            else
            {
                Response.Redirect("waitingfortheproduction.aspx", false);
            }
        }
        //event
        private void GotoCenn()
        {
            clsDB_sw.dbOpen(myclass.GetConnByDetaSowon);
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

        private void load_page_data()
        {
            Response.BufferOutput = false;
            if (Request.QueryString["cust_name"] != null)
            {
                cust_sname = Request.QueryString["cust_name"].Split(',')[0];
                if (Request.QueryString["cust_name"].Split(',').Length > 1)
                {
                    date_str = Request.QueryString["cust_name"].Split(',')[1].Split('=')[1];
                    date_end = Request.QueryString["cust_name"].Split(',')[2].Split('=')[1];
                    sql_condi = " sw_A22_FAB.STR_DATE >= " + date_str + " and sw_A22_FAB.STR_DATE <= " + date_end + "";
                }
                set_table_title();
            }
            else
            {
                Response.Redirect("waitingfortheproduction.aspx", false);
            }
        }

        private void set_table_title()
        {
            //titel
            string col_name = "";
            string sqlcmd = 德大機械.業務部_未生產分析詳細.客戶未生產詳細表(cust_sname, sql_condi);
            clsDB_sw.dbClose();
            clsDB_sw.dbOpen(myclass.GetConnByDekdekVisAssm);
            DataView dv = myclass.Add_LINE_GROUP(clsDB_sw.DataTable_GetTable(sqlcmd));
            clsDB_sw.dbOpen(myclass.GetConnByDetaSowon);
            if (dv.Count > 0)
            {
                public_dt = dv.ToTable("Table", false, "客戶簡稱", "產線群組", "製造批號", "訂單號碼", "客戶訂單", "品號", "訂單日期", "預計開工日", "進度", "狀態", "組裝日");
                for (int i = 0; i < public_dt.Columns.Count; i++)
                {
                    col_name = public_dt.Columns[i].ColumnName;
                    th += "<th>" + col_name + "</th>\n                                            ";
                }
            }
            else
            {
                無資料處理();
            }
            //public_dt.Dispose();
        }

        public string set_table_content()
        {
            tr = "";
            //content
            if (public_dt != null)
            {
                if (public_dt.Rows.Count > 0)
                {
                    foreach (DataRow row in public_dt.Rows)
                    {
                        tr += "<tr>\n";
                        tr += "<td>" + DataTableUtils.toString(row["客戶簡稱"]) + "</td>\n";
                        tr += "<td>" + DataTableUtils.toString(row["產線群組"]) + "</td>\n";
                        tr += "<td>" + DataTableUtils.toString(row["製造批號"]) + "</td>\n";
                        tr += "<td>" + DataTableUtils.toString(row["訂單號碼"]) + "</td>\n";
                        tr += "<td>" + DataTableUtils.toString(row["客戶訂單"]) + "</td>\n";
                        tr += "<td>" + DataTableUtils.toString(row["品號"]) + "</td>\n";
                        tr += "<td>" + DataTableUtils.toString(row["訂單日期"]) + "</td>\n";
                        tr += "<td>" + DataTableUtils.toString(row["預計開工日"]) + "</td>\n";
                        tr += "<td>" + DataTableUtils.toString(row["進度"]) + "</td>\n";
                        tr += "<td>" + DataTableUtils.toString(row["狀態"]) + "</td>\n";
                        tr += "<td>" + DataTableUtils.toString(row["組裝日"]) + "</td>\n";
                        tr += "</tr>\n";
                        Response.Write(tr);
                        Response.Flush();
                        Response.Clear();
                        tr = "";
                    }
                    public_dt.Dispose();
                }
                else
                {
                    無資料處理();
                }
            }
            else
            {
                無資料處理();
                //Response.Redirect("waitingfortheproduction.aspx", false);
            }
            return "";
        }
        protected void 無資料處理()
        {
            th = "<th class='center'>沒有資料載入</th>";
            tr = "<tr class='even gradeX'> <td class='center'> no data </ td ></ tr >";

        }
        //function
        private string get_datetime(int num)
        {
            return DateTime.Now.AddDays(-num).ToString("yyyyMMdd");
        }
        private int DaysBetween(DateTime d1, DateTime d2)
        {
            TimeSpan span = d2.Subtract(d1);
            return (int)span.TotalDays;
        }
        private void get_search_time(string btnID)
        {
            switch (btnID)
            {
                case "day":
                    date_str = get_datetime(0);
                    date_end = get_datetime(0);
                    break;
                case "week":
                    date_str = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + 1).ToString("yyyyMMdd");
                    date_end = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + 5).ToString("yyyyMMdd");
                    break;
                case "month":
                    date_str = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyyMMdd");
                    date_end = new DateTime(DateTime.Now.AddMonths(1).Year, DateTime.Now.AddMonths(1).Month, 1).AddDays(-1).ToString("yyyyMMdd");
                    break;
                case "firsthalf":
                    date_str = DateTime.Now.ToString("yyyy0101");
                    date_end = DateTime.Now.ToString("yyyy0630");
                    break;
                case "lasthalf":
                    date_str = DateTime.Now.ToString("yyyy0701");
                    date_end = DateTime.Now.ToString("yyyy1231");
                    break;
                case "year":
                    date_str = DateTime.Now.ToString("yyyy0101");
                    date_end = DateTime.Now.ToString("yyyy1231");
                    break;
                case "select":
                    string st_m = DataTableUtils.toString(txt_time_str.Value);
                    string ed_m = DataTableUtils.toString(txt_time_end.Value);
                    DateTime dt_st = DateTime.ParseExact(st_m, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime dt_ed = DateTime.ParseExact(ed_m, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                    if (DaysBetween(dt_st, dt_ed) > 730)
                    {
                        Response.Write("<script language='javascript'>alert('伺服器回應 : 日期起始不得大於 730 天 !');</script>");
                    }
                    else
                    {
                        date_str = dt_st.ToString("yyyyMMdd");
                        date_end = dt_ed.ToString("yyyyMMdd");
                    }
                    break;
            }
        }
        protected void button_select_Click(object sender, EventArgs e)
        {
            get_search_time(DataTableUtils.toString(((Control)sender).ID.Split('_')[1]));
            sql_condi = " sw_A22_FAB.STR_DATE >= " + date_str + " and sw_A22_FAB.STR_DATE <= " + date_end + "";
            GotoCenn();
            txt_time_str.Value = "";
            txt_time_end.Value = "";
        }
    }
}