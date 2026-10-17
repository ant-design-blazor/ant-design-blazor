// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /// <summary>Provides compact position metadata to a child control.</summary>
    public partial class SpaceCompactItem
    {
        [CascadingParameter] private SpaceCompact Parent { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public SpaceCompactItemPosition Position { get; set; }

        private SpaceCompactItemContext ItemContext => new(Parent?.Vertical == true, Parent?.Size, Position);
    }

    public enum SpaceCompactItemPosition
    {
        Only,
        First,
        Middle,
        Last
    }

    internal sealed class SpaceCompactItemContext
    {
        public SpaceCompactItemContext(bool vertical, InputSize? size, SpaceCompactItemPosition position)
        {
            Vertical = vertical;
            Size = size;
            Position = position;
        }

        public bool Vertical { get; }
        public InputSize? Size { get; }
        public SpaceCompactItemPosition Position { get; }

        public string ClassFor(string prefix)
        {
            var separator = Vertical ? "-vertical-" : "-";
            var position = Position switch
            {
                SpaceCompactItemPosition.Only => prefix + "-compact" + separator + "first-item " + prefix + "-compact" + separator + "last-item",
                SpaceCompactItemPosition.First => prefix + "-compact" + separator + "first-item",
                SpaceCompactItemPosition.Last => prefix + "-compact" + separator + "last-item",
                _ => null
            };
            var classes = new[]
            {
                prefix + "-compact" + separator + "item",
                position
            };
            return string.Join(" ", classes.Where(value => !string.IsNullOrWhiteSpace(value)));
        }
    }
}
