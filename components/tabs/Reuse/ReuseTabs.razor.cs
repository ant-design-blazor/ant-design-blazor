using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class ReuseTabs : AntDomComponentBase
    {
        [Parameter]
        public string TabPaneClass { get; set; }

        [Parameter]
        public RenderFragment<ReuseTabsPageItem> Body { get; set; } = context => context.Body;

        [Inject]
        public NavigationManager Navmgr { get; set; }

        [CascadingParameter(Name = "RouteView")]
        public ReuseTabsRouteView RouteView { get; set; }

        internal ReuseTabsPageItem[] Pages => RouteView.Pages;

        protected string CurrentUrl
        {
            get => Navmgr.Uri;
            set => Navmgr.NavigateTo(value);
        }

        protected void RemovePage(string key)
        {
            this.RouteView?.RemovePage(key);
            StateHasChanged();
        }
    }
}
