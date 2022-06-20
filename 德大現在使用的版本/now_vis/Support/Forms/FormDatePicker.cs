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
    public partial class FormDatePicker : Form
    {
        public string date_text = "";
        public FormDatePicker()
        {
            InitializeComponent();
            LicenseManager.Validate(typeof(FormDatePicker), this);
        }

        private void FormDatePicker_Load(object sender, EventArgs e)
        {

        }

        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            date_text = e.Start.ToString("yyyy-MM-dd");
            this.Close();
        }
    }
}
