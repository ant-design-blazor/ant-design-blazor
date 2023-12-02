// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Tour : AntDomComponentBase
    {
        /// <summary>
        /// Whether to show the arrow
        /// </summary>
        [Parameter] public bool Arrow { get; set; } = true;

        /// <summary>
        /// Whether the arrow to point to the center of the element
        /// </summary>
        [Parameter] public bool ArrowPointAtCenter { get; set; } = true;

        /// <summary>
        /// Customize close icon
        /// </summary>
        [Parameter] public string CloseIcon { get; set; }

        /// <summary>
        /// The template for <see cref="CloseIcon" />
        /// </summary>
        [Parameter] public RenderFragment CloseIconTemplate { get; set; }

        /// <summary>
        /// Callback function on shutdown
        /// </summary>
        [Parameter] public EventCallback OnClose { get; set; }

        /// <summary>
        /// Whether to enable masking, change mask style and fill color by pass custom props
        /// </summary>
        [Parameter] public string Mask { get; set; }

        /// <summary>
        /// Type, affects the background color and text color
        /// </summary>
        [Parameter] public string Type { get; set; }

        /// <summary>
        /// Open tour
        /// </summary>
        [Parameter] public bool Open { get; set; }

        /// <summary>
        /// Callback when the step changes. Current is the previous step
        /// </summary>
        [Parameter] public EventCallback<int> OnChange { get; set; }

        /// <summary>
        /// What is the current step
        /// </summary>
        [Parameter] public int Current { get; set; }

        /// <summary>
        /// support pass custom scrollIntoView options
        /// </summary>
        [Parameter] public bool ScrollIntoViewOptions { get; set; }

        /// <summary>
        /// custom indicator
        /// </summary>
        [Parameter] public RenderFragment<(int current, int total)> IndicatorsRender { get; set; }

        /// <summary>
        /// Tour's zIndex
        /// </summary>
        [Parameter] public int ZIndex { get; set; } = 1001;
    }
}
