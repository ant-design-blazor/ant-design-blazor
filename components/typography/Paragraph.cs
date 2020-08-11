using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace AntDesign
{
    public class Paragraph : TypographyBase
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
                int i = 0;

                base.BuildRenderTree(builder);
                builder.OpenElement(i++, "div");
                builder.AddAttribute(i++, "class", this.ClassMapper.Class);
                builder.AddAttribute(i++, "style", Style);
                builder.OpenElement(i++, "span");
                if (Mark) builder.OpenElement(i++, "mark");
                if (Delete) builder.OpenElement(i++, "del");
                if (Underline) builder.OpenElement(i++, "u");
                if (Code) builder.OpenElement(i++, "code");
                if (Strong) builder.OpenElement(i++, "strong");
                builder.AddContent(i++, ChildContent);
                if (Strong) builder.CloseElement();
                if (Code) builder.CloseElement();
                if (Underline) builder.CloseElement();
                if (Delete) builder.CloseElement();
                if (Mark) builder.CloseElement();
                builder.CloseElement();
                if (Copyable)
                {
                    builder.OpenElement(i++, "a");
                    builder.AddAttribute(i++, "onclick", (Action)(async () => await Copy()));
                    builder.OpenComponent<Icon>(i++);
                    builder.AddAttribute(i++, "type", "copy");
                    builder.AddAttribute(i++, "theme", IconThemeType.Outline);
                    builder.CloseComponent();
                    builder.CloseElement();
                }
                builder.CloseElement();
            }
        }
    }
}
