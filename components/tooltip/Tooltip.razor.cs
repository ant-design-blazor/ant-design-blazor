using Microsoft.AspNetCore.Components;
using AntBlazor.Internal;
using OneOf;

namespace AntBlazor
{
    public partial class Tooltip : OverlayTrigger
    {
        [Parameter]
        public OneOf<string, RenderFragment> Title { get; set; } = string.Empty;

        public Tooltip()
        {
            PrefixCls = "ant-tooltip";
            Placement = PlacementType.TopCenter;
        }

        public override string GetOverlayEnterClass()
        {
            return "zoom-big-fast-enter zoom-big-fast-enter-active zoom-big-fast";
        }

        public override string GetOverlayLeaveClass()
        {
            return "zoom-big-fast-leave zoom-big-fast-leave-active zoom-big-fast";
        }

        public override string GetPlacementClass()
        {
            if (!string.IsNullOrEmpty(PlacementCls))
            {
                return PlacementCls;
            }

            if (PlacementType.TopCenter.Equals(Placement))
            {
                return $"{PrefixCls}-placement-top";
            }
            else if (PlacementType.BottomCenter.Equals(Placement))
            {
                return $"{PrefixCls}-placement-bottom";
            }
            else
            {
                return $"{PrefixCls}-placement-{Placement.Name}";
            }
        }

    }
}
