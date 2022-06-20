﻿using dek_erpvis_v2.cls;
using Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml;
using dekERP_dll.dekErp;
using MongoDB.Bson;
using Newtonsoft.Json;
using System.Web.Script.Services;

namespace dek_erpvis_v2.webservice
{
    /// <summary>
    ///dpPD_mqp 的摘要描述
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
    [System.Web.Script.Services.ScriptService]
    public class dpPD_mqp : System.Web.Services.WebService
    {
        ERP_Materials PCD = new ERP_Materials();

        [WebMethod]
        public XmlNode GetTheDataOfTheItemCodeToPie(string item_code, string date_str, string date_end)
        {
            myclass myclass = new myclass();
            XmlDocument xmlDoc = new XmlDocument();
            GlobalVar.UseDB_setConnString(myclass.GetConnByDetaSowon);

            //to get pie data
            string strCmd = "use FJWSQL select a.領料數,a.品名規格,a.用途說明 from (select ITEMIOS.ITEM_NO as 品號,item.ITEMNM as 品名規格,sum(itemios.QTY2) as 領料數,(CASE WHEN CHARINDEX ( '(',ITEMIOS.S_DESC COLLATE Latin1_General_CS_AS) > 0 THEN SUBSTRING( ITEMIOS.S_DESC,0,CHARINDEX('(',ITEMIOS.S_DESC)) ELSE ITEMIOS.S_DESC END ) as 用途說明 from ITEMIOS left join item as item on ITEMIOS.ITEM_NO=item.ITEM_NO where ITEMIOS.ITEM_NO = '" + item_code + "' and TRN_DATE>" + date_str + " and TRN_DATE< " + date_end + " group by ITEMIOS.ITEM_NO,item.ITEMNM,(CASE WHEN CHARINDEX ( '(',ITEMIOS.S_DESC COLLATE Latin1_General_CS_AS) > 0 THEN SUBSTRING( ITEMIOS.S_DESC,0,CHARINDEX('(',ITEMIOS.S_DESC)) ELSE ITEMIOS.S_DESC END ) having sum(itemios.QTY2) > 0 ) as a order by a.品號 asc";
            DataTable dt = DataTableUtils.GetDataTable(strCmd);
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
                    xmlElem.SetAttribute("item_name", DataTableUtils.toString(dt.Rows[0]["品名規格"]));
                    xmlDoc.AppendChild(xmlElem);
                    foreach (DataRow row in dt.Rows)
                    {
                        XmlElement xmlElemA = xmlDoc.CreateElement("Group");
                        xmlElemA.SetAttribute("用途說明", row["用途說明"].ToString());
                        xmlElemA.SetAttribute("領料數", DataTableUtils.toString(DataTableUtils.toDouble(DataTableUtils.toString(row["領料數"]))));
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
        public XmlNode GetTheDataOfTheItemCodeToCol(string item_code, string date_str, string date_end)
        {
            myclass myclass = new myclass();
            GlobalVar.UseDB_setConnString(myclass.GetConnByDetaSowon);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", ""));

            XmlElement RootElement = xmlDoc.CreateElement("ROOT_COL");//建立父節點
            xmlDoc.AppendChild(RootElement);

            string strCmd = "use FJWSQL select distinct a.用途說明 from (select (CASE WHEN CHARINDEX ( '(',ITEMIOS.S_DESC COLLATE Latin1_General_CS_AS) > 0 THEN SUBSTRING( ITEMIOS.S_DESC,0,CHARINDEX('(',ITEMIOS.S_DESC)) ELSE ITEMIOS.S_DESC END ) as 用途說明 ,sum(itemios.QTY2) as 領料數 from ITEMIOS left join item as item on ITEMIOS.ITEM_NO=item.ITEM_NO where ITEMIOS.ITEM_NO = '" + item_code + "' and TRN_DATE>'" + date_str + "'and TRN_DATE<'" + date_end + "' group by ITEMIOS.ITEM_NO,item.ITEMNM,ITEMIOS.S_DESC,TRN_DATE) as a group by a.領料數,a.用途說明 HAVING SUM(a.領料數) > 0";
            DataTable dt = DataTableUtils.DataTable_GetTable(strCmd);

            RootElement.SetAttribute("Value", DataTableUtils.toString(dt.Rows.Count));
            RootElement.SetAttribute("item_code", item_code);
            RootElement.SetAttribute("item_name", "");
            foreach (DataRow row_用途 in dt.Rows)
            {
                string 用途說明 = DataTableUtils.toString(row_用途["用途說明"]);
                XmlElement xmlElem = xmlDoc.CreateElement("for_" + 用途說明);//在父節點下建立子節點

                strCmd = "use FJWSQL select a.領料月份 from (select SUBSTRING(TRN_DATE,0,7) as 領料月份 , ITEMIOS.ITEM_NO as 品號,item.ITEMNM as 品名規格 from ITEMIOS left join item as item on ITEMIOS.ITEM_NO=item.ITEM_NO where ITEMIOS.ITEM_NO = '" + item_code + "' and TRN_DATE>'" + date_str + "'and TRN_DATE<'" + date_end + "' group by ITEMIOS.ITEM_NO,item.ITEMNM,ITEMIOS.S_DESC,TRN_DATE) as a group by a.品號,a.品名規格,a.領料月份 order by a.領料月份 ";
                DataTable dr = DataTableUtils.DataTable_GetTable(strCmd);
                foreach (DataRow row_月份 in dr.Rows)
                {
                    strCmd = "use FJWSQL select a.領料月份,sum(a.領料數) as 領料數 ,a.用途說明 from (select SUBSTRING(TRN_DATE,0,7) as 領料月份 , ITEMIOS.ITEM_NO as 品號,item.ITEMNM as 品名規格,sum(itemios.QTY2) as 領料數,(CASE WHEN CHARINDEX ( '(',ITEMIOS.S_DESC COLLATE Latin1_General_CS_AS) > 0 THEN SUBSTRING( ITEMIOS.S_DESC,0,CHARINDEX('(',ITEMIOS.S_DESC)) ELSE ITEMIOS.S_DESC END ) as 用途說明 from ITEMIOS left join item as item on ITEMIOS.ITEM_NO=item.ITEM_NO where ITEMIOS.ITEM_NO = '" + item_code + "' and TRN_DATE>'" + date_str + "'and TRN_DATE<'" + date_end + "' group by ITEMIOS.ITEM_NO,item.ITEMNM,ITEMIOS.S_DESC,TRN_DATE) as a where a.用途說明 ='" + DataTableUtils.toString(row_用途["用途說明"]) + "' and a.領料月份 ='" + DataTableUtils.toString(row_月份["領料月份"]) + "'  group by a.品號,a.品名規格,a.領料月份,a.用途說明 HAVING SUM(a.領料數) > 0 order by a.領料月份 ";
                    DataRow Get_row = DataTableUtils.DataTable_GetDataRow(strCmd);
                    if (Get_row != null)
                    {

                        XmlElement xmlElemA = xmlDoc.CreateElement("item");//子節點下建立參數
                        xmlElemA.SetAttribute("領料月份", DataTableUtils.toString(row_月份["領料月份"]));
                        xmlElemA.SetAttribute("用途說明", 用途說明);
                        xmlElemA.SetAttribute("領料數", DataTableUtils.toString(DataTableUtils.toDouble(Get_row["領料數"])));
                        xmlElem.AppendChild(xmlElemA);
                    }
                    else
                    {
                        XmlElement xmlElemA = xmlDoc.CreateElement("item");//子節點下建立參數
                        xmlElemA.SetAttribute("領料月份", DataTableUtils.toString(row_月份["領料月份"]));
                        xmlElemA.SetAttribute("用途說明", 用途說明);
                        xmlElemA.SetAttribute("領料數", "0");
                        xmlElem.AppendChild(xmlElemA);
                    }
                }
                RootElement.AppendChild(xmlElem);
            }
            return xmlDoc.DocumentElement;
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Get_Material(string Number)
        {
            DataTable dt = PCD.pick_list_datatable(Number + "#", "sowon");
            if (HtmlUtil.Check_DataTable(dt))
            {
                return JsonConvert.SerializeObject(dt);
            }
            else
                return "無資料";
        }
    }
}
