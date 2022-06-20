namespace Support.DashBoard
{
    partial class ucVisObj
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
            this.button_change = new System.Windows.Forms.Button();
            this.panel_ctrl = new System.Windows.Forms.Panel();
            this.panel_ctrl.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_change
            // 
            this.button_change.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_change.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.button_change.Location = new System.Drawing.Point(482, 4);
            this.button_change.Margin = new System.Windows.Forms.Padding(4);
            this.button_change.Name = "button_change";
            this.button_change.Size = new System.Drawing.Size(16, 16);
            this.button_change.TabIndex = 0;
            this.button_change.Text = "...";
            this.button_change.UseVisualStyleBackColor = false;
            this.button_change.Click += new System.EventHandler(this.button_change_Click);
            // 
            // panel_ctrl
            // 
            this.panel_ctrl.Controls.Add(this.button_change);
            this.panel_ctrl.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_ctrl.Location = new System.Drawing.Point(0, 0);
            this.panel_ctrl.Name = "panel_ctrl";
            this.panel_ctrl.Size = new System.Drawing.Size(504, 24);
            this.panel_ctrl.TabIndex = 1;
            this.panel_ctrl.Visible = false;
            // 
            // ucVisObj
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panel_ctrl);
            this.Font = new System.Drawing.Font("微軟正黑體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ucVisObj";
            this.Size = new System.Drawing.Size(504, 404);
            this.panel_ctrl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button_change;
        private System.Windows.Forms.Panel panel_ctrl;
    }
}
