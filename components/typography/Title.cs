using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace AntDesign
{
    public class Title : TypographyBase
    {
        [Parameter]
        public int Level { get; set; } = 1;

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
                int localLevel = Level < 1 || Level > 4 ? 1 : Level;

                builder.OpenElement(1, "h" + localLevel);
                builder.AddAttribute(2, "class", this.ClassMapper.Class);
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
