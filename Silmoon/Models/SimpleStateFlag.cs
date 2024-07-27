using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silmoon.Models
{
    [Obsolete]
    public class SimpleStateFlag
    {
        public int Code { get; set; } = 0;
        [Obsolete]
        public int StateCode => Code;
        public string Message { get; set; } = string.Empty;
        public bool Success { get; set; } = false;
        public SimpleStateFlag()
        {

        }
        public SimpleStateFlag(bool success, int stateCode, string message)
        {
            Success = success;
            Code = stateCode;
            Message = message;
        }
        public SimpleStateFlag(bool success, int stateCode)
        {
            Success = success;
            Code = stateCode;
        }
        public SimpleStateFlag(bool success, string message)
        {
            Success = success;
            Message = message;
        }
        public SimpleStateFlag(bool success) => Success = success;
        public static SimpleStateFlag Create(bool success) => new SimpleStateFlag(success);
        public static SimpleStateFlag Create(bool success, string message) => new SimpleStateFlag(success, message);
        public static SimpleStateFlag Create(bool success, int stateCode) => new SimpleStateFlag(success, stateCode);
        public static SimpleStateFlag Create(bool success, int stateCode, string message) => new SimpleStateFlag(success, stateCode, message);
        public static SimpleStateFlag FromStateFlag(StateFlag stateFlag)
        {
            return new SimpleStateFlag()
            {
                Message = stateFlag.Message,
                Code = stateFlag.Code,
                Success = stateFlag.Success
            };
        }
    }
    [Obsolete]
    public class SimpleStateFlag<T> : SimpleStateFlag
    {
        public T Data { get; set; }
        public SimpleStateFlag()
        {

        }
        public SimpleStateFlag(bool success, int stateCode, string message)
        {
            Success = success;
            Code = stateCode;
            Message = message;
        }
        public SimpleStateFlag(bool success, int stateCode)
        {
            Success = success;
            Code = stateCode;
        }
        public SimpleStateFlag(bool success, string message)
        {
            Success = success;
            Message = message;
        }
        public SimpleStateFlag(bool success)
        {
            Success = success;
        }
        public static SimpleStateFlag<T> Create(bool success, T data) => new SimpleStateFlag<T>(success) { Data = data };
        public static SimpleStateFlag<T> Create(bool success, string message, T data) => new SimpleStateFlag<T>(success, message) { Data = data };
        public static SimpleStateFlag<T> Create(bool success, int stateCode, T data) => new SimpleStateFlag<T>(success, stateCode) { Data = data };
        public static SimpleStateFlag<T> Create(bool success, int stateCode, string message, T data) => new SimpleStateFlag<T>(success, stateCode, message) { Data = data };
        public static SimpleStateFlag<T> FromStateFlag(StateFlag<T> stateFlag)
        {
            return new SimpleStateFlag<T>()
            {
                Data = stateFlag.Data,
                Message = stateFlag.Message,
                Code = stateFlag.Code,
                Success = stateFlag.Success
            };
        }
        public SimpleStateFlag<T> AppendData(T data)
        {
            Data = data;
            return this;
        }
    }

}
