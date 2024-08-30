// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace AntDesign
{
    public sealed class MenuService
    {
        private readonly NavigationManager _navmgr;
        private Dictionary<string, MenuItem> _titleCache = [];
        private Dictionary<string, BreadcrumbOption[]> _breadcrumbCache = [];
        private IEnumerable<MenuItem> _menuItems = [];

        internal event Action MenuItemLoaded;

        public MenuService(NavigationManager navmgr)
        {
            this._navmgr = navmgr;
        }

        internal void SetMenuItems(IEnumerable<MenuItem> menuItems)
        {
            _menuItems = menuItems;
            MenuItemLoaded?.Invoke();
        }

        public RenderFragment GetMenuTitle(string url)
        {
            return GetMenuItem(url)?.GetTitleContent();
        }

        public BreadcrumbOption[] GetBreadcrumbOptions()
        {
            var url = _navmgr.ToBaseRelativePath(_navmgr.Uri);
            if (url.Contains('?'))
            {
                url = url.Split('?')[0];
            }

            if (_breadcrumbCache.ContainsKey(url))
            {
                return _breadcrumbCache[url];
            }
            var matchedMenuItem = GetMenuItem(url);
            if (matchedMenuItem == null)
            {
                return [];
            }
            _breadcrumbCache[url] = GetBreadcrumbOptions(matchedMenuItem).ToArray();

            return _breadcrumbCache[url];
        }

        private MenuItem? GetMenuItem(string url)
        {
            if (_titleCache.ContainsKey(url))
            {
                return _titleCache[url];
            }

            var matchedMenuItem = _menuItems.FirstOrDefault(x => x.RouterLink != null && MenuHelper.ShouldMatch(NavLinkMatch.All, url.TrimStart('/'), x.RouterLink.TrimStart('/')));
            if (matchedMenuItem == null)
            {
                return null;
            }
            _titleCache.TryAdd(url, matchedMenuItem);
            return matchedMenuItem;
        }

        private IEnumerable<BreadcrumbOption> GetBreadcrumbOptions(MenuItem menuItem)
        {
            List<BreadcrumbOption> options = [];
            options.Add(new BreadcrumbOption()
            {
                Title = menuItem.Title,
                TitleTemplate = menuItem.GetTitleContent(),
                RouterLink = menuItem.RouterLink
            });

            var subMenu = menuItem.ParentMenu;

            while (subMenu != null)
            {
                options.Insert(0, new BreadcrumbOption()
                {
                    Title = subMenu.Title,
                    TitleTemplate = subMenu.TitleTemplate,
                    RouterLink = null
                });

                subMenu = subMenu.Parent;
            }

            return options;
        }
    }
}
