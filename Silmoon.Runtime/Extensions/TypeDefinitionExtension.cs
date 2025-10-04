using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Silmoon.Runtime.Extensions
{
    public static class TypeDefinitionExtension
    {
        /// <summary>
        /// 获取所有接口，包括基类实现的接口
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<InterfaceImplementation> GetAllInterfaces(this TypeDefinition type)
        {
            // 使用 HashSet 记录接口名称，确保接口的唯一性
            var interfaces = new List<InterfaceImplementation>();
            var interfaceNames = new HashSet<string>();

            // 添加当前类型的接口
            if (type.HasInterfaces)
            {
                foreach (var iface in type.Interfaces)
                {
                    if (interfaceNames.Add(iface.InterfaceType.FullName)) // 只有当名称未被添加时才加入
                    {
                        interfaces.Add(iface);
                    }
                }
            }

            // 获取所有基类并检查它们的接口
            var baseTypes = type.GetAllBaseTypes();
            foreach (var baseType in baseTypes)
            {
                if (baseType.HasInterfaces)
                {
                    foreach (var baseInterface in baseType.Interfaces)
                    {
                        if (interfaceNames.Add(baseInterface.InterfaceType.FullName)) // 避免重复
                        {
                            interfaces.Add(baseInterface);
                        }
                    }
                }
            }

            return interfaces;
        }

        /// <summary>
        /// 获取所有基类，迭代方式避免深度递归
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<TypeDefinition> GetAllBaseTypes(this TypeDefinition type)
        {
            var baseTypes = new List<TypeDefinition>();
            var current = type.BaseType;

            // 迭代遍历所有基类
            while (current != null)
            {
                var resolvedBaseType = current.Resolve();
                if (resolvedBaseType != null)
                {
                    baseTypes.Add(resolvedBaseType);
                    current = resolvedBaseType.BaseType;
                }
                else
                {
                    break; // 如果无法解析，停止
                }
            }

            return baseTypes;
        }
    }
}