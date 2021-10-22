// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public class RangeItemMark
    {
        public double Key { get; }
        public RenderFragment Value { get; }
        public string Style { get; }

        public RangeItemMark(double key, string value, string style = "")
        {
            Key = key;
            Value = (b) => b.AddContent(0, value);
            Style = style;
        }

        public RangeItemMark(double key, RenderFragment value, string style)
        {
            Key = key;
            Value = value;
            Style = style;
        }
    }
}
