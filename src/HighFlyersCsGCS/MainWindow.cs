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

		VideoWidget video;
		RS232 serial_port;
		bool connection_click_transaction = false;

		public MainWindow (Builder builder, IntPtr handle): base (handle)
		{
			builder.Autoconnect (this);
			DeleteEvent += OnDeleteEvent;

			startStopCameraToggleButton.Toggled += HandleStartStopClicked;

			video = new VideoWidget ();
			video.Expand = true;

			box2.Add (video);
			video.Show ();
		}

		void on_configurationButton_clicked (object sender, EventArgs e)
		{
			try {
				var builder = new Builder (null, "HighFlyers.GCS.interfaces.ConfigurationDialog.ui", null);
				var conf = new ConfigurationDialog (builder, builder.GetObject ("configuration_dialog").Handle);
				if (conf.Run () == (int)ResponseType.Ok) {
					video.InitPipeline ();
					if (startStopCameraToggleButton.Active) {
						video.Start ();
					}

					bool was_connected = serial_port != null && serial_port.IsConnected;

					if (was_connected)
						serial_port.Close ();

					serial_port = new RS232 (AppConfiguration.Instance.GetString ("Communication", "PortName"),
					                         AppConfiguration.Instance.GetInt ("Communication", "BaudRate"));
					serial_port.DataReceived += (s, ev) => Console.WriteLine (ev.Buffer.Length);
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
			} catch (Exception ex) {
				Console.WriteLine (ex.Message);
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
					// todo probably we don't need create this object everytime
					serial_port = new RS232 (AppConfiguration.Instance.GetString ("Communication", "PortName"),
					                         AppConfiguration.Instance.GetInt ("Communication", "BaudRate"));
					serial_port.DataReceived += (s, ev) => Console.WriteLine (ev.Buffer.Length); // todo don't forget about removing handler!
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

		void HandleStartStopClicked (object sender, EventArgs e)
		{
			if (startStopCameraToggleButton.Active) {
				video.InitPipeline ();
				video.Start ();
			}
			else
				video.Stop ();
		}

		protected void OnDeleteEvent (object sender, DeleteEventArgs a)
		{
			Application.Quit ();
			a.RetVal = true;
		}

		// todo write another methods here
		[System.Runtime.InteropServices.DllImport ("libAlgorithms.so")]
		static extern int dummy_method(IntPtr buf);
	}
}
