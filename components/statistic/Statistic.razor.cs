using System;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Statistic : StatisticComponentBase<decimal>
    {
        /// <summary>
        /// 设置小数点   string
        /// </summary>
        [Parameter] public string DecimalSeparator { get; set; } = ".";

        //Formatter 自定义数值展示(value) => ReactNode -

        /// <summary>
        /// 设置千分位标识符 string	,	
        /// </summary>
        [Parameter] public string GroupSeparator { get; set; } = ",";

        /// <summary>
        /// 数值精度,默认不取舍，当值为0或正整数时会取舍小数位，位数不足右边补0
        /// </summary>
        [Parameter] public int Precision { get; set; } = -1;

        private string IntegerPart { get {return Convert.ToDecimal(Value).ToString($"###{GroupSeparator}###"); } }

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
