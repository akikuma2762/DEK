using dek_erpvis_v2.cls;
using Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace dek_erpvis_v2.pages.dp_PM
{
    public partial class Asm_ELine : System.Web.UI.Page
    {
        public string th = "";
        public string tr = "";
        public string color = "";
        string acc = "";
        int count = 0;
        string URL_NAME = "";
        myclass myclass = new myclass();
        int all_FinishCount = 0;
        DataTable dt_all = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                URL_NAME = "Asm_ELine";
                color = HtmlUtil.change_color(acc);
                if (myclass.user_view_check(URL_NAME, acc))
                {
                    if (TextBox_Reportday.Text == "")
                        TextBox_Reportday.Text = DateTime.Now.ToString("yyyy-MM-dd");
                    if (TextBox_Actionday.Text == "")
                        TextBox_Actionday.Text = DateTime.Now.ToString("yyyy-MM-dd");
                    if (!IsPostBack)
                        load_data();
                }
                else
                    Response.Write("<script>alert('您無此權限!');location.href='../index.aspx';</script>");
            }
            else
                Response.Redirect(myclass.logout_url);
        }
        //儲存事件
        protected void Button_Save_Click(object sender, EventArgs e)
        {
            int count = 0;
            GlobalVar.UseDB_setConnString(myclass.GetConnByDetaELine);
            string sqlcmd = "";
            int i = 0;
            //啟動
            if (RadioButtonList_Status.SelectedItem.Value == "0")
            {
                sqlcmd = $"select * from DataSource where (Status is null OR Status ='' ) and ProductID = '{TextBox_Product.Text}' and RIGHT(DeliveryTime, 4) = {TextBox_Date.Text}";
                count = DataTableUtils.toInt(TextBox_Count.Text);
            }
            //完成
            else if (RadioButtonList_Status.SelectedItem.Value == "1")
            {
                sqlcmd = $"select * from DataSource where Status ='啟動' and (Executor is not null and Executor <> '') and  (Finisher is null OR Finisher='' ) and ProductID = '{TextBox_Product.Text}' and RIGHT(DeliveryTime, 4) = {TextBox_Date.Text}";
                count = DataTableUtils.toInt(TextBox_Reportcount.Text);
            }
            //出貨
            else if (RadioButtonList_Status.SelectedItem.Value == "2")
            {
                sqlcmd = $"select * from DataSource where Status ='完成' and (Executor is not null and Executor <> '') and  (Finisher is not null and  Finisher <>'' ) and  (ShipmentTime is null OR ShipmentTime='' ) and ProductID = '{TextBox_Product.Text}' and RIGHT(DeliveryTime, 4) = {TextBox_Date.Text}";
                count = DataTableUtils.toInt(TextBox_Shimpment.Text);
            }

            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);

            if (HtmlUtil.Check_DataTable(dt))
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow rew = dt.NewRow();
                    //啟動狀態
                    if (RadioButtonList_Status.SelectedItem.Value == "0")
                    {
                        rew["ID"] = DataTableUtils.toString(row["ID"]);
                        rew["Status"] = "啟動";
                        rew["Executor"] = RadioButtonList_ReportMan.SelectedItem.Text;
                        rew["Acttime"] = TextBox_Actionday.Text.Replace("-", "");
                        rew["ActionAcc"] = acc;
                    }
                    //完工狀態
                    else if (RadioButtonList_Status.SelectedItem.Value == "1")
                    {
                        rew["ID"] = DataTableUtils.toString(row["ID"]);
                        rew["Status"] = "完成";
                        rew["Finisher"] = RadioButtonList_ReportedMan.SelectedItem.Text;
                        rew["FinishTime"] = TextBox_Reportday.Text.Replace("-", "");
                        rew["FinishAcc"] = acc;
                    }
                    else if (RadioButtonList_Status.SelectedItem.Value == "2")
                    {
                        rew["ID"] = DataTableUtils.toString(row["ID"]);
                        rew["ShipmentTime"] = DateTime.Now.ToString("yyyyMMddHHmmss");
                        rew["ShipmentAcc"] = acc;
                    }
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDetaELine);
                    if (DataTableUtils.Update_DataRow("DataSource", $"ID='{DataTableUtils.toString(row["ID"])}'", rew))
                    {
                        GlobalVar.UseDB_setConnString(myclass.GetConnByDetaELine);
                        //存入LOG內
                        sqlcmd = $"select * from dataSourceLog where ID='{DataTableUtils.toString(row["ID"])}'";
                        DataTable dr = DataTableUtils.GetDataTable(sqlcmd);
                        if (dr != null)
                        {
                            bool ok = false;
                            DataRow rows = dr.NewRow();
                            //已有資料存在->狀態為完成
                            if (dr.Rows.Count > 0 && RadioButtonList_Status.SelectedItem.Value == "1")
                            {
                                rows["ID"] = DataTableUtils.toString(row["ID"]);
                                rows["status"] = "完成";
                                rows["Finisher"] = RadioButtonList_ReportedMan.SelectedItem.Text;
                                rows["FinishTime"] = TextBox_Reportday.Text.Replace("-", "");
                                rows["UpdateReportTime"] = DateTime.Now.ToString("yyyyMMddHHmmss");
                                rows["FinishAcc"] = acc;
                                GlobalVar.UseDB_setConnString(myclass.GetConnByDetaELine);
                                ok = DataTableUtils.Update_DataRow("dataSourceLog", $"ID='{DataTableUtils.toString(row["ID"])}'", rows);
                            }
                            //已經有資料存在->狀態為出貨
                            else if (dr.Rows.Count > 0 && RadioButtonList_Status.SelectedItem.Value == "2")
                            {
                                rows["ID"] = DataTableUtils.toString(row["ID"]);
                                rows["ShipmentTime"] = DateTime.Now.ToString("yyyyMMddHHmmss");
                                rows["UpdateShipmentTime"] = DateTime.Now.ToString("yyyyMMddHHmmss");
                                rows["ShipmentAcc"] = acc;
                                GlobalVar.UseDB_setConnString(myclass.GetConnByDetaELine);
                                ok = DataTableUtils.Update_DataRow("dataSourceLog", $"ID='{DataTableUtils.toString(row["ID"])}'", rows);
                            }
                            //新增->狀態為啟動
                            else
                            {
                                rows["ID"] = DataTableUtils.toString(row["ID"]);
                                rows["ProductID"] = DataTableUtils.toString(row["ProductID"]);
                                rows["ProductSn"] = DataTableUtils.toString(row["ProductSn"]);
                                rows["DeliveryTime"] = DataTableUtils.toString(row["DeliveryTime"]);
                                rows["Excutor"] = RadioButtonList_ReportMan.SelectedItem.Text;
                                rows["Acttime"] = TextBox_Actionday.Text.Replace("-", "");
                                rows["UpdateActTime"] = DateTime.Now.ToString("yyyyMMddHHmmss");
                                rows["status"] = "啟動";
                                try
                                {
                                    rows["Insertworkman"] = DataTableUtils.toString(row["Insertworkman"]);
                                }
                                catch
                                {

                                }
                                rows["ActionAcc"] = acc;
                                GlobalVar.UseDB_setConnString(myclass.GetConnByDetaELine);
                                ok = DataTableUtils.Insert_DataRow("dataSourceLog", rows);
                            }

                            if (!ok)
                                lineNotify("", $"LOG_ID：{DataTableUtils.toString(row["ID"])}存檔錯誤 \r\n 步驟為：{RadioButtonList_Status.SelectedItem.Value} \r\n 錯誤訊息如下：{DataTableUtils.ErrorMessage}");
                        }
                        else
                            lineNotify("", $"LOG_ID：{DataTableUtils.toString(row["ID"])}存檔錯誤 \r\n 步驟為：{RadioButtonList_Status.SelectedItem.Value} \r\n 錯誤訊息如下：{DataTableUtils.ErrorMessage}");
                        i++;
                        if (i == count)
                            break;

                    }
                    else
                        lineNotify("", $"DATA_ID：{DataTableUtils.toString(row["ID"])}存檔錯誤 \r\n 步驟為：{RadioButtonList_Status.SelectedItem.Value} \r\n 錯誤訊息如下：{DataTableUtils.ErrorMessage}");
                }
            }
            load_data();
        }
        //新增插單
        protected void Button_Add_Click(object sender, EventArgs e)
        {
            int add_qty = 0;
            int sn = 0;
            GlobalVar.UseDB_setConnString(myclass.GetConnByDetaELine);
            string sqlcmd = "select * from DataSource order by ID desc ";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
            if (HtmlUtil.Check_DataTable(dt))
            {
                GlobalVar.UseDB_setConnString(myclass.GetConnByDetaELine);
                //確認該品號是否在同一天被新增過
                sqlcmd = $"select * from DataSource where ProductID = '{TextBox_AddProductName.Text}' order by ID desc ";
                DataTable ds = DataTableUtils.GetDataTable(sqlcmd);
                if (HtmlUtil.Check_DataTable(ds))
                    sn = DataTableUtils.toInt(DataTableUtils.toString(ds.Rows[0]["ProductSn"]));
                Double max_id = DataTableUtils.toDouble(DataTableUtils.toString(dt.Rows[0]["ID"]));
                int j;
                for (int i = 0; i < DataTableUtils.toInt(TextBox_AddProductTotal.Text); i++)
                {
                    j = i + 1;
                    DataRow row = dt.NewRow();
                    row["ID"] = "" + (max_id + j);
                    row["ProductID"] = TextBox_AddProductName.Text;
                    row["ProductSn"] = "" + (j + sn);
                    row["DeliveryTime"] = TextBox_AddProductDate.Text.Replace("-", "");
                    row["Insertworkman"] = acc;
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDetaELine);
                    if (DataTableUtils.Insert_DataRow("DataSource", row))
                        add_qty++;
                }

                if (add_qty == DataTableUtils.toInt(TextBox_AddProductTotal.Text))
                {
                    TextBox_AddProductDate.Text = "";
                    TextBox_AddProductName.Text = "";
                    TextBox_AddProductTotal.Text = "";
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script>alert('伺服器回應 : 新增成功');</script>");
                }
                else
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script>alert('伺服器回應 : 新增失敗');</script>");
            }
            else
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "message", "<script>alert('伺服器回應 : 新增失敗');</script>");
            load_data();
        }

        //須執行之事件
        private void load_data()
        {
            Set_RadioButtonList();
            Set_DataTable();
        }
        //設定Datatable
        private void Set_DataTable()
        {
            GlobalVar.UseDB_setConnString(myclass.GetConnByDetaELine);
            string sqlcmd = "select distinct DeliveryTime  日期,  RIGHT(DeliveryTime, 4)  DeliveryTime ,UnFinish from DataSource where DeliveryTime <> '總計' order  by 日期 asc ";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
            string title = "";
            th = HtmlUtil.Set_Table_Title(dt, out title, "DeliveryTime");


            title = $"產品編號,{title}總計,完成數量,";

            sqlcmd = "select distinct max(id) ID, ProductID as 產品編號 from DataSource where ProductID <> '總計' group by  ProductID order by ID ";
            dt = DataTableUtils.GetDataTable(sqlcmd);
            if (HtmlUtil.Check_DataTable(dt))
                dt.Columns.Remove("ID");

            sqlcmd = "select * from DataSource";
            dt_all = DataTableUtils.GetDataTable(sqlcmd);

            if (HtmlUtil.Check_DataTable(dt))
            {
                dt.Rows.Add("總計");
                List<string> list = new List<string>(title.Split(','));
                for (int i = 1; i < list.Count - 1; i++)
                {
                    if (int.TryParse(list[i], out _))
                        dt.Columns.Add(list[i].Insert(2, "/"));
                    else
                        dt.Columns.Add(list[i]);
                }

                th = HtmlUtil.Set_Table_Title(dt, out title);
                tr = HtmlUtil.Set_Table_Content(dt, list, Asm_ELine_callback);
            }
            else
                HtmlUtil.NoData(out th, out tr);
        }
        //表格相關設定
        private string Asm_ELine_callback(DataRow row, string field_name)
        {
            string fontcolor = "";
            string value = "";
            if (field_name == "產品編號")
                count = 0;
            string js_code = "";
            int noaction = 0;
            int nofinish = 0;
            int noshipment = 0;
            string startman = "";
            string backgrond_color = "";
            int shipmentcount = 0;
            string tooltip = "";
            int allcount = 0;
            field_name = field_name.Replace("/", "");

            //判斷日子
            if (int.TryParse(field_name, out _))
            {
                //新增後總計會異常，需重新計算
                if (DataTableUtils.toString(row["產品編號"]) != "總計")
                {
                    //計算該日子要做的數量
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDetaELine);
                    string sqlcmd = $"select * from DataSource where ProductID='{DataTableUtils.toString(row["產品編號"])}' and RIGHT(DeliveryTime, 4)={field_name}  ";
                    DataTable dt = DataTableUtils.GetDataTable(sqlcmd);

                    if (HtmlUtil.Check_DataTable(dt))
                    {
                        value = "" + dt.Rows.Count;
                        count += dt.Rows.Count;
                        DataView tmpView = dt.DefaultView;
                        DataTable ds = tmpView.ToTable(true, "Status");
                        List<string> list = new List<string>();
                        foreach (DataRow roww in ds.Rows)
                            list.Add(DataTableUtils.toString(roww["Status"]));

                        if (list.Count == 1 && list.IndexOf("完成") != -1)
                            fontcolor = "green";
                        else if (list.Count == 1 && list.IndexOf("") != -1)
                            fontcolor = "black";
                        //只要含有啟動 ||或是多種情況發生的時候
                        else
                            fontcolor = "blue";

                        if (list.IndexOf("完成") != -1)
                        {
                            ds = dt.DefaultView.ToTable(true, "Finisher");

                            foreach (DataRow rew in ds.Rows)
                            {
                                sqlcmd = $"Finisher ='{DataTableUtils.toString(rew["Finisher"])}' and Finisher is not null and LEN(Finisher)>0 ";
                                DataRow[] rows = dt.Select(sqlcmd);
                                if (rows != null && rows.Length > 0)
                                    tooltip += $" {DataTableUtils.toString(rew["Finisher"])}：{rows.Length} <br/>";
                            }
                            tooltip = $"data-toggle=\"tooltip\" data-html=\"true\" data-placement=\"bottom\"  data-html=\"true\" title=\"\" data-original-title=\"<p style='font-size:15px'> {tooltip} </p>\"";
                        }
                    }
                    else
                        value = "   ";

                    //計算未啟動的數量
                    noaction = Get_Quantity(dt_all, $"ProductID='{DataTableUtils.toString(row["產品編號"])}' and substring(DeliveryTime, 5, 4)={field_name} and (Status is null OR Status = '') and (Executor is null OR Executor = '') and (Acttime is null OR  Acttime= '') ");

                    //計算未完成的數量
                    nofinish = Get_Quantity(dt_all, $"ProductID='{DataTableUtils.toString(row["產品編號"])}' and substring(DeliveryTime, 5, 4)={field_name} and Status ='啟動' and (Executor is not null and Executor <> '') and (Acttime is not null and Acttime <> '' ) and (FinishTime is null OR FinishTime='') ");

                    //計算未出貨數量
                    noshipment = Get_Quantity(dt_all, $"ProductID='{DataTableUtils.toString(row["產品編號"])}' and substring(DeliveryTime, 5, 4)={field_name} and Status = '完成' and (Acttime is not null AND Acttime <> '' ) and (FinishTime is not null AND FinishTime <> '' ) and (ShipmentTime is null OR ShipmentTime = '' ) ");

                    //計算已出貨數量
                    shipmentcount = Get_Quantity(dt_all, $"ProductID='{DataTableUtils.toString(row["產品編號"])}' and substring(DeliveryTime, 5, 4)={field_name} and Status = '完成' and (Acttime is not null AND Acttime <> '' ) and (FinishTime is not null AND FinishTime <> '' ) and (ShipmentTime is not null and ShipmentTime <> '' )   ");

                    //計算全部數量
                    allcount = Get_Quantity(dt_all, $"ProductID='{DataTableUtils.toString(row["產品編號"])}' and substring(DeliveryTime, 5, 4)={field_name}");

                    if (noshipment == 0 && nofinish == 0 && noaction == 0 && shipmentcount > 0)
                        backgrond_color = "yellow";

                    //顯示該品項的啟動人員
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDetaELine);
                    sqlcmd = $"select distinct Executor from DataSource where ProductID='{DataTableUtils.toString(row["產品編號"])}' and RIGHT(DeliveryTime, 4)={field_name} and Status ='啟動' and (Executor is not null and Executor <> '' ) and (Acttime is not null and Acttime <> '') and (FinishTime is null OR FinishTime = '' ) ";
                    dt = DataTableUtils.GetDataTable(sqlcmd);
                    if (HtmlUtil.Check_DataTable(dt))
                        startman = DataTableUtils.toString(dt.Rows[0]["Executor"]);
                    js_code = $"onclick=Calculate_Number('{DataTableUtils.toString(row["產品編號"])}','{field_name}','{noaction}','{nofinish}','{startman}','{noshipment}','{allcount}')  data-toggle = \"modal\" data-target = \"#exampleModal\" ";
                }
                else if (DataTableUtils.toString(row["產品編號"]) == "總計")
                {
                    value = Get_Quantity(dt_all, $" ProductID <> '總計' and substring(DeliveryTime, 5, 4)={field_name} ").ToString();
                    count += DataTableUtils.toInt(value);
                }

            }
            else if (field_name == "總計")
            {
                value = "" + count;
                js_code = "";
            }

            else if (field_name == "完成數量")
            {
                //整體完成數量
                GlobalVar.UseDB_setConnString(myclass.GetConnByDetaELine);
                string sqlcmd = $"select * from DataSource where ProductID = '{DataTableUtils.toString(row["產品編號"])}' and (FinishTime is not null and FinishTime <> '') ";
                DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
                try
                {
                    value = "" + dt.Rows.Count;
                    if (HtmlUtil.Check_DataTable(dt))
                    {
                        DataTable ds = dt.DefaultView.ToTable(true, "Finisher");

                        if (HtmlUtil.Check_DataTable(ds))
                        {
                            foreach (DataRow rew in ds.Rows)
                            {
                                sqlcmd = $"Finisher ='{DataTableUtils.toString(rew["Finisher"])}' and Finisher is not null and LEN(Finisher)>0 ";
                                DataRow[] rows = dt.Select(sqlcmd);
                                if (rows != null && rows.Length > 0)
                                    tooltip += $" {DataTableUtils.toString(rew["Finisher"])}：{rows.Length}<br/>";
                            }
                            tooltip = $"data-toggle=\"tooltip\" data-html=\"true\" data-placement=\"bottom\"  data-html=\"true\" title=\"\" data-original-title=\"<p style='font-size:15px'> {tooltip} </p>\"";
                        }
                    }
                }
                catch
                {
                    value = "0";
                }
                js_code = "";

                all_FinishCount += dt.Rows.Count;
                if (DataTableUtils.toString(row["產品編號"]) == "總計")
                    value = "" + all_FinishCount;
            }

            if (DataTableUtils.toString(row["產品編號"]) == "總計")
                js_code = "";


            if (value == "")
                return "";
            else
                return $"<td align='center' {js_code} style='color:{fontcolor};font-size:25px;background-color:{backgrond_color}' ><b {tooltip}>" + value + "</b></td>";
        }
        //設定RADIOBUTTONLIST的內容
        private void Set_RadioButtonList()
        {
            GlobalVar.UseDB_setConnString(myclass.GetConnByDetaELine);
            string sqlcmd = "select * from ElectricControl_Staff where On_Job = 'Y'";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
            if (HtmlUtil.Check_DataTable(dt))
            {
                List<RadioButtonList> radiolist = new List<RadioButtonList>();
                radiolist.Add(RadioButtonList_ReportMan);
                radiolist.Add(RadioButtonList_ReportedMan);

                for (int i = 0; i < radiolist.Count; i++)
                {
                    if (radiolist[i].Items.Count == 0)
                    {
                        ListItem list = new ListItem();
                        foreach (DataRow row in dt.Rows)
                        {
                            list = new ListItem(DataTableUtils.toString(row["Staff"]), DataTableUtils.toString(row["Staff"]));
                            radiolist[i].Items.Add(list);
                        }
                    }
                }
            }

        }
        //得到數量
        private int Get_Quantity(DataTable dt, string sqlcmd)
        {
            if (dt == null)
                return 0;
            else
            {
                DataRow[] row = dt.Select(sqlcmd);
                if (row != null)
                    return row.Length;
                else
                    return 0;
            }
        }

        //發出LINE提醒修改資料有異常的
        public static void lineNotify(string token, string msg)
        {
            token = WebUtils.GetAppSettings("ELine_Error");//提醒電控的資料存入有問題
            try
            {
                var request = (HttpWebRequest)WebRequest.Create("https://notify-api.line.me/api/notify");
                var postData = string.Format("message={0}", msg);
                var data = Encoding.UTF8.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                request.Headers.Add("Authorization", "Bearer " + token);

                using (var stream = request.GetRequestStream()) stream.Write(data, 0, data.Length);
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}