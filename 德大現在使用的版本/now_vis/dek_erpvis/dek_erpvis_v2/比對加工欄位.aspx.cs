using dek_erpvis_v2.cls;
using Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace dek_erpvis_v2
{
    public partial class WebForm3 : System.Web.UI.Page
    {
        List<string> fuck = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            fuck.Add("account_info");
            fuck.Add("alarm_currently_info");
            fuck.Add("alarm_history_info");
            fuck.Add("aps_info");
            fuck.Add("aps_info_dispatch");
            fuck.Add("aps_info_report");
            fuck.Add("aps_list_info");
            fuck.Add("authority_group_info");
            fuck.Add("authority_info");
            fuck.Add("craft_head");
            fuck.Add("craft_info");
            fuck.Add("custom_info");
            fuck.Add("error_report");
            fuck.Add("excel_head_list_info");
            fuck.Add("line_info");
            fuck.Add("mach_group");
            fuck.Add("mach_type");
            fuck.Add("mach_type_group");
            fuck.Add("machine_info");
            fuck.Add("model_info");
            fuck.Add("override_history_info");
            fuck.Add("produce_info");
            fuck.Add("product_currently_info");
            fuck.Add("product_head");
            fuck.Add("product_history_info");
            fuck.Add("product_info");
            fuck.Add("program_currently_info");
            fuck.Add("program_history_info");
            fuck.Add("realtime_info");
            fuck.Add("record_worktime");
            fuck.Add("show_column");
            fuck.Add("staff_info");
            fuck.Add("status_change");
            fuck.Add("status_currently_info");
            fuck.Add("status_history_info");
            fuck.Add("status_realtime_info");
            fuck.Add("status_type");
            fuck.Add("time_currently_info");
            fuck.Add("work_time_info");

            for (int i = 0; i < fuck.Count; i++)
                do_love(fuck[i]);
        }

        private void do_love(string ffcuk)
        {
            GlobalVar.UseDB_setConnString(myclass.GetConnByDekVisCnc_inside);
            string sqlcmd = $"select * from {ffcuk} limit 10";
            DataTable dt_original = DataTableUtils.GetDataTable(sqlcmd);

            List<string> list_original = new List<string>();
            if (dt_original != null)
                for (int i = 0; i < dt_original.Columns.Count; i++)
                    list_original.Add(dt_original.Columns[i].ToString());


            GlobalVar.UseDB_setConnString(clsDB_Server.GetConntionString_MySQL("192.168.80.128", "cnc_db", "root", "12345678"));
            DataTable dt_new = DataTableUtils.GetDataTable(sqlcmd);

            List<string> list_new = new List<string>();
            if (dt_new != null)
                for (int i = 0; i < dt_new.Columns.Count; i++)
                    list_new.Add(dt_new.Columns[i].ToString());

            List<string> no_get = new List<string>();

            for (int i = 0; i < list_original.Count; i++)
            {
                if (list_new.IndexOf(list_original[i]) == -1)
                    no_get.Add(list_original[i]);
            }



            if (no_get.Count != 0)
            {
                Label_NoUpdate.Text +=  $"資料表名稱：{ffcuk} \n <br /> "; 

                for (int i = 0; i < no_get.Count; i++)
                    Label_NoUpdate.Text += no_get[i] + " \n <br /> ";
                Label_NoUpdate.Text +=  "------------------------------------------- \n <br /> "; ;
            }

        }
    }
}