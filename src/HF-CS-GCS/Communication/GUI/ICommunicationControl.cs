using System.Windows.Forms;

namespace HighFlyers.CsGCS.Communication.GUI
{
    public abstract class ACommunicationControl : UserControl
    {
        abstract public ICommunication CommunicationObject { get; }
    }
}
