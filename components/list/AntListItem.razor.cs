using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign
{
    public partial class AntListItem : AntDomComponentBase
    {
        public string PrefixName { get; set; } = "ant-list-item";

        [Parameter] public string Content { get; set; }

        [Parameter] public RenderFragment Extra { get; set; }

        [Parameter] public List<RenderFragment> Actions { get; set; }

        [Parameter] public AntDirectionVHIType ItemLayout { get; set; } = AntDirectionVHIType.Horizontal;

        [Parameter] public ListGridType Grid { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public string ColStyle { get; set; }

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
                .Add(PrefixName)
                .If($"{PrefixName}-no-flex", () => !IsFlexMode());
        }

        private bool IsFlexMode()
        {
            if (ItemLayout == AntDirectionVHIType.Vertical)
            {
                return Extra != null;
            }

            return false;
        }

    }
}
