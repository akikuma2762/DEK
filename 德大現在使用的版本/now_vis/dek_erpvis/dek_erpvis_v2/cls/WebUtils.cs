using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
//using System.Web.Mail;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Web.Configuration;

public class WebUtils
{
	public WebUtils()
	{
		//
		// TODO: 在此加入建構函式的程式碼
		//
	}
    //----------------------------------------------------
    public static void PageLoadingAnimation(HttpResponse Response)
    {
        string script =
        "<!DOCTYPE html>"+
        "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">"+            
        "<div id='mydiv' style='position:absolute;top:30%;left:45%;'>Text</div>" +
        "<script language=javascript>" +
        "mydiv.innerText='';" +
        "var dots=0,dotmax=10;" +
        "var myvar;" +
        "function ShowWait()" +
        "{" +
          "var output='Loading';dots++;" +
           "if(dots>=dotmax) dots=1;" +
           "for(var x=0;x<dots;x++) {output += '.';}" +
           "mydiv.innerText=output;" +
        "}" +
        "function StartShowWait()" +
        "{" +
           "mydiv.style.visibility='visible';" +
           "myvar=window.setInterval('ShowWait()',100);" +
        "}" +
        "function HideWait(){mydiv.style.visibility='hidden';window.clearInterval(myvar);}" +
        "StartShowWait();" +
        "</script>";
        Response.Write(script);
        Response.Flush();
    }
    public static string GetAppSettings(string key)
    {
        return WebConfigurationManager.AppSettings[key];
    }

    public static string StringEncode(string data)
    {
        //先將字串轉成位元組
        byte[] StringByte = System.Text.UnicodeEncoding.Unicode.GetBytes(data);
        //將位元組編碼
        return HttpServerUtility.UrlTokenEncode(StringByte);
    }

    public static string StringDecode(string data)
    {   
        //先將字串解碼成位元組
        byte[] StringByte = HttpServerUtility.UrlTokenDecode(data);
        //將位元組轉成字串
        return System.Text.UnicodeEncoding.Unicode.GetString(StringByte);
    }

    public static string UrlStringEncode(string data)
    {
        if (GetAppSettings("URL_ENCODE") != "1") return data;
        return StringEncode(data);
    }

    public static string UrlStringDecode(string data)
    {
        if (GetAppSettings("URL_ENCODE") != "1") return data;
        return StringDecode(data);
    }

    public static void CommaTextToHashtable(string text, Hashtable table)
    {
        string[] str_array = text.Split(new char[] {',','&'});
        char [] sym_tab = { '=' };
        string[] key_value;
        table.Clear();
        foreach(string item in str_array)
        {
            key_value = item.Split(sym_tab);
            if (key_value.Length>1) 
                 text=key_value[1];
            else text="";
            table[key_value[0].Trim()] = text;
        }
    }

    public static string HashtableToCommaText(Hashtable table)
    {
        string text="";
        foreach (DictionaryEntry item in table)
        {
            if (text != "") text += ",";
            text += item.Key.ToString() + "=" + item.Value.ToString();
        }
        return text;
    }

    // "Comma,string" , "Quote""String",Space string,, NormalString
    // Will be converted to: 
    // [Comma,String][Quote"String][Space][String][][NormalString]
    public static ArrayList CommaTextToArrayList(string comma_text)
    {
        ArrayList arraylist=new ArrayList();
        string item_text = "";
        int max_index = comma_text.Length-1;
        int quote_count = 0, comma_count = 0;
        char code;
        for (int index = 0; index <= max_index;index++)
        {
            code = comma_text[index];
            if (code == '\"')
            {
                //"-----""-------""-------", ==> -----"-------"--------
                //"-----,------""-------", ==> -----,------"--------
                //is ----""
                if (index < max_index && comma_text[index + 1] == '\"') index++;
                else
                {
                    if (index >= max_index || comma_text[index + 1] == ',')
                    {
                        index++; quote_count = 1;
                    }
                    if (++quote_count < 2) continue;
                }
            }
            else if (code == ',' && quote_count == 0) // not in quote ??
            {
                // ----, or ---",
                comma_count = 2;
            }
            if (quote_count >= 2 || comma_count >= 2)
            {
                arraylist.Add(item_text);
                item_text = "";
                quote_count = 0;
                comma_count = 0;
            }
            else item_text += code;
        }
        if (item_text.Length != 0) arraylist.Add(item_text);
        return arraylist;
    }

