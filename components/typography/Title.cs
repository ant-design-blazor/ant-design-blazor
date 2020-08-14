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
                int localLevel = Level < 1 || Level > 4 ? 1 : Level;
                int i = 0;
                builder.OpenElement(i++, "h" + localLevel);
                builder.AddAttribute(i++, "class", this.ClassMapper.Class);
                builder.AddAttribute(i++, "style", Style);
                if (Mark) builder.OpenElement(i++, "mark");
                if (Delete) builder.OpenElement(i++, "del");
                if (Underline) builder.OpenElement(i++, "u");
                builder.AddContent(i++, ChildContent);
                if (Underline) builder.CloseElement();
                if (Delete) builder.CloseElement();
                if (Mark) builder.CloseElement();
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
