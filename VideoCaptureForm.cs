using Emgu.CV;
using Emgu.CV.CvEnum;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using SharpDX.MediaFoundation;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace GameCaptureForDiscord
{
    public partial class VideoCaptureForm : Form
    {
        internal const int _width = 1920;
        internal const int _height = 1080;
        internal VideoCapture _capture = null;
        internal readonly Mat _frame;
        internal int frameCountIn = 0;
        internal int frameCountOut = 0;
        internal double AudioDelayMS = 0;
        internal double VideoDelayMS = 0;
        internal readonly System.Windows.Forms.Timer fpsTimer;

        internal bool _captureInProgress;
        internal WaveIn wi;
        internal DirectSoundOut wo;
        internal BufferedWaveProvider bwp;
        internal SampleChannel sampleChannel;
        private ControlsForm controlsForm;

        public VideoCaptureForm()
        {
            InitializeComponent();
            CvInvoke.UseOpenCL = false;
            /*try
            {
                _capture = new VideoCapture(0, VideoCapture.API.Msmf);// causes slowdown
                _capture.Set(CapProp.FrameWidth, _width);// causes slowdown
                _capture.Set(CapProp.FrameHeight, _height);// causes slowdown
                //captureImageBox.Size = new Size(1910, 1070);
                _capture.ImageGrabbed += ProcessFrame;
            }
            catch (NullReferenceException excpt)
            {
                MessageBox.Show(excpt.Message);
            }*/
            _frame = new Mat();
            fpsTimer = new System.Windows.Forms.Timer()
            {
                Interval = Convert.ToInt32(TimeSpan.FromSeconds(1).TotalMilliseconds)
            };
            fpsTimer.Tick += UpdateFps;
            fpsTimer.Start();
            /*wo = new DirectSoundOut();
            wi = new WaveIn();
            wi.DataAvailable += new EventHandler<WaveInEventArgs>(wi_DataAvailable);

            bwp = new BufferedWaveProvider(wi.WaveFormat);
            bwp.DiscardOnBufferOverflow = true;
            sampleChannel = new SampleChannel(bwp);

            wo.Init(sampleChannel);*/

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /*const int margin = 5;
            Rectangle rect = new Rectangle(
                Screen.PrimaryScreen.WorkingArea.X + margin,
                Screen.PrimaryScreen.WorkingArea.Y + margin,
                Screen.PrimaryScreen.WorkingArea.Width - 2 * margin,
                Screen.PrimaryScreen.WorkingArea.Height - 2 * (margin - 7));
            Bounds = rect;*/
            //MinimumSize = new Size(_width, _height);
            captureImageBox.ContextMenuStrip.Items.Add(showControlsMenuItem);

            ClientSize = new Size(_width, _height);
            controlsForm = new ControlsForm();
            controlsForm.Show();
        }

        private void ReleaseData()
        {
            controlsForm?.Close();
            controlsForm?.Dispose();
            if (fpsTimer != null && fpsTimer.Enabled)
            {
                fpsTimer?.Stop();
            }

            fpsTimer?.Dispose();
            _capture?.Dispose();
            wo?.Dispose();
            wi?.Dispose();
        }

        internal void wi_DataAvailable(object sender, WaveInEventArgs e)
        {
            bwp.AddSamples(e.Buffer, 0, e.BytesRecorded);

        }

        private void UpdateFps(object sender, EventArgs e)
        {
            int fc = Interlocked.Exchange(ref frameCountIn, 0);
            if (_capture != null)
            {
                Text = $"FPS: {fc} Height: {_capture.Height} Width: {_capture.Width}";
            }
        }

        internal void ProcessFrame(object sender, EventArgs arg)
        {
            Interlocked.Increment(ref frameCountIn);
        }

        internal async Task SetupVideo(string sender)
        {
            if (_captureInProgress)
            {
                Program.mainForm._capture.Pause();
            }

            Program.mainForm._capture?.Dispose();
            Program.mainForm._capture = await Task.FromResult(new VideoCapture(
                GetCameraIndexForPartName(sender),
                VideoCapture.API.Msmf,
                new Tuple<CapProp, int>[] {
                        Tuple.Create(CapProp.HwAcceleration, (int)VideoAccelerationType.Any),
                        Tuple.Create(CapProp.FrameWidth, _width),
                        Tuple.Create(CapProp.FrameHeight, _height)
                }));// causes slowdown

            var framesObs = Observable.FromEventPattern(
                addHandler: h => Program.mainForm._capture.ImageGrabbed += h,
                removeHandler: h => Program.mainForm._capture.ImageGrabbed -= h
            ).Select(f =>
            {
                if (_capture != null && _capture.Ptr != IntPtr.Zero)
                {
                    Mat frame = new Mat();
                    _capture.Retrieve(frame, 0);
                    //captureImageBox.Image = _frame;
                    return frame;
                }
                return null;
            })
            .Where(f => f != null)
            //.Append(default)
            //.Scan((p,c) => { p?.Dispose(); return c; })
            .Delay(_ => Observable.Timer(TimeSpan.FromMilliseconds(VideoDelayMS)) );

            framesObs.Subscribe(f =>
            {
                captureImageBox.Image?.Dispose();
                captureImageBox.Image = f;
            });

            Program.mainForm._capture.ImageGrabbed += Program.mainForm.ProcessFrame;

            Program.mainForm.ClientSize = new Size(Program.mainForm._capture.Width, Program.mainForm._capture.Height);

            if (_captureInProgress)
            {
                Program.mainForm._capture.Start();
            }
        }

        #region Enumerations
        public static string[] ListOfAttachedCameras()
        {
            List<string> cameras = new List<string>();
            MediaAttributes attributes = new MediaAttributes(1);
            attributes.Set(CaptureDeviceAttributeKeys.SourceType.Guid, CaptureDeviceAttributeKeys.SourceTypeVideoCapture.Guid);
            Activate[] devices = MediaFactory.EnumDeviceSources(attributes);
            for (int i = 0; i < devices.Count(); i++)
            {
                string friendlyName = devices[i].Get(CaptureDeviceAttributeKeys.FriendlyName);
                cameras.Add(friendlyName);
            }
            return cameras.ToArray();
        }

        public static int GetCameraIndexForPartName(string partName)
        {
            string[] cameras = ListOfAttachedCameras();
            for (int i = 0; i < cameras.Count(); i++)
            {
                if (cameras[i].ToLower().Contains(partName.ToLower()))
                {
                    return i;
                }
            }
            return -1;
        }
        #endregion

        private void controlsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SplashForm.ShowSplashScreen();
            if (controlsForm.IsDisposed)
            {
                controlsForm = new ControlsForm();
            }
            controlsForm.Show();
            controlsForm.Focus();
            SplashForm.CloseForm();
        }
    }
}