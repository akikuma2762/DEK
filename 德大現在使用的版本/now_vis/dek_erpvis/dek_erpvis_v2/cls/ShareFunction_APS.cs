using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Support;
using System.Threading.Tasks;
using MongoDB.Driver;
using AjaxControlToolkit.HtmlEditor.Popups;

namespace dek_erpvis_v2.cls
{
    public enum WorkHourEditSource { 訂單 = 1, 報工明細 = 2 };
    public enum OrderStatus { OPEN = 1, CLOSE = 2, FAILURE = 3 };
    public enum MachineStatus { READY = 1, RUN = 2, FINISH = 3, MOLDING = 4, MOLDED = 5, ERROR = 6 };
    public class ShareFunction_APS
    {
        public static bool UpdataCNCVisData(string Order_Number, string Now_Task, string Mach_Name, WorkHourEditSource Source)
        {
            return false;
        }
        public static bool UpdataCNCVisStatus(string Order_Number, string Now_Task, string Mach_Status, WorkHourEditSource Source)
        {
            bool OK = false;
            string sqlcmd = "";
            string Condition = "";
            int NextTaskID = 0;
            DataTable dt_Order;
            DataTable dt_MachStatus;
            DataTable dt_CNC;
            DataRow dr_CNC;
            DataRow dr_Status;
            switch (Source)
            {
                case WorkHourEditSource.訂單:
                    sqlcmd = "SELECT * FROM " + ShareMemory.SQLWorkHour_Order + " where Project='" + Order_Number + "'";
                    dt_Order = DataTableUtils.GetDataTable(sqlcmd);
                    dt_CNC = DataTableUtils.GetDataTable(ShareMemory.SQLMach_content, "Order_Number = '" + Order_Number + "'");
                    sqlcmd = "SELECT workhour_orderstatus.Status as oStatus,workhour_orderstatus.Status_En,workhour_machstatus.Status as mStatus,workhour_machstatus.StatusEn FROM dek_aps.workhour_orderstatus  LEFT JOIN workhour_statusmapping ON workhour_orderstatus.id = workhour_statusmapping.ID_orderStatus LEFT JOIN workhour_machstatus ON workhour_machstatus.id = workhour_statusmapping.ID_MachStatus ";
                    dt_MachStatus = DataTableUtils.GetDataTable(sqlcmd);
                    if (dt_CNC != null)
                    {
                        foreach (DataRow dr_cnc in dt_CNC.Rows)
                        {
                            dr_cnc["Order_Status"] = dt_MachStatus.AsEnumerable().Where(w => w.Field<string>("oStatus") == Mach_Status).Select(s => s.Field<string>("Status_En")).FirstOrDefault().ToString();
                            dr_cnc["Mach_Status"] = dt_MachStatus.AsEnumerable().Where(s => s.Field<string>("oStatus") == Mach_Status).Select(o => o.Field<string>("StatusEn")).FirstOrDefault().ToString();
                            // QTY
                            dr_cnc["Now_QTY"] = CaculatorQTY(Order_Number).ToString();
                            // Updata
                            OK = DataTableUtils.Update_DataRow(ShareMemory.SQLMach_content, "Order_Number = '" + Order_Number + "'", dr_cnc);
                            System.Threading.Thread.Sleep(5);
                            // 
                            if (dr_cnc["Order_Status"].ToString() == OrderStatus.CLOSE.ToString() || dr_cnc["Order_Status"].ToString() == OrderStatus.FAILURE.ToString())
                                UpdataNextCNCProject(dr_cnc);
                        }
                    }
                    break;
                case WorkHourEditSource.報工明細:
                    Condition = "Status=" + "'" + Mach_Status + "'";
                    dr_Status = DataTableUtils.DataTable_GetDataRow(ShareMemory.SQLWorkHour_MachStatus, Condition);
                    if (dr_Status == null)
                    {
                        Condition = "StatusEn=" + "'" + Mach_Status + "'";
                        dr_Status = DataTableUtils.DataTable_GetDataRow(ShareMemory.SQLWorkHour_MachStatus, Condition);
                    }
                    Condition = "Order_Number=" + "'" + Order_Number + "'" + " AND " + "Now_Task=" + "'" + Now_Task + "'";
                    dr_CNC = DataTableUtils.DataTable_GetDataRow(ShareMemory.SQLMach_content, Condition);
                    if (dr_CNC != null)
                    {
                        //UPdata Now_QTY
                        dr_CNC["Now_QTY"] = CaculatorQTY(Order_Number, Now_Task).ToString();
                        dr_CNC["Mach_Status"] = dr_Status["StatusEn"].ToString();
                        Mach_Status = dr_Status["StatusEn"].ToString();
                        if (Mach_Status == MachineStatus.FINISH.ToString() || Mach_Status == MachineStatus.MOLDED.ToString())
                        {
                            dr_CNC["Order_Status"] = OrderStatus.CLOSE.ToString();
                            NextTaskID = UpdataNextCNCProject(dr_CNC);
                            ////Lock 要再判斷一層   如果下一個是  A段取  要連A加工族群一起鎖定
                            WorkHourLock(NextTaskID);
                        }
                        OK = DataTableUtils.Update_DataRow(ShareMemory.SQLMach_content, Condition, dr_CNC);
                    }
                    //Lock
                    //WorkHourLock(Order_Number, Now_Task);//緯凡規則-這一條根下一條都要鎖住(A段取報工...A加工也要lock)
                    break;
            }
            return OK;
        }
        public static void WorkHourLock(string Project, string TaskName, string JobID = "")
        {
            string Condition = "";
            DataRow dr_now;
            bool OK = false;
            Condition = "Project=" + "'" + Project + "'" + " AND " + "TaskName=" + "'" + TaskName + "'";
            if (!string.IsNullOrEmpty(JobID))
                Condition += " AND " + "JobID=" + "'" + JobID + "'";
            dr_now = DataTableUtils.DataTable_GetDataRow(ShareMemory.SQLWorkHour, Condition);
            // dr_now["Lock"] = "1";
            // bool OK = DataTableUtils.Update_DataRow(ShareMemory.SQLWorkHour, Condition, dr_now);
            // Next Lock 
            if (dr_now != null)
            {
                Condition = "Resource=" + "'" + dr_now["Resource"].ToString() + "'" + " AND " + "ID!=" + "'" + dr_now["ID"].ToString() + "'" + " AND " + "(" + "Status =" + "'" + MachineStatus.READY.ToString() + "'" + " OR " + " Status is null) " + " AND StartTime >=" + "'" + dr_now["StartTime"] + "'";
                DataTable dt_Work = DataTableUtils.GetDataTable("SELECT * FROM " + ShareMemory.SQLWorkHour + " Where " + Condition + "ORDER BY StartTime ASC");
                var NextProcess = dt_Work.AsEnumerable().FirstOrDefault();
                if (NextProcess != null)
                {
                    if (NextProcess["Locked"].ToString() != "1")
                    {
                        dr_now = NextProcess;
                        dr_now["Locked"] = "1";
                        Condition = "ID =" + "'" + dr_now["ID"].ToString() + "'";
                        OK = DataTableUtils.Update_DataRow(ShareMemory.SQLWorkHour, Condition, dr_now);
                    }
                    //
                    if (DataTableUtils.toDouble(NextProcess["Task"].ToString()) % ShareMemory.AdjustBase == 0)
                        WorkHourGroupLock(dt_Work, dr_now);
                }
            }
            //如果 Next Lock == 校模  下一個加工也要鎖住
        }
        public static void WorkHourLock(int NextTaskID)
        {
            string Condition = "";
            DataRow dr_now;
            bool OK = false;
            Condition = "ID=" + "'" + NextTaskID.ToString() + "'";
            dr_now = DataTableUtils.DataTable_GetDataRow(ShareMemory.SQLWorkHour, Condition);
            // Next Lock  && is 段取
            if (dr_now != null && DataTableUtils.toDouble(dr_now["Task"].ToString()) % ShareMemory.AdjustBase == 0)
            {
                // Next
                dr_now["Locked"] = "1";
                OK = DataTableUtils.Update_DataRow(ShareMemory.SQLWorkHour, Condition, dr_now);
                // Next Next 校模的話  要再綁一層
                Condition = "Resource=" + "'" + dr_now["Resource"].ToString() + "'" + " AND " + "ID!=" + "'" + dr_now["ID"].ToString() + "'" + " AND " + "(" + "Status =" + "'" + MachineStatus.READY.ToString() + "'" + " OR " + " Status is null) " + " AND StartTime >=" + "'" + dr_now["StartTime"] + "'";
                DataTable dt_Work = DataTableUtils.GetDataTable("SELECT * FROM " + ShareMemory.SQLWorkHour + " Where " + Condition + "ORDER BY StartTime ASC");
                WorkHourGroupLock(dt_Work, dr_now);
            }
        }
        public static void WorkHourGroupLock(DataTable _dt_Work, DataRow dr_now)
        {
            DataTable dt_lock = new DataTable();
            bool OK = false;
            string Condition = "";
            //將同一群組的都 lock  因為段取之後所有的加工都要lock
            var Lock = _dt_Work.AsEnumerable().Where(w => DataTableUtils.toInt(w.Field<int>("Task").ToString()) / DataTableUtils.toInt(dr_now["Task"].ToString()) == 1 && DataTableUtils.toInt(w.Field<int>("Task").ToString()) % DataTableUtils.toInt(dr_now["Task"].ToString()) != 0)
                                               .Where(w => w.Field<string>("Project") == dr_now["Project"].ToString());
            if (Lock != null && Lock.Count() != 0)
            {
                dt_lock = Lock.CopyToDataTable();
                foreach (DataRow dr in dt_lock.Rows)
                {
                    Condition = "ID =" + "'" + dr["ID"].ToString() + "'";
                    dr["Locked"] = "1";
                    OK = DataTableUtils.Update_DataRow(ShareMemory.SQLWorkHour, Condition, dr);
                }
            }
        }
        public static int CaculatorQTY(string OrderName, string TaskName = "")
        {
            DataTable dt_WorkHourDetailPiece;
            string Condition = "";
            if (string.IsNullOrEmpty(TaskName))
                Condition = "Project=" + "'" + OrderName;// + "'" + " AND " + "TaskName=" + "'" + Now_Task + "'";
            else
                Condition = "Project=" + "'" + OrderName + "'" + " AND " + "TaskName=" + "'" + TaskName + "'";
            dt_WorkHourDetailPiece = DataTableUtils.DataTable_GetTable(ShareMemory.SQLWorkHour_Detail, Condition);
            if (dt_WorkHourDetailPiece != null)
                return dt_WorkHourDetailPiece.AsEnumerable().Select(s => DataTableUtils.toInt(s.Field<string>("Piece"))).Sum();
            else
                return 0;
        }
        public static bool UpdataWorkHourData(string Project, string TaskName, WorkHourEditSource Source, string Status = "")
        {
            string sqlcmd = "";
            string Condition = "";
            DataTable dt_WorkHour;
            DataTable dt_MachStatus;
            DataTable dt_WorkHour_detail;
            DataRow dr_WorkHour;
            DataRow dr_Status;
            int Count = 0;
            //Get Status 
            bool OK = false;
            switch (Source)
            {
                case WorkHourEditSource.訂單:
                    sqlcmd = "SELECT workhour_orderstatus.Status as oStatus,workhour_orderstatus.Status_En,workhour_machstatus.Status as mStatus,workhour_machstatus.StatusEn FROM dek_aps.workhour_orderstatus  LEFT JOIN workhour_statusmapping ON workhour_orderstatus.id = workhour_statusmapping.ID_orderStatus LEFT JOIN workhour_machstatus ON workhour_machstatus.id = workhour_statusmapping.ID_MachStatus ";
                    dt_MachStatus = DataTableUtils.GetDataTable(sqlcmd);
                    Condition = "Project=" + "'" + Project + "'";// "Project=" + "'" + O_Order + "'" + " AND " + "TaskName=" + "'" + T_Order + "'";
                    dt_WorkHour = DataTableUtils.DataTable_GetTable(ShareMemory.SQLWorkHour, Condition);
                    if (dt_WorkHour != null)
                    {
                        foreach (DataRow dr in dt_WorkHour.Rows)
                        {
                            var status = dt_MachStatus.AsEnumerable().Where(s => s.Field<string>("oStatus").Trim() == Status).Select(o => o.Field<string>("StatusEn")).FirstOrDefault();
                            if (status != null)
                            {
                                dr["Status"] = status.ToString();
                                if (status.ToUpper() == MachineStatus.FINISH.ToString())
                                    dr["Locked"] = "1";
                                else if (status.ToUpper() == MachineStatus.ERROR.ToString())
                                    dr["Locked"] = "2";
                                else
                                {
                                }
                                OK = DataTableUtils.Update_DataRow(ShareMemory.SQLWorkHour, "id=" + "'" + dr["id"].ToString() + "'", dr);
                                System.Threading.Thread.Sleep(5);
                            }
                            else
                                continue;
                        }
                    }
                    break;
                case WorkHourEditSource.報工明細:
                    Condition = "Status=" + "'" + Status + "'";
                    dr_Status = DataTableUtils.DataTable_GetDataRow(ShareMemory.SQLWorkHour_MachStatus, Condition);
                    if (dr_Status == null)
                    {
                        Condition = "StatusEn=" + "'" + Status + "'";
                        dr_Status = DataTableUtils.DataTable_GetDataRow(ShareMemory.SQLWorkHour_MachStatus, Condition);
                    }


                    Condition = "Project=" + "'" + Project + "'";// "Project=" + "'" + O_Order + "'" + " AND " + "TaskName=" + "'" + T_Order + "'";
                    dt_WorkHour = DataTableUtils.DataTable_GetTable(ShareMemory.SQLWorkHour, Condition);
                    if (dt_WorkHour != null)
                    {

                        //本身自己的狀態 -> 更新
                        //Status
                        dr_WorkHour = dt_WorkHour.AsEnumerable().Where(s => s.Field<string>("TaskName") == TaskName).FirstOrDefault();// DataTableUtils.DataTable_GetDataRow(ShareMemory.SQLWorkHour, Condition);
                        dr_WorkHour["Status"] = dr_Status["StatusEn"];// dr_Status["Status"].ToString();
                        //Count
                        Condition = "Project=" + "'" + Project + "'" + " AND " + "TaskName=" + "'" + TaskName + "'";
                        dt_WorkHour_detail = DataTableUtils.DataTable_GetTable(ShareMemory.SQLWorkHour_Detail, Condition);
                        if (dt_WorkHour_detail != null)
                        {
                            Count = (from s in dt_WorkHour_detail.AsEnumerable()
                                     select new
                                     {
                                         piece = DataTableUtils.toInt(s.Field<string>("Piece"))
                                     }).Select(w => w.piece).Sum();
                        }
                        dr_WorkHour["CurrentPiece"] = Count.ToString();
                        dr_WorkHour["Locked"] = "1";
                        //
                        Condition = "ID=" + "'" + dr_WorkHour["id"].ToString() + "'";
                        OK = DataTableUtils.Update_DataRow(ShareMemory.SQLWorkHour, Condition, dr_WorkHour);
                        //如果是Task 校模  加工也要一起lock 
                        if (DataTableUtils.toInt(dr_WorkHour["Task"].ToString()) % 10 == 0)
                        {
                            //同一類組  1x 系列 都要鎖定 A加工 A1加工 A2加工.....
                            var CombinLock = dt_WorkHour.AsEnumerable()
                                .Where(w => DataTableUtils.toInt(w.Field<string>("Task")) / DataTableUtils.toInt(dr_WorkHour["Task"].ToString()) == 1 &&
                                   DataTableUtils.toInt(w.Field<string>("Task")) % DataTableUtils.toInt(dr_WorkHour["Task"].ToString()) != 0);
                            if (CombinLock != null)
                            {
                                foreach (DataRow dr in CombinLock.CopyToDataTable().Rows)
                                {
                                    dr["Locked"] = "1";
                                    Condition = "ID=" + "'" + dr["id"].ToString() + "'";
                                    OK = DataTableUtils.Update_DataRow(ShareMemory.SQLWorkHour, Condition, dr);
                                }
                            }
                        }
                    }
                    break;
            }
            return OK;
        }
        public static string CombinConditionStr(DataTable dt_condition, string ColumnsName)
        {
            string Condition = "";
            if (dt_condition != null)
            {
                var Oredr = dt_condition.AsEnumerable().GroupBy(s => s.Field<string>(ColumnsName)).Select(o => o.Key).ToList();
                foreach (string order_item in Oredr)
                {
                    if (order_item != Oredr.Last())
                        Condition += ColumnsName + "= " + "'" + order_item + "'" + " OR ";
                    else
                        Condition += ColumnsName + "= " + "'" + order_item + "'";
                }
            }
            return Condition;
        }
        public static bool UpdataWorkHourOrderData(string Project, string TaskName, WorkHourEditSource Source, string Status = "")
        {
            bool OK = false;
            string Condition = "";
            DataRow dr_WorkOrder;
            DataRow dr_Status;
            DataTable dt_WorkHour;
            switch (Source)
            {
                case WorkHourEditSource.訂單:

                    break;
                case WorkHourEditSource.報工明細:
                    Condition = "Status=" + "'" + Status + "'";
                    dr_Status = DataTableUtils.DataTable_GetDataRow(ShareMemory.SQLWorkHour_MachStatus, Condition);
                    if (dr_Status == null)
                    {
                        Condition = "StatusEn=" + "'" + Status + "'";
                        dr_Status = DataTableUtils.DataTable_GetDataRow(ShareMemory.SQLWorkHour_MachStatus, Condition);
                    }
                    Condition = "Project=" + "'" + Project + "'";// "Project=" + "'" + O_Order + "'" + " AND " + "TaskName=" + "'" + T_Order + "'";
                    dt_WorkHour = DataTableUtils.DataTable_GetTable(ShareMemory.SQLWorkHour, Condition);
                    if (dr_Status["StatusEn"].ToString() == MachineStatus.FINISH.ToString() && dt_WorkHour.AsEnumerable().Where(s => s.Field<string>("TaskName") != TaskName && s.Field<string>("status") != MachineStatus.FINISH.ToString() && s.Field<string>("status") != MachineStatus.MOLDED.ToString()).Count() == 0)
                    {
                        Condition = "Project=" + "'" + Project + "'";
                        dr_WorkOrder = DataTableUtils.DataTable_GetDataRow(ShareMemory.SQLWorkHour_Order, Condition);
                        dr_WorkOrder["Status"] = OrderStatus.CLOSE.ToString();
                        OK = DataTableUtils.Update_DataRow(ShareMemory.SQLWorkHour_Order, Condition, dr_WorkOrder);
                    }
                    break;
            }
            return false;
        }
        public static bool UpdataWorkHourDetailData(string Project, string TaskName, WorkHourEditSource Source, string Status = "")
        {
            return false;
        }
        public static bool UpdataWorkHourDetailStatus(string Project, string TaskName, WorkHourEditSource Source, string Status = "")
        {
            bool OK = false;
            string Condition = "";
            DataTable dt_WorkDetail;
            switch (Source)
            {
                case WorkHourEditSource.訂單:
                    Condition = "Project=" + "'" + Project + "'";
                    dt_WorkDetail = DataTableUtils.DataTable_GetTable(ShareMemory.SQLWorkHour_Detail, Condition);

                    foreach (DataRow dr in dt_WorkDetail.Rows)
                    {
                        if (Status == OrderStatus.CLOSE.ToString() || Status == OrderStatus.FAILURE.ToString())
                            dr["Status"] = Status == OrderStatus.CLOSE.ToString() ? MachineStatus.FINISH.ToString() : OrderStatus.FAILURE.ToString();
                        else
                            dr["Status"] = MachineStatus.READY.ToString();
                        OK = DataTableUtils.Update_DataRow(ShareMemory.SQLWorkHour_Detail, "id=" + "'" + dr["id"].ToString() + "'", dr);
                        System.Threading.Thread.Sleep(5);
                    }

                    break;
                case WorkHourEditSource.報工明細:

                    break;
            }
            return false;
        }
        public static int UpdataNextCNCProject(DataRow dr_Machine_Now)
        {
            string Condition = "";
            bool OK = false;
            //string sqlcmd = "SELECT workhour_orderstatus.Status as oStatus,workhour_orderstatus.Status_En,workhour_machstatus.Status as mStatus,workhour_machstatus.StatusEn FROM dek_aps.workhour_orderstatus  LEFT JOIN workhour_statusmapping ON workhour_orderstatus.id = workhour_statusmapping.ID_orderStatus LEFT JOIN workhour_machstatus ON workhour_machstatus.id = workhour_statusmapping.ID_MachStatus ";
            Condition = "Resource=" + "'" + dr_Machine_Now["Mach_Name"] + "'" +
                            " AND " + "(" + "Status =" + "'" + MachineStatus.READY.ToString() + "'" + " OR " + " Status is null" + ")" +
                            " AND StartTime >=" + "'" + dr_Machine_Now["Predict_End"] + "'";
            DataTable dr_Work = DataTableUtils.GetDataTable("SELECT * FROM " + ShareMemory.SQLWorkHour + " Where " + Condition + "ORDER BY StartTime ASC");
            var NextProcess = dr_Work.AsEnumerable().FirstOrDefault();
            //DataTable dt_CNC = DataTableUtils.DataTable_GetRowHeader(ShareMemory.SQLMach_content);
            if (NextProcess != null)
            {
                if (!string.IsNullOrEmpty(NextProcess["Status"].ToString()))
                    Condition = "StatusEn=" + "'" + NextProcess["Status"] + "'";
                else
                    Condition = "StatusEn=" + "'" + MachineStatus.READY.ToString() + "'";
                DataRow dr_MachStatus = DataTableUtils.DataTable_GetDataRow(ShareMemory.SQLWorkHour_MachStatus, Condition);
                Condition = "mach_Name=" + "'" + dr_Machine_Now["Mach_Name"] + "'";
                DataRow dr_CNC = DataTableUtils.DataTable_GetDataRow(ShareMemory.SQLMach_content, Condition);
                dr_CNC["Mach_Status"] = dr_MachStatus["StatusEn"].ToString();
                dr_CNC["Now_Task"] = NextProcess["TaskName"].ToString();
                dr_CNC["Predict_Start"] = NextProcess["StartTime"].ToString();
                dr_CNC["Predict_End"] = NextProcess["EndTime"].ToString();
                dr_CNC["Product_Number"] = NextProcess["JobID"].ToString();
                dr_CNC["Now_QTY"] = 0;
                dr_CNC["Target_QTY"] = NextProcess["TargetPiece"].ToString();
                dr_CNC["Percent"] = "0%";
                dr_CNC["Order_Number"] = NextProcess["Project"].ToString();
                dr_CNC["Product_Name"] = NextProcess["Job"].ToString();
                dr_CNC["Fix"] = NextProcess["Fix"].ToString();
                dr_CNC["Order_Status"] = OrderStatus.OPEN.ToString();
                //
                var Next2Process = dr_Work.AsEnumerable().Where(w => w.Field<int>("id").ToString() != NextProcess["id"].ToString()).FirstOrDefault();
                if (Next2Process != null)
                    dr_CNC["Next_Task"] = Next2Process["id"].ToString();
                else
                    dr_CNC["Next_Task"] = "";

                OK = DataTableUtils.Update_DataRow(ShareMemory.SQLMach_content, Condition, dr_CNC);
                if (Next2Process != null)
                    return DataTableUtils.toInt(Next2Process["id"].ToString());
            }
            return 0;
        }
        public static string GetDayWorkListTotoolTip(DataRow dr_MachNow, string[] ArraycolumnsName, ref string columnsName)
        {
            DataTable dt = new DataTable();
            DataTable dt_Job = new DataTable();
            string Condition = "";
            string Tooltip = "";
            if (string.IsNullOrEmpty(columnsName))
            {
                foreach (string col in ArraycolumnsName)
                {
                    if (ArraycolumnsName.Last() != col)
                        columnsName += col + ",";
                    else
                        columnsName += col;
                }
            }
            //Condition = " select " + columnsName + " From " + ShareMemory.SQLWorkHour + " Where " + "StartTime " + " > " + dr_MachNow["Predict_End"].ToString() + " AND " +
            //            " Resource=" + "'" + dr_MachNow["Mach_Name"].ToString() + "'" + " AND " +
            //            " Locked is null " + "order by StartTime"+" limit 10 ";
            Condition = " select " + columnsName + " From " + ShareMemory.SQLWorkHour + " Where " + "StartTime " + " > " + dr_MachNow["Predict_End"].ToString() + " AND " +
           " Resource=" + "'" + dr_MachNow["Mach_Name"].ToString() + "'" +
             " order by StartTime" + " limit 10 ";
            dt = DataTableUtils.DataTable_GetTable(Condition);
            //
            if (dt != null && dt.Rows.Count != 0)
            {
                //foreach (DataRow dt_t in dt.Rows)
                //{
                var job = dt.AsEnumerable().Where(w => DataTableUtils.toInt(w.Field<string>("Task")) % ShareMemory.AdjustBase != 0).Select(s => s);
                if (job != null && job.Count() != 0)
                {
                    dt_Job = job.CopyToDataTable();
                    for (int i = 0; i < dt_Job.Rows.Count; i++)
                    {
                        if (dt.Rows.IndexOf(dt_Job.Rows[i]) != dt_Job.Rows.Count - 1)
                            Tooltip += (i + 1).ToString().PadLeft(2, '0') + ".  " + " <font color =\"#ff0050\"> " + dt_Job.Rows[i]["Job"].ToString() + " </font> " + "  " + dt_Job.Rows[i]["TaskName"].ToString() + "  " + " <br> " + " <br> ";
                        else
                            Tooltip += (i + 1).ToString().PadLeft(2, '0') + ".  " + " <font color =\"#ff0050\"> " + dt_Job.Rows[i]["Job"].ToString() + " </font> " + "  " + dt_Job.Rows[i]["TaskName"].ToString() + "  " + " <br> ";
                        //if (dt.Rows.IndexOf(dt.Rows[i]) != dt.Rows.Count - 1)
                        //    Tooltip += (i + 1).ToString().PadLeft(2, '0') + ".  " + ShareFunction.StrToDate(dt.Rows[i]["StartTime"].ToString()).ToString("yyyy/MM/dd HH:mm:ss") + " --> " + " <font color =\"#ff0050\"> " + dt.Rows[i]["Job"].ToString() + " </font> "+ "  "  + dt.Rows[i]["TaskName"].ToString() + "  " + " <br> " + " <br> ";
                        //else
                        //    Tooltip += (i + 1).ToString().PadLeft(2, '0') + ".  " + ShareFunction.StrToDate(dt.Rows[i]["StartTime"].ToString()).ToString("yyyy/MM/dd HH:mm:ss") + " --> " + " <font color =\"#ff0050\"> " + dt.Rows[i]["Job"].ToString() + " </font> " + "  " + dt.Rows[i]["TaskName"].ToString() + "  " + " <br> ";
                    }
                }
                //}
            }
            else
            {
                Tooltip = "無工藝!";
            }
            Tooltip = Tooltip.Replace('"', '^');
            Tooltip = Tooltip.Replace("^", "*");
            return Tooltip;
        }
        //APS專用
        public static string check_right(string acc)
        {
            string ss = myclass.GetConnByDekVisErp;
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisErp);
            string sqlcmd = "SELECT * FROM vis.users where USER_ACC = '" + acc + "'";
            DataTable dt = DataTableUtils.GetDataTable(sqlcmd);

