﻿using System.Globalization;
using System.Text.Encodings.Web;
using AntDesign;
using AntDesign.JsInterop;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAntDesign(this IServiceCollection services)
        {
            services.TryAddScoped<DomEventService>();
            services.TryAddTransient<IDomEventListener>((sp) =>
            {
                var domEventService = sp.GetRequiredService<DomEventService>();
                return domEventService.CreateDomEventListerner();
            });

            services.TryAddScoped(sp => new HtmlRenderService(new HtmlRenderer(sp, sp.GetRequiredService<ILoggerFactory>(),
                        s => HtmlEncoder.Default.Encode(s)))
            );

            services.TryAddSingleton<IComponentIdGenerator, GuidComponentIdGenerator>();
            services.TryAddScoped<IconService>();
            services.TryAddScoped<InteropService>();
            services.TryAddScoped<NotificationService>();
            services.TryAddScoped<MessageService>();
            services.TryAddScoped<IMessageService>(provider => provider.GetRequiredService<MessageService>());
            services.TryAddScoped<ModalService>();
            services.TryAddScoped<DrawerService>();
            services.TryAddScoped<ConfirmService>();
            services.TryAddScoped<ImageService>();
            services.TryAddScoped<ConfigService>();
            services.TryAddSingleton<ReuseTabsService>();

            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.CurrentCulture;

            return services;
        }
    }
}
