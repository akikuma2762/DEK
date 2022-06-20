using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Support.userControl
{
#if DEBUG != true
    [LicenseProvider(typeof(dekLicenseProvider))]
#endif
    public partial class ucTextLabel : UserControl
    {
        [DllImport("user32.dll")]
        static extern bool HideCaret(System.IntPtr hWnd);
        public ucTextLabel()
        {
            InitializeComponent();
            LicenseManager.Validate(typeof(ucTextLabel), this);
            textBox_main.GotFocus += TextBox_main_GotFocus;
        }

        private void TextBox_main_GotFocus(object sender, EventArgs e)
        {
            HideCaret(_textBox.Handle);
        }

        public TextBox _textBox { get { return textBox_main; } }
        public Font _Font
        {
            get { return _textBox.Font; }
            set { _textBox.Font = value; }
        }
       

        public HorizontalAlignment _TextAlign
        {
            get { return textBox_main.TextAlign; }
            set { textBox_main.TextAlign = value; }
        }

        public string _Text
        {
            get { return textBox_main.Text; }
            set { textBox_main.Text = value; }
        }

        public Color _ForeColor
        {
            get { return _textBox.ForeColor; }
            set { _textBox.ForeColor = value; }
        }

        public Color _BackColor
        {
            get { return _textBox.BackColor; }
            set
            {
                _textBox.BackColor = value;
                base.BackColor = value;
            }
        }

    }
}
