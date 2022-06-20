using dek_erpvis_v2.cls;
using Support;
using System;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Collections.Generic;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Linq;

namespace dek_erpvis_v2
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        public string color = "";
        string acc = "";
        protected void Page_Load(object sender, EventArgs e)
        {


            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                color = HtmlUtil.change_color(acc);
                Print_Excel("H5BDA-200001", "1");
            }
            else Response.Redirect(myclass.logout_url);
        }
        private void Print_Excel(string Tool_Number, string LineNum)
        {
            DataTable ds = new DataTable();
            ds.Columns.Add("刀庫編號");
            ds.Columns.Add("發起時間");
            ds.Columns.Add("發起人");
            ds.Columns.Add("發起內容");
            ds.Columns.Add("回覆時間");
            ds.Columns.Add("回覆人");
            ds.Columns.Add("回覆內容");

            GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);
            string sqlcmd = $"select 異常維護編號, 工作站名稱,排程編號,維護人員姓名,維護人員單位,維護內容,時間紀錄,父編號,結案判定類型,結案內容 from 工作站異常維護資料表 left join 工作站型態資料表 on 工作站異常維護資料表.工作站編號 = 工作站型態資料表.工作站編號 where 排程編號 = '{Tool_Number}' and(父編號 is null OR 父編號 = 0) ";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    //一開始先加入父編號
                    DataRow rew = ds.NewRow();
                    rew["刀庫編號"] = Tool_Number;
                    rew["發起時間"] = ShareFunction.StrToDate(DataTableUtils.toString(row["時間紀錄"]));
                    rew["發起人"] = DataTableUtils.toString(row["維護人員姓名"]);
                    rew["發起內容"] = DataTableUtils.toString(row["維護內容"]);
                    ds.Rows.Add(rew);
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDekdekVisAssmHor);
                    sqlcmd = $"select * from 工作站異常維護資料表 where 父編號 = {DataTableUtils.toString(row["異常維護編號"])}";
                    DataTable dd = DataTableUtils.GetDataTable(sqlcmd);
                    if (dd != null && dd.Rows.Count > 0)
                    {
                        foreach (DataRow rrw in dd.Rows)
                        {
                            rew = ds.NewRow();
                            //表尚未結案
                            if (DataTableUtils.toString(rrw["結案判定類型"]) == null || DataTableUtils.toString(rrw["結案判定類型"]) == "")
                            {
                                rew["刀庫編號"] = Tool_Number;
                                rew["回覆時間"] = ShareFunction.StrToDate(DataTableUtils.toString(rrw["時間紀錄"]));
                                rew["回覆人"] = DataTableUtils.toString(rrw["維護人員姓名"]);
                                rew["回覆內容"] = DataTableUtils.toString(rrw["維護內容"]);
                            }
                            //表已結案
                            else
                            {
                                rew["刀庫編號"] = Tool_Number;
                                rew["回覆時間"] = ShareFunction.StrToDate(DataTableUtils.toString(rrw["時間紀錄"]));
                                rew["回覆人"] = "[結案類型]：" + DataTableUtils.toString(rrw["結案判定類型"]);
                                if (DataTableUtils.toString(rrw["結案內容"]) == null || DataTableUtils.toString(rrw["結案內容"]) == "")
                                    rew["回覆內容"] = DataTableUtils.toString(rrw["維護內容"]);
                                else
                                    rew["回覆內容"] = DataTableUtils.toString(rrw["結案內容"]);
                            }
                            ds.Rows.Add(rew);
                        }
                    }
                    //用一個空格去做區分
                    rew = ds.NewRow();
                    rew["刀庫編號"] = "";
                    rew["發起時間"] = "";
                    rew["發起人"] = "";
                    rew["發起內容"] = "";
                    rew["回覆時間"] = "";
                    rew["回覆人"] = "";
                    rew["回覆內容"] = "";
                    ds.Rows.Add(rew);
                }
            }

            ToExcel(ds);


        }


        public void ToExcel(DataTable dt)
        {
            int ct = dt.Rows.Count;
      
            using (XLWorkbook wb = new XLWorkbook())
            {
             
                wb.Worksheets.Add(dt, "分頁");


                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", $"attachment;filename={DateTime.Now:yyyyMMdd}.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }

        }
    }
}