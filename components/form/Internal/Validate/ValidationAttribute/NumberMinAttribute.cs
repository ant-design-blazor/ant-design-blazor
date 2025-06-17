// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace AntDesign.Internal.Form.Validate;

internal sealed class NumberMinAttribute : ValidationAttribute
{
    internal decimal Min { get; }

    internal NumberMinAttribute(decimal min)
    {
        Min = min;
    }

    public override string FormatErrorMessage(string name)
    {
        return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, Min);
    }

    public override bool IsValid(object value)
    {
        if (value == null)
        {
            return true;
        }
        var type = value.GetType();
        var typeMaxValue = GetMaxValueString(type);

        var attribute = new RangeAttribute(type, Min.ToString(), typeMaxValue);

        return attribute.IsValid(value);
    }

    private static string GetMaxValueString(Type type)
    {
        if (type == typeof(byte)) return byte.MaxValue.ToString();
        if (type == typeof(short)) return short.MaxValue.ToString();
        if (type == typeof(int)) return int.MaxValue.ToString();
        if (type == typeof(long)) return long.MaxValue.ToString();
        if (type == typeof(float)) return float.MaxValue.ToString();
        if (type == typeof(double)) return double.MaxValue.ToString();
        if (type == typeof(sbyte)) return sbyte.MaxValue.ToString();
        if (type == typeof(ushort)) return ushort.MaxValue.ToString();
        if (type == typeof(uint)) return uint.MaxValue.ToString();
        if (type == typeof(ulong)) return ulong.MaxValue.ToString();
        if (type == typeof(decimal)) return decimal.MaxValue.ToString();

        return null;
    }
}
