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
    public partial class FormYesNo : Form
    {
        public string _視窗抬頭文字
        {
            get { return this.Text; }
            set { this.Text = value; }
        }
        public string _訊息文字;
        public string _執行結果文字;
        public Button _確定按鈕 { get { return button_ok; } }
        public Button _取消按鈕 { get { return button_cancel; } }
        public Point _位置
        {
            get { return this.Location; }
            set { this.Location = value; }
        }

        int delta_w, delta_h;
        int min_width;
        bool is_beep;
        public FormYesNo()
        {
            Init("", "!! 請注意 !!", true);
        }

        public FormYesNo(string mesg)
        {
            Init(mesg, "!! 請注意 !!", true);
        }
        public FormYesNo(string mesg, string title, bool is_beep=true)
        {
            Init(mesg, title, is_beep);
        }

        private void Init(string mesg, string title, bool is_beep)
        {
            InitializeComponent();
            LicenseManager.Validate(typeof(FormYesNo), this);
            button_ok.Tag = DialogResult.OK;
            button_cancel.Tag = DialogResult.Cancel;
            this.Text = title;
            _訊息文字 = mesg;
            this.is_beep = is_beep;
            min_width = ClientSize.Width;
            delta_w = this.Width - ClientSize.Width;
            delta_h = this.Height - ClientSize.Height + panel_bottom.Height+panel_top.Height;
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
            float width = delta_w, 
                  height = delta_h;
            SizeF size;
            textBox_Mesg.Text = "";
            foreach(string str in str_tab)
            {
                textBox_Mesg.AppendText(str+ Environment.NewLine);
                size = gr.MeasureString(str + "AAAA", textBox_Mesg.Font);
                height += size.Height;
                if (size.Width > width) width = size.Width;
            }
            size = gr.MeasureString("", textBox_Mesg.Font);
            height += size.Height;
            if (width < min_width) width = min_width;
            this.Height = (int)height;
            this.Width = (int)width;
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

        private void button_ok_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            _執行結果文字 = btn.Text;
            DialogResult = (DialogResult) btn.Tag;
            Close();
        }
    }
}
