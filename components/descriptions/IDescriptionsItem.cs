using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;
using OneOf;

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
    }
}
