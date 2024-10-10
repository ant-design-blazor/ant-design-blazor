// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

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

        /// <summary>
        /// Set the child element
        /// </summary>
        [Parameter] public RenderFragment ChildContent { get; set; }

        private string FlexStyle => new CssStyleBuilder()
            .AddStyle("flex-wrap", Wrap)
            .AddStyle("align-items", Align)
            .AddStyle("justify-content", Justify)
            .AddStyle("flex", FlexCss)
            .AddStyle("gap", Gap.IsIn(null, "small", "middle", "large") ? "" : (CssSizeLength)Gap)
            .Build();

        protected override void OnInitialized()
        {
            base.OnInitialized();
            SetClass();
        }

        private void SetClass()
        {
            var hashId = UseStyle("ant-flex", AntDesign.FlexStyle.UseComponentStyle);
            ClassMapper.Add("ant-flex")
                .Add(hashId)
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
            builder.AddAttribute(3, "style", $"{FlexStyle} {Style} ");
            builder.AddAttribute(4, "id", $"{Id}");
            builder.AddElementReferenceCapture(5, r => Ref = r);
            builder.AddContent(6, ChildContent);
            builder.CloseElement();

            builder.AddContent(7, _styleContent);
        }
    }
}
