// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace AntDesign
{
    public class Link : TypographyBase
    {
        protected override string HtmlType => "a";

        protected override bool IsKeyboard => Keyboard;

        [Parameter]
        public string Href { get; set; }

        [Parameter]
        public string Target { get; set; }

        protected override void AddElementAttributes(RenderTreeBuilder builder, int sequence)
        {
            builder.AddAttribute(sequence, "href", Href);
            builder.AddAttribute(sequence + 1, "target", Target);
        }

        [Parameter]
        public bool Keyboard { get; set; }
    }
}
