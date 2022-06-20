namespace Support
{
    partial class FormPropertyEdit
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
            this.button_panel_close = new System.Windows.Forms.Button();
            this.propertyGrid_Obj = new System.Windows.Forms.PropertyGrid();
            this.panel_Right = new System.Windows.Forms.Panel();
            this.panel_Right.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_panel_close
            // 
            this.button_panel_close.Location = new System.Drawing.Point(7, 42);
            this.button_panel_close.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_panel_close.Name = "button_panel_close";
            this.button_panel_close.Size = new System.Drawing.Size(84, 39);
            this.button_panel_close.TabIndex = 5;
            this.button_panel_close.Text = "完成";
            this.button_panel_close.UseVisualStyleBackColor = true;
            this.button_panel_close.Click += new System.EventHandler(this.button_panel_close_Click);
            // 
            // propertyGrid_Obj
            // 
            this.propertyGrid_Obj.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid_Obj.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid_Obj.Name = "propertyGrid_Obj";
            this.propertyGrid_Obj.Size = new System.Drawing.Size(331, 502);
            this.propertyGrid_Obj.TabIndex = 11;
            // 
            // panel_Right
            // 
            this.panel_Right.Controls.Add(this.button_panel_close);
            this.panel_Right.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel_Right.Location = new System.Drawing.Point(331, 0);
            this.panel_Right.Name = "panel_Right";
            this.panel_Right.Size = new System.Drawing.Size(96, 502);
            this.panel_Right.TabIndex = 12;
            // 
            // FormPropertyEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(427, 502);
            this.Controls.Add(this.propertyGrid_Obj);
            this.Controls.Add(this.panel_Right);
            this.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FormPropertyEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "參數設定";
            this.panel_Right.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_panel_close;
        private System.Windows.Forms.PropertyGrid propertyGrid_Obj;
        private System.Windows.Forms.Panel panel_Right;
    }
}