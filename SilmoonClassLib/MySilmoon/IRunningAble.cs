using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Silmoon.MySilmoon
{
    public interface IRunningAble
    {
        RunningState State
        {
            get;
        }
        void StartA();
        bool Start();
        bool Stop();
        bool Suspend();
        bool Resume();
    }
    /// <summary>
    /// 可探测的运行状态
    /// </summary>
    public enum RunningState
    {
        /// <summary>
        /// 刚初始化，未开始运行。
        /// </summary>
        Init = 0,
        /// <summary>
        /// 运行中
        /// </summary>
        Running = 1,
        /// <summary>
        /// 已经停止
        /// </summary>
        Stopped = 2,
        /// <summary>
        /// 已经挂起
        /// </summary>
        Suspended = 3,
    }
}
