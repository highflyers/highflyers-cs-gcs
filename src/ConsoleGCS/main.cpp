#include <stdio.h>
#include <gst/gst.h>







static void on_pad_added (GstElement *element,
						  GstPad     *pad,
						  gpointer    data)
{
  GstPad *sinkpad;
  GstElement *videosink = (GstElement *) data;

  sinkpad = gst_element_get_static_pad (videosink, "sink");

  gst_pad_link (pad, sinkpad);
  gst_object_unref (sinkpad);
}


int main (int   argc,
		  char *argv[])
{
  GstElementFactory *factory;

  GMainLoop *loop;
  /* init GStreamer */
  gst_init (&argc, &argv);

  loop = g_main_loop_new (NULL, FALSE);

  GstElement *udp, *rtph264depay, *decodebin, *videosink, *pipeline;

  GstStructure *str;

  pipeline = gst_pipeline_new ("Video-Drone");
  udp = gst_element_factory_make("udpsrc", "udp_source");
  rtph264depay = gst_element_factory_make("rtph264depay", "rtph");
  decodebin = gst_element_factory_make("decodebin", "decoder");
  videosink = gst_element_factory_make("autovideosink", "video_sink");

  if( !udp || !rtph264depay || !decodebin || !videosink || !pipeline)
  {
	  g_printerr ("One element could not be created. Exiting.\n");
	  return -1;
  }

  GstCaps *caps;

  caps = gst_caps_new_simple("application/x-rtp", 
							"multicast-group", G_TYPE_STRING, "192.168.1.40" ,
							"port", G_TYPE_INT, 5004, NULL); 
  
  g_object_set(udp, "caps", caps, NULL);
  gst_caps_unref(caps);

  gst_bin_add_many (GST_BIN (pipeline),
                    udp, rtph264depay, decodebin, videosink, NULL);

  gst_element_link_many(udp,rtph264depay,decodebin, NULL);

  g_signal_connect (decodebin, "pad-added", G_CALLBACK (on_pad_added), videosink);

  gst_element_set_state (pipeline, GST_STATE_PLAYING);		//tutaj wszystko rusza

  g_main_loop_run (loop);

  gst_element_set_state (pipeline, GST_STATE_NULL);
  gst_object_unref (GST_OBJECT (pipeline));
 
  return 0;
}