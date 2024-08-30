// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /// <summary>
    /// Not currently used. Planned for future development.
    /// </summary>
    public class BreadcrumbOption
    {
        public string Title { get; set; }

        public RenderFragment TitleTemplate { get; set; }

        public Dictionary<string, object> Params { get; set; }

        public string RouterLink { get; set; }
    }
}
