// ConsoleGCS.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <gst/gst.h>

int _tmain(int argc, _TCHAR* argv[])
{
	GstElement* source, *sink;
	GstPipeline* pipeline = (GstPipeline*)gst_pipeline_new("pipe");
	source = gst_element_factory_make("videotestsrc", "src");
	sink = gst_element_factory_make("xvimagesink", "sink");

	gst_bin_add_many((GstBin*)pipeline, source, sink, NULL);
	gst_element_link(source, sink);

	gst_element_set_state((GstElement*)pipeline, GST_STATE_PLAYING);

	return 0;
}

