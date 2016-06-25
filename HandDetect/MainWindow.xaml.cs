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
                //grdHSV.Visibility = System.Windows.Visibility.Collapsed;

                btnStart.Click += btnStart_Click;

                rbDBGG.Checked += rbDBG_Checked;
                rbSDYCRCB.Checked += rbSDYCRCB_Checked;
                rbSDHSV.Checked += rbSDHSV_Checked;
                rbSDHSV.Unchecked += rbSDHSV_Unchecked;
                rbFD.Checked += rbFD_Checked;
                rbDBGC.Checked += rbSDFD_Checked;
                rbHD.Checked += rbHD_Checked;

                txtBlurFactor.TextChanged += txtBlurFactor_TextChanged;
                chkBlur.Checked += chkBlur_Checked;
                chkBlur.Unchecked += chkBlur_Unchecked;

                txtYCRCBYL.Text = YCRCB.YL.ToString();
                txtYCRCBYH.Text = YCRCB.YH.ToString();
                txtYCRCBCRL.Text = YCRCB.CRL.ToString();
                txtYCRCBCRH.Text = YCRCB.CRH.ToString();
                txtYCRCBCBL.Text = YCRCB.CBL.ToString();
                txtYCRCBCBH.Text = YCRCB.CBH.ToString();

                txtYCRCBYL.TextChanged += txtYCRCBYL_TextChanged;
                txtYCRCBYH.TextChanged += txtYCRCBYH_TextChanged;
                txtYCRCBCRL.TextChanged += txtYCRCBCRL_TextChanged;
                txtYCRCBCRH.TextChanged += txtYCRCBCRH_TextChanged;
                txtYCRCBCBL.TextChanged += txtYCRCBCBL_TextChanged;
                txtYCRCBCBH.TextChanged += txtYCRCBCBH_TextChanged;
            }
            catch(Exception ex)
            {
                string err = ex.Message + "\r\n" + ex.StackTrace;
                MessageBox.Show(err);
            }
        }

        #region YCRCB Text Change
        void txtYCRCBCBH_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                int value = 0;
                if(int.TryParse(txtYCRCBCBH.Text, out value))
                {
                    YCRCB.CBH = value;
                }
            }
            catch(Exception ex)
            {
                string err = ex.Message + "\r\n" + ex.StackTrace;
                MessageBox.Show(err);
            }
        }

        void txtYCRCBCBL_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                int value = 0;
                if (int.TryParse(txtYCRCBCBL.Text, out value))
                {
                    YCRCB.CBL = value;
                }
            }
            catch (Exception ex)
            {
                string err = ex.Message + "\r\n" + ex.StackTrace;
                MessageBox.Show(err);
            }
        }

        void txtYCRCBCRH_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                int value = 0;
                if (int.TryParse(txtYCRCBCRH.Text, out value))
                {
                    YCRCB.CRH = value;
                }
            }
            catch (Exception ex)
            {
                string err = ex.Message + "\r\n" + ex.StackTrace;
                MessageBox.Show(err);
            }
        }

        void txtYCRCBCRL_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                int value = 0;
                if (int.TryParse(txtYCRCBCRL.Text, out value))
                {
                    YCRCB.CRL = value;
                }
            }
            catch (Exception ex)
            {
                string err = ex.Message + "\r\n" + ex.StackTrace;
                MessageBox.Show(err);
            }
        }

        void txtYCRCBYH_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                int value = 0;
                if (int.TryParse(txtYCRCBYH.Text, out value))
                {
                    YCRCB.YH = value;
                }
            }
            catch (Exception ex)
            {
                string err = ex.Message + "\r\n" + ex.StackTrace;
                MessageBox.Show(err);
            }
        }

        void txtYCRCBYL_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                int value = 0;
                if (int.TryParse(txtYCRCBYL.Text, out value))
                {
                    YCRCB.YL = value;
                }
            }
            catch (Exception ex)
            {
                string err = ex.Message + "\r\n" + ex.StackTrace;
                MessageBox.Show(err);
            }
        }
        #endregion

        #region Blur
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

                if (!int.TryParse(txtBlurFactor.Text, out factor))
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

        #endregion

        #region WorkMode
        
        void rbSDYCRCB_Checked(object sender, RoutedEventArgs e)
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

        void rbFD_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                for (int i = 0; i < nCameraCount; i++)
                {
                    ((Camera)Cameras[i]).WorkType = 2;
                }
            }
            catch (Exception ex)
            {
                string err = ex.Message + "\r\n" + ex.StackTrace;
                MessageBox.Show(err);
            }
        }

        void rbSDFD_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                for (int i = 0; i < nCameraCount; i++)
                {
                    ((Camera)Cameras[i]).WorkType = 3;
                }
            }
            catch (Exception ex)
            {
                string err = ex.Message + "\r\n" + ex.StackTrace;
                MessageBox.Show(err);
            }
        }

        void rbSDHSV_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                for (int i = 0; i < nCameraCount; i++)
                {
                    ((Camera)Cameras[i]).WorkType = 4;
                }

                //grdHSV.Visibility = System.Windows.Visibility.Visible;
                grdYCRCB.Visibility = System.Windows.Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                string err = ex.Message + "\r\n" + ex.StackTrace;
                MessageBox.Show(err);
            }
        }

        void rbSDHSV_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                //grdHSV.Visibility = System.Windows.Visibility.Collapsed;
                grdYCRCB.Visibility = System.Windows.Visibility.Visible;
            }
            catch (Exception ex)
            {
                string err = ex.Message + "\r\n" + ex.StackTrace;
                MessageBox.Show(err);
            }
        }

        void rbHD_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                for (int i = 0; i < nCameraCount; i++)
                {
                    ((Camera)Cameras[i]).WorkType = 5;
                }

                //grdHSV.Visibility = System.Windows.Visibility.Visible;
                grdYCRCB.Visibility = System.Windows.Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                string err = ex.Message + "\r\n" + ex.StackTrace;
                MessageBox.Show(err);
            }
        }

        #endregion

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
