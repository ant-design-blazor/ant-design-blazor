﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class MenuItemGroup : AntDomComponentBase
    {
        [CascadingParameter]
        public Menu RootMenu { get; set; }

        /// <summary>
        /// Title of the group
        /// </summary>
        [Parameter]
        public string Title { get; set; }

        /// <summary>
        /// Title of the group
        /// </summary>
        [Parameter]
        public RenderFragment TitleTemplate { get; set; }

        /// <summary>
        /// SubMenus or MenuItems
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Unique ID
        /// </summary>
        [Parameter]
        public string Key { get; set; }

        protected override void OnInitialized()
        {
            ClassMapper.Add($"{RootMenu.PrefixCls}-item-group");
        }
    }
}
