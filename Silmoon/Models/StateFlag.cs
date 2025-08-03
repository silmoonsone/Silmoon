using Silmoon.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Models
{
    public class StateFlag
    {
        public string Message { get; set; }
        public int Code { get; set; }
        public bool Success { get; set; } = false;

        public static StateFlag Create(bool success) => new StateFlag() { Success = success };
        public static StateFlag Create(bool success, string message) => new StateFlag() { Success = success, Message = message };
        public static StateFlag Create(bool success, int code) => new StateFlag() { Success = success, Code = code };
        public static StateFlag Create(bool success, int code, string message) => new StateFlag() { Success = success, Message = message, Code = code };
    }

    public class StateFlag<T> : StateFlag
    {
        public T Data { get; set; }

        public static StateFlag<T> Create(bool success, T data) => new StateFlag<T>() { Success = success, Data = data };
        public static StateFlag<T> Create(bool success, T data, string message) => new StateFlag<T>() { Success = success, Message = message, Data = data };
        public static StateFlag<T> Create(bool success, int code, T data) => new StateFlag<T>() { Success = success, Code = code, Data = data };
        public static StateFlag<T> Create(bool success, int code, T data, string message) => new StateFlag<T>() { Success = success, Message = message, Code = code, Data = data };
    }
}
