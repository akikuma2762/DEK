using dek_erpvis_v2.cls;
using Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace dek_erpvis_v2.pages.dp_CNC
{
    public partial class Image_Upload : System.Web.UI.Page
    {
        public string Machine = "";
        string acc = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                Machine = Request.QueryString["mach_name"];
            }
            else
                Response.Redirect(myclass.logout_url);

        }

        protected void Button_Upload_Click(object sender, EventArgs e)
        {
            if (HtmlUtil.FileUpload_Name(FileUpload_Image, "CNC_Image", true, $"{Machine}_{DateTime.Now.ToString("yyyyMMddHHmmss")}") != "")
            {
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                string sqlcmd = $"select * from machine_info where mach_name = '{Machine}'";
                DataTable dt = DataTableUtils.GetDataTable(sqlcmd);

                if (HtmlUtil.Check_DataTable(dt))
                {
                    DataRow row = dt.NewRow();
                    row["_id"] = DataTableUtils.toString(dt.Rows[0]["_id"]);
                    row["img_url"] = $"{Machine}_{DateTime.Now.ToString("yyyyMMddHHmmss")}";
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
                    if (DataTableUtils.Update_DataRow("machine_info", $"mach_name = '{Machine}'", row))
                        Response.Write("<script>alert('上傳完畢!');location.href='../index.aspx';</script>");
                    else
                        Response.Write("<script>alert('資料庫存入失敗!');location.href='../index.aspx';</script>");
                }
                else
                    Response.Write("<script>alert('上傳失敗!');location.href='../index.aspx';</script>");
            }
            else
                Response.Write("<script>alert('上傳失敗!');location.href='../index.aspx';</script>");
        }
    }
}