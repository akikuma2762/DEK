namespace Support.userControl
{
    partial class ucDataRowFilter
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
            this.panel_filter = new System.Windows.Forms.Panel();
            this.panel_PageControl = new System.Windows.Forms.Panel();
            this.label_總頁數 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button_nav_next = new System.Windows.Forms.Button();
            this.numericUpDown_目前頁面 = new System.Windows.Forms.NumericUpDown();
            this.button_nav_prev = new System.Windows.Forms.Button();
            this.button_nav_end = new System.Windows.Forms.Button();
            this.button_nav_home = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDown_每頁筆數 = new System.Windows.Forms.NumericUpDown();
            this.groupBox_Search = new System.Windows.Forms.GroupBox();
            this.checkBox_遞減 = new System.Windows.Forms.CheckBox();
            this.textBox_查詢字串 = new System.Windows.Forms.TextBox();
            this.comboBox_排序欄位 = new System.Windows.Forms.ComboBox();
            this.comboBox_查詢欄位 = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox_FieldSelect = new System.Windows.Forms.GroupBox();
            this.button_選取欄位_反向 = new System.Windows.Forms.Button();
            this.button_選取欄位_清除 = new System.Windows.Forms.Button();
            this.button_選取欄位_全選 = new System.Windows.Forms.Button();
            this.checkedListBox_選取欄位 = new System.Windows.Forms.CheckedListBox();
            this.groupBox_dataGridView = new System.Windows.Forms.GroupBox();
            this.dataGridView_Data = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip_main = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.資料控制板顯示隱藏ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.字型ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel_filter.SuspendLayout();
            this.panel_PageControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_目前頁面)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_每頁筆數)).BeginInit();
            this.groupBox_Search.SuspendLayout();
            this.groupBox_FieldSelect.SuspendLayout();
            this.groupBox_dataGridView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Data)).BeginInit();
            this.contextMenuStrip_main.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_filter
            // 
            this.panel_filter.BackColor = System.Drawing.SystemColors.Control;
            this.panel_filter.Controls.Add(this.panel_PageControl);
            this.panel_filter.Controls.Add(this.groupBox_Search);
            this.panel_filter.Controls.Add(this.groupBox_FieldSelect);
            this.panel_filter.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_filter.Location = new System.Drawing.Point(0, 0);
            this.panel_filter.Margin = new System.Windows.Forms.Padding(2);
            this.panel_filter.Name = "panel_filter";
            this.panel_filter.Size = new System.Drawing.Size(671, 131);
            this.panel_filter.TabIndex = 28;
            this.panel_filter.Visible = false;
            this.panel_filter.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ucDataRowFilter_MouseDown);
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
            this.panel_PageControl.Controls.Add(this.label4);
            this.panel_PageControl.Controls.Add(this.numericUpDown_每頁筆數);
            this.panel_PageControl.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel_PageControl.Location = new System.Drawing.Point(488, 0);
            this.panel_PageControl.Margin = new System.Windows.Forms.Padding(2);
            this.panel_PageControl.Name = "panel_PageControl";
            this.panel_PageControl.Size = new System.Drawing.Size(182, 131);
            this.panel_PageControl.TabIndex = 17;
            this.panel_PageControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ucDataRowFilter_MouseDown);
            // 
            // label_總頁數
            // 
            this.label_總頁數.AutoSize = true;
            this.label_總頁數.Location = new System.Drawing.Point(3, 34);
            this.label_總頁數.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_總頁數.Name = "label_總頁數";
            this.label_總頁數.Size = new System.Drawing.Size(50, 18);
            this.label_總頁數.TabIndex = 35;
            this.label_總頁數.Text = "共xx頁";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 59);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 18);
            this.label1.TabIndex = 36;
            this.label1.Text = "目前頁面";
            // 
            // button_nav_next
            // 
            this.button_nav_next.Location = new System.Drawing.Point(90, 91);
            this.button_nav_next.Margin = new System.Windows.Forms.Padding(2);
            this.button_nav_next.Name = "button_nav_next";
            this.button_nav_next.Size = new System.Drawing.Size(37, 31);
            this.button_nav_next.TabIndex = 29;
            this.button_nav_next.Text = ">";
            this.button_nav_next.UseVisualStyleBackColor = true;
            this.button_nav_next.Click += new System.EventHandler(this.button_nav_home_Click);
            // 
            // numericUpDown_目前頁面
            // 
            this.numericUpDown_目前頁面.Location = new System.Drawing.Point(87, 57);
            this.numericUpDown_目前頁面.Margin = new System.Windows.Forms.Padding(2);
            this.numericUpDown_目前頁面.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_目前頁面.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_目前頁面.Name = "numericUpDown_目前頁面";
            this.numericUpDown_目前頁面.Size = new System.Drawing.Size(57, 26);
            this.numericUpDown_目前頁面.TabIndex = 34;
            this.numericUpDown_目前頁面.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDown_目前頁面.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_目前頁面.ValueChanged += new System.EventHandler(this.numericUpDown_目前頁面_ValueChanged);
            // 
            // button_nav_prev
            // 
            this.button_nav_prev.Location = new System.Drawing.Point(52, 91);
            this.button_nav_prev.Margin = new System.Windows.Forms.Padding(2);
            this.button_nav_prev.Name = "button_nav_prev";
            this.button_nav_prev.Size = new System.Drawing.Size(37, 31);
            this.button_nav_prev.TabIndex = 30;
            this.button_nav_prev.Text = "<";
            this.button_nav_prev.UseVisualStyleBackColor = true;
            this.button_nav_prev.Click += new System.EventHandler(this.button_nav_home_Click);
            // 
            // button_nav_end
            // 
            this.button_nav_end.Location = new System.Drawing.Point(128, 91);
            this.button_nav_end.Margin = new System.Windows.Forms.Padding(2);
            this.button_nav_end.Name = "button_nav_end";
            this.button_nav_end.Size = new System.Drawing.Size(37, 31);
            this.button_nav_end.TabIndex = 28;
            this.button_nav_end.Text = ">|";
            this.button_nav_end.UseVisualStyleBackColor = true;
            this.button_nav_end.Click += new System.EventHandler(this.button_nav_home_Click);
            // 
            // button_nav_home
            // 
            this.button_nav_home.Location = new System.Drawing.Point(16, 91);
            this.button_nav_home.Margin = new System.Windows.Forms.Padding(2);
            this.button_nav_home.Name = "button_nav_home";
            this.button_nav_home.Size = new System.Drawing.Size(37, 31);
            this.button_nav_home.TabIndex = 31;
            this.button_nav_home.Text = "|<";
            this.button_nav_home.UseVisualStyleBackColor = true;
            this.button_nav_home.Click += new System.EventHandler(this.button_nav_home_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 5);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 18);
            this.label4.TabIndex = 32;
            this.label4.Text = "每頁筆數";
            // 
            // numericUpDown_每頁筆數
            // 
            this.numericUpDown_每頁筆數.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown_每頁筆數.Location = new System.Drawing.Point(87, 3);
            this.numericUpDown_每頁筆數.Margin = new System.Windows.Forms.Padding(2);
            this.numericUpDown_每頁筆數.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numericUpDown_每頁筆數.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown_每頁筆數.Name = "numericUpDown_每頁筆數";
            this.numericUpDown_每頁筆數.Size = new System.Drawing.Size(58, 26);
            this.numericUpDown_每頁筆數.TabIndex = 33;
            this.numericUpDown_每頁筆數.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDown_每頁筆數.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown_每頁筆數.ValueChanged += new System.EventHandler(this.DataGridView_Reload);
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
            this.groupBox_Search.Location = new System.Drawing.Point(247, 0);
            this.groupBox_Search.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox_Search.Name = "groupBox_Search";
            this.groupBox_Search.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupBox_Search.Size = new System.Drawing.Size(241, 131);
            this.groupBox_Search.TabIndex = 16;
            this.groupBox_Search.TabStop = false;
            this.groupBox_Search.Text = "搜尋條件";
            // 
            // checkBox_遞減
            // 
            this.checkBox_遞減.AutoSize = true;
            this.checkBox_遞減.Location = new System.Drawing.Point(178, 19);
            this.checkBox_遞減.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_遞減.Name = "checkBox_遞減";
            this.checkBox_遞減.Size = new System.Drawing.Size(55, 22);
            this.checkBox_遞減.TabIndex = 20;
            this.checkBox_遞減.Text = "遞減";
            this.checkBox_遞減.UseVisualStyleBackColor = true;
            this.checkBox_遞減.CheckedChanged += new System.EventHandler(this.DataGridView_Reload);
            // 
            // textBox_查詢字串
            // 
            this.textBox_查詢字串.Location = new System.Drawing.Point(82, 88);
            this.textBox_查詢字串.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_查詢字串.Name = "textBox_查詢字串";
            this.textBox_查詢字串.Size = new System.Drawing.Size(151, 26);
            this.textBox_查詢字串.TabIndex = 19;
            this.textBox_查詢字串.TextChanged += new System.EventHandler(this.DataGridView_Reload);
            // 
            // comboBox_排序欄位
            // 
            this.comboBox_排序欄位.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_排序欄位.FormattingEnabled = true;
            this.comboBox_排序欄位.Location = new System.Drawing.Point(82, 19);
            this.comboBox_排序欄位.Margin = new System.Windows.Forms.Padding(2);
            this.comboBox_排序欄位.Name = "comboBox_排序欄位";
            this.comboBox_排序欄位.Size = new System.Drawing.Size(91, 26);
            this.comboBox_排序欄位.TabIndex = 5;
            this.comboBox_排序欄位.SelectedIndexChanged += new System.EventHandler(this.DataGridView_Reload);
            // 
            // comboBox_查詢欄位
            // 
            this.comboBox_查詢欄位.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_查詢欄位.FormattingEnabled = true;
            this.comboBox_查詢欄位.Location = new System.Drawing.Point(82, 54);
            this.comboBox_查詢欄位.Margin = new System.Windows.Forms.Padding(2);
            this.comboBox_查詢欄位.Name = "comboBox_查詢欄位";
            this.comboBox_查詢欄位.Size = new System.Drawing.Size(151, 26);
            this.comboBox_查詢欄位.TabIndex = 5;
            this.comboBox_查詢欄位.SelectedIndexChanged += new System.EventHandler(this.DataGridView_Reload);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 22);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 18);
            this.label5.TabIndex = 1;
            this.label5.Text = "排序欄位";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 90);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 18);
            this.label3.TabIndex = 2;
            this.label3.Text = "查詢字串";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 57);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "查詢欄位";
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
            this.groupBox_FieldSelect.Margin = new System.Windows.Forms.Padding(2, 5, 2, 5);
            this.groupBox_FieldSelect.Name = "groupBox_FieldSelect";
            this.groupBox_FieldSelect.Padding = new System.Windows.Forms.Padding(2, 5, 2, 5);
            this.groupBox_FieldSelect.Size = new System.Drawing.Size(247, 131);
            this.groupBox_FieldSelect.TabIndex = 15;
            this.groupBox_FieldSelect.TabStop = false;
            this.groupBox_FieldSelect.Text = "調整顯示欄位";
            // 
            // button_選取欄位_反向
            // 
            this.button_選取欄位_反向.Location = new System.Drawing.Point(176, 93);
            this.button_選取欄位_反向.Margin = new System.Windows.Forms.Padding(2, 5, 2, 5);
            this.button_選取欄位_反向.Name = "button_選取欄位_反向";
            this.button_選取欄位_反向.Size = new System.Drawing.Size(60, 29);
            this.button_選取欄位_反向.TabIndex = 6;
            this.button_選取欄位_反向.Text = "反向";
            this.button_選取欄位_反向.UseVisualStyleBackColor = true;
            this.button_選取欄位_反向.Click += new System.EventHandler(this.button_選取欄位_全選_Click);
            // 
            // button_選取欄位_清除
            // 
            this.button_選取欄位_清除.Location = new System.Drawing.Point(176, 59);
            this.button_選取欄位_清除.Margin = new System.Windows.Forms.Padding(2, 5, 2, 5);
            this.button_選取欄位_清除.Name = "button_選取欄位_清除";
            this.button_選取欄位_清除.Size = new System.Drawing.Size(60, 29);
            this.button_選取欄位_清除.TabIndex = 6;
            this.button_選取欄位_清除.Text = "清除";
            this.button_選取欄位_清除.UseVisualStyleBackColor = true;
            this.button_選取欄位_清除.Click += new System.EventHandler(this.button_選取欄位_全選_Click);
            // 
            // button_選取欄位_全選
            // 
            this.button_選取欄位_全選.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button_選取欄位_全選.Location = new System.Drawing.Point(176, 29);
            this.button_選取欄位_全選.Margin = new System.Windows.Forms.Padding(2, 5, 2, 5);
            this.button_選取欄位_全選.Name = "button_選取欄位_全選";
            this.button_選取欄位_全選.Size = new System.Drawing.Size(60, 29);
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
            this.checkedListBox_選取欄位.Location = new System.Drawing.Point(2, 24);
            this.checkedListBox_選取欄位.Margin = new System.Windows.Forms.Padding(2, 5, 2, 5);
            this.checkedListBox_選取欄位.Name = "checkedListBox_選取欄位";
            this.checkedListBox_選取欄位.Size = new System.Drawing.Size(166, 102);
            this.checkedListBox_選取欄位.TabIndex = 4;
            this.checkedListBox_選取欄位.SelectedValueChanged += new System.EventHandler(this.DataGridView_Reload);
            // 
            // groupBox_dataGridView
            // 
            this.groupBox_dataGridView.Controls.Add(this.dataGridView_Data);
            this.groupBox_dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_dataGridView.Location = new System.Drawing.Point(0, 131);
            this.groupBox_dataGridView.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox_dataGridView.Name = "groupBox_dataGridView";
            this.groupBox_dataGridView.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox_dataGridView.Size = new System.Drawing.Size(671, 306);
            this.groupBox_dataGridView.TabIndex = 30;
            this.groupBox_dataGridView.TabStop = false;
            this.groupBox_dataGridView.Text = " 資料內容 ";
            // 
            // dataGridView_Data
            // 
            this.dataGridView_Data.AllowUserToAddRows = false;
            this.dataGridView_Data.AllowUserToDeleteRows = false;
            this.dataGridView_Data.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dataGridView_Data.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Data.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_Data.Location = new System.Drawing.Point(2, 21);
            this.dataGridView_Data.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView_Data.Name = "dataGridView_Data";
            this.dataGridView_Data.ReadOnly = true;
            this.dataGridView_Data.RowTemplate.Height = 27;
            this.dataGridView_Data.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_Data.Size = new System.Drawing.Size(667, 283);
            this.dataGridView_Data.TabIndex = 30;
            this.dataGridView_Data.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_Data_CellMouseDoubleClick);
            this.dataGridView_Data.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_Data_RowEnter);
            this.dataGridView_Data.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ucDataRowFilter_MouseDown);
            // 
            // contextMenuStrip_main
            // 
            this.contextMenuStrip_main.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.contextMenuStrip_main.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip_main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.資料控制板顯示隱藏ToolStripMenuItem,
            this.字型ToolStripMenuItem});
            this.contextMenuStrip_main.Name = "contextMenuStrip_main";
            this.contextMenuStrip_main.Size = new System.Drawing.Size(251, 52);
            this.contextMenuStrip_main.Text = "資料控制板---顯示/隱藏";
            // 
            // 資料控制板顯示隱藏ToolStripMenuItem
            // 
            this.資料控制板顯示隱藏ToolStripMenuItem.Name = "資料控制板顯示隱藏ToolStripMenuItem";
            this.資料控制板顯示隱藏ToolStripMenuItem.Size = new System.Drawing.Size(250, 24);
            this.資料控制板顯示隱藏ToolStripMenuItem.Text = "資料控制板---顯示/隱藏";
            this.資料控制板顯示隱藏ToolStripMenuItem.Click += new System.EventHandler(this.資料控制板顯示隱藏ToolStripMenuItem_Click);
            // 
            // 字型ToolStripMenuItem
            // 
            this.字型ToolStripMenuItem.Name = "字型ToolStripMenuItem";
            this.字型ToolStripMenuItem.Size = new System.Drawing.Size(250, 24);
            this.字型ToolStripMenuItem.Text = "字型";
            this.字型ToolStripMenuItem.Click += new System.EventHandler(this.字型ToolStripMenuItem_Click);
            // 
            // ucDataRowFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.groupBox_dataGridView);
            this.Controls.Add(this.panel_filter);
            this.Font = new System.Drawing.Font("Microsoft JhengHei", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "ucDataRowFilter";
            this.Size = new System.Drawing.Size(671, 437);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ucDataRowFilter_MouseDown);
            this.panel_filter.ResumeLayout(false);
            this.panel_PageControl.ResumeLayout(false);
            this.panel_PageControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_目前頁面)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_每頁筆數)).EndInit();
            this.groupBox_Search.ResumeLayout(false);
            this.groupBox_Search.PerformLayout();
            this.groupBox_FieldSelect.ResumeLayout(false);
            this.groupBox_dataGridView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Data)).EndInit();
            this.contextMenuStrip_main.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_filter;
        private System.Windows.Forms.Label label_總頁數;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_nav_end;
        private System.Windows.Forms.NumericUpDown numericUpDown_每頁筆數;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDown_目前頁面;
        private System.Windows.Forms.Button button_nav_next;
        private System.Windows.Forms.Button button_nav_prev;
        private System.Windows.Forms.Button button_nav_home;
        private System.Windows.Forms.GroupBox groupBox_Search;
        private System.Windows.Forms.TextBox textBox_查詢字串;
        private System.Windows.Forms.ComboBox comboBox_排序欄位;
        private System.Windows.Forms.ComboBox comboBox_查詢欄位;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox_FieldSelect;
        private System.Windows.Forms.Button button_選取欄位_反向;
        private System.Windows.Forms.Button button_選取欄位_清除;
        private System.Windows.Forms.Button button_選取欄位_全選;
        private System.Windows.Forms.CheckedListBox checkedListBox_選取欄位;
        private System.Windows.Forms.GroupBox groupBox_dataGridView;
        private System.Windows.Forms.DataGridView dataGridView_Data;
        private System.Windows.Forms.Panel panel_PageControl;
        private System.Windows.Forms.CheckBox checkBox_遞減;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_main;
        private System.Windows.Forms.ToolStripMenuItem 資料控制板顯示隱藏ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 字型ToolStripMenuItem;
    }
}
