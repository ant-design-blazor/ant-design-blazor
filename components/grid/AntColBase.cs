using Microsoft.AspNetCore.Components;

namespace AntBlazor
{
    public class AntColBase : AntDomComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }
    }
}