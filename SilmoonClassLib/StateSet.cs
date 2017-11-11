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
        public StateSet() { }
        public StateSet(T state)
        {
            Set(state, null);
        }
        public StateSet(T state, string message)
        {
            Set(state, message);
        }
    }
    public class StateSet<T, TO> : StateSet<T>
    {
        public TO UserState { get; set; }
        public StateSet<T, TO> Set(T state, TO userState, string message)
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
        public StateSet() { }
        public StateSet(T state, TO userState)
        {
            Set(state, userState, null);
        }
        public StateSet(T state, TO userState, string message)
        {
            Set(state, userState, message);
        }

    }
}
