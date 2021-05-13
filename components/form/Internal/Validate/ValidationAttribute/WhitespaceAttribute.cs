using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace AntDesign.Internal
{
    internal class WhitespaceAttribute : ValidationAttribute
    {

        internal WhitespaceAttribute() : base("The field {0} should not be whitespace")// TODO: localizable
        {
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            if (!(value is string valueAsstring))
            {
                return true;
            }

            return valueAsstring != " ";
        }
    }
}
