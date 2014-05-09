using System;
using System.Text;
using System.IO;

namespace HighFlyers.GCS
{
	public class PipelineBuilder
	{
		public enum PipelineType
		{
			Test,
			Rtp,
			V4l2,
			Custom
		};

		StringBuilder pipeline_builder;
		AppConfiguration settings;
		string filename = String.Empty;

		public PipelineBuilder ()
		{
			settings = AppConfiguration.Instance;
		}

		public Gst.Pipeline BuildPipeline ()
		{
			pipeline_builder = new StringBuilder ();
			var type = (PipelineType)settings.GetInt ("Video", "Source");

			switch (type) {
				case PipelineType.Test:
				BuildTestPipeline ();
				break;
			case PipelineType.Rtp:
				BuildRtpPipeline ();
				break;
			case PipelineType.V4l2:
				BuildV4l2SrcPipeline ();
				break;
			case PipelineType.Custom:
				BuildCustomPipeline ();
				break;
			default:
				throw new Exception ("Unknown video source type");
			}

			if (type != PipelineType.Custom) {
				pipeline_builder.Append (" ! textoverlay name=toverlay xpos=30 ypos=30 halignment=left valignment=top font-desc=\"arial 22\" ! autovideosink");
			}

			string built_pipeline = pipeline_builder.ToString ();

			if (filename != String.Empty) {
				int index = built_pipeline.LastIndexOf (" ! autovideosink", StringComparison.Ordinal);

				if (index != -1 && built_pipeline.IndexOf ("filesink") == -1) {
					built_pipeline = built_pipeline.Remove (index);

					ImproveFilename ();
					built_pipeline += " ! tee name=my_videosink ! queue ! autovideosink my_videosink. ! queue ! avenc_h263 ! avimux ! filesink location=" + filename;
				}
			}
												//here is some compilation error !!!!
			Logger.Instance.Log (LogLevel.Info, "Created pipeline: " + built_pipeline);op  ! appsink name=buffer_producer";
			return Gst.Parse.Launch (built_pipeline) as Gst.Pipeline;
		}

		public void AppendRecorder ()
		{
			filename = settings.GetString ("Video", "Filename");
		}

		public void RemoveRecorder ()
		{
			filename = String.Empty;
		}

		void BuildTestPipeline ()
		{
			pipeline_builder.AppendFormat ("videotestsrc pattern={0}", settings.GetInt ("Video", "TestPattern"));
		}

		void BuildRtpPipeline ()
		{
			pipeline_builder.AppendFormat ("udpsrc port={0} caps=\"application/x-rtp, media=(string)video, clock-rate=(int)90000, encoding-name=(string)H264\"", settings.GetInt ("Video", "UDPPort"));
			pipeline_builder.Append (" ! rtph264depay");
			pipeline_builder.AppendFormat (" ! video/x-h264, width={0}, height={1}, framerate={2}/1", 
			                               settings.GetInt ("Video", "Width"), settings.GetInt ("Video", "Height"),
			                               settings.GetInt ("Video", "Framerate"));
			pipeline_builder.Append (" ! avdec_h264 ! videoconvert");
		}

		void BuildV4l2SrcPipeline ()
		{
			pipeline_builder.AppendFormat ("v4l2src device={0}", settings.GetString ("Video", "V4l2Device"));
		}

		
		void BuildCustomPipeline ()
		{
			pipeline_builder.Append (settings.GetString ("Video", "CustomPipeline"));
		}

		void ImproveFilename ()
		{
			if (String.IsNullOrEmpty (filename))
				return;

			int i = 0;

			while (File.Exists(filename)) {
				filename = Path.Combine (Path.GetDirectoryName (filename), Path.GetFileNameWithoutExtension (filename)
				                         + i + Path.GetExtension (filename));
				i++;
			}
		}
	}
}

