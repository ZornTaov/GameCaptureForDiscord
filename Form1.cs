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
using System.Windows.Forms;
namespace GameCaptureForDiscord
{
    public partial class Form1 : Form
    {
        private const int _width = 1920;
        private const int _height = 1080;
        private VideoCapture _capture = null;
        private bool _captureInProgress;
        private readonly Mat _frame;
        private int frameCount = 0;
        private readonly System.Windows.Forms.Timer fpsTimer;

        WaveIn wi;
        DirectSoundOut wo;
        private BufferedWaveProvider bwp;
        private SampleChannel volumeSampleProvider;

        public Form1()
        {
            InitializeComponent();
            CvInvoke.UseOpenCL = false;
            try
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
            }
            _frame = new Mat();
            fpsTimer = new System.Windows.Forms.Timer()
            {
                Interval = Convert.ToInt32(TimeSpan.FromSeconds(1).TotalMilliseconds)
            };
            fpsTimer.Tick += UpdateFps;
            fpsTimer.Start();
            toolStripComboBox1.ComboBox.DataSource = ListOfAttachedCameras();// causes slowdown
            wo = new DirectSoundOut();
            wi = new WaveIn();
            wi.DataAvailable += new EventHandler<WaveInEventArgs>(wi_DataAvailable);

            bwp = new BufferedWaveProvider(wi.WaveFormat);
            bwp.DiscardOnBufferOverflow = true;
            volumeSampleProvider = new SampleChannel(bwp);

            wo.Init(volumeSampleProvider);
            if (Environment.OSVersion.Version.Major >= 6)
            {
                LoadWasapiDevicesCombo();
            }
            else
            {
                comboWasapiDevices.Enabled = false;
            }
            LoadWaveInDevicesCombo();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            const int margin = 5;
            Rectangle rect = new Rectangle(
                Screen.PrimaryScreen.WorkingArea.X + margin,
                Screen.PrimaryScreen.WorkingArea.Y + margin,
                Screen.PrimaryScreen.WorkingArea.Width - 2 * margin,
                Screen.PrimaryScreen.WorkingArea.Height - 2 * (margin - 7));
            Bounds = rect;
            MinimumSize = new System.Drawing.Size(_width, _height + 20);
            ClientSize = new System.Drawing.Size(_width, _height + 20);
            SplashForm.CloseForm();
            WindowState = FormWindowState.Minimized;
            WindowState = FormWindowState.Normal;
            Focus(); Show();
        }
        private void ReleaseData()
        {
            if (fpsTimer != null && fpsTimer.Enabled)
            {
                fpsTimer?.Stop();
            }

            fpsTimer?.Dispose();
            _capture?.Dispose();
            wo?.Dispose();
            wi?.Dispose();
        }

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
        private void LoadWasapiDevicesCombo()
        {
            MMDeviceEnumerator deviceEnum = new MMDeviceEnumerator();
            //var devices = deviceEnum.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active).ToList();
            IEnumerable<DirectSoundDeviceInfo> devices = DirectSoundOut.Devices;
            comboWasapiDevices.ComboBox.DataSource = devices;
            comboWasapiDevices.ComboBox.DisplayMember = "Description";
        }

        private void LoadWaveInDevicesCombo()
        {
            WaveInCapabilities[] devices = Enumerable.Range(0, WaveIn.DeviceCount).Select(n => WaveIn.GetCapabilities(n)).ToArray();

            comboWaveInDevice.ComboBox.DataSource = devices;
            comboWaveInDevice.ComboBox.DisplayMember = "ProductName";
        }
        void wi_DataAvailable(object sender, WaveInEventArgs e)
        {
            bwp.AddSamples(e.Buffer, 0, e.BytesRecorded);

        }

        private void UpdateFps(object sender, EventArgs e)
        {
            int fc = Interlocked.Exchange(ref frameCount, 0);
            Text = $"FPS: {fc} Height: {_capture.Height} Width: {_capture.Width}";
        }

        private void ProcessFrame(object sender, EventArgs arg)
        {
            if (_capture != null && _capture.Ptr != IntPtr.Zero)
            {
                _capture.Retrieve(_frame, 0);

                captureImageBox.Image = _frame;
                Interlocked.Increment(ref frameCount);
            }
        }


