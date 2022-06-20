using dekWS;
using ERPVIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dek_erpvis_v2.webservice
{
    public class Service
    {
        wsClient dek_ws;
        public Service()
        {
            //之後要換一下，不然連接不到
           dek_ws = new wsClient("http://210.61.157.250:57958/webservice/WebService_ToJson.asmx");
         //   dek_ws = new wsClient("http://localhost:6268/webservice/WebService_ToJson.asmx");
            iTec_Sales SLS = new iTec_Sales();
        }
        //訂單統計表
        public string GetOrdersInformation(string start, string end, string status, dekModel img_or_tbl, string line_or_custom, string qty_or_amt, int max_records)
        {
            string param = string.Format("start={0}&end={1}&status={2}&img_or_tbl={3}&line_or_custom={4}&qty_or_amt={5}&max_records={6}", start, end, status, img_or_tbl, line_or_custom, qty_or_amt, max_records);
            string path = "OrdersInformation";
            string text = "";
            dek_ws.CallWebService(path, param, out text, 30000);
            if (text == "")
                return GetOrdersInformation(start, end, status, img_or_tbl, line_or_custom, qty_or_amt, max_records);
            else
                return text;
        }
        //訂單詳細表
        public string GetOrdersDetailInformation(string start, string end, string custom, string status)
        {
            string param = string.Format("start={0}&end={1}&custom={2}&status={3}", start, end, custom, status);
            string path = "OrdersDetailInformation";
            string text = "";
            dek_ws.CallWebService(path, param, out text, 30000);
            if (text == "")
                return GetOrdersDetailInformation(start, end, custom, status);
            else
                return text;
        }
        //出貨統計表
        public string GetShipmentInformation(string start, string end, dekModel model)
        {
            string param = string.Format("start={0}&end={1}&model={2}", start, end, model);
            string path = "ShipmentInformation";
            string text = "";
            dek_ws.CallWebService(path, param, out text, 30000);
            if (text == "")
                return GetShipmentInformation(start, end, model);
            else
                return text;
        }
        //出貨詳細表
        public string GetShipmentDetailInformation(string start, string end, string custom)
        {
            string param = string.Format("start={0}&end={1}&custom={2}", start, end, custom);
            string path = "ShipmentDetailInformation";
            string text = "";
            dek_ws.CallWebService(path, param, out text, 30000);
            if (text == "")
                return GetShipmentDetailInformation(start, end, custom);
            else
                return text;
        }
        //運輸架未歸還統計
        public string GettransportrackstatisticsInformation(dekModel model)
        {
            string param = string.Format("model={0}", model);
            string path = "transportrackstatisticsInformation";
            string text = "";
            dek_ws.CallWebService(path, param, out text, 30000);
            if (text == "")
                return GettransportrackstatisticsInformation(model);
            else
                return text;
        }
        //未交易客戶資訊
        public string GetUntradedCustomerInformation(string start, string end, string symbol, int day)
        {
            string param = string.Format($"start={start}&end={end}&symbol={symbol}&day={day}");
            string path = "UntradedCustomerInformation";
            string text = "";
            dek_ws.CallWebService(path, param, out text, 500);
            if (text == "")
                return GetUntradedCustomerInformation(start, end, symbol, day);
            else
                return text;
        }
         //領料表
         public string Getpick(string Number)
        {
            string param = string.Format($"Number={Number}");
            string path = "Get_Material";
            string text = "";
            dek_ws.CallWebService(path, param, out text, 500);
            if (text == "")
                return Getpick(Number);
            else
                return text;
        }
    }
}