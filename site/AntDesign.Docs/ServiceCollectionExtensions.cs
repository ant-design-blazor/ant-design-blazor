using System.Reflection;
using AntDesign.Docs.Highlight;
using AntDesign.Docs.Localization;
using AntDesign.Docs.Routing;
using AntDesign.Docs.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAntDesignDocs(this IServiceCollection services)
        {
            services.AddAntDesign();
            services.AddScoped<RouteManager>();
            services.AddScoped<DemoService>();
            services.AddScoped<IconListService>();
            services.AddScoped<ILanguageService>(_ => new InAssemblyLanguageService(Assembly.GetExecutingAssembly()));
            services.AddScoped<IPrismHighlighter, PrismHighlighter>();

            return services;
        }
    }
}
