using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dek_erpvis_v2.cls
{
    enum Operator { 相加, 相減, 相乘, 相除 };
    enum WorkType { 人,物件};
    enum onTimeStatus { 超前, 落後 };
    public class ShareMemory
    {
        //===================SQL Table =====================
        //employees
        public static string SQLAsm_EmployeeData = "員工資料表";
        public static string SQLAsm_EmployeeWork = "員工工作紀錄資料表";
        //work class
        public static string SQLAsm_WorkClass = "工作班別資料表";
        //Work Craft
        public static string SQLAsm_WorkCraft_Name = "工藝名稱資料表";
        public static string SQLAsm_WorkCraft_Group = "工藝群組資料表";
        public static string SQLAsm_WorkCraft_Project = "工藝工程資料表";
        public static string SQLAsm_WorkCraft_Group_Mem = "工藝群組成員資料表";
        public static string SQLAsm_WorkCraft_Project_Mem = "工藝工程成員資料表";
        //Workstation
        public static string SQLAsm_WorkStation_Log = "工作站歷程資料表";
        public static string SQLAsm_WorkStation_Type = "工作站型態資料表";
        public static string SQLAsm_WorkStation_State = "工作站狀態資料表";
        public static string SQLAsm_WorkStation_Error = "工作站異常紀錄資料表";
        public static string SQLAsm_WorkStation_ErrorMant = "工作站異常維護資料表";
        public static string SQLAsm_WorkStation_ErrorProcessStatus = "工作站異常狀態資料表";
        public static string SQLAsm_WorkStation_Note = "工作站備註資料表";
        //Mix 
        public static string SQLEmp_Craft_Prj = "員工工藝工程資料表";
        public static string SQLEmp_Craft_Gup = "員工工藝群組資料表";
        public static string SQLEmp_Craft_Name = "員工工藝名稱資料表";
        //company
        public static string SQLAsm_CompanyInfo = "公司資料表";
        public static string SQLAsm_CompanyDepartment = "公司部門資料表";
        //other   Dispatched
        public static string SQLAsm_WorkFlow = "工藝流程資料表";
        public static string SQLAsm_WorkPoint = "工藝流程點資料表";
        public static string SQLAsm_RowsData = "組裝資料表";
        public static string SQLAsm_Relation = "關聯需求資料表";
        public static string SQLAsm_Dispatched = "排程派工資料表";
        public static string SQLAsm_TableRef = "資料表管理";
        public static string SQLAsm_FormData = "功能頁面管理資料表";
        public static string SQLAsm_Level = "權限資料表";
        public static string SQLAsm_MachineID_Line = "機器代號產線表";
        public static string SQLAsm_WorkStation_ErrorRingGroup = "工作站異常通知群組資料表";
        //
        public static string PrimaryKey = "排程編號";
        public static string WorkStationName = "工作站名稱";
        public static string WorkStationNum = "工作站編號";
        public static string Craft_Project_Key = "工程";
        public static string Craft_Project_SubKey = "群組";
        //dek_Aps                  
        public static string SQLWorkHour_Order = "workhour_order";
        public static string SQLWorkHour_Detail = "workhour_detail";
        public static string SQLWorkHour = "workhour";
        public static string SQLWorkHour_OrderStatus = "workhour_Orderstatus";
        public static string SQLWorkHour_MachStatus = "workhour_machstatus";
        //CNC_db
        public static string SQLMach_content = "Mach_content";
        public static string SQLMach_contentSelect = "mach_content_select";
        //Vis_DB
        public static string SQLVis_User = "users";
        //unit
        public static string PieceUnit = "(件)";
        public static string TimeUnit = "min";
        // set
        public static int ShowErrorCount = 3;
        public static int AdjustBase = 10;
        //
        public static DateTime StandTime = DateTime.ParseExact("10000101000000", "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
    }
}