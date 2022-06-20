namespace Support.userControl
{
    partial class ucDataGridView
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.contextMenuStrip_main = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.資料控制板ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.變更字型ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox_main = new System.Windows.Forms.GroupBox();
            this.dataGridView_main = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip_main.SuspendLayout();
            this.groupBox_main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_main)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip_main
            // 
            this.contextMenuStrip_main.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.contextMenuStrip_main.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip_main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.資料控制板ToolStripMenuItem,
            this.變更字型ToolStripMenuItem});
            this.contextMenuStrip_main.Name = "contextMenuStrip_main";
            this.contextMenuStrip_main.Size = new System.Drawing.Size(185, 64);
            this.contextMenuStrip_main.Text = "資料控制板";
            // 
            // 資料控制板ToolStripMenuItem
            // 
            this.資料控制板ToolStripMenuItem.Name = "資料控制板ToolStripMenuItem";
            this.資料控制板ToolStripMenuItem.Size = new System.Drawing.Size(184, 30);
            this.資料控制板ToolStripMenuItem.Text = "資料控制板";
            this.資料控制板ToolStripMenuItem.Click += new System.EventHandler(this.資料控制板ToolStripMenuItem_Click);
            // 
            // 變更字型ToolStripMenuItem
            // 
            this.變更字型ToolStripMenuItem.Name = "變更字型ToolStripMenuItem";
            this.變更字型ToolStripMenuItem.Size = new System.Drawing.Size(184, 30);
            this.變更字型ToolStripMenuItem.Text = "變更字型";
            this.變更字型ToolStripMenuItem.Click += new System.EventHandler(this.變更字型ToolStripMenuItem_Click);
            // 
            // groupBox_main
            // 
            this.groupBox_main.Controls.Add(this.dataGridView_main);
            this.groupBox_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_main.Font = new System.Drawing.Font("微軟正黑體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBox_main.Location = new System.Drawing.Point(0, 0);
            this.groupBox_main.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox_main.Name = "groupBox_main";
            this.groupBox_main.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox_main.Size = new System.Drawing.Size(448, 275);
            this.groupBox_main.TabIndex = 1;
            this.groupBox_main.TabStop = false;
            this.groupBox_main.Text = "資料內容";
            // 
            // dataGridView_main
            // 
            this.dataGridView_main.AllowUserToAddRows = false;
            this.dataGridView_main.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微軟正黑體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_main.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView_main.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_main.Location = new System.Drawing.Point(4, 28);
            this.dataGridView_main.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dataGridView_main.MultiSelect = false;
            this.dataGridView_main.Name = "dataGridView_main";
            this.dataGridView_main.ReadOnly = true;
            this.dataGridView_main.RowTemplate.Height = 27;
            this.dataGridView_main.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_main.Size = new System.Drawing.Size(440, 242);
            this.dataGridView_main.TabIndex = 1;
            this.dataGridView_main.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_main_RowEnter);
            this.dataGridView_main.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridView_main_MouseDown);
            // 
            // ucDataGridView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox_main);
            this.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ucDataGridView";
            this.Size = new System.Drawing.Size(448, 275);
            this.contextMenuStrip_main.ResumeLayout(false);
            this.groupBox_main.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_main)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_main;
        private System.Windows.Forms.ToolStripMenuItem 資料控制板ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 變更字型ToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox_main;
        private System.Windows.Forms.DataGridView dataGridView_main;
    }
}
