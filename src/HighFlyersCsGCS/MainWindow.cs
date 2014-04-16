using System;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;

public partial class MainWindow: Gtk.Window
{

	[UI] Gtk.Button button1;
	[UI] Gtk.Label label1;
	int clickedTimes;

	public MainWindow (Builder builder, IntPtr handle): base (handle)
	{
		builder.Autoconnect (this);
		DeleteEvent += OnDeleteEvent;

		button1.Clicked += onButtonClick;
	}

	protected void onButtonClick (object sender, EventArgs a)
	{
		clickedTimes++;
		label1.Text = string.Format ("Hello World! This button has been clicked {0} time(s).", clickedTimes);
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}
}
