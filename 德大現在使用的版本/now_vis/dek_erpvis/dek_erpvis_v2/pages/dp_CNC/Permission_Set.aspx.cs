using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace dek_erpvis_v2.pages.dp_CNC
{
    public partial class Machine_OverView : System.Web.UI.Page
    {
        //public string[] RealTime_Info = { "設備名稱", "校機人員", "加工人員", "設備狀態", "設備稼動(百分比 - 當日)", "設備稼動(長條圖 - 當日)", "客戶名稱", "產品名稱", "料件編號", "加工程式", "生產件數", "預計生產件數", "預計完工時間", "問題回報" };
        public string[] RealTime_Info = { "校機人員", "加工人員", "設備稼動", "設備稼動_長條圖", "客戶名稱", "產品名稱", "料件編號", "加工程式", "生產件數", "預計生產件數", "生產進度", "預計完工時間", "問題回報", "異警資訊" };

        MTLinkiDB.MTLinkiDB MTLinki_DB = new MTLinkiDB.MTLinkiDB();

        protected void Page_Load(object sender, EventArgs e)
        {
            //MTLinki_DB.GetConntionString_MongoDB("MTLINKi", "dek2241", "2241", "172.23.10.102");
            MTLinki_DB.GetConntionString_MongoDB("MTLINKi", "dek2241", "2241", "192.168.1.221", "");
            Get_RealTime_Info();
        }

        private void Get_RealTime_Info()
        {
            CheckBoxList checkBoxList = new CheckBoxList();
            checkBoxList.ID = "checkBoxList_LINE";
            List<string> ST_RealTime_Info = MTLinki_DB.Get_DB_Info("_dek_RealTime_Info", Query.NE("_id", 0), new string[] { "顯示資訊", "顯示與否" });

            for (int iIndex = 0; iIndex < ST_RealTime_Info.Count; iIndex++)
            {
                checkBoxList.Items.Add(ST_RealTime_Info[iIndex].Split(',')[0]);
                if (ST_RealTime_Info[iIndex].Split(',')[1] == "Y")
                    checkBoxList.Items[iIndex].Selected = true;
                else
                    checkBoxList.Items[iIndex].Selected = false;
            }
            Panel_Line.Controls.Add(checkBoxList);
        }

        protected void Button_SetRealTime_Info_Click(object sender, EventArgs e)
        {
            Set_RealTime_Info();
        }

        public void Set_RealTime_Info()
        {
            int iIndex = 0;
            MongoDB.Bson.BsonDocument insert_data;
            MTLinki_DB.RemoveAll_MongoDB_Data("_dek_RealTime_Info");
            foreach (Control cbxlist in this.Panel_Line.Controls)
            {
                if (cbxlist is CheckBoxList)
                {
                    foreach (ListItem Items in ((CheckBoxList)cbxlist).Items)
                    {
                        MTLinki_DB.SelectDataTable_IMongo("_dek_RealTime_Info");
                        if (Items.Selected == true)
                            insert_data = MTLinki_DB.getNewData("{'顯示資訊':'" + RealTime_Info[iIndex] + "','顯示與否':'Y'}");
                        else
                            insert_data = MTLinki_DB.getNewData("{'顯示資訊':'" + RealTime_Info[iIndex] + "','顯示與否':'N'}");
                        if (insert_data.Count() > 0)
                            MTLinki_DB.Insert_MongoDB_Data_Bson(insert_data);
                        iIndex++;
                    }
                }
            }
        }
    }
}