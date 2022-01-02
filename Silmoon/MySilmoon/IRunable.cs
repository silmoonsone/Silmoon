using System;
using System.Collections.Generic;
using System.Text;
using static Silmoon.MySilmoon.Runable;

namespace Silmoon.MySilmoon
{
    public interface IRunable
    {
        event OperateHandler OnStart;
        event OperateHandler OnStop;
        event OperateHandler OnSuspend;
        event OperateHandler OnResume;
        event OperateHandler OnStateChange;

        RunningState State { get; }
        bool Start();
        bool Stop();
        bool Suspend();
        bool Resume();
    }
    public enum RunningState
    {
        Init,
        Unstarted,
        Starting,
        Running,
        Stopping,
        Stopped,
        Suspending,
        Suspended,
        Resuming,
    }
}
