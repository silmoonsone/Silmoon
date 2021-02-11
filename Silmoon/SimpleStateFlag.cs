﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silmoon
{
    public class SimpleStateFlag
    {
        public int StateCode { get; set; } = 0;
        public string Message { get; set; } = "";
        public bool Success { get; set; } = false;
        public object Data { get; set; }
        public SimpleStateFlag()
        {

        }
        public SimpleStateFlag(bool success, int stateCode, string message)
        {
            Success = success;
            StateCode = stateCode;
            Message = message;
        }
        public SimpleStateFlag(bool success, int stateCode)
        {
            Success = success;
            StateCode = stateCode;
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
        public static SimpleStateFlag Create(bool success)
        {
            return new SimpleStateFlag(success);
        }
        public static SimpleStateFlag Create(bool success, string message)
        {
            return new SimpleStateFlag(success, message);
        }
        public static SimpleStateFlag Create(bool success, int stateCode)
        {
            return new SimpleStateFlag(success, stateCode);
        }
        public static SimpleStateFlag Create(bool success, int stateCode, string message)
        {
            return new SimpleStateFlag(success, stateCode, message);
        }
        public static SimpleStateFlag Create(bool success, object data)
        {
            return new SimpleStateFlag(success) { Data = data };
        }
        public static SimpleStateFlag Create(bool success, string message, object data)
        {
            return new SimpleStateFlag(success, message) { Data = data };
        }
        public static SimpleStateFlag Create(bool success, int stateCode, object data)
        {
            return new SimpleStateFlag(success, stateCode) { Data = data };
        }
        public static SimpleStateFlag Create(bool success, int stateCode, string message, object data)
        {
            return new SimpleStateFlag(success, stateCode, message) { Data = data };
        }
        public SimpleStateFlag AppendData(object data)
        {
            Data = data;
            return this;
        }
    }
    public class SimpleStateFlag<T>
    {
        public int StateCode { get; set; } = 0;
        public string Message { get; set; } = "";
        public bool Success { get; set; } = false;
        public T Data { get; set; }
        public SimpleStateFlag()
        {

        }
        public SimpleStateFlag(bool success, int stateCode, string message)
        {
            Success = success;
            StateCode = stateCode;
            Message = message;
        }
        public SimpleStateFlag(bool success, int stateCode)
        {
            Success = success;
            StateCode = stateCode;
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
        public static SimpleStateFlag<T> Create(bool success)
        {
            return new SimpleStateFlag<T>(success);
        }
        public static SimpleStateFlag<T> Create(bool success, string message)
        {
            return new SimpleStateFlag<T>(success, message);
        }
        public static SimpleStateFlag<T> Create(bool success, int stateCode)
        {
            return new SimpleStateFlag<T>(success, stateCode);
        }
        public static SimpleStateFlag<T> Create(bool success, int stateCode, string message)
        {
            return new SimpleStateFlag<T>(success, stateCode, message);
        }
        public static SimpleStateFlag<T> Create(bool success, T data)
        {
            return new SimpleStateFlag<T>(success) { Data = data };
        }
        public static SimpleStateFlag<T> Create(bool success, string message, T data)
        {
            return new SimpleStateFlag<T>(success, message) { Data = data };
        }
        public static SimpleStateFlag<T> Create(bool success, int stateCode, T data)
        {
            return new SimpleStateFlag<T>(success, stateCode) { Data = data };
        }
        public static SimpleStateFlag<T> Create(bool success, int stateCode, string message, T data)
        {
            return new SimpleStateFlag<T>(success, stateCode, message) { Data = data };
        }
        public SimpleStateFlag<T> AppendData(T data)
        {
            Data = data;
            return this;
        }
    }

}
