using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Gma.System.MouseKeyHook;

namespace AutoRunes
{
    public class MouseManager
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct MousePoint
        {
            public int X;
            public int Y;

            public MousePoint(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        [Flags]
        public enum MouseEventFlags
        {
            LeftDown = 0x00000002,
            LeftUp = 0x00000004,
            MiddleDown = 0x00000020,
            MiddleUp = 0x00000040,
            Move = 0x00000001,
            Absolute = 0x00008000,
            RightDown = 0x00000008,
            RightUp = 0x00000010
        }

        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(out MousePoint lpMousePoint);

        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        private IKeyboardMouseEvents globalHook;

        public void addListener(EventHandler<MouseEventExtArgs> cb)
        {
            globalHook.MouseDownExt += cb;
        }

        public void removeListener(EventHandler<MouseEventExtArgs> cb)
        {
            globalHook.MouseDownExt -= cb;
        }

        public void enableMouseClicks()
        {
            globalHook = Hook.GlobalEvents();
        }

        public void disableMouseClicks()
        {
            globalHook.Dispose();
            globalHook = null;
        }

        public void SetCursorPosition(int x, int y)
        {
            SetCursorPos(x, y);
        }

        public void SetCursorPosition(MousePoint point)
        {
            SetCursorPos(point.X, point.Y);
        }

        public MousePoint GetCursorPosition()
        {
            MousePoint currentMousePoint;
            var gotPoint = GetCursorPos(out currentMousePoint);
            if (!gotPoint) { currentMousePoint = new MousePoint(0, 0); }
            return currentMousePoint;
        }

        public void MouseEvent(MouseEventFlags value)
        {
            MousePoint position = GetCursorPosition();

            mouse_event((int) value, position.X, position.Y, 0, 0);
        }

        public void Click()
        {
            MouseEvent(MouseEventFlags.LeftDown | MouseEventFlags.LeftUp);
        }
    }
}