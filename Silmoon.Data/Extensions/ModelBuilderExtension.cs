#if NET7_0_OR_GREATER
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
#endif
using Newtonsoft.Json;
using Silmoon.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silmoon.Data.Extensions
{
    public static class ModelBuilderExtension
    {
#if NET7_0_OR_GREATER
        // 为 Dictionary<TKey, TValue> 提供的方法
        public static void UseJsonConversionWithComparer<TKey, TValue>(this PropertyBuilder<Dictionary<TKey, TValue>> builder)
        {
            builder.HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<Dictionary<TKey, TValue>>(v));

            builder.Metadata.SetValueComparer(new ValueComparer<Dictionary<TKey, TValue>>(
                (c1, c2) => c1.Count == c2.Count && !c1.Except(c2).Any(),
                c => c.Aggregate(0, (a, pair) => HashCode.Combine(a, pair.Key.GetHashCode(), pair.Value.GetHashCode())),
                c => c.ToDictionary(entry => entry.Key, entry => entry.Value)
            ));
        }

        // 为 List<T> 提供的方法
        public static void UseJsonConversionWithComparer<T>(this PropertyBuilder<List<T>> builder)
        {
            builder.HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<List<T>>(v));

            builder.Metadata.SetValueComparer(new ValueComparer<List<T>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()
            ));
        }

        // 为 HashSet<T> 提供的方法
        public static void UseJsonConversionWithComparer<T>(this PropertyBuilder<HashSet<T>> builder)
        {
            builder.HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<HashSet<T>>(v));

            builder.Metadata.SetValueComparer(new ValueComparer<HashSet<T>>(
                (c1, c2) => c1.SetEquals(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => new HashSet<T>(c)
            ));
        }

        // 为 Enum 提供的方法
        public static void UseJsonConversionWithComparer<T>(this PropertyBuilder<T> builder) where T : struct, Enum
        {
            builder.HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<T>(v));

            builder.Metadata.SetValueComparer(new ValueComparer<T>(
                (c1, c2) => c1.Equals(c2),
                c => c.GetHashCode(),
                c => Enum.Parse<T>(c.ToString())
            ));
        }
        public static void UseJsonConversion<T>(this PropertyBuilder<T> builder)
        {
            builder.HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<T>(v));
        }
#endif
    }
}
