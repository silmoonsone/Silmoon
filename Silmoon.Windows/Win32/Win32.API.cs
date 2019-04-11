using System;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;
using Silmoon.Windows.Win32.API;


namespace Silmoon.Windows.Win32.API
{
    public struct WindowInfo
    {
        public IntPtr hWnd;
        public string szWindowName;
        public string szClassName;
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