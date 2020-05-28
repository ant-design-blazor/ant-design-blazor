using AntDesign.Docs.Build.CLI.Extensions;
using Microsoft.Extensions.CommandLineUtils;

namespace AntDesign.Docs.Build.CLI
{
    public class CommandLineApplicationFactory
    {
        private readonly IAppCommandResolver _appCommandResolver;

        public CommandLineApplicationFactory(IAppCommandResolver appCommandResolver)
        {
            _appCommandResolver = appCommandResolver;
        }

        public CommandLineApplication Create()
        {
            var app = new CommandLineApplication
            {
                Name = "ant-design-blazor",
                FullName = "Ant Design Blazor Command Line Tool",
                Description =
                    "Generate demo structured file for Docs."
            };

            app.HelpOption();
            app.VersionOptionFromAssemblyAttributes(typeof(Program).Assembly);

            app.OnExecute(() =>
            {
                app.ShowHelp();
                return 2;
            });

            _appCommandResolver.Resolve(app);

            return app;
        }
    }
}