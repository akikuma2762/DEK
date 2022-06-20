namespace Support.Forms
{
    partial class FormMessage_YesNo_Input
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label_提示文字 = new System.Windows.Forms.Label();
            this.numericUpDown_quantity = new System.Windows.Forms.NumericUpDown();
            this.label_content = new System.Windows.Forms.Label();
            this.button_確定 = new System.Windows.Forms.Button();
            this.button_取消 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_quantity)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.label_提示文字);
            this.panel1.Controls.Add(this.numericUpDown_quantity);
            this.panel1.Controls.Add(this.label_content);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(361, 233);
            this.panel1.TabIndex = 0;
            // 
            // label_提示文字
            // 
            this.label_提示文字.AutoSize = true;
            this.label_提示文字.Location = new System.Drawing.Point(20, 180);
            this.label_提示文字.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_提示文字.Name = "label_提示文字";
            this.label_提示文字.Size = new System.Drawing.Size(73, 20);
            this.label_提示文字.TabIndex = 2;
            this.label_提示文字.Text = "使用數量";
            // 
            // numericUpDown_quantity
            // 
            this.numericUpDown_quantity.Location = new System.Drawing.Point(108, 176);
            this.numericUpDown_quantity.Margin = new System.Windows.Forms.Padding(2);
            this.numericUpDown_quantity.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.numericUpDown_quantity.Name = "numericUpDown_quantity";
            this.numericUpDown_quantity.Size = new System.Drawing.Size(120, 29);
            this.numericUpDown_quantity.TabIndex = 1;
            // 
            // label_content
            // 
            this.label_content.AutoSize = true;
            this.label_content.Location = new System.Drawing.Point(16, 20);
            this.label_content.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_content.Name = "label_content";
            this.label_content.Size = new System.Drawing.Size(45, 20);
            this.label_content.TabIndex = 0;
            this.label_content.Text = "label";
            // 
            // button_確定
            // 
            this.button_確定.Location = new System.Drawing.Point(104, 241);
            this.button_確定.Margin = new System.Windows.Forms.Padding(2);
            this.button_確定.Name = "button_確定";
            this.button_確定.Size = new System.Drawing.Size(100, 31);
            this.button_確定.TabIndex = 1;
            this.button_確定.Text = "確定";
            this.button_確定.UseVisualStyleBackColor = true;
            this.button_確定.Click += new System.EventHandler(this.button_確定_Click);
            // 
            // button_取消
            // 
            this.button_取消.Location = new System.Drawing.Point(220, 241);
            this.button_取消.Margin = new System.Windows.Forms.Padding(2);
            this.button_取消.Name = "button_取消";
            this.button_取消.Size = new System.Drawing.Size(100, 31);
            this.button_取消.TabIndex = 2;
            this.button_取消.Text = "取消";
            this.button_取消.UseVisualStyleBackColor = true;
            this.button_取消.Click += new System.EventHandler(this.button_取消_Click);
            // 
            // FormMessage_YesNo_Input
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(361, 282);
            this.Controls.Add(this.button_取消);
            this.Controls.Add(this.button_確定);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Microsoft JhengHei", 12F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMessage_YesNo_Input";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormMessage_YesNo_TextBox";
            this.Load += new System.EventHandler(this.FormMessage_YesNo_Input_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_quantity)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label_content;
        private System.Windows.Forms.Button button_確定;
        private System.Windows.Forms.Button button_取消;
        private System.Windows.Forms.Label label_提示文字;
        private System.Windows.Forms.NumericUpDown numericUpDown_quantity;
    }
}