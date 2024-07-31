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

        private List<AntDomComponentBase> _components = new();

        protected override void OnInitialized()
        {
            base.OnInitialized();

            ClassMapper.Add(_prefixCls)
                .If($"{_prefixCls}-block", () => Block)
                .GetIf(() => $"{_prefixCls}-{Direction}", () => Direction.IsIn("vertical", "horizontal"))
                .GetIf(() => $"{_prefixCls}-{Size}", () => IsPresetSize)
                ;
        }

        internal void AddComponent(AntDomComponentBase component)
        {
            _components.Add(component);

            var prefixCls = component.PrefixClassName;

            component.ClassMapper
                .GetIf(() => $"{prefixCls}-compact-item", () => Direction.IsIn("vertical", "horizontal"))
                .If($"{prefixCls}-compact-first-item", () => _components.IndexOf(component) == 0)
                .If($"{prefixCls}-compact-last-item", () => _components.IndexOf(component) == _components.Count - 1)
                .If($"{prefixCls}-compact-{Direction}-item", () => Direction.IsIn("vertical", "horizontal"))
                .If($"{prefixCls}-compact-{Direction}-first-item", () => Direction.IsIn("vertical", "horizontal") && _components.IndexOf(component) == 0)
                .If($"{prefixCls}-compact-{Direction}-last-item", () => Direction.IsIn("vertical", "horizontal") && _components.IndexOf(component) == _components.Count - 1)
            ;
        }

        internal void RemoveComponent(AntDomComponentBase component)
        {
            _components.Remove(component);
        }
    }
}
