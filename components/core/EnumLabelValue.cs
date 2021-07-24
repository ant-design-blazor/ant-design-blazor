// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign
{
    public class EnumLabelValue<TEnum>
    {
        /// <summary>
        /// name of the enum
        /// </summary>
        public string Label { get; set; }

        public TEnum Value { get; set; }

        public EnumLabelValue(string label, TEnum value)
        {
            Label = label;
            Value = value;
        }
    }
}
