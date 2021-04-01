using System.Collections.Generic;
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
            direction = direction?.ToLowerInvariant();
            await _jS.InvokeVoidAsync(JSInteropConstants.SetDomAttribute, "html", new Dictionary<string, string>
            {
                ["class"] = direction,
                ["data-direction"] = direction
            });

        }
    }
}
