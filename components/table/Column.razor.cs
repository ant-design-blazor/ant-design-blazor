using AntBlazor.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace AntBlazor
{
    public sealed partial class Column
    {
        internal struct IsRenderingHeadWrapper
        {
            public bool Value { get; }

            public IsRenderingHeadWrapper(bool value) => Value = value;
        }

        [CascadingParameter]
        internal ITable Target { get; set; }

        [CascadingParameter]
        internal IsRenderingHeadWrapper IsRenderingHead { get; set; }

        [Parameter]
        public object Data { get; set; }

        [Parameter]
        public RenderFragment Title { get; set; }

        [Parameter]
        public RenderFragment Render { get; set; }
    }
}
