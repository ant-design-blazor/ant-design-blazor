// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /// <summary>
    /// Joins adjacent controls into a compact group.
    /// Each child should be wrapped in <see cref="SpaceCompactItem"/> so its first/middle/last
    /// position can be represented without adding an extra DOM element.
    /// </summary>
    public partial class SpaceCompact : AntDomComponentBase
    {
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public InputSize? Size { get; set; }
        [Parameter] public bool Block { get; set; }
        [Parameter] public bool Vertical { get; set; }

        private string CompactClass => string.Join(" ", new[]
        {
            "ant-space-compact",
            Block ? "ant-space-compact-block" : null,
            Vertical ? "ant-space-compact-vertical" : null,
            RTL ? "ant-space-compact-rtl" : null,
            Class
        }.Where(value => !string.IsNullOrWhiteSpace(value)));

    }
}
