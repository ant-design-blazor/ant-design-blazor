// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace AntDesign.Docs.Utils
{
    public static class StringExtensions
    {
        public static string TrimWhiteSpace(this string str)
        {
            if (str == null)
            {
                return null;
            }
            char[] whiteSpace = { '\r', '\n', '\f', '\t', '\v' };
            return str.Trim(whiteSpace).Trim();
        }

        public static string FixLineBreakForWeb(this string str)
        {
            return str.Replace(Environment.NewLine, "<br/>");
        }

        public static string FixTabsForWeb(this string str)
        {
            return str.Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;");
        }

        public static string FixSpaceForWeb(this string str)
        {
            return str.Replace(" ", "&nbsp;");
        }
    }
}
