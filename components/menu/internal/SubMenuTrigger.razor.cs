using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntBlazor.Internal
{
    public partial class SubMenuTrigger : OverlayTrigger
    {
        [CascadingParameter(Name = "SubMenu")]
        public SubMenu SubMenuComponent { get; set; }

        [Parameter]
        public string TriggerClass { get; set; }
    }
}
