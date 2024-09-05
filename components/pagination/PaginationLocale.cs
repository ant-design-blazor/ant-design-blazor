// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json.Serialization;

namespace AntDesign
{
    public class PaginationLocale
    {
        [JsonPropertyName("items_per_page")]
        public string ItemsPerPage { get; set; }

        [JsonPropertyName("jump_to")]
        public string JumpTo { get; set; }

        [JsonPropertyName("jump_to_confirm")]
        public string JumpToConfirm { get; set; }

        [JsonPropertyName("page")]
        public string Page { get; set; }

        [JsonPropertyName("prev_page")]
        public string PrevPage { get; set; }

        [JsonPropertyName("next_page")]
        public string NextPage { get; set; }

        [JsonPropertyName("prev_5")]
        public string Prev5 { get; set; }

        [JsonPropertyName("next_5")]
        public string Next5 { get; set; }

        [JsonPropertyName("prev_3")]
        public string Prev3 { get; set; }

        [JsonPropertyName("next_3")]
        public string Next3 { get; set; }
    }
}
