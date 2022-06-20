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
#if DEBUG != true
    [LicenseProvider(typeof(dekLicenseProvider))]
#endif
    public partial class ucDataGridView : UserControl
    {
        Forms.FormDataFilter filter = null;
        public ucDataGridView()
        {
            InitializeComponent();
            LicenseManager.Validate(typeof(ucDataGridView), this);
            _Title = "資料內容";
        }

        public void Init(string 資料表名稱, string 欄位名稱列 = "*", string SQL查詢條件 = "")
        {
            _資料表名稱 = 資料表名稱.Trim();
            _欄位名稱列 = 欄位名稱列.Trim();
            if (_欄位名稱列 == "") _欄位名稱列 = "*";
            _SQL查詢條件 = SQL查詢條件.Trim();
            _目前頁數 = 0;
            _資料總筆數 = -1;
            LoadData();
            filter =
                new Forms.FormDataFilter(dataGridView_main, _資料表名稱, _欄位名稱列, _SQL查詢條件);
        }
        //==========================================================================
        public DataTableReadyEvent OnDataTableReady = null;
        public DataGridViewCellEvent OnRowEnter = null;
        public int SelectedRowIndex = -1;
        public DataRow SelectedRow = null;

        public DataGridView _dataGridView
        {
            get { return dataGridView_main; }
        }

        public ContextMenuStrip _contextMenuStrip { get { return contextMenuStrip_main; } }

        private Dictionary<string, Control> control_list = null;
        public Dictionary<string, Control> UserControlList
        {
            get { return control_list; }
            set { control_list = value; }
        }

        public string _Title { get; set; }
        public string _資料表名稱 { get; set; }
        public string _欄位名稱列;
        public string _SQL查詢條件 { get; set; }

        int record_count = 0;
        public string _額外過濾條件 = "";
        public string _排序條件 = "";
        public int _資料總筆數
        {
            get { return record_count; }
            set { record_count = value; }
        }

        public int _總頁數
        {
            get { return (_資料總筆數 + _每頁筆數 - 1) / _每頁筆數; }
        }

        int page_size = 10;
        public int _每頁筆數
        {
            get { return page_size; }
            set { page_size = (value < 1) ? 1 : value; }
        }
        int cur_page = 0;
        public int _目前頁數
        {
            get { return cur_page; }
            set
            {
                if (value < 0) value = 0;
                if (value >= _總頁數) value = _總頁數 - 1;
                cur_page = value;
            }
        }

        

        string getSQLCond()
        {
            string sqlcond = "";
            if (_SQL查詢條件 != "")
                sqlcond += _SQL查詢條件;
            if (_額外過濾條件 != "")
            {
                if (sqlcond != "")
                    sqlcond += " and ";
                sqlcond += _額外過濾條件;
            }
            return sqlcond;
        }
        string getSQLCommand()
        {
            string sqlcmd = string.Format("select {0} from {1}", _欄位名稱列, _資料表名稱);
            string sqlcond = getSQLCond();
            if (sqlcond != "") sqlcmd += " where " + sqlcond;
            if (_排序條件 != "") sqlcmd += " order by " + _排序條件;
            return sqlcmd;
        }

        public void LoadData()
        {
            DataTable dt = (DataTable)dataGridView_main.DataSource;
            if (dt != null) dt.Dispose();
            string sqlcmd = getSQLCommand();
            if (_資料總筆數 < 0)
            {
                _資料總筆數 = DataTableUtils.RowCount(_資料表名稱, getSQLCond());               
                if (_目前頁數 > _總頁數) _目前頁數 = _總頁數 - 1;
                else if (_目前頁數<0) _目前頁數 = 0;
            }
            dt = DataTableUtils.DataTable_GetTable(sqlcmd, _目前頁數 * _每頁筆數, _每頁筆數);
            dataGridView_main.DataSource = dt;
            string text = string.Format("{0} --- 資料總數: {1} 筆, 第 {2} / {3} 頁, 每頁 {4} 筆",
                _Title, _資料總筆數, _目前頁數+1, _總頁數, _每頁筆數);
            groupBox_main.Text = text;
            if (OnDataTableReady != null) OnDataTableReady(dt);
        }

        private void dataGridView_main_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            contextMenuStrip_main.Show((Control)sender, e.Location);
        }

        private void dataGridView_main_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            SelectedRowIndex = e.RowIndex;
            if (SelectedRowIndex >= 0)
            {
                DataTable data_table = (DataTable)dgv.DataSource;
                SelectedRow = data_table.Rows[SelectedRowIndex];
                if (UserControlList != null && UserControlList.Count > 0)
                {
                    DataTableUtils.DataRow_ToControl(data_table.Rows[e.RowIndex], UserControlList);
                }
                if (OnRowEnter != null) OnRowEnter(dgv, e);
            }
            else SelectedRow = null;
        }

        //--------------------------------------------------------------
        public void Page_Goto(int page_no)
        {
            _目前頁數 = page_no;
            LoadData();
        }

        public void Page_First()
        {
            Page_Goto(0);
        }
        public void Page_Last()
        {
            Page_Goto(_總頁數 - 1);
        }

        public void Page_Next()
        {
            Page_Goto(_目前頁數 + 1);
        }

        public void Page_Prev()
        {
            Page_Goto(_目前頁數 - 1);
        }       

        private void 資料控制板ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            filter.設定分頁(_資料總筆數, _每頁筆數, _目前頁數 + 1);
            if (filter.ShowDialog()==DialogResult.OK)
            {
                string text, value;
                _每頁筆數 = (int) filter._每頁筆數.Value;
                _目前頁數 = (int)filter._目前頁面.Value - 1;
                text = filter._排序欄位.Text.Trim();
                if (text != "" && filter._排序遞減)
                {
                    text += " desc";
                }
                _排序條件 = text;
                text = filter._查詢欄位.Text.Trim();
                if (text!="")
                {
                    value = filter._查詢字串.Trim();
                    if (value == "") text = "";
                    else text += string.Format(" like '%{0}%'", value);
                }
                _額外過濾條件 = text;
                _資料總筆數 = -1;
                LoadData();
                string name;
                for (int index = 0; index < filter._選取欄位.Items.Count; index++)
                {
                    name = filter._選取欄位.Items[index].ToString();
                    try
                    {
                        _dataGridView.Columns[name].Visible =
                            filter._選取欄位.GetItemChecked(index);
                    }
                    catch (Exception) { }
                }
            }
            //filter.Dispose();
        }

        private void 變更字型ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog font_dialog = new FontDialog();
            font_dialog.Font = this.Font;
            if (font_dialog.ShowDialog() == DialogResult.OK)
            {
                //this.Font = font_dialog.Font;
                dataGridView_main.DefaultCellStyle.Font = font_dialog.Font;
                dataGridView_main.ColumnHeadersDefaultCellStyle.Font = font_dialog.Font;
            }
            font_dialog.Dispose();
        }
        //-----------------------------------------------------------------

    }
}
