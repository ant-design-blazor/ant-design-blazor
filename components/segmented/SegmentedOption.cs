// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign
{
    public record struct SegmentedOption<TValue>(TValue Value, string Label = null, bool Disabled = false)
    {
        public string Label { get; set; } = Label ?? Value.ToString();
    }
}
