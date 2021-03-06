#include "Image_provider.h"
#include "Image_processor.h"

using namespace std;
using namespace cv;

void log (const std::string& msg)
{
	cout << "Log: " << msg << endl;
}

int main (int   argc, char *argv[])
{
	Digit_recognition digit = Digit_recognition();		//intialization of digit algorithm 
	Image_provider pipeline = Image_provider(argc, argv);
	int key;

	if (!pipeline.Pipeline_initialization())
	{
		log ("Cannot initialize pipeline");
		return 0;
	}

	int port = argc > 2 ? atoi(argv[2]) : 5004;
	const char* ip = argc > 2 ? argv[1] : "192.168.1.60";
	pipeline.Setting_caps(port, ip);

	if (!pipeline.Linking_pipeline())
	{
		log ("Cannot link pipeline");
		return 0;
	}

	if (!pipeline.Start())
	{
		log ("Cannot start pipeline");
		return 0;
	}

	namedWindow("original", CV_WINDOW_AUTOSIZE);
	namedWindow("Display", CV_WINDOW_AUTOSIZE);
	setMouseCallback("original", &Digit_recognition::CallBackFunc , &digit);
	while(1)
	{
		digit.Set_main_image(pipeline.Get_Image());				//get image poprawi� �eby dobry obrazek zwraca�
		key = waitKey(10);
		if(key == 13 || key == 10)
		{
			if (digit.can_compute())
			{
				digit.compute();
				digit.thresholding2();
				//get_digit();
				//imshow("Display", cropped_image);
			}
			else
				log("Cannot compute, not enough points (4 required)");
		}
		else if(key == 32)
			{
				printf("%d\n", digit.get_digit());
			}
			else if(key == 27)
				break;

		digit.compute();
		digit.draw_points();

		imshow("original", digit.Get_main_image());
	}

	destroyAllWindows();

	return 0;
}



