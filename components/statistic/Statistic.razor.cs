using System;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Statistic<TValue> : StatisticComponentBase<TValue>
    {
        [Parameter] public string DecimalSeparator { get; set; } = ".";

        [Parameter] public string GroupSeparator { get; set; } = ",";

        [Parameter] public int Precision { get; set; }

        private (string integerPart, string fractionalPart) SeparateDecimal()
        {
            decimal decimalValue;
            if (Value is decimal d)
            {
                decimalValue = d;
            }
            else if (Value is string value)
            {
                if (decimal.TryParse(value, out var @decimal))
                {
                    decimalValue = @decimal;
                }
                else
                {
                    return (value ?? "", "");
                }
            }
            else
            {
                decimalValue = Convert.ToDecimal(Value, CultureInfo.InvariantCulture);
            }

            var intValue = (int)decimalValue;

            var intString = intValue == 0 ? (decimalValue >= 0 ? "0" : "-0") : intValue.ToString($"###{GroupSeparator}###", CultureInfo.InvariantCulture);

            decimalValue = Math.Abs(decimalValue - intValue);

            var fractionalPart = "";
            if (decimalValue == 0 && Precision > 0)
            {
                fractionalPart = DecimalSeparator.PadRight(Precision + 1, '0');
            }
            if (decimalValue != 0)
            {
                if (Precision <= 0)
                {
                    fractionalPart = decimalValue.ToString(CultureInfo.InvariantCulture)
                        .Replace("0.", DecimalSeparator, true, CultureInfo.InvariantCulture);
                }
                else
                {
                    decimalValue = Math.Round(decimalValue, Precision);
                    fractionalPart = decimalValue.ToString(CultureInfo.InvariantCulture)
                        .Replace("0.", DecimalSeparator, true, CultureInfo.InvariantCulture)
                        .PadRight(Precision + 1, '0');
                }
            }

            return (intString, fractionalPart);
        }
    }
}
