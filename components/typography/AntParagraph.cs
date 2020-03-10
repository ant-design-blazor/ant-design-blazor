using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace AntBlazor
{
    public class AntParagraph:AntTypographyBase
    {
        [Parameter]
        public bool code { get; set; } = false;
        [Parameter]
        public RenderFragment ChildContent { get; set; }

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

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            // According to Ant-Design 4.0, Fallback to 1 if level is invalid.

            builder.OpenElement(0, "div");
            builder.OpenElement(1, "span");
            builder.AddAttribute(2, "class", this.ClassMapper.Class);
            if (mark) builder.OpenElement(3, "mark");
            if (delete) builder.OpenElement(4, "del");
            if (underline) builder.OpenElement(5, "u");
            if (code) builder.OpenElement(6, "code");
            if (strong) builder.OpenElement(7, "strong");
            builder.AddContent(6, ChildContent);
            if (strong) builder.CloseElement();
            if (code) builder.CloseElement();
            if (underline) builder.CloseElement();
            if (delete) builder.CloseElement();
            if (mark) builder.CloseElement();
            builder.CloseElement();
            builder.CloseElement();
        }
    }
}
