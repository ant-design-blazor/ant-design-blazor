using System.Threading.Tasks;
using AntDesign.Docs.Services;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Docs.Pages
{
    public partial class Index : ComponentBase
    {
        private Recommend[] _recommends = { };

        private Product[] _products = { };

        private MoreProps[] _moreArticles = { };

        [Inject] private DemoService DemoService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await FetchData();

            language.LanguageChanged += async (sender, args) =>
            {
                await FetchData();
                StateHasChanged();
            };
        }

        private async Task FetchData()
        {
            _recommends = await DemoService.GetRecommend();

            _products = await DemoService.GetProduct();
            _moreArticles = await DemoService.GetMore();
        }
    }
}
