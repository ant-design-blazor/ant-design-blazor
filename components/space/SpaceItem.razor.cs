// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

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
        private string[] _compactClasses = Array.Empty<string>();
        private string ItemStyle => Compact == null ? Style : $"{Style}display: contents;";

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
            var first = isFirst();
            var last = isLast();
            _compactClasses = new[]
            {
                "compact-item",
                first ? "compact-first-item" : null,
                last ? "compact-last-item" : null,
                $"compact-{direction}-item",
                first ? $"compact-{direction}-first-item" : null,
                last ? $"compact-{direction}-last-item" : null,
                RTL ? "compact-item-rtl" : null
            }.Where(x => x != null).ToArray();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (Compact != null && _compactClasses.Length > 0)
            {
                await Js.InvokeVoidAsync(JSInteropConstants.StyleHelper.AddSpaceCompactClasses, Ref, _compactClasses);
            }
        }

        protected override void Dispose(bool disposing)
        {
            Compact?.RemoveComponent(this);
            base.Dispose(disposing);
        }
    }
}
