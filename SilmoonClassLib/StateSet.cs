using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon
{
    /// <summary>
    /// 表示一个带有具体信息的状态
    /// </summary>
    /// <typeparam name="T">状态的类型</typeparam>
    public class StateSet<T>
    {
        /// <summary>
        /// 状态
        /// </summary>
        public T State { get; set; }
        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 设置一个状态和信息
        /// </summary>
        /// <param name="state">状态</param>
        /// <param name="message">信息</param>
        /// <returns></returns>
        public StateSet<T> Set(T state, string message)
        {
            State = state;
            Message = message;
            return this;
        }
        /// <summary>
        /// 构建实例
        /// </summary>
        public StateSet() { }
        /// <summary>
        /// 构建实例，并且指定状态
        /// </summary>
        /// <param name="state"></param>
        public StateSet(T state)
        {
            Set(state, null);
        }
        /// <summary>
        /// 构建实例，并且指定状态和信息
        /// </summary>
        /// <param name="state"></param>
        /// <param name="message"></param>
        public StateSet(T state, string message)
        {
            Set(state, message);
        }
    }
    /// <summary>
    /// 表示一个带有具体信息的数据和状态
    /// </summary>
    /// <typeparam name="T">状态的类型</typeparam>
    /// <typeparam name="TO">包含数据的类型</typeparam>
    public class StateSet<T, TO> : StateSet<T>
    {
        /// <summary>
        /// 包含的数据
        /// </summary>
        public TO UserState { get; set; }
        /// <summary>
        /// 构建实例，指定信息、状态和数据
        /// </summary>
        /// <param name="state"></param>
        /// <param name="userState"></param>
        /// <param name="message"></param>
        /// <returns></returns>
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
