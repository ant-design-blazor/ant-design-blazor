// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AntDesign;

public class MaskInputConverter
{
    public delegate string SymbolConverter(string mask, string resultString, char symbol);

    private readonly Regex _allowedInput;
    private readonly Regex _maskSymbolsToReplace;
    private readonly SymbolConverter _symbolConverter;

    public MaskInputConverter(Regex allowedInput, Regex maskSymbolsToReplace, SymbolConverter symbolConverter = null)
    {
        _allowedInput = allowedInput;
        _maskSymbolsToReplace = maskSymbolsToReplace;
        _symbolConverter = symbolConverter;
    }

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
        var chars = value.Where(x => allowedSymbols.IsMatch(x.ToString())).ToArray();
        var newValue = new StringBuilder();
        var insertedCount = 0;
        for (var maskIndex = 0; maskIndex < masks.Length; maskIndex++)
        {
            var maskItem = masks[maskIndex];
            var isKeySymbol = keySymbols.IsMatch(maskItem.ToString());
            if (insertedCount == chars.Length)
            {
                if (!isKeySymbol)
                {
                    newValue.Append(maskItem);
                }

                break;
            }

            if (isKeySymbol)
            {
                var symbol = chars[insertedCount];

                if (_symbolConverter is not null)
                {
                    var symbolsToAdd = _symbolConverter.Invoke(mask, newValue.ToString(), symbol);

                    newValue.Append(symbolsToAdd);

                    maskIndex += symbolsToAdd.Length - 1;
                }
                else
                {
                    newValue.Append(symbol);
                }

                insertedCount++;
                continue;
            }

            newValue.Append(maskItem);
        }

        return newValue.ToString();
    }
}
