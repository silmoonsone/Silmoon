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
        public StateSet<TState> Set(TState state, string message = null)
        {
            State = state;
            Message = message;
            return this;
        }

        /// <summary>
        /// 构建实例
        /// </summary>
        public StateSet()
        {
        }

        public StateSet(TState state) => Set(state);

        public StateSet(TState state, string message = null) => Set(state, message);

        public static StateSet<TState> Create(TState state) => new StateSet<TState>(state);

        public static StateSet<TState> Create(TState state, string message) => new StateSet<TState>(state, message);

        public override string ToString() => "State: {" + State + "}, Message: {" + Message + "}";
    }

    /// <summary>
    /// 表示一个带有具体信息的数据和状态
    /// </summary>
    /// <typeparam name="TState">状态的类型</typeparam>
    /// <typeparam name="TData">包含数据的类型</typeparam>
    public class StateSet<TState, TData> : StateSet<TState>
    {
        public TData Data { get; set; }

        public StateSet<TState, TData> Set(TState state, TData data, string message = null)
        {
            State = state;
            Message = message;
            Data = data;
            return this;
        }

        public new StateSet<TState, TData> Set(TState state, string message = null)
        {
            State = state;
            Message = message;
            Data = default;
            return this;
        }

        public StateSet()
        {
        }

        public StateSet(TState state, TData data) => Set(state, data);

        public StateSet(TState state, TData data, string message = null) => Set(state, data, message);

        public static StateSet<TState, TData> Create(TState state, TData data = default) => new StateSet<TState, TData>(state, data);

        public static StateSet<TState, TData> Create(TState state, TData data, string message) => new StateSet<TState, TData>(state, data, message);

        public override string ToString() => "State: {" + State + "}, Data:{" + Data + "}, Message: {" + Message + "}";
    }
}