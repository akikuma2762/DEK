using dek_erpvis_v2.cls;
using Support;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace dek_erpvis_v2.cls
{
    public class HtmlUtil
    {
        public static string GetConnByDekVisErp = myclass.GetConnByDekVisErp;
        //把等號兩邊做成制式化格式
        public static string AttibuteValue(string item, string value, string quotsymbol = "'")
        {
            return $"{item}={quotsymbol}{value}{quotsymbol}";
        }
        //把文字加底線拉、斜體等等
        public static string ToTag(string tagname, string value)
        {
            return $"<{tagname}>{value}</{tagname}>";
        }
        //html的格式
        public static string ToHref(string text, string href, string target = "", string title = "")
        {
            // <a href="要連結的 URL 放這裡" target="連結目標" title="連結替代文字"> 要顯示的連結文字或圖片放這裡 </a>
            string result = "<a " + AttibuteValue("href", href);
            if (target != "")
                result += AttibuteValue("target", target);
            if (title != "")
                result += AttibuteValue("title", title);
            result += string.Format(" >{0}</a>", text);
            return result;
        }
        //印出每個欄位的名稱(dt→表格,field_name→迴圈的欄位名稱,Add_title→與輸出表格不符合時，要新增的欄位名稱)
        public static string Set_Table_Title(DataTable dt, out string field_name, string Add_title = "", string align = "")//Add_title→要印出之列的欄位名稱
        {
            if (Check_DataTable(dt))
            {
                string th = "";
                string title_name = "";//紀錄每個資料欄位名稱用
                string col_name = "";
                //dt與輸出表格相符
                if (Add_title == "")
                {
                    //沒有資料的處理方式
                    if (dt.Rows.Count <= 0)
                    {
                        th = "<th class=\"center\">沒有資料載入</th>";
                        title_name = "";
                    }
                    else
                    {
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            col_name = dt.Columns[i].ColumnName;
                            th += $"<th {align}>{col_name}</th>";
                            title_name += col_name + ",";
                        }
                    }
                }
                //dt與輸入表格不符(要新增欄位名稱)
                else
                {
                    //沒有資料的處理方式
                    if (dt.Rows.Count <= 0)
                    {
                        th = "<th class=\"center\">沒有資料載入</th>";
                        title_name = "";
                    }
                    else
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            if (row[Add_title] != null)
                            {
                                if (col_name != DataTableUtils.toString(row[Add_title]))
                                {
                                    col_name = DataTableUtils.toString(row[Add_title]);
                                    title_name += col_name + ",";
                                    th += $"<th {align}>{col_name}</th>";
                                }
                            }
                        }
                    }
                }

                field_name = title_name;
                return th;
            }
            else
            {
                field_name = "";
                return "<th class=\"center\">沒有資料載入</th>";
            }


        }
        public static string Set_Table_Title(List<string> list)
        {
            string th = "";
            for (int i = 0; i < list.Count - 1; i++)
                th += $"<th>{list[i]}</th>";
            return th;
        }
        //利用表格以及字串去獲取每個欄位內容(dt→表格,第二個是欄位名稱,callback事件)
        public static string Set_Table_Content(DataTable dt, string[] TitleList, Func<DataRow, string, string> callback = null, string align = "")
        {
            string tr = "";
            string result = "";
            string field_value;
            if (Check_DataTable(dt))
            {
                foreach (DataRow row in dt.Rows)
                {
                    result = "";
                    for (int i = 0; i < TitleList.Length - 1; i++)
                    {
                        field_value = "";
                        if (callback != null)
                            field_value = callback(row, TitleList[i]);
                        if (field_value == "" || field_value == "<td></td>")
                        {
                            if (TitleList[i] != "備註")
                                field_value = $"<td {align}>{DataTableUtils.toString(row[TitleList[i]])}</td>";
                        }
                        else if (field_value == "1")
                            field_value = "";

                        result += field_value;
                    }

                    if (result != "")
                    {
                        tr += "<tr>";
                        tr += result;
                        tr += "</tr>";
                    }

                }
            }
            else
                tr = "<tr> <td class=\"center\"> no data </td></tr>";
            return tr;
        }
        public static string Set_Table_Content(DataTable dt, string Title_comma_text, Func<DataRow, string, string> callback = null, string align = "")
        {
            if (Check_DataTable(dt))
                return Set_Table_Content(dt, Title_comma_text.Split(','), callback, align);
            else
                return "<tr> <td class=\"center\"> no data </td></tr>"; ;
        }
        public static string Set_Table_Content(DataTable dt, List<string> TitleList, Func<DataRow, string, string> callback = null, string align = "")
        {
            if (Check_DataTable(dt))
                return Set_Table_Content(dt, TitleList.ToArray(), callback, align);
            else
                return "<tr> <td class=\"center\"> no data </td></tr>";
        }
        //計算兩日期間的差距
        public static int DaysBetween(DateTime date_str, DateTime date_end)
        {
            TimeSpan span = date_end.Subtract(date_str);
            return (int)span.TotalDays;
        }
        //沒有資料時顯示
        public static string NoData(out string th, out string tr)
        {
            th = "<th>沒有資料載入</th>";
            tr = "<tr><td>no data</td></tr>";
            return "'沒有資料'";
        }
        public static string NoData(out StringBuilder th, out StringBuilder tr)
        {
            StringBuilder ta = new StringBuilder();
            StringBuilder tb = new StringBuilder();

            ta.Append("<th>沒有資料載入</th>");
            tb.Append("<tr><td>no data</td></tr>");

            th = ta;
            tr = tb;

            return "'沒有資料'";
        }
        //繪製長條圖(dt→表格,x_value→x軸,y_value→y軸,unit→單位,backvalue→合計數量/金額)
        /// <summary>
        /// 產生長條圖
        /// </summary>
        /// <param name="dt">資料來源</param>
        /// <param name="x_value">X軸文字</param>
        /// <param name="y_value">Y軸文字</param>
        /// <param name="unit">單位</param>
        /// <param name="backvalue">Y軸總和</param>
        /// <param name="LineName">StackColumn用，需先回傳上一個順序</param>
        /// <param name="LineNum">StackColumn用，依據上一個順序呈現</param>
        /// <param name="show_number">是否要在圖表上呈現文字</param>
        /// <returns></returns>
        public static string Set_Chart(DataTable dt, string x_value, string y_value, string unit, out int backvalue, out List<string> LineName, List<string> LineNum = null, bool show_number = true)
        {
            if (!Check_DataTable(dt))
            {
                backvalue = 0;
                LineName = null;
                return "";
            }


            List<string> Returt_Line = new List<string>();
            //dt重複值合併
            DataTable ds = new DataTable();
            for (int i = 0; i < dt.Columns.Count; i++)
                ds.Columns.Add(DataTableUtils.toString(dt.Columns[i]));

            DataTable Line = dt.DefaultView.ToTable(true, new string[] { x_value });
            double count = 0;
            string sqlcmd = "";
            foreach (DataRow row in Line.Rows)
            {
                DataRow rew = ds.NewRow();
                count = 0;
                sqlcmd = $"{x_value}='{DataTableUtils.toString(row[x_value])}'";
                DataRow[] rows = dt.Select(sqlcmd);
                for (int i = 0; i < rows.Length; i++)
                    count += DataTableUtils.toDouble(DataTableUtils.toString(rows[i][y_value]));

                for (int i = 0; i < ds.Columns.Count; i++)
                {
                    object obj = ds.Columns[i];

                    if (DataTableUtils.toString(ds.Columns[i]) == y_value)
                        rew[ds.Columns[i]] = count;
                    else
                        rew[ds.Columns[i]] = rows[0][DataTableUtils.toString(ds.Columns[i])];
                }
                ds.Rows.Add(rew);
            }

            dt = ds;


            string value = "";
            string x_text;
            string y_text;
            int add_value = 0;
            if (LineNum == null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string index = "";
                    x_text = DataTableUtils.toString(row[x_value]);
                    y_text = DataTableUtils.toString(row[y_value]).Split('.')[0];
                    add_value += DataTableUtils.toInt(y_text);
                    if (show_number)
                        index = ",indexLabel:'" + y_text + unit + "'";
                    if (y_text != "0")
                        value += "{ " +
                                $"y: {y_text}, label: '{Change_ColumnName(x_text)}' {index} " +
                                 "},";
                    else
                        value += "{" +
                                $" y: {y_text}, label: '{Change_ColumnName(x_text)}' " +
                                 "},";

                    Returt_Line.Add(Change_ColumnName(x_text));
                }
            }
            else
            {
                //再算前n名的有異常
                for (int i = 0; i < LineNum.Count; i++)
                {
                    sqlcmd = $"{x_value}='{LineNum[i]}'";
                    DataRow[] row = dt.Select(sqlcmd);
                    if (row != null && row.Length > 0)
                    {
                        string index = "";
                        x_text = DataTableUtils.toString(row[0][x_value]);
                        y_text = DataTableUtils.toString(row[0][y_value]).Split('.')[0];
                        if (show_number)
                            index = ",indexLabel:'" + y_text + unit + "'";


                        if (y_text != "0")
                            value += "{ y: " + y_text + ", label: '" + Change_ColumnName(x_text) + "' " + index + " },";
                        else
                            value += "{ y: " + y_text + ", label: '" + Change_ColumnName(x_text) + "' },";
                    }
                    else
                        value += "{ y: 0, label: '" + Change_ColumnName(LineNum[i]) + "' },";
                }
                foreach (DataRow row in dt.Rows)
                    add_value += DataTableUtils.toInt((DataTableUtils.toString(row[y_value])).Split('.')[0]);
            }


            LineName = Returt_Line;
            backvalue = add_value;
            return value;
        }
        //繪製長條圖(dt→表格,x_value→x軸,y_value→y軸,unit→單位,backvalue→合計數量/金額)
        public static string Set_Chart(DataTable dt, string x_value, string y_value, string unit, out int backvalue, int total = 0)
        {
            //dt重複值合併
            DataTable ds = new DataTable();
            for (int i = 0; i < dt.Columns.Count; i++)
                ds.Columns.Add(DataTableUtils.toString(dt.Columns[i]));

            DataTable Line = dt.DefaultView.ToTable(true, new string[] { x_value });
            double count = 0;
            string sqlcmd = "";
            foreach (DataRow row in Line.Rows)
            {
                DataRow rew = ds.NewRow();
                count = 0;
                sqlcmd = x_value + " ='" + DataTableUtils.toString(row[x_value]) + "'";
                DataRow[] rows = dt.Select(sqlcmd);
                for (int i = 0; i < rows.Length; i++)
                    count += DataTableUtils.toDouble(DataTableUtils.toString(rows[i][y_value]));

                for (int i = 0; i < ds.Columns.Count; i++)
                {
                    object obj = ds.Columns[i];

                    if (DataTableUtils.toString(ds.Columns[i]) == y_value)
                        rew[ds.Columns[i]] = count;
                    else
                        rew[ds.Columns[i]] = rows[0][DataTableUtils.toString(ds.Columns[i])];
                }
                ds.Rows.Add(rew);
            }

            dt = ds;


            string value = "";
            string x_text;
            string y_text;
            int add_value = 0;
            foreach (DataRow row in dt.Rows)
            {
                x_text = DataTableUtils.toString(row[x_value]);
                y_text = DataTableUtils.toString(row[y_value]).Split('.')[0];
                add_value += DataTableUtils.toInt(y_text);
                value += "{ y: " + y_text + ", label: '" + x_text + "',indexLabel:' " + y_text + unit + "' },";
            }
            backvalue = add_value;
            return value;
        }
        //按鈕的事件->之後可以砍了
        public static string Button_Click(string btnID, string[] s, string txt_time_str, string txt_time_end, out string date_str, out string date_end)
        {
            string dt_st = "";
            string dt_ed = "";
            string wairning = "";
            switch (btnID)
            {
                case "week":
                    dt_st = DateTime.Now.Date.AddDays(-(int)(DateTime.Now.DayOfWeek) + 1).ToString("yyyyMMdd");//当前周的开始日期
                    dt_ed = DateTime.Now.Date.AddDays(7 - (int)(DateTime.Now.DayOfWeek)).ToString("yyyyMMdd");//当前周的结束日期
                    break;
                case "month":
                    dt_st = s[0];
                    dt_ed = s[1];
                    break;
                case "firsthalf":
                    dt_st = DateTime.Now.ToString("yyyy0101");
                    dt_ed = DateTime.Now.ToString("yyyy0630");
                    break;
                case "lasthalf":
                    dt_st = DateTime.Now.ToString("yyyy0701");
                    dt_ed = DateTime.Now.ToString("yyyy1231");
                    break;
                case "year":
                    dt_st = DateTime.Now.ToString("yyyy0101");
                    dt_ed = DateTime.Now.ToString("yyyy1231");
                    break;
                case "select":
                    DateTime d_st = DateTime.ParseExact(DataTableUtils.toString(txt_time_str), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime d_ed = DateTime.ParseExact(DataTableUtils.toString(txt_time_end), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                    dt_st = d_st.ToString("yyyyMMdd");
                    dt_ed = d_ed.ToString("yyyyMMdd");
                    break;
            }
            date_str = dt_st;
            date_end = dt_ed;
            return wairning;
        }
        //轉換日期顯示格是
        public static string changetimeformat(string date, string symbol = "/")
        {
            if (date != "" && date.Length == 8)
            {
                date = date.Insert(6, symbol);
                date = date.Insert(4, symbol);
            }
            return date;
        }
        //找到使用者選擇的CSS  然後展示出來
        public static string change_color(string acc)
        {
            clsDB_Server clsDB = new clsDB_Server(GetConnByDekVisErp);
            string color = "";
            clsDB.dbOpen(myclass.GetConnByDekVisErp);
            string sqlcmd = $"SELECT * FROM Account_Image where Account_name = '{acc}' and Image_link is null";
            DataTable dt = clsDB.DataTable_GetTable(sqlcmd);
            if (Check_DataTable(dt))
                color = DataTableUtils.toString(dt.Rows[0]["background"]);
            else
                color = "custom";

            if (color == "custom")
                return $"<link href=\"../../assets/build/css/{color}.css\" rel=\"stylesheet\" /> \n <link href=\"../../assets/build/css/Change_Table_Button.css\" rel=\"stylesheet\" /> \n ";
            else if (color == "custom_old")
                return $"<link href=\"../../assets/build/css/{color}.css\" rel=\"stylesheet\" /> \n <link href=\"../../assets/build/css/Change_Table_Button_old.css\" rel=\"stylesheet\"  /> \n ";
            else
                return $"<link href=\"../../assets/build/css/{color}.css\" rel=\"stylesheet\" /> \n <link href=\"../../assets/build/css/Change_Table_Button_person.css\" rel=\"stylesheet\" /> \n ";
        }

        //回傳字串分割的陣列
        public static string[] Return_str(string value, string key = "")
        {
            string keyword = ConfigurationManager.AppSettings["URL_ENCODE"];
            string[] str = null;
            if (keyword == "1")
            {
                if (key == "")
                    value = WebUtils.UrlStringDecode(value);
            }
            value = value.Trim();
            value = value.Replace(",", "^").Replace("=", "^");
            str = value.Split('^');
            return str;
        }
        /// <summary>
        /// 把Request.Query[""]轉成dictionary，value->Request.Query[""]
        /// </summary>
        /// <param name="value"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Dictionary<string, string> Return_dictionary(string value, string key = "")
        {
            Dictionary<string, string> url_dictionary = new Dictionary<string, string>();
            string keyword = ConfigurationManager.AppSettings["URL_ENCODE"];
            List<string> list = new List<string>();
            if (keyword == "1")
            {
                if (key == "")
                    value = WebUtils.UrlStringDecode(value);
            }
            value = value.Trim();
            value = value.Replace(",", "^").Replace("=", "^");
            list = new List<string>(value.Split('^'));
            for (int i = 0; i < list.Count; i++)
            {
                if (i % 2 == 0)
                    url_dictionary.Add(list[i], list[i + 1]);
            }
            return url_dictionary;
        }
        //查詢Dictonary是否存在，若不存在，則回傳空值
        public static string Search_Dictionary(Dictionary<string, string> keyValues, string value)
        {
            try
            {
                return keyValues[value];
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 確認可上傳之檔案
        /// </summary>
        /// <returns></returns>
        public static string Check_File()
        {
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisErp);
            string sqlcmd = "SELECT Name from File_Extension where Open_YN = 'Y'";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
            string file_name = "";
            if (Check_DataTable(dt))
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                    file_name += i == 0 ? DataTableUtils.toString(dt.Rows[i]["Name"]) : $"|{DataTableUtils.toString(dt.Rows[i]["Name"])}";
            }
            return file_name;
        }
        /// <summary>
        /// 查詢帳號對應的欄位(通常查user)
        /// </summary>
        /// <param name="acc">帳號</param>
        /// <param name="field">欲查之column</param>
        /// <returns></returns>
        public static string Search_acc_Column(string acc, string field = "power")
        {
            if (acc != "")
            {
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisErp);
                string sqlcmd = $"Select * From Users where User_acc = '{acc}'";
                DataTable dt = DataTableUtils.GetDataTable(sqlcmd);
                if (Check_DataTable(dt))
                    return DataTableUtils.toString(dt.Rows[0][field]);
                else
                    return "";
            }
            else
                return "";
        }

        /// <summary>
        /// 上傳圖片用
        /// </summary>
        /// <param name="file">FileUpload元件</param>
        /// <param name="road">上傳的路徑</param>
        /// <param name="recover">是否可覆蓋</param>
        /// <param name="Machine">機台照片名稱</param>
        /// <returns></returns>
        public static string FileUpload_Name(FileUpload file, string road, bool recover = false, string Machine = "")
        {
            string Image_Save = "";
            if (file.FileName != "")
            {
                //表示不要被覆蓋(例如異常圖片之類的不可覆蓋)
                if (!recover)
                {
                    foreach (HttpPostedFile postedFile in file.PostedFiles)
                    {
                        string ext = Path.GetExtension(postedFile.FileName).Replace(".", "");
                        Regex regex = new Regex(@"^" + Check_File() + "$", RegexOptions.IgnoreCase);
                        bool ok = regex.IsMatch(ext);
                        if (ok == true)
                            Image_Save += checkImage(postedFile, Image_Save, ext, road);
                        else
                            return "檔案有誤，請重新上傳";
                    }
                }
                else//表示可以覆蓋(像是機台或是機型照片那種唯一的)
                {
                    foreach (HttpPostedFile postedFile in file.PostedFiles)
                    {
                        string path = $"{WebConfigurationManager.AppSettings["disk"]}:\\{road}\\";
                        string ext = Path.GetExtension(postedFile.FileName).Replace(".", "");
                        Regex regex = new Regex(@"^" + Check_File() + "$", RegexOptions.IgnoreCase);
                        bool ok = regex.IsMatch(ext);
                        if (ok)
                        {
                            if (Machine != "")
                                postedFile.SaveAs($"{path}{Machine}.jpg");
                            else
                                postedFile.SaveAs(path + postedFile.FileName);

                            Image_Save += $"{road}/{postedFile.FileName}\n";
                        }
                    }
                    return Image_Save;
                }
            }
            else
                return "";

            return Image_Save;

        }
        static string checkImage(HttpPostedFile postedFile, string Image_Save, string ext, string local)
        {
            string replace_name = "";
            string image_name = "";//最後回傳的
            string name = "";
            string path2 = $"{WebConfigurationManager.AppSettings["disk"]}:\\{local}\\";
            if (ext != "")
            {
                //找到目前時間
                string timenow = DateTime.Now.ToString().Replace('/', '-').Replace(' ', '_').Replace(':', '-');
                checkName(timenow, Image_Save, out replace_name);
                //加上副檔名
                replace_name += "." + ext;
                postedFile.SaveAs(path2 + replace_name);
                name = $"{local}/{replace_name}";
                image_name = $"{name}\n";
            }
            return image_name;
        }
        static string checkName(string name, string Image_Save, out string replace_name, int num = 1)
        {
            if (Image_Save.IndexOf(name) > 0)
            {
                name = name.Split('(')[0];
                name = $"{name}({num})";
                num++;
                checkName(name, Image_Save, out replace_name, num);
            }
            else
                replace_name = name;
            return name;
        }

        //展示SQL語法用(and,OR)
        public static string Combine_SQL(List<string> list, string type, string columns)
        {
            string sqlcmd = "";

            for (int i = 0; i < list.Count; i++)
                sqlcmd += i == 0 ? $" {columns} = '{list[i]}' " : $" {type} {columns} = '{list[i]}' ";
            return sqlcmd != "" ? $" and ( {sqlcmd} ) " : "";
        }
        /// <summary>
        /// 檢查該資料表是否為空
        /// </summary>
        /// <param name="dt">資料表</param>
        /// <returns></returns>
        public static bool Check_DataTable(DataTable dt)
        {
            if (dt == null)
                return false;
            else if (dt.Rows.Count == 0)
                return false;

            string data = "";
            try
            {
                data = DataTableUtils.toString(dt.Columns[0]);
            }
            catch
            {
                try
                {
                    data = DataTableUtils.toString(dt.Columns[0]);
                }
                catch
                {
                    data = "";
                }
            }

            if (dt != null && dt.Rows.Count > 0 && data != "語法")
                return true;
            else
                return false;
        }
        /// <summary>
        /// 儲存cookie，以便回到上一頁點選之頁數(EX：第三頁進入明細，上一頁則回到第三頁，而非第一頁)
        /// </summary>
        /// <param name="pagename">頁面名稱</param>
        /// <param name="value">標的物(唯一值  EX：客戶)</param>
        /// <returns></returns>
        public static HttpCookie Save_Cookies(string pagename, string value)
        {
            HttpCookie cookies = new HttpCookie(pagename);
            cookies[$"{pagename}_cust"] = value;
            cookies.Expires = DateTime.Now.AddDays(30);
            return cookies;
        }
        /// <summary>
        /// 為了加工VIS用的欄位
        /// </summary>
        /// <param name="x_text">欄位名稱</param>
        /// <returns></returns>
        static string Change_ColumnName(string x_text)
        {
            return x_text == "工藝名稱" ? "產品名稱(運行程式)" : x_text;
        }

        /// <summary>
        /// 字串轉時間
        /// </summary>
        /// <param name="_date">時間(格式：yyyyMMddHHmmss || yyyyMMdd)</param>
        /// <returns></returns>
        public static DateTime StrToDate(string _date)
        {
            if (_date != null && _date.Length < 14 && _date.Substring(0, 1) != "0")
                _date = $"{_date}080000";
            DateTime Trs;
            Trs = StrToDateTime(_date, "yyyyMMddHHmmss");

            return Trs;
        }
        public static DateTime StrToDateTime(string time, string Sourceformat)
        {
            try
            {
                return DateTime.ParseExact(time, Sourceformat, System.Globalization.CultureInfo.CurrentCulture);
            }
            catch
            {
                return new DateTime();
            }
        }

        /// <summary>
        /// 轉換時間戳記(甘特圖用)
        /// </summary>
        /// <param name="time">時間(格式：yyyyMMddHHmmss)</param>
        /// <returns></returns>
        public static string GetTimeStamp(DateTime time)
        {
            //  DateTime time = DateTime.Now;
            long ts = ConvertDateTimeToInt(time);
            return ts.ToString();
        }
        private static long ConvertDateTimeToInt(DateTime time)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位      
            return t;
        }

        /// <summary>
        /// 儲存使用者設定的欄位順序
        /// </summary>
        /// <param name="Columns">欄位順序(以逗點隔開)</param>
        /// <param name="pagename">頁面名稱</param>
        /// <param name="acc">使用者帳號</param>
        public static void Save_Columns(string Columns, string pagename, string acc)
        {
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisErp);
            string sqlcmd = $"select * from Save_PageColumns where Account = '{acc}' and PageName='{pagename}'";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);

            //先刪除舊有的
            if (HtmlUtil.Check_DataTable(dt))
            {
                GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisErp);
                DataTableUtils.Delete_Record("Save_PageColumns", $"Account = '{acc}' and PageName='{pagename}'");
            }

            //新增新的
            List<string> list = new List<string>(Columns.Split(','));
            DataTable save = new DataTable();
            save.Columns.Add("Columns");//欄位
            save.Columns.Add("Orders");//流水號
            save.Columns.Add("Account");//帳號
            save.Columns.Add("PageName");//頁面名稱
            save.Columns.Add("ChangeTime");//修改時間
            for (int i = 0; i < list.Count - 1; i++)
                save.Rows.Add(list[i], i, acc, pagename, DateTime.Now.ToString("yyyyMMddHHmmss"));

            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisErp);
            DataTableUtils.Insert_TableRows("Save_PageColumns", save);

        }
        /// <summary>
        /// 回傳該帳號設定之頁面欄位顯示順序
        /// </summary>
        /// <param name="acc">帳號名稱</param>
        /// <param name="pagename">頁面名稱</param>
        /// <returns></returns>
        /// <summary>
        /// 從資料庫讀取欄位順序
        /// </summary>
        /// <param name="acc">帳號</param>
        /// <param name="pagename">頁面名稱</param>
        /// <returns></returns>
        public static List<string> Get_ColumnsList(string acc, string pagename)
        {
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisErp);
            string sqlcmd = $"select * from save_pagecolumns where acc= '{acc}' and pagename='{pagename}' order by cast( orders as signed) asc";
            DataTable columns = DataTableUtils.GetDataTable(sqlcmd);
            List<string> columns_list = new List<string>();
            if (Check_DataTable(columns))
            {
                foreach (DataRow row in columns.Rows)
                    columns_list.Add(DataTableUtils.toString(row["columnname"]));
                columns_list.Add("");
            }
            return columns_list;
        }
        /// <summary>
        /// 比對順序
        /// </summary>
        /// <param name="dt">系統給的</param>
        /// <param name="history_list">已經儲存的</param>
        /// <returns></returns>
        public static List<string> Comparison_ColumnOrder(DataTable dt, List<string> history_list)
        {
            //取得dt的欄位順序
            List<string> columns = new List<string>();
            for (int i = 0; i < dt.Columns.Count; i++)
                columns.Add(dt.Columns[i].ToString());
            columns.Add("");

            if (history_list == null || history_list.Count == 0)
                return columns;
            else
            {
                List<string> order_list = new List<string>();

                for (int i = 0; i < history_list.Count; i++)
                    if (columns.IndexOf(history_list[i]) != -1)
                        order_list.Add(history_list[i]);

                //第二次比對是否有新增
                for (int i = 0; i < columns.Count; i++)
                    if (history_list.IndexOf(columns[i]) == -1)
                        order_list.Add(columns[i]);

                //空白永遠移到最後
                order_list.Remove("");
                order_list.Add("");
                return order_list;
            }

        }
        /// <summary>
        /// 94多型
        /// </summary>
        /// <param name="noworder_list">系統給的</param>
        /// <param name="history_list">已經儲存的</param>
        /// <returns></returns>
        public static List<string> Comparison_ColumnOrder(List<string> noworder_list, List<string> history_list)
        {
            if (history_list == null || history_list.Count == 0)
                return noworder_list;
            else
            {
                List<string> order_list = new List<string>();
                for (int i = 0; i < history_list.Count; i++)
                    if (noworder_list.IndexOf(history_list[i]) != -1)
                        order_list.Add(history_list[i]);

                //第二次比對是否有新增
                for (int i = 0; i < noworder_list.Count; i++)
                    if (history_list.IndexOf(noworder_list[i]) == -1)
                        order_list.Add(noworder_list[i]);

                //空白永遠移到最後
                order_list.Remove("");
                order_list.Add("");
                return order_list;
            }

        }

        public static StringBuilder Set_Table_Title(bool ok, DataTable dt, out string field_name, string Add_title = "", string Jump_field = "")
        {
            StringBuilder th = new StringBuilder();
            string title_name = "";//紀錄每個資料欄位名稱用
            string col_name = "";
            //dt與輸出表格相符
            if (Add_title == "")
            {
                //沒有資料的處理方式
                if (dt.Rows.Count <= 0)
                {
                    th.Append("<th class=\"center\">沒有資料載入</th>");
                    title_name = "";
                }
                else
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (Jump_field.Contains(dt.Columns[i].ColumnName) == false)
                        {
                            col_name = dt.Columns[i].ColumnName;
                            th.Append($"<th>{col_name}</th>");
                            title_name += col_name + ",";
                        }

                    }
                }
            }
            //dt與輸入表格不符(要新增欄位名稱)
            else
            {
                //沒有資料的處理方式
                if (dt.Rows.Count <= 0)
                {
                    th.Append("<th class=\"center\">沒有資料載入</th>");
                    title_name = "";
                }
                else
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row[Add_title] != null)
                        {
                            if (col_name != DataTableUtils.toString(row[Add_title]))
                            {
                                col_name = DataTableUtils.toString(row[Add_title]);
                                title_name += col_name + ",";
                                th.Append($"<th>{col_name}</th>");
                            }
                        }
                    }
                }
            }
            field_name = title_name;
            return th;
        }
        public static StringBuilder Set_Table_Title(List<string> list, string style = "")
        {
            StringBuilder th = new StringBuilder();
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count - 1; i++)
                    th.Append($"<th {style}>{list[i]}</th>");
            }
            else
                th.Append("<th class=\"center\">沒有資料載入</th>");
            return th;
        }
        public static StringBuilder Set_Table_Content(bool ok, DataTable dt, List<string> TitleList, Func<DataRow, string, string> callback = null)
        {
            return Set_Table_Content(ok, dt, TitleList.ToArray(), callback);
        }

        public static StringBuilder Set_Table_Content(bool ok, DataTable dt, string[] TitleList, Func<DataRow, string, string> callback = null)
        {
            StringBuilder tr = new StringBuilder();
            string field_value;
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    tr.Append("<tr>");
                    for (int i = 0; i < TitleList.Length - 1; i++)
                    {
                        field_value = "";
                        if (callback != null)
                            field_value = callback(row, TitleList[i]);
                        if (field_value == "" || field_value == "<td></td>")
                            field_value = $"<td style=\"vertical-align: middle; text-align: center;\">{DataTableUtils.toString(row[TitleList[i]])}</td>";
                        if (field_value != "1")
                            tr.Append(field_value);
                    }
                    tr.Append("</tr>");
                }
            }
            else
            {
                tr.Append("<tr> <td style=\"vertical-align: middle; text-align: center;\"> no data </td></tr>");
            }
            return tr;
        }
        public static StringBuilder Set_Table_Content(bool ok, DataTable dt, string Title_comma_text, Func<DataRow, string, string> callback = null)
        {
            return Set_Table_Content(ok, dt, Title_comma_text.Split(','), callback);
        }
        /// <summary>
        /// 轉換成千分位
        /// </summary>
        /// <param name="yValue">數值</param>
        /// <returns></returns>
        public static string Trans_Thousand(object yValue)
        {
            int yValue_trans = DataTableUtils.toInt(DataTableUtils.toString(yValue));
            return DataTableUtils.toString(yValue_trans.ToString("N0"));
        }
        /// <summary>
        /// 回傳印出圖片的表格
        /// </summary>
        /// <param name="dt">明細資料表</param>
        /// <param name="X_Text">X軸名稱</param>
        /// <param name="Y_Text">Y軸名稱</param>
        /// <returns></returns>
        public static DataTable PrintChart_DataTable(DataTable dt, string X_Text, string Y_Text, string condition = "", bool no_count = false)
        {
            DataTable dt_Return = new DataTable();
            dt_Return.Columns.Add(X_Text);
            dt_Return.Columns.Add(Y_Text, typeof(double));
            string sqlcmd = "";
            double count = 0;

            //避免有空白之類的廠商重複出現
            List<string> avoid_again = new List<string>();
            if (Check_DataTable(dt))
            {
                DataTable dt_Xtext = dt.DefaultView.ToTable(true, new string[] { X_Text });
                foreach (DataRow row in dt_Xtext.Rows)
                {
                    if (avoid_again.IndexOf(DataTableUtils.toString(row[X_Text]).Trim()) == -1)
                    {
                        count = 0;
                        sqlcmd = $"{X_Text}='{row[X_Text]}' {condition}";
                        DataRow[] rows = dt.Select(sqlcmd);
                        if (rows != null && rows.Length > 0)
                        {
                            if (!no_count)
                                for (int i = 0; i < rows.Length; i++)
                                    count += DataTableUtils.toDouble(DataTableUtils.toString(rows[i][Y_Text]));
                            else
                                count = rows.Length;
                        }
                        else
                            count = 0;
                        avoid_again.Add(DataTableUtils.toString(row[X_Text]).Trim());
                        dt_Return.Rows.Add(DataTableUtils.toString(row[X_Text]), count.ToString("0"));
                    }
                }
                return dt_Return;
            }
            else
                return dt_Return;
        }

        /// <summary>
        /// 領料單明細用
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string PrintChart_String(DataTable dt)
        {
            string return_string = "";
            DataTable dt_Clone = dt.Clone();
            //避免無法修改
            foreach (DataRow row in dt.Rows)
                dt_Clone.ImportRow(row);
            dt_Clone.AcceptChanges();

            //月份進行修改
            foreach (DataRow row in dt_Clone.Rows)
                row["領料單日期"] = DataTableUtils.toString(row["領料單日期"]).Replace("/", "").Substring(0, 6);

            DataTable date = dt_Clone.DefaultView.ToTable(true, new string[] { "領料單日期" });
            //取出領料用途
            DataTable use = dt_Clone.DefaultView.ToTable(true, new string[] { "用途說明" });

            foreach (DataRow row in use.Rows)
            {
                string datapoint = "";
                foreach (DataRow rew in date.Rows)
                {
                    double total = 0;
                    string sqlcmd = $"領料單日期='{rew["領料單日期"]}' and 用途說明='{row["用途說明"]}'";
                    DataRow[] rows = dt_Clone.Select(sqlcmd);
                    if (rows != null && rows.Length > 0)
                    {
                        for (int i = 0; i < rows.Length; i++)
                            total += DataTableUtils.toDouble(DataTableUtils.toString(rows[i]["領料數量"]));
                        datapoint += "{" +
                                    $" y: {total:0}, label: '{rew["領料單日期"]}' " +
                                     "},";
                    }
                }
                return_string += "{ " +
                                $"type: 'stackedColumn', showInLegend: true, name:'{row["用途說明"]}', dataPoints: [{datapoint}] " +
                                 "},";
            }
            return return_string;
        }

        /// <summary>
        /// 從明細取得所需資料
        /// </summary>
        /// <param name="dt">資料表</param>
        /// <param name="columns">資料表欄位</param>
        /// <param name="columns_value">該欄位內容</param>
        /// <returns></returns>
        public static DataTable Get_InformationDataTable(DataTable dt, string columns, string columns_value)
        {
            DataTable dt_clone = dt.Clone();
            string sqlcmd = columns_value != "" ? $"{columns}='{columns_value}'" : "";
            DataRow[] rows = dt.Select(sqlcmd);
            if (rows != null && rows.Length > 0)
            {
                for (int i = 0; i < rows.Length; i++)
                    dt_clone.ImportRow(rows[i]);
            }
            dt_clone.AcceptChanges();
            return dt_clone;
        }
        public static DataTable Get_InformationDataTable(DataTable dt, string sqlcmd)
        {
            DataTable dt_clone = dt.Clone();
            DataRow[] rows = dt.Select(sqlcmd);
            if (rows != null && rows.Length > 0)
            {
                for (int i = 0; i < rows.Length; i++)
                    dt_clone.ImportRow(rows[i]);
            }
            dt_clone.AcceptChanges();
            return dt_clone;
        }
        public static DataTable Get_MonthCapacity_InformationDataTable(DataTable dt, string columns, string columns_value,string date_str,string date_end,string type)
        {
            DataTable dt_clone = dt.Clone();
            string sqlcmd = columns_value != "" && type!= "month_capacity_total" ? $"{columns}='{columns_value}'" : "";
            sqlcmd = sqlcmd==""? $"入庫日>{date_str} and 入庫日 <{date_end} or 入庫日 is null " : $"({sqlcmd} and 入庫日>{date_str} and 入庫日 <{date_end} )" + "OR" + $"({sqlcmd} and 入庫日 is null)";
            DataRow[] rows = dt.Select(sqlcmd);
            if (rows != null && rows.Length > 0)
            {
                for (int i = 0; i < rows.Length; i++)
                    dt_clone.ImportRow(rows[i]);
            }
            dt_clone.AcceptChanges();
            return dt_clone;
        }

        /// <summary>
        /// 設定核取方塊
        /// </summary>
        /// <param name="dt">資料表</param>
        /// <param name="cbx">checkboxlist id</param>
        /// <param name="text">顯示文字欄位</param>
        /// <param name="value">顯示值欄位</param>
        /// <param name="all">是否加入全部</param>
        /// <param name="select">預設被選的值</param>
        public static void Set_Element(DataTable dt, CheckBoxList cbx, string text, string value = "", bool all = false, string select = "", bool select_all = false)
        {
            if (Check_DataTable(dt))
            {
                ListItem list = new ListItem();
                if (all)
                {
                    list = new ListItem("全部", "");
                    cbx.Items.Add(list);
                }

                foreach (DataRow row in dt.Rows)
                {
                    list = value == "" ? new ListItem(DataTableUtils.toString(row[text])) : new ListItem(DataTableUtils.toString(row[text]), DataTableUtils.toString(row[value]));
                    if (select != "" && (select == DataTableUtils.toString(row[text]).Trim() || select + "(臥)" == DataTableUtils.toString(row[text]).Trim()))
                        list.Selected = true;
                    if (select_all)
                        list.Selected = true;
                    cbx.Items.Add(list);
                }
            }
        }
        /// <summary>
        /// 設定下拉選單
        /// </summary>
        /// <param name="dt">資料表</param>
        /// <param name="drop">DropDownList id</param>
        /// <param name="text">顯示文字欄位</param>
        /// <param name="value">顯示值欄位</param>
        /// <param name="all">是否加入全部</param>
        public static void Set_Element(DataTable dt, DropDownList drop, string text, string value = "", bool all = false)
        {
            if (Check_DataTable(dt))
            {
                ListItem list = new ListItem();
                if (all)
                {
                    list = new ListItem("全部", "");
                    drop.Items.Add(list);
                }

                foreach (DataRow row in dt.Rows)
                {
                    list = value == "" ? new ListItem(DataTableUtils.toString(row[text])) : new ListItem(DataTableUtils.toString(row[text]), DataTableUtils.toString(row[value]));
                    drop.Items.Add(list);
                }
            }
        }


    }
    //儲存錯誤的地方
    public class Utilities
    {
        public static Exception LastError;
    }
    //Json to DataTable
    public static class JsonToDataTable
    {
        //從API處取得JSON內容
        public static string HttpGet(string url)
        {
            //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }

        }
        //把JSON轉換成DATATABLE
        public static DataTable JsonStringToDataTable(string jsonString)
        {
            DataTable dt = new DataTable();
            string[] jsonStringArray = Regex.Split(jsonString.Replace("[", "").Replace("]", ""), "},{");
            List<string> ColumnsName = new List<string>();
            foreach (string jSA in jsonStringArray)
            {
                string[] jsonStringData = Regex.Split(jSA.Replace("{", "").Replace("}", ""), ",");
                foreach (string ColumnsNameData in jsonStringData)
                {
                    try
                    {
                        int idx = ColumnsNameData.IndexOf(":");
                        string ColumnsNameString = ColumnsNameData.Substring(0, idx - 1).Replace("\"", "");
                        if (!ColumnsName.Contains(ColumnsNameString))
                        {
                            ColumnsName.Add(ColumnsNameString);
                        }
                    }
                    catch (Exception ex)
                    {
                        //如果沒有資料，直接回傳NULL，避免出錯
                        //throw new Exception(string.Format("Error Parsing Column Name : {0}", ColumnsNameData));
                        return null;
                    }
                }
                break;
            }
            foreach (string AddColumnName in ColumnsName)
            {
                dt.Columns.Add(AddColumnName.Replace("\\", ""));
            }
            foreach (string jSA in jsonStringArray)
            {
                string[] RowData = Regex.Split(jSA.Replace("{", "").Replace("}", ""), ",");
                DataRow nr = dt.NewRow();
                foreach (string rowData in RowData)
                {
                    try
                    {
                        int idx = rowData.IndexOf(":");
                        string RowColumns = rowData.Substring(0, idx - 1).Replace("\"", "").Replace("\\", "");
                        string RowDataString = rowData.Substring(idx + 1).Replace("\"", "").Replace("\\", "");
                        nr[RowColumns] = RowDataString;
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
                dt.Rows.Add(nr);
            }
            return dt;
        }

    }
    public static class Set_Html
    {
        //生產推移圖專用
        public static string Set_TabModel(string LineName, string th, string tr, List<string> value, string s_th = "", string s_tr = "")
        {
            //應有進度那邊不同而已
            //return
            //$"<div id=\"div_{LineName}\">" +
            //    //TAB部分
            //    $"<ul id=\"myTab_{LineName}\" class=\"nav nav-tabs\" role=\"tablist\">" +
            //        $"<li role=\"presentation\" class=\"active\" style=\"box-shadow: 3px 3px 9px gray;\"><a href=\"#tab_content1_{LineName}\" id=\"home-tab_{LineName}\" role=\"tab\" data-toggle=\"tab\" aria-expanded=\"true\">圖片模式</a>" +
            //        $"</li>" +
            //        $"<li role=\"presentation\" class=\"\" style=\"box-shadow: 3px 3px 9px gray;\"><a href=\"#tab_content2_{LineName}\" id=\"profile-tab_{LineName}\" role=\"tab\" data-toggle=\"tab\" aria-expanded=\"false\">表格模式</a>" +
            //        $"</li>" +
            //    //$"<li role=\"presentation\" class=\"\" style=\"box-shadow: 3px 3px 9px gray;\"><a href=\"#tab_content3_{LineName}\" id=\"profile-tab2_{LineName}\" role=\"tab\" data-toggle=\"tab\" aria-expanded=\"false\">每日進度表格</a>" +
            //    //$"</li>" +
            //    $"</ul>" +

            //    $"<div id=\"myTabContent\" class=\"tab-content\">" +
            //        $"<div role=\"tabpanel\" class=\"tab-pane fade active in\" id=\"tab_content1_{LineName}\" aria-labelledby=\"home-tab\">" +
            //            $"<div class=\"x_panel Div_Shadow\">" +
            //                $"<div class=\"row\">" +
            //                    $"<div class=\"dashboard_graph x_panel\">" +
            //                        $"<div class=\"col-md-10 col-sm-12 col-xs-12\" id=\"hidepercent_{LineName}\">" +
            //                            $"<div class=\"x_content\">" +
            //                                $"<div style=\"text-align: right; width: 100%; padding: 0;\">" +
            //                                    $"<button style=\"display: none\" type=\"button\" id=\"exportChart_{LineName}\" title=\"另存成圖片\">" +
            //                                        $"<img src=\"../../assets/images/download.jpg\" style=\"width: 36.39px; height: 36.39px;\">" +
            //                                    $"</button>" +
            //                                $"</div>" +
            //                                $"<div class=\"row \">" +
            //                                    $"<div class=\"col-md-12 col-sm-12 col-xs-12\">" +
            //                                        $"<div class=\"col-md-12 col-sm-12 col-xs-12\">" +
            //                                            $"<div id=\"chart_bar_{LineName}\" class=\"Canvas_height\">" +
            //                                            $"</div>" +
            //                                        $"</div>" +
            //                                    $"</div>" +
            //                                $"</div>" +
            //                            $"</div>" +
            //                        $"</div>" +
            //                        $"<div class=\"col-md-2 col-sm-2 col-xs-12 text-center _sumcount\">" +
            //                            $"<hr>" +
            //                            $"<div class=\"col-md-12 col-sm-12 col-xs-3\">" +
            //                                $"<div class=\"h2 mb-0 text-warning\" style=\"margin-bottom: 10px;\">{value[0]}" +
            //                                $"</div>" +
            //                                $"<div class=\"text-muted\">" +
            //                                    $"應有進度" +
            //                                    $"<br />" +
            //                                    $"({value[1]}/{value[2]})" +
            //                                $"</div>" +
            //                                $"<hr>" +
            //                            $"</div>" +
            //                            $"<div class=\"col-md-12 col-sm-12 col-xs-3\">" +
            //                                $"<div class=\"h2 mb-0 text-success\" style=\"margin-bottom: 10px;\">{value[3]}" +
            //                                $"</div>" +
            //                                $"<div class=\"text-muted\">" +
            //                                    $"實際進度" +
            //                                    $"<br />" +
            //                                    $"({value[4]}/{value[5]})" +
            //                                $"</div>" +
            //                                $"<hr>" +
            //                            $"</div>" +
            //                            $"<div class=\"col-md-12 col-sm-12 col-xs-3\">" +
            //                                $"<div class=\"h2 mb-0 text-success\" style=\"margin-bottom: 10px;\">{value[6]}" +
            //                                $"</div>" +
            //                                $"<div class=\"text-muted\">" +
            //                                    $"未下架台數" +
            //                                    $"<br />" +
            //                                $"</div>" +
            //                                $"<hr>" +
            //                            $"</div>" +
            //                            $"<div class=\"col-md-12 col-sm-12 col-xs-3\">" +
            //                                $"<div class=\"h2 mb-0 text-success\" style=\"margin-bottom: 10px;\">{value[7]}" +
            //                                $"</div>" +
            //                                $"<div class=\"text-muted\">" +
            //                                    $"落後台數" +
            //                                $"</div>" +
            //                                $"<hr>" +
            //                            $"</div>" +
            //                        $"</div>" +
            //                    $"</div>" +
            //                $"</div>" +
            //            $"</div>" +
            //            $"<div class=\"col-md-12 col-sm-12 col-xs-12\" id=\"hidediv_{LineName}\" style=\"display: none\">" +
            //                $"<div class=\"dashboard_graph x_panel\">" +
            //                    $"<div class=\"x_content\">" +
            //                        $"<div style=\"text-align: right; width: 100%; padding: 0;\">" +
            //                            $"<button style=\"display: none\" type=\"button\" id=\"exportimage_{LineName}\" title=\"另存成圖片\">" +
            //                                $"<img src=\"../../assets/images/download.jpg\" style=\"width: 36.39px; height: 36.39px;\">" +
            //                            $"</button>" +
            //                        $"</div>" +
            //                        $"<div class=\"row Canvas_height\">" +
            //                            $"<div class=\"col-md-12 col-sm-12 col-xs-12\">" +
            //                                $"<div class=\"col-md-12 col-sm-12 col-xs-10\">" +
            //                                    $"<div id=\"chartContainer_{LineName}\" style=\"height: 500px; max-width: 100%;\">" +
            //                                    $"</div>" +
            //                                $"</div>" +
            //                            $"</div>" +
            //                        $"</div>" +
            //                    $"</div>" +
            //                $"</div>" +
            //            $"</div>" +
            //        $"</div>" +

            //        $"<div role=\"tabpanel\" class=\"tab-pane fade\" id=\"tab_content2_{LineName}\" aria-labelledby=\"profile-tab\">" +
            //            $"<div class=\"x_panel Div_Shadow\">" +
            //                $"<div class=\"row\">" +
            //                    $"<div class=\"col-md-12 col-sm-12 col-xs-12\">" +
            //                        $"<div class=\"x_panel zpanel\">" +
            //                            $"<div class=\"x_title\">" +
            //                                    $"<h1 class=\"text-center _mdTitle\" style=\"width: 100%\"><b>{LineName}未結案列表</b></h1>" +
            //                                    $"<h3 class=\"text-center _xsTitle\" style=\"width: 100%\"><b>{LineName}未結案列表</b></h3>" +
            //                                $"<div class=\"clearfix\">" +
            //                                $"</div>" +
            //                            $"</div>" +
            //                            $"<div class=\"x_content\">" +
            //                                $"<p class=\"text-muted font-13 m-b-30\">" +
            //                                $"</p>" +
            //                                $"<table id=\"datatable_{LineName}\" class=\"table table-ts table-bordered nowrap\" cellspacing=\"0\" width=\"100%\">" +
            //                                    $"<thead>" +
            //                                        $"<tr id=\"tr_row\">" +
            //                                            th +
            //                                        $"</tr>" +
            //                                    $"</thead>" +
            //                                    $"<tbody>" +
            //                                        tr +
            //                                    $"</tbody>" +
            //                                $"</table>" +
            //                            $"</div>" +
            //                        $"</div>" +
            //                    $"</div>" +
            //                $"</div>" +
            //            $"</div>" +
            //        $"</div>" +

            //    //$"<div role=\"tabpanel\" class=\"tab-pane fade\" id=\"tab_content3_{LineName}\" aria-labelledby=\"profile-tab\">" +
            //    //    $"<div class=\"x_panel Div_Shadow\">" +
            //    //        $"<div class=\"row\">" +
            //    //            $"<div class=\"col-md-12 col-sm-12 col-xs-12\">" +
            //    //                $"<div class=\"x_panel zpanel\">" +
            //    //                    $"<div class=\"x_title\">" +
            //    //                        $"<h1 class=\"text-center _mdTitle\" style=\"width: 100%\"><b>{LineName}每日生產列表</b></h1>" +
            //    //                        $"<h3 class=\"text-center _xsTitle\" style=\"width: 100%\"><b>{LineName}每日生產列表</b></h3>" +
            //    //                    $"<div class=\"clearfix\"></div>" +
            //    //                    $"</div>" +
            //    //                    $"<div class=\"x_content\">" +
            //    //                    $"<p class=\"text-muted font-13 m-b-30\">" +
            //    //                    $"</p>" +
            //    //                    $"<table id=\"datatable2_{LineName}\" class=\"table table-ts table-bordered nowrap\" cellspacing=\"0\" width=\"100%\">" +
            //    //                        $"<thead>" +
            //    //                            $"<tr id=\"tr_row\">" +
            //    //                                s_th +
            //    //                            $"</tr>" +
            //    //                        $"</thead>" +
            //    //                        $"<tbody>" +
            //    //                            s_tr +
            //    //                        $"</tbody>" +
            //    //                    $"</table>" +
            //    //                $"</div>" +
            //    //            $"</div>" +
            //    //        $"</div>" +
            //    //    $"</div>" +
            //    //$"</div>" +
            //   // $"</div>" +
            //    $"<div class=\"clearfix\"></div>" +
            //    $"<br/>" +
            //$"</div>" +
            //$"</div>";


            return
            $"<div id=\"div_{LineName}\">" +
                //TAB部分
                $"<ul id=\"myTab_{LineName}\" class=\"nav nav-tabs\" role=\"tablist\">" +
                    $"<li role=\"presentation\" class=\"active\" style=\"box-shadow: 3px 3px 9px gray;\"><a href=\"#tab_content1_{LineName}\" id=\"home-tab_{LineName}\" role=\"tab\" data-toggle=\"tab\" aria-expanded=\"true\">圖片模式</a>" +
                    $"</li>" +
                    $"<li role=\"presentation\" class=\"\" style=\"box-shadow: 3px 3px 9px gray;\"><a href=\"#tab_content2_{LineName}\" id=\"profile-tab_{LineName}\" role=\"tab\" data-toggle=\"tab\" aria-expanded=\"false\">表格模式</a>" +
                    $"</li>" +
                //$"<li role=\"presentation\" class=\"\" style=\"box-shadow: 3px 3px 9px gray;\"><a href=\"#tab_content3_{LineName}\" id=\"profile-tab2_{LineName}\" role=\"tab\" data-toggle=\"tab\" aria-expanded=\"false\">每日進度表格</a>" +
                //$"</li>" +
                $"</ul>" +

                $"<div id=\"myTabContent\" class=\"tab-content\">" +
                    $"<div role=\"tabpanel\" class=\"tab-pane fade active in\" id=\"tab_content1_{LineName}\" aria-labelledby=\"home-tab\">" +
                        $"<div class=\"x_panel Div_Shadow\">" +
                            $"<div class=\"row\">" +
                                $"<div class=\"dashboard_graph x_panel\">" +
                                    $"<div class=\"col-md-10 col-sm-12 col-xs-12\" id=\"hidepercent_{LineName}\">" +
                                        $"<div class=\"x_content\">" +
                                            $"<div style=\"text-align: right; width: 100%; padding: 0;\">" +
                                                $"<button style=\"display: none\" type=\"button\" id=\"exportChart_{LineName}\" title=\"另存成圖片\">" +
                                                    $"<img src=\"../../assets/images/download.jpg\" style=\"width: 36.39px; height: 36.39px;\">" +
                                                $"</button>" +
                                            $"</div>" +
                                            $"<div class=\"row \">" +
                                                $"<div class=\"col-md-12 col-sm-12 col-xs-12\">" +
                                                    $"<div class=\"col-md-12 col-sm-12 col-xs-12\">" +
                                                        $"<div id=\"chart_bar_{LineName}\" class=\"Canvas_height\">" +
                                                        $"</div>" +
                                                    $"</div>" +
                                                $"</div>" +
                                            $"</div>" +
                                        $"</div>" +
                                    $"</div>" +
                                    $"<div class=\"col-md-2 col-sm-2 col-xs-12 text-center _sumcount\">" +
                                        $"<hr>" +
                                        $"<div class=\"col-md-12 col-sm-12 col-xs-3\">" +
                                            $"<div class=\"h2 mb-0 text-warning\" style=\"margin-bottom: 10px;\">{value[2]}" +
                                            $"</div>" +
                                            $"<div class=\"text-muted\">" +
                                                $"總計" +
                                            // $"<br />" +
                                            // $"({value[1]}/{value[2]})" +
                                            $"</div>" +
                                            $"<hr>" +
                                        $"</div>" +
                                        $"<div class=\"col-md-12 col-sm-12 col-xs-3\">" +
                                            $"<div class=\"h2 mb-0 text-success\" style=\"margin-bottom: 10px;\">{value[1]}<label style=\"font-size:8px\">({value[0]})</label>" +
                                            $"</div>" +
                                            $"<div class=\"text-muted\">" +
                                                $"應有" +
                                            //$"<br />" +
                                            //$"({value[4]}/{value[5]})" +
                                            $"</div>" +
                                            $"<hr>" +
                                        $"</div>" +
                                        $"<div class=\"col-md-12 col-sm-12 col-xs-3\">" +
                                            $"<div class=\"h2 mb-0 text-success\" style=\"margin-bottom: 10px;\">{value[4]}<label style=\"font-size:8px\">({value[3]})</label>" +
                                            $"</div>" +
                                            $"<div class=\"text-muted\">" +
                                                $"實際" +
                                                $"<br />" +
                                            $"</div>" +
                                            $"<hr>" +
                                        $"</div>" +
                                        $"<div class=\"col-md-12 col-sm-12 col-xs-3\">" +
                                            $"<div class=\"h2 mb-0 text-success\" style=\"margin-bottom: 10px;\">{value[6]}" +
                                            $"</div>" +
                                            $"<div class=\"text-muted\">" +
                                                $"相差" +
                                            $"</div>" +
                                            $"<hr>" +
                                        $"</div>" +
                                        $"<div class=\"col-md-12 col-sm-12 col-xs-3\">" +
                                            $"<div class=\"h2 mb-0 text-success\" style=\"margin-bottom: 10px;\">{value[7]}" +
                                            $"</div>" +
                                            $"<div class=\"text-muted\">" +
                                                $"落後" +
                                            $"</div>" +
                                            $"<hr>" +
                                        $"</div>" +
                                    $"</div>" +
                                $"</div>" +
                            $"</div>" +
                        $"</div>" +
                        $"<div class=\"col-md-12 col-sm-12 col-xs-12\" id=\"hidediv_{LineName}\" style=\"display: none\">" +
                            $"<div class=\"dashboard_graph x_panel\">" +
                                $"<div class=\"x_content\">" +
                                    $"<div style=\"text-align: right; width: 100%; padding: 0;\">" +
                                        $"<button style=\"display: none\" type=\"button\" id=\"exportimage_{LineName}\" title=\"另存成圖片\">" +
                                            $"<img src=\"../../assets/images/download.jpg\" style=\"width: 36.39px; height: 36.39px;\">" +
                                        $"</button>" +
                                    $"</div>" +
                                    $"<div class=\"row Canvas_height\">" +
                                        $"<div class=\"col-md-12 col-sm-12 col-xs-12\">" +
                                            $"<div class=\"col-md-12 col-sm-12 col-xs-10\">" +
                                                $"<div id=\"chartContainer_{LineName}\" style=\"height: 500px; max-width: 100%;\">" +
                                                $"</div>" +
                                            $"</div>" +
                                        $"</div>" +
                                    $"</div>" +
                                $"</div>" +
                            $"</div>" +
                        $"</div>" +
                    $"</div>" +

                    $"<div role=\"tabpanel\" class=\"tab-pane fade\" id=\"tab_content2_{LineName}\" aria-labelledby=\"profile-tab\">" +
                        $"<div class=\"x_panel Div_Shadow\">" +
                            $"<div class=\"row\">" +
                                $"<div class=\"col-md-12 col-sm-12 col-xs-12\">" +
                                    $"<div class=\"x_panel zpanel\">" +
                                        $"<div class=\"x_title\">" +
                                                $"<h1 class=\"text-center _mdTitle\" style=\"width: 100%\"><b>{LineName}未結案列表</b></h1>" +
                                                $"<h3 class=\"text-center _xsTitle\" style=\"width: 100%\"><b>{LineName}未結案列表</b></h3>" +
                                            $"<div class=\"clearfix\">" +
                                            $"</div>" +
                                        $"</div>" +
                                        $"<div class=\"x_content\">" +
                                            $"<p class=\"text-muted font-13 m-b-30\">" +
                                            $"</p>" +
                                            $"<table id=\"datatable_{LineName}\" class=\"table table-ts table-bordered nowrap\" cellspacing=\"0\" width=\"100%\">" +
                                                $"<thead>" +
                                                    $"<tr id=\"tr_row\">" +
                                                        th +
                                                    $"</tr>" +
                                                $"</thead>" +
                                                $"<tbody>" +
                                                    tr +
                                                $"</tbody>" +
                                            $"</table>" +
                                        $"</div>" +
                                    $"</div>" +
                                $"</div>" +
                            $"</div>" +
                        $"</div>" +
                    $"</div>" +

                //$"<div role=\"tabpanel\" class=\"tab-pane fade\" id=\"tab_content3_{LineName}\" aria-labelledby=\"profile-tab\">" +
                //    $"<div class=\"x_panel Div_Shadow\">" +
                //        $"<div class=\"row\">" +
                //            $"<div class=\"col-md-12 col-sm-12 col-xs-12\">" +
                //                $"<div class=\"x_panel zpanel\">" +
                //                    $"<div class=\"x_title\">" +
                //                        $"<h1 class=\"text-center _mdTitle\" style=\"width: 100%\"><b>{LineName}每日生產列表</b></h1>" +
                //                        $"<h3 class=\"text-center _xsTitle\" style=\"width: 100%\"><b>{LineName}每日生產列表</b></h3>" +
                //                    $"<div class=\"clearfix\"></div>" +
                //                    $"</div>" +
                //                    $"<div class=\"x_content\">" +
                //                    $"<p class=\"text-muted font-13 m-b-30\">" +
                //                    $"</p>" +
                //                    $"<table id=\"datatable2_{LineName}\" class=\"table table-ts table-bordered nowrap\" cellspacing=\"0\" width=\"100%\">" +
                //                        $"<thead>" +
                //                            $"<tr id=\"tr_row\">" +
                //                                s_th +
                //                            $"</tr>" +
                //                        $"</thead>" +
                //                        $"<tbody>" +
                //                            s_tr +
                //                        $"</tbody>" +
                //                    $"</table>" +
                //                $"</div>" +
                //            $"</div>" +
                //        $"</div>" +
                //    $"</div>" +
                //$"</div>" +
                // $"</div>" +
                $"<div class=\"clearfix\"></div>" +
                $"<br/>" +
            $"</div>" +
            $"</div>";

        }
        public static string Set_Image(string LineName, string TimeRange, string Value1, string Value2)
        {
            return $"set_image('{LineName}', \"chartContainer_{LineName}\",{TimeRange}, [{Value1}], [{Value2}], \"exportimage_{LineName}\", \"chart_bar_{LineName}\", \"exportChart_{LineName}\");";
        }
        public static string Set_Table(string LineName, string table_num = "")
        {
            return $"set_Table('#datatable{table_num}_{LineName}');";
        }
    }
}

//為了局部刷新要另開視窗用
public static class RedirectHelper
{
    public static void Redirect(this HttpResponse response, string url, string target, string windowFeatures)
    {
        if ((String.IsNullOrEmpty(target) ||
        target.Equals("_self", StringComparison.OrdinalIgnoreCase)) &&
        String.IsNullOrEmpty(windowFeatures))
        {
            response.Redirect(url);
        }
        else
        {
            Page page = (Page)HttpContext.Current.Handler; if (page == null)
            {
                throw new
                InvalidOperationException("Cannot redirect to new window .");
            }
            url = page.ResolveClientUrl(url);
            string script;
            if (!String.IsNullOrEmpty(windowFeatures))
            {
                script = @"window.open(""{0}"", ""{1}"", ""{2}"");";
            }
            else
            {
                script = @"window.open(""{0}"", ""{1}"");";
            }
            script = String.Format(script, url, target, windowFeatures);
            ScriptManager.RegisterStartupScript(page,
           typeof(Page), "Redirect", script, true);
        }
    }
}//HTML 引用JS
public class Use_Javascript
{
    public static string Quote_Javascript()
    {
        return "<!-- jQuery -->\n" +
                "<script src=\"../../assets/vendors/jquery/dist/jquery.min.js\"></script>\n" +
                "<!-- Bootstrap -->\n" +
                "<script src=\"../../assets/vendors/bootstrap/dist/js/bootstrap.min.js\"></script>\n" +
                "<!-- FastClick -->\n" +
                "<!-- NProgress -->" +
    "<script src=\"../../assets/vendors/nprogress/nprogress.js\"></script>" +
    "<!-- bootstrap-progressbar -->" +
    "<script src=\"../../assets/vendors/bootstrap-progressbar/bootstrap-progressbar.min.js\"></script>" +
       "<!-- bootstrap-wysiwyg -->" +
    "<script src=\"../../assets/vendors/bootstrap-wysiwyg/js/bootstrap-wysiwyg.min.js\"></script>" +
    "<script src=\"../../assets/vendors/jquery.hotkeys/jquery.hotkeys.js\"></script>" +
    "<script src=\"../../assets/vendors/google-code-prettify/src/prettify.js\"></script>" +
    "<!-- jQuery Tags Input -->" +
    "<script src=\"../../assets/vendors/jquery.tagsinput/src/jquery.tagsinput.js\"></script>" +
      "<!-- starrr -->" +
    "<script src=\"../../assets/vendors/starrr/dist/starrr.js\"></script>" +
      "<!-- Parsley -->" +
    "<script src=\"../../assets/vendors/parsleyjs/dist/parsley.min.js\"></script>" +
                "<script src=\"../../assets/vendors/fastclick/lib/fastclick.js\"></script>\n" +
                "<!-- iCheck -->\n" +
                "<script src=\"../../assets/vendors/iCheck/icheck.min.js\"></script>\n" +
                "<!-- bootstrap-daterangepicker -->\n" +
                "<script src=\"../../assets/vendors/moment/min/moment.min.js\"></script>\n" +
                "<script src=\"../../assets/vendors/bootstrap-daterangepicker/daterangepicker.js\"></script>\n" +
                "<!-- Switchery -->\n" +
                "<script src=\"../../assets/vendors/switchery/dist/switchery.min.js\"></script>\n" +
                "<!-- Select2 -->\n" +
                "<script src=\"../../assets/vendors/select2/dist/js/select2.full.min.js\"></script>\n" +
                "<!-- Autosize -->\n" +
                "<script src=\"../../assets/vendors/autosize/dist/autosize.min.js\"></script>\n" +
                "<!-- jQuery autocomplete -->\n" +
                "<script src=\"../../assets/vendors/devbridge-autocomplete/dist/jquery.autocomplete.min.js\"></script>\n" +
                "<!-- Custom Theme Scripts -->\n" +
                "<script src=\"../../assets/build/js/custom.min.js\"></script>\n" +
                "<!-- FloatingActionButton -->\n" +
                "<script src=\"../../assets/vendors/FloatingActionButton/js/index.js\"></script>\n" +
                "<!-- canvasjs -->\n" +
                "<script src=\"../../assets/vendors/canvas_js/canvasjs.min.js\"></script>\n" +
                "<!-- bootstrap-touchspin-master -->\n" +
                "<script src=\"../../assets/vendors/bootstrap-touchspin-master/dist/jquery.bootstrap-touchspin.js\"></script>\n" +
                "<!-- Datatables -->\n" +
                "<script src=\"../../assets/vendors/datatables.net/js/jquery.dataTables.min.js\"></script>\n" +
                "<script src=\"../../assets/vendors/datatables.net-bs/js/dataTables.bootstrap.min.js\"></script>\n" +
                "<script src=\"../../assets/vendors/datatables.net-buttons/js/dataTables.buttons.min.js\"></script>\n" +
                "<script src=\"../../assets/vendors/datatables.net-buttons-bs/js/buttons.bootstrap.min.js\"></script>\n" +
                "<script src=\"../../assets/vendors/datatables.net-buttons/js/buttons.flash.min.js\"></script>\n" +
                "<script src=\"../../assets/vendors/datatables.net-buttons/js/buttons.html5.min.js\"></script>\n" +
                "<script src=\"../../assets/vendors/datatables.net-buttons/js/buttons.print.min.js\"></script>\n" +
                "<script src=\"../../assets/vendors/datatables.net-responsive/js/dataTables.responsive.min.js\"></script>\n" +
                "<script src=\"../../assets/vendors/datatables.net-responsive-bs/js/responsive.bootstrap.js\"></script>\n" +
                "<script src=\"../../assets/vendors/datatables.net-scroller/js/dataTables.scroller.min.js\"></script>\n" +
                "<script src=\"../../assets/vendors/datatables.net-colReorder/dataTables.colReorder.min.js\"></script>\n" +
                "<script src=\"../../assets/vendors/jszip/dist/jszip.min.js\"></script>\n" +
                "<script src=\"../../assets/vendors/pdfmake/build/pdfmake.min.js\"></script>\n" +
                "<script src=\"../../assets/vendors/pdfmake/build/vfs_fonts.js\"></script>\n" +
                   "<script src=\"../../assets/vendors/time/loading.js\"></script>" +
                "<script src=\"../../assets/vendors/Create_HtmlCode/HtmlCode20211210.js\"></script>";
    }
}