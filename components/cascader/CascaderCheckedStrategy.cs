// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign
{
    /// <summary>
    /// Determines which checked Cascader nodes are rendered as selection tags.
    /// </summary>
    public enum CascaderCheckedStrategy
    {
        /// <summary>Render every checked node.</summary>
        ShowAll,

        /// <summary>Render parent nodes when all their checkable descendants are checked; otherwise render checked leaves.</summary>
        ShowParent,

        /// <summary>Render only checked leaf nodes.</summary>
        ShowChild
    }
}
