using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AntBlazor
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAntBlazor(this IServiceCollection services)
        {
            services.TryAddSingleton(new HttpClient());
            return services;
        }
    }
}