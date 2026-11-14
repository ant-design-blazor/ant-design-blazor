// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /// <summary>
    /// <see cref="Space"/> item, use to set item style
    /// </summary>
    public partial class SpaceItem : AntDomComponentBase
    {
        [CascadingParameter]
        private Space Parent { get; set; }

        /// <summary>
        /// Child content
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

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
