// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.Internal;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /**
    <summary>
    <para>The floating card popped by clicking or hovering.</para>

    <h2>When To Use</h2>

    <para>A simple popup menu to provide extra information or operations.</para>

    <para>Comparing with <c>Tooltip</c>, besides information <c>Popover</c> card can also provide action elements like links and buttons.</para>

    <h2>Two types</h2>

    <para>There are 2 rendering approaches for <c>Popover</c>: </para> 
    <list type="number">
        <item>Wraps child element (content of the <c>Popover</c>) with a <c>div</c> tag (default approach).</item>
        <item>Child element is not wrapped with anything. This approach requires usage of <c>Unbound</c> tag inside <c>Popover</c> and depending on the child element type (please refer to the first example):
            <list type="bullet">
                <item>html tag: has to have its <c>@ref</c> set to <c>@context.Current</c></item>
                <item><c>Ant Design Blazor</c> component: has to have its <c>RefBack</c> attribute set to <c>@context</c>.</item>
            </list>       
        </item>
    </list>
    <para>Please ensure that the child node of <c>Popover</c> accepts <c>onMouseEnter</c>, <c>onMouseLeave</c>, <c>onFocus</c>, <c>onClick</c> events.</para>
    </summary>
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.DataDisplay, "https://gw.alipayobjects.com/zos/alicdn/1PNL1p_cO/Popover.svg", Title = "Popover", SubTitle = "气泡卡片")]
    public partial class Popover : OverlayTrigger
    {
        /// <summary>
        /// Title string of the card
        /// </summary>
        [Parameter]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Title content of the card. Takes priority over <see cref="Title"/>
        /// </summary>
        [Parameter]
        public RenderFragment TitleTemplate { get; set; }

        /// <summary>
        /// Content string of the card
        /// </summary>
        [Parameter]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Content of the card. Takes priority over <see cref="Content"/>
        /// </summary>
        [Parameter]
        public RenderFragment ContentTemplate { get; set; }

        /// <summary>
        /// Point the arrow at the center of the wrapped element or not
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool ArrowPointAtCenter { get; set; } = false;

        /// <summary>
        /// Delay in seconds, before tooltip is shown on mouse enter
        /// </summary>
        /// <default value="0.1" />
        [Parameter]
        public double MouseEnterDelay { get; set; } = 0.1;

        /// <summary>
        /// Delay in seconds, before tooltip is hidden on mouse leave
        /// </summary>
        /// <default value="0.1" />
        [Parameter]
        public double MouseLeaveDelay { get; set; } = 0.1;

        public Popover()
        {
            PrefixCls = "ant-popover";
            Placement = Placement.Top;
        }

        internal override string GetOverlayEnterClass()
        {
            return "ant-zoom-big-enter ant-zoom-big-enter-active ant-zoom-big";
        }

        internal override string GetOverlayLeaveClass()
        {
            return "ant-zoom-big-leave ant-zoom-big-leave-active ant-zoom-big";
        }

        internal override async Task Show(int? overlayLeft = null, int? overlayTop = null)
        {
            if (Trigger.Contains(AntDesign.Trigger.Hover))
            {
                await Task.Delay((int)(MouseEnterDelay * 1000));
            }

            await base.Show(overlayLeft, overlayTop);
        }

        internal override async Task Hide(bool force = false)
        {
            if (Trigger.Contains(AntDesign.Trigger.Hover))
            {
                await Task.Delay((int)(MouseLeaveDelay * 1000));
            }

            await base.Hide(force);
        }
    }
}
