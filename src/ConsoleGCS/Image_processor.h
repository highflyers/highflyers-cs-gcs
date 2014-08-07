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
	cv::Mat *main_image, cropped_image;
	cv::Mat imageThresh_;
	int lowH;
	int highH;
	int lowS;
	int highS;
	int lowV;
	int highV;

	cv::Point points[4];
	int how_many_points;

	void thresholding();

	void thresholding2();

	void compute();

	void draw_points();

	static void CallBackFunc(int event, int xx, int yy, int flags, void* userdata);

	bool is_pixel_set(uchar color);

	int get_digit();


public:
	Digit_recognition()
	{
		int lowH = 0;
		int highH = 15;
		int lowS = 10;
		int highS = 255;
		int lowV = 20;
		int highV = 255;
		int how_many_points = 0;
	}

	void Set_main_image(cv::Mat* image);

	int Launch();

};

#endif