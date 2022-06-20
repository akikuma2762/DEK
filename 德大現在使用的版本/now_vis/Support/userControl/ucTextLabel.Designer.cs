namespace Support.userControl
{
    partial class ucTextLabel
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
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
            this.textBox_main = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBox_main
            // 
            this.textBox_main.BackColor = System.Drawing.Color.White;
            this.textBox_main.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_main.Font = new System.Drawing.Font("Microsoft JhengHei", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox_main.Location = new System.Drawing.Point(0, 0);
            this.textBox_main.Multiline = true;
            this.textBox_main.Name = "textBox_main";
            this.textBox_main.Size = new System.Drawing.Size(152, 38);
            this.textBox_main.TabIndex = 1;
            this.textBox_main.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ucTextLabel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBox_main);
            this.Font = new System.Drawing.Font("Microsoft JhengHei", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Name = "ucTextLabel";
            this.Size = new System.Drawing.Size(152, 38);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_main;
    }
}
