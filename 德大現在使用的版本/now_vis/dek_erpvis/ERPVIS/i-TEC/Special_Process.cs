using Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPVIS.i_TEC
{
    //這邊是要經過特殊處理，再回傳回去的
    public class Special_Process
    {
        //----------------------------------------------------訂單統計表---------------------------------------------
        public static DataTable Order_AddColumn(DataTable dt, List<string> list)
        {
            bool ok = Return_ok(list, "產線代號");
            if (ok == true)
                return dt;
            else
            {
                DataTable dr = new DataTable();
                dr.Columns.Add("產線代號");
                for (int i = 0; i < list.Count; i++)
                    dr.Columns.Add(list[i]);


                foreach (DataRow row in dt.Rows)
                {
                    DataRow rqw = dr.NewRow();
                    rqw["產線代號"] = "10000";
                    for (int i = 0; i < list.Count; i++)
                        rqw[list[i]] = row[list[i]];

                    dr.Rows.Add(rqw);
                }
                return dr;
            }
        }
        //----------------------------------------------------訂單統計表---------------------------------------------
        public static DataTable OrdersDetail_Table(DataTable dt, List<string> list)
        {

            bool ok = Return_ok(list, "產線代號");
            if (ok == true)
                return dt;
            else
            {
                DataTable dr = new DataTable();
                dr.Columns.Add("產線代號");
                for (int i = 0; i < list.Count; i++)
                    dr.Columns.Add(list[i]);


                foreach (DataRow row in dt.Rows)
                {
                    DataRow rqw = dr.NewRow();
                    rqw["產線代號"] = "10000";
                    for (int i = 0; i < list.Count - 1; i++)
                        rqw[list[i]] = row[list[i]];
                    DataTable item = dt.DefaultView.ToTable(true, new string[] { "CCS" });
                    //for (int i = 0; i < item.Rows.Count; i++)
                    //{
                    //    string sqlcmd = "CCS ='" + DataTableUtils.toString(row["CCS"]) + "'";
                    //    DataRow[] rows = dt.Select(sqlcmd);
                    //    if (rows.Length >= DataTableUtils.toInt(DataTableUtils.toString(row["數量"])))
                    //        rqw["數量"] = "1";
                    //    else

                    //}
                    rqw["數量"] = row["數量"];
                    dr.Rows.Add(rqw);
                }
                return dr;
            }
        }
        //----------------------------------------------------出貨統計表---------------------------------------------
        //出貨專用格式(圖片)
        public static DataTable Shipment_Image(DataTable dt, List<string> list)
        {
            //要做出的表格(產線代號 數量)
            bool ok = Return_ok(list, "產線代號");

            if (ok == true)
                return dt;
            else
            {
                DataTable dr = new DataTable();
                // dr.Columns.Add("產線代號");
                dr.Columns.Add("數量");
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dr.NewRow();
                    //row["產線代號"] = "10000";//若該公司只有一條產線，則這個提供調整
                    if (dt.Rows.Count > 1)
                    {
                        if (DataTableUtils.toString(dt.Rows[0]["分類"]) == "3")
                            row["數量"] = DataTableUtils.toInt(DataTableUtils.toString(dt.Rows[0]["數量"])) - DataTableUtils.toInt(DataTableUtils.toString(dt.Rows[1]["數量"]));
                        else
                            row["數量"] = DataTableUtils.toInt(DataTableUtils.toString(dt.Rows[1]["數量"])) - DataTableUtils.toInt(DataTableUtils.toString(dt.Rows[0]["數量"]));
                    }
                    else
                    {
                        if (DataTableUtils.toString(dt.Rows[0]["分類"]) == "3")
                            row["數量"] = DataTableUtils.toInt(DataTableUtils.toString(dt.Rows[0]["數量"]));
                        else
                            row["數量"] = 0 - DataTableUtils.toInt(DataTableUtils.toString(dt.Rows[0]["數量"]));
                    }
                    dr.Rows.Add(row);
                }
                return dr;
            }
        }
        //出貨專用格式(表格)
        public static DataTable Shipment_Table(DataTable dt, List<string> list)
        {
            //要做出的表格(客戶簡稱 產線代號 數量)
            bool ok = Return_ok(list, "產線代號");

            if (ok == true)
                return dt;
            else
            {
                if (dt.Rows.Count > 0)
                {
                    string Name = list[0];
                    string Classification = list[1];
                    string count = list[2];
                    DataTable dr = new DataTable();
                    if (dt.Rows.Count > 0)
                    {
                        dr.Columns.Add("產線代號");
                        dr.Columns.Add(Name);

                        dr.Columns.Add(count, typeof(int));

                        dr.PrimaryKey = new DataColumn[] { dr.Columns[Name] };
                        string pre_item = "";
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow row = dr.NewRow();
                            row["產線代號"] = "10000";
                            if (pre_item == DataTableUtils.toString(dt.Rows[i][Name]))
                            {
                                row[Name] = transportrack_name(DataTableUtils.toString(dt.Rows[i][Name]));

                                if (DataTableUtils.toString(dt.Rows[i][Classification]) == "4")
                                    row[count] = DataTableUtils.toInt(DataTableUtils.toString(dt.Rows[i - 1][count])) - DataTableUtils.toInt(DataTableUtils.toString(dt.Rows[i][count]));
                                else
                                    row[count] = DataTableUtils.toInt(DataTableUtils.toString(dt.Rows[i][count])) - DataTableUtils.toInt(DataTableUtils.toString(dt.Rows[i - 1][count]));

                                //找到row的位置 把它移除
                                DataRow[] rqw = dr.Select(Name + "='" + transportrack_name(DataTableUtils.toString(row[Name])) + "'");
                                int num = dr.Rows.IndexOf(rqw[0]);
                                dr.Rows.RemoveAt(num);
                            }
                            else
                            {
                                row[Name] = transportrack_name(DataTableUtils.toString(dt.Rows[i][Name]));
                                row[count] = DataTableUtils.toString(dt.Rows[i][count]);
                                pre_item = DataTableUtils.toString(dt.Rows[i][Name]);
                            }
                            dr.Rows.Add(row);
                        }
                    }
                    DataView view = new DataView(dr);
                    view.Sort = "數量 desc";
                    DataTable selected = view.ToTable();
                    return selected;
                }
                else
                    return dt;
            }
        }

        //----------------------------------------------------出貨統計表---------------------------------------------
        //----------------------------------------------------出貨詳細表---------------------------------------------
        public static DataTable ShipmentDetails_Table(DataTable dt, List<string> list)
        {

            bool ok = Return_ok(list, "產線代號");
            if (ok == true)
                return dt;
            else
            {
                DataTable dr = new DataTable();
                dr.Columns.Add("產線代號");
                dr.Columns.Add("客戶簡稱");

                dr.Columns.Add("品號");
                dr.Columns.Add("品名");
                dr.Columns.Add("數量");

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rqw in dt.Rows)
                    {
                        DataRow row = dr.NewRow();
                        row["產線代號"] = "10000";
                        row["客戶簡稱"] = rqw["客戶簡稱"];

                        row["品號"] = rqw["品號"];
                        row["品名"] = rqw["品名"];
                        if (DataTableUtils.toString(rqw["銷貨或退回"]) == "3")
                            row["數量"] = rqw["數量"];
                        else
                            row["數量"] = "-" + rqw["數量"];
                        dr.Rows.Add(row);
                    }
                }
                return dr;
            }
        }
        //----------------------------------------------------出貨詳細表---------------------------------------------
        //----------------------------------------------------運輸架區-----------------------------------------------
        //運輸架專用格式(圖片)
        public static DataTable transportrackstatistics_Image(DataTable dt, List<string> list)
        {
            //要做出的表格(品號 數量)
            if (list.Count == 3)
            {
                string Name = list[0];//品號
                string Classification = list[1];//分類
                string count = list[2];//數量
                DataTable dr = new DataTable();
                if (dt.Rows.Count > 0)
                {
                    dr.Columns.Add(Name);
                    dr.Columns.Add(count, typeof(int));
                    dr.PrimaryKey = new DataColumn[] { dr.Columns[Name] };
                    string pre_item = "";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow row = dr.NewRow();
                        if (pre_item == DataTableUtils.toString(dt.Rows[i][Name]))
                        {
                            row[Name] = transportrack_name(DataTableUtils.toString(dt.Rows[i][Name]));
                            if (DataTableUtils.toString(dt.Rows[i][Classification]) == "4")
                                row[count] = DataTableUtils.toInt(DataTableUtils.toString(dt.Rows[i - 1][count])) - DataTableUtils.toInt(DataTableUtils.toString(dt.Rows[i][count]));
                            else
                                row[count] = DataTableUtils.toInt(DataTableUtils.toString(dt.Rows[i][count])) - DataTableUtils.toInt(DataTableUtils.toString(dt.Rows[i - 1][count]));

                            //找到row的位置 把它移除
                            DataRow[] rqw = dr.Select(Name + "='" + transportrack_name(DataTableUtils.toString(row[Name])) + "'");
                            int num = dr.Rows.IndexOf(rqw[0]);
                            dr.Rows.RemoveAt(num);
                        }
                        else
                        {
                            row[Name] = transportrack_name(DataTableUtils.toString(dt.Rows[i][Name]));
                            row[count] = DataTableUtils.toString(dt.Rows[i][count]);
                            pre_item = DataTableUtils.toString(dt.Rows[i][Name]);
                        }
                        dr.Rows.Add(row);
                    }
                    DataView view = new DataView(dr);
                    view.Sort = "數量 desc";
                    DataTable selected = view.ToTable();
                    return selected;
                }
                else
                    return null;
            }
            else
                return dt;
        }

        //運輸架專用格式(表格)
        public static DataTable transportrackstatistics_Table(DataTable dt, List<string> list)
        {
            //要做出的表格(客戶簡稱 運輸架名稱 小計)
            if (list.Count > 3)
            {
                string custom = list[0];//客戶
                string item_no = list[1];//品號
                string Classification = list[2];//分類
                string Count = list[3];//數量
                DataTable ds = new DataTable();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (DataTableUtils.toString(dt.Columns[i]) != Classification)
                        ds.Columns.Add(DataTableUtils.toString(dt.Columns[i]));
                }


                //相同的做完處理的
                int a = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow row = ds.NewRow();
                    row[custom] = dt.Rows[i][custom];
                    row[item_no] = transportrack_name(DataTableUtils.toString(dt.Rows[i][item_no]));
                    if (i > 0 &&
                    DataTableUtils.toString(dt.Rows[i][Classification]) != DataTableUtils.toString(dt.Rows[i - 1][Classification]) &&
                    DataTableUtils.toString(dt.Rows[i][custom]) == DataTableUtils.toString(dt.Rows[i - 1][custom]) &&
                    DataTableUtils.toString(dt.Rows[i][item_no]) == DataTableUtils.toString(dt.Rows[i - 1][item_no]))
                    {
                        if (DataTableUtils.toString(dt.Rows[i][Classification]) == "3")
                            row[Count] = DataTableUtils.toInt(DataTableUtils.toString(dt.Rows[i][Count])) - DataTableUtils.toInt(DataTableUtils.toString(dt.Rows[i - 1][Count]));
                        else
                            row[Count] = DataTableUtils.toInt(DataTableUtils.toString(dt.Rows[i - 1][Count])) - DataTableUtils.toInt(DataTableUtils.toString(dt.Rows[i][Count]));
                        a--;
                        ds.Rows.RemoveAt(a);
                        ds.Rows.Add(row);
                        a++;
                    }
                    else
                    {
                        if (DataTableUtils.toString(dt.Rows[i][Classification]) == "3")
                            row[Count] = DataTableUtils.toInt(DataTableUtils.toString(dt.Rows[i][Count]));
                        else
                            row[Count] = 0 - DataTableUtils.toInt(DataTableUtils.toString(dt.Rows[i][Count]));
                        ds.Rows.Add(row);
                        a++;
                    }
                }


                return ds;
            }
            else
                return dt;
        }

        //運輸架名稱對應
        private static string transportrack_name(string item)
        {
            string sqlcmd = "select da02 from tdm01 where da01 = '" + item + "'";
            DataTable dt = iTechDB.Get_DataTable(sqlcmd);
            if (dt != null && dt.Rows.Count > 0)
                return item + " " + DataTableUtils.toString(dt.Rows[0]["da02"]);
            else
                return item;
        }
        //----------------------------------------------------運輸架區---------------------------------------------

        //----------------------------------------------------特殊運算用-------------------------------------------
        private static bool Return_ok(List<string> list, string keyword)
        {
            bool ok = false;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == keyword)
                    ok = true;
            }
            return ok;
        }
    }
}
