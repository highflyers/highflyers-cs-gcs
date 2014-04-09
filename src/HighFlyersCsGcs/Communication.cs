using System;
using System.IO;
using System.IO.Ports;

namespace HighFlyers.GCS
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class Communication : Gtk.Bin
	{
		SerialPort port;

		public event DataEventHandler DataReceived;
		public event DataEventHandler DataSent;

		public void CallDataEventHandler(DataEventHandler evt, byte[] data)
		{
			if (evt != null)
				evt (this, new DataEventArgs (data));
		}

		public Communication ()
		{
			this.Build ();
			ReloadPorts ();
		}

		protected void OnRescanPortsButtonClicked (object sender, EventArgs e)
		{
			ReloadPorts ();
		}

		void SerialDataReceived (object sender, SerialDataReceivedEventArgs e)
		{
			byte[] buffer = new byte[port.BytesToRead];
			port.Read(buffer, 0, buffer.Length);
			CallDataEventHandler (DataReceived, buffer);
		}

		void ReloadPorts ()
		{
			portsComboBox.Model = new Gtk.ListStore(typeof (string));

			foreach (var p in SerialPort.GetPortNames ())
				portsComboBox.AppendText (p);
		}

		protected void OnOpenCloseButtonClicked (object sender, EventArgs e)
		{
			if (IsOpen) {
				try {
					Close ();
					openCloseButton.Label = "Open port";
				} catch (Exception ex) {
					// TODO
				}

			} else {
				try {
					Open ();
					openCloseButton.Label = "Close port";
				} catch (Exception ex) {
					// TODO
				}
			}
		}

		public bool IsOpen
		{ 
			get { return port != null && port.IsOpen; } 
		}
		
		private void Close()
		{
			port.DataReceived -= SerialDataReceived;
			port.Close();
		}

		private void Open ()
		{
			port = new SerialPort (portsComboBox.ActiveText, Convert.ToInt32 (baudRateComboBox.ActiveText));
			port.Open ();

			if (!IsOpen)
				throw new IOException ("Cannot open port");

			port.DataReceived += SerialDataReceived;
		}

		public void SendByte (byte b)
		{
			var bytes = new byte[] { b };
			SendBytes (bytes);
		}

		public void SendBytes (byte[] bytes)
		{
			port.Write (bytes, 0, bytes.Length);
			CallDataEventHandler (DataSent, bytes);
		}
	}

	public class DataEventArgs : EventArgs
	{
		public byte[] Bytes 
		{
			get;
			private set;
		}
	
		public DataEventArgs(byte[] data) : base()
		{
			Bytes = data;
		}
	}

	public delegate void DataEventHandler(object sender, DataEventArgs e);
}

