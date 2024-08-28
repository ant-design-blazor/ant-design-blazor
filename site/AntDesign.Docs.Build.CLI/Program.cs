using System;
using System.Net.Http;
using AntDesign.Docs.Build.CLI.Command;
using AntDesign.Docs.Build.CLI.Services.Translation;
using AntDesign.Docs.Build.CLI.Utils;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AntDesign.Docs.Build.CLI
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            return new Program().Run(args);
        }

        private readonly IServiceProvider _serviceProvider;

        public Program()
        {
            _serviceProvider = ConfigureServices();
        }

        private int Run(string[] args)
        {
            try
            {
                var app = _serviceProvider.GetRequiredService<CommandLineApplication>();
                return app.Execute(args);
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred. {e.Message}");
                return 1;
            }
        }

        private IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            ConfigureOptions(services);

            services.AddSingleton<CommandLineApplicationFactory>();
            services.AddSingleton(p => p.GetRequiredService<CommandLineApplicationFactory>().Create());
            services.AddSingleton<IAppCommandResolver, AppCommandResolver>();
            services.AddSingleton<PlatformInformationArbiter>();
            services.AddSingleton<DirectoryProvider>();
            services.AddSingleton<ShellProcessFactory>();
            services.AddSingleton<IAppCommand, GenerateDemoJsonCommand>();
            services.AddSingleton<IAppCommand, GenerateMenuJsonCommand>();
            services.AddSingleton<IAppCommand, GenerateDocsToHtmlCommand>();
            services.AddSingleton<IAppCommand, GenerateIconsToJsonCommand>();
            services.AddHttpClient<GoogleTranslate>(client =>
            {
                client.BaseAddress = new Uri("https://translate.google.com");
            });
            services.AddHttpClient<AzureTranslate>(client =>
            {
                client.BaseAddress = new Uri("https://api.cognitive.microsofttranslator.com");
            });
            services.AddSingleton<ITranslate>(provider =>
            {
                // You can switch to translating documentation with the Google translation service if desired
                // var translateClient = provider.GetRequiredService<GoogleTranslate>();
                var translateClient = provider.GetRequiredService<AzureTranslate>();

                return new TranslateCache(translateClient);
            });

            return services.BuildServiceProvider();
        }

        private static void ConfigureOptions(ServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.private.json", optional: true, reloadOnChange: true)
                .AddUserSecrets<Program>()
                .AddEnvironmentVariables()
                .Build();

            services.Configure<AzureTranslateOptions>(configuration.GetSection("AzureTranslate"));
        }
    }
}
