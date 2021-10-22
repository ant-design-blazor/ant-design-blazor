// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using OneOf;

namespace AntDesign
{
    public class RangeItemData : IRangeItemData
    {
        public RangeItemData() { }

        public RangeItemData((double first, double second) value)
        {
            Value = value;
        }

        public RangeItemData((double first, double second) value, string description = "") : this(value)
        {
            Description = description;
        }

        public RangeItemData(
            (double first, double second) value,
            string description = "",
            string icon = "") : this(value, description)
        {
            Icon = icon;
        }

        public RangeItemData(
            (double first, double second) value,
            string description = "",
            string icon = "",
            OneOf<Color, string>? fontColor = null) : this(value, description, icon)
        {
            FontColor = fontColor ?? default;
        }

        public RangeItemData(
            (double first, double second) value,
            string description = "",
            string icon = "",
            OneOf<Color, string>? fontColor = null,
            OneOf<Color, string>? color = null) : this(value, description, icon, fontColor)
        {
            Color = color ?? default;
        }

        public RangeItemData(
            (double first, double second) value,
            string description = "",
            string icon = "",
            OneOf<Color, string>? fontColor = null,
            OneOf<Color, string>? color = null,
            OneOf<Color, string>? focusColor = null) : this(value, description, icon, fontColor, color)
        {
            FocusColor = focusColor ?? default;
        }

        public RangeItemData(
            (double first, double second) value,
            string description = "",
            string icon = "",
            OneOf<Color, string>? fontColor = null,
            OneOf<Color, string>? color = null,
            OneOf<Color, string>? focusColor = null,
            OneOf<Color, string>? focusBorderColor = null) : this(value, description, icon, fontColor, color, focusColor)
        {
            FocusBorderColor = focusBorderColor ?? default;
        }

        public (double first, double second) Value { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public OneOf<Color, string> FontColor { get; set; }
        public OneOf<Color, string> Color { get; set; }
        public OneOf<Color, string> FocusColor { get; set; }
        public OneOf<Color, string> FocusBorderColor { get; set; }
        public Action<(double, double)> OnChange { get; set; }
        public Action<(double, double)> OnAfterChange { get; set; }
    }
}
