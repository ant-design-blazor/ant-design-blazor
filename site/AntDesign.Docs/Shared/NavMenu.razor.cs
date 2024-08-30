// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace AntDesign.Docs.Shared
{
    public partial class NavMenu : ComponentBase, IDisposable
    {
        protected override async Task OnInitializedAsync()
        {
            MenuItems = await DemoService.GetCurrentMenuItems();

            LocalizationService.LanguageChanged += OnLanguageChanged;
            NavigationManager.LocationChanged += OnLocationChanged;

            StateHasChanged();

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
            StateHasChanged();
        }

        public void Dispose()
        {
            LocalizationService.LanguageChanged -= OnLanguageChanged;
            NavigationManager.LocationChanged -= OnLocationChanged;
        }
    }
}
