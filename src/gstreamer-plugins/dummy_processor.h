#ifndef DUMMY_PROCESSOR_H
#define DUMMY_PROCESSOR_H

#include <cassert>
#include <gstreamermm.h>
#include <gstreamermm/private/element_p.h>


class DummyProcessor : public Gst::Element
{
	Glib::RefPtr<Gst::Pad> sinkpad;
    Glib::RefPtr<Gst::Pad> srcpad;
    Glib::Property<Glib::ustring> sample_property;
public:
	static void base_init(Gst::ElementClass<DummyProcessor> *klass)
	{
		klass->set_metadata("Dummy processor",
                "Video/Processing", "Dummy processing", "Marcin Kolny <marcin.kolny@gmail.com>");

        klass->add_pad_template(Gst::PadTemplate::create("sink", Gst::PAD_SINK, Gst::PAD_ALWAYS,
                        Gst::Caps::create_any()));
        klass->add_pad_template(Gst::PadTemplate::create("src", Gst::PAD_SRC, Gst::PAD_ALWAYS,
                        Gst::Caps::create_from_string("video/x-raw, format=I420, width=320, height=240, framerate=30/1, pixel-aspect-ratio=1/1, interlace-mode=progressive")));
    }

    Gst::FlowReturn chain(const Glib::RefPtr<Gst::Pad> &pad, Glib::RefPtr<Gst::Buffer> &buf)
    {
        buf = buf->create_writable();
        assert(buf->gobj()->mini_object.refcount==1);
        Glib::RefPtr<Gst::MapInfo> mapinfo(new Gst::MapInfo());
        buf->map(mapinfo, Gst::MAP_WRITE);
        //std::sort(mapinfo->get_data(), mapinfo->get_data() + buf->get_size());
        for (int i = 0; i < buf->get_size(); i++ ) mapinfo->get_data()[i] = mapinfo->get_data()[buf->get_size()-1-i];
        buf->unmap(mapinfo);
        assert(buf->gobj()->mini_object.refcount==1);
        return srcpad->push(buf);
    }

    explicit DummyProcessor(GstElement *gobj)
        : Glib::ObjectBase(typeid(DummyProcessor)), // type must be registered before use
          Gst::Element(gobj),
          sample_property(*this, "sample_property", "def_val")

    {
        add_pad(sinkpad = Gst::Pad::create(get_pad_template("sink"), "sink"));
        add_pad(srcpad = Gst::Pad::create(get_pad_template("src"), "src"));
        sinkpad->set_chain_function(sigc::mem_fun(*this, &DummyProcessor::chain));
    }

    static bool register_foo(Glib::RefPtr<Gst::Plugin> plugin)
    {
        Gst::ElementFactory::register_element(plugin, "foomm", 10, Gst::register_mm_type<DummyProcessor>("foomm"));

        return true;
    }
};

#endif /* CMLNSRC_H_ */
