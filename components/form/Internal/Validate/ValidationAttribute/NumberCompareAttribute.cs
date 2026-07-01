// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace AntDesign.Internal.Form.Validate;

/// <summary>
/// <see cref="ValidationAttribute"/> for number comparisons.
/// </summary>
/// <param name="number">the target number to be compared</param>
/// <param name="comparison">
/// <list type="table">
/// <item>
/// <term>-1</term>
/// <description>the input value should be less than or equal to the target number(MAX).</description>
/// </item>
/// <item>
/// <term>1</term>
/// <description>the input value should be greater than or equal to the target number(Min).</description>
/// </item>
/// <item>
/// <term>others</term>
/// <description>the input value should be equal to the target number.</description>
/// </item>
/// </list>
/// </param>
internal abstract class NumberCompareAttribute(decimal number, int comparison) : ValidationAttribute
{
    private static readonly Dictionary<Type, Func<object, decimal>> _defaultDecimalConverters = new()
    {
        { typeof(decimal), static value => (decimal)value },
        { typeof(double), static value => (decimal)(double)value },
        { typeof(float), static value => (decimal)(float)value },
        { typeof(int), static value => (int)value },
        { typeof(long), static value => (long)value },
        { typeof(short), static value => (short)value },
        { typeof(sbyte), static value => (sbyte)value },
        { typeof(ulong), static value => (ulong)value },
        { typeof(uint), static value => (uint)value },
        { typeof(ushort), static value => (ushort)value },
        { typeof(byte), static value => (byte)value },
        { typeof(decimal?), static value => ((decimal?)value).Value },
        { typeof(double?), static value => (decimal)((double?)value).Value },
        { typeof(float?), static value => (decimal)((float?)value).Value },
        { typeof(int?), static value => ((int?)value).Value },
        { typeof(long?), static value => ((long?)value).Value },
        { typeof(short?), static value => ((short?)value).Value },
        { typeof(sbyte?), static value => ((sbyte?)value).Value },
        { typeof(ulong?), static value => ((ulong?)value).Value },
        { typeof(uint?), static value => ((uint?)value).Value },
        { typeof(ushort?), static value => ((ushort?)value).Value },
        { typeof(byte?), static value => ((byte?)value).Value }
    };

    internal decimal Number { get; } = number;

    public override string FormatErrorMessage(string name)
    {
        return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, Number);
    }

    public override bool IsValid(object value)
    {
        if (value is null)
        {
            return true;
        }

        var valueType = value.GetType();
        if (_defaultDecimalConverters.TryGetValue(valueType, out var converter))
        {
            var convertedValue = converter.Invoke(value);
            return IsValidValue(convertedValue, comparison);
        }

        if (valueType == typeof(string))
        {
            return decimal.TryParse((string)value, out var parsedValue)
                && IsValidValue(parsedValue, comparison);
        }

        return false;
    }

    private bool IsValidValue(decimal val, int comparison) => comparison switch
    {
        -1 => val <= Number,
        1 => val >= Number,
        _ => val == Number
    };
}
