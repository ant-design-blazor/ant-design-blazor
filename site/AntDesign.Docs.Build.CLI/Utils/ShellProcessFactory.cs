// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign.Docs.Build.CLI.Utils
{
    public class ShellProcessFactory
    {
        public ShellProcess Create(string name, string argument = null)
        {
            return new ShellProcess(name, argument).Start();
        }

        public int Release(ShellProcess process)
        {
            process.Close();
            return process.ExitCode;
        }
    }
}
