#ifndef IMAGE_PROCESSOR_H
#define IMAGE_PROCESSOR_H

#include <stdio.h>
#include <string.h>
#include <vector>
#include "opencv2/opencv.hpp"
#include "defines.h"


class Digit_recognition
{
private:
	cv::Mat main_image, cropped_image;
	cv::Mat imageThresh_;
	int lowH;
	int highH;
	int lowS;
	int highS;
	int lowV;
	int highV;

	cv::Point points[4];
	int how_many_points;

public:
	Digit_recognition()
	{
		lowH = 0;
		highH = 15;
		lowS = 10;
		highS = 255;
		lowV = 20;
		highV = 255;
		how_many_points = 0;
	}

	void Set_main_image(cv::Mat image);

	cv::Mat Get_main_image();

	void thresholding();

	void thresholding2();

	void compute();

	void draw_points();

	static void CallBackFunc(int event, int xx, int yy, int flags, void* userdata);

	bool is_pixel_set(uchar color);

	int get_digit();

	bool can_compute() const;
};

#endif
