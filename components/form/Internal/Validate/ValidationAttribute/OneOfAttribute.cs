// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json;

namespace AntDesign.Internal.Form.Validate;

internal sealed class OneOfAttribute(object[] values, string[] enumOptions = null) : ValidationAttribute
{
    internal object[] Values { get; set; } = values;

    internal string EnumOptions { get; set; } = enumOptions != null ? string.Join(',', enumOptions) : null;

    public override string FormatErrorMessage(string name)
    {
        var options = EnumOptions ?? JsonSerializer.Serialize(Values).Trim('[', ']');
        return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, options);
    }

    public override bool IsValid(object value)
    {
        if (value == null)
        {
            return true;
        }

        if (value is Array)
        {
            return false;
        }

        foreach (var v in Values)
        {
            if (v.Equals(value))
            {
                return true;
            }
        }

        return false;
    }
}
