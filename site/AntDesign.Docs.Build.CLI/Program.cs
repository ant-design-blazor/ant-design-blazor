using System;
using AntDesign.Docs.Build.CLI.Command;
using AntDesign.Docs.Build.CLI.Utils;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

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
            return services.BuildServiceProvider();
        }
    }
}
