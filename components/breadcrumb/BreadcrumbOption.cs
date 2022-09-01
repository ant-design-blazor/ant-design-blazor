using System;
using System.Collections.Generic;

namespace AntDesign
{
    /// <summary>
    /// Not currently used. Planned for future development.
    /// </summary>
    public class BreadcrumbOption
    {
        public string Label { get; set; }

        public Dictionary<string, object> Params { get; set; }

        public Uri Url { get; set; }
    }
}
