using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class ReuseTabs : AntDomComponentBase
    {
        [Inject]
        public NavigationManager Navmgr { get; set; }

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

        [CascadingParameter(Name = "RouteView")]
        public ReuseTabsRouteView RouteView { get; set; }

        private ReuseTabsPageItem[] Pages => RouteView?.Pages;

        private string CurrentUrl
        {
            get => RouteView?.CurrentUrl;
            set => RouteView?.Navmgr.NavigateTo(value);
        }

        [Inject]
        private ReuseTabsService ReuseTabsService { get; set; }

        protected override void OnInitialized()
        {
            ReuseTabsService.GetNewKeyByUrl += GetNewKeyByUrl;

            ReuseTabsService.OnClosePage += RemovePage;
            ReuseTabsService.OnCloseOther += RemoveOther;
            ReuseTabsService.OnCloseAll += RemoveAll;
            ReuseTabsService.OnCloseCurrent += RemoveCurrent;            
        }

        protected override void Dispose(bool disposing)
        {
            ReuseTabsService.GetNewKeyByUrl -= GetNewKeyByUrl;

            ReuseTabsService.OnClosePage -= RemovePage;
            ReuseTabsService.OnCloseOther -= RemoveOther;
            ReuseTabsService.OnCloseAll -= RemoveAll;
            ReuseTabsService.OnCloseCurrent -= RemoveCurrent;

            base.Dispose(disposing);
        }

        private void RemovePage(string key)
        {
            var reuseTabsPageItem = Pages.FirstOrDefault(w => w.Url == key);
            if (reuseTabsPageItem != null && reuseTabsPageItem.ShowForever)
            {
                return;
            }

            this.RouteView?.RemovePage(key);
            StateHasChanged();
        }

        private void RemoveOther(string key)
        {
            foreach (var item in Pages.Where(x => x.Closable && x.Url != key && !x.ShowForever))
            {
                this.RouteView?.RemovePage(item.Url);
            }
            StateHasChanged();
        }

        private void RemoveAll()
        {
            foreach (var item in Pages.Where(x => x.Closable && !x.ShowForever))
            {
                this.RouteView?.RemovePage(item.Url);
            }
            StateHasChanged();
        }

        private void RemoveCurrent()
        {
            RemovePage(this.CurrentUrl);
        }

        private string GetNewKeyByUrl(string url)
        {
            return RouteView?.GetNewKeyByUrl(url);
        }

    }
}
