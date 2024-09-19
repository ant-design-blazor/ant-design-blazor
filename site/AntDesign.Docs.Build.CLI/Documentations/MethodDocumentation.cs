// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign.Docs.Build.CLI.Documentations
{
    internal sealed class MethodDocumentation : IApiDocumentation
    {
        public string Signature { get; set; }
        public string ReturnType { get; set; }
        public string Summary { get; set; }
    }
}
