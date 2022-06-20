using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace dekERP_dll.dekErp
{
    public class WebUtils
    {
        //回傳加密後文字
        public static string UrlStringEncode(string data, string URL_ENCODE = "1")
        {
            if (URL_ENCODE != "1") return data;
            return StringEncode(data);
        }
        //回傳解密後文字
        public static string UrlStringDecode(string data, string URL_ENCODE = "1")
        {
            if (URL_ENCODE != "1") return data;
            return StringDecode(data);
        }
        //加密
        public static string StringEncode(string data)
        {
            //先將字串轉成位元組
            byte[] StringByte = Encoding.Unicode.GetBytes(data);
            //將位元組編碼
            return HttpServerUtility.UrlTokenEncode(StringByte);
        }
        //解密
        public static string StringDecode(string data)
        {
            //先將字串解碼成位元組
            byte[] StringByte = HttpServerUtility.UrlTokenDecode(data);
            //將位元組轉成字串
            return Encoding.Unicode.GetString(StringByte);
        }
    }

    //ini檔案的讀寫
    public class IniManager
    {
        private string filePath;
        private StringBuilder lpReturnedString;
        private int bufferSize;

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string lpString, string lpFileName);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);

        public IniManager(string iniPath)
        {
            filePath = iniPath;
            //調整可讀入的字串長度
            bufferSize = 15000;
            lpReturnedString = new StringBuilder(bufferSize);
        }

        // read ini date depend on section and key
        public string ReadIniFile(string section, string key, string defaultValue)
        {
            lpReturnedString.Clear();
            GetPrivateProfileString(section, key, defaultValue, lpReturnedString, bufferSize, filePath);
            return lpReturnedString.ToString();
        }

        // write ini data depend on section and key
        public void WriteIniFile(string section, string key, Object value)
        {
            WritePrivateProfileString(section, key, value.ToString(), filePath);
        }
        //// 删除ini文件下所有段落
        public void ClearAllSection(string filePath, string title)
        {
            WritePrivateProfileString(title, null, null, filePath);
        }
    }
}
