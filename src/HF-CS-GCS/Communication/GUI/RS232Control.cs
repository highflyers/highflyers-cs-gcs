using System;
using System.Linq;

namespace HF_CS_GCS.Communication.GUI
{
    public partial class RS232Control : ACommunicationControl
    {
        public RS232Control()
        {
            InitializeComponent();

            RescanPorts();
            
            baudRateComboBox.SelectedIndex = 0;
        }

        public override ICommunication CommunicationObject
        {
            get
            {
                return new RS232(portListComboBox.SelectedItem.ToString(), Convert.ToInt32(baudRateComboBox.SelectedItem));
            }
        }

        private void rescanPortsButton_Click(object sender, EventArgs e)
        {
            RescanPorts();
        }

        private void RescanPorts()
        {
            portListComboBox.Items.Clear();
            portListComboBox.Items.AddRange(RS232.AvailablePorts().Cast<object>().ToArray());
            
            if (portListComboBox.Items.Count > 0)
                portListComboBox.SelectedIndex = 0;
        }
    }
}
