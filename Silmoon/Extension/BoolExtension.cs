using Silmoon.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Extension
{
    public static class BoolExtension
    {
        public static StateSet<bool> ToStateSet(this bool value, string message = null) => StateSet<bool>.Create(value, message);
        public static StateSet<bool, T> ToStateSet<T>(this bool value, T data, string message = null) => StateSet<bool, T>.Create(value, data, message);
        public static StateFlag ToStateFlag(this bool value, string message = null) => StateFlag.Create(value, message);
        public static StateFlag ToStateFlag<T>(this bool value, T data, string message = null) => StateFlag<T>.Create(value, data, message);
        public static T IfElse<T>(this bool value, T trueResult, T falseResult) => value ? trueResult : falseResult;
        public static T IfElse<T>(this bool? value, T trueResult, T falseResult, T nullResult) => value.HasValue ? IfElse(value.Value, trueResult, falseResult) : nullResult;
        public static void IfTrue(this bool value, Action action)
        {
            if (value) action();
        }
        public static void IfFalse(this bool value, Action action)
        {
            if (!value) action();
        }
        public static void IfElse(this bool value, Action trueAction, Action falseAction)
        {
            if (value) trueAction();
            else falseAction();
        }

#if NET10_0_OR_GREATER
        extension(bool)
        {
            public static bool Flip() => Random.Shared.Next(0, 2) == 0;
        }
#endif
    }
}
