// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AntDesign.core.Helpers;

public static class MaskHelper
{
    /// <summary>
    /// Fill symbols to mask (at now only for dates)
    /// </summary>
    /// <param name="value"></param>
    /// <param name="mask"></param>
    /// <returns></returns>
    public static string Fill(string value, string mask)
    {
        if (string.IsNullOrWhiteSpace(mask))
            return value;

        var allowedSymbols = new Regex("[0-9]");
        var keySymbols = new Regex("[a-zA-Z]");
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