        private void startCaptureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_capture != null)
            {
                if (_captureInProgress)
                {  //stop the capture
                    startCaptureToolStripMenuItem.Text = "Start Capture";
                    _capture.Pause();
                    wi.StopRecording();
                    wo.Stop();
                }
                else
                {
                    //start the capture
                    startCaptureToolStripMenuItem.Text = "Stop";
                    _capture.Start();
                    wi.StartRecording();
                    wo.Play();
                }

                _captureInProgress = !_captureInProgress;
            }

        }

        #region DirectShow List Video Devices
        //===================================
        internal static readonly Guid SystemDeviceEnum = new Guid(0x62BE5D10, 0x60EB, 0x11D0, 0xBD, 0x3B, 0x00, 0xA0, 0xC9, 0x11, 0xCE, 0x86);
        internal static readonly Guid VideoInputDevice = new Guid(0x860BB310, 0x5D01, 0x11D0, 0xBD, 0x3B, 0x00, 0xA0, 0xC9, 0x11, 0xCE, 0x86);

        [ComImport, Guid("55272A00-42CB-11CE-8135-00AA004BB851"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface IPropertyBag
        {
            [PreserveSig]
            int Read(
                [In, MarshalAs(UnmanagedType.LPWStr)] string propertyName,
                [In, Out, MarshalAs(UnmanagedType.Struct)] ref object pVar,
                [In] IntPtr pErrorLog);
            [PreserveSig]
            int Write(
                [In, MarshalAs(UnmanagedType.LPWStr)] string propertyName,
                [In, MarshalAs(UnmanagedType.Struct)] ref object pVar);
        }

        [ComImport, Guid("29840822-5B84-11D0-BD3B-00A0C911CE86"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface ICreateDevEnum
        {
            [PreserveSig]
            int CreateClassEnumerator([In] ref Guid type, [Out] out IEnumMoniker enumMoniker, [In] int flags);
        }

        private void ListVideoDevices(ToolStripComboBox xcombobox)
        {
            Object bagObj = null;
            object comObj;
            ICreateDevEnum enumDev;
            IEnumMoniker enumMon = null;
            IMoniker[] moniker = new IMoniker[100];
            IPropertyBag bag;
            try
            {
                // Get the system device enumerator
                Type srvType = Type.GetTypeFromCLSID(SystemDeviceEnum);
                // create device enumerator
                comObj = Activator.CreateInstance(srvType);
                enumDev = (ICreateDevEnum)comObj;
                // Create an enumerator to find filters of specified category
                enumDev.CreateClassEnumerator(VideoInputDevice, out enumMon, 0);
                Guid bagId = typeof(IPropertyBag).GUID;
                while (enumMon.Next(1, moniker, IntPtr.Zero) == 0)
                {
                    // get property bag of the moniker
                    moniker[0].BindToStorage(null, null, ref bagId, out bagObj);
                    bag = (IPropertyBag)bagObj;
                    // read FriendlyName
                    object val = "";
                    bag.Read("FriendlyName", ref val, IntPtr.Zero);
                    //list in box
                    xcombobox.Items.Add((string)val);
                }

            }
            catch (Exception)
            {
            }
            finally
            {
                bag = null;
                if (bagObj != null)
                {
                    Marshal.ReleaseComObject(bagObj);
                    bagObj = null;
                }
                enumDev = null;
                enumMon = null;
                moniker = null;
            }
            if (xcombobox.Items.Count > 0)
            {
                xcombobox.SelectedIndex = 0;
            }
        }
        #endregion //List Video Devices

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {
            if (_captureInProgress)
            {
                _capture.Pause();
            }

            _capture?.Dispose();
            _capture = new VideoCapture(GetCameraIndexForPartName(((ToolStripComboBox)sender).SelectedItem as string), VideoCapture.API.Msmf);

            _capture.Set(CapProp.FrameWidth, _width);
            _capture.Set(CapProp.FrameHeight, _height);
            //captureImageBox.Size = new Size(1910, 1070);
            _capture.ImageGrabbed += ProcessFrame;

            if (_captureInProgress)
            {
                _capture.Start();
            }
        }

        private void ToolStripComboBox_DropDown(object sender, EventArgs e)
        {

            using (System.Drawing.Graphics graphics = CreateGraphics())
            {
                int maxWidth = 0;
                foreach (object obj in ((ToolStripComboBox)sender).Items)
                {
                    System.Drawing.SizeF area = graphics.MeasureString(obj.ToString(), ((ToolStripComboBox)sender).Font);
                    maxWidth = Math.Max((int)area.Width, maxWidth);
                }
                ((ToolStripComboBox)sender).DropDownWidth = maxWidth;
            }
        }

        private void comboWaveInDevice_Click(object sender, EventArgs e)
        {
            if (_captureInProgress)
            {
                wi.StopRecording();
            }

            wi?.Dispose();
            wi = new WaveIn(Handle);
            wi.DeviceNumber = ((ToolStripComboBox)sender).SelectedIndex;
            wi.DataAvailable += new EventHandler<WaveInEventArgs>(wi_DataAvailable);
            bwp = new BufferedWaveProvider(wi.WaveFormat);
            bwp.DiscardOnBufferOverflow = true;
            volumeSampleProvider = new SampleChannel(bwp);

            wo.Init(volumeSampleProvider);
            Console.WriteLine("Input now: {0}, should be {1}",
                ((WaveInCapabilities)((ToolStripComboBox)sender).SelectedItem).ProductName,
                WaveIn.GetCapabilities(wi.DeviceNumber).ProductName);
            if (_captureInProgress)
            {
                wi.StartRecording();
            }
        }

        private void comboWasapiDevices_Click(object sender, EventArgs e)
        {
            if (_captureInProgress)
            {
                wo.Stop();
            }

            wo?.Dispose();
            /*wo = new WaveOut(this.Handle);
            wo.Init(bwp);
            wo.DeviceNumber = ((ToolStripComboBox)sender).SelectedIndex;*/
            wo = new DirectSoundOut(((DirectSoundDeviceInfo)((ToolStripComboBox)sender).SelectedItem).Guid);
            Console.WriteLine("Input now: {0}, should be {1}",
                ((DirectSoundDeviceInfo)((ToolStripComboBox)sender).SelectedItem).Description,
                DirectSoundOut.Devices.First(d => d.Guid == ((DirectSoundDeviceInfo)((ToolStripComboBox)sender).SelectedItem).Guid).Description);

            if (_captureInProgress)
            {
                wo.Play();
            }
        }

        private void volumeSlider1_VolumeChanged(object sender, EventArgs e)
        {
            volumeSampleProvider.Volume = (sender as NAudio.Gui.VolumeSlider).Volume;
        }
    }
}