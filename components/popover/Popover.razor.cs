﻿using System;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.Internal;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Popover : OverlayTrigger
    {
        [Parameter]
        public string Title { get; set; } = string.Empty;

        [Parameter]
        public RenderFragment TitleTemplate { get; set; }

        [Parameter]
        public string Content { get; set; } = string.Empty;

        [Parameter]
        public RenderFragment ContentTemplate { get; set; }

        [Parameter]
        public bool ArrowPointAtCenter { get; set; } = false;

        [Parameter]
        public double MouseEnterDelay { get; set; } = 0.1;

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
