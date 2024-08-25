// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace AntDesign.Internal.Form.Validate
{
    internal class NumberMaxAttribute : ValidationAttribute
    {
        internal decimal Max { get; }

        internal NumberMaxAttribute(decimal max)
        {
            Max = max;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, Max);
        }

        public override bool IsValid(object value)
        {
            var type = value.GetType();
            var typeMinValue = GetMinValueString(type);

            var attribute = new RangeAttribute(type, typeMinValue, Max.ToString());

            return attribute.IsValid(value);
        }

        private static string GetMinValueString(Type type)
        {
            if (type == typeof(byte)) return byte.MinValue.ToString();
            if (type == typeof(short)) return short.MinValue.ToString();
            if (type == typeof(int)) return int.MinValue.ToString();
            if (type == typeof(long)) return long.MinValue.ToString();
            if (type == typeof(float)) return float.MinValue.ToString();
            if (type == typeof(double)) return double.MinValue.ToString();
            if (type == typeof(sbyte)) return sbyte.MinValue.ToString();
            if (type == typeof(ushort)) return ushort.MinValue.ToString();
            if (type == typeof(uint)) return uint.MinValue.ToString();
            if (type == typeof(ulong)) return ulong.MinValue.ToString();
            if (type == typeof(decimal)) return decimal.MinValue.ToString();

            return null;
        }
    }
}
