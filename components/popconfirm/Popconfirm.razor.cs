﻿using System;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class Popconfirm : OverlayTrigger
    {
        [Parameter]
        public string Title { get; set; } = string.Empty;

        [Parameter]
        public RenderFragment TitleTemplate { get; set; }

        [Parameter]
        public string CancelText { get; set; } = "Cancel";

        [Parameter]
        public string OkText { get; set; } = "OK";

        [Parameter]
        public string OkType { get; set; } = "primary";

        [Parameter]
        public ButtonProps OkButtonProps { get; set; }

        [Parameter]
        public ButtonProps CancelButtonProps { get; set; }

        [Parameter]
        public string Icon { get; set; } = "exclamation-circle";

        [Parameter]
        public RenderFragment IconTemplate { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnCancel { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnConfirm { get; set; }

        [Parameter]
        public bool ArrowPointAtCenter { get; set; } = false;

        [Parameter]
        public double MouseEnterDelay { get; set; } = 0.1;

        [Parameter]
        public double MouseLeaveDelay { get; set; } = 0.1;

        public Popconfirm()
        {
            PrefixCls = "ant-popover";
            Placement = Placement.Top;
            Trigger = new[] { AntDesign.Trigger.Click };
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

        private async Task Cancel(MouseEventArgs args)
        {
            if (OnCancel.HasDelegate)
            {
                await OnCancel.InvokeAsync(args);
            }
            await base.Hide();
        }

        private async Task Confirm(MouseEventArgs args)
        {
            if (OnConfirm.HasDelegate)
            {
                await OnConfirm.InvokeAsync(args);
            }
            await base.Hide();
        }

        internal override string GetOverlayEnterClass()
        {
            return "ant-zoom-big-enter ant-zoom-big-enter-active ant-zoom-big";
        }

        internal override string GetOverlayLeaveClass()
        {
            return "ant-zoom-big-leave ant-zoom-big-leave-active ant-zoom-big";
        }
    }
}
