// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;

namespace AntDesign
{
#if NET6_0_OR_GREATER
    [CascadingTypeParameter(nameof(TItem))]
#endif
    public class DirectoryTree<TItem> : Tree<TItem>
    {
        public DirectoryTree()
        {
            base.BlockNode = true;
            base.Directory = true;
        }
    }
}
