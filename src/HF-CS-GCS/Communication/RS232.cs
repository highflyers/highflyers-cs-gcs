using System;
using System.ComponentModel;
using System.IO;
using System.IO.Ports;

namespace HF_CS_GCS.Communication
{
    class RS232 : ICommunication
    {
        private readonly SerialPort port;
        private readonly object writeLocker = new object();

        public RS232(string portName, int baudRate = 9600, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One)
        {
            port = new SerialPort(portName, baudRate, parity, dataBits, stopBits);

            if (port == null)
                throw new Exception("Cannot create SerialPort object");

            port.DataReceived += PortOnDataReceived;
        }

        public int SendData(byte[] data)
        {
            if (!port.IsOpen)
                throw new IOException("Cannot send data: port is closed");

            lock (writeLocker)
            {
                port.Write(data, 0, data.Length);

                if (DataSent != null)
                    DataSent(data);

                return port.BytesToWrite;
            }
        }

        public void Open()
        {
            if (IsOpen)
                throw new WarningException("Port is already opened");

            port.Open();

            if (!IsOpen)
                throw new IOException("Cannot open port");
        }

        public void Close()
        {
            if (!IsOpen)
                throw new WarningException("Port is already closed");

            port.Close();

            if (IsOpen)
                throw new IOException("Cannot close port");
        }

        public bool IsOpen
        {
            get { return port.IsOpen; }
        }

        public string PortName
        {
            get { return port.PortName; }
        }

        public int BaudRate
        {
            get { return port.BaudRate; }
        }

        static public string[] AvailablePorts()
        {
            return SerialPort.GetPortNames();
        }

        public event DataReceivedEventHandler DataReceived;
        public event DataReceivedEventHandler DataSent;

        private void PortOnDataReceived(object sender, SerialDataReceivedEventArgs serialDataReceivedEventArgs)
        {
            if (!port.IsOpen) 
                throw new IOException("What?? Data received when port is closed?");

            int count = port.BytesToRead;
            if (count <= 0) return;

            var receivedData = new byte[count];
            port.Read(receivedData, 0, count);
            
            if (DataReceived != null)
                DataReceived(receivedData);
        }
    }
}
