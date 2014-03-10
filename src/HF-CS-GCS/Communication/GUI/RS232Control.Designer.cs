namespace HighFlyers.CsGCS.Communication.GUI
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
            this.mainPanel = new System.Windows.Forms.Panel();
            this.rescanPortsButton = new System.Windows.Forms.Button();
            this.baudRateComboBox = new System.Windows.Forms.ComboBox();
            this.portListComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.mainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.rescanPortsButton);
            this.mainPanel.Controls.Add(this.baudRateComboBox);
            this.mainPanel.Controls.Add(this.portListComboBox);
            this.mainPanel.Controls.Add(this.label2);
            this.mainPanel.Controls.Add(this.label1);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(203, 73);
            this.mainPanel.TabIndex = 0;
            // 
            // rescanPortsButton
            // 
            this.rescanPortsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rescanPortsButton.Location = new System.Drawing.Point(123, 7);
            this.rescanPortsButton.Name = "rescanPortsButton";
            this.rescanPortsButton.Size = new System.Drawing.Size(77, 23);
            this.rescanPortsButton.TabIndex = 10;
            this.rescanPortsButton.Text = "Rescan";
            this.rescanPortsButton.UseVisualStyleBackColor = true;
            this.rescanPortsButton.Click += rescanPortsButton_Click;
            // 
            // baudRateComboBox
            // 
            this.baudRateComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.baudRateComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.baudRateComboBox.FormattingEnabled = true;
            this.baudRateComboBox.Items.AddRange(new object[] {
            "4800",
            "9600"});
            this.baudRateComboBox.Location = new System.Drawing.Point(65, 45);
            this.baudRateComboBox.Name = "baudRateComboBox";
            this.baudRateComboBox.Size = new System.Drawing.Size(135, 21);
            this.baudRateComboBox.TabIndex = 9;
            // 
            // portListComboBox
            // 
            this.portListComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.portListComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.portListComboBox.FormattingEnabled = true;
            this.portListComboBox.Location = new System.Drawing.Point(38, 9);
            this.portListComboBox.Name = "portListComboBox";
            this.portListComboBox.Size = new System.Drawing.Size(79, 21);
            this.portListComboBox.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Baud rate:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Port:";
            // 
            // RS232Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainPanel);
            this.Name = "RS232Control";
            this.Size = new System.Drawing.Size(203, 73);
            this.mainPanel.ResumeLayout(false);
            this.mainPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Button rescanPortsButton;
        private System.Windows.Forms.ComboBox baudRateComboBox;
        private System.Windows.Forms.ComboBox portListComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;

    }
}
