namespace Support.DashBoard
{
    partial class ucWaveFFT
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.ucPanel3D_WaveLeft = new Support.userControl.ucPanel3D();
            this.panel_Wave = new System.Windows.Forms.Panel();
            this.chart_TimeDomain = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panel_Freq = new System.Windows.Forms.Panel();
            this.chart_FreqDomain = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.ucPanel3D_Freq = new Support.userControl.ucPanel3D();
            this.panel_main = new System.Windows.Forms.Panel();
            this.ucPanel3D_WaveLeft.SuspendLayout();
            this.panel_Wave.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart_TimeDomain)).BeginInit();
            this.panel_Freq.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart_FreqDomain)).BeginInit();
            this.ucPanel3D_Freq.SuspendLayout();
            this.panel_main.SuspendLayout();
            this.SuspendLayout();
            // 
            // ucPanel3D_WaveLeft
            // 
            this.ucPanel3D_WaveLeft._BackColor = System.Drawing.SystemColors.Control;
            this.ucPanel3D_WaveLeft._Border_ColorBottomRight = System.Drawing.Color.Gray;
            this.ucPanel3D_WaveLeft._Border_ColorTopLeft = System.Drawing.Color.LightGray;
            this.ucPanel3D_WaveLeft._Border_LineWidth = 3;
            this.ucPanel3D_WaveLeft._Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            // 
            // ucPanel3D_WaveLeft._Panel
            // 
            this.ucPanel3D_WaveLeft._Panel.BackColor = System.Drawing.SystemColors.Control;
            this.ucPanel3D_WaveLeft._Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucPanel3D_WaveLeft._Panel.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ucPanel3D_WaveLeft._Panel.ForeColor = System.Drawing.Color.Black;
            this.ucPanel3D_WaveLeft._Panel.Location = new System.Drawing.Point(2, 2);
            this.ucPanel3D_WaveLeft._Panel.Margin = new System.Windows.Forms.Padding(0);
            this.ucPanel3D_WaveLeft._Panel.Name = "_Panel";
            this.ucPanel3D_WaveLeft._Panel.Size = new System.Drawing.Size(39, 122);
            this.ucPanel3D_WaveLeft._Panel.TabIndex = 0;
            this.ucPanel3D_WaveLeft._Panel.Click += new System.EventHandler(this.ucPanel3D1__Panel_Click);
            this.ucPanel3D_WaveLeft._Text = "時域";
            this.ucPanel3D_WaveLeft._TextColor = System.Drawing.Color.Black;
            this.ucPanel3D_WaveLeft._TextOrientation = System.Windows.Forms.Orientation.Vertical;
            this.ucPanel3D_WaveLeft.BackColor = System.Drawing.SystemColors.Control;
            this.ucPanel3D_WaveLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.ucPanel3D_WaveLeft.Location = new System.Drawing.Point(0, 0);
            this.ucPanel3D_WaveLeft.Margin = new System.Windows.Forms.Padding(0);
            this.ucPanel3D_WaveLeft.Name = "ucPanel3D_WaveLeft";
            this.ucPanel3D_WaveLeft.Padding = new System.Windows.Forms.Padding(2);
            this.ucPanel3D_WaveLeft.Size = new System.Drawing.Size(43, 126);
            this.ucPanel3D_WaveLeft.TabIndex = 0;
            // 
            // panel_Wave
            // 
            this.panel_Wave.Controls.Add(this.chart_TimeDomain);
            this.panel_Wave.Controls.Add(this.ucPanel3D_WaveLeft);
            this.panel_Wave.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_Wave.Location = new System.Drawing.Point(0, 0);
            this.panel_Wave.Name = "panel_Wave";
            this.panel_Wave.Size = new System.Drawing.Size(448, 126);
            this.panel_Wave.TabIndex = 2;
            // 
            // chart_TimeDomain
            // 
            this.chart_TimeDomain.BackColor = System.Drawing.Color.SteelBlue;
            this.chart_TimeDomain.BorderSkin.BackColor = System.Drawing.Color.White;
            this.chart_TimeDomain.BorderSkin.PageColor = System.Drawing.Color.Transparent;
            this.chart_TimeDomain.BorderSkin.SkinStyle = System.Windows.Forms.DataVisualization.Charting.BorderSkinStyle.Raised;
            chartArea1.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea1.AxisX.InterlacedColor = System.Drawing.Color.Black;
            chartArea1.AxisX.LabelStyle.Enabled = false;
            chartArea1.AxisX2.MajorGrid.Enabled = false;
            chartArea1.AxisX2.MajorTickMark.Enabled = false;
            chartArea1.AxisY.LabelStyle.Enabled = false;
            chartArea1.BackColor = System.Drawing.Color.SteelBlue;
            chartArea1.Name = "ChartArea";
            this.chart_TimeDomain.ChartAreas.Add(chartArea1);
            this.chart_TimeDomain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart_TimeDomain.Location = new System.Drawing.Point(43, 0);
            this.chart_TimeDomain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chart_TimeDomain.Name = "chart_TimeDomain";
            series1.ChartArea = "ChartArea";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series1.Color = System.Drawing.Color.White;
            series1.Name = "Series1";
            this.chart_TimeDomain.Series.Add(series1);
            this.chart_TimeDomain.Size = new System.Drawing.Size(405, 126);
            this.chart_TimeDomain.TabIndex = 3;
            this.chart_TimeDomain.Text = "chart1";
            this.chart_TimeDomain.Click += new System.EventHandler(this.ucPanel3D1__Panel_Click);
            // 
            // panel_Freq
            // 
            this.panel_Freq.Controls.Add(this.chart_FreqDomain);
            this.panel_Freq.Controls.Add(this.ucPanel3D_Freq);
            this.panel_Freq.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_Freq.Location = new System.Drawing.Point(0, 126);
            this.panel_Freq.Name = "panel_Freq";
            this.panel_Freq.Size = new System.Drawing.Size(448, 142);
            this.panel_Freq.TabIndex = 3;
            // 
            // chart_FreqDomain
            // 
            this.chart_FreqDomain.BackColor = System.Drawing.Color.Wheat;
            this.chart_FreqDomain.BorderSkin.PageColor = System.Drawing.Color.Transparent;
            this.chart_FreqDomain.BorderSkin.SkinStyle = System.Windows.Forms.DataVisualization.Charting.BorderSkinStyle.Raised;
            chartArea2.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea2.AxisX.InterlacedColor = System.Drawing.Color.Black;
            chartArea2.AxisX.LabelStyle.Enabled = false;
            chartArea2.AxisX2.MajorGrid.Enabled = false;
            chartArea2.AxisX2.MajorTickMark.Enabled = false;
            chartArea2.AxisY.LabelStyle.Enabled = false;
            chartArea2.BackColor = System.Drawing.Color.Wheat;
            chartArea2.Name = "ChartArea";
            this.chart_FreqDomain.ChartAreas.Add(chartArea2);
            this.chart_FreqDomain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart_FreqDomain.Location = new System.Drawing.Point(43, 0);
            this.chart_FreqDomain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chart_FreqDomain.Name = "chart_FreqDomain";
            series2.ChartArea = "ChartArea";
            series2.Color = System.Drawing.Color.Blue;
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chart_FreqDomain.Series.Add(series2);
            this.chart_FreqDomain.Size = new System.Drawing.Size(405, 142);
            this.chart_FreqDomain.TabIndex = 3;
            this.chart_FreqDomain.Click += new System.EventHandler(this.ucPanel3D1__Panel_Click);
            // 
            // ucPanel3D_Freq
            // 
            this.ucPanel3D_Freq._BackColor = System.Drawing.SystemColors.Control;
            this.ucPanel3D_Freq._Border_ColorBottomRight = System.Drawing.Color.Gray;
            this.ucPanel3D_Freq._Border_ColorTopLeft = System.Drawing.Color.LightGray;
            this.ucPanel3D_Freq._Border_LineWidth = 3;
            this.ucPanel3D_Freq._Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            // 
            // ucPanel3D_Freq._Panel
            // 
            this.ucPanel3D_Freq._Panel.BackColor = System.Drawing.SystemColors.Control;
            this.ucPanel3D_Freq._Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucPanel3D_Freq._Panel.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ucPanel3D_Freq._Panel.ForeColor = System.Drawing.Color.Black;
            this.ucPanel3D_Freq._Panel.Location = new System.Drawing.Point(3, 3);
            this.ucPanel3D_Freq._Panel.Margin = new System.Windows.Forms.Padding(0);
            this.ucPanel3D_Freq._Panel.Name = "_Panel";
            this.ucPanel3D_Freq._Panel.Size = new System.Drawing.Size(37, 136);
            this.ucPanel3D_Freq._Panel.TabIndex = 0;
            this.ucPanel3D_Freq._Panel.Click += new System.EventHandler(this.ucPanel3D1__Panel_Click);
            this.ucPanel3D_Freq._Text = "頻域";
            this.ucPanel3D_Freq._TextColor = System.Drawing.Color.Black;
            this.ucPanel3D_Freq._TextOrientation = System.Windows.Forms.Orientation.Vertical;
            this.ucPanel3D_Freq.BackColor = System.Drawing.SystemColors.Control;
            this.ucPanel3D_Freq.Dock = System.Windows.Forms.DockStyle.Left;
            this.ucPanel3D_Freq.Location = new System.Drawing.Point(0, 0);
            this.ucPanel3D_Freq.Margin = new System.Windows.Forms.Padding(0);
            this.ucPanel3D_Freq.Name = "ucPanel3D_Freq";
            this.ucPanel3D_Freq.Padding = new System.Windows.Forms.Padding(3);
            this.ucPanel3D_Freq.Size = new System.Drawing.Size(43, 142);
            this.ucPanel3D_Freq.TabIndex = 0;
            // 
            // panel_main
            // 
            this.panel_main.Controls.Add(this.panel_Freq);
            this.panel_main.Controls.Add(this.panel_Wave);
            this.panel_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_main.Location = new System.Drawing.Point(0, 32);
            this.panel_main.Name = "panel_main";
            this.panel_main.Size = new System.Drawing.Size(448, 268);
            this.panel_main.TabIndex = 4;
            // 
            // ucWaveFFT
            // 
            this._CtrlVisible = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel_main);
            this.Name = "ucWaveFFT";
            this.Size = new System.Drawing.Size(448, 300);
            this.SizeChanged += new System.EventHandler(this.ucWaveFFT_SizeChanged);
            this.Controls.SetChildIndex(this.panel_main, 0);
            this.ucPanel3D_WaveLeft.ResumeLayout(false);
            this.panel_Wave.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart_TimeDomain)).EndInit();
            this.panel_Freq.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart_FreqDomain)).EndInit();
            this.ucPanel3D_Freq.ResumeLayout(false);
            this.panel_main.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private userControl.ucPanel3D ucPanel3D_WaveLeft;
        private System.Windows.Forms.Panel panel_Wave;
        private System.Windows.Forms.Panel panel_Freq;
        private userControl.ucPanel3D ucPanel3D_Freq;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_TimeDomain;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_FreqDomain;
        private System.Windows.Forms.Panel panel_main;
    }
}
