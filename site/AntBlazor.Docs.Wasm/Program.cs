using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac;

namespace AntBlazor.Docs.Wasm
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            //add Autofac
            var containerBuilder = new ContainerBuilder();
            builder.ConfigureContainer(new AbpAutofacServiceProviderFactory(containerBuilder));
            builder.Services.AddObjectAccessor(containerBuilder);

            builder.Services.AddBaseAddressHttpClient();

            //Need to register IConfiguration manually
            builder.Services.AddSingleton(typeof(IConfiguration), new ConfigurationBuilder().Build());
            builder.Services.AddApplication<AntBlazorDocsWasmModule>();
 
            await builder.Build().RunAsync();
        }
    }
}