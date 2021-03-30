using System.Threading.Tasks;
using AntDesign.Docs.Localization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AntDesign.Docs.Shared
{
    public partial class FixedWidgets
    {
        [Inject] private ILanguageService Language { get; set; }
        [Inject] private IJSRuntime JS { get; set; }

        private ElementReference _linkRef;

        private string _themeFileUrl;

        private async Task ChangeTheme(string theme)
        {
            await JS.InvokeVoidAsync(JSInteropConstants.SetDomAttribute, "body", "data-theme", theme);
            _themeFileUrl = $"_content/AntDesign.Docs/css/docs.{theme}.css";
            await JS.InvokeVoidAsync(JSInteropConstants.AddElementToBody, _linkRef);
        }
    }
}
