using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AntDesign
{
    /// <summary>Context provided to a custom Tabs overflow popup.</summary>
    public sealed class TabsMoreContext
    {
        public IReadOnlyList<TabPane> RestTabs { get; init; }
        public string ActiveKey { get; init; }
        public Func<string, Task> OnChange { get; init; }
        public Func<Task> OnClose { get; init; }
    }
}
