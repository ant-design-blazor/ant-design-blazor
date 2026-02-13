// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /// <summary>
    /// Overlay service for displaying render fragments at arbitrary positions
    /// </summary>
    public interface IOverlayService
    {
        /// <summary>
        /// Open an overlay with the specified render fragment at the given position
        /// </summary>
        /// <param name="content">The content to render</param>
        /// <param name="x">X coordinate (left position in pixels)</param>
        /// <param name="y">Y coordinate (top position in pixels)</param>
        /// <param name="container">Container selector (default: body)</param>
        /// <returns>A reference to the overlay instance that can be used to close it</returns>
        Task<OverlayReference> OpenAsync(RenderFragment content, double x, double y, string container = "body");

        /// <summary>
        /// Open an overlay with the specified render fragment at the given position
        /// </summary>
        /// <param name="content">The content to render</param>  
        /// <param name="x">X coordinate (left position in pixels)</param>
        /// <param name="y">Y coordinate (top position in pixels)</param>
        /// <param name="container">Container selector (default: body)</param>
        /// <returns>A reference to the overlay instance that can be used to close it</returns>
        OverlayReference Open(RenderFragment content, double x, double y, string container = "body");

        /// <summary>
        /// Close all open overlays
        /// </summary>
        void CloseAll();
    }
}
