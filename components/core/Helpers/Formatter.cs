using System;
using System.Linq.Expressions;
using System.Globalization;

namespace AntDesign.Helpers
{
    internal static class Formatter<T>
    {
        private static readonly Lazy<Func<T, string, string>> _formatFunc =
            new Lazy<Func<T, string, string>>(GetFormatLambda, true);

        public static string Format(T source, string format)
        {
            return _formatFunc.Value.Invoke(source, format);
        }

        private static Func<T, string, string> GetFormatLambda()
        {
            var type = typeof(T);
            var p1 = Expression.Parameter(typeof(T));
            var p2 = Expression.Parameter(typeof(string));

            Expression body = p2;
            if (type.IsSubclassOf(typeof(IFormattable)))
            {
                var method = type.GetMethod("ToString", new[] {typeof(string), typeof(IFormatProvider)});
                body = Expression.Call(Expression.Convert(p1, type), method, p2, Expression.Constant(null));
            }
            else
            {
                var method = type.GetMethod("ToString", new[] {typeof(string)});
                if (method != null)
                {
                    body = Expression.Call(Expression.Convert(p1, type), method, p2);
                }
            }

            return Expression.Lambda<Func<T, string, string>>(body, p1, p2).Compile();
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
