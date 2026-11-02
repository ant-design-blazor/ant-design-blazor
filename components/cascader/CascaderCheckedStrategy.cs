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
        /// <summary>
        /// All checked nodes are rendered as selection tags.
        /// </summary>
        ShowAll,

        /// <summary>
        /// Only checked leaf nodes are rendered as selection tags.
        /// </summary>
        ShowParent,

        /// <summary>
        /// Only checked parent nodes are rendered as selection tags.
        /// </summary>
        ShowChild
    }
}
