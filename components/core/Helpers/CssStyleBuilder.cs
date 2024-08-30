// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text;

namespace AntDesign
{
    public class CssStyleBuilder
    {
        // An example below:
        // "font-size: 10px; color: red; background-color: white; "
        private const string StyleKeyValuePairSplitString = ": ";

        private const string StyleItemSplitString = "; ";

        private readonly StringBuilder _stringBuilder = new();

        public CssStyleBuilder AddStyle(string style)
        {
            if (style.EndsWith(StyleItemSplitString[0]))
            {
                _stringBuilder.Append(style)
                    .Append(StyleItemSplitString[1]);
            }
            else if (style.EndsWith(StyleItemSplitString, System.StringComparison.Ordinal))
            {
                _stringBuilder.Append(style);
            }
            else
            {
                _stringBuilder.Append(style.Trim().TrimEnd(';'))
                    .Append(StyleItemSplitString);
            }
            return this;
        }

        public CssStyleBuilder AddStyle(string styleName, string styleValue)
        {
            if (string.IsNullOrWhiteSpace(styleValue))
                return this;

            _stringBuilder.Append(styleName)
                .Append(StyleKeyValuePairSplitString)
                .Append(styleValue)
                .Append(StyleItemSplitString);

            return this;
        }

        public string Build() => _stringBuilder.ToString();

        public CssStyleBuilder Clear()
        {
            _stringBuilder.Clear();
            return this;
        }
    }
}
