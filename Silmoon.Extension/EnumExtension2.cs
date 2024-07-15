using Silmoon.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace Silmoon.Extension
{
    public static class EnumExtension2
    {
        public static string GetDisplayName(this Enum @enum)
        {
            var type = @enum.GetType();
            var field = type.GetField(@enum.ToString());
            var obj = (DisplayAttribute)field.GetCustomAttribute(typeof(DisplayAttribute));//得到特性
            return obj?.Name ?? @enum.ToString();
        }

        public static List<NameValue<T>> ToNameValues<T>() where T : Enum
        {
            List<NameValue<T>> results = new List<NameValue<T>>();
            foreach (T @enum in Enum.GetValues(typeof(T)))
            {
                results.Add(new NameValue<T>(@enum.ToString(), @enum));
            }
            return results;
        }
        public static List<NameValue<T>> ToDisplayNameValues<T>() where T : Enum
        {
            List<NameValue<T>> results = new List<NameValue<T>>();
            foreach (T @enum in Enum.GetValues(typeof(T)))
            {
                results.Add(new NameValue<T>(@enum.GetDisplayName(), @enum));
            }
            return results;
        }
        public static NameValue<T> ToNameValue<T>(this T @enum) where T : Enum
        {
            var type = @enum.GetType();
            var field = type.GetField(@enum.ToString());
            var obj = (DisplayAttribute)field.GetCustomAttribute(typeof(DisplayAttribute));//得到特性
            return new NameValue<T>(@enum.ToString(), @enum);
        }
        public static NameValue<T> ToDisplayNameValue<T>(this T @enum) where T : Enum
        {
            var type = @enum.GetType();
            var field = type.GetField(@enum.ToString());
            var obj = (DisplayAttribute)field.GetCustomAttribute(typeof(DisplayAttribute));//得到特性
            return new NameValue<T>(obj?.Name ?? @enum.GetDisplayName(), @enum);
        }
    }
}
