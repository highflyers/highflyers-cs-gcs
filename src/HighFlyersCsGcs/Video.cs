using Gst;
using Gst.Video;
using System;

namespace HighFlyers.GCS
{
	public class Video
	{
		public Pipeline Pipeline
		{ get; private set; }

		ulong windowHandle;

		public Video (ulong windowHandle)
		{
			Application.Init ();

			this.windowHandle = windowHandle;
			CreatePipeline ();
		}

		public void Start ()
		{
			Pipeline.SetState (State.Playing);
		}

		public void Stop ()
		{
			Pipeline.SetState (State.Null);
		}

		private void CreatePipeline ()
		{
			Pipeline = new Pipeline ("HighFlyers.Client.Pipeline");
			Element src = ElementFactory.Make ("videotestsrc");

			Element sink = ElementFactory.Make ("xvimagesink");

			Pipeline.Add (src);
			Pipeline.Add (sink);
			try {
				var _playbin = ElementFactory.Make  ("playbin", "playbin");
			VideoOverlayAdapter adapter = new VideoOverlayAdapter (sink.Handle);
			adapter.WindowHandle = windowHandle;
			adapter.HandleEvents (true);

			}catch (Exception ex) {
				System.Console.WriteLine (ex.Message);
			}
			src.Link (sink);
		}

		public void Configure ()
		{
			
		}




	}
}

