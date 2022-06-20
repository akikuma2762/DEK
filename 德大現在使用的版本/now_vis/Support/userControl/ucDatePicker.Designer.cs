namespace Support.userControl
{
    partial class ucDatePicker
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBox_date = new System.Windows.Forms.TextBox();
            this.button_OpenMonthPicker = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.textBox_date);
            this.panel1.Controls.Add(this.button_OpenMonthPicker);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(226, 29);
            this.panel1.TabIndex = 2;
            // 
            // textBox_date
            // 
            this.textBox_date.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_date.Enabled = false;
            this.textBox_date.Location = new System.Drawing.Point(0, 0);
            this.textBox_date.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox_date.Name = "textBox_date";
            this.textBox_date.Size = new System.Drawing.Size(190, 29);
            this.textBox_date.TabIndex = 2;
            // 
            // button_OpenMonthPicker
            // 
            this.button_OpenMonthPicker.Dock = System.Windows.Forms.DockStyle.Right;
            this.button_OpenMonthPicker.Location = new System.Drawing.Point(190, 0);
            this.button_OpenMonthPicker.Name = "button_OpenMonthPicker";
            this.button_OpenMonthPicker.Size = new System.Drawing.Size(36, 29);
            this.button_OpenMonthPicker.TabIndex = 3;
            this.button_OpenMonthPicker.Text = "...";
            this.button_OpenMonthPicker.UseVisualStyleBackColor = true;
            this.button_OpenMonthPicker.Click += new System.EventHandler(this.button_OpenMonthPicker_Click);
            // 
            // ucDatePicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Microsoft JhengHei", 12F);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ucDatePicker";
            this.Size = new System.Drawing.Size(226, 29);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button_OpenMonthPicker;
        private System.Windows.Forms.TextBox textBox_date;
    }
}
