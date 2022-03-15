using Silmoon.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Models
{
    public class StateFlag
    {
        public string Message { get; set; }
        [Obsolete]
        public int StateCode { get => Code; set { Code = value; } }
        public int Code { get; set; }
        public bool Success { get; set; } = false;

        public StateFlag()
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
            return new StateFlag() { Success = Success, Code = Code };
        }
        public static StateFlag Create(bool Success, int Code, string Message)
        {
            return new StateFlag() { Success = Success, Message = Message, Code = Code };
        }
    }

    public class StateFlag<T> : StateFlag
    {
        public T Data { get; set; }

        public StateFlag()
        {

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
            return new StateFlag<T>() { Success = Success, Code = Code, Data = Data };
        }
        public static StateFlag<T> Create(bool Success, int Code, string Message, T Data)
        {
            return new StateFlag<T>() { Success = Success, Message = Message, Code = Code, Data = Data };
        }
    }

}
