// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using AntDesign.Docs.Services;
using AntDesign.Extensions.Localization;
using System.Net.Http;
using Microsoft.Extensions.Localization;

namespace AntDesign.Docs.Shared
{
    public partial class NavMenu : ComponentBase, IDisposable
    {
        [Inject]
        public ILocalizationService LocalizationService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public HttpClient HttpClient { get; set; }

        [Inject]
        public DemoService DemoService { get; set; }

        [Inject]
        public VersionService VersionService { get; set; }

        [Parameter]
        public DemoMenuItem[] MenuItems { get; set; } = { };

        [Inject]
        public IStringLocalizer L { get; set; }

        private string VersionInfo { get; set; } = "0.0.0";

        protected override async Task OnInitializedAsync()
        {
            MenuItems = await DemoService.GetCurrentMenuItems();

            LocalizationService.LanguageChanged += OnLanguageChanged;
            NavigationManager.LocationChanged += OnLocationChanged;

            StateHasChanged();

            VersionInfo = await VersionService.GetCurrentVersion();

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

        private bool IsComponentUpdated(DemoMenuItem menuItem)
        {
            if (string.IsNullOrEmpty(menuItem.LatestVersion))
                return false;

            // 将版本号分割成数组
            var currentVersionParts = VersionInfo.Split('.');
            var latestVersionParts = menuItem.LatestVersion.Split('.');

            // 确保至少有两位版本号
            if (currentVersionParts.Length < 2 || latestVersionParts.Length < 2)
                return false;

            // 只比较前两位版本号
            return currentVersionParts[0] == latestVersionParts[0]
                && currentVersionParts[1] == latestVersionParts[1];
        }

        public void Dispose()
        {
            LocalizationService.LanguageChanged -= OnLanguageChanged;
            NavigationManager.LocationChanged -= OnLocationChanged;
        }
    }
}
