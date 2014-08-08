#ifndef IMAGE_PROVIDER_H
#define IMAGE_PROVIDER_H

#include <gst/gst.h>
#include <gst/app/app.h>
#include "opencv2/core/core.hpp"
#include <opencv2/highgui/highgui.hpp>

static GstPad* decodebin_pad;

class Image_provider
{
public:

GstElement *udp, *rtph264depay, *decodebin,  *pipeline, *appsink;
GstElement *videoconvert, *videoconvert_decodebin, *caps_filter;


Image_provider(int   argc,
		  char *argv[])
{
	/* init GStreamer */
	gst_init (&argc, &argv);
}

Image_provider()
{
	gst_init(NULL,NULL);
}

~Image_provider()
{
	gst_object_unref (GST_OBJECT (pipeline));
}

bool Pipeline_initialization();

void Setting_caps(int port, const char* ip);

bool Linking_pipeline();

bool Start();

void Stop();

cv::Mat Get_Image();

static void on_pad_added (GstElement *element,
						  GstPad     *pad,
						  gpointer    data);

};


#endif
