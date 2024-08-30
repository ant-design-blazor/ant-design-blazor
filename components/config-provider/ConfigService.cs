// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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

        public async Task SetCustomTheme(string cssLinkPath, string csslinkSelector)
        {
            var js = @$"document.querySelector(""{csslinkSelector}"").setAttribute(""href"", ""{cssLinkPath}"");";
            await _jS.InvokeVoidAsync("eval", js);
        }

        public async Task SetTheme(GlobalTheme theme)
        {
            var js = """
                    let item = Array.from(document.getElementsByTagName("link")).find((item) =>
                       item.getAttribute("href")?.match("_content/AntDesign/css/ant-design-blazor")
                    );
                    if (!item) {
                        item = document.createElement('link');
                        item.rel="stylesheet";
                        document.head.appendChild(item);
                    }

                    item.href = "{{theme}}";

                    """.Replace("{{theme}}", theme.Value);

            await _jS.InvokeVoidAsync("eval", js);
        }
    }
}
