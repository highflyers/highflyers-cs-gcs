using System;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;

namespace HighFlyers.GCS
{
	public class MainWindow: Gtk.Window
	{
		[UI] Gtk.Box box2;
		[UI] Gtk.ToggleButton startStopCameraToggleButton;
		[UI] ToggleButton recordCameraToggleButton;
		[UI] ToggleButton connectionToggleButton;
		[UI] ToggleButton hudToggleButton;
		[UI] Box mapBox;
		Map.FileMapWidget map;
		VideoWidget video;
		RS232 serial_port;
		bool connection_click_transaction = false;
		HighFlyers.Protocol.Parser<HighFlyers.Protocol.FrameBuilder> parser = new HighFlyers.Protocol.Parser<HighFlyers.Protocol.FrameBuilder> ();

		public MainWindow (Builder builder, IntPtr handle): base (handle)
		{
			builder.Autoconnect (this);
			DeleteEvent += OnDeleteEvent;

			startStopCameraToggleButton.Toggled += HandleStartStopClicked;

			video = new VideoWidget ();
			video.Expand = true;

			box2.Add (video);
			video.Show ();

			map = new Map.FileMapWidget ();
			map.Expand = true;

			mapBox.Add (map);
			map.Show ();

			parser.FrameParsed += HandleFrameParsed;

			serial_port = new RS232 (AppConfiguration.Instance.GetString ("Communication", "PortName"),
			                         AppConfiguration.Instance.GetInt ("Communication", "BaudRate"));
			serial_port.DataReceived += DataReceivedHandler;
		}

		void HandleFrameParsed (object sender, HighFlyers.Protocol.FrameParsedEventArgs args)
		{
			var data = args.ParsedFrame as HighFlyers.Protocol.Frames.GPSData;

			if (data == null)
				return;

			var ToHumanReadable = new Func<double, string> (v => {
				{
					int h = (int)Math.Floor (v);
					int m = (int)((v - h) * 60);
					double s = (v - h - m / 60.0) * 3600;
					return h + "* " + m + "' " + Math.Round (s, 2) + "''";
				}});

			Gtk.Application.Invoke (delegate {
				if (!hudToggleButton.Active) {
					video.SetOverlay ("");
					return;
				}

				string time = data.time.ToString ();
				time = time.Insert (time.Length - 2, ":");
				time = time.Insert (time.Length - 5, ":");
				video.SetOverlay ("GPS Time: " + time + "\nLatitude: " + ToHumanReadable (data.latitude) + "\nLongitude: " + ToHumanReadable (data.longitude));
			});
		
		}

		void on_configurationButton_clicked (object sender, EventArgs e)
		{
			bool was_connected = serial_port != null && serial_port.IsConnected;
			try {
				var builder = new Builder (null, "HighFlyers.GCS.interfaces.ConfigurationDialog.ui", null);
				var conf = new ConfigurationDialog (builder, builder.GetObject ("configuration_dialog").Handle);
				if (conf.Run () == (int)ResponseType.Ok) {
					video.InitPipeline ();
					if (startStopCameraToggleButton.Active) {
						video.Start ();
					}

					if (was_connected)
						serial_port.Close ();

					serial_port.UpdateParameters (AppConfiguration.Instance.GetString ("Communication", "PortName"),
					                         AppConfiguration.Instance.GetInt ("Communication", "BaudRate"));
					if (was_connected)
						serial_port.Open ();
				}
			} catch (Exception ex) {
				Console.WriteLine (ex.Message);
			}

			try {
				if (was_connected)
					serial_port.Open ();
			} catch (Exception ex) {
				Console.WriteLine ("Cannot reopen port: " + ex.Message);
				connection_click_transaction = true;
				connectionToggleButton.Active = serial_port.IsConnected;
				connection_click_transaction = false;
			}
		}

		protected void on_recordCameraToggleButton_toggled (object sender, EventArgs e)
		{
			if (recordCameraToggleButton.Active) {
				video.StartRecording ();
			} else { 
				video.StopRecording ();
			}
		}

		protected void on_connectionToggleButton_toggled (object sender, EventArgs e)
		{
			if (connection_click_transaction)
				return;

			if (connectionToggleButton.Active) {
				try {
					serial_port.Open ();
				} catch (Exception ex) {
					// todo loggin again...
					Console.WriteLine ("Cannot connect with port: " + ex.Message);
					connection_click_transaction = true;
					connectionToggleButton.Active = serial_port.IsConnected;
					connection_click_transaction = false;
				} finally {
					if (serial_port == null || !serial_port.IsConnected) {
						connection_click_transaction = true;
						connectionToggleButton.Active = false;
						connection_click_transaction = false;
					}
				}
			} else {
				serial_port.Close ();
			}
		}

		void DataReceivedHandler (object sender, DataEventArgs e)
		{
			try {
				parser.AppendBytes (e.Buffer);
			} catch (Exception ex) {
				// todo loggin
				Console.WriteLine (ex.Message);
			}
		}

		void HandleStartStopClicked (object sender, EventArgs e)
		{
			if (startStopCameraToggleButton.Active) {
				video.InitPipeline ();
				video.Start ();
			} else
				video.Stop ();
		}

		protected void OnDeleteEvent (object sender, DeleteEventArgs a)
		{
			Application.Quit ();
			a.RetVal = true;
		}
		// todo write another methods here
		[System.Runtime.InteropServices.DllImport ("libAlgorithms.so")]
		static extern int dummy_method (IntPtr buf);
	}
}
