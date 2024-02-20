using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace AntDesign.core.Services;

public class ClientDimensionService
{
    private double? _scrollBarSize;
    private readonly IJSRuntime _js;

    public ClientDimensionService(IJSRuntime js)
    {
        _js = js;
    }

    public async Task<double> GetScrollBarSizeAsync()
    {
        _scrollBarSize ??= await _js.InvokeAsync<double>(JSInteropConstants.DomMainpulationHelper.GetScrollBarSize, false);

        return _scrollBarSize.Value;
    }
}
