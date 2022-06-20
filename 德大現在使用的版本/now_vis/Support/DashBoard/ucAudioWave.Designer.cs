namespace Support.DashBoard
{
    partial class ucAudioWave
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
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip_main = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem_輸出入設定 = new System.Windows.Forms.ToolStripMenuItem();
            this.啟動停止ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ucWaveFFT_main = new Support.DashBoard.ucWaveFFT();
            this.contextMenuStrip_main.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip_main
            // 
            this.contextMenuStrip_main.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip_main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_輸出入設定,
            this.啟動停止ToolStripMenuItem});
            this.contextMenuStrip_main.Name = "contextMenuStrip_main";
            this.contextMenuStrip_main.Size = new System.Drawing.Size(135, 48);
            // 
            // toolStripMenuItem_輸出入設定
            // 
            this.toolStripMenuItem_輸出入設定.Name = "toolStripMenuItem_輸出入設定";
            this.toolStripMenuItem_輸出入設定.Size = new System.Drawing.Size(134, 22);
            this.toolStripMenuItem_輸出入設定.Text = "輸出入設定";
            this.toolStripMenuItem_輸出入設定.Click += new System.EventHandler(this.toolStripMenuItem_輸出入設定_Click);
            // 
            // 啟動停止ToolStripMenuItem
            // 
            this.啟動停止ToolStripMenuItem.Name = "啟動停止ToolStripMenuItem";
            this.啟動停止ToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.啟動停止ToolStripMenuItem.Text = "啟動/停止";
            this.啟動停止ToolStripMenuItem.Click += new System.EventHandler(this.啟動停止ToolStripMenuItem_Click);
            // 
            // ucWaveFFT_main
            // 
            this.ucWaveFFT_main._CtrlVisible = false;
            this.ucWaveFFT_main._FFTBarCount = 32;
            this.ucWaveFFT_main._IsAutoScaleFFT = false;
            this.ucWaveFFT_main._IsAutoScaleWave = false;
            this.ucWaveFFT_main._Title = "音 頻 分 析";
            this.ucWaveFFT_main._Title_Align = System.Windows.Forms.HorizontalAlignment.Center;
            this.ucWaveFFT_main._Title_BackColor = System.Drawing.SystemColors.Control;
            this.ucWaveFFT_main._Title_Font = new System.Drawing.Font("微軟正黑體", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ucWaveFFT_main._Title_ForeColor = System.Drawing.SystemColors.WindowText;
            this.ucWaveFFT_main._Title_Height = 28;
            this.ucWaveFFT_main.BackColor = System.Drawing.SystemColors.Control;
            this.ucWaveFFT_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucWaveFFT_main.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ucWaveFFT_main.ForeColor = System.Drawing.SystemColors.Control;
            this.ucWaveFFT_main.Location = new System.Drawing.Point(0, 0);
            this.ucWaveFFT_main.Margin = new System.Windows.Forms.Padding(5);
            this.ucWaveFFT_main.Name = "ucWaveFFT_main";
            this.ucWaveFFT_main.Size = new System.Drawing.Size(461, 316);
            this.ucWaveFFT_main.TabIndex = 1;
            // 
            // ucAudioWave
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ucWaveFFT_main);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ucAudioWave";
            this.Size = new System.Drawing.Size(461, 316);
            this.SizeChanged += new System.EventHandler(this.panel_main_SizeChanged);
            this.contextMenuStrip_main.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_main;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_輸出入設定;
        private System.Windows.Forms.ToolStripMenuItem 啟動停止ToolStripMenuItem;
        private ucWaveFFT ucWaveFFT_main;
    }
}
