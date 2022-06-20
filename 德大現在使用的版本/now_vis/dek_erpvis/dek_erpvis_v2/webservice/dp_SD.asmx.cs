using dek_erpvis_v2.cls;
using dekERP_dll.dekErp;
using Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml;


namespace dek_erpvis_v2.webservice
{
    /// <summary>
    ///dp_SD 的摘要描述
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
    [System.Web.Script.Services.ScriptService]
    public class dp_SD : System.Web.Services.WebService
    {

        iTec_Sales sales = new iTec_Sales();
        ERP_Sales SLS = new ERP_Sales();
        [WebMethod]
        public XmlNode GetShipment_details(string cust_name, string item_code, string date_str, string date_end)
        {
            clsDB_Server clsDB_sw = new clsDB_Server(myclass.GetConnByDetaSowon);
            //clsDB_sw.dbOpen();
            string sqlcmd = 德大機械.業務部_出貨詳細.客戶出貨詳細表_webservice(cust_name, item_code, date_str, date_end);
            DataTable dt = clsDB_sw.DataTable_GetTable(sqlcmd);

            //Bsondocument bson = MongodbUtils.datatabletoBsondocument(dt);
            // bson --> 壓縮 or 加密
            //return html(table);
            // 

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
                    xmlElem.SetAttribute("item_code", item_code);
                    xmlElem.SetAttribute("cust_name", cust_name);
                    xmlDoc.AppendChild(xmlElem);
                    foreach (DataRow row in dt.Rows)
                    {
                        XmlElement xmlElemA = xmlDoc.CreateElement("Group");
                        xmlElemA.SetAttribute("序列", DataTableUtils.toString(dt.Rows.IndexOf(row) + 1));
                        xmlElemA.SetAttribute("出貨日期", HtmlUtil.changetimeformat(DataTableUtils.toString(row["出貨日期"])));
                        xmlElemA.SetAttribute("出貨單號", DataTableUtils.toString(row["出貨單號"]));
                        xmlElemA.SetAttribute("製造批號", DataTableUtils.toString(row["製造批號"]));
                        xmlElemA.SetAttribute("CCS", DataTableUtils.toString(row["CCS"]));
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
        [WebMethod]
        public XmlNode GetShipment_details_dek(string cust_name, string item_code, string date_str, string date_end)
        {
            DataTable dt = sales.Get_Shipment(date_str, date_end, cust_name, item_code);

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
                    xmlElem.SetAttribute("item_code", item_code);
                    xmlElem.SetAttribute("cust_name", cust_name);
                    xmlDoc.AppendChild(xmlElem);
                    foreach (DataRow row in dt.Rows)
                    {
                        XmlElement xmlElemA = xmlDoc.CreateElement("Group");
                        xmlElemA.SetAttribute("序列", DataTableUtils.toString(dt.Rows.IndexOf(row) + 1));
                        xmlElemA.SetAttribute("出貨日期", HtmlUtil.changetimeformat(DataTableUtils.toString(row["出貨日期"])));
                        xmlElemA.SetAttribute("出貨單號", DataTableUtils.toString(row["出貨單號"]));
                        xmlElemA.SetAttribute("製造批號", DataTableUtils.toString(row["製造批號"]));
                        xmlElemA.SetAttribute("CCS", DataTableUtils.toString(row["CCS"]));

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



        [WebMethod]
        public XmlNode GetShipment_details_New(string date_str, string date_end, string cust_name, string item_code, string Factory)
        {

            DataTable dt = SLS.Get_Shipment(date_str, date_end, cust_name, item_code, Factory);

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
                    xmlElem.SetAttribute("item_code", item_code);
                    xmlElem.SetAttribute("cust_name", cust_name);
                    xmlDoc.AppendChild(xmlElem);
                    foreach (DataRow row in dt.Rows)
                    {
                        XmlElement xmlElemA = xmlDoc.CreateElement("Group");
                        xmlElemA.SetAttribute("序列", DataTableUtils.toString(dt.Rows.IndexOf(row) + 1));
                        xmlElemA.SetAttribute("出貨日期", HtmlUtil.changetimeformat(DataTableUtils.toString(row["出貨日期"])));
                        xmlElemA.SetAttribute("出貨單號", DataTableUtils.toString(row["出貨單號"]));
                        xmlElemA.SetAttribute("製造批號", DataTableUtils.toString(row["製造批號"]));
                        xmlElemA.SetAttribute("CCS", DataTableUtils.toString(row["CCS"]));
                        xmlElemA.SetAttribute("數量", DataTableUtils.toString(row["數量"]));
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
    }
}
