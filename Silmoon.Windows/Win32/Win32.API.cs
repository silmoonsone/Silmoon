using System;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;
using Silmoon.Windows.Win32.API;


namespace Silmoon.Windows.Win32.API
{
    public class USER32
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
            EnumWindows(delegate(IntPtr hWnd, int lParam)
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
    public class ADVAPI32
    {
        public const int SE_PRIVILEGE_ENABLED = 0x00000002;
        public const int TOKEN_QUERY = 0x00000008;
        public const int TOKEN_ADJUST_PRIVILEGES = 0x00000020;
        public const string SE_SHUTDOWN_NAME = "SeShutdownPrivilege";
        public const int EWX_LOGOFF = 0x00000000;
        public const int EWX_SHUTDOWN = 0x00000001;
        public const int EWX_REBOOT = 0x00000002;
        public const int EWX_FORCE = 0x00000004;
        public const int EWX_POWEROFF = 0x00000008;
        public const int EWX_FORCEIFHUNG = 0x00000010;

        [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool OpenProcessToken(IntPtr h, int acc, ref IntPtr phtok);

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool LookupPrivilegeValue(string host, string name, ref long pluid);

        [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool AdjustTokenPrivileges(IntPtr htok, bool disall, ref TokPriv1Luid newst, int len, IntPtr prev, IntPtr relen);
    }
    public class KERNEL32
    {
        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetCurrentProcess();

        public static IntPtr getCurrentProcess()
        {
            return GetCurrentProcess();
        }
    }
    public struct WindowInfo
    {
        public IntPtr hWnd;
        public string szWindowName;
        public string szClassName;
    }

    public struct ShutdownEnum
    {
        public enum ExitWindows : uint
        {
            LogOff = 0x00,      //注销
            ShutDown = 0x01,    //关机
            Reboot = 0x02,      //重启
            Force = 0x04,
            PowerOff = 0x08,
            ForceIfHung = 0x10
        }
        public enum ShutdownReason : uint
        {
            MajorApplication = 0x00040000,
            MajorHardware = 0x00010000,
            MajorLegacyApi = 0x00070000,
            MajorOperatingSystem = 0x00020000,
            MajorOther = 0x00000000,
            MajorPower = 0x00060000,
            MajorSoftware = 0x00030000,
            MajorSystem = 0x00050000,

            MinorBlueScreen = 0x0000000F,
            MinorCordUnplugged = 0x0000000b,
            MinorDisk = 0x00000007,
            MinorEnvironment = 0x0000000c,
            MinorHardwareDriver = 0x0000000d,
            MinorHotfix = 0x00000011,
            MinorHung = 0x00000005,
            MinorInstallation = 0x00000002,
            MinorMaintenance = 0x00000001,
            MinorMMC = 0x00000019,
            MinorNetworkConnectivity = 0x00000014,
            MinorNetworkCard = 0x00000009,
            MinorOther = 0x00000000,
            MinorOtherDriver = 0x0000000e,
            MinorPowerSupply = 0x0000000a,
            MinorProcessor = 0x00000008,
            MinorReconfig = 0x00000004,
            MinorSecurity = 0x00000013,
            MinorSecurityFix = 0x00000012,
            MinorSecurityFixUninstall = 0x00000018,
            MinorServicePack = 0x00000010,
            MinorServicePackUninstall = 0x00000016,
            MinorTermSrv = 0x00000020,
            MinorUnstable = 0x00000006,
            MinorUpgrade = 0x00000003,
            MinorWMI = 0x00000015,

            FlagUserDefined = 0x40000000,
            FlagPlanned = 0x80000000
        }
    }
    // 这个结构体将会传递给API。使用StructLayout(...特性，确保其中的成员是按顺序排列的，C#编译器不会对其进行调整。 
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TokPriv1Luid
    {
        public int Count;
        public long Luid;
        public int Attr;
    }

}