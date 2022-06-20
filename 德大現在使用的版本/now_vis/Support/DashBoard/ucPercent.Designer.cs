namespace Support.DashBoard
{
    partial class ucPercent
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
            this.circularProgressBar_main = new CircularProgressBar.CircularProgressBar();
            this.SuspendLayout();
            // 
            // circularProgressBar_main
            // 
            this.circularProgressBar_main.AnimationFunction = WinFormAnimation.KnownAnimationFunctions.Liner;
            this.circularProgressBar_main.AnimationSpeed = 500;
            this.circularProgressBar_main.BackColor = System.Drawing.Color.Transparent;
            this.circularProgressBar_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.circularProgressBar_main.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold);
            this.circularProgressBar_main.ForeColor = System.Drawing.Color.White;
            this.circularProgressBar_main.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(79)))), ((int)(((byte)(104)))));
            this.circularProgressBar_main.InnerMargin = 0;
            this.circularProgressBar_main.InnerWidth = -1;
            this.circularProgressBar_main.Location = new System.Drawing.Point(0, 0);
            this.circularProgressBar_main.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.circularProgressBar_main.MarqueeAnimationSpeed = 2000;
            this.circularProgressBar_main.Name = "circularProgressBar_main";
            this.circularProgressBar_main.OuterColor = System.Drawing.Color.White;
            this.circularProgressBar_main.OuterMargin = -25;
            this.circularProgressBar_main.OuterWidth = 26;
            this.circularProgressBar_main.ProgressColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(151)))), ((int)(((byte)(218)))));
            this.circularProgressBar_main.ProgressWidth = 17;
            this.circularProgressBar_main.SecondaryFont = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            this.circularProgressBar_main.Size = new System.Drawing.Size(192, 182);
            this.circularProgressBar_main.StartAngle = 270;
            this.circularProgressBar_main.SubscriptColor = System.Drawing.Color.White;
            this.circularProgressBar_main.SubscriptMargin = new System.Windows.Forms.Padding(10, -35, 0, 0);
            this.circularProgressBar_main.SubscriptText = "";
            this.circularProgressBar_main.SuperscriptColor = System.Drawing.Color.White;
            this.circularProgressBar_main.SuperscriptMargin = new System.Windows.Forms.Padding(5, 25, 0, 0);
            this.circularProgressBar_main.SuperscriptText = "%";
            this.circularProgressBar_main.TabIndex = 2;
            this.circularProgressBar_main.Text = "50";
            this.circularProgressBar_main.TextMargin = new System.Windows.Forms.Padding(8, 8, 0, 0);
            this.circularProgressBar_main.Value = 50;
            // 
            // ucPercent
            // 
            this._Title_Height = 36;
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.Controls.Add(this.circularProgressBar_main);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "ucPercent";
            this.Size = new System.Drawing.Size(192, 182);
            this.Controls.SetChildIndex(this.circularProgressBar_main, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private CircularProgressBar.CircularProgressBar circularProgressBar_main;
    }
}
