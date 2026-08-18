// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign
{
    /// <summary>
    /// The key and column of an item after masonry layout.
    /// </summary>
    public sealed class MasonryLayoutItem
    {
        /// <summary>
        /// The item key.
        /// </summary>
        public object Key { get; init; }

        /// <summary>
        /// The item's column.
        /// </summary>
        public int Column { get; init; }
    }
}
