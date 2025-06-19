// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace AntDesign.Internal.Form.Validate;

internal sealed class StringRangeAttribute(int minimum, int maximum) : ValidationAttribute
{
    public int Maximum { get; } = maximum;

    public int Minimum { get; } = minimum;

    public override string FormatErrorMessage(string name)
    {
        return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, Maximum, Minimum);
    }

    public override bool IsValid(object value)
    {
        if (value == null)
        {
            return true;
        }

        if (value is not string stringValue)
        {
            return true;
        }

        return stringValue.Length >= Minimum && stringValue.Length <= Maximum;
    }
}
