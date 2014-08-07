#include "Image_provider.h"
#include "Image_processor.h"

using namespace std;
using namespace cv;






int main (int   argc, char *argv[])
{
	Digit_recognition digit = Digit_recognition();		//intialization of digit algorithm 
	Image_provider pipeline = Image_provider(argc, argv);
	int key;

	pipeline.Pipeline_initialization();
	pipeline.Setting_caps();
	pipeline.Linking_pipeline();
	pipeline.Start();



	

	namedWindow("original", CV_WINDOW_AUTOSIZE);
	namedWindow("Display", CV_WINDOW_AUTOSIZE);
	setMouseCallback("original", &Digit_recognition::CallBackFunc , &digit);
	while(1)
	{
		digit.Set_main_image(pipeline.Get_Image());				//get image poprawiæ ¿eby dobry obrazek zwraca³
		key = waitKey(10);
		if(key == 13 || key == 10)
		{
			digit.compute();
			digit.thresholding2();
			//get_digit();
			//imshow("Display", cropped_image);
		}
		else if(key == 32)
			{
				printf("%d\n", digit.get_digit());
			}
			else if(key >= 0)
				break;

		digit.compute();
		digit.draw_points();

		imshow("original", digit.Get_main_image());
	}
	destroyAllWindows();


	return 0;
}



