// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.Encodings.Web;
using AntDesign;
using AntDesign.Core;
using AntDesign.Core.Services;
using AntDesign.Filters;
using AntDesign.JsInterop;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAntDesign(this IServiceCollection services)
        {
            // singleton services is only the app configuration, no need to distinguish between different users
            services.TryAddSingleton<IComponentIdGenerator, GuidComponentIdGenerator>();
            services.TryAddSingleton<IFieldFilterTypeResolver, DefaultFieldFilterTypeResolver>();

            // scoped services within IJSRuntime
            services.TryAddScoped<DomEventService>();
            services.TryAddTransient<IDomEventListener>((sp) =>
            {
                var domEventService = sp.GetRequiredService<DomEventService>();
                return domEventService.CreateDomEventListerner();
            });

            services.TryAddScoped(sp => new HtmlRenderService(new HtmlRenderer(sp, sp.GetRequiredService<ILoggerFactory>(),
                        s => HtmlEncoder.Default.Encode(s)))
            );
            services.TryAddScoped<IconService>();
            services.TryAddScoped<InteropService>();
            services.TryAddScoped<ClientDimensionService>();
            services.TryAddScoped<ConfigService>();

            // detect if it is webassembly
            var serviceLifetime = RuntimeInformation.IsOSPlatform(OSPlatform.Create("BROWSER")) ? ServiceLifetime.Singleton : ServiceLifetime.Scoped;

            // The services that are not shared between users, so need be scoped in server, but not in webassembly(it's single scoped).
            services.TryAdd(new ServiceDescriptor(typeof(NotificationService), typeof(NotificationService), serviceLifetime));
            services.TryAdd(new ServiceDescriptor(typeof(MessageService), typeof(MessageService), serviceLifetime));
            services.TryAdd(new ServiceDescriptor(typeof(ConfirmService), typeof(ConfirmService), serviceLifetime));
            services.TryAdd(new ServiceDescriptor(typeof(ModalService), typeof(ModalService), serviceLifetime));
            services.TryAdd(new ServiceDescriptor(typeof(DrawerService), typeof(DrawerService), serviceLifetime));
            services.TryAdd(new ServiceDescriptor(typeof(ImageService), typeof(ImageService), serviceLifetime));
            services.TryAdd(new ServiceDescriptor(typeof(ReuseTabsService), typeof(ReuseTabsService), serviceLifetime));
            services.TryAdd(new ServiceDescriptor(typeof(MenuService), typeof(MenuService), serviceLifetime));

            services.TryAddScoped<INotificationService>(provider => provider.GetRequiredService<NotificationService>());
            services.TryAddScoped<IMessageService>(provider => provider.GetRequiredService<MessageService>());
            services.TryAddScoped<IConfirmService>(provider => provider.GetRequiredService<ConfirmService>());

            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.CurrentCulture;

            return services;
        }
    }
}
