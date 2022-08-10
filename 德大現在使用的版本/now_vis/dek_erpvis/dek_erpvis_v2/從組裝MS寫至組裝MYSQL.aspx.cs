using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Support;

namespace dek_erpvis_v2
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            //立式
            //list.Add("工作班別資料表");
            //list.Add("工作站型態資料表");
            //list.Add("工作站異常狀態資料表");
            //list.Add("工作站異常通知群組資料表");
            //list.Add("工作站異常通知資料表");
            //list.Add("工作站備註資料表");
            //list.Add("工作站結案異常類型資料表");
            //list.Add("工藝名稱資料表");
            //list.Add("工藝流程資料表");
            //list.Add("工藝流程點資料表");
            //list.Add("公司部門資料表");
            //list.Add("公司資料表");
            //list.Add("功能頁面管理資料表");
            //list.Add("自動更新資料表");
            //list.Add("員工工作紀錄資料表");
            //list.Add("員工工藝工程資料表");
            //list.Add("員工工藝名稱資料表");
            //list.Add("員工工藝群組資料表");
            //list.Add("員工資料表");
            //list.Add("排程派工資料表");
            //list.Add("產品特性資料表");
            //list.Add("組裝架資料表");
            //list.Add("組裝特徵資料表");
            //list.Add("組裝資料表_new");
            //list.Add("資料表管理");
            //list.Add("操作狀態資料表");
            //list.Add("機器代號產線表");
            //list.Add("關聯需求資料表");
            //list.Add("權限資料表");
            ////會變動的
            //list.Add("工作站異常維護資料表");
            //list.Add("工作站異常紀錄資料表");
            //list.Add("工作站狀態資料表");
            //list.Add("工作站歷程資料表");
            //list.Add("組裝資料表");
            //list.Add("組裝工藝");
            //string db = "dekvisassm";

            ////臥式
            //list.Add("工作站型態資料表");
            //list.Add("工作站異常狀態資料表");
            //list.Add("工作站異常通知群組資料表");
            //list.Add("工作站異常通知資料表");
            //list.Add("工作站備註資料表");
            //list.Add("工作站結案異常類型資料表");
            //list.Add("工藝名稱資料表");
            //list.Add("工藝流程資料表");
            //list.Add("工藝流程點資料表");
            //list.Add("公司部門資料表");
            //list.Add("公司資料表");
            //list.Add("功能頁面管理資料表");
            //list.Add("自動更新資料表");
            //list.Add("臥式工藝");
            //list.Add("臥式副線");
            //list.Add("員工工作紀錄資料表");
            //list.Add("員工工藝工程資料表");
            //list.Add("員工工藝名稱資料表");
            //list.Add("員工工藝群組資料表");
            //list.Add("員工資料表");
            //list.Add("排程派工資料表");
            //list.Add("組裝架資料表");
            //list.Add("資料表管理");
            //list.Add("操作狀態資料表");
            //list.Add("機器代號產線表");
            //list.Add("關聯需求資料表");
            //list.Add("權限資料表");
            ////會變動的
            //list.Add("工作站異常紀錄資料表");
            //list.Add("工作站歷程資料表");
            //list.Add("組裝資料表");
            //list.Add("工作站異常維護資料表");
            //list.Add("工作站狀態資料表");
            //string db = "detavishor";

            string db = "dekviserp";
            list.Add("assembly_group");
            list.Add("assembly_line");
            list.Add("assembly_line_group");
            list.Add("high_power");



            for (int x = 0; x < list.Count; x++)
            {
                GlobalVar.UseDB_setConnString(clsDB_Server.GetConntionString_MsSQL("172.23.10.106,1433", db, "sa", "dek1234"));
                string sqlcmd = $"select * from {list[x]}";
                DataTable ds = DataTableUtils.GetDataTable(sqlcmd);

                GlobalVar.UseDB_setConnString(clsDB_Server.GetConntionString_MySQL("192.168.80.128", db, "root", "12345678"));
                sqlcmd = $"select * from {list[x]}";
                DataTable dt = DataTableUtils.GetDataTable(sqlcmd);

                foreach (DataRow row in ds.Rows)
                {
                    DataRow rew = dt.NewRow();
                    for (int i = 0; i < ds.Columns.Count; i++)
                    {
                        rew[dt.Columns[i].ToString()] = row[dt.Columns[i].ToString()];

                    }
                    bool ok = DataTableUtils.Insert_DataRow(list[x], rew);
                }

                GlobalVar.UseDB_setConnString(clsDB_Server.GetConntionString_MsSQL("172.23.10.106,1433", db, "sa", "dek1234"));
                sqlcmd = $"select * from {list[x]}";
                ds = DataTableUtils.GetDataTable(sqlcmd);

                GlobalVar.UseDB_setConnString(clsDB_Server.GetConntionString_MySQL("192.168.80.128", db, "root", "12345678"));
                sqlcmd = $"select * from {list[x]}";
                dt = DataTableUtils.GetDataTable(sqlcmd);

                if (ds.Rows.Count == dt.Rows.Count)
                    Label2.Text += list[x] + "複製成功 \n";
                else
                    Label2.Text += list[x] + "複製失敗 \n";
            }





        }

        protected void TextBox2_TextChanged(object sender, EventArgs e)
        {
            Response.Write("<script>alert('" + TextBox2.Text + "')</script>");
        }
    }
}