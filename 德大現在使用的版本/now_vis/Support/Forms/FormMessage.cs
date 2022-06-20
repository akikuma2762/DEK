using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Support.Forms
{
#if DEBUG != true
    [LicenseProvider(typeof(dekLicenseProvider))]
#endif
    public partial class FormMessage : Form
    {
        public string _視窗抬頭文字
        {
            get { return this.Text; }
            set { this.Text = value; }
        }
        public string _訊息文字;
        public Point _位置
        {
            get { return this.Location; }
            set { this.Location = value; }
        }
        int delta_w, delta_h;
        int min_width;
        bool is_beep;
        public FormMessage()
        {
            Init("", "!! 請注意 !!", true);
        }

        public FormMessage(string mesg)
        {
            Init(mesg, "!! 請注意 !!", true);
        }
        public FormMessage(string mesg, string title, bool is_beep=true)
        {
            Init(mesg, title, is_beep);
        }

        private void Init(string mesg, string title, bool is_beep)
        {
            InitializeComponent();
            LicenseManager.Validate(typeof(FormMessage), this);
            this.Text = title;
            _訊息文字 = mesg;
            this.is_beep = is_beep;
            min_width = ClientSize.Width;
            delta_w = this.Width - ClientSize.Width;
            delta_h = this.Height - ClientSize.Height + panel_bottom.Height + panel_top.Height;
            textBox_Mesg.GotFocus += TextBox_Mesg_GotFocus;
        }

        [DllImport("user32.dll")]
        static extern bool HideCaret(IntPtr hWnd);
        private void TextBox_Mesg_GotFocus(object sender, EventArgs e)
        {
            HideCaret(textBox_Mesg.Handle);
        }

        private void AdjustSize()
        {
            Graphics gr = textBox_Mesg.CreateGraphics();
            string[] str_tab = _訊息文字.Split('\n');
            string str;
            float width = delta_w, 
                  height = delta_h;
            SizeF size;
            textBox_Mesg.Text = "";
            for (int index = 0; index < str_tab.Length; index++)
            {
                if (index > 0)
                    textBox_Mesg.AppendText(Environment.NewLine);
                str = str_tab[index];
                textBox_Mesg.AppendText(str);
                size = gr.MeasureString(str + "AAAA", textBox_Mesg.Font);
                height += size.Height;
                if (size.Width > width) width = size.Width;
            }
            size = gr.MeasureString("", textBox_Mesg.Font);
            height += size.Height;
            if (width < min_width) width = min_width;
            this.Height = (int)(height+0.5f);
            this.Width = (int)(width+0.5f);
        }

        public new void Show()
        {
            AdjustSize();
            if (is_beep)
                System.Media.SystemSounds.Hand.Play();
            base.Show();
            Refresh();
        }

        public new DialogResult ShowDialog()
        {
            AdjustSize();
            if (is_beep)
                System.Media.SystemSounds.Hand.Play();
            return base.ShowDialog();
        }

        private void FormMessage_Load(object sender, EventArgs e)
        {

        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
