using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public ReuseTabsLocale Locale { get; set; } = LocaleProvider.CurrentLocale.ReuseTabs;

        [CascadingParameter]
        private RouteData RouteData { get; set; }

        private string CurrentUrl
        {
            get => Navmgr.Uri;
            set => Navmgr.NavigateTo(value);
        }

        [Inject]
        private ReuseTabsService ReuseTabsService { get; set; }

        private readonly Dictionary<string, ReuseTabsPageItem> _pageMap = new();

        private ReuseTabsPageItem[] Pages => _pageMap.Values.Where(x => !x.Ignore).OrderBy(x => x.CreatedAt).ToArray();

        protected override void OnInitialized()
        {
            ReuseTabsService.GetNewKeyByUrl += GetNewKeyByUrl;

            ReuseTabsService.OnClosePage += RemovePage;
            ReuseTabsService.OnCloseOther += RemoveOther;
            ReuseTabsService.OnCloseAll += RemoveAll;
            ReuseTabsService.OnCloseCurrent += RemoveCurrent;
        }

        public override Task SetParametersAsync(ParameterView parameters)
        {
            if (parameters.TryGetValue(nameof(RouteData), out RouteData routeData))
            {
                var pageType = routeData.PageType;
                var body = CreateBody(routeData, CurrentUrl);

                var reuseTabsPageItem = _pageMap.ContainsKey(CurrentUrl) ? _pageMap[CurrentUrl] : null;
                if (reuseTabsPageItem == null)
                {
                    _pageMap[CurrentUrl] = new ReuseTabsPageItem
                    {
                        Body = body,
                        Url = CurrentUrl,
                        CreatedAt = DateTime.Now,
                        Ignore = false
                    };
                }
                else if (reuseTabsPageItem.Body is null)
                {
                    reuseTabsPageItem.Body = body;
                }
            }

            return base.SetParametersAsync(parameters);
        }

        private RenderFragment CreateBody(RouteData routeData, string url)
        {
            return builder =>
            {
                builder.OpenComponent(0, routeData.PageType);
                foreach (var routeValue in routeData.RouteValues)
                {
                    builder.AddAttribute(1, routeValue.Key, routeValue.Value);
                }

                builder.AddComponentReferenceCapture(2, @ref =>
                {
                    GetPageInfo(_pageMap[url], routeData.PageType, url, @ref);
                });

                builder.CloseComponent();
            };
        }

        private void GetPageInfo(ReuseTabsPageItem pageItem, Type pageType, string url, object page)
        {
            if (page is IReuseTabsPage resuse)
            {
                pageItem.Title ??= resuse.GetPageTitle();
            }

            var attributes = pageType.GetCustomAttributes(true);

            if (attributes.FirstOrDefault(x => x is ReuseTabsPageTitleAttribute) is ReuseTabsPageTitleAttribute titleAttr && titleAttr != null)
            {
                pageItem.Title ??= titleAttr.Title?.ToRenderFragment();
            }

            if (attributes.FirstOrDefault(x => x is ReuseTabsPageAttribute) is ReuseTabsPageAttribute attr && attr != null)
            {
                pageItem.Title ??= attr.Title?.ToRenderFragment();
                pageItem.Ignore = attr.Ignore;
                pageItem.Closable = attr.Closable;
                pageItem.Pin = attr.Pin;
            }

            pageItem.Title ??= url.ToRenderFragment();
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
            if (reuseTabsPageItem?.Pin == true)
            {
                return;
            }

            RemovePageBase(key);
            StateHasChanged();
        }

        private void RemoveOther(string key)
        {
            foreach (var item in Pages.Where(x => x.Closable && x.Url != key && !x.Pin))
            {
                RemovePageBase(item.Url);
            }
            StateHasChanged();
        }

        private void RemoveAll()
        {
            foreach (var item in Pages.Where(x => x.Closable && !x.Pin))
            {
                RemovePageBase(item.Url);
            }
            StateHasChanged();
        }

        private void RemoveCurrent()
        {
            RemovePage(this.CurrentUrl);
        }

        private string GetNewKeyByUrl(string url)
        {
            return GetNewKeyByUrlBase(url);
        }

        public void RemovePageBase(string key)
        {
            _pageMap.Remove(key);
        }

        public void RemovePageWithRegex(string pattern)
        {
            foreach (var key in _pageMap.Keys)
            {
                if (Regex.IsMatch(key, pattern))
                {
                    _pageMap.Remove(key);
                }
            }
        }

        public string GetNewKeyByUrlBase(string url)
        {
            if (url.StartsWith("/"))
            {
                return url;
            }

            return "/" + url;
        }
    }
}
