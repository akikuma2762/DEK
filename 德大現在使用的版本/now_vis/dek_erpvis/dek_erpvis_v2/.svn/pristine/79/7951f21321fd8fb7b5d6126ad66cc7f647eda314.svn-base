using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Support;
using dek_erpvis_v2.cls;

namespace dek_erpvis_v2.pages.exp_page
{
    public partial class xy_test : System.Web.UI.Page
    {
        public string title = "";
        public string xString = "";
        public string yString = "";
        public string dt_st = "";
        public string dt_ed = "";
        public string count = "";
        public string unit = "";
        public string chartType = "";
        public string status = "";
        public string chartData = "";
        string sqlCmd = "";
        class FiledString
        {
            public string FILED_TableName = "CORDSUB";
            public string FILED_Pline = "PLINE_NO";
            public string FILED_PlineGroup = "PLINE_GROUP";
            public string FILED_Cust = "CUST_NO";
            public string FILED_CustNM2 = "CUSTNM2";

        }
        FiledString filedString = new FiledString();
        myclass myclass = new myclass();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                MainProcess();
            }
        }
        //Process-------------------------------
        private void MainProcess()
        {
            GetCondition();
            CreatingSQL();
            GetChartData();
        }
        private void GetCondition()
        {
            xString = dropdownlist_X.SelectedValue;
            yString = dropdownlist_y.SelectedValue;
            title = filedString.FILED_TableName + "." + yString + "(" + xString + ")";
            dt_st = textbox_dt1.Text;
            dt_ed = textbox_dt2.Text;
            status = DropDownList_orderStatus.SelectedValue;
            count = dropdownlist_count.SelectedValue;
            chartType = dropdownlist_chartType.SelectedValue;
            unit = "數量";
        }
        private void CreatingSQL()
        {
            if (xString == filedString.FILED_Pline)
            {
                string Columns = " item_22.PLINE_NO ";
                sqlCmd = SQLCMD("", Columns, "");
            }
            else if (xString == filedString.FILED_Cust)
            {
                string Columns = filedString.FILED_CustNM2;
                string Top = count;
                xString = Columns;
                sqlCmd = SQLCMD(Top, Columns, yString);
            }
        }
        private void GetChartData()
        {
            clsDB_Server clsDB_Server = new clsDB_Server(cls.myclass.GetConnByDetaSowon);
            DataTable dt = clsDB_Server.DataTable_GetTable(sqlCmd);
            if (xString == filedString.FILED_Pline)
            {
                dt = myclass.Add_LINE_GROUP(dt, filedString.FILED_PlineGroup, filedString.FILED_Pline).ToTable();
                xString = filedString.FILED_PlineGroup;
            }

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string xValue = row[xString].ToString();
                    string yValue = row[yString].ToString().Split('.')[0];
                    string label = xString;
                    string indexLabel = yValue;
                    chartData += ChartDataJson(yValue, xValue, indexLabel);
                }
            }
        }

        //Function------------------------------
        private string SQLCMD(string TOP, string Columns, string OrderBy)
        {
            string InA22_Fab = "";
            string GroupBy = Columns;
            string Scloce = SQLCMD_SCLOCE(ref InA22_Fab);
            if (TOP != "") TOP = " TOP ( " + TOP + " ) ";
            if (OrderBy != "") OrderBy = " order by " + OrderBy + " desc ";
            string sqlCmd_ = " SELECT                                                                              " +
                             " " + TOP + "                                                                         " +
                             " sum(QUANTITY) as QUANTITY,                                                          " +
                             " sum(AMOUNT) as AMOUNT,                                                              " +
                             " " + Columns + "                                                                     " +
                             " FROM CORDSUB                                                                        " +
                             " left join ITEM_22 on ITEM_22.ITEM_NO = CORDSUB.ITEM_NO                              " +
                             " left join CUST on  CUST.CUST_NO = CORDSUB.CUST_NO                                   " +
                             " where ITEM_22.PLINE_NO > 0 and D_DATE >= " + dt_st + " and D_DATE <= " + dt_ed + "  " +
                             " " + Scloce + "                                                                      " +
                             " " + InA22_Fab + "                                                                   " +
                             " group by " + GroupBy + "                                                            " +
                               OrderBy;
            return sqlCmd_;
        }
        private string SQLCMD_SCLOCE(ref string InA22_Fab)
        {
            string Closed    = " AND CORDSUB.SCLOSE !='未結' ";
            string NotClosed = " AND CORDSUB.SCLOSE ='未結'  ";
            string IN        = " IN                          ";
            string NotIN     = " NOT IN                      ";
            string Sclose    = "";
            switch (DropDownList_orderStatus.SelectedValue.ToString())
            {
                case "1":
                    //已結案訂單
                    Sclose = Closed;
                    break;
                case "2":
                    //未結案訂單
                    Sclose = NotClosed;
                    break;
                case "3":
                    //未結案訂單+已排程訂單
                    Sclose = NotClosed;
                    InA22_Fab = SQLCMD_A22FAB(IN);
                    break;
                case "4":
                    //未結案訂單+未排程訂單
                    Sclose = NotClosed;
                    InA22_Fab = SQLCMD_A22FAB(NotIN);
                    break;
            }
            return Sclose;
        }
        private string SQLCMD_A22FAB(string Condition)
        {
            return " AND (CORDSUB.TRN_NO + CORDSUB.SN)                                  " +
                   " " + Condition + "                                                  " +
                   " (SELECT A22_FAB.CORD_NO + A22_FAB.CORD_SN                          " +
                   " FROM A22_FAB AS A22_FAB                                            " +
                   " WHERE                                                              " +
                   " CORDSUB.TRN_NO = A22_FAB.CORD_NO AND CORDSUB.SN = A22_FAB.CORD_SN) ";
        }
        private string ChartDataJson(string yValue,string xValue,string indexLabel)
        {
            return "{ y: " + yValue + ", label: '" + xValue + "', indexLabel: '" + indexLabel + "' },";
        }
        //Event-------------------------------
        protected void Button_submit_Click(object sender, EventArgs e)
        {
            MainProcess();
        }
    }
}