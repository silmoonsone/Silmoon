using System;
using System.Collections.Generic;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Xml;
using Silmoon.MySilmoon.Instance;
using Silmoon.Service;
using Silmoon.Threading;

namespace Silmoon.MySilmoon
{
    /// <summary>
    /// 对银月产品公共库公共属性进行重用
    /// </summary>
    public class SilmoonProductGBCInternat : RunningAble, ISilmoonProductGBCInternat
    {
        private string _productString = "NULL";
        private int _revision = 0;
        private string _releaseVersion = "0";
        private RunningState _runningState = RunningState.Stopped;
        private bool _initProduceInfo = false;
        private string _userIdentity = "#undefined";

        public event OutputTextMessageHandler OnOutputTextMessage;
        public event OutputTextMessageHandler OnInputTextMessage;
        public event ThreadExceptionEventHandler OnThreadException;
        public event Action<VersionResult> OnValidateVersion;

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

        public SilmoonProductGBCInternat()
        {

        }

        public void onOutputText(string message)
        {
            onOutputText(message, 0);
        }
        public void onOutputText(string message, int flag)
        {
            if (OnOutputTextMessage != null) OnOutputTextMessage(message, flag);
        }
        public void onInputText(string message)
        {
            onInputText(message, 0);
        }
        public void onInputText(string message, int flag)
        {
            if (OnInputTextMessage != null) OnInputTextMessage(message, flag);
        }
        public void onThreadException(object sender, ThreadExceptionEventArgs e)
        {
            if (OnThreadException != null) OnThreadException(sender, e);
        }

        public void AsyncValidateVersion()
        {
            Threads.ExecAsync(delegate()
            {
                if (OnValidateVersion != null)
                {
                    var result = MyConfigure.GetRemoteVersion(_productString, _userIdentity);
                    OnValidateVersion(result);
                }
            });
        }

        /// <summary>
        /// 停止目前启动前台功能相应的后台服务。
        /// </summary>
        /// <param name="serviceName">后台服务名称</param>
        public void StopAppService(string serviceName)
        {
            if (Environment.UserInteractive)
            {
                onOutputText("GBC : Application runing at interactive mode...", -999);
                if (ServiceControl.IsExisted(serviceName))
                {
                    onOutputText("GBC : Application associate service(" + serviceName + ") is exist...", -999);
                    using (ServiceController sc = new ServiceController(serviceName))
                    {
                        sc.Refresh();
                        if (sc.Status == ServiceControllerStatus.Running && sc.CanStop)
                        {
                            onOutputText("GBC : Application associate service(" + serviceName + ") is running, shutdown it...", -999);
                            sc.Stop();
                            sc.WaitForStatus(ServiceControllerStatus.Stopped);
                            onOutputText("GBC : Application associate service(" + serviceName + ") has been shutdown...", -999);
                        }
                        else
                        {
                            onOutputText("GBC : Application associate service(" + serviceName + ") not running or can't stop it...", -999);
                        }
                    }
                }
            }
        }

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