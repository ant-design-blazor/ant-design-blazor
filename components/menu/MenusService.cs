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
    public partial class MenusService
    {
        private readonly NavigationManager _navmgr;
        private IReadOnlyCollection<MenuItem> _menuItems;

        /// <summary>
        /// The MenuItem information list of the currently opened MenuItem, which can be used for caching and recovery
        /// </summary>
        public IReadOnlyCollection<MenuItem> MenuItems => _menuItems;

        public MenusService(NavigationManager navmgr)
        {
            this._navmgr = navmgr;
        }

        public string GetMenuTitle(string hrefAbsolutePath, object menuObject)
        {
            if (menuObject is Menu menu)
            {
                return GetTitleFromMenu(hrefAbsolutePath, menu);
            }
            else if (menuObject is SubMenu subMenu)
            {
                return GetTitleFromSubMenu(hrefAbsolutePath, subMenu);
            }
            else
            {
                throw new ArgumentException("Invalid menu object type. Not Menu or SubMenu");
            }
        }

        private string GetTitleFromMenu(string absolutePath, Menu menu)
        {
            var menuItem = menu.MenuItems.FirstOrDefault(s => new MenuHelper().ShouldMatch(NavLinkMatch.All, s.RouterLink, absolutePath));
            return menuItem?.Title ?? "";
        }

        private string GetTitleFromSubMenu(string absolutePath, SubMenu subMenu)
        {
            var menuItem = subMenu.RootMenu.MenuItems.FirstOrDefault(s => new MenuHelper().ShouldMatch(NavLinkMatch.All, s.RouterLink, absolutePath));
            return menuItem?.Title ?? "";
        }
    }
}
