using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using System.ComponentModel;
using System.Windows.Threading;

namespace HandDetect.Class
{
    public class Camera
    {
        OpenCvSharp.VideoCapture capture;
        BackgroundWorker bw = new BackgroundWorker();
        Mat source = new Mat();
        Mat YCRCB = new Mat();
        Mat gray = new Mat();
        Mat labelColor = new Mat();
        Mat background = new Mat();
        bool IsWork = true;
        int index = 0;
        Mat img_labels = new Mat();
        Mat stats = new Mat();
        Mat centroids = new Mat();

        int maxSize = 0;
        Rect maxRect = new Rect();

        BackgroundSubtractorMOG2 bs;

        MainWindow MW = null;

        public bool IsBlur = false;
        public int nBlurFactor = 10;

        /// <summary>
        /// 0: DetectSkin
        /// 1: DeleteBackground
        /// </summary>
        public int WorkType = 0;

        public Camera(int _index, MainWindow _mw)
        {
            try
            {
                this.index = _index;
                capture = new VideoCapture(index);
                bs = BackgroundSubtractorMOG2.Create();
                MW = _mw;

                if (!capture.IsOpened())
                {
                    System.Windows.MessageBox.Show("연결 실패");
                }

                //capture.Set(CaptureProperty.FrameWidth, 640);
                //capture.Set(CaptureProperty.FrameHeight, 480);

                //source = new Mat();
                //source = new Mat(640, 480, MatType.CV_8UC3);
                //source = new Mat(640, 480, MatType.CV_64FC3);

                bw.DoWork += Bw_DoWork;

                bw.RunWorkerAsync();
            }
            catch(Exception ex)
            {
                string err = ex.Message + "\r\n" + ex.StackTrace;
                System.Windows.MessageBox.Show(err);
            }

        }

        private void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Cv2.NamedWindow(index.ToString());

                while (IsWork)
                {
                    capture.Read(source);

                    if (WorkType  == 1)
                    {
                        bs.Apply(source, background);

                        if(this.IsBlur)
                        {
                            Cv2.Blur(background, background, new Size(nBlurFactor, nBlurFactor));
                        }

                        Cv2.ImShow(index.ToString(), background);

                        int c = Cv2.WaitKey(10);

                        if (c != -1) { break; }
                    }
                    else if(WorkType == 0)
                    {
                        Cv2.CvtColor(source, YCRCB, ColorConversionCodes.BGR2YCrCb);

                        Cv2.InRange(YCRCB, new Scalar(0, 133, 77), new Scalar(255, 173, 127), gray);

                        //Cv2.Blur(gray, gray, new Size(10, 10));

                        Cv2.CvtColor(gray, labelColor, ColorConversionCodes.GRAY2BGR);

                        if (this.IsBlur)
                        {
                            Cv2.Blur(labelColor, labelColor, new Size(nBlurFactor, nBlurFactor));
                        }

                        int numOfLables = Cv2.ConnectedComponentsWithStats(gray, img_labels, stats, centroids, OpenCvSharp.PixelConnectivity.Connectivity8, MatType.CV_32S);

                        maxSize = 0;
                        maxRect = new Rect();

                        //라벨링 된 이미지에 각각 직사각형으로 둘러싸기 
                        for (int j = 1; j < numOfLables; j++)
                        {
                            //int area = stats.At<int>(j, (int)ConnectedComponentsTypes.Area);
                            int left = stats.At<int>(j, (int)ConnectedComponentsTypes.Left);
                            int top = stats.At<int>(j, (int)ConnectedComponentsTypes.Top);
                            int width = stats.At<int>(j, (int)ConnectedComponentsTypes.Width);
                            int height = stats.At<int>(j, (int)ConnectedComponentsTypes.Height);

                            int size = width * height;

                            if (maxSize < size)
                            {
                                maxSize = size;
                                maxRect.Left = left;
                                maxRect.Top = top;
                                maxRect.Width = width;
                                maxRect.Height = height;
                            }

                            //Cv2.Rectangle(img_color, new Point(left, top), new Point(left + width, top + height),  new Scalar(0, 0, 255), 1);
                            //Cv2.PutText(img_color, j.ToString(), new Point(left + 20, top + 20),HersheyFonts.HersheySimplex, 1, new Scalar(255, 0, 0), 2);
                        }

                        Cv2.Rectangle(labelColor, new Point(maxRect.Left, maxRect.Top), new Point(maxRect.Left + maxRect.Width, maxRect.Top + maxRect.Height), new Scalar(0, 0, 255), 1);

                        string textMsg0 = "X: " + maxRect.Left + ", Y: " + maxRect.Top;
                        string textMsg1 = "Width: " + maxRect.Width + ", Height: " + maxRect.Height;
                        string textMsg2 = "Size: " + maxRect.Width * maxRect.Height;

                        //OpenCvSharp.HersheyFonts.HersheyComplexSmall
                        //Cv2.PutText(labelColor, textMsg, new Point(maxRect.Left + 20, maxRect.Top + 20), HersheyFonts.HersheySimplex, 1, new Scalar(0, 0, 255));
                        Cv2.PutText(labelColor, textMsg0, new Point(20, 20), OpenCvSharp.HersheyFonts.HersheyPlain, 1, new Scalar(0, 0, 255));
                        Cv2.PutText(labelColor, textMsg1, new Point(20, 40), OpenCvSharp.HersheyFonts.HersheyPlain, 1, new Scalar(0, 0, 255));
                        Cv2.PutText(labelColor, textMsg2, new Point(20, 60), OpenCvSharp.HersheyFonts.HersheyPlain, 1, new Scalar(0, 0, 255));

                        //Cv2.ImShow(index.ToString(), source);
                        Cv2.ImShow(index.ToString(), labelColor);

                        int c = Cv2.WaitKey(10);

                        if (c != -1) { break; }
                    }
                }
            }
            catch(Exception ex)
            {
                string err = ex.Message + "\r\n" + ex.StackTrace;
                System.Windows.MessageBox.Show(err);
            }
        }

    }
}
