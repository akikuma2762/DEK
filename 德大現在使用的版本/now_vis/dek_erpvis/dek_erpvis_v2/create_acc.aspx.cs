﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using dek_erpvis_v2.cls;
using Support;
using System.Text.RegularExpressions;

namespace dek_erpvis_v2
{
    public partial class create_acc : System.Web.UI.Page
    {
        myclass myclass = new myclass();
        clsDB_Server clsDB_sw = new clsDB_Server("");
        protected void Page_Load(object sender, EventArgs e)
        {
            Label_code.Text = new Random().Next(9999).ToString();
            InitDropDown();
            Set_DropDownlist();
        }

        private void InitDropDown()
        {
            clsDB_sw.dbOpen(myclass.GetConnByDekVisErp);
            if (clsDB_sw.IsConnected == true)
            {
                DataTable dt = clsDB_sw.GetDataTable("SELECT distinct DPM_NAME,DPM_NAME2 FROM DEPARTMENT where DPM_NAME2 != '系統'");
                if (HtmlUtil.Check_DataTable(dt))
                {
                    ListItem list = new ListItem("-請選擇-", "");
                    DropDownList_depart.Items.Add(list);

                    foreach (DataRow row in dt.Rows)
                    {
                        list = new ListItem(DataTableUtils.toString(row["DPM_NAME2"]), DataTableUtils.toString(row["DPM_NAME"]));
                        DropDownList_depart.Items.Add(list);
                    }
                }
            }
        }
        protected void Button_add_Click(object sender, EventArgs e)
        {
            string acc_ = DataTableUtils.toString(TextBox_Acc.Text.Trim());//.Replace("-", "").Replace("'", "");
            string pwd_ = DataTableUtils.toString(TextBox_Pwd1.Text.Trim());
            bool error = true;

            if (acc_ != "" && pwd_ != "")
            {
                Regex regex = new Regex("[-']");
                error = regex.IsMatch(acc_) || regex.IsMatch(pwd_);
            }
            if (error != true)
            {
                clsDB_sw.dbOpen(myclass.GetConnByDekVisErp);
                string sqlcmd = $"SELECT * FROM USERS where USER_ACC = '{TextBox_Acc.Text}'";
                DataTable dt = clsDB_sw.DataTable_GetTable(sqlcmd);
                if (dt != null && dt.Rows.Count <= 0)
                {
                    sqlcmd = $"SELECT * FROM USERS where USER_NUM = '{TextBox_phone.Text}'";
                    DataTable dr = clsDB_sw.DataTable_GetTable(sqlcmd);
                    if (dr != null && dr.Rows.Count <= 0)
                    {
                        DataRow row = dt.NewRow();
                        row["USER_ID"] = "U" + myclass.get_ran_id();
                        row["USER_ACC"] = TextBox_Acc.Text.ToString();
                        row["USER_NAME"] = TextBox_Name.Text.ToString();
                        row["USER_PWD"] = dekSecure.dekHashCode.SHA256(TextBox_Pwd1.Text.ToString());
                        row["USER_NUM"] = TextBox_phone.Text.ToString();
                        row["USER_DPM"] = DataTableUtils.toString(DropDownList_depart.SelectedValue);
                        row["STATUS"] = "ON";
                        row["ADM"] = "N";
                        row["ADD_TIME"] = DateTime.Now.ToString();
                        row["Belong_Factory"] = DropDownList_Factory.SelectedItem.Value;
                        if (clsDB_sw.Insert_DataRow("USERS", row) == true)
                            Response.Write("<script>alert('伺服器回應 : 已申請成功! 即將跳轉至登入頁!');location.href='../login.aspx';</script>");
                    }
                    else
                        Response.Write("<script>alert('伺服器回應 : 此手機號碼已經有人使用! 請輸入其它號碼!');</script>");
                }
                else
                    Response.Write("<script>alert('伺服器回應 : 此帳號已經有人使用! 請輸入其它帳號!');</script>");
            }
            else
                Response.Write("<script>alert('帳號或密碼包含特殊符號，請重新輸入');</script>");
        }
        private void Set_DropDownlist()
        {
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            string sqlcmd = "SELECT distinct area_name FROM account_info where (area_name <> '全廠' and area_name <> '測試區')  ";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);

            DropDownList_Factory.Items.Clear();
            ListItem listItem = new ListItem();
            listItem = new ListItem("全廠", "全廠");
            DropDownList_Factory.Items.Add(listItem);

            if (HtmlUtil.Check_DataTable(dt) && DropDownList_Factory.Items.Count == 1)
            {
                foreach (DataRow row in dt.Rows)
                {
                    listItem = new ListItem(DataTableUtils.toString(row["area_name"]), DataTableUtils.toString(row["area_name"]));
                    DropDownList_Factory.Items.Add(listItem);
                }
            }
        }
    }
}