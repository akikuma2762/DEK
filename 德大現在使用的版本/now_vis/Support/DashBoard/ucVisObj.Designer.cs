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
            this.textBox_Title = new System.Windows.Forms.TextBox();
            this.panel_buttons = new System.Windows.Forms.Panel();
            this.button_change_child = new System.Windows.Forms.Button();
            this.button_hide = new System.Windows.Forms.Button();
            this.panel_ctrl.SuspendLayout();
            this.panel_buttons.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_change
            // 
            this.button_change.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_change.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.button_change.Location = new System.Drawing.Point(35, 5);
            this.button_change.Margin = new System.Windows.Forms.Padding(5);
            this.button_change.Name = "button_change";
            this.button_change.Size = new System.Drawing.Size(18, 18);
            this.button_change.TabIndex = 0;
            this.button_change.Text = "...";
            this.button_change.UseVisualStyleBackColor = false;
            this.button_change.Click += new System.EventHandler(this.button_change_Click);
            // 
            // panel_ctrl
            // 
            this.panel_ctrl.Controls.Add(this.textBox_Title);
            this.panel_ctrl.Controls.Add(this.panel_buttons);
            this.panel_ctrl.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_ctrl.Location = new System.Drawing.Point(0, 0);
            this.panel_ctrl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel_ctrl.Name = "panel_ctrl";
            this.panel_ctrl.Size = new System.Drawing.Size(237, 35);
            this.panel_ctrl.TabIndex = 1;
            this.panel_ctrl.Visible = false;
            // 
            // textBox_Title
            // 
            this.textBox_Title.BackColor = System.Drawing.SystemColors.Control;
            this.textBox_Title.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox_Title.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Title.Location = new System.Drawing.Point(0, 0);
            this.textBox_Title.Multiline = true;
            this.textBox_Title.Name = "textBox_Title";
            this.textBox_Title.ReadOnly = true;
            this.textBox_Title.Size = new System.Drawing.Size(143, 35);
            this.textBox_Title.TabIndex = 1;
            this.textBox_Title.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // panel_buttons
            // 
            this.panel_buttons.BackColor = System.Drawing.Color.Transparent;
            this.panel_buttons.Controls.Add(this.button_change);
            this.panel_buttons.Controls.Add(this.button_change_child);
            this.panel_buttons.Controls.Add(this.button_hide);
            this.panel_buttons.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel_buttons.Location = new System.Drawing.Point(143, 0);
            this.panel_buttons.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel_buttons.Name = "panel_buttons";
            this.panel_buttons.Size = new System.Drawing.Size(94, 35);
            this.panel_buttons.TabIndex = 0;
            this.panel_buttons.TabStop = true;
            this.panel_buttons.Visible = false;
            // 
            // button_change_child
            // 
            this.button_change_child.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_change_child.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.button_change_child.Location = new System.Drawing.Point(6, 5);
            this.button_change_child.Margin = new System.Windows.Forms.Padding(5);
            this.button_change_child.Name = "button_change_child";
            this.button_change_child.Size = new System.Drawing.Size(18, 18);
            this.button_change_child.TabIndex = 1;
            this.button_change_child.Text = "...";
            this.button_change_child.UseVisualStyleBackColor = false;
            this.button_change_child.Click += new System.EventHandler(this.button_change_Click);
            // 
            // button_hide
            // 
            this.button_hide.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_hide.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.button_hide.Location = new System.Drawing.Point(64, 5);
            this.button_hide.Margin = new System.Windows.Forms.Padding(5);
            this.button_hide.Name = "button_hide";
            this.button_hide.Size = new System.Drawing.Size(18, 18);
            this.button_hide.TabIndex = 2;
            this.button_hide.Text = "...";
            this.button_hide.UseVisualStyleBackColor = false;
            this.button_hide.Click += new System.EventHandler(this.button_hide_Click);
            // 
            // ucVisObj
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.panel_ctrl);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ForeColor = System.Drawing.SystemColors.Control;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "ucVisObj";
            this.Size = new System.Drawing.Size(237, 184);
            this.panel_ctrl.ResumeLayout(false);
            this.panel_ctrl.PerformLayout();
            this.panel_buttons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button_change;
        private System.Windows.Forms.Panel panel_ctrl;
        private System.Windows.Forms.Button button_change_child;
        private System.Windows.Forms.Button button_hide;
        private System.Windows.Forms.Panel panel_buttons;
        private System.Windows.Forms.TextBox textBox_Title;
    }
}
