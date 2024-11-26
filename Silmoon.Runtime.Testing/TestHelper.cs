using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silmoon.Runtime.Testing
{
    public static class TestHelper
    {
        public static HashSet<string> GetUsedTypes(TypeDefinition type)
        {
            var usedTypes = new HashSet<string>();

            // 获取所有字段使用的类型
            foreach (var field in type.Fields)
            {
                usedTypes.Add(field.FieldType.FullName);
            }

            // 获取所有属性使用的类型
            foreach (var property in type.Properties)
            {
                usedTypes.Add(property.PropertyType.FullName);
            }

            // 获取所有方法和它们的参数、返回值、局部变量使用的类型
            foreach (var method in type.Methods)
            {
                // 方法返回类型
                usedTypes.Add(method.ReturnType.FullName);

                // 方法参数类型
                foreach (var parameter in method.Parameters)
                {
                    usedTypes.Add(parameter.ParameterType.FullName);
                }

                // 方法内局部变量类型
                if (method.HasBody)
                {
                    foreach (var variable in method.Body.Variables)
                    {
                        usedTypes.Add(variable.VariableType.FullName);
                    }
                }

                // 解析 IL 指令，获取可能使用到的类型
                foreach (var instruction in method.Body.Instructions)
                {
                    if (instruction.Operand is TypeReference typeReference)
                    {
                        usedTypes.Add(typeReference.FullName);
                    }
                }
            }

            return usedTypes;
        }
    }
}
