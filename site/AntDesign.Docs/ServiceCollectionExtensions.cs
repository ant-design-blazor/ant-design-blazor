using System.Reflection;
using AntDesign.Docs.Highlight;
using AntDesign.Docs.Services;
using AntDesign.Extensions.Localization;
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

            services.AddSimpleEmbeddedJsonLocalization(options =>
            {
                options.ResourcesPath = "Resources";
                options.Resources = SimpleStringLocalizerOptions.BuildResources("Resources", Assembly.GetExecutingAssembly());
            });

            //services.AddSimpleInteractiveStringLocalizer();
            services.AddInteractiveStringLocalizer();
            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });

            //services.AddJsonLocalization(b =>
            //{
            //    b.UseEmbeddedJson(o => o.ResourcesPath = "Resources");
            //}, ServiceLifetime.Singleton);
            //services.AddLocalization(options => options.ResourcesPath = "Resources");

            return services;
        }
    }
}
