// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Text;

namespace AntDesign
{
    public class CssStyleBuilder
    {
        private readonly List<string> _styles = new();

        public void AddStyle(string style)
        {
            _styles.Add(style.Trim().TrimEnd(';'));
        }

        public void AddStyle(string styleName, string styleValue)
        {
            _styles.Add($"{styleName}: {styleValue}");
        }

        public string Build()
        {
            StringBuilder totalStyle = new StringBuilder();
            foreach (string style in _styles)
            {
                totalStyle.Append(style).Append("; ");
            }

            return totalStyle.ToString();
        }
    }
}
