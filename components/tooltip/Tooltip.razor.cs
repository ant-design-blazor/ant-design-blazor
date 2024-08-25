// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using System.Threading.Tasks;
using AntDesign.Internal;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Tooltip : OverlayTrigger
    {
        [Parameter]
        public string Title { get; set; } = string.Empty;
        [Parameter]
        public RenderFragment TitleTemplate { get; set; }
        [Parameter]
        public bool ArrowPointAtCenter { get; set; } = false;

        [Parameter]
        public double MouseEnterDelay { get; set; } = 0.1;

        [Parameter]
        public double MouseLeaveDelay { get; set; } = 0.1;

        [Parameter]
        public int TabIndex { get; set; } = 0;

        public Tooltip()
        {
            PrefixCls = "ant-tooltip";
            Placement = Placement.Top;
        }

        internal override string GetOverlayEnterClass()
        {
            return "ant-zoom-big-fast-enter ant-zoom-big-fast-enter-active ant-zoom-big-fast";
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
        }

        internal override async Task Hide(bool force = false)
        {
            if (Trigger.Contains(AntDesign.Trigger.Hover))
            {
                await Task.Delay((int)(MouseLeaveDelay * 1000));
            }

            await base.Hide(force);
        }

        //internal async Task ChildElementMoved() =>await GetOverlayComponent().UpdatePosition();

    }
}
