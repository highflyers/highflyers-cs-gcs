using System.Windows.Forms;
using HF_CS_GCS.Communication;
using HF_CS_GCS.Communication.GUI;

namespace HF_CS_GCS.GUI
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();

            communicationTabPage.Controls.Add(new CommunicationControl(CommunicationType.RS232){Dock = DockStyle.Fill});
        }
    }
}
