using System;
using Gtk;

namespace HighFlyers.GCS
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Init ();
			Gst.Application.Init ();
			Builder builder = new Builder (null, "HighFlyers.GCS.interfaces.MainWindow.ui", null);
			MainWindow win = new MainWindow (builder, builder.GetObject ("window1").Handle);
			win.Show ();
			Application.Run ();
		}
	}
}
