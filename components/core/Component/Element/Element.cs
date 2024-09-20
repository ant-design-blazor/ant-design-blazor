// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign.Internal
{
    public class Element : AntDomComponentBase
    {
        [Parameter]
        public string HtmlTag { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public EventCallback<ElementReference> RefChanged { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> Attributes { get; set; } = new Dictionary<string, object>();

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            builder.OpenElement(0, HtmlTag);
            builder.AddMultipleAttributes(1, Attributes);
            builder.AddAttribute(2, "class", Class);
            builder.AddAttribute(3, "style", Style);
            if (HtmlTag == "button")
                builder.AddEventStopPropagationAttribute(5, "onclick", true);

            builder.AddElementReferenceCapture(6, async capturedRef =>
            {
                Ref = capturedRef;

                if (RefChanged.HasDelegate)
                    await RefChanged.InvokeAsync(Ref);
            });

            builder.AddContent(10, ChildContent);
            builder.CloseElement();
        }
    }
}
