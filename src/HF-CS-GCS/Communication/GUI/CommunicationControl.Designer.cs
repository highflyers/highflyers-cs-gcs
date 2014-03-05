namespace HF_CS_GCS.Communication.GUI
{
    partial class CommunicationControl
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
            this.openCloseButton = new System.Windows.Forms.Button();
            this.configConnectionPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // openCloseButton
            // 
            this.openCloseButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.openCloseButton.Location = new System.Drawing.Point(0, 127);
            this.openCloseButton.Name = "openCloseButton";
            this.openCloseButton.Size = new System.Drawing.Size(150, 23);
            this.openCloseButton.TabIndex = 0;
            this.openCloseButton.Text = "Open connection";
            this.openCloseButton.UseVisualStyleBackColor = true;
            this.openCloseButton.Click += new System.EventHandler(this.openCloseButton_Click);
            // 
            // configConnectionPanel
            // 
            this.configConnectionPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.configConnectionPanel.Location = new System.Drawing.Point(0, 0);
            this.configConnectionPanel.Name = "configConnectionPanel";
            this.configConnectionPanel.Size = new System.Drawing.Size(150, 127);
            this.configConnectionPanel.TabIndex = 1;
            // 
            // CommunicationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.configConnectionPanel);
            this.Controls.Add(this.openCloseButton);
            this.Name = "CommunicationControl";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button openCloseButton;
        private System.Windows.Forms.Panel configConnectionPanel;
    }
}
