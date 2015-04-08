using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.MySilmoon
{
    public abstract class RunningAble : IRunningAble, IDisposable
    {
        public RunningAble()
        {

        }
        public event OperateHandler OnStart;
        public event OperateHandler OnStop;
        public event OperateHandler OnSuspend;
        public event OperateHandler OnResume;

        #region IRunningAble 成员
        private RunningState _runningState = RunningState.Stopped;
        public RunningState RunningState
        {
            get { return _runningState; }
            private set { _runningState = value; }
        }
        public void StartA()
        {
            Start();
        }
        public bool Start()
        {
            bool success = RunningState == MySilmoon.RunningState.Stopped;
            RunningState = MySilmoon.RunningState.Running;
            if (OnStart != null) OnStart(ref success);
            if (!success) RunningState = RunningState.Stopped;
            return success;
        }
        public bool Stop()
        {
            MySilmoon.RunningState runstate = RunningState;
            bool success = RunningState != MySilmoon.RunningState.Stopped;
            RunningState = MySilmoon.RunningState.Stopped;
            if (OnStop != null) OnStop(ref success);
            if (!success) RunningState = runstate;
            return success;
        }
        public bool Suspend()
        {
            bool success = RunningState == MySilmoon.RunningState.Running;
            RunningState = MySilmoon.RunningState.Suspended;
            OnSuspend(ref success);
            if (!success) RunningState = RunningState.Running;
            return success;
        }
        public bool Resume()
        {
            bool success = RunningState == MySilmoon.RunningState.Suspended;
            RunningState = MySilmoon.RunningState.Running;
            OnResume(ref success);
            if (!success) RunningState = RunningState.Suspended;
            return success;
        }
        #endregion

        #region IDisposable 成员

        public virtual void Dispose()
        {
            try
            {
                Stop();
            }
            catch { }
        }

        #endregion

        public delegate void OperateHandler(ref bool success);
    }
}
