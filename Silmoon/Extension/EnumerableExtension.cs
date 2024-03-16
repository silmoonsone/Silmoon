using System;
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
        public static void ForEachEx<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }
    }
}
