using System.Globalization;
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
            services.TryAddScoped<DomEventService>();
            services.TryAddScoped(sp => new HtmlRenderService(new HtmlRenderer(sp, sp.GetRequiredService<ILoggerFactory>(),
                        s => HtmlEncoder.Default.Encode(s)))
            );

            services.TryAddSingleton<IComponentIdGenerator, GuidComponentIdGenerator>();
            services.TryAddScoped<IconService>();
            services.TryAddScoped<InteropService>();
            services.TryAddScoped<NotificationService>();
            services.TryAddScoped<MessageService>();
            services.TryAddScoped<ModalService>();
            services.TryAddScoped<DrawerService>();
            services.TryAddScoped<ConfirmService>();
            services.TryAddScoped<ImageService>();

            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.CurrentCulture;

            return services;
        }
    }
}
