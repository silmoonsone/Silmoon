using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.MySilmoon
{
    public abstract class Runable : IRunable
    {
        private RunningState state = RunningState.Unstarted;

        public event OperateHandler OnStart;
        public event OperateHandler OnStop;
        public event OperateHandler OnSuspend;
        public event OperateHandler OnResume;
        public event OperateHandler OnStateChange;

        #region IRunningAble 成员
        public RunningState State
        {
            get => state; private set
            {
                state = value;
                OnStateChange?.Invoke(this, RunableEventArgs.Create(state));
            }
        }
        public bool Start()
        {
            if (State != RunningState.Running)
            {
                State = RunningState.Starting;
                OnStart?.Invoke(this, RunableEventArgs.Create(State));
                State = RunningState.Running;
                return true;
            }
            else return false;
        }
        public bool Stop()
        {
            if (State == RunningState.Running || State == RunningState.Suspended)
            {
                State = RunningState.Stopping;
                OnStop?.Invoke(this, RunableEventArgs.Create(State));
                State = RunningState.Stopped;
                return true;
            }
            else return false;
        }
        public bool Suspend()
        {
            if (State == RunningState.Running)
            {
                State = RunningState.Suspending;
                OnSuspend?.Invoke(this, RunableEventArgs.Create(State));
                State = RunningState.Suspended;
                return true;
            }
            else return false;
        }
        public bool Resume()
        {
            if (State == RunningState.Suspended)
            {
                State = RunningState.Resuming;
                OnResume?.Invoke(this, RunableEventArgs.Create(State));
                State = RunningState.Running;
                return true;
            }
            else return false;
        }
        #endregion


        public delegate void OperateHandler(Runable sender, RunableEventArgs e);
    }
}
