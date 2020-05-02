using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;

namespace AntBlazor
{
    public class AntParagraph : AntTypographyBase
    {
        [Parameter]
        public bool Code { get; set; } = false;

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
                builder.OpenElement(0, "div");
                builder.AddAttribute(1, "class", this.ClassMapper.Class);
                builder.OpenElement(2, "span");
                if (Mark) builder.OpenElement(3, "mark");
                if (Delete) builder.OpenElement(4, "del");
                if (Underline) builder.OpenElement(5, "u");
                if (Code) builder.OpenElement(6, "code");
                if (Strong) builder.OpenElement(7, "strong");
                builder.AddContent(8, ChildContent);
                if (Strong) builder.CloseElement();
                if (Code) builder.CloseElement();
                if (Underline) builder.CloseElement();
                if (Delete) builder.CloseElement();
                if (Mark) builder.CloseElement();
                builder.CloseElement();
                if (Copyable)
                {
                    builder.OpenElement(9, "a");
                    builder.AddAttribute(10, "onclick", (Action)(async () => await Copy()));
                    builder.OpenComponent<AntIcon>(10);
                    builder.AddAttribute(11, "type", "copy");
                    builder.AddAttribute(12, "theme", AntIconThemeType.Outline);
                    builder.CloseComponent();
                    builder.CloseElement();
                }
                builder.CloseElement();
            }
        }
    }
}
