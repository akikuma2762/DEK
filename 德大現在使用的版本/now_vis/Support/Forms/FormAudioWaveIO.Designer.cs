namespace Support.Forms
{
    partial class FormAudioWaveIO
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
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox_audioIn = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_audioOut = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox_samplesPerSec = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox_bitsPerSample = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBox_channels = new System.Windows.Forms.ComboBox();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.button_OK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Audio Input";
            // 
            // comboBox_audioIn
            // 
            this.comboBox_audioIn.FormattingEnabled = true;
            this.comboBox_audioIn.Location = new System.Drawing.Point(169, 6);
            this.comboBox_audioIn.Name = "comboBox_audioIn";
            this.comboBox_audioIn.Size = new System.Drawing.Size(304, 33);
            this.comboBox_audioIn.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(141, 25);
            this.label2.TabIndex = 0;
            this.label2.Text = "Audio Output";
            // 
            // comboBox_audioOut
            // 
            this.comboBox_audioOut.FormattingEnabled = true;
            this.comboBox_audioOut.Location = new System.Drawing.Point(169, 45);
            this.comboBox_audioOut.Name = "comboBox_audioOut";
            this.comboBox_audioOut.Size = new System.Drawing.Size(304, 33);
            this.comboBox_audioOut.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(55, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(137, 25);
            this.label3.TabIndex = 0;
            this.label3.Text = "samples / sec";
            // 
            // comboBox_samplesPerSec
            // 
            this.comboBox_samplesPerSec.FormattingEnabled = true;
            this.comboBox_samplesPerSec.Items.AddRange(new object[] {
            "5000",
            "8000",
            "11025",
            "22050",
            "44100"});
            this.comboBox_samplesPerSec.Location = new System.Drawing.Point(51, 121);
            this.comboBox_samplesPerSec.Name = "comboBox_samplesPerSec";
            this.comboBox_samplesPerSec.Size = new System.Drawing.Size(141, 33);
            this.comboBox_samplesPerSec.TabIndex = 4;
            this.comboBox_samplesPerSec.Text = "5000";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(207, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(132, 25);
            this.label4.TabIndex = 0;
            this.label4.Text = "bits / sample";
            // 
            // comboBox_bitsPerSample
            // 
            this.comboBox_bitsPerSample.FormattingEnabled = true;
            this.comboBox_bitsPerSample.Items.AddRange(new object[] {
            "8",
            "16"});
            this.comboBox_bitsPerSample.Location = new System.Drawing.Point(242, 121);
            this.comboBox_bitsPerSample.Name = "comboBox_bitsPerSample";
            this.comboBox_bitsPerSample.Size = new System.Drawing.Size(65, 33);
            this.comboBox_bitsPerSample.TabIndex = 5;
            this.comboBox_bitsPerSample.Text = "16";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(351, 93);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 25);
            this.label5.TabIndex = 0;
            this.label5.Text = "channels";
            // 
            // comboBox_channels
            // 
            this.comboBox_channels.FormattingEnabled = true;
            this.comboBox_channels.Items.AddRange(new object[] {
            "1",
            "2"});
            this.comboBox_channels.Location = new System.Drawing.Point(364, 121);
            this.comboBox_channels.Name = "comboBox_channels";
            this.comboBox_channels.Size = new System.Drawing.Size(54, 33);
            this.comboBox_channels.TabIndex = 6;
            this.comboBox_channels.Text = "1";
            // 
            // button_Cancel
            // 
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new System.Drawing.Point(115, 190);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(131, 50);
            this.button_Cancel.TabIndex = 1;
            this.button_Cancel.Text = "取消";
            this.button_Cancel.UseVisualStyleBackColor = true;
            // 
            // button_OK
            // 
            this.button_OK.Location = new System.Drawing.Point(287, 190);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(131, 50);
            this.button_OK.TabIndex = 0;
            this.button_OK.Text = "確定";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // FormAudioWaveIO
            // 
            this.AcceptButton = this.button_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_Cancel;
            this.ClientSize = new System.Drawing.Size(481, 253);
            this.ControlBox = false;
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.comboBox_channels);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboBox_bitsPerSample);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBox_samplesPerSec);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBox_audioOut);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox_audioIn);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FormAudioWaveIO";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Audio Wave I/O Config";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox_audioIn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox_audioOut;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox_samplesPerSec;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBox_bitsPerSample;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBox_channels;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.Button button_OK;
    }
}