using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace AntDesign
{
    public class ConfigService
    {
        private IJSRuntime _jS;

        public ConfigService(IJSRuntime js)
        {
            _jS = js;
        }

        public async Task ChangeDirection(string direction)
        {
            await _jS.InvokeVoidAsync(JSInteropConstants.SetDomAttribute, "html", "class", direction?.ToLowerInvariant());
        }
    }
}
