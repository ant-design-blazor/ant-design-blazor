using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace AntBlazor
{
    public class AntText : AntTypographyBase
    {
        [Parameter]
        public bool code { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            SetClassMap();
        }

        protected void SetClassMap()
        {
            string prefixName = "ant-typography";
            ClassMapper.Clear()
                .Add("ant-typography")
                .If($"{prefixName}-{type}", () => !string.IsNullOrEmpty(type))
                .If($"{prefixName}-disabled", () => disabled);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            SetClassMap();
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            // According to Ant-Design 4.0, Fallback to 1 if level is invalid.

            builder.OpenElement(0, "span");
            builder.AddAttribute(1, "class", this.ClassMapper.Class);
            if (mark) builder.OpenElement(2, "mark");
            if (delete) builder.OpenElement(3, "del");
            if (underline) builder.OpenElement(4, "u");
            if (code) builder.OpenElement(5, "code");
            if (strong) builder.OpenElement(6, "strong");
            builder.AddContent(6, ChildContent);
            if (strong) builder.CloseElement();
            if (code) builder.CloseElement();
            if (underline) builder.CloseElement();
            if (delete) builder.CloseElement();
            if (mark) builder.CloseElement();
            if (copyable)
            {
                builder.OpenElement(7, "a");
                builder.AddAttribute(8, "onclick", (Action)(async () => await Copy()));
                builder.OpenComponent<AntIcon>(9);
                builder.AddAttribute(10, "type", "copy");
                builder.AddAttribute(11, "theme", AntIconThemeType.Outline);
                builder.CloseComponent();
                builder.CloseElement();
            }
            builder.CloseElement();
        }
    }
}
