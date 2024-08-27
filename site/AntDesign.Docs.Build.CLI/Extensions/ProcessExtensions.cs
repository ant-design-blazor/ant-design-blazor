// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;

namespace AntDesign.Docs.Build.CLI.Extensions
{
    public static class ProcessExtensions
    {
        public static void Exec(this Process process, string command)
        {
            if (!process.HasExited)
                process.StandardInput.WriteLine(command);
        }
    }
}
