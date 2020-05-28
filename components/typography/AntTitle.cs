using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign
{
    public class AntTitle : AntTypographyBase
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

                builder.OpenElement(0, "h" + localLevel);
                builder.AddAttribute(1, "class", this.ClassMapper.Class);
                if (Mark) builder.OpenElement(2, "mark");
                if (Delete) builder.OpenElement(3, "del");
                if (Underline) builder.OpenElement(4, "u");
                builder.AddContent(5, ChildContent);
                if (Underline) builder.CloseElement();
                if (Delete) builder.CloseElement();
                if (Mark) builder.CloseElement();
                if (Copyable)
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
}
