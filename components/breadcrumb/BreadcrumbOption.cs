using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AntDesign
{
    /// <summary>
    /// Not currently used. Planned for future development.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class BreadcrumbOption
    {
        public string Label { get; set; }

        public Dictionary<string, object> Params { get; set; }

        public string Url { get; set; }
    }
}
