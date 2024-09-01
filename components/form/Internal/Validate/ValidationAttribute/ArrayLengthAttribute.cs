﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace AntDesign.Internal.Form.Validate
{
    internal class ArrayLengthAttribute : ValidationAttribute
    {
        internal int Length { get; }

        internal ArrayLengthAttribute(int length)
        {
            Length = length;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, Length);
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

            return valueAsArray.Length == Length;
        }
    }
}
