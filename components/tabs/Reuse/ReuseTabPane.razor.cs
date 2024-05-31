// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class ReuseTabPane
    {
        [Parameter]
        public ReuseTabsPageItem ReuseTabsPageItem { get; set; }

        protected override bool ShouldRender()
        {
            return ReuseTabsPageItem.Url == ReuseTabsService.CurrentUrl;
        }
    }
}
