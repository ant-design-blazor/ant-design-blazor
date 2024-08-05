// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;

namespace AntDesign.Internal.Form.Validate
{
    internal class ArrayRangeAttribute : ValidationAttribute
    {
        public int MaxLength { get; }

        public int MinLength { get; }

        public ArrayRangeAttribute(int minLength, int maxLength)
        {
            MinLength = minLength;
            MaxLength = maxLength;
        }

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

            if (!(value is Array valueAsArray))
            {
                return false;
            }

            return valueAsArray.Length >= MinLength && valueAsArray.Length <= MaxLength;
        }
    }
}
