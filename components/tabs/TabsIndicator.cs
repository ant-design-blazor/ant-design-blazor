using System;

namespace AntDesign
{
    /// <summary>Configures the size and alignment of the Tabs ink indicator.</summary>
    public sealed class TabsIndicator
    {
        /// <summary>Calculates the indicator size from the active tab's measured width or height.</summary>
        public Func<int, int> Size { get; set; }

        /// <summary>Aligns the indicator within the active tab.</summary>
        public TabsIndicatorAlign Align { get; set; } = TabsIndicatorAlign.Center;
    }

    public enum TabsIndicatorAlign
    {
        Start,
        Center,
        End
    }
}
