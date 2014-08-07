#include "Image_provider.h"



void Image_provider::on_pad_added (GstElement *element,
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


int Image_provider::Pipeline_initialization()
{
	pipeline = gst_pipeline_new ("Video-Drone");
	udp = gst_element_factory_make("udpsrc", "udp_source");
	rtph264depay = gst_element_factory_make("rtph264depay", "rtph");
	decodebin = gst_element_factory_make("decodebin", "decoder");
	appsink  = gst_element_factory_make("appsink", "appsink");
	videoconvert = gst_element_factory_make("videoconvert", "videoconvert");
	videoconvert_decodebin = gst_element_factory_make("videoconvert", "videoconvert_decodbin");
	caps_filter = gst_element_factory_make("capsfilter", "capsfilter");


	if( !udp || !rtph264depay || !decodebin
		|| !pipeline  || !appsink || !videoconvert || !videoconvert_decodebin || !caps_filter)
	{
		g_printerr ("One element could not be created. Exiting.\n");
		return -1;
	}
}


void Image_provider::Setting_caps(int port, const char* ip)
{
	//caps for upd element
	GstCaps *caps;
	caps = gst_caps_new_simple("application/x-rtp", NULL);
	g_object_set(udp, "port", port, NULL);
	g_object_set(udp, "multicast-group", ip, NULL);
	g_object_set(udp, "caps", caps, NULL);
	gst_caps_unref(caps); 

	//caps for videoconvert (capsfilter)
	GstCaps *caps_decodbin;
	caps_decodbin = gst_caps_new_simple("video/x-raw", 
		"format", G_TYPE_STRING ,"BGR", NULL); 
	g_object_set(caps_filter, "caps", caps_decodbin, NULL);
	gst_caps_unref(caps_decodbin);
}

int Image_provider::Linking_pipeline()
{
	gst_bin_add_many (GST_BIN (pipeline),
		udp, rtph264depay, decodebin,
		appsink, videoconvert, videoconvert_decodebin, caps_filter,  NULL);


	if( !gst_element_link_many(udp,rtph264depay,decodebin, NULL) ||
		!gst_element_link_many(videoconvert_decodebin, caps_filter, videoconvert, appsink, NULL) )	
	{
		g_printerr ("Error in linking elements.\n");
		return -1; 
	}

	g_signal_connect (decodebin, "pad-added", G_CALLBACK (on_pad_added), videoconvert_decodebin);
}

void Image_provider::Start()
{
	gst_element_set_state (pipeline, GST_STATE_PLAYING);
}

void Image_provider::Stop()
{
	gst_element_set_state (pipeline, GST_STATE_NULL);
}

cv::Mat Image_provider::Get_Image()
{
	GstSample * sample = NULL;
	GstBuffer* sampleBuffer = NULL;
	GstMapInfo bufferInfo;
	gint width, height;
	const GstStructure *str;
	cv::Mat main_image;

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
					//return -1;		//error in reading caps
				}

				main_image = cv::Mat(height,width, CV_8UC3 ,bufferInfo.data);
				gst_buffer_unmap(sampleBuffer, &bufferInfo);
			}
			gst_sample_unref(sample);
		}
	return main_image;
}