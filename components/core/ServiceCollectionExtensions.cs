using System;
using System.Net.Http;
using System.Text.Encodings.Web;
using AntDesign;
using AntDesign.JsInterop;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAntDesign(this IServiceCollection services)
        {
            services.AddHttpClient("AntDesign");
            services.TryAddScoped<DomEventService>();
            services.TryAddScoped(sp => new HtmlRenderService(new HtmlRenderer(sp, sp.GetRequiredService<ILoggerFactory>(),
                        s => HtmlEncoder.Default.Encode(s)))
            );

            services.TryAddScoped<IconService>();
            services.TryAddScoped<InteropService>();
            services.TryAddScoped<NotificationService>();
            services.TryAddScoped<MessageService>();
            services.TryAddScoped<ModalService>();
            services.TryAddScoped<DrawerService>();

            return services;
        }
    }
}
