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

		VideoWidget video;

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
				if (conf.Run () == (int) ResponseType.Ok) {
					video.InitPipeline();
					if (startStopCameraToggleButton.Active) {
						video.Start ();
					}
				}

			} catch (Exception ex) {
				Console.WriteLine (ex.Message);
			}
		}

		protected void on_recordCameraToggleButton_toggled (object sender, EventArgs e)
		{
			if (recordCameraToggleButton.Active) {
				video.StartRecording ("filename");
			} else { 
				video.StopRecording ();
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
	}
}
