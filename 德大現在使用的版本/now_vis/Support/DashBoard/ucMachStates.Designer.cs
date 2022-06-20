namespace Support.DashBoard
{
    partial class ucMachStates
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
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
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.panel_main = new System.Windows.Forms.Panel();
            this.pictureBox_machine = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbl_達成率 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lbl_程式 = new System.Windows.Forms.Label();
            this.lbl_操作狀態 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lbl_稼動率 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lbl_生產進度 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lbl_機台不良率 = new System.Windows.Forms.Label();
            this.button_生產狀況表 = new System.Windows.Forms.Button();
            this.lbl_MachineName = new System.Windows.Forms.Label();
            this.gr_機台使用狀況 = new System.Windows.Forms.GroupBox();
            this.lbl_預計完工 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.lbl_製令 = new System.Windows.Forms.Label();
            this.lbl_操作員 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lbl_預定保養日 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_M_totalTime = new System.Windows.Forms.Label();
            this.lbl_M_PowerT = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel_main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_machine)).BeginInit();
            this.panel1.SuspendLayout();
            this.gr_機台使用狀況.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_main
            // 
            this.panel_main.Controls.Add(this.pictureBox_machine);
            this.panel_main.Controls.Add(this.panel1);
            this.panel_main.Controls.Add(this.button_生產狀況表);
            this.panel_main.Controls.Add(this.lbl_MachineName);
            this.panel_main.Controls.Add(this.gr_機台使用狀況);
            this.panel_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_main.Location = new System.Drawing.Point(0, 24);
            this.panel_main.Name = "panel_main";
            this.panel_main.Size = new System.Drawing.Size(541, 400);
            this.panel_main.TabIndex = 2;
            // 
            // pictureBox_machine
            // 
            this.pictureBox_machine.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox_machine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox_machine.Location = new System.Drawing.Point(0, 40);
            this.pictureBox_machine.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox_machine.Name = "pictureBox_machine";
            this.pictureBox_machine.Size = new System.Drawing.Size(341, 246);
            this.pictureBox_machine.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_machine.TabIndex = 55;
            this.pictureBox_machine.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.lbl_達成率);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label16);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.lbl_程式);
            this.panel1.Controls.Add(this.lbl_操作狀態);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.lbl_稼動率);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.lbl_生產進度);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.lbl_機台不良率);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(341, 40);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 246);
            this.panel1.TabIndex = 56;
            // 
            // lbl_達成率
            // 
            this.lbl_達成率.AutoSize = true;
            this.lbl_達成率.Font = new System.Drawing.Font("微軟正黑體", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_達成率.Location = new System.Drawing.Point(12, 71);
            this.lbl_達成率.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_達成率.Name = "lbl_達成率";
            this.lbl_達成率.Size = new System.Drawing.Size(70, 36);
            this.lbl_達成率.TabIndex = 1;
            this.lbl_達成率.Text = "N/A";
            this.lbl_達成率.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(17, 40);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 25);
            this.label2.TabIndex = 26;
            this.label2.Text = "狀態";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label16.Location = new System.Drawing.Point(99, 40);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(72, 25);
            this.label16.TabIndex = 45;
            this.label16.Text = "稼動率";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label9.Location = new System.Drawing.Point(17, 173);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(52, 25);
            this.label9.TabIndex = 33;
            this.label9.Text = "程式";
            // 
            // lbl_程式
            // 
            this.lbl_程式.AutoSize = true;
            this.lbl_程式.Font = new System.Drawing.Font("微軟正黑體", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_程式.Location = new System.Drawing.Point(11, 137);
            this.lbl_程式.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_程式.Name = "lbl_程式";
            this.lbl_程式.Size = new System.Drawing.Size(70, 36);
            this.lbl_程式.TabIndex = 7;
            this.lbl_程式.Text = "N/A";
            // 
            // lbl_操作狀態
            // 
            this.lbl_操作狀態.AutoSize = true;
            this.lbl_操作狀態.Font = new System.Drawing.Font("微軟正黑體", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_操作狀態.Location = new System.Drawing.Point(11, 1);
            this.lbl_操作狀態.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_操作狀態.Name = "lbl_操作狀態";
            this.lbl_操作狀態.Size = new System.Drawing.Size(78, 36);
            this.lbl_操作狀態.TabIndex = 43;
            this.lbl_操作狀態.Text = "RUN";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label12.Location = new System.Drawing.Point(86, 108);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(112, 25);
            this.label12.TabIndex = 39;
            this.label12.Text = "機台不良率";
            // 
            // lbl_稼動率
            // 
            this.lbl_稼動率.AutoSize = true;
            this.lbl_稼動率.Font = new System.Drawing.Font("微軟正黑體", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_稼動率.Location = new System.Drawing.Point(107, 1);
            this.lbl_稼動率.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_稼動率.Name = "lbl_稼動率";
            this.lbl_稼動率.Size = new System.Drawing.Size(70, 36);
            this.lbl_稼動率.TabIndex = 40;
            this.lbl_稼動率.Text = "N/A";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.Location = new System.Drawing.Point(4, 107);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 25);
            this.label4.TabIndex = 7;
            this.label4.Text = "達成率";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_生產進度
            // 
            this.lbl_生產進度.AutoSize = true;
            this.lbl_生產進度.Font = new System.Drawing.Font("微軟正黑體", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_生產進度.Location = new System.Drawing.Point(107, 138);
            this.lbl_生產進度.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_生產進度.Name = "lbl_生產進度";
            this.lbl_生產進度.Size = new System.Drawing.Size(70, 36);
            this.lbl_生產進度.TabIndex = 2;
            this.lbl_生產進度.Text = "N/A";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label11.Location = new System.Drawing.Point(89, 173);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(92, 25);
            this.label11.TabIndex = 10;
            this.label11.Text = "生產進度";
            // 
            // lbl_機台不良率
            // 
            this.lbl_機台不良率.AutoSize = true;
            this.lbl_機台不良率.Font = new System.Drawing.Font("微軟正黑體", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_機台不良率.Location = new System.Drawing.Point(107, 70);
            this.lbl_機台不良率.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_機台不良率.Name = "lbl_機台不良率";
            this.lbl_機台不良率.Size = new System.Drawing.Size(70, 36);
            this.lbl_機台不良率.TabIndex = 42;
            this.lbl_機台不良率.Text = "N/A";
            // 
            // button_生產狀況表
            // 
            this.button_生產狀況表.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_生產狀況表.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button_生產狀況表.Location = new System.Drawing.Point(415, 4);
            this.button_生產狀況表.Margin = new System.Windows.Forms.Padding(4);
            this.button_生產狀況表.Name = "button_生產狀況表";
            this.button_生產狀況表.Size = new System.Drawing.Size(100, 32);
            this.button_生產狀況表.TabIndex = 54;
            this.button_生產狀況表.Text = "加工歷程";
            this.button_生產狀況表.UseVisualStyleBackColor = true;
            // 
            // lbl_MachineName
            // 
            this.lbl_MachineName.BackColor = System.Drawing.SystemColors.Control;
            this.lbl_MachineName.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbl_MachineName.Font = new System.Drawing.Font("微軟正黑體", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_MachineName.ForeColor = System.Drawing.Color.Black;
            this.lbl_MachineName.Location = new System.Drawing.Point(0, 0);
            this.lbl_MachineName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_MachineName.Name = "lbl_MachineName";
            this.lbl_MachineName.Size = new System.Drawing.Size(541, 40);
            this.lbl_MachineName.TabIndex = 52;
            this.lbl_MachineName.Text = "無法連線";
            this.lbl_MachineName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_MachineName.Click += new System.EventHandler(this.lbl_MachineName_Click);
            // 
            // gr_機台使用狀況
            // 
            this.gr_機台使用狀況.BackColor = System.Drawing.SystemColors.Info;
            this.gr_機台使用狀況.Controls.Add(this.lbl_預計完工);
            this.gr_機台使用狀況.Controls.Add(this.label14);
            this.gr_機台使用狀況.Controls.Add(this.lbl_製令);
            this.gr_機台使用狀況.Controls.Add(this.lbl_操作員);
            this.gr_機台使用狀況.Controls.Add(this.label7);
            this.gr_機台使用狀況.Controls.Add(this.label10);
            this.gr_機台使用狀況.Controls.Add(this.lbl_預定保養日);
            this.gr_機台使用狀況.Controls.Add(this.label1);
            this.gr_機台使用狀況.Controls.Add(this.lbl_M_totalTime);
            this.gr_機台使用狀況.Controls.Add(this.lbl_M_PowerT);
            this.gr_機台使用狀況.Controls.Add(this.label5);
            this.gr_機台使用狀況.Controls.Add(this.label3);
            this.gr_機台使用狀況.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gr_機台使用狀況.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gr_機台使用狀況.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.gr_機台使用狀況.Location = new System.Drawing.Point(0, 286);
            this.gr_機台使用狀況.Margin = new System.Windows.Forms.Padding(4);
            this.gr_機台使用狀況.Name = "gr_機台使用狀況";
            this.gr_機台使用狀況.Padding = new System.Windows.Forms.Padding(4);
            this.gr_機台使用狀況.Size = new System.Drawing.Size(541, 114);
            this.gr_機台使用狀況.TabIndex = 53;
            this.gr_機台使用狀況.TabStop = false;
            this.gr_機台使用狀況.Text = "機台使用狀況";
            // 
            // lbl_預計完工
            // 
            this.lbl_預計完工.AutoSize = true;
            this.lbl_預計完工.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_預計完工.Location = new System.Drawing.Point(386, 77);
            this.lbl_預計完工.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_預計完工.Name = "lbl_預計完工";
            this.lbl_預計完工.Size = new System.Drawing.Size(50, 25);
            this.lbl_預計完工.TabIndex = 11;
            this.lbl_預計完工.Text = "N/A";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label14.Location = new System.Drawing.Point(293, 80);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(92, 25);
            this.label14.TabIndex = 10;
            this.label14.Text = "預計完工";
            // 
            // lbl_製令
            // 
            this.lbl_製令.AutoSize = true;
            this.lbl_製令.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_製令.Location = new System.Drawing.Point(386, 52);
            this.lbl_製令.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_製令.Name = "lbl_製令";
            this.lbl_製令.Size = new System.Drawing.Size(50, 25);
            this.lbl_製令.TabIndex = 9;
            this.lbl_製令.Text = "N/A";
            // 
            // lbl_操作員
            // 
            this.lbl_操作員.AutoSize = true;
            this.lbl_操作員.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_操作員.Location = new System.Drawing.Point(386, 23);
            this.lbl_操作員.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_操作員.Name = "lbl_操作員";
            this.lbl_操作員.Size = new System.Drawing.Size(50, 25);
            this.lbl_操作員.TabIndex = 9;
            this.lbl_操作員.Text = "N/A";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label7.Location = new System.Drawing.Point(293, 26);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 25);
            this.label7.TabIndex = 8;
            this.label7.Text = "操作員";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label10.Location = new System.Drawing.Point(293, 54);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(67, 25);
            this.label10.TabIndex = 8;
            this.label10.Text = "製   令";
            // 
            // lbl_預定保養日
            // 
            this.lbl_預定保養日.AutoSize = true;
            this.lbl_預定保養日.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_預定保養日.Location = new System.Drawing.Point(140, 82);
            this.lbl_預定保養日.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_預定保養日.Name = "lbl_預定保養日";
            this.lbl_預定保養日.Size = new System.Drawing.Size(50, 25);
            this.lbl_預定保養日.TabIndex = 7;
            this.lbl_預定保養日.Text = "N/A";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(5, 82);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 25);
            this.label1.TabIndex = 6;
            this.label1.Text = "預定保養日";
            // 
            // lbl_M_totalTime
            // 
            this.lbl_M_totalTime.AutoSize = true;
            this.lbl_M_totalTime.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_M_totalTime.Location = new System.Drawing.Point(140, 56);
            this.lbl_M_totalTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_M_totalTime.Name = "lbl_M_totalTime";
            this.lbl_M_totalTime.Size = new System.Drawing.Size(75, 25);
            this.lbl_M_totalTime.TabIndex = 5;
            this.lbl_M_totalTime.Text = "-------";
            // 
            // lbl_M_PowerT
            // 
            this.lbl_M_PowerT.AutoSize = true;
            this.lbl_M_PowerT.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_M_PowerT.Location = new System.Drawing.Point(140, 28);
            this.lbl_M_PowerT.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_M_PowerT.Name = "lbl_M_PowerT";
            this.lbl_M_PowerT.Size = new System.Drawing.Size(75, 25);
            this.lbl_M_PowerT.TabIndex = 4;
            this.lbl_M_PowerT.Text = "-------";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label5.Location = new System.Drawing.Point(5, 28);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(144, 25);
            this.label5.TabIndex = 3;
            this.label5.Text = "總開機時間(分)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.Location = new System.Drawing.Point(5, 56);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(144, 25);
            this.label3.TabIndex = 1;
            this.label3.Text = "總加工時間(分)";
            // 
            // ucMachStates
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.panel_main);
            this.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Name = "ucMachStates";
            this.Size = new System.Drawing.Size(541, 424);
            this.Controls.SetChildIndex(this.panel_main, 0);
            this.panel_main.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_machine)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.gr_機台使用狀況.ResumeLayout(false);
            this.gr_機台使用狀況.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Panel panel_main;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbl_達成率;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lbl_程式;
        private System.Windows.Forms.Label lbl_操作狀態;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lbl_稼動率;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbl_生產進度;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lbl_機台不良率;
        private System.Windows.Forms.Button button_生產狀況表;
        private System.Windows.Forms.PictureBox pictureBox_machine;
        private System.Windows.Forms.Label lbl_MachineName;
        private System.Windows.Forms.GroupBox gr_機台使用狀況;
        private System.Windows.Forms.Label lbl_預計完工;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label lbl_製令;
        private System.Windows.Forms.Label lbl_操作員;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lbl_預定保養日;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_M_totalTime;
        private System.Windows.Forms.Label lbl_M_PowerT;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
    }
}
