using System;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Photino.Blazor;

namespace AntDesign.Docs.Desktop.Photino
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<HttpHandler>();
            services.AddHttpClient("Default", client =>
            {
                client.DefaultRequestHeaders.Add(
                    "User-Agent",
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.129 Safari/537.36 Edg/81.0.416.68");
            }).AddHttpMessageHandler<HttpHandler>();
            services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Default"));

            services.AddAntDesignDocs();
        }

        public void Configure(DesktopApplicationBuilder app)
        {
            var jsRuntime = app.Services.GetService<IJSRuntime>() as JSRuntime;
            var jsonOptionsOfJSRuntime = (JsonSerializerOptions)typeof(JSRuntime).GetProperty(
                "JsonSerializerOptions",
                BindingFlags.NonPublic | BindingFlags.Instance).GetValue(jsRuntime);
            var converter = new ElementReferenceJsonConverter(
                new WebElementReferenceContext(jsRuntime));
            jsonOptionsOfJSRuntime.Converters.Add(converter);
            app.AddComponent<App>("app");
        }
    }
}
