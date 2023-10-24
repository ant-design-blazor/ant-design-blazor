using System.Globalization;
using System.Text.Encodings.Web;
using AntDesign;
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
            services.TryAddScoped<INotificationService>(provider => provider.GetRequiredService<NotificationService>());
            services.TryAddScoped<MessageService>();
            services.TryAddScoped<IMessageService>(provider => provider.GetRequiredService<MessageService>());
            services.TryAddScoped<ModalService>();
            services.TryAddScoped<DrawerService>();
            services.TryAddScoped<ConfirmService>();
            services.TryAddScoped<IConfirmService>(provider => provider.GetRequiredService<ConfirmService>());
            services.TryAddScoped<ImageService>();
            services.TryAddScoped<ConfigService>();
            services.TryAddScoped<ReuseTabsService>();
            services.TryAddScoped<IFieldFilterTypeResolver, DefaultFieldFilterTypeResolver>();

            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.CurrentCulture;

            return services;
        }
    }
}
