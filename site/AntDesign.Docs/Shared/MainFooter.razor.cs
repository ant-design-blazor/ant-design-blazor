using System.Threading.Tasks;
using AntDesign.Extensions.Localization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;

namespace AntDesign.Docs.Shared
{
    public partial class MainFooter
    {
        // region: color
        private static string _colorHex = "#1890ff";

        [Inject] private IJSRuntime JS { get; set; }
        [Inject] private IMessageService Message { get; set; }

        [Inject] private ILocalizationService Language { get; set; }
        [Inject] private IStringLocalizer Localizer { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Language.LanguageChanged += (sender, args) =>
            {
                InvokeAsync(StateHasChanged);
            };
        }

        private async Task ChangeColor(ChangeEventArgs args)
        {
            _colorHex = args.Value.ToString();

            await Message.Loading(Localizer["app.footer.primary-color-changing"].Value);

            await JS.InvokeVoidAsync("changeColor", _colorHex, DotNetObjectReference.Create(this));
        }

        [JSInvokable]
        public void OnColorChanged()
        {
            Message.Success(Localizer["app.footer.primary-color-changed"].Value);
        }
    }
}
