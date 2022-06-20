namespace Support.Forms
{
    partial class FormMessage
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox_Mesg = new System.Windows.Forms.TextBox();
            this.button_ok = new System.Windows.Forms.Button();
            this.panel_bottom = new System.Windows.Forms.Panel();
            this.panel_top = new System.Windows.Forms.Panel();
            this.panel_bottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox_Mesg
            // 
            this.textBox_Mesg.BackColor = System.Drawing.SystemColors.Control;
            this.textBox_Mesg.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox_Mesg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Mesg.Font = new System.Drawing.Font("Microsoft JhengHei", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox_Mesg.ForeColor = System.Drawing.SystemColors.WindowText;
            this.textBox_Mesg.Location = new System.Drawing.Point(0, 10);
            this.textBox_Mesg.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_Mesg.Multiline = true;
            this.textBox_Mesg.Name = "textBox_Mesg";
            this.textBox_Mesg.ReadOnly = true;
            this.textBox_Mesg.Size = new System.Drawing.Size(320, 72);
            this.textBox_Mesg.TabIndex = 0;
            this.textBox_Mesg.TabStop = false;
            this.textBox_Mesg.Text = "Test Text";
            this.textBox_Mesg.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // button_ok
            // 
            this.button_ok.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.button_ok.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button_ok.Location = new System.Drawing.Point(126, 12);
            this.button_ok.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(84, 37);
            this.button_ok.TabIndex = 0;
            this.button_ok.Text = "確定";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // panel_bottom
            // 
            this.panel_bottom.Controls.Add(this.button_ok);
            this.panel_bottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_bottom.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.panel_bottom.Location = new System.Drawing.Point(0, 82);
            this.panel_bottom.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.panel_bottom.Name = "panel_bottom";
            this.panel_bottom.Size = new System.Drawing.Size(320, 57);
            this.panel_bottom.TabIndex = 2;
            // 
            // panel_top
            // 
            this.panel_top.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_top.Location = new System.Drawing.Point(0, 0);
            this.panel_top.Name = "panel_top";
            this.panel_top.Size = new System.Drawing.Size(320, 10);
            this.panel_top.TabIndex = 3;
            // 
            // FormMessage
            // 
            this.AcceptButton = this.button_ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(320, 139);
            this.Controls.Add(this.textBox_Mesg);
            this.Controls.Add(this.panel_bottom);
            this.Controls.Add(this.panel_top);
            this.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMessage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "!! 請注意 !!";
            this.Load += new System.EventHandler(this.FormMessage_Load);
            this.panel_bottom.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_Mesg;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.Panel panel_bottom;
        private System.Windows.Forms.Panel panel_top;
    }
}