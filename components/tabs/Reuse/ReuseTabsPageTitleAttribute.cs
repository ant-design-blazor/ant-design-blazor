using System;

namespace AntDesign
{
    public class ReuseTabsPageTitleAttribute : Attribute
    {
        public string Title { get; set; }

        public ReuseTabsPageTitleAttribute(string title)
        {
            Title = title;
        }
    }
}
