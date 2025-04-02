// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using AntDesign.Core.Reflection;

namespace AntDesign.Core.Helpers;

#if NET5_0 || NET6_0
internal static partial class Formatter<[System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.All)] T>
#else
internal static partial class Formatter<T>
#endif
{
    // For run tests with another culture. Need to refactor this
#pragma warning disable IDE1006 // 命名样式
#if DEBUG
    private static readonly CultureInfo CurrentCulture = CultureInfo.InvariantCulture;
#else
    private static readonly CultureInfo CurrentCulture = CultureInfo.CurrentCulture;
#endif
#pragma warning restore IDE1006 // 命名样式

    private static readonly Lazy<Func<T, string, string>> _formatFunc = new(GetFormatLambda, isThreadSafe: true);

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
        Expression hasValueExpression = sourceType.IsValueType ? Expression.Constant(true) : Expression.NotEqual(sourceProperty, Expression.Default(sourceType));
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

        if (typeof(IFormattable).IsAssignableFrom(sourceType))
        {
            var method = typeof(IFormattable).GetMethod(nameof(ToString), [typeof(string), typeof(IFormatProvider)]);
            body = Expression.Call(Expression.Convert(variable, sourceType), method, parsedFormatString, Expression.Constant(CurrentCulture));
        }
        else
        {
            var method = sourceType.GetMethod(nameof(ToString), [typeof(string)]);
            if (method != null)
            {
                body = Expression.Call(Expression.Convert(variable, sourceType), method, parsedFormatString);
            }
        }

        var condition = Expression.Condition(hasValueExpression, body, Expression.Constant(string.Empty));
        return Expression.Lambda<Func<T, string, string>>(condition, sourceProperty, formatString).Compile();
    }

#if NET7_0_OR_GREATER
    [GeneratedRegex("[^d|^h|^m|^s|^f|^F]+")]
    private static partial Regex ParseSpanTimeFormatStringRegex();
#else
    private static readonly Regex _parseSpanTimeFormatStringRegex = new("[^d|^h|^m|^s|^f|^F]+");
#endif

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
#if NET7_0_OR_GREATER
        return ParseSpanTimeFormatStringRegex().Replace(format, "'$0'");
#else
        return _parseSpanTimeFormatStringRegex.Replace(format, "'$0'");
#endif
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
        return num.ToString("p", CultureInfo.InvariantCulture).Replace(" ", string.Empty, StringComparison.Ordinal);
    }
}
