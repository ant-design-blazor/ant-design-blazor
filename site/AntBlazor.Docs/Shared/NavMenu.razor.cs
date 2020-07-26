using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Docs.Shared
{
    public partial class NavMenu : ComponentBase
    {
        protected override async Task OnInitializedAsync()
        {
            MenuItems = await DemoService.GetCurrentMenuItems();

            LanguageService.LanguageChanged += async (_, args) =>
            {
                MenuItems = await DemoService.GetCurrentMenuItems();
                await InvokeAsync(StateHasChanged);
            };

            NavigationManager.LocationChanged += async (_, args) =>
            {
                MenuItems = await DemoService.GetCurrentMenuItems();
                StateHasChanged();
            };

            StateHasChanged();

            await base.OnInitializedAsync();
        }
    }
}
