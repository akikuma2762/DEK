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
    public partial class ucDatePicker : UserControl
    {
        public TextBox _date { get { return textBox_date; } }
        public ucDatePicker()
        {
            InitializeComponent();
            LicenseManager.Validate(typeof(ucDatePicker), this);
        }

        private void button_OpenMonthPicker_Click(object sender, EventArgs e)
        {
            Support.Forms.FormDatePicker formDatePicker = new Forms.FormDatePicker();
            formDatePicker.StartPosition = FormStartPosition.CenterScreen;
            formDatePicker.ShowDialog();
            _date.Text = formDatePicker.date_text;

        }
    }
}
