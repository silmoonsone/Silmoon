using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Silmoon.Extension
{
    public static class ArrayExtension
    {
        /// <summary>
        /// 合并两个数组，移除重复元素。
        /// 使用 HashSet 优化性能，避免 O(n²) 的 Contains 操作。
        /// </summary>
        /// <typeparam name="T">数组元素类型</typeparam>
        /// <param name="array1">第一个数组</param>
        /// <param name="array2">第二个数组</param>
        /// <returns>合并后的不包含重复元素的新数组</returns>
        /// <example>
        /// <code>
        /// int[] arr1 = {1, 2, 3};
        /// int[] arr2 = {3, 4, 5};
        /// int[] merged = arr1.Merge(arr2); // 结果: {1, 2, 3, 4, 5}
        /// 
        /// string[] names1 = {"Alice", "Bob"};
        /// string[] names2 = {"Bob", "Charlie"};
        /// string[] allNames = names1.Merge(names2); // 结果: {"Alice", "Bob", "Charlie"}
        /// </code>
        /// </example>
        public static T[] Merge<T>(this T[] array1, T[] array2)
        {
            if (array1 == null && array2 == null) return new T[0];
            if (array1 == null) return array2;
            if (array2 == null) return array1;

            var seen = new HashSet<T>();
            var result = new List<T>(array1.Length + array2.Length); // 预分配容量

            // 添加第一个数组的所有元素
            foreach (var item in array1)
            {
                if (seen.Add(item)) // HashSet.Add 返回 true 表示元素不存在
                {
                    result.Add(item);
                }
            }

            // 添加第二个数组的不重复元素
            foreach (var item in array2)
            {
                if (seen.Add(item)) // HashSet.Add 返回 true 表示元素不存在
                {
                    result.Add(item);
                }
            }

            return result.ToArray();
        }
        /// <summary>
        /// 将数组中的所有元素使用指定的分隔符合并成一个字符串。
        /// </summary>
        /// <param name="array">要合并的数组</param>
        /// <param name="split">用于分隔元素的字符串</param>
        /// <param name="removeLastSplit">是否移除最后一个分隔符，默认为 true</param>
        /// <param name="prefix">每个元素的前缀，默认为空字符串</param>
        /// <param name="suffix">每个元素的后缀，默认为空字符串</param>
        /// <returns>合并后的字符串</returns>
        /// <example>
        /// <code>
        /// int[] numbers = {1, 2, 3, 4, 5};
        /// string result = numbers.Join(", "); // 结果: "1, 2, 3, 4, 5"
        /// 
        /// string[] items = {"apple", "banana", "cherry"};
        /// string html = items.Join("", true, "&lt;", "&gt;"); // 结果: "&lt;apple&gt;&lt;banana&gt;&lt;cherry&gt;"
        /// </code>
        /// </example>
        public static string Join(this Array array, string split, bool removeLastSplit = true, string prefix = StringHelper.EmptyString, string suffix = StringHelper.EmptyString)
        {
            if (array == null || array.Length == 0) return string.Empty;

            var stringBuilder = new StringBuilder(array.Length * (prefix.Length + suffix.Length + split.Length + 10)); // 预分配容量
            foreach (object s in array)
            {
                stringBuilder.Append(prefix);
                stringBuilder.Append(s);
                stringBuilder.Append(suffix);
                stringBuilder.Append(split);
            }

            if (removeLastSplit && stringBuilder.Length > 0)
            {
                stringBuilder.Length -= split.Length;
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// 将数组中的所有元素转换为字符串数组。
        /// </summary>
        /// <param name="array">要转换的数组</param>
        /// <returns>字符串数组，如果输入为 null 则返回 null</returns>
        /// <example>
        /// <code>
        /// int[] numbers = {1, 2, 3, 4, 5};
        /// string[] strings = numbers.ToStringArray(); // 结果: {"1", "2", "3", "4", "5"}
        /// 
        /// DateTime[] dates = {DateTime.Now, DateTime.Today};
        /// string[] dateStrings = dates.ToStringArray(); // 结果: {"2024-01-01 12:00:00", "2024-01-01 00:00:00"}
        /// </code>
        /// </example>
        public static string[] ToStringArray(this Array array)
        {
            if (array == null) return null;

            string[] result = new string[array.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = array.GetValue(i)?.ToString() ?? string.Empty;
            }
            return result;
        }
        /// <summary>
        /// 检查集合是否为 null 或空集合。
        /// </summary>
        /// <typeparam name="T">集合元素类型</typeparam>
        /// <param name="array">要检查的集合</param>
        /// <returns>如果集合为 null 或空集合则返回 true，否则返回 false</returns>
        /// <example>
        /// <code>
        /// List&lt;int&gt; list1 = null;
        /// bool result1 = list1.IsNullOrEmpty(); // 结果: true
        /// 
        /// List&lt;string&gt; list2 = new List&lt;string&gt;();
        /// bool result2 = list2.IsNullOrEmpty(); // 结果: true
        /// 
        /// List&lt;int&gt; list3 = new List&lt;int&gt; {1, 2, 3};
        /// bool result3 = list3.IsNullOrEmpty(); // 结果: false
        /// </code>
        /// </example>
        public static bool IsNullOrEmpty<T>(this ICollection<T> array) => array == null || array?.Count == 0;
        /// <summary>
        /// 比较两个相同类型集合的差异，返回缺失和多余的元素。
        /// </summary>
        /// <typeparam name="T">集合元素类型</typeparam>
        /// <param name="sourceCollection">源集合</param>
        /// <param name="destinationCollection">目标集合</param>
        /// <param name="isEqualComparison">元素相等性比较函数</param>
        /// <returns>包含差异结果的列表</returns>
        /// <example>
        /// <code>
        /// List&lt;int&gt; source = {1, 2, 3, 4};
        /// List&lt;int&gt; destination = {2, 3, 4, 5};
        /// var differences = source.CompareDifferences(destination, (a, b) => a == b);
        /// // 结果: Missing: {1}, Extra: {5}
        /// 
        /// List&lt;string&gt; names1 = {"Alice", "Bob", "Charlie"};
        /// List&lt;string&gt; names2 = {"Bob", "Charlie", "David"};
        /// var nameDiffs = names1.CompareDifferences(names2, (a, b) => a.Equals(b, StringComparison.OrdinalIgnoreCase));
        /// // 结果: Missing: {"Alice"}, Extra: {"David"}
        /// </code>
        /// </example>
        public static List<DiffResult<T>> CompareDifferences<T>(this IEnumerable<T> sourceCollection, IEnumerable<T> destinationCollection, Func<T, T, bool> isEqualComparison)
        {
            if (sourceCollection == null) sourceCollection = new List<T>();
            if (destinationCollection == null) destinationCollection = new List<T>();

            var sourceList = sourceCollection.ToList();
            var destinationList = destinationCollection.ToList();

            var missingItems = sourceList
                .Where(sourceItem => !destinationList.Any(destItem => isEqualComparison(sourceItem, destItem)))
                .Select(item => new DiffResult<T> { Data = item, Type = DiffType.Missing });

            var extraItems = destinationList
                .Where(destItem => !sourceList.Any(sourceItem => isEqualComparison(sourceItem, destItem)))
                .Select(item => new DiffResult<T> { Data = item, Type = DiffType.Extra });

            var differences = missingItems.Concat(extraItems).ToList();

            return differences;
        }
        /// <summary>
        /// 比较两个不同类型集合的差异，返回缺失和多余的元素。
        /// </summary>
        /// <typeparam name="TSourceT">源集合元素类型</typeparam>
        /// <typeparam name="TDestinationT">目标集合元素类型</typeparam>
        /// <param name="sourceCollection">源集合</param>
        /// <param name="destinationCollection">目标集合</param>
        /// <param name="isEqualComparison">元素相等性比较函数</param>
        /// <returns>包含差异结果的列表</returns>
        /// <example>
        /// <code>
        /// List&lt;int&gt; source = {1, 2, 3, 4};
        /// List&lt;string&gt; destination = {"2", "3", "4", "5"};
        /// var differences = source.CompareCollections(destination, (a, b) => a.ToString() == b);
        /// // 结果: Missing: {1}, Extra: {"5"}
        /// 
        /// List&lt;Person&gt; people = {new Person("Alice"), new Person("Bob")};
        /// List&lt;string&gt; names = {"Bob", "Charlie"};
        /// var diffs = people.CompareCollections(names, (p, n) => p.Name == n);
        /// // 结果: Missing: {Person("Alice")}, Extra: {"Charlie"}
        /// </code>
        /// </example>
        public static List<DiffResult<TSourceT, TDestinationT>> CompareCollections<TSourceT, TDestinationT>(this IEnumerable<TSourceT> sourceCollection, IEnumerable<TDestinationT> destinationCollection, Func<TSourceT, TDestinationT, bool> isEqualComparison)
        {
            if (sourceCollection == null) sourceCollection = new List<TSourceT>();
            if (destinationCollection == null) destinationCollection = new List<TDestinationT>();

            var sourceList = sourceCollection.ToList();
            var destinationList = destinationCollection.ToList();

            var missingItems = sourceList
                .Where(sourceItem => !destinationList.Any(destItem => isEqualComparison(sourceItem, destItem)))
                .Select(item => new DiffResult<TSourceT, TDestinationT> { SourceData = item, Type = DiffType.Missing });

            var extraItems = destinationList
                .Where(destItem => !sourceList.Any(sourceItem => isEqualComparison(sourceItem, destItem)))
                .Select(item => new DiffResult<TSourceT, TDestinationT> { DestinationData = item, Type = DiffType.Extra });

            var differences = missingItems.Concat(extraItems).ToList();

            return differences;
        }

        #region Obsolete Methods - 兼容性保留，将在未来版本中删除

        /// <summary>
        /// 比较两个相同类型集合的差异，返回缺失和多余的元素
        /// </summary>
        /// <typeparam name="T">集合元素类型</typeparam>
        /// <param name="sourceCollection">源集合</param>
        /// <param name="destinationCollection">目标集合</param>
        /// <param name="isEqualComparison">元素相等性比较函数</param>
        /// <returns></returns>
        [Obsolete("方法名 'CompareDiffWith' 不够清晰，请使用 'CompareDifferences' 方法", false)]
        public static List<DiffResult<T>> CompareDiffWith<T>(this IEnumerable<T> sourceCollection, IEnumerable<T> destinationCollection, Func<T, T, bool> isEqualComparison) => CompareDifferences(sourceCollection, destinationCollection, isEqualComparison);

        /// <summary>
        /// 比较两个不同类型集合的差异，返回缺失和多余的元素
        /// </summary>
        /// <typeparam name="TSourceT">源集合元素类型</typeparam>
        /// <typeparam name="TDestinationT">目标集合元素类型</typeparam>
        /// <param name="sourceCollection">源集合</param>
        /// <param name="destinationCollection">目标集合</param>
        /// <param name="isEqualComparison">元素相等性比较函数</param>
        /// <returns></returns>
        [Obsolete("方法名 'CompareWith' 不够清晰，请使用 'CompareCollections' 方法", false)]
        public static List<DiffResult<TSourceT, TDestinationT>> CompareWith<TSourceT, TDestinationT>(this IEnumerable<TSourceT> sourceCollection, IEnumerable<TDestinationT> destinationCollection, Func<TSourceT, TDestinationT, bool> isEqualComparison) => CompareCollections(sourceCollection, destinationCollection, isEqualComparison);

        #endregion
    }
    /// <summary>
    /// 表示集合比较结果的差异项（相同类型）。
    /// </summary>
    /// <typeparam name="T">元素类型</typeparam>
    public class DiffResult<T>
    {
        /// <summary>
        /// 差异项的数据
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 差异类型（缺失或多余）
        /// </summary>
        public DiffType Type { get; set; }
    }

    /// <summary>
    /// 表示集合比较结果的差异项（不同类型）。
    /// </summary>
    /// <typeparam name="SourceT">源集合元素类型</typeparam>
    /// <typeparam name="DestinationT">目标集合元素类型</typeparam>
    public class DiffResult<SourceT, DestinationT>
    {
        /// <summary>
        /// 源集合中的数据
        /// </summary>
        public SourceT SourceData { get; set; }

        /// <summary>
        /// 目标集合中的数据
        /// </summary>
        public DestinationT DestinationData { get; set; }

        /// <summary>
        /// 差异类型（缺失或多余）
        /// </summary>
        public DiffType Type { get; set; }
    }

    /// <summary>
    /// 差异类型枚举
    /// </summary>
    public enum DiffType
    {
        /// <summary>
        /// 缺失项：在源集合中存在但在目标集合中不存在
        /// </summary>
        Missing,

        /// <summary>
        /// 多余项：在目标集合中存在但在源集合中不存在
        /// </summary>
        Extra
    }
}