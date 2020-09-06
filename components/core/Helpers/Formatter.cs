using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
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
            var sourceType = typeof(T);
            var sourceProperty = Expression.Parameter(typeof(T));
            var formatString = Expression.Parameter(typeof(string));

            Expression variable = sourceProperty;
            Expression body = Expression.Call(sourceProperty, typeof(object).GetMethod(nameof(ToString)));
            Expression hasValueExpression = sourceType.IsValueType ? (Expression)Expression.Constant(true) : Expression.NotEqual(sourceProperty, Expression.Default(sourceType));
            Expression parsedFormatString = formatString;

            if (sourceType == typeof(TimeSpan))
            {
                parsedFormatString = Expression.Call(typeof(Formatter<TimeSpan>).GetMethod(nameof(ParseSpanTimeFormatString), BindingFlags.NonPublic | BindingFlags.Static), formatString);
            }

            if (TypeDefined<T>.IsNullable)
            {
                sourceType = TypeDefined<T>.NullableType;
                hasValueExpression = Expression.Equal(Expression.Property(sourceProperty, "HasValue"), Expression.Constant(true));
                variable = Expression.Condition(hasValueExpression, Expression.Property(sourceProperty, "Value"), Expression.Default(sourceType));
            }

            if (sourceType.IsSubclassOf(typeof(IFormattable)))
            {
                var method = sourceType.GetMethod(nameof(ToString), new[] { typeof(string), typeof(IFormatProvider) });
                body = Expression.Call(Expression.Convert(variable, sourceType), method, parsedFormatString, Expression.Constant(null));
            }
            else
            {
                var method = sourceType.GetMethod(nameof(ToString), new[] { typeof(string) });
                if (method != null)
                {
                    body = Expression.Call(Expression.Convert(variable, sourceType), method, parsedFormatString);
                }
            }

            var condition = Expression.Condition(hasValueExpression, body, Expression.Constant(string.Empty));
            return Expression.Lambda<Func<T, string, string>>(condition, sourceProperty, formatString).Compile();
        }

        /// <summary>
        /// parse other characters in format string.
        /// </summary>
        /// <remarks>refer to https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-timespan-format-strings#other-characters</remarks>
        /// <param name="format"></param>
        /// <returns></returns>
        private static string ParseSpanTimeFormatString(string format)
        {
            if (string.IsNullOrWhiteSpace(format))
            {
                return format;
            }
            return Regex.Replace(format, "[^d|^h|^m|^s|^f|^F]+", "'$0'");
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
            return num.ToString("p", CultureInfo.InvariantCulture).Replace(" ", "", StringComparison.Ordinal);
        }
    }
}
