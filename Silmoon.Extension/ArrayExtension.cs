using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silmoon.Extension
{
    public static class ArrayExtension
    {
        public static string[] ToStringArray(this Array array)
        {
            string[] result = new string[array.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = array.GetValue(i).ToString();
            }
            return result;
        }
        public static JArray ToJArray(this Array array)
        {
            var result = JArray.FromObject(array);
            return result;
        }
        public static JArray ToJArray<T>(this List<T> list)
        {
            var result = JArray.FromObject(list);
            return result;
        }
        public static bool IsNullOrEmpty(this Array array)
        {
            return array == null || array?.Length == 0;
        }
        public static bool IsNullOrEmpty<T>(this List<T> array)
        {
            return array == null || array?.Count == 0;
        }
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
