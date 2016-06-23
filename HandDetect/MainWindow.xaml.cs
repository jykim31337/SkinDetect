using HandDetect.Class;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HandDetect
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        int nCameraCount = 0;
        ArrayList Cameras = new ArrayList();

        public MainWindow()
        {
            InitializeComponent();
            InitControl();
        }

        public void InitControl()
        {
            try
            {
                btnStart.Click += btnStart_Click;

                rbDBG.Checked += rbDBG_Checked;
                rbSD.Checked += rbSD_Checked;

                txtBlurFactor.TextChanged += txtBlurFactor_TextChanged;
                chkBlur.Checked += chkBlur_Checked;
                chkBlur.Unchecked += chkBlur_Unchecked;
            }
            catch(Exception ex)
            {
                string err = ex.Message + "\r\n" + ex.StackTrace;
                MessageBox.Show(err);
            }
        }

        void chkBlur_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                for (int i = 0; i < nCameraCount; i++)
                {
                    ((Camera)Cameras[i]).IsBlur = false;
                }
            }
            catch (Exception ex)
            {
                string err = ex.Message + "\r\n" + ex.StackTrace;
                MessageBox.Show(err);
            }
        }

        void chkBlur_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                for (int i = 0; i < nCameraCount; i++)
                {
                    ((Camera)Cameras[i]).IsBlur = true;
                }
            }
            catch (Exception ex)
            {
                string err = ex.Message + "\r\n" + ex.StackTrace;
                MessageBox.Show(err);
            }
        }

        void txtBlurFactor_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                int factor = 10;

                if(!int.TryParse( txtBlurFactor.Text, out factor))
                {
                    return;
                }

                for (int i = 0; i < nCameraCount; i++)
                {
                    ((Camera)Cameras[i]).nBlurFactor = factor;
                }
            }
            catch (Exception ex)
            {
                string err = ex.Message + "\r\n" + ex.StackTrace;
                MessageBox.Show(err);
            }
        }

        void rbSD_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                for(int i = 0; i < nCameraCount; i++)
                {
                    ((Camera)Cameras[i]).WorkType = 0;
                }
            }
            catch (Exception ex)
            {
                string err = ex.Message + "\r\n" + ex.StackTrace;
                MessageBox.Show(err);
            }
        }

        void rbDBG_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                for (int i = 0; i < nCameraCount; i++)
                {
                    ((Camera)Cameras[i]).WorkType = 1;
                }
            }
            catch (Exception ex)
            {
                string err = ex.Message + "\r\n" + ex.StackTrace;
                MessageBox.Show(err);
            }
        }

        void btnStart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Camera tmpCamera = new Camera(nCameraCount, this);

                nCameraCount++;

                Cameras.Add(tmpCamera);
            }
            catch (Exception ex)
            {
                string err = ex.Message + "\r\n" + ex.StackTrace;
                MessageBox.Show(err);
            }
        }
    }
}
