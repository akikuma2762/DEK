using ERPVIS;
using MongoDB.Bson;
using Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Xml;

namespace dek_erpvis_v2.webservice
{    /// <summary>
     ///WebService_ToJson 的摘要描述
     /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
    // [System.Web.Script.Services.ScriptService]
    public class WebService_ToJson : System.Web.Services.WebService
    {
        IniManager iniManager = new IniManager("D:/DATAWINCommand.ini");
        iTec_Sales SLS = new iTec_Sales();
        //訂單統計表
        [WebMethod(Description = "DemoMethod to get total.", EnableSession = true)]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public string OrdersInformation(string start, string end, OrderStatus status, dekModel img_or_tbl, OrderLineorCust line_or_custom, string qty_or_amt, int max_records)
        {
            //type(Image/Table)
            DataTable dt = new DataTable();
            OrderType type;
            if (qty_or_amt == "qty")
                type = OrderType.數量;
            else
                type = OrderType.金額;

            dt = SLS.Orders(start, end, status, img_or_tbl, line_or_custom, type, max_records);
            return Return_Josn(dt);
        }

        //訂單詳細表
        [WebMethod(Description = "DemoMethod to get total.", EnableSession = true)]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public string OrdersDetailInformation(string start, string end, string custom, OrderStatus status)
        {
            //type(Image/Table)
            DataTable dt = new DataTable();
            dt = SLS.Orders_Details(start, end, custom, status);
            return Return_Josn(dt);
        }


        //出貨統計表
        [WebMethod(Description = "DemoMethod to get total.", EnableSession = true)]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public string ShipmentInformation(string start, string end, dekModel model)
        {
            //type(Image/Table)
            DataTable dt = new DataTable();
            dt = SLS.Shipment(start, end, model);
            return Return_Josn(dt);
        }

        //出貨詳細表-小計部分--等等做
        [WebMethod]
        public XmlNode GetShipment_details(string start, string end, string custom, string item)
        {
            //dt_st(開始時間) dt_ed(結束時間) cust(顧客代號) item(商品代號)
            DataTable dt = new DataTable();
            dt = SLS.Get_Shipment(start, end, custom, item);

            XmlDocument xmlDoc = new XmlDocument();
            XmlElement xmlElem = xmlDoc.CreateElement("ROOT_PIE");
            if (dt != null)
            {
                if (dt.Rows.Count <= 0)
                {
                    xmlElem.SetAttribute("Value", "0");
                }
                else
                {
                    xmlElem.SetAttribute("Value", DataTableUtils.toString(dt.Rows.Count));
                    xmlElem.SetAttribute("item", item);
                    xmlElem.SetAttribute("cust", custom);
                    xmlDoc.AppendChild(xmlElem);
                    foreach (DataRow row in dt.Rows)
                    {
                        XmlElement xmlElemA = xmlDoc.CreateElement("Group");
                        xmlElemA.SetAttribute("序列", DataTableUtils.toString(dt.Rows.IndexOf(row) + 1));
                        xmlElemA.SetAttribute("出貨日期", DataTableUtils.toString(row["出貨日期"]));
                        //xmlElemA.SetAttribute("訂單號碼", DataTableUtils.toString(row["訂單號碼"]));
                        xmlElemA.SetAttribute("出貨單號", DataTableUtils.toString(row["出貨單號"]));
                        xmlElemA.SetAttribute("刀庫編號", DataTableUtils.toString(row["刀庫編號"]));
                        xmlElemA.SetAttribute("CCS", DataTableUtils.toString(row["CCS"]));
                        // xmlElemA.SetAttribute("數量", DataTableUtils.toString(row["數量"]));
                        xmlElemA.SetAttribute("訂單備註", DataTableUtils.toString(row["訂單備註"]));
                        xmlDoc.DocumentElement.AppendChild(xmlElemA);
                    }
                    return xmlDoc.DocumentElement;
                }
            }
            else
            {
                xmlElem.SetAttribute("Value", "-1");
            }

            xmlDoc.AppendChild(xmlElem);
            dt.Dispose();
            return xmlDoc.DocumentElement;
        }

        //出貨詳細表
        [WebMethod(Description = "DemoMethod to get total.", EnableSession = true)]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public string ShipmentDetailInformation(string start, string end, string custom)
        {
            //type(Image/Table)
            DataTable dt = new DataTable();
            dt = SLS.Shipment_Detail(start, end, custom);
            return Return_Josn(dt);
        }
        //運輸架在外統計表
        [WebMethod(Description = "DemoMethod to get total.", EnableSession = true)]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public string transportrackstatisticsInformation(dekModel model)
        {
            //type(Image/Table)
            DataTable dt = new DataTable();
            dt = SLS.Transportrackstatistics(model);
            return Return_Josn(dt);
        }
        //未交易客戶
        [WebMethod(Description = "DemoMethod to get total.", EnableSession = true)]
        [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
        public string UntradedCustomerInformation(string start, string end, string symbol, int day)
        {
            //start(開始時間) end(結束時間) symbol(< > =) day(差距幾天) 
            DataTable dt = new DataTable();
            dt = SLS.UntradedCustomer(start, end, symbol, day);
            return Return_Josn(dt);
        }
        //-----------------------------------------------------------------------
        //返回JSON文字
        string Return_Josn(DataTable dt)
        {
            BsonDocument bson = MongoUtils.DataTableToBsonDocument(dt);
            return bson.ToJson();
        }
    }
}