// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using System.Threading.Tasks;
using AntDesign.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AntDesign
{
    /**
    <summary>
    <para>A simple text popup tip.</para>

    <h2>When To Use</h2>

    <list type="bullet">
        <item>The tip is shown on mouse enter, and is hidden on mouse leave. The <c>Tooltip</c> doesn't support complex text or operations.</item>
        <item>To provide an explanation of a button/text/operation. It's often used instead of the html <c>title</c> attribute.</item>
    </list>

    <h2>Two types</h2>

    <para>There are 2 rendering approaches for <c>Tooltip</c>:</para>
    <list type="number">
        <item>Wraps child element (content of the <c>Tooltip</c>) with a <c>div</c> (default approach).</item>
        <item>
            Child element is not wrapped with anything. This approach requires usage of <c>Unbound</c> tag inside <c>Tooltip</c> and depending on the child element type (please refer to the first example):
            <list type="bullet">
                <item>html tag: has to have its <c>@ref</c> set to <c>@context.Current</c> </item>
                <item><c>Ant Design Blazor</c> component: has to have its <c>RefBack</c> attribute set to <c>@context</c>.</item>
            </list>
        </item>
    </list>

    <para>Please ensure that the child node of <c>Tooltip</c> accepts <c>onMouseEnter</c>, <c>onMouseLeave</c>, <c>onFocus</c>, <c>onClick</c> events.</para>
    </summary>
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.DataDisplay, "https://gw.alipayobjects.com/zos/alicdn/Vyyeu8jq2/Tooltp.svg", Title= "Tooltip",SubTitle = "文字提示")]
    public partial class Tooltip : OverlayTrigger
    {
        private readonly string _tooltipId = $"ant-blazor-tooltip-{System.Guid.NewGuid():N}";
        private RenderFragment TooltipContent => builder =>
        {
            if (TitleTemplate != null)
                builder.AddContent(0, TitleTemplate);
            else
                builder.AddContent(1, Title);
        };

        private string GetTooltipArrowStyle()
        {
            var placement = GetPlacementType().Name;
            if (placement.StartsWith("top", System.StringComparison.Ordinal))
            {
                var left = placement == "topLeft" ? "33%" : placement == "topRight" ? "66%" : "50%";
                return $"position: absolute; bottom: 0px; left: {left};";
            }
            if (placement.StartsWith("bottom", System.StringComparison.Ordinal))
            {
                var left = placement == "bottomLeft" ? "33%" : placement == "bottomRight" ? "66%" : "50%";
                return $"position: absolute; top: 0px; left: {left};";
            }
            if (placement.StartsWith("left", System.StringComparison.Ordinal))
            {
                var top = placement == "leftTop" ? "33%" : placement == "leftBottom" ? "66%" : "50%";
                return $"position: absolute; right: 0px; top: {top};";
            }

            var rightTop = placement == "rightTop" ? "33%" : placement == "rightBottom" ? "66%" : "50%";
            return $"position: absolute; left: 0px; top: {rightTop};";
        }

        /// <summary>
        /// The text shown in the tooltip
        /// </summary>
        [Parameter]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Content shown in the tooltip
        /// </summary>
        [Parameter]
        public RenderFragment TitleTemplate { get; set; }

        /// <summary>
        /// Whether the arrow is pointed at the center of target
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

        /// <summary>
        /// Tab index of the tooltip
        /// </summary>
        /// <default value="0" />
        [Parameter]
        public int TabIndex { get; set; } = 0;

        public Tooltip()
        {
            PrefixCls = "ant-tooltip";
            Placement = Placement.Top;
        }

        internal override string GetOverlayEnterClass()
        {
            // React removes its motion classes after the enter transition. The
            // shared Blazor Overlay keeps the returned class for its lifetime,
            // so Tooltip should not leave a completed-motion transform behind.
            return string.Empty;
        }

        internal override string GetOverlayLeaveClass()
        {
            return "ant-zoom-big-fast-leave ant-zoom-big-fast-leave-active ant-zoom-big-fast";
        }

        internal override async Task Show(int? overlayLeft = null, int? overlayTop = null)
        {
            if (Trigger.Contains(AntDesign.Trigger.Hover))
            {
                await Task.Delay((int)(MouseEnterDelay * 1000));
            }

            await base.Show(overlayLeft, overlayTop);
            if (IsOverlayShow())
            {
                await SetTriggerOpenState(true);
            }
        }

        internal override async Task Hide(bool force = false)
        {
            if (Trigger.Contains(AntDesign.Trigger.Hover))
            {
                await Task.Delay((int)(MouseLeaveDelay * 1000));
            }

            await base.Hide(force);
            if (!IsOverlayShow())
            {
                await SetTriggerOpenState(false);
            }
        }

        private async Task SetTriggerOpenState(bool open)
        {
            var module = await Js.InvokeAsync<Microsoft.JSInterop.IJSObjectReference>(
                "import", "./_content/AntDesign/js/tooltipTriggerHelper.js");
            try
            {
                await module.InvokeVoidAsync("setTooltipTriggerState", Ref, "ant-tooltip-open", _tooltipId, open, Unbound is null);
            }
            finally
            {
                await module.DisposeAsync();
            }
        }
    }
}
