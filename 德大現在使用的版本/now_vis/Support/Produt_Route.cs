using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Support
{
    public class Produt_Route
    {
        public static float Cost_工序總成本(int 加工時間, int 上下料時間, float 單位成本, float 其他成本)
        {
            float 總成本 = (加工時間 + 上下料時間) * 單位成本 + 其他成本;
            return 總成本;
        }
        public static float Cost_取得訂單成本(string OrderID)
        {
            float 訂單的產品總成本 = 0;
            float 訂單的其他總成本 = 0;
            //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
            string strCmd = "SELECT PK_Index,OrderID,NcProductID,Product_Quantity FROM MakeOrder_Product_Detail where OrderID = '"+ OrderID + "'";
            DataTable dt = Support.DataTableUtils.GetDataTable(strCmd);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                訂單的產品總成本 += (Convert.ToSingle(Cost_產品成本(dt.Rows[i]["NcProductID"].ToString())[0].Split(',')[1].ToString()) + Convert.ToSingle(Cost_產品成本(dt.Rows[i]["NcProductID"].ToString())[0].Split(',')[2].ToString())) * Convert.ToSingle(dt.Rows[i]["Product_Quantity"].ToString());
            }
            //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
            string strCmd_ = "SELECT Order_ID, sum(Cost_Money) FROM MakeOrder_Cost_Detail where Order_ID = '"+ OrderID + "' group by Order_ID";
            DataTable dt_ = Support.DataTableUtils.GetDataTable(strCmd_);

            if (dt_.Rows.Count != 0)
            {
                訂單的其他總成本 = Convert.ToSingle(dt_.Rows[0][1].ToString());
            }

            dt.Clear();
            dt_.Clear();

            return 訂單的產品總成本 + 訂單的其他總成本;
        }
        public static float Cost_產品所有成本(string 產品編號)
        {
            return 0;
        }
        public static List<string> Cost_產品成本(string 產品ID)
        {
            List<string> CostList = new List<string>();
            string 產品其他成本 = "0";
            string 工序總成本 = "0";

            string strCmd = "SELECT WorkProduct_ID,sum(Cost_Money) as ProcessTotalCost FROM WorkProduct_Cost_Detail where WorkProduct_Cost_Detail.WorkProduct_ID ='" + 產品ID + "' group by WorkProduct_Cost_Detail.WorkProduct_ID";
            DataTable dt其他總成本 = Support.DataTableUtils.GetDataTable(strCmd);
            string strCmd_ = "SELECT NcProductID,sum((WorkProcess.WP_WorkTT + WorkProcess.WP_MaterielTT)/60*WorkProcess.WP_Price + WorkProcess.WP_CostPrice) as ProductTotalCost FROM WorkProduct_Process_Detail inner join WorkProcess on WorkProcess.WorkProcess_ID = WorkProduct_Process_Detail.WorkProcess_ID where NcProductID = '" + 產品ID + "' GROUP BY  WorkProduct_Process_Detail.NcProductID";
            DataTable dt_總工序成本 = Support.DataTableUtils.GetDataTable(strCmd_);
            if (dt其他總成本.Rows.Count != 0)
            {
                產品其他成本 = dt其他總成本.Rows[0][1].ToString();
            }

            if (dt_總工序成本.Rows.Count != 0)
            {
                工序總成本 = dt_總工序成本.Rows[0][1].ToString();
            }
            strCmd = 產品ID + "," + 產品其他成本 + "," + 工序總成本;
            CostList.Add(strCmd);
            return CostList;
        }
    }
}
