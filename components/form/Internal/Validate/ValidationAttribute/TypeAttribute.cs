// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;

namespace AntDesign.Internal.Form.Validate;

internal sealed class TypeAttribute(FormFieldType type) : ValidationAttribute
{
    internal FormFieldType Type { get; set; } = type;

    public override string FormatErrorMessage(string name)
    {
        return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, Type.ToString());
    }

    public override bool IsValid(object value)
    {
        if (value is null)
        {
            return true;
        }

        return Type switch
        {
            FormFieldType.String => value is string,
            FormFieldType.Number => IsNumber(value),
            FormFieldType.Integer => IsInteger(value),
            FormFieldType.Float => IsFloat(value),
            FormFieldType.Boolean => value is bool,
            FormFieldType.Regexp => IsRegexp(value),
            FormFieldType.Array => value is Array,
            FormFieldType.Object => value is not null,
            FormFieldType.Date => value is DateTime,
            FormFieldType.Url => new UrlAttribute().IsValid(value),
            FormFieldType.Email => new EmailAddressAttribute().IsValid(value),
            _ => false,
        };
    }

    private static bool IsRegexp(object value)
    {
        if (value is not string valueAsStrng)
        {
            return false;
        }

        try
        {
            _ = Regex.IsMatch("Test Regex pattern", valueAsStrng); // Mark sure the pattern is valid
        }
        catch
        {
            return false;
        }
        return true;
    }

    private static bool IsNumber(object value)
    {
        ReadOnlySpan<Type> numberTypes = [
            typeof(byte),
            typeof(short),
            typeof(int),
            typeof(long),
            typeof(float),
            typeof(double),
            typeof(sbyte),
            typeof(ushort),
            typeof(uint),
            typeof(ulong),
            typeof(decimal),
        ];
        return OneOf(value.GetType(), numberTypes);
    }

    private static bool IsInteger(object value)
    {
        ReadOnlySpan<Type> integerTypes = [
            typeof(byte),
            typeof(short),
            typeof(int),
            typeof(long),
            typeof(sbyte),
            typeof(ushort),
            typeof(uint),
            typeof(ulong),
        ];
        return OneOf(value.GetType(), integerTypes);
    }

    private static bool IsFloat(object value) => OneOf(value.GetType(), [typeof(float), typeof(double), typeof(decimal)]);

    private static bool OneOf(Type t, ReadOnlySpan<Type> types)
    {
        for (var i = 0; i < types.Length; i++)
        {
            if (types[i] == t)
            {
                return true;
            }
        }
        return false;
    }
}
