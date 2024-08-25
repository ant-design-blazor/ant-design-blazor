using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public interface IDescriptionsItem
    {
        internal ElementReference Ref { get; set; }

        string Title { get; set; }

        RenderFragment TitleTemplate { get; set; }

        int Span { get; set; }

        RenderFragment ChildContent { get; set; }

        public string LabelStyle { get; set; }

        public string ContentStyle { get; set; }
    }
}
