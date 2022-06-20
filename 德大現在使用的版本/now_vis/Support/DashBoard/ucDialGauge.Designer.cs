namespace Support.DashBoard
{
    partial class ucDialGauge
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
            System.Windows.Forms.AGaugeLabel aGaugeLabel1 = new System.Windows.Forms.AGaugeLabel();
            System.Windows.Forms.AGaugeLabel aGaugeLabel2 = new System.Windows.Forms.AGaugeLabel();
            System.Windows.Forms.AGaugeRange aGaugeRange1 = new System.Windows.Forms.AGaugeRange();
            System.Windows.Forms.AGaugeRange aGaugeRange2 = new System.Windows.Forms.AGaugeRange();
            System.Windows.Forms.AGaugeRange aGaugeRange3 = new System.Windows.Forms.AGaugeRange();
            this.panel_main = new System.Windows.Forms.Panel();
            this.aGauge_main = new System.Windows.Forms.AGauge();
            this.panel_main.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_main
            // 
            this.panel_main.BackColor = System.Drawing.Color.White;
            this.panel_main.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel_main.Controls.Add(this.aGauge_main);
            this.panel_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_main.Location = new System.Drawing.Point(0, 37);
            this.panel_main.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel_main.Name = "panel_main";
            this.panel_main.Size = new System.Drawing.Size(195, 152);
            this.panel_main.TabIndex = 2;
            // 
            // aGauge_main
            // 
            this.aGauge_main.BackColor = System.Drawing.Color.White;
            this.aGauge_main.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.aGauge_main.BaseArcColor = System.Drawing.Color.Black;
            this.aGauge_main.BaseArcRadius = 80;
            this.aGauge_main.BaseArcStart = 135;
            this.aGauge_main.BaseArcSweep = 270;
            this.aGauge_main.BaseArcWidth = 2;
            this.aGauge_main.Center = new System.Drawing.Point(100, 120);
            this.aGauge_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.aGauge_main.Font = new System.Drawing.Font("微軟正黑體", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.aGauge_main.GaugeAutoSize = true;
            aGaugeLabel1.Color = System.Drawing.SystemColors.ControlText;
            aGaugeLabel1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            aGaugeLabel1.Name = "GaugeLabel_Value";
            aGaugeLabel1.Position = new System.Drawing.Point(0, 54);
            aGaugeLabel1.Text = "0";
            aGaugeLabel2.Color = System.Drawing.SystemColors.WindowText;
            aGaugeLabel2.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            aGaugeLabel2.Name = "GaugeLabel_Unit";
            aGaugeLabel2.Position = new System.Drawing.Point(0, 84);
            aGaugeLabel2.Text = "";
            this.aGauge_main.GaugeLabels.Add(aGaugeLabel1);
            this.aGauge_main.GaugeLabels.Add(aGaugeLabel2);
            aGaugeRange1.Color = System.Drawing.Color.Aqua;
            aGaugeRange1.EndValue = 30F;
            aGaugeRange1.InnerRadius = 65;
            aGaugeRange1.InRange = false;
            aGaugeRange1.Name = "GaugeRange_Low";
            aGaugeRange1.OuterRadius = 80;
            aGaugeRange1.StartValue = 0F;
            aGaugeRange2.Color = System.Drawing.Color.LimeGreen;
            aGaugeRange2.EndValue = 80F;
            aGaugeRange2.InnerRadius = 65;
            aGaugeRange2.InRange = false;
            aGaugeRange2.Name = "GaugeRange_Mid";
            aGaugeRange2.OuterRadius = 80;
            aGaugeRange2.StartValue = 30F;
            aGaugeRange3.Color = System.Drawing.Color.Red;
            aGaugeRange3.EndValue = 100F;
            aGaugeRange3.InnerRadius = 65;
            aGaugeRange3.InRange = false;
            aGaugeRange3.Name = "GaugeRange_High";
            aGaugeRange3.OuterRadius = 80;
            aGaugeRange3.StartValue = 80F;
            this.aGauge_main.GaugeRanges.Add(aGaugeRange1);
            this.aGauge_main.GaugeRanges.Add(aGaugeRange2);
            this.aGauge_main.GaugeRanges.Add(aGaugeRange3);
            this.aGauge_main.Location = new System.Drawing.Point(0, 0);
            this.aGauge_main.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.aGauge_main.MaxValue = 100F;
            this.aGauge_main.MinValue = 0F;
            this.aGauge_main.Name = "aGauge_main";
            this.aGauge_main.NeedleColor1 = System.Windows.Forms.AGaugeNeedleColor.Blue;
            this.aGauge_main.NeedleColor2 = System.Drawing.Color.Silver;
            this.aGauge_main.NeedleRadius = 65;
            this.aGauge_main.NeedleType = System.Windows.Forms.NeedleType.Advance;
            this.aGauge_main.NeedleWidth = 6;
            this.aGauge_main.ScaleLinesInterColor = System.Drawing.Color.Black;
            this.aGauge_main.ScaleLinesInterInnerRadius = 70;
            this.aGauge_main.ScaleLinesInterOuterRadius = 80;
            this.aGauge_main.ScaleLinesInterWidth = 1;
            this.aGauge_main.ScaleLinesMajorColor = System.Drawing.Color.Black;
            this.aGauge_main.ScaleLinesMajorInnerRadius = 70;
            this.aGauge_main.ScaleLinesMajorOuterRadius = 85;
            this.aGauge_main.ScaleLinesMajorStepValue = 10F;
            this.aGauge_main.ScaleLinesMajorWidth = 1;
            this.aGauge_main.ScaleLinesMinorColor = System.Drawing.Color.Black;
            this.aGauge_main.ScaleLinesMinorInnerRadius = 73;
            this.aGauge_main.ScaleLinesMinorOuterRadius = 80;
            this.aGauge_main.ScaleLinesMinorTicks = 9;
            this.aGauge_main.ScaleLinesMinorWidth = 1;
            this.aGauge_main.ScaleNumbersColor = System.Drawing.Color.Black;
            this.aGauge_main.ScaleNumbersFormat = "";
            this.aGauge_main.ScaleNumbersRadius = 105;
            this.aGauge_main.ScaleNumbersRotation = 0;
            this.aGauge_main.ScaleNumbersStartScaleLine = 1;
            this.aGauge_main.ScaleNumbersStepScaleLines = 1;
            this.aGauge_main.Size = new System.Drawing.Size(191, 148);
            this.aGauge_main.TabIndex = 3;
            this.aGauge_main.Value = 0F;
            this.aGauge_main.Click += new System.EventHandler(this.aGauge1_Click);
            // 
            // ucDialGauge
            // 
            this._CtrlVisible = true;
            this._Title_Height = 37;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.panel_main);
            this.Name = "ucDialGauge";
            this.Size = new System.Drawing.Size(195, 189);
            this.Controls.SetChildIndex(this.panel_main, 0);
            this.panel_main.ResumeLayout(false);
            this.panel_main.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_main;
        private System.Windows.Forms.AGauge aGauge_main;
    }
}
