using System;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;

namespace HighFlyers.GCS
{
	public class MainWindow: Gtk.Window
	{
		[UI] Gtk.Box box2;
		[UI] Gtk.ToggleButton startStopCameraToggleButton;

		VideoWidget video;

		public MainWindow (Builder builder, IntPtr handle): base (handle)
		{
			builder.Autoconnect (this);
			DeleteEvent += OnDeleteEvent;

			startStopCameraToggleButton.Toggled += HandleStartStopClicked;

			video = new VideoWidget (VideoWidget.PipelineType.Test);
			box2.Add (video);
			video.Show ();

		}

		void on_configurationButton_clicked (object sender, EventArgs e)
		{
			try {
				var builder = new Builder (null, "HighFlyers.GCS.interfaces.ConfigurationDialog.ui", null);
				var conf = new ConfigurationDialog (builder, builder.GetObject ("configuration_dialog").Handle);
				conf.Run ();
			} catch (Exception ex) {
				Console.WriteLine (ex.Message);
			}
		}

		void HandleStartStopClicked (object sender, EventArgs e)
		{
			if (startStopCameraToggleButton.Active)
				video.Start ();
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
