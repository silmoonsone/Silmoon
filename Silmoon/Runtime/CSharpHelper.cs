using Silmoon.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Runtime
{
    public class CSharpHelper
    {
        public static void TryRun(Action action)
        {
            try
            {
                action();
            }
            catch { }
        }
        public static void TryRun<T>(Action<T> action, T t)
        {
            try
            {
                action(t);
            }
            catch { }
        }
        public static void TryRun<T1, T2>(Action<T1, T2> action, T1 t1, T2 t2)
        {
            try
            {
                action(t1, t2);
            }
            catch { }
        }
        public static void TryRun<T1, T2, T3>(Action<T1, T2, T3> action, T1 t1, T2 t2, T3 t3)
        {
            try
            {
                action(t1, t2, t3);
            }
            catch { }
        }
        public static void TryRun<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            try
            {
                action(t1, t2, t3, t4);
            }
            catch { }
        }
        public static StateSet<bool, ReturnT> TryRun<ReturnT>(Func<ReturnT> func)
        {
            try
            {
                return StateSet<bool, ReturnT>.Create(true, func());
            }
            catch { }
            return StateSet<bool, ReturnT>.Create(false, default);
        }
        public static StateSet<bool, ReturnT> TryRun<T, ReturnT>(Func<T, ReturnT> func, T t)
        {
            try
            {
                return StateSet<bool, ReturnT>.Create(true, func(t));
            }
            catch { }
            return StateSet<bool, ReturnT>.Create(false, default);
        }
        public static StateSet<bool, ReturnT> TryRun<T1, T2, ReturnT>(Func<T1, T2, ReturnT> func, T1 t1, T2 t2)
        {
            try
            {
                return StateSet<bool, ReturnT>.Create(true, func(t1, t2));
            }
            catch { }
            return StateSet<bool, ReturnT>.Create(false, default);
        }
        public static StateSet<bool, ReturnT> TryRun<T1, T2, T3, ReturnT>(Func<T1, T2, T3, ReturnT> func, T1 t1, T2 t2, T3 t3)
        {
            try
            {
                return StateSet<bool, ReturnT>.Create(true, func(t1, t2, t3));
            }
            catch { }
            return StateSet<bool, ReturnT>.Create(false, default);
        }
        public static StateSet<bool, ReturnT> TryRun<T1, T2, T3, T4, ReturnT>(Func<T1, T2, T3, T4, ReturnT> func, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            try
            {
                return StateSet<bool, ReturnT>.Create(true, func(t1, t2, t3, t4));
            }
            catch { }
            return StateSet<bool, ReturnT>.Create(false, default);
        }
    }
}
