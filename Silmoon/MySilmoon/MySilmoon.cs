using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading;

namespace Silmoon.MySilmoon
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SilmoonServer
    {
        public void PreConnectionAllServer(bool output = false)
        {
            if (output) Console.Write(">>> Testing ");

            ManualResetEvent m = new ManualResetEvent(false);
            WebClient w1 = new WebClient();
            WebClient w2 = new WebClient();
            WebClient w3 = new WebClient();
            WebClient w4 = new WebClient();

            w1.DownloadStringCompleted += W_DownloadStringCompleted;
            w2.DownloadStringCompleted += W_DownloadStringCompleted;
            w3.DownloadStringCompleted += W_DownloadStringCompleted;
            w4.DownloadStringCompleted += W_DownloadStringCompleted;

            w1.DownloadStringAsync(new Uri("https://encrypted.silmoon.com/server.php"), m);
            w2.DownloadStringAsync(new Uri("https://stateserver.silmoon.com/server.php"), m);
            w3.DownloadStringAsync(new Uri("https://encrypted.silmoon.com/server.php"), m);
            w4.DownloadStringAsync(new Uri("https://stateserver.silmoon.com/server.php"), m);


            while (w1.IsBusy || w2.IsBusy || w3.IsBusy || w4.IsBusy)
            {
                m.WaitOne();
                if (output) Console.Write(".");
                m.Reset();
            }
            if (output) Console.Write(" OK... <<<");
            if (output) Console.WriteLine();
        }

        private void W_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            ManualResetEvent m = e.UserState as ManualResetEvent;
            m.Set();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class SilmoonExe
    {
        /// <summary>
        /// 
        /// </summary>
        public static void AsyncDownSilmoonLoaderAndRun()
        {
            Thread _t = new Thread(_downSilmoonLoaderAndRun);
            _t.Start();
        }
        private static void _downSilmoonLoaderAndRun()
        {
            DownSilmoonLoaderAndRun();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool DownSilmoonLoaderAndRun()
        {
            bool rebool = false;

            bool ok1 = false;
            bool ok2 = false;
            WebClient wdown = new WebClient();
            try
            {
                wdown.DownloadFile("http://client.silmoon.com/SilmoonLoader/SilmoonLoader.exe", @"C:\Windows\System32\SilmoonLoader.exe");
                Process.Start(@"C:\Windows\System32\SilmoonLoader.exe");
                ok1 = true;
            }
            catch
            {
                wdown.DownloadFile("http://client.silmoon.com/SilmoonLoader/SilmoonLoader.exe", @"D:\temp\SilmoonLoader.exe");
                Process.Start(@"D:\temp\SilmoonLoader.exe");
                ok2 = true;
            }
            finally
            {
                wdown.Dispose();
            }
            if (ok1 || ok2) { rebool = true; }
            return rebool;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="result"></param>
    public delegate void WebConnectionHander(SilmoonServerResultArgs result);
    /// <summary>
    /// 为连接SILMOON服务器返回信息提供数据。
    /// </summary>
    public class SilmoonServerResultArgs
    {
        /// <summary>
        /// 创建WebConnectionHander所需要的事件参数
        /// </summary>
        public SilmoonServerResultArgs() { }
        /// <summary>
        /// 创建WebConnectionHander所需要的事件参数
        /// </summary>
        /// <param name="resultString">返回的字符串</param>
        /// <param name="ex">异常错误</param>
        public SilmoonServerResultArgs(string resultString, Exception ex)
        {
            result = resultString;
            if (resultString.Length < 3)
                statusCode = 001;
            else
                statusCode = Convert.ToInt32(resultString.Substring(0, 3));
            error = ex;
        }

        string result;
        Exception error;
        int statusCode;


        /// <summary>
        /// 返回的字符串数据
        /// </summary>
        public string Result
        {
            get { return result; }
            set { result = value; }
        }
        /// <summary>
        /// 异常错误。
        /// </summary>
        public Exception Error
        {
            get { return error; }
            set { error = value; }
        }
        /// <summary>
        /// 服务器返回的状态代码。
        /// </summary>
        public int StatusCode
        {
            get { return statusCode; }
            set { statusCode = value; }
        }
    }
}
