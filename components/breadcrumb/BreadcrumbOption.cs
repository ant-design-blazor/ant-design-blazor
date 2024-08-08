using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
