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

namespace Support.DashBoard
{
#if DEBUG!=true
    [LicenseProvider(typeof(dekLicenseProvider))]
#endif
    public partial class ucVisObj : UserControl
    {
        public VisObjProperty _Property;
        public UserControl _ucChild;
        //---------------------------------------------------------
        private int update_time_left_ms = 0;
        public ucVisObj()
        {
            InitializeComponent();
            DoubleBuffered = true;
            LicenseManager.Validate(typeof(ucVisObj), this);
            _ucChild = null;
            _Property = new VisObjProperty(this);
            _CtrlPanel.Height = 32;
            _CtrlPanel.Visible = false;
            textBox_Title.GotFocus += textBox_Title_GotFocus;
        }
        //====================================================================
        public Panel _CtrlPanel { get { return panel_ctrl; } }
        public bool _CtrlVisible
        {
            get { return panel_buttons.Visible; }
            set
            {
                panel_buttons.Visible = value;
                if (value==true)
                {
                    panel_ctrl.Visible = true;
                }
                else panel_ctrl.Visible = (_Title.Length>0);
            }
        }

        private void SetTitleHeight()
        {
            if (Text != null && Text != "")
            {
                Size size = TextRenderer.MeasureText(_Title, _Title_Font);
                if (panel_ctrl.Height < size.Height)
                {
                    panel_ctrl.Height = size.Height + 4;
                    Invalidate();
                }
            }
        }
        public override string Text
        {
            get { return textBox_Title.Text; }
            set
            {
                if (value != "")
                {
                    _CtrlPanel.Visible = true;
                }
                textBox_Title.Text = value;
                SetTitleHeight();
            }
        }
        
        public string _Title
        {
            get { return Text; }
            set
            {
                Text = value;
            }
        }

        public Color _Title_BackColor
        {
            get { return textBox_Title.BackColor; }
            set { textBox_Title.BackColor = value; }
        }

        public Color _Title_ForeColor
        {
            get { return textBox_Title.ForeColor; }
            set { textBox_Title.ForeColor = value; }
        }

        public Font _Title_Font
        {
            get { return textBox_Title.Font; }
            set
            {
                textBox_Title.Font = value;
                if (_Title != "")
                {
                    SetTitleHeight();
                }
            }
        }

        public int _Title_Height
        {
            get { return panel_ctrl.Height; }
            set { panel_ctrl.Height = value; }
        }

        public HorizontalAlignment _Title_Align
        {
            get { return textBox_Title.TextAlign; }
            set { textBox_Title.TextAlign = value; }
        }
        //---------------------------------------------------------
        [DllImport("user32.dll")] static extern bool HideCaret(System.IntPtr hWnd);
        private void textBox_Title_GotFocus(object sender, EventArgs e)
        {
            HideCaret(textBox_Title.Handle);
        }

        public void _CtrlButton_MoveResize(int x,int y,int w=0,int h=0)
        {
            button_change.Left = x;
            button_change.Top = y;
            if (w > 0) button_change.Width = w;
            if (h > 0) button_change.Height = h;
            if (button_change.Left < 0)
                button_change.Left = 0;
            else if (button_change.Right>this.ClientSize.Width)
                button_change.Left = this.ClientSize.Width - button_change.Width;
            if (button_change.Top < 0) button_change.Top = 0;
            else if (button_change.Bottom > this.ClientSize.Height)
                button_change.Top = this.ClientSize.Height - button_change.Height;
        }
        public void _MoveResize(int left, int top, int width = 0, int height = 0)
        {
            Left = left;
            Top = top;
            if (width > 0) Width = width;
            if (height > 0) Height = height;
        }

        public void _UpdateRefresh(int time_period_ms)
        {
            if (update_time_left_ms >= 0)
            {
                update_time_left_ms -= time_period_ms;
                if (update_time_left_ms <= 0)
                {
                    Refresh();
                    update_time_left_ms = (int)(_Property._更新速率_間隔秒數*1000);
                }
            }
        }

        private void button_change_Click(object sender, EventArgs e)
        {
            int update_ms = update_time_left_ms;
            update_time_left_ms = -1;
            Support.FormPropertyEdit form = new Support.FormPropertyEdit();
            if (sender==button_change_child &&  _ucChild != null)
                form.propertyGrid.SelectedObject = _ucChild;
            else form.propertyGrid.SelectedObject = _Property;
            form.ShowDialog();
            form.Dispose();
            _CtrlVisible = false;
            update_time_left_ms = update_ms;
        }

        private void button_hide_Click(object sender, EventArgs e)
        {
            _CtrlVisible = false;
        }

    }

    public class VisObjProperty
    {
        private UserControl parent;
        private int UpdateRate_ms = 5000;

        public VisObjProperty(UserControl ctrl)
        {
            parent = ctrl;
        }

        public double _更新速率_間隔秒數
        {
            get { return UpdateRate_ms / 1000.0; }
            set
            {
                if (value < 1) value = 1;
                UpdateRate_ms = (int)(value * 1000.0);
            }
        }
        public DateTime _資料起始時間 { get; set; }// Data_TimeStart;
        public TimeSpan _資料時間間隔 { get; set; }
        
        public Point _位置
        {
            get { return parent.Location; }
            set { parent.Location = value; }
        }

        public Size _大小
        {
            get { return parent.Size; }
            set { parent.Size = value; }
        }
        

    }
}
