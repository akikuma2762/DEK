﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using dek_erpvis_v2.cls;
using System.Data;
using Support;

namespace dek_erpvis_v2.pages.dp_PM
{
    public partial class Asm_Cahrt_Detail : System.Web.UI.Page
    {
        public string color = "";
        public string ColumnsData = "<th class='center'>沒有資料載入</th>";
        public string RowsData = "<tr class='even gradeX'> <td class='center'> no data </ td ></ tr >";
        public string NowYear = "";
        public string NowMonth = "";
        public string NowDay = "";
        public string Key = "";
        public string ErrorType = "";
        public string KeyTitle = "";
        public string SubKeyTitle = "";
        public string ValueTitle = "";
        public string SubKey = "";
        public string Value = "";
        public string TableWrap = "";
        public string FromPageAspx = "";
        public string FromPageTitle = "";
        clsDB_Server clsDB_sw = new clsDB_Server("");
        ShareFunction SFun = new ShareFunction();
        myclass myclass = new myclass();
        private void GotoCenn()
        {
            clsDB_sw.dbOpen(SFun.GetConnByDekVisTmp);
            DataTableUtils.Conn_String = SFun.GetConnByDekVisTmp;
            if (clsDB_sw.IsConnected == true)
                load_page_data();
            else
                Response.Write("<script language='javascript'>alert('伺服器回應 : 無法載入資料，" + clsDB_sw.ErrorMessage + " 請聯絡德科人員或檢查您的網路連線。');</script>");
        }
        private void load_page_data()
        {
            if (GlobalVar.Conn_status == true) //資料庫連線成功要做的事()
                LoadData();
            else   //資料庫連線失敗要做的事()
                Response.Write("<script language='javascript'>alert('伺服器回應 : 無法載入資料，" + " 請聯絡德科人員或檢查您的網路連線。');</script>");
        }
        private void LoadData()
        {
            string[] ColumnsData_array;
            string ss = Request.QueryString["key"];
            //這裡要處理一下
            ColumnsData_array = HtmlUtil.Return_str(Request.QueryString["key"]);
            string url = "";
            if(ColumnsData_array.Length == 7)
            {
                for (int i = 0; i < ColumnsData_array.Length; i++)
                {
                    if (i % 2 == 0)
                    {
                        if (i != ColumnsData_array.Length - 1)
                            url += ColumnsData_array[i] + ",";
                        else
                            url += ColumnsData_array[i];
                    }
                    else
                    {

                        if (i != ColumnsData_array.Length - 1)
                            url += ColumnsData_array[i] + "=";
                        else
                            url += ColumnsData_array[i];
                    }
                }
            }
           else
            {
                for (int i = 1; i < ColumnsData_array.Length; i++)
                {
                    if (i % 2 == 0)
                    {
             
                        if (i != ColumnsData_array.Length - 1)
                            url += ColumnsData_array[i] + "=";
                        else
                            url += ColumnsData_array[i];
                    }
                    else
                    {
                        if (i != ColumnsData_array.Length - 1)
                            url += ColumnsData_array[i] + ",";
                        else
                            url += ColumnsData_array[i];
                    }
                }
            }
            Dictionary<string, string> dc_parameter = SFun.AnsQueryStringChart(Server.UrlDecode(url));
            ColumnsData_array = SFun.GetChartColumnName(dc_parameter["Key"].ToString(), null);
            if (ColumnsData_array[0] != " ")
            {
                ColumnsData = ColumnsData_array[0];
                TableWrap = ColumnsData_array[1];
            }
            GetTableData(dc_parameter);
        }
        private void GetTableData(Dictionary<string, string> _dc_parameter)
        {
            string tr, msg;
            Dictionary<string, string> dc_PageInf = new Dictionary<string, string>();
            tr = SFun.DispatchData(_dc_parameter, dc_PageInf);
            if (tr != null)
            {
                RowsData = tr;
                if (dc_PageInf.Count == 7)
                {
                    KeyTitle = dc_PageInf["KeyTitle"];
                    SubKeyTitle = dc_PageInf["SubKeyTitle"];
                    ValueTitle = dc_PageInf["ValueTitle"];
                    SubKey = dc_PageInf["SubKey"];
                    Value = dc_PageInf["Value"];
                    string[] url = dc_PageInf["FromPageAspx"].Split('?');
                    FromPageAspx = url[0] + Request.Url.Query;
                    FromPageTitle = dc_PageInf["FromPageTitle"];
                }
            }
            else
            {
                msg = "查無此資料，確認輸入資訊正確";
                Response.Write("<script language='javascript'>alert('伺服器回應 : " + msg + "');</script>");
                Nodata();
            }
        }
        private void Nodata()
        {
            ColumnsData = "<th class='center'>沒有資料載入</th>";
            RowsData = "<tr class='even gradeX'> <td class='center'> no data </ td ></ tr >";
        }
        //========================================
        protected void Page_Load(object sender, EventArgs e)
        {
            string acc = "";
            string CompLoacation = "";
            //HttpCookie CompInfo = Request.Cookies["VisCompany"];
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                // string URL_NAME = "Asm_Cahrt_Detail";
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
               
                color = HtmlUtil.change_color(acc);
                //if (CompInfo != null)
                //if (Session["Location"] != null)
                //    CompLoacation = Session["Location"].ToString();
                CompLoacation = ShareFunction.Last_Place(acc);
                // if (myclass.user_view_check(URL_NAME, acc) == true)
                // {
                if (!IsPostBack)
                {
                    if (CompLoacation.ToUpper().Contains("HOR"))
                        SFun.GetConnByDekVisTmp = SFun.GetConnByDekdekVisAssmHor;
                    GotoCenn();
                }

                // }
                // else
                // {
                //     Response.Write("<script>alert('您無法瀏覽此頁面 請向該單位主管主管申請權限!');location.href='../index.aspx';</script>");
                // }

               
            }
            else
            {
                Response.Redirect(myclass.logout_url);
                //Response.Write("<script>alert('您無法瀏覽此頁面 請向該單位主管主管申請權限!');location.href='../index.aspx';</script>");
            }

        }
    }
}