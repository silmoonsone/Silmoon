using Silmoon.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Extension
{
    public static class BoolExtension
    {
        public static StateSet<bool> ToStateSet(this bool value, string Message = null) => StateSet<bool>.Create(value, Message);
        public static StateSet<bool, T> ToStateSet<T>(this bool value, T Data, string Message = null) => StateSet<bool, T>.Create(value, Data, Message);
        public static StateFlag ToStateFlag(this bool value, string Message = null) => StateFlag.Create(value, Message);
        public static StateFlag ToStateFlag<T>(this bool value, T Data, string Message = null) => StateFlag<T>.Create(value, Data, Message);
        public static T IfElse<T>(this bool value, T trueResult, T falseResult) => value ? trueResult : falseResult;
    }
}