            if (DataTableUtils.toString(dt.Rows[0]["ADM"]) == "Y" || DataTableUtils.toString(dt.Rows[0]["WorkHour"]) == "Y")
                return "";
            else
                return "disabled=\"disabled\"";
        }
        //取得帳戶的名稱
        public static string GetAccName(string acc)
        {
            DataTableUtils.Conn_String = myclass.GetConnByDekVisErp;
            DataRow dr = DataTableUtils.DataTable_GetDataRow(ShareMemory.SQLVis_User, "USER_ACC=" + "'" + acc + "'");
            DataTableUtils.Conn_String = myclass.GetConnByDekVisErp;
            if (dr != null)
                return dr["USER_NAME"].ToString();
            else
                return "";
        }
        //
        public static string Calculate_QTY(string _Project, string _TaskName, string QTY, int id)
        {
            if (QTY == "")
                QTY = "0";
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisAps);
            DataTable dt_Prj = DataTableUtils.DataTable_GetTable(ShareMemory.SQLWorkHour, "Project=" + "'" + _Project + "'");
            DataTable dt_detail = DataTableUtils.DataTable_GetTable(ShareMemory.SQLWorkHour_Detail, "Project=" + "'" + _Project + "'");
            var Task = dt_Prj.AsEnumerable().Where(w => w.Field<string>("TaskName") == _TaskName).Select(s => s.Field<string>("Task")).FirstOrDefault();
            int BaseTask = DataTableUtils.toInt(Task.ToString()) / 10;//1// dt_Prj.AsEnumerable().Where(w => w.Field<int>("Task") % Task == 1).Select(s => s.Field<string>("TaskName"));
            var TargetPiece = dt_Prj.AsEnumerable().Select(s => DataTableUtils.toInt(s.Field<string>("TargetPiece"))).Max();
            //int sum = 0;
            if (BaseTask != 0)
            {
                int Sum = (from c in dt_detail.AsEnumerable()
                           from a in dt_Prj.AsEnumerable()
                           where a.Field<string>("TaskName") == c.Field<string>("TaskName") && DataTableUtils.toInt(a.Field<string>("Task")) / (BaseTask * 10) == 1
                           select new
                           {
                               piece = DataTableUtils.toInt(c.Field<string>("Piece"))
                           }).Select(w => w.piece).Sum();
                int OriSum = (from c in dt_detail.AsEnumerable()
                              from a in dt_Prj.AsEnumerable()
                              where a.Field<string>("TaskName") == c.Field<string>("TaskName") && DataTableUtils.toInt(a.Field<string>("Task")) / (BaseTask * 10) == 1
                              where c.Field<string>("TaskName") != _TaskName
                              select new
                              {
                                  piece = DataTableUtils.toInt(c.Field<string>("Piece"))
                              }).Select(w => w.piece).Sum();
                Sum = id == 1 ? OriSum : Sum;
                if (Sum + DataTableUtils.toInt(QTY) > TargetPiece && TargetPiece != 0)
                    return (Sum + DataTableUtils.toInt(QTY) - TargetPiece).ToString();
                else
                    return "";
            }
            else
                return "";
        }
    }
}