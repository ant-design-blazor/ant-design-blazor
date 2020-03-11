using System.Net.Http;
using System.Text.Encodings.Web;
using AntBlazor.JsInterop;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace AntBlazor
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAntBlazor(this IServiceCollection services)
        {
            services.TryAddSingleton<HttpClient>();
            services.TryAddScoped<DomEventService>();
            services.TryAddScoped(sp =>
            {
                return new HtmlRenderService(new HtmlRenderer(sp, sp.GetRequiredService<ILoggerFactory>(),
                        s => HtmlEncoder.Default.Encode(s)));
            });

            return services;
        }
    }
}