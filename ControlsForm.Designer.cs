
namespace GameCaptureForDiscord
{
    partial class ControlsForm
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
            this.volumeSlider1 = new NAudio.Gui.VolumeSlider();
            this.startCaptureButton = new System.Windows.Forms.Button();
            this.comboVideoDevices = new System.Windows.Forms.ComboBox();
            this.comboWaveInDevice = new System.Windows.Forms.ComboBox();
            this.comboWasapiDevices = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.setSoundButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // volumeSlider1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.volumeSlider1, 2);
            this.volumeSlider1.Dock = System.Windows.Forms.DockStyle.Top;
            this.volumeSlider1.Location = new System.Drawing.Point(3, 123);
            this.volumeSlider1.Name = "volumeSlider1";
            this.volumeSlider1.Size = new System.Drawing.Size(278, 15);
            this.volumeSlider1.TabIndex = 5;
            this.volumeSlider1.VolumeChanged += new System.EventHandler(this.volumeSlider1_VolumeChanged);
            // 
            // startCaptureButton
            // 
            this.startCaptureButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.startCaptureButton.Location = new System.Drawing.Point(3, 3);
            this.startCaptureButton.Name = "startCaptureButton";
            this.startCaptureButton.Size = new System.Drawing.Size(85, 24);
            this.startCaptureButton.TabIndex = 6;
            this.startCaptureButton.Text = "StartCapture";
            this.startCaptureButton.Click += new System.EventHandler(this.StartCaptureButton_Click);
            // 
            // comboVideoDevices
            // 
            this.comboVideoDevices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboVideoDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboVideoDevices.Location = new System.Drawing.Point(94, 33);
            this.comboVideoDevices.Name = "comboVideoDevices";
            this.comboVideoDevices.Size = new System.Drawing.Size(187, 21);
            this.comboVideoDevices.TabIndex = 7;
            this.comboVideoDevices.DropDown += new System.EventHandler(this.ComboBox_DropDown);
            this.comboVideoDevices.SelectedIndexChanged += new System.EventHandler(this.ComboVideoDevices_Changed);
            // 
            // comboWaveInDevice
            // 
            this.comboWaveInDevice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboWaveInDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboWaveInDevice.Location = new System.Drawing.Point(94, 63);
            this.comboWaveInDevice.Name = "comboWaveInDevice";
            this.comboWaveInDevice.Size = new System.Drawing.Size(187, 21);
            this.comboWaveInDevice.TabIndex = 8;
            this.comboWaveInDevice.DropDown += new System.EventHandler(this.ComboBox_DropDown);
            this.comboWaveInDevice.SelectedIndexChanged += new System.EventHandler(this.ComboWaveInDevice_Changed);
            // 
            // comboWasapiDevices
            // 
            this.comboWasapiDevices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboWasapiDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboWasapiDevices.Location = new System.Drawing.Point(94, 93);
            this.comboWasapiDevices.Name = "comboWasapiDevices";
            this.comboWasapiDevices.Size = new System.Drawing.Size(187, 21);
            this.comboWasapiDevices.TabIndex = 9;
            this.comboWasapiDevices.DropDown += new System.EventHandler(this.ComboBox_DropDown);
            this.comboWasapiDevices.SelectedIndexChanged += new System.EventHandler(this.ComboWasapiDevices_Changed);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.volumeSlider1, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.startCaptureButton, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.comboVideoDevices, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.comboWasapiDevices, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.comboWaveInDevice, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.setSoundButton, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(284, 141);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 33);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 24);
            this.label1.TabIndex = 10;
            this.label1.Text = "Video Device";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 63);
            this.label2.Margin = new System.Windows.Forms.Padding(3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 24);
            this.label2.TabIndex = 11;
            this.label2.Text = "Input Device";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 93);
            this.label3.Margin = new System.Windows.Forms.Padding(3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 24);
            this.label3.TabIndex = 12;
            this.label3.Text = "Output Device";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // setSoundButton
            // 
            this.setSoundButton.AutoSize = true;
            this.setSoundButton.Location = new System.Drawing.Point(94, 3);
            this.setSoundButton.Name = "setSoundButton";
            this.setSoundButton.Size = new System.Drawing.Size(115, 23);
            this.setSoundButton.TabIndex = 13;
            this.setSoundButton.Text = "Set Sound Devices";
            this.setSoundButton.UseVisualStyleBackColor = true;
            this.setSoundButton.Click += new System.EventHandler(this.setSoundButton_Click);
            // 
            // ControlsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 141);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(300, 180);
            this.MinimumSize = new System.Drawing.Size(300, 180);
            this.Name = "ControlsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ControlsForm";
            this.Load += new System.EventHandler(this.ControlsForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private NAudio.Gui.VolumeSlider volumeSlider1;
        private System.Windows.Forms.ComboBox comboWaveInDevice;
        private System.Windows.Forms.ComboBox comboWasapiDevices;
        private System.Windows.Forms.Button startCaptureButton;
        private System.Windows.Forms.ComboBox comboVideoDevices;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button setSoundButton;
    }
}