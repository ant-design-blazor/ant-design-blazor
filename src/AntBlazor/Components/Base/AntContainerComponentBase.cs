using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.CompilerServices;
using Microsoft.AspNetCore.Components.Rendering;

namespace AntBlazor
{
    public abstract class AntContainerComponentBase : AntDomComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public string Tag { get; set; } = "div";

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, Tag);
            builder.AddAttribute(1, "class", ClassMapper.Class);
            builder.AddAttribute(2, "style", GenerateStyle());
            builder.AddMultipleAttributes(3,
                RuntimeHelpers
                    .TypeCheck<global::System.Collections.Generic.IEnumerable<
                        global::System.Collections.Generic.KeyValuePair<string, object>>>(Attributes));
            builder.AddAttribute(4, "Id", Id);
            builder.AddElementReferenceCapture(5, (__value) => { Ref = __value; });
            builder.AddContent(7, ChildContent);
            builder.CloseElement();
        }
    }
}