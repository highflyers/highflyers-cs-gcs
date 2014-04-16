using GLib;
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

		public VideoWidget () : base ()
		{
			Frame frame = new Frame ();
			drawing_area = new DrawingArea ();
			drawing_area.SetSizeRequest (400, 300);
			drawing_area.DoubleBuffered = false;
			frame.Add (drawing_area);

			Add (frame);
			DeleteEvent += OnDeleteEvent;
			frame.ShowAll ();

			BuildPipeline ();
		}

		void BuildPipeline ()
		{
			pipeline = new Pipeline ();
			pipeline.Bus.EnableSyncMessageEmission ();
			pipeline.Bus.AddSignalWatch ();

			pipeline.Bus.SyncMessage += delegate (object bus, SyncMessageArgs sargs) {
				Gst.Message msg = sargs.Message;

				if (!Gst.Video.GlobalVideo.IsVideoOverlayPrepareWindowHandleMessage (msg) || !(msg.Src is Element))
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

			var source = ElementFactory.Make ("videotestsrc", "videos");
			var sink = ElementFactory.Make ("autovideosink", "elemens");
			pipeline.Add (source);
			pipeline.Add (sink);
			source.Link (sink);
		}

		void OnDeleteEvent (object sender, DeleteEventArgs args)
		{
			pipeline.SetState (Gst.State.Null);
			pipeline.Dispose ();
			pipeline = null;
			args.RetVal = true;
		}

		void ChangeStateWithDelay (Gst.State desired_state)
		{
			StateChangeReturn sret = pipeline.SetState (desired_state);

			if (sret == StateChangeReturn.Async) {
				State state, pending;
				sret = pipeline.GetState (out state, out pending, Constants.SECOND * 5L);
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
