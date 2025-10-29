﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace AntDesign.Internal.Form.Validate;

internal sealed class ArrayRangeAttribute(int minLength, int maxLength) : ValidationAttribute
{
    public int MaxLength { get; } = maxLength;

    public int MinLength { get; } = minLength;

    public override string FormatErrorMessage(string name)
    {
        return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, MinLength, MaxLength);
    }

    public override bool IsValid(object value)
    {
        if (value == null)
        {
            return true;
        }

        if (value is not Array valueAsArray)
        {
            return false;
        }

        return valueAsArray.Length >= MinLength && valueAsArray.Length <= MaxLength;
    }
}
