using Microsoft.AspNetCore.Components;

namespace AntDesign.Internal
{
    public partial class SubMenuTrigger : OverlayTrigger
    {
        [CascadingParameter(Name = "SubMenu")]
        public SubMenu SubMenuComponent { get; set; }

        [Parameter]
        public string TriggerClass { get; set; }

        internal override string GetOverlayEnterClass()
        {
            if (Placement == Placement.RightTop)
            {
                return "ant-zoom-big-enter ant-zoom-big-enter-active ant-zoom-big";
            }

            return base.GetOverlayEnterClass();
        }

        internal override string GetOverlayLeaveClass()
        {
            if (Placement == Placement.RightTop)
            {
                return "ant-zoom-big-leave ant-zoom-big-leave-active ant-zoom-big";
            }

            return base.GetOverlayLeaveClass();
        }
    }
}
