// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign.Docs.Build.CLI.Documentations
{
    public class PropertyDocumentation : IApiDocumentation
    {
        public string Name { get; internal set; }
        public bool IsParameter { get; internal set; }
        public string Summary { get; internal set; }
        public string Type { get; internal set; }
        public string Default { get; internal set; }
        public string ObsoleteMessage { get; internal set; }
        public bool IsMethod { get; internal set; }
    }
}
