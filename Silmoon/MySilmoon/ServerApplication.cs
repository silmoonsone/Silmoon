using Silmoon.MySilmoon.Models;
using Silmoon.Threading;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Silmoon.MySilmoon
{
    /// <summary>
    /// 对银月产品公共库公共属性进行重用
    /// </summary>
    [Obsolete("Use General hosting service instead")]
    public class ServerApplication : Runable, IServerApplication
    {
        private bool _initProduceInfo = false;

        public event OutputTextMessageHandler OnOutputLine;
        public event OutputTextMessageHandler OnInputLine;
        public event ThreadExceptionEventHandler OnError;
        public event Action<VersionResult> OnValidateVersion;

        public string ProductString { get; set; } = "NULL";
        public int Revision { get; set; } = 0;
        public string ReleaseVersion { get; set; } = "0";
        public string UserIdentity { get; set; } = "#undefined";

        public ServerApplication()
        {

        }



        public void OutputLine()
        {
            OutputLine(null, 0);
        }
        public void OutputLine(string message, int flag = 0)
        {
            OnOutputLine?.Invoke(this, ServerApplicationEventArgs.Create(message, flag));
        }
        public void InputLine(string message, int flag = 0)
        {
            OnInputLine?.Invoke(this, ServerApplicationEventArgs.Create(message, flag));
        }
        public void RunAndWaitConsoleLine()
        {
            while (State != RunningState.Stopped && State != RunningState.Init && State != RunningState.Unstarted)
            {
                var consoleInput = Console.ReadLine();
                InputLine(consoleInput, 0);
            }
        }

        public void Error(object sender, ThreadExceptionEventArgs e)
        {
            if (OnError != null) OnError(sender, e);
        }

        /// <summary>
        /// 使用一个异步方法使用产品字符标识和版本Revision使用MyConfigure.GetRemoteVersion验证程序，由OnValidateVersion处理验证结果。
        /// </summary>
        public void AsyncValidateVersion()
        {
            Task.Run(() =>
            {
                if (OnValidateVersion != null)
                {
                    var result = MyConfigure.GetRemoteVersion(ProductString, UserIdentity);
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
        public VersionResult ValidateVersion()
        {
            return MyConfigure.GetRemoteVersion(ProductString, UserIdentity);
        }

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
                ProductString = productString;
                Revision = revision;
                ReleaseVersion = releaseVersion;
                _initProduceInfo = true;
                return true;
            }
            else
                return false;
        }
    }
    public delegate void OutputTextMessageHandler(IServerApplication sender, ServerApplicationEventArgs e);
}
