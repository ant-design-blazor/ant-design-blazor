using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace AntDesign
{
    public partial class ReuseTabs : AntDomComponentBase
    {
        [Parameter]
        public string TabPaneClass { get; set; }

        [Parameter]
        public bool Draggable { get; set; }

        [Parameter]
        public TabSize Size { get; set; }

        [Parameter]
        public RenderFragment<ReuseTabsPageItem> Body { get; set; } = context => context.Body;

        [Parameter]
        public ReuseTabsLocale Locale { get; set; } = LocaleProvider.CurrentLocale.ReuseTabs;

        [Parameter]
        public bool HidePages { get; set; }

        [CascadingParameter]
        private RouteData RouteData { get; set; }

        [Inject]
        private NavigationManager Navmgr { get; set; }

        [Inject]
        private ReuseTabsService ReuseTabsService { get; set; }

        [CascadingParameter(Name = "AntDesign.InReusePageContent")]
        private bool InReusePageContent { get; set; }

        protected override void OnInitialized()
        {
            if (InReusePageContent)
            {
                return;
            }
            base.OnInitialized();
            ReuseTabsService.Init(true);
            ReuseTabsService.OnStateHasChanged += OnStateHasChanged;
            ReuseTabsService.TrySetRouteData(RouteData, true);

            Navmgr.LocationChanged += OnLocationChanged;
        }

        protected override bool ShouldRender() => !InReusePageContent;

        protected override void Dispose(bool disposing)
        {
            ReuseTabsService.OnStateHasChanged -= OnStateHasChanged;
            Navmgr.LocationChanged -= OnLocationChanged;
            base.Dispose(disposing);
        }

        private void OnLocationChanged(object o, LocationChangedEventArgs _)
        {
            ReuseTabsService.TrySetRouteData(RouteData, true);

            StateHasChanged();
        }

        private void OnStateHasChanged()
        {
            _ = InvokeStateHasChangedAsync();
        }
    }
}
