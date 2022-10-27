using System.Threading.Tasks;
using AntDesign.TestApp.Maui.Models;
using AntDesign.TestApp.Maui.Services;
using Microsoft.AspNetCore.Components;

namespace AntDesign.TestApp.Maui.Pages.Profile
{
    public partial class Basic
    {
        private BasicProfileDataType _data = new BasicProfileDataType();

        [Inject] protected IProfileService ProfileService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _data = await ProfileService.GetBasicAsync();
        }
    }
}