// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public class Text : TypographyBase
    {
        protected override string HtmlType => "span";

        protected override bool IsKeyboard => Keyboard;

        [Parameter]
        public bool Keyboard { get; set; }
    }
}