    public static string ArrayListToCommaText(ArrayList arraylist)
    {
        string comma_text = "",text;
        bool is_quote = false;
        foreach (string item in arraylist)
        {
            if (item == null) break;
            text = item;
            is_quote=text.Contains("\"");
            if (is_quote) text = text.Replace("\"", "\"\"");
            if (is_quote || text.Contains(',') || text.Contains(' ')) 
                text = "\"" + text + "\"";
            if (comma_text.Length>0) comma_text += ",";
            comma_text += text;
        }
        return comma_text;
    }

    /*
    public static void CommaTextArrayListTest()
    {
        ArrayList arraylist = new ArrayList();
        arraylist.Add("Comma,string");
        arraylist.Add("Quote\"String");
        arraylist.Add("Space string");
        arraylist.Add("");
        arraylist.Add("Normal String");
        arraylist.Add("Normal\",\"String");
        arraylist.Add(",,,");
        string text = WebUtils.ArrayListToCommaText(arraylist);
        arraylist = WebUtils.CommaTextToArrayList(text);
    }
    */

    public static string[] ToStringArray(string text, char seperator_code)
    {
        return text.Split(new char[] { seperator_code });
    }

    public static string StringArrayToCommaText(string [] strlist)
    {
        string comma_text = "", text;
        bool is_quote = false;
        for(int index=0;index<strlist.Length;index++)
        {
            text = strlist[index];
            is_quote = text.Contains("\"");
            if (is_quote) text = text.Replace("\"", "\"\"");
            if (is_quote || text.Contains(','))
                text = "\"" + text + "\"";
            if (comma_text.Length > 0) comma_text += ",";
            comma_text += text;
        }
        return comma_text;
    }

    public static string[] CommaTextToStringArray(string comma_text)
    {
        //return ToStringArray(comma_text, ',');
        ArrayList arraylist=CommaTextToArrayList(comma_text);
        string [] strlist=new string[arraylist.Count];
        int index=0;
        foreach(string text in arraylist)
            strlist[index++] = text;
        arraylist.Clear();
        return strlist;
    }
    //--------------------------------------------------------------------------------------------------------------
    //--
    //--------------------------------------------------------------------------------------------------------------
    private static Hashtable QueryStringHashTable;

    public static void QueryString_LoadSessionRequest(System.Web.SessionState.HttpSessionState session, HttpRequest request)
    {
        if (QueryStringHashTable == null)
             QueryStringHashTable=new Hashtable();
        else QueryStringHashTable.Clear();
        string query_str = HttpUtility.UrlDecode(request.QueryString.ToString());
        string key="";
        int pos = query_str.IndexOf("KEY=");
        if (pos >=0)
        {
            //ID=xxxx,KEY=CNO=asf,OWNER=xxx
            key = UrlStringDecode(query_str.Remove(0, pos+4));
            query_str = query_str.Remove(pos)+key;
        }
        CommaTextToHashtable(query_str, QueryStringHashTable);
        QueryStringHashTable["KEY"] =key;
        if (GetString(QueryStringHashTable["ID"]) == "") QueryStringHashTable["ID"] = "BlogHome";
        QueryStringHashTable["ROOT_PATH"] = request.ApplicationPath;
        QueryStringHashTable["SERVER_PATH"] = request.MapPath("~");
        QueryStringHashTable["USER_ID"]=session["USER_ID"];
        QueryStringHashTable["USER_TYPE"]=session["USER_TYPE"];
        QueryStringHashTable["USER_NAME"]=session["USER_NAME"];
    }

    public static string QueryString_GetString(string id)
    {
        return GetString(QueryStringHashTable, id);
    }

    public static void QueryString_SetString(string id,string value)
    {
        QueryStringHashTable[id]=value;
    }

    public static string QueryString_KeyReplace(string key,string value)
    {
        string key_text = WebUtils.QueryString_GetString("KEY");
        Hashtable table = new Hashtable();
        CommaTextToHashtable(key_text, table);
        if (value != "") table[key] = value;
        else table.Remove(key);
        return HashtableToCommaText(table);
    }

    public static string QueryString_KeyRemoveAction()
    {
        string key_text = WebUtils.QueryString_GetString("KEY");
        int pos = key_text.IndexOf(",ACT");
        if (pos <= 0) return key_text;
        return key_text.Substring(0, pos);
    }
    //--------------------------------------------------------------------------------------------------------------
    //--
    //--------------------------------------------------------------------------------------------------------------
    public static string ToHyperLink(string text, string attribute)
    {
        return "<a href=" + attribute + ">" + text + "</a>";
    }

    public static string BlogMain_IDKey(string id, string key)
    {
        string page = WebUtils.QueryString_GetString("ROOT_PATH").Trim();
        int len = page.Length;
        if (len == 0 || page[len - 1] != '/') page += "/";
        page += "BlogMain.aspx" + "?ID=" + id + ",KEY=" + UrlStringEncode(key);
        return page;
    }

