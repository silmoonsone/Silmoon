using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Windows.Forms;
using System.Threading;

namespace Silmoon.SmSystem
{
    public sealed class SmTime
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern int SetLocalTime(ref SystemTime lpSystemTime);
        private SystemTime st = new SystemTime();
        private static bool _asyncTimeSlient = true;

        public SystemTime SystemTime
        {
            get { return st; }
            set { st = value; }
        }
        public SmTime()
        {

        }

        public void SetSystemTime(short Year, short Month, short Day, short Hour, short Minute, short Second)
        {
            st.wYear = Year;
            st.wMonth = Month;
            st.wDay = Day;
            st.wHour = Hour;
            st.wMinute = Minute;
            st.wSecond = Second;
        }
        public bool SetLocalTime()
        {
            bool rebool = false;
            if (SetLocalTime(ref st) == 1) { rebool = true; }
            else { rebool = false; }
            return rebool;
        }

        public static bool StaticSetLocalTime(short Year, short Month, short Day, short Hour, short Minute, short Second)
        {
            bool rebool = false;
            SystemTime st = new SystemTime();
            st.wYear = Year;
            st.wMonth = Month;
            st.wDay = Day;
            st.wHour = Hour;
            st.wMinute = Minute;
            st.wSecond = Second;
            if (SetLocalTime(ref st) == 1) { rebool = true; }
            else { rebool = false; }
            return rebool;
        }
        public static bool StaticSetLocalTime(DateTime times)
        {
            bool rebool = false;
            SystemTime st = new SystemTime();
            st.wYear = Convert.ToInt16(times.ToString("yyyy"));
            st.wMonth = Convert.ToInt16(times.ToString("MM"));
            st.wDay = Convert.ToInt16(times.ToString("dd"));
            st.wHour = Convert.ToInt16(times.ToString("HH"));
            st.wMinute = Convert.ToInt16(times.ToString("mm"));
            st.wSecond = Convert.ToInt16(times.ToString("ss"));
            if (SetLocalTime(ref st) == 1) { rebool = true; }
            else { rebool = false; }
            return rebool;
        }
        public static void StaticAsyncLocalTime(bool slient)
        {
            _asyncTimeSlient = slient;
            Thread _th_TimeAsync = new Thread(StaticAsyncThread);
            _th_TimeAsync.Start();
        }
        private static SystemTimeHander _completedEvent;
        public static void StaticAsyncLocalTime(bool slient, SystemTimeHander callBack)
        {
            SystemTimeEventArgs _eventArgs = new SystemTimeEventArgs();
            _eventArgs.Now = DateTime.Now;
            _eventArgs.Option = SystemTimeOption.Processing;
            _completedEvent = callBack;

            if (_completedEvent != null) { _completedEvent(_eventArgs); }
            StaticAsyncLocalTime(slient);
        }
        private static void StaticAsyncThread()
        {
            SystemTimeEventArgs _eventArgs = new SystemTimeEventArgs();

            try
            {
                WebClient wClit = new WebClient();
                string result = wClit.DownloadString(new Uri("http://www.silmoon.com/System/NowTime"));

                _eventArgs.Result = Convert.ToDateTime(result);
                _eventArgs.Now = DateTime.Now;
                if (SmTime.StaticSetLocalTime(Convert.ToDateTime(result)))
                {
                    if (!_asyncTimeSlient)
                    { MessageBox.Show("同步时间成功[" + result + "]", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                    _eventArgs.Option = SystemTimeOption.Completed;
                }
                else
                {
                    if (!_asyncTimeSlient)
                    { MessageBox.Show("同步时间失败[" + result + "]", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    _eventArgs.Option = SystemTimeOption.Fail;
                }
            }
            catch (Exception e)
            {
                if (!_asyncTimeSlient)
                { System.Windows.Forms.MessageBox.Show("同步时间出错：\r\n\r\n" + e.ToString(), "错误", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error); }
                _eventArgs.Error = e;
            }
            if (_completedEvent != null) { _completedEvent(_eventArgs); }
        }

    }
    public delegate void SystemTimeHander(SystemTimeEventArgs e);
    public class SystemTimeEventArgs : System.EventArgs
    {
        public SystemTimeOption Option;
        public DateTime Result;
        public DateTime Now;
        public Exception Error;
    }
    public enum SystemTimeOption
    {
        Processing=1,
        Completed=2,
        Fail=3,
    }

    public struct SystemTime
    {
        public short wYear;
        public short wMonth;
        public short wDayOfWeek;
        public short wDay;
        public short wHour;
        public short wMinute;
        public short wSecond;
        public short wMilliseconds;
    }
    public class DOS
    {
        public static string GetShortName(string s)
        {
            StringBuilder shortpath = new StringBuilder(255);
            kernel32.GetShortPathName(s, shortpath, shortpath.Capacity);
            return shortpath.ToString();

        }
    }
    public class kernel32
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetShortPathName(
         string lpszLongPath,
         StringBuilder shortFile,
         int cchBuffer
        );

        [DllImport("winmm.dll", EntryPoint = "mciSendString", CharSet = CharSet.Auto)]
        public static extern int mciSendString(
         string lpstrCommand,
         string lpstrReturnString,
         int uReturnLength,
         int hwndCallback
        );
    }


}