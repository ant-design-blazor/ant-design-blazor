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
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtionsions
    {
        public static void AddBlazorStringLocalizer(this IServiceCollection services)
        {
            services.AddTransient(typeof(IStringLocalizer<>), typeof(BlazorStringLocalizer<>));
        }

        public static void AddBlazorLocalization(this IServiceCollection services, Action<LocalizationOptions> setupAction)
        {
            var options = new LocalizationOptions();
            setupAction(options);
        }

        internal static void AddLocalizationServices(IServiceCollection services)
        {
            services.AddSingleton<ILanguageService>(new InAssemblyLanguageService());
            services.TryAddSingleton<IStringLocalizerFactory, EmbeddedJsonStringLocalizerFactory>();
            services.TryAddTransient<IStringLocalizer>(sp => new BlazorStringLocalizer(options.ResourcesPath, "", sp.GetRequiredService<IStringLocalizerFactory>(), sp.GetRequiredService<ILanguageService>()));
        }

        internal static void AddLocalizationServices(
            IServiceCollection services,
            Action<LocalizationOptions> setupAction)
        {
            AddLocalizationServices(services);
            services.Configure(setupAction);
        }
    }
}
