using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using OneOf;

namespace AntDesign.Internal
{
    internal class NumberAttribute : ValidationAttribute
    {
        internal decimal Number { get; }

        internal NumberAttribute(decimal number) : base("The field {0} must be {1}")// TODO: localizable
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

            if (!(value is decimal valueAsDecimal))
            {
                return false;
            }

            return Number == valueAsDecimal;
        }
    }
}
