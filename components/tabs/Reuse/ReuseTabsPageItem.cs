using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public class ReuseTabsPageItem
    {
        public string Url { get; set; }

        public RenderFragment Title { get; set; }

        public RenderFragment Body { get; set; }
    }
}
