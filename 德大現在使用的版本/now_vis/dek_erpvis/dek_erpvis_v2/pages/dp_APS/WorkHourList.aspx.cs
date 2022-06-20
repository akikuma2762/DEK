using dek_erpvis_v2.cls;
using Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace dek_erpvis_v2.pages.dp_APS
{
    public partial class WorkHourList : System.Web.UI.Page
    {
        public string order_num = "";
        public string product_name = "";
        public string product_num = "";
        public string color = "";
        string TaskName = "";
        string acc = "";
        public string th = "";
        public string tr = "";
        myclass myclass = new myclass();
        //------------------------------------------事件-----------------------------------------------------
        //網頁載入事件
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                if (TextBox_Start.Text == "" && TextBox_End.Text == "")
                {
                    TextBox_Start.Text = "2019-01-01";
                    TextBox_End.Text = "2200-12-31";
                }
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                color = HtmlUtil.change_color(acc);

                if (!IsPostBack)
                {
                    Load_Data();
                    Set_Table(TextBox_Start.Text, TextBox_End.Text);
                }

            }
            else Response.Redirect(myclass.logout_url);
        }
        //搜尋事件
        protected void Button_Search_Click(object sender, EventArgs e)
        {
            Load_Data();
            string sql = "";
            if (DropDownList_Resource.Text != "All")
                sql = "and Resource = '" + DropDownList_Resource.Text + "'";
            Set_Table(TextBox_Start.Text, TextBox_End.Text, sql);
        }
        //------------------------------------------------------方法----------------------------------------------------------
        //載入相關資訊
        private void Load_Data()
        {
            if (Request.QueryString["key"] != null)
            {
                string Condition = "";
                string KeyParam = Request.QueryString["key"];
                string[] value;
                if (!string.IsNullOrEmpty(KeyParam))
                {
                    value = HtmlUtil.Return_str(KeyParam);
                    if (value.Length == 2)
                    {
                        //from orderList
                        Condition = "Project" + "=" + "'" + value[1] + "'";
                        TaskName = "";
                    }
                    else
                    {
                        //From CNC
                        Condition = "Project" + "=" + "'" + value[1] + "'" + " AND " + value[2] + "=" + "'" + value[3] + "'";
                        TaskName = value[3];
                    }
                    order_num = value[1];

                }
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisAps);
                //顯示所有資訊的DATATABLE
                string sql_cmd = "SELECT * FROM " + ShareMemory.SQLWorkHour + " where " + Condition+ "order by Task";
                DataTable dt = DataTableUtils.GetDataTable(sql_cmd);

                foreach (DataRow row in dt.Rows)
                {
                    product_name = DataTableUtils.toString(row["Job"]);
                    product_num = DataTableUtils.toString(row["JobID"]);
                    break;
                }

            }
            else
                Response.Redirect("OrderList.aspx");
        }
        //載入表格
        private void Set_Table(string start, string end, string type = "")
        {
            string Condition = "";
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisAps);
            double Start_Time = DataTableUtils.toDouble(start.Replace("-", "") + "000000");
            double End_Time = DataTableUtils.toDouble(end.Replace("-", "") + "235959");
            if (string.IsNullOrEmpty(TaskName))
                Condition = "Project=" + "'" + order_num + "'";
            else
                Condition = "Project=" + "'" + order_num + "'" + " AND " + "TaskName=" + "'" + TaskName + "'";
            //顯示所有資訊的DATATABLE
            string sql_cmd = "SELECT id," +
                                    "TaskName as 工藝名稱, " +
                                    "Resource as 機台名稱, " +
                                    "StartTime as 預定起始時間, " +
                                    "EndTime as 預定結束時間, " +
                                    "CurrentPiece as 累積數量, " +
                                    "TargetPiece as 需求總數量 " +
                                    "FROM dek_aps.workhour " +
                                    "where " +
                                    Condition + "and (StartTime >= '" + Start_Time + "' and StartTime <= '" + End_Time + "') " + type +
                                    "or " +
                                    Condition + "and (EndTime >= '" + Start_Time + "' and EndTime <= '" + End_Time + "')" + type;
            DataTable dt = DataTableUtils.GetDataTable(sql_cmd);
            dt.Columns.Add("新增");
            dt.Columns.Add("結束");
            dt.Columns.Add("報工明細");
            List<string> list = new List<string>();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (DataTableUtils.toString(dt.Columns[i]) == "新增" || DataTableUtils.toString(dt.Columns[i]) == "結束")
                    th += "<th></th>";
                else
                    th += "<th>" + dt.Columns[i] + "</th>";
                list.Add(DataTableUtils.toString(dt.Columns[i]));
            }
            list.Add(" ");
            dt.Columns.Add(" ");
            tr = HtmlUtil.Set_Table_Content(dt, list, WorkHourListback);
            if (type == "")
                Give_Dropdownlist(dt);
        }
        //把選項塞入下拉式選單
        private void Give_Dropdownlist(DataTable dt)
        {
            DropDownList_Resource.Items.Clear();
            ListItem list = null;
            list = list = new ListItem("All", "All");
            DropDownList_Resource.Items.Add(list);
            string pre_compony = "";
            foreach (DataRow row in dt.Rows)
            {
                if (pre_compony != DataTableUtils.toString(row["機台名稱"]))
                {
                    list = new ListItem(DataTableUtils.toString(row["機台名稱"]), DataTableUtils.toString(row["機台名稱"]));
                    DropDownList_Resource.Items.Add(list);
                }
                pre_compony = DataTableUtils.toString(row["機台名稱"]);
            }
        }
        //執行callback事件
        private string WorkHourListback(DataRow row, string fieldname)
        {
            string type = "";
            string value = "";
            bool end_YN = false;
            if (fieldname == "報工明細")
            {
                string url = string.Format("WorkHourDetail.aspx?key={0}", WebUtils.UrlStringEncode(HtmlUtil.AttibuteValue("WorkHourID", DataTableUtils.toString(row["id"]), "") + HtmlUtil.AttibuteValue(",Project", order_num, "") + HtmlUtil.AttibuteValue(",TaskName", DataTableUtils.toString(row["工藝名稱"]), "")));
                url = HtmlUtil.ToTag("u", HtmlUtil.ToHref("進入明細", url));
                //按鈕
                value = "<td style='width:7%'>" + url + "</td>\n";
            }
            else if (fieldname == "預定起始時間" || fieldname == "預定結束時間")
            {
                DateTime dt = DateTime.ParseExact(DataTableUtils.toString(row[fieldname]), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                value = "<td>" + dt.ToString("yyyy/MM/dd  HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture) + "</td>\n";
            }
            else if (fieldname == "累積數量")
            {
                string sql_cmd = "SELECT Piece from dek_aps.workhour_detail where Project = '" + order_num + "' and TaskName = '" + DataTableUtils.toString(row["工藝名稱"]) + "'";
                DataTable dt = DataTableUtils.GetDataTable(sql_cmd);
                int total = 0;
                foreach (DataRow rew in dt.Rows)
                    total += DataTableUtils.toInt(DataTableUtils.toString(rew["piece"]));
                value = "<td>" + total.ToString() + "</td>\n";
            }
            else if (fieldname == "新增")
            {

                bool run_end = Check_Segmenttake(order_num, DataTableUtils.toString(row["工藝名稱"]));
                bool ok = Check_DataTable(DataTableUtils.toString(row["工藝名稱"]), DataTableUtils.toString(row["id"]), "中", out type, out end_YN);
                if (end_YN)//完成後無法新增了
                    value = "<td>新增</td>\n";
                else
                {
                    if (run_end)
                    {
                        //已經開始進行動作了，新增跟結束可以使用了
                        if (ok)
                            value = "<td><u><a onclick=Add('" + DataTableUtils.toString(row["id"]) + "','" + order_num + "','" + DataTableUtils.toString(row["工藝名稱"]) + "','" + type + "') data-toggle = \"modal\" data-target = \"#exampleModal_information\">新增</a></u></td>\n";
                        //尚未開始進行作業，新增跟結束不能動作
                        else
                            value = "<td><u><a onclick=start('" + DataTableUtils.toString(row["id"]) + "','" + order_num + "','" + DataTableUtils.toString(row["工藝名稱"]) + "','" + type + "')>開始</a></u></td>\n";
                    }
                    else
                    {
                        if (ok)
                            value = "<td>新增</td>\n";
                        //尚未開始進行作業，新增跟結束不能動作
                        else
                            value = "<td>開始</td>\n";
                    }
                }

            }
            else if (fieldname == "結束")
            {
                bool ok = Check_DataTable(DataTableUtils.toString(row["工藝名稱"]), DataTableUtils.toString(row["id"]), "完成", out type, out end_YN);
                //已經開始進行動作了，新增跟結束可以使用了
                if (end_YN)//完成後無法新增了
                    value = "<td>完成</td>\n";
                else
                {
                    if (!ok)
                        value = "<td>完成</td>\n";//沒有開始的話，不能按下結束
                                                //尚未開始進行作業，新增跟結束不能動作
                    else
                        value = "<td><u><a onclick=Add('" + DataTableUtils.toString(row["id"]) + "','" + order_num + "','" + DataTableUtils.toString(row["工藝名稱"]) + "','" + type + "') data-toggle = \"modal\" data-target = \"#exampleModal_information\">完成</a></u></td>\n";
                }
            }
            return value;
        }
        //確認是否已經開始，以及回傳他的狀態
        private bool Check_DataTable(string Craft_Name, string ID, string status, out string type, out bool end_YN)
        {
            //確認是否已經開始進行動作
            string sql_cmd = "SELECT Piece from dek_aps.workhour_detail where Project = '" + order_num + "' and TaskName = '" + Craft_Name + "'";
            DataTable dt = DataTableUtils.GetDataTable(sql_cmd);
            string now_status = "";
            //判斷目前是加工還是段取
            sql_cmd = "select Task from " + ShareMemory.SQLWorkHour + " where ID = '" + ID + "'";
            DataRow rew = DataTableUtils.DataTable_GetDataRow(sql_cmd);
            int judge_num = DataTableUtils.toInt(DataTableUtils.toString(rew["Task"]));
            //這裡之後要修改--不一定只有段取跟加工
            if (judge_num % 10 > 0)
                now_status = "加工";
            else
                now_status = "段取";

            //判斷是否結束
            sql_cmd = "SELECT Status from " + ShareMemory.SQLWorkHour_Detail + " where Project = '" + order_num + "' and TaskName = '" + Craft_Name + "' order by ID desc limit 1"; ;
            rew = DataTableUtils.DataTable_GetDataRow(sql_cmd);
            try
            {
                if (rew != null)
                {
                    if (DataTableUtils.toString(rew["Status"]) == "MOLDED" || DataTableUtils.toString(rew["Status"]) == "FINISH")
                        end_YN = true;//表示完成
                    else
                        end_YN = false;//表示未完成
                }
                else
                    end_YN = false;//表示未完成
            }
            catch
            {
                end_YN = false;//表示未完成
            }

            sql_cmd = "select StatusEn from " + ShareMemory.SQLWorkHour_MachStatus + " where Status = '" + now_status + status + "'";
            rew = DataTableUtils.DataTable_GetDataRow(sql_cmd);
            try
            {
                type = DataTableUtils.toString(rew["StatusEn"]);
            }
            catch
            {
                type = "";
            }


            if (dt != null && dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
        //新增資料
        protected void Button_Add_Click(object sender, EventArgs e)
        {   /*
             * list[0] WorkHourID
             * list[1] Project
             * list[2] TaskName
             * list[3] Status
             * list[4] 判斷是否為開始-無意義
             */
            
            List<string> list = new List<string>(TextBox_textTemp.Text.Split(','));
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisAps);
            //取得ID最大數
            string sqlcmd = "SELECT ID FROM " + ShareMemory.SQLWorkHour_Detail + " order by ID desc limit 1";
            DataRow rew = DataTableUtils.DataTable_GetDataRow(sqlcmd);
            //按下開始的時候
            sqlcmd = "select * from " + ShareMemory.SQLWorkHour_Detail + " where WorkHourID = '" + list[0] + "' and Project = '" + list[1] + "' and TaskName = '" + list[2] + "'";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
            //判斷是否有超出預設數量
            string alert = ShareFunction_APS.Calculate_QTY(list[1], list[2], TextBox_Qty.Text, DataTableUtils.toInt(list[0]));
            if (string.IsNullOrEmpty(alert) && dt != null)
            {
                DataRow row = dt.NewRow();
                if (dt.Rows.Count != 0)
                    row["ID"] = DataTableUtils.toInt(DataTableUtils.toString(rew["ID"])) + 1;
                else
                    row["ID"] = "0";
                //
                if (list.Count == 5)//開始的數量
                    row["piece"] = 0;
                else
                    row["piece"] = DataTableUtils.toInt(TextBox_Qty.Text);//新增跟結束的數量
                row["Status"] = list[3];
                row["Project"] = list[1];
                row["TaskName"] = list[2];
                row["WorkHourID"] = list[0];
                row["WorkDate"] = DateTime.Now.ToString("yyyy-MM-dd");
                row["WorkTime"] = DateTime.Now.ToString("HH:mm");
                row["StaffName"] = ShareFunction_APS.GetAccName(acc);

                if (DataTableUtils.Insert_DataRow(ShareMemory.SQLWorkHour_Detail, row))
                {
                    //Updata order APS List Status 
                    ShareFunction_APS.UpdataWorkHourData(list[1], list[2], WorkHourEditSource.報工明細, list[3]);
                    //Updata CNC VIS Status
                    ShareFunction_APS.UpdataCNCVisStatus(list[1], list[2], list[3], WorkHourEditSource.報工明細);
                    //Order Status 
                    ShareFunction_APS.UpdataWorkHourOrderData(list[1], list[2], WorkHourEditSource.報工明細, list[3]);
                    // Response.Write("<script>alert('該工藝開始');location.href='WorkHourList.aspx" + Request.Url.Query + "';</script>");
                    Response.Redirect("WorkHourList.aspx" + Request.Url.Query);

                }
                else
                    // Response.Write("<script>alert('失敗');location.href='WorkHourList.aspx" + Request.Url.Query + "';</script>");
                    Response.Redirect("WorkHourList.aspx" + Request.Url.Query);
            }
            else
                Response.Write("<script>alert('數量比目標多" + alert + "件，請重新輸入!');location.href='WorkHourList.aspx" + Request.Url.Query + "'; </script>");

        }
        //判斷該段取是否開始進行
        private bool Check_Segmenttake(string Project, string TaskName)
        {
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisAps);
            string sqlcmd = "select * from workhour where Project = '" + Project + "' and TaskName = '" + TaskName + "'";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
            int task_num = DataTableUtils.toInt(DataTableUtils.toString(dt.Rows[0]["Task"]));

            //表示它為段取
            if (task_num % 10 == 0)
                return true;
            //段取之外
            else
            {
                task_num = task_num / 10;
                //確定它的段取
                task_num = task_num * 10;
                sqlcmd = "select ID from workhour where Project = '" + Project + "' and Task = '" + task_num + "'";
                dt = DataTableUtils.GetDataTable(sqlcmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    sqlcmd = "select * from workhour_detail where WorkHourID = '" + dt.Rows[0]["ID"] + "' order by ID desc limit 1";
                    dt = DataTableUtils.GetDataTable(sqlcmd);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        if (DataTableUtils.toString(dt.Rows[0]["Status"]) == "MOLDED" || DataTableUtils.toString(dt.Rows[0]["Status"]) == "FINISH")
                            return true;
                        else
                            return false;
                    }
                    else
                        return false;
                }
                else
                    return false;

            }
        }

    }
}