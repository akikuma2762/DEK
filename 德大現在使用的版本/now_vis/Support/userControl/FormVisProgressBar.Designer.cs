namespace Support.userControl
{
    partial class FormVisProgressBar
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
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panel_Progressbar = new System.Windows.Forms.Panel();
            this.panel_Textbox = new System.Windows.Forms.Panel();
            this.panel_Progressbar.SuspendLayout();
            this.panel_Textbox.SuspendLayout();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(23, 4);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(119, 19);
            this.progressBar1.TabIndex = 0;
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(168, 114);
            this.textBox1.TabIndex = 2;
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // panel_Progressbar
            // 
            this.panel_Progressbar.BackColor = System.Drawing.SystemColors.Control;
            this.panel_Progressbar.Controls.Add(this.progressBar1);
            this.panel_Progressbar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_Progressbar.Location = new System.Drawing.Point(0, 114);
            this.panel_Progressbar.Name = "panel_Progressbar";
            this.panel_Progressbar.Size = new System.Drawing.Size(168, 28);
            this.panel_Progressbar.TabIndex = 3;
            // 
            // panel_Textbox
            // 
            this.panel_Textbox.Controls.Add(this.textBox1);
            this.panel_Textbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_Textbox.Location = new System.Drawing.Point(0, 0);
            this.panel_Textbox.Name = "panel_Textbox";
            this.panel_Textbox.Size = new System.Drawing.Size(168, 114);
            this.panel_Textbox.TabIndex = 4;
            // 
            // FormVisProgressBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(168, 142);
            this.Controls.Add(this.panel_Textbox);
            this.Controls.Add(this.panel_Progressbar);
            this.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FormVisProgressBar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.panel_Progressbar.ResumeLayout(false);
            this.panel_Textbox.ResumeLayout(false);
            this.panel_Textbox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Panel panel_Progressbar;
        private System.Windows.Forms.Panel panel_Textbox;
    }
}