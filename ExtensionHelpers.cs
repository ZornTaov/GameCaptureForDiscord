using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameCaptureForDiscord
{
    public static class ExtensionHelpers
    {
        /// <summary>
        /// TODO: Documentation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="control"></param>
        /// <param name="action"></param>
        public static void Invoke<T>(this T control, Action action) where T : Control
        {
            control.Invoke(action);
        }
    }
}
