using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using dek_erpvis_v2.cls;
using System.Data;
using Support;
using System.IO;
using System.Text.RegularExpressions;

namespace dek_erpvis_v2.pages.dp_PM
{
    public partial class Upload_Image : System.Web.UI.Page
    {
        string type = "";
        string acc = "";
        bool close_yn = false;
        clsDB_Server clsDB_sw = new clsDB_Server("");
        ShareFunction SFun = new ShareFunction();
        myclass myclass = new myclass();
        protected void Page_Load(object sender, EventArgs e)
        {
            // 单击后呼叫photoTotal，photo中將FileUpload1的路径給值到textbox
            this.FileUpload_image.Attributes.Add("onchange", "photoTotal()");
            //隱藏FileUpload1控件 以及 textBox 主要因為如果直接visible = false 則無法附值
            this.FileUpload_image.Style.Add("display", "none");
            this.txtFileUrl.Style.Add("display", "none");

            if (Request.QueryString["ErrorID"] != null)
            {
                string[] str = Request.QueryString["ErrorID"].Split(',');

                type = str[3].Split('=')[1].ToLower();
                if (str[3].Split('=')[1].ToLower() == "ver")
                    DataTableUtils.Conn_String = SFun.GetConnByDekVisTmp;
                else if (str[3].Split('=')[1].ToLower() == "hor")
                    DataTableUtils.Conn_String = SFun.GetConnByDekdekVisAssmHor;
                try
                {
                    if (str[4].Split('=')[1] == "1")
                        close_yn = true;
                }
                catch
                {

                }
                DataRow dr = DataTableUtils.DataTable_GetDataRow("工作站型態資料表", "工作站編號='" + str[1].Split('=')[1] + "'");
                Label_name.Text = dr["工作站名稱"].ToString();//工作站編號
                Label_num.Text = str[0];
                Label4.Text = str[2].Split('=')[1];//ID
                                                   //Label5.Text = acc;
            }
            else if (Request.QueryString["key"] != null)
            {
                string s = Request.QueryString["key"];
                string[] parameter = HtmlUtil.Return_str(Request.QueryString["key"]);
                if (parameter.Length == 10 || parameter.Length == 8)
                {
                    type = parameter[7].ToLower();

                    if (parameter[7].ToLower() == "ver")
                        DataTableUtils.Conn_String = SFun.GetConnByDekVisTmp;
                    else if (parameter[7].ToLower() == "hor")
                        DataTableUtils.Conn_String = SFun.GetConnByDekdekVisAssmHor;
                    try
                    {
                        if (parameter[9] == "1")
                            close_yn = true;

                    }
                    catch
                    {

                    }

                    DataRow dr = DataTableUtils.DataTable_GetDataRow("工作站型態資料表", "工作站編號='" + parameter[3] + "'");
                    Label_name.Text = dr["工作站名稱"].ToString();//工作站編號
                    Label_num.Text = parameter[1];
                    Label4.Text = parameter[5];//ID
                }
                else
                    Response.Redirect(myclass.logout_url);
            }
            else
                Response.Redirect(myclass.logout_url);
        }
        //按鈕事件
        protected void Button_Upload_Click(object sender, EventArgs e)
        {
            string Image_Save = "";
            string local = "";
            if (close_yn == false)
                local = "Backup_Error_Image";
            else
                local = "Backup_File";
            Image_Save = HtmlUtil.FileUpload_Name(FileUpload_image, local);



            //if (FileUpload_image.FileName != "")
            //{
            //    foreach (HttpPostedFile postedFile in FileUpload_image.PostedFiles)
            //    {
            //        string ext = Path.GetExtension(postedFile.FileName).Replace(".", "");
            //        Regex regex = new Regex(@"^PNG|JPG|HEIC|HEIF|JPEG|MP4|XLS|XLSX|PDF|DWG$", RegexOptions.IgnoreCase);
            //        bool ok = regex.IsMatch(ext);
            //        if (ok == true)
            //        {
            //            if (close_yn == false)
            //                Image_Save += checkImage(postedFile, Image_Save, ext, "Backup_Error_Image");
            //            else
            //                Image_Save += checkImage(postedFile, Image_Save, ext, "Backup_File");
            //        }
            //        else
            //        {
            //            string url = HttpContext.Current.Request.Url.AbsoluteUri;
            //            Response.Write("<script>alert('圖片格式錯誤，無法上傳!');location.href='" + url + "'; </script>");
            //            return;
            //        }
            //    }
            //}
            if (Image_Save == "")
                Response.Write("<script>alert('尚未選擇任何檔案!');</script>");
            else
            {
                int Change_ID = Int32.Parse(Label4.Text);
                //確定無誤才上傳資料庫
                if (Label_name.Text != "" && Label_num.Text != "")
                {
                    Upload_Data(Change_ID, Image_Save);
                    Response.Write("<script>alert('上傳完畢!');location.href='../index.aspx';</script>");
                }
            }

        }
        //修改
        private void Upload_Data(int ID, string Image_name)
        {
            if (type == "ver")
                DataTableUtils.Conn_String = SFun.GetConnByDekVisTmp;
            else if (type == "hor")
                DataTableUtils.Conn_String = SFun.GetConnByDekdekVisAssmHor;


            string SqlStr = "select Top 1 * From 工作站異常維護資料表 where 異常維護編號 = '" + ID + "' ORDER BY " + "異常維護編號" + " desc";
            DataTable dt = DataTableUtils.DataTable_GetTable(SqlStr);

            if (HtmlUtil.Check_DataTable(dt))
            {
                DataRow row = dt.NewRow();
                if (close_yn == false)
                    row["圖片檔名"] = Image_name;
                else
                    row["結案附檔"] = Image_name;

                if (type == "ver")
                    clsDB_sw.dbOpen(SFun.GetConnByDekVisTmp);
                else if (type == "hor")
                    clsDB_sw.dbOpen(SFun.GetConnByDekdekVisAssmHor);
                if (clsDB_sw.Update_DataRow("工作站異常維護資料表", "異常維護編號= '" + ID + "'", row) == true)
                {

                }
            }
            else
            {
                DataRow row = dt.NewRow();
                row["異常維護編號"] = ID;
                if (close_yn == false)
                    row["圖片檔名"] = Image_name;
                else
                    row["結案附檔"] = Image_name;

                if (type == "ver")
                    clsDB_sw.dbOpen(SFun.GetConnByDekVisTmp);
                else if (type == "hor")
                    clsDB_sw.dbOpen(SFun.GetConnByDekdekVisAssmHor);

                if (clsDB_sw.Insert_DataRow("工作站異常維護資料表", row) == true)
                {

                }
            }
        }

    }
}