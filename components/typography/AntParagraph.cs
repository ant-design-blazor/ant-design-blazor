using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace AntBlazor
{
    public class AntParagraph : AntTypographyBase
    {
        [Inject]
        private HtmlRenderService _service { get; set; }
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

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            SetClassMap();
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            // According to Ant-Design 4.0, Fallback to 1 if level is invalid.
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "class", this.ClassMapper.Class);
            builder.OpenElement(2, "span");
            if (mark) builder.OpenElement(3, "mark");
            if (delete) builder.OpenElement(4, "del");
            if (underline) builder.OpenElement(5, "u");
            if (code) builder.OpenElement(6, "code");
            if (strong) builder.OpenElement(7, "strong");
            builder.AddContent(8, ChildContent);
            if (strong) builder.CloseElement();
            if (code) builder.CloseElement();
            if (underline) builder.CloseElement();
            if (delete) builder.CloseElement();
            if (mark) builder.CloseElement();
            builder.CloseElement();
            if (copyable)
            {
                builder.OpenElement(9, "a");
                builder.AddAttribute(10, "onclick", (Action)(async ()=> await this.JsInvokeAsync<object>(JSInteropConstants.log, await _service.RenderAsync(ChildContent))));
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
