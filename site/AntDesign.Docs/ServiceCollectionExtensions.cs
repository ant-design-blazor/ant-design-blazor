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
            services.AddSingleton<ILanguageService>(new InAssemblyLanguageService(Assembly.GetExecutingAssembly()));
            services.AddScoped<IPrismHighlighter, PrismHighlighter>();

            services.TryAddSingleton<IStringLocalizerFactory>(sp => ActivatorUtilities.CreateInstance<EmbeddedJsonStringLocalizerFactory>(sp, "Resources", Assembly.GetExecutingAssembly()));
            services.TryAddTransient(typeof(IStringLocalizer<>), typeof(StringLocalizer<>));
            services.TryAddTransient(sp =>
            {
                var language = sp.GetService<ILanguageService>();
                return sp.GetRequiredService<IStringLocalizerFactory>().Create($"Resources", "");
            });
            return services;
        }
    }
}
