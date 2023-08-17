// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AntDesign;

public class MaskInputConverter
{
    private readonly Regex _allowedInput;
    private readonly Regex _maskSymbolsToReplace;

    public MaskInputConverter(Regex allowedInput, Regex maskSymbolsToReplace)
    {
        _allowedInput = allowedInput;
        _maskSymbolsToReplace = maskSymbolsToReplace;
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
        foreach (var maskItem in masks)
        {
            var isKeySymbol = keySymbols.IsMatch(maskItem.ToString()) && maskItem != 'T';
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
                newValue.Append(chars[insertedCount]);
                insertedCount++;

                continue;
            }

            newValue.Append(maskItem);
        }

        return newValue.ToString();
    }
}
