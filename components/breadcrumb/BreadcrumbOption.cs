// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /// <summary>Defines one item or separator in a <see cref="Breadcrumb"/>.</summary>
    public class BreadcrumbOption
    {
        /// <summary>Entry title. Tokens such as <c>:id</c> are replaced using <see cref="Breadcrumb.Params"/>.</summary>
        public string Title { get; set; }

        public RenderFragment TitleTemplate { get; set; }

        /// <summary>Optional destination URL. Named tokens are replaced using <see cref="Breadcrumb.Params"/>.</summary>
        public string Href { get; set; }

        /// <summary>Compatibility alias used by generated navigation entries.</summary>
        public string RouterLink { get; set; }

        /// <summary>Set to <c>separator</c> to render an explicit separator entry.</summary>
        public string Type { get; set; }

        /// <summary>Content for an explicit separator entry.</summary>
        public string Separator { get; set; }

        public Dictionary<string, object> Params { get; set; }
    }
}
