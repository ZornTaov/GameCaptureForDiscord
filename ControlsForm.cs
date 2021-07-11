using Emgu.CV;
using Emgu.CV.CvEnum;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using SharpDX.MediaFoundation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace GameCaptureForDiscord
{
    public partial class ControlsForm : Form
    {
        private bool loaded = false;
        internal bool _captureInProgress;
        public ControlsForm()
        {
            InitializeComponent();
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

        private async void ControlsForm_Load(object sender, EventArgs e)
        {
            comboVideoDevices.DataSource = await Task.FromResult(ListOfAttachedCameras());// causes slowdown
            loaded = true;
            SetupSoundDrivers();
            SplashForm.CloseForm();
            Program.mainForm.Invoke(() =>
            {
                Program.mainForm.WindowState = FormWindowState.Minimized;
                Program.mainForm.WindowState = FormWindowState.Normal;
                Program.mainForm.Focus();
                Program.mainForm.Show();
            });
            WindowState = FormWindowState.Minimized;
            WindowState = FormWindowState.Normal;
            Show();
            Focus();
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
        private void LoadWasapiDevicesCombo()
        {
            //MMDeviceEnumerator deviceEnum = new MMDeviceEnumerator();
            //var devices = deviceEnum.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active).ToList();
            IEnumerable<DirectSoundDeviceInfo> devices = DirectSoundOut.Devices;
            comboWasapiDevices.DataSource = devices;
            comboWasapiDevices.DisplayMember = "Description";
        }

        private void LoadWaveInDevicesCombo()
        {
            WaveInCapabilities[] devices = Enumerable.Range(0, WaveIn.DeviceCount).Select(n => WaveIn.GetCapabilities(n)).ToArray();

            comboWaveInDevice.DataSource = devices;
            comboWaveInDevice.DisplayMember = "ProductName";
        }
        #endregion

        #region Setup
        private void SetupSoundDrivers()
        {
            if (!loaded) return;
            Program.mainForm.Invoke(() =>
            {
                if (_captureInProgress)
                {
                    Program.mainForm.wi.StopRecording();
                    Program.mainForm.wo.Stop();
                }

                Program.mainForm.wo?.Dispose();
                Program.mainForm.wi?.Dispose();

                Program.mainForm.wi = new WaveIn(Program.mainForm.Handle);
                Program.mainForm.wi.DeviceNumber = comboWaveInDevice.SelectedIndex;
                Program.mainForm.wi.DataAvailable += new EventHandler<WaveInEventArgs>(Program.mainForm.wi_DataAvailable);

                Program.mainForm.bwp = new BufferedWaveProvider(Program.mainForm.wi.WaveFormat);
                Program.mainForm.bwp.DiscardOnBufferOverflow = true;

                Program.mainForm.sampleChannel = new SampleChannel(Program.mainForm.bwp);

                Program.mainForm.wo = new DirectSoundOut(((DirectSoundDeviceInfo)comboWasapiDevices.SelectedItem).Guid);
                Program.mainForm.wo.Init(Program.mainForm.sampleChannel);

                Console.WriteLine("Input now: {0}, should be {1}",
                    ((WaveInCapabilities)comboWaveInDevice.SelectedItem).ProductName,
                    WaveIn.GetCapabilities(Program.mainForm.wi.DeviceNumber).ProductName);
                Console.WriteLine("Input now: {0}, should be {1}",
                    ((DirectSoundDeviceInfo)comboWasapiDevices.SelectedItem).Description,
                    DirectSoundOut.Devices.First(d => d.Guid == ((DirectSoundDeviceInfo)comboWasapiDevices.SelectedItem).Guid).Description);

                if (_captureInProgress)
                {
                    Program.mainForm.wi.StartRecording();
                    Program.mainForm.wo.Play();
                }
            });
        }
        #endregion

        #region Clicks
        private void StartCaptureButton_Click(object sender, EventArgs e)
        {
            Program.mainForm.Invoke(() =>
            {
                if (Program.mainForm._capture != null)
                {
                    if (_captureInProgress)
                    {  //stop the capture
                        startCaptureButton.Text = "Start Capture";
                        Program.mainForm._capture.Pause();
                        Program.mainForm.wi.StopRecording();
                        Program.mainForm.wo.Stop();
                    }
                    else
                    {
                        //start the capture
                        startCaptureButton.Text = "Stop";
                        Program.mainForm._capture.Start();
                        Program.mainForm.wi.StartRecording();
                        Program.mainForm.wo.Play();
                    }

                    _captureInProgress = !_captureInProgress;
                }
            });
        }

        private void setSoundButton_Click(object sender, EventArgs e)
        {
            SetupSoundDrivers();
        }
        #endregion

        #region DropDown
        private void ComboBox_DropDown(object sender, EventArgs e)
        {

            using (Graphics graphics = CreateGraphics())
            {
                int maxWidth = 0;
                foreach (object obj in ((ComboBox)sender).Items)
                {
                    SizeF area = graphics.MeasureString(obj.ToString(), ((ComboBox)sender).Font);
                    maxWidth = Math.Max((int)area.Width, maxWidth);
                }
                ((ComboBox)sender).DropDownWidth = maxWidth;
            }
        }
        #endregion

        #region Changed
        private void ComboVideoDevices_Changed(object sender, EventArgs e)
        {
            Program.mainForm.Invoke(async () =>
            {
                if (_captureInProgress)
                {
                    Program.mainForm._capture.Pause();
                }

                Program.mainForm._capture?.Dispose();
                Program.mainForm._capture = await Task.FromResult(new VideoCapture(
                    GetCameraIndexForPartName(((ComboBox)sender).SelectedItem as string),
                    VideoCapture.API.Msmf,
                    new Tuple<CapProp, int>[] {
                        Tuple.Create(CapProp.FrameWidth, Form1._width),
                        Tuple.Create(CapProp.FrameHeight, Form1._height)
                    }));// causes slowdown

                Program.mainForm._capture.ImageGrabbed += Program.mainForm.ProcessFrame;

                Program.mainForm.ClientSize = new Size(Program.mainForm._capture.Width, Program.mainForm._capture.Height);

                if (_captureInProgress)
                {
                    Program.mainForm._capture.Start();
                }
            });
        }

        private void ComboWaveInDevice_Changed(object sender, EventArgs e)
        {
            SetupSoundDrivers();
        }

        private void ComboWasapiDevices_Changed(object sender, EventArgs e)
        {
            SetupSoundDrivers();
        }

        private void volumeSlider1_VolumeChanged(object sender, EventArgs e)
        {
            Program.mainForm.Invoke(() =>
            {
                Program.mainForm.sampleChannel.Volume = (sender as NAudio.Gui.VolumeSlider).Volume;
            });
        }
        #endregion
    }
}
