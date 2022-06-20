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
    public partial class ucAnalogMeter : ucVisObj
    {
        public ucAnalogMeter()
        {
            Init(0, 100, "VU", 0);
            _Title = "音量大小";
        }
        public ucAnalogMeter(double min, double max,string 單位 = "V", int 小數點位數=2)
        {
            Init(min, max, 單位, 小數點位數);
        }
        public void Init(double min, double max, string 單位, int 小數點位數)
        {
            InitializeComponent();
            _ucChild = this;
            _小數點位數 = 小數點位數;
            _最小值 = min;
            _Value = min;
            _最大值 = max;
            _單位 = 單位;
        }

        private void ucVuMeter_main_Click(object sender, EventArgs e)
        {
            _CtrlVisible = !_CtrlVisible;
        }
        public ucPointerMeter _ucMeter { get { return ucPointerMeter_main; } }
        public TextBox _textBoxValue { get { return ucTextLabel_value._textBox; } }

        public override Color BackColor
        {
            get { return base.BackColor; }
            set
            {
                if (ucPointerMeter_main != null)
                {
                    ucPointerMeter_main.BackColor = value;
                    ucTextLabel_value.BackColor = value;
                }
                base.BackColor = value;
            }
        }

        public string _單位
        {
            get { return _ucMeter._MeterText; }
            set { _ucMeter._MeterText = value; }
        }

        public int _小數點位數
        {
            get { return _ucMeter._小數點位數; }
            set
            {
                _ucMeter._小數點位數 = value;
            }
        }

        public double _Value
        {
            get { return _ucMeter.Value; }
            set
            {
                _ucMeter.Value = value;
                string text= string.Format("{0}", _ucMeter.Value);
                ucTextLabel_value._Text = text;
            }
        }
        
        public double _最大值
        {
            get { return _ucMeter.Maximum; }
            set { _ucMeter.Maximum = value; }
        }

        public double _最小值
        {
            get { return _ucMeter.Minimum; }
            set { _ucMeter.Minimum = value; }
        }

        public override void Refresh()
        {
            Random ran = new Random();
            int scale = _ucMeter._放大倍率數;
            int delta = (int)(_最大值 - _最小值) * scale;
            _Value = (double)ran.Next(delta) / scale + _最小值;
            base.Refresh();
        }

       
    }
}
