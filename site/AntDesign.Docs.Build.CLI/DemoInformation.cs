// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using AntDesign.Docs.Build.CLI.Utils;

namespace AntDesign.Docs.Build.CLI
{
    public class DemoInformation
    {
        public string Code { get; internal set; }
        public (DescriptionYaml Meta, string Style, Dictionary<string, string> Descriptions) DescriptionContent { get; set; }
        public string Name { get; internal set; }
        public string Type { get; internal set; }
    }
}
