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
        public static List<DiffResult<T>> CompareWith<T>(this IEnumerable<T> mainCollection, IEnumerable<T> otherCollection, Func<T, T, bool> areEqual, Func<T, int> getHashCode)
        {
            var mainLookup = mainCollection.ToLookup(getHashCode);
            var otherLookup = otherCollection.ToLookup(getHashCode);

            var missingItems = mainCollection
                .Where(item => !otherLookup[getHashCode(item)].Any(x => areEqual(x, item)))
                .Select(item => new DiffResult<T> { Data = item, Type = DiffType.Missing });

            var extraItems = otherCollection
                .Where(item => !mainLookup[getHashCode(item)].Any(x => areEqual(x, item)))
                .Select(item => new DiffResult<T> { Data = item, Type = DiffType.Extra });

            var differences = missingItems.Concat(extraItems).ToList();

            return differences;
        }
    }
    public class DiffResult<T>
    {
        public T Data { get; set; }
        public DiffType Type { get; set; }
    }
    public enum DiffType
    {
        Missing,
        Extra
    }
}
