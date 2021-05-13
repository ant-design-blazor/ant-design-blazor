using System.ComponentModel.DataAnnotations;

namespace AntDesign.Internal
{
    internal class WhitespaceAttribute : ValidationAttribute
    {
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
