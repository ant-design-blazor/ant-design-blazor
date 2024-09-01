// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.CommandLineUtils;

namespace AntDesign.Docs.Build.CLI
{
    public interface IAppCommand
    {
        string Name { get; }

        void Execute(CommandLineApplication command);
    }
}
