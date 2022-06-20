namespace Support.DashBoard
{
    partial class ucAnalogMeter
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
            this.ucPointerMeter_main = new Support.ucPointerMeter();
            this.ucTextLabel_value = new Support.userControl.ucTextLabel();
            this.SuspendLayout();
            // 
            // ucPointerMeter_main
            // 
            this.ucPointerMeter_main._MeterText = "VU";
            this.ucPointerMeter_main._小數點位數 = 0;
            this.ucPointerMeter_main.BackColor = System.Drawing.Color.Linen;
            this.ucPointerMeter_main.DialBackground = System.Drawing.SystemColors.Window;
            this.ucPointerMeter_main.DialTextNegative = System.Drawing.Color.Red;
            this.ucPointerMeter_main.DialTextPositive = System.Drawing.Color.Black;
            this.ucPointerMeter_main.DialTextZero = System.Drawing.Color.DarkGreen;
            this.ucPointerMeter_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucPointerMeter_main.Led1ColorOff = System.Drawing.Color.Green;
            this.ucPointerMeter_main.Led1ColorOn = System.Drawing.Color.Lime;
            this.ucPointerMeter_main.Led1Count = 6;
            this.ucPointerMeter_main.Led2ColorOff = System.Drawing.Color.Olive;
            this.ucPointerMeter_main.Led2ColorOn = System.Drawing.Color.Yellow;
            this.ucPointerMeter_main.Led2Count = 6;
            this.ucPointerMeter_main.Led3ColorOff = System.Drawing.Color.Maroon;
            this.ucPointerMeter_main.Led3ColorOn = System.Drawing.Color.Red;
            this.ucPointerMeter_main.Led3Count = 4;
            this.ucPointerMeter_main.LedSize = new System.Drawing.Size(6, 14);
            this.ucPointerMeter_main.LedSpace = 3;
            this.ucPointerMeter_main.Location = new System.Drawing.Point(0, 46);
            this.ucPointerMeter_main.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.ucPointerMeter_main.Maximum = 100D;
            this.ucPointerMeter_main.MeterScale = Support.MeterScale.Analog;
            this.ucPointerMeter_main.Minimum = 0D;
            this.ucPointerMeter_main.Name = "ucPointerMeter_main";
            this.ucPointerMeter_main.NeedleColor = System.Drawing.Color.Black;
            this.ucPointerMeter_main.PeakHold = false;
            this.ucPointerMeter_main.Peakms = 200;
            this.ucPointerMeter_main.PeakNeedleColor = System.Drawing.Color.Red;
            this.ucPointerMeter_main.ShowDialOnly = false;
            this.ucPointerMeter_main.ShowLedPeak = false;
            this.ucPointerMeter_main.ShowTextInDial = true;
            this.ucPointerMeter_main.Size = new System.Drawing.Size(341, 201);
            this.ucPointerMeter_main.TabIndex = 4;
            this.ucPointerMeter_main.UseLedLight = false;
            this.ucPointerMeter_main.Value = 60D;
            this.ucPointerMeter_main.Click += new System.EventHandler(this.ucVuMeter_main_Click);
            // 
            // ucTextLabel_value
            // 
            this.ucTextLabel_value._BackColor = System.Drawing.Color.White;
            this.ucTextLabel_value._Font = new System.Drawing.Font("微軟正黑體", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ucTextLabel_value._ForeColor = System.Drawing.SystemColors.WindowText;
            this.ucTextLabel_value._Text = "";
            this.ucTextLabel_value._TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ucTextLabel_value.BackColor = System.Drawing.Color.Linen;
            this.ucTextLabel_value.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ucTextLabel_value.Font = new System.Drawing.Font("微軟正黑體", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ucTextLabel_value.Location = new System.Drawing.Point(0, 247);
            this.ucTextLabel_value.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.ucTextLabel_value.Name = "ucTextLabel_value";
            this.ucTextLabel_value.Size = new System.Drawing.Size(341, 37);
            this.ucTextLabel_value.TabIndex = 5;
            // 
            // ucAnalogMeter
            // 
            this._CtrlVisible = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Linen;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.ucPointerMeter_main);
            this.Controls.Add(this.ucTextLabel_value);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "ucAnalogMeter";
            this.Size = new System.Drawing.Size(341, 284);
            this.Controls.SetChildIndex(this.ucTextLabel_value, 0);
            this.Controls.SetChildIndex(this.ucPointerMeter_main, 0);
            this.ResumeLayout(false);

        }

        #endregion
        private ucPointerMeter ucPointerMeter_main;
        private userControl.ucTextLabel ucTextLabel_value;
    }
}
