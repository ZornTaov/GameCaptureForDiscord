
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
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showControlsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.captureImageBox)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // captureImageBox
            // 
            this.captureImageBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.captureImageBox.Location = new System.Drawing.Point(0, 0);
            this.captureImageBox.Name = "captureImageBox";
            this.captureImageBox.Size = new System.Drawing.Size(1904, 1041);
            this.captureImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.captureImageBox.TabIndex = 1;
            this.captureImageBox.TabStop = false;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showControlsMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(152, 26);
            // 
            // showControlsMenuItem
            // 
            this.showControlsMenuItem.Name = "showControlsMenuItem";
            this.showControlsMenuItem.Size = new System.Drawing.Size(180, 22);
            this.showControlsMenuItem.Text = "Show Controls";
            this.showControlsMenuItem.Click += new System.EventHandler(this.controlsToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1904, 1041);
            this.Controls.Add(this.captureImageBox);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Camera Capture";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.captureImageBox)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Emgu.CV.UI.ImageBox captureImageBox;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem showControlsMenuItem;
    }
}

