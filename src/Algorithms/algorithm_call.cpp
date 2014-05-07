#include <gst/gst.h>

extern "C" {

int dummy_method (GstBuffer* buf)
{
	GstMapInfo map;

	if (gst_buffer_map (buf, &map, GST_MAP_READ))
	{	
		// algorithm code here
		// usage: map.data <- raw data		
  		gst_buffer_unmap (buf, &map);
	}
	
	return gst_buffer_get_size(buf);
}

}
