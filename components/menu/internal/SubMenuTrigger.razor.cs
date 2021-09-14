using Microsoft.AspNetCore.Components;

namespace AntDesign.Internal
{
    public partial class SubMenuTrigger : OverlayTrigger
    {
        [CascadingParameter(Name = "SubMenu")]
        public SubMenu SubMenuComponent { get; set; }

        [Parameter]
        public string TriggerClass { get; set; }
    }
}
