#include "Image_processor.h"

using namespace cv;


void Digit_recognition::Set_main_image(Mat* image)
{
	main_image = image;
}

void Digit_recognition::thresholding()
{
	Mat imageHSV_;
	cvtColor(cropped_image, imageHSV_, COLOR_BGR2HSV);
	inRange(imageHSV_, Scalar(lowH, lowS, lowV), Scalar(highH, highS, highV), imageThresh_);

	erode(imageThresh_, imageThresh_, getStructuringElement(MORPH_ELLIPSE, Size(5,5)));
	dilate(imageThresh_, imageThresh_, getStructuringElement(MORPH_ELLIPSE, Size(5,5)));

	dilate(imageThresh_, imageThresh_, getStructuringElement(MORPH_ELLIPSE, Size(5,5)));
	erode(imageThresh_, imageThresh_, getStructuringElement(MORPH_ELLIPSE, Size(5,5)));

	imshow("Display", imageThresh_);
}

void Digit_recognition::thresholding2()
{
	Mat imageHSV_;
	printf("%d %d\n", cropped_image.rows, cropped_image.cols);

	Mat channel;
	vector<Mat> channels(3);
	split(cropped_image, channels);

	channel = channels[2];

	//cvtColor(cropped_image,imageHSV_, COLOR_BGR2HSV);
	//cvtColor(cropped_image, imageHSV_, COLOR_BGR2GRAY);
	//adaptiveThreshold(channel, imageThresh_, 100, ADAPTIVE_THRESH_MEAN_C,THRESH_BINARY_INV, 25,10);
	threshold(channel, imageThresh_, 0, 255, CV_THRESH_BINARY | CV_THRESH_OTSU);
	//inRange(imageHSV_, Scalar(lowH, lowS, lowV), Scalar(highH, highS, highV), imageThresh_);

	//erode(imageThresh_, imageThresh_, getStructuringElement(MORPH_ELLIPSE, Size(5,5)));	
	//Mat image2, image3, image4;
	erode(imageThresh_, imageThresh_, getStructuringElement(MORPH_ELLIPSE, Size(3,3)));
	//cvtColor(cropped_image, image2, COLOR_BGR2HSV);
	//inRange(image2, Scalar(lowH, lowS, lowV), Scalar(highH, highS, highV), image3);
	Mat horizontal, vertical;
	//erode(imageThresh_, horizontal, getStructuringElement(MORPH_RECT, Size(13,3)));

	//erode(imageThresh_, vertical, getStructuringElement(MORPH_RECT, Size(13, 3)));
	//bitwise_or(horizontal, vertical, imageThresh_);

	imshow("Display", imageThresh_);
}

void Digit_recognition::compute()
{
	// find max and min coordinates
	if(how_many_points <4)
		return;
	
	int xmin = points[0].x;
	int ymin = points[0].y;
	int xmax = points[0].x;
	int ymax = points[0].y;

	for(int i = 0; i < 4; ++i)
	{
		if(xmax < points[i].x)
			xmax = points[i].x;

		if(ymax < points[i].y)
			ymax = points[i].y;

		if(xmin > points[i].x)
			xmin = points[i].x;

		if(ymin > points[i].y)
			ymin = points[i].y;
	}

	Rect roi(xmin  , ymin , xmax - xmin , ymax - ymin );
	cropped_image = (*main_image)(roi);
	rectangle(*main_image, Point(xmin -2 , ymin - 2), Point(xmax + 2,ymax + 2), Scalar(0,255,255),2,8);
	//imshow("original", main_image);


	//on_change(0, NULL);
}

void Digit_recognition::draw_points()
{
	int counter = how_many_points > 4 ? 4 : how_many_points;
	for(int i = 0; i < counter; ++i)
	{
		circle(*main_image, points[i], 2, Scalar(255,0,255),10,8,0);
		circle(*main_image, points[i], 2, Scalar(255,255,0),2,8,0);
	}
}

 void Digit_recognition::CallBackFunc(int event, int xx, int yy, int flags, void* userdata)
{
	Digit_recognition *temp = static_cast<Digit_recognition*>(userdata);
	if(event == EVENT_LBUTTONUP)
	{
		temp->points[temp->how_many_points % 4] = Point(xx,yy);
		temp->how_many_points++;
	}
}

bool Digit_recognition::is_pixel_set(uchar color)
{
	if(color > 0)
		return true;
	else
		return false;
}

int Digit_recognition::get_digit()
{
	Mat image1 = Mat(imageThresh_);

	int x1, x2, y1, y2;
	int digit = D_UNKNOWN;
	int i,j;
	int found_pixels;
	uchar color;
	
	int width = image1.cols;
	int height = image1.rows;

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
				++found_pixels; //Vertex of the rectangl
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
      case D_ZERO: 		return 0; break;
      case D_ONE:  		return 1; break;
      case D_TWO:  		return 2; break;
      case D_THREE: 	return 3; break;
      case D_FOUR: 		return 4; break;
      case D_FIVE:		return 5; break;
      case D_SIX:		return 6; break;
      case D_SEVEN: /* fallthrough */
      case D_ALTSEVEN: 	return 7; break;
      case D_EIGHT:		return 8; break;
      case D_NINE: /* fallthrough */
      case D_ALTNINE:	return 9; break;
	  default: 			return -1;
	}

	
}


int Digit_recognition::Launch()
{
	int key;
	
	namedWindow("original", CV_WINDOW_AUTOSIZE);
	namedWindow("Display", CV_WINDOW_AUTOSIZE);
	setMouseCallback("original", &Digit_recognition::CallBackFunc , this);

	while(1)
	{
		key = waitKey(10);
		if(key == 13 || key == 10)
		{
			compute();
			thresholding2();
			//get_digit();
			//imshow("Display", cropped_image);
		}
		else if(key == 32)
			{
				printf("%d\n", get_digit());
			}
			else if(key >= 0)
				break;

		compute();
		draw_points();

		imshow("original", *main_image);
	}
	destroyAllWindows();
	return 0;
}

