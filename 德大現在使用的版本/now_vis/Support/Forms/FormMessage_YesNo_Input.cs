using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Support;

namespace Support.Forms
{
#if DEBUG != true
    [LicenseProvider(typeof(dekLicenseProvider))]
#endif
    public partial class FormMessage_YesNo_Input : Form
    {
        //標題
        public string Title { get { return this.Text; } set { Text = value; } }
        //內容
        public string content { get { return label_content.Text; } set { label_content.Text = value; } }
        //取值
        public string quantity { get { return numericUpDown_quantity.Text; } set { numericUpDown_quantity.Text = value; } }
        //輸入框旁的提示文字
        public string info { get { return label_提示文字.Text; } set { label_提示文字.Text = value; } }
        //限制上限數值
        public int Max = 0;

        public FormMessage_YesNo_Input()
        {
            InitializeComponent();
            LicenseManager.Validate(typeof(FormMessage_YesNo_Input), this);
        }
        private void button_確定_Click(object sender, EventArgs e)
        {
            int 輸入數量 = DataTableUtils.toInt(numericUpDown_quantity.Text.ToString());

            if (輸入數量 < 0)
            {
                Support.Forms.FormMessage formMessage = new FormMessage("輸入數量錯誤,請重新確認", "提醒");
                formMessage.ShowDialog();
            }
            else if (輸入數量 > Max)
            {
                Support.Forms.FormMessage formMessage = new FormMessage("輸入數量不得大於庫存量(" + Max + ")", "提醒");
                formMessage.ShowDialog();
            }
            else
            {
                this.Close();
            }
        }
        private void button_取消_Click(object sender, EventArgs e)
        {
            numericUpDown_quantity.Text=null;
            this.Close();
        }

        private void FormMessage_YesNo_Input_Load(object sender, EventArgs e)
        {

        }
    }
}
