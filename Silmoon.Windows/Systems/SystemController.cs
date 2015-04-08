using System;
using System.Collections.Generic;
using System.Text;
using Silmoon.Windows.Win32.API;

namespace Silmoon.Windows.Systems
{
    public class SystemController
    {
        public static bool ShutdownLocalhost(ShutdownEnum.ExitWindows options, ShutdownEnum.ShutdownReason reason)
        {
            TokPriv1Luid tp;
            IntPtr hproc = KERNEL32.getCurrentProcess();
            IntPtr zeroPtr = IntPtr.Zero;
            ADVAPI32.OpenProcessToken(hproc, ADVAPI32.TOKEN_ADJUST_PRIVILEGES | ADVAPI32.TOKEN_QUERY, ref zeroPtr);
            tp.Count = 1;
            tp.Luid = 0;
            tp.Attr = ADVAPI32.SE_PRIVILEGE_ENABLED;
            ADVAPI32.LookupPrivilegeValue(null, ADVAPI32.SE_SHUTDOWN_NAME, ref tp.Luid);
            ADVAPI32.AdjustTokenPrivileges(zeroPtr, false, ref tp, 0, IntPtr.Zero, IntPtr.Zero);
            return USER32.exitWindowsEx(options, reason);
        }
    }
}
