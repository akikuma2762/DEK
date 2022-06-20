using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Support.DashBoard
{
    public partial class ucDataView : ucVisObj
    {
        public ucDataView(string table_name, string 欄位名稱列 = "*", string sql_查詢條件 = "")
        {
            InitializeComponent();
            _ucChild = this;
            ReLoad(table_name, 欄位名稱列, sql_查詢條件);
            ToolStripMenuItem item = new ToolStripMenuItem();
            item.Text = "屬性編輯控制板";
            item.Click += Item顯示控制板_Click;
            ucDataGridView_main._contextMenuStrip.Items.Add(item);
        }

        private void Item顯示控制板_Click(object sender, EventArgs e)
        {
            _CtrlVisible = !_CtrlVisible;
        }

        public void ReLoad(string table_name, string 欄位名稱列, string sql_查詢條件)
        {
            ucDataGridView_main.Init(table_name, 欄位名稱列, sql_查詢條件);
        }

        public userControl.ucDataGridView _ucDataGridView { get { return ucDataGridView_main; } }

        public override void Refresh()
        {
            ucDataGridView_main.LoadData();
            base.Refresh();
        }
    }
}
