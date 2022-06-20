namespace Support.DashBoard
{
    partial class ucLedMatrix
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
            this.ucLEDs_Value = new Support.userControl.ucLEDs();
            this.SuspendLayout();
            // 
            // ucLEDs_Value
            // 
            this.ucLEDs_Value._LedColor = Support.userControl.LedColors.Red;
            this.ucLEDs_Value._LedMatrixSize = new System.Drawing.Size(8, 4);
            this.ucLEDs_Value._LedPosition = Support.userControl.LedPosition.Center;
            this.ucLEDs_Value._LedSize = 42;
            this.ucLEDs_Value._LedValue = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            this.ucLEDs_Value.BackColor = System.Drawing.Color.Transparent;
            this.ucLEDs_Value.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucLEDs_Value.Location = new System.Drawing.Point(0, 0);
            this.ucLEDs_Value.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.ucLEDs_Value.Name = "ucLEDs_Value";
            this.ucLEDs_Value.Size = new System.Drawing.Size(356, 229);
            this.ucLEDs_Value.TabIndex = 4;
            this.ucLEDs_Value.Click += new System.EventHandler(this.ucTextLabel_title_Click);
            // 
            // ucLedMatrix
            // 
            this._Title_Height = 46;
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.ucLEDs_Value);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "ucLedMatrix";
            this.Size = new System.Drawing.Size(356, 229);
            this.Controls.SetChildIndex(this.ucLEDs_Value, 0);
            this.ResumeLayout(false);

        }

        #endregion
        private userControl.ucLEDs ucLEDs_Value;
    }
}
