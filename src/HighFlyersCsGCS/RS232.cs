using Mono.Unix.Native;
using Mono.Unix;
using System.IO;
using System.Threading;
using System;
using System.Linq;

// For tests purpose, use 
// $ socat -d -d pty,raw,echo=0 pty,raw,echo=0
// command

namespace HighFlyers.GCS
{
	public delegate void DataEventHandler (object sender, DataEventArgs e);
	
	public class RS232
	{
		string port_name;
		int baud_rate;

		int port_descriptor;
		UnixStream stream;
		Thread reader;

		public event DataEventHandler DataReceived;

		public bool IsConnected {
			get {
				// todo implement it!
				return true;
			}
		}

		public RS232 (string port, int baudRate)
		{
			port_name = port;
			baud_rate = baudRate;
		}

		public void Open () 
		{
			port_descriptor = Syscall.open(port_name, OpenFlags.O_RDWR);


			if (port_descriptor == -1)
				throw new IOException ("Cannot open port (but please, don't ask me about the reason)!");

			stream = new UnixStream(port_descriptor);

			
			reader = new Thread (new ThreadStart (ReadData));
			reader.Start ();
		}

		public void Close ()
		{
			reader.Abort ();
			reader.Join ();
			Syscall.close (port_descriptor);
		}

		public void Write (byte[] buf)
		{
			stream.Write (buf, 0, buf.Length);
		}

		public void Write (byte b)
		{
			stream.WriteByte (b);
		}

		void ReadData ()
		{
			while (true) {
				var buf = new byte[1024];
				int len = stream.Read (buf, 0, 1024);

				if (len > 0 && DataReceived != null)
					DataReceived(this, new DataEventArgs (buf.Take(len).ToArray()));
			}
		}
	}

	public class DataEventArgs : EventArgs
	{
		public byte[] Buffer {
			get;
			private set;
		}

		public DataEventArgs (byte[] buffer)
		{
			Buffer = buffer;
		}
	}
}

