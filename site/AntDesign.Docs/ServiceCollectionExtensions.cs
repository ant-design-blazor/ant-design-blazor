using System.Reflection;
using AntDesign.Docs.Highlight;
using AntDesign.Docs.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAntDesignDocs(this IServiceCollection services)
        {
            services.AddAntDesign();
            services.AddScoped<DemoService>();
            services.AddScoped<IconListService>();
            services.AddScoped<IPrismHighlighter, PrismHighlighter>();

            services.AddEmbeddedJsonLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });

            //services.AddInteractiveStringLocalizer();
            //services.AddLocalization(options => options.ResourcesPath = "Resources");

            return services;
        }
    }
}
