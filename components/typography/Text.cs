using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign
{
    public class Text : TypographyBase
    {
        [Parameter]
        public bool Code { get; set; }

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
                .If($"{prefixName}-{Type}", () => !string.IsNullOrEmpty(Type))
                .If($"{prefixName}-disabled", () => Disabled);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            SetClassMap();
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (builder != null)
            {
                base.BuildRenderTree(builder);
                // According to Ant-Design 4.0, Fallback to 1 if level is invalid.

                builder.OpenElement(0, "span");
                builder.AddAttribute(1, "class", this.ClassMapper.Class);
                if (Mark) builder.OpenElement(2, "mark");
                if (Delete) builder.OpenElement(3, "del");
                if (Underline) builder.OpenElement(4, "u");
                if (Code) builder.OpenElement(5, "code");
                if (Strong) builder.OpenElement(6, "strong");
                builder.AddContent(6, ChildContent);
                if (Strong) builder.CloseElement();
                if (Code) builder.CloseElement();
                if (Underline) builder.CloseElement();
                if (Delete) builder.CloseElement();
                if (Mark) builder.CloseElement();
                if (Copyable)
                {
                    builder.OpenElement(7, "a");
                    builder.AddAttribute(8, "onclick", (Action)(async () => await Copy()));
                    builder.OpenComponent<Icon>(9);
                    builder.AddAttribute(10, "type", "copy");
                    builder.AddAttribute(11, "theme", IconThemeType.Outline);
                    builder.CloseComponent();
                    builder.CloseElement();
                }

                builder.CloseElement();
            }
        }
    }
}
