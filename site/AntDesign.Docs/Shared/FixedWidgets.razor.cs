using System.Threading.Tasks;
using AntDesign.Docs.Localization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign.Docs.Shared
{
    public partial class FixedWidgets : AntDomComponentBase
    {
        [Inject] private ILanguageService Language { get; set; }
        [Inject] private IJSRuntime JS { get; set; }

        private ElementReference _linkRef;

        private string _themeFileUrl;

        protected override void OnInitialized()
        {
            ClassMapper.Add("fixed-widgets")
                .If("fixed-widgets-rtl", () => RTL);
        }

        private async Task ChangeTheme(string theme)
        {
            await JS.InvokeVoidAsync(JSInteropConstants.SetDomAttribute, "body", "data-theme", theme);
            _themeFileUrl = $"_content/AntDesign.Docs/css/docs.{theme}.css";
            await JS.InvokeVoidAsync(JSInteropConstants.AddElementToBody, _linkRef);
        }
    }
}
