using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using OneOf;

namespace AntDesign.Internal
{
    internal class ArrayLengthAttribute : ValidationAttribute
    {
        internal int Length { get; }

        internal ArrayLengthAttribute(int length) : base("The length of field {0} must be {1}")// TODO: localizable
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
