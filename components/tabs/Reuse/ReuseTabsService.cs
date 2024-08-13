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
    public partial class ReuseTabsService
    {
        private readonly NavigationManager _navmgr;
        private readonly MenuService _menusService;
        private readonly Dictionary<string, ReuseTabsPageItem> _pageMap = [];
        private IReadOnlyCollection<ReuseTabsPageItem> _pages;

        internal event Action OnStateHasChanged;

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

        /// <summary>
        /// The page information list of the currently opened page, which can be used for caching and recovery
        /// </summary>
        public IReadOnlyCollection<ReuseTabsPageItem> Pages => _pages;

        public ReuseTabsService(NavigationManager navmgr, MenuService menusService)
        {
            _navmgr = navmgr;
            _menusService = menusService;
        }

        /// <summary>
        /// Create a tab without navigation, the page doesn't really render until the tab is clicked
        /// </summary>
        /// <param name="pageUrl">The url of target page</param>
        /// <param name="titleTemplate">The title show on the tab</param>
        public void CreateTab(string pageUrl, RenderFragment titleTemplate = null)
        {
            if (_pageMap.ContainsKey(pageUrl))
            {
                return;
            }
            AddPage(pageUrl, new ReuseTabsPageItem() { Url = pageUrl, Title = titleTemplate ?? pageUrl.ToRenderFragment(), CreatedAt = DateTime.MinValue });
            OnStateHasChanged?.Invoke();
        }

        /// <summary>
        /// Create a tab without navigation, the page doesn't really render until the tab is clicked
        /// </summary>
        /// <param name="pageUrl">The url of target page</param>
        /// <param name="title">The title show on the tab</param>
        public void CreateTab(string pageUrl, string title)
        {
            if (_pageMap.ContainsKey(pageUrl))
            {
                return;
            }
            AddPage(pageUrl, new ReuseTabsPageItem() { Url = pageUrl, Title = title.ToRenderFragment(), CreatedAt = DateTime.MinValue });
            OnStateHasChanged?.Invoke();
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
        /// Close the page corresponding to the specified key
        /// </summary>
        /// <param name="key">The specified page's key</param>
        public bool ClosePage(string key)
        {
            var reuseTabsPageItem = _pages?.FirstOrDefault(w => w.Url == key);
            if (reuseTabsPageItem?.Closable != true)
            {
                return false;
            }

            RemovePageBase(key);
            StateHasChanged();

            return true;
        }

        /// <summary>
        /// Close all pages except the page with the specified key
        /// </summary>
        /// <param name="key">The specified page's key</param>
        public void CloseOther(string key)
        {
            foreach (var item in _pages?.Where(x => x.Closable && x.Url != key && !x.Pin))
            {
                RemovePageBase(item.Url);
            }
            StateHasChanged();
        }

        /// <summary>
        /// Close all pages that is Closable or is not Pinned
        /// </summary>
        public void CloseAll()
        {
            foreach (var item in _pages?.Where(x => x.Closable && !x.Pin))
            {
                RemovePageBase(item.Url);
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
        /// Reload the page corresponding to the specified key
        /// </summary>
        /// <param name="key"></param>
        public void ReloadPage(string key)
        {
            key ??= CurrentUrl;
            _pageMap[key].Body = null;
            if (CurrentUrl == key)
            {
                CurrentUrl = key; // auto reload current page, and other page would be load by tab navigation.
            }
            StateHasChanged();
        }

        public void Update()
        {
            StateHasChanged();
        }

        internal void StateHasChanged()
        {
            OnStateHasChanged?.Invoke();
        }

        internal void Init(bool reuse)
        {
            if (reuse)
            {
                ScanReuseTabsPageAttribute();
            }
        }

        internal void TrySetRouteData(RouteData routeData, bool reuse)
        {
            if (routeData == null)
            {
                return;
            }

            if (!reuse)
            {
                _pageMap.Clear();
            }

            var reuseTabsPageItem = _pageMap.ContainsKey(CurrentUrl) ? _pageMap[CurrentUrl] : null;
            if (reuseTabsPageItem == null)
            {
                reuseTabsPageItem = new ReuseTabsPageItem
                {
                    Url = CurrentUrl,
                    CreatedAt = DateTime.Now,
                };

                AddPage(CurrentUrl, reuseTabsPageItem);
            }

            reuseTabsPageItem.Body ??= CreateBody(routeData, reuseTabsPageItem);
            OnStateHasChanged?.Invoke();
        }

        private RenderFragment CreateBody(RouteData routeData, ReuseTabsPageItem item)
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
                pageItem.KeepAlive = attr.KeepAlive;
                pageItem.Order = attr.Order;
            }

            pageItem.Title ??= _menusService.GetMenuTitle(url) ?? url.ToRenderFragment();
        }

        /// <summary>
        /// 获取所有程序集
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Assembly> GetAllAssembly()
        {
            IEnumerable<Assembly> assemblies = new List<Assembly>();
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly == null) return assemblies;
            var referencedAssemblies = entryAssembly.GetReferencedAssemblies().Select(Assembly.Load);
            assemblies = new List<Assembly> { entryAssembly }.Union(referencedAssemblies);
            return assemblies;
        }

        /// <summary>
        /// 扫描 ReuseTabsPageAttribute 特性
        /// </summary>
        private void ScanReuseTabsPageAttribute()
        {
            var list = GetAllAssembly();

            foreach (var item in list)
            {
                var allClass = item.ExportedTypes
                    .Where(w => w.GetCustomAttribute<ReuseTabsPageAttribute>()?.Pin == true);
                foreach (var pageType in allClass)
                {
                    this.AddReuseTabsPageItem(pageType);
                }
            }

            CurrentUrl ??= _pages.FirstOrDefault()?.Url;
        }

        private void AddReuseTabsPageItem(Type pageType)
        {
            var routeAttribute = pageType.GetCustomAttribute<RouteAttribute>();
            var reuseTabsAttribute = pageType.GetCustomAttribute<ReuseTabsPageAttribute>();

            var url = reuseTabsAttribute?.PinUrl ?? routeAttribute.Template;
            var reuseTabsPageItem = new ReuseTabsPageItem();
            GetPageInfo(reuseTabsPageItem, pageType, url, Activator.CreateInstance(pageType));
            reuseTabsPageItem.CreatedAt = DateTime.MinValue;
            reuseTabsPageItem.Url = url;
            AddPage(url, reuseTabsPageItem);
        }

        private void AddPage(string key, ReuseTabsPageItem pageItem)
        {
            _pageMap.TryAdd(key, pageItem);
            _pages = _pageMap.Values.Where(x => !x.Ignore)
                .OrderBy(x => x.CreatedAt)
                .ThenByDescending(x => x.Pin ? 1 : 0)
                .ThenBy(x => x.Order)
                .ToList();
        }

        private void RemovePageBase(string key)
        {
            _pageMap[key].Body = null;
            _pageMap.Remove(key);
            _pages = _pageMap.Values.Where(x => !x.Ignore)
                .OrderBy(x => x.CreatedAt)
                .ThenByDescending(x => x.Pin ? 1 : 0)
                .ThenBy(x => x.Order)
                .ToList();
        }
    }
}
