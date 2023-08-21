// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Text.RegularExpressions;

namespace AntDesign;

public static class MaskInputConverterFactory
{
    public static MaskInputConverter CreateForDateTime()
    {
        return new(new Regex("[0-9]"), new Regex("[a-zA-Z](?<!T)"), MaskInputSymbolConverter);
    }

    private static string MaskInputSymbolConverter(string mask, string resultStr, char symbol)
    {
        var indexOfMonthPattern = mask.IndexOf("MM", StringComparison.InvariantCulture);
        if (indexOfMonthPattern >= 0 && resultStr.Length == indexOfMonthPattern && symbol >= '2')
        {
            return new string(new []{ '0', symbol });
        }
            
        var indexOfDayPattern = mask.IndexOf("dd", StringComparison.InvariantCulture);
        if (indexOfDayPattern >= 0 && resultStr.Length == indexOfDayPattern && symbol >= '4')
        {
            return new string(new []{ '0', symbol });
        }

        var indexOfMinutesPattern = mask.IndexOf("mm", StringComparison.InvariantCulture);
        if (indexOfMinutesPattern >= 0 && resultStr.Length == indexOfMinutesPattern && symbol >= '7')
        {
            return new string(new []{ '0', symbol });
        }
        
        var indexOfSecondsPattern = mask.IndexOf("ss", StringComparison.InvariantCulture);
        if (indexOfSecondsPattern >= 0 && resultStr.Length == indexOfSecondsPattern && symbol >= '7')
        {
            return new string(new []{ '0', symbol });
        }
        
        var indexOfHoursClockPattern = mask.IndexOf("HH", StringComparison.InvariantCulture);
        if (indexOfHoursClockPattern >= 0 && resultStr.Length == indexOfHoursClockPattern && symbol >= '3')
        {
            return new string(new []{ '0', symbol });
        }
        
        var indexOfHoursPattern = mask.IndexOf("hh", StringComparison.InvariantCulture);
        if (indexOfHoursPattern >= 0 && resultStr.Length == indexOfHoursPattern && symbol >= '2')
        {
            return new string(new []{ '0', symbol });
        }
            
        return new string(symbol, 1);
    }
}
