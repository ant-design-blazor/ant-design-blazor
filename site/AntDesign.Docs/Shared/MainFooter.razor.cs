using System.Threading.Tasks;
using AntDesign.Docs.Localization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AntDesign.Docs.Shared
{
    public partial class MainFooter
    {
        // region: color
        private static string _colorHex = "#1890ff";

        [Inject] private IJSRuntime JS { get; set; }
        [Inject] private MessageService Message { get; set; }

        [Inject] private ILanguageService Language { get; set; }

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

            await Message.Loading(Language.Resources["app.footer.primary-color-changing"]);

            await JS.InvokeVoidAsync("changeColor", _colorHex, DotNetObjectReference.Create(this));
        }

        [JSInvokable]
        public void OnColorChanged()
        {
            Message.Success(Language.Resources["app.footer.primary-color-changed"]);
        }
    }
}
