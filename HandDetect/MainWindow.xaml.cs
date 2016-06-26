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

            chkLabelColor.Checked += chkLabelColor_Checked;
            chkLabelColor.Unchecked += chkLabelColor_Unchecked;

            txtExposure.TextChanged += txtExposure_TextChanged;
        }

        #region Exposure
        void txtExposure_TextChanged(object sender, TextChangedEventArgs e)
        {
            int value = 0;

            if(int.TryParse(txtExposure.Text, out value))
            {
                for (int i = 0; i < nCameraCount; i++)
                {
                    ((Camera)Cameras[i]).SetExpocure(value);
                }
            }
        }
        #endregion

        #region YCRCB Text Change
        void txtYCRCBCBH_TextChanged(object sender, TextChangedEventArgs e)
        {
            int value = 0;
            if (int.TryParse(txtYCRCBCBH.Text, out value))
            {
                YCRCB.CBH = value;
            }
        }

        void txtYCRCBCBL_TextChanged(object sender, TextChangedEventArgs e)
        {
            int value = 0;
            if (int.TryParse(txtYCRCBCBL.Text, out value))
            {
                YCRCB.CBL = value;
            }
        }

        void txtYCRCBCRH_TextChanged(object sender, TextChangedEventArgs e)
        {
            int value = 0;
            if (int.TryParse(txtYCRCBCRH.Text, out value))
            {
                YCRCB.CRH = value;
            }
        }

        void txtYCRCBCRL_TextChanged(object sender, TextChangedEventArgs e)
        {
            int value = 0;
            if (int.TryParse(txtYCRCBCRL.Text, out value))
            {
                YCRCB.CRL = value;
            }
        }

        void txtYCRCBYH_TextChanged(object sender, TextChangedEventArgs e)
        {
            int value = 0;
            if (int.TryParse(txtYCRCBYH.Text, out value))
            {
                YCRCB.YH = value;
            }
        }

        void txtYCRCBYL_TextChanged(object sender, TextChangedEventArgs e)
        {
            int value = 0;
            if (int.TryParse(txtYCRCBYL.Text, out value))
            {
                YCRCB.YL = value;
            }
        }
        #endregion

        #region Blur
        void chkBlur_Unchecked(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < nCameraCount; i++)
            {
                ((Camera)Cameras[i]).IsBlur = false;
            }
        }

        void chkBlur_Checked(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < nCameraCount; i++)
            {
                ((Camera)Cameras[i]).IsBlur = true;
            }
        }

        void txtBlurFactor_TextChanged(object sender, TextChangedEventArgs e)
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

        #endregion

        #region LabelColor
        void chkLabelColor_Unchecked(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < nCameraCount; i++)
            {
                ((Camera)Cameras[i]).IsLabelColor = false;
            }
        }

        void chkLabelColor_Checked(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < nCameraCount; i++)
            {
                ((Camera)Cameras[i]).IsLabelColor = true;
            }
        }
        #endregion

        #region WorkMode

        void rbSDYCRCB_Checked(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < nCameraCount; i++)
            {
                ((Camera)Cameras[i]).WorkType = 0;
            }
        }

        void rbDBG_Checked(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < nCameraCount; i++)
            {
                ((Camera)Cameras[i]).WorkType = 1;
            }
        }

        void rbFD_Checked(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < nCameraCount; i++)
            {
                ((Camera)Cameras[i]).WorkType = 2;
            }
        }

        void rbSDFD_Checked(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < nCameraCount; i++)
            {
                ((Camera)Cameras[i]).WorkType = 3;
            }
        }

        void rbSDHSV_Checked(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < nCameraCount; i++)
            {
                ((Camera)Cameras[i]).WorkType = 4;
            }

            grdYCRCB.IsEnabled = false;
        }

        void rbSDHSV_Unchecked(object sender, RoutedEventArgs e)
        {
            grdYCRCB.IsEnabled = true;
        }

        void rbHD_Checked(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < nCameraCount; i++)
            {
                ((Camera)Cameras[i]).WorkType = 5;
            }
        }

        #endregion

        void btnStart_Click(object sender, RoutedEventArgs e)
        {
            Camera tmpCamera = new Camera(nCameraCount, this);

            nCameraCount++;

            Cameras.Add(tmpCamera);
        }
    }
}
