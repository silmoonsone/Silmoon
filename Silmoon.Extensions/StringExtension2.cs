using System;
using System.ComponentModel.DataAnnotations;

namespace Silmoon.Extensions
{
    public static class StringExtension2
    {
        /// <summary>
        /// 将字符串按枚举成员的 <see cref="DisplayAttribute.Name"/> 解析为枚举值；无特性时与成员名比较（与 <see cref="EnumExtension2.GetDisplayName"/> 一致）。
        /// </summary>
        public static T ToEnumFromDisplayName<T>(this string value, bool ignoreCase = false) where T : Enum => ToEnumFromDisplayNameCore<T>(value, ignoreCase);

        /// <summary>
        /// 将字符串按 <see cref="DisplayAttribute.Name"/> 解析为枚举值；解析失败时根据 <paramref name="throwException"/> 抛出异常或返回默认值。
        /// </summary>
        public static T ToEnumFromDisplayName<T>(this string value, bool throwException, T defaultValue = default, bool ignoreCase = false) where T : Enum
        {
            try
            {
                return ToEnumFromDisplayNameCore<T>(value, ignoreCase);
            }
            catch (Exception)
            {
                if (throwException) throw;
                return defaultValue;
            }
        }

        private static T ToEnumFromDisplayNameCore<T>(string value, bool ignoreCase) where T : Enum
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            var comparison = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            foreach (T enumValue in Enum.GetValues(typeof(T)))
            {
                if (string.Equals(enumValue.GetDisplayName(), value, comparison))
                    return enumValue;
            }
            throw new ArgumentException($"The value '{value}' does not match any [Display(Name)] or member name of enum '{typeof(T).Name}'.", nameof(value));
        }
    }
}
