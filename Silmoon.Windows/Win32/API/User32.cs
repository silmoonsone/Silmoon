using Silmoon.Windows.Win32.API.APIEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Silmoon.Windows.Win32.API
{
    public class User32
    {
        [DllImport("user32.dll")]
        private static extern int SetWindowTextW(IntPtr hWhd, [MarshalAs(UnmanagedType.BStr)]string lpString);
        [DllImport("user32.dll")]
        private static extern int GetWindowTextW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)]StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll")]
        private static extern int GetClassNameW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)]StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        private static extern bool EnumWindows(WNDENUMPROC lpEnumFunc, int lParam);

        private delegate bool WNDENUMPROC(IntPtr hWnd, int lParam);
        [DllImport("User32.DLL")]
        private static extern int SendMessage(int hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        static extern bool ExitWindowsEx(ShutdownEnum.ExitWindows uFlags, ShutdownEnum.ShutdownReason dwReason);

        /// <summary>
        /// 设置指定句柄的窗口的标题
        /// </summary>
        /// <param name="hWnd">句柄</param>
        /// <param name="text">标题</param>
        /// <returns></returns>
        public static int setWindowTextW(IntPtr hWnd, string text)
        {
            return SetWindowTextW(hWnd, text);
        }
        /// <summary>
        /// 获取指定句柄的窗口的标题
        /// </summary>
        /// <param name="hWnd">标题</param>
        /// <param name="sbuilder">已有缓冲区的StringBuilder</param>
        /// <param name="reffCount">返回长度</param>
        /// <returns></returns>
        public static int getWindowTextW(IntPtr hWnd, StringBuilder sbuilder, int reffCount)
        {
            return GetWindowTextW(hWnd, sbuilder, reffCount);
        }
        /// <summary>
        /// 返回指定窗口的类信息
        /// </summary>
        /// <param name="hWnd">句柄</param>
        /// <param name="sbuilder">已有缓冲区的StringBuilder</param>
        /// <param name="reffCount">返回长度</param>
        /// <returns></returns>
        public static int getClassNameW(IntPtr hWnd, StringBuilder sbuilder, int reffCount)
        {
            return GetClassNameW(hWnd, sbuilder, reffCount);
        }
        /// <summary>
        /// 获取当前活动窗口的句柄
        /// </summary>
        /// <returns></returns>
        public static IntPtr getForegroundWindow()
        {
            return GetForegroundWindow();
        }
        /// <summary>
        /// 设置当前的活动窗口
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <returns></returns>
        public static bool setForegroundWindow(IntPtr hWnd)
        {
            return SetForegroundWindow(hWnd);
        }
        /// <summary>
        /// 枚举当前桌面所有的句柄！
        /// </summary>
        /// <returns></returns>
        public static WindowInfo[] enumWindows()
        {
            List<WindowInfo> wndList = new List<WindowInfo>();

            //enum all desktop windows 
            EnumWindows(delegate (IntPtr hWnd, int lParam)
            {
                WindowInfo wnd = new WindowInfo();
                StringBuilder sb = new StringBuilder(256);
                //get hwnd 
                wnd.hWnd = hWnd;
                //get window name 
                GetWindowTextW(hWnd, sb, sb.Capacity);
                wnd.szWindowName = sb.ToString();
                //get window class 
                GetClassNameW(hWnd, sb, sb.Capacity);
                wnd.szClassName = sb.ToString();
                //add it into list 
                wndList.Add(wnd);
                return true;
            }, 0);

            return wndList.ToArray();
        }
        /// <summary>
        /// 向指定的句柄窗口发送消息
        /// </summary>
        /// <param name="hWnd">目标句柄</param>
        /// <param name="mSg">消息ID</param>
        /// <param name="WParam">参数1</param>
        /// <param name="LParam">参数2</param>
        /// <returns></returns>
        public static int sendMessage(int hWnd, int mSg, int WParam, int LParam)
        {
            return SendMessage(hWnd, mSg, WParam, LParam);
        }

        public static bool exitWindowsEx(ShutdownEnum.ExitWindows Pew, ShutdownEnum.ShutdownReason Per)
        {
            return ExitWindowsEx(Pew, Per);
        }

    }
}
