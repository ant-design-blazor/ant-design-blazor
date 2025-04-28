// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using AntDesign.Core.Documentation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using OneOf;

namespace AntDesign
{
    /**
    <summary>
    <para>A flex layout container for alignment.</para>

    <h2>When To Use</h2>

    <list type="bullet">
        <item>Good for setting spacing between elements.</item>
        <item>Suitable for setting various horizontal and vertical alignments.</item>
    </list>
    
    <h3>Difference with Space component</h3>
    <list type="bullet">
        <item>Space is used to set the spacing between inline elements. It will add a wrapper element for each child element for inline alignment. Suitable for equidistant arrangement of multiple child elements in rows and columns.</item>
        <item>Flex is used to set the layout of block-level elements. It does not add a wrapper element. Suitable for layout of child elements in vertical or horizontal direction, and provides more flexibility and control.</item>
    </list>

    </summary>
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.Layout, "https://mdn.alipayobjects.com/huamei_7uahnr/afts/img/A*SMzgSJZE_AwAAAAAAAAAAAAADrJ8AQ/original", Columns = 1, Title = "Flex", SubTitle = "弹性布局")]
    public partial class Flex : AntDomComponentBase
    {
        /// <summary>
        /// Is direction of the flex vertical, use flex-direction: column
        /// </summary>
        [Obsolete("Use FlexDirection instead")]
        [Parameter]
        public bool Vertical
        {
            get => Direction == FlexDirection.Vertical;
            set
            {
                if (value)
                    Direction = FlexDirection.Vertical;
                else
                    Direction = FlexDirection.Horizontal;
            }
        }

        /// <summary>
        /// Sets the direction of the flex, either horizontal or vertical
        /// </summary>
        [PublicApi("1.2.0")]
        [Parameter] public FlexDirection Direction { get; set; } = FlexDirection.Horizontal;

        /// <summary>
        /// Set whether the element is displayed in a single line or in multiple lines
        /// reference flex-wrap:https://developer.mozilla.org/en-US/docs/Web/CSS/flex-wrap
        /// </summary>
        [Parameter] public OneOf<FlexWrap, string> Wrap { get; set; } = FlexWrap.NoWrap;

        /// <summary>
        /// Sets the alignment of elements in the direction of the main axis
        /// reference justify-content https://developer.mozilla.org/en-US/docs/Web/CSS/justify-content
        /// </summary>
        [Parameter] public OneOf<FlexJustify, string> Justify { get; set; } = FlexJustify.Normal;

        /// <summary>
        /// Sets the alignment of elements in the direction of the cross axis
        /// reference align-items https://developer.mozilla.org/en-US/docs/Web/CSS/align-items
        /// </summary>
        [Parameter] public OneOf<FlexAlign, string> Align { get; set; } = FlexAlign.Normal;

        /// <summary>
        /// flex CSS shorthand properties
        /// reference flex https://developer.mozilla.org/en-US/docs/Web/CSS/flex
        /// </summary>
        [Parameter] public string FlexCss { get; set; } = "normal";

        /// <summary>
        /// Sets the gap between grids
        /// small | middle | large | string | number
        /// </summary>
        [Parameter] public OneOf<FlexGap, string> Gap { get; set; } = FlexGap.Normal;

        /// <summary>
        /// Custom element type
        /// </summary>
        [Parameter] public string Component { get; set; } = "div";

        /// <summary>
        /// Set the child element
        /// </summary>
        [Parameter] public RenderFragment ChildContent { get; set; }

        private static readonly Dictionary<FlexWrap, string> _wrapMap = new()
        {
            [FlexWrap.NoWrap] = "nowrap",
            [FlexWrap.Wrap] = "wrap",
            [FlexWrap.WrapReverse] = "wrap-reverse",
        };

        private static readonly Dictionary<FlexAlign, string> _alignMap = new()
        {
            [FlexAlign.Normal] = "normal",
            [FlexAlign.Center] = "center",
            [FlexAlign.Start] = "start",
            [FlexAlign.End] = "end",
            [FlexAlign.SelfStart] = "self-start",
            [FlexAlign.SelfEnd] = "self-end",
            [FlexAlign.Baseline] = "baseline",
            [FlexAlign.FirstBaseline] = "first baseline",
            [FlexAlign.LastBaseline] = "last baseline",
            [FlexAlign.Stretch] = "stretch",
            [FlexAlign.AnchorCenter] = "anchor-center",
            [FlexAlign.Safe] = "safe",
            [FlexAlign.Unsafe] = "unsafe",
            [FlexAlign.FlexStart] = "flex-start",
            [FlexAlign.FlexEnd] = "flex-end",
        };

        private static readonly Dictionary<FlexJustify, string> _justifyMap = new()
        {
            [FlexJustify.Start] = "start",
            [FlexJustify.End] = "end",
            [FlexJustify.FlexStart] = "flex-start",
            [FlexJustify.FlexEnd] = "flex-end",
            [FlexJustify.Center] = "center",
            [FlexJustify.Left] = "left",
            [FlexJustify.Right] = "right",
            [FlexJustify.Normal] = "normal",
            [FlexJustify.SpaceBetween] = "space-between",
            [FlexJustify.SpaceAround] = "space-around",
            [FlexJustify.SpaceEvenly] = "space-evenly",
            [FlexJustify.Stretch] = "stretch",
            [FlexJustify.Safe] = "safe",
            [FlexJustify.Unsafe] = "unsafe"
        };

        private static readonly Dictionary<FlexGap, string> _gapMap = new()
        {
            [FlexGap.Normal] = "normal",
            [FlexGap.Small] = "small",
            [FlexGap.Middle] = "middle",
            [FlexGap.Large] = "large",
        };

        private string FlexStyle => new CssStyleBuilder()
            .AddStyle("flex-wrap", Wrap.IsT0 ? _wrapMap[Wrap.AsT0].ToString() : Wrap.AsT1)
            .AddStyle("align-items", Align.IsT0 ? _alignMap[Align.AsT0].ToString() : Align.AsT1)
            .AddStyle("justify-content", Justify.IsT0 ? _justifyMap[Justify.AsT0].ToString() : Justify.AsT1)
            .AddStyle("flex", FlexCss)
            .AddStyle("gap", Gap.IsT0 ? "" : (CssSizeLength)Gap.AsT1)
            .Build();

        protected override void OnInitialized()
        {
            base.OnInitialized();
            SetClass();
        }

        private void SetClass()
        {
            ClassMapper.Add("ant-flex")
                .GetIf(() => "ant-flex-vertical", () => Direction == FlexDirection.Vertical)
                .GetIf(() => "ant-flex-align-stretch", () => Direction == FlexDirection.Vertical && Align.IsT0 && Align.AsT0 == FlexAlign.Normal)
                .GetIf(() => $"ant-flex-align-{_alignMap[Align.AsT0]}", () => Align.IsT0)
                .GetIf(() => $"ant-flex-justify-{_justifyMap[Justify.AsT0]}", () => Justify.IsT0)
                .GetIf(() => $"ant-flex-gap-{_gapMap[Gap.AsT0]}", () => Gap.IsT0)
                .GetIf(() => $"ant-flex-wrap-{_wrapMap[Wrap.AsT0]}", () => Wrap.IsT0)
                ;
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(1, Component ?? "div");

            builder.AddAttribute(2, "class", ClassMapper.Class);
            builder.AddAttribute(3, "style", $"{FlexStyle} {Style} ");
            builder.AddAttribute(4, "id", $"{Id}");
            builder.AddElementReferenceCapture(5, r => Ref = r);
            builder.AddContent(6, ChildContent);
            builder.CloseElement();
        }
    }
}
