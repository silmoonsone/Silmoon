using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Silmoon.Windows.Desktops
{
    public class Desktop
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct MARGINS
        {
            public int Left;
            public int Right;
            public int Top;
            public int Bottom;
        };

        [DllImport("DwmApi.dll")]
        static extern int DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS pMarInset);
        [DllImport("dwmapi.dll", EntryPoint = "DwmEnableComposition")]
        extern static uint Win32DwmEnableComposition(uint uCompositionAction);
        [DllImport("dwmapi.dll", PreserveSig = false)]
        extern static bool DwmIsCompositionEnabled();


        public const Int32 AW_HOR_POSITIVE = 0x00000001;    // 从左到右打开窗口  
        public const Int32 AW_HOR_NEGATIVE = 0x00000002;    // 从右到左打开窗口  
        public const Int32 AW_VER_POSITIVE = 0x00000004;    // 从上到下打开窗口  
        public const Int32 AW_VER_NEGATIVE = 0x00000008;    // 从下到上打开窗口  
        public const Int32 AW_CENTER = 0x00000010;
        public const Int32 AW_HIDE = 0x00010000;        // 在窗体卸载时若想使用本函数就得加上此常量  
        public const Int32 AW_ACTIVATE = 0x00020000;    //在窗体通过本函数打开后，默认情况下会失去焦点，除非加上本常量  
        public const Int32 AW_SLIDE = 0x00040000;
        public const Int32 AW_BLEND = 0x00080000;       // 淡入淡出效果  
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern bool AnimateWindow(
        IntPtr hwnd, // handle of window      
        int dwTime, // duration of animation      
        int dwFlags // animation type      
        ); 

        public static bool SetAreoArea(IntPtr ptr, ref MARGINS margins)
        {
            try
            {
                int hr = DwmExtendFrameIntoClientArea(ptr, ref margins);
                if (hr < 0)
                {
                    return false;
                }
            }
            catch (DllNotFoundException)
            {
                return false;
            }
            return true;
        }
        public static bool AnimateWindowManaged(IntPtr hwnd, int dwTime, int dwFlags)
        {
            try
            {
                return AnimateWindow(hwnd, dwTime, dwFlags);
            }
            catch { return false; }
        }
    }
}
