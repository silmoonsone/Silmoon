using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Silmoon.Extension;
using Silmoon.Extension.Enums;

namespace Silmoon.Extension.Models
{
    public class ApiResult
    {
        public ApiResult()
        {
            State = ResultState.Unknown;
        }
        public Exception Exception { get; set; }
        public ResultState State { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
        public object Raw { get; set; }
        public ApiResult Init(ResultState State, string Message, int Code = 0, Exception Exception = null)
        {
            this.Code = Code;
            this.State = State;
            this.Message = Message;
            this.Exception = Exception;
            return this;
        }
        public static string CreateToJsonString(ResultState resultState, string Message = null, int Code = 0, Exception Exception = null)
        {
            return Create(resultState, Message, Code, Exception).ToJsonString();
        }
        public static ApiResult Create(ResultState ResultState, string Message = null, int Code = 0, Exception Exception = null)
        {
            var r = new ApiResult().Init(ResultState, Message, Code, Exception);
            return r;
        }
        public virtual ApiResult SetException(Exception Exception)
        {
            this.Exception = Exception;
            return this;
        }
    }
    public class ApiResult<T> : ApiResult
    {
        public ApiResult()
        {
            State = ResultState.Unknown;
        }
        public T Data { get; set; }
        public ApiResult<T> Init(ResultState state, T Data, string Message = null, int Code = 0, Exception exception = null)
        {
            Init(null, state, Data, Message, Code, exception);
            return this;
        }
        public ApiResult<T> Init(string Raw, ResultState State, T Data, string Message, int Code = 0, Exception Exception = null)
        {
            this.Raw = Raw;
            this.Code = Code;
            this.State = State;
            this.Data = Data;
            this.Message = Message;
            this.Exception = Exception;
            return this;
        }
        public static string CreateToJsonString(ResultState resultState, T Data, string Message = null, int Code = 0, Exception Exception = null)
        {
            return Create(resultState, Data, Message, Code, Exception).ToJsonString();
        }
        public static ApiResult<T> Create(ResultState ResultState, T Data, string Message = null, int Code = 0, Exception Exception = null)
        {
            var r = new ApiResult<T>().Init(ResultState, Data, Message, Code, Exception);
            return r;
        }
        public override ApiResult SetException(Exception Exception)
        {
            this.Exception = Exception;
            return this;
        }
    }
}
