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
    public partial class ucLedStatus : ucVisObj
    {        
        Timer timer = new Timer();
        public ucLedStatus()
        {
            InitializeComponent();
            _ucChild = this;
            _LedOnOff = false;
            timer.Tick += Timer_Tick;
            timer.Interval = 50;
            timer.Enabled = false;
        }

        public Color _BackColor
        {
            get { return ucLEDs_main.BackColor; }
            set { ucLEDs_main.BackColor = value; }
        }
        private void CtrlPanelOnClick(object sender, EventArgs e)
        {
            _CtrlVisible = !_CtrlVisible;
        }

        private void setLedOnOff()
        {
            int val = ucLEDs_main._LedValue[0];
            if (is_on)
            {
                ucLEDs_main._LedValue[0] = val ^ 1;
                Refresh();
            }
            else
            {
                if (val != 0)
                {
                    ucLEDs_main._LedValue[0] = 0;
                    Refresh();
                }
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Enabled = false;
            int val = ucLEDs_main._LedValue[0] ^ 1;
            if (is_on)
            {
                ucLEDs_main._LedValue[0] = val;
                if (val != 0) timer.Interval = 250;
                else timer.Interval = 150;
                timer.Enabled = true;
                Refresh();
            }
            else
            {
                if (val == 0)
                {
                    ucLEDs_main._LedValue[0] = 0;
                    Refresh();
                }
            }
        }

        bool is_on;
        public bool _LedOnOff
        {
            get
            {
                return is_on;
            }
            set
            {
                if (is_on != value)
                {
                    is_on = value;
                    timer.Enabled = true;
                }
            }
        }

        public userControl.LedColors _LedColor
        {
            get { return ucLEDs_main._LedColor; }
            set { ucLEDs_main._LedColor = value; }
        }
        public int _LedSize
        {
            get { return ucLEDs_main._LedSize; }
            set { ucLEDs_main._LedSize = value; }
        }
        public userControl.ucLEDs _ucLEDs
        {
            get { return ucLEDs_main; }
            set { ucLEDs_main = value; Refresh(); }
        }

            /*
            int count = 0;
            public override void Refresh()
            {
                _LedOnOff = !_LedOnOff;
                if (++count>3)
                {
                    count = 0;
                    int color = (int)ucLEDs1._LedColor + 1;
                    if (color > 7) color = 1;
                    ucLEDs1._LedColor = (userControl.LedColors)color;
                }
                base.Refresh();
            }
            */

        }
}
