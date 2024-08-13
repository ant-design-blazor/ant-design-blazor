using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public interface IDescriptionsItem
    {
        [Parameter]
        ElementReference Ref { get; set; }

        [Parameter]
        string Title { get; set; }

        [Parameter]
        RenderFragment TitleTemplate { get; set; }

        [Parameter]
        int Span { get; set; }

        [Parameter]
        RenderFragment ChildContent { get; set; }

        [Parameter]
        public string LabelStyle { get; set; }

        [Parameter]
        public string ContentStyle { get; set; }
    }
}
