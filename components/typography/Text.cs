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
            ClassMapper
                .Add("ant-typography")
                .GetIf(() => $"{prefixName}-{Type}", () => !string.IsNullOrEmpty(Type))
                .If($"{prefixName}-disabled", () => Disabled)
                .If($"{prefixName}-rtl", () => RTL);
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (builder != null)
            {
                base.BuildRenderTree(builder);
                // According to Ant-Design 4.0, Fallback to 1 if level is invalid.

                builder.OpenElement(1, "span");
                builder.AddAttribute(2, "class", this.ClassMapper.Class);
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
