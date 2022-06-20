using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Support
{
#if DEBUG != true
    [LicenseProvider(typeof(dekLicenseProvider))]
#endif
    public partial class FormPropertyEdit : Form
    {
        public PropertyGrid propertyGrid { get { return propertyGrid_Obj; } }
        public FormPropertyEdit()
        {
            InitializeComponent();
            LicenseManager.Validate(typeof(FormPropertyEdit), this);
        }

        private void button_panel_close_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
