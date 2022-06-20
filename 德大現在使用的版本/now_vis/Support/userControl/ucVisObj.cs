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
    public partial class ucVisObj : UserControl
    {
        public VisObjProperty property;
        //---------------------------------------------------------
        public Button CtrllButton { get { return button_change; } }
        public Panel CtrlPanel { get { return panel_ctrl; } }
        //---------------------------------------------------------
        private int update_time_left_ms = 0;
        public ucVisObj()
        {
            InitializeComponent();
            property = new VisObjProperty(this);
            button_change.BringToFront();
        }

        public void CtrlButton_MoveResize(int x,int y,int w=0,int h=0)
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
        public void MoveResize(int left, int top, int width = 0, int height = 0)
        {
            Left = left;
            Top = top;
            if (width > 0) Width = width;
            if (height > 0) Height = height;
        }

        public void UpdateRefresh(int time_period_ms)
        {
            if (update_time_left_ms >= 0)
            {
                update_time_left_ms -= time_period_ms;
                if (update_time_left_ms <= 0)
                {
                    Refresh();
                    update_time_left_ms = (int)(property.更新速率_間隔秒數*1000);
                }
            }
        }

        private void button_change_Click(object sender, EventArgs e)
        {
            int update_ms = update_time_left_ms;
            update_time_left_ms = -1;
            Support.FormVisObj form = new Support.FormVisObj(this);
            form.propertyGrid.SelectedObject = this.property;
            form.ShowDialog();
            form.Dispose();
            update_time_left_ms = update_ms;
        }
    }

    public class VisObjProperty
    {
        private UserControl parent;
        private int UpdateRate_ms = 5000;
        public double 更新速率_間隔秒數
        {
            get { return UpdateRate_ms / 1000.0; }
            set
            {
                if (value < 1) value = 1;
                UpdateRate_ms = (int)(value * 1000.0);
            }
        }
        public DateTime 資料起始時間 { get; set; }// Data_TimeStart;
        public TimeSpan 資料時間間隔 { get; set; }
 
        public VisObjProperty(UserControl ctrl)
        {
            parent = ctrl;
        }
        
        public Point 位置
        {
            get { return parent.Location; }
            set { parent.Location = value; }
        }

        public Size 大小
        {
            get { return parent.Size; }
            set { parent.Size = value; }
        }
        

    }
}
