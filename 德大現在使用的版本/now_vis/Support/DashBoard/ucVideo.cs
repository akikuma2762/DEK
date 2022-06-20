using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;

namespace Support.DashBoard
{
    public partial class ucVideo : ucVisObj
    {
        public PictureBox _pictureBox { get { return pictureBox_Video; } }
        public bool _IsStart
        {
            get
            {
                return videoSource != null &&
                    videoSource.IsRunning;
            }
        }
        //--------------------------------------------
        IVideoSource videoSource = null;
        static FilterInfoCollection video_DeviceList =
            new FilterInfoCollection(FilterCategory.VideoInputDevice);

        public ucVideo()
        {
            InitializeComponent();
            _ucChild = this;
        }

        public static  List<string> VideoDeviceList
        {
            get
            {
                List<string> list = new List<string>();
                foreach (FilterInfo device in video_DeviceList)
                {
                    list.Add(device.Name);
                }
                return list;
            }
        }

        public bool Start_VideoDevice(int device_no)
        {
            VideoCaptureDevice videoDevice;
            if (device_no >= video_DeviceList.Count) return false;
            videoDevice = new VideoCaptureDevice(video_DeviceList[device_no].MonikerString);
            return Start((IVideoSource)videoDevice);
        }

        public bool Start_VideoDevice(string device_name)
        {
            int index = 0;
            foreach (FilterInfo info in video_DeviceList)
            {
                if (info.Name == device_name)
                {
                    return Start_VideoDevice(index);
                }
                index++;
            }
            return false;
        }

        public bool Start_IPCam(string url, int frame_interval = 0, string login = "", string pass = "")
        {
            //string url = "http://172.23.10.104/video.cgi";
            // create video source
            JPEGStream jpegSource = new JPEGStream(url);
            if (login != "")
            {
                jpegSource.Login = login;
                jpegSource.Password = pass;
            }
            jpegSource.FrameInterval = frame_interval;
            return Start((IVideoSource)jpegSource);
        }

        public bool Start_VideoFile(string avi_file)
        {
            // create video source
            FileVideoSource fileSource = new FileVideoSource(avi_file);
            return Start((IVideoSource)fileSource);
        }

        private bool Start(IVideoSource video_source)
        {
            Stop();
            videoSource = video_source;
            videoSource.NewFrame += VideoSource_NewFrame;
            videoSource.Start();
            return true;
        }
        public void Stop()
        {
            if (videoSource == null) return;
            videoSource.Stop();
        }
        //--------------------------------------------------------------
        public void VideoDeviceSetup()
        {
            Stop();
            AForge.Video.DirectShow.VideoCaptureDeviceForm form = new VideoCaptureDeviceForm();
            if (form.ShowDialog()==DialogResult.OK)
            {
                
            }
        }
        //--------------------------------------------------------------
        private void pictureBox_Video_Click(object sender, EventArgs e)
        {
            _CtrlVisible = !_CtrlVisible;
        }

        private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        { 
            Bitmap bmp= (Bitmap)eventArgs.Frame.Clone();
            //Graphics gr = Graphics.FromImage(bmp);
            //gr.DrawString(DateTime.Now.ToString(), this.Font, Brushes.Yellow, new PointF(5, 5));
            pictureBox_Video.Image = bmp;
        }

        public override void Refresh()
        {
            base.Refresh();
        }
    }
}
