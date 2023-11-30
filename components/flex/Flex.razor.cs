// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace AntDesign
{
    public partial class Flex : AntDomComponentBase
    {
        /// <summary>
        /// Is direction of the flex vertical, use flex-direction: column
        /// </summary>
        [Parameter] public bool Vertical { get; set; }

        /// <summary>
        /// Set whether the element is displayed in a single line or in multiple lines
        /// reference flex-wrap:https://developer.mozilla.org/en-US/docs/Web/CSS/flex-wrap
        /// </summary>
        [Parameter] public string Wrap { get; set; } = "nowrap";

        /// <summary>
        /// Sets the alignment of elements in the direction of the main axis
        /// reference justify-content https://developer.mozilla.org/en-US/docs/Web/CSS/justify-content
        /// </summary>
        [Parameter] public string Justify { get; set; } = "normal";

        /// <summary>
        /// Sets the alignment of elements in the direction of the cross axis
        /// reference align-items https://developer.mozilla.org/en-US/docs/Web/CSS/align-items
        /// </summary>
        [Parameter] public string Align { get; set; } = "normal";

        /// <summary>
        /// flex CSS shorthand properties
        /// reference flex https://developer.mozilla.org/en-US/docs/Web/CSS/flex
        /// </summary>
        [Parameter] public string FlexCss { get; set; } = "normal";

        /// <summary>
        /// Sets the gap between grids
        /// small | middle | large | string | number
        /// </summary>
        [Parameter] public string Gap { get; set; } = "normal";

        /// <summary>
        /// Custom element type
        /// </summary>
        [Parameter] public string Component { get; set; } = "div";

        [Parameter] public RenderFragment ChildContent { get; set; }

        private string CustomizeGapStyle => Gap.IsIn("small", "middle", "large") ? "" : $"gap: {Gap}px;";
        private string FlexStyle => !string.IsNullOrWhiteSpace(FlexCss) ? $"flex: {FlexCss};" : "";

        protected override void OnInitialized()
        {
            base.OnInitialized();
            SetClass();
        }

        private void SetClass()
        {
            ClassMapper.Add("ant-flex")
                .GetIf(() => "ant-flex-vertical", () => Vertical)
                .GetIf(() => "ant-flex-align-stretch", () => Vertical && Align == "normal")
                .GetIf(() => $"ant-flex-align-{Align.ToLowerInvariant()}", () => !string.IsNullOrWhiteSpace(Align))
                .GetIf(() => $"ant-flex-justify-{Justify.ToLowerInvariant()}", () => !string.IsNullOrWhiteSpace(Justify))
                .GetIf(() => $"ant-flex-gap-{Gap}", () => Gap.IsIn("small", "middle", "large"))
                .GetIf(() => $"ant-flex-wrap-{Wrap}", () => !string.IsNullOrWhiteSpace(Wrap))
                ;
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(1, Component ?? "div");

            builder.AddAttribute(2, "class", ClassMapper.Class);
            builder.AddAttribute(3, "style", $"{CustomizeGapStyle} {FlexStyle} {Style}");
            builder.AddAttribute(4, "id", $"{Id}");
            builder.AddComponentReferenceCapture(5, r => Ref = (ElementReference)r);

            builder.CloseElement();
        }
    }
}
