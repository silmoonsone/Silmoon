using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon
{
    /// <summary>
    /// 表示一个带有具体信息的状态
    /// </summary>
    /// <typeparam name="T_State">状态的类型</typeparam>
    public class StateSet<T_State>
    {
        /// <summary>
        /// 状态
        /// </summary>
        public T_State State { get; set; }
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
        public StateSet<T_State> Set(T_State state, string message = "")
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
        public StateSet(T_State state)
        {
            Set(state, null);
        }
        /// <summary>
        /// 构建实例，并且指定状态和信息
        /// </summary>
        /// <param name="state"></param>
        /// <param name="message"></param>
        public StateSet(T_State state, string message = "")
        {
            Set(state, message);
        }

        public static StateSet<T_State> Create(T_State state)
        {
            return new StateSet<T_State>(state);
        }
        public static StateSet<T_State> Create(T_State state, string message)
        {
            return new StateSet<T_State>(state, message);

        }
    }
    /// <summary>
    /// 表示一个带有具体信息的数据和状态
    /// </summary>
    /// <typeparam name="T_State">状态的类型</typeparam>
    /// <typeparam name="T_Data">包含数据的类型</typeparam>
    public class StateSet<T_State, T_Data> : StateSet<T_State>
    {
        /// <summary>
        /// 包含的数据
        /// </summary>
        public T_Data Data { get; set; }
        /// <summary>
        /// 构建实例，指定信息、状态和数据
        /// </summary>
        /// <param name="state"></param>
        /// <param name="userState"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public StateSet<T_State, T_Data> Set(T_State state, T_Data data, string message = "")
        {
            State = state;
            Message = message;
            Data = data;
            return this;
        }
        public new StateSet<T_State, T_Data> Set(T_State state, string message = "")
        {
            State = state;
            Message = message;
            Data = default(T_Data);
            return this;
        }
        public StateSet() { }
        public StateSet(T_State state, T_Data data)
        {
            Set(state, data, null);
        }
        public StateSet(T_State state, T_Data data, string message = "")
        {
            Set(state, data, message);
        }

        public static StateSet<T_State, T_Data> Create(T_State state, T_Data data)
        {
            return new StateSet<T_State, T_Data>(state, data);

        }
        public static StateSet<T_State, T_Data> Create(T_State state, T_Data data, string message)
        {
            return new StateSet<T_State, T_Data>(state, data, message);

        }
    }
}
