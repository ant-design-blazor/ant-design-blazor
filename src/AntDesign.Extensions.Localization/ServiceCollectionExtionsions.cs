// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AntDesign.Extensions.Localization;
using AntDesign.Extensions.Localization.EmbeddedJson;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtionsions
    {
        /// <summary>
        /// Adds services required for interactive application localization. 
        /// <para> 
        /// Then you can use <see cref="ILocalizationService"/> to change the current language at runtime.
        /// </para>
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddInteractiveStringLocalizer(this IServiceCollection services)
        {
            services.AddScoped<ILocalizationService, LocalizationService>();
            services.AddTransient(typeof(IStringLocalizer<>), typeof(InteractiveStringLocalizer<>));
            return services;
        }

        public static IServiceCollection AddSimpleEmbeddedJsonLocalization(this IServiceCollection services)
        {
            var assembly = Assembly.GetCallingAssembly();
            services.AddOptions();
            AddEmbeddedJsonLocalizationServices(services, o => o.ResourcesAssembly ??= assembly);
            return services;
        }

        public static IServiceCollection AddSimpleEmbeddedJsonLocalization(this IServiceCollection services, Action<SimpleStringLocalizerOptions> setupAction)
        {
            var assembly = Assembly.GetCallingAssembly();
            AddEmbeddedJsonLocalizationServices(services, setupAction);
            services.Configure<SimpleStringLocalizerOptions>(o => o.ResourcesAssembly ??= assembly);
            return services;
        }

        /// <summary>
        /// Implement a <see cref="IStringLocalizer"/> with <see cref="InteractiveStringLocalizer"/>.
        /// </summary>
        public static void AddSimpleInteractiveStringLocalizer(this IServiceCollection services)
        {
            var assembly = Assembly.GetCallingAssembly();
            services.TryAddTransient<IStringLocalizer>(sp =>
            {
                var blazorOptions = sp.GetRequiredService<IOptions<SimpleStringLocalizerOptions>>();
                var localeOptions = sp.GetRequiredService<IOptions<LocalizationOptions>>();
                blazorOptions.Value.ResourcesPath = localeOptions.Value.ResourcesPath;
                blazorOptions.Value.ResourcesAssembly ??= assembly;

                return ActivatorUtilities.CreateInstance<InteractiveStringLocalizer>(sp, blazorOptions);
            });

            AddInteractiveStringLocalizer(services);
        }


        internal static void AddEmbeddedJsonLocalizationServices(IServiceCollection services)
        {
            services.TryAddSingleton<IStringLocalizerFactory, SimpleEmbeddedJsonLocalizerFactory>();
            AddSimpleInteractiveStringLocalizer(services);
        }

        internal static void AddEmbeddedJsonLocalizationServices(IServiceCollection services, Action<SimpleStringLocalizerOptions> setupAction)
        {
            AddEmbeddedJsonLocalizationServices(services);
            services.Configure(setupAction);
        }
    }
}
