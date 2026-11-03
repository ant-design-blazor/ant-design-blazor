// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /// <summary>
    /// <see cref="Space"/> item, use to set item style
    /// </summary>
    public partial class SpaceItem : AntDomComponentBase
    {
        [CascadingParameter]
        private SpaceCompact Compact { get; set; }

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
            Compact?.AddComponent(this);
        }

        protected override void OnParametersSet()
        {
            if (Parent != null)
            {
                _index = Parent.SpaceItemCount++;
            }
            base.OnParametersSet();
        }

        internal void RefreshCompactClasses()
        {
            _ = InvokeAsync(StateHasChanged);
        }

        internal void SetCompactClasses(string direction, Func<bool> isFirst, Func<bool> isLast)
        {
            ClassMapper
                .If("ant-space-item-compact-item", () => direction.IsIn("vertical", "horizontal"))
                .If("ant-space-item-compact-first-item", isFirst)
                .If("ant-space-item-compact-last-item", isLast)
                .GetIf(() => $"ant-space-item-compact-{direction}-item", () => direction.IsIn("vertical", "horizontal"))
                .If($"ant-space-item-compact-{direction}-first-item", () => direction.IsIn("vertical", "horizontal") && isFirst())
                .If($"ant-space-item-compact-{direction}-last-item", () => direction.IsIn("vertical", "horizontal") && isLast());
        }

        protected override void Dispose(bool disposing)
        {
            Compact?.RemoveComponent(this);
            base.Dispose(disposing);
        }
    }
}
