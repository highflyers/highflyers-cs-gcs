#include <stdio.h>
#include "opencv2/opencv.hpp"
#include "defines.h"

using namespace std;
using namespace cv;

Mat image;

Mat imageHSV, imageThresh;
int lowH = 10;
int highH = 179;
int lowS = 100;
int highS = 255;
int lowV = 20;
int highV = 255;

bool is_pixel_set(uchar color)
{
	if(color == 0)
		return true;
	else
		return false;
}

void on_change(int, void*)
{
	cvtColor(image, imageHSV, COLOR_BGR2HSV);
	inRange(imageHSV, Scalar(lowH, lowS, lowV), Scalar(highH, highS, highV), imageThresh);

	erode(imageThresh, imageThresh, getStructuringElement(MORPH_ELLIPSE, Size(9,9)));
	dilate(imageThresh, imageThresh, getStructuringElement(MORPH_ELLIPSE, Size(9,9)));

	dilate(imageThresh, imageThresh, getStructuringElement(MORPH_ELLIPSE, Size(9,9)));
	erode(imageThresh, imageThresh, getStructuringElement(MORPH_ELLIPSE, Size(9,9)));

	imshow("Display", imageThresh);
}

int main(int argc, char** argv)
{
	if(argc < 2)
	{
		cout << "Wrong number of parameters" << endl;
		return -1;
	}

	image = imread(argv[1], CV_LOAD_IMAGE_COLOR);

	if(!image.data)
	{
		cout << "Could not open or find the image" << endl;
		return -1;
	}

	namedWindow("Display", CV_WINDOW_AUTOSIZE);
	namedWindow("Original", CV_WINDOW_AUTOSIZE);
	imshow("Original", image);
	createTrackbar("LowH", "Display", &lowH, 179, on_change);
	createTrackbar("HighH", "Display", &highH, 179, on_change);
	createTrackbar("LowS", "Display", &lowS, 255, on_change);
	createTrackbar("HighS", "Display", &highS, 255, on_change);
	createTrackbar("LowV", "Display", &lowV, 255, on_change);
	createTrackbar("HighV", "Display", &highV, 255, on_change);

	for(;;)
	{
		if(waitKey(30) == 27)
		{
			break;
		}
	}
	

	Mat image1 = Mat(imageThresh);
	bitwise_not(imageThresh, image1);

	int x1, x2, y1, y2;
	int digit = D_UNKNOWN;
	int i,j;
	int found_pixels;
	uchar color;
	
	int width = image1.cols;
	int height = image1.rows;

	//int foreground = DARK;

	bool is_left = false;
	for (i = 0; i < width; ++i)
	{
		found_pixels = 0;
		for(j = 0; j < height; ++j)
		{
			color = image1.at<uchar>(j,i);
			if(is_pixel_set(color))
			{
				++found_pixels;
				if(found_pixels > IGNORE_PIXELS)
				{
					if(!is_left)
					{
						x1 = i;
						y1 = 0;
						is_left = true;
					}
					x2 = i;
					y2 = height - 1;
				}
			}
		}
	}

	bool is_top = false;
	for(j = 0; j < height; ++j)
	{
		found_pixels = 0;
		for(i = 0; i < width; ++i)
		{
			color = image1.at<uchar>(j,i);
			if(is_pixel_set(color))
			{
				++found_pixels;
				if(found_pixels > IGNORE_PIXELS)
				{
					if(!is_top)
					{
						y1 = j;
						is_top = true;
					}
					y2 = j;
				}
			}
		}
	}

	// identify digit 1
	if((y2 - y1)/(x2-x1) > ONE_RATIO)
		digit = D_ONE;


    int middle=0, quarter=0, three_quarters=0; /* scanlines */
    int d_height=0; /* height of digit */
    /* if digits[d].digit == D_ONE do nothing */
    if(digit != D_ONE) {
      int third=1; /* in which third we are */
      int half;
      found_pixels=0; /* how many pixels are already found */
      d_height = y2 - y1;
      /* check horizontal segments */
      /* vertical scan at x == middle */
      middle = (x1 + x2) / 2;
      for(j=y1; j<= y2; j++) {
		  color = image1.at<uchar>(j,middle);
        if(is_pixel_set(color)) /* dark i.e. pixel is set */ {
          found_pixels++;
        }
        /* pixels in first third count towards upper segment */
        if(j >= y1 + d_height/3 && third == 1) {
          if(found_pixels >= NEED_PIXELS) {
            digit |= HORIZ_UP; /* add upper segment */
          }
          found_pixels = 0;
          third++;
        } else if(j >= y1 + 2*d_height/3 && third == 2) {
        /* pixels in second third count towards middle segment */
          if(found_pixels >= NEED_PIXELS) {
            digit |= HORIZ_MID; /* add middle segment */
          }
          found_pixels = 0;
          third++;
        }
      }
      /* found_pixels contains pixels of last third */
      if(found_pixels >= NEED_PIXELS) {
        digit |= HORIZ_DOWN; /* add lower segment */
      }
      found_pixels = 0;
      /* check upper vertical segments */
      half=1; /* in which half we are */
      quarter = y1 + (y2 - y1) / 4;
      for(i = x1; i<= x2; i++) {
		  color = image1.at<uchar>(quarter,i);
        if(is_pixel_set(color)) /* dark i.e. pixel is set */ {
          found_pixels++;
        }
        if(i >= middle && half == 1) {
          if(found_pixels >= NEED_PIXELS) {
            digit |= VERT_LEFT_UP;
          }
          found_pixels = 0;
          half++;
        }
      }
      if(found_pixels >= NEED_PIXELS) {
        digit |= VERT_RIGHT_UP;
      }
      found_pixels = 0;
      half = 1;
      /* check lower vertical segments */
      half=1; /* in which half we are */
      three_quarters = y1 + 3 * (y2 - y1) / 4;
      for(i = x1; i<= x2; i++) {
		  color = image1.at<uchar>(three_quarters,i);
        if(is_pixel_set(color)) /* dark i.e. pixel is set */ {
          found_pixels++;
        }
        if(i >= middle && half == 1) {
          if(found_pixels >= NEED_PIXELS) {
            digit |= VERT_LEFT_DOWN;
          }
          found_pixels = 0;
          half++;
        }
      }
      if(found_pixels >= NEED_PIXELS) {
        digit |= VERT_RIGHT_DOWN;
      }
      found_pixels = 0;
    }

	switch(digit) {
      case D_ZERO: cout << "O" << endl; break;
      case D_ONE: cout << "1" << endl; break;
      case D_TWO:cout << "2" << endl;break;
      case D_THREE: cout << "3" << endl;break;
      case D_FOUR: cout << "4" << endl;break;
      case D_FIVE: cout << "5" << endl; break;
      case D_SIX: cout << "6" << endl; break;
      case D_SEVEN: /* fallthrough */
      case D_ALTSEVEN: cout << "7" << endl;break;
      case D_EIGHT: cout << "8" << endl; break;
      case D_NINE: /* fallthrough */
      case D_ALTNINE: cout << "9" << endl; break;
	  default: cout << "UNKNOWN" << endl;
	}

	return 0;
}
