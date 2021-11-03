using System.Linq;
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

        [Inject]
        public NavigationManager Navmgr { get; set; }

        [CascadingParameter(Name = "RouteView")]
        public ReuseTabsRouteView RouteView { get; set; }

        private ReuseTabsPageItem[] Pages => RouteView?.Pages;

        private string CurrentUrl
        {
            get => Navmgr.Uri;
            set => Navmgr.NavigateTo(value);
        }

        private void RemovePage(string key)
        {
            this.RouteView?.RemovePage(key);
            StateHasChanged();
        }

        private void RemoveOther(string key)
        {
            foreach (var item in Pages.Where(x => x.Closable && x.Url != key))
            {
                this.RouteView?.RemovePage(item.Url);
            }
            StateHasChanged();
        }

        private void RemoveAll()
        {
            foreach (var item in Pages.Where(x => x.Closable))
            {
                this.RouteView?.RemovePage(item.Url);
            }
            StateHasChanged();
        }
    }
}
