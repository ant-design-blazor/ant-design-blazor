using System.Reflection;
using AntDesign.Docs.Highlight;
using AntDesign.Docs.Localization;
using AntDesign.Docs.Localization.EmbeddedJson;
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
            services.AddSingleton<ILanguageService>(new InAssemblyLanguageService());
            services.AddScoped<IPrismHighlighter, PrismHighlighter>();

            services.TryAddSingleton<IStringLocalizerFactory>(sp => ActivatorUtilities.CreateInstance<EmbeddedJsonStringLocalizerFactory>(sp, "Resources", Assembly.GetExecutingAssembly()));
            services.TryAddTransient(typeof(IStringLocalizer<>), typeof(BlazorStringLocalizer<>));
            services.TryAddTransient<IStringLocalizer>(sp => new BlazorStringLocalizer("Resources", "", sp.GetService<IStringLocalizerFactory>(), sp.GetService<ILanguageService>()));
            return services;
        }
    }
}
