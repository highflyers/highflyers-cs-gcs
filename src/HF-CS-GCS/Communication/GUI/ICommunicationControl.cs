using System.Windows.Forms;

namespace HF_CS_GCS.Communication.GUI
{
    public abstract class ACommunicationControl : UserControl
    {
        abstract public ICommunication CommunicationObject { get; }
    }
}
