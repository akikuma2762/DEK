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
    public partial class FormDataRowSelect : Form
    {
        public FormDataRowSelect(string table, string sql_查詢條件, bool is_multi_row_select = false)
        {
            InitializeComponent();
            LicenseManager.Validate(typeof(FormDataRowSelect), this);
            ucDataRowFilter1.Init(table, sql_查詢條件, is_multi_row_select);
            ucDataRowFilter1.Ctrl_PanelFilter.Visible = true;
        }

        public string Title { get { return this.Text; } set { Text = value; } }
        public DataRow SelectedRow{ get{ return ucDataRowFilter1.SelectedRow; } }

        private void FormDataRowSelect_Load(object sender, EventArgs e)
        {
            ucDataRowFilter1.Cell_DoubleClick_TrunOn = true;
        }

        public DataGridView dataGridView
        {
            get { return ucDataRowFilter1.dataGridView; }
        }
        public userControl.DataGridViewCellEvent OnRowEnter
        {
            get { return ucDataRowFilter1.OnRowEnter; }
            set { ucDataRowFilter1.OnRowEnter = value; }
        }

        private void button_確定_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Close();
        }
    }
}
