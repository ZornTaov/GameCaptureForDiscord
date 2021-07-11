using System;
using System.Windows.Forms;

namespace GameCaptureForDiscord
{
    static class Program
    {
        internal static VideoCaptureForm mainForm;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            SplashForm.ShowSplashScreen();
            mainForm = new VideoCaptureForm();
            Application.Run(mainForm);
        }
    }
}
