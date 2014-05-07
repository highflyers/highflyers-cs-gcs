#include "dummy_processor.h"

bool register_plugin(const Glib::RefPtr<Gst::Plugin>& plugin)
{
	if (!DummyProcessor::register_foo(plugin))
		return false;

	return true;
}

static gboolean plugin_init(GstPlugin *plugin)
{
	Gst::init();
	return register_plugin(Glib::wrap(plugin, true));
}

#define PACKAGE "HFPACKAGE"

GST_PLUGIN_DEFINE(
	GST_VERSION_MAJOR, 
	GST_VERSION_MINOR, 
	hf_algo_lib,
	"HF processing elements",
	plugin_init, 
	"1.0", 
	"GPL", 
	"HFGST",
	"http://uav.polsl.pl")