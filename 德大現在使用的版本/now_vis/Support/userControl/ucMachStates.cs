using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Support;

namespace Support.DashBoard
{
    public partial class ucMachStates : ucVisObj
    {
        public string machine_id;
        string ImgFilePath = @"D:\dekFile\Img\Mach_\";
        string UserName = "未登錄";
        string OrderSN = "未登錄";
        public int update_rate_ms = 10000;


        public ucMachStates(string mach_id)
        {
            InitializeComponent();
            machine_id = mach_id;
            this.BackColor = Color.Silver;
            this.Text = mach_id;
        }

        public void MoveTo(int x,int y,int w=0,int h=0)
        {
            this.Left = x;
            this.Top = y;
            if (w > 0) this.Width = w;
            if (h > 0) this.Height = h;
        }
        private void UpdateColor(Color back, Color val,Color rate)
        {
            this.BackColor = back;
            lbl_操作狀態.ForeColor = val;
            lbl_程式.ForeColor = val;
            lbl_生產進度.ForeColor = val;
            lbl_稼動率.ForeColor = rate;
            lbl_達成率.ForeColor = rate;
            lbl_機台不良率.ForeColor = rate;
        }
        public override void Refresh()
        {
            DataTableUtils.UseDB_Change(DB_NAME.DB_VisMach);
            try
            {
                string MachID = machine_id;
                string str;

                //查詢目前機台資料&作業狀態----------------------------------------------------------------------------------

                /*
                Mach_Info:
                    Mach_ID,    
                
                NC_Register:
                            dy_feedrate_override,dy_sp_override, Part_Total , Part_Required, Part_Count, 
                            Alarm_Count , Nc_Mode, Nc_Status, PowerON, Operate_time, Prog_mainName , 
                            Cycle_time, User_ID
                User_Info: 
                    User_ID,FAC_ID,User_Admin,User_Name,User_Password,
                */
                DataTable dt_mach_info = MachData.MachInfo_機台資料(machine_id);
                DataTable dt_mach_status = MachData.MachInfo_機台作業狀態(machine_id);

                string user_id = DataTableUtils.DataRow_ColumntoString(dt_mach_info, 0, "User_ID");
                DataTable dt_操作員 = MachData.GetTableRow("User_Info", "User_ID = '" + user_id + "'");
                UserName = DataTableUtils.DataRow_ColumntoString(dt_操作員, 0, "User_Name", "未登錄");
                /*
                 * User_WorkingLoginLog:  [UK_index],[User_ID],[Mach_ID],[Order_ID],[update_dt]
                 * MakeOrder:  [OrderID],[Make_DT],[Sales_Executive],[Make_SN],[Client_Order],[Client_Name]
                              ,[Client_Liaison],[Type_Of_Payment],[Order_Paid],[Make_Price],[Make_Quantity]
                              ,[Make_Complete_Count],[Make_Manufacture_dt],[Make_Complete_dt]
                              ,[Delivery_City],[Delivery_District],[Delivery_Address],[Make_State]
                              ,[Order_Description],[Make_CostPrice_other],[update_dt]
                 */

                string GetOrderSN = "SELECT a.UK_index,a.User_ID,a.Mach_ID,a.Order_ID,a.update_dt," +
                                    " b.Mach_Name,c.Make_SN FROM User_WorkingLoginLog a " +
                                    " inner join Mach_Info b on a.Mach_ID = b.Mach_ID inner join MakeOrder c" +
                                    " on a.Order_ID = c.OrderID where User_ID='" + user_id + "'";
                DataTable dt_製令 = DataTableUtils.DataTable_GetRow(GetOrderSN);

                OrderSN = DataTableUtils.DataRow_ColumntoString(dt_製令,0, "Make_SN", "未登錄");
                
      
     
                //---------------------------------------------------------------------------------------------------------

                //查詢目前設定的上下班時間-------------------------------------------------------------------------------------                
                //WR_Start:"0800", WR_End:"1700"
                DataTable dt = MachData.GetTableRow("Work_Range", "Work_Range_ID='1'");
                str = DateTime.Now.ToString("yyyyMMdd");
                string WR_Start = str + DataTableUtils.DataRow_ColumntoString(dt, 0, "WR_Start", "") + "00";
                string WR_End = DateTime.Now.ToString("yyyyMMddhhmmss");
                //---------------------------------------------------------------------------------------------------------

                //查詢今日上班時間內的總加工件數------------------------------------------------------------------------------
                string strCmd3 = String.Format("select a.Mach_ID,SUM(b.NC_Run_Sec) as RunTime , sum(b.NC_Idle_Sec) as IdleTime " +
                                "from Mach_Info a inner join NC_WorkProcess_Detail b on a.Mach_ID = b.Mach_ID " +
                                "where b.Start_DT between '{0}' and  '{1}' and End_DT != '' and a.Mach_ID = '{2}'" +
                                " GROUP BY a.Mach_ID order by a.Mach_ID", WR_Start, WR_End, MachID);
                DataTable dt3 = DataTableUtils.DataTable_GetTable(strCmd3);
                //----------------------------------------------------------------------------------------------------------

                //查詢目前所使用的加工程式和件數------------------------------------------------------------------------------
                //NC_Register:  Mach_ID,Nc_Status,Prog_mainName,Part_Count,Part_Required,update_dt
                DataTable dt4 = MachData.MachInfo_機台作業狀態(MachID);
                //---------------------------------------------------------------------------------------------------------

                //不良率------------------------------------------------------------------------------
                string strCmd5 = "SELECT a.Mach_ID,b.Mach_ShortName," +
                                "sum (Process_Complete_Count) as 良數 ," +
                                "sum(Bad_Count) as 不良數," +
                                "(cast((sum(Process_Complete_Count) - sum(Bad_Count)) as float) /" +
                                " NULLIF(sum(Process_Complete_Count), 0)) * 100 as 機台不良率_百分比" +
                                " FROM MakeOrder_Process_Detail a inner join Mach_Info b on a.Mach_ID = b.Mach_ID " +
                                " where a.Mach_ID = '" + MachID + "' GROUP BY a.Mach_ID,b.Mach_ShortName";
                
                DataTable dt_機台不良率 = DataTableUtils.DataTable_GetTable(strCmd5);
                DataTable dt_mach_produce = MachData.MachInfo_機台生產資料(machine_id);
                //------------------------------------------------------------------------------------

                //機台照片------------------------------------------------------------------------------
                str=DataTableUtils.DataRow_ColumntoString(dt_mach_info, 0, "Mach_Img", "");
                if (System.IO.Path.HasExtension(str) != true) str += ".jpg";
                pictureBox1.ImageLocation = ImgFilePath + str;
                //-------------------------------------------------------------------------------------

                //預定保養日-----------------------------------------------------------------------------
                lbl_預定保養日.Text = MachData.MachInfo_機台保養資料(MachID, "NextTime");
                //--------------------------------------------------------------------------------------
                string 稼動率=MachData.MachInfo_稼動率(MachID, WR_Start, WR_End);
                //-------------------------------------------------------------------------------------

                string NC_status = DataTableUtils.DataRow_ColumntoString(dt_mach_status, 0, "Nc_Status");
                switch (NC_status)
                {
                    case "0":
                        NC_status = "離線";
                        gr_機台使用狀況.ForeColor = Color.Black;
                        UpdateColor(Color.Silver, Color.Black, Color.Black);
                        break;
                    case "1":
                        NC_status = "運作";
                        UpdateColor(Color.WhiteSmoke, Color.ForestGreen, Color.SteelBlue);
                        break;
                    case "2":
                        NC_status = "警告";
                        Color back = this.BackColor;

                        if (back != Color.LightCoral)
                        {
                            back = Color.Tomato;
                        }
                        else if (back == Color.Tomato)
                        {
                            back = Color.LightCoral;
                        }
                        UpdateColor(back, Color.Black, Color.Black);
                        Debug.WriteLine(this.BackColor);

                        break;
                    case "3":
                        NC_status = "閒置";
                        this.BackColor = Color.Khaki;
                        break;
                }

                lbl_MachineName.Text = DataTableUtils.DataRow_ColumntoString(dt_mach_status, 0, "mach_name");
                lbl_操作狀態.Text = NC_status;

                if (NC_status != "離線")
                {
                    lbl_M_PowerT.Text = DataTableUtils.DataRow_ColumntoString(dt_mach_status, 0, "PowerON");
                    lbl_M_totalTime.Text = DataTableUtils.DataRow_ColumntoString(dt_mach_status, 0, "Operate_time");
                    lbl_程式.Text = DataTableUtils.DataRow_ColumntoString(dt_mach_status, 0, "Prog_mainName");
                    lbl_生產進度.Text = DataTableUtils.DataRow_ColumntoString(dt_mach_status, 0, "Part_Count") + 
                            "/" + DataTableUtils.DataRow_ColumntoString(dt_mach_status, 0, "Part_required");
                    lbl_稼動率.Text = 稼動率;
                    lbl_操作員.Text = UserName;
                    lbl_製令.Text = OrderSN;
                    string str_part_count = DataTableUtils.DataRow_ColumntoString(dt_mach_status, 0, "Part_Count");
                    string str_part_require= DataTableUtils.DataRow_ColumntoString(dt_mach_status, 0, "Part_required");
                    int ratio = DataTableUtils.toInt(str_part_count) * 100 / DataTableUtils.toInt(str_part_require, 1);
                    lbl_達成率.Text = ratio.ToString() + "%";
                    if (dt_機台不良率.Rows.Count != 0 && dt_機台不良率.Rows[0][4].ToString() != "")
                    {
                        lbl_機台不良率.Text = dt_機台不良率.Rows[0]["機台不良率_百分比"].ToString() + "%";
                    }
                }
            }
            catch { }
            DataTableUtils.UseDB_Restore();
            base.Refresh();            
        }
    }
}

