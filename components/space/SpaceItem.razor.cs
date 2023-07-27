using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class SpaceItem : AntDomComponentBase
    {
        [CascadingParameter]
        public Space Parent { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

 
        private string _marginStyle = "";
        private int _index;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            ClassMapper.Add("ant-space-item");
        }

        protected override void OnParametersSet()
        {
            _index = Parent.SpaceItemCount++;
            base.OnParametersSet();
        }
    }
}
