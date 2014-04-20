using System;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;

namespace HighFlyers.GCS
{
	public partial class MainWindow: Gtk.Window
	{
		[UI] Gtk.Button start_player;
		[UI] Gtk.Button stop_player;
		[UI] Gtk.Box box1;

		VideoWidget video;

		public MainWindow (Builder builder, IntPtr handle): base (handle)
		{
			builder.Autoconnect (this);
			DeleteEvent += OnDeleteEvent;

			start_player.Clicked += (sender, e) => video.Start ();
			stop_player.Clicked += (sender, e) => video.Stop ();

			video = new VideoWidget (VideoWidget.PipelineType.Test);
			box1.Add (video);
			video.Show ();
		}

		protected void OnDeleteEvent (object sender, DeleteEventArgs a)
		{
			Application.Quit ();
			a.RetVal = true;
		}
	}
}
