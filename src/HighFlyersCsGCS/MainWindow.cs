using System;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;

namespace HighFlyers.GCS
{
	public class MainWindow: Gtk.Window
	{
		[UI] Gtk.Box box2;
		[UI] Gtk.ToggleButton startStopCameraToggleButton;

		Builder builder;
		VideoWidget video;

		const string settingsFileName = "settings.xml";
		GLib.KeyFile settings;

		public MainWindow (Builder builder, IntPtr handle): base (handle)
		{
			builder.Autoconnect (this);
			DeleteEvent += OnDeleteEvent;

			startStopCameraToggleButton.Toggled += HandleStartStopClicked;

			video = new VideoWidget (VideoWidget.PipelineType.Test);
			box2.Add (video);
			video.Show ();
		
			this.builder = builder;

			settings = (System.IO.File.Exists (settingsFileName)) ? 
				new GLib.KeyFile (settingsFileName, GLib.KeyFileFlags.KeepComments) :
				new GLib.KeyFile ();
		}

		void on_configurationButton_clicked (object sender, EventArgs e)
		{
			(builder.GetObject ("configuration_window")as Window).Show ();
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
