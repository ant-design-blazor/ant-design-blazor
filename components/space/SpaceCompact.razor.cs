// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    public partial class SpaceCompact : AntDomComponentBase
    {
        [CascadingParameter]
        public Space Parent { get; set; }

        [Parameter] public bool Block { get; set; }

        [Parameter] public string Direction { get; set; } = "horizontal";

        [Parameter] public string Size { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }

        private string _prefixCls = "ant-space-compact";

        private bool IsPresetSize => Size.IsIn("small", "middle", "large");

        private readonly List<SpaceItem> _items = new();

        private bool _refreshComponents;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            ClassMapper.Add(_prefixCls)
                .If($"{_prefixCls}-block", () => Block)
                .GetIf(() => $"{_prefixCls}-{Direction}", () => Direction.IsIn("vertical", "horizontal"))
                .GetIf(() => $"{_prefixCls}-{Size}", () => IsPresetSize)
                ;
        }

        internal void AddComponent(SpaceItem item)
        {
            if (item == null || _items.Contains(item))
            {
                return;
            }

            _items.Add(item);
            item.SetCompactClasses(Direction, () => _items.IndexOf(item) == 0, () => _items.IndexOf(item) == _items.Count - 1);
            _refreshComponents = true;
        }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);

            if (!_refreshComponents)
            {
                return;
            }

            _refreshComponents = false;
            foreach (var item in _items)
            {
                item.RefreshCompactClasses();
            }
        }

        internal void RemoveComponent(SpaceItem item)
        {
            _items.Remove(item);
            _refreshComponents = true;
        }
    }
}
