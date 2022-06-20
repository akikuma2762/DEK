using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using dek_erpvis_v2.cls;
using Support;

namespace dek_erpvis_v2.pages.dp_PM
{
    public partial class Upload_MachineImage : System.Web.UI.Page
    {
        string acc = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);

                // 单击后呼叫photoTotal，photo中將FileUpload1的路径給值到textbox
                this.FileUpload_image.Attributes.Add("onchange", "photoTotal()");
                //隱藏FileUpload1控件 以及 textBox 主要因為如果直接visible = false 則無法附值
                this.FileUpload_image.Style.Add("display", "none");
                this.txtFileUrl.Style.Add("display", "none");
            }
            else
                Response.Redirect(myclass.logout_url);
        }

        protected void Button_Upload_Click(object sender, EventArgs e)
        {
            string Machine_road = "";
            string Replace_Mach = "";

            if (DropDownList_Type.SelectedItem.Value == "ver")
            {

                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
                Machine_road = HtmlUtil.FileUpload_Name(FileUpload_image, "Machine_Image_Ver", true);
                Replace_Mach = Machine_road.Replace("Machine_Image_Ver/", "^");
            }

            else if (DropDownList_Type.SelectedItem.Value == "hor")
            {
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);
                Machine_road = HtmlUtil.FileUpload_Name(FileUpload_image, "Machine_Image_Hor", true);
                Replace_Mach = Machine_road.Replace("Machine_Image_Hor/", "^");
            }

            List<string> list = new List<string>(Replace_Mach.Split('^'));
            List<string> Mach = new List<string>(Machine_road.Split('\n'));
            for (int i = 1; i < list.Count; i++)
            {
                string machine = list[i].Split('.')[0];
                if (DropDownList_Type.SelectedItem.Value == "ver")
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
                else if (DropDownList_Type.SelectedItem.Value == "hor")
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);

                string sqlcmd = $"select * from Machine_Image where Ｍachine_Name = '{machine}' ";
                DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
                string message = DataTableUtils.ErrorMessage;

                DataRow row = dt.NewRow();
                //修改等同覆蓋，所以無需動作
                if (dt != null && dt.Rows.Count == 0)
                {
                    row["Ｍachine_Name"] = machine;
                    row["Machine_ImageUrl"] = Mach[i - 1];
                    row["Upload_man"] = acc;
                    if (DropDownList_Type.SelectedItem.Value == "ver")
                        GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssm);
                    else if (DropDownList_Type.SelectedItem.Value == "hor")
                        GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);


                    DataTableUtils.Insert_DataRow("Machine_Image", row);
                }
            }
            if (Machine_road != "")
                Response.Write("<script>alert('新增成功')</script>");
            else
                Response.Write("<script>alert('新增失敗')</script>");
        }
    }
}