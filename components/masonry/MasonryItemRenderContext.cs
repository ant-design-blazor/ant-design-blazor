// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /// <summary>
    /// Rendering information for a masonry item.
    /// </summary>
    public sealed class MasonryItemRenderContext<T>
    {
        internal MasonryItemRenderContext(MasonryItem<T> item, int index, int column)
        {
            Item = item;
            Index = index;
            Column = column;
        }

        /// <summary>
        /// The original masonry item.
        /// </summary>
        public MasonryItem<T> Item { get; }

        /// <summary>
        /// The item's unique identifier.
        /// </summary>
        public object Key => Item.Key;

        /// <summary>
        /// Custom data stored on the item.
        /// </summary>
        public T Data => Item.Data;

        /// <summary>
        /// The item index.
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// The column assigned to the item.
        /// </summary>
        public int Column { get; }

        /// <summary>
        /// Custom display content.
        /// </summary>
        public RenderFragment ChildContent => Item.ChildContent;
    }
}
