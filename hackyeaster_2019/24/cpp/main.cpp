#include "opencv2/opencv.hpp"

using namespace cv;
using namespace std;

int main() {
    Mat im = imread("picture.jpg", IMREAD_GRAYSCALE);
    Mat thresholded;
    Mat blurred;

    adaptiveThreshold(im, thresholded, 255, ADAPTIVE_THRESH_GAUSSIAN_C, THRESH_BINARY, 11, 6);
    blur(thresholded, blurred, Size{4, 4});
    threshold(blurred, thresholded, 235, 255, THRESH_BINARY);

    SimpleBlobDetector::Params parameters;

    parameters.filterByArea = true;
    parameters.minArea = 200;

    parameters.filterByConvexity = true;
    parameters.minConvexity = 0.3;
    parameters.maxConvexity = 1;

    parameters.filterByInertia = false;
    parameters.filterByColor = false;

    vector<KeyPoint> keypoints;

    Ptr<SimpleBlobDetector> detector = SimpleBlobDetector::create(parameters);
    detector->detect(thresholded, keypoints);

    std::cout << keypoints.size() << std::endl;

    Mat markedImage;
    drawKeypoints(im, keypoints, markedImage, Scalar{0, 0, 255}, DrawMatchesFlags::DRAW_RICH_KEYPOINTS);

    imshow("Keypoints", markedImage);
    waitKey(0);
}
