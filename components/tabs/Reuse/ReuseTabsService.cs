// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /// <summary>
    /// The service for reuse tabs
    /// </summary>
    public partial class ReuseTabsService : IDisposable
    {
        private readonly NavigationManager _navmgr;
        private readonly MenuService _menusService;
        private ICollection<ReuseTabsPageItem> _pages = new List<ReuseTabsPageItem>();

        internal event Action OnStateHasChanged;

        private ReuseTabsPageItem _currentPage;

        internal string CurrentUrl
        {
            get => "/" + _navmgr.ToBaseRelativePath(_navmgr.Uri);
            set
            {
                try
                {
                    _navmgr.NavigateTo(value.StartsWith('/') ? value[1..] : value);
                }
                catch (NavigationException)
                {
                    // would throw exception during static rendering
                }
            }
        }

        private string _activeKey;

        internal string ActiveKey
        {
            get => _activeKey;
            set
            {
                _activeKey = value;
                var pageItem = _pages.FirstOrDefault(r => r.Key == value);
                if (pageItem != null && (pageItem.Url != CurrentUrl))
                {
                    CurrentUrl = pageItem.Url;
                }
            }
        }

        /// <summary>
        /// The page information list of the currently opened page, which can be used for caching and recovery
        /// </summary>
        public ICollection<ReuseTabsPageItem> Pages => _pages;

        public ReuseTabsService(NavigationManager navmgr, MenuService menusService)
        {
            _navmgr = navmgr;
            _menusService = menusService;

            // Because the menu would be loaded asynchronously sometimes,
            // So need to refresh the title after menu loaded.
            _menusService.MenuItemLoaded += StateHasChanged;
        }

        internal ReuseTabsPageItem CurrentPage => _currentPage;

        /// <summary>
        /// Create a tab without navigation, the page doesn't really render until the tab is clicked
        /// </summary>
        /// <param name="pageUrl">The url of target page</param>
        /// <param name="titleTemplate">The title show on the tab</param>
        public void CreateTab(string pageUrl, RenderFragment titleTemplate = null)
        {
            if (_pages.Any(x => x.Url == pageUrl))
                return;
            AddPage(new ReuseTabsPageItem() { Url = pageUrl, Title = titleTemplate ?? pageUrl.ToRenderFragment(), CreatedAt = DateTime.MinValue });
            OnStateHasChanged?.Invoke();
        }

        /// <summary>
        /// Create a tab without navigation, the page doesn't really render until the tab is clicked
        /// </summary>
        /// <param name="pageUrl">The url of target page</param>
        /// <param name="title">The title show on the tab</param>
        public void CreateTab(string pageUrl, string title)
        {
            CreateTab(pageUrl, title.ToRenderFragment());
        }

        //public void Pin(string key)
        //{
        //    var reuseTabsPageItem = Pages.FirstOrDefault(w => w.Url == key);
        //    if (reuseTabsPageItem == null)
        //    {
        //        return;
        //    }
        //    reuseTabsPageItem.Pin = true;
        //    StateHasChanged();
        //}

        /// <summary>
        /// Close the page corresponding to the specified url
        /// </summary>
        /// <param name="url">The specified page's url</param>
        public bool ClosePage(string url)
        {
            var reuseTabsPageItem = _pages?.FirstOrDefault(w => w.Url == url);
            if (reuseTabsPageItem?.Closable != true)
            {
                return false;
            }

            RemovePageBase(reuseTabsPageItem.Url);
            StateHasChanged();

            return true;
        }

        /// <summary>
        /// Close the page corresponding to the specified key
        /// </summary>
        /// <param name="key">The specified page's key</param>
        internal bool ClosePageByKey(string key)
        {
            var reuseTabsPageItem = _pages?.FirstOrDefault(w => w.Key == key);
            if (reuseTabsPageItem?.Closable != true)
            {
                return false;
            }

            RemovePageBase(reuseTabsPageItem.Url);
            StateHasChanged();

            return true;
        }

        /// <summary>
        /// Close all pages except the page with the specified url
        /// </summary>
        /// <param name="url">The specified page's url</param>
        public void CloseOther(string url)
        {
            var closablePages = _pages?.Where(x => x.Closable && x.Url != url && !x.Pin).Select(x => x.Url).ToArray();
            foreach (var item in closablePages)
            {
                RemovePageBase(item);
            }
            StateHasChanged();
        }

        /// <summary>
        /// Close all pages that is Closable or is not Pinned
        /// </summary>
        public void CloseAll()
        {
            var closablePages = _pages?.Where(x => x.Closable && !x.Pin).Select(x => x.Url).ToArray();
            foreach (var item in closablePages)
            {
                RemovePageBase(item);
            }
            StateHasChanged();
        }

        /// <summary>
        /// Close current page
        /// </summary>
        public void CloseCurrent()
        {
            ClosePage(this.CurrentUrl);
        }

        /// <summary>
        /// Reload Current Page
        /// </summary>
        public void ReloadPage()
        {
            ReloadPage(null);
        }

        /// <summary>
        /// Reload the page corresponding to the specified url
        /// </summary>
        /// <param name="url"></param>
        public void ReloadPage(string url)
        {
            url ??= CurrentUrl;
            var reuseTabsPageItem = _pages?.FirstOrDefault(w => w.Url == url);
            reuseTabsPageItem.Body = null;
            reuseTabsPageItem.CachedRenderFragment = null; // Clear cached fragment to force re-render
            reuseTabsPageItem.Rendered = false;

            // only reload current page, and other page would be load by tab navigation
            if (reuseTabsPageItem?.Key == ActiveKey)
            {
                StateHasChanged();
            }
        }

        /// <summary>
        /// Update the state of the <see cref="ReuseTabs"/>
        /// </summary>
        public void Update()
        {
            StateHasChanged();
        }

        /// <summary>
        /// Update the options of the specified page
        /// </summary>
        /// <param name="url"> The url of the page </param>
        /// <param name="optionsAction"></param>
        public void UpdatePage(string url, Action<ReuseTabsPageItem> optionsAction)
        {
            var reuseTabsPageItem = _pages?.FirstOrDefault(w => w.Url == url);
            if (reuseTabsPageItem == null)
            {
                return;
            }

            optionsAction?.Invoke(reuseTabsPageItem);
            StateHasChanged();
        }

        public void UpdatePage(Action<ReuseTabsPageItem> optionsAction)
        {
            UpdatePage(CurrentUrl, optionsAction);
        }

        internal void StateHasChanged()
        {
            OnStateHasChanged?.Invoke();
        }

        internal void Init(bool reuse)
        {
            if (reuse)
            {
                ScanPinnedPageAttribute();
            }
        }

        internal void TrySetRouteData(RouteData routeData, bool reuse)
        {
            if (!reuse)
            {
                _pages.Clear();
            }

            var reuseTabsPageItem = _pages?.FirstOrDefault(w => w.Url == CurrentUrl || (w.Singleton && w.TypeName == routeData.PageType?.FullName));

            if (reuseTabsPageItem == null)
            {
                reuseTabsPageItem = new ReuseTabsPageItem
                {
                    Url = CurrentUrl,
                    CreatedAt = DateTime.Now,
                };

                AddPage(reuseTabsPageItem);
            }
            else if (reuseTabsPageItem.Singleton)
            {
                reuseTabsPageItem.Url = CurrentUrl; // singleton page would change url
                reuseTabsPageItem.RouteData = routeData; // to update component parameters
            }

            reuseTabsPageItem.Rendered = true;

            if (routeData == null)
            {
                reuseTabsPageItem.Title ??= b =>
                {
                    var url = reuseTabsPageItem.Url;
                    b.AddContent(0, _menusService.GetMenuTitle(url) ?? url.ToRenderFragment());
                };
            }
            else
            {
                reuseTabsPageItem.RouteData = routeData;
                reuseTabsPageItem.TypeName = routeData.PageType.FullName;
                reuseTabsPageItem.Body ??= CreateBody(reuseTabsPageItem);
            }

            ActiveKey = reuseTabsPageItem.Key;
            _currentPage = reuseTabsPageItem;

            OnStateHasChanged?.Invoke();
        }

        private RenderFragment CreateBody(ReuseTabsPageItem item)
        {
            return builder =>
            {
                var routeData = item.RouteData;
                builder.OpenComponent(0, routeData.PageType);
                foreach (var routeValue in routeData.RouteValues)
                {
                    builder.AddAttribute(1, routeValue.Key, routeValue.Value);
                }

                builder.AddComponentReferenceCapture(2, @ref =>
                {
                    GetPageInfo(item, routeData.PageType, item.Url, @ref);
                });

                builder.CloseComponent();
            };
        }

        private void GetPageInfo(ReuseTabsPageItem pageItem, Type pageType, string url, object page)
        {
            if (page is IReuseTabsPage resuse)
            {
                pageItem.Title = resuse.GetPageTitle();
            }

            var attributes = pageType.GetCustomAttributes(true);

            if (attributes.FirstOrDefault(x => x is ReuseTabsPageTitleAttribute) is ReuseTabsPageTitleAttribute titleAttr && titleAttr is { Title.Length: > 0 })
            {
                pageItem.Title ??= titleAttr.Title?.ToRenderFragment();
            }

            if (attributes.FirstOrDefault(x => x is ReuseTabsPageAttribute) is ReuseTabsPageAttribute attr && attr != null)
            {
                if (!string.IsNullOrWhiteSpace(attr.Title))
                {
                    pageItem.Title ??= attr.Title?.ToRenderFragment();
                }

                pageItem.Ignore = attr.Ignore;
                pageItem.Closable = attr.Closable;
                pageItem.Pin = attr.Pin;
                pageItem.KeepAlive = attr.KeepAlive;
                pageItem.Order = attr.Order;
                pageItem.Singleton = attr.Singleton;
            }

            pageItem.Title ??= b =>
            {
                b.AddContent(0, _menusService.GetMenuTitle(url) ?? url.ToRenderFragment());
            };

            FilterPages();
        }

        /// <summary>
        /// Get all assembly
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<Assembly> GetAllAssembly()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }

        /// <summary>
        /// Scan ReuseTabsPageAttribute
        /// </summary>
        private void ScanPinnedPageAttribute()
        {
            var list = GetAllAssembly();

            foreach (var item in list)
            {
                var allClass = item.ExportedTypes
                    .Where(w => w.GetCustomAttribute<ReuseTabsPageAttribute>()?.Pin == true);
                foreach (var pageType in allClass)
                {
                    this.AddPinnedPageItem(pageType);
                }
            }
            if (CurrentUrl == null)
                ActiveKey = _pages.FirstOrDefault()?.Key;
        }

        private void AddPinnedPageItem(Type pageType)
        {
            var routeAttribute = pageType.GetCustomAttribute<RouteAttribute>();
            var reuseTabsAttribute = pageType.GetCustomAttribute<ReuseTabsPageAttribute>();

            var url = reuseTabsAttribute?.PinUrl ?? routeAttribute.Template;
            if (_pages.Any(p => p.Url == url || (p.Singleton && p.TypeName == pageType.FullName)))
            {
                return;
            }

            var reuseTabsPageItem = new ReuseTabsPageItem
            {
                Url = url,
                CreatedAt = DateTime.MinValue,
                TypeName = pageType.FullName
            };
            GetPageInfo(reuseTabsPageItem, pageType, url, Activator.CreateInstance(pageType));
            AddPage(reuseTabsPageItem);
        }

        private void AddPage(ReuseTabsPageItem pageItem)
        {
            // Use a stable key to prevent re-render of other pages when one is removed.
            // Prefer URL; for singleton pages fallback to type name.
            pageItem.Key = pageItem.Url ?? pageItem.TypeName ?? pageItem.GetHashCode().ToString();
            _pages.Add(pageItem);
        }

        private void FilterPages()
        {
            _pages = _pages.Where(x => !x.Ignore)
               .OrderBy(x => x.CreatedAt)
               .ThenByDescending(x => x.Pin ? 1 : 0)
               .ThenBy(x => x.Order)
               .ToList();
        }

        private void RemovePageBase(string key)
        {
            var pageItem = _pages.Where(x => x.Url == key).FirstOrDefault();
            if (pageItem != null)
            {
                // Do not null out Body here; removing from collection is enough.
                // Clearing Body caused other tabs to rebuild due to fragment tree changes.
                _pages.Remove(pageItem);
            }
        }

        public void Dispose()
        {
            _menusService.MenuItemLoaded -= StateHasChanged;
        }
    }
}
