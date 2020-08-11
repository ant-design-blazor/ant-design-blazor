using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using AntDesign.Core.Reflection;

namespace AntDesign.Helpers
{
    internal static class Formatter<T>
    {
        private static readonly Lazy<Func<T, string, string>> _formatFunc = new Lazy<Func<T, string, string>>(GetFormatLambda, true);

        public static string Format(T source, string format)
        {
            return _formatFunc.Value.Invoke(source, format);
        }

        private static Func<T, string, string> GetFormatLambda()
        {
            var type = typeof(T);
            var p1 = Expression.Parameter(typeof(T));
            var p2 = Expression.Parameter(typeof(string));

            Expression variable = p1;
            Expression body = p2;
            Expression hasValueExpression = type.IsValueType ? (Expression)Expression.Constant(true) : Expression.NotEqual(p1, Expression.Default(type));

            if (TypeDefined<T>.IsNullable)
            {
                type = TypeDefined<T>.NullableType;
                hasValueExpression = Expression.Equal(Expression.Property(p1, "HasValue"), Expression.Constant(true));
                variable = Expression.Condition(hasValueExpression, Expression.Property(p1, "Value"), Expression.Default(type));
            }

            if (type.IsSubclassOf(typeof(IFormattable)))
            {
                var method = type.GetMethod("ToString", new[] { typeof(string), typeof(IFormatProvider) });
                body = Expression.Call(Expression.Convert(variable, type), method, p2, Expression.Constant(null));
            }
            else
            {
                var method = type.GetMethod("ToString", new[] { typeof(string) });
                if (method != null)
                {
                    body = Expression.Call(Expression.Convert(variable, type), method, p2);
                }
            }

            var condition = Expression.Condition(hasValueExpression, body, Expression.Constant(string.Empty));
            return Expression.Lambda<Func<T, string, string>>(condition, p1, p2).Compile();
        }
    }

    internal static class Formatter
    {
        /// <summary>
        /// under WASM mode, when format a double number to percentage, there will be a blank between number and %, '35.00 %'
        /// use this method instead to avoid the blank space
        /// </summary>
        /// <returns></returns>
        public static string ToPercentWithoutBlank(double num)
        {
            return num.ToString("p", CultureInfo.CurrentCulture).Replace(" ", "", StringComparison.Ordinal);
        }
    }
}
