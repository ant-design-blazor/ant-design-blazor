using System;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Statistic<TValue> : StatisticComponentBase<TValue>
    {
        [Parameter] public string DecimalSeparator { get; set; } = ".";

        [Parameter] public string GroupSeparator { get; set; } = ",";

        [Parameter] public int Precision { get; set; } = -1;

        private string IntegerPart { get { return Convert.ToDecimal(Value).ToString($"###{GroupSeparator}###"); } }

        private string FractionalPart
        {
            get
            {
                string tem;
                if (Precision > 0)
                    tem = Math.Round(Convert.ToDecimal(Value), Precision).ToString().Contains('.')
                        ? Math.Round(Convert.ToDecimal(Value), Precision).ToString().Split('.').Last().PadRight(Precision, '0')
                        : string.Empty.PadRight(Precision, '0');
                else if (Precision < 0)
                    tem = Value.ToString().Contains('.') ? Value.ToString().Split('.').Last() : null;
                else
                    tem = null;
                return string.IsNullOrEmpty(tem) ? null : DecimalSeparator + tem;
            }
        }
    }
}
