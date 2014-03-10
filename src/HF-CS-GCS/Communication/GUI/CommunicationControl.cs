using System;
using System.Windows.Forms;

namespace HighFlyers.CsGCS.Communication.GUI
{
    public abstract partial class CommunicationControl : UserControl
    {
        protected Communication Communication;

        protected CommunicationControl(Communication communication)
        {
            InitializeComponent();
            Communication = communication;
        }

        protected abstract void UpdateModel();

        private void openCloseButton_Click(object sender, EventArgs e)
        {
            if (!Communication.IsOpen)
            {
                UpdateModel();
                Communication.DataReceived += CommunicationOnDataReceived;
                Communication.DataSent += CommunicationOnDataSent;
                Communication.Open();
                openCloseButton.Text = @"Close connection";
            }
            else
            {
                Communication.Close();
                Communication.DataReceived -= CommunicationOnDataReceived;
                Communication.DataSent -= CommunicationOnDataSent;
                openCloseButton.Text = @"Open connection";
            }
        }

        private void CommunicationOnDataSent(byte[] data)
        {
            Utils.InvokeOrDie(receivedDatarichTextBox,
                () => { sentDataRichTextBox.Text += System.Text.Encoding.Default.GetString(data); });
        }

        private void CommunicationOnDataReceived(byte[] data)
        {
            Utils.InvokeOrDie(receivedDatarichTextBox,
                () => { receivedDatarichTextBox.Text += System.Text.Encoding.Default.GetString(data); });
        }
    }
}
