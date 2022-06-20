using dek_erpvis_v2.cls;
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
    ///dp_PM 的摘要描述
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
    // [System.Web.Script.Services.ScriptService]
    public class dp_PM : System.Web.Services.WebService
    {

        string GetConnByDekdekVisAssmHor = clsDB_Server.GetConntionString_MsSQL("210.61.157.250,5872", "DetaVisHor", "sa", "asus54886961");
        string GetConnByDekVisTmp = clsDB_Server.GetConntionString_MsSQL("210.61.157.250,5872", "dekVisAssm", "sa", "asus54886961");

        clsDB_Server clsDB_sw = new clsDB_Server("");
        myclass myclass = new myclass();
        [WebMethod]
        public XmlNode Get_ErrorDetails(string ID, string Error_ID, string Type)
        {
            string Link = "";
            if (Type.ToLower() == "ver")
                Link = GetConnByDekVisTmp;
            else if (Type.ToLower() == "hor")
                Link = GetConnByDekdekVisAssmHor;

            clsDB_sw.dbOpen(Link);
            DataTableUtils.Conn_String = Link;
            //子DATATABLE
            string sqlcmd = "select 異常維護編號, 維護人員姓名,維護人員單位,維護內容,時間紀錄,圖片檔名 from 工作站異常維護資料表 where 父編號 = " + ID + " and 排程編號 like '%" + Error_ID + "%' order by 異常維護編號 asc";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);

            XmlDocument xmlDoc = new XmlDocument();
            XmlElement xmlElem = xmlDoc.CreateElement("ROOT_PIE");
            if (dt != null)
            {
                if (dt.Rows.Count <= 0)
                {
                    xmlElem.SetAttribute("Value", DataTableUtils.toString(dt.Rows.Count));
                    xmlElem.SetAttribute("ID", ID);
                    xmlElem.SetAttribute("Error_ID", Error_ID);
                    xmlDoc.AppendChild(xmlElem);
                    XmlElement xmlElemA = xmlDoc.CreateElement("Group");
                    xmlElemA.SetAttribute("序列", "");
                    xmlElemA.SetAttribute("人員", "");
                    xmlElemA.SetAttribute("單位", "");
                    xmlElemA.SetAttribute("內容", "");
                    xmlElemA.SetAttribute("圖片檔名", "");
                    xmlDoc.DocumentElement.AppendChild(xmlElemA);
                    return xmlDoc.DocumentElement;
                }
                else
                {
                    xmlElem.SetAttribute("Value", DataTableUtils.toString(dt.Rows.Count));
                    xmlElem.SetAttribute("ID", ID);
                    xmlElem.SetAttribute("Error_ID", Error_ID);
                    xmlDoc.AppendChild(xmlElem);
                    foreach (DataRow row in dt.Rows)
                    {
                        XmlElement xmlElemA = xmlDoc.CreateElement("Group");
                        xmlElemA.SetAttribute("序列", "<center>" + "<input  style='width:26%' type ='checkbox' name='items' value='" + row["異常維護編號"].ToString() + "' id ='check-all' class='flat'>\n" + "</center>");
                        xmlElemA.SetAttribute("人員", DataTableUtils.toString("<center><font color=\"red\">" + row["時間紀錄"]) + "</font>" + " <br> " + DataTableUtils.toString(row["維護人員姓名"]) + "<center>");
                        xmlElemA.SetAttribute("單位", "<center>" + DataTableUtils.toString(row["維護人員單位"]) + "</center>");
                        xmlElemA.SetAttribute("內容", DataTableUtils.toString(row["維護內容"]));
                        string URLLink = "";
                        if (DataTableUtils.toString(row["圖片檔名"]) != null && DataTableUtils.toString(row["圖片檔名"]) != "")
                        {
                            string[] image = DataTableUtils.toString(row["圖片檔名"]).Split('\n');
                            for (int i = 0; i < image.Length - 1; i++)
                            {
                                int num = i + 1;
                                string[] sp = image[i].Split('.');
                                if (sp[sp.Length - 1] == "mp4" || sp[sp.Length - 1] == "MP4")//判斷是影片或是圖片
                                    URLLink += "<u><a href = '" + image[i] + " ' target='_blank'>影片" + num + "</u><br>";
                                else if (sp[sp.Length - 1] == "XLS" || sp[sp.Length - 1] == "xls" || sp[sp.Length - 1] == "XLSX" || sp[sp.Length - 1] == "xlsx")
                                    URLLink += "<u><a href = '" + image[i] + " ' target='_blank'>EXCEL表" + num + "</u><br>";
                                else if (sp[sp.Length - 1] == "PDF" || sp[sp.Length - 1] == "pdf")
                                    URLLink += "<u><a href = '" + image[i] + " ' target='_blank'>PDF檔" + num + "</u><br>";
                                else
                                    URLLink += "<u><a href = '" + image[i] + " ' target='_blank'>圖片" + num + "</u><br>";
                            }
                        }
                        xmlElemA.SetAttribute("圖片檔名", URLLink);
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
