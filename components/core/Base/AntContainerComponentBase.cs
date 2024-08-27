// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace AntDesign
{
    public abstract class AntContainerComponentBase : AntDomComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public string Tag { get; set; } = "div";

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (builder != null)
            {
                builder.OpenElement(0, Tag);
                builder.AddAttribute(1, "class", ClassMapper.Class);
                builder.AddAttribute(2, "style", GenerateStyle());
                builder.AddAttribute(4, "id", Id);
                builder.AddElementReferenceCapture(5, (value) => { Ref = value; });
                builder.AddContent(7, ChildContent);
                builder.CloseElement();
            }
        }
    }
}
