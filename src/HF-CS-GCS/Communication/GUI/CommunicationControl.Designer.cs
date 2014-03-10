namespace HighFlyers.CsGCS.Communication.GUI
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.receivedDatarichTextBox = new System.Windows.Forms.RichTextBox();
            this.sentDataRichTextBox = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // openCloseButton
            // 
            this.openCloseButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.openCloseButton.Location = new System.Drawing.Point(0, 424);
            this.openCloseButton.Name = "openCloseButton";
            this.openCloseButton.Size = new System.Drawing.Size(497, 23);
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
            this.configConnectionPanel.Size = new System.Drawing.Size(497, 94);
            this.configConnectionPanel.TabIndex = 1;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.receivedDatarichTextBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.sentDataRichTextBox);
            this.splitContainer1.Size = new System.Drawing.Size(497, 326);
            this.splitContainer1.SplitterDistance = 249;
            this.splitContainer1.TabIndex = 2;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.configConnectionPanel);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer1);
            this.splitContainer2.Size = new System.Drawing.Size(497, 424);
            this.splitContainer2.SplitterDistance = 94;
            this.splitContainer2.TabIndex = 3;
            // 
            // receivedDatarichTextBox
            // 
            this.receivedDatarichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.receivedDatarichTextBox.Location = new System.Drawing.Point(0, 0);
            this.receivedDatarichTextBox.Name = "receivedDatarichTextBox";
            this.receivedDatarichTextBox.Size = new System.Drawing.Size(249, 326);
            this.receivedDatarichTextBox.TabIndex = 0;
            this.receivedDatarichTextBox.Text = "";
            // 
            // sentDataRichTextBox
            // 
            this.sentDataRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sentDataRichTextBox.Location = new System.Drawing.Point(0, 0);
            this.sentDataRichTextBox.Name = "sentDataRichTextBox";
            this.sentDataRichTextBox.Size = new System.Drawing.Size(244, 326);
            this.sentDataRichTextBox.TabIndex = 0;
            this.sentDataRichTextBox.Text = "";
            // 
            // CommunicationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer2);
            this.Controls.Add(this.openCloseButton);
            this.Name = "CommunicationControl";
            this.Size = new System.Drawing.Size(497, 447);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button openCloseButton;
        protected System.Windows.Forms.Panel configConnectionPanel;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.RichTextBox receivedDatarichTextBox;
        private System.Windows.Forms.RichTextBox sentDataRichTextBox;
        private System.Windows.Forms.SplitContainer splitContainer2;
    }
}
