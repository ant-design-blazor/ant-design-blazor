using AntBlazor.Docs.Localization;
using AntBlazor.Docs.Routing;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAntBlazorDocs(this IServiceCollection services)
        {
            services.AddAntBlazor();
            services.AddSingleton<RouteManager>();
            services.AddSingleton<ILanguageService, InAssemblyLanguageService>();
            return services;
        }
    }
}