// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using AntDesign.Core.Style;
using AntDesign.Docs.Highlight;
using AntDesign.Docs.Services;
using AntDesign.Extensions.Localization;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAntDesignDocs(this IServiceCollection services)
        {
            services.AddAntDesign();
            services.AddSingleton<DemoServiceCache>();
            services.AddScoped<DemoService>();
            services.AddScoped<IconListService>();
            services.AddScoped<IPrismHighlighter, PrismHighlighter>();
            services.AddSingleton<IStyleManager, StyleManager>();

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

            services.AddScoped<CompilerService>();
            return services;
        }
    }
}
