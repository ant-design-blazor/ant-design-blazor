using Microsoft.AspNetCore.Components;

namespace AntDesign.TestApp.Maui.Pages.Dashboard.Monitor
{
    public partial class Gauge
    {
        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public string Color { get; set; }

        [Parameter]
        public int? Height { get; set; }

        [Parameter]
        public int? BgColor { get; set; }

        [Parameter]
        public int Percent { get; set; }

        [Parameter]
        public bool? ForceFit { get; set; }
    }
}