// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json.Serialization;

namespace AntDesign.Docs.MCP.Models;

public class DemoModel
{
    [JsonPropertyName("Component")]
    public string Component { get; set; } = string.Empty;

    [JsonPropertyName("Scenario")]
    public string Scenario { get; set; } = string.Empty;

    [JsonPropertyName("Source")]
    public string Source { get; set; } = string.Empty;

    [JsonPropertyName("Description")]
    public string Description { get; set; } = string.Empty;
}