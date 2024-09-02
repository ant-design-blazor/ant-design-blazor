// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /**
    <summary>
    <para>Display statistic number.</para>

    <h2>When To Use</h2>

    <list type="bullet">
        <item>When want to highlight some data.</item>
        <item>When want to display statistic data with description.</item>
    </list>
    </summary>
    <seealso cref="CountDown"/>
    <inheritdoc/>
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.DataDisplay, "https://gw.alipayobjects.com/zos/antfincdn/rcBNhLBrKbE/Statistic.svg")]
    public partial class Statistic<TValue> : StatisticComponentBase<TValue>
    {
        /// <summary>
        /// Decimal separator for number formatting
        /// </summary>
        /// <default value="." />
        [Parameter]
        public string DecimalSeparator { get; set; } = ".";

        /// <summary>
        /// Group separator for number formatting
        /// </summary>
        /// <default value="," />
        [Parameter]
        public string GroupSeparator { get; set; } = ",";

        /// <summary>
        /// Number of decimal places for rounding
        /// </summary>
        /// <default value="0" />
        [Parameter]
        public int Precision { get; set; }

        /// <summary>
        /// Specifies the culture to use for formatting the number.
        /// </summary>
        [Parameter] public CultureInfo CultureInfo { get; set; } = LocaleProvider.CurrentLocale.CurrentCulture;

        protected override void OnInitialized()
        {
            DecimalSeparator ??= CultureInfo.NumberFormat.NumberDecimalSeparator;
            GroupSeparator ??= CultureInfo.NumberFormat.NumberGroupSeparator;

            base.OnInitialized();
        }

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
