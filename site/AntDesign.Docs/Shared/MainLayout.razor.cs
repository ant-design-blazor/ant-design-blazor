﻿using System;
using System.Threading.Tasks;
using AntDesign.Docs.Localization;
using AntDesign.Docs.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace AntDesign.Docs.Shared
{
    public partial class MainLayout : LayoutComponentBase
    {
        private bool _drawerVisible = false;

        public string CurrentLanguage => LanguageService.CurrentCulture.Name;

        private bool _isMobile;

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ILanguageService LanguageService { get; set; }

        [Inject]
        public DemoService DemoService { get; set; }

        internal PrevNextNav PrevNextNav { get; set; }

        public async Task ChangePrevNextNav(string currentTitle)
        {
            if (string.IsNullOrWhiteSpace(currentTitle))
                return;

            var currentSubmenuUrl = DemoService.GetCurrentSubMenuUrl();
            var prevNext = await DemoService.GetPrevNextMenu(currentSubmenuUrl, currentTitle);

            PrevNextNav?.SetPrevNextNav(prevNext[0], prevNext[1]);
        }
    }
}