    public static string ToHyperLinkID(string text, string id, string key)
    {
        return ToHyperLink(text, BlogMain_IDKey(id, key));
    }

    public static string GetString(object obj)
    {
        if (obj == null) return "";
        return obj.ToString();
    }

    public static string GetString(Hashtable reclist, string id)
    {
        if (reclist == null) return "";
        return GetString(reclist[id]);
    }

    public static int ToInt(string text, int def_val)
    {
    int value;
        try { value = int.Parse(text); }
        catch (Exception) { value = def_val; }
        return value;
    }

    public static DateTime ToDateTime(string text)
    {
        DateTime date_time;
        try { date_time = DateTime.Parse(text); }
        catch (Exception) { date_time=DateTime.Parse("1000/1/1"); }
        return date_time;
    }

    public static ListItem[] CommaTextToListItems(string comma_text, string selected_item)
    {
        string[] str_array = CommaTextToStringArray(comma_text);
        ListItemCollection list_item = new ListItemCollection();
        string[] key_value;
        ListItem item;
        string value;
        list_item.Clear();
        foreach (string data in str_array)
        {
            key_value = ToStringArray(data, '=');
            if (key_value.Length < 1) continue;
            item = new ListItem();
            item.Text = key_value[0].Trim();
            if (key_value.Length > 1)
                value = key_value[1].Trim();
            else value = item.Text;
            if (selected_item != "" && value == selected_item)
            {
                item.Selected = true;
                selected_item = "";
            }
            item.Value = value;
            list_item.Add(item);
        }
        ListItem [] item_array = new ListItem [list_item.Count];
        for (int index = 0; index < list_item.Count; index++)
            item_array[index] = list_item[index];
        list_item.Clear();
        return item_array;
    }

    public static void CommaTextToDropDownList(string comma_text, string selected_item, DropDownList ddlist)
    {
        ListItem[] list_item = CommaTextToListItems(comma_text, selected_item);
        ddlist.Items.AddRange(list_item);
    }

    public static void SetNumericDropDownList(DropDownList ddlist, double selected_value, double start, double end, double step)
    {
        ListItem item;
        int count = (int)((end - start) / step);
        for (int i = 0; i <= count; i++, start += step)
        {
            item = new ListItem();
            item.Text = start.ToString();
            if (selected_value == start)
                item.Selected = true;
            ddlist.Items.Add(item);
        }
    }

    public static DropDownList GetNumericDropDownList(double selected_value, double start, double end, double step)
    {
        DropDownList ddlist = new DropDownList();
        SetNumericDropDownList(ddlist, selected_value, start, end, step);
        return ddlist;
    }
    //--------------------------------------------------------------------------------------

    //--------------------------------------------------------------------------------------
    public static void CommaTextToTable(Table table, TableRow row ,string comma_text)
    {
        string[] tab_header = CommaTextToStringArray(comma_text);
        TableCell cell;
        foreach (string item in tab_header)
        {
            cell = new TableCell();
            cell.Text = item;
            row.Cells.Add(cell);
        }
        table.Rows.Add(row);
    }

    public static void CommaTextToTable(Table table,string comma_text)
    {
        TableRow row=new TableRow();
        CommaTextToTable(table,row ,comma_text);
    }

    //-----------------------------------------------------------------------------------
    public static Control FindWebControl(Control web_ctrl, string ctrl_id, string ctrl_type)
    {
        if (web_ctrl != null)
        {
            string name = web_ctrl.ID;
            if (name != null && name.Contains(ctrl_id))
            {
                if (ctrl_type == null || ctrl_type == "") return web_ctrl;
                if (web_ctrl.GetType().ToString().Contains(ctrl_type)) return web_ctrl;
            }
            foreach (Control subctrl in web_ctrl.Controls)
            {
                web_ctrl = FindWebControl(subctrl, ctrl_id, ctrl_type);
                if (web_ctrl != null) return web_ctrl;
            }
            web_ctrl = null;
        }
        return web_ctrl;
    }
    public static Control GetPostBackControl(Page page,string ctrl_type)
    {
        Control ctrl = null;
        string ctrlname = page.Request.Params.Get("__EVENTTARGET");
        if (ctrlname != null && ctrlname != "")
             ctrl = page.FindControl(ctrlname);
        else
        {
            foreach (string ctl in page.Request.Form)
            {
                Control c = page.FindControl(ctl);
                if (c == null) continue;
                if (c.GetType().ToString().Contains(ctrl_type))
                {
                    ctrl = c;
                    break;
                }
            }
        }
        return ctrl;
    }

