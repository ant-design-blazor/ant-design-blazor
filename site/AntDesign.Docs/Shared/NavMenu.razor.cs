// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace AntDesign.Docs.Shared
{
    public partial class NavMenu : ComponentBase, IDisposable
    {
        public string _searchTerm = "";
        private List<string> _searchOptions = new List<string>() { };

        protected override async Task OnInitializedAsync()
        {
            MenuItems = await DemoService.GetCurrentMenuItems();

            foreach (var item in MenuItems)
            {
                _searchOptions.Add(item.Title);
                if (item.Children != null)
                {
                    foreach (var itemchild in item.Children)
                    {
                        _searchOptions.Add(itemchild.Title);
                    }
                }
            }

            LocalizationService.LanguageChanged += OnLanguageChanged;
            NavigationManager.LocationChanged += OnLocationChanged;

            await InvokeAsync(StateHasChanged);
            await base.OnInitializedAsync();
        }

        private async void OnLanguageChanged(object sender, CultureInfo args)
        {
            MenuItems = await DemoService.GetCurrentMenuItems();
            await InvokeAsync(StateHasChanged);
        }

        private async void OnLocationChanged(object sender, LocationChangedEventArgs args)
        {
            MenuItems = await DemoService.GetCurrentMenuItems();
            await InvokeAsync(StateHasChanged);
        }

        private async void OnComponentsSearchCleared()
        {
            MenuItems = await DemoService.GetCurrentMenuItems();
        }

        private async void OnComponentsSearch(ChangeEventArgs e)
        {
            var searchText = e.Value.ToString().ToLower();
            MenuItems = await DemoService.GetSearchedMenuItems(searchText);
        }

        public void Dispose()
        {
            LocalizationService.LanguageChanged -= OnLanguageChanged;
            NavigationManager.LocationChanged -= OnLocationChanged;
        }
    }
}
