using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Silmoon.Extension
{
    public static class EnumerableExtension
    {
        public static bool SynchronizeCollectionSizes<TSource, TTarget>(this IEnumerable<TSource> source, IList<TTarget> target) where TTarget : new()
        {
            var sourceCount = source.Count();
            var itemsToAdd = sourceCount - target.Count;
            var result = false;
            if (itemsToAdd > 0)
            {
                // 需要添加元素以匹配源集合的大小
                for (int i = 0; i < itemsToAdd; i++)
                {
                    target.Add(new TTarget());
                    result = true;
                }
            }
            else
            {
                // 需要从目标集合中移除多余的元素
                // 注意：从末尾开始移除是更安全、更高效的方式
                for (int i = target.Count - 1; i >= sourceCount; i--)
                {
                    target.RemoveAt(i);
                    result = true;
                }
            }
            return result;
        }

        public static void Each<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source is null) return;
            foreach (var item in source)
            {
                action(item);
            }
        }
        public static void Each<T>(this IEnumerable source, Action<T> action)
        {
            if (source is null) return;
            foreach (var item in source)
            {
                action((T)item);
            }
        }
        public static void Each(this IEnumerable source, Action<object> action)
        {
            if (source is null) return;
            foreach (var item in source)
            {
                action(item);
            }
        }

        public static void Each<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            if (source is null) return;
            int index = 0;
            foreach (var item in source)
            {
                action(item, ++index);
            }
        }
        public static void Each<T>(this IEnumerable source, Action<T, int> action)
        {
            if (source is null) return;
            int index = 0;
            foreach (var item in source)
            {
                action((T)item, ++index);
            }
        }
        public static void Each(this IEnumerable source, Action<object, int> action)
        {
            if (source is null) return;
            int index = 0;
            foreach (var item in source)
            {
                action(item, ++index);
            }
        }


        public static void Each<T>(this IEnumerable<T> source, Func<T, bool> action)
        {
            if (source is null) return;
            foreach (var item in source)
            {
                if (!action(item)) break;
            }
        }
        public static void Each<T>(this IEnumerable source, Func<T, bool> action)
        {
            if (source is null) return;
            foreach (var item in source)
            {
                if (!action((T)item)) break;
            }
        }
        public static void Each(this IEnumerable source, Func<object, bool> action)
        {
            if (source is null) return;
            foreach (var item in source)
            {
                if (!action(item)) break;
            }
        }

        public static void Each<T>(this IEnumerable<T> source, Func<T, int, bool> action)
        {
            if (source is null) return;
            int index = 0;
            foreach (var item in source)
            {
                if (!action(item, ++index)) break;
            }
        }
        public static void Each<T>(this IEnumerable source, Func<T, int, bool> action)
        {
            if (source is null) return;
            int index = 0;
            foreach (var item in source)
            {
                if (!action((T)item, ++index)) break;
            }
        }
        public static void Each(this IEnumerable source, Func<object, int, bool> action)
        {
            if (source is null) return;
            int index = 0;
            foreach (var item in source)
            {
                if (!action(item, ++index)) break;
            }
        }

    }
}
