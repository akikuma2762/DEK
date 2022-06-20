using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Support
{
    class CommaText
    {
        public static string toCommaText(List<string> list)
        {
            if (list == null) return "";
            return string.Join(",", list);
        }

        public static string toCommaText(string [] str_array)
        {
            if (str_array == null) return "";
            return string.Join(",", str_array);
        }

        public static string toCommaText(CheckedListBox clb)
        {
            if (clb == null) return "";
            string comma_text = "";
            foreach (object obj in clb.CheckedItems)
            {
                if (comma_text != "") comma_text += ",";
                comma_text += obj.ToString();
            }
            return comma_text;
        }

        public static string toCommaText(DataRow row)
        {
            string str = "";
            for (int index = 0; index < row.Table.Columns.Count; index++)
            {
                if (index > 0) str += ",";
                str += row[index].ToString();
            }
            return str;
        }
    }
}
