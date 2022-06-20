using dek_erpvis_v2.cls;
using Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace dek_erpvis_v2.pages.dp_PM
{
    public partial class Asm_ELine_details : System.Web.UI.Page
    {
        public string th = "";
        public string tr = "";
        public string Finish_th = "";
        public string Finish_tr = "";
        public string point_th = "";
        public string point_tr = "";
        public string color = "";
        string acc, URL_NAME;
        string condition = "";
        public string js_code = "";
        public List<string> html_code = new List<string>();
        myclass myclass = new myclass();
        德大機械 德大機械 = new 德大機械();
        List<string> list = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie userInfo = Request.Cookies["userInfo"];
            if (userInfo != null)
            {
                //一開始先加上去，避免發生BUG
                //已完工
                html_code.Add("active");
                html_code.Add("true");
                html_code.Add("active in");
                //績效表
                html_code.Add("");
                html_code.Add("false");
                html_code.Add("");
                //效能測試用
                URL_NAME = "Asm_ELine_details";
                acc = DataTableUtils.toString(userInfo["user_ACC"]);
                color = HtmlUtil.change_color(acc);
                if (myclass.user_view_check(URL_NAME, acc) == true)
                {
                    html_code.Clear();
                    //已完工
                    html_code.Add("active");
                    html_code.Add("true");
                    html_code.Add("active in");
                    //績效表
                    html_code.Add("");
                    html_code.Add("false");
                    html_code.Add("");

                    string[] s = 德大機械.德大專用月份(acc).Split(',');
                    if (textbox_dt1.Text == "" || textbox_dt2.Text == "")
                    {
                        textbox_dt1.Text = HtmlUtil.changetimeformat(s[0], "-");
                        textbox_dt2.Text = HtmlUtil.changetimeformat(s[1], "-");

                    }
                    HtmlUtil.NoData(out point_th, out point_tr);
                    load_data();
                }
                else
                    Response.Write("<script>alert('您無法瀏覽此頁面 請向該單位主管申請權限!');location.href='../index.aspx';</script>");
            }
            else
                Response.Redirect(myclass.logout_url);
        }
        private void load_data()
        {
            //設定電控產線的人員
            Set_RadioButtonList();
            //已完工列表
            Set_FinishTable();
            //績效表
            Set_PerformanceDataTable();
            //動態產生TextBox給人填寫
           // Show_TextBox();
        }
        //顯示人名的Radiobutton
        private void Set_RadioButtonList()
        {
            GlobalVar.UseDB_setConnString(myclass.GetConnByDetaELine);
            string sqlcmd = "select * from ElectricControl_Staff where Staff is not null and On_Job ='Y'";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);

            if (HtmlUtil.Check_DataTable(dt))
            {
                if (RadioButtonList_Finish.Items.Count == 0)
                {
                    RadioButtonList_Finish.Items.Clear();
                    ListItem list = new ListItem();
                    list = new ListItem("全部", "全部");
                    list.Selected = true;
                    RadioButtonList_Finish.Items.Add(list);

                    foreach (DataRow row in dt.Rows)
                    {
                        list = new ListItem(DataTableUtils.toString(row["Staff"]), DataTableUtils.toString(row["Staff"]));
                        RadioButtonList_Finish.Items.Add(list);
                    }
                }
            }
        }
        //顯示完成的品號及數量
        private void Set_FinishTable()
        {
            GlobalVar.UseDB_setConnString(myclass.GetConnByDetaELine);
            string sqlcmd = "select distinct DeliveryTime as 日期,  RIGHT(DeliveryTime, 4) as DeliveryTime  from DataSource where DeliveryTime <> '總計' order  by 日期 asc ";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
            string title = "";
            th = HtmlUtil.Set_Table_Title(dt, out title, "DeliveryTime");
            title = "產品編號," + title;


            sqlcmd = "select distinct max(id) ID, ProductID as 產品編號 from DataSource where ProductID <> '總計' group by  ProductID order by ID ";
            dt = DataTableUtils.GetDataTable(sqlcmd);
            if (HtmlUtil.Check_DataTable(dt))
                dt.Columns.Remove("ID");

            if (HtmlUtil.Check_DataTable(dt))
            {
                List<string> list = new List<string>(title.Split(','));
                for (int i = 1; i < list.Count - 1; i++)
                {
                    if (int.TryParse(list[i], out _))
                        dt.Columns.Add(list[i].Insert(2, "/"));
                    else
                        dt.Columns.Add(list[i]);
                }

                Finish_th = HtmlUtil.Set_Table_Title(dt, out title);
                Finish_tr = HtmlUtil.Set_Table_Content(dt, title, Asm_ELine_callback);
            }
            else
                HtmlUtil.NoData(out Finish_th, out Finish_tr);
        }
        private string Asm_ELine_callback(DataRow row, string field_name)
        {
            string value = "";
            string background_color = "";
            field_name = field_name.Replace("/", "");
            if (int.TryParse(field_name, out _))
            {
                GlobalVar.UseDB_setConnString(myclass.GetConnByDetaELine);
                string sqlcmd = $"select * from DataSource where ProductID='{DataTableUtils.toString(row["產品編號"])}' and RIGHT(DeliveryTime, 4)={field_name} and (FinishTime is not null and FinishTime <> '') and (Finisher is not null and Finisher <> '') ";
                DataTable dt = DataTableUtils.GetDataTable(sqlcmd);

                if (HtmlUtil.Check_DataTable(dt))
                    value = "" + dt.Rows.Count;

                GlobalVar.UseDB_setConnString(myclass.GetConnByDetaELine);
                sqlcmd = $" select * from DataSource where ProductID='{DataTableUtils.toString(row["產品編號"])}' and RIGHT(DeliveryTime, 4)={field_name} and Status = '完成' and (Acttime is not null AND Acttime <> '' ) and (FinishTime is not null AND FinishTime <> '' ) and (ShipmentTime is not null and ShipmentTime <> '' )  ";
                dt = DataTableUtils.GetDataTable(sqlcmd);
                if (HtmlUtil.Check_DataTable(dt))
                    background_color = "yellow";

            }

            if (value == "")
                return "";
            else
                return $"<td align='center' style='font-size:25px;background-color:{background_color}' ><b>{value}</b></td>";
        }
        //顯示績效
        private void Set_PerformanceDataTable()
        {
            GlobalVar.UseDB_setConnString(myclass.GetConnByDetaELine);
            string sqlcmd = $"select RIGHT(DeliveryTime, 4) as 預交日期,SUBSTRING(FinishTime, 5, 4)  as 報工日期,dataSourceLog.ProductID as 品號,JobName as 品名規格,count(*) as 數量,Finisher as 組裝人員,Acttime as 啟動時間,FinishTime as 完成時間,JobValue as 組裝點數 ,((CAST(count(*) as float)*CAST(JobValue as float))) as 小計 from dataSourceLog left join ElectricalCraft on dataSourceLog.ProductID  = ElectricalCraft.JobID where (FinishTime is not null and FinishTime <> '') and SUBSTRING(FinishTime, 1, 8)>={textbox_dt1.Text.Replace("-", "")}  and SUBSTRING(FinishTime, 1, 8) <={textbox_dt2.Text.Replace("-", "")} {condition}  group by dataSourceLog.ProductID,SUBSTRING(FinishTime, 1, 8),JobName,Finisher,JobValue,DeliveryTime,FinishTime,Acttime";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);

            if (HtmlUtil.Check_DataTable(dt))
            {
                string title = "";
                th = HtmlUtil.Set_Table_Title(dt, out title);
                tr = HtmlUtil.Set_Table_Content(dt, title, Asm_ELine_details_callback);
            }
            else
                HtmlUtil.NoData(out th, out tr);

        }
        private string Asm_ELine_details_callback(DataRow row, string field_name)
        {
            string value = "";
            if (field_name.Contains("時間"))
                value = ShareFunction.StrToDate(DataTableUtils.toString(row[field_name])).ToString("yyyy/MM/dd");
            else if (field_name.Contains("日期"))
                value = DataTableUtils.toString(row[field_name]).Insert(2, "/");
            if (value == "")
                return "";
            else
                return $"<td>{value}</td>";
        }
        //搜尋績效
        protected void button_search_Click(object sender, EventArgs e)
        {
            if (RadioButtonList_Finish.SelectedItem.Text != "全部")
                condition = $" and  Finisher = '{RadioButtonList_Finish.SelectedItem.Text}' ";
            html_code.Clear();
            //已完工
            html_code.Add("");
            html_code.Add("false");
            html_code.Add("");
            //績效表
            html_code.Add("active");
            html_code.Add("true");
            html_code.Add("active in");


            Set_PerformanceDataTable();
        }
        //產生輸入控制項
        private void Show_TextBox()
        {
            GlobalVar.UseDB_setConnString(myclass.GetConnByDetaELine);
            string sqlcmd = "select * from ElectricControl_Staff where On_Job = 'Y'";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
            js_code = "";
            if (HtmlUtil.Check_DataTable(dt))
            {
                string[] daterange = 德大機械.德大專用月份(acc).Split(',');
                //日期起
                PlaceHolder_Performance.Controls.Add(new LiteralControl("<div class=\"col-md-4 col-sm-12 col-xs-12\">\n"));
                PlaceHolder_Performance.Controls.Add(new LiteralControl("<div class=\"col-md-5 col-sm-12 col-xs-12\">\n"));
                Label lb = new Label();
                lb.Text = "起始時間";
                lb.ID = "Labelstart";
                lb.Attributes.Add("style", "margin-top:5px;margin-bottom:5px;margin-left:5%");
                PlaceHolder_Performance.Controls.Add(lb);
                PlaceHolder_Performance.Controls.Add(new LiteralControl("</div>\n"));

                PlaceHolder_Performance.Controls.Add(new LiteralControl("<div class=\"col-md-4 col-sm-12 col-xs-12\">\n"));
                TextBox tx = new TextBox();
                tx.Text = HtmlUtil.changetimeformat(daterange[0], "-");
                tx.TextMode = TextBoxMode.Date;
                tx.ID = "Textstart";
                tx.Attributes.Add("style", "margin-top:5px;margin-bottom:5px;margin-left:5%");
                PlaceHolder_Performance.Controls.Add(tx);
                PlaceHolder_Performance.Controls.Add(new LiteralControl("</div>\n"));
                PlaceHolder_Performance.Controls.Add(new LiteralControl("</div>\n"));
                js_code += $"list+=document.getElementById('ContentPlaceHolder1_Textstart').value+',';\n";

                //日期迄
                PlaceHolder_Performance.Controls.Add(new LiteralControl("<div class=\"col-md-4 col-sm-12 col-xs-12\">\n"));
                PlaceHolder_Performance.Controls.Add(new LiteralControl("<div class=\"col-md-5 col-sm-12 col-xs-12\">\n"));
                lb = new Label();
                lb.Text = "結束時間";
                lb.ID = "LabelEnd";
                lb.Attributes.Add("style", "margin-top:5px;margin-bottom:5px;margin-left:5%");
                PlaceHolder_Performance.Controls.Add(lb);
                PlaceHolder_Performance.Controls.Add(new LiteralControl("</div>\n"));

                PlaceHolder_Performance.Controls.Add(new LiteralControl("<div class=\"col-md-4 col-sm-12 col-xs-12\">\n"));
                tx = new TextBox();
                tx.Text = HtmlUtil.changetimeformat(daterange[1], "-");
                tx.TextMode = TextBoxMode.Date;
                tx.ID = "TextEnd";
                tx.Attributes.Add("style", "margin-top:5px;margin-bottom:5px;margin-left:5%");
                PlaceHolder_Performance.Controls.Add(tx);
                PlaceHolder_Performance.Controls.Add(new LiteralControl("</div>\n"));
                PlaceHolder_Performance.Controls.Add(new LiteralControl("</div>\n"));
                js_code += $"list+=document.getElementById('ContentPlaceHolder1_TextEnd').value+',';\n";

                foreach (DataRow row in dt.Rows)
                {
                    //電控人員
                    PlaceHolder_Performance.Controls.Add(new LiteralControl("<div class=\"col-md-12 col-sm-12 col-xs-12\">\n"));
                    lb = new Label();
                    lb.Text = DataTableUtils.toString(row["Staff"]);
                    lb.ID = "LabelName" + DataTableUtils.toString(row["ID"]);
                    lb.Attributes.Add("style", "color:red");
                    PlaceHolder_Performance.Controls.Add(lb);
                    PlaceHolder_Performance.Controls.Add(new LiteralControl("</div>\n"));
                    js_code += $"list+='{DataTableUtils.toString(row["Staff"])},';\n";
                    GlobalVar.UseDB_setConnString(myclass.GetConnByDetaELine);
                    sqlcmd = "select * from Can_ChangeColumns";
                    DataTable ds = DataTableUtils.GetDataTable(sqlcmd);

                    if (HtmlUtil.Check_DataTable(ds))
                    {
                        foreach (DataRow rew in ds.Rows)
                        {
                            //Label
                            PlaceHolder_Performance.Controls.Add(new LiteralControl("<div class=\"col-md-4 col-sm-12 col-xs-12\">\n"));
                            PlaceHolder_Performance.Controls.Add(new LiteralControl("<div class=\"col-md-5 col-sm-12 col-xs-12\">\n"));
                            lb = new Label();
                            lb.Text = DataTableUtils.toString(rew["Chinese_Name"]);
                            lb.ID = "LabelName" + DataTableUtils.toString(row["ID"]) + DataTableUtils.toString(rew["Codename"]);
                            lb.Attributes.Add("style", "margin-top:5px;margin-bottom:5px;margin-left:5%");
                            PlaceHolder_Performance.Controls.Add(lb);
                            PlaceHolder_Performance.Controls.Add(new LiteralControl("</div>\n"));
                            //TextBox
                            PlaceHolder_Performance.Controls.Add(new LiteralControl("<div class=\"col-md-4 col-sm-12 col-xs-12\">\n"));
                            tx = new TextBox();
                            tx.Text = "";
                            tx.ID = "TextName" + DataTableUtils.toString(row["ID"]) + DataTableUtils.toString(rew["Codename"]);
                            tx.Attributes.Add("style", "margin-top:5px;margin-bottom:5px;margin-left:5%");
                            PlaceHolder_Performance.Controls.Add(tx);
                            PlaceHolder_Performance.Controls.Add(new LiteralControl("</div>\n"));
                            PlaceHolder_Performance.Controls.Add(new LiteralControl("</div>\n"));

                            js_code += $"list+=document.getElementById('ContentPlaceHolder1_TextName{DataTableUtils.toString(row["ID"])}{DataTableUtils.toString(rew["Codename"])}').value+',';\n";
                        }
                    }
                }


                PlaceHolder_Performance.Controls.Add(new LiteralControl("<div class=\"col-md-4 col-sm-12 col-xs-12\">\n"));
                PlaceHolder_Performance.Controls.Add(new LiteralControl("<button id=\"btncalculation\" type=\"button\" class=\"btn btn-primary antosubmit2\">計算</button>\n"));
                PlaceHolder_Performance.Controls.Add(new LiteralControl("</div>\n"));
            }

        }
        protected void button_btncalculation_Click(object sender, EventArgs e)
        {
            list.Clear();
            list = new List<string>(TextBox_Result.Text.Split(','));
            //先產生表格
            DataTable dt = Empty_DataTable(list);

            GlobalVar.UseDB_setConnString(myclass.GetConnByDetaELine);
            string sqlcmd = "select * from ElectricControl_Staff where On_Job = 'Y' ";
            DataTable person = DataTableUtils.GetDataTable(sqlcmd);
            int i = 1;

            if (HtmlUtil.Check_DataTable(person) && list.Count == 24)
            {
                foreach (DataRow row in person.Rows)
                {
                    int staff_local = list.IndexOf(DataTableUtils.toString(row["Staff"]));
                    List<string> value = Get_Point(DataTableUtils.toString(row["Staff"]), list[0], list[1]);
                    double true_point = DataTableUtils.toDouble(value[0]) + ((DataTableUtils.toDouble(value[0]) / 100) * DataTableUtils.toDouble(list[(staff_local + 3)]));
                    DataRow rew = dt.NewRow();
                    rew["序號"] = "電控組裝" + i;
                    rew["月份"] = DataTableUtils.toInt(list[1].Substring(5, 2)) + "月份";
                    rew["組裝人員"] = DataTableUtils.toInt(list[1].Substring(5, 2)) + "月份" + DataTableUtils.toString(row["Staff"]);
                    rew["組裝點數"] = value[0];
                    rew["組裝除外工時另增點數"] = list[(staff_local + 1)];
                    rew["組裝疏失懲處"] = list[(staff_local + 2)];
                    rew["總點數"] = value[0];
                    rew["主管評分(±20%)"] = list[(staff_local + 3)];

                    rew[DataTableUtils.toInt(list[1].Substring(5, 2)) + "月加權後實際點數"] = Math.Round(true_point, 2);
                    rew["上班天數"] = list[(staff_local + 4)];
                    rew["上班時數(天數*8h)"] = DataTableUtils.toDouble(list[(staff_local + 4)]) * 8;
                    rew["加班時數(含週六加班)"] = list[(staff_local + 5)];
                    rew["組裝除外工時記錄表"] = list[(staff_local + 6)];

                    rew["平均點數(點數/工時)"] = Math.Round(true_point / (DataTableUtils.toDouble(list[(staff_local + 4)]) * 8 + (DataTableUtils.toDouble(list[(staff_local + 5)]) * 1.33)), 2);
                    rew["名次(依總點數評比)"] = "";
                    rew["名次"] = "";
                    rew["組裝件數"] = DataTableUtils.toInt(value[1]);

                    dt.Rows.Add(rew);
                    i++;
                }
            }

            i = 1;


            DataView dv_mant = new DataView(dt);
            dv_mant.Sort = " 總點數 desc";
            dt = dv_mant.ToTable();

            foreach (DataRow dr in dt.Rows) // search whole table
            {
                dr["名次(依總點數評比)"] = i;
                if (i == 1)
                    dr["名次"] = "★";
                i++;
            }

            dv_mant = new DataView(dt);
            dv_mant.Sort = " 序號 asc";
            dt = dv_mant.ToTable();
            dt.Rows.Add("",
                        "",
                        "",
                        Calculation_Total(dt, "組裝點數"),
                        "",
                        "",
                                   Calculation_Total(dt, "總點數"),
                        "",
                        "",
                        "",
                        "",
                         Calculation_Total(dt, "加班時數(含週六加班)"),
                        "",
                        "",
                        "",
                        "",
                         Calculation_Total(dt, "組裝件數")); ;//double


            if (HtmlUtil.Check_DataTable(dt))
            {
                string title = "";
                point_th = HtmlUtil.Set_Table_Title(dt, out title);
                point_tr = HtmlUtil.Set_Table_Content(dt, title);
            }
            else
                HtmlUtil.NoData(out point_th, out point_tr);
            PlaceHolder_hide.Visible = true;
        }

        //計算組裝點數
        private List<string> Get_Point(string man, string start, string end)
        {
            List<string> Return_Value = new List<string>();
            GlobalVar.UseDB_setConnString(myclass.GetConnByDetaELine);
            string sqlcmd = $"SELECT (sum(cast(JobValue as DECIMAL(9,2))) ) as 組裝點數 ,count(*) as 組裝件數 	FROM 	dataSourceLog left join ElectricalCraft on ElectricalCraft.JobID = dataSourceLog.ProductID where Finisher is not null and LEN(Finisher)>0 and '{start}000000'<= FinishTime and FinishTime <='{end}235959' and Finisher = '{man}'";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
            if (HtmlUtil.Check_DataTable(dt))
            {
                Return_Value.Add(DataTableUtils.toString(dt.Rows[0]["組裝點數"]));
                Return_Value.Add(DataTableUtils.toString(dt.Rows[0]["組裝件數"]));
            }
            else
            {
                Return_Value.Add("0");
                Return_Value.Add("0");
            }


            return Return_Value;
        }

        //取得最後印出的表格(只有欄位，內容空)
        private DataTable Empty_DataTable(List<string> list)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("序號");
            dt.Columns.Add("月份");
            dt.Columns.Add("組裝人員");
            dt.Columns.Add("組裝點數");
            dt.Columns.Add("組裝除外工時另增點數");
            dt.Columns.Add("組裝疏失懲處");
            dt.Columns.Add("總點數", typeof(double));
            dt.Columns.Add("主管評分(±20%)");
            dt.Columns.Add(DataTableUtils.toInt(list[1].Substring(5, 2)) + "月加權後實際點數");
            dt.Columns.Add("上班天數");
            dt.Columns.Add("上班時數(天數*8h)");
            dt.Columns.Add("加班時數(含週六加班)");
            dt.Columns.Add("組裝除外工時記錄表");
            dt.Columns.Add("平均點數(點數/工時)");
            dt.Columns.Add("名次(依總點數評比)");
            dt.Columns.Add("名次");
            dt.Columns.Add("組裝件數", typeof(double));
            return dt;
        }
        ////計算總和
        private string Calculation_Total(DataTable dt, string columns)
        {
            DataTable ds = dt.DefaultView.ToTable(true, new string[] { columns });
            double value = 0;
            if (HtmlUtil.Check_DataTable(ds))
            {
                foreach (DataRow row in ds.Rows)
                    value += DataTableUtils.toDouble(DataTableUtils.toString(row[columns]));
                return value.ToString();
            }
            else
                return "0";
        }
    }
}