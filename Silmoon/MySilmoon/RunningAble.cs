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
        public RunningState State
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
            bool success = State == MySilmoon.RunningState.Stopped;
            ///这里对是否开始做了判断，对于重复开始的情况做了处理，但是下面的几个方法没有。
            if (success)
            {
                State = MySilmoon.RunningState.Running;
                if (OnStart != null) OnStart(ref success);
                if (!success) State = RunningState.Stopped;
            }
            return success;
        }
        public bool Stop()
        {
            MySilmoon.RunningState runstate = State;
            bool success = State != MySilmoon.RunningState.Stopped;
            State = MySilmoon.RunningState.Stopped;
            if (OnStop != null) OnStop(ref success);
            if (!success) State = runstate;
            return success;
        }
        public bool Suspend()
        {
            bool success = State == MySilmoon.RunningState.Running;
            State = MySilmoon.RunningState.Suspended;
            OnSuspend(ref success);
            if (!success) State = RunningState.Running;
            return success;
        }
        public bool Resume()
        {
            bool success = State == MySilmoon.RunningState.Suspended;
            State = MySilmoon.RunningState.Running;
            OnResume(ref success);
            if (!success) State = RunningState.Suspended;
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
