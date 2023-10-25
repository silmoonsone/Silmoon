using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silmoon.Extension
{
    public static class EnumExtension
    {
        public static T AddFlag<T>(this T value, T flag) where T : Enum => (T)Enum.ToObject(typeof(T), Convert.ToUInt64(value) | Convert.ToUInt64(flag));

        public static T RemoveFlag<T>(this T value, T flag) where T : Enum => (T)Enum.ToObject(typeof(T), Convert.ToUInt64(value) & ~Convert.ToUInt64(flag));
        public static T Parse<T>(string value) where T : Enum => (T)Enum.Parse(typeof(T), value);



        public static T[] GetFlagEnumArray<T>(this T value, bool IncludeZeroEnum = false) where T : Enum
        {
            if (!typeof(T).IsDefined(typeof(FlagsAttribute), false)) throw new ArgumentException("The generic type parameter must be a flagged enum.");

            List<T> flagEnums = new List<T>();

            foreach (T flag in Enum.GetValues(typeof(T)))
            {
                if (!IncludeZeroEnum && EqualityComparer<T>.Default.Equals(flag, default)) continue; // 如果是默认值（通常为None），跳过
                //if (value.HasFlag(flag))
                if ((Convert.ToUInt64(value) & Convert.ToUInt64(flag)) == Convert.ToUInt64(flag))
                    flagEnums.Add(flag);
            }

            return flagEnums.ToArray();
        }
        public static string[] ToFlagStringArray<T>(this T value, bool IncludeZeroEnum = false) where T : Enum => GetFlagEnumArray(value, IncludeZeroEnum).Select(e => e.ToString()).ToArray();
        public static string[] ToStringArray<T>(this T[] enums) where T : Enum => enums.Select(e => e.ToString()).ToArray();
    }
}
