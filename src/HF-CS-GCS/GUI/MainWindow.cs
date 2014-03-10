using System;
using System.Windows.Forms;
using HighFlyers.CsGCS.Communication;

namespace HighFlyers.CsGCS.GUI
{
    public partial class MainWindow : Form
    {
        private readonly Communication.Communication communication;

        public MainWindow()
        {
            InitializeComponent();

            const CommunicationType type = CommunicationType.RS232;
            CommunicationFactory communicationFactory;

            switch (type)
            {
                case CommunicationType.RS232:
                    communicationFactory = new RS232Factory();
                    break;
                default:
                    throw new Exception("undefined communication factory");
            }

            communication = communicationFactory.CreateCommunication();
            var comCtrl = communicationFactory.CreateControl(communication);
            comCtrl.Dock = DockStyle.Fill;
            communicationTabPage.Controls.Add(comCtrl);
        }
    }
}
