using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

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

        protected override void OnInitialized()
        {
            base.OnInitialized();

            ReuseTabsService.OnStateHasChanged += OnStateHasChanged;
        }

        protected override void Dispose(bool disposing)
        {
            ReuseTabsService.OnStateHasChanged -= OnStateHasChanged;
            base.Dispose(disposing);
        }

        public override Task SetParametersAsync(ParameterView parameters)
        {
            if (parameters.TryGetValue(nameof(RouteData), out RouteData routeData))
            {
                ReuseTabsService.TrySetRouteData(routeData, true);
            }

            return base.SetParametersAsync(parameters);
        }

        private void OnStateHasChanged()
        {
            _ = InvokeStateHasChangedAsync();
        }
    }
}
