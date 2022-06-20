using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Support.Forms
{
#if DEBUG != true
    [LicenseProvider(typeof(dekLicenseProvider))]
#endif
    public partial class FormDataFilter : Form
    {
        public FormDataFilter(DataGridView dgv, string 資料表名稱, string 欄位名稱列, string SQL查詢條件)
        {
            InitializeComponent();
            LicenseManager.Validate(typeof(FormAudioWaveIO), this);
            _datagridview = dgv;
            _資料表名稱 = 資料表名稱.Trim();
            _欄位名稱列 = 欄位名稱列.Trim();
            _SQL查詢條件 = SQL查詢條件.Trim();
            Init();
        }

        public CheckedListBox _選取欄位 { get { return checkedListBox_選取欄位; } }
        public ComboBox _排序欄位 { get { return comboBox_排序欄位; } }
        public bool _排序遞減 { get { return checkBox_遞減.Checked; } }
        public ComboBox _查詢欄位 { get { return comboBox_查詢欄位; } }
        public string _查詢字串 { get { return textBox_查詢字串.Text; } }
        public NumericUpDown _目前頁面 { get { return numericUpDown_目前頁面; } }
        public NumericUpDown _每頁筆數
        {
            get { return numericUpDown_每頁筆數; }
        }
        //-----------------------------------------------------------------
        int _資料總筆數;
        DataGridView _datagridview;
        string _資料表名稱;
        string _欄位名稱列;
        string _SQL查詢條件;

        private void Init()
        {
            _查詢欄位.Items.Clear();
            _排序欄位.Items.Clear();
            _查詢欄位.Items.Add("");
            _排序欄位.Items.Add("");
            if (_欄位名稱列 == "" || _欄位名稱列 == "*")
            {
                string name;
                DataTable dt=DataTableUtils.DataTable_GetRowHeader(_資料表名稱);
                for(int index=0;index<dt.Columns.Count;index++)
                {
                    name = dt.Columns[index].ColumnName;
                    _查詢欄位.Items.Add(name);
                    _排序欄位.Items.Add(name);
                }
                dt.Dispose();
            }
            else
            {
                string[] str_tab = _欄位名稱列.Split(',');
                foreach(string name in str_tab)
                {
                    _查詢欄位.Items.Add(name);
                    _排序欄位.Items.Add(name);
                }
            }
            //-----------------------------------------------
            _選取欄位.Items.Clear();
            foreach (DataGridViewColumn column in _datagridview.Columns)
            {
                _選取欄位.Items.Add(column.Name, column.Visible);
            }
        }

        public void 設定分頁(int total_records, int page_size,int cur_page)
        {
            _資料總筆數 = total_records;
            _每頁筆數.Value = page_size;
            int pages = (total_records + page_size - 1) / page_size;
            label_總頁數.Text = string.Format("共有 {0} 筆資料，{1} 分頁", total_records, pages);
            numericUpDown_目前頁面.Value = 1;
            if (pages < 1) pages = 1;
            numericUpDown_目前頁面.Maximum = pages;
            if (cur_page > pages) cur_page = pages;
            else if (cur_page < 1) cur_page = 1;
            numericUpDown_目前頁面.Value = cur_page;
        }
       
        private void button_確定_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }


        private void button_nav_home_Click(object sender, EventArgs e)
        {
            decimal value = numericUpDown_目前頁面.Value;
            if (sender == button_nav_home) value = 1;
            else if (sender == button_nav_end) value = numericUpDown_目前頁面.Maximum;
            else if (sender == button_nav_next) value++;
            else if (sender == button_nav_prev) value--;
            if (value <= 0) value = 1;
            else if (value > numericUpDown_目前頁面.Maximum) value = numericUpDown_目前頁面.Maximum;
            numericUpDown_目前頁面.Value = value;
        }

        private void numericUpDown_每頁筆數_ValueChanged(object sender, EventArgs e)
        {
            設定分頁(_資料總筆數, (int)numericUpDown_每頁筆數.Value, (int) numericUpDown_目前頁面.Value);
        }

        private void button_選取欄位_全選_Click(object sender, EventArgs e)
        {
            bool is_checked = true;
            if (sender == button_選取欄位_清除) is_checked = false;
            for (int index = 0; index < checkedListBox_選取欄位.Items.Count; index++)
            {
                if (sender == button_選取欄位_反向)
                    is_checked = !checkedListBox_選取欄位.GetItemChecked(index);
                checkedListBox_選取欄位.SetItemChecked(index, is_checked);
            }
        }

        private void FormDataFilter_Load(object sender, EventArgs e)
        {

        }
    }
}
