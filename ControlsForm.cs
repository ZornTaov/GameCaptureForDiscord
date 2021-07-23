using Emgu.CV;
using Emgu.CV.CvEnum;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using SharpDX.MediaFoundation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace GameCaptureForDiscord
{
    public partial class ControlsForm : Form
    {
        private bool loaded = false;
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
            comboVideoDevices.DataSource = await Task.FromResult(VideoCaptureForm.ListOfAttachedCameras());// causes slowdown
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
                if (Program.mainForm._captureInProgress)
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
                var delay = new OffsetSampleProvider(Program.mainForm.sampleChannel)
                {
                    DelayBy = TimeSpan.FromMilliseconds(Program.mainForm.AudioDelayMS)
                };
                Program.mainForm.wo = new DirectSoundOut(((DirectSoundDeviceInfo)comboWasapiDevices.SelectedItem).Guid);
                Program.mainForm.wo.Init(delay);

                Console.WriteLine("Input now: {0}, should be {1}",
                    ((WaveInCapabilities)comboWaveInDevice.SelectedItem).ProductName,
                    WaveIn.GetCapabilities(Program.mainForm.wi.DeviceNumber).ProductName);
                Console.WriteLine("Input now: {0}, should be {1}",
                    ((DirectSoundDeviceInfo)comboWasapiDevices.SelectedItem).Description,
                    DirectSoundOut.Devices.First(d => d.Guid == ((DirectSoundDeviceInfo)comboWasapiDevices.SelectedItem).Guid).Description);

                if (Program.mainForm._captureInProgress)
                {
                    Program.mainForm.sampleChannel.Volume = volumeSlider1.Volume;
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
                    if (Program.mainForm._captureInProgress)
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

                    Program.mainForm._captureInProgress = !Program.mainForm._captureInProgress;
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
                await Program.mainForm.SetupVideo(comboVideoDevices.SelectedItem as string);
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
                Program.mainForm.sampleChannel.Volume = volumeSlider1.Volume;
            });
        }

        private void numericVideoDelay_ValueChanged(object sender, EventArgs e)
        {
            Program.mainForm.Invoke(() =>
            {
                Program.mainForm.VideoDelayMS = (double)numericVideoDelay.Value;
            });
        }

        private void numericAudioDelay_ValueChanged(object sender, EventArgs e)
        {
            Program.mainForm.Invoke(() =>
            {
                Program.mainForm.AudioDelayMS = (double)numericAudioDelay.Value;
            });
            SetupSoundDrivers();
        }
        #endregion

    }
}
