using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Silmoon.Windows.Win32.API
{
    public class Kernel32
    {
        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetCurrentProcess();

        public static IntPtr getCurrentProcess()
        {
            return GetCurrentProcess();
        }
    }
}