    //-----------------------------------------------------------------------------------
    /*
    本文為 Html 格式
    message.IsBodyHtml = True

    Encoding 字碼設定
    UTF8
    message.BodyEncoding = Encoding.UTF8
    message.SubjectEncoding = Encoding.UTF8

    Big5
    message.BodyEncoding = Encoding.GetEncoding("Big5")
    message.SubjectEncoding = Encoding.GetEncoding("Big5")
　
    CC 副本
    Dim mailCC As New MailAddress(ccAddress)
    message.CC.Add(mailCC)

    */
    private static void SendEmail_Callback(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
    {
        var msg = (MailMessage) e.UserState;
        string ret_mesg;
        if (e.Cancelled)
        {
            ret_mesg="Cancelled - " + msg.Subject.ToString() + " : " + msg.To.ToString();
        }
        if (e.Error != null)
        {
            ret_mesg="ERROR: " + msg.To.ToString() + " : " + e.Error.ToString();
        }
        else
        {
           ret_mesg="EmailGateway: " + msg.Subject.ToString();
        }
    } 

    /*public static string SendEmail_Html(string mail_to, string subject, string body)
    {
        string mail_from = WebUtils.GetAppSettings("ADMIN_EMAIL");
        string smtp_server = WebUtils.GetAppSettings("SMTP_SERVER");
        string smtp_email_id = WebUtils.GetAppSettings("SMTP_USER_ID");
        string smtp_email_pass = WebUtils.GetAppSettings("SMTP_USER_PASS");
        string enc_key = WebUtils.GetAppSettings("ENCRIPT_KEY");
        smtp_email_pass = MyEncript.Decript(smtp_email_pass, enc_key);
        string ret_mesg = "";
        //------------------------------------------------------------------
        MailMessage message=null;
        SmtpClient mailserver = null;
        try
        {
            mailserver = new SmtpClient(smtp_server);
            if (smtp_email_id != "" && smtp_email_pass != "")
                mailserver.Credentials = new System.Net.NetworkCredential(smtp_email_id, smtp_email_pass);
            message = new MailMessage(mail_from, mail_to, subject, body);
            message.IsBodyHtml = true;
            message.BodyEncoding = message.SubjectEncoding = System.Text.Encoding.UTF8;
            mailserver.Send(message);
            //mailserver.SendCompleted += SendEmail_Callback;
            //object userState = message;
            //mailserver.SendAsync(message, userState); 
        }
        catch (SmtpException ex)
        {
            ret_mesg = ex.Message + "\r\n" + ex.InnerException.Message;
            //ret_mesg = ex.ToString();
        }
        finally
        {
            if (message != null) message.Dispose();
            if (mailserver != null) mailserver.Dispose();
        }
        return ret_mesg;
    }*/
    //-----------------------------------------------------------------------------------
    public static bool IsPasswordValid(Hashtable reclist, ref string mesg)
    {
        if (GetString(reclist, "帳號密碼").Length < 5)
        {
            mesg = "帳號密碼至少要5個字以上"; return false;
        }
        if (GetString(reclist, "帳號密碼") != GetString(reclist, "確認密碼"))
        {
            mesg = "兩個帳號密碼內容不一致"; return false;
        }
        return true;
    }
    public static bool IsEmailValid(Hashtable reclist, ref string mesg)
    {
        Regex regex_email = new Regex(".+@.+\\..+");
        if (!regex_email.IsMatch(GetString(reclist, "電子郵件")))
        {
            mesg = "請輸入正確之電子郵件"; return false;
        }
        return true;
    }
    public static bool IsStudentNoValid(Hashtable reclist, ref string mesg)
    {
        string text = GetString(reclist, "帳號類別");
        if (text != "學生") return true;
        Regex stuno = new Regex("[0-9]{6,9}");
        if (!stuno.IsMatch(GetString(reclist, "學號")))
        {
            mesg = "請輸入正確學號";
            return false;
        }
        return true;
    }
    public static bool IsUserDataValid(Hashtable reclist, ref string mesg)
    {
        if (GetString(reclist, "姓名") == "")
        {
            mesg = "請輸入姓名"; return false;
        }
        if (!IsEmailValid(reclist, ref mesg)) return false;
        if (!IsStudentNoValid(reclist, ref mesg)) return false;
        return true;
    }
    public static bool IsRegisterDataValid(Hashtable reclist,ref string mesg)
    {
        if (GetString(reclist,"帳號名稱") == "")
        {
            mesg = "請輸入帳號"; return false;
        }
        if (!IsUserDataValid(reclist, ref mesg)) return false;
        if (!IsPasswordValid(reclist, ref mesg)) return false;
        return true;
    }

}