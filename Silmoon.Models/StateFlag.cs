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
        public static StateFlag Create(bool Success, object UserState)
        {
            return new StateFlag() { Success = Success, Data = UserState };
        }
        public static StateFlag Create(bool Success, string Message, object UserState)
        {
            return new StateFlag() { Success = Success, Message = Message, Data = UserState };
        }
        public static StateFlag Create(bool Success, int Code, object UserState)
        {
            return new StateFlag() { Success = Success, StateCode = Code, Data = UserState };
        }
        public static StateFlag Create(bool Success, int Code, string Message, object UserState)
        {
            return new StateFlag() { Success = Success, Message = Message, StateCode = Code, Data = UserState };
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
        public static StateFlag<T> Create(bool Success, T UserState)
        {
            return new StateFlag<T>() { Success = Success, Data = UserState };
        }
        public static StateFlag<T> Create(bool Success, string Message, T UserState)
        {
            return new StateFlag<T>() { Success = Success, Message = Message, Data = UserState };
        }
        public static StateFlag<T> Create(bool Success, int Code, T UserState)
        {
            return new StateFlag<T>() { Success = Success, StateCode = Code, Data = UserState };
        }
        public static StateFlag<T> Create(bool Success, int Code, string Message, T UserState)
        {
            return new StateFlag<T>() { Success = Success, Message = Message, StateCode = Code, Data = UserState };
        }
    }

}
