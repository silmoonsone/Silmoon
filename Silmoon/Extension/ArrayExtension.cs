using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silmoon.Extension
{
    public static class ArrayExtension
    {
        public static T[] Merge<T>(this T[] array1, T[] array2)
        {
            List<T> list = new List<T>();
            foreach (var item in array1)
            {
                list.Add(item);
            }

            foreach (var item in array2)
            {
                if (!list.Contains(item))
                {
                    list.Add(item);
                }
            }
            return list.ToArray();
        }

        public static string[] ToStringArray(this Array array)
        {
            string[] result = new string[array.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = array.GetValue(i).ToString();
            }
            return result;
        }
        public static bool IsNullOrEmpty(this Array array) => array == null || array?.Length == 0;
        public static bool IsNullOrEmpty<T>(this List<T> array) => array == null || array?.Count == 0;
        public static List<DiffResult<T>> CompareWith<T>(this IEnumerable<T> sourceCollection, IEnumerable<T> destinationCollection, Func<T, T, bool> areEqual)
        {
            var missingItems = sourceCollection
                .Where(mainItem => !destinationCollection.Any(otherItem => areEqual(mainItem, otherItem)))
                .Select(item => new DiffResult<T> { Data = item, Type = DiffType.Missing });

            var extraItems = destinationCollection
                .Where(otherItem => !sourceCollection.Any(mainItem => areEqual(mainItem, otherItem)))
                .Select(item => new DiffResult<T> { Data = item, Type = DiffType.Extra });

            var differences = missingItems.Concat(extraItems).ToList();

            return differences;
        }
        public static List<DiffResult<TSourceT, TDestinationT>> CompareWith<TSourceT, TDestinationT>(this IEnumerable<TSourceT> sourceCollection, IEnumerable<TDestinationT> destinationCollection, Func<TSourceT, TDestinationT, bool> areEqual)
        {
            var missingItems = sourceCollection
                .Where(sourceItem => !destinationCollection.Any(destItem => areEqual(sourceItem, destItem)))
                .Select(item => new DiffResult<TSourceT, TDestinationT> { SourceData = item, Type = DiffType.Missing });

            var extraItems = destinationCollection
                .Where(destItem => !sourceCollection.Any(sourceItem => areEqual(sourceItem, destItem)))
                .Select(item => new DiffResult<TSourceT, TDestinationT> { DestinationData = item, Type = DiffType.Extra });

            var differences = missingItems.Concat(extraItems).ToList();

            return differences;
        }
    }
    public class DiffResult<T>
    {
        public T Data { get; set; }
        public DiffType Type { get; set; }
    }
    public class DiffResult<SourceT, DestinationT>
    {
        public SourceT SourceData { get; set; } // Previously Data1
        public DestinationT DestinationData { get; set; } // Previously Data2
        public DiffType Type { get; set; }
    }
    public enum DiffType
    {
        Missing,
        Extra
    }
}