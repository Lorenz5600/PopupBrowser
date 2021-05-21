using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PopupBrowser
{

    public static class OSHelper
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            public Int32 X;
            public Int32 Y;
        };

        public static Point GetScreenCursor(System.Windows.Media.Visual visual)
        {
            var w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);
            var transform = PresentationSource.FromVisual(visual).CompositionTarget.TransformFromDevice;
            return transform.Transform(new Point(w32Mouse.X, w32Mouse.Y));
        }

        public static bool OSUsesLightTheme
        {
            get
            {
                return 1==(int)Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize")?.GetValue("AppsUseLightTheme",1);
            }
        }
    }

    
}
