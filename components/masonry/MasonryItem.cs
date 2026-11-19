// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /// <summary>
    /// A masonry item.
    /// </summary>
    public class MasonryItem<T>
    {
        /// <summary>
        /// Unique identifier for the item
        /// </summary>
        public object Key { get; set; }

        /// <summary>
        /// Specifies the column to which the item belongs
        /// </summary>
        public int? Column { get; set; }

        /// <summary>
        /// Height of the item
        /// </summary>
        public int? Height { get; set; }

        /// <summary>
        /// Custom display content, takes precedence over itemRender
        /// </summary>
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Custom data storage
        /// </summary>
        public T Data { get; set; }
    }
}
