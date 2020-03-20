using AntBlazor.Docs.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace AntBlazor.Docs
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAntBlazorDocs(this IServiceCollection services)
        {
            services.AddAntBlazor();
            services.AddSingleton<RouteManager>();
            return services;
        }
    }
}