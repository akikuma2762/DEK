using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ERPVIS
{
    public enum OrderStatus { All, Finished, Unfinished, Scheduled, Unscheduled };
    public enum dekModel { Image, Table };
    public enum OrderLineorCust { Line, Custom };
    public enum OrderType { 數量, 金額 };
    //業務部
    interface IdepSales
    {
        //訂單數量及金額統計 start(開始時間) end(結束時間) status(訂單狀態 0全部 1已結 2未結) amt_or_qty(數量/金額) img_or_tbl(圖/表) line_or_custom(產線/客戶) max_records(前N筆資料)
        DataTable Orders(string start, string end, OrderStatus status, dekModel img_or_tbl, OrderLineorCust line_or_custom, OrderType qty_or_amt, int max_records);
        DataTable Orders(DateTime start, DateTime end, OrderStatus status, dekModel img_or_tbl, OrderLineorCust line_or_custom, OrderType qty_or_amt, int max_records);

        //訂單詳細表 start(開始時間) end(結束時間) custom(客戶名稱) status(訂單狀態 0全部 1已結案 2未結案)
        DataTable Orders_Details(string start, string end, string custom, OrderStatus status);
        DataTable Orders_Details(DateTime start, DateTime end, string custom, OrderStatus status);

        //出貨統計表 start(開始時間) end(結束時間) img_or_tbl(圖/表)
        DataTable Shipment(string start, string end, dekModel img_or_tbl);
        DataTable Shipment(DateTime start, DateTime end, dekModel img_or_tbl);

        //出貨詳細表 start(開始時間) end(結束時間) custom(客戶名稱)
        DataTable Shipment_Detail(string start, string end, string custom);
        DataTable Shipment_Detail(DateTime start, DateTime end, string custom);

        //出貨詳細表 小計 start(開始時間) end(結束時間) custom(客戶名稱) item(物品名稱)
        DataTable Get_Shipment(string start, string end, string custom, string item);
        DataTable Get_Shipment(DateTime start, DateTime end, string custom, string item);

        //運輸架未歸還統計 img_or_tbl(圖/表)
        DataTable Transportrackstatistics(dekModel img_or_tbl);

        //未交易客戶 start(開始時間) end(結束時間) symbol(< = >) day(天數)
        DataTable UntradedCustomer(string start, string end, string symbol, int day);
        DataTable UntradedCustomer(DateTime start, DateTime end, string symbol, int day);
    }

    //資材部
    interface IdepMaterial
    {

    }

    //倉管部
    interface IdepHouse
    {
        DataTable stockanalysis(string day, dekModel model);
        DataTable stockanalysis(DateTime day, dekModel model);
    }

    //ini檔案使用
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
            bufferSize = 8192;
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
        // 删除ini文件下所有段落
        public void ClearAllSection(string filePath)
        {
            WritePrivateProfileString("DetaWin", null, null, filePath);
        }
    }


}
