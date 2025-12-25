using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Silmoon.Extension
{
    public static class TypeExtension
    {
        /// <summary>
        /// 获取一个类型指定特性的所有属性信息
        /// </summary>
        /// <typeparam name="T">指定的特性</typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static PropertyInfo[] GetProperties<T>(this Type type) where T : Attribute => type.GetProperties().Where(x => x.GetCustomAttribute<T>() != null).ToArray();
        /// <summary>
        /// 获取一个类型指定特性的所有属性的特性
        /// </summary>
        /// <typeparam name="T">指定的特性</typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T[] GetPropertyAttributes<T>(this Type type) where T : Attribute => GetProperties<T>(type).Select(x => x.GetCustomAttribute<T>()).ToArray();

        public static Type[] GetAllBaseTypes(this Type type)
        {
            var baseTypes = new List<Type>();
            var current = type.BaseType;
            // 迭代遍历所有基类
            while (current != null)
            {
                baseTypes.Add(current);
                current = current.BaseType;
            }
            return baseTypes.ToArray();
        }
        public static Type[] GetAllInterfaces(this Type type) => type.GetInterfaces();
    }
}
