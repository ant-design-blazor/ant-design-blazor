using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AntDesign.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;

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
            Placement = PlacementType.Top;
            Trigger = new[] { TriggerType.Click };
        }

        internal override async Task Show(int? overlayLeft = null, int? overlayTop = null)
        {
            if (Trigger.Contains(TriggerType.Hover))
            {
                await Task.Delay((int)(MouseEnterDelay * 1000));
            }

            await base.Show(overlayLeft, overlayTop);
        }

        internal override async Task Hide(bool force = false)
        {
            if (Trigger.Contains(TriggerType.Hover))
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
    }
}
