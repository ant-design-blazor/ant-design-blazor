using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace AntBlazor
{
    public class AntTitle : AntTypographyBase
    {
        [Parameter]
        public int level { get; set; } = 1;

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
            int localLevel = level < 1 || level > 4 ? 1 : level;

            builder.OpenElement(0, "h" + localLevel);
            builder.AddAttribute(1, "class", this.ClassMapper.Class);
            if (mark) builder.OpenElement(2, "mark");
            if (delete) builder.OpenElement(3, "del");
            if (underline) builder.OpenElement(4, "u");
            builder.AddContent(5, ChildContent);
            if (underline) builder.CloseElement();
            if (delete) builder.CloseElement();
            if (mark) builder.CloseElement();
            if (copyable)
            {
                builder.OpenElement(6, "a");
                builder.AddAttribute(7, "onclick", (Action)(async () => await Copy()));
                builder.OpenComponent<AntIcon>(8);
                builder.AddAttribute(9, "type", "copy");
                builder.AddAttribute(10, "theme", AntIconThemeType.Outline);
                builder.CloseComponent();
                builder.CloseElement();
            }
            builder.CloseElement();
        }
    }
}
