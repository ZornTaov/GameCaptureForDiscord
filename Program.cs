using System;
using System.Windows.Forms;

namespace GameCaptureForDiscord
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            SplashForm.ShowSplashScreen();
            Application.Run(new Form1());
        }
    }
}
