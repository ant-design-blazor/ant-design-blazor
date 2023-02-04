﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace AntDesign
{
    public class Paragraph : TypographyBase
    {
        private const string PrefixName = "ant-typography";

        [Parameter]
        public bool Code { get; set; } = false;

        [Parameter]
        public bool Keyboard { get; set; } = false;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            SetClassMap();
        }

        protected void SetClassMap()
        {
            ClassMapper.Clear()
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
                builder.OpenElement(1, "div");
                builder.AddAttribute(2, "class", ClassMapper.Class);
                builder.AddAttribute(3, "style", Style);
                builder.OpenElement(4, "span");
                if (Mark) builder.OpenElement(5, "mark");
                if (Delete) builder.OpenElement(6, "del");
                if (Underline) builder.OpenElement(7, "u");
                if (Code) builder.OpenElement(8, "code");
                if (Keyboard) builder.OpenElement(9, "kbd");
                if (Strong) builder.OpenElement(10, "strong");
                builder.AddContent(11, ChildContent);
                if (Strong) builder.CloseElement();
                if (Code) builder.CloseElement();
                if (Keyboard) builder.CloseElement();
                if (Underline) builder.CloseElement();
                if (Delete) builder.CloseElement();
                if (Mark) builder.CloseElement();
                builder.CloseElement();
                if (Copyable)
                {
                    builder.OpenElement(12, "a");
                    builder.AddAttribute(13, "onclick", (Action)(async () => await Copy()));
                    builder.OpenComponent<Icon>(14);
                    builder.AddAttribute(15, "type", "copy");
                    builder.AddAttribute(16, "theme", IconThemeType.Outline);
                    builder.CloseComponent();
                    builder.CloseElement();
                }
                builder.AddElementReferenceCapture(17, r => Ref = r);
                builder.CloseElement();
            }
        }
    }
}
