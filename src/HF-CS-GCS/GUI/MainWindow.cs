using System.Windows.Forms;
using HighFlyers.CsGCS.Communication;
using HighFlyers.CsGCS.Communication.GUI;

namespace HighFlyers.CsGCS.GUI
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
