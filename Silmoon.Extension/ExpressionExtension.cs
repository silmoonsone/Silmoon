﻿using Silmoon.Runtime;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Silmoon.Extension
{
    public static class ExpressionExtension
    {
        public static PropertyValueInfo[] GetMemberAssignment<T>(this Expression<Func<T, T>> expression)
        {
            if (expression is null) throw new ArgumentNullException(nameof(expression), "Expression cannot be null.");

            if (expression.Body is MemberInitExpression body)
            {
                List<PropertyValueInfo> results = new List<PropertyValueInfo>();
                foreach (MemberAssignment memberAssignment in body.Bindings)
                {
                    if (memberAssignment.Expression is ConstantExpression constantExpression)
                    {
                        string name = memberAssignment.Member.Name;
                        var value = constantExpression.Value;
                        var type = constantExpression.Type;
                        results.Add(new PropertyValueInfo(name, type, value));
                    }
                    else
                        throw new NotSupportedException($"Only constant expressions are supported, found {memberAssignment.Expression.GetType().Name}");
                }
                return results.ToArray();
            }
            else
                throw new NotSupportedException($"Only MemberInitExpression is supported, found {expression.Body.GetType().Name}");
        }
        public static string GetSelectExpression<T>(this Expression<Func<T, object>> expression)
        {
            switch (expression.Body)
            {
                case MemberExpression member:
                    return member.Member.Name; // 直接属性访问
                case UnaryExpression unary when unary.Operand is MemberExpression member:
                    return member.Member.Name; // 转换后的属性访问（如强制转换）
                default:
                    throw new NotSupportedException($"Unsupported select expression: {expression.Body.GetType().Name}");
            }
        }

    }
}
