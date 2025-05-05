// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
