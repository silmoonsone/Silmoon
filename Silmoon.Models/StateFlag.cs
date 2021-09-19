using Silmoon.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Models
{
    public class StateFlag : IStateFlag
    {
        public string Message { get; set; } = "";
        public int StateCode { get; set; } = 0;
        public bool Success { get; set; } = false;
        public object Data { get; set; }

        StateFlag()
        {

        }

        public static StateFlag Create(bool Success)
        {
            return new StateFlag() { Success = Success };
        }
        public static StateFlag Create(bool Success, string Message)
        {
            return new StateFlag() { Success = Success, Message = Message };
        }
        public static StateFlag Create(bool Success, int Code)
        {
            return new StateFlag() { Success = Success, StateCode = Code };
        }
        public static StateFlag Create(bool Success, int Code, string Message)
        {
            return new StateFlag() { Success = Success, Message = Message, StateCode = Code };
        }
        public static StateFlag Create(bool Success, object Data)
        {
            return new StateFlag() { Success = Success, Data = Data };
        }
        public static StateFlag Create(bool Success, string Message, object Data)
        {
            return new StateFlag() { Success = Success, Message = Message, Data = Data };
        }
        public static StateFlag Create(bool Success, int Code, object Data)
        {
            return new StateFlag() { Success = Success, StateCode = Code, Data = Data };
        }
        public static StateFlag Create(bool Success, int Code, string Message, object Data)
        {
            return new StateFlag() { Success = Success, Message = Message, StateCode = Code, Data = Data };
        }
        public StateFlag AppendData(object data)
        {
            Data = data;
            return this;
        }
    }

    public class StateFlag<T> : IStateFlag<T>
    {
        public string Message { get; set; } = "";
        public int StateCode { get; set; } = 0;
        public bool Success { get; set; } = false;
        public T Data { get; set; }

        StateFlag()
        {

        }

        public static StateFlag<T> Create(bool Success)
        {
            return new StateFlag<T>() { Success = Success };
        }
        public static StateFlag<T> Create(bool Success, string Message)
        {
            return new StateFlag<T>() { Success = Success, Message = Message };
        }
        public static StateFlag<T> Create(bool Success, int Code)
        {
            return new StateFlag<T>() { Success = Success, StateCode = Code };
        }
        public static StateFlag<T> Create(bool Success, int Code, string Message)
        {
            return new StateFlag<T>() { Success = Success, Message = Message, StateCode = Code };
        }
        public static StateFlag<T> Create(bool Success, T Data)
        {
            return new StateFlag<T>() { Success = Success, Data = Data };
        }
        public static StateFlag<T> Create(bool Success, string Message, T Data)
        {
            return new StateFlag<T>() { Success = Success, Message = Message, Data = Data };
        }
        public static StateFlag<T> Create(bool Success, int Code, T Data)
        {
            return new StateFlag<T>() { Success = Success, StateCode = Code, Data = Data };
        }
        public static StateFlag<T> Create(bool Success, int Code, string Message, T Data)
        {
            return new StateFlag<T>() { Success = Success, Message = Message, StateCode = Code, Data = Data };
        }
        public StateFlag<T> AppendData(T data)
        {
            Data = data;
            return this;
        }
    }

}
