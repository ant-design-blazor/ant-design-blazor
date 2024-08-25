﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class ListItemMeta : AntDomComponentBase
    {
        public string PrefixName { get; set; } = "ant-list-item-meta";

        [Parameter] public string Title { get; set; }

        [Parameter] public RenderFragment TitleTemplate { get; set; }

        [Parameter] public string Avatar { get; set; }

        [Parameter] public RenderFragment AvatarTemplate { get; set; }

        [Parameter] public string Description { get; set; }

        [Parameter] public RenderFragment DescriptionTemplate { get; set; }

        [CascadingParameter] public ListItem ListItem { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            SetClassMap();
        }

        protected void SetClassMap()
        {
            ClassMapper.Clear()
                .Add(PrefixName);
        }
    }
}
