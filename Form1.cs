using Emgu.CV;
using Emgu.CV.CvEnum;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using SharpDX.MediaFoundation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace GameCaptureForDiscord
{
    public partial class Form1 : Form
    {
        internal const int _width = 1920;
        internal const int _height = 1080;
        internal VideoCapture _capture = null;
        internal readonly Mat _frame;
        internal int frameCount = 0;
        internal readonly System.Windows.Forms.Timer fpsTimer;

        internal WaveIn wi;
        internal DirectSoundOut wo;
        internal BufferedWaveProvider bwp;
        internal SampleChannel sampleChannel;
        private ControlsForm controlsForm;

        public Form1()
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
            int fc = Interlocked.Exchange(ref frameCount, 0);
            if (_capture != null)
            {
                Text = $"FPS: {fc} Height: {_capture.Height} Width: {_capture.Width}";
            }
        }

        internal void ProcessFrame(object sender, EventArgs arg)
        {
            if (_capture != null && _capture.Ptr != IntPtr.Zero)
            {
                _capture.Retrieve(_frame, 0);

                captureImageBox.Image = _frame;
                Interlocked.Increment(ref frameCount);
            }
        }

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