﻿using System;
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
        Mat imgSource = new Mat();
        Mat imgYCRCB = new Mat();
        Mat imgHSV = new Mat();
        Mat imgGray = new Mat();
        Mat imgColor = new Mat();
        //Mat imgBackground = new Mat();
        bool IsWork = true;
        int index = 0;
        Mat imgTmpLabels = new Mat();
        Mat imgTmpStats = new Mat();
        Mat imgTmpCentroids = new Mat();

        int maxSize = 0;
        Rect maxRect = new Rect();

        BackgroundSubtractorMOG2 bs;

        MainWindow MW = null;

        CascadeClassifier haarCascade = new CascadeClassifier(@"..\..\..\..\..\opencv_3.1\sources\data\haarcascades_cuda\haarcascade_frontalface_alt_tree.xml");
        //CascadeClassifier haarCascade = new CascadeClassifier(@"..\..\..\..\..\opencv_3.1\sources\data\haarcascades_cuda\haarcascade_frontalface_alt2.xml");
        Rect[] faces;

        public bool IsBlur = false;
        public int nBlurFactor = 10;

        /// <summary>
        /// 0: DetectSkin
        /// 1: DeleteBackgroundGray
        /// 2: FaceDetect
        /// 3: DeleteBackgroundColor
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
                    return;
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
                Cv2.NamedWindow(index.ToString(), WindowMode.AutoSize);

                while (IsWork)
                {
                    capture.Read(imgSource);

                    if(WorkType == 0)
                    {
                        Cv2.CvtColor(imgSource, imgYCRCB, ColorConversionCodes.BGR2YCrCb);

                        Cv2.InRange(imgYCRCB, new Scalar(YCRCB.YL, YCRCB.CRL, YCRCB.CBL), new Scalar(YCRCB.YH, YCRCB.CRH, YCRCB.CBH), imgGray);

                        //Cv2.Blur(gray, gray, new Size(10, 10));

                        Cv2.CvtColor(imgGray, imgColor, ColorConversionCodes.GRAY2BGR);

                        if (this.IsBlur)
                        {
                            Cv2.Blur(imgColor, imgColor, new Size(nBlurFactor, nBlurFactor));
                        }

                        int numOfLables = Cv2.ConnectedComponentsWithStats(imgGray, imgTmpLabels, imgTmpStats, imgTmpCentroids, OpenCvSharp.PixelConnectivity.Connectivity8, MatType.CV_32S);

                        maxSize = 0;
                        maxRect = new Rect();

                        //라벨링 된 이미지에 각각 직사각형으로 둘러싸기 
                        for (int j = 1; j < numOfLables; j++)
                        {
                            //int area = stats.At<int>(j, (int)ConnectedComponentsTypes.Area);
                            int left = imgTmpStats.At<int>(j, (int)ConnectedComponentsTypes.Left);
                            int top = imgTmpStats.At<int>(j, (int)ConnectedComponentsTypes.Top);
                            int width = imgTmpStats.At<int>(j, (int)ConnectedComponentsTypes.Width);
                            int height = imgTmpStats.At<int>(j, (int)ConnectedComponentsTypes.Height);

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

                        Cv2.Rectangle(imgColor, new Point(maxRect.Left, maxRect.Top), new Point(maxRect.Left + maxRect.Width, maxRect.Top + maxRect.Height), new Scalar(0, 0, 255), 1);

                        string textMsg0 = "X: " + maxRect.Left + ", Y: " + maxRect.Top;
                        string textMsg1 = "Width: " + maxRect.Width + ", Height: " + maxRect.Height;
                        string textMsg2 = "Size: " + maxRect.Width * maxRect.Height;

                        //OpenCvSharp.HersheyFonts.HersheyComplexSmall
                        //Cv2.PutText(labelColor, textMsg, new Point(maxRect.Left + 20, maxRect.Top + 20), HersheyFonts.HersheySimplex, 1, new Scalar(0, 0, 255));
                        Cv2.PutText(imgColor, textMsg0, new Point(20, 20), OpenCvSharp.HersheyFonts.HersheyPlain, 1, new Scalar(0, 0, 255));
                        Cv2.PutText(imgColor, textMsg1, new Point(20, 40), OpenCvSharp.HersheyFonts.HersheyPlain, 1, new Scalar(0, 0, 255));
                        Cv2.PutText(imgColor, textMsg2, new Point(20, 60), OpenCvSharp.HersheyFonts.HersheyPlain, 1, new Scalar(0, 0, 255));

                        //Cv2.ImShow(index.ToString(), source);
                        Cv2.ImShow(index.ToString(), imgColor);

                        int c = Cv2.WaitKey(10);

                        if (c != -1) { break; }
                    }
                    else if (WorkType == 1)
                    {
                        bs.Apply(imgSource, imgGray);

                        if (this.IsBlur)
                        {
                            Cv2.Blur(imgGray, imgGray, new Size(nBlurFactor, nBlurFactor));
                        }

                        Cv2.ImShow(index.ToString(), imgGray);

                        int c = Cv2.WaitKey(10);

                        if (c != -1) { break; }
                    }
                    else if (WorkType == 2)
                    {
                        Cv2.CvtColor(imgSource, imgGray, ColorConversionCodes.BGR2GRAY);

                        if (this.IsBlur)
                        {
                            Cv2.Blur(imgSource, imgSource, new Size(nBlurFactor * 3, nBlurFactor * 3));
                        }
                        
                        faces = haarCascade.DetectMultiScale(imgGray, 1.08, 2, HaarDetectionType.ScaleImage, new OpenCvSharp.Size(30, 30));

                        foreach (Rect face in faces)
                        {
                            var pt1 = new OpenCvSharp.Point
                            {
                                X = face.X,
                                Y = face.Y
                            };
                            var pt2 = new OpenCvSharp.Point
                            {
                                X = face.X + face.Width,
                                Y = face.Y + face.Height
                            };
                            Cv2.Rectangle(imgSource, pt1, pt2, new Scalar(0, 255, 0), 2);
                        }

                        Cv2.ImShow(index.ToString(), imgSource);

                        int c = Cv2.WaitKey(10);

                        if (c != -1) { break; }
                    }
                    else if(WorkType == 3)
                    {
                        bs.Apply(imgSource, imgGray);

                        for(int y = 0; y < imgSource.Rows; y ++)
                        {
                            for(int x = 0; x < imgSource.Cols; x++)
                            {
                                int pointerColorCount = (y * 640 * 3) + (x * 3);

                                int pointerGrayCount = (y * 640 * 1) + (x * 1);

                                unsafe
                                {
                                    if (imgGray.DataPointer[pointerGrayCount] == 0)
                                    {
                                        imgSource.DataPointer[pointerColorCount + 2] = 0;
                                        imgSource.DataPointer[pointerColorCount + 1] = 0;
                                        imgSource.DataPointer[pointerColorCount + 0] = 0;
                                    }
                                }
                            }
                        }



                        if (this.IsBlur)
                        {
                            Cv2.Blur(imgSource, imgSource, new Size(nBlurFactor, nBlurFactor));
                        }

                        Cv2.ImShow(index.ToString(), imgSource);

                        int c = Cv2.WaitKey(10);

                        if (c != -1) { break; }
                    }
                    else if (WorkType == 4)
                    {
                        //Cv2.CvtColor(imgSource, imgHSV, ColorConversionCodes.BGR2HSV);
                        Cv2.CvtColor(imgSource, imgHSV, ColorConversionCodes.BGR2HSV_FULL);

                        //Cv2.InRange(imgHSV, new Scalar(0, 10, 60), new Scalar(20, 150, 255), imgGray);

                        //Cv2.InRange(imgHSV, new Scalar(0, 40, 60), new Scalar(20, 150, 255), imgGray);

                        //is it Same
                        //Cv2.InRange(imgHSV, new Scalar(0, 0.28 * 255, 0), new Scalar(25, 0.68 * 255, 255), imgGray);
                        Cv2.InRange(imgHSV, new Scalar(0, 71.4, 0), new Scalar(25, 173.4, 255), imgGray);

                        //Cv2.InRange(imgHSV, new Scalar(0, 48, 80), new Scalar(20, 255, 255), imgGray);

                        //Cv2.InRange(imgHSV, new Scalar(0, 50, 80), new Scalar(120, 255, 255), imgGray);

                        Cv2.CvtColor(imgGray, imgColor, ColorConversionCodes.GRAY2BGR);

                        if (this.IsBlur)
                        {
                            Cv2.Blur(imgColor, imgColor, new Size(nBlurFactor, nBlurFactor));
                        }

                        int numOfLables = Cv2.ConnectedComponentsWithStats(imgGray, imgTmpLabels, imgTmpStats, imgTmpCentroids, OpenCvSharp.PixelConnectivity.Connectivity8, MatType.CV_32S);

                        maxSize = 0;
                        maxRect = new Rect();

                        //라벨링 된 이미지에 각각 직사각형으로 둘러싸기 
                        for (int j = 1; j < numOfLables; j++)
                        {
                            //int area = stats.At<int>(j, (int)ConnectedComponentsTypes.Area);
                            int left = imgTmpStats.At<int>(j, (int)ConnectedComponentsTypes.Left);
                            int top = imgTmpStats.At<int>(j, (int)ConnectedComponentsTypes.Top);
                            int width = imgTmpStats.At<int>(j, (int)ConnectedComponentsTypes.Width);
                            int height = imgTmpStats.At<int>(j, (int)ConnectedComponentsTypes.Height);

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

                        Cv2.Rectangle(imgColor, new Point(maxRect.Left, maxRect.Top), new Point(maxRect.Left + maxRect.Width, maxRect.Top + maxRect.Height), new Scalar(0, 0, 255), 1);

                        string textMsg0 = "X: " + maxRect.Left + ", Y: " + maxRect.Top;
                        string textMsg1 = "Width: " + maxRect.Width + ", Height: " + maxRect.Height;
                        string textMsg2 = "Size: " + maxRect.Width * maxRect.Height;

                        //OpenCvSharp.HersheyFonts.HersheyComplexSmall
                        //Cv2.PutText(labelColor, textMsg, new Point(maxRect.Left + 20, maxRect.Top + 20), HersheyFonts.HersheySimplex, 1, new Scalar(0, 0, 255));
                        Cv2.PutText(imgColor, textMsg0, new Point(20, 20), OpenCvSharp.HersheyFonts.HersheyPlain, 1, new Scalar(0, 0, 255));
                        Cv2.PutText(imgColor, textMsg1, new Point(20, 40), OpenCvSharp.HersheyFonts.HersheyPlain, 1, new Scalar(0, 0, 255));
                        Cv2.PutText(imgColor, textMsg2, new Point(20, 60), OpenCvSharp.HersheyFonts.HersheyPlain, 1, new Scalar(0, 0, 255));

                        //Cv2.ImShow(index.ToString(), source);
                        Cv2.ImShow(index.ToString(), imgColor);

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
