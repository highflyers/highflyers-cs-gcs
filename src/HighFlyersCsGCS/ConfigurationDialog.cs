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
		StringBuilder pipeline_builder;

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
			pipelineTextView.Buffer.Text = settings.GetString ("Video", "Pipeline");
			recordedFilenameEntry.Text = settings.GetString ("Video", "Filename");
		}

		protected void on_ok_button_clicked (object sender, EventArgs e)
		{
			try {
				BuildPipeline ();
				settings.SetString ("Video", "Pipeline", pipeline_builder.ToString ());
				settings.SetString ("Video", "Filename", recordedFilenameEntry.Text);
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

		void BuildPipeline ()
		{
			pipeline_builder = new StringBuilder ();

			switch (videoSourceComboBox.Active) {
			case 0:
				BuildTestPipeline ();
				break;
			case 1:
				BuildRtpPipeline ();
				break;
			case 2:
				BuildV4l2SrcPipeline ();
				break;
			case 3:
				BuildCustomPipeline ();
				break;
			default:
				throw new Exception ("Unknown video source type");
			}

			pipeline_builder.Append (" ! autovideosink");
		}

		void BuildTestPipeline ()
		{
			pipeline_builder.AppendFormat ("videotestsrc pattern={0}", testSourceSampleComboBox.Active);
		}

		void BuildRtpPipeline ()
		{
			pipeline_builder.AppendFormat ("udpsrc port={0}", Convert.ToInt32 (portEntry.Text));
			pipeline_builder.Append (" ! application/x-rtp, payload=96");
			pipeline_builder.Append (" ! rtpjitterbuffer mode=slave latency=200 drop-on-latency=true");
			pipeline_builder.Append (" ! rtph264depay");
			pipeline_builder.AppendFormat (" ! video/x-h264, width={0}, height={1}, framerate={2}/1", 
			                               Convert.ToInt32 (widthEntry.Text),
			                               Convert.ToInt32 (heightEntry.Text), 
			                               Convert.ToInt32 (framerateEntry.Text));
			pipeline_builder.Append (" ! avdec_h264 ! videoconvert");
		}

		void BuildV4l2SrcPipeline ()
		{
			TreeIter tree;
			cameraDeviceComboBox.GetActiveIter (out tree);
			var selectedText = (String)cameraDeviceComboBox.Model.GetValue (tree, 0);
			pipeline_builder.AppendFormat ("v4l2src device={0}", selectedText);
		}

		void BuildCustomPipeline ()
		{
			pipeline_builder.Append (pipelineTextView.Buffer.Text);
		}
	}
}

