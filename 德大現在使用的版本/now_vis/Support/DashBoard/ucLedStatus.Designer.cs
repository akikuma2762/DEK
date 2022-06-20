namespace Support.DashBoard
{
    partial class ucLedStatus
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
            this.ucLEDs_main = new Support.userControl.ucLEDs();
            this.SuspendLayout();
            // 
            // ucLEDs_main
            // 
            this.ucLEDs_main._LedColor = Support.userControl.LedColors.Red;
            this.ucLEDs_main._LedMatrixSize = new System.Drawing.Size(1, 1);
            this.ucLEDs_main._LedPosition = Support.userControl.LedPosition.Center;
            this.ucLEDs_main._LedSize = 48;
            this.ucLEDs_main._LedValue = new int[] {
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
            this.ucLEDs_main.BackColor = System.Drawing.Color.Transparent;
            this.ucLEDs_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucLEDs_main.Location = new System.Drawing.Point(0, 0);
            this.ucLEDs_main.Margin = new System.Windows.Forms.Padding(5);
            this.ucLEDs_main.Name = "ucLEDs_main";
            this.ucLEDs_main.Size = new System.Drawing.Size(94, 95);
            this.ucLEDs_main.TabIndex = 2;
            this.ucLEDs_main.Click += new System.EventHandler(this.CtrlPanelOnClick);
            // 
            // ucLedStatus
            // 
            this._Title_Height = 36;
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.ucLEDs_main);
            this.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.Name = "ucLedStatus";
            this.Size = new System.Drawing.Size(94, 95);
            this.Click += new System.EventHandler(this.CtrlPanelOnClick);
            this.Controls.SetChildIndex(this.ucLEDs_main, 0);
            this.ResumeLayout(false);

        }

        #endregion
        private userControl.ucLEDs ucLEDs_main;
    }
}
