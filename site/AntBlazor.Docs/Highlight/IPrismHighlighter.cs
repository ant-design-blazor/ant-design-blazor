using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntBlazor.Docs.Highlight
{
    public interface IPrismHighlighter
    {
        Task<MarkupString> HighlightAsync(string code, string language);
    }
}