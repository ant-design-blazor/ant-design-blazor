using Microsoft.AspNetCore.Components;
using AntDesign.Internal;
using OneOf;
using System.Threading.Tasks;
using System.Linq;

namespace AntDesign
{
    public partial class Tooltip : OverlayTrigger
    {
        [Parameter]
        public OneOf<string, RenderFragment, MarkupString> Title { get; set; } = string.Empty;

        [Parameter]
        public bool ArrowPointAtCenter { get; set; } = false;

        [Parameter]
        public double MouseEnterDelay { get; set; } = 0.1;

        [Parameter]
        public double MouseLeaveDelay { get; set; } = 0.1;

        public Tooltip()
        {
            PrefixCls = "ant-tooltip";
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
