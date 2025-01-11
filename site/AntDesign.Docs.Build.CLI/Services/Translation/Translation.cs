// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json.Serialization;

namespace AntDesign.Docs.Build.CLI.Services.Translation
{
    public class Translation
    {
        public string Translated { get; set; }

        [JsonIgnore]
        public bool Used { get; set; }
    }
}
