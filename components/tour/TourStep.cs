// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;
using AntDesign.JsInterop;

namespace AntDesign
{
    public class TourStep
    {
        /// <summary>
        /// Get the element the guide card points to. Empty makes it show in center of screen
        /// </summary>
        public ForwardRef Target { get; set; }

        /// <summary>
        /// Get the element the guide card points to. Empty makes it show in center of screen
        /// </summary>
        public bool Arrow { get; set; }

        /// <summary>
        /// Customize close icon
        /// </summary>
        public RenderFragment CloseIcon { get; set; }

        /// <summary>
        /// Displayed pictures or videos
        /// </summary>
        public RenderFragment Cover { get; set; }

        /// <summary>
        /// title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Position of the guide card relative to the target element
        /// </summary>
        public Placement Placement { get; set; }

        /// <summary>
        /// Callback function on shutdown
        /// </summary>
        public Action OnClose { get; set; }

        /// <summary>
        /// Whether to enable masking, change mask style and fill color by pass custom props, the default follows the mask property of Tour
        /// </summary>
        public string Mask { get; set; }

        /// <summary>
        /// Type, affects the background color and text color
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Properties of the Next button
        /// </summary>
        public TourStepButtonProps NextButtonProps { get; set; }

        /// <summary>
        /// 	Properties of the previous button
        /// </summary>
        public TourStepButtonProps PrevButtonProps { get; set; }

        /// <summary>
        /// support pass custom scrollIntoView options, the default follows the scrollIntoViewOptions property of Tour
        /// </summary>
        public bool ScrollIntoViewOptions { get; set; }

        internal HtmlElement Dom { get; set; }
    }

    public class TourStepButtonProps
    {
        public RenderFragment Children { get; set; }

        public Action OnClick { get; set; }
    }
}
