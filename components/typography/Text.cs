using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace AntDesign
{
    public class Text : TypographyBase
    {
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
                int i = 0;

                builder.OpenElement(i++, "span");
                builder.AddAttribute(i++, "class", this.ClassMapper.Class);
                builder.AddAttribute(i++, "style", Style);
                if (Mark) builder.OpenElement(i++, "mark");
                if (Delete) builder.OpenElement(i++, "del");
                if (Underline) builder.OpenElement(i++, "u");
                if (Code) builder.OpenElement(i++, "code");
                if (Keyboard) builder.OpenElement(i++, "kbd");
                if (Strong) builder.OpenElement(i++, "strong");
                builder.AddContent(i++, ChildContent);
                if (Strong) builder.CloseElement();
                if (Code) builder.CloseElement();
                if (Keyboard) builder.CloseElement();
                if (Underline) builder.CloseElement();
                if (Delete) builder.CloseElement();
                if (Mark) builder.CloseElement();
                if (Copyable)
                {
                    builder.OpenElement(i++, "a");
                    builder.AddAttribute(i++, "onclick", (Action)(async () => await Copy()));
                    builder.OpenComponent<Icon>(i++);
                    builder.AddAttribute(i++, "Type", "copy");
                    builder.AddAttribute(i++, "Theme", IconThemeType.Outline);
                    builder.CloseComponent();
                    builder.CloseElement();
                }

                builder.CloseElement();
            }
        }
    }
}
