// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json.Serialization;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Core.JsInterop.ObservableApi
{
    public class ResizeObserverEntry
    {
        [JsonPropertyName("borderBoxSize")]
        public BoxSize BorderBoxSize { get; set; }

        [JsonPropertyName("contentBoxSize")]
        public BoxSize ContentBoxSize { get; set; }

        [JsonPropertyName("contentRect")]
        public DomRect ContentRect { get; set; }

        [JsonPropertyName("target")]
        public ElementReference Target { get; set; }

    }
}
