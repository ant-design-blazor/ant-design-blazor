using System;
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

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await FetchData();

            Language.LanguageChanged += HandleLanguageChanged;
        }

        private async void HandleLanguageChanged(object _, CultureInfo culture)
        {
            await FetchData();
            await InvokeAsync(StateHasChanged);
        }

        private async Task FetchData()
        {
            _recommends = await DemoService.GetRecommend();

            _products = await DemoService.GetProduct();
            _moreArticles = await DemoService.GetMore();
        }

        public void Dispose()
        {
            Language.LanguageChanged -= HandleLanguageChanged;
        }
    }
}
