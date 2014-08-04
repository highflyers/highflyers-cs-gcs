#include <stdio.h>
#include <gst/gst.h>

int
main (int   argc,
      char *argv[])
{
  GstElementFactory *factory;
  /* init GStreamer */
  gst_init (&argc, &argv);

  GstElement *udp, *rtph264depay, *decodebin, *videosink, *pipeline;

  pipeline = gst_pipeline_new ("Video-Drone");
  udp = gst_element_factory_make("udpsrc", "udp_source");
  rtph264depay = gst_element_factory_make("rthp264depay", "rthp");
  decodebin = gst_element_factory_make("decodebin", "decoder");
  videosink = gst_element_factory_make("autovideosink", "video_sink");
 
  return 0;
}