using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Silmoon.Runtime.Extensions
{
    public static class TypeExtension
    {
        public static object? Invoke(this Type type, object instance, MethodExecuteInfo methodExecuteInfo) => type.GetMethod(methodExecuteInfo.Name)?.Invoke(instance, methodExecuteInfo.Parameters);
        /// <summary>
        /// 获取一个类型指定特性的所有属性信息
        /// </summary>
        /// <typeparam name="T">指定的特性</typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static PropertyInfo[] GetProperties<T>(this Type type) where T : Attribute => [.. type.GetProperties().Where(x => x.GetCustomAttribute<T>() is not null)];
        /// <summary>
        /// 获取一个类型指定特性的所有属性的特性
        /// </summary>
        /// <typeparam name="T">指定的特性</typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T[] GetPropertyAttributes<T>(this Type type) where T : Attribute
        {
            var properties = GetProperties<T>(type);
            return [.. properties.Select(x => x.GetCustomAttribute<T>())];
        }
    }
}
