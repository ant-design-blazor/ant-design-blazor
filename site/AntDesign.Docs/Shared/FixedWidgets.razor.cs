using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.Generic;
using Microsoft.Extensions.Localization;

namespace AntDesign.Docs.Shared
{
    public partial class FixedWidgets
    {
        [Inject] private IStringLocalizer Localizer { get; set; }
        [Inject] private IJSRuntime JS { get; set; }

        private ElementReference _linkRef;

        private string _themeFileUrl;


        private async Task ChangeTheme(string theme)
        {
            _themeFileUrl = $"_content/AntDesign.Docs/css/{theme}.css";

            _ = JS.InvokeVoidAsync(JSInteropConstants.AddElementToBody, _linkRef);
            _ = JS.InvokeVoidAsync(JSInteropConstants.SetDomAttribute, "html", new Dictionary<string, string>
            {
                ["data-theme"] = theme
            });
        }
    }
}
