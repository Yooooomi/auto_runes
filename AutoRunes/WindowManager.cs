using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AutoRunes
{
    public class WindowManager
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string strClassName, string strWindowName);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        private IntPtr currentWindow;

        public struct Rect
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
        }

        public WindowManager()
        {
        }

        public bool AssignWindow(String name)
        {
            Process[] processes = Process.GetProcessesByName(name);
            if (processes.Length == 0) return false;
            Process lol = processes[0];
            currentWindow = lol.MainWindowHandle;
            return true;
        }

        public bool isUsable()
        {
            return currentWindow != IntPtr.Zero;
        }

        public void SetFocus()
        {
            SetForegroundWindow(currentWindow);
        }

        public Rect GetWindowPos()
        {
            Rect rect = new Rect();
            GetWindowRect(currentWindow, ref rect);
            return rect;
        }
    }
}