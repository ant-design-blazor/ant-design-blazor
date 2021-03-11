// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public class PaginationItemRenderContext
    {
        public int Page { get; set; }

        public PaginationItemType Type { get; set; }

        public RenderFragment<PaginationItemRenderContext> OriginalElement { get; set; }

        public bool Disabled { get; set; }

        public PaginationItemRenderContext(int page, PaginationItemType type, RenderFragment<PaginationItemRenderContext> originalElement, bool disabled = false)
        {
            Page = page;
            Type = type;
            OriginalElement = originalElement;
            Disabled = disabled;
        }
    }
}
