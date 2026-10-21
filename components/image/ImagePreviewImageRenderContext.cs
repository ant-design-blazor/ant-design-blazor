// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /// <summary>
    /// Image and transform state supplied to a custom Image preview renderer.
    /// </summary>
    public sealed class ImagePreviewImageRenderContext
    {
        internal ImagePreviewImageRenderContext(
            RenderFragment originalNode,
            double scale,
            int rotation,
            bool flipX,
            bool flipY,
            string url,
            string alt,
            string width,
            string height)
        {
            OriginalNode = originalNode;
            Scale = scale;
            Rotation = rotation;
            FlipX = flipX;
            FlipY = flipY;
            Url = url;
            Alt = alt;
            Width = width;
            Height = height;
        }

        /// <summary>The default preview image element.</summary>
        public RenderFragment OriginalNode { get; }

        /// <summary>Current zoom scale.</summary>
        public double Scale { get; }

        /// <summary>Current rotation angle in degrees.</summary>
        public int Rotation { get; }

        /// <summary>Whether the image is flipped horizontally.</summary>
        public bool FlipX { get; }

        /// <summary>Whether the image is flipped vertically.</summary>
        public bool FlipY { get; }

        /// <summary>Preview image URL.</summary>
        public string Url { get; }

        /// <summary>Alternative text of the source image.</summary>
        public string Alt { get; }

        /// <summary>Declared source image width.</summary>
        public string Width { get; }

        /// <summary>Declared source image height.</summary>
        public string Height { get; }
    }
}
