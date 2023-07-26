﻿using System;
using System.Globalization;
using System.Threading.Tasks;
using AntDesign.Docs.Localization;
using AntDesign.Docs.Services;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Docs.Pages
{
    public partial class Index : ComponentBase, IDisposable
    {
        private Recommend[] _recommends = { };

        private Product[] _products = { };

        private MoreProps[] _moreArticles = { };

        [Inject] private DemoService DemoService { get; set; }
        [Inject] private ILanguageService Language { get; set; }

        private bool _rendered;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Language.LanguageChanged += HandleLanguageChanged;
        }

        private void HandleLanguageChanged(object _, CultureInfo culture)
        {
            _rendered = true;
            _ = FetchData();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            if (firstRender)
            {
                _rendered = true;
                StateHasChanged();
                return;
            }

            if (_rendered)
            {
                _rendered = false;
                _ = FetchData();
            }
        }

        private async Task FetchData()
        {
            _recommends = await DemoService.GetRecommend();
            _products = await DemoService.GetProduct();
            _moreArticles = await DemoService.GetMore();
            await InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            Language.LanguageChanged -= HandleLanguageChanged;
        }
    }
}
