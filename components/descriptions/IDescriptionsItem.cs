// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
