using Silmoon.Runtime;
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
                    string name = memberAssignment.Member.Name;
                    var value = GetValue(memberAssignment.Expression);
                    var type = value != null ? value.GetType() : typeof(object);
                    results.Add(new PropertyValueInfo(name, type, value));
                }
                return results.ToArray();
            }
            else
                throw new NotSupportedException("Only MemberInitExpression is supported, found " + expression.Body.GetType().Name);
        }

        private static object GetValue(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Constant:
                    return ((ConstantExpression)expression).Value;
                case ExpressionType.MemberAccess:
                case ExpressionType.Call:
                case ExpressionType.Convert:
                case ExpressionType.New:
                case ExpressionType.NewArrayInit:
                case ExpressionType.NewArrayBounds:
                case ExpressionType.ArrayIndex:
                case ExpressionType.Conditional:
                    var lambda = Expression.Lambda(expression);
                    var compiled = lambda.Compile();
                    return compiled.DynamicInvoke();
                default:
                    throw new NotSupportedException("Expression type '" + expression.GetType().Name + "' is not supported.");
            }
        }

        public static string GetPreprotyNameExpression<T>(this Expression<Func<T, object>> expression)
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
        public static List<string> GetPreprotyNamesExpressions<T>(params Expression<Func<T, object>>[] expressions) => GetPreprotyNamesExpressions((IEnumerable<Expression<Func<T, object>>>)expressions);
        public static List<string> GetPreprotyNamesExpressions<T>(this IEnumerable<Expression<Func<T, object>>> expressions)
        {
            var fieldNames = new List<string>();

            foreach (var expression in expressions)
            {
                switch (expression.Body)
                {
                    case MemberExpression member:
                        fieldNames.Add(member.Member.Name);
                        break;

                    case UnaryExpression unary when unary.Operand is MemberExpression member:
                        fieldNames.Add(member.Member.Name);
                        break;

                    default:
                        throw new NotSupportedException($"Unsupported expression: {expression.Body.GetType().Name}");
                }
            }

            return fieldNames;
        }
    }
}
