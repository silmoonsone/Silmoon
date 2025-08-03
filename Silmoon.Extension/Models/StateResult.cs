using Newtonsoft.Json;
using Silmoon.Models;
using Silmoon.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Extension.Models
{
    public class StateResult
    {
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("code")]
        public int Code { get; set; }
        [JsonProperty("success")]
        public bool Success { get; set; } = false;

        public static StateResult Create(bool success) => new StateResult() { Success = success };
        public static StateResult Create(bool success, string message) => new StateResult() { Success = success, Message = message };
        public static StateResult Create(bool success, int code) => new StateResult() { Success = success, Code = code };
        public static StateResult Create(bool success, int code, string message) => new StateResult() { Success = success, Message = message, Code = code };
        public StateResult Set(bool success, string message)
        {
            Success = success;
            Message = message;
            return this;
        }
        public StateResult Set(bool success, int code)
        {
            Success = success;
            Code = code;
            return this;
        }
        public StateResult Set(bool success, int code, string message)
        {
            Success = success;
            Code = code;
            Message = message;
            return this;
        }

        public StateFlag ToStateFlag()
        {
            return new StateFlag()
            {
                Success = Success,
                Code = Code,
                Message = Message
            };
        }
    }

    public class StateResult<T> : StateResult
    {
        [JsonProperty("data")]
        public T Data { get; set; }

        public static StateResult<T> Create(bool success, T data) => new StateResult<T>() { Success = success, Data = data };
        public static StateResult<T> Create(bool success, T data, string message) => new StateResult<T>() { Success = success, Message = message, Data = data };
        public static StateResult<T> Create(bool success, T data, int code) => new StateResult<T>() { Success = success, Code = code, Data = data };
        public static StateResult<T> Create(bool success, T data, int code, string message) => new StateResult<T>() { Success = success, Message = message, Code = code, Data = data };

        public StateResult<T> Set(bool success, T data, string message)
        {
            Success = success;
            Data = data;
            Message = message;
            return this;
        }
        public StateResult<T> Set(bool success, T data, int code, string message = null)
        {
            Success = success;
            Data = data;
            Code = code;
            Message = message;
            return this;
        }

        public new StateFlag<T> ToStateFlag()
        {
            return new StateFlag<T>()
            {
                Success = Success,
                Code = Code,
                Message = Message,
                Data = Data
            };
        }
    }
}
