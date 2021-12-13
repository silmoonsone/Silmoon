using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Xml;
using Silmoon.Threading;

namespace Silmoon.MySilmoon
{
    /// <summary>
    /// 对银月产品公共库公共属性进行重用
    /// </summary>
    public class ServerApplication : RunningAble, IServerApplication
    {
        private string _productString = "NULL";
        private int _revision = 0;
        private string _releaseVersion = "0";
        private bool _initProduceInfo = false;
        private string _userIdentity = "#undefined";

        public event OutputTextMessageHandler OnOutputLine;
        public event OutputTextMessageHandler OnInputLine;
        public event ThreadExceptionEventHandler OnException;

        /// <summary>
        /// 标识产品名称字符串
        /// </summary>
        public string ProductString
        {
            get { return _productString; }
            set { _productString = value; }
        }
        /// <summary>
        /// 产品发布序号
        /// </summary>
        public int Revision
        {
            get { return _revision; }
            set { _revision = value; }
        }
        /// <summary>
        /// 版本号
        /// </summary>
        public string ReleaseVersion
        {
            get { return _releaseVersion; }
            set { _releaseVersion = value; }
        }
        public string UserIdentity
        {
            get { return _userIdentity; }
            set { _userIdentity = value; }
        }

        public ServerApplication()
        {

        }


        public void OutputLine()
        {
            OutputLine(null, 0);
        }
        public void OutputLine(object message, int flag = 0)
        {
            if (message is null)
                OutputLine(null, flag);
            else
                OutputLine(message.ToString(), flag);
        }
        public void OutputLine(string message, int flag = 0)
        {
            OnOutputLine?.Invoke(message, flag);
        }
        public void InputLine(string message, int flag = 0)
        {
            OnInputLine?.Invoke(message, flag);
        }

        /// <summary>
        /// 让GBC引发线程错误事件，由GBC的OnThreadException事件捕获并处理。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void onThreadException(object sender, ThreadExceptionEventArgs e)
        {
            if (OnException != null) OnException(sender, e);
        }

        /// <summary>
        /// 使用一个异步方法使用产品字符标识和版本Revision使用MyConfigure.GetRemoteVersion验证程序，由OnValidateVersion处理验证结果。
        /// </summary>
        public void AsyncValidateVersion()
        {
            ThreadHelper.ExecAsync(delegate ()
            {
                if (OnValidateVersion != null)
                {
                    var result = MyConfigure.GetRemoteVersion(_productString, _userIdentity);
                    OnValidateVersion(result);
                }
                else
                {
                    ///当没有对回调验证事件过程调用的时候，而去调用了验证过程的处理，这里理应当抛出一个异常。
                }
            });
        }
        /// <summary>
        /// 使用产品字符标识和版本Revision使用MyConfigure.GetRemoteVersion验证程序，由OnValidateVersion处理验证结果，阻塞线程。
        /// </summary>

        /// <summary>
        /// 停止目前启动前台功能相应的后台服务。
        /// </summary>
        /// <param name="serviceName">后台服务名称</param>
        //public void StopAppService(string serviceName)
        //{
        //    if (Environment.UserInteractive)
        //    {
        //        OutputLine("GBC : Application runing at interactive mode...", -999);
        //        if (ServiceControl.IsExisted(serviceName))
        //        {
        //            OutputLine("GBC : Application associate service(" + serviceName + ") is exist...", -999);
        //            using (ServiceController sc = new ServiceController(serviceName))
        //            {
        //                sc.Refresh();
        //                if (sc.Status == ServiceControllerStatus.Running && sc.CanStop)
        //                {
        //                    OutputLine("GBC : Application associate service(" + serviceName + ") is running, shutdown it...", -999);
        //                    sc.Stop();
        //                    sc.WaitForStatus(ServiceControllerStatus.Stopped);
        //                    OutputLine("GBC : Application associate service(" + serviceName + ") has been shutdown...", -999);
        //                }
        //                else
        //                {
        //                    OutputLine("GBC : Application associate service(" + serviceName + ") not running or can't stop it...", -999);
        //                }
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// 初始化公共属性
        /// </summary>
        /// <param name="productString">指定产品名称字符串</param>
        /// <param name="revision">指定发布产品的序号</param>
        public bool InitProductInfo(string productString, int revision, string releaseVersion = "0")
        {
            if (!_initProduceInfo)
            {
                _productString = productString;
                _revision = revision;
                _releaseVersion = releaseVersion;
                _initProduceInfo = true;
                return true;
            }
            else
                return false;
        }
    }
    public delegate void OutputTextMessageHandler(string message, int flag);
}