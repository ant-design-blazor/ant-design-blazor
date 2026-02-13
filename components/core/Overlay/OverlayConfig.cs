// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /// <summary>
    /// Configuration for an overlay instance
    /// </summary>
    internal class OverlayConfig
    {
        /// <summary>
        /// Unique identifier
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Content to render
        /// </summary>
        public RenderFragment Content { get; set; }

        /// <summary>
        /// X coordinate (left position in pixels)
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Y coordinate (top position in pixels)
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Container selector (e.g., "body", "#myContainer")
        /// </summary>
        public string Container { get; set; } = "body";

        /// <summary>
        /// Whether this overlay is visible
        /// </summary>
        public bool Visible { get; set; } = true;
    }
}
