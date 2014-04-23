using System;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;

namespace HighFlyers.GCS
{
	public class ConfigurationDialog : Gtk.Dialog
	{
		[UI] TextView pipelineTextView;

		AppConfiguration settings;

		public ConfigurationDialog (Builder builder, IntPtr handle) : base(handle)
		{
			builder.Autoconnect (this);
			this.settings = AppConfiguration.Instance;
			(AddButton (Stock.Cancel, ResponseType.Cancel) as Button).Clicked += (sender, e) => Destroy ();
			(AddButton (Stock.Ok, ResponseType.Ok) as Button).Clicked += on_ok_button_clicked;

			LoadSettings ();
		}

		private void LoadSettings ()
		{
			pipelineTextView.Buffer.Text = settings.GetString ("Video", "Pipeline");
		}

		protected void on_ok_button_clicked(object sender, EventArgs e)
		{
			settings.SetString ("Video", "Pipeline", pipelineTextView.Buffer.Text);

			settings.Save ();
			Destroy ();
		}
	}
}

