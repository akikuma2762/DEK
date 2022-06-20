namespace Support.DashBoard
{
    partial class ucDataView
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
            this.ucDataGridView_main = new Support.userControl.ucDataGridView();
            this.SuspendLayout();
            // 
            // ucDataGridView_main
            // 
            this.ucDataGridView_main._SQL查詢條件 = null;
            this.ucDataGridView_main._Title = "資料內容";
            this.ucDataGridView_main._每頁筆數 = 10;
            this.ucDataGridView_main._目前頁數 = -1;
            this.ucDataGridView_main._資料總筆數 = 0;
            this.ucDataGridView_main._資料表名稱 = null;
            this.ucDataGridView_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucDataGridView_main.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ucDataGridView_main.Location = new System.Drawing.Point(0, 24);
            this.ucDataGridView_main.Margin = new System.Windows.Forms.Padding(4);
            this.ucDataGridView_main.Name = "ucDataGridView_main";
            this.ucDataGridView_main.Size = new System.Drawing.Size(374, 223);
            this.ucDataGridView_main.TabIndex = 4;
            this.ucDataGridView_main.UserControlList = null;
            // 
            // ucDataView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ucDataGridView_main);
            this.Name = "ucDataView";
            this.Size = new System.Drawing.Size(374, 247);
            this.Controls.SetChildIndex(this.ucDataGridView_main, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private userControl.ucDataGridView ucDataGridView_main;
    }
}
