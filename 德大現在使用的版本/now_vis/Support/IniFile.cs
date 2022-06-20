using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Support
{

    // SetupIniIP
    public class SetupIniIP
    {
        //=========================
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        [DllImport("Kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        private static extern int GetPrivateProfileSection(
                [In] [MarshalAs(UnmanagedType.LPStr)] string section,
                // Note that because the key/value pars are returned as null-terminated
                // strings with the last string followed by 2 null-characters, we cannot
                // use StringBuilder.
                [In] IntPtr pReturnedString,
                [In] UInt32 nSize,
                [In] [MarshalAs(UnmanagedType.LPStr)] string strFileName
            );
        //----------------------------------------------------------------
        private string filename;
        private string cur_section;
        public SetupIniIP(string file,string section="")
        {
            filename = System.Environment.CurrentDirectory + "\\" + file + "";
            cur_section = section;
        }

        public string[] ReadSection(string section)
        {
            // Allocate in unmanaged memory a buffer of suitable size.
            // Specified here the max size of 32767 as documentated in MSDN.
            IntPtr pBuffer = Marshal.AllocHGlobal(32767);
            // Start with an array of 1 string only. 
            // Will embellish as we go along.
            string[] strArray = new string[0];
            int uiNumCharCopied = 0;
            uiNumCharCopied = GetPrivateProfileSection(section, pBuffer, 32767, filename);
            // iStartAddress will point to the first character of the buffer,
            int iStartAddress = pBuffer.ToInt32();
            // iEndAddress will point to the last null char in the buffer.
            int iEndAddress = iStartAddress + (int)uiNumCharCopied;

            // Navigate through pBuffer.
            while (iStartAddress < iEndAddress)
            {
                // Determine the current size of the array.
                int iArrayCurrentSize = strArray.Length;
                // Increment the size of the string array by 1.
                Array.Resize<string>(ref strArray, iArrayCurrentSize + 1);
                // Get the current string which starts at "iStartAddress".
                string strCurrent = Marshal.PtrToStringAnsi(new IntPtr(iStartAddress));
                // Insert "strCurrent" into the string array.
                strArray[iArrayCurrentSize] = strCurrent;
                // Make "iStartAddress" point to the next string.
                iStartAddress += (strCurrent.Length + 1);
            }
            Marshal.FreeHGlobal(pBuffer);
            pBuffer = IntPtr.Zero;
            return strArray;
        }

        public void WriteString(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, filename);
        }
        public string ReadString(string Section, string Key,string def_str="")
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, filename);
            if (i <= 0) return def_str;
            return temp.ToString();
        }

        public int ReadInt(string Section, string Key,int def_value=0)
        {
            return Utils.StrUtils.ToInt(ReadString(Section, Key), def_value);
        }

        public double ReadDouble(string Section, string Key, double def_value = 0)
        {
            return Utils.StrUtils.ToDouble(ReadString(Section, Key), def_value);
        }

        public DateTime ReadDateTime(string Section, string Key, string def_datetime = "1/1/1")
        {
            return Utils.StrUtils.ToDateTime(ReadString(Section, Key), def_datetime);
        }

        //----------------
        public void Section_goto(string section)
        {
            cur_section = section;
        }
        public void Section_WriteString(string Key, string Value)
        {
            WriteString(cur_section, Key, Value);
        }

        public string Section_ReadString(string Key,string def_str="")
        {
            return ReadString(cur_section, Key, def_str);
        }

        public int Section_ReadInt(string Key, int def_value = 0)
        {
            return ReadInt(cur_section, Key, def_value);
        }

        public double Section_ReadDouble(string Section, string Key, double def_value = 0)
        {
            return ReadDouble(cur_section, Key, def_value);
        }

        public DateTime Section_ReadDateTime(string Section, string Key, string def_datetime = "1/1/1")
        {
            return ReadDateTime(cur_section, Key, def_datetime);
        }

    }
    
}
