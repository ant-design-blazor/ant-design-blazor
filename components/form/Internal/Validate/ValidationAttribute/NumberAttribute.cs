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

            if (!(value is decimal valueAsDecimal))
            {
                return false;
            }

            return Number == valueAsDecimal;
        }
    }
}
