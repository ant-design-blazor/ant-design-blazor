// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace AntDesign
{
    public class Text : TypographyBase
    {
        private const string PrefixName = "ant-typography";

        [Parameter]
        public bool Code { get; set; }

        [Parameter]
        public bool Keyboard { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            SetClassMap();
        }

        protected void SetClassMap()
        {
            ClassMapper
                .Add("ant-typography")
                .GetIf(() => $"{PrefixName}-{Type}", () => !string.IsNullOrEmpty(Type))
                .If($"{PrefixName}-disabled", () => Disabled)
                .If($"{PrefixName}-rtl", () => RTL);
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (builder != null)
            {
                base.BuildRenderTree(builder);

                builder.OpenElement(1, "span");
                builder.AddAttribute(2, "class", ClassMapper.Class);
                builder.AddAttribute(3, "style", Style);
                if (Mark) builder.OpenElement(4, "mark");
                if (Delete) builder.OpenElement(5, "del");
                if (Underline) builder.OpenElement(6, "u");
                if (Code) builder.OpenElement(7, "code");
                if (Keyboard) builder.OpenElement(8, "kbd");
                if (Strong) builder.OpenElement(9, "strong");
                builder.AddContent(10, ChildContent);
                if (Strong) builder.CloseElement();
                if (Code) builder.CloseElement();
                if (Keyboard) builder.CloseElement();
                if (Underline) builder.CloseElement();
                if (Delete) builder.CloseElement();
                if (Mark) builder.CloseElement();
                if (Copyable)
                {
                    builder.OpenElement(11, "a");
                    builder.AddAttribute(12, "onclick", (Action)(async () => await Copy()));
                    builder.OpenComponent<Icon>(13);
                    builder.AddAttribute(14, "Type", "copy");
                    builder.AddAttribute(15, "Theme", IconThemeType.Outline);
                    builder.CloseComponent();
                    builder.CloseElement();
                }
                builder.AddElementReferenceCapture(16, r => Ref = r);
                builder.CloseElement();
            }
        }
    }
}
