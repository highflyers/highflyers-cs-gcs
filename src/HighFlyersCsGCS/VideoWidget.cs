using Gtk;
using System;
using Gst;
using Gst.Video;
using System.Runtime.InteropServices;

namespace HighFlyers.GCS
{
	class VideoWidget : Gtk.Bin
	{
		DrawingArea drawing_area;
		Pipeline pipeline;

		public enum PipelineType
		{
			Rtp,
			Test
		}

		public VideoWidget (PipelineType pipelineType)
		{
			Frame frame = new Frame ();
			drawing_area = new DrawingArea ();
			drawing_area.SetSizeRequest (400, 300);
			drawing_area.DoubleBuffered = false;
			frame.Add (drawing_area);

			Add (frame);
			DeleteEvent += OnDeleteEvent;
			frame.ShowAll ();

			InitPipeline ();
			switch (pipelineType) {
			case PipelineType.Rtp:
				BuildRtpPipeline ();
				break;
			case PipelineType.Test:
				BuildTestPipeline ();
				break;
			default:
				throw new NotImplementedException ("Not implemented pipeline type: " + pipelineType);
			}
		}

		void InitPipeline ()
		{
			pipeline = new Pipeline ();
			pipeline.Bus.EnableSyncMessageEmission ();
			pipeline.Bus.AddSignalWatch ();

			pipeline.Bus.SyncMessage += delegate (object bus, SyncMessageArgs sargs) {
				Gst.Message msg = sargs.Message;

				if (!GlobalVideo.IsVideoOverlayPrepareWindowHandleMessage (msg) || !(msg.Src is Element))
					return;

				VideoOverlayAdapter adapter = new VideoOverlayAdapter (msg.Src.Handle);
				adapter.WindowHandle = gdk_x11_window_get_xid (drawing_area.Window.Handle);
			};

			pipeline.Bus.Message += delegate (object bus, MessageArgs margs) {
				Message message = margs.Message;

				switch (message.Type) {
					case Gst.MessageType.Error:
					GLib.GException err;
					string msg;
					message.ParseError (out err, out msg);
					Console.WriteLine (String.Format ("Error message: {0}", msg));
					break;
					case Gst.MessageType.Eos:
					Console.WriteLine ("EOS");
					break;
				}
			};
		}

		void BuildRtpPipeline ()
		{	
			Element source = ElementFactory.Make ("udpsrc", "source"),
			appFilter = ElementFactory.Make ("capsfilter", "appfilter"),
			jitter = ElementFactory.Make ("rtpjitterbuffer", "jitter"),
			depay = ElementFactory.Make ("rtph264depay", "depay"),
			videoFilter = ElementFactory.Make ("capsfilter", "videofilter"),
			decoder = ElementFactory.Make ("avdec_h264", "decoder"),
			converter = ElementFactory.Make ("videoconvert", "converter"),
			sink = ElementFactory.Make ("autovideosink", "sink");

			source ["port"] = 5000;
			appFilter ["caps"] = "application/x-rtp, payload=96";
			jitter ["mode"] = 1;
			jitter ["latency"] = 200;
			jitter ["drop-on-latency"] = true;
			videoFilter ["caps"] = "video/x-h-264, width=640, height=480,, framerate=15/1";
			
			pipeline.Add (source);pipeline.Add (appFilter); pipeline.Add (jitter);
			pipeline.Add (depay); pipeline.Add (videoFilter); pipeline.Add (decoder);
			pipeline.Add (converter); pipeline.Add (sink);
			source.Link (appFilter); appFilter.Link (jitter); jitter.Link (depay); depay.Link (videoFilter);
			videoFilter.Link (decoder);decoder.Link (converter); converter.Link (sink);
		}

		void BuildTestPipeline ()
		{
			Element source = ElementFactory.Make ("videotestsrc", "videos"),
			sink = ElementFactory.Make ("autovideosink", "elemens");
			pipeline.Add (source);
			pipeline.Add (sink);
			source.Link (sink);
		}

		void OnDeleteEvent (object sender, GLib.SignalArgs args)
		{
			pipeline.SetState (Gst.State.Null);
			pipeline.Dispose ();
			pipeline = null;
			args.RetVal = true;
		}

		void ChangeStateWithDelay (State desiredState)
		{
			StateChangeReturn sret = pipeline.SetState (desiredState);

			if (sret == StateChangeReturn.Async) {
				State state, pending;
				sret = pipeline.GetState (out state, out pending, Constants.SECOND * 15L);
			}

			if (sret == StateChangeReturn.Success) {
				// TODO find another way to log succesful operations
				Console.WriteLine ("State change successful");
			} else {
				// TODO is it the best way to notify about errors? Maybe bool instead?
				throw new Exception (String.Format ("State change failed: {0} \n", sret));
			}
		}

		public void Start ()
		{
			ChangeStateWithDelay (Gst.State.Playing);
		}

		public void Stop ()
		{
			ChangeStateWithDelay (Gst.State.Null);
		}

		[DllImport ("libgdk-3.so.0") ]
		static extern uint gdk_x11_window_get_xid (IntPtr handle);
	}
}
