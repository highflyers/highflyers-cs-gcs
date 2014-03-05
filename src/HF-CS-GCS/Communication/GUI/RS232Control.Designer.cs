namespace HF_CS_GCS.Communication.GUI
{
    partial class RS232Control
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.portListComboBox = new System.Windows.Forms.ComboBox();
            this.baudRateComboBox = new System.Windows.Forms.ComboBox();
            this.rescanPortsButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Port:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Baud rate:";
            // 
            // portListComboBox
            // 
            this.portListComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.portListComboBox.FormattingEnabled = true;
            this.portListComboBox.Location = new System.Drawing.Point(38, 7);
            this.portListComboBox.Name = "portListComboBox";
            this.portListComboBox.Size = new System.Drawing.Size(79, 21);
            this.portListComboBox.TabIndex = 3;
            // 
            // baudRateComboBox
            // 
            this.baudRateComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.baudRateComboBox.FormattingEnabled = true;
            this.baudRateComboBox.Location = new System.Drawing.Point(65, 43);
            this.baudRateComboBox.Name = "baudRateComboBox";
            this.baudRateComboBox.Size = new System.Drawing.Size(135, 21);
            this.baudRateComboBox.TabIndex = 4;
            // 
            // rescanPortsButton
            // 
            this.rescanPortsButton.Location = new System.Drawing.Point(123, 5);
            this.rescanPortsButton.Name = "rescanPortsButton";
            this.rescanPortsButton.Size = new System.Drawing.Size(77, 23);
            this.rescanPortsButton.TabIndex = 5;
            this.rescanPortsButton.Text = "Rescan";
            this.rescanPortsButton.UseVisualStyleBackColor = true;
            this.rescanPortsButton.Click += new System.EventHandler(this.rescanPortsButton_Click);
            // 
            // RS232Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rescanPortsButton);
            this.Controls.Add(this.baudRateComboBox);
            this.Controls.Add(this.portListComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "RS232Control";
            this.Size = new System.Drawing.Size(203, 73);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox portListComboBox;
        private System.Windows.Forms.ComboBox baudRateComboBox;
        private System.Windows.Forms.Button rescanPortsButton;
    }
}
