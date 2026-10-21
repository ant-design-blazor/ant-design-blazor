// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    /// <summary>
    /// Data for a Timeline node when using <see cref="Timeline.Items"/>.
    /// </summary>
    public sealed class TimelineItemData
    {
        /// <summary>The node title.</summary>
        public string Title { get; set; }

        /// <summary>Optional rich title content, which takes precedence over <see cref="Title"/>.</summary>
        public RenderFragment TitleContent { get; set; }

        /// <summary>The node description.</summary>
        public string Content { get; set; }

        /// <summary>Optional rich node content, which takes precedence over <see cref="Content"/>.</summary>
        public RenderFragment ContentContent { get; set; }

        /// <summary>Named or custom node color.</summary>
        public OneOf<TimelineDotColor, string>? Color { get; set; }

        /// <summary>Custom node icon.</summary>
        public RenderFragment Icon { get; set; }

        /// <summary>Shows a loading icon when no custom icon is supplied.</summary>
        public bool Loading { get; set; }

        /// <summary>Overrides the timeline mode for this node.</summary>
        public TimelineMode? Placement { get; set; }

        /// <summary>Additional CSS class for this node.</summary>
        public string Class { get; set; }

        /// <summary>Inline style for this node.</summary>
        public string Style { get; set; }
    }
}
