using System;
using System.Windows.Forms;

namespace HighFlyers.CsGCS.Communication.GUI
{
    public partial class CommunicationControl : UserControl
    {
        private Communication communication;
        private readonly ACommunicationControl communicationCtrl;

        public CommunicationControl(CommunicationType type)
        {
            InitializeComponent();

            switch (type)
            {
                case CommunicationType.RS232:
                    communicationCtrl = new RS232Control();
                    break;
                case CommunicationType.Wifi:
                    throw new NotImplementedException();
                default:
                    throw new Exception("Unknow connection type: " + type);
            }

            configConnectionPanel.Controls.Add(communicationCtrl);
        }

        private void openCloseButton_Click(object sender, EventArgs e)
        {
            if (communication == null || !communication.IsOpen)
            {
                communication = communicationCtrl.CommunicationObject;
                communication.DataReceived += CommunicationOnDataReceived;
                communication.DataSent += CommunicationOnDataSent;
                communication.Open();
                openCloseButton.Text = @"Close connection";
            }
            else
            {
                communication.Close();
                communication.DataReceived -= CommunicationOnDataReceived;
                communication.DataSent -= CommunicationOnDataSent;
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
