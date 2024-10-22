// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
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

        private static readonly Dictionary<string, string> _spaceSize = new()
        {
            [SpaceSize.Small] = "8",
            [SpaceSize.Middle] = "16",
            [SpaceSize.Large] = "24"
        };

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

        private void ChangeSize()
        {
            var size = Parent.Size;
            var direction = Parent.Direction;

            size.Switch(sigleSize =>
            {
                _marginStyle = direction == DirectionVHType.Horizontal ? (_index != Parent.SpaceItemCount - 1 ? $"margin-right:{GetSize(sigleSize)};" : "") : $"margin-bottom:{GetSize(sigleSize)};";
            },
            arraySize =>
            {
                _marginStyle = (_index != Parent.SpaceItemCount - 1 ? $"margin-right:{GetSize(arraySize.Item1)};" : "") + $"margin-bottom:{GetSize(arraySize.Item2)};";
            });
        }

        private CssSizeLength GetSize(string size)
        {
            var originalSize = size.IsIn(_spaceSize.Keys) ? _spaceSize[size] : size;
            if (Parent?.Split != null)
            {
                return ((CssSizeLength)originalSize).Value / 2;
            }

            return originalSize;
        }
    }
}
