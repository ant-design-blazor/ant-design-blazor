using System.Net.Http;
using AntBlazor.JsInterop;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AntBlazor
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAntBlazor(this IServiceCollection services)
        {
            services.TryAddSingleton<HttpClient>();
            services.TryAddScoped<DomEventService>();
            return services;
        }
    }
}