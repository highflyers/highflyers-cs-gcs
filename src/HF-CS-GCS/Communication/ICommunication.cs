namespace HF_CS_GCS.Communication
{
    delegate void DataReceivedEventHandler(byte[] data); 

    interface ICommunication
    {
        int SendData(byte[] data);
        void Open();
        void Close();
        bool IsOpen { get; set; }
        event DataReceivedEventHandler DataReceived;
    }
}
