using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Support.userControl
{
#if DEBUG != true
    [LicenseProvider(typeof(dekLicenseProvider))]
#endif
    public partial class FormVisProgressBar : Form
    {
        public TextBox Textbox { get { return textBox1; } }
        public ProgressBar Progressbar { get { return progressBar1; } }
        public string Title { get { return this.Text; } set { this.Text = value; } }

        public FormVisProgressBar(string title)
        {
            InitializeComponent();
            LicenseManager.Validate(typeof(FormVisProgressBar), this);
            this.Text = title;
        }
        
        public void ShowMesg()
        {
            textBox1.SelectionLength = 0;
            base.Show();
            Refresh();
        }
    }
}
