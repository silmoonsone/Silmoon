using Silmoon.Extensions.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Extensions
{
    public static class BoolExtension2
    {
        public static StateResult ToStateResult(this bool value, string message = null) => StateResult.Create(value, message);
        public static StateResult ToStateResult(this bool value, int code, string message = null) => StateResult.Create(value, code, message);
        public static StateResult ToStateResult<T>(this bool value, T data, string message = null) => StateResult<T>.Create(value, data, message);
        public static StateResult ToStateResult<T>(this bool value, T data, int code, string message = null) => StateResult<T>.Create(value, data, code, message);
    }
}
