namespace HF_CS_GCS.Communication
{
    public delegate void DataReceivedEventHandler(byte[] data);

    public enum CommunicationType
    {
        RS232,
        Wifi
    }

    public interface ICommunication
    {
        int SendData(byte[] data);
        void Open();
        void Close();
        bool IsOpen { get; }
        event DataReceivedEventHandler DataReceived;
    }
}
