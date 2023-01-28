// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class ListItemMeta : AntDomComponentBase
    {
        internal string PrefixName { get; set; } = "ant-list-item-meta";

        /// <summary>
        /// Title for the list item
        /// </summary>
        [Parameter]
        public string Title { get; set; }

        /// <summary>
        /// Title content of the list item. Takes priority over <see cref="Title"/>
        /// </summary>
        [Parameter]
        public RenderFragment TitleTemplate { get; set; }

        /// <summary>
        /// The avatar of list item
        /// </summary>
        [Parameter]
        public string Avatar { get; set; }

        /// <summary>
        /// Avatar content of the list item. Takes priority over <see cref="Avatar"/>
        /// </summary>
        [Parameter]
        public RenderFragment AvatarTemplate { get; set; }

        /// <summary>
        /// The description of list item
        /// </summary>
        [Parameter]
        public string Description { get; set; }

        /// <summary>
        /// The description content of list item. Takes priority over <see cref="Description"/>
        /// </summary>
        [Parameter]
        public RenderFragment DescriptionTemplate { get; set; }

        [CascadingParameter]
        public ListItem ListItem { get; set; }

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
