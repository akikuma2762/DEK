using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace dekERP_dll
{
    //建立參數
    /// <summary>
    /// 0->全部  1->已完成  2->未完成 3->已排程 4->未排程 5->製令入庫 6->製令未入庫
    /// </summary>
    public enum OrderStatus { All = 0, Finished = 1, Unfinished = 2, Scheduled = 3, Unscheduled = 4, Stock = 5, Unstock = 6 };
    public enum dekModel { Image, Table };
    public enum OrderLineorCust { Line, Custom };
    public enum OrderType { 數量, 金額 };
    public enum TransportrackstatisticsImageType { Normal, Abnormal, All }
    public enum SupplierShortageType { 收貨單, 採購單 };

    //---------------------------------------------舊的方式------------------------------------------------------
    //業務部
    interface IdepSales
    {
        //訂單數量及金額統計 start(開始時間) end(結束時間) status(訂單狀態 0全部 1已結 2未結) amt_or_qty(數量/金額) img_or_tbl(圖/表) line_or_custom(產線/客戶) max_records(前N筆資料)
        DataTable Orders(string start, string end, OrderStatus status, dekModel img_or_tbl, OrderLineorCust line_or_custom, OrderType qty_or_amt, int max_records);
        DataTable Orders(DateTime start, DateTime end, OrderStatus status, dekModel img_or_tbl, OrderLineorCust line_or_custom, OrderType qty_or_amt, int max_records);

        //訂單逾期統計 start(開始時間) amt_or_qty(數量/金額) img_or_tbl(圖/表) line_or_custom(產線/客戶)
        DataTable Orders_Over(string start, dekModel img_or_tbl, OrderLineorCust line_or_custom, OrderType qty_or_amt);
        DataTable Orders_Over(DateTime start, dekModel img_or_tbl, OrderLineorCust line_or_custom, OrderType qty_or_amt);

        //訂單詳細表 start(開始時間) end(結束時間) custom(客戶名稱) status(訂單狀態 0全部 1已結案 2未結案)
        DataTable Orders_Details(string start, string end, string custom, OrderStatus status);
        DataTable Orders_Details(DateTime start, DateTime end, string custom, OrderStatus status);

        //逾期訂單詳細表 start(開始時間) custom(客戶名稱)
        DataTable Orders_Over_Details(string start, string custom);
        DataTable Orders_Over_Details(DateTime start, string custom);

        //出貨統計表 start(開始時間) end(結束時間) img_or_tbl(圖/表)
        DataTable Shipment(string start, string end, dekModel img_or_tbl);
        DataTable Shipment(DateTime start, DateTime end, dekModel img_or_tbl);

        //出貨詳細表 start(開始時間) end(結束時間) custom(客戶名稱)
        DataTable Shipment_Detail(string start, string end, string custom);
        DataTable Shipment_Detail(DateTime start, DateTime end, string custom);

        //出貨詳細表 小計 start(開始時間) end(結束時間) custom(客戶名稱) item(物品名稱)
        DataTable Get_Shipment(string start, string end, string custom, string item);
        DataTable Get_Shipment(DateTime start, DateTime end, string custom, string item);

        //運輸架未歸還統計 img_or_tbl(圖/表) type(正常/異常/全部) 
        DataTable Transportrackstatistics(dekModel img_or_tbl, TransportrackstatisticsImageType type, string condition);

        //未交易客戶 start(開始時間) end(結束時間) symbol(< = >) day(天數)
        DataTable UntradedCustomer(string start, string end, string symbol, int day);
        DataTable UntradedCustomer(DateTime start, DateTime end, string symbol, int day);

        DataTable TestLinkTable(string sqlcmd);
    }
    //資材部
    interface IdepMaterial
    {
        //物料領用統計表 start(開始時間) end(結束時間)
        DataTable materialrequirementplanning(string start, string end, string type, string item);
        DataTable materialrequirementplanning(DateTime start, DateTime end, string type, string item);

        //物料領用統計表詳細資訊 start(開始時間) end(結束時間) item(物料) img_or_tbl(圖/表)
        DataTable materialrequirementplanning_Detail(string item, string start, string end, dekModel img_or_tbl);
        DataTable materialrequirementplanning_Detail(string item, DateTime start, DateTime end, dekModel img_or_tbl);

        //供應商達交率 start(開始時間) end(結束時間)
        DataTable Supplierscore(string start, string end);
        DataTable Supplierscore(DateTime start, DateTime end);

        //供應商 start(開始時間) end(結束時間) custom(客戶名稱)
        DataTable Supplierscore_Detail(string start, string end, string custom);
        DataTable Supplierscore_Detail(DateTime start, DateTime end, string custom);

        //供應商催料總表 type(收貨單/採購單) supplier(供應商) supplierName(供應商簡稱)  start(開始時間) end(結束時間) itemNO(品號) Reminder_Date(收貨單→催料單號/採購單→計算後日期)
        DataTable SupplierShortage(SupplierShortageType type, string supplier, string supplierName, string start, string end, string itemNo, string Reminder_Date);

        //產生領料單表頭 Number(刀庫編號)
        DataTable pick_list_title(string Number);

        //產生領料單表格 Number(刀庫編號)
        DataTable pick_list_datatable(string Number);

        //取得CheckBoxList RadioButtonList DropDownList的內容
        DataTable Item_DataTable(string ini_Name, string start = "", string end = "");
    }
    //倉管部
    interface IdepHouse
    {
        //成品庫存分析 img_or_tbl(圖/表) start(開始時間) end(結束時間) day(逾期天數)
        DataTable stockanalysis(dekModel img_or_tbl, int day);

        //成品庫存分析細節  start(開始時間) end(結束時間) custom(客戶名稱)
        DataTable stockanalysis_Details(string start, string custom);
        DataTable stockanalysis_Details(DateTime start, string custom);

        //呆滯物料統計表  item_type(物料類別) day(期限日) warehouse(倉庫位置) item_num(物料編號)
        DataTable InactiveInventory(string item_type, string warehouse, string day);

        //報廢數量統計表 start(開始時間) end(結束時間) condition(報廢人員的SQL語法)
        DataTable Scrapped(string start, string end, string condition);
        DataTable Scrapped(DateTime start, DateTime end, string condition);

        //庫存明細列表 CBX(所選的倉別)
        DataTable Inventory_Total_Amount(CheckBoxList cbx);


    }
    //生產部
    interface IdepProduct
    {

    }
    //加工部
    interface IdenMachine
    {

    }


    //---------------------------------------------新的方式------------------------------------------------------
    //業務部
    interface Get_Sales
    {
        /// <summary>
        /// 訂單及數量統計明細
        /// </summary>
        /// <param name="start">開始時間</param>
        /// <param name="end">結束時間</param>
        /// <param name="status">訂單狀態(0全部 1已結 2未結 3已排程但未結 4未排程且未結)</param>
        /// <param name="source">來源( dek 德科,sowon 首旺,itec 鉅茂,....→讀取ini用)</param>
        /// <param name="detail">是否為明細 true=>抓入庫日 false=>不抓入庫日</param>
        /// <param name="custom">客戶名稱(可不填)</param>
        /// <returns></returns>
        DataTable Orders_Detail(DateTime start, DateTime end, OrderStatus status, string source, bool detail = false, string custom = "");
        DataTable Orders_Detail(string start, string end, OrderStatus status, string source, bool detail = false, string custom = "");

        /// <summary>
        /// 訂單逾期數量明細
        /// </summary>
        /// <param name="start">開始時間</param>
        /// <param name="source">來源( dek 德科,sowon 首旺,itec 鉅茂,....→讀取ini用)</param>
        /// <param name="detail">是否為明細 true=>抓入庫日 false=>不抓入庫日</param>
        /// <param name="custom">客戶名稱(可不填)</param>
        /// <returns></returns>
        DataTable Orders_Over_Detail(DateTime start, string source, bool detail = false, string custom = "");
        DataTable Orders_Over_Detail(string start, string source, bool detail = false, string custom = "");

        /// <summary>
        /// 出貨統計明細
        /// </summary>
        /// <param name="start">開始時間</param>
        /// <param name="end">結束時間</param>
        /// <param name="source">來源( dek 德科,sowon 首旺,itec 鉅茂,....→讀取ini用)</param>
        /// <param name="custom">客戶名稱(可不填)</param>
        /// <returns></returns>
        DataTable Shipment_Detail(DateTime start, DateTime end, string source, string custom = "");
        DataTable Shipment_Detail(string start, string end, string source, string custom = "");

        /// <summary>
        /// 出貨統計表小計
        /// </summary>
        /// <param name="start">開始時間</param>
        /// <param name="end">結束時間</param>
        /// <param name="custom">客戶名稱</param>
        /// <param name="item">料號</param>
        /// <param name="source">來源( dek 德科,sowon 首旺,itec 鉅茂,....→讀取ini用)</param>
        /// <returns></returns>
        DataTable Get_Shipment(DateTime start, DateTime end, string custom, string item, string source);
        DataTable Get_Shipment(string start, string end, string custom, string item, string source);

        /// <summary>
        /// 未交易客戶
        /// </summary>
        /// <param name="start">開始時間</param>
        /// <param name="end">結束時間</param>
        /// <param name="symbol">符號(><)</param>
        /// <param name="day">天數(ex：365)</param>
        /// <param name="source">來源( dek 德科,sowon 首旺,itec 鉅茂,....→讀取ini用)</param>
        /// <returns></returns>
        DataTable UntradedCustomer(DateTime start, DateTime end, string symbol, int day, string source);
        DataTable UntradedCustomer(string start, string end, string symbol, int day, string source);

        /// <summary>
        /// 運輸架未歸還統計
        /// </summary>
        /// <param name="source">來源( dek 德科,sowon 首旺,itec 鉅茂,....→讀取ini用)</param>
        /// <returns></returns>
        DataTable Transportrackstatistics(string acc, string source);

        /// <summary>
        /// 訂單變更紀錄
        /// </summary>
        /// <param name="start">開始時間</param>
        /// <param name="end">結束時間</param>
        /// <param name="source">來源( dek 德科,sowon 首旺,itec 鉅茂,....→讀取ini用)</param>
        /// <returns></returns>
        DataTable Recordsofchangetheorder_Details(DateTime start, DateTime end, string source);
        DataTable Recordsofchangetheorder_Details(string start, string end, string source);

        /// <summary>
        /// 取得該訂單之預計開工日
        /// </summary>
        /// <param name="start">開始時間</param>
        /// <param name="end">結束時間</param>
        /// <param name="source">來源( dek 德科,sowon 首旺,itec 鉅茂,....→讀取ini用)</param>
        /// <returns></returns>
        DataTable Orders_StartDay(DateTime start, DateTime end, string source);
        DataTable Orders_StartDay(string start, string end, string source);

        /// <summary>
        /// 每月各客戶訂單數量
        /// </summary>
        /// <param name="start">開始時間</param>
        /// <param name="end">結束時間</param>
        /// <param name="source">來源( dek 德科,sowon 首旺,itec 鉅茂,....→讀取ini用)</param>
        /// <param name="detail">是否為明細 true=>抓入庫日 false=>不抓入庫日</param>
        /// <returns></returns>
        DataTable Orders_Month(DateTime start, DateTime end, string source, bool detail = false);
        DataTable Orders_Month(string start, string end, string source, bool detail = false);

        /// <summary>
        /// 他月交期變更至本月
        /// </summary>
        /// <param name="start">起始時間</param>
        /// <param name="end">結束時間</param>
        /// <param name="source">來源</param>
        /// <returns></returns>
        DataTable Change_Date_To_Now(DateTime start, DateTime end, string source);
        DataTable Change_Date_To_Now(string start, string end, string source);

        /// <summary>
        /// 本月交期變更至他月
        /// </summary>
        /// <param name="start">起始時間</param>
        /// <param name="end">結束時間</param>
        /// <param name="source">來源</param>
        /// <returns></returns>
        DataTable Change_Date_To_Other(DateTime start, DateTime end, string source);
        DataTable Change_Date_To_Other(string start, string end, string source);

        /// <summary>
        /// 本月交期提前或是落後
        /// </summary>
        /// <param name="start">起始時間</param>
        /// <param name="end">結束時間</param>
        /// <param name="source">來源</param>
        /// <returns></returns>
        DataTable Change_Now_To_Now(DateTime start, DateTime end, string source);
        DataTable Change_Now_To_Now(string start, string end, string source);

    }

    //資材部
    interface Get_Material
    {
        /// <summary>
        /// 供應商達交率明細
        /// </summary>
        /// <param name="start">開始時間</param>
        /// <param name="end">結束時間</param>
        /// <param name="source">來源( dek 德科,sowon 首旺,itec 鉅茂,....→讀取ini用)</param>
        /// <param name="custom">客戶名稱(可不填)</param>
        /// <returns></returns>
        DataTable Supplierscore_Detail(DateTime start, DateTime end, string source, string custom = "");
        DataTable Supplierscore_Detail(string start, string end, string source, string custom = "");

        /// <summary>
        /// 取得最新催料單
        /// </summary>
        /// <param name="source">來源( dek 德科,sowon 首旺,itec 鉅茂,....→讀取ini用)</param> 
        /// <returns></returns>
        DataTable MaxOrder(string source);

        /// <summary>
        /// 取得催料日期
        /// </summary>
        /// <param name="source">來源( dek 德科,sowon 首旺,itec 鉅茂,....→讀取ini用)</param>
        /// <returns></returns>
        DataTable Shortage(string source);

        /// <summary>
        /// 取得最新催料單
        /// </summary>
        /// <param name="supplier">供應商代碼</param>
        /// <param name="supplierName">供應商簡稱</param>
        /// <param name="start">起始時間</param>
        /// <param name="end">結束時間</param>
        /// <param name="itemno">料號</param>
        /// <param name="maxorder">催料單號</param>
        /// <param name="source">來源( dek 德科,sowon 首旺,itec 鉅茂,....→讀取ini用)</param>  
        /// <returns></returns>
        DataTable SupplierShortage_Urge(string supplier, string supplierName, string start, string end, string itemno, string maxorder, string source);
        DataTable SupplierShortage_Urge(string supplier, string supplierName, DateTime start, DateTime end, string itemno, string maxorder, string source);

        /// <summary>
        /// 依據催料單取得相對應的採購/加工單
        /// </summary>
        /// <param name="condition">依據催料單找加工/採購單</param>
        /// <param name="source">來源( dek 德科,sowon 首旺,itec 鉅茂,....→讀取ini用)</param>     
        /// <returns></returns>
        DataTable SupplierShortage_Delivery(List<string> orders, string source);

        /// <summary>
        /// 物料領用統計表明細
        /// </summary>
        /// <param name="start">開始時間</param>
        /// <param name="end">結束時間</param>
        /// <param name="rbx">RadioButtonList的ID</param>
        /// <param name="item">輸入的名稱</param>
        /// <param name="source">來源( dek 德科,sowon 首旺,itec 鉅茂,....→讀取ini用)</param> 
        /// <returns></returns>
        DataTable materialrequirementplanning_Detail(DateTime start, DateTime end, RadioButtonList rbx, string item, string source);
        DataTable materialrequirementplanning_Detail(string start, string end, RadioButtonList rbx, string item, string source);

        /// <summary>
        /// 產生領料單表頭
        /// </summary>
        /// <param name="Number">刀庫編號 OR 刀庫編號#刀庫編號#(須轉List)</param>
        /// <param name="source">來源( dek 德科,sowon 首旺,itec 鉅茂,....→讀取ini用)</param> 
        /// <returns></returns>
        DataTable pick_list_title(string Number, string source);

        /// <summary>
        /// 產生領料單明細
        /// </summary>
        /// <param name="Number">刀庫編號 OR 刀庫編號#刀庫編號#(須轉List)</param>
        /// <param name="source">來源( dek 德科,sowon 首旺,itec 鉅茂,....→讀取ini用)</param> 
        /// <returns></returns>
        DataTable pick_list_datatable(string Number, string source);

        /// <summary>
        /// 取得CheckBoxList RadioButtonList DropDownList的內容
        /// </summary>
        /// <param name="ini_Name">ini的值</param>
        /// <param name="source">來源( dek 德科,sowon 首旺,itec 鉅茂,....→讀取ini用)</param>
        /// <param name="start">開始時間(可不填)</param>
        /// <param name="end">結束時間(可不填)</param>
        /// <returns></returns>
        DataTable Item_DataTable(string ini_Name, string source, string start = "", string end = "");
    }

    //倉管部
    interface Get_House
    {
        /// <summary>
        /// 成品庫存明細
        /// </summary>
        /// <param name="start">開始天數</param>
        /// <param name="source">來源( dek 德科,sowon 首旺,itec 鉅茂,....→讀取ini用)</param>
        /// <param name="custom">客戶名稱(可不填)</param>
        /// <returns></returns>
        DataTable stockanalysis_Details(string source, string custom = "");

        /// <summary>
        /// 庫存明細列表
        /// </summary>
        /// <param name="cbx">物料倉儲位置</param>
        /// <returns></returns>
        DataTable Inventory_Total_Amount(CheckBoxList warehouse, CheckBoxList type, string source);

        /// <summary>
        /// 呆滯物料統計表
        /// </summary>
        /// <param name="item_type">物料類別</param>
        /// <param name="warehouse">物料倉儲位置</param>
        /// <param name="day">小於某天</param>
        /// <param name="source">來源( dek 德科,sowon 首旺,itec 鉅茂,....→讀取ini用)</param>
        /// <returns></returns>
        DataTable InactiveInventory(CheckBoxList item_type, CheckBoxList warehouse, string day, string source);

        /// <summary>
        /// 報廢數量統計表
        /// </summary>
        /// <param name="start">開始時間</param>
        /// <param name="end">結束時間</param>
        /// <param name="condition">報廢人員語法( (... OR ....) )</param>
        /// <param name="source">來源( dek 德科,sowon 首旺,itec 鉅茂,....→讀取ini用)</param>
        /// <returns></returns>
        DataTable Scrapped(DateTime start, DateTime end, CheckBoxList Scrapped_personnel, string source);
        DataTable Scrapped(string start, string end, CheckBoxList Scrapped_personnel, string source);

    }

    //生產部
    interface Get_Product
    {
        /// <summary>
        /// 每日刀套生產數量表
        /// </summary>
        /// <param name="start">起始時間</param>
        /// <param name="end">結束時間</param>
        /// <param name="source">來源( dek 德科,sowon 首旺,itec 鉅茂,....→讀取ini用)</param>
        /// <returns></returns>
        DataTable KnifeSet(string start, string end, string source);
        DataTable KnifeSet(DateTime start, DateTime end, string source);


        /// <summary>
        /// 各機台欠料表
        /// </summary>
        /// <param name="start">起始時間</param>
        /// <param name="end">結束時間</param>
        /// <param name="source">來源( dek 德科,sowon 首旺,itec 鉅茂,....→讀取ini用)</param>
        /// <returns></returns>
        DataTable Lost_Material(string start, string end, string source);
    }
}
