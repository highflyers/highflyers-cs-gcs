using System;
using Gtk;

namespace HighFlyersCsGCS
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Init ();
			Builder builder = new Builder (null, "HighFlyersCsGCS.interfaces.MainWindow.ui", null);
			MainWindow win = new MainWindow (builder, builder.GetObject ("window1").Handle);
			win.Show ();
			Application.Run ();
		}
	}
}
