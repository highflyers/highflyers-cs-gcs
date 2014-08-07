#include <gst/gst.h>
#include <gst/app/app.h>

#include <Windows.h>

#include "opencv2/core/core.hpp"
#include <opencv2/highgui/highgui.hpp>



using namespace std;
using namespace cv;

GstPad * decodebin_pad;

static void on_pad_added (GstElement *element,
						  GstPad     *pad,
						  gpointer    data)
{
	GstPad *sinkpad;
	GstElement *tee = (GstElement *) data;

	sinkpad = gst_element_get_static_pad (tee, "sink");
	decodebin_pad = pad;
	gst_pad_link (pad, sinkpad);
	gst_object_unref (sinkpad);
}


int main (int   argc,
		  char *argv[])
{
	/* init GStreamer */
	gst_init (&argc, &argv);

	GstElement *udp, *rtph264depay, *decodebin, *videosink, *pipeline, *tee, *appsink;
	GstElement *video_queue, *video_queue_second, *videoconvert, *videoconvert_decodebin, *caps_filter;

	GstPadTemplate *tee_src_pad_template;
	GstPad *tee_video_pad, *tee_video_pad_second;
	GstPad *queue_video_pad, *queue_video_pad_second;

	pipeline = gst_pipeline_new ("Video-Drone");
	udp = gst_element_factory_make("udpsrc", "udp_source");
	rtph264depay = gst_element_factory_make("rtph264depay", "rtph");
	decodebin = gst_element_factory_make("decodebin", "decoder");
	videosink = gst_element_factory_make("autovideosink", "video_sink");
	tee = gst_element_factory_make("tee", "tee");
	appsink  = gst_element_factory_make("appsink", "appsink");
	video_queue = gst_element_factory_make ("queue", "video_queue");
	video_queue_second = gst_element_factory_make ("queue", "video_queue_second");
	videoconvert = gst_element_factory_make("videoconvert", "videoconvert");
	videoconvert_decodebin = gst_element_factory_make("videoconvert", "videoconvert_decodbin");
	caps_filter = gst_element_factory_make("capsfilter", "capsfilter");


	if( !udp || !rtph264depay || !decodebin || !videosink 
		|| !pipeline || !tee || !appsink || !videoconvert || !videoconvert_decodebin || !caps_filter)
	{
		g_printerr ("One element could not be created. Exiting.\n");
		return -1;
	}

	GstCaps *caps;
	caps = gst_caps_new_simple("application/x-rtp", 
		"multicast-group", G_TYPE_STRING, "192.168.1.201" ,
		"port", G_TYPE_INT, 5004, NULL); 

	g_object_set(udp, "caps", caps, NULL);
	gst_caps_unref(caps);

	//caps for videoconvert (capsfilter)
	GstCaps *caps_decodbin;
	caps_decodbin = gst_caps_new_simple("video/x-raw", 
		"format", G_TYPE_STRING ,"RGB", NULL); 
	g_object_set(caps_filter, "caps", caps_decodbin, NULL);
	gst_caps_unref(caps_decodbin);


	gst_bin_add_many (GST_BIN (pipeline),
		udp, rtph264depay, decodebin, videosink,  tee, video_queue , video_queue_second, 
		appsink, videoconvert, videoconvert_decodebin, caps_filter,  NULL);


	if( !gst_element_link_many(udp,rtph264depay,decodebin, NULL) ||
		!gst_element_link_many(video_queue, videosink, NULL) ||
		!gst_element_link_many(video_queue_second, videoconvert, caps_filter,videoconvert_decodebin, appsink, NULL) )	
	{
		g_printerr ("Error in linking elements.\n");
		return -1; 
	}


	tee_src_pad_template = gst_element_class_get_pad_template (GST_ELEMENT_GET_CLASS (tee), "src_%u");
	tee_video_pad = gst_element_request_pad (tee, tee_src_pad_template, NULL, NULL);
	tee_video_pad_second = gst_element_request_pad (tee, tee_src_pad_template, NULL, NULL);
	queue_video_pad = gst_element_get_static_pad (video_queue, "sink");
	queue_video_pad_second = gst_element_get_static_pad (video_queue_second, "sink");

	if (gst_pad_link (tee_video_pad, queue_video_pad) != GST_PAD_LINK_OK ||
		gst_pad_link (tee_video_pad_second, queue_video_pad_second) != GST_PAD_LINK_OK) {
			g_printerr ("Tee could not be linked.\n");
			gst_object_unref (pipeline);
			return -1;
	}
	gst_object_unref (queue_video_pad);
	gst_object_unref (queue_video_pad_second);

	g_signal_connect (decodebin, "pad-added", G_CALLBACK (on_pad_added), tee);


	gst_element_set_state (pipeline, GST_STATE_PLAYING);		//here it start


	GstSample * sample = NULL;
	GstBuffer* sampleBuffer = NULL;
	GstMapInfo bufferInfo;
	gint width, height;
	const GstStructure *str;

	while (true)
	{
		sample = gst_app_sink_pull_sample(GST_APP_SINK(appsink));
		if(sample != NULL)
		{
			sampleBuffer = gst_sample_get_buffer(sample);
			if(sampleBuffer != NULL)
			{
				GstCaps* caps_buffer = gst_pad_query_caps(decodebin_pad, NULL);

				

				gst_buffer_map(sampleBuffer, &bufferInfo, GST_MAP_READ);

				str = gst_caps_get_structure (caps_buffer, 0);
				if (!gst_structure_get_int (str, "width", &width) ||
					!gst_structure_get_int (str, "height", &height))
				{
					return -1;		//error in reading caps
				}

				Mat image = cv::Mat(height,width, CV_8UC3 ,bufferInfo.data);

				namedWindow( "Display window", WINDOW_AUTOSIZE );
				imshow( "Display window", image );
				waitKey(1);

				gst_buffer_unmap(sampleBuffer, &bufferInfo);
			}
			gst_sample_unref(sample);
		}
	}


	gst_element_release_request_pad (tee, tee_video_pad);
	gst_element_release_request_pad (tee, tee_video_pad_second);
	gst_object_unref (tee_video_pad);
	gst_object_unref (tee_video_pad_second);


	gst_element_set_state (pipeline, GST_STATE_NULL);
	gst_object_unref (GST_OBJECT (pipeline));

	return 0;
}