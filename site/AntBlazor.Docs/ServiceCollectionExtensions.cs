using System.Reflection;
using AntBlazor.Docs.Highlight;
using AntBlazor.Docs.Localization;
using AntBlazor.Docs.Routing;
using AntBlazor.Docs.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAntBlazorDocs(this IServiceCollection services)
        {
            services.AddAntBlazor();
            services.AddSingleton<RouteManager>();
            services.AddScoped<DemoService>();
            services.AddSingleton<ILanguageService>(new InAssemblyLanguageService(Assembly.GetExecutingAssembly()));
            services.AddScoped<IPrismHighlighter, PrismHighlighter>();
            return services;
        }
    }
}