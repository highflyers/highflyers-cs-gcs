#include <gst/gst.h>
#include <gst/app/app.h>

#include <Windows.h>

#include "opencv2/core/core.hpp"




using namespace std;

static void on_pad_added (GstElement *element,
						  GstPad     *pad,
						  gpointer    data)
{
  GstPad *sinkpad;
  GstElement *tee = (GstElement *) data;

  sinkpad = gst_element_get_static_pad (tee, "sink");

  gst_pad_link (pad, sinkpad);
  gst_object_unref (sinkpad);
}


int main (int   argc,
		  char *argv[])
{
  GMainLoop *loop;
  int* data;
  /* init GStreamer */
  gst_init (&argc, &argv);

  loop = g_main_loop_new (NULL, FALSE);

  GstElement *udp, *rtph264depay, *decodebin, *videosink, *pipeline, *tee, *appsink;
  GstElement *video_queue, *video_queue_second;

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

  if( !udp || !rtph264depay || !decodebin || !videosink || !pipeline || !tee || !appsink )
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

  gst_bin_add_many (GST_BIN (pipeline),
                    udp, rtph264depay, decodebin, videosink,  tee, video_queue , video_queue_second ,appsink,  NULL);

  gst_element_link_many(udp,rtph264depay,decodebin, NULL);
  gst_element_link_many(video_queue, videosink, NULL);
  gst_element_link_many(video_queue_second, appsink, NULL);

 

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
  


  gst_element_set_state (pipeline, GST_STATE_PLAYING);		//tutaj wszystko rusza

  //g_main_loop_run (loop);

  GstSample * sample = NULL;
  GstBuffer* sampleBuffer = NULL;
  GstMapInfo bufferInfo;


  while (true)
  {
	  sample = gst_app_sink_pull_sample(GST_APP_SINK(appsink));
	  if(sample != NULL)
      {
		  sampleBuffer = gst_sample_get_buffer(sample);
		   if(sampleBuffer != NULL)
        {
			GstCaps* caps_buffer = gst_app_sink_get_caps(GST_APP_SINK(appsink));
            gst_buffer_map(sampleBuffer, &bufferInfo, GST_MAP_READ);

			cv::Mat image = cv::Mat();

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