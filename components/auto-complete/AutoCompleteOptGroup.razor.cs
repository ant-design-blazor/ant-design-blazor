using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class AutoCompleteOptGroup : AntDomComponentBase
    {
        [Parameter]
        public string Label { get; set; }

        [Parameter]
        public RenderFragment LabelFragment { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }
    }
}
