// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text;

namespace AntDesign.Core.Extensions;

internal static class StringBuilderExtensions
{
    /// <summary>
    /// Append text if condition is <c>true</c>
    /// </summary>
    /// <param name="source"></param>
    /// <param name="text"></param>
    /// <param name="condition"></param>
    /// <returns></returns>
    public static StringBuilder AppendIf(this StringBuilder source, string text, bool condition)
    {
        if (condition)
        {
            source.Append(text);
        }
        return source;
    }

    public static StringBuilder AppendIfNotNullOrEmpty(this StringBuilder source, string text) => AppendIf(source, text, !string.IsNullOrEmpty(text));

    public static StringBuilder AppendIfNotNullOrWhiteSpace(this StringBuilder source, string text) => AppendIf(source, text, !string.IsNullOrWhiteSpace(text));
}
