// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json.Serialization;

namespace AntDesign.Docs.MCP.Models;

public class ComponentModel
{
    [JsonPropertyName("Category")]
    public string Category { get; set; } = string.Empty;

    [JsonPropertyName("Title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("SubTitle")]
    public string SubTitle { get; set; } = string.Empty;

    [JsonPropertyName("Type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("Desc")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("ApiDoc")]
    public string ApiDoc { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"""
        Component: {Title}
        Category: {Category}
        Type: {Type}
        {(string.IsNullOrEmpty(SubTitle) ? "" : $"SubTitle: {SubTitle}\n")}
        Description:
        {Description.Trim()}

        API Documentation:
        {ApiDoc.Trim()}
        """;
    }
}