using System;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using System.Text;

namespace HighFlyers.GCS
{
	public class ConfigurationDialog : Dialog
	{
		[UI] TextView pipelineTextView;
		[UI] Notebook videoSourceNotebook;
		[UI] ComboBox videoSourceComboBox;
		[UI] ComboBox testSourceSampleComboBox;
		[UI] ListStore testSourceSampleListStore;
		[UI] Entry portEntry;
		[UI] Entry framerateEntry;
		[UI] Entry widthEntry;
		[UI] Entry heightEntry;
		[UI] ComboBox cameraDeviceComboBox;
		[UI] ListStore videoDeviceListStore;
		[UI] Entry recordedFilenameEntry;
		AppConfiguration settings;

		public ConfigurationDialog (Builder builder, IntPtr handle) : base(handle)
		{
			builder.Autoconnect (this);
			this.settings = AppConfiguration.Instance;
			(AddButton (Stock.Cancel, ResponseType.Cancel) as Button).Clicked += (sender, e) => Destroy ();
			(AddButton (Stock.Ok, ResponseType.Ok) as Button).Clicked += on_ok_button_clicked;

			// todo list enum values instead of numbers
			for (int i = 0; i <= 22; i++)
				testSourceSampleListStore.AppendValues (new string[] { i.ToString() });

			HideVideoSourceTabs ();
			ReloadCameraList ();
			LoadSettings ();
		}

		void ReloadCameraList ()
		{
			videoDeviceListStore.Clear ();

			foreach (var filename in System.IO.Directory.EnumerateFiles ("/dev")) {
				if (filename.StartsWith ("/dev/video", StringComparison.Ordinal)) {
					videoDeviceListStore.AppendValues (new string[] { filename });
				}
			}

			if (videoDeviceListStore.IterNChildren() > 0) {
				cameraDeviceComboBox.Active = 0;
			}
		}

		void LoadSettings ()
		{
			pipelineTextView.Buffer.Text = settings.GetString ("Video", "CustomPipeline");
			recordedFilenameEntry.Text = settings.GetString ("Video", "Filename");
			videoSourceComboBox.Active = settings.GetInt ("Video", "Source");
			testSourceSampleComboBox.Active = settings.GetInt ("Video", "TestPattern");
			portEntry.Text = settings.GetInt ("Video", "UDPPort").ToString ();
			widthEntry.Text = settings.GetInt ("Video", "Width").ToString ();
			heightEntry.Text = settings.GetInt ("Video", "Height").ToString ();
			framerateEntry.Text = settings.GetInt ("Video", "Framerate").ToString ();
		}

		protected void on_ok_button_clicked (object sender, EventArgs e)
		{
			try {
				settings.SetString ("Video", "Filename", recordedFilenameEntry.Text);
				settings.SetInt ("Video", "Source", videoSourceComboBox.Active);
				settings.SetInt ("Video", "TestPattern", testSourceSampleComboBox.Active);
				settings.SetInt ("Video", "UDPPort", Convert.ToInt32(portEntry.Text));
				settings.SetInt ("Video", "Width", Convert.ToInt32(widthEntry.Text));
				settings.SetInt ("Video", "Height", Convert.ToInt32(heightEntry.Text));
				settings.SetInt ("Video", "Framerate", Convert.ToInt32(framerateEntry.Text));

				TreeIter tree;
				cameraDeviceComboBox.GetActiveIter (out tree);
				var selectedText = (String)cameraDeviceComboBox.Model.GetValue (tree, 0);

				settings.SetString ("Video", "V4l2Device", selectedText);
				settings.SetString ("Video", "CustomPipeline", pipelineTextView.Buffer.Text);

			} catch (Exception ex) {
				// todo improve log
				Console.WriteLine ("Cannot save pipeline: " + ex.Message);
			}
			settings.Save ();
			Destroy ();
		}

		protected void on_videoSourceComboBox_changed (object sender, EventArgs e)
		{
			HideVideoSourceTabs ();
		}

		protected void on_reloadCameraListButton_clicked (object sender, EventArgs e)
		{
			ReloadCameraList ();
		}

		protected void on_chooseRecordedFilenameButton_clicked (object sender, EventArgs e)
		{
			var chooser = new FileChooserDialog ("Select File", Toplevel as Gtk.Window, FileChooserAction.Save);
			chooser.AddButton (Stock.Cancel, ResponseType.Cancel);
			chooser.AddButton (Stock.Ok, ResponseType.Ok);

			if (chooser.Run () == (int)ResponseType.Ok) {
				recordedFilenameEntry.Text = chooser.Filename;
			}
			chooser.Destroy ();
		}

		void HideVideoSourceTabs ()
		{
			for (int i = 0; i < videoSourceNotebook.NPages; i++) {
				var page = videoSourceNotebook.GetNthPage (i);

				if (videoSourceComboBox.Active == i) {
					page.Show ();
				} else {
					page.Hide ();
				}
			}
		}
	}
}

