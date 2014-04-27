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
		string rec_filename;
		bool recorder = false;

		public VideoWidget ()
		{
			Frame frame = new Frame ();
			drawing_area = new DrawingArea ();
			drawing_area.SetSizeRequest (400, 300);
			drawing_area.DoubleBuffered = false;
			frame.Add (drawing_area);

			Add (frame);
			DeleteEvent += OnDeleteEvent;
			frame.ShowAll ();

		}

		public void InitPipeline ()
		{
			if (pipeline != null) {
				ChangeStateWithDelay (Gst.State.Null);
			}

			string p = AppConfiguration.Instance.GetString ("Video", "Pipeline");

			if (recorder) {
				int index = p.LastIndexOf (" ! autovideosink", StringComparison.Ordinal);
				p = p.Remove (index);
				p += " ! tee name=my_videosink ! queue ! autovideosink my_videosink. ! queue ! avenc_h263 ! avimux ! filesink location=" + rec_filename;
			}

			pipeline = Parse.Launch (p) as Pipeline;
			pipeline.Bus.EnableSyncMessageEmission ();
			pipeline.Bus.AddSignalWatch ();

			pipeline.Bus.SyncMessage += delegate (object bus, SyncMessageArgs sargs) {
				Message msg = sargs.Message;

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

		public void StartRecording (string filename)
		{
			recorder = true;
			rec_filename = filename;

			if (IsPlaying) {
				Stop ();
				InitPipeline ();
				Start ();
			}
		}

		public void StopRecording ()
		{
			recorder = false;

			if (IsPlaying) {
				Stop ();
				InitPipeline ();
				Start ();
			}
		}

		public bool IsPlaying {
			get {
				if (pipeline != null) {
					Gst.State state, pending;
					pipeline.GetState (out state, out pending, 0);
					return state == Gst.State.Playing;
				}
				return false;
			}
		}


		[DllImport ("libgdk-3.so.0") ]
		static extern uint gdk_x11_window_get_xid (IntPtr handle);
	}
}
