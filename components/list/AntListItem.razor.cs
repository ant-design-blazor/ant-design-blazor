using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace AntBlazor
{
    public partial class AntListItem : AntDomComponentBase
    {
        internal string _prefixName = "ant-list-item";

        [Parameter] public string Content { get; set; }

        [Parameter] public RenderFragment Extra { get; set; }

        // todo: list
        [Parameter] public string Actions { get; set; }

        [Parameter] public AntDirectionVHIType ItemLayout { get; set; } = AntDirectionVHIType.Horizontal;

        [Parameter] public RenderFragment ChildContent { get; set; }

        public bool IsVerticalAndExtra()
        {
            return this.ItemLayout == AntDirectionVHIType.Vertical && this.Extra != null;
        }

        protected override void OnInitialized()
        {
            SetClassMap();
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            SetClassMap();
        }

        protected void SetClassMap()
        {
            ClassMapper.Clear()
                .Add(_prefixName);
        }
    }
}
