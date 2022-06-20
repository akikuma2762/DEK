namespace Support.Forms
{
    partial class FormDataFilter
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
            this.groupBox_FieldSelect = new System.Windows.Forms.GroupBox();
            this.button_選取欄位_反向 = new System.Windows.Forms.Button();
            this.button_選取欄位_清除 = new System.Windows.Forms.Button();
            this.button_選取欄位_全選 = new System.Windows.Forms.Button();
            this.checkedListBox_選取欄位 = new System.Windows.Forms.CheckedListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox_Search = new System.Windows.Forms.GroupBox();
            this.checkBox_遞減 = new System.Windows.Forms.CheckBox();
            this.textBox_查詢字串 = new System.Windows.Forms.TextBox();
            this.comboBox_排序欄位 = new System.Windows.Forms.ComboBox();
            this.comboBox_查詢欄位 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel_PageControl = new System.Windows.Forms.Panel();
            this.label_總頁數 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button_nav_next = new System.Windows.Forms.Button();
            this.numericUpDown_目前頁面 = new System.Windows.Forms.NumericUpDown();
            this.button_nav_prev = new System.Windows.Forms.Button();
            this.button_nav_end = new System.Windows.Forms.Button();
            this.button_nav_home = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.numericUpDown_每頁筆數 = new System.Windows.Forms.NumericUpDown();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button_確定 = new System.Windows.Forms.Button();
            this.button_取消 = new System.Windows.Forms.Button();
            this.groupBox_FieldSelect.SuspendLayout();
            this.groupBox_Search.SuspendLayout();
            this.panel_PageControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_目前頁面)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_每頁筆數)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox_FieldSelect
            // 
            this.groupBox_FieldSelect.BackColor = System.Drawing.SystemColors.Info;
            this.groupBox_FieldSelect.Controls.Add(this.button_選取欄位_反向);
            this.groupBox_FieldSelect.Controls.Add(this.button_選取欄位_清除);
            this.groupBox_FieldSelect.Controls.Add(this.button_選取欄位_全選);
            this.groupBox_FieldSelect.Controls.Add(this.checkedListBox_選取欄位);
            this.groupBox_FieldSelect.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox_FieldSelect.Location = new System.Drawing.Point(0, 0);
            this.groupBox_FieldSelect.Margin = new System.Windows.Forms.Padding(3, 8, 3, 8);
            this.groupBox_FieldSelect.Name = "groupBox_FieldSelect";
            this.groupBox_FieldSelect.Padding = new System.Windows.Forms.Padding(3, 8, 3, 8);
            this.groupBox_FieldSelect.Size = new System.Drawing.Size(344, 159);
            this.groupBox_FieldSelect.TabIndex = 33;
            this.groupBox_FieldSelect.TabStop = false;
            this.groupBox_FieldSelect.Text = "調整顯示欄位";
            // 
            // button_選取欄位_反向
            // 
            this.button_選取欄位_反向.Location = new System.Drawing.Point(260, 103);
            this.button_選取欄位_反向.Margin = new System.Windows.Forms.Padding(3, 8, 3, 8);
            this.button_選取欄位_反向.Name = "button_選取欄位_反向";
            this.button_選取欄位_反向.Size = new System.Drawing.Size(75, 38);
            this.button_選取欄位_反向.TabIndex = 6;
            this.button_選取欄位_反向.Text = "反向";
            this.button_選取欄位_反向.UseVisualStyleBackColor = true;
            this.button_選取欄位_反向.Click += new System.EventHandler(this.button_選取欄位_全選_Click);
            // 
            // button_選取欄位_清除
            // 
            this.button_選取欄位_清除.Location = new System.Drawing.Point(260, 63);
            this.button_選取欄位_清除.Margin = new System.Windows.Forms.Padding(3, 8, 3, 8);
            this.button_選取欄位_清除.Name = "button_選取欄位_清除";
            this.button_選取欄位_清除.Size = new System.Drawing.Size(75, 38);
            this.button_選取欄位_清除.TabIndex = 6;
            this.button_選取欄位_清除.Text = "清除";
            this.button_選取欄位_清除.UseVisualStyleBackColor = true;
            this.button_選取欄位_清除.Click += new System.EventHandler(this.button_選取欄位_全選_Click);
            // 
            // button_選取欄位_全選
            // 
            this.button_選取欄位_全選.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button_選取欄位_全選.Location = new System.Drawing.Point(260, 22);
            this.button_選取欄位_全選.Margin = new System.Windows.Forms.Padding(3, 8, 3, 8);
            this.button_選取欄位_全選.Name = "button_選取欄位_全選";
            this.button_選取欄位_全選.Size = new System.Drawing.Size(75, 38);
            this.button_選取欄位_全選.TabIndex = 5;
            this.button_選取欄位_全選.Text = "全選";
            this.button_選取欄位_全選.UseVisualStyleBackColor = true;
            this.button_選取欄位_全選.Click += new System.EventHandler(this.button_選取欄位_全選_Click);
            // 
            // checkedListBox_選取欄位
            // 
            this.checkedListBox_選取欄位.Dock = System.Windows.Forms.DockStyle.Left;
            this.checkedListBox_選取欄位.Font = new System.Drawing.Font("Microsoft JhengHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.checkedListBox_選取欄位.FormattingEnabled = true;
            this.checkedListBox_選取欄位.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.checkedListBox_選取欄位.Location = new System.Drawing.Point(3, 30);
            this.checkedListBox_選取欄位.Margin = new System.Windows.Forms.Padding(3, 8, 3, 8);
            this.checkedListBox_選取欄位.Name = "checkedListBox_選取欄位";
            this.checkedListBox_選取欄位.Size = new System.Drawing.Size(247, 121);
            this.checkedListBox_選取欄位.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 28);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 20);
            this.label5.TabIndex = 1;
            this.label5.Text = "排序欄位";
            // 
            // groupBox_Search
            // 
            this.groupBox_Search.BackColor = System.Drawing.Color.Wheat;
            this.groupBox_Search.Controls.Add(this.checkBox_遞減);
            this.groupBox_Search.Controls.Add(this.textBox_查詢字串);
            this.groupBox_Search.Controls.Add(this.comboBox_排序欄位);
            this.groupBox_Search.Controls.Add(this.comboBox_查詢欄位);
            this.groupBox_Search.Controls.Add(this.label5);
            this.groupBox_Search.Controls.Add(this.label3);
            this.groupBox_Search.Controls.Add(this.label2);
            this.groupBox_Search.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox_Search.Location = new System.Drawing.Point(0, 159);
            this.groupBox_Search.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.groupBox_Search.Name = "groupBox_Search";
            this.groupBox_Search.Padding = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.groupBox_Search.Size = new System.Drawing.Size(341, 139);
            this.groupBox_Search.TabIndex = 34;
            this.groupBox_Search.TabStop = false;
            this.groupBox_Search.Text = "搜尋條件";
            // 
            // checkBox_遞減
            // 
            this.checkBox_遞減.AutoSize = true;
            this.checkBox_遞減.Location = new System.Drawing.Point(267, 23);
            this.checkBox_遞減.Name = "checkBox_遞減";
            this.checkBox_遞減.Size = new System.Drawing.Size(60, 24);
            this.checkBox_遞減.TabIndex = 20;
            this.checkBox_遞減.Text = "遞減";
            this.checkBox_遞減.UseVisualStyleBackColor = true;
            // 
            // textBox_查詢字串
            // 
            this.textBox_查詢字串.Location = new System.Drawing.Point(104, 100);
            this.textBox_查詢字串.Name = "textBox_查詢字串";
            this.textBox_查詢字串.Size = new System.Drawing.Size(224, 29);
            this.textBox_查詢字串.TabIndex = 19;
            // 
            // comboBox_排序欄位
            // 
            this.comboBox_排序欄位.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_排序欄位.FormattingEnabled = true;
            this.comboBox_排序欄位.Location = new System.Drawing.Point(104, 25);
            this.comboBox_排序欄位.Name = "comboBox_排序欄位";
            this.comboBox_排序欄位.Size = new System.Drawing.Size(134, 28);
            this.comboBox_排序欄位.TabIndex = 5;
            // 
            // comboBox_查詢欄位
            // 
            this.comboBox_查詢欄位.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_查詢欄位.FormattingEnabled = true;
            this.comboBox_查詢欄位.Location = new System.Drawing.Point(104, 64);
            this.comboBox_查詢欄位.Name = "comboBox_查詢欄位";
            this.comboBox_查詢欄位.Size = new System.Drawing.Size(224, 28);
            this.comboBox_查詢欄位.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "查詢字串";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "查詢欄位";
            // 
            // panel_PageControl
            // 
            this.panel_PageControl.BackColor = System.Drawing.Color.LightSalmon;
            this.panel_PageControl.Controls.Add(this.label_總頁數);
            this.panel_PageControl.Controls.Add(this.label1);
            this.panel_PageControl.Controls.Add(this.button_nav_next);
            this.panel_PageControl.Controls.Add(this.numericUpDown_目前頁面);
            this.panel_PageControl.Controls.Add(this.button_nav_prev);
            this.panel_PageControl.Controls.Add(this.button_nav_end);
            this.panel_PageControl.Controls.Add(this.button_nav_home);
            this.panel_PageControl.Controls.Add(this.label6);
            this.panel_PageControl.Controls.Add(this.numericUpDown_每頁筆數);
            this.panel_PageControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_PageControl.Location = new System.Drawing.Point(344, 0);
            this.panel_PageControl.Name = "panel_PageControl";
            this.panel_PageControl.Size = new System.Drawing.Size(273, 159);
            this.panel_PageControl.TabIndex = 36;
            // 
            // label_總頁數
            // 
            this.label_總頁數.AutoSize = true;
            this.label_總頁數.Location = new System.Drawing.Point(5, 85);
            this.label_總頁數.Name = "label_總頁數";
            this.label_總頁數.Size = new System.Drawing.Size(57, 20);
            this.label_總頁數.TabIndex = 35;
            this.label_總頁數.Text = "共xx頁";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 20);
            this.label1.TabIndex = 36;
            this.label1.Text = "目前頁面";
            // 
            // button_nav_next
            // 
            this.button_nav_next.Location = new System.Drawing.Point(148, 118);
            this.button_nav_next.Name = "button_nav_next";
            this.button_nav_next.Size = new System.Drawing.Size(41, 32);
            this.button_nav_next.TabIndex = 29;
            this.button_nav_next.Text = ">";
            this.button_nav_next.UseVisualStyleBackColor = true;
            this.button_nav_next.Click += new System.EventHandler(this.button_nav_home_Click);
            // 
            // numericUpDown_目前頁面
            // 
            this.numericUpDown_目前頁面.Location = new System.Drawing.Point(101, 48);
            this.numericUpDown_目前頁面.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown_目前頁面.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_目前頁面.Name = "numericUpDown_目前頁面";
            this.numericUpDown_目前頁面.Size = new System.Drawing.Size(98, 29);
            this.numericUpDown_目前頁面.TabIndex = 34;
            this.numericUpDown_目前頁面.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDown_目前頁面.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // button_nav_prev
            // 
            this.button_nav_prev.Location = new System.Drawing.Point(93, 118);
            this.button_nav_prev.Name = "button_nav_prev";
            this.button_nav_prev.Size = new System.Drawing.Size(41, 32);
            this.button_nav_prev.TabIndex = 30;
            this.button_nav_prev.Text = "<";
            this.button_nav_prev.UseVisualStyleBackColor = true;
            this.button_nav_prev.Click += new System.EventHandler(this.button_nav_home_Click);
            // 
            // button_nav_end
            // 
            this.button_nav_end.Location = new System.Drawing.Point(203, 118);
            this.button_nav_end.Name = "button_nav_end";
            this.button_nav_end.Size = new System.Drawing.Size(41, 32);
            this.button_nav_end.TabIndex = 28;
            this.button_nav_end.Text = ">|";
            this.button_nav_end.UseVisualStyleBackColor = true;
            this.button_nav_end.Click += new System.EventHandler(this.button_nav_home_Click);
            // 
            // button_nav_home
            // 
            this.button_nav_home.Location = new System.Drawing.Point(37, 118);
            this.button_nav_home.Name = "button_nav_home";
            this.button_nav_home.Size = new System.Drawing.Size(41, 32);
            this.button_nav_home.TabIndex = 31;
            this.button_nav_home.Text = "|<";
            this.button_nav_home.UseVisualStyleBackColor = true;
            this.button_nav_home.Click += new System.EventHandler(this.button_nav_home_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 20);
            this.label6.TabIndex = 32;
            this.label6.Text = "每頁筆數";
            // 
            // numericUpDown_每頁筆數
            // 
            this.numericUpDown_每頁筆數.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown_每頁筆數.Location = new System.Drawing.Point(102, 5);
            this.numericUpDown_每頁筆數.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown_每頁筆數.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_每頁筆數.Name = "numericUpDown_每頁筆數";
            this.numericUpDown_每頁筆數.Size = new System.Drawing.Size(97, 29);
            this.numericUpDown_每頁筆數.TabIndex = 33;
            this.numericUpDown_每頁筆數.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDown_每頁筆數.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown_每頁筆數.ValueChanged += new System.EventHandler(this.numericUpDown_每頁筆數_ValueChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel_PageControl);
            this.panel1.Controls.Add(this.groupBox_FieldSelect);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(617, 159);
            this.panel1.TabIndex = 37;
            // 
            // button_確定
            // 
            this.button_確定.Location = new System.Drawing.Point(501, 221);
            this.button_確定.Name = "button_確定";
            this.button_確定.Size = new System.Drawing.Size(71, 35);
            this.button_確定.TabIndex = 38;
            this.button_確定.Text = "確定";
            this.button_確定.UseVisualStyleBackColor = true;
            this.button_確定.Click += new System.EventHandler(this.button_確定_Click);
            // 
            // button_取消
            // 
            this.button_取消.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_取消.Location = new System.Drawing.Point(381, 221);
            this.button_取消.Name = "button_取消";
            this.button_取消.Size = new System.Drawing.Size(71, 35);
            this.button_取消.TabIndex = 38;
            this.button_取消.Text = "取消";
            this.button_取消.UseVisualStyleBackColor = true;
            // 
            // FormDataFilter
            // 
            this.AcceptButton = this.button_確定;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_取消;
            this.ClientSize = new System.Drawing.Size(617, 298);
            this.Controls.Add(this.button_取消);
            this.Controls.Add(this.button_確定);
            this.Controls.Add(this.groupBox_Search);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FormDataFilter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "資料過濾器";
            this.Load += new System.EventHandler(this.FormDataFilter_Load);
            this.groupBox_FieldSelect.ResumeLayout(false);
            this.groupBox_Search.ResumeLayout(false);
            this.groupBox_Search.PerformLayout();
            this.panel_PageControl.ResumeLayout(false);
            this.panel_PageControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_目前頁面)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_每頁筆數)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox_FieldSelect;
        private System.Windows.Forms.Button button_選取欄位_反向;
        private System.Windows.Forms.Button button_選取欄位_清除;
        private System.Windows.Forms.Button button_選取欄位_全選;
        private System.Windows.Forms.CheckedListBox checkedListBox_選取欄位;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox_Search;
        private System.Windows.Forms.CheckBox checkBox_遞減;
        private System.Windows.Forms.TextBox textBox_查詢字串;
        private System.Windows.Forms.ComboBox comboBox_排序欄位;
        private System.Windows.Forms.ComboBox comboBox_查詢欄位;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel_PageControl;
        private System.Windows.Forms.Label label_總頁數;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_nav_next;
        private System.Windows.Forms.NumericUpDown numericUpDown_目前頁面;
        private System.Windows.Forms.Button button_nav_prev;
        private System.Windows.Forms.Button button_nav_end;
        private System.Windows.Forms.Button button_nav_home;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericUpDown_每頁筆數;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button_確定;
        private System.Windows.Forms.Button button_取消;
    }
}