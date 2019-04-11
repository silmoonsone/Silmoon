using System;
using System.Collections.Generic;
using System.Text;
using Silmoon.Windows.Win32.API;
using Silmoon.Windows.Win32.API.APIEnum;

namespace Silmoon.Windows.Systems
{
    public class SystemController
    {
        public static bool ShutdownLocalhost(ShutdownEnum.ExitWindows options, ShutdownEnum.ShutdownReason reason)
        {
            TokPriv1Luid tp;
            IntPtr hproc = Kernel32.getCurrentProcess();
            IntPtr zeroPtr = IntPtr.Zero;
            AdvApi32.OpenProcessToken(hproc, AdvApi32.TOKEN_ADJUST_PRIVILEGES | AdvApi32.TOKEN_QUERY, ref zeroPtr);
            tp.Count = 1;
            tp.Luid = 0;
            tp.Attr = AdvApi32.SE_PRIVILEGE_ENABLED;
            AdvApi32.LookupPrivilegeValue(null, AdvApi32.SE_SHUTDOWN_NAME, ref tp.Luid);
            AdvApi32.AdjustTokenPrivileges(zeroPtr, false, ref tp, 0, IntPtr.Zero, IntPtr.Zero);
            return User32.exitWindowsEx(options, reason);
        }
    }
}
