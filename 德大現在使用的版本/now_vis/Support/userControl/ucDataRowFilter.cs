using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Support.userControl
{
    public delegate void DataGridViewCellEvent(DataGridView dgv, DataGridViewCellEventArgs e);
    public delegate void DataGridViewCellMouseEvent(DataGridView dgv, DataGridViewCellMouseEventArgs e);
    public delegate void DataTableReadyEvent(DataTable table);
    public delegate void ToolStripItemEventArgs(EventArgs e);
#if DEBUG != true
    [LicenseProvider(typeof(dekLicenseProvider))]
#endif
    public partial class ucDataRowFilter : UserControl
    {
        public ucDataRowFilter()
        {
            InitializeComponent();
            LicenseManager.Validate(typeof(ucDataRowFilter), this);
            /*
            ContextMenu ctx_menu = new ContextMenu();
            ctx_menu.MenuItems.Add("資料控制板---顯示/隱藏", MenuItem_PanelFilterClick);
            dataGridView_Data.ContextMenu = ctx_menu;
            */
        }
        //===================================================================
        public DataGridViewCellEvent OnRowEnter = null;
        public DataGridViewCellMouseEvent Mouse_Double_Click = null;
        public ToolStripItemEventArgs ToolStripItem = null;

        public DataTableReadyEvent OnDataTableReady = null;
        public DataGridView dataGridView
        {
            get { return dataGridView_Data; }
        }
        public int SelectedRowIndex = -1;
        public DataRow SelectedRow = null;
        public Panel Ctrl_PanelFilter { get { return panel_filter; } }
        public GroupBox Ctrl_DatGirdView { get { return groupBox_dataGridView; } }

        public GroupBox Ctrl_FieldSelect { get { return groupBox_FieldSelect; } }
        public GroupBox Ctrl_SearchControl { get { return groupBox_Search; } }
        public Panel Ctrl_PageControl { get { return panel_PageControl; } }
        public string SQL_查詢字串
        {
            get { return textBox_查詢字串.Text; }
            set { textBox_查詢字串.Text = value; }
        }
        public decimal 每頁筆數
        {
            get { return numericUpDown_每頁筆數.Value; }
            set { numericUpDown_每頁筆數.Value = value; }
        }
        public Dictionary<string, Control> UserControlList
        {
            get { return control_list; }
            set { control_list = value; }
        }
        public bool Cell_DoubleClick_TrunOn = false;
        //--------------------------------
        private string table_name, sql_查詢條件;
        private Dictionary<string, Control> control_list = null;
        

        public void Init(string table,string sql_查詢條件,bool is_multi_select)
        {
            DataTable data_table = (DataTable)dataGridView_Data.DataSource;
            if (data_table != null) data_table.Dispose();
            dataGridView_Data.DataSource = null;
            dataGridView_Data.MultiSelect = is_multi_select;
            table_name = table;
            this.sql_查詢條件 = sql_查詢條件.Trim();
            checkedListBox_選取欄位.Items.Clear();
            comboBox_查詢欄位.Items.Clear();
            comboBox_排序欄位.Items.Clear();
            textBox_查詢字串.Text = "";
            data_table = DataTableUtils.DataTable_GetRowHeader(table);
            string name;
            comboBox_查詢欄位.Items.Add("");
            comboBox_排序欄位.Items.Add("");
            foreach (DataColumn col in data_table.Columns)
            {
                name = col.ColumnName;
                checkedListBox_選取欄位.Items.Add(name, false);
                comboBox_查詢欄位.Items.Add(name);
                comboBox_排序欄位.Items.Add(name);
            }
            data_table.Dispose();
            DataGridView_Reload(null, null);
        }

        public void cbx_reload(DataGridView dgv )
        {
            DataTable data_table = (DataTable)dgv.DataSource;
            //checkedListBox_選取欄位.Items.Clear();
            //comboBox_查詢欄位.Items.Clear();
            //comboBox_排序欄位.Items.Clear();

            string name;
            comboBox_查詢欄位.Items.Add("");
            comboBox_排序欄位.Items.Add("");
            foreach (DataColumn col in data_table.Columns)
            {
                name = col.ColumnName;
                checkedListBox_選取欄位.Items.Add(name, false);
                comboBox_查詢欄位.Items.Add(name);
                comboBox_排序欄位.Items.Add(name);
            }
        }

        private string SqlCmd_BuildCond()
        {
            string sql_cond = sql_查詢條件;
            string field_name = comboBox_查詢欄位.Text.Trim();
            string value = textBox_查詢字串.Text.Trim();
            if (field_name != "" && value != "")
            {
                value = string.Format(" {0} like '%{1}%'", field_name, value);
                if (sql_cond != "")
                    sql_cond += " and " + value;
                else sql_cond = value;
            }
            return sql_cond;
        }

        private string SqlCmd_Build()
        {
            string sqlcmd = string.Format("select * from {0}", table_name);
            string sql_cond = SqlCmd_BuildCond();
            if (sql_cond!="") sqlcmd += " where " + sql_cond;
            string field_name = comboBox_排序欄位.Text.Trim();
            if (field_name!="")
            {
                sqlcmd += " order by " + field_name;
                if (checkBox_遞減.Checked) sqlcmd += " DESC";
            }
            return sqlcmd;
        }

        private void DataGridView_SetInvisibleColumns(DataGridView dgv)
        {
            //Remove unnecessary columns
            if (checkedListBox_選取欄位.CheckedItems.Count <= 0) return;
            //找出有哪些欄位沒有被選到，找到將其隱藏
            for (int index = 0; index < dgv.Columns.Count; index++)
            {
                string name = dgv.Columns[index].Name;
                if (checkedListBox_選取欄位.Items.IndexOf(name) >= 0)
                {
                    dgv.Columns[index].Visible =
                        (checkedListBox_選取欄位.CheckedItems.IndexOf(name) >= 0);
                }
            }
        }

        private void DataTable_LoadPage()
        {
            int page_size = (int)numericUpDown_每頁筆數.Value;
            int page_no = (int) numericUpDown_目前頁面.Value; 
            string sqlcmd = SqlCmd_Build();
            DataTable data_table = (DataTable)dataGridView_Data.DataSource;
            if (data_table != null) data_table.Dispose();
            page_no = (page_no - 1) * page_size;
            data_table = DataTableUtils.DataTable_GetTable(sqlcmd, page_no, page_size);
            dataGridView_Data.DataSource = data_table;
            DataGridView_SetInvisibleColumns(dataGridView_Data);
            if (OnDataTableReady != null)
                OnDataTableReady(data_table);
        }
       
        public void DataGridView_Reload(object sender, EventArgs e)
        {
            string sql_cond = SqlCmd_BuildCond();
            int page_size = (int) numericUpDown_每頁筆數.Value;
            int record_count = DataTableUtils.RowCount(table_name, sql_cond);
            int no= (record_count + page_size - 1) / page_size;
            if (no < 1) no = 1;
            numericUpDown_目前頁面.Maximum = no;
            numericUpDown_目前頁面.Value = 1;
            label_總頁數.Text = string.Format("資料總數: {0}筆 / {1}頁", record_count, no);
            DataTable_LoadPage();
        }

        private void button_選取欄位_全選_Click(object sender, EventArgs e)
        {
            bool is_checked = true;
            if (sender == button_選取欄位_清除) is_checked = false;
            //else if (sender == button_選取欄位_全選) is_checked = true;
            for (int index = 0; index < checkedListBox_選取欄位.Items.Count; index++)
            {
                if (sender== button_選取欄位_反向)
                    is_checked = !checkedListBox_選取欄位.GetItemChecked(index);
                checkedListBox_選取欄位.SetItemChecked(index, is_checked);
            }
            DataGridView_Reload(sender, e);
        }

        //-------------------------------------------------------------------
        private void numericUpDown_目前頁面_ValueChanged(object sender, EventArgs e)
        {
            DataTable_LoadPage();
        }

        private void button_nav_home_Click(object sender, EventArgs e)
        {
            int page_no = (int)numericUpDown_目前頁面.Value;
            if (sender == button_nav_home) page_no = 1;
            else if (sender == button_nav_next) page_no++;
            else if (sender == button_nav_prev) page_no--;
            else if (sender == button_nav_end) page_no = (int) numericUpDown_目前頁面.Maximum;
            if (page_no < 1) page_no = 1;
            else if (page_no > (int)numericUpDown_目前頁面.Maximum)
                page_no = (int) numericUpDown_目前頁面.Maximum;
            numericUpDown_目前頁面.Value = page_no;
            DataTable_LoadPage();
        }

        private void ucDataRowFilter_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            contextMenuStrip_main.Show((Control) sender,e.Location);
        }

        private void 資料控制板顯示隱藏ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ctrl_PanelFilter.Visible = !Ctrl_PanelFilter.Visible;
            if (ToolStripItem == null) return;
            ToolStripItem(e);
        }

        private void 字型ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog font_dialog = new FontDialog();
            font_dialog.Font = this.Font;
            if (font_dialog.ShowDialog()==DialogResult.OK)
            {
                this.Font = font_dialog.Font;
            }
            font_dialog.Dispose();
        }

        private void dataGridView_Data_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //20180530 John
            if(Cell_DoubleClick_TrunOn == true && e.Button == MouseButtons.Left)
            {
                SelectedRowIndex = e.RowIndex;
                if (SelectedRowIndex >= 0)
                {
                    DataTable data_table = (DataTable)dataGridView.DataSource;
                    SelectedRow = data_table.Rows[SelectedRowIndex];
                    if (UserControlList != null && UserControlList.Count > 0)
                    {
                        DataTableUtils.DataRow_ToControl(data_table.Rows[e.RowIndex], UserControlList);
                    }
                    if (Mouse_Double_Click != null)
                        Mouse_Double_Click((DataGridView)sender, e);

                    this.ParentForm.DialogResult = DialogResult.OK;
                    this.ParentForm.Close();
                }
            }
        }

        private void dataGridView_Data_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView_Data_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            SelectedRowIndex = e.RowIndex;
            if (SelectedRowIndex >= 0)
            {
                DataTable data_table = (DataTable)dataGridView.DataSource;
                SelectedRow = data_table.Rows[SelectedRowIndex];
                if (UserControlList != null && UserControlList.Count > 0)
                {
                    DataTableUtils.DataRow_ToControl(data_table.Rows[e.RowIndex], UserControlList);
                }
                if (OnRowEnter != null)
                    OnRowEnter((DataGridView)sender, e);
            }
            else SelectedRow = null;
        }



        //--------------------------------------------------------------
    }
}
