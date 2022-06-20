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
    public partial class ucDialGauge : ucVisObj
    {
        public ucDialGauge()
        {
            //Init(0, 100, 0, 10);  //20181106WSWANG--
            Init(0, 20000, 0, 3000);
        }
        //public ucDialGauge(float 最小值 = 0, float 最大值 = 100, float 預設值 = 0, float 主格線單位 = 10)//20181106WSWANG--
        public ucDialGauge(float 最小值 = 0, float 最大值 = 20000, float 預設值 = 0, float 主格線單位 = 3000)
        {
            Init(最小值, 最大值, 預設值, 主格線單位);
        }

        void Init(float 最小值, float 最大值, float 預設值, float 主格線單位)
        {
            InitializeComponent();
            _ucChild = this;
            _aGauge.MinValue = 最小值;
            _aGauge.MaxValue = 最大值;
            _aGauge.Value = 預設值;
            _aGauge.ScaleLinesMajorStepValue = 主格線單位;
            _設定警示範圍(最小值, 最小值);
        }

        private void aGauge1_Click(object sender, EventArgs e)
        {
            _CtrlVisible = !_CtrlVisible;
        }

        public AGauge _aGauge { get { return aGauge_main; } }        
        public float _最大值
        { get { return _aGauge.MaxValue; } set { _aGauge.MaxValue = value; } }

        public  Color _BackColor
        {
            get => aGauge_main.BackColor;
            set => aGauge_main.BackColor = value;
        }
        public float _最小值
        { get { return _aGauge.MinValue; } set { _aGauge.MinValue = value; } }
        public float _Value
        {
            get { return aGauge_main.Value; }
            set
            {
                _aGauge.Value = value;
                if (_aGauge.GaugeLabels.Count > 0)
                {
                    string text = value.ToString(_aGauge.ScaleNumbersFormat);
                    _aGauge.GaugeLabels[0].Text = text;
                }
                if (_警示顏色顯示)
                {
                    Color color;
                    if (_Value < _警示下限值)
                        color = Color.LightCyan;
                    else if (_Value > _警示上限值)
                        color = Color.Orange;
                    else color = Color.LightGreen;
                    _aGauge.BackColor = color;
                }
                //else _aGauge.BackColor = Color.White;
                else _aGauge.BackColor = Color.Gray;
            }
        }

        public string _單位名稱
        {
            get { return _aGauge.GaugeLabels[1].Text; }
            set { _aGauge.GaugeLabels[1].Text = value; }
        }

        public float _主格線單位
        {
            get { return _aGauge.ScaleLinesMajorStepValue; }
            set { _aGauge.ScaleLinesMajorStepValue = value; }
        }
        public float _警示下限值
        {
            get { return _aGauge.GaugeRanges[0].EndValue; }
            set
            {
                _aGauge.GaugeRanges[0].StartValue = _aGauge.MinValue;
                _aGauge.GaugeRanges[0].EndValue = value;
                _aGauge.GaugeRanges[1].StartValue = value;
            }
        }
        public float _警示上限值
        {
            get { return _aGauge.GaugeRanges[1].EndValue; }
            set
            {
                _aGauge.GaugeRanges[1].EndValue = value;
                _aGauge.GaugeRanges[2].StartValue = value;
                _aGauge.GaugeRanges[2].EndValue = _aGauge.MaxValue;
            }
        }

        public bool _警示顏色顯示 { get; set; }

        public AGaugeRangeCollection Range { get { return _aGauge.GaugeRanges; } }

        public void _設定警示範圍(float low_limit,float up_limit)
        {
            _警示顏色顯示 = (low_limit < up_limit);
            _警示下限值 = low_limit;
            _警示上限值 = up_limit;
            if (low_limit>=up_limit)
                _aGauge.GaugeRanges[2].EndValue = low_limit;
        }
        public override void Refresh()
        {
            Random ran = new Random();
            int delta = (int)(_aGauge.MaxValue - _aGauge.MinValue) * 10;
            _Value = _Value + (delta / 2 - ran.Next(delta)) / 10;
            base.Refresh();
        }


    }
}
