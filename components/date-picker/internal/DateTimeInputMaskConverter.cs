// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AntDesign.Core;

namespace AntDesign.Internal;

internal class DateTimeInputMaskConverter : IInputMaskConverter
{
    private static readonly Regex _allowedInput = new("[0-9]");
    private static readonly Regex _maskSymbolsToReplace = new("[a-zA-Z](?<!T)");

    /// <summary>
    /// Convert string value to mask
    /// </summary>
    /// <param name="value"></param>
    /// <param name="mask"></param>
    /// <returns></returns>
    public string Convert(string value, string mask)
    {
        if (string.IsNullOrWhiteSpace(mask))
            return value;

        var allowedSymbols = _allowedInput;
        var keySymbols = _maskSymbolsToReplace;
        var masks = mask.ToArray();
        var inputSymbols = value.Where(x => allowedSymbols.IsMatch(x.ToString())).ToArray();
        var newValue = new StringBuilder(mask.Length);
        var insertedCount = 0;
        for (var maskIndex = 0; maskIndex < masks.Length; maskIndex++)
        {
            var maskItem = masks[maskIndex];
            var isKeySymbol = keySymbols.IsMatch(maskItem.ToString());
            if (insertedCount == inputSymbols.Length)
            {
                if (!isKeySymbol)
                {
                    newValue.Append(maskItem);
                }

                break;
            }

            if (isKeySymbol)
            {
                var symbol = inputSymbols[insertedCount];

                var symbolsToAdd = ConvertSymbol(mask, newValue.ToString(), symbol);

                newValue.Append(symbolsToAdd);

                maskIndex += symbolsToAdd.Length - 1;

                insertedCount++;
                continue;
            }

            newValue.Append(maskItem);
        }

        return newValue.ToString();
    }

    public bool CanInputKey(string key)
    {
        if (key.Length > 1)
        {
            return false;
        }

        return _allowedInput.IsMatch(key);
    }

    public int LastIndexForDeletion(string value)
    {
        for (var index = value.Length - 1; index > 0; index--)
        {
            var symbol = value[index];

            if (_allowedInput.IsMatch(symbol.ToString()))
            {
                return index;
            }
        }

        return 0;
    }

    private static string ConvertSymbol(string mask, string resultStr, char symbol)
    {
        var indexOfMonthPattern = mask.IndexOf("MM", StringComparison.InvariantCulture);
        if (indexOfMonthPattern >= 0 && resultStr.Length == indexOfMonthPattern && symbol >= '2')
        {
            return new string(new[] { '0', symbol });
        }

        var indexOfDayPattern = mask.IndexOf("dd", StringComparison.InvariantCulture);
        if (indexOfDayPattern >= 0)
        {
            if (resultStr.Length == indexOfDayPattern + 1 && symbol > '1' && resultStr[^1] == '3')
            {
                return new string(new[] { '1' });
            }
            if (resultStr.Length == indexOfDayPattern && symbol >= '4')
            {
                return new string(new[] { '0', symbol });
            }
        }

        var indexOfMinutesPattern = mask.IndexOf("mm", StringComparison.InvariantCulture);
        if (indexOfMinutesPattern >= 0 && resultStr.Length == indexOfMinutesPattern && symbol >= '7')
        {
            return new string(new[] { '0', symbol });
        }

        var indexOfSecondsPattern = mask.IndexOf("ss", StringComparison.InvariantCulture);
        if (indexOfSecondsPattern >= 0 && resultStr.Length == indexOfSecondsPattern && symbol >= '7')
        {
            return new string(new[] { '0', symbol });
        }

        var indexOfHoursClockPattern = mask.IndexOf("HH", StringComparison.InvariantCulture);
        if (indexOfHoursClockPattern >= 0 && resultStr.Length == indexOfHoursClockPattern && symbol >= '3')
        {
            return new string(new[] { '0', symbol });
        }

        var indexOfHoursPattern = mask.IndexOf("hh", StringComparison.InvariantCulture);
        if (indexOfHoursPattern >= 0 && resultStr.Length == indexOfHoursPattern && symbol >= '2')
        {
            return new string(new[] { '0', symbol });
        }

        return new string(symbol, 1);
    }
}
