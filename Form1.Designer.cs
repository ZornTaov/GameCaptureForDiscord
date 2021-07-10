
namespace GameCaptureForDiscord
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            ReleaseData();
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.captureImageBox = new Emgu.CV.UI.ImageBox();
            this.startCaptureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox();
            this.comboWaveInDevice = new System.Windows.Forms.ToolStripComboBox();
            this.comboWasapiDevices = new System.Windows.Forms.ToolStripComboBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.volumeSlider1 = new NAudio.Gui.VolumeSlider();
            ((System.ComponentModel.ISupportInitialize)(this.captureImageBox)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // captureImageBox
            // 
            this.captureImageBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.captureImageBox.Location = new System.Drawing.Point(0, 30);
            this.captureImageBox.Name = "captureImageBox";
            this.captureImageBox.Size = new System.Drawing.Size(1264, 670);
            this.captureImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.captureImageBox.TabIndex = 1;
            this.captureImageBox.TabStop = false;
            // 
            // startCaptureToolStripMenuItem
            // 
            this.startCaptureToolStripMenuItem.Name = "startCaptureToolStripMenuItem";
            this.startCaptureToolStripMenuItem.Size = new System.Drawing.Size(85, 23);
            this.startCaptureToolStripMenuItem.Text = "StartCapture";
            this.startCaptureToolStripMenuItem.Click += new System.EventHandler(this.startCaptureToolStripMenuItem_Click);
            // 
            // toolStripComboBox1
            // 
            this.toolStripComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBox1.Name = "toolStripComboBox1";
            this.toolStripComboBox1.Size = new System.Drawing.Size(121, 23);
            this.toolStripComboBox1.DropDown += new System.EventHandler(this.ToolStripComboBox_DropDown);
            this.toolStripComboBox1.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBox1_Click);
            // 
            // comboWaveInDevice
            // 
            this.comboWaveInDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboWaveInDevice.Name = "comboWaveInDevice";
            this.comboWaveInDevice.Size = new System.Drawing.Size(121, 23);
            this.comboWaveInDevice.DropDown += new System.EventHandler(this.ToolStripComboBox_DropDown);
            this.comboWaveInDevice.SelectedIndexChanged += new System.EventHandler(this.comboWaveInDevice_Click);
            // 
            // comboWasapiDevices
            // 
            this.comboWasapiDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboWasapiDevices.Name = "comboWasapiDevices";
            this.comboWasapiDevices.Size = new System.Drawing.Size(121, 23);
            this.comboWasapiDevices.DropDown += new System.EventHandler(this.ToolStripComboBox_DropDown);
            this.comboWasapiDevices.SelectedIndexChanged += new System.EventHandler(this.comboWasapiDevices_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startCaptureToolStripMenuItem,
            this.toolStripComboBox1,
            this.comboWaveInDevice,
            this.comboWasapiDevices});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1264, 27);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // volumeSlider1
            // 
            this.volumeSlider1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.volumeSlider1.Location = new System.Drawing.Point(952, 8);
            this.volumeSlider1.Name = "volumeSlider1";
            this.volumeSlider1.Size = new System.Drawing.Size(300, 16);
            this.volumeSlider1.TabIndex = 4;
            this.volumeSlider1.VolumeChanged += new System.EventHandler(this.volumeSlider1_VolumeChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1264, 701);
            this.Controls.Add(this.volumeSlider1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.captureImageBox);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximumSize = new System.Drawing.Size(1280, 740);
            this.MinimumSize = new System.Drawing.Size(1280, 740);
            this.Name = "Form1";
            this.Text = "Camera Capture";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.captureImageBox)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Emgu.CV.UI.ImageBox captureImageBox;
        private System.Windows.Forms.ToolStripComboBox comboWaveInDevice;
        private System.Windows.Forms.ToolStripComboBox comboWasapiDevices;
        private System.Windows.Forms.ToolStripMenuItem startCaptureToolStripMenuItem;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private NAudio.Gui.VolumeSlider volumeSlider1;
    }
}

