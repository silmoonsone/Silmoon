using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.MySilmoon
{
    [Obsolete("Use General hosting service instead")]
    public class RunableEventArgs : EventArgs
    {
        RunableEventArgs(RunningState State)
        {

        }
        public RunningState State { get; private set; }
        public static RunableEventArgs Create(RunningState State)
        {
            return new RunableEventArgs(State);
        }
    }
}
