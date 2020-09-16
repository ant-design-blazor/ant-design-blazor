using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class AntListItemMeta
    {

        public string PrefixName { get; set; } = "ant-list-item-meta";

        [Parameter] public RenderFragment Title { get; set; }

        [Parameter] public string Avatar { get; set; }

        [Parameter] public RenderFragment AvatarTemplate { get; set; }

        [Parameter] public string Description { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
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
                .Add(PrefixName);
        }

    }
}
