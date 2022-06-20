namespace Support.DashBoard
{
    partial class ucVideo
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
            this.pictureBox_Video = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Video)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox_Video
            // 
            this.pictureBox_Video.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox_Video.Location = new System.Drawing.Point(0, 0);
            this.pictureBox_Video.Name = "pictureBox_Video";
            this.pictureBox_Video.Size = new System.Drawing.Size(504, 404);
            this.pictureBox_Video.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Video.TabIndex = 2;
            this.pictureBox_Video.TabStop = false;
            this.pictureBox_Video.Click += new System.EventHandler(this.pictureBox_Video_Click);
            // 
            // ucVideo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBox_Video);
            this.Name = "ucVideo";
            this.Controls.SetChildIndex(this.pictureBox_Video, 0);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Video)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox_Video;
    }
}
