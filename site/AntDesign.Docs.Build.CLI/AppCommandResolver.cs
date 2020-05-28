using System.Collections.Generic;
using Microsoft.Extensions.CommandLineUtils;

namespace AntDesign.Docs.Build.CLI
{
    public class AppCommandResolver : IAppCommandResolver
    {
        private readonly IEnumerable<IAppCommand> _appCommands;

        public AppCommandResolver(IEnumerable<IAppCommand> appCommands)
        {
            _appCommands = appCommands;
        }

        public void Resolve(CommandLineApplication application)
        {
            foreach (var command in _appCommands)
            {
                application.Command(command.Name, command.Execute);
            }
        }
    }
}