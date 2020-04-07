using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AntBlazor.Docs.Highlight
{
    public class PrismHighlighter : IPrismHighlighter
    {
        private IJSRuntime JSRuntime { get; }

        public PrismHighlighter(IJSRuntime jsRuntime)
        {
            JSRuntime = jsRuntime;
        }

        public async Task<MarkupString> HighlightAsync(string code, string language)
        {
            string hilighted = await JSRuntime.InvokeAsync<string>("antBlazor.Prism.highlight", code, language);

            return new MarkupString(hilighted);
        }
    }
}