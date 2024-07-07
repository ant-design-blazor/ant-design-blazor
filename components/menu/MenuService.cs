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

        public MenuService(NavigationManager navmgr)
        {
            this._navmgr = navmgr;
        }

        internal void SetMenuItems(IEnumerable<MenuItem> menuItems)
        {
            _menuItems = menuItems;
            _breadcrumbCache = menuItems.ToDictionary(x => x.RouterLink, x => GetBreadcrumbOptions(x).ToArray());
        }

        private IEnumerable<BreadcrumbOption> GetBreadcrumbOptions(MenuItem menuItem)
        {
            List<BreadcrumbOption> options = [];
            options.Add(new BreadcrumbOption()
            {
                Label = menuItem.Title,
                RouterLink = menuItem.RouterLink
            });

            var subMenu = menuItem.ParentMenu;

            while (subMenu != null)
            {
                options.Insert(0, new BreadcrumbOption()
                {
                    Label = subMenu.Title,
                    RouterLink = null
                });

                subMenu = subMenu.Parent;
            }

            return options;
        }

        public RenderFragment GetMenuTitle(string url)
        {
            if (_titleCache.ContainsKey(url))
            {
                return _titleCache[url].GetTitleContent();
            }

            var matchedMenuItem = _menuItems.FirstOrDefault(x => MenuHelper.ShouldMatch(NavLinkMatch.All, url, x.RouterLink));
            _titleCache[url] = matchedMenuItem;
            return matchedMenuItem.GetTitleContent();
        }

        public BreadcrumbOption[] GetBreadcrumbOptions(string url)
        {
            if (_breadcrumbCache.ContainsKey(url))
            {
                return _breadcrumbCache[url];
            }
            var matchedMenuItem = _titleCache.ContainsKey(url) ? _titleCache[url] : _menuItems.FirstOrDefault(x => MenuHelper.ShouldMatch(NavLinkMatch.All, url, x.RouterLink));
            _breadcrumbCache[url] = GetBreadcrumbOptions(matchedMenuItem).ToArray();

            return _breadcrumbCache[url];
        }
    }
}
