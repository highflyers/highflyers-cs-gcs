#include <gst/gst.h>

extern "C" {

int dummy_method (GstBuffer* buf)
{
	return gst_buffer_get_size(buf);
}

}
