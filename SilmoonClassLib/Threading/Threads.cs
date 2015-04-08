using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Silmoon.Threading
{
    public class Threads
    {
        public static Thread ExecAsync(ThreadStart start)
        {
            Thread _th = new Thread(start);
            _th.IsBackground = true;
            _th.Start();
            return _th;
        }
        public static Thread ExecAsync(ParameterizedThreadStart start, object state)
        {
            Thread _th = new Thread(start);
            _th.IsBackground = true;
            _th.Start(state);
            return _th;
        }
        public static Thread ExecAsync(ThreadStart start, ThreadExceptionEventHandler onExceptionCallback)
        {
            internalProtectExecuteClass executeClass = new internalProtectExecuteClass(start, onExceptionCallback);
            Thread _th = new Thread(executeClass.Execute);
            _th.IsBackground = true;
            _th.Start();
            return _th;
        }
        public static Thread ExecAsync(ParameterizedThreadStart start, object state, ThreadExceptionEventHandler onExceptionCallback)
        {
            internalProtectExecuteClass executeClass = new internalProtectExecuteClass(start, state, onExceptionCallback);
            Thread _th = new Thread(executeClass.Execute2);
            _th.IsBackground = true;
            _th.Start();
            return _th;
        }

        class internalProtectExecuteClass
        {
            event ThreadExceptionEventHandler _onExceptionCallback;
            event ThreadStart _start1;
            event ParameterizedThreadStart _start2;
            object _obj2;

            public internalProtectExecuteClass(ThreadStart start, ThreadExceptionEventHandler onExceptionCallback)
            {
                _start1 = start;
                _onExceptionCallback = onExceptionCallback;
            }
            public internalProtectExecuteClass(ParameterizedThreadStart start, object state, ThreadExceptionEventHandler onExceptionCallback)
            {
                _start2 = start;
                _obj2 = state;
                _onExceptionCallback = onExceptionCallback;
            }
            public void Execute()
            {
                try
                {
                    if (_start1 != null)
                        _start1();
                    else throw (ThreadStartException)new SystemException("没有执行委托代码");
                }
                catch (Exception ex)
                {
                    if (_onExceptionCallback != null)
                        _onExceptionCallback(this, new ThreadExceptionEventArgs(ex));
                }
            }
            public void Execute2()
            {
                try
                {
                    if (_start2 != null)
                        _start2(_obj2);
                    else throw (ThreadStartException)new SystemException("没有执行委托代码");
                }
                catch (Exception ex)
                {
                    if (_onExceptionCallback != null)
                        _onExceptionCallback(this, new ThreadExceptionEventArgs(ex));
                }
            }
        }
        public delegate bool StatusStart();
    }
}
