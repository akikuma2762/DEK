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
    public partial class ucLedMatrix : ucVisObj
    {
        public ucLedMatrix()
        {
            InitializeComponent();
            _ucChild = this;
        }

        public int[] _LedValue
        {
            get { return ucLEDs_Value._LedValue; }
            set
            {
                ucLEDs_Value._LedValue = value;
            }
        }

        public userControl.ucLEDs _ucLEDs
        {
            get { return ucLEDs_Value; }
            set { ucLEDs_Value = value; }
        }
        public userControl.LedColors _LedColor
        {
            get { return ucLEDs_Value._LedColor; }
            set
            {
                ucLEDs_Value._LedColor = value;
            }
        }

        private void ucTextLabel_title_Click(object sender, EventArgs e)
        {
            _CtrlVisible = !_CtrlVisible;
        }

        //int count =0;
        public override void Refresh()
        {
            /*
            Random ran = new Random();
            for(int index=0;index<ucLEDs_Value._LedMatrixSize.Height;index++)
               _LedValue[index] = ran.Next();
            if (++count >=3)
            {
                count = 0;
                _LedColor = (userControl.LedColors)(ran.Next(7) + 1);
            }
            */
            base.Refresh();
        }

        
    }
}
