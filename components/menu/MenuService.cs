// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public sealed class MenuService(NavigationManager navmgr)
    {
        private readonly Dictionary<string, MenuItem> _titleCache = [];
        private readonly Dictionary<string, BreadcrumbOption[]> _breadcrumbCache = [];
        private readonly List<MenuItem> _menuItems = [];

        internal event Action MenuItemLoaded;

        internal void SetMenuItem(MenuItem menuItem)
        {
            _menuItems.Add(menuItem);
            MenuItemLoaded?.Invoke();
        }

        public RenderFragment GetMenuTitle(string url)
        {
            return GetMenuItem(url)?.GetTitleContent();
        }

        public BreadcrumbOption[] GetBreadcrumbOptions()
        {
            var url = navmgr.ToBaseRelativePath(navmgr.Uri);
            if (url.Contains('?'))
            {
                url = url.Split('?')[0];
            }

            if (!_breadcrumbCache.TryGetValue(url, out var opts))
            {
                var menu = GetMenuItem(url);
                if (menu is null)
                {
                    return [];
                }
                opts = [.. GetBreadcrumbOptions(menu)];
                _breadcrumbCache.Add(url, opts);
            }
            return opts;
        }

        private MenuItem? GetMenuItem(string url)
        {
            if (!_titleCache.TryGetValue(url, out var title))
            {
                title = _menuItems.Find(x => x.RouterLink != null && MenuHelper.ShouldMatch(x.RouterMatch, url.TrimStart('/'), x.RouterLink.TrimStart('/')));
                if (title is null)
                {
                    return null;
                }
                _titleCache.TryAdd(url, title);
                return title;
            }
            return title;
        }

        private static List<BreadcrumbOption> GetBreadcrumbOptions(MenuItem menuItem)
        {
            List<BreadcrumbOption> options = [];
            options.Add(new()
            {
                Title = menuItem.Title,
                TitleTemplate = menuItem.GetTitleContent(),
                RouterLink = menuItem.RouterLink
            });

            var subMenu = menuItem.ParentMenu;

            while (subMenu != null)
            {
                options.Insert(0, new()
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
