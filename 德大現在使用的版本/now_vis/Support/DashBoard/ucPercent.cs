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
    public partial class ucPercent : ucVisObj
    {
        public enum 外觀類型 { 深海藍_預設, 天空藍, 葡萄紫, 晚霞橘 }
        public ucPercent()
        {
            InitializeComponent();
            _CtrlVisible = false;
            _ucChild = this;
            _外觀 = 外觀類型.深海藍_預設;
        }

        public CircularProgressBar.CircularProgressBar _circularProgressBar
        {
            get { return circularProgressBar_main; }
        }

        //-------------------------------------------------------------
        private void ucCircleProgressBar_Click(object sender, EventArgs e)
        {
            _CtrlVisible = !_CtrlVisible;
        }


        //========================================================
        public int _Value
        {
            get { return _circularProgressBar.Value; }
            set
            {
                double percent;
                if (value < _Minimum) { value = _Minimum; percent = 0; }
                else if (value > _Maximum) { value = _Maximum; percent = 100; }
                else
                {
                    percent = value;
                    percent = (percent - _Minimum) / (_Maximum - _Minimum) * 100;
                }
                _circularProgressBar.Value = value;
                int data = (int)percent;
                _中心圓文字 = data.ToString();
                data = ((int)(percent * 100)) % 100;
                string text = "";
                if (data != 0) text = "." + data.ToString();
                _中心圓右下附註訊息 = text;
            }
        }

        public int _Maximum
        {
            get { return _circularProgressBar.Maximum; }
            set
            {
                if (value < _Minimum)
                {
                    value = _Minimum;
                    _Value = value;
                }
                _circularProgressBar.Maximum = value;
            }
        }

        public int _Minimum
        {
            get { return _circularProgressBar.Minimum; }
            set
            {
                if (value > _Maximum)
                {
                    value = _Maximum;
                    _Value = value;
                }
                _circularProgressBar.Minimum = value;
            }
        }

        外觀類型 style_name;
        public 外觀類型 _外觀
        {
            get { return style_name; }
            set { 設定外觀類型(value); style_name = value; }
        }

        public Color _控制項背景顏色 { set { _circularProgressBar.BackColor = value; } }

        public Font _中心字體大小 { set { _circularProgressBar.Font = value; } }
        public Color _中心字形顏色 { set { _circularProgressBar.ForeColor = value; } }
        public Color _中心圓背景顏色 { set { _circularProgressBar.InnerColor = value; } }
        public int _中心圓背景內縮 { set { _circularProgressBar.InnerMargin = value; } }
        public int _中心圓背景擴張 { set { _circularProgressBar.InnerWidth = value; } }
        public string _中心圓文字 { set { _circularProgressBar.Text = value; } }
        public Padding _中心文字位置 { set { _circularProgressBar.TextMargin = value; } }

        public string _中心圓右上附註訊息 { set { _circularProgressBar.SuperscriptText = value; } }
        public Padding _中心圓右上附註訊息位置 { set { _circularProgressBar.SuperscriptMargin = value; } }
        public Color _中心圓右上角附註訊息顏色 { set { _circularProgressBar.SuperscriptColor = value; } }
        public Font _中心圓右上附註訊息大小 { set { _circularProgressBar.SecondaryFont = value; } }

        public string _中心圓右下附註訊息 { set { _circularProgressBar.SubscriptText = value; } }
        public Padding _中心圓右下附註訊息位置 { set { _circularProgressBar.SubscriptMargin = value; } }
        public Color _中心圓右下角附註訊息顏色 { set { _circularProgressBar.SubscriptColor = value; } }

        public Padding _外圈圓大小 { set { _circularProgressBar.Margin = value; } }
        public Color _外圈圓無進度顏色 { set { _circularProgressBar.OuterColor = value; } }
        public Color _外圈圓進度條顏色 { set { _circularProgressBar.ProgressColor = value; } }
        public int _外圈圓寬度 { set { _circularProgressBar.ProgressWidth = value; } }
        public int _外圈圓角度 { set { _circularProgressBar.StartAngle = value; } }


        public void 設定外觀類型(外觀類型 style_name)
        {
            Control chart = circularProgressBar_main;
            _中心文字位置 = new Padding(0, 8, 0, 0);
            _中心字體大小 = new Font(chart.Font.Name, 30);
            //_中心圓右上附註訊息 = "%";
            _中心圓右上附註訊息位置 = new Padding(10, 20, 0, 0);
            _中心圓右上附註訊息大小 = new Font(chart.Font.Name, 18);
            _中心圓右下附註訊息位置 = new Padding(5, -25, 0, 0);
            _外圈圓寬度 = 10;
            _外圈圓大小 = new Padding(-24);

            switch (style_name)
            {
                case 外觀類型.天空藍:
                    _中心圓背景顏色 = Color.FromArgb(224, 224, 224);                   
                    _中心字形顏色 = Color.FromArgb(0, 94, 186);
                    _中心圓右上角附註訊息顏色 = Color.FromArgb(0, 94, 186);
                    _中心圓右下角附註訊息顏色 = Color.DarkGray;
                    _外圈圓無進度顏色 = Color.White;
                    _外圈圓進度條顏色 = Color.FromArgb(0, 94, 186);
                    break;
                case 外觀類型.葡萄紫:
                    _中心圓背景顏色 = Color.FromArgb(224, 224, 224); // this.ParentForm.BackColor;
                    _中心字形顏色 = Color.FromArgb(138, 137, 138);                   
                    _中心圓右上角附註訊息顏色 = Color.FromArgb(138, 137, 138);                    
                    _中心圓右下角附註訊息顏色 = Color.FromArgb(138, 137, 138);                    
                    _外圈圓無進度顏色 = Color.FromArgb(197, 195, 197);
                    _外圈圓進度條顏色 = Color.FromArgb(144, 77, 173);
                    break;

                case 外觀類型.晚霞橘:
                    _中心圓背景顏色 = Color.Aqua;// Color.FromArgb(224, 223, 225);
                    _中心字形顏色 = Color.FromArgb(63, 62, 63);                    
                    _中心圓右上角附註訊息顏色 = Color.Black;
                    _中心圓右下角附註訊息顏色 = Color.FromArgb(165, 165, 165);
                    _外圈圓無進度顏色 = Color.FromArgb(129, 128, 129);
                    _外圈圓進度條顏色 = Color.FromArgb(255, 132, 0);
                    break;
                case 外觀類型.深海藍_預設:
                    _中心圓背景顏色 = Color.FromArgb(60, 79, 104);
                    _中心字形顏色 = Color.White;
                    _中心圓右上角附註訊息顏色 = Color.LightGray;
                    _中心圓右下角附註訊息顏色 = Color.White;
                    _外圈圓無進度顏色 = Color.White;
                    _外圈圓進度條顏色 = Color.FromArgb(50, 150, 220);
                    break;
            }
        }
    }
}
