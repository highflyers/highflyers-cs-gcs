using System;
using System.Windows.Forms;

namespace HF_CS_GCS.Communication.GUI
{
    public partial class CommunicationControl : UserControl
    {
        private ICommunication communication;
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
                communication.Open();
                openCloseButton.Text = @"Close connection";
            }
            else
            {
                communication.Close();
                openCloseButton.Text = @"Open connection";
            }
        }
    }
}
