// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json.Serialization;

namespace AntDesign.JsInterop
{
    public class DomRect
    {
        [JsonPropertyName("x")]
        public decimal X { get; set; }

        [JsonPropertyName("y")]
        public decimal Y { get; set; }

        [JsonPropertyName("width")]
        public decimal Width { get; set; }

        [JsonPropertyName("height")]
        public decimal Height { get; set; }

        [JsonPropertyName("top")]
        public decimal Top { get; set; }

        [JsonPropertyName("right")]
        public decimal Right { get; set; }

        [JsonPropertyName("bottom")]
        public decimal Bottom { get; set; }

        [JsonPropertyName("left")]
        public decimal Left { get; set; }
    }
}
