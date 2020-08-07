using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AntDesign.Internal;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    public partial class Popover : OverlayTrigger
    {
        [Parameter] public OneOf<string, RenderFragment> Title { get; set; } = string.Empty;

        [Parameter] public OneOf<string, RenderFragment> Content { get; set; } = string.Empty;

        [Parameter] public bool ArrowPointAtCenter { get; set; } = false;

        [Parameter] public double MouseEnterDelay { get; set; } = 0.1;

        [Parameter] public double MouseLeaveDelay { get; set; } = 0.1;

        public Popover()
        {
            PrefixCls = "ant-popover";
            Placement = PlacementType.Top;
        }

        internal override string GetOverlayEnterClass()
        {
            return "zoom-big-fast-enter zoom-big-fast-enter-active zoom-big-fast";
        }

        internal override string GetOverlayLeaveClass()
        {
            return "zoom-big-fast-leave zoom-big-fast-leave-active zoom-big-fast";
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
    }
}
