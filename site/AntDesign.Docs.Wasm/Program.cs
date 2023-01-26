using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace AntDesign.Docs.Wasm
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            if (!CultureInfo.CurrentCulture.Name.IsIn("en-US", "zh-CN"))
            {
                CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
                CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
            }

            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddAntDesignDocs();

            await builder.Build().RunAsync();
        }
    }
}
