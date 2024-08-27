// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json.Serialization;

namespace AntDesign.Core.JsInterop.ObservableApi
{
    public class BoxSize
    {
        [JsonPropertyName("blockSize")]
        public decimal BlockSize { get; set; }

        [JsonPropertyName("inlineSize")]
        public decimal InlineSize { get; set; }
    }
}
