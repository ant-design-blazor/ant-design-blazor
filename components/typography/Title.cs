// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace AntDesign
{
    public class Title : TypographyBase
    {
        private const string PrefixName = "ant-typography";

        private const int DefaultLevel = 1;

        private int _level = DefaultLevel;

        [Parameter]
        public int Level
        {
            get
            {
                return _level;
            }
            set
            {
                _level = value < 1 || value > 4
                    ? DefaultLevel
                    : value;
            }
        }

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

                builder.OpenElement(1, "h" + Level);
                builder.AddAttribute(2, "class", ClassMapper.Class);
                builder.AddAttribute(3, "style", Style);
                if (Mark) builder.OpenElement(4, "mark");
                if (Delete) builder.OpenElement(5, "del");
                if (Underline) builder.OpenElement(6, "u");
                builder.AddContent(7, ChildContent);
                if (Underline) builder.CloseElement();
                if (Delete) builder.CloseElement();
                if (Mark) builder.CloseElement();
                if (Copyable)
                {
                    builder.OpenElement(8, "a");
                    builder.AddAttribute(9, "onclick", (Action)(async () => await Copy()));
                    builder.OpenComponent<Icon>(10);
                    builder.AddAttribute(11, "type", "copy");
                    builder.AddAttribute(12, "theme", IconThemeType.Outline);
                    builder.CloseComponent();
                    builder.CloseElement();
                }
                builder.AddElementReferenceCapture(13, r => Ref = r);
                builder.CloseElement();
            }
        }
    }
}
