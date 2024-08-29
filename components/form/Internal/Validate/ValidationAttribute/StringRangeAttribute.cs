// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace AntDesign.Internal.Form.Validate
{
    internal class StringRangeAttribute : ValidationAttribute
    {
        public int Maximum { get; }

        public int Minimum { get; }

        public StringRangeAttribute(int minimum, int maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

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
}
