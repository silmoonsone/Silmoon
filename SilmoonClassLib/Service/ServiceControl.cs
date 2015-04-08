using System;
using System.Threading;
using System.ServiceProcess;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Silmoon.Net;
using Microsoft.Win32;

namespace Silmoon.Service
{
    /// <summary>
    /// 对系统服务进程操作
    /// </summary>
    public sealed class ServiceControl : MarshalByRefObject , IDisposable
    {
        public ServiceControl()
        {
            InitClass();
        }
        private void InitClass()
        {

        }

        public event SmServiceEventHandler OnServiceStateChange;

        void onServiceStateChange(SmServiceEventArgs e)
        {
            if (this.OnServiceStateChange != null)
            {
                this.OnServiceStateChange(this, e);
            }
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        /// <param name="serviceName">服务名</param>
        /// <returns></returns>
        public bool StopService(string serviceName)
        {
            bool isOK = false;
            SmServiceEventArgs es = new SmServiceEventArgs();
            es.ServiceName = serviceName;
            es.ServiceOption = ServiceOptions.Stop;

            if (IsExisted(serviceName))
            {
                es.CompleteState = ServiceCompleteStateType.Trying;
                onServiceStateChange(es);

                if (CanStop(serviceName))
                {
                    System.ServiceProcess.ServiceController service = new System.ServiceProcess.ServiceController(serviceName);
                    if (service.Status == System.ServiceProcess.ServiceControllerStatus.Running)
                    {
                        try
                        {
                            service.Stop();

                            for (int i = 0; i < 30; i++)
                            {
                                service.Refresh();
                                System.Threading.Thread.Sleep(1000);
                                if (service.Status == System.ServiceProcess.ServiceControllerStatus.Stopped)
                                {
                                    es.CompleteState = ServiceCompleteStateType.Successfully;
                                    es.Error = null;
                                    isOK = true;
                                    break;
                                }
                                if (i == 30)
                                {
                                    es.Error = new Exception("服务在控制时间内操作超时");
                                    es.CompleteState = ServiceCompleteStateType.Timeout;
                                }
                            }
                        }
                        catch
                        {
                            es.Error = new Exception("服务停止失败");
                            es.CompleteState = ServiceCompleteStateType.UncanStop;
                        }
                    }
                    service.Close();
                }
                else
                {
                    es.Error = new Exception("服务不能控制为停止");
                    es.CompleteState = ServiceCompleteStateType.UncanStop;
                }
            }
            else
            {
                es.Error = new Exception("指定的服务不存在");
                es.CompleteState = ServiceCompleteStateType.NoExist;
            }
            onServiceStateChange(es);
            return isOK;
        }
        /// <summary>
        /// 开始服务
        /// </summary>
        /// <param name="serviceName">服务名</param>
        /// <returns></returns>
        public bool StartService(string serviceName)
        {
            bool isOK = false;
            SmServiceEventArgs es = new SmServiceEventArgs();

            es.ServiceName = serviceName;
            es.ServiceOption = ServiceOptions.Start;
            if (IsExisted(serviceName))
            {
                //服务存在
                es.CompleteState = ServiceCompleteStateType.Trying;
                onServiceStateChange(es);

                if (GetRunType(serviceName) != ServiceStartType.Disabled)
                {

                    //服务不是禁用的
                    System.ServiceProcess.ServiceController service = new System.ServiceProcess.ServiceController(serviceName);
                    if (service.Status != ServiceControllerStatus.Running && service.Status != ServiceControllerStatus.StartPending)
                    {
                        //服务不是运行中的
                        try { service.Start(); }
                        catch (Exception ex)
                        {
                            es.Error = ex;
                            es.CompleteState = ServiceCompleteStateType.Error;
                            onServiceStateChange(es);
                            return false;
                        }

                        for (int i = 0; i < 30; i++)
                        {
                            service.Refresh();
                            System.Threading.Thread.Sleep(1000);
                            if (service.Status == System.ServiceProcess.ServiceControllerStatus.Running)
                            {
                                es.CompleteState = ServiceCompleteStateType.Successfully;
                                es.Error = null;
                                onServiceStateChange(es);
                                isOK = true;
                                break;
                            }
                            if (i == 30)
                            {
                                es.Error = new Exception("服务在控制时间内操作超时");
                                es.CompleteState = ServiceCompleteStateType.Timeout;
                                isOK = true;
                            }
                        }
                    }
                    else
                    {
                        es.CompleteState = ServiceCompleteStateType.Successfully;
                        es.Error = new Exception("服务已经运行");
                        onServiceStateChange(es);
                    }
                    service.Close();
                }
                else
                {
                    //服是禁用的
                    es.Error = new Exception("服务在禁用状态不能操作");
                    es.CompleteState = ServiceCompleteStateType.Disabled;
                    onServiceStateChange(es);
                }
            }
            else
            {
                //服务不存在
                es.Error = new Exception("指定的服务不存在");
                es.CompleteState = ServiceCompleteStateType.NoExist;
                onServiceStateChange(es);
            }
            return isOK;
        }
        /// <summary>
        /// 重启服务
        /// </summary>
        /// <param name="serviceName">服务名</param>
        /// <returns></returns>
        public bool ResetService(string serviceName)
        {
            bool isOK = false;
            SmServiceEventArgs es = new SmServiceEventArgs();

            if (IsExisted(serviceName))
            {
                es.ServiceName = serviceName;
                es.ServiceOption = ServiceOptions.Reset;
                es.CompleteState = ServiceCompleteStateType.Trying;
                onServiceStateChange(es);

                if (StopService(serviceName))
                    if (StartService(serviceName)) { isOK = true; }

                //es.ServiceOption = ServiceOptions.Reset;
                //es.Error = !isOK;
                //if (!isOK) { es.CompleteState = ServiceCompleteStateType.Successfully; }
                //else { es.CompleteState = ServiceCompleteStateType.Error; }
                //es.Error = !isOK;
                //onServiceStateChange(es);
            }
            else
            {
                es.ServiceName = serviceName;
                es.ServiceOption = ServiceOptions.Reset;
                es.CompleteState = ServiceCompleteStateType.NoExist;
                es.Error = new Exception("指定的服务不存在");
                onServiceStateChange(es);
            }

            return isOK;
        }

        /// <summary>
        /// 异步控制服务
        /// </summary>
        /// <param name="serviceName">服务名</param>
        /// <param name="so">控制选项</param>
        public void AsyncService(string serviceName, ServiceOptions so)
        {
            ServiceThreadMethod stm = new ServiceThreadMethod(this, serviceName, so);
            Thread _th = new Thread(new ThreadStart(stm.RunThread));
            _th.Start();
        }
        private class ServiceThreadMethod
        {
            public ServiceControl _ss;
            public string _serviceName;
            public ServiceOptions _so;

            public void RunThread()
            {
                if (_ss != null)
                {
                    if (_serviceName != null)
                    {
                        switch (_so)
                        {
                            case ServiceOptions.Start:
                                _ss.StartService(_serviceName);
                                break;
                            case ServiceOptions.Stop:
                                _ss.StopService(_serviceName);
                                break;
                            case ServiceOptions.Reset:
                                _ss.ResetService(_serviceName);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            public ServiceThreadMethod(ServiceControl ss, string serviceName, ServiceOptions so)
            {
                _ss = ss;
                _serviceName = serviceName;
                _so = so;
            }
        }

        /// <summary>
        /// 返回访问是否能够停止
        /// </summary>
        /// <param name="serviceName">服务名</param>
        /// <returns></returns>
        public bool CanStop(string serviceName)
        {
            if (IsExisted(serviceName))
            {
                ServiceController cs = new ServiceController();
                cs.ServiceName = serviceName;
                if (cs.CanStop)
                {
                    cs.Close();
                    return true;
                }
                else
                {
                    cs.Close();
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 服务是否存在
        /// </summary>
        /// <param name="serviceName">服务名</param>
        /// <returns></returns>
        public static bool IsExisted(string serviceName)
        {
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController s in services)
            {
                if (s.ServiceName.ToLower() == serviceName.ToLower())
                { return true; }
            }
            return false;
        }
        /// <summary>
        /// 设置服务启动方式
        /// </summary>
        /// <param name="_type">类型</param>
        /// <param name="serviceName">服务名</param>
        public static void SetRunType(ServiceStartType _type, string serviceName)
        {
            RegistryKey k = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\" + serviceName + "\\", true);
            k.SetValue("Start", Convert.ToInt32(_type), RegistryValueKind.DWord);
            k.Close();
        }
        /// <summary>
        /// 获取服务启动方式
        /// </summary>
        /// <param name="serviceName">服务名</param>
        /// <returns></returns>
        public static ServiceStartType GetRunType(string serviceName)
        {
            if (IsExisted(serviceName))
            {
                RegistryKey k = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\" + serviceName + "\\", true);
                ServiceStartType sstype = ((ServiceStartType)Convert.ToInt32(k.GetValue("Start")));
                k.Close();
                return sstype;
            }
            else
                throw new Exception("服务不存在");
        }
        /// <summary>
        /// 设置服务类型
        /// </summary>
        /// <param name="serviceName">服务名</param>
        /// <param name="_type">类型</param>
        public static void SetServiceType(string serviceName, ServiceType _type)
        {
            RegistryKey k = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\" + serviceName + "\\", true);
            k.SetValue("Type", Convert.ToInt32(_type), RegistryValueKind.DWord);
            k.Close();
        }
        /// <summary>
        /// 获取服务类型
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public static ServiceType GetServiceType(string serviceName)
        {
            if (IsExisted(serviceName))
            {
                RegistryKey k = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\" + serviceName + "\\", true);
                ServiceType sstype = ((ServiceType)Convert.ToInt32(k.GetValue("Type")));
                k.Close();
                return sstype;
            }
            else
                throw new Exception("服务不存在");
        }

        #region IDisposable 成员

        public void Dispose()
        {

        }

        #endregion
    }
    /// <summary>
    /// 为SmServiceEventHandler提供事件参数
    /// </summary>
    [Serializable]
    public class SmServiceEventArgs : System.EventArgs
    {
        private string _serviceName;
        private Exception _error = null;
        private ServiceCompleteStateType _completeState = ServiceCompleteStateType.None;
        private ServiceOptions _serviceOption = ServiceOptions.None;

        public ServiceOptions ServiceOption
        {
            get { return _serviceOption; }
            set { _serviceOption = value; }
        }
        public string ServiceName
        {
            get { return _serviceName; }
            set { _serviceName = value; }
        }
        public Exception Error
        {
            get { return _error; }
            set { _error = value; }
        }
        public ServiceCompleteStateType CompleteState
        {
            get { return _completeState; }
            set { _completeState = value; }
        }
    }
    /// <summary>
    /// Service事件托管
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void SmServiceEventHandler(object sender, SmServiceEventArgs e);
    /// <summary>
    /// Service 状态
    /// </summary>
    public enum ServiceCompleteStateType
    {
        Successfully = 1,
        Unsucceed = 2,
        NoExist = 3,
        Timeout = 4,
        UncanStop = 5,
        Error = 6,
        Trying = 7,
        Disabled = 8,
        None = 0,
    }
    /// <summary>
    /// 
    /// </summary>
    public enum ServiceOptions
    {
        None = 0,
        Stop = 1,
        Start = 2,
        Reset = 3
    }
    /// <summary>
    /// 服务自启动方式
    /// </summary>
    public enum ServiceStartType
    {
        Starting = 0,
        System = 1,
        Automatic = 2,
        Manual = 3,
        Disabled = 4,
        NoExist = 5,
    }
    /// <summary>
    /// 服务类型枚举
    /// </summary>
    public enum ServiceType
    {
        /// <summary>
        /// 一般的配置
        /// </summary>
        Normal = 16,
        /// <summary>
        /// 允许服务与桌面互交
        /// </summary>
        ShowOnDesktop = 272,
    }
}