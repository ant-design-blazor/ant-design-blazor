using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Docs.Highlight
{
    public interface IPrismHighlighter
    {
        ValueTask<MarkupString> HighlightAsync(string code, string language);

        Task HighlightAllAsync();
    }
}