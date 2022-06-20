﻿using dek_erpvis_v2.cls;
using Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace dek_erpvis_v2.pages.SYS_CONTROL
{
    public partial class Color_Change : System.Web.UI.Page
    {
        public string give_js = "";
        public string th = "";
        public string tr = "";
        clsDB_Server clsDB_sw = new clsDB_Server("");
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                string acc = DataTableUtils.toString(userInfo["user_ACC"]);
                color_value(acc);
            }
            else { Response.Redirect(myclass.logout_url); }
            Session["check"] = 0;

            th = "<th>測試1</th><th>測試2</th><th>測試3</th> ";
            tr = "<tr><td>123</td><td>123</td><td>123</td></tr><tr><td>456</td><td>456</td><td>456</td></tr>";
        }
        protected void button_select_Click(object sender, EventArgs e)
        {
            Session["back"] = 0;
            Session.Remove("check");
            Response.Write("<script>alert('即將返回參數設定畫面!');location.href='set_parameter.aspx'; </script>");

        }
        private void color_value(string acc)
        {
            clsDB_sw.dbOpen(myclass.GetConnByDekVisErp);
            string sql_cmd = "";
            DataTable dt = null;
            string back = "";
            sql_cmd = "SELECT * FROM Account_Image WHERE Account_name = '" + acc + "' and background = 'custom_person' and RGB_Set <> ' '";
            dt = clsDB_sw.DataTable_GetTable(sql_cmd);
            string[] name = {"Sidebar_Color","GreenB_Color","SidebarText_Color"
                            ,"Label_Color","Background_Color","Column_Color"
                                                        ,"ColumnText_Color","DataTable_Color","DataTableText_Color"};
            if (dt.Rows.Count > 0)//有資料就讀資料
            {
                string value = DataTableUtils.toString(dt.Rows[0]["RGB_Set"]);
                string[] str = value.Split(';');
                for (int i = 0; i < name.Length; i++)
                {
                    for (int j = 0; j < str.Length; j++)
                    {
                        if (name[i] == str[j].Split(',')[0])
                        {
                            back += "'"+str[j].Split(',')[1] + "',";
                            break;
                        }
                    }
                }
                give_js = "colorvalue = [" + back + "];";
            }
            else//沒資料就讀cookie
            {
                give_js = "var x = document.cookie.split(';');\n" +
                          "var colorvalue = [];\n" +
                          "for (var i = 0; i < x.length; i++)\n" +
                          "{\n" +
                                "var item = x[i].trim();\n" +
                                "for (var j = 0; j < colorname.length; j++)\n" +
                                "{\n" +
                                    "if (item.indexOf(colorname[j]) == 0)\n" +
                                        "colorvalue[j] = item.substring(colorname[j].length, item.length);\n" +
                                "}\n" +
                          "}\n";

            }
        }
    }
}