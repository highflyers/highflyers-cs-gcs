namespace HighFlyers.CsGCS.Communication
{
    public delegate void DataReceivedEventHandler(byte[] data);
    public delegate void DataSentEventHandler(byte[] data);

    public enum CommunicationType
    {
        RS232,
        Wifi
    }

    public abstract class Communication
    {
        public abstract int SendData(byte[] data);
        public abstract void Open();
        public abstract void Close();
        public abstract bool IsOpen { get; }
        
        public event DataReceivedEventHandler DataReceived;
        public event DataReceivedEventHandler DataSent;

        protected virtual void OnDataReceived(byte[] data)
        {
            if (DataReceived != null) 
                DataReceived(data);
        }

        protected virtual void OnDataSent(byte[] data)
        {
            if (DataSent != null) 
                DataSent(data);
        }
    }
}
