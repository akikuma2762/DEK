namespace Support.Forms
{
    partial class FormDataRowSelect
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
            this.button_確定 = new System.Windows.Forms.Button();
            this.button_取消 = new System.Windows.Forms.Button();
            this.ucDataRowFilter1 = new Support.userControl.ucDataRowFilter();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_確定
            // 
            this.button_確定.Location = new System.Drawing.Point(392, 8);
            this.button_確定.Name = "button_確定";
            this.button_確定.Size = new System.Drawing.Size(92, 38);
            this.button_確定.TabIndex = 1;
            this.button_確定.Text = "確定";
            this.button_確定.UseVisualStyleBackColor = true;
            this.button_確定.Click += new System.EventHandler(this.button_確定_Click);
            // 
            // button_取消
            // 
            this.button_取消.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_取消.Location = new System.Drawing.Point(219, 8);
            this.button_取消.Name = "button_取消";
            this.button_取消.Size = new System.Drawing.Size(92, 38);
            this.button_取消.TabIndex = 1;
            this.button_取消.Text = "取消";
            this.button_取消.UseVisualStyleBackColor = true;
            // 
            // ucDataRowFilter1
            // 
            this.ucDataRowFilter1.BackColor = System.Drawing.SystemColors.Control;
            this.ucDataRowFilter1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucDataRowFilter1.Font = new System.Drawing.Font("Microsoft JhengHei", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ucDataRowFilter1.Location = new System.Drawing.Point(0, 0);
            this.ucDataRowFilter1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ucDataRowFilter1.Name = "ucDataRowFilter1";
            this.ucDataRowFilter1.Size = new System.Drawing.Size(674, 388);
            this.ucDataRowFilter1.SQL_查詢字串 = "";
            this.ucDataRowFilter1.TabIndex = 2;
            this.ucDataRowFilter1.UserControlList = null;
            this.ucDataRowFilter1.每頁筆數 = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button_取消);
            this.panel1.Controls.Add(this.button_確定);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 388);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(674, 52);
            this.panel1.TabIndex = 3;
            // 
            // FormDataRowSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_取消;
            this.ClientSize = new System.Drawing.Size(674, 440);
            this.Controls.Add(this.ucDataRowFilter1);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FormDataRowSelect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "選取資料列";
            this.Load += new System.EventHandler(this.FormDataRowSelect_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button_確定;
        private System.Windows.Forms.Button button_取消;
        private userControl.ucDataRowFilter ucDataRowFilter1;
        private System.Windows.Forms.Panel panel1;
    }
}