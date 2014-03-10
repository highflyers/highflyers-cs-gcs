using System;
using System.Linq;

namespace HighFlyers.CsGCS.Communication.GUI
{
    public partial class RS232Control : CommunicationControl
    {
        public RS232Control(Communication communication) : base(communication)
        {
            InitializeComponent();

            RescanPorts();
            baudRateComboBox.SelectedIndex = 0;
            configConnectionPanel.Controls.Add(mainPanel);
        }

        protected override void UpdateModel()
        {
            var rs232Com = Communication as RS232;

            if (rs232Com != null)
            {
                rs232Com.BaudRate = Convert.ToInt32(baudRateComboBox.SelectedItem);
                rs232Com.PortName = portListComboBox.SelectedItem.ToString();
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
