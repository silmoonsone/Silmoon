using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Models
{
    /// <summary>
    /// 表示一个带有具体信息的状态
    /// </summary>
    /// <typeparam name="TState">状态的类型</typeparam>
    public class StateSet<TState>
    {
        /// <summary>
        /// 状态
        /// </summary>
        public TState State { get; set; }
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
        public StateSet<TState> Set(TState state, string message = "")
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
        public StateSet(TState state)
        {
            Set(state, null);
        }
        /// <summary>
        /// 构建实例，并且指定状态和信息
        /// </summary>
        /// <param name="state"></param>
        /// <param name="message"></param>
        public StateSet(TState state, string message = "")
        {
            Set(state, message);
        }

        public static StateSet<TState> Create(TState state)
        {
            return new StateSet<TState>(state);
        }
        public static StateSet<TState> Create(TState state, string message)
        {
            return new StateSet<TState>(state, message);

        }
    }
    /// <summary>
    /// 表示一个带有具体信息的数据和状态
    /// </summary>
    /// <typeparam name="TState">状态的类型</typeparam>
    /// <typeparam name="TData">包含数据的类型</typeparam>
    public class StateSet<TState, TData> : StateSet<TState>
    {
        /// <summary>
        /// 包含的数据
        /// </summary>
        public TData Data { get; set; }
        /// <summary>
        /// 构建实例，指定信息、状态和数据
        /// </summary>
        /// <param name="state"></param>
        /// <param name="userState"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public StateSet<TState, TData> Set(TState state, TData data, string message = "")
        {
            State = state;
            Message = message;
            Data = data;
            return this;
        }
        public new StateSet<TState, TData> Set(TState state, string message = "")
        {
            State = state;
            Message = message;
            Data = default(TData);
            return this;
        }
        public StateSet() { }
        public StateSet(TState state, TData data)
        {
            Set(state, data, null);
        }
        public StateSet(TState state, TData data, string message = "")
        {
            Set(state, data, message);
        }

        public static StateSet<TState, TData> Create(TState state, TData data = default)
        {
            return new StateSet<TState, TData>(state, data);

        }
        public static StateSet<TState, TData> Create(TState state, TData data, string message)
        {
            return new StateSet<TState, TData>(state, data, message);

        }
    }
}
