// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace AntDesign.Internal.Form.Validate
{
    internal class NumberAttribute : ValidationAttribute
    {
        internal decimal Number { get; }

        internal NumberAttribute(decimal number)
        {
            Number = number;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, Number);
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            return value switch
            {
                decimal valueAsDecimal => Number == valueAsDecimal,
                double valueAsDouble => Number == (decimal)valueAsDouble,
                float valueAsFloat => Number == (decimal)valueAsFloat,
                int valueAsInt => Number == (decimal)valueAsInt,
                long valueAsLong => Number == (decimal)valueAsLong,
                _ => false,
            };
        }
    }
}
