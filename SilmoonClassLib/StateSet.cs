using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon
{
    public class StateSet<T>
    {
        public T State { get; set; }
        public string Message { get; set; }
        public StateSet<T> Set(T state, string message)
        {
            State = state;
            Message = message;
            return this;
        }
    }
    public class StateSet<T, TO> : StateSet<T>
    {
        public TO UserState { get; set; }
        public StateSet<T, TO> Set(T state, string message, TO userState)
        {
            State = state;
            Message = message;
            UserState = userState;
            return this;
        }
        public new StateSet<T, TO> Set(T state, string message)
        {
            State = state;
            Message = message;
            UserState = default(TO);
            return this;
        }

    }
}
