// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /// <summary>
    /// State and actions supplied to a custom Image preview toolbar.
    /// </summary>
    public sealed class ImagePreviewActionsRenderContext
    {
        internal ImagePreviewActionsRenderContext(
            RenderFragment originalNode,
            double scale,
            int rotation,
            bool flipX,
            bool flipY,
            int current,
            int total,
            string imageUrl,
            EventCallback onFlipY,
            EventCallback onFlipX,
            EventCallback onRotateLeft,
            EventCallback onRotateRight,
            EventCallback onZoomOut,
            EventCallback onZoomIn,
            EventCallback onReset,
            EventCallback onClose)
        {
            OriginalNode = originalNode;
            Scale = scale;
            Rotation = rotation;
            FlipX = flipX;
            FlipY = flipY;
            Current = current;
            Total = total;
            ImageUrl = imageUrl;
            OnFlipY = onFlipY;
            OnFlipX = onFlipX;
            OnRotateLeft = onRotateLeft;
            OnRotateRight = onRotateRight;
            OnZoomOut = onZoomOut;
            OnZoomIn = onZoomIn;
            OnReset = onReset;
            OnClose = onClose;
        }

        /// <summary>Default toolbar content that can be retained alongside custom actions.</summary>
        public RenderFragment OriginalNode { get; }

        /// <summary>Current zoom scale.</summary>
        public double Scale { get; }

        /// <summary>Current rotation angle in degrees.</summary>
        public int Rotation { get; }

        /// <summary>Whether the image is flipped horizontally.</summary>
        public bool FlipX { get; }

        /// <summary>Whether the image is flipped vertically.</summary>
        public bool FlipY { get; }

        /// <summary>One-based index of the current image.</summary>
        public int Current { get; }

        /// <summary>Total number of preview images.</summary>
        public int Total { get; }

        /// <summary>URL of the current preview image.</summary>
        public string ImageUrl { get; }

        /// <summary>Flip vertically.</summary>
        public EventCallback OnFlipY { get; }

        /// <summary>Flip horizontally.</summary>
        public EventCallback OnFlipX { get; }

        /// <summary>Rotate left.</summary>
        public EventCallback OnRotateLeft { get; }

        /// <summary>Rotate right.</summary>
        public EventCallback OnRotateRight { get; }

        /// <summary>Zoom out.</summary>
        public EventCallback OnZoomOut { get; }

        /// <summary>Zoom in.</summary>
        public EventCallback OnZoomIn { get; }

        /// <summary>Reset the image transform.</summary>
        public EventCallback OnReset { get; }

        /// <summary>Close the preview.</summary>
        public EventCallback OnClose { get; }
    }
}
