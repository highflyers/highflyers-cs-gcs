using HighFlyers.CsGCS.Communication.GUI;

namespace HighFlyers.CsGCS.Communication
{
    abstract class CommunicationFactory
    {
        public abstract Communication CreateCommunication();
        public abstract CommunicationControl CreateControl(Communication communication);
    }

    class RS232Factory : CommunicationFactory
    {
        public override Communication CreateCommunication()
        {
            return new RS232();
        }

        public override CommunicationControl CreateControl(Communication communication)
        {
            return new RS232Control(communication);
        }
    }
}
