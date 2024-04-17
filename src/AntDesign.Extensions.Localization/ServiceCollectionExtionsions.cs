// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AntDesign.Docs.Localization;
using AntDesign.Docs.Localization.EmbeddedJson;
using AntDesign.Extensions.Localization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtionsions
    {
        public static void AddBlazorStringLocalizer(this IServiceCollection services)
        {
            services.AddTransient(typeof(IStringLocalizer<>), typeof(BlazorStringLocalizer<>));
        }

        public static IServiceCollection AddBlazorLocalization(this IServiceCollection services)
        {
            var assembly = Assembly.GetCallingAssembly();
            services.AddOptions();
            AddLocalizationServices(services, o => o.ResourcesAssembly ??= assembly);
            return services;
        }

        public static IServiceCollection AddBlazorLocalization(this IServiceCollection services, Action<BlazorLocalizationOptions> setupAction)
        {
            var assembly = Assembly.GetCallingAssembly();
            AddLocalizationServices(services, setupAction);
            services.Configure<BlazorLocalizationOptions>(o => o.ResourcesAssembly ??= assembly);
            return services;
        }

        internal static void AddLocalizationServices(IServiceCollection services)
        {
            services.AddSingleton<ILanguageService>(new InAssemblyLanguageService());
            services.TryAddSingleton<IStringLocalizerFactory, EmbeddedJsonStringLocalizerFactory>();
            services.TryAddTransient<IStringLocalizer>(sp => ActivatorUtilities.CreateInstance<BlazorStringLocalizer>(sp, sp.GetRequiredService<IOptions<BlazorLocalizationOptions>>()));
        }

        internal static void AddLocalizationServices(IServiceCollection services, Action<BlazorLocalizationOptions> setupAction)
        {
            AddLocalizationServices(services);
            services.Configure(setupAction);
        }
    }
}
